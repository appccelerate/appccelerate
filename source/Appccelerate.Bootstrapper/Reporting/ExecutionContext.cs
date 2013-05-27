//-------------------------------------------------------------------------------
// <copyright file="ExecutionContext.cs" company="Appccelerate">
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
    /// Execution context implementation which is also responsible for creating executable contexts.
    /// The IDescribable is released right after creation.
    /// </summary>
    public class ExecutionContext : IExecutionContext
    {
        private readonly ICollection<IExecutableContext> executables;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionContext"/> class.
        /// </summary>
        /// <param name="describable">The describable.</param>
        public ExecutionContext(IDescribable describable)
        {
            Ensure.ArgumentNotNull(describable, "describable");

            this.executables = new Collection<IExecutableContext>();

            this.Name = describable.Name;
            this.Description = describable.Describe();
        }

        /// <inheritdoc />
        public string Name { get; private set; }

        /// <inheritdoc />
        public string Description { get; private set; }

        /// <inheritdoc />
        public IEnumerable<IExecutableContext> Executables
        {
            get
            {
                return this.executables;
            }
        }

        /// <inheritdoc />
        public IExecutableContext CreateExecutableContext(IDescribable describable)
        {
            var executableInfo = this.CreateExecutableContextCore(describable);

            this.executables.Add(executableInfo);

            return executableInfo;
        }

        /// <summary>
        /// Creates the executable context implementation.
        /// </summary>
        /// <param name="describable">The describable which is passed to the executable context.</param>
        /// <returns>A new instance of the executable context implementation.</returns>
        protected virtual IExecutableContext CreateExecutableContextCore(IDescribable describable)
        {
            return new ExecutableContext(describable);
        }
    }
}