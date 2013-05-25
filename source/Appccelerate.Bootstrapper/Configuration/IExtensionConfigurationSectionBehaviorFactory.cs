//-------------------------------------------------------------------------------
// <copyright file="IExtensionConfigurationSectionBehaviorFactory.cs" company="Appccelerate">
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
    /// Factory which creates the necessary dependencies for the <see cref="ExtensionConfigurationSectionBehavior"/>.
    /// </summary>
    public interface IExtensionConfigurationSectionBehaviorFactory
    {
        /// <summary>
        /// Creates the extension property reflector.
        /// </summary>
        /// <returns>The instance.</returns>
        IReflectExtensionProperties CreateReflectExtensionProperties();

        /// <summary>
        /// Creates the extension property assigner.
        /// </summary>
        /// <returns>The instance.</returns>
        IAssignExtensionProperties CreateAssignExtensionProperties();

        /// <summary>
        /// Creates the consume configuration instance.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>The instance.</returns>
        IConsumeConfiguration CreateConsumeConfiguration(IExtension extension);

        /// <summary>
        /// Creates the instance which knows the section name.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>The instance.</returns>
        IHaveConfigurationSectionName CreateHaveConfigurationSectionName(IExtension extension);

        /// <summary>
        /// Creates the instance which loads configuration sections.
        /// </summary>
        /// <param name="extension">The extensions.</param>
        /// <returns>The instance.</returns>
        ILoadConfigurationSection CreateLoadConfigurationSection(IExtension extension);

        /// <summary>
        /// Creates the instance which has conversion callbacks.
        /// </summary>
        /// <param name="extension">The extensions.</param>
        /// <returns>The instance.</returns>
        IHaveConversionCallbacks CreateHaveConversionCallbacks(IExtension extension);

        /// <summary>
        /// Creates the instance which has the default conversion callback.
        /// </summary>
        /// <param name="extension">The extensions.</param>
        /// <returns>The instance.</returns>
        IHaveDefaultConversionCallback CreateHaveDefaultConversionCallback(IExtension extension);
    }
}