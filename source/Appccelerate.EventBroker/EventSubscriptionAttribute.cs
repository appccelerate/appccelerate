//-------------------------------------------------------------------------------
// <copyright file="EventSubscriptionAttribute.cs" company="Appccelerate">
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
    /// Declares a handler as an <see cref="IEventTopic"/> subscription.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class EventSubscriptionAttribute : Attribute
    {
        /// <summary>
        /// The URI of the event topic this publication refers to.
        /// </summary>
        private readonly string topic;

        /// <summary>
        /// Which threading strategy handler is used for this subscription.
        /// </summary>
        private readonly Type handlerType;

        /// <summary>
        /// Which matchers are used for this subscription.
        /// </summary>
        private readonly List<Type> matcherTypes = new List<Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSubscriptionAttribute"/> class using the specified handler to execute the subscription.
        /// </summary>
        /// <param name="topic">The name of the <see cref="IEventTopic"/> to subscribe to.</param>
        /// <param name="handlerType">The type of the handler to execute the subscription (on publisher thread, user interface, ...).</param>
        public EventSubscriptionAttribute(string topic, Type handlerType) : this(topic, handlerType, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSubscriptionAttribute"/> class using the specified handler to execute the subscription and the
        /// specified subscription scope matcher.
        /// </summary>
        /// <param name="topic">The name of the <see cref="IEventTopic"/> to subscribe to.</param>
        /// <param name="handlerType">The type of the handler to execute the subscription (on publisher thread, user interface, ...).</param>
        /// <param name="matcherTypes">Type of the matchers used for this subscription.</param>
        public EventSubscriptionAttribute(string topic, Type handlerType, params Type[] matcherTypes)
        {
            Ensure.ArgumentNotNullOrEmpty(topic, "topic");

            this.topic = topic;
            this.handlerType = handlerType;

            if (matcherTypes != null && matcherTypes.Length > 0)
            {
                this.matcherTypes.AddRange(matcherTypes);
            }
        }

        /// <summary>
        /// Gets the name of the <see cref="IEventTopic"/> the decorated method is subscribed to.
        /// </summary>
        public string Topic
        {
            get { return this.topic; }
        }

        /// <summary>
        /// Gets the type of the subscription execution handler.
        /// </summary>
        /// <value>The type of the subscription execution handler.</value>
        public Type HandlerType
        {
            get { return this.handlerType; }
        }

        /// <summary>
        /// Gets the types of the matchers.
        /// </summary>
        /// <value>The types of the matchers.</value>
        public IEnumerable<Type> MatcherTypes
        {
            get { return this.matcherTypes; }
        }
    }
}
