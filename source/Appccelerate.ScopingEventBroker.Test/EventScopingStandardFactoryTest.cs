//-------------------------------------------------------------------------------
// <copyright file="EventScopingStandardFactoryTest.cs" company="Appccelerate">
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

namespace Appccelerate.ScopingEventBroker
{
    using System;
    using System.Reflection;
    using Appccelerate.EventBroker;
    using Appccelerate.EventBroker.Handlers;
    using Appccelerate.EventBroker.Internals.Subscriptions;
    using Appccelerate.ScopingEventBroker.Internals;

    using FakeItEasy;

    using FluentAssertions;

    using Xunit;
    using Xunit.Extensions;

    public class EventScopingStandardFactoryTest
    {
        [Fact]
        public void CreateScopeContext_ShouldDelegateToDecorated()
        {
            var scopeContext = A.Fake<IEventScopeContext>();
            var eventScopeFactory = A.Fake<IEventScopeFactory>();
            A.CallTo(() => eventScopeFactory.CreateScopeContext()).Returns(scopeContext);

            var testee = new EventScopingStandardFactory(eventScopeFactory);

            var result = testee.CreateScopeContext();

            result.Should().BeSameAs(scopeContext);
        }

        [Theory]
        [InlineData(typeof(OnBackground))]
        [InlineData(typeof(OnUserInterfaceAsync))]
        public void DecoratesAsynchronousHandlerWithScopingHandler(Type handlerType)
        {
            var testee = new EventScopingStandardFactory(new FakeDecoratorCreatingEventScopeFactory());

            IHandler handler = testee.CreateHandler(handlerType);

            handler.Should()
                   .BeOfType<FakeDecorator>()
                   .And.Subject.As<FakeDecorator>()
                   .DecoratedHandlerType.Should()
                   .Be(handlerType);
        }

        [Theory]
        [InlineData(typeof(OnUserInterface))]
        [InlineData(typeof(OnPublisher))]
        public void CreatesStandardSynchronousHandlers(Type handlerType)
        {
            var testee = new EventScopingStandardFactory(new FakeDecoratorCreatingEventScopeFactory());

            IHandler handler = testee.CreateHandler(handlerType);

            handler.GetType().Should().Be(handlerType);
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

            public void Handle(IEventTopicInfo eventTopic, object subscriber, object sender, EventArgs e, IDelegateWrapper delegateWrapper)
            {
            }
        }
    }
}