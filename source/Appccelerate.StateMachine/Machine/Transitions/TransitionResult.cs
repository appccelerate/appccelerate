//-------------------------------------------------------------------------------
// <copyright file="TransitionResult.cs" company="Appccelerate">
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

namespace Appccelerate.StateMachine.Machine.Transitions
{
    using System;
    using System.Collections.Generic;

    using Appccelerate.StateMachine.Machine.States;

    /// <summary>
    /// Represents the result of a transition.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    internal class TransitionResult<TState, TEvent>
        : ITransitionResult<TState, TEvent>
        where TState : IComparable
        where TEvent : IComparable
    {
        /// <summary>
        /// This value represents that no transition was fired.
        /// </summary>
        public static readonly ITransitionResult<TState, TEvent> NotFired = new TransitionResult<TState, TEvent>(false, null, null);

        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionResult{TState,TEvent}"/> class.
        /// </summary>
        /// <param name="fired">if set to <c>true</c> [fired].</param>
        /// <param name="newState">The new state.</param>
        /// <param name="exceptions">The exceptions.</param>
        public TransitionResult(bool fired, IState<TState, TEvent> newState, ICollection<Exception> exceptions)
        {
            this.Fired = fired;
            this.NewState = newState;
            this.Exceptions = exceptions;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ITransitionResult{TState, TEvent}"/> is fired.
        /// </summary>
        /// <value><c>true</c> if fired; otherwise, <c>false</c>.</value>
        public bool Fired { get; private set; }

        /// <summary>
        /// Gets the new state the state machine is in.
        /// </summary>
        /// <value>The new state.</value>
        public IState<TState, TEvent> NewState { get; private set; }

        /// <summary>
        /// Gets all exceptions that occurred during executing the transition.
        /// </summary>
        /// <value>The exceptions.</value>
        public ICollection<Exception> Exceptions { get; private set; }
    }
}