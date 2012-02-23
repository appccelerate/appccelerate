//-------------------------------------------------------------------------------
// <copyright file="State.cs" company="Appccelerate">
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
    /// A state of the state machine.
    /// A state can be a sub-state or super-state of another state.
    /// </summary>
    /// <typeparam name="TState">The type of the state id.</typeparam>
    /// <typeparam name="TEvent">The type of the event id.</typeparam>
    public class State<TState, TEvent> 
        : IState<TState, TEvent>
        where TState : IComparable
        where TEvent : IComparable
    {
        /// <summary>
        /// Collection of the sub-states of this state.
        /// </summary>
        private readonly List<IState<TState, TEvent>> subStates;

        /// <summary>
        /// Collection of transitions that start in this state (<see cref="ITransition{TState,TEvent}.Source"/> is equal to this state).
        /// </summary>
        private readonly TransitionDictionary<TState, TEvent> transitions;

        private readonly IStateMachineInformation<TState, TEvent> stateMachineInformation;

        private readonly IExtensionHost<TState, TEvent> extensionHost;

        /// <summary>
        /// The level of this state within the state hierarchy [1..maxLevel]
        /// </summary>
        private int level;

        /// <summary>
        /// The super-state of this state. Null for states with <see cref="level"/> equal to 1.
        /// </summary>
        private IState<TState, TEvent> superState;

        /// <summary>
        /// The initial sub-state of this state.
        /// </summary>
        private IState<TState, TEvent> initialState;

        /// <summary>
        /// The <see cref="HistoryType"/> of this state.
        /// </summary>
        private HistoryType historyType = HistoryType.None;

        /// <summary>
        /// Initializes a new instance of the <see cref="State&lt;TState, TEvent&gt;"/> class.
        /// </summary>
        /// <param name="id">The unique id of this state.</param>
        /// <param name="stateMachineInformation">The state machine information.</param>
        /// <param name="extensionHost">The extension host.</param>
        public State(TState id, IStateMachineInformation<TState, TEvent> stateMachineInformation, IExtensionHost<TState, TEvent> extensionHost)
        {
            this.Id = id;
            this.level = 1;
            this.stateMachineInformation = stateMachineInformation;
            this.extensionHost = extensionHost;

            this.subStates = new List<IState<TState, TEvent>>();
            this.transitions = new TransitionDictionary<TState, TEvent>(this);

            this.EntryActions = new List<IActionHolder>();
            this.ExitActions = new List<IActionHolder>();
        }

        /// <summary>
        /// Gets or sets the last active state of this state.
        /// </summary>
        /// <value>The last state of the active.</value>
        public IState<TState, TEvent> LastActiveState { get; set; }

        /// <summary>
        /// Gets the unique id of this state.
        /// </summary>
        /// <value>The id of this state.</value>
        public TState Id { get; private set; }

        /// <summary>
        /// Gets the entry actions.
        /// </summary>
        /// <value>The entry actions.</value>
        public IList<IActionHolder> EntryActions { get; private set; }

        /// <summary>
        /// Gets the exit actions.
        /// </summary>
        /// <value>The exit action.</value>
        public IList<IActionHolder> ExitActions { get; private set; }

        /// <summary>
        /// Gets or sets the initial sub state of this state.
        /// </summary>
        /// <value>The initial sub state of this state.</value>
        public IState<TState, TEvent> InitialState
        {
            get
            {
                return this.initialState;
            }

            set
            {
                this.CheckInitialStateIsNotThisInstance(value);
                this.CheckInitialStateIsASubState(value);

                this.initialState = this.LastActiveState = value;
            }
        }

        /// <summary>
        /// Gets or sets the super-state of this state.
        /// </summary>
        /// <remarks>
        /// The <see cref="Level"/> of this state is changed accordingly to the super-state.
        /// </remarks>
        /// <value>The super-state of this super.</value>
        public IState<TState, TEvent> SuperState
        {
            get
            {
                return this.superState;
            }

            set
            {
                this.CheckSuperStateIsNotThisInstance(value);

                this.superState = value;

                this.SetInitialLevel();
            }
        }

        /// <summary>
        /// Gets or sets the level of this state in the state hierarchy.
        /// When set then the levels of all sub-states are changed accordingly.
        /// </summary>
        /// <value>The level.</value>
        public int Level
        {
            get
            {
                return this.level;
            }
            
            set
            {
                this.level = value;

                this.SetLevelOfSubStates();
            }
        }

        /// <summary>
        /// Gets or sets the history type of this state.
        /// </summary>
        /// <value>The type of the history.</value>
        public HistoryType HistoryType
        {
            get { return this.historyType; } 
            set { this.historyType = value; }
        }

        /// <summary>
        /// Gets the sub-states of this state.
        /// </summary>
        /// <value>The sub-states of this state.</value>
        public ICollection<IState<TState, TEvent>> SubStates 
        { 
            get { return this.subStates; }
        }

        /// <summary>
        /// Gets the transitions that start in this state.
        /// </summary>
        /// <value>The transitions.</value>
        public TransitionDictionary<TState, TEvent> Transitions
        {
            get { return this.transitions; }
        }

        /// <summary>
        /// Goes recursively up the state hierarchy until a state is found that can handle the event.
        /// </summary>
        /// <param name="context">The event context.</param>
        /// <returns>The result of the transition.</returns>
        public ITransitionResult<TState, TEvent> Fire(ITransitionContext<TState, TEvent> context)
        {
            Ensure.ArgumentNotNull(context, "context");

            ITransitionResult<TState, TEvent> result = TransitionResult<TState, TEvent>.NotFired;

            var transitionsForEvent = this.transitions[context.EventId];
            if (transitionsForEvent != null)
            {
                foreach (ITransition<TState, TEvent> transition in transitionsForEvent)
                {
                    result = transition.Fire(context);
                    if (result.Fired)
                    {
                        return result;
                    }
                }
            }

            if (this.SuperState != null)
            {
                result = this.SuperState.Fire(context);
            }

            return result;
        }

        /// <summary>
        /// Enters this state.
        /// </summary>
        /// <param name="stateContext">The event context.</param>
        public void Entry(IStateContext<TState, TEvent> stateContext)
        {
            Ensure.ArgumentNotNull(stateContext, "stateContext");

            stateContext.AddRecord(this.Id, RecordType.Enter);

            this.ExecuteEntryActions(stateContext);
        }

        /// <summary>
        /// Exits this state, executes the exit action and sets the <see cref="LastActiveState"/> on the super-state.
        /// </summary>
        /// <param name="stateContext">The event context.</param>
        public void Exit(IStateContext<TState, TEvent> stateContext)
        {
            Ensure.ArgumentNotNull(stateContext, "stateContext");

            stateContext.AddRecord(this.Id, RecordType.Exit);

            this.ExecuteExitActions(stateContext);
            this.SetThisStateAsLastStateOfSuperState();
        }

        /// <summary>
        /// Enters this state by its history depending on <see cref="HistoryType"/>.
        /// The <see cref="Entry"/> method has to be called already.
        /// </summary>
        /// <param name="stateContext">The event context.</param>
        /// <returns>
        /// The active state (depends on <see cref="HistoryType"/>.
        /// </returns>
        public IState<TState, TEvent> EnterByHistory(IStateContext<TState, TEvent> stateContext)
        {
            IState<TState, TEvent> result = this;

            switch (this.HistoryType)
            {
                case HistoryType.None:
                    result = this.EnterHistoryNone(stateContext);
                    break;

                case HistoryType.Shallow:
                    result = this.EnterHistoryShallow(stateContext);
                    break;

                case HistoryType.Deep:
                    result = this.EnterHistoryDeep(stateContext);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Enters this state is shallow mode:
        /// The entry action is executed and the initial state is entered in shallow mode if there is one.
        /// </summary>
        /// <param name="stateContext">The event context.</param>
        /// <returns>The entered state.</returns>
        public IState<TState, TEvent> EnterShallow(IStateContext<TState, TEvent> stateContext)
        {
            this.Entry(stateContext);

            return this.initialState == null ?
                                                 this :
                                                          this.initialState.EnterShallow(stateContext);
        }

        /// <summary>
        /// Enters this state is deep mode:
        /// The entry action is executed and the initial state is entered in deep mode if there is one.
        /// </summary>
        /// <param name="stateContext">The event context.</param>
        /// <returns>The active state.</returns>
        public IState<TState, TEvent> EnterDeep(IStateContext<TState, TEvent> stateContext)
        {
            this.Entry(stateContext);

            return this.LastActiveState == null ?
                                                    this :
                                                             this.LastActiveState.EnterDeep(stateContext);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return this.Id.ToString();
        }

        /// <summary>
        /// Handles the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="stateContext">The state context.</param>
        private static void HandleException(Exception exception, IStateContext<TState, TEvent> stateContext)
        {
            stateContext.OnExceptionThrown(exception);
        }

        /// <summary>
        /// Sets the initial level depending on the level of the super state of this instance.
        /// </summary>
        private void SetInitialLevel()
        {
            this.Level = this.superState != null ? this.superState.Level + 1 : 1;
        }

        /// <summary>
        /// Sets the level of all sub states.
        /// </summary>
        private void SetLevelOfSubStates()
        {
            foreach (var state in this.subStates)
            {
                state.Level = this.level + 1;
            }
        }

        private void ExecuteEntryActions(IStateContext<TState, TEvent> stateContext)
        {
            foreach (var actionHolder in this.EntryActions)
            {
                this.ExecuteEntryAction(actionHolder, stateContext);
            }
        }

        private void ExecuteEntryAction(IActionHolder actionHolder, IStateContext<TState, TEvent> stateContext)
        {
            try
            {
                actionHolder.Execute();
            }
            catch (Exception exception)
            {
                this.HandleEntryActionException(stateContext, exception);
            }
        }

        private void HandleEntryActionException(IStateContext<TState, TEvent> stateContext, Exception exception)
        {
            this.extensionHost.ForEach(
                extension =>
                extension.HandlingEntryActionException(
                    this.stateMachineInformation, this, stateContext, ref exception));

            HandleException(exception, stateContext);

            this.extensionHost.ForEach(
                extension =>
                extension.HandledEntryActionException(
                    this.stateMachineInformation, this, stateContext, exception));
        }

        private void ExecuteExitActions(IStateContext<TState, TEvent> stateContext)
        {
            foreach (var actionHolder in this.ExitActions)
            {
                this.ExecuteExitAction(actionHolder, stateContext);
            }
        }

        private void ExecuteExitAction(IActionHolder actionHolder, IStateContext<TState, TEvent> stateContext)
        {
            try
            {
                actionHolder.Execute();
            }
            catch (Exception exception)
            {
                this.HandleExitActionException(stateContext, exception);
            }
        }

        private void HandleExitActionException(IStateContext<TState, TEvent> stateContext, Exception exception)
        {
            this.extensionHost.ForEach(
                extension =>
                extension.HandlingExitActionException(
                    this.stateMachineInformation, this, stateContext, ref exception));

            HandleException(exception, stateContext);

            this.extensionHost.ForEach(
                extension =>
                extension.HandledExitActionException(
                    this.stateMachineInformation, this, stateContext, exception));
        }

        /// <summary>
        /// Sets this instance as the last state of this instance's super state.
        /// </summary>
        private void SetThisStateAsLastStateOfSuperState()
        {
            if (this.superState != null)
            {
                this.superState.LastActiveState = this;
            }
        }

        /// <summary>
        /// Enters this instance with history type = deep.
        /// </summary>
        /// <param name="stateContext">The state context.</param>
        /// <returns>The entered state.</returns>
        private IState<TState, TEvent> EnterHistoryDeep(IStateContext<TState, TEvent> stateContext)
        {
            return this.LastActiveState != null
                       ?
                           this.LastActiveState.EnterDeep(stateContext)
                       :
                           this;
        }

        /// <summary>
        /// Enters this instance with history type = shallow.
        /// </summary>
        /// <param name="stateContext">The state context.</param>
        /// <returns>The entered state.</returns>
        private IState<TState, TEvent> EnterHistoryShallow(IStateContext<TState, TEvent> stateContext)
        {
            return this.LastActiveState != null
                       ?
                           this.LastActiveState.EnterShallow(stateContext)
                       :
                           this;
        }

        /// <summary>
        /// Enters this instance with history type = none.
        /// </summary>
        /// <param name="stateContext">The state context.</param>
        /// <returns>The entered state.</returns>
        private IState<TState, TEvent> EnterHistoryNone(IStateContext<TState, TEvent> stateContext)
        {
            return this.initialState != null
                       ?
                           this.initialState.EnterShallow(stateContext)
                       :
                           this;
        }

        /// <summary>
        /// Throws an exception if the new super state is this instance.
        /// </summary>
        /// <param name="newSuperState">The value.</param>
        private void CheckSuperStateIsNotThisInstance(IState<TState, TEvent> newSuperState)
        {
            if (this == newSuperState)
            {
                throw new ArgumentException(ExceptionMessages.StateCannotBeItsOwnSuperState(this.ToString()));
            }
        }

        /// <summary>
        /// Throws an exception if the new initial state is this instance.
        /// </summary>
        /// <param name="newInitialState">The value.</param>
        private void CheckInitialStateIsNotThisInstance(IState<TState, TEvent> newInitialState)
        {
            if (this == newInitialState)
            {
                throw new ArgumentException(ExceptionMessages.StateCannotBeTheInitialSubStateToItself(this.ToString()));
            }
        }

        /// <summary>
        /// Throws an exception if the new initial state is not a sub-state of this instance.
        /// </summary>
        /// <param name="value">The value.</param>
        private void CheckInitialStateIsASubState(IState<TState, TEvent> value)
        {
            if (value.SuperState != this)
            {
                throw new ArgumentException(ExceptionMessages.StateCannotBeTheInitialStateOfSuperStateBecauseItIsNotADirectSubState(value.ToString(), this.ToString()));
            }
        }
    }
}