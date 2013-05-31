//-------------------------------------------------------------------------------
// <copyright file="EventTopicHost.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Internals
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Appccelerate.EventBroker.Internals.GlobalMatchers;

    /// <summary>
    /// Default implementation of a <see cref="IEventTopicHost"/>.
    /// </summary>
    internal class EventTopicHost : IEventTopicHost
    {
        private readonly IFactory factory;

        private readonly IExtensionHost extensionHost;

        private readonly IGlobalMatchersProvider globalMatchersProvider;

        /// <summary>
        /// Map from event topic URI to event topic instance.
        /// </summary>
        private Dictionary<string, IEventTopic> eventTopics = new Dictionary<string, IEventTopic>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTopicHost"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="extensionHost">The extension host holding all extensions.</param>
        /// <param name="globalMatchersProvider">The global matchers provider.</param>
        public EventTopicHost(IFactory factory, IExtensionHost extensionHost, IGlobalMatchersProvider globalMatchersProvider)
        {
            this.factory = factory;
            this.extensionHost = extensionHost;
            this.globalMatchersProvider = globalMatchersProvider;
        }

        /// <summary>
        /// Gets a event topic. Result is never null. Event topic is created if it does not yet exist.
        /// </summary>
        /// <param name="topic">The topic URI identifying the event topic to return.</param>
        /// <returns>A non-null event topic. Event topic is created if it does not yet exist.</returns>
        /// <remarks>
        /// Returns a non null instance of the dictionary.
        /// </remarks>
        /// <value>The event topics.</value>
        public IEventTopic GetEventTopic(string topic)
        {
            if (this.eventTopics.ContainsKey(topic))
            {
                return this.eventTopics[topic];
            }

            lock (this)
            {
                // recheck inside monitor
                if (this.eventTopics.ContainsKey(topic))
                {
                    return this.eventTopics[topic];
                }

                IEventTopic eventTopic = this.factory.CreateEventTopicInternal(topic, this.globalMatchersProvider);

                // copy
                this.eventTopics = new Dictionary<string, IEventTopic>(this.eventTopics) { { topic, eventTopic } };

                this.extensionHost.ForEach(extension => extension.CreatedTopic(eventTopic));

                return eventTopic;
            }
        }

        /// <summary>
        /// Describes all event topics:
        /// publications, subscriptions, names, thread options, matchers, event arguments.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void DescribeTo(TextWriter writer)
        {
            Ensure.ArgumentNotNull(writer, "writer");

            foreach (IEventTopic eventTopic in this.eventTopics.Values)
            {
                eventTopic.DescribeTo(writer);
                writer.WriteLine();
            }
        }

        /// <summary>
        /// See <see cref="IDisposable.Dispose"/> for more information.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            lock (this)
            {
                foreach (IEventTopic eventTopic in this.eventTopics.Values)
                {
                    eventTopic.Dispose();
                }

                this.eventTopics.Clear();
            }
        }
    }
}