//-------------------------------------------------------------------------------
// <copyright file="Transition.cs" company="Appccelerate">
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
    using System.Globalization;

    using Appccelerate.StateMachine.Machine.ActionHolders;
    using Appccelerate.StateMachine.Machine.GuardHolders;
    
    /// <summary>
    /// A transition of the state machine.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public class Transition<TState, TEvent>
        : ITransition<TState, TEvent>
        where TState : IComparable
        where TEvent : IComparable
    {
        /// <summary>
        /// The actions that are executed when this transition is fired.
        /// </summary>
        private readonly List<IActionHolder> actions;

        private readonly IExtensionHost<TState, TEvent> extensionHost;
        private readonly IStateMachineInformation<TState, TEvent> stateMachineInformation;

        /// <summary>
        /// Initializes a new instance of the <see cref="Transition&lt;TState, TEvent&gt;"/> class.
        /// </summary>
        /// <param name="stateMachineInformation">The state machine information.</param>
        /// <param name="extensionHost">The extension host.</param>
        public Transition(IStateMachineInformation<TState, TEvent> stateMachineInformation, IExtensionHost<TState, TEvent> extensionHost)
        {
            this.stateMachineInformation = stateMachineInformation;
            this.extensionHost = extensionHost;

            this.actions = new List<IActionHolder>();
        }

        /// <summary>
        /// Gets or sets the source state of this transition.
        /// </summary>
        /// <value>The source state.</value>
        public IState<TState, TEvent> Source { get; set; }

        /// <summary>
        /// Gets or sets the target state of this transition.
        /// </summary>
        /// <value>The target state.</value>
        public IState<TState, TEvent> Target { get; set; }

        /// <summary>
        /// Gets or sets the guard of this transition.
        /// </summary>
        /// <value>The guard.</value>
        public IGuardHolder Guard { get; set; }

        /// <summary>
        /// Gets the actions of this transition.
        /// </summary>
        /// <value>The actions.</value>
        public ICollection<IActionHolder> Actions
        {
            get { return this.actions; }
        }

        /// <summary>
        /// Gets a value indicating whether this is an internal transition.
        /// </summary>
        /// <value><c>true</c> if this is an internal transition; otherwise, <c>false</c>.</value>
        private bool InternalTransition
        {
            get { return this.Target == null; }
        }

        /// <summary>
        /// Fires the transition.
        /// </summary>
        /// <param name="context">The event context.</param>
        /// <returns>The result of the transition.</returns>
        public ITransitionResult<TState, TEvent> Fire(ITransitionContext<TState, TEvent> context)
        {
            Ensure.ArgumentNotNull(context, "context");

            if (!this.ShouldFire(context))
            {
                return TransitionResult<TState, TEvent>.NotFired;
            }

            context.OnTransitionBegin();

            IState<TState, TEvent> newState = context.State;

            if (!this.InternalTransition)
            {
                this.UnwindSubStates(context);

                this.Fire(this.Source, this.Target, context);

                newState = this.Target.EnterByHistory(context);
            }
            else
            {
                this.PerformActions(context);
            }

            return new TransitionResult<TState, TEvent>(true, newState, context.Exceptions);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Transition from state {0} to state {1}.", this.Source, this.Target);
        }

        /// <summary>
        /// Handles an exception thrown during performing the transition or guard evaluation.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="context">The context.</param>
        private static void HandleException(Exception exception, ITransitionContext<TState, TEvent> context)
        {
            context.OnExceptionThrown(exception);
        }

        /// <summary>
        /// Recursively traverses the state hierarchy, exiting states along
        /// the way, performing the action, and entering states to the target.
        /// </summary>
        /// <remarks>
        /// There exist the following transition scenarios:
        /// 0. there is no target state (internal transition)
        ///    --> handled outside this method.
        /// 1. The source and target state are the same (self transition)
        ///    --> perform the transition directly:
        ///        Exit source state, perform transition actions and enter target state
        /// 2. The target state is a direct or indirect sub-state of the source state
        ///    --> perform the transition actions, then traverse the hierarchy 
        ///        from the source state down to the target state,
        ///        entering each state along the way.
        ///        No state is exited.
        /// 3. The source state is a sub-state of the target state
        ///    --> traverse the hierarchy from the source up to the target, 
        ///        exiting each state along the way. 
        ///        Then perform transition actions.
        ///        Finally enter the target state.
        /// 4. The source and target state share the same super-state
        /// 5. All other scenarios:
        ///    a. The source and target states reside at the same level in the hierarchy 
        ///       but do not share the same direct super-state
        ///    --> exit the source state, move up the hierarchy on both sides and enter the target state
        ///    b. The source state is lower in the hierarchy than the target state
        ///    --> exit the source state and move up the hierarchy on the source state side
        ///    c. The target state is lower in the hierarchy than the source state
        ///    --> move up the hierarchy on the target state side, afterward enter target state
        /// </remarks>
        /// <param name="source">The source state.</param>
        /// <param name="target">The target state.</param>
        /// <param name="context">The event context.</param>
        private void Fire(IState<TState, TEvent> source, IState<TState, TEvent> target, ITransitionContext<TState, TEvent> context)
        {
            if (source == this.Target)
            {
                // Handles 1.
                // Handles 3. after traversing from the source to the target.
                source.Exit(context);
                this.PerformActions(context);
                this.Target.Entry(context);
            }
            else if (source == target)
            {
                // Handles 2. after traversing from the target to the source.
                this.PerformActions(context);
            }
            else if (source.SuperState == target.SuperState)
            {
                //// Handles 4.
                //// Handles 5a. after traversing the hierarchy until a common ancestor if found.
                source.Exit(context);
                this.PerformActions(context);
                target.Entry(context);
            }
            else
            {
                // traverses the hierarchy until one of the above scenarios is met.

                // Handles 3.
                // Handles 5b.
                if (source.Level > target.Level)
                {
                    source.Exit(context);
                    this.Fire(source.SuperState, target, context);
                }
                else if (source.Level < target.Level)
                {
                    // Handles 2.
                    // Handles 5c.
                    this.Fire(source, target.SuperState, context);
                    target.Entry(context);
                }
                else
                {
                    // Handles 5a.
                    source.Exit(context);
                    this.Fire(source.SuperState, target.SuperState, context);
                    target.Entry(context);
                }
            }
        }

        private bool ShouldFire(ITransitionContext<TState, TEvent> context)
        {
            try
            {
                return this.Guard == null || this.Guard.Execute(context.EventArgument);
            }
            catch (Exception exception)
            {
                this.extensionHost.ForEach(extention => extention.HandlingGuardException(this.stateMachineInformation, this, context, ref exception));
                
                HandleException(exception, context);

                this.extensionHost.ForEach(extention => extention.HandledGuardException(this.stateMachineInformation, this, context, exception));

                return false;
            }
        }

        private void PerformActions(ITransitionContext<TState, TEvent> context)
        {
            foreach (var action in this.actions)
            {
                try
                {
                    action.Execute(context.EventArgument);
                }
                catch (Exception exception)
                {
                    this.extensionHost.ForEach(extension => extension.HandlingTransitionException(this.stateMachineInformation, this, context, ref exception));
                    
                    HandleException(exception, context);

                    this.extensionHost.ForEach(extension => extension.HandledTransitionException(this.stateMachineInformation, this, context, exception));
                }
            }
        }

        private void UnwindSubStates(ITransitionContext<TState, TEvent> context)
        {
            for (IState<TState, TEvent> o = context.State; o != this.Source; o = o.SuperState)
            {
                o.Exit(context);
            }
        }
    }
}