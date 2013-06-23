//-------------------------------------------------------------------------------
// <copyright file="AbstractEventScopeContextTest.cs" company="Appccelerate">
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

namespace Appccelerate.ScopingEventBroker.Internals.Context
{
    using System;

    using FakeItEasy;

    using FluentAssertions;

    using Xunit;

    public class AbstractEventScopeContextTest
    {
        private readonly IEventScopeInternal eventScope;

        private readonly IEventScopeFactory eventScopeFactory;

        private readonly TestAbstractEventScopeContext testee;

        public AbstractEventScopeContextTest()
        {
            this.eventScope = A.Fake<IEventScopeInternal>();
            this.eventScopeFactory = A.Fake<IEventScopeFactory>();

            this.testee = new TestAbstractEventScopeContext(this.eventScopeFactory);
        }

        [Fact]
        public void Current_WhenConstructed_ShouldBeNull()
        {
            this.SetupFactoryCreatesScope();

            this.testee.Current.Should().BeNull();
        }

        [Fact]
        public void Acquire_ShouldSetCurrent()
        {
            this.SetupFactoryCreatesScope();

            this.testee.Acquire();

            this.testee.Current.Should().NotBeNull();
        }

        [Fact]
        public void DisposeCurrent_RemoveCurrent()
        {
            this.SetupFactoryCreatesScope();

            using (this.testee.Acquire())
            {
            }

            this.testee.Current.Should().BeNull();
        }

        [Fact]
        public void Dispose_ShouldDisposeInner()
        {
            this.SetupFactoryCreatesScope();

            using (this.testee.Acquire())
            {
            }

            A.CallTo(() => this.eventScope.Dispose()).MustHaveHappened();
        }

        [Fact]
        public void ReleaseOnAcquired_ShouldReleaseAcquired()
        {
            this.SetupFactoryCreatesScope();

            using (IEventScope scope = this.testee.Acquire())
            {
                scope.Release();
            }

            A.CallTo(() => this.eventScope.Release()).MustHaveHappened();
        }

        [Fact]
        public void CancelOnAcquired_ShouldCancelAcquired()
        {
            this.SetupFactoryCreatesScope();

            using (IEventScope scope = this.testee.Acquire())
            {
                scope.Cancel();
            }

            A.CallTo(() => this.eventScope.Cancel()).MustHaveHappened();
        }

        [Fact]
        public void RegisterOnAcquired_ShouldRegisterOnAcquired()
        {
            this.SetupFactoryCreatesScope();

            this.testee.Acquire();
            this.testee.Current.Register(() => { });

            A.CallTo(() => this.eventScope.Register(A<Action>.Ignored)).MustHaveHappened();
        }

        private void SetupFactoryCreatesScope()
        {
            A.CallTo(() => this.eventScopeFactory.CreateScope()).Returns(this.eventScope);
        }

        private class TestAbstractEventScopeContext : AbstractEventScopeContext
        {
            public TestAbstractEventScopeContext(IEventScopeFactory eventScopeFactory)
                : base(eventScopeFactory)
            {
            }

            protected override IEventScopeInternal CurrentScope
            {
                get;
                set;
            }
        }
    }
}