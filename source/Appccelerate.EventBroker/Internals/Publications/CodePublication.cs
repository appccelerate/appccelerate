//-------------------------------------------------------------------------------
// <copyright file="CodePublication.cs" company="Appccelerate">
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

    using Appccelerate.EventBroker.Matchers;

    /// <summary>
    /// Publication that was registered by code.
    /// </summary>
    /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
    internal class CodePublication<TEventArgs> : Publication where TEventArgs : EventArgs
    {
        /// <summary>
        /// This name is used for events of publications made in code. The real name cannot be accessed through the event handler.
        /// </summary>
        public const string EventNameOfCodePublication = "publication by code";

        /// <summary>
        /// Type of the event args of the published event.
        /// </summary>
        private readonly Type eventArgsType;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodePublication{TEventArgs}"/> class.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="eventHandler">The event handler.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="publicationMatchers">The publication matchers.</param>
        public CodePublication(
            IEventTopicExecuter topic,
            object publisher,
            ref EventHandler eventHandler,
            HandlerRestriction handlerRestriction,
            IList<IPublicationMatcher> publicationMatchers) :
                base(topic, publisher, handlerRestriction, publicationMatchers)
        {
            eventHandler += this.PublicationHandler;

            this.eventArgsType = typeof(EventArgs);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodePublication{TEventArgs}"/> class.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="eventHandler">The event handler.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="publicationMatchers">The publication matchers.</param>
        public CodePublication(
            IEventTopicExecuter topic,
            object publisher,
            ref EventHandler<TEventArgs> eventHandler,
            HandlerRestriction handlerRestriction,
            IList<IPublicationMatcher> publicationMatchers) :
                base(topic, publisher, handlerRestriction, publicationMatchers)
        {
            eventHandler += this.PublicationHandler;

            this.eventArgsType = eventHandler.GetType().GetGenericArguments()[0];
        }

        /// <summary>
        /// Gets the name of the event on the <see cref="Publication.Publisher"/>.
        /// </summary>
        public override string EventName
        {
            get { return EventNameOfCodePublication; }
        }

        /// <summary>
        /// Gets the type of the event arguments.
        /// </summary>
        /// <value>The type of the event arguments.</value>
        public override Type EventArgsType
        {
            get { return this.eventArgsType; }
        }

        /// <summary>
        /// Unregisters the specified published event.
        /// </summary>
        /// <param name="publishedEvent">The published event.</param>
        public void Unregister(ref EventHandler publishedEvent)
        {
            publishedEvent -= this.PublicationHandler;
        }

        /// <summary>
        /// Unregisters the specified published event.
        /// </summary>
        /// <param name="publishedEvent">The published event.</param>
        public void Unregister(ref EventHandler<TEventArgs> publishedEvent)
        {
            publishedEvent -= this.PublicationHandler;    
        }

        /// <summary>
        /// Implementation of the disposable pattern.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// Fires the event publication. This method is registered to the event on the publisher.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void PublicationHandler(object sender, EventArgs e)
        {
            this.Fire(sender, e);
        }
    }
}