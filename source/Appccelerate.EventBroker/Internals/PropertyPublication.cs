//-------------------------------------------------------------------------------
// <copyright file="PropertyPublication.cs" company="Appccelerate">
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
    using System.IO;
    using System.Reflection;

    using Appccelerate.Formatters;

    using Exceptions;
    using Matchers;

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

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyPublication"/> class.
        /// </summary>
        /// <param name="topic">The event topic this publication belongs to.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="eventInfo">The <see cref="EventInfo"/> of the publisher that registers this event topic.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="publicationMatchers">The publication matchers.</param>
        public PropertyPublication(
            IEventTopic topic,
            object publisher,
            EventInfo eventInfo,
            HandlerRestriction handlerRestriction,
            IList<IPublicationMatcher> publicationMatchers) : 
                base(topic, publisher, handlerRestriction, publicationMatchers)
        {
            this.eventInfo = eventInfo;

            if (this.eventInfo.EventHandlerType == null)
            {
                throw new Exception("EventHandlerType on published event must not be null (internal EventBroker failure).");
            }

            ThrowIfInvalidEventHandler(this.eventInfo);
            ThrowIfEventIsStatic(this.eventInfo);

            this.eventArgsType = this.eventInfo.EventHandlerType == typeof(EventHandler)
                                     ? typeof(EventArgs)
                                     : this.eventInfo.EventHandlerType.GetGenericArguments()[0];

            Delegate handler = Delegate.CreateDelegate(
                this.eventInfo.EventHandlerType,
                this,
                GetType().GetMethod("PublicationHandler"));
            this.eventInfo.AddEventHandler(publisher, handler);
        }
        
        #endregion

        #region Data

        /// <summary>
        /// Gets the name of the event on the <see cref="Publication.Publisher"/>.
        /// </summary>
        public override string EventName
        {
            get { return this.eventInfo.Name; }
        }

        /// <summary>
        /// Gets the type of the event arguments.
        /// </summary>
        /// <value>The type of the event arguments.</value>
        public override Type EventArgsType
        {
            get { return this.eventArgsType; }
        }

        #endregion

        /// <summary>
        /// Fires the event publication. This method is registered to the event on the publisher.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void PublicationHandler(object sender, EventArgs e)
        {
            this.Fire(sender, e);
        }

        #region DescribeTo

        /// <summary>
        /// Describes this publication
        /// name, scope, event handler.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void DescribeTo(TextWriter writer)
        {
            if (this.IsPublisherAlive)
            {
                base.DescribeTo(writer);
                
                writer.Write(", EventHandler type = ");
                writer.Write(this.eventInfo.EventHandlerType.FullNameToString());
            }
        }

        #endregion

        #region Dispose

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
                    Delegate.CreateDelegate(this.eventInfo.EventHandlerType, this, GetType().GetMethod("PublicationHandler")));
            }
        }

        #endregion

        #region Publication Event Validation

        /// <summary>
        /// Throws a <see cref="StaticPublisherEventException"/> if the published event is defined static.
        /// </summary>
        /// <param name="publishedEvent">The published event.</param>
        /// <exception cref="StaticPublisherEventException">Thrown if the published event is defined static.</exception>
        private static void ThrowIfEventIsStatic(EventInfo publishedEvent)
        {
            if (publishedEvent.GetAddMethod().IsStatic || publishedEvent.GetRemoveMethod().IsStatic)
            {
                throw new StaticPublisherEventException(publishedEvent);
            }
        }

        /// <summary>
        /// Throws an <see cref="InvalidPublicationSignatureException"/> if defined event handler on publisher
        /// is not an <see cref="EventHandler"/>.
        /// </summary>
        /// <param name="info">The event info of the published event.</param>
        /// <exception cref="InvalidPublicationSignatureException">Thrown if defined event handler on publisher is not an <see cref="EventHandler"/>.</exception>
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

        #endregion
    }
}