//-------------------------------------------------------------------------------
// <copyright file="ISubscription.cs" company="Appccelerate">
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
    using System.IO;

    using Appccelerate.EventBroker.Internals;
    using Appccelerate.EventBroker.Matchers;

    /// <summary>
    /// Represents a subscription to an <see cref="EventTopic"/>.
    /// </summary>
    public interface ISubscription
    {
        /// <summary>
        /// Gets the type of the event arguments this subscription is using.
        /// </summary>
        /// <value>The type of the event arguments.</value>
        Type EventArgsType { get; }

        /// <summary>
        /// Gets the subscriber of the event.
        /// </summary>
        object Subscriber { get; }

        /// <summary>
        /// Gets the handler of this subscription.
        /// </summary>
        /// <value>The handler of this subscription.</value>
        IHandler Handler { get; }

        /// <summary>
        /// Gets the subscription matchers.
        /// </summary>
        /// <value>The subscription matchers.</value>
        IList<ISubscriptionMatcher> SubscriptionMatchers { get; }

        /// <summary>
        /// Gets the handler method name that's subscribed to the event.
        /// </summary>
        string HandlerMethodName { get; }

        /// <summary>
        /// Describes this subscription:
        /// name, thread option, scope, event arguments.
        /// </summary>
        /// <param name="writer">The writer.</param>
        void DescribeTo(TextWriter writer);

        /// <summary>
        /// Gets the handler that will be called by the <see cref="IEventTopic"/> during a firing sequence.
        /// </summary>
        /// <returns>A delegate that is used to call the subscription handler.</returns>
        EventTopicFireDelegate GetHandler();
    }
}