//-------------------------------------------------------------------------------
// <copyright file="IEventBroker.cs" company="Appccelerate">
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
    using Matchers;

    /// <summary>
    /// Interface for <see cref="EventBroker"/>.
    /// Use this interface to reference the event broker from your classes. This gives you the possibility to
    /// mock it.
    /// </summary>
    public interface IEventBroker : IDisposable
    {
        /// <summary>
        /// Describes all event topics of this event broker:
        /// publications, subscriptions, names, thread options, scopes, event args.
        /// </summary>
        /// <param name="writer">The writer.</param>
        void DescribeTo(TextWriter writer);

        void Register(object item);

        void Unregister(object item);

        /// <summary>
        /// Fires the specified topic directly on the <see cref="IEventBroker"/> without a real publisher.
        /// This is useful when temporarily created objects need to fire events.
        /// The event is fired globally but can be subscribed with <see cref="Matchers.ISubscriptionMatcher"/>.
        /// </summary>
        /// <param name="topic">The topic URI.</param>
        /// <param name="publisher">The publisher (for event flow and logging).</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="sender">The sender (passed to the event handler).</param>
        /// <param name="eventArgs">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void Fire(string topic, object publisher, HandlerRestriction handlerRestriction, object sender, EventArgs eventArgs);

        /// <summary>
        /// Adds the extension.
        /// </summary>
        /// <param name="extension">The extension.</param>
        void AddExtension(IEventBrokerExtension extension);

        /// <summary>
        /// Removes the specified extension.
        /// </summary>
        /// <param name="extension">The extension.</param>
        void RemoveExtension(IEventBrokerExtension extension);

        /// <summary>
        /// Clears all extensions, including the default logger extension.
        /// </summary>
        void ClearExtensions();

        /// <summary>
        /// Adds the global matcher.
        /// </summary>
        /// <param name="matcher">The matcher.</param>
        void AddGlobalMatcher(IMatcher matcher);

        /// <summary>
        /// Removes the global matcher.
        /// </summary>
        /// <param name="matcher">The matcher.</param>
        void RemoveGlobalMatcher(IMatcher matcher);
    }
}