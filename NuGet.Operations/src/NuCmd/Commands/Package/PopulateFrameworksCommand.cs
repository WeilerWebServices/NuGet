﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NuGet;
using PowerArgs;

namespace NuCmd.Commands.Package
{
    [Description("Populate the Package Frameworks index in the database from the data in the package itself")]
    public class PopulateFrameworksCommand : EnvironmentCommandBase
    {
        [ArgPosition(0)]
        [ArgShortcut("i")]
        [ArgDescription("The ID of the package to process when processing only a single package registration")]
        public string Id { get; set; }

        [ArgPosition(1)]
        [ArgShortcut("v")]
        [ArgDescription("The Version of the package to process")]
        public string Version { get; set; }

        [ArgShortcut("a")]
        [ArgDescription("Set this flag to process all versions of the package when the ID is specified, or all packages if ID is not specified.")]
        public bool All { get; set; }

        [ArgDescription("Filters the packages to be processed down to those that currently have 'Unsupported' frameworks")]
        public bool Unsupported { get; set; }

        [ArgShortcut("db")]
        [ArgDescription("SQL Connection string for the package database.")]
        public string DatabaseConnectionString { get; set; }

        [ArgShortcut("st")]
        [ArgDescription("Azure Storage Connection string for the package storage.")]
        public string StorageConnectionString { get; set; }

        [ArgRequired]
        [ArgShortcut("work")]
        [ArgDescription("Directory in which to put resume data and other work")]
        public string WorkDirectory { get; set; }

        private CloudStorageAccount StorageAccount { get; set; }

        private static readonly int _padLength = Enum.GetValues(typeof(PackageReportState)).Cast<PackageReportState>().Select(p => p.ToString().Length).Max();

        private const string SqlAllPackages = @"
                    SELECT      p.[Key], pr.Id, Version = p.NormalizedVersion, p.Hash, p.Created
                    FROM        Packages p
                    INNER JOIN  PackageRegistrations pr
                            ON  pr.[Key] = p.PackageRegistrationKey
                    WHERE       (NullIf(@Id, '') IS NULL OR pr.Id = @Id)
                            AND (@All = 1 OR p.NormalizedVersion = @Version)
                    ORDER BY    p.Created DESC";

        private const string SqlUnsupportedPackages = @"
                    SELECT      DISTINCT
                                p.[Key], pr.Id, Version = p.NormalizedVersion, p.Hash, p.Created
                    FROM        PackageFrameworks pf
                    INNER JOIN  Packages p
                            ON  p.[Key] = pf.Package_Key
                    INNER JOIN  PackageRegistrations pr
                            ON  pr.[Key] = p.PackageRegistrationKey
                    WHERE       TargetFramework LIKE '%Unsupported%'
                            AND (NullIf(@Id, '') IS NULL OR pr.Id = @Id)
                            AND (@All = 1 OR p.NormalizedVersion = @Version)
                    ORDER BY    p.Created DESC";

        private static readonly JsonSerializer _serializer = new JsonSerializer()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            TypeNameHandling = TypeNameHandling.None,
            Formatting = Formatting.Indented,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        };

        protected override async Task OnExecute()
        {
            if (String.IsNullOrWhiteSpace(Version) && !All)
            {
                await Console.WriteErrorLine(Strings.Package_AllVersionsRequiredIfVersionNull);
                return;
            }

            // Get Datacenter 0
            var dc = GetDatacenter(0, required: false);
            if (dc != null)
            {

                var defaults = await LoadDefaultsFromAzure(dc, DatabaseConnectionString, StorageConnectionString);
                DatabaseConnectionString = defaults.DatabaseConnectionString;
                StorageConnectionString = defaults.StorageConnectionString;
            }

            StorageAccount = CloudStorageAccount.Parse(StorageConnectionString);

            // Parse the version
            if (!String.IsNullOrWhiteSpace(Version))
            {
                Version = SemanticVersionHelper.Normalize(Version);
            }

            if (!String.IsNullOrWhiteSpace(Version) && All)
            {
                await Console.WriteErrorLine(Strings.Package_VersionAndAllVersionsSpecified);
                return;
            }

            IEnumerable<dynamic> packages;
            int totalCount;

            // Get all of the packages to be processed
            using (var conn = new SqlConnection(DatabaseConnectionString))
            {
                await conn.OpenAsync();
                var sql = Unsupported ? SqlUnsupportedPackages : SqlAllPackages;

                await Console.WriteInfoLine("Getting the list of packages to process from SQL.{0}", Unsupported ? " Filtering to those containing an 'Unsupported' framework." : "");
                packages = conn.Query(sql, new
                    {
                        Id,
                        All,
                        Version
                    });

                totalCount = packages.Count();
                await Console.WriteInfoLine("Found {0} package(s) to process", totalCount);
            }

            if (!WhatIf)
            {
                bool confirmed = await Console.Confirm(
                    String.Format(
                        Strings.Package_PopulatePackageFrameworksCommand_Confirm,
                        packages.Count(),
                        (dc == null ? "<unknown>" : dc.FullName)),
                    defaultValue: true);

                if (!confirmed)
                {
                    await Console.WriteErrorLine(Strings.SystemConsole_ConfirmDenied);
                    return;
                }
            }

            int processedCount = 0;

            packages
                .AsParallel()
                .AsOrdered()
                // Use 2 threads per processor, because we might find ourselves
                // waiting on SQL
                .WithDegreeOfParallelism(System.Environment.ProcessorCount * 2)
                .ForAll(package =>
                {
                    var thisPackageId = Interlocked.Increment(ref processedCount);
                    ProcessPackage(package, thisPackageId, totalCount);
                });

        }

        private void ProcessPackage(dynamic package, int thisPackageId, int totalCount)
        {
            string countPad = new string('0', totalCount.ToString().Length);

            Console.WriteInfoLine("[{2}/{3} ~{4}%] Processing Package: {0}@{1} (created {5})",
                (string)package.Id,
                (string)package.Version,
                thisPackageId.ToString(countPad),
                totalCount.ToString(countPad),
                (((double)thisPackageId / (double)totalCount) * 100).ToString("000.00"),
                (DateTime)package.Created).Wait();

            try
            {
                var reportPath = Path.Combine(WorkDirectory, package.Id + "_" + package.Version + ".json");
                var bustedReportPath = Path.Combine(WorkDirectory, package.Id + "_" + package.Version + "_" + package.Hash + ".json");

                var report = new PackageFrameworkReport()
                {
                    Id = package.Id,
                    Version = package.Version,
                    Key = package.Key,
                    Hash = package.Hash,
                    Created = package.Created,
                    State = PackageReportState.Unresolved
                };

                if (File.Exists(bustedReportPath))
                {
                    File.Move(bustedReportPath, reportPath);
                }

                using (var conn = new SqlConnection(DatabaseConnectionString))
                {
                    bool resolved = false;

                    if (File.Exists(reportPath))
                    {
                        using (var reader = File.OpenText(reportPath))
                        {
                            var savedReport = (PackageFrameworkReport)_serializer.Deserialize(
                                reader, typeof(PackageFrameworkReport));

                            if (savedReport != null)
                            {
                                report = savedReport;
                                resolved = ResolveReport(report, conn) && report.State == PackageReportState.Resolved;
                            }
                        }
                    }

                    if (!resolved)
                    {
                        try
                        {
                            var downloadPath = DownloadPackage(package);
                            var nugetPackage = new ZipPackage(downloadPath);

                            var supportedFrameworks = GetSupportedFrameworks(nugetPackage);
                            report.PackageFrameworks = supportedFrameworks.ToArray();

                            GetExistingFrameworks((int)(package.Key), report, conn);

                            File.Delete(downloadPath);

                            ResolveReport(report, conn);
                        }
                        catch (Exception ex)
                        {
                            report.State = PackageReportState.Error;
                            report.Error = ex.ToString();
                        }
                    }
                }

                using (var writer = File.CreateText(reportPath))
                {
                    _serializer.Serialize(writer, report);
                }

                Console.WriteInfoLine("[{2}/{3} ~{4}%] {6} Package: {0}@{1} (created {5})",
                    (string)package.Id,
                    (string)package.Version,
                    thisPackageId.ToString(countPad),
                    totalCount.ToString(countPad),
                    (((double)thisPackageId / (double)totalCount) * 100).ToString("000.00"),
                    (DateTime)package.Created,
                    report.State.ToString().PadRight(_padLength, ' ')).Wait();

                if (report.State == PackageReportState.Error)
                {
                    Console.WriteErrorLine("Recorded Error for Package {0}@{1}: {2}", report.Id,  report.Version, report.Error).Wait();
                }
            }
            catch (Exception ex)
            {
                Console.WriteErrorLine("[{2}/{3} ~{4}%] Error for Package: {0}@{1}: {5}",
                    (string)package.Id,
                    (string)package.Version,
                    thisPackageId.ToString(countPad),
                    totalCount.ToString(countPad),
                    (((double)thisPackageId / (double)totalCount) * 100).ToString("000.00"),
                    ex.ToString()).Wait();
            }
        }

        private bool ResolveReport(PackageFrameworkReport report, SqlConnection conn)
        {
            report.State = PackageReportState.Unresolved;

            if (report.Operations == null)
            {
                return false;
            }

            Console.WriteInfoLine(" {0}@{1} Operations to complete: {2}", report.Id, report.Version, report.Operations.Count()).Wait();

            foreach (var operation in report.Operations)
            {
                if (operation.Type == PackageFrameworkOperationType.Add)
                {
                    try
                    {
                        if (!WhatIf)
                        {
                            conn.Execute(@"
                                INSERT  PackageFrameworks(TargetFramework, Package_Key)
                                VALUES  (@targetFramework, @packageKey)", new
                                {
                                    targetFramework = operation.Framework,
                                    packageKey = report.Key
                                });
                        }

                        Console.WriteDataLine(" + Id={0}, Version={1}, Key={2}, Fx={3}", report.Id, report.Version, report.Key, operation.Framework).Wait();
                        operation.Applied = true;
                    }
                    catch (Exception ex)
                    {
                        report.State = PackageReportState.Error;
                        operation.Applied = false;
                        operation.Error = ex.ToString();

                        Console.WriteErrorLine(" {0}@{1} '{2}' Error: {3}", report.Id, report.Version, operation.Framework, operation.Error).Wait();
                    }
                }
                else if (operation.Type == PackageFrameworkOperationType.Remove)
                {
                    try
                    {
                        if (!WhatIf)
                        {
                            conn.Execute(@"
                                DELETE  PackageFrameworks
                                WHERE   TargetFramework = @targetFramework
                                    AND Package_Key = @packageKey", new
                                {
                                    targetFramework = operation.Framework,
                                    packageKey = report.Key
                                });
                        }

                        Console.WriteDataLine(" - Id={0}, Version={1}, Key={2}, Fx={3}", report.Id, report.Version, report.Key, operation.Framework).Wait();
                        operation.Applied = true;
                    }
                    catch (Exception ex)
                    {
                        report.State = PackageReportState.Error;
                        operation.Applied = false;
                        operation.Error = ex.ToString();

                        Console.WriteErrorLine(" {0}@{1} '{2}' Error: {3}", report.Id, report.Version, operation.Framework, operation.Error).Wait();
                    }
                }
            }

            if (report.Operations.All(o => o.Applied))
            {
                report.State = PackageReportState.Resolved;
            }

            return report.State != PackageReportState.Error;
        }

        private string DownloadPackage(dynamic package)
        {
            string id = ((string)package.Id).ToLowerInvariant();
            string version = ((string)package.Version).ToLowerInvariant();
            string hash = WebUtility.UrlEncode((string)package.Hash);

            // Get the blob URL
            var client = StorageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference("packages");
            var nupkg = id + "." + version + ".nupkg";
            var blob = container.GetBlockBlobReference(nupkg);

            var localFile = Path.Combine(Path.GetTempPath(), nupkg);
            if (File.Exists(localFile))
            {
                File.Delete(localFile);
            }

            Console.WriteInfoLine(Strings.Package_DownloadingBlob).Wait();
            blob.DownloadToFile(localFile, FileMode.CreateNew);

            return localFile;
        }

        private static IEnumerable<string> GetSupportedFrameworks(IPackage nugetPackage)
        {
            return nugetPackage.GetSupportedFrameworks().Select(fn =>
                fn == null ? null : VersionUtility.GetShortFrameworkName(fn)).ToArray();                
        }

        private void GetExistingFrameworks(int packageKey, PackageFrameworkReport report, SqlConnection conn)
        {
            // Get all target frameworks in the db for this package
            report.DatabaseFrameworks = new HashSet<string>(conn.Query<string>(@"
                    SELECT  TargetFramework
                    FROM    PackageFrameworks
                    WHERE   Package_Key = @packageKey", new
            {
                packageKey = packageKey
            })).ToArray();

            var adds = report.PackageFrameworks.Except(report.DatabaseFrameworks).Select(targetFramework =>
                new PackageFrameworkOperation()
                {
                    Type = PackageFrameworkOperationType.Add,
                    Framework = targetFramework,
                    Applied = false,
                    Error = "Not Started"
                });
            var rems = report.DatabaseFrameworks.Except(report.PackageFrameworks).Select(targetFramework =>
                new PackageFrameworkOperation()
                {
                    Type = PackageFrameworkOperationType.Remove,
                    Framework = targetFramework,
                    Applied = false,
                    Error = "Not Started"
                });

            report.Operations = Enumerable.Concat(adds, rems).ToArray();
        }

        public class Package
        {
            public string Hash { get; set; }
            public string Id { get; set; }
            public int Key { get; set; }
            public string Version { get; set; }
            public string NormalizedVersion { get; set; }
            public DateTime Created { get; set; }
        }

        public class PackageFrameworkReport
        {
            public string Id { get; set; }
            public string Version { get; set; }
            public int Key { get; set; }
            public string Hash { get; set; }
            public DateTime Created { get; set; }
            public string[] DatabaseFrameworks { get; set; }
            public string[] PackageFrameworks { get; set; }
            public PackageFrameworkOperation[] Operations { get; set; }
            public string Error { get; set; }

            [JsonConverter(typeof(StringEnumConverter))]
            public PackageReportState State { get; set; }
        }

        public class PackageFrameworkOperation
        {
            [JsonConverter(typeof(StringEnumConverter))]
            public PackageFrameworkOperationType Type { get; set; }
            public string Framework { get; set; }
            public bool Applied { get; set; }
            public string Error { get; set; }
        }

        public enum PackageFrameworkOperationType
        {
            Add,
            Remove
        }

        public enum PackageReportState
        {
            Unresolved,
            Resolved,
            Error
        }
    }
}
