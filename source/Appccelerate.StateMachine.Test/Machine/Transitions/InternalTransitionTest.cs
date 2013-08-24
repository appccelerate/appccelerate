﻿//-------------------------------------------------------------------------------
// <copyright file="InternalTransitionTest.cs" company="Appccelerate">
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
    using FakeItEasy;
    using Xunit;

    public class InternalTransitionTest : SuccessfulTransitionWithExecutedActionsTestBase
    {
        public InternalTransitionTest()
        {
            this.Source = Builder<States, Events>.CreateState().Build();
            this.Target = this.Source;
            this.TransitionContext = Builder<States, Events>.CreateTransitionContext().WithState(this.Source).Build();

            this.Testee.Source = this.Source;
            this.Testee.Target = null; // target == null indicates an internal transition
        }

        [Fact]
        public void DoesNotExitState()
        {
            this.Testee.Fire(this.TransitionContext);

            A.CallTo(() => this.Source.Exit(this.TransitionContext)).MustNotHaveHappened();
        }

        [Fact]
        public void DoesNotEnterState()
        {
            this.Testee.Fire(this.TransitionContext);

            A.CallTo(() => this.Source.Entry(this.TransitionContext)).MustNotHaveHappened();
        }
    }
}