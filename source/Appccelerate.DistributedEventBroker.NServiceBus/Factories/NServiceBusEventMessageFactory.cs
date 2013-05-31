//-------------------------------------------------------------------------------
// <copyright file="NServiceBusEventMessageFactory.cs" company="Appccelerate">
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

namespace Appccelerate.DistributedEventBroker.NServiceBus.Factories
{
    using System;
    using Appccelerate.DistributedEventBroker.Factories;
    using Appccelerate.DistributedEventBroker.Messages;
    using Appccelerate.DistributedEventBroker.NServiceBus.Messages;

    /// <summary>
    /// Event message factory which creates NServiceBus specific message types.
    /// </summary>
    public class NServiceBusEventMessageFactory : AbstractEventMessageFactory
    {
        /// <summary>
        /// Creates the event fired message of type <see cref="NServiceBusEventFired"/>.
        /// </summary>
        /// <param name="initializer">The initializer which can be used to initialize messages.</param>
        /// <returns>An initialized message.</returns>
        public override IEventFired CreateEventFiredMessage(Action<IEventFired> initializer)
        {
            return this.CreateEventFiredMessage<NServiceBusEventFired, IEventFired>(initializer);
        }
    }
}