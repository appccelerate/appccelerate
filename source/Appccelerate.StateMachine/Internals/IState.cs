//-------------------------------------------------------------------------------
// <copyright file="IState.cs" company="Appccelerate">
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
    using System.Collections.Generic;

    /// <summary>
    /// Represents a state of the state machine.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public interface IState<TState, TEvent>
        where TState : IComparable
        where TEvent : IComparable
    {
        /// <summary>
        /// Gets the id of this state.
        /// </summary>
        /// <value>The id of this state.</value>
        TState Id { get; }

        /// <summary>
        /// Gets or sets the initial sub-state. Null if this state has no sub-states.
        /// </summary>
        /// <value>The initial sub-state. Null if this state has no sub-states.</value>
        IState<TState, TEvent> InitialState { get; set; }

        /// <summary>
        /// Gets or sets the super-state. Null if this is a root state.
        /// </summary>
        /// <value>The super-state.</value>
        IState<TState, TEvent> SuperState { get; set; }

        /// <summary>
        /// Gets the sub-states.
        /// </summary>
        /// <value>The sub-states.</value>
        ICollection<IState<TState, TEvent>> SubStates { get; }

        /// <summary>
        /// Gets the transitions.
        /// </summary>
        /// <value>The transitions.</value>
        TransitionDictionary<TState, TEvent> Transitions { get; }

        /// <summary>
        /// Gets or sets the level in the hierarchy.
        /// </summary>
        /// <value>The level in the hierarchy.</value>
        int Level { get; set; }

        /// <summary>
        /// Gets or sets the last active state of this state.
        /// </summary>
        /// <value>The last state of the active.</value>
        IState<TState, TEvent> LastActiveState { get; set; }

        /// <summary>
        /// Gets the entry actions.
        /// </summary>
        /// <value>The entry actions.</value>
        IList<IActionHolder> EntryActions { get; }

        /// <summary>
        /// Gets the exit actions.
        /// </summary>
        /// <value>The exit actions.</value>
        IList<IActionHolder> ExitActions { get; }

        /// <summary>
        /// Gets or sets the history type of this state.
        /// </summary>
        /// <value>The type of the history.</value>
        HistoryType HistoryType { get; set; }

        /// <summary>
        /// Fires the specified event id on this state.
        /// </summary>
        /// <param name="context">The event context.</param>
        /// <returns>The result of the transition.</returns>
        ITransitionResult<TState, TEvent> Fire(ITransitionContext<TState, TEvent> context);

        /// <summary>
        /// Enters this state.
        /// </summary>
        /// <param name="stateContext">The event context.</param>
        void Entry(IStateContext<TState, TEvent> stateContext);

        /// <summary>
        /// Exits this state.
        /// </summary>
        /// <param name="stateContext">The event context.</param>
        void Exit(IStateContext<TState, TEvent> stateContext);

        /// <summary>
        /// Enters this state by its history depending on <see cref="State{TState,TEvent}.HistoryType"/>.
        /// The <see cref="State{TState,TEvent}.Entry"/> method has to be called already.
        /// </summary>
        /// <param name="stateContext">The event context.</param>
        /// <returns>
        /// The active state (depends on <see cref="State{TState,TEvent}.HistoryType"/>.
        /// </returns>
        IState<TState, TEvent> EnterByHistory(IStateContext<TState, TEvent> stateContext);

        /// <summary>
        /// Enters this state is shallow mode:
        /// The entry action is executed and the initial state is entered in shallow mode if there is one.
        /// </summary>
        /// <param name="stateContext">The event context.</param>
        /// <returns>The active state.</returns>
        IState<TState, TEvent> EnterShallow(IStateContext<TState, TEvent> stateContext);

        /// <summary>
        /// Enters this state is deep mode:
        /// The entry action is executed and the initial state is entered in deep mode if there is one.
        /// </summary>
        /// <param name="stateContext">The event context.</param>
        /// <returns>The active state.</returns>
        IState<TState, TEvent> EnterDeep(IStateContext<TState, TEvent> stateContext);

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        string ToString();
    }
}