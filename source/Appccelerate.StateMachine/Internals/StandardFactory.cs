//-------------------------------------------------------------------------------
// <copyright file="StandardFactory.cs" company="Appccelerate">
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
    /// Standard implementation of the state machine factory.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public class StandardFactory<TState, TEvent> : IFactory<TState, TEvent>
        where TState : IComparable
        where TEvent : IComparable
    {
        private readonly IStateMachineInformation<TState, TEvent> stateMachineInformation;
        private readonly IExtensionHost<TState, TEvent> extensionHost;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardFactory&lt;TState, TEvent&gt;"/> class.
        /// </summary>
        /// <param name="stateMachineInformation">The state machine information.</param>
        /// <param name="extensionHost">The extension host.</param>
        public StandardFactory(IStateMachineInformation<TState, TEvent> stateMachineInformation, IExtensionHost<TState, TEvent> extensionHost)
        {
            this.stateMachineInformation = stateMachineInformation;
            this.extensionHost = extensionHost;
        }

        /// <summary>
        /// Creates a state.
        /// </summary>
        /// <param name="id">The id of the state.</param>
        /// <returns>A newly created state.</returns>
        public virtual IState<TState, TEvent> CreateState(TState id)
        {
            return new State<TState, TEvent>(id, this.stateMachineInformation, this.extensionHost);
        }

        /// <summary>
        /// Creates a transition.
        /// </summary>
        /// <returns>A newly created transition.</returns>
        public virtual ITransition<TState, TEvent> CreateTransition()
        {
            return new Transition<TState, TEvent>(this.stateMachineInformation, this.extensionHost);
        }

        /// <summary>
        /// Creates an action holder.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A newly created action holder.</returns>
        public virtual IActionHolder CreateActionHolder(Action action)
        {
            return new ActionHolder(action);
        }

        /// <summary>
        /// Creates an action holder.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>A newly created action holder.</returns>
        public virtual IActionHolder CreateActionHolder<T>(Action<T> action, T parameter)
        {
            return new ActionHolder<T>(action, parameter);    
        }

        /// <summary>
        /// Creates the transition action holder.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A newly created transition action holder.</returns>
        public virtual ITransitionActionHolder CreateTransitionActionHolder(Action action)
        {
            return new ArgumentLessTransitionActionHolder(action);
        }

        /// <summary>
        /// Creates the transition action holder.
        /// </summary>
        /// <typeparam name="T">The type of the action argument.</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>A newly created transition action holder.</returns>
        public virtual ITransitionActionHolder CreateTransitionActionHolder<T>(Action<T> action)
        {
            return new ArgumentTransitionActionHolder<T>(action);
        }
        
        /// <summary>
        /// Creates the guard holder.
        /// </summary>
        /// <param name="guard">The guard.</param>
        /// <returns>A newly created guard holder.</returns>
        public virtual IGuardHolder CreateGuardHolder(Func<bool> guard)
        {
            return new ArgumentLessGuardHolder(guard);
        }

        /// <summary>
        /// Creates the guard holder.
        /// </summary>
        /// <typeparam name="T">The type of the guard argument.</typeparam>
        /// <param name="guard">The guard.</param>
        /// <returns>A newly created guard holder.</returns>
        public virtual IGuardHolder CreateGuardHolder<T>(Func<T, bool> guard)
        {
            return new ArgumentGuardHolder<T>(guard);
        }

        /// <summary>
        /// Creates a state context.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="notifier">The notifier.</param>
        /// <returns>A newly created state context.</returns>
        public virtual IStateContext<TState, TEvent> CreateStateContext(IState<TState, TEvent> state, INotifier<TState, TEvent> notifier)
        {
            return new StateContext<TState, TEvent>(state, notifier);
        }

        /// <summary>
        /// Creates a transition context.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="eventArgument">The event argument.</param>
        /// <param name="notifier">The notifier.</param>
        /// <returns>A newly created transition context.</returns>
        public virtual ITransitionContext<TState, TEvent> CreateTransitionContext(IState<TState, TEvent> state, TEvent eventId, object eventArgument, INotifier<TState, TEvent> notifier)
        {
            return new TransitionContext<TState, TEvent>(state, eventId, eventArgument, notifier);
        }

        /// <summary>
        /// Creates a state machine initializer.
        /// </summary>
        /// <param name="initialState">The initial state.</param>
        /// <param name="stateContext">The state context.</param>
        /// <returns>A newly created initializer.</returns>
        public virtual StateMachineInitializer<TState, TEvent> CreateStateMachineInitializer(IState<TState, TEvent> initialState, IStateContext<TState, TEvent> stateContext)
        {
            return new StateMachineInitializer<TState, TEvent>(initialState, stateContext);
        }
    }
}