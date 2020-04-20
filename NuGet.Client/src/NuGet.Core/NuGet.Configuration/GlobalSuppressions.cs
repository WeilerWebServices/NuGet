﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Build", "CA1825:Avoid unnecessary zero-length array allocations.  Use Array.Empty<string>() instead.", Justification = "<Pending>", Scope = "member", Target = "~F:NuGet.Configuration.WebProxy._bypassList")]
[assembly: SuppressMessage("Build", "CA1304:The behavior of 'string.ToLower()' could vary based on the current user's locale settings. Replace this call in 'CertificateItem.CertificateItem(string, HashAlgorithmName, [bool])' with a call to 'string.ToLower(CultureInfo)'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.CertificateItem.#ctor(System.String,NuGet.Common.HashAlgorithmName,System.Boolean)")]
[assembly: SuppressMessage("Build", "CA1305:The behavior of 'bool.ToString()' could vary based on the current user's locale settings. Replace this call in 'CertificateItem.CertificateItem(string, HashAlgorithmName, [bool])' with a call to 'bool.ToString(IFormatProvider)'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.CertificateItem.#ctor(System.String,NuGet.Common.HashAlgorithmName,System.Boolean)")]
[assembly: SuppressMessage("Build", "CA1304:The behavior of 'string.ToUpper()' could vary based on the current user's locale settings. Replace this call in 'CertificateItem.CertificateItem(XElement, SettingsFile)' with a call to 'string.ToUpper(CultureInfo)'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.CertificateItem.#ctor(System.Xml.Linq.XElement,NuGet.Configuration.SettingsFile)")]
[assembly: SuppressMessage("Build", "CA1305:The behavior of 'bool.ToString()' could vary based on the current user's locale settings. Replace this call in 'CertificateItem.CertificateItem(XElement, SettingsFile)' with a call to 'bool.ToString(IFormatProvider)'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.CertificateItem.#ctor(System.Xml.Linq.XElement,NuGet.Configuration.SettingsFile)")]
[assembly: SuppressMessage("Build", "CA1507:Use nameof in place of string literal 'path'", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.FileSystemUtility.EnsureTrailingCharacter(System.String,System.Char)~System.String")]
[assembly: SuppressMessage("Build", "CA1305:The behavior of 'int.ToString()' could vary based on the current user's locale settings. Replace this call in 'NuGetConfiguration.NuGetConfiguration(SettingsFile)' with a call to 'int.ToString(IFormatProvider)'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.NuGetConfiguration.#ctor(NuGet.Configuration.SettingsFile)")]
[assembly: SuppressMessage("Build", "CA1305:The behavior of 'string.Format(string, object)' could vary based on the current user's locale settings. Replace this call in 'NuGetConfiguration.NuGetConfiguration(XElement, SettingsFile)' with a call to 'string.Format(IFormatProvider, string, params object[])'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.NuGetConfiguration.#ctor(System.Xml.Linq.XElement,NuGet.Configuration.SettingsFile)")]
[assembly: SuppressMessage("Build", "CA1305:The behavior of 'char.ToString()' could vary based on the current user's locale settings. Replace this call in 'OwnersItem.AsXNode()' with a call to 'char.ToString(IFormatProvider)'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.OwnersItem.AsXNode~System.Xml.Linq.XNode")]
[assembly: SuppressMessage("Build", "CA1305:The behavior of 'char.ToString()' could vary based on the current user's locale settings. Replace this call in 'OwnersItem.Update(SettingItem)' with a call to 'char.ToString(IFormatProvider)'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.OwnersItem.Update(NuGet.Configuration.SettingItem)")]
[assembly: SuppressMessage("Build", "CA1062:In externally visible method 'PackageSourceProvider.PackageSourceProvider(ISettings settings, IEnumerable<PackageSource> configurationDefaultSources, bool enablePackageSourcesChangedEvent)', validate parameter 'configurationDefaultSources' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.PackageSourceProvider.#ctor(NuGet.Configuration.ISettings,System.Collections.Generic.IEnumerable{NuGet.Configuration.PackageSource},System.Boolean)")]
[assembly: SuppressMessage("Build", "CA1031:Modify 'AddDisabledSource' to catch a more specific allowed exception type, or rethrow the exception.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.PackageSourceProvider.AddDisabledSource(System.String,System.Boolean,System.Boolean@)")]
[assembly: SuppressMessage("Build", "CA1031:Modify 'RemovePackageSource' to catch a more specific allowed exception type, or rethrow the exception.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.PackageSourceProvider.RemovePackageSource(System.String,System.Boolean,System.Boolean@)")]
[assembly: SuppressMessage("Build", "CA1062:In externally visible method 'void PackageSourceProvider.SaveActivePackageSource(PackageSource source)', validate parameter 'source' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.PackageSourceProvider.SaveActivePackageSource(NuGet.Configuration.PackageSource)")]
[assembly: SuppressMessage("Build", "CA1031:Modify 'SaveActivePackageSource' to catch a more specific allowed exception type, or rethrow the exception.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.PackageSourceProvider.SaveActivePackageSource(NuGet.Configuration.PackageSource)")]
[assembly: SuppressMessage("Build", "CA1062:In externally visible method 'IWebProxy ProxyCache.GetProxy(Uri sourceUri)', validate parameter 'sourceUri' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.ProxyCache.GetProxy(System.Uri)~System.Net.IWebProxy")]
[assembly: SuppressMessage("Build", "CA1307:The behavior of 'string.Equals(string, string)' could vary based on the current user's locale settings. Replace this call in 'NuGet.Configuration.ProxyCache.IsSystemProxySet(System.Uri)' with a call to 'string.Equals(string, string, System.StringComparison)'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.ProxyCache.IsSystemProxySet(System.Uri)~System.Boolean")]
[assembly: SuppressMessage("Build", "CA1305:The behavior of 'char.ToString()' could vary based on the current user's locale settings. Replace this call in 'RepositoryItem.Clone()' with a call to 'char.ToString(IFormatProvider)'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.RepositoryItem.Clone~NuGet.Configuration.SettingBase")]
[assembly: SuppressMessage("Build", "CA1305:The behavior of 'char.ToString()' could vary based on the current user's locale settings. Replace this call in 'RepositoryItem.Update(SettingItem)' with a call to 'char.ToString(IFormatProvider)'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.RepositoryItem.Update(NuGet.Configuration.SettingItem)")]
[assembly: SuppressMessage("Build", "CA1062:In externally visible method 'ISettings Settings.LoadImmutableSettingsGivenConfigPaths(IList<string> configFilePaths, SettingsLoadingContext settingsLoadingContext)', validate parameter 'settingsLoadingContext' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.Settings.LoadImmutableSettingsGivenConfigPaths(System.Collections.Generic.IList{System.String},NuGet.Configuration.SettingsLoadingContext)~NuGet.Configuration.ISettings")]
[assembly: SuppressMessage("Build", "CA1303:Method 'ISettings Settings.LoadMachineWideSettings(string root, params string[] paths)' passes a literal string as parameter 'message' of a call to 'ArgumentException.ArgumentException(string message)'. Retrieve the following string(s) from a resource table instead: \"root cannot be null or empty\".", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.Settings.LoadMachineWideSettings(System.String,System.String[])~NuGet.Configuration.ISettings")]
[assembly: SuppressMessage("Build", "CA1827:Count() is used where Any() could be used instead to improve performance.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.TrustedSignerItem.#ctor(System.Xml.Linq.XElement,NuGet.Configuration.SettingsFile)")]
[assembly: SuppressMessage("Build", "CA1031:Modify 'TryRemoveAllMergedWith' to catch a more specific allowed exception type, or rethrow the exception.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.VirtualSettingSection.TryRemoveAllMergedWith(NuGet.Configuration.SettingItem,NuGet.Configuration.SettingItem@)~System.Boolean")]
[assembly: SuppressMessage("Build", "CA1307:The behavior of 'string.IndexOf(string)' could vary based on the current user's locale settings. Replace this call in 'NuGet.Configuration.WebProxy.CreateProxyUri(string)' with a call to 'string.IndexOf(string, System.StringComparison)'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.WebProxy.CreateProxyUri(System.String)~System.Uri")]
[assembly: SuppressMessage("Build", "CA2000:Call System.IDisposable.Dispose on object created by 'XmlReader.Create(input, settings)' before all references to it are out of scope.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Configuration.XmlUtility.LoadSafe(System.IO.Stream,System.Xml.Linq.LoadOptions)~System.Xml.Linq.XDocument")]
[assembly: SuppressMessage("Build", "CA1305:The behavior of 'bool.ToString()' could vary based on the current user's locale settings. Replace this call in 'CertificateItem.AllowUntrustedRoot.set' with a call to 'bool.ToString(IFormatProvider)'.", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Configuration.CertificateItem.AllowUntrustedRoot")]
[assembly: SuppressMessage("Build", "CA1304:The behavior of 'string.ToLower()' could vary based on the current user's locale settings. Replace this call in 'CertificateItem.AllowUntrustedRoot.set' with a call to 'string.ToLower(CultureInfo)'.", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Configuration.CertificateItem.AllowUntrustedRoot")]
[assembly: SuppressMessage("Build", "CA1304:The behavior of 'string.ToUpper()' could vary based on the current user's locale settings. Replace this call in 'CertificateItem.HashAlgorithm.set' with a call to 'string.ToUpper(CultureInfo)'.", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Configuration.CertificateItem.HashAlgorithm")]
[assembly: SuppressMessage("Build", "CA1825:Avoid unnecessary zero-length array allocations.  Use Array.Empty<string>() instead.", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Configuration.WebProxy.BypassList")]
[assembly: SuppressMessage("Build", "CA2237:Add [Serializable] to NuGetConfigurationException as this type implements ISerializable", Justification = "<Pending>", Scope = "type", Target = "~T:NuGet.Configuration.NuGetConfigurationException")]
