//-------------------------------------------------------------------------------
// <copyright file="SynchronousReverseExecutor.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Execution
{
    using System.Collections.Generic;

    using Appccelerate.Bootstrapper.Reporting;
    using Appccelerate.Bootstrapper.Syntax;
    using Appccelerate.Formatters;

    /// <summary>
    /// Synchronously executes the provided syntax on the extensions by reversing the order the the extensions.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extensions</typeparam>
    public class SynchronousReverseExecutor<TExtension> : IExecutor<TExtension>
        where TExtension : IExtension
    {
        /// <inheritdoc />
        public string Name
        {
            get
            {
                return this.GetType().FullNameToString();
            }
        }

        /// <inheritdoc />
        public void Execute(ISyntax<TExtension> syntax, IEnumerable<TExtension> extensions, IExecutionContext executionContext)
        {
            Ensure.ArgumentNotNull(syntax, "syntax");
            Ensure.ArgumentNotNull(executionContext, "executionContext");

            var reversedExtensions = new Stack<TExtension>(extensions);

            foreach (IExecutable<TExtension> executable in syntax)
            {
                IExecutableContext executableContext = executionContext.CreateExecutableContext(executable);

                executable.Execute(reversedExtensions, executableContext);
            }
        }

        /// <inheritdoc />
        public string Describe()
        {
            return "Runs all executables synchronously on the extensions in the reverse order which they were added.";
        }
    }
}