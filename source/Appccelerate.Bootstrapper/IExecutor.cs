//-------------------------------------------------------------------------------
// <copyright file="IExecutor.cs" company="Appccelerate">
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
    using System.Collections.Generic;

    using Appccelerate.Bootstrapper.Reporting;
    using Appccelerate.Bootstrapper.Syntax;

    /// <summary>
    /// The executor is responsible for executing a given syntax on the extensions.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public interface IExecutor<TExtension> : IDescribable
        where TExtension : IExtension
    {
        /// <summary>
        /// Executes the specified syntax on the extensions.
        /// </summary>
        /// <param name="syntax">The syntax.</param>
        /// <param name="extensions">The extensions.</param>
        /// <param name="executionContext">The execution context which is used for reporting</param>
        void Execute(ISyntax<TExtension> syntax, IEnumerable<TExtension> extensions, IExecutionContext executionContext);
    }
}