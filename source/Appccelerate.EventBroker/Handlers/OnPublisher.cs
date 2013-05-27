//-------------------------------------------------------------------------------
// <copyright file="OnPublisher.cs" company="Appccelerate">
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

    using Appccelerate.EventBroker.Internals.Subscriptions;

    /// <summary>
    /// Handler that executes the subscription on the same thread the publisher is currently running (synchronous).
    /// </summary>
    public class OnPublisher : EventBrokerHandlerBase
    {
        /// <summary>
        /// Gets the kind of the handler, whether it is a synchronous or asynchronous handler.
        /// </summary>
        /// <value>The kind of the handler (synchronous or asynchronous).</value>
        public override HandlerKind Kind
        {
            get { return HandlerKind.Synchronous; }
        }
        
        public override void Handle(IEventTopicInfo eventTopic, object subscriber, object sender, EventArgs e, IDelegateWrapper delegateWrapper)
        {
            Ensure.ArgumentNotNull(delegateWrapper, "delegateWrapper");

            try
            {
                delegateWrapper.Invoke(subscriber, sender, e);
            }
            catch (TargetInvocationException ex)
            {
                this.HandleSubscriberMethodException(ex, eventTopic);
            }
        }
    }
}