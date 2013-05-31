//-------------------------------------------------------------------------------
// <copyright file="EventBrokerWithUserDefinedAutoMapperExtension.cs" company="Appccelerate">
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

    using Appccelerate.EventBroker;
    using Appccelerate.EventBroker.Internals;
    using Appccelerate.MappingEventBroker.Conventions;

    using Machine.Specifications;

    using Moq;

    public class EventBrokerWithUserDefinedAutoMapperExtension
    {
        protected static IEventBroker eventBroker;

        protected static FuncTopicConvention convention;

        protected static Publisher source;

        protected static Mock<IMapper> mapper;
        protected static Mock<IDestinationEventArgsTypeProvider> typeProvider;

        protected static Subscriber destination;

        protected static MappingEventBrokerExtension extension;

        Establish context = () =>
            {
                source = new Publisher();
                destination = new Subscriber();

                convention =
                    new FuncTopicConvention(
                        topic => StartsWith(topic, @"topic://") || StartsWith(topic, @"userdefined://"),
                        s => s.Replace(@"topic://", @"userdefined://"));

                mapper = new Mock<IMapper>();
                typeProvider = new Mock<IDestinationEventArgsTypeProvider>();

                var mock = new Mock<MappingEventBrokerExtension>(mapper.Object, typeProvider.Object) { CallBase = true };
                extension = mock.Object;

                eventBroker = new EventBroker();
                eventBroker.AddMappingExtension(extension);

                eventBroker.Register(source);
                eventBroker.Register(destination);
            };

        private static bool StartsWith(IEventTopicInfo eventTopic, string start)
        {
            return eventTopic.Uri.StartsWith(start, StringComparison.Ordinal);
        }
    }

    [Subject(Concern.MappingWithUserDefinedConvention)]
    public class when_publishing_topic_which_matches_user_defined_convention_with_defined_mapping : EventBrokerWithUserDefinedAutoMapperExtension
    {
        protected static string sourceEventDescription = "Source";

        Establish context = () =>
        {
            typeProvider.Setup(p => p.GetDestinationEventArgsType(Moq.It.IsAny<string>(), Moq.It.IsAny<Type>())).Returns(typeof(DestinationEventArgs));

            mapper.SetupMapping();
        };

        Because of = () =>
        {
            source.Publish(sourceEventDescription);
        };

        Behaves_like<MappedEventFiredBehavior> event_argument_auto_mapper;
    }

    [Subject(Concern.MappingWithUserDefinedConvention)]
    public class when_publishing_topic_which_matches_user_defined_convention_with_defined_mapping_but_without_subscriber : EventBrokerWithDefaultAutoMapperExtension
    {
        protected static bool wasCalled;

        protected static string sourceEventDescription = "Source";

        Establish context = () =>
        {
            typeProvider.Setup(p => p.GetDestinationEventArgsType(Moq.It.IsAny<string>(), Moq.It.IsAny<Type>())).Returns(() => null);

            mapper.SetupMapping();

            extension.SetMissingMappingAction(ctx => wasCalled = true);
        };

        Because of = () =>
        {
            source.Publish(sourceEventDescription);
        };

        Behaves_like<MappedEventNotFiredBehavior> not_an_event_argument_auto_mapper;
    }
}