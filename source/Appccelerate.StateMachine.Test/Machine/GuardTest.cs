//-------------------------------------------------------------------------------
// <copyright file="GuardTest.cs" company="Appccelerate">
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

namespace Appccelerate.StateMachine.Machine
{
    using FluentAssertions;

    using Xunit;

    /// <summary>
    /// Tests the guard feature of the <see cref="StateMachine"/>.
    /// </summary>
    public class GuardTest
    {
        /// <summary>
        /// Object under test.
        /// </summary>
        private readonly StateMachine<StateMachine.States, StateMachine.Events> testee;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuardTest"/> class.
        /// </summary>
        public GuardTest()
        {
            this.testee = new StateMachine<StateMachine.States, StateMachine.Events>();
        }

        /// <summary>
        /// Only the transition with a guard returning true is executed and the event arguments are passed to the guard.
        /// </summary>
        [Fact]
        public void TransitionWithGuardReturningTrueIsExecuted()
        {
            this.testee.In(StateMachine.States.A)
                .On(StateMachine.Events.A)
                    .If(() => false).Goto(StateMachine.States.B)
                    .If(() => true).Goto(StateMachine.States.C)
                    .If(() => false).Goto(StateMachine.States.D);

            this.testee.Initialize(StateMachine.States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(StateMachine.Events.A);

            Assert.Equal(StateMachine.States.C, this.testee.CurrentStateId);
        }

        [Fact]
        public void OtherwiseIsExecutedWhenNoOtherGuardReturnsTrue()
        {
            this.testee.In(StateMachine.States.A)
                .On(StateMachine.Events.A)
                    .If(() => false).Goto(StateMachine.States.B)
                    .Otherwise().Goto(StateMachine.States.C);

            this.testee.Initialize(StateMachine.States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(StateMachine.Events.A);

            Assert.Equal(StateMachine.States.C, this.testee.CurrentStateId);
        }

        [Fact]
        public void EventArgumentIsPassedToTheGuard()
        {
            const string OriginalEventArgument = "test";

            string eventArgument = null;

            this.testee.In(StateMachine.States.A)
                .On(StateMachine.Events.A)
                    .If<string>(argument =>
                        {
                            eventArgument = argument;
                            return true;
                        })
                    .Goto(StateMachine.States.B);

            this.testee.Initialize(StateMachine.States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(StateMachine.Events.A, OriginalEventArgument);

            eventArgument.Should().Be(OriginalEventArgument);
        }

        /// <summary>
        /// When all guards deny the execution of its transition then ???
        /// </summary>
        [Fact]
        public void AllGuardsReturnFalse()
        {
            bool transitionDeclined = false;

            this.testee.TransitionDeclined += (sender, e) => { transitionDeclined = true; };

            this.testee.In(StateMachine.States.A)
                .On(StateMachine.Events.A).If(() => false).Goto(StateMachine.States.B)
                .On(StateMachine.Events.A).If(() => false).Goto(StateMachine.States.C);

            this.testee.Initialize(StateMachine.States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(StateMachine.Events.A);

            Assert.Equal(StateMachine.States.A, this.testee.CurrentStateId);
            Assert.True(transitionDeclined, "transition was not declined.");
        }

        [Fact]
        public void GuardWithoutArguments()
        {
            this.testee.In(StateMachine.States.A)
                .On(StateMachine.Events.B)
                    .If(() => false).Goto(StateMachine.States.C)
                    .If(() => true).Goto(StateMachine.States.B);

            this.testee.Initialize(StateMachine.States.A);
            this.testee.EnterInitialState();
            this.testee.Fire(StateMachine.Events.B);

            Assert.Equal(StateMachine.States.B, this.testee.CurrentStateId);
        }

        [Fact]
        public void GuardWithASingleArgument()
        {
            this.testee.In(StateMachine.States.A)
                .On(StateMachine.Events.B)
                    .If<int>(SingleIntArgumentGuardReturningFalse).Goto(StateMachine.States.C)
                    .If(() => false).Goto(StateMachine.States.D)
                    .If(() => false).Goto(StateMachine.States.E)
                    .If<int>(SingleIntArgumentGuardReturningTrue).Goto(StateMachine.States.B);

            this.testee.Initialize(StateMachine.States.A);
            this.testee.EnterInitialState();
            this.testee.Fire(StateMachine.Events.B, 3);

            Assert.Equal(StateMachine.States.B, this.testee.CurrentStateId);
        }

        private static bool SingleIntArgumentGuardReturningTrue(int i)
        {
            return true;
        }

        private static bool SingleIntArgumentGuardReturningFalse(int i)
        {
            return false;
        }
    }
}