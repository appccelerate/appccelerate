//-------------------------------------------------------------------------------
// <copyright file="NServiceBusEventFiredHandler.cs" company="Appccelerate">
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

namespace Appccelerate.DistributedEventBroker.NServiceBus.Handlers
{
    using DistributedEventBroker.Handlers;
    using Messages;
    using global::NServiceBus;

    /// <summary>
    /// Handler which subscribes to <see cref="INServiceBusEventFired"/>
    /// </summary>
    public class NServiceBusEventFiredHandler : EventFiredHandlerBase, IHandleMessages<INServiceBusEventFired>
    {
        /// <summary>
        /// Handles a message.
        /// </summary>
        /// <param name="message">The message to handle.</param>
        /// <remarks>
        /// This method will be called when a message arrives on the bus and should contain
        ///             the custom logic to execute when the message is received.
        /// </remarks>
        public void Handle(INServiceBusEventFired message)
        {
            this.DoHandle(message);
        }
    }
}