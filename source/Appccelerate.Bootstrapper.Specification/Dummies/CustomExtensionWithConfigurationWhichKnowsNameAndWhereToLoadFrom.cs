//-------------------------------------------------------------------------------
// <copyright file="CustomExtensionWithConfigurationWhichKnowsNameAndWhereToLoadFrom.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Specification.Dummies
{
    using System.Configuration;

    using Appccelerate.Bootstrapper.Configuration;
    using Appccelerate.Formatters;

    public class CustomExtensionWithConfigurationWhichKnowsNameAndWhereToLoadFrom : ICustomExtensionWithConfiguration,
        IHaveConfigurationSectionName, ILoadConfigurationSection, IConsumeConfigurationSection
    {
        public bool SectionNameAcquired { get; private set; }

        public string SectionAcquired { get; private set; }

        public FakeConfigurationSection AppliedSection { get; private set; }

        public string SectionName
        {
            get
            {
                this.SectionNameAcquired = true;

                return "FakeConfigurationSection";
            }
        }

        /// <inheritdoc />
        public string Name
        {
            get
            {
                return this.GetType().FullNameToString();
            }
        }

        public string Describe()
        {
            return
                "Custom Extension which defines a custom section name and overrides the configuration section loading mechanism";
        }

        public void Apply(ConfigurationSection section)
        {
            this.AppliedSection = section as FakeConfigurationSection;
        }

        public ConfigurationSection GetSection(string sectionName)
        {
            this.SectionAcquired = sectionName;

            return sectionName == this.SectionName ? new FakeConfigurationSection("KnowsName|KnowsLoading") : null;
        }
    }
}