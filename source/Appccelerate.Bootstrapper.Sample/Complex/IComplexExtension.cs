//-------------------------------------------------------------------------------
// <copyright file="IComplexExtension.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Sample.Complex
{
    using System.Collections.Generic;

    using Funq;

    /// <summary>
    /// Interface for all complex extensions
    /// </summary>
    public interface IComplexExtension : IExtension
    {
        /// <summary>
        /// Called when the system is started.
        /// </summary>
        void Start();

        /// <summary>
        /// Called before the Container is created. Allows to register dependencies during bootstrapping.
        /// </summary>
        /// <param name="funqlets">The funqlet collection.</param>
        void ContainerInitializing(ICollection<IFunqlet> funqlets);

        /// <summary>
        /// Called when the Container is created. Allows to resolve dependencies during bootstrapping.
        /// </summary>
        /// <param name="container">The container which allows resolves</param>
        void ContainerInitialized(Container container);

        /// <summary>
        /// Called when the system is ready and started.
        /// </summary>
        void Ready();

        /// <summary>
        /// Called wen the system is shutting down.
        /// </summary>
        void Shutdown();
    }
}