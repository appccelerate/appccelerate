//-------------------------------------------------------------------------------
// <copyright file="EventInspector.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Appccelerate.EventBroker.Internals.Exceptions;

    using Matchers;

    /// <summary>
    /// The <see cref="EventInspector"/> scans classes for publications or subscriptions.
    /// </summary>
    internal class EventInspector : IEventInspector
    {
        /// <summary>
        /// The factory.
        /// </summary>
        private readonly IFactory factory;

        /// <summary>
        /// Extension host holding all extensions.
        /// </summary>
        private readonly IExtensionHost extensionHost;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventInspector"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="extensionHost">The extension host holding all extensions.</param>
        public EventInspector(IFactory factory, IExtensionHost extensionHost)
        {
            this.factory = factory;
            this.extensionHost = extensionHost;
        }

        /// <summary>
        /// Processes a publishers.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        /// <param name="register">true to register publications, false to unregister them.</param>
        /// <param name="eventTopicHost">The event topic host.</param>
        /// <remarks>Scans the members of the <paramref name="publisher"/> and registers or unregisters publications.</remarks>
        public void ProcessPublisher(object publisher, bool register, IEventTopicHost eventTopicHost)
        {
            List<EventInfo> eventInfos = new List<EventInfo>();
            eventInfos.AddRange(publisher.GetType().GetEvents());
            foreach (Type interfaceType in publisher.GetType().GetInterfaces())
            {
                eventInfos.AddRange(interfaceType.GetEvents());
            }

            foreach (EventInfo eventInfo in eventInfos)
            {
                foreach (EventPublicationAttribute attr in eventInfo.GetCustomAttributes(typeof(EventPublicationAttribute), true))
                {
                    this.HandlePublisher(publisher, register, eventInfo, attr, eventTopicHost);
                }
            }

            this.extensionHost.ForEach(extension => extension.ProcessedPublisher(publisher, register, eventTopicHost));
        }

        /// <summary>
        /// Processes the subscriber.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="register">true to register subscriptions, false to unregister them.</param>
        /// <param name="eventTopicHost">The event topic host.</param>
        /// <remarks>Scans the members of the <paramref name="subscriber"/> and registers or unregisters subscriptions.</remarks>
        public void ProcessSubscriber(object subscriber, bool register, IEventTopicHost eventTopicHost)
        {
            List<MethodInfo> methodInfos = new List<MethodInfo>();
            methodInfos.AddRange(subscriber.GetType().GetMethods());
            foreach (Type interfaceType in subscriber.GetType().GetInterfaces())
            {
                methodInfos.AddRange(interfaceType.GetMethods());
            }

            foreach (MethodInfo methodInfo in methodInfos)
            {
                foreach (EventSubscriptionAttribute attr in methodInfo.GetCustomAttributes(typeof(EventSubscriptionAttribute), true))
                {
                    this.HandleSubscriber(subscriber, register, methodInfo, attr, eventTopicHost);
                }
            }

            this.extensionHost.ForEach(extension => extension.ProcessedSubscriber(subscriber, register, eventTopicHost));
        }

        /// <summary>
        /// Processes the publisher.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="matchers">The matchers.</param>
        /// <param name="register">true to register publications, false to unregister them.</param>
        /// <param name="eventTopicHost">The event topic host.</param>
        public void ProcessPublisher(string topic, object publisher, string eventName, HandlerRestriction handlerRestriction, IList<IPublicationMatcher> matchers, bool register, IEventTopicHost eventTopicHost)
        {
            Ensure.ArgumentNotNull(publisher, "publisher");
            Ensure.ArgumentNotNull(eventTopicHost, "eventTopicHost");

            EventInfo eventInfo = publisher.GetType().GetEvent(eventName);

            if (eventInfo == null)
            {
                throw new PublisherEventNotFoundException(publisher.GetType(), eventName);
            }

            IEventTopic eventTopic = eventTopicHost.GetEventTopic(topic);

            if (register)
            {
                eventTopic.AddPublication(publisher, eventInfo, handlerRestriction, matchers);
            }
            else
            {
                eventTopic.RemovePublication(publisher, eventInfo);
            }
        }

        /// <summary>
        /// Processes the subscriber.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">The handler method.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="matchers">The matchers.</param>
        /// <param name="register">true to register subscriptions, false to unregister them.</param>
        /// <param name="eventTopicHost">The event topic host.</param>
        public void ProcessSubscriber(string topic, object subscriber, EventHandler handlerMethod, IHandler handler, ISubscriptionMatcher[] matchers, bool register, IEventTopicHost eventTopicHost)
        {
            Ensure.ArgumentNotNull(handlerMethod, "handlerMethod");

            this.HandleSubscriber(eventTopicHost, topic, register, subscriber, handlerMethod.Method, handler, matchers);
        }

        /// <summary>
        /// Processes the subscriber.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="topic">The topic.</param>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">The handler method.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="matchers">The matchers.</param>
        /// <param name="register">true to register subscriptions, false to unregister them.</param>
        /// <param name="eventTopicHost">The event topic host.</param>
        public void ProcessSubscriber<TEventArgs>(string topic, object subscriber, EventHandler<TEventArgs> handlerMethod, IHandler handler, ISubscriptionMatcher[] matchers, bool register, IEventTopicHost eventTopicHost) where TEventArgs : EventArgs
        {
            Ensure.ArgumentNotNull(handlerMethod, "handlerMethod");

            this.HandleSubscriber(eventTopicHost, topic, register, subscriber, handlerMethod.Method, handler, matchers);
        }

        /// <summary>
        /// Handles the publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        /// <param name="register">true to register publications, false to unregister them.</param>
        /// <param name="eventInfo">The published event..</param>
        /// <param name="attr">The attribute</param>
        /// <param name="eventTopicHost">The event topic host.</param>
        private void HandlePublisher(
            object publisher, 
            bool register, 
            EventInfo eventInfo, 
            EventPublicationAttribute attr, 
            IEventTopicHost eventTopicHost)
        {
            IEventTopic topic = eventTopicHost.GetEventTopic(attr.Topic);
            if (register)
            {
                List<IPublicationMatcher> matchers = new List<IPublicationMatcher>();
                foreach (Type type in attr.MatcherTypes)
                {
                    matchers.Add(this.factory.CreatePublicationMatcher(type));
                }

                topic.AddPublication(
                    publisher, 
                    eventInfo, 
                    attr.HandlerRestriction, 
                    matchers);
            }
            else
            {
                topic.RemovePublication(publisher, eventInfo);
            }
        }

        /// <summary>
        /// Handles the subscriber.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="register">true to register subscriptions, false to unregister them.</param>
        /// <param name="methodInfo">The handler method.</param>
        /// <param name="attr">The subscription attribute.</param>
        /// <param name="eventTopicHost">The event topic host.</param>
        private void HandleSubscriber(
            object subscriber, 
            bool register, 
            MethodInfo methodInfo, 
            EventSubscriptionAttribute attr,
            IEventTopicHost eventTopicHost)
        {
            IEventTopic topic = eventTopicHost.GetEventTopic(attr.Topic);
            if (register)
            {
                List<ISubscriptionMatcher> matchers = new List<ISubscriptionMatcher>();
                foreach (Type type in attr.MatcherTypes)
                {
                    matchers.Add(this.factory.CreateSubscriptionMatcher(type));
                }

                topic.AddSubscription(
                    subscriber, 
                    methodInfo, 
                    this.factory.CreateHandler(attr.HandlerType), 
                    matchers);
            }
            else
            {
                topic.RemoveSubscription(subscriber, methodInfo);
            }
        }

        /// <summary>
        /// Handles the subscriber.
        /// </summary>
        /// <param name="eventTopicHost">The event topic host.</param>
        /// <param name="topic">The topic.</param>
        /// <param name="register">if set to <c>true</c> [register].</param>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">The handler method.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="matchers">The matchers.</param>
        private void HandleSubscriber(IEventTopicHost eventTopicHost, string topic, bool register, object subscriber, MethodInfo handlerMethod, IHandler handler, ISubscriptionMatcher[] matchers)
        {
            IEventTopic eventTopic = eventTopicHost.GetEventTopic(topic);
            if (register)
            {
                eventTopic.AddSubscription(subscriber, handlerMethod, handler, matchers);
            }
            else
            {
                eventTopic.RemoveSubscription(subscriber, handlerMethod);
            }
        }
    }
}