//-------------------------------------------------------------------------------
// <copyright file="IGotoInIfSyntax.cs" company="Appccelerate">
//   Copyright (c) 2008-2012
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
    /// Defines the Goto syntax inside an If.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public interface IGotoInIfSyntax<TState, TEvent> : IIfOrOtherwiseSyntax<TState, TEvent>
    {
        /// <summary>
        /// Defines the transition actions.
        /// </summary>
        /// <param name="actions">The actions to execute when the transition is taken.</param>
        /// <returns>Event syntax</returns>
        IGotoInIfSyntax<TState, TEvent> Execute(params Action[] actions);

        /// <summary>
        /// Defines the transition actions.
        /// </summary>
        /// <typeparam name="T">The type of the action argument.</typeparam>
        /// <param name="actions">The actions to execute when the transition is taken.</param>
        /// <returns>Event syntax</returns>
        IGotoInIfSyntax<TState, TEvent> Execute<T>(params Action<T>[] actions);
    }
}