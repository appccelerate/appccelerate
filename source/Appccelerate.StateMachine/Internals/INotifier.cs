//-------------------------------------------------------------------------------
// <copyright file="INotifier.cs" company="Appccelerate">
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

namespace Appccelerate.StateMachine.Internals
{
    using System;

    /// <summary>
    /// Provides functionalities to notify events.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public interface INotifier<TState, TEvent>
        where TState : IComparable
        where TEvent : IComparable
    {
        /// <summary>
        /// Called when an exception was thrown.
        /// </summary>
        /// <param name="stateContext">The context.</param>
        /// <param name="exception">The exception.</param>
        void OnExceptionThrown(IStateContext<TState, TEvent> stateContext, Exception exception);

        /// <summary>
        /// Called when an exception was thrown in a transition.
        /// </summary>
        /// <param name="transitionContext">The transition context.</param>
        /// <param name="exception">The exception.</param>
        void OnExceptionThrown(ITransitionContext<TState, TEvent> transitionContext, Exception exception);

        /// <summary>
        /// Called before a transition is executed.
        /// </summary>
        /// <param name="transitionContext">The context.</param>
        void OnTransitionBegin(ITransitionContext<TState, TEvent> transitionContext);
    }
}