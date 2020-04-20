﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using NuCmd.Commands.Db;
using NuGet.Services;
using PowerArgs;

namespace NuCmd.Commands.Package
{
    [Description("Adds owners to a set of package IDs")]
    public class AddOwnersCommand : EnvironmentCommandBase
    {
        [ArgRequired]
        [ArgShortcut("i")]
        [ArgDescription("A comma-separated list of the IDs to reserve")]
        public string[] Ids { get; set; }

        [ArgRequired]
        [ArgShortcut("o")]
        [ArgDescription("A comma-separated list of owners to provide for the package registration")]
        public string[] Owners { get; set; }

        [ArgShortcut("db")]
        [ArgDescription("SQL Connection string for the package database. Inferred by default from the environment")]
        public string DatabaseConnectionString { get; set; }

        protected override async Task OnExecute()
        {
            // Gather default data
            SqlConnectionInfo connInfo;
            if (String.IsNullOrEmpty(DatabaseConnectionString))
            {
                connInfo = await GetSqlConnectionInfo(
                    0,
                    KnownSqlConnection.Legacy.ToString(),
                    specifiedAdminUser: null,
                    specifiedAdminPassword: null,
                    promptForPassword: false);
            }
            else
            {
                connInfo = new SqlConnectionInfo(
                    new SqlConnectionStringBuilder(DatabaseConnectionString),
                    credential: null);
            }

            await Console.WriteInfoLine(Strings.Package_ReserveCommand_Reserving,
                String.Join(", ", Ids),
                String.Join(", ", Owners),
                connInfo.ConnectionString.DataSource);
            using (var connection = await connInfo.Connect())
            {
                foreach (var id in Ids)
                {
                    if (!WhatIf)
                    {
                        var result = (await connection.QueryAsync<int>(@"
                            IF EXISTS (SELECT 0 FROM PackageRegistrations WHERE [Id] = @id)
	                            SELECT 0
                            ELSE BEGIN
                                INSERT INTO PackageRegistrations([Id], [DownloadCount]) VALUES(@id, 0)
	                            SELECT 1
                            END",
                            new { id })).Single();
                        if (result == 0)
                        {

                            var existingOwners = await connection.QueryAsync<string>(@"
                                SELECT      u.Username
                                FROM        PackageRegistrations pr
                                INNER JOIN  PackageRegistrationOwners pro ON pro.PackageRegistrationKey = pr.[Key]
                                INNER JOIN  [Users] u ON u.[Key] = pro.UserKey
                                WHERE       pr.[Id] = @id
                                ORDER BY    u.Username",
                            new { id });

                            // If we're in quiet mode, only show errors for packages owned by users not in the target
                            if (existingOwners.Any(o => !Owners.Contains(o)))
                            {
                                await Console.WriteErrorLine(
                                    Strings.Package_ReserveCommand_IdAlreadyExists,
                                    id);

                                foreach (string owner in existingOwners)
                                {
                                    await Console.WriteInfoLine(Strings.Package_ReserveCommand_ExistingOwner, owner);
                                }
                            }

                        }
                    }
                    foreach (var owner in Owners)
                    {
                        if (!WhatIf)
                        {
                            var result = (await connection.QueryAsync<int>(@"
                                DECLARE @prKey int = (SELECT [Key] FROM PackageRegistrations WHERE Id = @id)
                                DECLARE @uKey int = (SELECT [Key] FROM Users WHERE Username = @owner)

                                IF EXISTS (SELECT 0 FROM PackageRegistrationOwners WHERE PackageRegistrationKey = @prKey AND UserKey = @uKey)
	                                SELECT 0
                                ELSE BEGIN
                                    INSERT INTO PackageRegistrationOwners([PackageRegistrationKey], [UserKey])
                                    VALUES(@prKey, @uKey)
	                                SELECT 1
                                END",
                                new { id, owner })).Single();

                            if (result == 0)
                            {
                                await Console.WriteInfoLine(Strings.Package_ReserveCommand_OwnerAlreadyExists, owner, id);
                            }
                        }
                    }
                }
            }
        }
    }
}
