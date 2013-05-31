//-------------------------------------------------------------------------------
// <copyright file="ExtensionConfigurationSection.cs" company="Appccelerate">
//   Copyright (c) 2008-2013
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

namespace Appccelerate.Bootstrapper.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Configuration section for bootstrapper extensions.
    /// </summary>
    public class ExtensionConfigurationSection : ConfigurationSection
    {
        private const string ConfigurationKeyName = "Configuration";

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        [ConfigurationProperty(ConfigurationKeyName,
            IsDefaultCollection = false)]
        public ExtensionSettingsElementCollection Configuration
        {
            get
            {
                ExtensionSettingsElementCollection settingsElementCollection =
                (ExtensionSettingsElementCollection)base[ConfigurationKeyName];
                return settingsElementCollection;
            }

            set
            {
                base[ConfigurationKeyName] = value;
            }
        }
    }
}