//-------------------------------------------------------------------------------
// <copyright file="ExtensionConfigurationSectionHelper.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Helper class which builds configuration sections.
    /// </summary>
    public static class ExtensionConfigurationSectionHelper
    {
        /// <summary>
        /// Creates a <see cref="ExtensionConfigurationSection"/> according to the provided settings dictionary.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>A newly created <see cref="ExtensionConfigurationSection"/>.</returns>
        public static ExtensionConfigurationSection CreateSection(IEnumerable<KeyValuePair<string, string>> settings)
        {
            return CreateSection(settings.ToArray());
        }

        /// <summary>
        /// Creates a <see cref="ExtensionConfigurationSection"/> according to the provided settings dictionary.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>A newly created <see cref="ExtensionConfigurationSection"/>.</returns>
        public static ExtensionConfigurationSection CreateSection(params KeyValuePair<string, string>[] settings)
        {
            var section = new ExtensionConfigurationSection();
            var elementCollection = new ExtensionSettingsElementCollection();

            foreach (ExtensionSettingsElement element in settings.Select(settingPair => new ExtensionSettingsElement { Key = settingPair.Key, Value = settingPair.Value }))
            {
                elementCollection["Configuration"] = element;
            }

            section.Configuration = elementCollection;

            return section;
        }
    }
}