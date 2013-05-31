//-------------------------------------------------------------------------------
// <copyright file="PerThreadEventScopeContextTest.cs" company="Appccelerate">
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
    using System.Threading;

    using FakeItEasy;

    using FluentAssertions;

    using Xunit;

    public class PerThreadEventScopeContextTest : IDisposable
    {
        private readonly PerThreadEventScopeContext testee;

        public PerThreadEventScopeContextTest()
        {
            this.testee = new PerThreadEventScopeContext(A.Fake<IEventScopeFactory>());
        }

        [Fact]
        public void Acquire_WhenSameThread_ShouldReturnSame()
        {
            IEventScope firstScope = this.testee.Acquire();
            IEventScope secondScope = this.testee.Acquire();

            firstScope.Should().BeSameAs(secondScope);
        }

        [Fact]
        public void Acquire_WhenSameThreadAndDisposed_ShouldReturnNew()
        {
            IEventScope firstScope = this.testee.Acquire();
            firstScope.Dispose();

            using (IEventScope secondScope = this.testee.Acquire())
            {
                firstScope.Should().NotBeSameAs(secondScope);
            }
        }

        [Fact]
        public void Acquire_WhenDifferentThread_ReturnsNew()
        {
            IEventScope firstScopeTaskResult = null;
            IEventScope secondScopeTaskResult = null;

            var firstScopeTask = new Thread(() => firstScopeTaskResult = this.testee.Acquire());
            var secondScopeTask = new Thread(() => secondScopeTaskResult = this.testee.Acquire());

            firstScopeTask.Start();
            firstScopeTask.Join();

            secondScopeTask.Start();
            secondScopeTask.Join();

            using (IEventScope firstScope = firstScopeTaskResult)
            using (IEventScope secondScope = secondScopeTaskResult)
            {
                firstScope.Should().NotBeSameAs(secondScope);
            }
        }

        [Fact]
        public void Dispose_WhenDisposeWasAlreadyCalled_ShouldNotThrow()
        {
            this.testee.Dispose();

            this.testee.Invoking(x => x.Dispose())
                .ShouldNotThrow();
        }

        public void Dispose()
        {
            this.testee.Dispose();
        }
    }
}