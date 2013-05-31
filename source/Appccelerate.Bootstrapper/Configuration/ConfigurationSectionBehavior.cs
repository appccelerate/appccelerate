//-------------------------------------------------------------------------------
// <copyright file="ConfigurationSectionBehavior.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.Configuration;

    using Appccelerate.Formatters;

    /// <summary>
    /// Adds behavior to the IBootstrapper to load configuration sections.
    /// </summary>
    public class ConfigurationSectionBehavior : IBehavior<IExtension>
    {
        private readonly IConfigurationSectionBehaviorFactory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSectionBehavior"/> class.
        /// </summary>
        public ConfigurationSectionBehavior()
            : this(new DefaultConfigurationSectionBehaviorFactory())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSectionBehavior"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public ConfigurationSectionBehavior(IConfigurationSectionBehaviorFactory factory)
        {
            this.factory = factory;
        }

        /// <inheritdoc />
        public string Name
        {
            get
            {
                return this.GetType().FullNameToString();
            }
        }

        /// <inheritdoc />
        public void Behave(IEnumerable<IExtension> extensions)
        {
            Ensure.ArgumentNotNull(extensions, "extensions");

            foreach (IExtension extension in extensions)
            {
                IConsumeConfigurationSection consumer = this.factory.CreateConsumeConfigurationSection(extension);
                IHaveConfigurationSectionName sectionNameProvider = this.factory.CreateHaveConfigurationSectionName(extension);
                ILoadConfigurationSection sectionProvider = this.factory.CreateLoadConfigurationSection(extension);

                string sectionName = sectionNameProvider.SectionName;
                ConfigurationSection section = sectionProvider.GetSection(sectionName);

                consumer.Apply(section);
            }
        }

        /// <inheritdoc />
        public string Describe()
        {
            return "Automatically provides configuration sections for all extensions.";
        }
    }
}