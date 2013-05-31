//-------------------------------------------------------------------------------
// <copyright file="IExtensionPoint.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper
{
    /// <summary>
    /// Extension point for bootstrapper.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public interface IExtensionPoint<TExtension>
        where TExtension : IExtension
    {
        /// <summary>
        /// Adds the extension to the bootstrapping mechanism. The extensions are executed in the order which they were
        /// added.
        /// </summary>
        /// <param name="extension">The extension to be added.</param>
        void AddExtension(TExtension extension);
    }
}