//-------------------------------------------------------------------------------
// <copyright file="PropertyPublication.cs" company="Appccelerate">
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
    using System.Reflection;

    using Appccelerate.EventBroker.Exceptions;
    using Appccelerate.EventBroker.Internals.Exceptions;
    using Appccelerate.EventBroker.Matchers;
    using Appccelerate.Formatters;

    /// <summary>
    /// Represents a topic publication.
    /// </summary>
    internal class PropertyPublication : Publication
    {
        /// <summary>
        /// The event on the publisher this publication stands for.
        /// </summary>
        private readonly EventInfo eventInfo;

        private readonly Type eventArgsType;

        public PropertyPublication(
            IEventTopicExecuter topic,
            object publisher,
            EventInfo eventInfo,
            HandlerRestriction handlerRestriction,
            IList<IPublicationMatcher> publicationMatchers) : 
                base(topic, publisher, handlerRestriction, publicationMatchers)
        {
            this.eventInfo = eventInfo;

            if (this.eventInfo.EventHandlerType == null)
            {
                throw new EventBrokerException("EventHandlerType on published event must not be null (internal EventBroker failure).");
            }

            ThrowIfInvalidEventHandler(this.eventInfo);
            ThrowIfEventIsStatic(this.eventInfo);

            this.eventArgsType = this.eventInfo.EventHandlerType == typeof(EventHandler)
                                     ? typeof(EventArgs)
                                     : this.eventInfo.EventHandlerType.GetGenericArguments()[0];

            Delegate handler = Delegate.CreateDelegate(
                this.eventInfo.EventHandlerType,
                this,
                this.GetType().GetMethod("PublicationHandler"));
            this.eventInfo.AddEventHandler(publisher, handler);
        }
        
        public override string EventName
        {
            get { return this.eventInfo.Name; }
        }

        public override Type EventArgsType
        {
            get { return this.eventArgsType; }
        }

        public void PublicationHandler(object sender, EventArgs e)
        {
            this.Fire(sender, e);
        }

        public override void DescribeTo(TextWriter writer)
        {
            Ensure.ArgumentNotNull(writer, "writer");

            if (!this.IsPublisherAlive)
            {
                return;
            }

            base.DescribeTo(writer);
                
            writer.Write(", EventHandler type = ");
            writer.Write(this.eventInfo.EventHandlerType.FullNameToString());
        }

        /// <summary>
        /// Implementation of the disposable pattern.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        /// <remarks>
        /// Unregisters the event handler.
        /// </remarks>
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.IsPublisherAlive)
            {
                this.eventInfo.RemoveEventHandler(
                    this.Publisher,
                    Delegate.CreateDelegate(this.eventInfo.EventHandlerType, this, this.GetType().GetMethod("PublicationHandler")));
            }
        }

        private static void ThrowIfEventIsStatic(EventInfo publishedEvent)
        {
            if (publishedEvent.GetAddMethod().IsStatic || publishedEvent.GetRemoveMethod().IsStatic)
            {
                throw new StaticPublisherEventException(publishedEvent);
            }
        }

        private static void ThrowIfInvalidEventHandler(EventInfo info)
        {
            if (typeof(EventHandler).IsAssignableFrom(info.EventHandlerType) ||
                (info.EventHandlerType.IsGenericType &&
                 typeof(EventHandler<>).IsAssignableFrom(info.EventHandlerType.GetGenericTypeDefinition())))
            {
                return;
            }

            throw new InvalidPublicationSignatureException(info);
        }
    }
}