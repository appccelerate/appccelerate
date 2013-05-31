//-------------------------------------------------------------------------------
// <copyright file="ExtensionHost.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Extension
{
    using System.Collections.Generic;

    /// <summary>
    /// Generic extension host.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public class ExtensionHost<TExtension> : IExtensionHost<TExtension>
        where TExtension : IExtension
    {
        private readonly Queue<TExtension> extensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionHost{TExtension}"/> class.
        /// </summary>
        public ExtensionHost()
        {
            this.extensions = new Queue<TExtension>();
        }

        /// <summary>
        /// Gets the extensions.
        /// </summary>
        public IEnumerable<TExtension> Extensions
        {
            get
            {
                return this.extensions;
            }
        }

        /// <summary>
        /// Adds the extension to the bootstrapping mechanism. The extensions are executed in the order which they were
        /// added.
        /// </summary>
        /// <param name="extension">The extension to be added.</param>
        public void AddExtension(TExtension extension)
        {
            this.extensions.Enqueue(extension);
        }
    }
}