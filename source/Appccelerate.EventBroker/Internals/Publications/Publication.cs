//-------------------------------------------------------------------------------
// <copyright file="Publication.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Internals.Publications
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Appccelerate.EventBroker.Matchers;
    using Appccelerate.Formatters;

    /// <summary>
    /// Represents a topic publication.
    /// </summary>
    internal abstract class Publication : IPublication
    {
        /// <summary>
        /// The event topic this publication is registered on.
        /// </summary>
        private readonly IEventTopic topic;

        /// <summary>
        /// Weak reference to the publisher.
        /// </summary>
        private readonly WeakReference publisher;

        /// <summary>
        /// The matchers used on this publication.
        /// </summary>
        private readonly IList<IPublicationMatcher> publicationMatchers;

        /// <summary>
        /// Restriction of this publication for its subscription handlers.
        /// </summary>
        private readonly HandlerRestriction handlerRestriction;

        /// <summary>
        /// Initializes a new instance of the <see cref="Publication"/> class.
        /// </summary>
        /// <param name="topic">The event topic this publication belongs to.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="publicationMatchers">The publication matchers.</param>
        protected Publication(
            IEventTopic topic,
            object publisher,
            HandlerRestriction handlerRestriction,
            IList<IPublicationMatcher> publicationMatchers)
        {
            this.topic = topic;
            this.publisher = new WeakReference(publisher);
            this.handlerRestriction = handlerRestriction;
            this.publicationMatchers = publicationMatchers;
        }
        
        /// <summary>
        /// Gets the publisher of the event.
        /// </summary>
        public object Publisher
        {
            get { return this.publisher.Target; }
        }

        /// <summary>
        /// Gets the name of the event on the <see cref="Publication.Publisher"/>.
        /// </summary>
        /// <value></value>
        public abstract string EventName { get; }

        /// <summary>
        /// Gets the subscriber handler restriction.
        /// </summary>
        /// <value>The subscriber handler restriction.</value>
        public HandlerRestriction HandlerRestriction
        {
            get { return this.handlerRestriction; }
        }

        /// <summary>
        /// Gets the publication matchers.
        /// </summary>
        /// <value>The publication matcher.</value>
        public IList<IPublicationMatcher> PublicationMatchers
        {
            get { return this.publicationMatchers; }
        }

        /// <summary>
        /// Gets the type of the event arguments.
        /// </summary>
        /// <value>The type of the event arguments.</value>
        public abstract Type EventArgsType { get; }

        /// <summary>
        /// Gets a value indicating whether the publisher is alive (not garbage collected).
        /// </summary>
        /// <value><c>true</c> if the publisher is alive; otherwise, <c>false</c>.
        /// </value>
        protected bool IsPublisherAlive
        {
            get { return this.publisher.IsAlive;  }
        }
        
        /// <summary>
        /// Describes this publication
        /// name, scope, event handler.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public virtual void DescribeTo(TextWriter writer)
        {
            Ensure.ArgumentNotNull(writer, "writer");

            if (this.publisher.IsAlive)
            {
                writer.Write(this.Publisher.GetType().FullNameToString());

                if (this.Publisher is INamedItem)
                {
                    writer.Write(", Name = ");
                    writer.Write(((INamedItem)this.Publisher).EventBrokerItemName);
                }
                
                writer.Write(", Event = ");
                writer.Write(this.EventName);
                writer.Write(", matchers = ");
                foreach (IPublicationMatcher publicationMatcher in this.publicationMatchers)
                {
                    publicationMatcher.DescribeTo(writer);
                    writer.Write(" ");
                }
            }
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
        protected abstract void Dispose(bool disposing);

        /// <summary>
        /// Fires the event publication.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void Fire(object sender, EventArgs e)
        {
            this.topic.Fire(sender, e, this);
        }
    }
}