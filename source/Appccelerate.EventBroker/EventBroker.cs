//-------------------------------------------------------------------------------
// <copyright file="EventBroker.cs" company="Appccelerate">
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
    using System.IO;
    using System.Linq;

    using Appccelerate.EventBroker.Internals;
    using Appccelerate.EventBroker.Matchers;

    /// <summary>
    /// The <see cref="EventBroker"/> is the facade component to the event broker framework.
    /// It provides the registration and unregistration functionality for event publisher and subscribers.
    /// </summary>
    public class EventBroker : IEventBroker, IEventRegisterer, IExtensionHost
    {
        /// <summary>
        /// The inspector used to find publications and subscription within a class.
        /// </summary>
        private readonly IEventInspector eventInspector;
        
        /// <summary>
        /// The event topic host that holds all event topics of this event broker.
        /// </summary>
        private readonly IEventTopicHost eventTopicHost;
        
        /// <summary>
        /// The factory used to create event broker related instances.
        /// </summary>
        private readonly IFactory factory;

        /// <summary>
        /// List of all extensions for this event broker.
        /// </summary>
        private readonly List<IEventBrokerExtension> extensions = new List<IEventBrokerExtension>();

        private readonly IGlobalMatchersHost globalMatchersHost;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventBroker"/> class.
        /// The <see cref="StandardFactory"/> is used to create <see cref="IHandler"/>s <see cref="IPublicationMatcher"/>s and
        /// <see cref="ISubscriptionMatcher"/>s.
        /// </summary>
        public EventBroker() : this(new StandardFactory())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventBroker"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public EventBroker(IFactory factory)
        {
            this.factory = factory;

            this.factory.Initialize(this);

            this.globalMatchersHost = this.factory.CreateGlobalMatchersHost();
            this.eventTopicHost = this.factory.CreateEventTopicHost(this.globalMatchersHost);
            this.eventInspector = this.factory.CreateEventInspector();
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
            this.eventInspector.ProcessPublisher(item, true, this.eventTopicHost);
            this.eventInspector.ProcessSubscriber(item, true, this.eventTopicHost);
            
            if (item is IEventBrokerRegisterable)
            {
                ((IEventBrokerRegisterable)item).Register(this);
            }

            this.extensions.ForEach(extension => extension.RegisteredItem(item));
        }

        /// <summary>
        /// Unregisters the specified item from this event broker.
        /// </summary>
        /// <param name="item">The item to unregister.</param>
        public void Unregister(object item)
        {
            this.eventInspector.ProcessPublisher(item, false, this.eventTopicHost);
            this.eventInspector.ProcessSubscriber(item, false, this.eventTopicHost);

            if (item is IEventBrokerRegisterable)
            {
                ((IEventBrokerRegisterable)item).Unregister(this);
            }

            this.extensions.ForEach(extension => extension.UnregisteredItem(item));
        }

        /// <summary>
        /// Registers the event as publication.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="matchers">The matchers.</param>
        public void RegisterEvent(string topic, object publisher, string eventName, HandlerRestriction handlerRestriction, params IPublicationMatcher[] matchers)
        {
            if (publisher == null)
            {
                throw new ArgumentNullException("publisher", "publisher must not be null.");
            }

            this.eventInspector.ProcessPublisher(topic, publisher, eventName, handlerRestriction, matchers, true, this.eventTopicHost);
        }

        /// <summary>
        /// Registers a handler method.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">The handler method.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="matchers">The matchers.</param>
        public void RegisterHandlerMethod(string topic, object subscriber, EventHandler handlerMethod, IHandler handler, params ISubscriptionMatcher[] matchers)
        {
            this.eventInspector.ProcessSubscriber(topic, subscriber, handlerMethod, handler, matchers, true, this.eventTopicHost);
        }

        /// <summary>
        /// Registers a handler method.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="topic">The topic.</param>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">The handler method.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="matchers">The matchers.</param>
        public void RegisterHandlerMethod<TEventArgs>(string topic, object subscriber, EventHandler<TEventArgs> handlerMethod, IHandler handler, params ISubscriptionMatcher[] matchers) where TEventArgs : EventArgs
        {
            this.eventInspector.ProcessSubscriber(topic, subscriber, handlerMethod, handler, matchers, true, this.eventTopicHost);
        }

        /// <summary>
        /// Fires the specified topic directly on the <see cref="IEventBroker"/> without a real publisher.
        /// This is useful when temporarily created objects need to fire events.
        /// The event is fired globally but can be matched with <see cref="Matchers.ISubscriptionMatcher"/>.
        /// </summary>
        /// <param name="topic">The topic URI.</param>
        /// <param name="publisher">The publisher (for event flow and logging).</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="sender">The sender (passed to the event handler).</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void Fire(string topic, object publisher, HandlerRestriction handlerRestriction, object sender, EventArgs e)
        {
            IEventTopic eventTopic = this.eventTopicHost.GetEventTopic(topic);
            var spontaneousPublication = new SpontaneousPublication(eventTopic, publisher, e.GetType(), handlerRestriction, new List<IPublicationMatcher>());
            eventTopic.AddPublication(spontaneousPublication);
            
            eventTopic.Fire(
                    sender,
                    e,
                    spontaneousPublication);

            eventTopic.RemovePublication(spontaneousPublication);
            spontaneousPublication.Dispose();
        }

        /// <summary>
        /// Describes all event topics of this event broker:
        /// publications, subscriptions, names, thread options, scopes, event arguments.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void DescribeTo(TextWriter writer)
        {
            this.eventTopicHost.DescribeTo(writer);
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
            
            eventTopic.AddPublication(
                publisher,
                ref publishedEvent,
                handlerRestriction,
                matchers);
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

            eventTopic.AddPublication(
                publisher, 
                ref publishedEvent,
                handlerRestriction,
                matchers);
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

            eventTopic.RemovePublication(publisher, ref publishedEvent);
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

            eventTopic.RemovePublication(publisher, ref publishedEvent);
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
            IEventTopic eventTopic = this.eventTopicHost.GetEventTopic(topic);

            eventTopic.AddSubscription(
                subscriber, 
                handlerMethod.Method, 
                handler, 
                matchers != null ? new List<ISubscriptionMatcher>(matchers) : new List<ISubscriptionMatcher>());
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
            IEventTopic eventTopic = this.eventTopicHost.GetEventTopic(topic);

            eventTopic.AddSubscription(
                subscriber, 
                handlerMethod.Method, 
                handler, 
                matchers != null ? new List<ISubscriptionMatcher>(matchers) : new List<ISubscriptionMatcher>());
        }

        /// <summary>
        /// Removes a subscription.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">The handler method.</param>
        public void RemoveSubscription(string topic, object subscriber, EventHandler handlerMethod)
        {
            IEventTopic eventTopic = this.eventTopicHost.GetEventTopic(topic);

            eventTopic.RemoveSubscription(subscriber, handlerMethod.Method);
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
            IEventTopic eventTopic = this.eventTopicHost.GetEventTopic(topic);

            eventTopic.RemoveSubscription(subscriber, handlerMethod.Method);
        }

        /// <summary>
        /// Adds the specified extension. The extension will be considered in any future operation.
        /// </summary>
        /// <param name="extension">The extension.</param>
        public void AddExtension(IEventBrokerExtension extension)
        {
            this.extensions.Add(extension);
        }

        /// <summary>
        /// Removes the specified extension.
        /// </summary>
        /// <param name="extension">The extension.</param>
        public void RemoveExtension(IEventBrokerExtension extension)
        {
            this.extensions.Remove(extension);
        }

        /// <summary>
        /// Clears all extensions, including the default logger extension.
        /// </summary>
        public void ClearExtensions()
        {
            this.extensions.Clear();
        }

        /// <summary>
        /// Executes the specified action for all extensions.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        void IExtensionHost.ForEach(Action<IEventBrokerExtension> action)
        {
            this.extensions.ForEach(action);
        }

        /// <summary>
        /// Adds the global matcher.
        /// </summary>
        /// <param name="matcher">The matcher.</param>
        public void AddGlobalMatcher(IMatcher matcher)
        {
            this.globalMatchersHost.AddMatcher(matcher);
        }

        /// <summary>
        /// Removes the global matcher.
        /// </summary>
        /// <param name="matcher">The matcher.</param>
        public void RemoveGlobalMatcher(IMatcher matcher)
        {
            this.globalMatchersHost.RemoveMatcher(matcher);
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
        /// Implementation of the disposable pattern.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        /// <remarks>
        /// Unregisters the event handler of all topics
        /// </remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.eventTopicHost.Dispose();
            }
        }
    }
}
