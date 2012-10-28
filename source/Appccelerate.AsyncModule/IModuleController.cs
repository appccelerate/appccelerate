//-------------------------------------------------------------------------------
// <copyright file="IModuleController.cs" company="Appccelerate">
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

namespace Appccelerate.AsyncModule
{
    using System;
    using Events;
    using Extensions;

    /// <summary>
    /// The module controller is accessed through this interface.
    /// </summary>
    public interface IModuleController
    {
        /// <summary>
        /// This event is raised before the module is started. Extensions
        /// use this event to insert actions before the module starts.
        /// </summary>
        event EventHandler BeforeModuleStart;

        /// <summary>
        /// This event is raised after the module has been started. Extensions
        /// use this event to insert actions after the module starts.
        /// </summary>
        event EventHandler AfterModuleStart;

        /// <summary>
        /// This event is raised before the module is stopped. Extensions
        /// use this event to insert actions before the module stops.
        /// </summary>
        event EventHandler BeforeModuleStop;

        /// <summary>
        /// This event is raised after the module has been stopped. Extensions
        /// use this event to insert actions after the module stops.
        /// </summary>
        event EventHandler AfterModuleStop;

        /// <summary>
        /// This event is raised before a call to the message consumer. Extensions
        /// use this event to insert actions before the message is consumed.
        /// </summary>
        event EventHandler<BeforeConsumeMessageEventArgs> BeforeConsumeMessage;

        /// <summary>
        /// This event is raised after a call to the message consumer. Extensions
        /// use this event to insert actions after the message has been consumed.
        /// </summary>
        event EventHandler<AfterConsumeMessageEventArgs> AfterConsumeMessage;

        /// <summary>
        /// This event is raised before the message is queued. Extensions
        /// use this event to insert actions before the message is queued.
        /// </summary>
        event EventHandler<EnqueueMessageEventArgs> BeforeEnqueueMessage;

        /// <summary>
        /// This event is raised after a message was queued. Extensions
        /// use this event to insert actions after the message is queued.
        /// </summary>
        event EventHandler<EnqueueMessageEventArgs> AfterEnqueueMessage;

        /// <summary>
        /// This event is raised when an exception is thrown during consuming a message.
        /// This event is used to react meaningful to exceptions like a retrying mechanism.
        /// The event handler can return a value indicating whether the exception was handled.
        /// </summary>
        event EventHandler<ConsumeMessageExceptionEventArgs> ConsumeMessageExceptionOccurred;

        /// <summary>
        /// This event is raised when an unhandled exception in a module occurred that was not handled
        /// by the <see cref="ConsumeMessageExceptionOccurred"/> event.
        /// This event is just for notification, afterwards the module will continue with the next message.
        /// </summary>
        event EventHandler<UnhandledModuleExceptionEventArgs> UnhandledModuleExceptionOccured;
        
        /// <summary>
        /// Gets the number of messages in the message queue.
        /// </summary>
        int MessageCount { get; }

        /// <summary>
        /// Gets the messages in the message queue.
        /// </summary>
        /// <value>The messages.</value>
        object[] Messages { get; }

        /// <summary>
        /// Gets a value indicating whether the thread(s) managed by the module controller is/are
        /// alive.
        /// </summary>
        /// <value><c>true</c> if this instance is alive; otherwise, <c>false</c>.</value>
        bool IsAlive { get; }

        /// <summary>
        /// Gets or sets the log extension.
        /// </summary>
        /// <value>The log extension.</value>
        IAsyncModuleLogExtension LogExtension { get; set; }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="module">The module to control.</param>
        /// <param name="numberOfThreads">The number of worker threads.</param>
        /// <param name="runWithBackgroundThread">if set to <c>true</c> then background threads are used.</param>
        /// <param name="logger">The name used for the logger.</param>
        void Initialize(object module, int numberOfThreads, bool runWithBackgroundThread, string logger);

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="module">The module to control.</param>
        /// <param name="runWithBackgroundThread">if set to <c>true</c> then background threads are used.</param>
        void Initialize(object module, bool runWithBackgroundThread);

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="module">The module to control.</param>
        /// <param name="numberOfThreads">The number of worker threads.</param>
        void Initialize(object module, int numberOfThreads);

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="module">The module to control.</param>
        void Initialize(object module);

        /// <summary>
        /// Called by the coordinator to send a message to the module
        /// managed by the controller.
        /// </summary>
        /// <param name="message">
        /// The message can be of any type as long as the sender and 
        /// the receiver both know it.
        /// </param>
        void EnqueueMessage(object message);

        /// <summary>
        /// Puts a message to the front of the message queue of the module managed
        /// by this controller.
        /// </summary>
        /// <param name="message">
        /// The message to be queued.
        /// </param>
        void EnqueuePriorityMessage(object message);

        /// <summary>
        /// Clears all messages still in the queue and returns them.
        /// </summary>
        /// <returns>All messages that were cleared from the queue.</returns>
        object[] ClearMessages();
        
        /// <summary>
        /// Starts the message consumer thread(s). If the thread(s) is/are already 
        /// started this method returns immediately.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the message consumer thread(s).
        /// </summary>
        void Stop();

        /// <summary>
        /// Stops the message consumer thread(s) after finishing
        /// an message currently processed. If the thread is
        /// not responding anymore it is killed.
        /// </summary>
        /// <param name="timeout">The timeout that defines how long we will wait until the worker threads are killed, i.e. how long a currently 
        /// processing message is tolerated.</param>
        void Stop(TimeSpan timeout);

        /// <summary>
        /// Stops the message consumer threads. This method can be called from a message consumer thread.
        /// </summary>
        void StopAsync();

        /// <summary>
        /// Adds an extension to this controller.
        /// </summary>
        /// <typeparam name="TExtension">The type of the extension.</typeparam>
        /// <param name="extension">The extension.</param>
        void AddExtension<TExtension>(TExtension extension) where TExtension : IModuleExtension;

        /// <summary>
        /// Gets the extension with the specified type.
        /// </summary>
        /// <typeparam name="TExtension">The type of the extension.</typeparam>
        /// <returns>The extension with the specified type.</returns>
        TExtension GetExtension<TExtension>();
    }
}
