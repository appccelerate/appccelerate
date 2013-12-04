﻿//-------------------------------------------------------------------------------
// <copyright file="GuardsTransitionTest.cs" company="Appccelerate">
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
    using Appccelerate.StateMachine.Machine.GuardHolders;
    using FakeItEasy;
    using FluentAssertions;
    using Xunit;

    public class GuardsTransitionTest : TransitionTestBase
    {
        public GuardsTransitionTest()
        {
            this.Source = Builder<States, Events>.CreateState().Build();
            this.Target = Builder<States, Events>.CreateState().Build();
            this.TransitionContext = Builder<States, Events>.CreateTransitionContext().WithState(this.Source).Build();

            this.Testee.Source = this.Source;
            this.Testee.Target = this.Target;
        }

        [Fact]
        public void Executes_WhenGuardIsMet()
        {
            var guard = Builder<States, Events>.CreateGuardHolder().ReturningTrue().Build();
            this.Testee.Guard = guard;

            this.Testee.Fire(this.TransitionContext);

            A.CallTo(() => this.Target.Entry(this.TransitionContext)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void DoesNotExecute_WhenGuardIsNotMet()
        {
            var guard = Builder<States, Events>.CreateGuardHolder().ReturningFalse().Build();
            this.Testee.Guard = guard;

            this.Testee.Fire(this.TransitionContext);

            A.CallTo(() => this.Target.Entry(this.TransitionContext)).MustNotHaveHappened();
        }

        [Fact]
        public void ReturnsNotFiredTransitionResult_WhenGuardIsNotMet()
        {
            var guard = Builder<States, Events>.CreateGuardHolder().ReturningFalse().Build();
            this.Testee.Guard = guard;

            ITransitionResult<States, Events> result = this.Testee.Fire(this.TransitionContext);

            result.Should().BeNotFiredTransitionResult<States, Events>();
        }

        [Fact]
        public void NotifiesExtensions_WhenGuardIsNotMet()
        {
            var extension = A.Fake<IExtension<States, Events>>();
            this.ExtensionHost.Extension = extension;

            IGuardHolder guard = Builder<States, Events>.CreateGuardHolder().ReturningFalse().Build();
            this.Testee.Guard = guard;

            ITransitionResult<States, Events> result = this.Testee.Fire(this.TransitionContext);

            A.CallTo(() => extension.SkippedTransition(
                this.StateMachineInformation,
                A<ITransition<States, Events>>.That.Matches(t => t.Source == this.Source && t.Target == this.Target),
                this.TransitionContext)).MustHaveHappened();
        }
    }
}