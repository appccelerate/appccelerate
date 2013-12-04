//-------------------------------------------------------------------------------
// <copyright file="ExtensionWithExtensionConfigurationSection.cs" company="Appccelerate">
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
    using System.Globalization;
    using System.Reflection;

    using Appccelerate.Bootstrapper.Configuration;

    using log4net;

    /// <summary>
    /// Extension which uses the automatic property conversion mechanism from <see cref="ExtensionConfigurationSection"/>.
    /// </summary>
    public class ExtensionWithExtensionConfigurationSection : ComplexExtensionBase
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets or sets the endpoint address.
        /// </summary>
        public string EndpointAddress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the server shall be started.
        /// </summary>
        public bool StartServer { get; set; }

        /// <inheritdoc />
        public override void Start()
        {
            Log.Info("ExtensionWithExtensionConfigurationSection is starting.");

            Log.InfoFormat(CultureInfo.InvariantCulture, " - EndpointAddress: {0} <<{1}>>", this.EndpointAddress, this.EndpointAddress.GetType().Name);
            Log.InfoFormat(CultureInfo.InvariantCulture, " - StartServer: {0}", this.StartServer);
        }

        /// <inheritdoc />
        public override string Describe()
        {
            return "Extensions which consumes a configuration section";
        }
    }
}