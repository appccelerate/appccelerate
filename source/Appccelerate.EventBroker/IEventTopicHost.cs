//-------------------------------------------------------------------------------
// <copyright file="IEventTopicHost.cs" company="Appccelerate">
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
    using System.IO;

    /// <summary>
    /// An event topic hosts is context where publications and subscriptions are wired together through <see cref="IEventTopic"/>s.
    /// </summary>
    /// <remarks>
    /// Only publications and subscription in the same host are wired together. You can use several event topic hosts by exchanging them during registration on the <see cref="EventBroker"/>.
    /// </remarks>
    public interface IEventTopicHost : IDisposable
    {
        /// <summary>
        /// Gets a event topic. Result is never null. Event topic is created if it does not yet exist.
        /// </summary>
        /// <param name="topic">The topic URI identifying the event topic to return.</param>
        /// <returns>A non-null event topic. Event topic is created if it does not yet exist.</returns>
        /// <remarks>
        /// Returns a non null instance of the dictionary.
        /// </remarks>
        /// <value>The event topics.</value>
        IEventTopic GetEventTopic(string topic);

        /// <summary>
        /// Describes all event topics:
        /// publications, subscriptions, names, thread options, scopes, event arguments.
        /// </summary>
        /// <param name="writer">The writer.</param>
        void DescribeTo(TextWriter writer);
    }
}