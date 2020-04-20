﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using NuCmd.Models;
using PowerArgs;
using NuGet.Services.Operations.Helpers;

namespace NuCmd.Commands.Package
{
    [Description("Deletes a package from the primary datacenter in the target NuGet environment")]
    public class DeleteCommand : EnvironmentCommandBase
    {
        [ArgPosition(0)]
        [ArgShortcut("i")]
        [ArgDescription("The ID of the package to delete")]
        public string Id { get; set; }

        [ArgPosition(1)]
        [ArgShortcut("v")]
        [ArgDescription("The Version of the package to delete")]
        public string Version { get; set; }

        [ArgShortcut("a")]
        [ArgDescription("Set this flag to delete all versions of the package.")]
        public bool AllVersions { get; set; }

        [ArgRequired]
        [ArgShortcut("r")]
        [ArgDescription("The reason for deletion. Must be specified.")]
        public string Reason { get; set; }

        [ArgShortcut("db")]
        [ArgDescription("SQL Connection string for the package database.")]
        public string DatabaseConnectionString { get; set; }

        [ArgShortcut("st")]
        [ArgDescription("Azure Storage Connection string for the package storage.")]
        public string StorageConnectionString { get; set; }

        [ArgShortcut("q")]
        [ArgDescription("Quiet mode - no prompt for entering the ID to confirm deletion. Use caution!!")]
        public bool Quiet { get; set; }

        [ArgShortcut("pc")]
        [ArgDescription("Package config to be used for bulk delete scenarios")]
        public string PackagesConfig { get; set; }

        private CloudStorageAccount StorageAccount { get; set; }

        protected override async Task OnExecute()
        {
            if (!String.IsNullOrEmpty(PackagesConfig))
            {
                List<PackageConfigParser.Package> IdAndVersion = PackageConfigParser.GetIdAndVersionFromPackagesConfig(PackagesConfig);
                if (IdAndVersion == null)
                {
                    await Console.WriteErrorLine(String.Format(Strings.Package_DeleteCommand_InvalidPackagesConfig, PackagesConfig));
                }

                await DeleteP(IdAndVersion);
                return;
            }

            if (String.IsNullOrEmpty(Id) && String.IsNullOrEmpty(PackagesConfig))
            {
                await Console.WriteErrorLine(Strings.Package_DeleteCommand_InvalidArguments);
                return;
            }

            if (String.IsNullOrWhiteSpace(Version) && !AllVersions)
            {
                await Console.WriteErrorLine(Strings.Package_AllVersionsRequiredIfVersionNull);
                return;
            }

            if (!String.IsNullOrWhiteSpace(Version) && AllVersions)
            {
                await Console.WriteErrorLine(Strings.Package_VersionAndAllVersionsSpecified);
                return;
            }

            PackageConfigParser.Package package = new PackageConfigParser.Package();
            package.Id = Id;
            package.Version = Version;

            List<PackageConfigParser.Package> singlePackage = new List<PackageConfigParser.Package>();
            singlePackage.Add(package);
            await DeleteP(singlePackage);
       }

        private async Task DeleteP(List<PackageConfigParser.Package> IdAndVersion)
        {
            var dc = GetDatacenter(0, required: false);
                if (dc != null)
                {
                    var defaults = await LoadDefaultsFromAzure(dc, DatabaseConnectionString, StorageConnectionString);
                    DatabaseConnectionString = defaults.DatabaseConnectionString;
                    StorageConnectionString = defaults.StorageConnectionString;
                }

                StorageAccount = CloudStorageAccount.Parse(StorageConnectionString);


                // Connect to the database
                using (var conn = new SqlConnection(DatabaseConnectionString))
                {
                    await conn.OpenAsync();
                    foreach (PackageConfigParser.Package pkg in IdAndVersion)
                    {
                        Id = pkg.Id;
                        Version = pkg.Version;

                        // Parse the version
                        if (!String.IsNullOrWhiteSpace(Version))
                        {
                            Version = SemanticVersionHelper.Normalize(Version);
                        }

                        

                        var packages = conn.Query(@"
                    SELECT
                        p.[Key],
                        p.PackageRegistrationKey,
                        pr.Id,
                        p.NormalizedVersion AS Version, 
                        p.Hash 
                    FROM Packages p
                    INNER JOIN PackageRegistrations pr ON p.PackageRegistrationKey = pr.[Key]
                    WHERE pr.Id = @Id AND (@AllVersions = 1 OR p.NormalizedVersion = @Version)", new
                                                                                               {
                                                                                                   Id,
                                                                                                   AllVersions,
                                                                                                   Version
                                                                                               });

                         await Console.WriteInfoLine(Strings.Package_DeleteCommand_DeleteList_Header, (dc == null ? "<unknown>" : dc.FullName));
                        foreach (var package in packages)
                        {
                            await Console.WriteInfoLine(
                                Strings.Package_DeleteCommand_DeleteList_Item,
                                (string)package.Id,
                                (string)package.Version);
                        }

                        // Ask the user to confirm by typing the ID
                        if (!WhatIf && !Quiet)
                        {
                            await Console.WriteInfoLine(Strings.Package_DeleteCommand_NonWhatIf);
                            string typed = await Console.Prompt(Strings.Package_DeleteCommand_DeleteList_Confirm);
                            if (!String.Equals(typed, Id, StringComparison.Ordinal))
                            {
                                await Console.WriteErrorLine(Strings.Package_DeleteCommand_IncorrectId, typed);
                                return;
                            }
                        }

                        foreach (var package in packages)
                        {
                            await DeletePackage(package, conn);
                        }

                        if (AllVersions)
                        {
                            await DeleteRegistration(conn);
                        }
                    }
                }
        }
                

        private async Task DeletePackage(dynamic package, SqlConnection conn)
        {
            // Capture the data from the database
            var packageRecord = await conn.QueryDatatable(
                "SELECT * FROM Packages WHERE [Key] = @key",
                new SqlParameter("@key", package.Key));
            var registrationRecord = await conn.QueryDatatable(
                "SELECT * FROM PackageRegistrations WHERE [Key] = @key",
                new SqlParameter("@key", package.PackageRegistrationKey));

            // Create a delete audit record
            var auditRecord = new PackageAuditRecord(
                package.Id,
                package.Version,
                package.Hash,
                packageRecord,
                registrationRecord,
                PackageAuditAction.Deleted,
                Reason);

            await Console.WriteInfoLine(Strings.Package_DeleteCommand_WritingAuditRecord, auditRecord.GetPath());
            if (!WhatIf)
            {
                await auditRecord.WriteAuditRecord("package", StorageAccount);
            }

            await DeletePackageData(package, conn);

            await DeletePackageBlob(package);

            await Console.WriteInfoLine(Strings.Package_DeleteCommand_DeletionCompleted);
        }

        private async Task DeleteRegistration(SqlConnection conn)
        {
            // Capture the data from the database
            var registrationRecord = await conn.QueryDatatable(
                "SELECT * FROM PackageRegistrations WHERE id = @Id",
                new SqlParameter("@Id", Id));

            // Create a delete audit record
            var auditRecord = new PackageRegistrationAuditRecord(
                Id,
                registrationRecord,
                PackageRegistrationAuditAction.Deleted,
                Reason);

            await Console.WriteInfoLine(Strings.Package_DeleteCommand_WritingRegistrationAuditRecord, auditRecord.GetPath());
            if (!WhatIf)
            {
                await auditRecord.WriteAuditRecord("packageregistrations", StorageAccount);
            }

            // Delete all data
            var result = conn.Query(@"
                BEGIN TRAN

                DECLARE @actions TABLE(
                    TableName nvarchar(50),
                    Value nvarchar(MAX)
                )

                DELETE por 
                OUTPUT 'PackageOwnerRequests' AS TableName, u.Username AS Value INTO @actions
                FROM PackageOwnerRequests por 
                JOIN PackageRegistrations pr ON pr.[Key] = por.PackageRegistrationKey 
                JOIN Users u ON por.NewOwnerKey = u.[Key]
                WHERE pr.Id = @Id

                DELETE por 
                OUTPUT 'PackageRegistrationOwners' AS TableName, u.Username AS Value INTO @actions
                FROM PackageRegistrationOwners por 
                JOIN PackageRegistrations pr ON pr.[Key] = por.PackageRegistrationKey 
                JOIN Users u ON por.UserKey = u.[Key]
                WHERE pr.Id = @Id

                DELETE pr 
                OUTPUT 'PackageRegistrations' AS TableName, deleted.Id AS Value INTO @actions
                FROM PackageRegistrations pr 
                WHERE pr.Id = @Id

                SELECT * FROM @actions
                " + (WhatIf ? "ROLLBACK TRAN" : "COMMIT TRAN"), new
                {
                    Id = Id
                });
            await Console.WriteInfoLine(Strings.Package_DeleteCommand_DatabaseActions);
            await Console.WriteTable(result, d => new
            {
                Action = "DELETE",
                Table = (string)d.TableName,
                Value = (string)d.Value
            });
        }

        private async Task DeletePackageData(dynamic package, SqlConnection conn)
        {
            await Console.WriteInfoLine(
                Strings.Package_DeleteCommand_DeletingPackageData,
                (string)package.Id,
                (string)package.Version,
                conn.Database,
                conn.DataSource);

            var result = conn.Query(@"
                BEGIN TRAN

                DECLARE @actions TABLE(
                    TableName nvarchar(50),
                    Value nvarchar(MAX)
                )

                DELETE pa 
                OUTPUT 'PackageAuthors' AS TableName, deleted.Name AS Value INTO @actions
                FROM PackageAuthors pa 
                JOIN Packages p ON p.[Key] = pa.PackageKey 
                WHERE p.[Key] = @key

                DELETE pd 
                OUTPUT 
                    'PackageDependencies' AS TableName, 
                    (ISNULL(deleted.Id, '') + ' ' + ISNULL(deleted.VersionSpec, '') + ' ' + ISNULL(deleted.TargetFramework, '')) AS Value 
                    INTO @actions
                FROM PackageDependencies pd 
                JOIN Packages p 
                ON p.[Key] = pd.PackageKey 
                WHERE p.[Key] = @key

                DELETE ps 
                FROM PackageStatistics ps 
                JOIN Packages p ON p.[Key] = ps.PackageKey 
                WHERE p.[Key] = @key

                INSERT INTO @actions
                SELECT 'PackageStatistics' AS TableName, @@RowCount AS Value

                DELETE pf 
                FROM PackageEdits pf 
                JOIN Packages p ON p.[Key] = pf.PackageKey 
                WHERE p.[Key] = @key
                    
                INSERT INTO @actions
                SELECT 'PackageEdits' AS TableName, @@RowCount AS Value

                DELETE pe 
                FROM PackageEdits pe
                JOIN Packages p ON p.[Key] = pe.PackageKey 
                WHERE p.[Key] = @key
                    
                INSERT INTO @actions
                SELECT 'PackageEdits' AS TableName, @@RowCount AS Value

                DELETE pf 
                OUTPUT
                    'PackageFrameworks' AS TableName,
                    deleted.TargetFramework AS Value
                    INTO @actions
                FROM PackageFrameworks pf 
                JOIN Packages p ON p.[Key] = pf.Package_Key 
                WHERE p.[Key] = @key

                DELETE ph
                OUTPUT
                    'PackageHistories' AS TableName,
                    deleted.Hash AS Value
                    INTO @actions
                FROM PackageHistories ph 
                JOIN Packages p ON p.[Key] = ph.PackageKey 
                WHERE p.[Key] = @key

                DELETE p 
                OUTPUT
                    'Packages' AS TableName,
                    (pr.Id + ' ' + deleted.NormalizedVersion) AS Value
                    INTO @actions
                FROM Packages p 
                JOIN PackageRegistrations pr ON p.PackageRegistrationKey = pr.[Key]
                WHERE p.[Key] = @key

                SELECT * FROM @actions
                " + (WhatIf ? "ROLLBACK TRAN" : "COMMIT TRAN"), new
                {
                    key = (int)package.Key
                });
            await Console.WriteInfoLine(Strings.Package_DeleteCommand_DatabaseActions);
            await Console.WriteTable(result, d => new
            {
                Action = "DELETE",
                Table = (string)d.TableName,
                Value = (string)d.Value
            });
        }

        private async Task DeletePackageBlob(dynamic package)
        {
            string id = ((string)package.Id).ToLowerInvariant();
            string version = ((string)package.Version).ToLowerInvariant();
            string hash = WebUtility.UrlEncode((string)package.Hash);

            // Get the blob URL
            var client = StorageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference("packages");
            var blob = container.GetBlockBlobReference(
                id + "." + version + ".nupkg");

            var backupContainer = client.GetContainerReference("ng-backups");
            await backupContainer.CreateIfNotExistsAsync();
            var backupBlob = backupContainer.GetBlockBlobReference(
                "packages/" + id + "/" + version + "/" + hash + ".nupkg");

            await Console.WriteInfoLine(Strings.Package_DeleteCommand_CheckingBackup);
            if (await backupBlob.ExistsAsync())
            {
                await Console.WriteInfoLine(Strings.Package_DeleteCommand_BackupExists);
            }
            else
            {
                await Console.WriteInfoLine(Strings.Package_DeleteCommand_BackingUp);
                if (!await blob.ExistsAsync())
                {
                    await Console.WriteWarningLine(Strings.Package_DeleteCommand_MissingSourceBlob);
                }

                // Download the blob locally
                var localFile = Path.GetTempFileName();
                if (File.Exists(localFile))
                {
                    File.Delete(localFile);
                }
                await Console.WriteInfoLine(Strings.Package_DownloadingBlob);
                if (!WhatIf)
                {
                    await blob.DownloadToFileAsync(localFile, FileMode.CreateNew);
                }

                // Upload to the backup blob
                await Console.WriteInfoLine(Strings.Package_DeleteCommand_UploadingBackup);
                if (!WhatIf)
                {
                    await backupBlob.UploadFromFileAsync(localFile, FileMode.Open);
                }
            }

            // Delete the blob
            await Console.WriteInfoLine(Strings.Package_DeleteCommand_DeletingPackageBlob, blob.Uri.AbsoluteUri);
            if (!WhatIf)
            {
                await blob.DeleteIfExistsAsync(
                    DeleteSnapshotsOption.IncludeSnapshots,
                    AccessCondition.GenerateEmptyCondition(),
                    new BlobRequestOptions(), new OperationContext());
            }
        }
    }
}
