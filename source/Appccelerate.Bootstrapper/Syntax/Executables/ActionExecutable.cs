//-------------------------------------------------------------------------------
// <copyright file="ActionExecutable.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Syntax.Executables
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq.Expressions;

    using Appccelerate.Bootstrapper.Reporting;
    using Appccelerate.Formatters;

    /// <summary>
    /// The executable which executes an action.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public class ActionExecutable<TExtension> : IExecutable<TExtension>
        where TExtension : IExtension
    {
        private readonly Queue<IBehavior<TExtension>> behaviors;

        private readonly Action action;

        private readonly Expression<Action> actionExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionExecutable{TExtension}"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public ActionExecutable(Expression<Action> action)
        {
            Ensure.ArgumentNotNull(action, "action");

            this.behaviors = new Queue<IBehavior<TExtension>>();

            this.actionExpression = action;
            this.action = this.actionExpression.Compile();
        }

        /// <inheritdoc />
        public string Name
        {
            get
            {
                return this.GetType().FullNameToString();
            }
        }

        /// <inheritdoc />
        public void Execute(IEnumerable<TExtension> extensions, IExecutableContext executableContext)
        {
            Ensure.ArgumentNotNull(executableContext, "executableContext");

            foreach (IBehavior<TExtension> behavior in this.behaviors)
            {
                executableContext.CreateBehaviorContext(behavior);

                behavior.Behave(extensions);
            }

            this.action();
        }

        /// <inheritdoc />
        public void Add(IBehavior<TExtension> behavior)
        {
            this.behaviors.Enqueue(behavior);
        }

        /// <inheritdoc />
        public string Describe()
        {
            return string.Format(CultureInfo.InvariantCulture, "Executes \"{0}\" during bootstrapping.", this.actionExpression);
        }
    }
}