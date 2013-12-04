//-------------------------------------------------------------------------------
// <copyright file="ExtensionWithExtensionConfigurationSectionWithDictionary.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Sample.Complex.Extensions
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;

    using Appccelerate.Bootstrapper.Configuration;

    using log4net;

    /// <summary>
    /// Configuration section which consumes key value pairs from the corresponding <see cref="ExtensionConfigurationSection"/>.
    /// </summary>
    public class ExtensionWithExtensionConfigurationSectionWithDictionary : ComplexExtensionBase,
        IConsumeConfiguration
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionWithExtensionConfigurationSectionWithDictionary"/> class.
        /// </summary>
        public ExtensionWithExtensionConfigurationSectionWithDictionary()
        {
            this.Configuration = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets the configuration dictionary which is filled by the producer.
        /// </summary>
        public IDictionary<string, string> Configuration { get; private set; }

        /// <inheritdoc />
        public override void Start()
        {
            base.Start();

            Log.Info("ExtensionWithExtensionConfigurationSectionWithDictionary is starting.");

            foreach (KeyValuePair<string, string> keyValuePair in this.Configuration)
            {
                Log.InfoFormat(CultureInfo.InvariantCulture, " - Key {0} / Value {1}", keyValuePair.Key, keyValuePair.Value);
            }
        }

        /// <inheritdoc />
        public override string Describe()
        {
            return "Extension which has a dictionary which is filled through a configuration section";
        }
    }
}