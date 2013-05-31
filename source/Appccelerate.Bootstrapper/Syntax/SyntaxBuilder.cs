//-------------------------------------------------------------------------------
// <copyright file="SyntaxBuilder.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Syntax
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Appccelerate.Bootstrapper.Behavior;

    /// <summary>
    /// The syntax builder.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public class SyntaxBuilder<TExtension> : ISyntaxBuilderWithoutContext<TExtension>
        where TExtension : IExtension
    {
        private static readonly Action BeginWith = () => { };

        private static readonly Action EndWith = () => { };

        private readonly Queue<IExecutable<TExtension>> executables;

        private readonly IExecutableFactory<TExtension> executableFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxBuilder&lt;TExtension&gt;"/> class.
        /// </summary>
        /// <remarks>Uses the ExecutableFactory{TExtension}</remarks>
        public SyntaxBuilder()
            : this(new ExecutableFactory<TExtension>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxBuilder&lt;TExtension&gt;"/> class.
        /// </summary>
        /// <param name="executableFactory">The executable factory.</param>
        public SyntaxBuilder(IExecutableFactory<TExtension> executableFactory)
        {
            this.executableFactory = executableFactory;
            this.executables = new Queue<IExecutable<TExtension>>();
        }

        /// <inheritdoc />
        public IWithBehavior<TExtension> Begin
        {
            get
            {
                this.WithAction(() => BeginWith());

                return this;
            }
        }

        /// <inheritdoc />
        public IEndWithBehavior<TExtension> End
        {
            get
            {
                this.WithAction(() => EndWith());

                return this;
            }
        }

        /// <summary>
        /// Gets the currently built executable
        /// </summary>
        protected IExecutable<TExtension> BuiltExecutable { get; private set; }

        /// <summary>
        /// Adds an execution action to the currently built syntax.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The current syntax builder.</returns>
        public IWithBehavior<TExtension> Execute(Expression<Action> action)
        {
            return this.WithAction(action);
        }

        /// <summary>
        /// Adds an context initializer and an execution action which gets
        /// access to the context to the currently built syntax.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="initializer">The context initializer.</param>
        /// <param name="action">The action with access to the context.</param>
        /// <returns>
        /// The current syntax builder.
        /// </returns>
        public IWithBehaviorOnContext<TExtension, TContext> Execute<TContext>(Expression<Func<TContext>> initializer, Expression<Action<TExtension, TContext>> action)
        {
            return this.WithInitializerAndActionOnExtension(initializer, action);
        }

        /// <summary>
        /// Adds an execution action which operates on the extension to the
        /// currently built syntax.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The current syntax builder.</returns>
        public IWithBehavior<TExtension> Execute(Expression<Action<TExtension>> action)
        {
            return this.WithActionOnExtension(action);
        }

        /// <summary>
        /// Attaches a behavior to the currently built executable.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns>
        /// The syntax.
        /// </returns>
        public IWithBehavior<TExtension> With(IBehavior<TExtension> behavior)
        {
            this.BuiltExecutable.Add(behavior);

            return this;
        }

        /// <summary>
        /// Attaches a lazy behavior to the currently built executable.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns>
        /// The syntax.
        /// </returns>
        public IWithBehavior<TExtension> With(Expression<Func<IBehavior<TExtension>>> behavior)
        {
            this.BuiltExecutable.Add(new LazyBehavior<TExtension>(behavior));

            return this;
        }

        /// <summary>
        /// Attaches a lazy behavior to the currently built executable.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns>
        /// The syntax.
        /// </returns>
        IEndWithBehavior<TExtension> IEndWithBehavior<TExtension>.With(Expression<Func<IBehavior<TExtension>>> behavior)
        {
            this.BuiltExecutable.Add(new LazyBehavior<TExtension>(behavior));

            return this;
        }

        /// <summary>
        /// Attaches a behavior to the currently built executable.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns>
        /// The syntax.
        /// </returns>
        IEndWithBehavior<TExtension> IEndWithBehavior<TExtension>.With(IBehavior<TExtension> behavior)
        {
            this.BuiltExecutable.Add(behavior);

            return this;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<IExecutable<TExtension>> GetEnumerator()
        {
            return this.executables.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private IWithBehavior<TExtension> WithAction(Expression<Action> action)
        {
            var executable = this.executableFactory.CreateExecutable(action);

            this.executables.Enqueue(executable);
            this.BuiltExecutable = executable;

            return this;
        }

        private IWithBehavior<TExtension> WithActionOnExtension(Expression<Action<TExtension>> action)
        {
            var executable = this.executableFactory.CreateExecutable(action);

            this.executables.Enqueue(executable);
            this.BuiltExecutable = executable;

            return this;
        }

        private IWithBehaviorOnContext<TExtension, TContext> WithInitializerAndActionOnExtension<TContext>(Expression<Func<TContext>> initializer, Expression<Action<TExtension, TContext>> action)
        {
            var providerQueue = new Queue<Func<TContext, IBehavior<TExtension>>>();

            var executable = this.executableFactory.CreateExecutable(
                initializer,
                action,
                (behaviorAware, context) =>
                {
                    foreach (Func<TContext, IBehavior<TExtension>> provider in providerQueue)
                    {
                        behaviorAware.Add(provider(context));
                    }
                });

            this.executables.Enqueue(executable);
            this.BuiltExecutable = executable;

            return new SyntaxBuilderWithContext<TExtension, TContext>(this, providerQueue);
        }
    }
}