//-------------------------------------------------------------------------------
// <copyright file="StateMachineInitializer.cs" company="Appccelerate">
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
    /// Responsible for entering the initial state of the state machine. 
    /// All states up in the hierarchy are entered, too.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public class StateMachineInitializer<TState, TEvent>
        where TState : IComparable
        where TEvent : IComparable
    {
        /// <summary>
        /// The state to enter.
        /// </summary>
        private readonly IState<TState, TEvent> initialState;

        /// <summary>
        /// Context information of this operation.
        /// </summary>
        private readonly IStateContext<TState, TEvent> stateContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachineInitializer&lt;TState, TEvent&gt;"/> class.
        /// </summary>
        /// <param name="initialState">The initial state to enter.</param>
        /// <param name="stateContext">The state context.</param>
        public StateMachineInitializer(IState<TState, TEvent> initialState, IStateContext<TState, TEvent> stateContext)
        {
            this.initialState = initialState;
            this.stateContext = stateContext;
        }

        /// <summary>
        /// Enters the initial state by entering all states further up in the hierarchy.
        /// </summary>
        /// <returns>The entered state. The initial state or a sub state of the initial state.</returns>
        public IState<TState, TEvent> EnterInitialState()
        {
            var stack = this.TraverseUpTheStateHierarchy();
            this.TraverseDownTheStateHierarchyAndEnterStates(stack);

            return this.initialState.EnterByHistory(this.stateContext);
        }

        /// <summary>
        /// Traverses up the state hierarchy and build the stack of states.
        /// </summary>
        /// <returns>The stack containing all states up the state hierarchy.</returns>
        private Stack<IState<TState, TEvent>> TraverseUpTheStateHierarchy()
        {
            var stack = new Stack<IState<TState, TEvent>>();

            var state = this.initialState;
            while (state != null)
            {
                stack.Push(state);
                state = state.SuperState;
            }

            return stack;
        }

        /// <summary>
        /// Traverses down the state hierarchy and enter all states along.
        /// </summary>
        /// <param name="stack">The stack containing the state hierarchy.</param>
        private void TraverseDownTheStateHierarchyAndEnterStates(Stack<IState<TState, TEvent>> stack)
        {
            while (stack.Count > 0)
            {
                IState<TState, TEvent> state = stack.Pop();
                state.Entry(this.stateContext);
            }
        }
    }
}