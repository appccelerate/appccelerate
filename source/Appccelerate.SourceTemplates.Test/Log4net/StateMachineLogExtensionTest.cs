//-------------------------------------------------------------------------------
// <copyright file="StateMachineLogExtensionTest.cs" company="Appccelerate">
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

namespace Appccelerate.SourceTemplates.Log4Net
{
    using System;
    using System.Reflection;

    using Appccelerate.StateMachine;
    using Appccelerate.StateMachine.Machine;
    using Appccelerate.StateMachine.Machine.Contexts;

    using FakeItEasy;

    using log4net.Core;

    using Xunit;

    public class StateMachineLogExtensionTest : IDisposable
    {
        private readonly StateMachineLogExtension<States, Events> testee;

        private readonly Log4netHelper log4Net;

        public StateMachineLogExtensionTest()
        {
            this.testee = new StateMachineLogExtension<States, Events>();

            this.log4Net = new Log4netHelper();
        }

        public enum States
        {
            A,
            B,
        }

        public enum Events
        {
            A,
            B,
        }

        public void Dispose()
        {
            this.log4Net.Dispose();
        }

        [Fact]
        public void LogsFiredEvent()
        {
            const string StateMachineName = "test machine";
            const string Records = "test records";

            var stateMachineInformation = this.CreateStateMachineInformation(StateMachineName, States.B);

            var transitionContext = A.Fake<ITransitionContext<States, Events>>();
            A.CallTo(() => transitionContext.GetRecords()).Returns(Records);

            this.testee.FiredEvent(stateMachineInformation, transitionContext);

            this.log4Net.LogContains(string.Format("State machine {0} performed {1}.", StateMachineName, Records));
        }

        [Fact]
        public void LogsFiringEvent()
        {
            const string StateMachineName = "test machine";
            const States CurrentStateId = States.A;
            Events eventId = Events.A;
            object eventArgument = "A";

            var stateMachineInformation = this.CreateStateMachineInformation(StateMachineName, CurrentStateId);

            this.testee.FiringEvent(stateMachineInformation, ref eventId, ref eventArgument);

            this.log4Net.LogContains(
                string.Format(
                    "Fire event {0} on state machine {1} with current state {2} and event argument {3}.",
                    eventId,
                    StateMachineName,
                    CurrentStateId,
                    eventArgument));
        }

        [Fact]
        public void LogsHandlingEntryActionException()
        {
            const string StateMachineName = "test machine";
            const States CurrentStateId = States.A;
            const string ExceptionMessage = "test exception";
            Exception exception = new Exception(ExceptionMessage);

            var stateMachineInformationMock = this.CreateStateMachineInformation(StateMachineName, CurrentStateId);
            var stateMock = this.CreateState(CurrentStateId);
            var context = new TransitionContext<States, Events>(stateMock, new Missable<Events>(), null, null);

            this.testee.HandlingEntryActionException(stateMachineInformationMock, stateMock, context, ref exception);

            this.log4Net.LogContains(
                Level.Error,
                "Exception in entry action of state A of state machine test machine: System.Exception: " + ExceptionMessage);
        }

        [Fact]
        public void LogsHandlingExitActionException()
        {
            const string StateMachineName = "test machine";
            const States CurrentStateId = States.A;
            var stateMachineInformationMock = this.CreateStateMachineInformation(StateMachineName, CurrentStateId);
            var stateMock = this.CreateState(CurrentStateId);
            var context = new TransitionContext<States, Events>(stateMock, new Missable<Events>(), null, null);
            var exception = new Exception("test exception");

            this.testee.HandlingExitActionException(stateMachineInformationMock, stateMock, context, ref exception);

            this.log4Net.LogContains(
                Level.Error,
                "Exception in exit action of state A of state machine test machine: System.Exception: test exception");
        }

        [Fact]
        public void LogsHandlingGuardException()
        {
            const string StateMachineName = "test machine";
            const States CurrentStateId = States.A;
            var stateMachineInformationMock = this.CreateStateMachineInformation(StateMachineName, CurrentStateId);
            var transitionMock = A.Fake<ITransition<States, Events>>();
            var stateMock = this.CreateState(CurrentStateId);
            var transitionContext = new TransitionContext<States, Events>(stateMock, new Missable<Events>(Events.B), null, null);
            var exception = new Exception("test exception");

            this.testee.HandlingGuardException(stateMachineInformationMock, transitionMock, transitionContext, ref exception);

            this.log4Net.LogMatch(
                Level.Error,
                "Exception in guard of transition .* of state machine test machine: System.Exception: test exception");
        }

        [Fact]
        public void LogsHandlingTransitionException()
        {
            const string StateMachineName = "test machine";
            const States CurrentStateId = States.A;
            var stateMachineInformationMock = this.CreateStateMachineInformation(StateMachineName, CurrentStateId);
            var transitionMock = A.Fake<ITransition<States, Events>>();
            var stateMock = this.CreateState(CurrentStateId);
            var transitionContext = new TransitionContext<States, Events>(stateMock, new Missable<Events>(Events.B), null, null);
            var exception = new Exception("test exception");

            this.testee.HandlingTransitionException(stateMachineInformationMock, transitionMock, transitionContext, ref exception);

            this.log4Net.LogMatch(
                Level.Error,
                "Exception in action of transition .* of state machine test machine: System.Exception: test exception");
        }

        private IStateMachineInformation<States, Events> CreateStateMachineInformation(string stateMachineName, States currentStateId)
        {
            var stateMachineInformation = A.Fake<IStateMachineInformation<States, Events>>();
            A.CallTo(() => stateMachineInformation.Name).Returns(stateMachineName);
            A.CallTo(() => stateMachineInformation.CurrentStateId).Returns(currentStateId);

            return stateMachineInformation;
        }

        private IState<States, Events> CreateState(States stateId)
        {
            var state = A.Fake<IState<States, Events>>();
            A.CallTo(() => state.Id).Returns(stateId);

            return state;
        }
    }
}