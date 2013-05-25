//-------------------------------------------------------------------------------
// <copyright file="NotUserInterfaceThreadException.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Handlers
{
    using Appccelerate.EventBroker.Exceptions;

    /// <summary>
    /// An <see cref="EventBrokerException"/> thrown when a subscription on UI thread is found on a subscriber that is not registered on the UI thread.
    /// </summary>
    public class NotUserInterfaceThreadException : EventBrokerException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotUserInterfaceThreadException"/> class.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethodName">Name of the handler method.</param>
        public NotUserInterfaceThreadException(object subscriber, string handlerMethodName)
            : base("Subscriptions on UserInterface Thread can only be done with subscribers registered in UI thread. Subscriber: '{0}' HandlerMethod: '{1}'.", subscriber, handlerMethodName)
        {
        }
    }
}
