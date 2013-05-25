//-------------------------------------------------------------------------------
// <copyright file="Publication.cs" company="Appccelerate">
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
        private readonly IEventTopicExecuter topic;
        private readonly WeakReference publisher;
        private readonly IList<IPublicationMatcher> publicationMatchers;
        private readonly HandlerRestriction handlerRestriction;

        protected Publication(
            IEventTopicExecuter topic,
            object publisher,
            HandlerRestriction handlerRestriction,
            IList<IPublicationMatcher> publicationMatchers)
        {
            this.topic = topic;
            this.publisher = new WeakReference(publisher);
            this.handlerRestriction = handlerRestriction;
            this.publicationMatchers = publicationMatchers;
        }
        
        public object Publisher
        {
            get { return this.publisher.Target; }
        }

        public abstract string EventName { get; }

        public HandlerRestriction HandlerRestriction
        {
            get { return this.handlerRestriction; }
        }

        public IList<IPublicationMatcher> PublicationMatchers
        {
            get { return this.publicationMatchers; }
        }

        public abstract Type EventArgsType { get; }

        protected bool IsPublisherAlive
        {
            get { return this.publisher.IsAlive;  }
        }
        
        public virtual void DescribeTo(TextWriter writer)
        {
            Ensure.ArgumentNotNull(writer, "writer");

            if (!this.publisher.IsAlive)
            {
                return;
            }

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
        
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool disposing);

        protected virtual void Fire(object sender, EventArgs e)
        {
            this.topic.Fire(sender, e, this);
        }
    }
}