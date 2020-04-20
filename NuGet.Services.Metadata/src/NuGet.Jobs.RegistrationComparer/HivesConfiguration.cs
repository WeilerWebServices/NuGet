// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Jobs.RegistrationComparer
{
    public class HivesConfiguration
    {
        public HiveConfiguration Legacy { get; set; }
        public HiveConfiguration Gzipped { get; set; }
        public HiveConfiguration SemVer2 { get; set; }
    }
}
