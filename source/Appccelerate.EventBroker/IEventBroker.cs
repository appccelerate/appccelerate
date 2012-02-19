//-------------------------------------------------------------------------------
// <copyright file="IEventBroker.cs" company="Appccelerate">
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

        /// <summary>
        /// Fires the specified topic directly on the <see cref="IEventBroker"/> without a real publisher.
        /// This is useful when temporarily created objects need to fire events.
        /// The event is fired globally but can be subscribed with <see cref="Matchers.ISubscriptionMatcher"/>.
        /// </summary>
        /// <param name="topic">The topic URI.</param>
        /// <param name="publisher">The publisher (for event flow and logging).</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="sender">The sender (passed to the event handler).</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void Fire(string topic, object publisher, HandlerRestriction handlerRestriction, object sender, EventArgs e);

        /// <summary>
        /// Adds the extension.
        /// </summary>
        /// <param name="extension">The extension.</param>
        void AddExtension(IEventBrokerExtension extension);

        /// <summary>
        /// Registers the event as publication.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="publisher">The publisher.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="handlerRestriction">The handler restriction.</param>
        /// <param name="matchers">The matchers.</param>
        void RegisterEvent(string topic, object publisher, string eventName, HandlerRestriction handlerRestriction, params IPublicationMatcher[] matchers);

        /// <summary>
        /// Registers a handler method.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">The handler method.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="matchers">The matchers.</param>
        void RegisterHandlerMethod(string topic, object subscriber, EventHandler handlerMethod, IHandler handler, params ISubscriptionMatcher[] matchers);

        /// <summary>
        /// Registers a handler method.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="topic">The topic.</param>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">The handler method.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="matchers">The matchers.</param>
        void RegisterHandlerMethod<TEventArgs>(string topic, object subscriber, EventHandler<TEventArgs> handlerMethod, IHandler handler, params ISubscriptionMatcher[] matchers) where TEventArgs : EventArgs;

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
        /// Registers an item with this event broker.
        /// </summary>
        /// <remarks>
        /// The item is scanned for publications and subscriptions and wired to the corresponding invokers and handlers.
        /// </remarks>
        /// <param name="item">Item to register with the event broker.</param>
        void Register(object item);

        /// <summary>
        /// Unregisters the specified item from this event broker.
        /// </summary>
        /// <param name="item">The item to unregister.</param>
        void Unregister(object item);

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