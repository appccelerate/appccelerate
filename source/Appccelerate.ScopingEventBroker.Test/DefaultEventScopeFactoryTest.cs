//-------------------------------------------------------------------------------
// <copyright file="DefaultEventScopeFactoryTest.cs" company="Appccelerate">
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

namespace Appccelerate.ScopingEventBroker
{
    using Appccelerate.EventBroker;

    using FakeItEasy;

    using FluentAssertions;

    using Xunit;

    public class DefaultEventScopeFactoryTest
    {
        private readonly DefaultEventScopeFactory eventScopeFactory;

        public DefaultEventScopeFactoryTest()
        {
            this.eventScopeFactory = new DefaultEventScopeFactory();
        }

        [Fact]
        public void CreateHandlerDecorator_ShouldCreateDefaultDecorator()
        {
            IHandler decorator = this.eventScopeFactory.CreateHandlerDecorator(A.Fake<IHandler>());

            decorator.Should().BeOfType<ScopingHandlerDecorator>();
        }

        [Fact]
        public void CreateScopeHolder_ShouldCreateDefaultPerCallScopeHolder()
        {
            IEventScopeHolder scopeHolder = this.eventScopeFactory.CreateScopeHolder();

            scopeHolder.Should().BeOfType<PerCallEventScopeContext>();
        }
    }
}