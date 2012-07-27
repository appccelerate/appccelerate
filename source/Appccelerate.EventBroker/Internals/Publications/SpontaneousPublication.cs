//-------------------------------------------------------------------------------
// <copyright file="SpontaneousPublication.cs" company="Appccelerate">
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

    /// <summary>
    /// A spontaneous publication is used when there is no real publisher but 
    /// <see cref="EventBroker.Fire"/> was called directly to fire an event.
    /// </summary>
    internal class SpontaneousPublication : Publication
    {
        public const string SpontaneousEventName = "Fired on event broker";

        private readonly Type eventArgsType;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpontaneousPublication"/> class.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="eventArgsType">Type of the event args.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="publicationMatchers">The publication matchers.</param>
        public SpontaneousPublication(IEventTopicExecuter topic, object publisher, Type eventArgsType, HandlerRestriction handlerRestriction, IList<IPublicationMatcher> publicationMatchers) :
            base(topic, publisher, handlerRestriction, publicationMatchers)
        {
            this.eventArgsType = eventArgsType;
        }

        /// <summary>
        /// Gets the name of the event on the <see cref="Publication.Publisher"/>.
        /// For a spontaneous publication this is null.
        /// </summary>
        /// <value></value>
        public override string EventName
        {
            get { return SpontaneousEventName; }
        }

        /// <summary>
        /// Gets the type of the event handler.
        /// </summary>
        /// <value>The type of the event handler.</value>
        public override Type EventArgsType
        {
            get
            {
                return this.eventArgsType;
            }
        }

        /// <summary>
        /// Describes this publication
        /// name, scope, event handler.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void DescribeTo(TextWriter writer)
        {
            Ensure.ArgumentNotNull(writer, "writer");

            writer.Write(", spontaneous publication");
        }

        /// <summary>
        /// Implementation of the disposable pattern.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            // nothing to dispose
        }
    }
}