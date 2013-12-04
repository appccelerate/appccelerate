//-------------------------------------------------------------------------------
// <copyright file="EventBroker.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Appccelerate.EventBroker.Factories;
    using Appccelerate.EventBroker.Internals;
    using Appccelerate.EventBroker.Internals.GlobalMatchers;
    using Appccelerate.EventBroker.Internals.Publications;
    using Appccelerate.EventBroker.Matchers;

    /// <summary>
    /// The <see cref="EventBroker"/> is the facade component to the event broker framework.
    /// It provides the registration and deregistration functionality for event publisher and subscribers.
    /// </summary>
    public class EventBroker : IEventBroker, IExtensionHost
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

        private readonly IEventRegistrar registrar;

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
            Ensure.ArgumentNotNull(factory, "factory");

            this.factory = factory;

            this.factory.Initialize(this);

            this.globalMatchersHost = this.factory.CreateGlobalMatchersHost();
            this.eventTopicHost = this.factory.CreateEventTopicHost(this.globalMatchersHost);
            this.eventInspector = this.factory.CreateEventInspector();

            this.registrar = this.factory.CreateRegistrar(this.eventTopicHost, this.eventInspector, this);
        }

        public IRegistrar SpecialCasesRegistrar
        {
            get { return this.registrar; }
        }

        public void Register(object item)
        {
            this.registrar.Register(item);
        }

        public void Unregister(object item)
        {
            this.registrar.Unregister(item);
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
        /// <param name="eventArgs">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void Fire(string topic, object publisher, HandlerRestriction handlerRestriction, object sender, EventArgs eventArgs)
        {
            Ensure.ArgumentNotNull(eventArgs, "eventArgs");

            IEventTopic eventTopic = this.eventTopicHost.GetEventTopic(topic);
            using (var spontaneousPublication = new SpontaneousPublication(eventTopic, publisher, eventArgs.GetType(), handlerRestriction, new List<IPublicationMatcher>()))
            {
                eventTopic.AddPublication(spontaneousPublication);

                eventTopic.Fire(sender, eventArgs, spontaneousPublication);

                eventTopic.RemovePublication(publisher, SpontaneousPublication.SpontaneousEventName);
            }
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
