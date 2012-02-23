//-------------------------------------------------------------------------------
// <copyright file="IStateContext.cs" company="Appccelerate">
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
    using System.Collections.ObjectModel;

    /// <summary>
    /// Provides information about the current state.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    public interface IStateContext<TState, TEvent>
        where TState : IComparable where TEvent : IComparable
    {
        /// <summary>
        /// Gets the source state of the transition.
        /// </summary>
        /// <value>The state.</value>
        IState<TState, TEvent> State { get; }

        /// <summary>
        /// Gets the exceptions.
        /// </summary>
        /// <value>The exceptions.</value>
        ReadOnlyCollection<Exception> Exceptions { get; }

        /// <summary>
        /// Adds a record.
        /// </summary>
        /// <param name="stateId">The state id.</param>
        /// <param name="recordType">Type of the record.</param>
        void AddRecord(TState stateId, RecordType recordType);

        /// <summary>
        /// Gets all records in string representation.
        /// </summary>
        /// <returns>All records in string representation.</returns>
        string GetRecords();

        /// <summary>
        /// Called when an exception should be notified.
        /// </summary>
        /// <param name="exception">The exception.</param>
        void OnExceptionThrown(Exception exception);
    }
}