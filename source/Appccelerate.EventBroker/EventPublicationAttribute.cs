//-------------------------------------------------------------------------------
// <copyright file="EventPublicationAttribute.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Declares an event as an <see cref="IEventTopic"/> publication.
    /// </summary>
    [AttributeUsage(AttributeTargets.Event, AllowMultiple = true)]
    public sealed class EventPublicationAttribute : Attribute
    {
        /// <summary>
        /// The URI of the event topic this publication refers to.
        /// </summary>
        private readonly string topic;

        /// <summary>
        /// Which matchers are used for this publication.
        /// </summary>
        private readonly List<Type> matcherTypes = new List<Type>();

        /// <summary>
        /// The restriction this publication has for subscribers.
        /// </summary>
        private readonly HandlerRestriction handlerRestriction;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventPublicationAttribute"/> class with
        /// global publication scope.
        /// </summary>
        /// <param name="topic">The topic URI.</param>
        public EventPublicationAttribute(string topic) : this(topic, HandlerRestriction.None, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventPublicationAttribute"/> class.
        /// </summary>
        /// <param name="topic">The topic URI.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        public EventPublicationAttribute(string topic, HandlerRestriction handlerRestriction)
            : this(topic, handlerRestriction, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventPublicationAttribute"/> class.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="matcherTypes">The matcher types.</param>
        public EventPublicationAttribute(string topic, params Type[] matcherTypes)
            : this(topic, HandlerRestriction.None, matcherTypes)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventPublicationAttribute"/> class.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="matcherTypes">The matcher types.</param>
        public EventPublicationAttribute(string topic, HandlerRestriction handlerRestriction, params Type[] matcherTypes)
        {
            Ensure.ArgumentNotNullOrEmpty(topic, "topic");

            this.topic = topic;
            this.handlerRestriction = handlerRestriction;
            if (matcherTypes != null && matcherTypes.Length > 0)
            {
                this.matcherTypes.AddRange(matcherTypes);
            }
        }

        /// <summary>
        /// Gets the topic URI.
        /// </summary>
        /// <value>The topic URI.</value>
        public string Topic
        {
            get { return this.topic; }
        }

        /// <summary>
        /// Gets the types of the matchers.
        /// </summary>
        /// <value>The types of the matchers.</value>
        public IEnumerable<Type> MatcherTypes
        {
            get { return this.matcherTypes; }
        }

        /// <summary>
        /// Gets the handler restriction this publication has for its subscribers. Whether all (null),
        /// only synchronous or asynchronous handling is allowed.
        /// </summary>
        /// <value>The handler restriction.</value>
        public HandlerRestriction HandlerRestriction
        {
            get { return this.handlerRestriction; }
        }
    }
}
