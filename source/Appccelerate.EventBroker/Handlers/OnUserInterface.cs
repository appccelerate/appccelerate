//-------------------------------------------------------------------------------
// <copyright file="OnUserInterface.cs" company="Appccelerate">
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

namespace Appccelerate.EventBroker.Handlers
{
    using System;
    using System.Reflection;
    using System.Threading;

    /// <summary>
    /// Handler that executes the subscription synchronously on the user interface thread (Send semantics).
    /// </summary>
    public class OnUserInterface : EventBrokerHandlerBase
    {
        /// <summary>
        /// The synchronization context that is used to switch to the UI thread.
        /// </summary>
        private readonly UserInterfaceSyncContextHolder syncContextHolder = new UserInterfaceSyncContextHolder();

        /// <summary>
        /// Gets the kind of the handler, whether it is a synchronous or asynchronous handler.
        /// </summary>
        /// <value>The kind of the handler (synchronous or asynchronous).</value>
        public override HandlerKind Kind
        {
            get { return HandlerKind.Synchronous; }
        }

        /// <summary>
        /// Initializes the handler with the synchronization context for the user interface thread, which has to be the currently running process.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">Handler method on the subscriber.</param>
        /// <param name="extensionHost">The extension host.</param>
        public override void Initialize(object subscriber, MethodInfo handlerMethod, IExtensionHost extensionHost)
        {
            this.syncContextHolder.Initalize(subscriber, handlerMethod);
        }

        /// <summary>
        /// Executes the subscription synchronously on the user interface thread.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <param name="subscriptionHandler">The subscription handler.</param>
        public override void Handle(IEventTopicInfo eventTopic, object sender, EventArgs e, Delegate subscriptionHandler)
        {
            if (this.RunningOnUserInterfaceThread())
            {
                this.CallWithoutThreadSwitch(eventTopic, subscriptionHandler, sender, e);
            }
            else
            {
                this.CallWithThreadSwitch(eventTopic, subscriptionHandler, sender, e);
            }
        }

        private bool RunningOnUserInterfaceThread()
        {
            return Thread.CurrentThread.ManagedThreadId == this.syncContextHolder.ThreadId;
        }

        private void CallWithoutThreadSwitch(IEventTopicInfo eventTopic, Delegate subscriptionHandler, object sender, EventArgs e)
        {
            try
            {
                subscriptionHandler.DynamicInvoke(sender, e);
            }
            catch (TargetInvocationException exception)
            {
                this.HandleSubscriberMethodException(exception, eventTopic);
            }
        }

        private void CallWithThreadSwitch(IEventTopicInfo eventTopic, Delegate subscriptionHandler, object sender, EventArgs e)
        {
            this.syncContextHolder.SyncContext.Send(
                delegate(object data)
                    {
                        try
                        {
                            ((Delegate)data).DynamicInvoke(sender, e);
                        }
                        catch (TargetInvocationException exception)
                        {
                            this.HandleSubscriberMethodException(exception, eventTopic);
                        }
                    },
                subscriptionHandler);
        }
    }
}