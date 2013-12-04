//-------------------------------------------------------------------------------
// <copyright file="AbstractEventMessageFactory.cs" company="Appccelerate">
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

namespace Appccelerate.DistributedEventBroker.Factories
{
    using System;
    using Appccelerate.DistributedEventBroker.Messages;

    /// <summary>
    /// Abstract event message factory which uses a callback delegate to create
    /// messages and initialize the message properties.
    /// </summary>
    public abstract class AbstractEventMessageFactory : IEventMessageFactory
    {
        /// <summary>
        /// Creates the event fired message.
        /// </summary>
        /// <param name="initializer">The initializer which can be used to initialize messages.</param>
        /// <returns>An initialized message.</returns>
        public abstract IEventFired CreateEventFiredMessage(Action<IEventFired> initializer);

        /// <summary>
        /// Creates the event fired message by instantiating the provided generic types.
        /// </summary>
        /// <typeparam name="TEventFired">The type of the event fired message.</typeparam>
        /// <typeparam name="TIEventFired">The interface type of the event fired message.</typeparam>
        /// <param name="initializer">The initializer which can be used to initialize the message.</param>
        /// <returns>An initialized message.</returns>
        protected virtual TIEventFired CreateEventFiredMessage<TEventFired, TIEventFired>(Action<TIEventFired> initializer)
            where TIEventFired : IEventFired
            where TEventFired : TIEventFired, new()
        {
            Ensure.ArgumentNotNull(initializer, "initializer");

            var eventFired = new TEventFired();

            initializer(eventFired);

            return eventFired;
        }
    }
}