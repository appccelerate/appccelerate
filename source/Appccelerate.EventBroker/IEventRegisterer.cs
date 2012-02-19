//-------------------------------------------------------------------------------
// <copyright file="IEventRegisterer.cs" company="Appccelerate">
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
    using Matchers;

    /// <summary>
    /// This interface is passed to the registered publishers and subscribers so that they can register publications
    /// and subscriptions by code.
    /// </summary>
    public interface IEventRegisterer
    {
        /// <summary>
        /// Adds a publication with no handler restriction. Use this to register publications by code instead of using attributes.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="publishedEvent">The published event of the <paramref name="publisher"/>.</param>>
        /// <param name="matchers">The matchers.</param>
        void AddPublication(string topic, object publisher, ref EventHandler publishedEvent, params IPublicationMatcher[] matchers);

        /// <summary>
        /// Adds a publication. Use this to register publications by code instead of using attributes.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="publishedEvent">The published event of the <paramref name="publisher"/>.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="matchers">The matchers.</param>
        void AddPublication(string topic, object publisher, ref EventHandler publishedEvent, HandlerRestriction handlerRestriction, params IPublicationMatcher[] matchers);

        /// <summary>
        /// Adds a publication with no handler restriction. Use this to register publications by code instead of using attributes.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event args.</typeparam>
        /// <param name="topic">The topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="publishedEvent">The published event.</param>
        /// <param name="matchers">The matchers.</param>
        void AddPublication<TEventArgs>(string topic, object publisher, ref EventHandler<TEventArgs> publishedEvent, params IPublicationMatcher[] matchers) where TEventArgs : EventArgs;

        /// <summary>
        /// Adds a publication. Use this to register publications by code instead of using attributes.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event args.</typeparam>
        /// <param name="topic">The topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="publishedEvent">The published event.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="matchers">The matchers.</param>
        void AddPublication<TEventArgs>(string topic, object publisher, ref EventHandler<TEventArgs> publishedEvent, HandlerRestriction handlerRestriction, params IPublicationMatcher[] matchers) where TEventArgs : EventArgs;

        /// <summary>
        /// Removes a publication. Publications added have to be removed in order that the event broker can be disposed.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="publishedEvent">The published event.</param>
        void RemovePublication(string topic, object publisher, ref EventHandler publishedEvent);
        
        void RemovePublication<TEventArgs>(string topic, object publisher, ref EventHandler<TEventArgs> publishedEvent) where TEventArgs : EventArgs;

        /// <summary>
        /// Adds a subscription. Use this to register subscriptions by code instead of using attributes.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">The handler method.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="matchers">The subscription matchers.</param>
        void AddSubscription(string topic, object subscriber, EventHandler handlerMethod, IHandler handler, params ISubscriptionMatcher[] matchers);

        /// <summary>
        /// Adds a subscription. Use this to register subscriptions by code instead of using attributes.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event args.</typeparam>
        /// <param name="topic">The topic.</param>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">The handler method.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="matchers">The subscription matchers.</param>
        void AddSubscription<TEventArgs>(string topic, object subscriber, EventHandler<TEventArgs> handlerMethod, IHandler handler, params ISubscriptionMatcher[] matchers) where TEventArgs : EventArgs;

        /// <summary>
        /// Removes a subscription.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">The handler method.</param>
        void RemoveSubscription(string topic, object subscriber, EventHandler handlerMethod);

        /// <summary>
        /// Removes a subscription.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event args.</typeparam>
        /// <param name="topic">The topic.</param>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">The handler method.</param>
        void RemoveSubscription<TEventArgs>(string topic, object subscriber, EventHandler<TEventArgs> handlerMethod) where TEventArgs : EventArgs;

        /// <summary>
        /// Registers the event as publication.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="matchers">The matchers.</param>
        void RegisterEvent(string topic, object publisher, string eventName, HandlerRestriction handlerRestriction, params IPublicationMatcher[] matchers);

        /// <summary>
        /// Registers a handler method.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">The handler method.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="matchers">The matchers.</param>
        void RegisterHandlerMethod(string topic, object subscriber, EventHandler handlerMethod, IHandler handler, params ISubscriptionMatcher[] matchers);

        /// <summary>
        /// Registers a handler method.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="topic">The topic.</param>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">The handler method.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="matchers">The matchers.</param>
        void RegisterHandlerMethod<TEventArgs>(string topic, object subscriber, EventHandler<TEventArgs> handlerMethod, IHandler handler, params ISubscriptionMatcher[] matchers) where TEventArgs : EventArgs;

        /// <summary>
        /// Registers an item with this event broker.
        /// </summary>
        /// <remarks>
        /// The item is scanned for publications and subscriptions and wired to the corresponding invokers and handlers.
        /// </remarks>
        /// <param name="item">Item to register with the event broker.</param>
        void Register(object item);

        /// <summary>
        /// Unregisters the specified item from this event broker.
        /// </summary>
        /// <param name="item">The item to unregister.</param>
        void Unregister(object item);
    }
}