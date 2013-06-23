//-------------------------------------------------------------------------------
// <copyright file="ScopingHandlerDecoratorTest.cs" company="Appccelerate">
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
    using System.Reflection;

    using Appccelerate.EventBroker;
    using Appccelerate.EventBroker.Internals.Subscriptions;

    using FakeItEasy;

    using FluentAssertions;

    using Xunit;

    public class ScopingHandlerDecoratorTest
    {
        private readonly IHandler handler;
        private readonly IEventScopeHolder scopeHolder;

        private readonly ScopingHandlerDecorator testee;

        public ScopingHandlerDecoratorTest()
        {
            this.handler = A.Fake<IHandler>();
            this.scopeHolder = A.Fake<IEventScopeHolder>();

            this.testee = new ScopingHandlerDecorator(this.handler, this.scopeHolder);
        }

        [Fact]
        public void Initialize_MustInitializeDecoratedHandler()
        {
            var methodInfo = MethodBase.GetCurrentMethod() as MethodInfo;
            var subscriber = new object();
            var extensionHost = A.Fake<IExtensionHost>();

            this.testee.Initialize(subscriber, methodInfo, extensionHost);

            A.CallTo(() => this.handler.Initialize(subscriber, methodInfo, extensionHost)).MustHaveHappened();
        }

        [Fact]
        public void Kind_MustReturnDecoratedKind()
        {
            const HandlerKind ExpectedHandlerKind = HandlerKind.Asynchronous;

            A.CallTo(() => this.handler.Kind).Returns(ExpectedHandlerKind);

            this.testee.Kind.Should().Be(ExpectedHandlerKind);
        }

        [Fact]
        public void Handle_WhenNotScopeAvailable_MustExecuteHandler()
        {
            var eventTopic = A.Fake<IEventTopic>();
            var delegateWrapper = A.Fake<IDelegateWrapper>();
            A.CallTo(() => this.scopeHolder.Current).Returns(null);

            this.testee.Handle(eventTopic, null, null, EventArgs.Empty, delegateWrapper);

            A.CallTo(() => this.handler.Handle(eventTopic, null, null, EventArgs.Empty, delegateWrapper)).MustHaveHappened();
        }

        [Fact]
        public void Handle_WhenScopeAvailable_MustNotExecuteHandler()
        {
            A.CallTo(() => this.scopeHolder.Current).Returns(A.Fake<IEventScopeInternal>());

            this.testee.Handle(A.Fake<IEventTopic>(), null, null, EventArgs.Empty,  A.Fake<IDelegateWrapper>());

            A.CallTo(() => this.handler.Handle(A<IEventTopic>.Ignored, A<object>.Ignored, A<object>.Ignored, A<EventArgs>.Ignored, A<IDelegateWrapper>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public void Handle_WhenScopeAvailable_MustRegisterHandleAction()
        {
            var eventTopic = A.Fake<IEventTopic>();
            var registry = A.Fake<IEventScopeInternal>();

            A.CallTo(() => registry.Register(A<Action>.Ignored)).Invokes((Action a) => a());
            A.CallTo(() => this.scopeHolder.Current).Returns(registry);

            var delegateWrapper = A.Fake<IDelegateWrapper>();
            this.testee.Handle(eventTopic, null, null, EventArgs.Empty, delegateWrapper);

            A.CallTo(() => this.handler.Handle(eventTopic, null, null, EventArgs.Empty, delegateWrapper)).MustHaveHappened();
        }
    }
}