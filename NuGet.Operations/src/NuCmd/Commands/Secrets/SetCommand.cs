﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using NuGet.Services.Operations;
using NuGet.Services.Operations.Secrets;
using PowerArgs;

namespace NuCmd.Commands.Secrets
{
    [Description("Stores a value in the secret store, or sets an existing value")]
    public class SetCommand : SecretStoreCommandBase
    {
        [ArgRequired]
        [ArgPosition(0)]
        [ArgShortcut("k")]
        [ArgDescription("The name of the key to set")]
        public string Key { get; set; }

        [ArgPosition(1)]
        [ArgShortcut("v")]
        [ArgDescription("The value of the key to set. Leave blank to prompt for it as a password.")]
        public string Value { get; set; }

        [ArgShortcut("xin")]
        [ArgDescription("Sets the expiry date of the secret to the current date, plus this time")]
        public TimeSpan? ExpiresIn { get; set; }

        [ArgShortcut("xat")]
        [ArgDescription("Sets the expiry date of the secret to the provided date, in local time.")]
        public DateTime? ExpiresAt { get; set; }

        [ArgShortcut("t")]
        [ArgDescription("The type of the secret. Defaults to 'password'")]
        public SecretType? Type { get; set; }

        [ArgShortcut("gen")]
        [ArgDescription("Generates a new password as the value for the key.")]
        public bool Generate { get; set; }

        protected override async Task OnExecute()
        {
            // Open the store
            var store = await OpenSecretStore();

            if (String.IsNullOrEmpty(Value))
            {
                if (Generate)
                {
                    Value = Utils.GeneratePassword(timestamped: true);
                }
                else
                {
                    var secureValue = await Console.PromptForPassword(String.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Secrets_SetCommand_EnterValue,
                        Key));

                    // PromptForPassword returns a SecureString, but we need it to be unsecure for serialization unfortunately :(
                    IntPtr p = IntPtr.Zero;
                    try
                    {
                        p = Marshal.SecureStringToGlobalAllocUnicode(secureValue);
                        Value = Marshal.PtrToStringUni(p);
                    }
                    finally
                    {
                        Marshal.ZeroFreeGlobalAllocUnicode(p);
                    }
                }
            }

            if (ExpiresIn != null)
            {
                ExpiresAt = DateTime.Now + ExpiresIn.Value;
            }
            if (ExpiresAt != null)
            {
                ExpiresAt = ExpiresAt.Value.ToUniversalTime();
            }

            // Create the secret to set
            var secret = new Secret(new SecretName(Key, Datacenter), Value, DateTime.UtcNow, ExpiresAt, Type ?? SecretType.Password);

            if (ExpiresAt == null)
            {
                await Console.WriteInfoLine(Strings.Secrets_SetCommand_Writing, Key);
            }
            else
            {
                await Console.WriteInfoLine(Strings.Secrets_SetCommand_WritingWithExpiry, Key, ExpiresAt.Value);
            }

            if (!WhatIf)
            {
                await store.Write(secret, "nucmd set");
            }

            await Console.WriteInfoLine(Strings.Secrets_SetCommand_Written, Key);
        }
    }
}
