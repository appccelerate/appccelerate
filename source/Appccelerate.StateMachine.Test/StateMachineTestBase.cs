//-------------------------------------------------------------------------------
// <copyright file="StateMachineTestBase.cs" company="Appccelerate">
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

namespace Appccelerate.StateMachine
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using Appccelerate.StateMachine.Machine;
    using Appccelerate.StateMachine.Machine.Events;

    using FluentAssertions;

    using Xunit;

    /// <summary>
    /// Base for state machine test fixtures.
    /// </summary>
    /// <typeparam name="TStateMachine">The type of the state machine.</typeparam>
    public abstract class StateMachineTestBase<TStateMachine> : IDisposable
        where TStateMachine : IStateMachine<States, Events>
    {
        /// <summary>
        /// Object under test.
        /// </summary>
        private readonly TStateMachine testee;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachineTestBase&lt;TStateMachine&gt;"/> class.
        /// </summary>
        /// <param name="testee">The subject under test.</param>
        protected StateMachineTestBase(TStateMachine testee)
        {
            this.testee = testee;
            
            this.Exceptions = new List<EventArgs>();
            this.TransitionBeginMessages = new List<TransitionEventArgs<States, Events>>();
            this.TransitionCompletedMessages = new List<TransitionCompletedEventArgs<States, Events>>();
            this.TransitionDeclinedMessages = new List<TransitionEventArgs<States, Events>>();

            this.testee.TransitionExceptionThrown += (sender, e) => this.Exceptions.Add(e);
            this.testee.TransitionBegin += (sender, e) => this.TransitionBeginMessages.Add(e);
            this.testee.TransitionCompleted += (sender, e) => this.TransitionCompletedMessages.Add(e);
            this.testee.TransitionDeclined += (sender, e) => this.TransitionDeclinedMessages.Add(e);
        }

        /// <summary>
        /// Gets the exceptions that were notified.
        /// </summary>
        /// <value>The exceptions.</value>
        protected List<EventArgs> Exceptions { get; private set; }

        /// <summary>
        /// Gets the begin transition messages that were notified.
        /// </summary>
        protected List<TransitionEventArgs<States, Events>> TransitionBeginMessages { get; private set; }

        /// <summary>
        /// Gets the  transition completed messages that were notified.
        /// </summary>
        protected List<TransitionCompletedEventArgs<States, Events>> TransitionCompletedMessages { get; private set; }

        /// <summary>
        /// Gets the transition declined messages that were notified.
        /// </summary>
        protected List<TransitionEventArgs<States, Events>> TransitionDeclinedMessages { get; private set; }

        public void Dispose()
        {
            this.testee.Stop();
        }

        [Fact]
        public void InitializeWhenNotStartedThenNoStateIsEntered()
        {
            var enteredState = false;
            this.testee
                .In(States.A)
                .ExecuteOnEntry(() => enteredState = true);

            this.testee.Initialize(States.A);

            enteredState
                .Should().BeFalse();
        }

        [Fact]
        public void InitializeWhenStartedThenInitialStateIsEntered()
        {
            var enteredStateSignal = new AutoResetEvent(false);

            this.testee
                .In(States.A)
                .ExecuteOnEntry(() => enteredStateSignal.Set());

            this.testee.Initialize(States.A);
            this.testee.Start();

            enteredStateSignal.WaitOne(1000)
                .Should().BeTrue();
        }

        [Fact]
        public void InitializeWhenInitializedTwiceThenInvalidOperationException()
        {
            this.testee.Initialize(States.A);
            Action action = () => this.testee.Initialize(States.A);

            action.ShouldThrow<InvalidOperationException>().WithMessage(ExceptionMessages.StateMachineIsAlreadyInitialized);
        }

        [Fact]
        public void InitializeWhenStartingAnUninitializedStateMachineThenInvalidOperationException()
        {
            Action action = () => this.testee.Start();

            action
                .ShouldThrow<InvalidOperationException>();
        }

        /// <summary>
        /// The <see cref="IStateMachine{TState,TEvent}.IsRunning"/> reflects the state of the state machine.
        /// </summary>
        [Fact]
        public void StartStop()
        {
            this.testee.Initialize(States.A);

            bool runningInitially = this.testee.IsRunning;
            this.testee.Start();
            bool runningAfterStart = this.testee.IsRunning;
            this.testee.Stop();
            bool runningAfterStop = this.testee.IsRunning;

            Assert.False(runningInitially, "initially");
            Assert.True(runningAfterStart, "after start");
            Assert.False(runningAfterStop, "after stop");
        }

        /// <summary>
        /// An event can be fired onto the state machine and all notifications are signaled.
        /// </summary>
        [Fact]
        public void FireEvent()
        {
            AutoResetEvent allTransitionsCompleted = this.SetUpWaitForAllTransitions(1);

            this.testee.DefineHierarchyOn(StateMachine.States.B)
                .WithHistoryType(HistoryType.None)
                .WithInitialSubState(StateMachine.States.B1)
                .WithSubState(StateMachine.States.B2);

            this.testee.DefineHierarchyOn(StateMachine.States.C)
                .WithHistoryType(HistoryType.Shallow)
                .WithInitialSubState(StateMachine.States.C2)
                .WithSubState(StateMachine.States.C1);

            this.testee.DefineHierarchyOn(StateMachine.States.C1)
                .WithHistoryType(HistoryType.Shallow)
                .WithInitialSubState(StateMachine.States.C1A)
                .WithSubState(StateMachine.States.C1B);

            this.testee.DefineHierarchyOn(StateMachine.States.D)
                .WithHistoryType(HistoryType.Deep)
                .WithInitialSubState(StateMachine.States.D1)
                .WithSubState(StateMachine.States.D2);

            this.testee.DefineHierarchyOn(StateMachine.States.D1)
                .WithHistoryType(HistoryType.Deep)
                .WithInitialSubState(StateMachine.States.D1A)
                .WithSubState(StateMachine.States.D1B);

            this.testee.In(States.A)
                .On(Events.B).Goto(States.B);

            object eventArgument = "test";

            this.testee.Initialize(States.A);
            this.testee.Start();

            this.testee.Fire(Events.B, eventArgument);

            this.WaitForAllTransitions(allTransitionsCompleted);

            this.CheckBeginTransitionMessage(States.A, Events.B, eventArgument);
            this.CheckTransitionCompletedMessage(eventArgument, States.A, Events.B, States.B1);
            this.CheckNoExceptionMessage();
            this.CheckNoDeclinedTransitionMessage();
        }

        /// <summary>
        /// With FirePriority, an event can be added to the front of the queued events.
        /// </summary>
        [Fact]
        public void PriorityFire()
        {
            const int Transitions = 3;
            AutoResetEvent allTransitionsCompleted = this.SetUpWaitForAllTransitions(Transitions);

            this.testee.In(States.A)
                .On(Events.B).Goto(States.B).Execute(() =>
                {
                    this.testee.Fire(Events.D);
                    this.testee.FirePriority(Events.C);
                });

            this.testee.TransitionCompleted += (s, e) => Console.WriteLine("completed " + e.StateId + " to " + e.NewStateId);
            this.testee.TransitionExceptionThrown += (s, e) => Console.WriteLine("Exception: " + e.Exception);

            this.testee.In(States.B)
                .On(Events.C).Goto(States.C);

            this.testee.In(States.C)
                .On(Events.D).Goto(States.D);

            this.testee.Initialize(States.A);
            this.testee.Start();

            this.testee.Fire(Events.B);

            this.WaitForAllTransitions(allTransitionsCompleted);

            Assert.Equal(Transitions, this.TransitionCompletedMessages.Count);
            this.CheckNoDeclinedTransitionMessage();
            this.CheckNoExceptionMessage();
        }

        /// <summary>
        /// When the state machine is stopped then no events are processed.
        /// All events queued are processed when state machine is started.
        /// </summary>
        [Fact]
        public void StopAndRestart()
        {
            const int Transitions = 2;
            AutoResetEvent allTransitionsCompleted = this.SetUpWaitForAllTransitions(Transitions);

            this.testee.In(States.A)
                .On(Events.B).Goto(States.B);

            this.testee.In(States.B)
                .On(Events.C).Goto(States.C);

            this.testee.Initialize(States.A);
            this.testee.Start();

            this.testee.Stop();

            Assert.False(this.testee.IsRunning, "after stop, state machine should not be running.");

            this.testee.Fire(Events.B);
            this.testee.Fire(Events.C);

            Assert.Equal(0, this.TransitionBeginMessages.Count);

            this.testee.Start();

            this.WaitForAllTransitions(allTransitionsCompleted);

            Assert.Equal(Transitions, this.TransitionCompletedMessages.Count);
        }

        /// <summary>
        /// The state machine can be started twice with no effect.
        /// </summary>
        [Fact]
        public void StartTwice()
        {
            this.testee.Initialize(States.A);

            this.testee.Start();
            this.testee.Start();

            Assert.True(this.testee.IsRunning);
        }

        /// <summary>
        /// Checks the no declined transition message occurred.
        /// </summary>
        private void CheckNoDeclinedTransitionMessage()
        {
            Assert.Empty(this.TransitionDeclinedMessages);
        }

        /// <summary>
        /// Checks the no exception message occurred.
        /// </summary>
        private void CheckNoExceptionMessage()
        {
            Assert.Empty(this.Exceptions);
        }

        /// <summary>
        /// Checks the transition completed message.
        /// </summary>
        /// <param name="eventArgument">The event argument.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="newState">The new state.</param>
        private void CheckTransitionCompletedMessage(object eventArgument, States origin, Events eventId, States newState)
        {
            Assert.Equal(1, this.TransitionCompletedMessages.Count);
            Assert.Equal(origin, this.TransitionCompletedMessages[0].StateId);
            Assert.Equal(eventId, this.TransitionCompletedMessages[0].EventId);
            if (eventArgument != null)
            {
                Assert.Equal(eventArgument, this.TransitionCompletedMessages[0].EventArgument);
            }

            Assert.Equal(newState, this.TransitionCompletedMessages[0].NewStateId);
        }

        /// <summary>
        /// Checks the begin transition message.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="eventArgument">The event argument.</param>
        private void CheckBeginTransitionMessage(States origin, Events eventId, object eventArgument)
        {
            Assert.Equal(1, this.TransitionBeginMessages.Count);
            Assert.Equal(origin, this.TransitionBeginMessages[0].StateId);
            Assert.Equal(eventId, this.TransitionBeginMessages[0].EventId);
            Assert.Equal(eventArgument, this.TransitionBeginMessages[0].EventArgument);
        }

        /// <summary>
        /// Sets up the wait event that all transitions were completed.
        /// </summary>
        /// <param name="numberOfTransitionCompletedMessages">The number of transition completed messages.</param>
        /// <returns>Event that is signaled when <paramref name="numberOfTransitionCompletedMessages"/> transition completed messages were received.</returns>
        private AutoResetEvent SetUpWaitForAllTransitions(int numberOfTransitionCompletedMessages)
        {
            int numberOfTransitionCompletedMessagesReceived = 0;
            AutoResetEvent allTransitionsCompleted = new AutoResetEvent(false);
            this.testee.TransitionCompleted += (sender, e) =>
                                                   {
                                                       numberOfTransitionCompletedMessagesReceived++;
                                                       if (numberOfTransitionCompletedMessagesReceived == numberOfTransitionCompletedMessages)
                                                       {
                                                           allTransitionsCompleted.Set();
                                                       }
                                                   };

            return allTransitionsCompleted;
        }

        /// <summary>
        /// Waits for the event that was set-up with <see cref="SetUpWaitForAllTransitions"/> that all transitions were completed.
        /// </summary>
        /// <param name="allTransitionsCompleted">All transitions completed.</param>
        private void WaitForAllTransitions(AutoResetEvent allTransitionsCompleted)
        {
            Assert.True(allTransitionsCompleted.WaitOne(1000), "not enough transition completed events received within time-out.");
        }
    }
}