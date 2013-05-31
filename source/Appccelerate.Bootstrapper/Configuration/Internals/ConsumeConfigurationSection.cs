//-------------------------------------------------------------------------------
// <copyright file="ConsumeConfigurationSection.cs" company="Appccelerate">
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
    using System.Configuration;

    /// <summary>
    /// Default IConsumeConfigurationSection
    /// </summary>
    public class ConsumeConfigurationSection : IConsumeConfigurationSection
    {
        private readonly IConsumeConfigurationSection consumer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumeConfigurationSection"/> class.
        /// </summary>
        /// <param name="extension">The extension.</param>
        public ConsumeConfigurationSection(IExtension extension)
        {
            var consumerCandidate = extension as IConsumeConfigurationSection;
            this.consumer = consumerCandidate ?? new NullConsumer();
        }

        /// <summary>
        /// The configuration section which is consumed.
        /// </summary>
        /// <param name="section">The configuration section.</param>
        public void Apply(ConfigurationSection section)
        {
            this.consumer.Apply(section);
        }

        /// <summary>
        /// Consumer which does nothing with the applied configuration section.
        /// </summary>
        private class NullConsumer : IConsumeConfigurationSection
        {
            /// <inheritdoc />
            public void Apply(ConfigurationSection section)
            {
            }
        }
    }
}