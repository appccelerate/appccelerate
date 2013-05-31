//-------------------------------------------------------------------------------
// <copyright file="MassTransitDistributedEventBrokerExtension.cs" company="Appccelerate">
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

namespace Appccelerate.DistributedEventBroker.MassTransit
{
    using global::MassTransit;

    /// <summary>
    /// Distributed event broker extension for mass transit.
    /// </summary>
    public class MassTransitDistributedEventBrokerExtension : DistributedEventBrokerExtensionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MassTransitDistributedEventBrokerExtension"/> class.
        /// </summary>
        /// <param name="distributedEventBrokerIdentification">The distributed event broker identification.</param>
        /// <param name="serviceBus">The service bus.</param>
        public MassTransitDistributedEventBrokerExtension(string distributedEventBrokerIdentification, IServiceBus serviceBus)
            : base(distributedEventBrokerIdentification, new MassTransitEventBrokerBus(serviceBus))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MassTransitDistributedEventBrokerExtension"/> class.
        /// </summary>
        /// <param name="distributedEventBrokerIdentification">The distributed event broker identification.</param>
        /// <param name="serviceBus">The service bus.</param>
        /// <param name="factory">The factory.</param>
        public MassTransitDistributedEventBrokerExtension(string distributedEventBrokerIdentification, IServiceBus serviceBus, IDistributedFactory factory) :
            base(distributedEventBrokerIdentification, new MassTransitEventBrokerBus(serviceBus), factory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MassTransitDistributedEventBrokerExtension"/> class.
        /// </summary>
        /// <param name="distributedEventBrokerIdentification">The distributed event broker identification.</param>
        /// <param name="eventBrokerBus">The event broker bus.</param>
        /// <param name="factory">The factory.</param>
        public MassTransitDistributedEventBrokerExtension(string distributedEventBrokerIdentification, IEventBrokerBus eventBrokerBus, IDistributedFactory factory) :
            base(distributedEventBrokerIdentification, eventBrokerBus, factory)
        {
        }
    }
}