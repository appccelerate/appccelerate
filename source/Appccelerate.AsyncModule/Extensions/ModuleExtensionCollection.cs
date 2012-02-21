//-------------------------------------------------------------------------------
// <copyright file="ModuleExtensionCollection.cs" company="Appccelerate">
//   Copyright (c) 2008-2012
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

namespace Appccelerate.AsyncModule.Extensions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This collection contains module extensions.
    /// </summary>
    public class ModuleExtensionCollection
    {
        private readonly Dictionary<Type, object> dictionary = new Dictionary<Type, object>();

        /// <summary>
        /// Adds a new extension.
        /// </summary>
        /// <typeparam name="TExtension">
        /// With this type the extension is retrieved by GetExtension().
        /// </typeparam>
        /// <param name="extensionInstance">
        /// The actual extension instance.
        /// </param>
        public void Add<TExtension>(object extensionInstance)
        {
            if (this.dictionary.ContainsKey(typeof(TExtension)))
            {
                this.RemoveAndDetach(typeof(TExtension));
            }

            this.AddAndAttach(typeof(TExtension), extensionInstance);
        }

        /// <summary>
        /// Gets the extension from the module which was registered 
        /// with the type TExtensionType.
        /// </summary>
        /// <typeparam name="TExtension">
        /// The type identifying the extension to get.
        /// </typeparam>
        /// <returns>
        /// See above.
        /// </returns>
        public TExtension Get<TExtension>()
        {
            foreach (Type extensionType in this.dictionary.Keys)
            {
                if (typeof(TExtension).IsAssignableFrom(extensionType))
                {
                    return (TExtension)this.dictionary[extensionType];
                }
            }

            return default(TExtension);
        }

        /// <summary>
        /// Removes and detaches an extension.
        /// </summary>
        /// <param name="extensionType">
        /// The type of the extension to remove.
        /// </param>
        private void RemoveAndDetach(Type extensionType)
        {
            object extensionInstance = this.dictionary[extensionType];
            
            // Detach before removing the extension
            if (extensionInstance is IModuleExtension)
            {
                ((IModuleExtension)extensionInstance).Detach();
            }
            
            this.dictionary.Remove(extensionType);
        }

        /// <summary>
        /// Add the extension to the dictionary. If the extension is of the 
        /// type IModuleExtension, the Attach method of the extension is called, 
        /// so that the extension can add event handlers to the extension points.
        /// </summary>
        /// <param name="extensionType">
        /// Extension type to add.
        /// </param>
        /// <param name="extensionInstance">
        /// The actual extension.
        /// </param>
        private void AddAndAttach(Type extensionType, object extensionInstance)
        {
            this.dictionary.Add(extensionType, extensionInstance);
            if (extensionInstance is IModuleExtension)
            {
                ((IModuleExtension)extensionInstance).Attach();
            }
        }
    }
}
