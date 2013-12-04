//-------------------------------------------------------------------------------
// <copyright file="ColorElement.cs" company="Appccelerate">
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
    /// Configuration element which represents a color.
    /// </summary>
    public sealed class ColorElement : ConfigurationElement
    {
        private const string BackgroundKeyName = "background";

        private const string ForegroundKeyName = "foreground";

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        /// <value>
        /// The background color.
        /// </value>
        [ConfigurationProperty(BackgroundKeyName, DefaultValue = "FFFFFF", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\GHIJKLMNOPQRSTUVWXYZ", MinLength = 6, MaxLength = 6)]
        public string Background
        {
            get
            {
                return (string)this[BackgroundKeyName];
            }

            set
            {
                this[BackgroundKeyName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the foreground color.
        /// </summary>
        /// <value>
        /// The foreground color.
        /// </value>
        [ConfigurationProperty(ForegroundKeyName, DefaultValue = "000000", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\GHIJKLMNOPQRSTUVWXYZ", MinLength = 6, MaxLength = 6)]
        public string Foreground
        {
            get
            {
                return (string)this[ForegroundKeyName];
            }

            set
            {
                this[ForegroundKeyName] = value;
            }
        }
    }
}