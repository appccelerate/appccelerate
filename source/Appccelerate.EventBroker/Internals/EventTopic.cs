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
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Appccelerate.EventBroker.Internals.Exceptions;
    using Appccelerate.EventBroker.Internals.GlobalMatchers;
    using Appccelerate.EventBroker.Matchers;

    public class EventTopic : IEventTopic
    {
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

        public EventTopic(string uri, IFactory factory, IExtensionHost extensionHost, IGlobalMatchersProvider globalMatchersProvider)
        {
            this.Uri = uri;
            this.factory = factory;
            this.extensionHost = extensionHost;
            this.globalMatchersProvider = globalMatchersProvider;
        }

        public string Uri { get; private set; }

        public IEnumerable<IPublication> Publications
        {
            get
            {
                this.Clean();
                return new ReadOnlyCollection<IPublication>(this.publications);
            }
        }

        public IEnumerable<ISubscription> Subscriptions
        {
            get
            {
                this.Clean();
                return new ReadOnlyCollection<ISubscription>(this.subscriptions);
            }
        }

        public void AddPublication(IPublication publication)
        {
            this.Clean();
            this.ThrowIfRepeatedPublication(publication.Publisher, publication.EventName);

            this.extensionHost.ForEach(extension => extension.CreatedPublication(this, publication));

            foreach (ISubscription subscription in this.subscriptions)
            {
                ThrowIfPublisherAndSubscriberEventArgsMismatch(subscription, publication);
                ThrowIfSubscriptionHandlerDoesNotMatchHandlerRestrictionOfPublisher(subscription, publication);
            }

            lock (this)
            {
                var newPublications = new List<IPublication>(this.publications) { publication };

                this.publications = newPublications;

                this.extensionHost.ForEach(extension => extension.AddedPublication(this, publication));
            }
        }

        public void RemovePublication(IPublication publication)
        {
            this.Clean();

            if (publication == null)
            {
                return;
            }

            lock (this)
            {
                var newPublications = this.publications.Where(p => p != publication).ToList();

                this.publications = newPublications;

                this.extensionHost.ForEach(extension => extension.RemovedPublication(this, publication));
            }
        }

        public IPublication RemovePublication(object publisher, string eventName)
        {
            IPublication publication = this.FindPublication(publisher, eventName);

            if (publication == null)
            {
                return null;
            }

            this.RemovePublication(publication);

            return publication;
        }

        public void AddSubscription(object subscriber, MethodInfo handlerMethod, IHandler handler, IList<ISubscriptionMatcher> subscriptionMatchers)
        {
            Ensure.ArgumentNotNull(handlerMethod, "handlerMethod");

            this.Clean();
            ISubscription subscription = this.factory.CreateSubscription(subscriber, handlerMethod, handler, subscriptionMatchers);

            this.ThrowIfRepeatedSubscription(subscriber, handlerMethod.Name);
            foreach (IPublication publication in this.publications)
            {
                ThrowIfPublisherAndSubscriberEventArgsMismatch(subscription, publication);
                ThrowIfSubscriptionHandlerDoesNotMatchHandlerRestrictionOfPublisher(subscription, publication);
            }

            lock (this)
            {
                this.subscriptions = new List<ISubscription>(this.subscriptions) { subscription };

                this.extensionHost.ForEach(extension => extension.CreatedSubscription(this, subscription));

                this.extensionHost.ForEach(extension => extension.AddedSubscription(this, subscription));
            }
        }

        public void RemoveSubscription(object subscriber, MethodInfo handlerMethod)
        {
            Ensure.ArgumentNotNull(handlerMethod, "handlerMethod");

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

        public void DescribeTo(TextWriter writer)
        {
            Ensure.ArgumentNotNull(writer, "writer");

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

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);

            this.extensionHost.ForEach(extension => extension.Disposed(this));
        }

        public void Fire(object sender, EventArgs e, IPublication publication)
        {
            this.extensionHost.ForEach(extension => extension.FiringEvent(this, publication, sender, e));

            this.Clean();

            var handlers = this.GetHandlers();
            this.CallSubscriptionHandlers(sender, e, handlers, publication);

            this.extensionHost.ForEach(extension => extension.FiredEvent(this, publication, sender, e));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            foreach (IPublication publication in this.publications)
            {
                publication.Dispose();
            }

            this.publications.Clear();
        }

        private static void ThrowIfPublisherAndSubscriberEventArgsMismatch(ISubscription subscription, IPublication publication)
        {
            Type publisherEventArgsType = publication.EventArgsType;
            Type subscriberEventArgsType = subscription.EventArgsType;

            // check that the T in EventHandler<T> is matching, the IsAssignableFrom method return false event if types can be assigned
            // e.g. EventHandler<CustomEventArgs> is not assignable to EventHandler<EventArgs> when using IsAssignableFrom directly on even thandler type
            // therefore do the check on the event args type only.
            if (!subscriberEventArgsType.IsAssignableFrom(publisherEventArgsType))
            {
                using (var writer = new StringWriter(CultureInfo.InvariantCulture))
                {
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
        }

        /// <summary>
        /// Throws if subscription handler does not match handler restriction of publisher.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        /// <param name="publication">The publication.</param>
        private static void ThrowIfSubscriptionHandlerDoesNotMatchHandlerRestrictionOfPublisher(ISubscription subscription, IPublication publication)
        {
            if (publication.HandlerRestriction == HandlerRestriction.None)
            {
                return;
            }

            if ((int)subscription.Handler.Kind == (int)publication.HandlerRestriction)
            {
                return;
            }

            using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
            {
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

        /// <summary>
        /// Checks whether the event of the publisher has to be relayed to the subscriber (Matchers).
        /// </summary>
        /// <param name="publication">The publication.</param>
        /// <param name="subscription">The subscription.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <returns><code>true</code> if the event has to be relayed.</returns>
        private bool CheckMatchers(IPublication publication, ISubscription subscription, EventArgs e)
        {
            bool match = publication.PublicationMatchers.Aggregate(
                true,
                (current, publicationMatcher) => current & publicationMatcher.Match(publication, subscription, e));

            match = this.globalMatchersProvider.Matchers.Aggregate(
                match,
                (current, globalMatcher) => current & globalMatcher.Match(publication, subscription, e));

            return subscription.SubscriptionMatchers.Aggregate(
                match,
                (current, subscriptionMatcher) => current & subscriptionMatcher.Match(publication, subscription, e));
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
        /// Gets the handlers of this even topic
        /// </summary>
        /// <returns>
        /// Array of delegates, the handlers for this even topic.
        /// </returns>
        private IEnumerable<KeyValuePair<ISubscription, EventTopicFireDelegate>> GetHandlers()
        {
            return from subscription in this.subscriptions
                   let handler = subscription.GetHandler()
                   where handler != null
                   select new KeyValuePair<ISubscription, EventTopicFireDelegate>(subscription, handler);
        }

        /// <summary>
        /// Perform a sanity cleaning of the dead references to publishers and subscribers
        /// </summary>
        /// <devdoc>As the topic maintains <see cref="WeakReference"/> to publishers and subscribers,
        /// those instances that are finished but hadn't been removed from the topic will leak. This method
        /// deals with that case.</devdoc>
        private void Clean()
        {
            bool cleanSubscriptions = this.subscriptions.Any(subscription => subscription.Subscriber == null);
            bool cleanPublications = this.publications.Any(publication => publication.Publisher == null);

            if (cleanPublications)
            {
                this.CleanPublications();
            }

            if (cleanSubscriptions)
            {
                this.CleanSubscriptions();
            }
        }

        private void CleanPublications()
        {
            lock (this)
            {
                var newPublications = new List<IPublication>();
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

        private void CleanSubscriptions()
        {
            lock (this)
            {
                this.subscriptions = this.subscriptions.Where(subscription => subscription.Subscriber != null).ToList();
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
    }
}