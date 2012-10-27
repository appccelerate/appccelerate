//-------------------------------------------------------------------------------
// <copyright file="IModuleExtension.cs" company="Appccelerate">
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
    /// <summary>
    /// If a module needs to add event handlers to the 
    /// events of the module controller, it has to implement
    /// this interface, so that the module controller can 
    /// call Attach and Detach.
    /// </summary>
    public interface IModuleExtension
    {
        /// <summary>
        /// Sets the controller this extension belongs to.
        /// </summary>
        /// <value>The controller.</value>
        IModuleController ModuleController { set; }

        /// <summary>
        /// Called by the module controller to allow the extension
        /// to register itself for events.
        /// </summary>
        void Attach();

        /// <summary>
        /// Called by the module controller to allow the extension 
        /// to remove its event handlers.
        /// </summary>
        void Detach();
    }
}
