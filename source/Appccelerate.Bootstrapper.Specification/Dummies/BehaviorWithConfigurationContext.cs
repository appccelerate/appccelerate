//-------------------------------------------------------------------------------
// <copyright file="BehaviorWithConfigurationContext.cs" company="Appccelerate">
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
    using System.Globalization;
    using System.Linq;

    using Appccelerate.Formatters;

    public class BehaviorWithConfigurationContext : IBehavior<ICustomExtension>
    {
        private readonly IDictionary<string, string> configuration;

        private readonly string key;

        private readonly string value;

        public BehaviorWithConfigurationContext(IDictionary<string, string> configuration, string key, string value)
        {
            this.value = value;
            this.key = key;
            this.configuration = configuration;
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
        public void Behave(IEnumerable<ICustomExtension> extensions)
        {
            extensions.ToList().ForEach(e => e.Dump(string.Format(CultureInfo.InvariantCulture, "configuration modification with {0} = {1}", this.key, this.value)));

            this.configuration.Add(this.key, this.value);
        }

        /// <inheritdoc />
        public string Describe()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "Dumps the key \"{0}\" and value \"{1}\" and modifies the configuration with it.",
                this.key,
                this.value);
        }
    }
}