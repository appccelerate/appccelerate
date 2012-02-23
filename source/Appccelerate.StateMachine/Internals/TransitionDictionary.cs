//-------------------------------------------------------------------------------
// <copyright file="TransitionDictionary.cs" company="Appccelerate">
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
    /// Manages the transitions of a state.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public class TransitionDictionary<TState, TEvent>
        where TState : IComparable
        where TEvent : IComparable
    {
        /// <summary>
        /// The transitions.
        /// </summary>
        private readonly Dictionary<TEvent, List<ITransition<TState, TEvent>>> transitions;

        /// <summary>
        /// The state this transition dictionary belongs to.
        /// </summary>
        private readonly IState<TState, TEvent> state;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionDictionary&lt;TState, TEvent&gt;"/> class.
        /// </summary>
        /// <param name="state">The state.</param>
        public TransitionDictionary(IState<TState, TEvent> state)
        {
            this.state = state;
            this.transitions = new Dictionary<TEvent, List<ITransition<TState, TEvent>>>();
        }

        /// <summary>
        /// Gets the transitions for the specified event id.
        /// </summary>
        /// <value>transitions for the event id.</value>
        /// <param name="eventId">Id of the event.</param>
        /// <returns>The transitions for the event id.</returns>
        public ICollection<ITransition<TState, TEvent>> this[TEvent eventId]
        {
            get
            {
                List<ITransition<TState, TEvent>> result;

                this.transitions.TryGetValue(eventId, out result);

                return result;
            }
        }

        /// <summary>
        /// Adds the specified event id.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <param name="transition">The transition.</param>
        public void Add(TEvent eventId, ITransition<TState, TEvent> transition)
        {
            Ensure.ArgumentNotNull(transition, "transition");

            this.CheckTransitionDoesNotYetExist(transition);

            transition.Source = this.state;

            this.MakeSureEventExistsInTransitionList(eventId);

            this.transitions[eventId].Add(transition);
        }

        /// <summary>
        /// Gets all transitions.
        /// </summary>
        /// <returns>All transitions.</returns>
        public IEnumerable<TransitionInfo> GetTransitions()
        {
            var list = new List<TransitionInfo>();
            foreach (var eventId in this.transitions.Keys)
            {
                this.GetTransitionsOfEvent(eventId, list);
            }

            return list;
        }

        /// <summary>
        /// Throws an exception if the specified transition is already defined on this state.
        /// </summary>
        /// <param name="transition">The transition.</param>
        private void CheckTransitionDoesNotYetExist(ITransition<TState, TEvent> transition)
        {
            if (transition.Source != null)
            {
                throw new InvalidOperationException(ExceptionMessages.TransitionDoesAlreadyExist(transition, this.state));
            }
        }

        /// <summary>
        /// If there is no entry in the <see cref="transitions"/> dictionary then one is created.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        private void MakeSureEventExistsInTransitionList(TEvent eventId)
        {
            if (!this.transitions.ContainsKey(eventId))
            {
                var list = new List<ITransition<TState, TEvent>>();
                this.transitions.Add(eventId, list);
            }
        }

        /// <summary>
        /// Gets all the transitions associated to the specified event.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <param name="list">The list to add the transition.</param>
        private void GetTransitionsOfEvent(TEvent eventId, List<TransitionInfo> list)
        {
            foreach (var transition in this.transitions[eventId])
            {
                list.Add(new TransitionInfo(eventId, transition.Source, transition.Target, transition.Guard, transition.Actions));
            }
        }

        /// <summary>
        /// Describes a transition.
        /// </summary>
        public class TransitionInfo
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TransitionDictionary{TState, TEvent}.TransitionInfo"/> class.
            /// </summary>
            /// <param name="eventId">The event id.</param>
            /// <param name="source">The source.</param>
            /// <param name="target">The target.</param>
            /// <param name="guard">The guard.</param>
            /// <param name="actions">The actions.</param>
            public TransitionInfo(TEvent eventId, IState<TState, TEvent> source, IState<TState, TEvent> target, IGuardHolder guard, IEnumerable<ITransitionActionHolder> actions)
            {
                this.EventId = eventId;
                this.Source = source;
                this.Target = target;
                this.Guard = guard;
                this.Actions = actions;
            }

            /// <summary>
            /// Gets the event id.
            /// </summary>
            /// <value>The event id.</value>
            public TEvent EventId
            {
                get; private set;
            }

            /// <summary>
            /// Gets the source.
            /// </summary>
            /// <value>The source.</value>
            public IState<TState, TEvent> Source
            {
                get; private set;
            }

            /// <summary>
            /// Gets the target.
            /// </summary>
            /// <value>The target.</value>
            public IState<TState, TEvent> Target
            {
                get; private set;
            }

            /// <summary>
            /// Gets the guard.
            /// </summary>
            /// <value>The guard.</value>
            public IGuardHolder Guard
            {
                get; private set;
            }

            /// <summary>
            /// Gets the actions.
            /// </summary>
            /// <value>The actions.</value>
            public IEnumerable<ITransitionActionHolder> Actions
            {
                get; private set;
            }
        }
    }
}