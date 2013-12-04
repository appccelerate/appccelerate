//-------------------------------------------------------------------------------
// <copyright file="ReportingContext.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Reporting
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// The reporting context contains all necessary information about the bootstrapping process.
    /// </summary>
    public class ReportingContext : IReportingContext
    {
        private readonly Collection<IExtensionContext> extensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportingContext"/> class.
        /// </summary>
        public ReportingContext()
        {
            this.extensions = new Collection<IExtensionContext>();
        }

        /// <inheritdoc />
        public IExecutionContext Run { get; private set; }

        /// <inheritdoc />
        public IExecutionContext Shutdown { get; private set; }

        /// <inheritdoc />
        public IEnumerable<IExtensionContext> Extensions
        {
            get
            {
                return this.extensions;
            }
        }

        /// <inheritdoc />
        public IExecutionContext CreateRunExecutionContext(IDescribable describable)
        {
            this.Run = this.CreateExecutionContextCore(describable);
            return this.Run;
        }

        /// <inheritdoc />
        public IExecutionContext CreateShutdownExecutionContext(IDescribable describable)
        {
            this.Shutdown = this.CreateExecutionContextCore(describable);
            return this.Shutdown;
        }

        /// <inheritdoc />
        public IExtensionContext CreateExtensionContext(IDescribable describable)
        {
            var extensionInfo = this.CreateExtensionContextCore(describable);

            this.extensions.Add(extensionInfo);

            return extensionInfo;
        }

        /// <summary>
        /// Creates the extension context implementation.
        /// </summary>
        /// <param name="describable">The describable which is passed to the extension context.</param>
        /// <returns>A new instance of the extension context implementation.</returns>
        protected virtual IExtensionContext CreateExtensionContextCore(IDescribable describable)
        {
            return new ExtensionContext(describable);
        }

        /// <summary>
        /// Creates the execution context implementation.
        /// </summary>
        /// <param name="describable">The describable which is passed to the execution context.</param>
        /// <returns>A new instance of the execution context implementation.</returns>
        protected virtual IExecutionContext CreateExecutionContextCore(IDescribable describable)
        {
            return new ExecutionContext(describable);
        }
    }
}