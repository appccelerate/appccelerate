//-------------------------------------------------------------------------------
// <copyright file="FontElement.cs" company="Appccelerate">
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
    /// The configuration element which represents font configuration possibilities.
    /// </summary>
    public sealed class FontElement : ConfigurationElement
    {
        private const string NameKeyName = "name";

        private const string SizeKeyName = "size";

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [ConfigurationProperty(NameKeyName, DefaultValue = "Arial", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 60)]
        public string Name
        {
            get
            {
                return (string)this[NameKeyName];
            }

            set
            {
                this[NameKeyName] = value;
            }
        }

        /// <summary>
        /// Gets or sets the font size.
        /// </summary>
        /// <value>
        /// The font size.
        /// </value>
        [ConfigurationProperty(SizeKeyName, DefaultValue = "12", IsRequired = false)]
        [IntegerValidator(ExcludeRange = false, MaxValue = 24, MinValue = 6)]
        public int Size
        {
            get
            {
                return (int)this[SizeKeyName];
            }

            set
            {
                this[SizeKeyName] = value;
            }
        }
    }
}