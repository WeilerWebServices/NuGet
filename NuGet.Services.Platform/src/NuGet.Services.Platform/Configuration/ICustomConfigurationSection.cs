﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuGet.Services.Configuration
{
    public interface ICustomConfigurationSection
    {
        void Resolve(string prefix, ConfigurationHub hub);
    }
}
