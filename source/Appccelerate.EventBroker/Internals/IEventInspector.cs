//-------------------------------------------------------------------------------
// <copyright file="IEventInspector.cs" company="Appccelerate">
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
    using Matchers;

    /// <summary>
    /// An event inspector inspects objects for publications and subscriptions and creates <see cref="IEventTopic"/>s
    /// for them on the <see cref="IEventTopicHost"/>
    /// </summary>
    public interface IEventInspector
    {
        /// <summary>
        /// Processes a publishers.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        /// <param name="register">true to register publications, false to unregister them.</param>
        /// <param name="eventTopicHost">The event topic host.</param>
        /// <remarks>Scans the members of the <paramref name="publisher"/> and registers or unregisters publications.</remarks>
        void ProcessPublisher(object publisher, bool register, IEventTopicHost eventTopicHost);

        /// <summary>
        /// Processes the subscriber.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="register">true to register subscriptions, false to unregister them.</param>
        /// <param name="eventTopicHost">The event topic host.</param>
        /// <remarks>Scans the members of the <paramref name="subscriber"/> and registers or unregisters subscriptions.</remarks>
        void ProcessSubscriber(object subscriber, bool register, IEventTopicHost eventTopicHost);

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
        void ProcessPublisher(string topic, object publisher, string eventName, HandlerRestriction handlerRestriction, IList<IPublicationMatcher> matchers, bool register, IEventTopicHost eventTopicHost);

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
        void ProcessSubscriber(string topic, object subscriber, EventHandler handlerMethod, IHandler handler, ISubscriptionMatcher[] matchers, bool register, IEventTopicHost eventTopicHost);

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
        void ProcessSubscriber<TEventArgs>(string topic, object subscriber, EventHandler<TEventArgs> handlerMethod, IHandler handler, ISubscriptionMatcher[] matchers, bool register, IEventTopicHost eventTopicHost) where TEventArgs : EventArgs;
    }
}