//-------------------------------------------------------------------------------
// <copyright file="IStrategy.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper
{
    using System;

    using Appccelerate.Bootstrapper.Reporting;
    using Appccelerate.Bootstrapper.Syntax;

    /// <summary>
    /// Interface for strategies.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public interface IStrategy<TExtension> : IDisposable
        where TExtension : IExtension
    {
        /// <summary>
        /// Creates an executor which is responsible for executing the run syntax.
        /// </summary>
        /// <returns>The run executor.</returns>
        IExecutor<TExtension> CreateRunExecutor();

        /// <summary>
        /// Creates an executor which is responsible for executing the shutdown syntax.
        /// </summary>
        /// <returns>The shutdown executor.</returns>
        IExecutor<TExtension> CreateShutdownExecutor();

        /// <summary>
        /// Creates an extension resolver which can resolve extension and add them onto the provided extension point
        /// </summary>
        /// <returns>The extension resolver.</returns>
        IExtensionResolver<TExtension> CreateExtensionResolver();

        /// <summary>
        /// Creates a context which is used to track information about the bootstrapping process.
        /// </summary>
        /// <returns>The reporting context.</returns>
        IReportingContext CreateReportingContext();

        /// <summary>
        /// Builds the run syntax.
        /// </summary>
        /// <returns>The run syntax.</returns>
        ISyntax<TExtension> BuildRunSyntax();

        /// <summary>
        /// Builds the shutdown syntax.
        /// </summary>
        /// <returns>The shutdown syntax.</returns>
        ISyntax<TExtension> BuildShutdownSyntax();
    }
}