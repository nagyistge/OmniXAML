﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Xaml.Tests.Resources {
    using System;
    using System.Reflection;
    
    
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
    public class ScannerInputs {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ScannerInputs() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Xaml.Tests.Resources.ScannerInputs", typeof(ScannerInputs).GetTypeInfo().Assembly);
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
        ///   Looks up a localized string similar to &lt;DummyClass xmlns=&quot;root&quot; xmlns:a=&quot;another&quot;  /&gt;.
        /// </summary>
        public static string ElementWith2NsDeclarations {
            get {
                return ResourceManager.GetString("ElementWith2NsDeclarations", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;DummyClass&gt;
        ///&lt;DummyClass.Child&gt;
        ///&lt;ChildClass /&gt;
        ///&lt;/DummyClass.Child&gt;
        ///&lt;/DummyClass&gt;.
        /// </summary>
        public static string ElementWithChild {
            get {
                return ResourceManager.GetString("ElementWithChild", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;DummyClass&gt;
        ///&lt;Parent.Property/&gt;
        ///&lt;/DummyClass&gt;.
        /// </summary>
        public static string ElementWithCollapsedProperty {
            get {
                return ResourceManager.GetString("ElementWithCollapsedProperty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;DummyClass&gt;
        ///&lt;Parent.Property&gt;
        ///&lt;/Parent.Property&gt;
        ///&lt;/DummyClass&gt;.
        /// </summary>
        public static string ElementWithPropertyNoContents {
            get {
                return ResourceManager.GetString("ElementWithPropertyNoContents", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;Parent.Property&gt;.
        /// </summary>
        public static string PropertyTagOpen {
            get {
                return ResourceManager.GetString("PropertyTagOpen", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;DummyClass/&gt;.
        /// </summary>
        public static string SingleCollapsed {
            get {
                return ResourceManager.GetString("SingleCollapsed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;DummyClass xmlns=&quot;root&quot;  /&gt;.
        /// </summary>
        public static string SingleCollapsedWithNs {
            get {
                return ResourceManager.GetString("SingleCollapsedWithNs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;DummyClass&gt;&lt;/DummyClass&gt;.
        /// </summary>
        public static string SingleOpenAndClose {
            get {
                return ResourceManager.GetString("SingleOpenAndClose", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;DummyClass xmlns=&quot;root&quot;&gt;&lt;/DummyClass&gt;.
        /// </summary>
        public static string SingleOpenAndCloseWithNs {
            get {
                return ResourceManager.GetString("SingleOpenAndCloseWithNs", resourceCulture);
            }
        }
    }
}