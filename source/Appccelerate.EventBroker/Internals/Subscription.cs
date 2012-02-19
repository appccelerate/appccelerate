//-------------------------------------------------------------------------------
// <copyright file="Subscription.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Internals
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    using Appccelerate.EventBroker.Exceptions;
    using Appccelerate.EventBroker.Matchers;
    using Appccelerate.Formatters;

    /// <summary>
    /// Represents a topic subscription.
    /// </summary>
    internal class Subscription : ISubscription
    {
        /// <summary>
        /// Weak reference to the subscriber.
        /// </summary>
        private readonly WeakReference subscriber;

        /// <summary>
        /// Name of the handler method on the subscriber.
        /// </summary>
        private readonly string handlerMethodName;

        /// <summary>
        /// The method of the subscriber that is called on the event.
        /// </summary>
        private readonly MethodInfo handlerMethodInfo;

        /// <summary>
        /// Type of the event handler the subscription handler implements.
        /// </summary>
        private readonly Type eventHandlerType;

        /// <summary>
        /// The subscription matchers used for this subscription.
        /// </summary>
        private readonly IList<ISubscriptionMatcher> subscriptionMatchers;

        /// <summary>
        /// The handler used for this subscription.
        /// </summary>
        private readonly IHandler handler;

        /// <summary>
        /// The extension host holding all extensions.
        /// </summary>
        private readonly IExtensionHost extensionHost;

        private readonly Type eventArgsType;

        /// <summary>
        /// Initializes a new instance of the <see cref="Subscription"/> class.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">The handler method.</param>
        /// <param name="handler">The handler used to execute the subscription.</param>
        /// <param name="subscriptionMatchers">The subscription matchers used for this subscription.</param>
        /// <param name="extensionHost">The extension host holding all extensions.</param>
        public Subscription(
            object subscriber, 
            MethodInfo handlerMethod, 
            IHandler handler, 
            IList<ISubscriptionMatcher> subscriptionMatchers,
            IExtensionHost extensionHost)
        {
            if (handlerMethod == null)
            {
                throw new ArgumentNullException("handlerMethod", "handlerMethod must not be null.");
            }

            if (handlerMethod.IsStatic)
            {
                throw new StaticSubscriberHandlerException(handlerMethod);
            }

            this.subscriber = new WeakReference(subscriber);
            this.handlerMethodName = handlerMethod.Name;
            this.handlerMethodInfo = handlerMethod;
            this.subscriptionMatchers = subscriptionMatchers;
            this.handler = handler;
            this.extensionHost = extensionHost;

            ParameterInfo[] parameters = handlerMethod.GetParameters();
            if (IsValidEventHandler(parameters))
            {
                ParameterInfo parameterInfo = handlerMethod.GetParameters()[1];
                this.eventArgsType = parameterInfo.ParameterType;
                this.eventHandlerType = typeof(EventHandler<>).MakeGenericType(this.eventArgsType);
            }
            else
            {
                throw new InvalidSubscriptionSignatureException(handlerMethod);
            }

            handler.Initialize(subscriber, handlerMethod, this.extensionHost);
        }

        /// <summary>
        /// Gets the type of the event arguments this subscription is using.
        /// </summary>
        /// <value>The type of the event arguments.</value>
        public Type EventArgsType
        {
            get { return this.eventArgsType; }
        }

        /// <summary>
        /// Gets the subscriber of the event.
        /// </summary>
        public object Subscriber
        {
            get { return this.subscriber.Target; }
        }

        /// <summary>
        /// Gets the handler method name that's subscribed to the event.
        /// </summary>
        public string HandlerMethodName
        {
            get { return this.handlerMethodName; }
        }

        /// <summary>
        /// Gets the handler of this subscription.
        /// </summary>
        /// <value>The handler of this subscription.</value>
        public IHandler Handler
        {
            get { return this.handler; }
        }

        /// <summary>
        /// Gets the subscription matchers.
        /// </summary>
        /// <value>The subscription matchers.</value>
        public IList<ISubscriptionMatcher> SubscriptionMatchers
        {
            get { return this.subscriptionMatchers; }
        }

        /// <summary>
        /// Gets the handler that will be called by the <see cref="IEventTopic"/> during a firing sequence.
        /// </summary>
        /// <returns>A delegate that is used to call the subscription handler.</returns>
        public EventTopicFireDelegate GetHandler()
        {
            return this.EventTopicFireHandler;
        }

        /// <summary>
        /// Describes this subscription:
        /// name, thread option, scope, event arguments.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void DescribeTo(TextWriter writer)
        {
            if (this.subscriber.IsAlive)
            {
                writer.Write(this.Subscriber.GetType().FullName);

                if (this.Subscriber is INamedItem)
                {
                    writer.Write(", Name = ");
                    writer.Write(((INamedItem)this.Subscriber).EventBrokerItemName);
                }

                writer.Write(", Handler method = ");
                writer.Write(this.handlerMethodName);
                
                writer.Write(", Handler = ");
                writer.Write(this.handler.GetType().FullNameToString());

                writer.Write(", EventArgs type = ");
                writer.Write(this.eventHandlerType.FullNameToString());

                writer.Write(", matchers = ");
                foreach (ISubscriptionMatcher subscriptionMatcher in this.subscriptionMatchers)
                {
                    subscriptionMatcher.DescribeTo(writer);
                    writer.Write(" ");
                }
            }
        }

        /// <summary>
        /// Determines whether the specified parameters are valid event handler parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>True if valid parameters.</returns>
        private static bool IsValidEventHandler(ParameterInfo[] parameters)
        {
            return parameters.Length == 2 && typeof(EventArgs).IsAssignableFrom(parameters[1].ParameterType);
        }

        /// <summary>
        /// Handler that is called when a topic is fired.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <param name="publication">The publication.</param>
        private void EventTopicFireHandler(IEventTopic eventTopic, object sender, EventArgs e, IPublication publication)
        {
            if (this.Subscriber == null)
            {
                return;
            }
            
            Delegate subscriptionHandler = this.CreateSubscriptionDelegate();
            if (subscriptionHandler == null)
            {
                return;
            }
            
            this.extensionHost.ForEach(extension => extension.RelayingEvent(eventTopic, publication, this, this.handler, sender, e));

            this.handler.Handle(eventTopic, sender, e, subscriptionHandler);

            this.extensionHost.ForEach(extension => extension.RelayedEvent(eventTopic, publication, this, this.handler, sender, e));
        }

        /// <summary>
        /// Creates the subscription delegate.
        /// </summary>
        /// <returns>A delegate that is used to call the subscription handler method.</returns>
        private Delegate CreateSubscriptionDelegate()
        {
            return this.Subscriber != null ? Delegate.CreateDelegate(this.eventHandlerType, this.Subscriber, this.handlerMethodInfo) : null;
        }
    }
}