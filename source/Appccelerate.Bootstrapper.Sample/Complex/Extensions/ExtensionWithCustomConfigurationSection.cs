//-------------------------------------------------------------------------------
// <copyright file="ExtensionWithCustomConfigurationSection.cs" company="Appccelerate">
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
    using System.Configuration;
    using System.Globalization;
    using System.Reflection;

    using Appccelerate.Bootstrapper.Configuration;
    using Appccelerate.Bootstrapper.Sample.Complex.Configuration;

    using log4net;

    /// <summary>
    /// Extension which consumes a custom configuration section.
    /// </summary>
    public class ExtensionWithCustomConfigurationSection : ComplexExtensionBase, IConsumeConfigurationSection
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private CustomConfigurationSection section;

        /// <inheritdoc />
        public override string Describe()
        {
            return "Extension which consumes a custom configuration section";
        }

        /// <inheritdoc />
        public void Apply(ConfigurationSection section)
        {
            Ensure.ArgumentNotNull(section, "section");
            Ensure.ArgumentTypeAssignableFrom(typeof(CustomConfigurationSection), section.GetType(), "section");

            this.section = (CustomConfigurationSection)section;
        }

        /// <inheritdoc />
        public override void Start()
        {
            Log.Info("ExtensionWithCustomConfigurationSection is starting.");

            Log.Info(" Color settings:");
            Log.InfoFormat(CultureInfo.InvariantCulture, "  - Background: {0}", this.section.Color.Background);
            Log.InfoFormat(CultureInfo.InvariantCulture, "  - Foreground: {0}", this.section.Color.Foreground);

            Log.Info(" Font settings:");
            Log.InfoFormat(CultureInfo.InvariantCulture, "  - Name: {0}", this.section.Font.Name);
            Log.InfoFormat(CultureInfo.InvariantCulture, "  - Size: {0}", this.section.Font.Size);
        }
    }
}