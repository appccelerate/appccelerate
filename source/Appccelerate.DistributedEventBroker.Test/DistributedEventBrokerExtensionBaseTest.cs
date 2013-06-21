//-------------------------------------------------------------------------------
// <copyright file="DistributedEventBrokerExtensionBaseTest.cs" company="Appccelerate">
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

namespace Appccelerate.DistributedEventBroker
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Appccelerate.EventBroker;

    using Events;
    using Messages;
    using Moq;
    using Xunit;

    public class DistributedEventBrokerExtensionBaseTest
    {
        private const string DefaultTopicUri = "TestTopic";
        private readonly Mock<IEventBrokerBus> eventBrokerBus;

        private readonly Mock<IEventBroker> eventBroker;

        private readonly string distributedEventBrokerIdentification;

        private readonly Mock<IDistributedFactory> factory;

        private readonly TestableDistributedEventBrokerExtensionBase testee;

        private readonly Mock<IEventRegistrar> registerer;

        public DistributedEventBrokerExtensionBaseTest()
        {
            this.eventBrokerBus = new Mock<IEventBrokerBus>();
            this.eventBroker = new Mock<IEventBroker>();
            this.registerer = new Mock<IEventRegistrar>();
            this.factory = new Mock<IDistributedFactory>
                               {
                                   DefaultValue = DefaultValue.Mock
                               };

            this.distributedEventBrokerIdentification = "TestDistributedEventBroker";

            this.testee = new TestableDistributedEventBrokerExtensionBase(this.distributedEventBrokerIdentification, this.eventBrokerBus.Object, this.factory.Object, this.registerer.Object);
        }

        [Fact]
        public void Constructor_MustAssignBrokerBus()
        {
            Assert.Same(this.eventBrokerBus.Object, this.testee.TestEventBrokerBus);
        }

        [Fact]
        public void Constructor_MustAssignDistributedIdentification()
        {
            Assert.Equal(this.distributedEventBrokerIdentification, this.testee.TestDistributedEventBrokerIdentification);
        }

        [Fact]
        public void Constructor_MustAssignFactory()
        {
            Assert.Same(this.factory.Object, this.testee.TestFactory);
        }

        [Fact]
        public void Constructor_MustAssignEventArgsSerializer()
        {
            var serializer = this.GetSerializer();

            Assert.Same(serializer.Object, this.testee.TestSerializer);
        }

        [Fact]
        public void Constructor_MustAssignMessageFactory()
        {
            var messageFactory = this.GetMessageFactory();

            Assert.Same(messageFactory.Object, this.testee.TestMessageFactory);
        }

        [Fact]
        public void Constructor_MustAssignTopicSelectionStrategy()
        {
            var topicSelectionStrategy = this.GetTopicSelectionStrategy();

            Assert.Same(topicSelectionStrategy.Object, this.testee.TestSelectionStrategy);
        }

        [Fact]
        public void Constructor_CreateEventArgsSerializerByUsingTheFactory()
        {
            this.factory.Verify(f => f.CreateEventArgsSerializer());
        }

        [Fact]
        public void Constructor_CreateMessageFactoryByUsingTheFactory()
        {
            this.factory.Verify(f => f.CreateMessageFactory());
        }

        [Fact]
        public void Constructor_CreateTopicSelectionStrategyByUsingTheFactory()
        {
            this.factory.Verify(f => f.CreateTopicSelectionStrategy());
        }

        [Fact]
        public void Manage_MustTrackEventBrokerInstance()
        {
            var broker = CreateEventBroker();

            this.testee.Manage(broker.Object);

            Assert.Equal(broker.Object, this.testee.TestHostedEventBroker);
        }

        [Fact]
        public void Manage_WhenIdentificationProvided_MustUseIt()
        {
            var broker = CreateEventBroker();
            string eventBrokerIdentification = Guid.NewGuid().ToString();

            this.testee.Manage(broker.Object, eventBrokerIdentification);

            Assert.Equal(eventBrokerIdentification, this.testee.TestHostedEventBrokerIdentification);
        }

        [Fact]
        public void Manage_WhenNoIdentificationProvided_MustCreateDefaultOne()
        {
            var broker = CreateEventBroker();

            this.testee.Manage(broker.Object);

            Assert.NotNull(this.testee.TestHostedEventBrokerIdentification);
            Assert.DoesNotThrow(() => new Guid(this.testee.TestHostedEventBrokerIdentification));
        }

        [Fact]
        public void Manage_WhenTryingToManageMultipleEventBroker_MustThrowInvalidOperationException()
        {
            var eventBroker1 = CreateEventBroker();
            var eventBroker2 = CreateEventBroker();

            this.testee.Manage(eventBroker1.Object);

            Assert.Throws<InvalidOperationException>(() => this.testee.Manage(eventBroker2.Object));
        }

        [Fact]
        public void Manage_MustRegisterDynamicTopic()
        {
            var broker = CreateEventBroker();

            this.testee.Manage(broker.Object);

            this.registerer.Verify(registerer => 
                registerer.AddSubscription("topic://Appccelerate.DistributedEventBroker/TestDistributedEventBroker", this.testee, It.IsAny<EventHandler<EventArgs<IEventFired>>>(), It.IsAny<IHandler>()));
        }

        [Fact]
        public void FiringEvent_MustAcquireMessageWithMessageFactory()
        {
            this.SetupManagedEventBroker();

            CancelEventArgs cancelEventArgs = new CancelEventArgs(true);
            const HandlerRestriction HandlerRestriction = HandlerRestriction.Asynchronous;
            const string SimpleTopic = "topic://Simple";
            const string SerializedTopic = "SomeData";

            var serializer = this.GetSerializer();
            var messageFactory = this.GetMessageFactory();
            var message = new Mock<IEventFired>();
            message.SetupAllProperties();

            var eventTopic = new Mock<IEventTopicInfo>();
            eventTopic.SetupGet(topic => topic.Uri).Returns(SimpleTopic);

            var publication = new Mock<IPublication>();
            
            publication.SetupGet(p => p.HandlerRestriction).Returns(HandlerRestriction);

            serializer.Setup(s => s.Serialize(It.IsAny<CancelEventArgs>())).Returns(SerializedTopic);

            messageFactory
                .Setup(f => f.CreateEventFiredMessage(It.IsAny<Action<IEventFired>>()))
                .Callback((Action<IEventFired> initialization) => initialization(message.Object))
                .Returns(message.Object);

            this.SetupTopicAcceptedByStrategy(eventTopic);

            this.testee.FiringEvent(eventTopic.Object, publication.Object, this, cancelEventArgs);

            Assert.Equal(HandlerRestriction, message.Object.HandlerRestriction);
            Assert.Equal(SimpleTopic, message.Object.Topic);
            Assert.Equal(SerializedTopic, message.Object.EventArgs);
            Assert.Equal(cancelEventArgs.GetType().AssemblyQualifiedName, message.Object.EventArgsType);
            Assert.Equal(this.testee.TestHostedEventBrokerIdentification, message.Object.EventBrokerIdentification);
            Assert.Equal(this.distributedEventBrokerIdentification, message.Object.DistributedEventBrokerIdentification);
        }

        [Fact]
        public void FiringEvent_MustFireEventOnBus()
        {
            Mock<IEventTopicInfo> topic = GetDefaultTopic();

            this.SetupTopicAcceptedByStrategy(topic);

            this.testee.FiringEvent(topic.Object, new Mock<IPublication>().Object, this, EventArgs.Empty);

            this.eventBrokerBus.Verify(bus => bus.Publish(It.IsAny<IEventFired>()));
        }

        [Fact]
        public void FiringEvent_WhenSenderIsExtensionItself_MustNotFireEventOnBus()
        {
            this.testee.FiringEvent(new Mock<IEventTopic>().Object, new Mock<IPublication>().Object, this.testee, EventArgs.Empty);

            this.eventBrokerBus.Verify(bus => bus.Publish(It.IsAny<IEventFired>()), Times.Never());
        }

        [Fact]
        public void FiringEvent_MustOnlyFireWhenTopicWasPreviouslySelected()
        {
            var topic = new Mock<IEventTopicInfo>();
            topic.SetupGet(t => t.Uri).Returns("SomeTopic");

            this.SetupTopicNotAcceptedByStrategy(topic);

            this.testee.FiringEvent(topic.Object, new Mock<IPublication>().Object, this, EventArgs.Empty);

            this.eventBrokerBus.Verify(bus => bus.Publish(It.IsAny<IEventFired>()), Times.Never());
        }

        [Fact]
        public void CreatedTopic_MustTrackCreatedTopic()
        {
            this.SetupStrategyShallPassThrough();

            var topic = new Mock<IEventTopic>();

            topic.SetupGet(t => t.Uri).Returns("sometopic");

            this.testee.CreatedTopic(topic.Object);

            Assert.Equal(1, this.testee.TestTopics.Count());
            Assert.Equal(topic.Object.Uri, this.testee.TestTopics.Single());
        }

        [Fact]
        public void CreatedTopic_OnlyTrackTopicsWhichAreAcceptedByTheSelectionStrategy()
        {
            var strategy = this.GetTopicSelectionStrategy();
            var topic = new Mock<IEventTopic>();

            strategy.Setup(s => s.SelectTopic(topic.Object)).Returns(false);

            this.testee.CreatedTopic(topic.Object);

            Assert.Empty(this.testee.TestTopics);
        }

        [Fact]
        public void CreateTopic_WhenHandlerRestrictionSynchronous_MustThrowInvalidOperationException()
        {
            this.SetupStrategyShallPassThrough();

            var topic =
                Mock.Of<IEventTopicInfo>(
                    x =>
                    x.Publications ==
                    new List<IPublication>
                        {
                            Mock.Of<IPublication>(p => p.HandlerRestriction == HandlerRestriction.Asynchronous),
                            Mock.Of<IPublication>(p => p.HandlerRestriction == HandlerRestriction.Synchronous),
                            Mock.Of<IPublication>(p => p.HandlerRestriction == HandlerRestriction.Asynchronous),
                        });

            var exception = Assert.Throws<InvalidOperationException>(() => this.testee.CreatedTopic(topic));
        }

        [Fact]
        public void Disposed_MustReleaseTopic()
        {
            var topic = new Mock<IEventTopic>();
            const string ExpectedTopic = "topic://testtopic";
            topic.SetupGet(t => t.Uri).Returns(ExpectedTopic);

            this.testee.CreatedTopic(topic.Object);

            this.testee.Disposed(topic.Object);

            Assert.Empty(this.testee.TestTopics);
        }

        [Fact]
        public void HandleMessage_MustRefireEventOnHostedEventBroker()
        {
            const string ExpectedTopic = "topic://testtopic";
            const HandlerRestriction HandlerRestriction = HandlerRestriction.Asynchronous;
            CancelEventArgs cancelEventArgs = new CancelEventArgs(true);

            var serializer = this.GetSerializer();
            var eventFired = new Mock<IEventFired>();
            eventFired.SetupAllProperties();

            eventFired.Object.EventArgsType = typeof(CancelEventArgs).AssemblyQualifiedName;
            eventFired.Object.Topic = ExpectedTopic;
            eventFired.Object.HandlerRestriction = HandlerRestriction;

            serializer.Setup(s => s.Deserialize(typeof(CancelEventArgs), It.IsAny<string>())).Returns(cancelEventArgs);

            this.SetupManagedEventBroker();

            this.testee.TestHandleMessage(eventFired.Object);

            this.eventBroker.Verify(eb => eb.Fire(ExpectedTopic, this.testee, HandlerRestriction, this.testee, cancelEventArgs));
        }

        [Fact]
        public void HandleMessage_MustNotRefireOnHostedEventBrokerWhenIncomingEventIsFromManagedEventBroker()
        {
            const string ExpectedIdentification = "EventBrokerIdentification";

            var serializer = this.GetSerializer();
            var eventFired =
                Mock.Of<IEventFired>(
                    e =>
                    e.EventBrokerIdentification == ExpectedIdentification &&
                    e.EventArgsType == typeof(EventArgs).AssemblyQualifiedName);

            serializer.Setup(s => s.Deserialize(It.IsAny<Type>(), It.IsAny<string>())).Returns(EventArgs.Empty);

            this.SetupManagedEventBroker(ExpectedIdentification);

            this.testee.TestHandleMessage(eventFired);

            this.eventBroker.Verify(eb => eb.Fire(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<HandlerRestriction>(), It.IsAny<object>(), It.IsAny<EventArgs>()), Times.Never());
        }

        private static Mock<IEventBroker> CreateEventBroker()
        {
            return new Mock<IEventBroker>();
        }

        private static Mock<IEventTopicInfo> GetDefaultTopic()
        {
            var topic = new Mock<IEventTopicInfo>();
            topic.SetupGet(t => t.Uri).Returns(DefaultTopicUri);
            return topic;
        }

        private void SetupManagedEventBroker(string eventBrokerIdentification)
        {
            this.testee.Manage(this.eventBroker.Object, eventBrokerIdentification);
        }

        private void SetupManagedEventBroker()
        {
            this.testee.Manage(this.eventBroker.Object);
        }

        private void SetupStrategyShallPassThrough()
        {
            var strategy = this.GetTopicSelectionStrategy();
            strategy.Setup(s => s.SelectTopic(It.IsAny<IEventTopicInfo>())).Returns(true);
        }

        private void SetupStrategyShallNotPassThrough()
        {
            var strategy = this.GetTopicSelectionStrategy();
            strategy.Setup(s => s.SelectTopic(It.IsAny<IEventTopicInfo>())).Returns(false);
        }

        private void SetupTopicAcceptedByStrategy(Mock<IEventTopicInfo> topic)
        {
            this.SetupStrategyShallPassThrough();

            this.testee.CreatedTopic(topic.Object);
        }

        private void SetupTopicNotAcceptedByStrategy(Mock<IEventTopicInfo> topic)
        {
            this.SetupStrategyShallNotPassThrough();

            this.testee.CreatedTopic(topic.Object);
        }

        private Mock<IEventArgsSerializer> GetSerializer()
        {
            return Mock.Get(this.factory.Object.CreateEventArgsSerializer());
        }

        private Mock<IEventMessageFactory> GetMessageFactory()
        {
            return Mock.Get(this.factory.Object.CreateMessageFactory());
        }

        private Mock<ITopicSelectionStrategy> GetTopicSelectionStrategy()
        {
            return Mock.Get(this.factory.Object.CreateTopicSelectionStrategy());
        }

        private class TestableDistributedEventBrokerExtensionBase : DistributedEventBrokerExtensionBase
        {
            private readonly IEventRegistrar registerer;

            public TestableDistributedEventBrokerExtensionBase(string distributedEventBrokerIdentification, IEventBrokerBus eventBrokerBus, IDistributedFactory factory, IEventRegistrar registerer) :
                base(distributedEventBrokerIdentification, eventBrokerBus, factory)
            {
                this.registerer = registerer;
            }

            public IEnumerable<string> TestTopics
            {
                get { return this.Topics; }
            }

            public IEventBroker TestHostedEventBroker
            {
                get { return this.HostedEventBroker; }
            }

            public string TestHostedEventBrokerIdentification
            {
                get { return this.HostedEventBrokerIdentification; }
            }

            public IEventBrokerBus TestEventBrokerBus
            {
                get { return this.EventBrokerBus; }
            }

            public string TestDistributedEventBrokerIdentification
            {
                get { return this.DistributedEventBrokerIdentification; }
            }

            public IDistributedFactory TestFactory
            {
                get { return this.Factory; }
            }

            public IEventArgsSerializer TestSerializer
            {
                get { return this.Serializer;  }
            }

            public object TestMessageFactory
            {
                get { return this.MessageFactory;  }
            }

            public ITopicSelectionStrategy TestSelectionStrategy
            {
                get { return this.SelectionStrategy; }
            }

            public void TestHandleMessage(IEventFired eventFired)
            {
                this.HandleMessage(this, eventFired);
            }

            protected override IEventRegistrar EventRegistrar
            {
                get
                {
                    return this.registerer;
                }
            }

            protected override IHandler CreateHandler()
            {
                return new Mock<IHandler>().Object;
            }
        }
    }
}