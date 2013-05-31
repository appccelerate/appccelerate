//-------------------------------------------------------------------------------
// <copyright file="OnBackground.cs" company="Appccelerate">
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
    using System.Threading;

    using Appccelerate.EventBroker.Internals.Subscriptions;

    /// <summary>
    /// Handler that executes the subscription on a thread pool worker process (asynchronous).
    /// </summary>
    public class OnBackground : EventBrokerHandlerBase
    {
        /// <summary>
        /// Gets the kind of the handler, whether it is a synchronous or asynchronous handler.
        /// </summary>
        /// <value>The kind of the handler (synchronous or asynchronous).</value>
        public override HandlerKind Kind
        {
            get
            {
                return HandlerKind.Asynchronous;
            }
        }

        public override void Handle(IEventTopicInfo eventTopic, object subscriber, object sender, EventArgs e, IDelegateWrapper delegateWrapper)
        {
            ThreadPool.QueueUserWorkItem(
                delegate(object state)
                    {
                        try
                        {
                            var args = (CallInBackgroundArguments)state;
                            args.DelegateWrapper.Invoke(args.Subscriber, args.Sender, args.EventArgs);
                        }
                        catch (TargetInvocationException exception)
                        {
                            this.HandleSubscriberMethodException(exception, eventTopic);
                        }
                    },
                new CallInBackgroundArguments(subscriber, sender, e, delegateWrapper));
        }

        private struct CallInBackgroundArguments
        {
            public readonly IDelegateWrapper DelegateWrapper;

            public readonly object Subscriber;

            public readonly object Sender;

            public readonly EventArgs EventArgs;

            public CallInBackgroundArguments(object subscriber, object sender, EventArgs eventArgs, IDelegateWrapper delegateWrapper)
            {
                this.Subscriber = subscriber;
                this.Sender = sender;
                this.EventArgs = eventArgs;
                this.DelegateWrapper = delegateWrapper;
            }
        }
    }
}