//-------------------------------------------------------------------------------
// <copyright file="IEventMessageFactory.cs" company="Appccelerate">
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

namespace Appccelerate.DistributedEventBroker
{
    using System;
    using Messages;

    /// <summary>
    /// Event message factory definition which uses a callback to initialize messages.
    /// </summary>
    public interface IEventMessageFactory
    {
        /// <summary>
        /// Creates the event fired message.
        /// </summary>
        /// <param name="initializer">The initializer which can be used to initialize messages.</param>
        /// <returns>An initialized message.</returns>
        IEventFired CreateEventFiredMessage(Action<IEventFired> initializer);
    }
}