﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TeleBot.Visual {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.3.0.0")]
    internal sealed partial class AppSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static AppSettings defaultInstance = ((AppSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new AppSettings())));
        
        public static AppSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("tfu5h4rWwlLyxe8XkSV1E5TqTJMcGWyDYR1gDwxnSPRcUWA5VW5C1dUlYxYzPRhJ")]
        public string BinanceKey {
            get {
                return ((string)(this["BinanceKey"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("gU6ReQQiXlA5wFskKmfUXpSzNR5N6LgrrPY2xBS6uErnsoLRVqyBzpU09wS1u07s")]
        public string BinanceSecret {
            get {
                return ((string)(this["BinanceSecret"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("4c6b9d4bfb88487692bb5e73bd15e63a")]
        public string BittrexKey {
            get {
                return ((string)(this["BittrexKey"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("7404432c2d604f45a764ef0bdd7835d9")]
        public string BittrexSecret {
            get {
                return ((string)(this["BittrexSecret"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("529680487:AAFP2d-wSsdjlr2Zw8NT52wDnMWOKnItwqQ")]
        public string TelegramBotId {
            get {
                return ((string)(this["TelegramBotId"]));
            }
        }
    }
}
