//-------------------------------------------------------------------------------
// <copyright file="EventScopingStandardFactoryTest.cs" company="Appccelerate">
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
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Appccelerate.EventBroker;
    using Appccelerate.EventBroker.Handlers;

    using FluentAssertions;

    using Xunit.Extensions;

    public class EventScopingStandardFactoryTest
    {
        private const bool Decorated = true;
        private const bool NotDecorated = false;

        private readonly EventScopingStandardFactory testee;

        public EventScopingStandardFactoryTest()
        {
            this.testee = new EventScopingStandardFactory(new FakeDecoratorCreatingEventScopeFactory());
        }

        [Theory]
        [InlineData(typeof(OnBackground), Decorated)]
        [InlineData(typeof(OnUserInterface), NotDecorated)]
        [InlineData(typeof(OnUserInterfaceAsync), Decorated)]
        [InlineData(typeof(OnPublisher), NotDecorated)]
        public void CreateHandler_ShouldDecorateWhenHandlerAsynchronous(Type handlerType, bool decorated)
        {               
            IHandler handler = this.testee.CreateHandler(handlerType);

            ShouldBe(handler, decorated, handlerType);
        }

        private static void ShouldBe(IHandler handler, bool decorated, Type handlerType)
        {
            var assertions = new Dictionary<bool, Action<IHandler>>
            {
                { true, h => h.Should().BeOfType<FakeDecorator>()
                    .And.Subject.As<FakeDecorator>().DecoratedHandlerType
                        .Should().Be(handlerType) }, 
                { false, h => h.GetType().Should().Be(handlerType) }, 
            };

            assertions[decorated](handler);
        }

        private class FakeDecoratorCreatingEventScopeFactory : IEventScopeFactory
        {
            public IEventScopeInternal CreateScope()
            {
                throw new NotImplementedException();
            }

            public IEventScopeContext CreateScopeContext()
            {
                throw new NotImplementedException();
            }

            public IEventScopeHolder CreateScopeHolder()
            {
                throw new NotImplementedException();
            }

            public IHandler CreateHandlerDecorator(IHandler handler)
            {
                return new FakeDecorator(handler);
            }
        }

        private class FakeDecorator : IHandler
        {
            private readonly IHandler inner;

            public FakeDecorator(IHandler inner)
            {
                this.inner = inner;
            }

            public HandlerKind Kind
            {
                get { return HandlerKind.Synchronous; }
            }

            public Type DecoratedHandlerType
            {
                get
                {
                    return this.inner.GetType();
                }
            }

            public void Initialize(object subscriber, MethodInfo handlerMethod, IExtensionHost extensionHost)
            {
            }

            public void Handle(IEventTopic eventTopic, object sender, EventArgs e, Delegate subscriptionHandler)
            {
            }
        }
    }
}