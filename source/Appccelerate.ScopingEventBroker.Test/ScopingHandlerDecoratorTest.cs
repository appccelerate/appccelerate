//-------------------------------------------------------------------------------
// <copyright file="ScopingHandlerDecoratorTest.cs" company="Appccelerate">
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
    using System.Reflection;
    using Appccelerate.EventBroker;
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
            var subscriptionHandler = new Action(() => { });

            this.testee.Handle(eventTopic, null, EventArgs.Empty, subscriptionHandler);

            A.CallTo(() => this.handler.Handle(eventTopic, null, EventArgs.Empty, subscriptionHandler)).MustHaveHappened();
        }

        [Fact]
        public void Handle_WhenScopeAvailable_MustNotExecuteHandler()
        {
            A.CallTo(() => this.scopeHolder.Current).Returns(A.Fake<IEventScopeRegisterer>());

            this.testee.Handle(A.Fake<IEventTopic>(), null, EventArgs.Empty, new Action(() => { }));

            A.CallTo(() => this.handler.Handle(A<IEventTopic>.Ignored, A<object>.Ignored, A<EventArgs>.Ignored, A<Delegate>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public void Handle_WhenScopeAvailable_MustRegisterHandleAction()
        {
            var eventTopic = A.Fake<IEventTopic>();
            var registerer = A.Fake<IEventScopeRegisterer>();

            ////Refactor to fakeiteasy
            ////registerer.Setup(x => x.Register(It.IsAny<Action>())).Callback<Action>(a => a());
            ////this.scopeHolder.Setup(s => s.Current).Returns(registerer.Object);

            var subscriptionHandler = new Action(() => { });
            this.testee.Handle(eventTopic, null, EventArgs.Empty, subscriptionHandler);

            A.CallTo(() => this.handler.Handle(eventTopic, null, EventArgs.Empty, subscriptionHandler)).MustHaveHappened();
        }
    }
}