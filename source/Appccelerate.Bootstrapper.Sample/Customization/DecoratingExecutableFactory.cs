//-------------------------------------------------------------------------------
// <copyright file="DecoratingExecutableFactory.cs" company="Appccelerate">
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

namespace Appccelerate.Bootstrapper.Sample.Customization
{
    using System;
    using System.Linq.Expressions;

    using Appccelerate.Bootstrapper.Syntax;

    /// <summary>
    /// Decorates all created executable with the <see cref="DecoratingExecutable{TExtension}"/>
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public class DecoratingExecutableFactory<TExtension> : IExecutableFactory<TExtension>
        where TExtension : IExtension
    {
        private readonly IExecutableFactory<TExtension> decoratedExecutableFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecoratingExecutableFactory&lt;TExtension&gt;"/> class.
        /// </summary>
        public DecoratingExecutableFactory()
            : this(new ExecutableFactory<TExtension>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DecoratingExecutableFactory&lt;TExtension&gt;"/> class.
        /// </summary>
        /// <param name="decoratedExecutableFactory">The decorated executable factory.</param>
        public DecoratingExecutableFactory(IExecutableFactory<TExtension> decoratedExecutableFactory)
        {
            this.decoratedExecutableFactory = decoratedExecutableFactory;
        }

        /// <inheritdoc />
        /// <remarks>Decorates the created executable with <see cref="DecoratingExecutable{TExtension}"/></remarks>
        public IExecutable<TExtension> CreateExecutable(Expression<Action> action)
        {
            IExecutable<TExtension> decoratedExecutable = this.decoratedExecutableFactory.CreateExecutable(action);

            return new DecoratingExecutable<TExtension>(decoratedExecutable);
        }

        /// <inheritdoc />
        /// <remarks>Decorates the created executable with <see cref="DecoratingExecutable{TExtension}"/></remarks>
        public IExecutable<TExtension> CreateExecutable<TContext>(Expression<Func<TContext>> initializer, Expression<Action<TExtension, TContext>> action, Action<IBehaviorAware<TExtension>, TContext> contextInterceptor)
        {
            IExecutable<TExtension> decoratedExecutable = this.decoratedExecutableFactory.CreateExecutable(initializer, action, contextInterceptor);

            return new DecoratingExecutable<TExtension>(decoratedExecutable);
        }

        /// <inheritdoc />
        /// <remarks>Decorates the created executable with <see cref="DecoratingExecutable{TExtension}"/></remarks>
        public IExecutable<TExtension> CreateExecutable(Expression<Action<TExtension>> action)
        {
            IExecutable<TExtension> decoratedExecutable = this.decoratedExecutableFactory.CreateExecutable(action);

            return new DecoratingExecutable<TExtension>(decoratedExecutable);
        }
    }
}