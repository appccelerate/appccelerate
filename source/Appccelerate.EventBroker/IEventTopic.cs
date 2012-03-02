//-------------------------------------------------------------------------------
// <copyright file="IEventTopic.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Appccelerate.EventBroker.Internals;
    using Appccelerate.EventBroker.Matchers;

    /// <summary>
    /// Represents a point of communication on a certain topic between the topic publishers and the topic subscribers.
    /// </summary>
    public interface IEventTopic : IDisposable, IEventTopicInfo
    {
        /// <summary>
        /// Adds a publication to the topic.
        /// </summary>
        /// <param name="publisher">The object that publishes the event that will fire the topic.</param>
        /// <param name="eventInfo">The <see cref="EventInfo"/> of the publisher that registers this event topic.</param>
        /// <param name="handlerRestriction">The subscriber handler restriction.</param>
        /// <param name="matchers">The matchers for the publication.</param>
        void AddPublication(object publisher, EventInfo eventInfo, HandlerRestriction handlerRestriction, IList<IPublicationMatcher> matchers);

        /// <summary>
        /// Adds a publication to the topic.
        /// </summary>
        /// <param name="publisher">The object that publishes the event that will fire the topic.</param>
        /// <param name="eventHandler">The event handler that will fire the topic.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="matchers">The matchers.</param>
        void AddPublication(object publisher, ref EventHandler eventHandler, HandlerRestriction handlerRestriction, IList<IPublicationMatcher> matchers);

        /// <summary>
        /// Adds a publication to the topic.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments of the event handler.</typeparam>
        /// <param name="publisher">The object that publishes the event that will fire the topic.</param>
        /// <param name="eventHandler">The event handler that will fire the topic.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="matchers">The matchers.</param>
        void AddPublication<TEventArgs>(object publisher, ref EventHandler<TEventArgs> eventHandler, HandlerRestriction handlerRestriction, IList<IPublicationMatcher> matchers) where TEventArgs : EventArgs;

        /// <summary>
        /// Removes a publication from the topic.
        /// </summary>
        /// <param name="publisher">The object that contains the publication.</param>
        /// <param name="eventInfo">The event on the publisher that fires the topic.</param>
        void RemovePublication(object publisher, EventInfo eventInfo);

        /// <summary>
        /// Removes a publication from the topic.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        /// <param name="publishedEvent">The published event.</param>
        void RemovePublication(object publisher, ref EventHandler publishedEvent);

        /// <summary>
        /// Removes a publication from the topic.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        /// <param name="publishedEvent">The published event.</param>
        void RemovePublication<TEventArgs>(object publisher, ref EventHandler<TEventArgs> publishedEvent) where TEventArgs : EventArgs;

        /// <summary>
        /// Adds a subscription to this <see cref="EventTopic"/>.
        /// </summary>
        /// <param name="subscriber">The object that contains the method that will handle the <see cref="EventTopic"/>.</param>
        /// <param name="handlerMethod">The method on the subscriber that will handle the <see cref="EventTopic"/>.
        /// Use this overload when there are several methods with the same name on the subscriber.</param>
        /// <param name="handler">The handler that is used to execute the subscription.</param>
        /// <param name="subscriptionMatchers">Matchers for the subscription.</param>
        void AddSubscription(
            object subscriber, 
            MethodInfo handlerMethod, 
            IHandler handler, 
            IList<ISubscriptionMatcher> subscriptionMatchers);
        
        /// <summary>
        /// Removes a subscription from this <see cref="IEventTopic"/>.
        /// </summary>
        /// <param name="subscriber">The object that contains the method that will handle the <see cref="IEventTopic"/>.</param>
        /// <param name="handlerMethod">The method on the subscriber that will handle the <see cref="IEventTopic"/>.</param>
        void RemoveSubscription(object subscriber, MethodInfo handlerMethod);

        /// <summary>
        /// Adds the publication.
        /// </summary>
        /// <param name="publication">The publication.</param>
        void AddPublication(IPublication publication);

        void RemovePublication(IPublication publication);
    }
}