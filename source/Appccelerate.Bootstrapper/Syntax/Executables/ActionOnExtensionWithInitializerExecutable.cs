//-------------------------------------------------------------------------------
// <copyright file="ActionOnExtensionWithInitializerExecutable.cs" company="Appccelerate">
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
    /// Executable which executes an initializer and passes the initialized
    /// context to the action which operates on the extension.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public class ActionOnExtensionWithInitializerExecutable<TContext, TExtension> : IExecutable<TExtension>
        where TExtension : IExtension
    {
        private readonly Queue<IBehavior<TExtension>> behaviors;

        private readonly Expression<Func<TContext>> initializerExpression;
        private readonly Func<TContext> initializer;

        private readonly Expression<Action<TExtension, TContext>> actionExpression;
        private readonly Action<TExtension, TContext> action;

        private readonly Action<IBehaviorAware<TExtension>, TContext> contextInterceptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionOnExtensionWithInitializerExecutable{TContext, TExtension}"/> class.
        /// </summary>
        /// <param name="initializer">The initializer.</param>
        /// <param name="action">The action.</param>
        /// <param name="contextInterceptor">The behavior aware.</param>
        public ActionOnExtensionWithInitializerExecutable(Expression<Func<TContext>> initializer, Expression<Action<TExtension, TContext>> action, Action<IBehaviorAware<TExtension>, TContext> contextInterceptor)
        {
            Ensure.ArgumentNotNull(action, "action");

            this.contextInterceptor = contextInterceptor;
            this.behaviors = new Queue<IBehavior<TExtension>>();

            this.actionExpression = action;
            this.initializerExpression = initializer;

            this.action = this.actionExpression.Compile();
            this.initializer = this.initializerExpression.Compile();
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
            Ensure.ArgumentNotNull(extensions, "extensions");
            Ensure.ArgumentNotNull(executableContext, "executableContext");

            TContext context = this.initializer();

            this.contextInterceptor(this, context);

            foreach (IBehavior<TExtension> behavior in this.behaviors)
            {
                executableContext.CreateBehaviorContext(behavior);

                behavior.Behave(extensions);
            }

            foreach (TExtension extension in extensions)
            {
                this.action(extension, context);
            }
        }

        /// <summary>
        /// Adds the specified behavior.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        public void Add(IBehavior<TExtension> behavior)
        {
            this.behaviors.Enqueue(behavior);
        }

        /// <inheritdoc />
        public string Describe()
        {
            return string.Format(CultureInfo.InvariantCulture, "Initializes the context once with \"{0}\" and executes \"{1}\" on each extension during bootstrapping.", this.initializerExpression, this.actionExpression);
        }
    }
}