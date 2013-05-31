//-------------------------------------------------------------------------------
// <copyright file="IConfigurationSectionBehaviorFactory.cs" company="Appccelerate">
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
    /// <summary>
    /// Factory which creates the necessary dependencies for the <see cref="ConfigurationSectionBehavior"/>.
    /// </summary>
    public interface IConfigurationSectionBehaviorFactory
    {
        /// <summary>
        /// Creates the instance which knows the section name.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>The instance.</returns>
        IHaveConfigurationSectionName CreateHaveConfigurationSectionName(IExtension extension);

        /// <summary>
        /// Creates the instance which loads configuration sections.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>The instance.</returns>
        ILoadConfigurationSection CreateLoadConfigurationSection(IExtension extension);

        /// <summary>
        /// Creates the instance which consumes a configuration section.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>
        /// The instance.
        /// </returns>
        IConsumeConfigurationSection CreateConsumeConfigurationSection(IExtension extension);
    }
}