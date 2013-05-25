//-------------------------------------------------------------------------------
// <copyright file="ExecutableContext.cs" company="Appccelerate">
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
    /// Executable context implementation which is also responsible for creating behavior contexts.
    /// The IDescribable is released right after creation.
    /// </summary>
    public class ExecutableContext : IExecutableContext
    {
        private readonly ICollection<IBehaviorContext> behaviors;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutableContext"/> class.
        /// </summary>
        /// <param name="describable">The describable.</param>
        public ExecutableContext(IDescribable describable)
        {
            Ensure.ArgumentNotNull(describable, "describable");

            this.behaviors = new Collection<IBehaviorContext>();

            this.Name = describable.Name;
            this.Description = describable.Describe();
        }

        /// <inheritdoc />
        public string Name { get; private set; }

        /// <inheritdoc />
        public string Description { get; private set; }

        /// <inheritdoc />
        public IEnumerable<IBehaviorContext> Behaviors
        {
            get
            {
                return this.behaviors;
            }
        }

        /// <inheritdoc />
        public IBehaviorContext CreateBehaviorContext(IDescribable describable)
        {
            var behaviorInfo = this.CreateBehaviorContextCore(describable);

            this.behaviors.Add(behaviorInfo);

            return behaviorInfo;
        }

        /// <summary>
        /// Creates the behavior context implementation.
        /// </summary>
        /// <param name="describable">The describable which is passed to the behavior context.</param>
        /// <returns>A new instance of the behavior context implementation.</returns>
        protected virtual IBehaviorContext CreateBehaviorContextCore(IDescribable describable)
        {
            return new BehaviorContext(describable);
        }
    }
}