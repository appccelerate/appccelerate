//-------------------------------------------------------------------------------
// <copyright file="ExtensionWithExtensionConfigurationSectionWithConversionAndCustomizedLoading.cs" company="Appccelerate">
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
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net;
    using System.Reflection;

    using Appccelerate.Bootstrapper.Configuration;
    using Appccelerate.Bootstrapper.Configuration.Internals;

    using log4net;

    /// <summary>
    /// Extension which uses the configuration entries from another
    /// configuration section and provides callbacks for value conversion.
    /// </summary>
    public class ExtensionWithExtensionConfigurationSectionWithConversionAndCustomizedLoading : ComplexExtensionBase,
        IHaveConfigurationSectionName, IHaveConversionCallbacks
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Lazy<IDictionary<string, IConversionCallback>> conversion;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionWithExtensionConfigurationSectionWithConversionAndCustomizedLoading"/> class.
        /// </summary>
        public ExtensionWithExtensionConfigurationSectionWithConversionAndCustomizedLoading()
        {
            this.conversion =
                new Lazy<IDictionary<string, IConversionCallback>>(
                    () =>
                    new Dictionary<string, IConversionCallback>
                        {
                            { "EndpointAddress", new FuncConversionCallback((input, prop) => IPAddress.Parse(input)) },
                        });
        }

        /// <summary>
        /// Gets the section name.
        /// </summary>
        public string SectionName
        {
            get
            {
                // We want here the same section as ExtensionWithExtensionConfigurationSection.
                return typeof(ExtensionWithExtensionConfigurationSection).Name;
            }
        }

        /// <summary>
        /// Gets or sets the endpoint address.
        /// </summary>
        public IPAddress EndpointAddress { get; set; }

        /// <summary>
        /// Gets the conversion callbacks
        /// </summary>
        public IDictionary<string, IConversionCallback> ConversionCallbacks
        {
            get
            {
                return this.conversion.Value;
            }
        }

        /// <inheritdoc />
        public override void Start()
        {
            base.Start();

            Log.Info("ExtensionWithExtensionConfigurationSectionWithConversionAndCustomizedLoading is starting.");

            Log.InfoFormat(CultureInfo.InvariantCulture, " - EndpointAddress: {0} <<{1}>>", this.EndpointAddress, this.EndpointAddress.GetType().Name);
        }

        /// <inheritdoc />
        public override string Describe()
        {
            return
                "Extension which overloads the default configuration section loading mechanism and provides callback functions to turn the section values into properties";
        }
    }
}