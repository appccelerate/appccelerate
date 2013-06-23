//-------------------------------------------------------------------------------
// <copyright file="EventScopeTest.cs" company="Appccelerate">
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

namespace Appccelerate.ScopingEventBroker.Internals
{
    using System;

    using FluentAssertions;

    using Xunit;

    public class EventScopeTest : IDisposable
    {
        private readonly EventScope testee;

        public EventScopeTest()
        {
            this.testee = new EventScope();
        }

        [Fact]
        public void Release_WhenNotCanceled_ShouldExecutedCallbacks()
        {
            bool wasCalled = false;
            this.testee.Register(() => wasCalled = true);

            this.testee.Release();

            wasCalled.Should().BeTrue();
        }

        [Fact]
        public void Release_WhenCanceled_ShouldNotExecuteCallbacks()
        {
            bool wasCalled = false;
            this.testee.Register(() => wasCalled = true);
            this.testee.Cancel();

            this.testee.Release();

            wasCalled.Should().BeFalse();
        }

        public void Dispose()
        {
            this.testee.Dispose();
        }
    }
}