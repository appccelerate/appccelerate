//-------------------------------------------------------------------------------
// <copyright file="IEntryActionSyntax{TState,TEvent}.cs" company="Appccelerate">
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
    /// Defines the entry action syntax.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public interface IEntryActionSyntax<TState, TEvent> : IExitActionSyntax<TState, TEvent>
    {
        /// <summary>
        /// Defines entry actions.
        /// </summary>
        /// <param name="actions">The actions.</param>
        /// <returns>Exit action syntax.</returns>
        IExitActionSyntax<TState, TEvent> ExecuteOnEntry(params Action[] actions);

        /// <summary>
        /// Defines an entry action.
        /// </summary>
        /// <typeparam name="T">Type of the parameter of the entry action method.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="parameter">The parameter that will be passed to the entry action.</param>
        /// <returns>Exit action syntax.</returns>
        IExitActionSyntax<TState, TEvent> ExecuteOnEntry<T>(Action<T> action, T parameter);
    }

    /// <summary>
    /// Defines the exit action syntax.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public interface IExitActionSyntax<TState, TEvent> : IEventSyntax<TState, TEvent>
    {
        /// <summary>
        /// Defines exit actions.
        /// </summary>
        /// <param name="actions">The actions.</param>
        /// <returns>Event syntax.</returns>
        IEventSyntax<TState, TEvent> ExecuteOnExit(params Action[] actions);

        /// <summary>
        /// Defines an exit action.
        /// </summary>
        /// <typeparam name="T">Type of the parameter of the exit action method.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="parameter">The parameter that will be passed to the exit action.</param>
        /// <returns>Exit action syntax.</returns>
        IEventSyntax<TState, TEvent> ExecuteOnExit<T>(Action<T> action, T parameter);
    }

    /// <summary>
    /// Defines the event syntax.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public interface IEventSyntax<TState, TEvent>
    {
        /// <summary>
        /// Defines an event that is accepted.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <returns>On syntax.</returns>
        IOnSyntax<TState, TEvent> On(TEvent eventId);
    }

    /// <summary>
    /// Defines the syntax after On.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public interface IOnSyntax<TState, TEvent> : IEventSyntax<TState, TEvent>
    {
        /// <summary>
        /// Defines the target state of the transition.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>Goto syntax</returns>
        IGotoSyntax<TState, TEvent> Goto(TState target);
        
        /// <summary>
        /// Defines the transition actions.
        /// </summary>
        /// <param name="actions">The actions to execute when the transition is taken.</param>
        /// <returns>Event syntax</returns>
        IEventSyntax<TState, TEvent> Execute(params Action[] actions);

        /// <summary>
        /// Defines the transition actions.
        /// </summary>
        /// <typeparam name="T">The type of the action argument.</typeparam>
        /// <param name="actions">The actions to execute when the transition is taken.</param>
        /// <returns>Event syntax</returns>
        IEventSyntax<TState, TEvent> Execute<T>(params Action<T>[] actions);
        
        /// <summary>
        /// Defines a transition guard. The transition is only taken if the guard is fulfilled.
        /// </summary>
        /// <typeparam name="T">The type of the guard argument.</typeparam>
        /// <param name="guard">The guard.</param>
        /// <returns>If syntax.</returns>
        IIfSyntax<TState, TEvent> If<T>(Func<T, bool> guard);

        /// <summary>
        /// Defines a transition guard. The transition is only taken if the guard is fulfilled.
        /// </summary>
        /// <param name="guard">The guard.</param>
        /// <returns>If syntax.</returns>
        IIfSyntax<TState, TEvent> If(Func<bool> guard);
    }

    /// <summary>
    /// Defines the syntax after Goto.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public interface IGotoSyntax<TState, TEvent> : IEventSyntax<TState, TEvent>
    {
        /// <summary>
        /// Defines the transition actions.
        /// </summary>
        /// <param name="actions">The actions to execute when the transition is taken.</param>
        /// <returns>Event syntax</returns>
        IEventSyntax<TState, TEvent> Execute(params Action[] actions);

        /// <summary>
        /// Defines the transition actions.
        /// </summary>
        /// <typeparam name="T">The type of the action argument.</typeparam>
        /// <param name="actions">The actions to execute when the transition is taken.</param>
        /// <returns>Event syntax</returns>
        IEventSyntax<TState, TEvent> Execute<T>(params Action<T>[] actions);
    }

    /// <summary>
    /// Defines the If syntax.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public interface IIfSyntax<TState, TEvent>
    {
        /// <summary>
        /// Defines the target state of the transition.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>Goto syntax</returns>
        IGotoInIfSyntax<TState, TEvent> Goto(TState target);
        
        /// <summary>
        /// Defines the transition actions.
        /// </summary>
        /// <param name="actions">The actions to execute when the transition is taken.</param>
        /// <returns>Event syntax</returns>
        IIfOrOtherwiseSyntax<TState, TEvent> Execute(params Action[] actions);

        /// <summary>
        /// Defines the transition actions.
        /// </summary>
        /// <typeparam name="T">The type of the action argument.</typeparam>
        /// <param name="actions">The actions to execute when the transition is taken.</param>
        /// <returns>Event syntax</returns>
        IIfOrOtherwiseSyntax<TState, TEvent> Execute<T>(params Action<T>[] actions);
    }

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
        IIfOrOtherwiseSyntax<TState, TEvent> Execute(params Action[] actions);

        /// <summary>
        /// Defines the transition actions.
        /// </summary>
        /// <typeparam name="T">The type of the action argument.</typeparam>
        /// <param name="actions">The actions to execute when the transition is taken.</param>
        /// <returns>Event syntax</returns>
        IIfOrOtherwiseSyntax<TState, TEvent> Execute<T>(params Action<T>[] actions);
    }

    /// <summary>
    /// Defines the syntax for If or Otherwise.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public interface IIfOrOtherwiseSyntax<TState, TEvent> : IEventSyntax<TState, TEvent>
    {
        /// <summary>
        /// Defines a transition guard. The transition is only taken if the guard is fulfilled.
        /// </summary>
        /// <typeparam name="T">The type of the guaard argument.</typeparam>
        /// <param name="guard">The guard.</param>
        /// <returns>If syntax.</returns>
        IIfSyntax<TState, TEvent> If<T>(Func<T, bool> guard);

        /// <summary>
        /// Defines a transition guard. The transition is only taken if the guard is fulfilled.
        /// </summary>
        /// <param name="guard">The guard.</param>
        /// <returns>If syntax.</returns>
        IIfSyntax<TState, TEvent> If(Func<bool> guard);

        /// <summary>
        /// Defines the transition that is taken when the guards of all other transitions did not match.
        /// </summary>
        /// <returns>Default syntax.</returns>
        IOtherwiseSyntax<TState, TEvent> Otherwise();
    }

    /// <summary>
    /// Defines the Otherwise syntax
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public interface IOtherwiseSyntax<TState, TEvent> : IEventSyntax<TState, TEvent>
    {
        /// <summary>
        /// Defines the target state of the transition.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>Goto syntax</returns>
        IGotoSyntax<TState, TEvent> Goto(TState target);
        
        /// <summary>
        /// Defines the transition actions.
        /// </summary>
        /// <param name="actions">The actions to execute when the transition is taken.</param>
        /// <returns>Event syntax</returns>
        IEventSyntax<TState, TEvent> Execute(params Action[] actions);

        /// <summary>
        /// Defines the transition actions.
        /// </summary>
        /// <typeparam name="T">The type of the action argument.</typeparam>
        /// <param name="actions">The actions to execute when the transition is taken.</param>
        /// <returns>Event syntax</returns>
        IEventSyntax<TState, TEvent> Execute<T>(params Action<T>[] actions);
    }
}