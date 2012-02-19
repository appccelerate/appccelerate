//-------------------------------------------------------------------------------
// <copyright file="EventTopic.cs" company="Appccelerate">
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
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Exceptions;
    using Matchers;

    /// <summary>
    /// Represents a point of communication on a certain topic between the topic publishers and the topic subscribers.
    /// </summary>
    internal class EventTopic : IEventTopic
    {
        /// <summary>
        /// The URI that identifies this event topic uniquely on an event broker.
        /// </summary>
        private readonly string uri;

        /// <summary>
        /// Factory to create publications and subscriptions.
        /// </summary>
        private readonly IFactory factory;

        /// <summary>
        /// Extension host holding all extensions.
        /// </summary>
        private readonly IExtensionHost extensionHost;

        private readonly IGlobalMatchersProvider globalMatchersProvider;

        /// <summary>
        /// List of all publications that fire this event topic.
        /// </summary>
        private List<IPublication> publications = new List<IPublication>();

        /// <summary>
        /// List of all subscriptions that listen to this event topic.
        /// </summary>
        private List<ISubscription> subscriptions = new List<ISubscription>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTopic"/> class.
        /// </summary>
        /// <param name="uri">The topic URI.</param>
        /// <param name="factory">Factory to create publications and subscriptions.</param>
        /// <param name="extensionHost">The extension host holding all extensions.</param>
        /// <param name="globalMatchersProvider">The global matchers provider.</param>
        public EventTopic(string uri, IFactory factory, IExtensionHost extensionHost, IGlobalMatchersProvider globalMatchersProvider)
        {
            this.uri = uri;
            this.factory = factory;
            this.extensionHost = extensionHost;
            this.globalMatchersProvider = globalMatchersProvider;
        }

        /// <summary>
        /// Gets the topic URI.
        /// </summary>
        /// <value>The topic URI.</value>
        public string Uri
        {
            get { return this.uri; }
        }

        /// <summary>
        /// Gets the publications for the event topic.
        /// </summary>
        /// <remarks>The publications are frequently cleaned internally when
        /// necessary. Therefore the publications are only valid at the time
        /// when they are requested and should not be cached or referenced longer
        /// then necessary.</remarks>
        public IEnumerable<IPublication> Publications
        {
            get
            {
                this.Clean();
                return new ReadOnlyCollection<IPublication>(this.publications);
            }
        }

        /// <summary>
        /// Gets the subscriptions for the event topic.
        /// </summary>
        /// <remarks>The subscriptions are frequently cleaned internally when
        /// necessary. Therefore the subscriptions are only valid at the time
        /// when they are requested and should not be cached or referenced longer
        /// then necessary.</remarks>
        public IEnumerable<ISubscription> Subscriptions
        {
            get
            {
                this.Clean();
                return new ReadOnlyCollection<ISubscription>(this.subscriptions);
            }
        }

        /// <summary>
        /// Adds a publication to the topic.
        /// </summary>
        /// <param name="publisher">The object that publishes the event that will fire the topic.</param>
        /// <param name="eventInfo">The <see cref="EventInfo"/> of the publisher that registers this event topic.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="matchers">Matchers for publication.</param>
        public void AddPublication(
            object publisher,
            EventInfo eventInfo,
            HandlerRestriction handlerRestriction,
            IList<IPublicationMatcher> matchers)
        {
            IPublication publication = this.factory.CreatePublication(this, publisher, eventInfo, handlerRestriction, matchers);
            this.Clean();
            this.ThrowIfRepeatedPublication(publisher, eventInfo.Name);
            this.AddPublication(publication);
        }

        /// <summary>
        /// Adds a publication to the topic.
        /// </summary>
        /// <param name="publisher">The object that publishes the event that will fire the topic.</param>
        /// <param name="eventHandler">The event handler that will fire the topic.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="matchers">The matchers.</param>
        public void AddPublication(
            object publisher,
            ref EventHandler eventHandler,
            HandlerRestriction handlerRestriction,
            IList<IPublicationMatcher> matchers)
        {
            IPublication publication = this.factory.CreatePublication(this, publisher, ref eventHandler, handlerRestriction, matchers);
            this.Clean();
            this.AddPublication(publication);
        }

        /// <summary>
        /// Adds a publication to the topic.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments of the event handler.</typeparam>
        /// <param name="publisher">The object that publishes the event that will fire the topic.</param>
        /// <param name="eventHandler">The event handler that will fire the topic.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="matchers">The matchers.</param>
        public void AddPublication<TEventArgs>(
            object publisher, 
            ref EventHandler<TEventArgs> eventHandler, 
            HandlerRestriction handlerRestriction,
            IList<IPublicationMatcher> matchers) where TEventArgs : EventArgs
        {
            IPublication publication = this.factory.CreatePublication(this, publisher, ref eventHandler, handlerRestriction, matchers);
            this.Clean();
            this.AddPublication(publication);
        }

        /// <summary>
        /// Removes a publication from the topic.
        /// </summary>
        /// <param name="publisher">The object that contains the publication.</param>
        /// <param name="eventInfo">The event on the publisher that fires the topic.</param>
        public void RemovePublication(object publisher, EventInfo eventInfo)
        {
            this.Clean();
            IPublication publication = this.FindPublication(publisher, eventInfo.Name);
            if (publication != null)
            {
                lock (this)
                {
                    List<IPublication> newPublications = new List<IPublication>();
                    foreach (IPublication p in this.publications)
                    {
                        if (p != publication)
                        {
                            newPublications.Add(p);
                        }
                    }
                    
                    publication.Dispose();

                    this.publications = newPublications;

                    this.extensionHost.ForEach(extension => extension.RemovedPublication(this, publication));
                }
            }
        }

        /// <summary>
        /// Removes a publication from the topic.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        /// <param name="publishedEvent">The published event.</param>
        public void RemovePublication(object publisher, ref EventHandler publishedEvent)
        {
            this.Clean();
            IPublication publication = this.FindPublication(publisher, CodePublication<EventArgs>.EventNameOfCodePublication);
            if (publication != null)
            {
                lock (this)
                {
                    List<IPublication> newPublications = new List<IPublication>();
                    foreach (IPublication p in this.publications)
                    {
                        if (p != publication)
                        {
                            newPublications.Add(p);
                        }
                    }

                    this.factory.DestroyPublication(publication, ref publishedEvent);

                    this.publications = newPublications;

                    this.extensionHost.ForEach(extension => extension.RemovedPublication(this, publication));
                }
            }
        }

        public void RemovePublication<TEventArgs>(object publisher, ref EventHandler<TEventArgs> publishedEvent) where TEventArgs : EventArgs
        {
            this.Clean();
            IPublication publication = this.FindPublication(publisher, CodePublication<TEventArgs>.EventNameOfCodePublication);
            if (publication != null)
            {
                lock (this)
                {
                    List<IPublication> newPublications = new List<IPublication>();
                    foreach (IPublication p in this.publications)
                    {
                        if (p != publication)
                        {
                            newPublications.Add(p);
                        }
                    }

                    this.factory.DestroyPublication(publication, ref publishedEvent);

                    this.publications = newPublications;

                    this.extensionHost.ForEach(extension => extension.RemovedPublication(this, publication));
                }
            }
        }

        public void RemovePublication(IPublication publication)
        {
            // this method is a heck to get spontaneous publications with handler restrictions running!!
            this.Clean();
            if (publication != null)
            {
                lock (this)
                {
                    List<IPublication> newPublications = new List<IPublication>();
                    foreach (IPublication p in this.publications)
                    {
                        if (p != publication)
                        {
                            newPublications.Add(p);
                        }
                    }

                    this.publications = newPublications;

                    this.extensionHost.ForEach(extension => extension.RemovedPublication(this, publication));
                }
            }
        }

        /// <summary>
        /// Adds a subscription to this <see cref="EventTopic"/>.
        /// </summary>
        /// <param name="subscriber">The object that contains the method that will handle the <see cref="EventTopic"/>.</param>
        /// <param name="handlerMethod">The method on the subscriber that will handle the <see cref="EventTopic"/>.</param>
        /// <param name="handler">The handler that is used to execute the subscription.</param>
        /// <param name="subscriptionMatchers">Matchers for the subscription.</param>
        public void AddSubscription(object subscriber, MethodInfo handlerMethod, IHandler handler, IList<ISubscriptionMatcher> subscriptionMatchers)
        {
            this.Clean();
            ISubscription subscription = this.factory.CreateSubscription(subscriber, handlerMethod, handler, subscriptionMatchers);
            
            this.ThrowIfRepeatedSubscription(subscriber, handlerMethod.Name);
            foreach (IPublication publication in this.publications)
            {
                this.ThrowIfPublisherAndSubscriberEventArgsMismatch(subscription, publication);
                this.ThrowIfSubscriptionHandlerDoesNotMatchHandlerRestrictionOfPublisher(subscription, publication);
            }

            lock (this)
            {
                this.subscriptions = new List<ISubscription>(this.subscriptions) { subscription };

                this.extensionHost.ForEach(extension => extension.CreatedSubscription(this, subscription));

                this.extensionHost.ForEach(extension => extension.AddedSubscription(this, subscription));
            }
        }

        /// <summary>
        /// Removes a subscription from this <see cref="EventTopic"/>.
        /// </summary>
        /// <param name="subscriber">The object that contains the method that will handle the <see cref="EventTopic"/>.</param>
        /// <param name="handlerMethod">The method on the subscriber that will handle the <see cref="EventTopic"/>.</param>
        public void RemoveSubscription(object subscriber, MethodInfo handlerMethod)
        {
            this.Clean();
            ISubscription subscription = this.FindSubscription(subscriber, handlerMethod.Name);
            if (subscription != null)
            {
                lock (this)
                {
                    List<ISubscription> newSubscriptions = new List<ISubscription>();
                    foreach (ISubscription s in this.subscriptions)
                    {
                        if (s != subscription)
                        {
                            newSubscriptions.Add(s);
                        }
                    }

                    this.subscriptions = newSubscriptions;

                    this.extensionHost.ForEach(extension => extension.RemovedSubscription(this, subscription));
                }
            }
        }

        /// <summary>
        /// Describes this event topic:
        /// publications, subscriptions, names, thread options, scopes, event arguments.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void DescribeTo(TextWriter writer)
        {
            writer.Write("EventTopic: ");
            writer.Write(this.Uri);
            writer.WriteLine();

            writer.WriteLine("Publishers:");
            foreach (IPublication publication in this.publications)
            {
                publication.DescribeTo(writer);
                writer.WriteLine();
            }

            writer.WriteLine("Subscribers:");
            foreach (ISubscription subscription in this.subscriptions)
            {
                subscription.DescribeTo(writer);
                writer.WriteLine();
            }
        }

        /// <summary>
        /// Called to free resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);

            this.extensionHost.ForEach(extension => extension.Disposed(this));
        }

        /// <summary>
        /// Fires the <see cref="IEventTopic"/>.
        /// </summary>
        /// <param name="sender">The object that acts as the sender of the event to the subscribers. 
        /// Not always the publisher (it's the sender provided in the event call).</param>
        /// <param name="e">An <see cref="EventArgs"/> instance to be passed to the subscribers.</param>
        /// <param name="publication">The publication firing the event topic.</param>
        public void Fire(object sender, EventArgs e, IPublication publication)
        {
            this.extensionHost.ForEach(extension => extension.FiringEvent(this, publication, sender, e));

            this.Clean();

            var handlers = this.GetHandlers();
            this.CallSubscriptionHandlers(sender, e, handlers, publication);

            this.extensionHost.ForEach(extension => extension.FiredEvent(this, publication, sender, e));
        }

        /// <summary>
        /// Checks if the specified publication has been registered with this <see cref="EventTopic"/>.
        /// </summary>
        /// <param name="publisher">The object that contains the publication.</param>
        /// <param name="eventName">The name of event on the publisher that fires the topic.</param>
        /// <returns>true if the topic contains the requested publication; otherwise false.</returns>
        public bool ContainsPublication(object publisher, string eventName)
        {
            this.Clean();
            return this.FindPublication(publisher, eventName) != null;
        }

        /// <summary>
        /// Checks if the specified subscription has been registered with this <see cref="EventTopic"/>.
        /// </summary>
        /// <param name="subscriber">The object that contains the method that will handle the <see cref="EventTopic"/>.</param>
        /// <param name="handlerMethodName">The name of the method on the subscriber that will handle the <see cref="EventTopic"/>.</param>
        /// <returns>true, if the topic contains the subscription; otherwise false.</returns>
        public bool ContainsSubscription(object subscriber, string handlerMethodName)
        {
            this.Clean();
            return this.FindSubscription(subscriber, handlerMethodName) != null;
        }

        /// <summary>
        /// Adds the publication.
        /// </summary>
        /// <param name="publication">The publication.</param>
        public void AddPublication(IPublication publication)
        {
            this.extensionHost.ForEach(extension => extension.CreatedPublication(this, publication));

            foreach (ISubscription subscription in this.subscriptions)
            {
                this.ThrowIfPublisherAndSubscriberEventArgsMismatch(subscription, publication);
                this.ThrowIfSubscriptionHandlerDoesNotMatchHandlerRestrictionOfPublisher(subscription, publication);
            }

            lock (this)
            {
                List<IPublication> newPublications = new List<IPublication>(this.publications) { publication };

                this.publications = newPublications;

                this.extensionHost.ForEach(extension => extension.AddedPublication(this, publication));
            }
        }

        /// <summary>
        /// Called to free resources.
        /// </summary>
        /// <param name="disposing">Should be true when calling from Dispose().</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (IPublication publication in this.publications)
                {
                    publication.Dispose();
                }

                this.publications.Clear();
            }
        }

        /// <summary>
        /// Checks whether the event of the publisher has to be relayed to the subscriber (Matchers).
        /// </summary>
        /// <param name="publication">The publication.</param>
        /// <param name="subscription">The subscription.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <returns><code>true</code> if the event has to be relayed.</returns>
        private bool CheckMatchers(IPublication publication, ISubscription subscription, EventArgs e)
        {
            bool match = true;
            foreach (var publicationMatcher in publication.PublicationMatchers)
            {
                match &= publicationMatcher.Match(publication, subscription, e);
            }

            foreach (var globalMatcher in this.globalMatchersProvider.Matchers)
            {
                match &= globalMatcher.Match(publication, subscription, e);
            }

            foreach (var subscriptionMatcher in subscription.SubscriptionMatchers)
            {
                match &= subscriptionMatcher.Match(publication, subscription, e);
            }

            return match;
        }

        /// <summary>
        /// Searches for a already registered publication for the same publisher and event.
        /// </summary>
        /// <param name="publisher">The publisher that will be registered newly.</param>
        /// <param name="eventName">Name of the published event.</param>
        /// <returns>The publication that is already registered.</returns>
        private IPublication FindPublication(object publisher, string eventName)
        {
            IPublication publication = this.publications.SingleOrDefault(
                match => match.Publisher == publisher &&
                         match.EventName == eventName);
            return publication;
        }

        /// <summary>
        /// Gets the handlers of this even topic
        /// </summary>
        /// <returns>
        /// Array of delegates, the handlers for this even topic.
        /// </returns>
        private IEnumerable<KeyValuePair<ISubscription, EventTopicFireDelegate>> GetHandlers()
        {
            foreach (ISubscription subscription in this.subscriptions)
            {
                EventTopicFireDelegate handler = subscription.GetHandler();
                if (handler != null)
                {
                    yield return new KeyValuePair<ISubscription, EventTopicFireDelegate>(subscription, handler);
                }
            }
        }

        /// <summary>
        /// Returns the subscription of the specified subscriber.
        /// </summary>
        /// <param name="subscriber">The subscriber to look for.</param>
        /// <param name="handlerMethodName">Name of the handler method to look for.</param>
        /// <returns>The subscription for the specified subscriber and handler method, null if not found.</returns>
        private ISubscription FindSubscription(object subscriber, string handlerMethodName)
        {
            this.Clean();

            return this.subscriptions.SingleOrDefault(
                match => match.Subscriber == subscriber && match.HandlerMethodName == handlerMethodName);
        }

        /// <summary>
        /// Perform a sanity cleaning of the dead references to publishers and subscribers
        /// </summary>
        /// <devdoc>As the topic maintains <see cref="WeakReference"/> to publishers and subscribers,
        /// those instances that are finished but hadn't been removed from the topic will leak. This method
        /// deals with that case.</devdoc>
        private void Clean()
        {
            bool cleanSubscriptions = false;
            foreach (ISubscription subscription in this.subscriptions)
            {
                cleanSubscriptions |= subscription.Subscriber == null;
            }

            bool cleanPublications = false;
            foreach (IPublication publication in this.publications.ToArray())
            {
                cleanPublications |= publication.Publisher == null;
            }

            if (cleanSubscriptions)
            {
                lock (this)
                {
                    List<ISubscription> newSubscriptions = new List<ISubscription>();
                    foreach (ISubscription subscription in this.subscriptions)
                    {
                        if (subscription.Subscriber != null)
                        {
                            newSubscriptions.Add(subscription);
                        }
                    }

                    this.subscriptions = newSubscriptions;
                }
            }
            
            if (cleanPublications)
            {
                lock (this)
                {
                    List<IPublication> newPublications = new List<IPublication>();
                    foreach (IPublication publication in this.publications)
                    {
                        if (publication.Publisher != null)
                        {
                            newPublications.Add(publication);
                        }
                        else
                        {
                            publication.Dispose();
                        }
                    }

                    this.publications = newPublications;
                }
            }
        }

        /// <summary>
        /// Calls the subscription handlers.
        /// </summary>
        /// <param name="sender">The publisher.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <param name="handlers">The handlers to call.</param>
        /// <param name="publication">The publication firing the event topic.</param>
        private void CallSubscriptionHandlers(object sender, EventArgs e, IEnumerable<KeyValuePair<ISubscription, EventTopicFireDelegate>> handlers, IPublication publication)
        {
            foreach (var handler in handlers)
            {
                ISubscription subscription = handler.Key;
                EventTopicFireDelegate handlerMethod = handler.Value;
                if (this.CheckMatchers(publication, subscription, e))
                {
                    handlerMethod(this, sender, e, publication);
                }
                else
                {
                    this.extensionHost.ForEach(extension => extension.SkippedEvent(
                                                                this,
                                                                publication,
                                                                subscription,
                                                                sender,
                                                                e));
                }
            }
        }

        /// <summary>
        /// Throws a <see cref="RepeatedPublicationException"/> if a duplicate publication is detected.
        /// </summary>
        /// <param name="publisher">The publisher to add.</param>
        /// <param name="eventName">Name of the event to add.</param>
        /// <exception cref="RepeatedPublicationException">Thrown if a duplicate publication is detected.</exception>
        private void ThrowIfRepeatedPublication(object publisher, string eventName)
        {
            if (this.FindPublication(publisher, eventName) != null)
            {
                throw new RepeatedPublicationException(publisher, eventName);
            }
        }

        /// <summary>
        /// Throws a <see cref="RepeatedSubscriptionException"/> if a duplicate subscription is detected.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethodName">Name of the handler method.</param>
        private void ThrowIfRepeatedSubscription(object subscriber, string handlerMethodName)
        {
            if (this.FindSubscription(subscriber, handlerMethodName) != null)
            {
                throw new RepeatedSubscriptionException(subscriber, handlerMethodName);
            }
        }

        /// <summary>
        /// Throws an <see cref="EventTopicException"/> if publisher and subscriber use incompatible event arguments.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        /// <param name="publication">The publication.</param>
        /// <exception cref="EventTopicException">Thrown if publisher and subscriber use incompatible event arguments.</exception>
        private void ThrowIfPublisherAndSubscriberEventArgsMismatch(ISubscription subscription, IPublication publication)
        {
            Type publisherEventArgsType = publication.EventArgsType;
            Type subscriberEventArgsType = subscription.EventArgsType;

            // check that the T in EventHandler<T> is matching, the IsAssignableFrom method return false event if types can be assigned
            // e.g. EventHandler<CustomEventArgs> is not assignable to EventHandler<EventArgs> when using IsAssignableFrom directly on even thandler type
            // therefore do the check on the event args type only.
            if (!subscriberEventArgsType.IsAssignableFrom(publisherEventArgsType))
            {
                StringWriter writer = new StringWriter();
                writer.Write("Publication ");
                writer.WriteLine();
                publication.DescribeTo(writer);
                writer.WriteLine();
                writer.Write("does not match with subscription ");
                writer.WriteLine();
                subscription.DescribeTo(writer);

                throw new EventTopicException(writer.GetStringBuilder().ToString());
            }
        }

        /// <summary>
        /// Throws if subscription handler does not match handler restriction of publisher.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        /// <param name="publication">The publication.</param>
        private void ThrowIfSubscriptionHandlerDoesNotMatchHandlerRestrictionOfPublisher(ISubscription subscription, IPublication publication)
        {
            if (publication.HandlerRestriction == HandlerRestriction.None)
            {
                return;
            }

            if ((int)subscription.Handler.Kind == (int)publication.HandlerRestriction)
            {
                return;
            }

            StringWriter writer = new StringWriter();
            writer.Write("Publication ");
            writer.WriteLine();
            publication.DescribeTo(writer);
            writer.WriteLine();
            writer.Write("does not allow subscription ");
            writer.WriteLine();
            subscription.DescribeTo(writer);
            writer.WriteLine();
            writer.Write(" because publisher requires the subscription handler to be ");
            writer.Write(publication.HandlerRestriction);

            throw new EventTopicException(writer.GetStringBuilder().ToString());
        }
    }
}