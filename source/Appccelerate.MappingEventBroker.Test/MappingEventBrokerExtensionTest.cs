//-------------------------------------------------------------------------------
// <copyright file="MappingEventBrokerExtensionTest.cs" company="Appccelerate">
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

namespace Appccelerate.MappingEventBroker
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    using Appccelerate.EventBroker;
    using Appccelerate.EventBroker.Internals;

    using Moq;

    using Xunit;

    public class MappingEventBrokerExtensionTest
    {
        private const string TopicUri = "Topic";
        private const string MappedTopicUri = "MappedTopic";

        private readonly Mock<ITopicConvention> topicConvention;

        private readonly Mock<IPublication> publication;

        private readonly Mock<IEventTopic> eventTopic;

        private readonly Mock<IMapper> mapper;

        private readonly Mock<IEventBroker> eventBroker;

        private readonly Mock<IDestinationEventArgsTypeProvider> typeProvider;

        private readonly TestableAutoMapperEventBrokerExtension testee;

        public MappingEventBrokerExtensionTest()
        {
            this.topicConvention = new Mock<ITopicConvention>();
            this.publication = new Mock<IPublication>();
            this.eventTopic = new Mock<IEventTopic>();
            this.mapper = new Mock<IMapper>();
            this.eventBroker = new Mock<IEventBroker>();
            this.typeProvider = new Mock<IDestinationEventArgsTypeProvider>();

            this.testee = new TestableAutoMapperEventBrokerExtension(this.mapper.Object, this.topicConvention.Object, this.typeProvider.Object);
        }

        [Fact]
        public void CreatedTopic_MustTrackCreatedTopic()
        {
            this.SetupConventionShallPassThrough();
            this.SetupManageEventBroker();

            var topic = new Mock<IEventTopic>();

            topic.SetupGet(t => t.Uri).Returns("sometopic");

            this.testee.CreatedTopic(topic.Object);

            Assert.Equal(1, this.testee.TestTopics.Count());
            Assert.Equal(topic.Object, this.testee.TestTopics.Single());
        }

        [Fact]
        public void CreatedTopic_OnlyTrackTopicsWhichAreAcceptedByTheSelectionStrategy()
        {
            this.SetupManageEventBroker();

            var topic = new Mock<IEventTopic>();

            this.topicConvention.Setup(s => s.IsCandidate(topic.Object)).Returns(false);

            this.testee.CreatedTopic(topic.Object);

            Assert.Empty(this.testee.TestTopics);
        }

        [Fact]
        public void CreatedTopic_WhenNoEventBrokerManaged_MustThrowInvalidOperationException()
        {
            var topic = new Mock<IEventTopic>();

            Assert.Throws<InvalidOperationException>(() => this.testee.CreatedTopic(topic.Object));
        }

        [Fact]
        public void FiringEvent_MustMapTopicWithConvention()
        {
            this.SetupManageEventBroker();

            this.eventTopic.SetupGet(e => e.Uri).Returns(TopicUri);

            this.testee.FiringEvent(this.eventTopic.Object, this.publication.Object, this, EventArgs.Empty);

            this.topicConvention.Verify(c => c.MapTopic(TopicUri));
        }

        [Fact]
        public void FiringEvent_WhenTopicNotContained_MustNotRefire()
        {
            this.SetupManageEventBroker();
            this.SetupConvetionMapping();
            this.SetupPublication();
            this.SetupDestinationEventArgsProvider();

            this.eventTopic.SetupGet(e => e.Uri).Returns(TopicUri);

            this.testee.FiringEvent(this.eventTopic.Object, this.publication.Object, this, EventArgs.Empty);

            this.eventBroker.Verify(t => t.Fire(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<HandlerRestriction>(), It.IsAny<object>(), It.IsAny<EventArgs>()), Times.Never());
        }

        [Fact]
        public void FiringEvent_WhenTopicContained_MustRefire()
        {
            this.SetupManageEventBroker();
            this.SetupEventTopicAndConvention();
            this.SetupPublication();
            this.SetupDestinationEventArgsProvider();

            this.publication.SetupGet(p => p.HandlerRestriction).Returns(HandlerRestriction.Asynchronous);
            this.publication.SetupGet(p => p.Publisher).Returns(this);

            this.testee.FiringEvent(this.eventTopic.Object, this.publication.Object, this, EventArgs.Empty);

            this.eventBroker.Verify(t => t.Fire(MappedTopicUri, this, HandlerRestriction.Asynchronous, this, It.IsAny<EventArgs>()));
        }

        [Fact]
        public void FiringEvent_WhenTopicAndMappedTopicAreEqual_MustNotRefire()
        {
            this.SetupManageEventBroker();
            this.SetupSameEventTopics();
            this.SetupDestinationEventArgsProvider();
            this.SetupPublication();

            this.testee.FiringEvent(this.eventTopic.Object, this.publication.Object, this, EventArgs.Empty);

            this.eventBroker.Verify(t => t.Fire(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<HandlerRestriction>(), It.IsAny<object>(), It.IsAny<EventArgs>()), Times.Never());
        }

        [Fact]
        public void FiringEvent_WhenTopicContained_MustMapEventArgs()
        {
            this.SetupManageEventBroker();
            this.SetupEventTopicAndConvention();
            this.SetupPublication();
            this.SetupDestinationEventArgsProvider();

            this.testee.FiringEvent(this.eventTopic.Object, this.publication.Object, this, EventArgs.Empty);

            this.mapper.Verify(m => m.Map(typeof(EventArgs), typeof(CancelEventArgs), It.IsAny<EventArgs>()));
        }

        [Fact]
        public void FiringEvent_WhenNoMappingDefined_MustNotRefire()
        {
            this.SetupManageEventBroker();
            this.SetupEventTopicAndConvention();
            this.SetupPublication();
            this.testee.SetMissingMappingAction(ctx => { });

            this.testee.FiringEvent(this.eventTopic.Object, this.publication.Object, this, EventArgs.Empty);

            this.eventBroker.Verify(t => t.Fire(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<HandlerRestriction>(), It.IsAny<object>(), It.IsAny<EventArgs>()), Times.Never());
        }

        [Fact]
        public void FiringEvent_WhenNoMappingDefined_MustCallMissingMappingAction()
        {
            this.SetupManageEventBroker();
            this.SetupEventTopicAndConvention();
            this.SetupPublication();

            bool wasCalled = false;

            this.testee.SetMissingMappingAction(ctx => { wasCalled = true; });
            this.testee.FiringEvent(this.eventTopic.Object, this.publication.Object, this, EventArgs.Empty);

            Assert.True(wasCalled, "MissingMappingAction was not called");
        }

        [Fact]
        public void FiringEvent_WhenNoMappingDefined_MustPassRequiredArgumentsToMissingMappingAction()
        {
            this.SetupManageEventBroker();
            this.SetupEventTopicAndConvention();
            this.SetupPublication();

            IMissingMappingContext context = null;

            this.testee.SetMissingMappingAction(ctx => { context = ctx; });

            this.testee.FiringEvent(this.eventTopic.Object, this.publication.Object, this, EventArgs.Empty);

            Assert.NotNull(context);
            Assert.Same(this.eventTopic.Object, context.EventTopic);
            Assert.Same(MappedTopicUri, context.DestinationTopic);
            Assert.Same(this.publication.Object, context.Publication);
            Assert.Same(this, context.Sender);
            Assert.Same(EventArgs.Empty, context.EventArgs);
            Assert.IsType<ArgumentException>(context.Exception);
        }

        [Fact]
        public void FiringEvent_WhenMapperThrowsException_MustPassRequiredArgumentsToMissingMappingAction()
        {
            this.SetupManageEventBroker();
            this.SetupEventTopicAndConvention();
            this.SetupPublication();
            this.SetupDestinationEventArgsProvider();

            var exception = new InvalidOperationException();
            this.mapper.Setup(m => m.Map(It.IsAny<Type>(), It.IsAny<Type>(), It.IsAny<EventArgs>()))
                .Throws(exception);

            IMissingMappingContext context = null;

            this.testee.SetMissingMappingAction(ctx => { context = ctx; });

            this.testee.FiringEvent(this.eventTopic.Object, this.publication.Object, this, EventArgs.Empty);

            Assert.NotNull(context);
            Assert.Same(this.eventTopic.Object, context.EventTopic);
            Assert.Same(MappedTopicUri, context.DestinationTopic);
            Assert.Same(this.publication.Object, context.Publication);
            Assert.Same(this, context.Sender);
            Assert.Same(EventArgs.Empty, context.EventArgs);
            Assert.Same(exception, context.Exception);
        }

        [Fact]
        public void FiringEvent_MustAcquireEventArgsTypeFromTypeProvider()
        {
            this.SetupManageEventBroker();
            this.SetupEventTopicAndConvention();
            this.SetupPublication();
            this.SetupDestinationEventArgsProvider();

            this.testee.FiringEvent(this.eventTopic.Object, this.publication.Object, this, EventArgs.Empty);

            this.typeProvider.Verify(p => p.GetDestinationEventArgsType(MappedTopicUri, typeof(EventArgs)));
        }

        [Fact]
        public void FiringEvent_WhenNoEventBrokerManaged_MustThrowInvalidOperationException()
        {
            this.eventTopic.SetupGet(e => e.Uri).Returns(TopicUri);

            Assert.Throws<InvalidOperationException>(() => this.testee.FiringEvent(this.eventTopic.Object, this.publication.Object, this, EventArgs.Empty));
        }

        [Fact]
        public void Disposed_WhenContained_MustReleaseTopic()
        {
            this.SetupManageEventBroker();
            this.SetupEventTopicAndConvention();

            this.testee.Disposed(this.eventTopic.Object);

            Assert.Empty(this.testee.TestTopics);
        }

        [Fact]
        public void Manage_MustTrackEventBrokerInstance()
        {
            this.testee.Manage(this.eventBroker.Object);

            Assert.Equal(this.eventBroker.Object, this.testee.TestHostedEventBroker);
        }

        [Fact]
        public void Manage_WhenTryingToManageMultipleEventBroker_MustThrowInvalidOperationException()
        {
            this.testee.Manage(this.eventBroker.Object);

            Assert.Throws<InvalidOperationException>(() => this.testee.Manage(this.eventBroker.Object));
        }

        private void SetupManageEventBroker()
        {
            this.testee.Manage(this.eventBroker.Object);
        }

        private void SetupPublication()
        {
            this.publication.SetupGet(p => p.EventArgsType).Returns(typeof(EventArgs));
        }

        private void SetupSameEventTopics()
        {
            this.SetupEventTopicAndConvention();

            this.eventTopic.SetupGet(e => e.Uri).Returns(MappedTopicUri);
        }

        private void SetupEventTopicAndConvention()
        {
            this.SetupConvention();

            this.SetupEventTopic();
        }

        private void SetupEventTopic()
        {
            this.eventTopic.SetupGet(e => e.Uri).Returns(TopicUri);
            this.testee.CreatedTopic(this.eventTopic.Object);
        }

        private void SetupConvention()
        {
            this.SetupConventionShallPassThrough();
            this.SetupConvetionMapping();
        }

        private void SetupConvetionMapping()
        {
            this.topicConvention.Setup(s => s.MapTopic(It.IsAny<string>())).Returns(MappedTopicUri);
        }

        private void SetupConventionShallPassThrough()
        {
            this.topicConvention.Setup(s => s.IsCandidate(It.IsAny<IEventTopic>())).Returns(true);
        }

        private void SetupDestinationEventArgsProvider()
        {
            this.typeProvider.Setup(p => p.GetDestinationEventArgsType(MappedTopicUri, It.IsAny<Type>()))
                .Returns(typeof(CancelEventArgs));
        }

        private class TestableAutoMapperEventBrokerExtension : MappingEventBrokerExtension
        {
            public TestableAutoMapperEventBrokerExtension(IMapper mapper, ITopicConvention topicConvention, IDestinationEventArgsTypeProvider typeProvider)
                : base(mapper, topicConvention, typeProvider)
            {
            }

            public EventTopicCollection TestTopics
            {
                get { return this.Topics; }
            }

            public IEventBroker TestHostedEventBroker
            {
                get
                {
                    return this.HostedEventBroker;
                }
            }
        }
    }
}
