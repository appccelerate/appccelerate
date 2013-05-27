//-------------------------------------------------------------------------------
// <copyright file="ExtensionSettingsElement.cs" company="Appccelerate">
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
    /// Defines a service settings configuration element
    /// </summary>
    public sealed class ExtensionSettingsElement : ConfigurationElement
    {
        /// <summary>
        /// The key name.
        /// </summary>
        private const string KeyName = "key";

        /// <summary>
        /// The service settings value name.
        /// </summary>
        private const string ValueName = "value";

        /// <summary>
        /// Gets or sets the key of the service settings configuration element.
        /// </summary>
        /// <value>The key of the settings element.</value>
        [ConfigurationProperty(KeyName, IsRequired = true, IsKey = true)]
        public string Key
        {
            get { return (string)base[KeyName]; }
            set { base[KeyName] = value; }
        }

        /// <summary>
        /// Gets or sets the value of the service settings configuration element.
        /// </summary>
        /// <value>The value of the settings element.</value>
        [ConfigurationProperty(ValueName, IsRequired = true)]
        public string Value
        {
            get { return (string)base[ValueName]; }
            set { base[ValueName] = value; }
        }
    }
}