//-------------------------------------------------------------------------------
// <copyright file="LoadConfigurationSection.cs" company="Appccelerate">
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
    using System;
    using System.Configuration;

    /// <summary>
    /// Default ILoadConfigurationSection
    /// </summary>
    public class LoadConfigurationSection : ILoadConfigurationSection
    {
        private readonly Func<string, ConfigurationSection> sectionProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadConfigurationSection"/> class.
        /// </summary>
        /// <param name="extension">The extension.</param>
        public LoadConfigurationSection(IExtension extension)
        {
            var loader = extension as ILoadConfigurationSection;
            this.sectionProvider =
                section =>
                loader != null
                    ? loader.GetSection(section)
                    : (ConfigurationSection)ConfigurationManager.GetSection(section);
        }

        /// <summary>
        /// Gets a configuration section according to the specified name.
        /// </summary>
        /// <param name="sectionName">The section name.</param>
        /// <returns>
        /// The section.
        /// </returns>
        public ConfigurationSection GetSection(string sectionName)
        {
            return this.sectionProvider(sectionName);
        }
    }
}