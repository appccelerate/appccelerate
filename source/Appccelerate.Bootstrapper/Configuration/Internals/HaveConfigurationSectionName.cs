//-------------------------------------------------------------------------------
// <copyright file="HaveConfigurationSectionName.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Configuration.Internals
{
    /// <summary>
    /// Default IHaveConfigurationSectionName
    /// </summary>
    public class HaveConfigurationSectionName : IHaveConfigurationSectionName
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HaveConfigurationSectionName"/> class.
        /// </summary>
        /// <param name="extension">The extension.</param>
        public HaveConfigurationSectionName(IExtension extension)
        {
            Ensure.ArgumentNotNull(extension, "extension");

            var namer = extension as IHaveConfigurationSectionName;

            this.SectionName = namer != null
                ? namer.SectionName
                : extension.GetType().Name;
        }

        /// <summary>
        /// Gets the section name.
        /// </summary>
        public string SectionName { get; private set; }
    }
}