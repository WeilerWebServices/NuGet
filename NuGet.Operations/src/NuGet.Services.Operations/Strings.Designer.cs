﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NuGet.Services.Operations {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("NuGet.Services.Operations.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No secret stores are configured..
        /// </summary>
        public static string DeploymentEnvironment_NoSecretStore {
            get {
                return ResourceManager.GetString("DeploymentEnvironment_NoSecretStore", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No secret store named &apos;{0}&apos; is configured for this environment..
        /// </summary>
        public static string DeploymentEnvironment_NoSecretStoreWithName {
            get {
                return ResourceManager.GetString("DeploymentEnvironment_NoSecretStoreWithName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown datacenter: {0}..
        /// </summary>
        public static string DeploymentEnvironment_UnknownDatacenter {
            get {
                return ResourceManager.GetString("DeploymentEnvironment_UnknownDatacenter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This protector can only unprotect because it is missing the protection descriptor..
        /// </summary>
        public static string DpapiNGDataProtector_ProtectorCanOnlyUnprotect {
            get {
                return ResourceManager.GetString("DpapiNGDataProtector_ProtectorCanOnlyUnprotect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Secret store is corrupt. Metadata file &apos;{0}&apos; is missing..
        /// </summary>
        public static string DpapiSecretStoreProvider_MissingMetadata {
            get {
                return ResourceManager.GetString("DpapiSecretStoreProvider_MissingMetadata", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A secret store does not exist for &apos;{0}&apos;..
        /// </summary>
        public static string DpapiSecretStoreProvider_StoreDoesNotExist {
            get {
                return ResourceManager.GetString("DpapiSecretStoreProvider_StoreDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A secret store already exists for &apos;{0}&apos;..
        /// </summary>
        public static string DpapiSecretStoreProvider_StoreExists {
            get {
                return ResourceManager.GetString("DpapiSecretStoreProvider_StoreExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &apos;{0}&apos; environment variable is empty..
        /// </summary>
        public static string OperationsSession_EnvironmentVariableIsEmpty {
            get {
                return ResourceManager.GetString("OperationsSession_EnvironmentVariableIsEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The application model file &apos;{0}&apos; is invalid..
        /// </summary>
        public static string OperationsSession_InvalidAppModel {
            get {
                return ResourceManager.GetString("OperationsSession_InvalidAppModel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The service model file &apos;{0}&apos; does not exist..
        /// </summary>
        public static string OperationsSession_ServiceModelDoesNotExist {
            get {
                return ResourceManager.GetString("OperationsSession_ServiceModelDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown environment: &apos;{0}&apos;..
        /// </summary>
        public static string OperationsSession_UnknownEnvironment {
            get {
                return ResourceManager.GetString("OperationsSession_UnknownEnvironment", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;oldValue&apos; must be specified when recording a change..
        /// </summary>
        public static string SecretAuditEntry_OldValueRequired {
            get {
                return ResourceManager.GetString("SecretAuditEntry_OldValueRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid key. Expected &quot;&lt;datacenter&gt;:&lt;base64-encoded-name&gt;&quot;.
        /// </summary>
        public static string SecretKey_Invalid {
            get {
                return ResourceManager.GetString("SecretKey_Invalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown secret store type: &apos;{0}&apos;..
        /// </summary>
        public static string SecretStoreConnection_UnknownSecretStoreType {
            get {
                return ResourceManager.GetString("SecretStoreConnection_UnknownSecretStoreType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ambiguous match for secret &apos;{0}&apos; in scope &apos;{1}&apos;..
        /// </summary>
        public static string SharepointSecretStoreConnection_AmbiguousMatch {
            get {
                return ResourceManager.GetString("SharepointSecretStoreConnection_AmbiguousMatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This type does not support version &apos;{0}&apos; of the Sharepoint Secret Store..
        /// </summary>
        public static string SharepointSecretStoreConnection_UnsupportedVersion {
            get {
                return ResourceManager.GetString("SharepointSecretStoreConnection_UnsupportedVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Config setting is missing required attribute &apos;name&apos;..
        /// </summary>
        public static string XmlServiceModelDeserializer_SettingMissingName {
            get {
                return ResourceManager.GetString("XmlServiceModelDeserializer_SettingMissingName", resourceCulture);
            }
        }
    }
}