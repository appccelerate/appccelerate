//-------------------------------------------------------------------------------
// <copyright file="CustomExtensionWithExtensionConfigurationWhichHasCallbacks.cs" company="Appccelerate">
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
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;

    using Appccelerate.Bootstrapper.Configuration;
    using Appccelerate.Bootstrapper.Configuration.Internals;
    using Appccelerate.Formatters;

    public class CustomExtensionWithExtensionConfigurationWhichHasCallbacks : ICustomExtensionWithExtensionConfiguration,
        IHaveConversionCallbacks, IHaveDefaultConversionCallback, ILoadConfigurationSection
    {
        public IConversionCallback DefaultConversionCallback
        {
            get
            {
                return new FuncConversionCallback((value, info) => string.Format(CultureInfo.InvariantCulture, "{0}. Modified by Default!", value));
            }
        }

        public IDictionary<string, IConversionCallback> ConversionCallbacks
        {
            get
            {
                return new Dictionary<string, IConversionCallback>
                    {
                        { "SomeInt", new FuncConversionCallback((value, info) => Convert.ToInt32(value)) },
                        { "SomeString", new FuncConversionCallback((value, info) => string.Format(CultureInfo.InvariantCulture, "{0}. Modified by Callback!", value)) },
                    };
            }
        }

        public int SomeInt { get; set; }

        public string SomeString { get; set; }

        public string SomeStringWithDefault { get; set; }

        public string SomeStringWhichIsIgnored { get; set; }

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
            return "Custom extension which defines conversion callbacks";
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