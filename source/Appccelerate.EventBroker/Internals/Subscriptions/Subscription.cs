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

namespace Appccelerate.EventBroker.Internals.Subscriptions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    using Appccelerate.EventBroker.Internals.Exceptions;
    using Appccelerate.EventBroker.Matchers;
    using Appccelerate.Formatters;

    /// <summary>
    /// Represents a topic subscription.
    /// </summary>
    internal class Subscription : ISubscription
    {
        private readonly WeakReference subscriber;
        private readonly string handlerMethodName;
        private readonly MethodInfo handlerMethodInfo;
        private readonly Type eventHandlerType;
        private readonly IList<ISubscriptionMatcher> subscriptionMatchers;
        private readonly IHandler handler;
        private readonly IExtensionHost extensionHost;
        private readonly Type eventArgsType;

        public Subscription(
            object subscriber, 
            MethodInfo handlerMethod, 
            IHandler handler, 
            IList<ISubscriptionMatcher> subscriptionMatchers,
            IExtensionHost extensionHost)
        {
            Ensure.ArgumentNotNull(handlerMethod, "handlerMethod");

            CheckHandlerMethodIsNotStatic(handlerMethod);

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

        public Type EventArgsType
        {
            get { return this.eventArgsType; }
        }

        public object Subscriber
        {
            get { return this.subscriber.Target; }
        }

        public string HandlerMethodName
        {
            get { return this.handlerMethodName; }
        }

        public IHandler Handler
        {
            get { return this.handler; }
        }

        public IList<ISubscriptionMatcher> SubscriptionMatchers
        {
            get { return this.subscriptionMatchers; }
        }

        public EventTopicFireDelegate GetHandler()
        {
            return this.EventTopicFireHandler;
        }

        public void DescribeTo(TextWriter writer)
        {
            Ensure.ArgumentNotNull(writer, "writer");

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

        private static void CheckHandlerMethodIsNotStatic(MethodInfo handlerMethod)
        {
            if (handlerMethod.IsStatic)
            {
                throw new StaticSubscriberHandlerException(handlerMethod);
            }
        }

        private static bool IsValidEventHandler(ParameterInfo[] parameters)
        {
            return parameters.Length == 2 && typeof(EventArgs).IsAssignableFrom(parameters[1].ParameterType);
        }

        private void EventTopicFireHandler(IEventTopicInfo eventTopic, object sender, EventArgs e, IPublication publication)
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

        private Delegate CreateSubscriptionDelegate()
        {
            return this.Subscriber != null ? Delegate.CreateDelegate(this.eventHandlerType, this.Subscriber, this.handlerMethodInfo) : null;
        }
    }
}