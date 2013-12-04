//-------------------------------------------------------------------------------
// <copyright file="NServiceBusEventBrokerBus.cs" company="Appccelerate">
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

namespace Appccelerate.DistributedEventBroker.NServiceBus
{
    using System;
    using System.Globalization;
    using DistributedEventBroker.Messages;
    using Messages;
    
    using global::NServiceBus;

    /// <summary>
    /// NServiceBus specific event broker bus implementation.
    /// </summary>
    public class NServiceBusEventBrokerBus : IEventBrokerBus
    {
        private readonly IBus serviceBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="NServiceBusEventBrokerBus"/> class.
        /// </summary>
        /// <param name="serviceBus">The service bus.</param>
        public NServiceBusEventBrokerBus(IBus serviceBus)
        {
            this.serviceBus = serviceBus;
        }

        /// <summary>
        /// Publishes the specified event fired message on the bus.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Publish(IEventFired message)
        {
            var eventFired = message as INServiceBusEventFired;

            if (eventFired == null)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Type must be convertible to {0}!",
                        typeof(INServiceBusEventFired).Name),
                    "message");
            }

            this.serviceBus.Publish(eventFired);
        }
    }
}