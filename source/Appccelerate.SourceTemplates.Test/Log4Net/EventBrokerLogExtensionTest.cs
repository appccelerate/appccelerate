//-------------------------------------------------------------------------------
// <copyright file="EventBrokerLogExtensionTest.cs" company="Appccelerate">
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

namespace Appccelerate.SourceTemplates.Log4Net
{
    using System;

    using Appccelerate.EventBroker;

    using FakeItEasy;

    using Xunit;

    public class EventBrokerLogExtensionTest : IDisposable
    {
        private const string EventTopicUri = "Uri";
        private const string PublisherToString = "PublisherToString";
        private const string SenderToString = "SenderToString";
        private const string EventArgsToString = "EventArgsToString";
        private const string NamedPublisherName = "NamedPublisher";
        
        private readonly EventBrokerLogExtension testee;

        private readonly Log4netHelper log4Net;

        private readonly IEventTopicInfo eventTopicInfo;

        private readonly IPublication publication;

        private readonly object sender;

        private readonly EventArgs eventArgs;

        private readonly IPublication publicationWithNamedPublisher;

        public EventBrokerLogExtensionTest()
        {
            this.testee = new EventBrokerLogExtension();

            this.log4Net = new Log4netHelper();

            this.eventTopicInfo = CreateEventTopicInfo();
            this.publication = CreatePublication();
            this.publicationWithNamedPublisher = CreatePublicationWithNamedPublisher();
            this.sender = CreateSender();
            this.eventArgs = CreateEventArgs();
        }

        public void Dispose()
        {
            this.log4Net.Dispose();
        }

        [Fact]
        public void LogsFiredEvent()
        {
            this.testee.FiredEvent(this.eventTopicInfo, this.publication, this.sender, this.eventArgs);

            this.log4Net.LogMatch(string.Concat(
                "Fired event '", 
                EventTopicUri, 
                "'. Invoked by publisher '",
                PublisherToString,
                "' with sender '",
                SenderToString,
                "' and EventArgs '",
                EventArgsToString,
                "'."));
        }

        [Fact]
        public void LogsNameOfPublisherOnFiredEvent_WhenPublisherIsNamed()
        {
            this.testee.FiredEvent(this.eventTopicInfo, this.publicationWithNamedPublisher, this.sender, this.eventArgs);

            this.log4Net.LogMatch(string.Concat(
                "Fired event '",
                EventTopicUri,
                "'. Invoked by publisher '",
                PublisherToString,
                "' with name '",
                NamedPublisherName,
                "' with sender '",
                SenderToString,
                "' and EventArgs '",
                EventArgsToString,
                "'."));
        }

        private static IEventTopicInfo CreateEventTopicInfo()
        {
            var info = A.Fake<IEventTopicInfo>();

            A.CallTo(() => info.Uri).Returns(EventTopicUri);

            return info;
        }

        private static IPublication CreatePublication()
        {
            var publication = A.Fake<IPublication>();

            A.CallTo(() => publication.Publisher.ToString()).Returns(PublisherToString);

            return publication;
        }

        private static IPublication CreatePublicationWithNamedPublisher()
        {
            var publication = A.Fake<IPublication>();

            var publisher = A.Fake<INamedItem>();

            A.CallTo(() => publication.Publisher).Returns(publisher);
            A.CallTo(() => publisher.EventBrokerItemName).Returns(NamedPublisherName);
            A.CallTo(() => publisher.ToString()).Returns(PublisherToString);

            return publication;
        }

        private static object CreateSender()
        {
            return new NormalSender();
        }

        private static EventArgs CreateEventArgs()
        {
            var e = A.Fake<EventArgs>();
            
            A.CallTo(() => e.ToString()).Returns(EventArgsToString);
            
            return e;
        }

        private class NormalSender
        {
            public override string ToString()
            {
                return SenderToString;
            }
        }
    }
}