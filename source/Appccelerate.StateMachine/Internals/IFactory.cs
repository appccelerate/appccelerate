//-------------------------------------------------------------------------------
// <copyright file="IFactory.cs" company="Appccelerate">
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
    /// Provides creation methods for all objects needed inside the state machine.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public interface IFactory<TState, TEvent>
        where TState : IComparable
        where TEvent : IComparable
    {
        /// <summary>
        /// Creates a state.
        /// </summary>
        /// <param name="id">The id of the state.</param>
        /// <returns>A newly created state.</returns>
        IState<TState, TEvent> CreateState(TState id);

        /// <summary>
        /// Creates a transition.
        /// </summary>
        /// <returns>A newly created transition.</returns>
        ITransition<TState, TEvent> CreateTransition();

        /// <summary>
        /// Creates an action holder.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A newly created action holder.</returns>
        IActionHolder CreateActionHolder(Action action);

        /// <summary>
        /// Creates an action holder.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>A newly created action holder.</returns>
        IActionHolder CreateActionHolder<T>(Action<T> action, T parameter);

        /// <summary>
        /// Creates a transition action holder.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>A newly created transition action holder.</returns>
        ITransitionActionHolder CreateTransitionActionHolder(Action action);

        /// <summary>
        /// Creates a transition action holder.
        /// </summary>
        /// <typeparam name="T">Type of the action argument.</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>
        /// A newly created transition action holder.
        /// </returns>
        ITransitionActionHolder CreateTransitionActionHolder<T>(Action<T> action);

        /// <summary>
        /// Creates a guard holder.
        /// </summary>
        /// <param name="guard">The guard.</param>
        /// <returns>A newly created guard holder.</returns>
        IGuardHolder CreateGuardHolder(Func<bool> guard);

        /// <summary>
        /// Creates a guard holder.
        /// </summary>
        /// <typeparam name="T">Type of the guard argument.</typeparam>
        /// <param name="guard">The guard.</param>
        /// <returns>A newly created guard holder.</returns>
        IGuardHolder CreateGuardHolder<T>(Func<T, bool> guard);

        /// <summary>
        /// Creates a state context.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="notifier">The notifier.</param>
        /// <returns>A newly created state context.</returns>
        IStateContext<TState, TEvent> CreateStateContext(IState<TState, TEvent> state, INotifier<TState, TEvent> notifier);
        
        /// <summary>
        /// Creates a transition context.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="eventArgument">The event argument.</param>
        /// <param name="notifier">The notifier.</param>
        /// <returns>A newly created transition context.</returns>
        ITransitionContext<TState, TEvent> CreateTransitionContext(IState<TState, TEvent> state, TEvent eventId, object eventArgument, INotifier<TState, TEvent> notifier);

        /// <summary>
        /// Creates a state machine initializer.
        /// </summary>
        /// <param name="initialState">The initial state.</param>
        /// <param name="stateContext">The state context.</param>
        /// <returns>A newly created initializer.</returns>
        StateMachineInitializer<TState, TEvent> CreateStateMachineInitializer(IState<TState, TEvent> initialState, IStateContext<TState, TEvent> stateContext);
    }
}