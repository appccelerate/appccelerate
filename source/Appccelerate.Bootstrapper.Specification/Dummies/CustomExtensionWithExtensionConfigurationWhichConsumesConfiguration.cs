//-------------------------------------------------------------------------------
// <copyright file="CustomExtensionWithExtensionConfigurationWhichConsumesConfiguration.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.Configuration;

    using Appccelerate.Bootstrapper.Configuration;
    using Appccelerate.Formatters;

    public class CustomExtensionWithExtensionConfigurationWhichConsumesConfiguration : ICustomExtensionWithExtensionConfiguration,
        ILoadConfigurationSection, IConsumeConfiguration
    {
        public CustomExtensionWithExtensionConfigurationWhichConsumesConfiguration()
        {
            this.Configuration = new Dictionary<string, string>();
        }

        public IDictionary<string, string> Configuration { get; private set; }

        public string SectionAcquired { get; private set; }

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
            return "Custom extension which consumes the configuration section as dictionary";
        }

        public ConfigurationSection GetSection(string sectionName)
        {
            this.SectionAcquired = sectionName;

            return ExtensionConfigurationSectionHelper.CreateSection(
                new KeyValuePair<string, string>("SomeInt", "1"),
                new KeyValuePair<string, string>("SomeString", "SomeString"),
                new KeyValuePair<string, string>("SomeStringWithDefault", "SomeStringWithDefault"));
        }
    }
}