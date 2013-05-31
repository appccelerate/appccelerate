//-------------------------------------------------------------------------------
// <copyright file="DefaultTopicConvention.cs" company="Appccelerate">
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

namespace Appccelerate.MappingEventBroker.Conventions
{
    using System;

    using Appccelerate.EventBroker;
    using Appccelerate.EventBroker.Internals;

    /// <summary>
    /// The default convention does automatically remap all event topics which
    /// start with topic:// to topics which start with mapped://. Therefore if using this topic convention the following is possible:
    /// <code>
    ///    public class Publisher
    ///    {
    ///        [EventPublication(@"topic://Original")]
    ///        public event EventHandler Event;
    ///        private void InvokeEvent(EventArgs e)
    ///        {
    ///            EventHandler handler = Event;
    ///            if (handler != null) handler(this, e);
    ///        }
    ///        public void Publish()
    ///        {
    ///            this.InvokeEvent(EventArgs.Empty);
    ///        }
    ///    }
    ///    public class SubscriberOriginal
    ///    {
    ///        [EventSubscription(@"topic://Original", typeof(Appccelerate.EventBroker.Handlers.OnPublisher))]
    ///        public void HandleOriginal(object sender, EventArgs e)
    ///        {
    ///        }
    ///    }
    ///    public class SubscriberMapped
    ///    {
    ///        [EventSubscription(@"mapped://Original", typeof(Appccelerate.EventBroker.Handlers.OnPublisher))]
    ///        public void HandleOriginal(object sender, CancelEventArgs e)
    ///        {
    ///        }
    ///    }
    /// </code>
    /// </summary>
    public class DefaultTopicConvention : ITopicConvention
    {
        /// <summary>
        /// The default event topic URI for inputs.
        /// </summary>
        public const string EventTopicUriInput = @"topic://";

        /// <summary>
        /// The default event topic URI for outputs
        /// </summary>
        public const string EventTopicUriOutput = @"mapped://";

        /// <summary>
        /// Determines whether the specified event topic is a candidate to process.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <returns>
        /// <c>true</c> if the specified event topic is candidate; otherwise, <c>false</c>.
        /// </returns>
        public bool IsCandidate(IEventTopicInfo eventTopic)
        {
            return StartsWith(eventTopic, EventTopicUriInput);
        }

        /// <summary>
        /// Maps the topic from the source format to the destination format.
        /// </summary>
        /// <param name="topic">The source topic URI.</param>
        /// <returns>The mapped topic URI.</returns>
        public string MapTopic(string topic)
        {
            Ensure.ArgumentNotNullOrEmpty(topic, "topic");

            return topic.Replace(EventTopicUriInput, EventTopicUriOutput);
        }

        /// <summary>
        /// Determines whether the topics URI starts with start.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="start">The start of the topic.</param>
        /// <returns><see langword="true"/> if the topic URI starts with start;
        /// otherwise <see langword="false"/>.</returns>
        private static bool StartsWith(IEventTopicInfo eventTopic, string start)
        {
            return eventTopic.Uri.StartsWith(start, StringComparison.Ordinal);
        }
    }
}