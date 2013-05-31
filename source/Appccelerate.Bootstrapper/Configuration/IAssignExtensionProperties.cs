//-------------------------------------------------------------------------------
// <copyright file="IAssignExtensionProperties.cs" company="Appccelerate">
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
    /// Identifies the implementing class as a property assigner for extensions.
    /// </summary>
    public interface IAssignExtensionProperties
    {
        /// <summary>
        /// Automatically assigns the reflector property values on the provided extension properties if a match occurs..
        /// </summary>
        /// <param name="reflector">The reflector.</param>
        /// <param name="extension">The extension.</param>
        /// <param name="consumer">The configuration consumer.</param>
        /// <param name="conversionCallbacksProvider">The conversion callbacks provider.</param>
        /// <param name="defaultConversionCallbackProvider">The default conversion callback provider.</param>
        void Assign(IReflectExtensionProperties reflector, IExtension extension, IConsumeConfiguration consumer, IHaveConversionCallbacks conversionCallbacksProvider, IHaveDefaultConversionCallback defaultConversionCallbackProvider);
    }
}