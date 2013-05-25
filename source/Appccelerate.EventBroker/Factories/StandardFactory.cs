//-------------------------------------------------------------------------------
// <copyright file="StandardFactory.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Appccelerate.EventBroker.Internals;
    using Appccelerate.EventBroker.Internals.GlobalMatchers;
    using Appccelerate.EventBroker.Internals.Inspection;
    using Appccelerate.EventBroker.Internals.Publications;
    using Appccelerate.EventBroker.Internals.Subscriptions;
    using Appccelerate.EventBroker.Matchers;
    using PropertyPublication = Appccelerate.EventBroker.Internals.Publications.PropertyPublication;

    /// <summary>
    /// Standard implementation for the <see cref="IFactory"/> interface.
    /// </summary>
    public class StandardFactory : IFactory
    {
        /// <summary>
        /// Gets the extension host holding all extensions.
        /// </summary>
        protected IExtensionHost ExtensionHost { get; private set; }

        /// <summary>
        /// Initializes this factory with the specified <paramref name="extensionHost"/> holding all extensions.
        /// </summary>
        /// <param name="extensionHost">The extension host holding all extensions (this is the event broker).</param>
        public virtual void Initialize(IExtensionHost extensionHost)
        {
            this.ExtensionHost = extensionHost;    
        }

        /// <summary>
        /// Creates an event topic host.
        /// </summary>
        /// <param name="globalMatchersProvider">The global matchers provider.</param>
        /// <returns>A newly created event topic host.</returns>
        public virtual IEventTopicHost CreateEventTopicHost(IGlobalMatchersProvider globalMatchersProvider)
        {
            return new EventTopicHost(this, this.ExtensionHost, globalMatchersProvider);
        }

        /// <summary>
        /// Creates an event inspector.
        /// </summary>
        /// <returns>A newly created event inspector.</returns>
        public virtual IEventInspector CreateEventInspector()
        {
            return new EventInspector(this.ExtensionHost);
        }

        /// <summary>
        /// Creates a new event topic
        /// </summary>
        /// <param name="uri">The URI of the event topic.</param>
        /// <param name="globalMatchersProvider">The global matchers provider.</param>
        /// <returns>A newly created event topic</returns>
        public virtual IEventTopic CreateEventTopicInternal(string uri, IGlobalMatchersProvider globalMatchersProvider)
        {
            return new EventTopic(uri, this.ExtensionHost, globalMatchersProvider);
        }

        /// <summary>
        /// Creates a new publication
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="eventInfo">The event info.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="publicationMatchers">The publication matchers.</param>
        /// <returns>A newly created publication</returns>
        public virtual IPublication CreatePublication(
            IEventTopicExecuter eventTopic, 
            object publisher, 
            EventInfo eventInfo, 
            HandlerRestriction handlerRestriction, 
            IList<IPublicationMatcher> publicationMatchers)
        {
            return new PropertyPublication(eventTopic, publisher, eventInfo, handlerRestriction, publicationMatchers);
        }

        /// <summary>
        /// Creates a new publication.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="eventHandler">The event handler.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="publicationMatchers">The matchers.</param>
        /// <returns>A newly created publication</returns>
        public virtual IPublication CreatePublication(
            IEventTopicExecuter eventTopic,
            object publisher,
            ref EventHandler eventHandler,
            HandlerRestriction handlerRestriction,
            IList<IPublicationMatcher> publicationMatchers)
        {
            return new CodePublication<EventArgs>(eventTopic, publisher, ref eventHandler, handlerRestriction, publicationMatchers);
        }

        /// <summary>
        /// Creates a new publication.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="eventHandler">The event handler.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="publicationMatchers">The matchers.</param>
        /// <returns>A newly created publication</returns>
        public virtual IPublication CreatePublication<TEventArgs>(
            IEventTopicExecuter eventTopic,
            object publisher,
            ref EventHandler<TEventArgs> eventHandler,
            HandlerRestriction handlerRestriction,
            IList<IPublicationMatcher> publicationMatchers) where TEventArgs : EventArgs
        {
            return new CodePublication<TEventArgs>(eventTopic, publisher, ref eventHandler, handlerRestriction, publicationMatchers);
        }

        public virtual ISubscription CreateSubscription(
            object subscriber,
            DelegateWrapper delegateWrapper,
            IHandler handler,
            IList<ISubscriptionMatcher> subscriptionMatchers)
        {
            return new Subscription(subscriber, delegateWrapper, handler, subscriptionMatchers, this.ExtensionHost);
        }

        /// <summary>
        /// Creates a subscription execution handler. This handler defines on which thread the subscription is executed.
        /// </summary>
        /// <param name="handlerType">Type of the handler.</param>
        /// <returns>A new subscription execution handler.</returns>
        public virtual IHandler CreateHandler(Type handlerType)
        {
            Ensure.ArgumentNotNull(handlerType, "handlerType");

            AssertIsHandler(handlerType);

            return this.ActivateHandler(handlerType);
        }

        /// <summary>
        /// Creates a publication matcher.
        /// </summary>
        /// <param name="matcherType">Type of the matcher.</param>
        /// <returns>
        /// A newly created publication scope matcher.
        /// </returns>
        public virtual IPublicationMatcher CreatePublicationMatcher(Type matcherType)
        {
            Ensure.ArgumentNotNull(matcherType, "matcherType");

            AssertIsPublicationMatcher(matcherType);

            return this.ActivatePublicationMatcher(matcherType);
        }

        /// <summary>
        /// Creates a subscription scope matcher.
        /// </summary>
        /// <param name="matcherType">Type of the subscription matcher.</param>
        /// <returns>
        /// A newly create subscription scope matcher.
        /// </returns>
        public virtual ISubscriptionMatcher CreateSubscriptionMatcher(Type matcherType)
        {
            Ensure.ArgumentNotNull(matcherType, "matcherType");

            AssertIsSubscriptionMatcher(matcherType);

            return this.ActivateSubscriptionMatcher(matcherType);
        }

        /// <summary>
        /// Creates the global matchers host.
        /// </summary>
        /// <returns>A newly created global matchers host.</returns>
        public virtual IGlobalMatchersHost CreateGlobalMatchersHost()
        {
            return new GlobalMatchersHost();
        }

        public virtual IEventRegistrar CreateRegistrar(IEventTopicHost eventTopicHost, IEventInspector eventInspector, IExtensionHost extensionHost)
        {
            return new Registrar(this, eventTopicHost, eventInspector, extensionHost);
        }

        /// <summary>
        /// Asserts that the given handler type implements <see cref="IHandler"/> and is a class.
        /// </summary>
        /// <param name="handlerType">Type of the handler to check.</param>
        protected static void AssertIsHandler(Type handlerType)
        {
            if (!handlerType.IsClass || !typeof(IHandler).IsAssignableFrom(handlerType))
            {
                throw new ArgumentException("handlerType '" + handlerType + "' has to be a class implementing Appccelerate.EventBroker.IHandler.");
            }
        }

        /// <summary>
        /// Asserts that the given matcher type implements <see cref="ISubscriptionMatcher"/> and is a class.
        /// </summary>
        /// <param name="subscriptionMatcherType">Type of the matcher to check.</param>
        protected static void AssertIsSubscriptionMatcher(Type subscriptionMatcherType)
        {
            if (!subscriptionMatcherType.IsClass || !typeof(ISubscriptionMatcher).IsAssignableFrom(subscriptionMatcherType))
            {
                throw new ArgumentException("MatcherType '" + subscriptionMatcherType + "' has to be a class implementing Appccelerate.EventBroker.Matchers.ISubscriptionMatcher.");
            }
        }

        /// <summary>
        /// Asserts that the given matcher type implements <see cref="IPublicationMatcher"/> and is a class.
        /// </summary>
        /// <param name="publicationMatcherType">Type of the matcher to check.</param>
        protected static void AssertIsPublicationMatcher(Type publicationMatcherType)
        {
            if (!publicationMatcherType.IsClass || !typeof(IPublicationMatcher).IsAssignableFrom(publicationMatcherType))
            {
                throw new ArgumentException("MatcherType '" + publicationMatcherType + "' has to be a class implementing Appccelerate.EventBroker.ScopeMatchers.IPublicationMatcher.");
            }
        }

        /// <summary>
        /// Creates a new instance of a subscription matcher type. 
        /// </summary>
        /// <remarks>Only called when subscription matcher assertions in 
        /// <see cref="AssertIsSubscriptionMatcher"/> were successful.</remarks>
        /// <param name="subscriptionMatcherType">The subscription matcher type.</param>
        /// <returns>A new instance of <paramref name="subscriptionMatcherType"/>.</returns>
        protected virtual ISubscriptionMatcher ActivateSubscriptionMatcher(Type subscriptionMatcherType)
        {
            return (ISubscriptionMatcher)Activator.CreateInstance(subscriptionMatcherType);
        }

        /// <summary>
        /// Creates a new instance of a publication matcher type. 
        /// </summary>
        /// <remarks>Only called when publication matcher assertions in 
        /// <see cref="AssertIsPublicationMatcher"/> were successful.</remarks>
        /// <param name="publicationMatcherType">The publication matcher type.</param>
        /// <returns>A new instance of <paramref name="publicationMatcherType"/>.</returns>
        protected virtual IPublicationMatcher ActivatePublicationMatcher(Type publicationMatcherType)
        {
            return (IPublicationMatcher)Activator.CreateInstance(publicationMatcherType);
        }

        /// <summary>
        /// Creates a new instance of a handler type. 
        /// </summary>
        /// <remarks>Only called when handler matcher assertions in 
        /// <see cref="AssertIsHandler"/> were successful.</remarks>
        /// <param name="handlerType">The handler type.</param>
        /// <returns>A new instance of <paramref name="handlerType"/>.</returns>
        protected virtual IHandler ActivateHandler(Type handlerType)
        {
            return (IHandler)Activator.CreateInstance(handlerType);
        }
    }
}