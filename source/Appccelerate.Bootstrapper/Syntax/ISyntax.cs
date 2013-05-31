//-------------------------------------------------------------------------------
// <copyright file="ISyntax.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Generic syntax which operates on extensions.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public interface ISyntax<TExtension> : IEnumerable<IExecutable<TExtension>>
        where TExtension : IExtension
    {
    }

    /// <summary>
    /// Begin syntax which allows to define behavior which is executed at the beginning.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public interface IBeginSyntax<TExtension> : IExecuteAction<TExtension>,
                                                IExecuteActionOnExtension<TExtension>,
                                                IExecuteActionOnExtensionWithContext<TExtension>,
                                                IEndSyntax<TExtension>
        where TExtension : IExtension
    {
        /// <summary>
        /// Gets the begin of the syntax chain and attaches behavior to the begin
        /// </summary>
        IWithBehavior<TExtension> Begin { get; }
    }

    /// <summary>
    /// End syntax which allows to define behavior which is executed at the end.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public interface IEndSyntax<TExtension>
        where TExtension : IExtension
    {
        /// <summary>
        /// Gets the end of the syntax chain and attaches behavior to the end
        /// </summary>
        IEndWithBehavior<TExtension> End { get; }
    }

    /// <summary>
    /// Execute action syntax.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public interface IExecuteAction<TExtension> : ISyntax<TExtension>
        where TExtension : IExtension
    {
        /// <summary>
        /// Adds an execution action to the currently built syntax.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The current syntax builder.</returns>
        IWithBehavior<TExtension> Execute(Expression<Action> action);
    }

    /// <summary>
    /// Execute an action on an extension syntax.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public interface IExecuteActionOnExtension<TExtension> : ISyntax<TExtension>
        where TExtension : IExtension
    {
        /// <summary>
        /// Adds an execution action which operates on the extension to the
        /// currently built syntax.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The current syntax builder.</returns>
        IWithBehavior<TExtension> Execute(Expression<Action<TExtension>> action);
    }

    /// <summary>
    /// Execute an action on an extension with a context syntax.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public interface IExecuteActionOnExtensionWithContext<TExtension> : ISyntax<TExtension>
        where TExtension : IExtension
    {
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
        IWithBehaviorOnContext<TExtension, TContext> Execute<TContext>(
            Expression<Func<TContext>> initializer, Expression<Action<TExtension, TContext>> action);
    }

    /// <summary>
    /// Fluent definition syntax interface for behaviors.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public interface IWithBehavior<TExtension> : IExecuteAction<TExtension>,
                                                 IExecuteActionOnExtension<TExtension>,
                                                 IExecuteActionOnExtensionWithContext<TExtension>,
                                                 IEndSyntax<TExtension>
        where TExtension : IExtension
    {
        /// <summary>
        /// Attaches a behavior to the currently built executable.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns>
        /// The syntax.
        /// </returns>
        IWithBehavior<TExtension> With(IBehavior<TExtension> behavior);

        /// <summary>
        /// Attaches a lazy behavior to the currently built executable.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns>
        /// The syntax.
        /// </returns>
        IWithBehavior<TExtension> With(Expression<Func<IBehavior<TExtension>>> behavior);
    }

    /// <summary>
    /// Fluent definition syntax interface for behaviors which operate on contexts.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    public interface IWithBehaviorOnContext<TExtension, TContext> : IExecuteAction<TExtension>,
                                                                        IExecuteActionOnExtension<TExtension>,
                                                                        IExecuteActionOnExtensionWithContext<TExtension>,
                                                                        IEndSyntax<TExtension>
        where TExtension : IExtension
    {
        /// <summary>
        /// Attaches a behavior which has access to the context to the currently built executable.
        /// </summary>
        /// <param name="provider">The behavior provider.</param>
        /// <returns>The syntax.</returns>
        IWithBehaviorOnContext<TExtension, TContext> With(Expression<Func<TContext, IBehavior<TExtension>>> provider);
    }

    /// <summary>
    /// Terminal fluent definition syntax interface for behaviors.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    public interface IEndWithBehavior<TExtension>
        where TExtension : IExtension
    {
        /// <summary>
        /// Attaches a behavior to the currently built executable.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns>
        /// The syntax.
        /// </returns>
        IEndWithBehavior<TExtension> With(IBehavior<TExtension> behavior);

        /// <summary>
        /// Attaches a lazy behavior to the currently built executable.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns>
        /// The syntax.
        /// </returns>
        IEndWithBehavior<TExtension> With(Expression<Func<IBehavior<TExtension>>> behavior);
    }
}