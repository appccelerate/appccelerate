//-------------------------------------------------------------------------------
// <copyright file="ContextEventArgs.cs" company="Appccelerate">
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

namespace Appccelerate.StateMachine.Machine.Events
{
    using System;

    using Appccelerate.StateMachine.Machine.States;

    /// <summary>
    /// Event arguments holding context information.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public class ContextEventArgs<TState, TEvent>
        : EventArgs
        where TState : IComparable
        where TEvent : IComparable
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly IStateContext<TState, TEvent> stateContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextEventArgs{TState, TEvent}"/> class.
        /// </summary>
        /// <param name="stateContext">The event context.</param>
        protected ContextEventArgs(IStateContext<TState, TEvent> stateContext)
        {
            this.stateContext = stateContext;
        }

        /// <summary>
        /// Gets the event context.
        /// </summary>
        /// <value>The event context.</value>
        protected IStateContext<TState, TEvent> StateContext
        {
            get { return this.stateContext; }
        }
    }
}