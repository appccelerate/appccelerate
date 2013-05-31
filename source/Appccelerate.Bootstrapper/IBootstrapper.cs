//-------------------------------------------------------------------------------
// <copyright file="IBootstrapper.cs" company="Appccelerate">
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
    using System;

    /// <summary>
    /// Interface for bootstrapper sequence implementations.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public interface IBootstrapper<TExtension> : IExtensionPoint<TExtension>, IDisposable
        where TExtension : IExtension
    {
        /// <summary>
        /// Initializes the bootstrapper with the strategy.
        /// </summary>
        /// <param name="strategy">The strategy.</param>
        void Initialize(IStrategy<TExtension> strategy);

        /// <summary>
        /// Runs the bootstrapper.
        /// </summary>
        /// <exception cref="BootstrapperException">When an exception occurred during bootstrapping.</exception>
        void Run();

        /// <summary>
        /// Shutdowns the bootstrapper.
        /// </summary>
        /// <exception cref="BootstrapperException">When an exception occurred during bootstrapping.</exception>
        void Shutdown();
    }
}