//-------------------------------------------------------------------------------
// <copyright file="StateContext.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Text;

    using Appccelerate.StateMachine.Machine.States;

    /// <summary>
    /// Provides context information. Gathers information during executing operations on the state machine
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    [DebuggerDisplay("State = {state}")]
    public class StateContext<TState, TEvent> : IStateContext<TState, TEvent>
        where TState : IComparable
        where TEvent : IComparable
    {
        /// <summary>
        /// The source state of the transition.
        /// </summary>
        private readonly IState<TState, TEvent> state;

        /// <summary>
        /// The exceptions that occurred during performing an operation.
        /// </summary>
        private readonly IList<Exception> exceptions;

        /// <summary>
        /// The list of records (state exits, entries)
        /// </summary>
        private readonly List<Record> records;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateContext&lt;TState, TEvent&gt;"/> class.
        /// </summary>
        public StateContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateContext{TState,TEvent}"/> class.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="notifier">The notifier.</param>
        public StateContext(IState<TState, TEvent> state, INotifier<TState, TEvent> notifier)
        {
            this.state = state;
            this.Notifier = notifier;

            this.exceptions = new List<Exception>();
            this.records = new List<Record>();
        }

        /// <summary>
        /// Gets the source state of the transition.
        /// </summary>
        /// <value>The state.</value>
        public IState<TState, TEvent> State
        {
            get { return this.state; }
        }

        /// <summary>
        /// Gets the exceptions.
        /// </summary>
        /// <value>The exceptions.</value>
        public ReadOnlyCollection<Exception> Exceptions
        {
            get { return new ReadOnlyCollection<Exception>(this.exceptions); }
        }

        /// <summary>
        /// Gets the notifier.
        /// </summary>
        /// <value>The notifier.</value>
        protected INotifier<TState, TEvent> Notifier
        {
            get; private set;
        }

        /// <summary>
        /// Adds a record.
        /// </summary>
        /// <param name="stateId">The state id.</param>
        /// <param name="recordType">Type of the record.</param>
        public void AddRecord(TState stateId, RecordType recordType)
        {
            this.records.Add(new Record(stateId, recordType));
        }

        /// <summary>
        /// Gets all records in string representation.
        /// </summary>
        /// <returns>All records in string representation.</returns>
        public virtual string GetRecords()
        {
            StringBuilder result = new StringBuilder();

            this.records.ForEach(record => result.AppendFormat(" -> {0}", record));

            return result.ToString();
        }

        /// <summary>
        /// Called when an exception should be notified.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public virtual void OnExceptionThrown(Exception exception)
        {
            this.AddException(exception);
            this.Notifier.OnExceptionThrown(this, exception);   
        }

        /// <summary>
        /// Adds an exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        protected void AddException(Exception exception)
        {
            this.exceptions.Add(exception);
        }

        /// <summary>
        /// A record of a state exit or entry. Used to log the way taken by transitions and initialization.
        /// </summary>
        private class Record
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="StateContext{TState,TEvent}.Record"/> class.
            /// </summary>
            /// <param name="stateId">The state id.</param>
            /// <param name="recordType">Type of the record.</param>
            public Record(TState stateId, RecordType recordType)
            {
                this.StateId = stateId;
                this.RecordType = recordType;
            }

            /// <summary>
            /// Gets the state id.
            /// </summary>
            /// <value>The state id.</value>
            public TState StateId { get; private set; }

            /// <summary>
            /// Gets the type of the record.
            /// </summary>
            /// <value>The type of the record.</value>
            public RecordType RecordType { get; private set; }

            /// <summary>
            /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </returns>
            public override string ToString()
            {
                return this.RecordType + " " + this.StateId;
            }
        }
    }
}