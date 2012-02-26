//-------------------------------------------------------------------------------
// <copyright file="TransitionContext.cs" company="Appccelerate">
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

namespace Appccelerate.StateMachine.Machine.Contexts
{
    using System;
    using System.Diagnostics;

    using Appccelerate.StateMachine.Machine.States;

    /// <summary>
    /// Provides context information during a transition.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    [DebuggerDisplay("State = {state} Event = {eventId} EventArguments = {eventArguments}")]
    public class TransitionContext<TState, TEvent> : StateContext<TState, TEvent>, ITransitionContext<TState, TEvent>
        where TState : IComparable
        where TEvent : IComparable
    {
        /// <summary>
        /// The event that causes the transition.
        /// </summary>
        private readonly TEvent eventId;

        /// <summary>
        /// The event argument.
        /// </summary>
        private readonly object eventArgument;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionContext{TState,TEvent}"/> class.
        /// </summary>
        /// <param name="state">The source state.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="eventArgument">The event argument.</param>
        /// <param name="notifier">The notifier to fire events about transition events and exceptions.</param>
        public TransitionContext(IState<TState, TEvent> state, TEvent eventId, object eventArgument, INotifier<TState, TEvent> notifier)
            : base(state, notifier)
        {
            this.eventId = eventId;
            this.eventArgument = eventArgument;
        }

        /// <summary>
        /// Gets the event id.
        /// </summary>
        /// <value>The event id.</value>
        public TEvent EventId
        {
            get { return this.eventId; }
        }

        /// <summary>
        /// Gets the event argument.
        /// </summary>
        /// <value>The event argument.</value>
        public object EventArgument
        {
            get { return this.eventArgument; }
        }

        /// <summary>
        /// Called when an exception should be notified.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public override void OnExceptionThrown(Exception exception)
        {
            this.AddException(exception);
            this.Notifier.OnExceptionThrown(this, exception);
        }

        /// <summary>
        /// Called when a transition beginning should be notified.
        /// </summary>
        public void OnTransitionBegin()
        {
            this.Notifier.OnTransitionBegin(this);
        }
    }
}