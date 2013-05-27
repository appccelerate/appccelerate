//-------------------------------------------------------------------------------
// <copyright file="SyntaxBuilderWithContext.cs" company="Appccelerate">
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

    /// <summary>
    /// Special syntax builder which is used for chaining when a context initializer is used.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    public class SyntaxBuilderWithContext<TExtension, TContext> : ISyntaxBuilderWithContext<TExtension, TContext>
        where TExtension : IExtension
    {
        private readonly Queue<Func<TContext, IBehavior<TExtension>>> behaviorProviders;

        private readonly ISyntaxBuilderWithoutContext<TExtension> syntaxBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxBuilderWithContext&lt;TExtension, TContext&gt;"/> class.
        /// </summary>
        /// <param name="syntaxBuilder">The syntax builder.</param>
        /// <param name="behaviorProviders">The behavior providers.</param>
        public SyntaxBuilderWithContext(ISyntaxBuilderWithoutContext<TExtension> syntaxBuilder, Queue<Func<TContext, IBehavior<TExtension>>> behaviorProviders)
        {
            this.syntaxBuilder = syntaxBuilder;
            this.behaviorProviders = behaviorProviders;
        }

        /// <inheritdoc />
        public IEndWithBehavior<TExtension> End
        {
            get
            {
                return this.syntaxBuilder.End;
            }
        }

        /// <inheritdoc />
        public IWithBehavior<TExtension> Execute(Expression<Action> action)
        {
            return this.syntaxBuilder.Execute(action);
        }

        /// <inheritdoc />
        public IWithBehavior<TExtension> Execute(Expression<Action<TExtension>> action)
        {
            return this.syntaxBuilder.Execute(action);
        }

        /// <inheritdoc />
        public IWithBehaviorOnContext<TExtension, TChainedContext> Execute<TChainedContext>(Expression<Func<TChainedContext>> initializer, Expression<Action<TExtension, TChainedContext>> action)
        {
            return this.syntaxBuilder.Execute(initializer, action);
        }

        /// <inheritdoc />
        public IEndWithBehavior<TExtension> With(IBehavior<TExtension> behavior)
        {
            return ((IEndWithBehavior<TExtension>)this.syntaxBuilder).With(behavior);
        }

        /// <inheritdoc />
        public IEndWithBehavior<TExtension> With(Expression<Func<IBehavior<TExtension>>> behavior)
        {
            return ((IEndWithBehavior<TExtension>)this.syntaxBuilder).With(behavior);
        }

        /// <inheritdoc />
        public IWithBehaviorOnContext<TExtension, TContext> With(Expression<Func<TContext, IBehavior<TExtension>>> provider)
        {
            Ensure.ArgumentNotNull(provider, "provider");

            this.behaviorProviders.Enqueue(provider.Compile());

            return this;
        }

        /// <inheritdoc />
        public IEnumerator<IExecutable<TExtension>> GetEnumerator()
        {
            return this.syntaxBuilder.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}