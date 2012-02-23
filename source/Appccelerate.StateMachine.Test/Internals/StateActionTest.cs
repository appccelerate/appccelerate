//-------------------------------------------------------------------------------
// <copyright file="StateActionTest.cs" company="Appccelerate">
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
    using FluentAssertions;

    using Xunit;

    /// <summary>
    /// Tests that entry and exit actions are executed correctly.
    /// </summary>
    public class StateActionTest
    {
        private readonly StateMachine<States, Events> testee;

        public StateActionTest()
        {
            this.testee = new StateMachine<States, Events>();
        }

        [Fact]
        public void EntryAction()
        {
            bool entered = false;

            this.testee.In(States.A)
                .ExecuteOnEntry(() => entered = true);

            this.testee.Initialize(States.A);

            this.testee.EnterInitialState();

            Assert.True(entered, "entry action was not executed.");
        }

        [Fact]
        public void EntryActions()
        {
            bool entered1 = false;
            bool entered2 = false;

            this.testee.In(States.A)
                .ExecuteOnEntry(
                    () => entered1 = true,
                    () => entered2 = true);

            this.testee.Initialize(States.A);

            this.testee.EnterInitialState();

            entered1.Should().BeTrue("entry action was not executed.");
            entered2.Should().BeTrue("entry action was not executed.");
        }

        [Fact]
        public void ParameterizedEntryAction()
        {
            int i = 0;

            this.testee.In(States.A)
                .ExecuteOnEntry(parameter => i = parameter, 3);

            this.testee.Initialize(States.A);

            this.testee.EnterInitialState();

            Assert.Equal(3, i);
        }

        [Fact]
        public void ExitAction()
        {
            bool exit = false;

            this.testee.In(States.A)
                .ExecuteOnExit(() => exit = true)
                .On(Events.B).Goto(States.B);

            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(Events.B);

            Assert.True(exit, "exit action was not executed.");
        }

        [Fact]
        public void ExitActions()
        {
            bool exit1 = false;
            bool exit2 = false;

            this.testee.In(States.A)
                .ExecuteOnExit(
                    () => exit1 = true,
                    () => exit2 = true)
                .On(Events.B).Goto(States.B);

            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(Events.B);

            exit1.Should().BeTrue("exit action was not executed.");
            exit2.Should().BeTrue("exit action was not executed.");
        }

        [Fact]
        public void ParametrizedExitAction()
        {
            int i = 0;

            this.testee.In(States.A)
                .ExecuteOnExit(value => i = value, 3)
                .On(Events.B).Goto(States.B);

            this.testee.Initialize(States.A);
            this.testee.EnterInitialState();

            this.testee.Fire(Events.B);

            Assert.Equal(i, 3);
        }
    }
}