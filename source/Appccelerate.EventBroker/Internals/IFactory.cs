//-------------------------------------------------------------------------------
// <copyright file="IFactory.cs" company="Appccelerate">
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
    using System.Reflection;

    using Appccelerate.EventBroker.Internals.GlobalMatchers;
    using Appccelerate.EventBroker.Internals.Subscriptions;
    using Appccelerate.EventBroker.Matchers;

    /// <summary>
    /// Factory for creating subscription execution handlers and scope matcher.
    /// </summary>
    /// <remarks>
    /// The factory that is used by an <see cref="IEventBroker"/> can be set on the constructor of the event broker.
    /// </remarks>
    public interface IFactory
    {
        /// <summary>
        /// Initializes this factory with the specified <paramref name="extensionHost"/> holding all extensions.
        /// </summary>
        /// <param name="extensionHost">The extension host holding all extensions (this is the event broker).</param>
        void Initialize(IExtensionHost extensionHost);

        /// <summary>
        /// Creates an event topic host.
        /// </summary>
        /// <param name="globalMatchersProvider">The global matchers provider.</param>
        /// <returns>A newly created event topic host.</returns>
        IEventTopicHost CreateEventTopicHost(IGlobalMatchersProvider globalMatchersProvider);

        IEventInspector CreateEventInspector();

        /// <summary>
        /// Creates a new publication
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="eventInfo">The event info.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="publicationMatchers">The publication matchers.</param>
        /// <returns>A newly created publication</returns>
        IPublication CreatePublication(IEventTopicExecuter eventTopic, object publisher, EventInfo eventInfo, HandlerRestriction handlerRestriction, IList<IPublicationMatcher> publicationMatchers);

        /// <summary>
        /// Creates a new publication
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="eventHandler">The event handler.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="publicationMatchers">The publication matchers.</param>
        /// <returns>A newly created publication</returns>
        IPublication CreatePublication(IEventTopicExecuter eventTopic, object publisher, ref EventHandler eventHandler, HandlerRestriction handlerRestriction, IList<IPublicationMatcher> publicationMatchers);

        /// <summary>
        /// Creates a new publication
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="eventHandler">The event handler.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="publicationMatchers">The publication matchers.</param>
        /// <returns>A newly created publication</returns>
        IPublication CreatePublication<TEventArgs>(IEventTopicExecuter eventTopic, object publisher, ref EventHandler<TEventArgs> eventHandler, HandlerRestriction handlerRestriction, IList<IPublicationMatcher> publicationMatchers) where TEventArgs : EventArgs;

        ISubscription CreateSubscription(object subscriber, DelegateWrapper delegateWrapper, IHandler handler, IList<ISubscriptionMatcher> subscriptionMatchers);

        /// <summary>
        /// Creates a subscription execution handler. This handler defines on which thread the subscription is executed.
        /// </summary>
        /// <param name="handlerType">Type of the handler.</param>
        /// <returns>A new subscription execution handler.</returns>
        IHandler CreateHandler(Type handlerType);

        /// <summary>
        /// Creates a publication matcher.
        /// </summary>
        /// <param name="matcherType">Type of the matcher.</param>
        /// <returns>A newly created publication matcher.</returns>
        IPublicationMatcher CreatePublicationMatcher(Type matcherType);

        /// <summary>
        /// Creates a subscription matcher.
        /// </summary>
        /// <param name="matcherType">Type of the subscription matcher.</param>
        /// <returns>A newly create subscription matcher.</returns>
        ISubscriptionMatcher CreateSubscriptionMatcher(Type matcherType);

        /// <summary>
        /// Creates the global matchers host.
        /// </summary>
        /// <returns>A newly created global matchers host.</returns>
        IGlobalMatchersHost CreateGlobalMatchersHost();

        /// <summary>
        /// Creates a new event topic
        /// </summary>
        /// <param name="uri">The URI of the event topic.</param>
        /// <param name="globalMatchersProvider">The global matchers provider.</param>
        /// <returns>A newly created event topic</returns>
        IEventTopic CreateEventTopicInternal(string uri, IGlobalMatchersProvider globalMatchersProvider);

        IEventRegistrar CreateRegistrar(IEventTopicHost eventTopicHost, IEventInspector eventInspector, IExtensionHost extensionHost);
    }
}