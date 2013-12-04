//-------------------------------------------------------------------------------
// <copyright file="EventBrokerHandlerBase.cs" company="Appccelerate">
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
    using System;
    using System.Reflection;

    using Appccelerate.EventBroker.Exceptions;
    using Appccelerate.EventBroker.Internals.Subscriptions;

    /// <summary>
    /// Abstract base class for event broker handles providing the host of extensions.
    /// </summary>
    public abstract class EventBrokerHandlerBase : IHandler
    {
        /// <summary>
        /// Gets the kind of the handler, whether it is a synchronous or asynchronous handler.
        /// </summary>
        /// <value>The kind of the handler (synchronous or asynchronous).</value>
        public abstract HandlerKind Kind { get; }

        /// <summary>
        /// Gets the extension host.
        /// </summary>
        /// <value>The extension host.</value>
        protected IExtensionHost ExtensionHost { get; private set; }

        /// <summary>
        /// Initializes the handler.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">Name of the handler method on the subscriber.</param>
        /// <param name="extensionHost">Provides access to all registered extensions.</param>
        public virtual void Initialize(object subscriber, MethodInfo handlerMethod, IExtensionHost extensionHost)
        {
            this.ExtensionHost = extensionHost;
        }
        
        public abstract void Handle(IEventTopicInfo eventTopic, object subscriber, object sender, EventArgs e, IDelegateWrapper delegateWrapper);

        /// <summary>
        /// Handles a subscriber method exception by passing it to all extensions and re-throwing the inner exception in case that none of the
        /// extensions handled it.
        /// </summary>
        /// <param name="targetInvocationException">The targetInvocationException.</param>
        /// <param name="eventTopic">The event topic.</param>
        protected void HandleSubscriberMethodException(TargetInvocationException targetInvocationException, IEventTopicInfo eventTopic)
        {
            Ensure.ArgumentNotNull(targetInvocationException, "targetInvocationException");

            var innerException = targetInvocationException.InnerException;
            innerException.PreserveStackTrace();

            var context = new ExceptionHandlingContext();

            this.ExtensionHost.ForEach(extension => extension.SubscriberExceptionOccurred(eventTopic, innerException, context));
                
            if (!context.Handled)
            {
                throw innerException;
            }
        }
    }
}