//-------------------------------------------------------------------------------
// <copyright file="SubscriberHandlerNotFoundException.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Internals.Exceptions
{
    using System;

    using Appccelerate.EventBroker.Exceptions;

    /// <summary>
    /// An <see cref="EventBrokerException"/> thrown when a handler method can not be found.
    /// </summary>
    public class SubscriberHandlerNotFoundException : EventBrokerException
    {
        /// <summary>
        /// The type of the subscriber.
        /// </summary>
        private readonly Type subscriberType;
        
        /// <summary>
        /// The name of the handler method.
        /// </summary>
        private readonly string handlerMethodName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriberHandlerNotFoundException"/> class.
        /// </summary>
        /// <param name="subscriberType">Type of the subscriber.</param>
        /// <param name="handlerMethodName">Name of the handler method.</param>
        public SubscriberHandlerNotFoundException(Type subscriberType, string handlerMethodName) 
            : base("Subscriber handler not found: '{0}.{1}'", subscriberType.FullName, handlerMethodName)
        {
            this.subscriberType = subscriberType;
            this.handlerMethodName = handlerMethodName;
        }

        /// <summary>
        /// Gets the type of the subscriber.
        /// </summary>
        /// <value>The type of the subscriber.</value>
        public Type SubscriberType
        {
            get { return this.subscriberType; }
        }

        /// <summary>
        /// Gets the name of the handler method.
        /// </summary>
        /// <value>The name of the handler method.</value>
        public string HandlerMethodName
        {
            get { return this.handlerMethodName; }
        }
    }
}
