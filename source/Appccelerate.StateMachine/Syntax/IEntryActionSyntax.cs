//-------------------------------------------------------------------------------
// <copyright file="IEntryActionSyntax.cs" company="Appccelerate">
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

namespace Appccelerate.StateMachine.Syntax
{
    using System;

    /// <summary>
    /// Defines the entry action syntax.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public interface IEntryActionSyntax<TState, TEvent> : IExitActionSyntax<TState, TEvent>
    {
        /// <summary>
        /// Defines an entry action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>Exit action syntax.</returns>
        IEntryActionSyntax<TState, TEvent> ExecuteOnEntry(Action action);

        /// <summary>
        /// Defines an entry action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>Exit action syntax.</returns>
        /// <typeparam name="T">Type of the event argument passed to the action.</typeparam>
        IEntryActionSyntax<TState, TEvent> ExecuteOnEntry<T>(Action<T> action);

        /// <summary>
        /// Defines an entry action.
        /// </summary>
        /// <typeparam name="T">Type of the parameter of the entry action method.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="parameter">The parameter that will be passed to the entry action.</param>
        /// <returns>Exit action syntax.</returns>
        IEntryActionSyntax<TState, TEvent> ExecuteOnEntryParametrized<T>(Action<T> action, T parameter);
    }
}