//-------------------------------------------------------------------------------
// <copyright file="IExecutableFactory.cs" company="Appccelerate">
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
    using System.Linq.Expressions;

    /// <summary>
    /// Factory which is responsible for creating executables.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public interface IExecutableFactory<TExtension>
        where TExtension : IExtension
    {
        /// <summary>
        /// Creates an executable which executes an action.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        /// <returns>An executable.</returns>
        IExecutable<TExtension> CreateExecutable(Expression<Action> action);

        /// <summary>
        /// Creates an executable which executes an initializer and passes the initialized context to the action on the specified extension.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="initializer">The initializer which creates the context.</param>
        /// <param name="action">The action to be executed which gains access to the created context.</param>
        /// <param name="contextInterceptor">The context interceptor.</param>
        /// <returns>
        /// An executable.
        /// </returns>
        IExecutable<TExtension> CreateExecutable<TContext>(Expression<Func<TContext>> initializer, Expression<Action<TExtension, TContext>> action, Action<IBehaviorAware<TExtension>, TContext> contextInterceptor);

        /// <summary>
        /// Creates an executable which executes an action on the specified extension.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>An executable.</returns>
        IExecutable<TExtension> CreateExecutable(Expression<Action<TExtension>> action);
    }
}