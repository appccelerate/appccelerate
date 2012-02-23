//-------------------------------------------------------------------------------
// <copyright file="StateMachineTest.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using FakeItEasy;

    using FluentAssertions;

    using Xunit;

    /// <summary>
    /// Tests state machine initialization and state switching.
    /// </summary>
    public class StateMachineTest
    {
        /// <summary>
        /// Object under test.
        /// </summary>
        private readonly StateMachine<States, Events> testee;

        /// <summary>
        /// The list of recorded actions.
        /// </summary>
        private readonly List<Record> records;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachineTest"/> class.
        /// </summary>
        public StateMachineTest()
        {
            this.records = new List<Record>();

            this.testee = new StateMachine<States, Events>();

            this.testee.DefineHierarchyOn(States.B, States.B1, HistoryType.None, States.B1, States.B2);
            this.testee.DefineHierarchyOn(States.C, States.C2, HistoryType.Shallow, States.C1, States.C2);
            this.testee.DefineHierarchyOn(States.C1, States.C1a, HistoryType.Shallow, States.C1a, States.C1b);
            this.testee.DefineHierarchyOn(States.D, States.D1, HistoryType.Deep, States.D1, States.D2);
            this.testee.DefineHierarchyOn(States.D1, States.D1a, HistoryType.Deep, States.D1a, States.D1b);

            this.testee.In(States.A)
                .ExecuteOnEntry(() => this.RecordEntry(States.A))
                .ExecuteOnExit(() => this.RecordExit(States.A))
                .On(Events.B).Goto(States.B)
                .On(Events.C).Goto(States.C)
                .On(Events.D).Goto(States.D)
                .On(Events.A);

            this.testee.In(States.B)
                .ExecuteOnEntry(() => this.RecordEntry(States.B))
                .ExecuteOnExit(() => this.RecordExit(States.B))
                .On(Events.D).Goto(States.D);

            this.testee.In(States.B1)
                .ExecuteOnEntry(() => this.RecordEntry(States.B1))
                .ExecuteOnExit(() => this.RecordExit(States.B1))
                .On(Events.B2).Goto(States.B2);

            this.testee.In(States.B2)
                .ExecuteOnEntry(() => this.RecordEntry(States.B2))
                .ExecuteOnExit(() => this.RecordExit(States.B2))
                .On(Events.A).Goto(States.A)
                .On(Events.C1b).Goto(States.C1b);

            this.testee.In(States.C)
                .ExecuteOnEntry(() => this.RecordEntry(States.C))
                .ExecuteOnExit(() => this.RecordExit(States.C))
                .On(Events.A).Goto(States.A);

            this.testee.In(States.C1)
                .ExecuteOnEntry(() => this.RecordEntry(States.C1))
                .ExecuteOnExit(() => this.RecordExit(States.C1))
                .On(Events.C1b).Goto(States.C1b);

            this.testee.In(States.C2)
                .ExecuteOnEntry(() => this.RecordEntry(States.C2))
                .ExecuteOnExit(() => this.RecordExit(States.C2));

            this.testee.In(States.C1a)
                .ExecuteOnEntry(() => this.RecordEntry(States.C1a))
                .ExecuteOnExit(() => this.RecordExit(States.C1a));

            this.testee.In(States.C1b)
                .ExecuteOnEntry(() => this.RecordEntry(States.C1b))
                .ExecuteOnExit(() => this.RecordExit(States.C1b));

            this.testee.In(States.D)
                .ExecuteOnEntry(() => this.RecordEntry(States.D))
                .ExecuteOnExit(() => this.RecordExit(States.D));

            this.testee.In(States.D1)
                .ExecuteOnEntry(() => this.RecordEntry(States.D1))
                .ExecuteOnExit(() => this.RecordExit(States.D1));
            
            this.testee.In(States.D1a)
                .ExecuteOnEntry(() => this.RecordEntry(States.D1a))
                .ExecuteOnExit(() => this.RecordExit(States.D1a));

            this.testee.In(States.D1b)
                .ExecuteOnEntry(() => this.RecordEntry(States.D1b))
                .ExecuteOnExit(() => this.RecordExit(States.D1b))
                .On(Events.A).Goto(States.A)
                .On(Events.B1).Goto(States.B1);

            this.testee.In(States.E)
                .ExecuteOnEntry(() => this.RecordEntry(States.E))
                .ExecuteOnExit(() => this.RecordExit(States.E))
                .On(Events.A).Goto(States.A)
                .On(Events.E).Goto(States.E);
        }

        [Fact]
        public void InitializationWhenInitialStateIsNotYetEnteredThenNoActionIsPerformed()
        {
            this.testee.Initialize(States.A);

            this.CheckNoRemainingRecords();
        }

        /// <summary>
        /// After initialization the state machine is in the initial state and the initial state is entered.
        /// </summary>
        [Fact]
        public void InitializeToTopLevelState()
        {
            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            Assert.Equal(States.A, this.testee.CurrentStateId);
            
            this.CheckRecord<EntryRecord>(States.A);
            this.CheckNoRemainingRecords();
        }

        /// <summary>
        /// After initialization the state machine is in the initial state and the initial state is entered.
        /// All states up in the hierarchy of the initial state are entered, too.
        /// </summary>
        [Fact]
        public void InitializeToNestedState()
        {
            this.testee.Initialize(States.D1b);
            this.testee.EnterInitialState();

            Assert.Equal(States.D1b, this.testee.CurrentStateId);

            this.CheckRecord<EntryRecord>(States.D);
            this.CheckRecord<EntryRecord>(States.D1);
            this.CheckRecord<EntryRecord>(States.D1b);
            this.CheckNoRemainingRecords();
        }

        /// <summary>
        /// When the state machine is initializes to a state with sub-states then the hierarchy is recursively
        /// traversed to the most nested state along the chain of initial states.
        /// </summary>
        [Fact]
        public void InitializeStateWithSubStates()
        {
            this.testee.Initialize(States.D);
            this.testee.EnterInitialState();

            Assert.Equal(States.D1a, this.testee.CurrentStateId);

            this.CheckRecord<EntryRecord>(States.D);
            this.CheckRecord<EntryRecord>(States.D1);
            this.CheckRecord<EntryRecord>(States.D1a);
            this.CheckNoRemainingRecords();
        }

        /// <summary>
        /// When a transition between two states at the top level then the
        /// exit action of the source state is executed, then the action is performed
        /// and the entry action of the target state is executed.
        /// Finally, the current state is the target state.
        /// </summary>
        [Fact]
        public void ExecuteTransition()
        {
            this.testee.Initialize(States.E);
            this.testee.EnterInitialState();

            this.ClearRecords();

            this.testee.Fire(Events.A);

            Assert.Equal(States.A, this.testee.CurrentStateId);

            this.CheckRecord<ExitRecord>(States.E);
            this.CheckRecord<EntryRecord>(States.A);
            this.CheckNoRemainingRecords();
        }

        /// <summary>
        /// When a transition between two states with the same super state is executed then
        /// the exit action of source state, the transition action and the entry action of 
        /// the target state are executed.
        /// </summary>
        [Fact]
        public void ExecuteTransitionBetweenStatesWithSameSuperState()
        {
            this.testee.Initialize(States.B1);
            this.testee.EnterInitialState();

            this.ClearRecords();

            this.testee.Fire(Events.B2);

            Assert.Equal(States.B2, this.testee.CurrentStateId);

            this.CheckRecord<ExitRecord>(States.B1);
            this.CheckRecord<EntryRecord>(States.B2);
            this.CheckNoRemainingRecords();
        }

        /// <summary>
        /// When a transition between two states in different super states on different levels is executed
        /// then all states from the source up to the common super-state are exited and all states down to
        /// the target state are entered. In this case the target state is lower than the source state.
        /// </summary>
        [Fact]
        public void ExecuteTransitionBetweenStatesOnDifferentLevelsDownwards()
        {
            this.testee.Initialize(States.B2);
            this.testee.EnterInitialState();
            
            this.ClearRecords();
            
            this.testee.Fire(Events.C1b);

            Assert.Equal(States.C1b, this.testee.CurrentStateId);

            this.CheckRecord<ExitRecord>(States.B2);
            this.CheckRecord<ExitRecord>(States.B);
            this.CheckRecord<EntryRecord>(States.C);
            this.CheckRecord<EntryRecord>(States.C1);
            this.CheckRecord<EntryRecord>(States.C1b);
            this.CheckNoRemainingRecords();
        }

        /// <summary>
        /// When a transition between two states in different super states on different levels is executed
        /// then all states from the source up to the common super-state are exited and all states down to
        /// the target state are entered. In this case the target state is higher than the source state.
        /// </summary>
        [Fact]
        public void ExecuteTransitionBetweenStatesOnDifferentLevelsUpwards()
        {
            this.testee.Initialize(States.D1b);
            this.testee.EnterInitialState();

            this.ClearRecords();

            this.testee.Fire(Events.B1);

            Assert.Equal(States.B1, this.testee.CurrentStateId);

            this.CheckRecord<ExitRecord>(States.D1b);
            this.CheckRecord<ExitRecord>(States.D1);
            this.CheckRecord<ExitRecord>(States.D);
            this.CheckRecord<EntryRecord>(States.B);
            this.CheckRecord<EntryRecord>(States.B1);
            this.CheckNoRemainingRecords();
        }

        /// <summary>
        /// When a transition targets a super-state then the initial-state of this super-state is entered recursively
        /// down to the most nested state. No history here!
        /// </summary>
        [Fact]
        public void ExecuteTransitionWithInitialSubState()
        {
            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.ClearRecords();

            this.testee.Fire(Events.B);

            Assert.Equal(States.B1, this.testee.CurrentStateId);

            this.CheckRecord<ExitRecord>(States.A);
            this.CheckRecord<EntryRecord>(States.B);
            this.CheckRecord<EntryRecord>(States.B1);
            this.CheckNoRemainingRecords();
        }

        /// <summary>
        /// When a transition targets a super-state with <see cref="HistoryType.None"/> then the initial
        /// sub-state is entered whatever sub.state was last active.
        /// </summary>
        [Fact]
        public void ExecuteTransitionWithHistoryTypeNone()
        {
            this.testee.Initialize(States.B2);
            this.testee.EnterInitialState();
            this.testee.Fire(Events.A);

            this.ClearRecords();

            this.testee.Fire(Events.B);

            this.CheckRecord<ExitRecord>(States.A);
            this.CheckRecord<EntryRecord>(States.B);
            this.CheckRecord<EntryRecord>(States.B1);
            this.CheckNoRemainingRecords();
        }

        /// <summary>
        /// When a transition targets a super-state with <see cref="HistoryType.Shallow"/> then the last
        /// active sub-state is entered and the initial-state of the entered sub-state is entered (no recursive history).
        /// </summary>
        [Fact]
        public void ExecuteTransitionWithHistoryTypeShallow()
        {
            this.testee.Initialize(States.C1b);
            this.testee.EnterInitialState();
            this.testee.Fire(Events.A);

            this.ClearRecords();

            this.testee.Fire(Events.C);

            Assert.Equal(States.C1a, this.testee.CurrentStateId);

            this.CheckRecord<ExitRecord>(States.A);
            this.CheckRecord<EntryRecord>(States.C);
            this.CheckRecord<EntryRecord>(States.C1);
            this.CheckRecord<EntryRecord>(States.C1a);
            this.CheckNoRemainingRecords();
        }

        /// <summary>
        /// When a transition targets a super-state with <see cref="HistoryType.Deep"/> then the last
        /// active sub-state is entered recursively down to the most nested state.
        /// </summary>
        [Fact]
        public void ExecuteTransitionWithHistoryTypeDeep()
        {
            this.testee.Initialize(States.D1b);
            this.testee.EnterInitialState();
            this.testee.Fire(Events.A);

            this.ClearRecords();

            this.testee.Fire(Events.D);

            Assert.Equal(States.D1b, this.testee.CurrentStateId);

            this.CheckRecord<ExitRecord>(States.A);
            this.CheckRecord<EntryRecord>(States.D);
            this.CheckRecord<EntryRecord>(States.D1);
            this.CheckRecord<EntryRecord>(States.D1b);
            this.CheckNoRemainingRecords();
        }

        /// <summary>
        /// The state hierarchy is recursively walked up until a state can handle the event.
        /// </summary>
        [Fact]
        public void ExecuteTransitionHandledBySuperState()
        {
            this.testee.Initialize(States.C1b);
            this.testee.EnterInitialState();
            
            this.ClearRecords();

            this.testee.Fire(Events.A);

            Assert.Equal(States.A, this.testee.CurrentStateId);

            this.CheckRecord<ExitRecord>(States.C1b);
            this.CheckRecord<ExitRecord>(States.C1);
            this.CheckRecord<ExitRecord>(States.C);
            this.CheckRecord<EntryRecord>(States.A);
            this.CheckNoRemainingRecords();
        }

        /// <summary>
        /// Internal transitions do not trigger any exit or entry actions and the state machine remains in the same state.
        /// </summary>
        [Fact]
        public void InternalTransition()
        {
            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();
            this.ClearRecords();

            this.testee.Fire(Events.A);

            Assert.Equal(States.A, this.testee.CurrentStateId);
        }

        [Fact]
        public void ExecuteSelfTransition()
        {
            this.testee.Initialize(States.E);
            this.testee.EnterInitialState();
            this.ClearRecords();

            this.testee.Fire(Events.E);

            Assert.Equal(States.E, this.testee.CurrentStateId);

            this.CheckRecord<ExitRecord>(States.E);
            this.CheckRecord<EntryRecord>(States.E);
            this.CheckNoRemainingRecords();
        }

        [Fact]
        public void ExecuteTransitionToNephew()
        {
            this.testee.Initialize(States.C1a);
            this.testee.EnterInitialState();
            this.ClearRecords();

            this.testee.Fire(Events.C1b);

            Assert.Equal(States.C1b, this.testee.CurrentStateId);

            this.CheckRecord<ExitRecord>(States.C1a);
            this.CheckRecord<EntryRecord>(States.C1b);
            this.CheckNoRemainingRecords();
        }

        [Fact]
        public void ExtensionsWhenExtensionsAreClearedThenNoExtensionIsRegistered()
        {
            bool executed = false;
            var extension = A.Fake<IExtension<States, Events>>();

            this.testee.AddExtension(extension);
            this.testee.ClearExtensions();

            this.testee.ForEach(e => executed = true);

            executed
                .Should().BeFalse();
        }

        /// <summary>
        /// Records the entry into a state
        /// </summary>
        /// <param name="state">The state.</param>
        private void RecordEntry(States state)
        {
            this.records.Add(new EntryRecord { State = state });
        }

        /// <summary>
        /// Records the exit out of a state.
        /// </summary>
        /// <param name="state">The state.</param>
        private void RecordExit(States state)
        {
            this.records.Add(new ExitRecord { State = state });
        }

        /// <summary>
        /// Clears the records.
        /// </summary>
        private void ClearRecords()
        {
            this.records.Clear();    
        }

        /// <summary>
        /// Checks that the first record in the list of records is of type <typeparamref name="T"/> and involves the specified state.
        /// The record is removed after the check.
        /// </summary>
        /// <typeparam name="T">Type of the record.</typeparam>
        /// <param name="state">The state.</param>
        private void CheckRecord<T>(States state)
        {
            Record record = this.records.FirstOrDefault();

            Assert.NotNull(record);
            Assert.True(record.GetType() == typeof(T) && record.State == state, record.Message);

            this.records.RemoveAt(0);
        }

        /// <summary>
        /// Checks that no remaining records are present.
        /// </summary>
        private void CheckNoRemainingRecords()
        {
            if (this.records.Count == 0)
            {
                return;
            }

            var sb = new StringBuilder("there are additional records:");
            foreach (string s in from record in this.records select record.GetType().Name + "-" + record.State)
            {
                sb.AppendLine();
                sb.Append(s);
            }

            Assert.True(this.records.Count == 0, sb.ToString());
        }

        /// <summary>
        /// A record of something that happened.
        /// </summary>
        private abstract class Record
        {
            /// <summary>
            /// Gets or sets the state.
            /// </summary>
            /// <value>The state.</value>
            public States State { get; set; }

            /// <summary>
            /// Gets the message.
            /// </summary>
            /// <value>The message.</value>
            public abstract string Message { get; }
        }

        /// <summary>
        /// Record of a state entry.
        /// </summary>
        private class EntryRecord : Record
        {
            /// <summary>
            /// Gets the message.
            /// </summary>
            /// <value>The message.</value>
            public override string Message
            {
                get { return "State " + this.State + "not entered."; }
            }
        }

        /// <summary>
        /// Record of a state exit.
        /// </summary>
        private class ExitRecord : Record
        {
            /// <summary>
            /// Gets the message.
            /// </summary>
            /// <value>The message.</value>
            public override string Message
            {
                get { return "State " + this.State + "not exited."; }
            }
        }
    }
}