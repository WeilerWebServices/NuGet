﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Build", "CA2211:Non-constant fields should not be visible", Justification = "<Pending>", Scope = "member", Target = "~F:NuGet.LibraryModel.LibraryDependencyType.Build")]
[assembly: SuppressMessage("Build", "CA2211:Non-constant fields should not be visible", Justification = "<Pending>", Scope = "member", Target = "~F:NuGet.LibraryModel.LibraryDependencyType.Default")]
[assembly: SuppressMessage("Build", "CA2211:Non-constant fields should not be visible", Justification = "<Pending>", Scope = "member", Target = "~F:NuGet.LibraryModel.LibraryDependencyType.Platform")]
[assembly: SuppressMessage("Build", "CA1308:In method 'GetHashCode', replace the call to 'ToLowerInvariant' with 'ToUpperInvariant'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.LibraryModel.CentralPackageVersion.GetHashCode~System.Int32")]
[assembly: SuppressMessage("Build", "CA1062:In externally visible method 'int DownloadDependency.CompareTo(DownloadDependency other)', validate parameter 'other' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.LibraryModel.DownloadDependency.CompareTo(NuGet.LibraryModel.DownloadDependency)~System.Int32")]
[assembly: SuppressMessage("Build", "CA1308:In method 'GetHashCode', replace the call to 'ToLowerInvariant' with 'ToUpperInvariant'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.LibraryModel.DownloadDependency.GetHashCode~System.Int32")]
[assembly: SuppressMessage("Build", "CA2225:Provide a method named 'ToLibraryRange' or 'FromDownloadDependency' as an alternate for operator op_Implicit.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.LibraryModel.DownloadDependency.op_Implicit(NuGet.LibraryModel.DownloadDependency)~NuGet.LibraryModel.LibraryRange")]
[assembly: SuppressMessage("Build", "CA1062:In externally visible method 'DownloadDependency.implicit operator LibraryRange(DownloadDependency library)', validate parameter 'library' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.LibraryModel.DownloadDependency.op_Implicit(NuGet.LibraryModel.DownloadDependency)~NuGet.LibraryModel.LibraryRange")]
[assembly: SuppressMessage("Build", "CA1308:In method 'GetFlagString', replace the call to 'ToLowerInvariant' with 'ToUpperInvariant'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.LibraryModel.LibraryDependencyTargetUtils.GetFlagString(NuGet.LibraryModel.LibraryDependencyTarget)~System.String")]
[assembly: SuppressMessage("Build", "CA1305:The behavior of 'string.Format(string, object)' could vary based on the current user's locale settings. Replace this call in 'LibraryDependencyTypeKeyword.Parse(string)' with a call to 'string.Format(IFormatProvider, string, params object[])'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.LibraryModel.LibraryDependencyTypeKeyword.Parse(System.String)~NuGet.LibraryModel.LibraryDependencyTypeKeyword")]
[assembly: SuppressMessage("Build", "CA1062:In externally visible method 'T LibraryExtensions.GetItem<T>(Library library, string key)', validate parameter 'library' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.LibraryModel.LibraryExtensions.GetItem``1(NuGet.LibraryModel.Library,System.String)~``0")]
[assembly: SuppressMessage("Build", "CA1062:In externally visible method 'T LibraryExtensions.GetRequiredItem<T>(Library library, string key)', validate parameter 'library' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.LibraryModel.LibraryExtensions.GetRequiredItem``1(NuGet.LibraryModel.Library,System.String)~``0")]
[assembly: SuppressMessage("Build", "CA1062:In externally visible method 'bool LibraryExtensions.IsEclipsedBy(LibraryRange library, LibraryRange other)', validate parameter 'other' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.LibraryModel.LibraryExtensions.IsEclipsedBy(NuGet.LibraryModel.LibraryRange,NuGet.LibraryModel.LibraryRange)~System.Boolean")]
[assembly: SuppressMessage("Build", "CA1062:In externally visible method 'int LibraryIdentity.CompareTo(LibraryIdentity other)', validate parameter 'other' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.LibraryModel.LibraryIdentity.CompareTo(NuGet.LibraryModel.LibraryIdentity)~System.Int32")]
[assembly: SuppressMessage("Build", "CA2225:Provide a method named 'ToLibraryRange' or 'FromLibraryIdentity' as an alternate for operator op_Implicit.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.LibraryModel.LibraryIdentity.op_Implicit(NuGet.LibraryModel.LibraryIdentity)~NuGet.LibraryModel.LibraryRange")]
[assembly: SuppressMessage("Build", "CA1062:In externally visible method 'LibraryIdentity.implicit operator LibraryRange(LibraryIdentity library)', validate parameter 'library' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.LibraryModel.LibraryIdentity.op_Implicit(NuGet.LibraryModel.LibraryIdentity)~NuGet.LibraryModel.LibraryRange")]
[assembly: SuppressMessage("Build", "CA1308:In method 'GetFlags', replace the call to 'ToLowerInvariant' with 'ToUpperInvariant'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.LibraryModel.LibraryIncludeFlagUtils.GetFlags(System.Collections.Generic.IEnumerable{System.String})~NuGet.LibraryModel.LibraryIncludeFlags")]
[assembly: SuppressMessage("Build", "CA1308:In method 'GetFlagString', replace the call to 'ToLowerInvariant' with 'ToUpperInvariant'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.LibraryModel.LibraryIncludeFlagUtils.GetFlagString(NuGet.LibraryModel.LibraryIncludeFlags)~System.String")]
[assembly: SuppressMessage("Build", "CA2227:Change 'Items' to be read-only by removing the property setter.", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.LibraryModel.Library.Items")]
[assembly: SuppressMessage("Build", "CA2227:Change 'NoWarn' to be read-only by removing the property setter.", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.LibraryModel.LibraryDependency.NoWarn")]
[assembly: SuppressMessage("Build", "CA1067:Type NuGet.LibraryModel.FrameworkDependency should override Equals because it implements IEquatable<T>", Justification = "<Pending>", Scope = "type", Target = "~T:NuGet.LibraryModel.FrameworkDependency")]
[assembly: SuppressMessage("Build", "CA1052:Type 'FrameworkDependencyFlagsUtils' is a static holder type but is neither static nor NotInheritable", Justification = "<Pending>", Scope = "type", Target = "~T:NuGet.LibraryModel.FrameworkDependencyFlagsUtils")]
[assembly: SuppressMessage("Build", "CA1052:Type 'LibraryDependencyTargetUtils' is a static holder type but is neither static nor NotInheritable", Justification = "<Pending>", Scope = "type", Target = "~T:NuGet.LibraryModel.LibraryDependencyTargetUtils")]
[assembly: SuppressMessage("Build", "CA1052:Type 'LibraryIncludeFlagUtils' is a static holder type but is neither static nor NotInheritable", Justification = "<Pending>", Scope = "type", Target = "~T:NuGet.LibraryModel.LibraryIncludeFlagUtils")]
