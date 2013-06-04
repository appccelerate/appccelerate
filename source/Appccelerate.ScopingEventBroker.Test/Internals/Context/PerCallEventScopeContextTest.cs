//-------------------------------------------------------------------------------
// <copyright file="PerCallEventScopeContextTest.cs" company="Appccelerate">
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
    using FakeItEasy;

    using FluentAssertions;

    using Xunit;

    public class PerCallEventScopeContextTest
    {
        private readonly PerCallEventScopeContext testee;

        public PerCallEventScopeContextTest()
        {
            this.testee = new PerCallEventScopeContext(A.Fake<IEventScopeFactory>());
        }

        [Fact]
        public void Acquire_WhenAcquiredWithoutDispose_ShouldReturnSame()
        {
            IEventScope firstScope = this.testee.Acquire();
            IEventScope secondScope = this.testee.Acquire();

            firstScope.Should().BeSameAs(secondScope);
        }

        [Fact]
        public void Acquire_WhenAcquiredWithDispose_ShouldReturnNew()
        {
            IEventScope firstScope = this.testee.Acquire();
            firstScope.Dispose();

            using (IEventScope secondScope = this.testee.Acquire())
            {
                firstScope.Should().NotBeSameAs(secondScope);
            }
        }
    }
}