//-------------------------------------------------------------------------------
// <copyright file="CodePublication.cs" company="Appccelerate">
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

        private readonly Type eventArgsType;

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

        public override string EventName
        {
            get { return EventNameOfCodePublication; }
        }

        public override Type EventArgsType
        {
            get { return this.eventArgsType; }
        }

        public void Unregister(ref EventHandler publishedEvent)
        {
            publishedEvent -= this.PublicationHandler;
        }

        public void Unregister(ref EventHandler<TEventArgs> publishedEvent)
        {
            publishedEvent -= this.PublicationHandler;    
        }

        protected override void Dispose(bool disposing)
        {
        }

        private void PublicationHandler(object sender, EventArgs e)
        {
            this.Fire(sender, e);
        }
    }
}