//-------------------------------------------------------------------------------
// <copyright file="CustomConfigurationSection.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Sample.Complex.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Custom configuration section example stolen from
    /// http://msdn.microsoft.com/en-us/library/2tw134k3.aspx
    /// </summary>
    public sealed class CustomConfigurationSection : ConfigurationSection
    {
        private const string RemoteOnlyKeyName = "remoteOnly";

        private const string FontKeyName = "font";

        private const string ColorKeyName = "color";

        /// <summary>
        /// Gets or sets a value indicating whether the settings are applied remote only.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the settings should be applied remote only; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty(RemoteOnlyKeyName, DefaultValue = "false", IsRequired = false)]
        public bool RemoteOnly
        {
            get
            {
                return (bool)this[RemoteOnlyKeyName];
            }

            set
            {
                this[RemoteOnlyKeyName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        /// <value>
        /// The font.
        /// </value>
        [ConfigurationProperty(FontKeyName)]
        public FontElement Font
        {
            get
            {
                return (FontElement)this[FontKeyName];
            }

            set
            {
                this[FontKeyName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        [ConfigurationProperty(ColorKeyName)]
        public ColorElement Color
        {
            get
            {
                return (ColorElement)this[ColorKeyName];
            }

            set
            {
                this[ColorKeyName] = value;
            }
        }
    }
}