//-------------------------------------------------------------------------------
// <copyright file="Registrar.cs" company="Appccelerate">
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
    using System.Linq;
    using System.Reflection;

    using Appccelerate.EventBroker.Internals.Exceptions;
    using Appccelerate.EventBroker.Internals.Inspection;
    using Appccelerate.EventBroker.Internals.Publications;
    using Appccelerate.EventBroker.Internals.Subscriptions;
    using Appccelerate.EventBroker.Matchers;
    using Appccelerate.Events;

    public class Registrar : IEventRegistrar
    {
        private readonly IFactory factory;

        private readonly IEventTopicHost eventTopicHost;

        private readonly IEventInspector eventInspector;

        private readonly IExtensionHost extensionHost;

        public Registrar(IFactory factory, IEventTopicHost eventTopicHost, IEventInspector eventInspector, IExtensionHost extensionHost)
        {
            this.factory = factory;
            this.eventTopicHost = eventTopicHost;
            this.eventInspector = eventInspector;
            this.extensionHost = extensionHost;
        }

        /// <summary>
        /// Registers the event as publication.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="matchers">The matchers.</param>
        public void AddPublication(string topic, object publisher, string eventName, HandlerRestriction handlerRestriction, params IPublicationMatcher[] matchers)
        {
            Ensure.ArgumentNotNull(publisher, "publisher");

            EventInfo eventInfo = this.eventInspector.ScanPublisherForEvent(publisher, eventName);

            IEventTopic eventTopic = this.eventTopicHost.GetEventTopic(topic);

            IPublication publication = this.factory.CreatePublication(
                eventTopic,
                publisher,
                eventInfo,
                handlerRestriction,
                matchers);

            eventTopic.AddPublication(publication);
        }

        public void RemovePublication(string topic, object publisher, string eventName)
        {
            IEventTopic eventTopic = this.eventTopicHost.GetEventTopic(topic);

            IPublication publication = eventTopic.RemovePublication(publisher, eventName);

            if (publication != null)
            {
                publication.Dispose();
            }
        }

        /// <summary>
        /// Registers an item with this event broker.
        /// </summary>
        /// <remarks>
        /// The item is scanned for publications and subscriptions and wired to the corresponding invokers and handlers.
        /// </remarks>
        /// <param name="item">Item to register with the event broker.</param>
        public void Register(object item)
        {
            ScanResult scanResult = this.eventInspector.Scan(item);

            this.RegisterPropertyPublications(item, scanResult);
            this.RegisterPropertySubscriptions(item, scanResult);

            this.CallRegisterIfRegisterableOn(item);

            this.extensionHost.ForEach(extension => extension.RegisteredItem(item));
        }

        /// <summary>
        /// Unregisters the specified item from this event broker.
        /// </summary>
        /// <param name="item">The item to unregister.</param>
        public void Unregister(object item)
        {
            ScanResult scanResult = this.eventInspector.Scan(item);

            this.UnregisterPropertyPublications(item, scanResult.Publications);
            this.UnregisterPropertySubscriptions(item, scanResult.Subscription);

            this.CallUnregisterIfRegisterableOn(item);

            this.extensionHost.ForEach(extension => extension.UnregisteredItem(item));
        }

        /// <summary>
        /// Adds a publication with no handler restriction. Use this to register publications by code instead of using attributes.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="publishedEvent">The published event of the <paramref name="publisher"/>.</param>
        /// <param name="matchers">The matchers.</param>
        public void AddPublication(string topic, object publisher, ref EventHandler publishedEvent, params IPublicationMatcher[] matchers)
        {
            this.AddPublication(topic, publisher, ref publishedEvent, HandlerRestriction.None, matchers);
        }

        /// <summary>
        /// Adds a publication. Use this to register publications by code instead of using attributes.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="publishedEvent">The published event of the <paramref name="publisher"/>.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="matchers">The matchers.</param>
        public void AddPublication(string topic, object publisher, ref EventHandler publishedEvent, HandlerRestriction handlerRestriction, params IPublicationMatcher[] matchers)
        {
            IEventTopic eventTopic = this.eventTopicHost.GetEventTopic(topic);

            IPublication publication = this.factory.CreatePublication(eventTopic, publisher, ref publishedEvent, handlerRestriction, matchers);

            eventTopic.AddPublication(publication);
        }

        /// <summary>
        /// Adds a publication with no handler restriction. Use this to register publications by code instead of using attributes.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="topic">The topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="publishedEvent">The published event.</param>
        /// <param name="matchers">The matchers.</param>
        public void AddPublication<TEventArgs>(string topic, object publisher, ref EventHandler<TEventArgs> publishedEvent, params IPublicationMatcher[] matchers) where TEventArgs : EventArgs
        {
            this.AddPublication(topic, publisher, ref publishedEvent, HandlerRestriction.None, matchers);
        }

        /// <summary>
        /// Adds a publication. Use this to register publications by code instead of using attributes.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="topic">The topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="publishedEvent">The published event.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="matchers">The matchers.</param>
        public void AddPublication<TEventArgs>(string topic, object publisher, ref EventHandler<TEventArgs> publishedEvent, HandlerRestriction handlerRestriction, params IPublicationMatcher[] matchers) where TEventArgs : EventArgs
        {
            IEventTopic eventTopic = this.eventTopicHost.GetEventTopic(topic);
            IPublication publication = this.factory.CreatePublication(eventTopic, publisher, ref publishedEvent, handlerRestriction, matchers);

            eventTopic.AddPublication(publication);
        }

        /// <summary>
        /// Removes a publication. Publications added with <see cref="AddPublication(string,object,ref EventHandler,HandlerRestriction,IPublicationMatcher[])"/> have to be removed in order that the event broker can be disposed.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="publishedEvent">The published event.</param>
        public void RemovePublication(string topic, object publisher, ref EventHandler publishedEvent)
        {
            IEventTopic eventTopic = this.eventTopicHost.GetEventTopic(topic);

            IPublication publication = eventTopic.RemovePublication(publisher, CodePublication<EventArgs>.EventNameOfCodePublication);

            var codePublication = publication as CodePublication<EventArgs>;
            if (codePublication != null)
            {
                codePublication.Unregister(ref publishedEvent);
            }
        }

        /// <summary>
        /// Removes a publication. Publications added with <see cref="AddPublication(string,object,ref EventHandler,HandlerRestriction,IPublicationMatcher[])"/> have to be removed in order that the event broker can be disposed.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="publishedEvent">The published event.</param>
        public void RemovePublication<TEventArgs>(string topic, object publisher, ref EventHandler<TEventArgs> publishedEvent) where TEventArgs : EventArgs
        {
            IEventTopic eventTopic = this.eventTopicHost.GetEventTopic(topic);

            IPublication publication = eventTopic.RemovePublication(publisher, CodePublication<TEventArgs>.EventNameOfCodePublication);

            var codePublication = publication as CodePublication<TEventArgs>;
            if (codePublication != null)
            {
                codePublication.Unregister(ref publishedEvent);
            }
        }

        /// <summary>
        /// Adds a subscription. Use this to register subscriptions by code instead of using attributes.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">The handler method.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="matchers">The subscription matchers.</param>
        public void AddSubscription(string topic, object subscriber, EventHandler handlerMethod, IHandler handler, params ISubscriptionMatcher[] matchers)
        {
            Ensure.ArgumentNotNull(handlerMethod, "handlerMethod");

            this.AddSubscription(topic, subscriber, handler, matchers, handlerMethod.Method);
        }

        /// <summary>
        /// Adds a subscription. Use this to register subscriptions by code instead of using attributes.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="topic">The topic.</param>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">The handler method.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="matchers">The subscription matchers.</param>
        public void AddSubscription<TEventArgs>(string topic, object subscriber, EventHandler<TEventArgs> handlerMethod, IHandler handler, params ISubscriptionMatcher[] matchers) where TEventArgs : EventArgs
        {
            Ensure.ArgumentNotNull(handler, "handler");
            Ensure.ArgumentNotNull(handlerMethod, "handlerMethod");

            IEventTopic eventTopic = this.eventTopicHost.GetEventTopic(topic);

            handler.Initialize(subscriber, handlerMethod.Method, this.extensionHost);

            DelegateWrapper delegateWrapper = GetDelegateWrapper(handlerMethod.Method);
            ISubscription subscription = this.factory.CreateSubscription(
                subscriber,
                delegateWrapper,
                handler,
                matchers != null ? new List<ISubscriptionMatcher>(matchers) : new List<ISubscriptionMatcher>());

            eventTopic.AddSubscription(subscription);
        }

        public void AddSubscription(string topic, object subscriber, Action<EventArgs> handlerMethod, IHandler handler, params ISubscriptionMatcher[] matchers)
        {
            Ensure.ArgumentNotNull(handlerMethod, "handlerMethod");

            this.AddSubscription(topic, subscriber, handler, matchers, handlerMethod.Method);
        }

        public void AddSubscription<TEventArgValue>(string topic, object subscriber, Action<TEventArgValue> handlerMethod, IHandler handler, params ISubscriptionMatcher[] matchers)
        {
            Ensure.ArgumentNotNull(handlerMethod, "handlerMethod");

            this.AddSubscription(topic, subscriber, handler, matchers, handlerMethod.Method);
        }

        public void AddSubscription(string topic, object subscriber, Action handlerMethod, IHandler handler, params ISubscriptionMatcher[] matchers)
        {
            Ensure.ArgumentNotNull(handlerMethod, "handlerMethod");

            this.AddSubscription(topic, subscriber, handler, matchers, handlerMethod.Method);
        }

        /// <summary>
        /// Removes a subscription.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">The handler method.</param>
        public void RemoveSubscription(string topic, object subscriber, EventHandler handlerMethod)
        {
            Ensure.ArgumentNotNull(handlerMethod, "handlerMethod");

            this.RemoveSubscription(topic, subscriber, handlerMethod.Method);
        }

        /// <summary>
        /// Removes a subscription.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="topic">The topic.</param>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">The handler method.</param>
        public void RemoveSubscription<TEventArgs>(string topic, object subscriber, EventHandler<TEventArgs> handlerMethod) where TEventArgs : EventArgs
        {
            Ensure.ArgumentNotNull(handlerMethod, "handlerMethod");

            IEventTopic eventTopic = this.eventTopicHost.GetEventTopic(topic);

            eventTopic.RemoveSubscription(subscriber, handlerMethod.Method);
        }

        public void RemoveSubscription(string topic, object subscriber, Action<EventArgs> handlerMethod)
        {
            Ensure.ArgumentNotNull(handlerMethod, "handlerMethod");

            this.RemoveSubscription(topic, subscriber, handlerMethod.Method);
        }

        public void RemoveSubscription(string topic, object subscriber, Action handlerMethod)
        {
            Ensure.ArgumentNotNull(handlerMethod, "handlerMethod");

            this.RemoveSubscription(topic, subscriber, handlerMethod.Method);
        }

        public void RemoveSubscription<TEventArgValue>(string topic, object subscriber, Action<TEventArgValue> handlerMethod)
        {
            Ensure.ArgumentNotNull(handlerMethod, "handlerMethod");

            this.RemoveSubscription(topic, subscriber, handlerMethod.Method);
        }

        private static void CheckHandlerMethodIsNotStatic(MethodInfo handlerMethod)
        {
            if (handlerMethod.IsStatic)
            {
                throw new StaticSubscriberHandlerException(handlerMethod);
            }
        }

        private static DelegateWrapper GetDelegateWrapper(MethodInfo handlerMethod)
        {
            CheckHandlerMethodIsNotStatic(handlerMethod);

            ParameterInfo[] parameters = handlerMethod.GetParameters();
            bool hasSenderAndMatchingEventArgs = parameters.Length == 2 && parameters[0].ParameterType == typeof(object)
                                                 && typeof(EventArgs).IsAssignableFrom(parameters[1].ParameterType);

            bool hasOnlyEventArgs = parameters.Length == 1 && typeof(EventArgs).IsAssignableFrom(parameters[0].ParameterType);

            bool hasOnlyUnwrappedEventArgs = parameters.Length == 1;

            bool hasNoArguments = parameters.Length == 0;

            ParameterInfo parameterInfo = handlerMethod.GetParameters().LastOrDefault();

            if (hasSenderAndMatchingEventArgs)
            {
                return new SenderAndEventArgsDelegateWrapper(parameterInfo.ParameterType, handlerMethod);
            }

            if (hasOnlyEventArgs)
            {
                return new EventArgsOnlyDelegateWrapper(parameterInfo.ParameterType, handlerMethod);
            }

            if (hasOnlyUnwrappedEventArgs)
            {
                return (DelegateWrapper)Activator.CreateInstance(
                    typeof(UnwrappedEventArgsOnlyDelegateWrapper<>).MakeGenericType(handlerMethod.GetParameters().Single().ParameterType),
                    typeof(EventArgs<>).MakeGenericType(parameterInfo.ParameterType),
                    handlerMethod);
            }

            if (hasNoArguments)
            {
                return new NoArgumentsDelegateWrapper(handlerMethod);
            }

            throw new InvalidSubscriptionSignatureException(handlerMethod);
        }

        private void AddSubscription(string topic, object subscriber, IHandler handler, IEnumerable<ISubscriptionMatcher> matchers, MethodInfo methodInfo)
        {
            IEventTopic eventTopic = this.eventTopicHost.GetEventTopic(topic);

            handler.Initialize(subscriber, methodInfo, this.extensionHost);

            DelegateWrapper delegateWrapper = GetDelegateWrapper(methodInfo);
            ISubscription subscription = this.factory.CreateSubscription(
                subscriber,
                delegateWrapper,
                handler,
                matchers != null ? new List<ISubscriptionMatcher>(matchers) : new List<ISubscriptionMatcher>());

            eventTopic.AddSubscription(subscription);
        }

        private void RemoveSubscription(string topic, object subscriber, MethodInfo methodInfo)
        {
            IEventTopic eventTopic = this.eventTopicHost.GetEventTopic(topic);

            eventTopic.RemoveSubscription(subscriber, methodInfo);
        }

        private void RegisterPropertyPublications(object item, ScanResult scanResult)
        {
            foreach (PropertyPublicationScanResult propertyPublication in scanResult.Publications)
            {
                var publicationMatchers = from publicationMatcherType in propertyPublication.PublicationMatcherTypes
                                          select this.factory.CreatePublicationMatcher(publicationMatcherType);

                IEventTopic eventTopic = this.eventTopicHost.GetEventTopic(propertyPublication.Topic);

                IPublication publication = this.factory.CreatePublication(
                    eventTopic,
                    item,
                    propertyPublication.Event,
                    propertyPublication.HandlerRestriction,
                    publicationMatchers.ToList());

                eventTopic.AddPublication(publication);
            }
        }

        private void RegisterPropertySubscriptions(object item, ScanResult scanResult)
        {
            foreach (PropertySubscriptionScanResult propertySubscription in scanResult.Subscription)
            {
                IEventTopic eventTopic = this.eventTopicHost.GetEventTopic(propertySubscription.Topic);

                var subscriptionMatchers = from subscriptionMatcherType in propertySubscription.SubscriptionMatcherTypes
                                           select this.factory.CreateSubscriptionMatcher(subscriptionMatcherType);

                var handler = this.factory.CreateHandler(propertySubscription.HandlerType);
                handler.Initialize(item, propertySubscription.Method, this.extensionHost);

                DelegateWrapper delegateWrapper = GetDelegateWrapper(propertySubscription.Method);
                ISubscription subscription = this.factory.CreateSubscription(item, delegateWrapper, handler, subscriptionMatchers.ToList());

                eventTopic.AddSubscription(subscription);
            }
        }

        private void UnregisterPropertyPublications(object publisher, IEnumerable<PropertyPublicationScanResult> propertyPublications)
        {
            foreach (PropertyPublicationScanResult propertyPublication in propertyPublications)
            {
                IEventTopic topic = this.eventTopicHost.GetEventTopic(propertyPublication.Topic);

                IPublication publication = topic.RemovePublication(publisher, propertyPublication.Event.Name);

                if (publication != null)
                {
                    publication.Dispose();
                }
            }
        }

        private void UnregisterPropertySubscriptions(object subscriber, IEnumerable<PropertySubscriptionScanResult> propertySubscriptions)
        {
            foreach (PropertySubscriptionScanResult propertySubscription in propertySubscriptions)
            {
                IEventTopic topic = this.eventTopicHost.GetEventTopic(propertySubscription.Topic);

                topic.RemoveSubscription(subscriber, propertySubscription.Method);
            }
        }

        private void CallRegisterIfRegisterableOn(object item)
        {
            var eventBrokerRegisterable = item as IEventBrokerRegisterable;
            if (eventBrokerRegisterable != null)
            {
                eventBrokerRegisterable.Register(this);
            }
        }

        private void CallUnregisterIfRegisterableOn(object item)
        {
            var eventBrokerRegisterable = item as IEventBrokerRegisterable;
            if (eventBrokerRegisterable != null)
            {
                eventBrokerRegisterable.Unregister(this);
            }
        }
    }
}