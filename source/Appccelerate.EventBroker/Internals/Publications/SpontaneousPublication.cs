//-------------------------------------------------------------------------------
// <copyright file="SpontaneousPublication.cs" company="Appccelerate">
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

    /// <summary>
    /// A spontaneous publication is used when there is no real publisher but 
    /// <see cref="EventBroker.Fire"/> was called directly to fire an event.
    /// </summary>
    internal class SpontaneousPublication : Publication
    {
        public const string SpontaneousEventName = "Fired on event broker";

        private readonly Type eventArgsType;

        public SpontaneousPublication(IEventTopicExecuter topic, object publisher, Type eventArgsType, HandlerRestriction handlerRestriction, IList<IPublicationMatcher> publicationMatchers) :
            base(topic, publisher, handlerRestriction, publicationMatchers)
        {
            this.eventArgsType = eventArgsType;
        }

        public override string EventName
        {
            get { return SpontaneousEventName; }
        }

        public override Type EventArgsType
        {
            get
            {
                return this.eventArgsType;
            }
        }

        public override void DescribeTo(TextWriter writer)
        {
            Ensure.ArgumentNotNull(writer, "writer");

            writer.Write(", spontaneous publication");
        }

        protected override void Dispose(bool disposing)
        {
            // nothing to dispose
        }
    }
}