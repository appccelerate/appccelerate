//-------------------------------------------------------------------------------
// <copyright file="EventTopic.cs" company="Appccelerate">
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
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Appccelerate.EventBroker.Internals.Exceptions;
    using Appccelerate.EventBroker.Internals.GlobalMatchers;

    public class EventTopic : IEventTopic
    {
        private readonly IExtensionHost extensionHost;
        private readonly IGlobalMatchersProvider globalMatchersProvider;

        private List<IPublication> publications = new List<IPublication>();
        private List<ISubscription> subscriptions = new List<ISubscription>();

        public EventTopic(string uri, IExtensionHost extensionHost, IGlobalMatchersProvider globalMatchersProvider)
        {
            this.Uri = uri;
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
            Ensure.ArgumentNotNull(publication, "publication");

            this.Clean();
            this.ThrowIfRepeatedPublication(publication.Publisher, publication.EventName);

            this.extensionHost.ForEach(extension => extension.CreatedPublication(this, publication));

            foreach (ISubscription subscription in this.subscriptions)
            {
                ThrowIfPublisherAndSubscriberEventArgumentsMismatch(subscription, publication);
                ThrowIfSubscriptionHandlerDoesNotMatchHandlerRestrictionOfPublisher(subscription, publication);
            }

            lock (this)
            {
                var newPublications = new List<IPublication>(this.publications) { publication };

                this.publications = newPublications;
            }

            this.extensionHost.ForEach(extension => extension.AddedPublication(this, publication));
        }

        public IPublication RemovePublication(object publisher, string eventName)
        {
            this.Clean();

            IPublication publication = this.FindPublication(publisher, eventName);

            if (publication == null)
            {
                return null;
            }

            this.RemovePublication(publication);

            return publication;
        }

        public void AddSubscription(ISubscription subscription)
        {
            this.Clean();

            this.ThrowIfRepeatedSubscription(subscription.Subscriber, subscription.HandlerMethodName);
            foreach (IPublication publication in this.publications)
            {
                ThrowIfPublisherAndSubscriberEventArgumentsMismatch(subscription, publication);
                ThrowIfSubscriptionHandlerDoesNotMatchHandlerRestrictionOfPublisher(subscription, publication);
            }

            lock (this)
            {
                this.subscriptions = new List<ISubscription>(this.subscriptions) { subscription };

                this.extensionHost.ForEach(extension => extension.CreatedSubscription(this, subscription));
            }

            this.extensionHost.ForEach(extension => extension.AddedSubscription(this, subscription));
        }

        public void RemoveSubscription(object subscriber, MethodInfo handlerMethod)
        {
            Ensure.ArgumentNotNull(handlerMethod, "handlerMethod");

            this.Clean();

            ISubscription subscription = this.FindSubscription(subscriber, handlerMethod.Name);

            if (subscription == null)
            {
                return;
            }

            this.RemoveSubscription(subscription);
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

        public void Fire(object sender, EventArgs e, IPublication publication)
        {
            this.extensionHost.ForEach(extension => extension.FiringEvent(this, publication, sender, e));

            this.Clean();

            var handlers = this.GetSubscriptionHandlers();
            this.CallSubscriptionHandlers(sender, e, handlers, publication);

            this.extensionHost.ForEach(extension => extension.FiredEvent(this, publication, sender, e));
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);

            this.extensionHost.ForEach(extension => extension.Disposed(this));
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

        private static void ThrowIfPublisherAndSubscriberEventArgumentsMismatch(ISubscription subscription, IPublication publication)
        {
            Type publisherEventArgsType = publication.EventArgsType;
            Type subscriberEventArgsType = subscription.EventArgsType;

            // check that the T in EventHandler<T> is matching, the IsAssignableFrom method return false event if types can be assigned
            // e.g. EventHandler<CustomEventArgs> is not assignable to EventHandler<EventArgs> when using IsAssignableFrom directly on event handler type
            // therefore do the check on the event arguments type only.
            // subscriberEventArgsType can be null if the handler method has no parameters.
            if (subscriberEventArgsType != null && !subscriberEventArgsType.IsAssignableFrom(publisherEventArgsType))
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

            using (var writer = new StringWriter(CultureInfo.InvariantCulture))
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

        private void RemovePublication(IPublication publication)
        {
            lock (this)
            {
                var newPublications = this.publications.Where(p => p != publication).ToList();

                this.publications = newPublications;
            }

            this.extensionHost.ForEach(extension => extension.RemovedPublication(this, publication));
        }

        private void RemoveSubscription(ISubscription subscription)
        {
            lock (this)
            {
                var newSubscriptions = this.subscriptions.Where(s => s != subscription).ToList();

                this.subscriptions = newSubscriptions;
            }

            this.extensionHost.ForEach(extension => extension.RemovedSubscription(this, subscription));
        }

        private IPublication FindPublication(object publisher, string eventName)
        {
            IPublication publication = this.publications.SingleOrDefault(
                match => match.Publisher == publisher &&
                         match.EventName == eventName);

            return publication;
        }

        private ISubscription FindSubscription(object subscriber, string handlerMethodName)
        {
            return this.subscriptions.SingleOrDefault(
                match => match.Subscriber == subscriber && match.HandlerMethodName == handlerMethodName);
        }

        private IEnumerable<KeyValuePair<ISubscription, EventTopicFireDelegate>> GetSubscriptionHandlers()
        {
            return from subscription in this.subscriptions
                   let handler = subscription.GetHandler()
                   where handler != null
                   select new KeyValuePair<ISubscription, EventTopicFireDelegate>(subscription, handler);
        }

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
        /// Checks whether the event of the publisher has to be relayed to the subscriber (Matchers).
        /// </summary>
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

        private void ThrowIfRepeatedPublication(object publisher, string eventName)
        {
            if (this.FindPublication(publisher, eventName) != null)
            {
                throw new RepeatedPublicationException(publisher, eventName);
            }
        }

        private void ThrowIfRepeatedSubscription(object subscriber, string handlerMethodName)
        {
            if (this.FindSubscription(subscriber, handlerMethodName) != null)
            {
                throw new RepeatedSubscriptionException(subscriber, handlerMethodName);
            }
        }
    }
}