//-------------------------------------------------------------------------------
// <copyright file="ModuleController.cs" company="Appccelerate">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using Events;
    using Extensions;

    /// <summary>
    /// The module coordinator adds a message queue and a message
    /// consumer thread to the controlled object.
    /// </summary>
    public class ModuleController : IModuleController
    {
        /// <summary>
        /// Default number of threads used to consume messages.
        /// </summary>
        private const int DefaultNumberOfThreads = 1;

        /// <summary>
        /// Default timeout that defines how long the controller waits for a consuming message
        /// before killing the worker thread.
        /// </summary>
        private static readonly TimeSpan DefaultTimeOut = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Lock object the access <see cref="threadStopping"/> field.
        /// </summary>
        private readonly object threadStoppingLock = new object();

        /// <summary>
        /// Lock object which synchronizes producer and consumer.
        /// </summary>
        private readonly object lockingObject = new object();

        /// <summary>
        /// The extensions of the module.
        /// </summary>
        private readonly ModuleExtensionCollection extensions;

        /// <summary>
        /// Message queue. Is filled through the PostMessage method
        /// of the ModuleCoordinator. The messages are consumed by
        /// the configured consumer delegate.
        /// </summary>
        private readonly LinkedList<object> messageQueue;

        /// <summary>
        /// Whether the worker threads are background threads or not.
        /// </summary>
        private bool runWithBackgroundThread;

        /// <summary>
        /// Number of threads used to consume messages.
        /// </summary>
        private int numberOfThreads = DefaultNumberOfThreads;

        /// <summary>
        /// This threads listen to messages which are placed in 
        /// the message queue.
        /// </summary>
        private List<Thread> messageConsumerThreads;

        /// <summary>
        /// A list of message consumer methods.
        /// </summary>
        private List<MethodInfo> consumeMessageMethodInfos;

        /// <summary>
        /// The module controlled by the controller.
        /// </summary>
        private object controlledModule;

        /// <summary>
        /// Indicates whether we are stopping worker threads.
        /// </summary>
        private bool threadStopping;

        private IAsyncModuleLogExtension logExtension;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleController"/> class.
        /// </summary>
        public ModuleController()
        {
            this.messageQueue = new LinkedList<object>();
            this.extensions = new ModuleExtensionCollection();
        }

        /// <summary>
        /// This event is raised when an unhandled exception in a module occurred.
        /// </summary>
        public event EventHandler<UnhandledModuleExceptionEventArgs> UnhandledModuleExceptionOccured;

        /// <summary>
        /// This event is raised before the module is started. Extensions
        /// use this event to insert actions before the module starts.
        /// </summary>
        public event EventHandler BeforeModuleStart;

        /// <summary>
        /// This event is raised after the module has been started. Extensions
        /// use this event to insert actions after the module starts.
        /// </summary>
        public event EventHandler AfterModuleStart;

        /// <summary>
        /// This event is raised before the module is stopped. Extensions
        /// use this event to insert actions before the module stops.
        /// </summary>
        public event EventHandler BeforeModuleStop;

        /// <summary>
        /// This event is raised after the module has been stopped. Extensions
        /// use this event to insert actions after the module stops.
        /// </summary>
        public event EventHandler AfterModuleStop;

        /// <summary>
        /// This event is raised before a call to the message consumer. Extensions
        /// use this event to insert actions before the message is consumed.
        /// </summary>
        public event EventHandler<BeforeConsumeMessageEventArgs> BeforeConsumeMessage;

        /// <summary>
        /// This event is raised after a call to the message consumer. Extensions
        /// use this event to insert actions after the message has been consumed.
        /// </summary>
        public event EventHandler<AfterConsumeMessageEventArgs> AfterConsumeMessage;

        /// <summary>
        /// This event is raised before the message is enqueued. Extensions
        /// use this event to insert actions after before the message is enqueued.
        /// </summary>
        public event EventHandler<EnqueueMessageEventArgs> BeforeEnqueueMessage;

        /// <summary>
        /// This event is raised after the message is enqueued. Extensions
        /// use this event to insert actions after before the message is enqueued.
        /// </summary>
        public event EventHandler<EnqueueMessageEventArgs> AfterEnqueueMessage;

        /// <summary>
        /// This event is raised when an exception is thrown during consuming a message.
        /// This event is used to react meaningful to exceptions like a retrying mechanism.
        /// The event handler can return a value indicating whether the exception was handled.
        /// </summary>
        public event EventHandler<ConsumeMessageExceptionEventArgs> ConsumeMessageExceptionOccurred;

        /// <summary>
        /// Gets the number of messages in the message queue.
        /// </summary>
        public int MessageCount
        {
            get
            {
                lock (this.messageQueue)
                {
                    return this.messageQueue.Count;
                }
            }
        }

        /// <summary>
        /// Gets the messages currently in the queue.
        /// </summary>
        /// <value>The messages.</value>
        public object[] Messages
        {
            get
            {
                lock (this.lockingObject)
                {
                    return this.messageQueue.ToArray();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the message consumer thread(s) is/are alive.
        /// </summary>
        public bool IsAlive
        {
            get
            {
                lock (this.lockingObject)
                {
                    return this.controlledModule != null && this.messageConsumerThreads.Count > 0;
                }
            }
        }

        /// <summary>
        /// Gets or sets the log extension.
        /// </summary>
        /// <value>The log extension.</value>
        public IAsyncModuleLogExtension LogExtension
        {
            get
            {
                return this.logExtension ?? new EmptyAsyncModuleLogExtension();
            }

            set
            {
                this.logExtension = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether we are stopping the worker threads.
        /// </summary>
        /// <value><c>true</c> if threads are stopping; otherwise, <c>false</c>.</value>
        private bool ThreadStopping
        {
            get
            {
                lock (this.threadStoppingLock)
                {
                    return this.threadStopping;
                }
            }

            set
            {
                lock (this.threadStoppingLock)
                {
                    this.threadStopping = value;
                }
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="module">The module to control.</param>
        /// <param name="numberOfThreads">The number of worker threads.</param>
        /// <param name="runWithBackgroundThread">if set to <c>true</c> then background threads are used.</param>
        /// <param name="logger">The name used for the logger.</param>
        public void Initialize(object module, int numberOfThreads, bool runWithBackgroundThread, string logger)
        {
            if (this.controlledModule != null)
            {
                throw new InvalidOperationException("ModuleController is already initialized.");
            }

            if (module == null)
            {
                throw new ArgumentNullException("module", "module must not be null.");
            }

            if (numberOfThreads <= 0)
            {
                throw new ArgumentException("numberOfThreads has to be greater than 0.", "numberOfThreads");
            }

            this.numberOfThreads = numberOfThreads;
            this.messageConsumerThreads = new List<Thread>(numberOfThreads);

            this.runWithBackgroundThread = runWithBackgroundThread;

            this.controlledModule = module;
            this.consumeMessageMethodInfos = GetConsumeMessageMethodInfos(module);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="module">The module to control.</param>
        /// <param name="runWithBackgroundThread">if set to <c>true</c> then background threads are used.</param>
        public void Initialize(object module, bool runWithBackgroundThread)
        {
            this.Initialize(module, DefaultNumberOfThreads, runWithBackgroundThread, null);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="module">The module to control.</param>
        /// <param name="numberOfThreads">The number of worker threads.</param>
        public void Initialize(object module, int numberOfThreads)
        {
            this.Initialize(module, numberOfThreads, false, null);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="module">The module to control.</param>
        public void Initialize(object module)
        {
           this.Initialize(module, DefaultNumberOfThreads, false, null);
        }

        /// <summary>
        /// Adds an extension to this controller.
        /// </summary>
        /// <typeparam name="TExtension">The type of the extension.</typeparam>
        /// <param name="extension">The extension.</param>
        public void AddExtension<TExtension>(TExtension extension) where TExtension : IModuleExtension
        {
            extension.ModuleController = this;
            this.extensions.Add<TExtension>(extension);

            this.LogExtension.AddedExtension(extension, this.controlledModule);
        }

        /// <summary>
        /// Gets the extension with the specified type.
        /// </summary>
        /// <typeparam name="TExtension">The type of the extension.</typeparam>
        /// <returns>The extension with the specified type.</returns>
        public TExtension GetExtension<TExtension>()
        {
            return this.extensions.Get<TExtension>();
        }

        /// <summary>
        /// Puts a message into the message queue of the module managed
        /// by this controller.
        /// </summary>
        /// <param name="message">
        /// The message to be enqueued.
        /// </param>
        public void EnqueueMessage(object message)
        {
            this.EnqueueMessage(message, false);
        }

        /// <summary>
        /// Puts a message to the front of the message queue of the module managed
        /// by this controller.
        /// </summary>
        /// <param name="message">
        /// The message to be enqueued.
        /// </param>
        public void EnqueuePriorityMessage(object message)
        {
            this.EnqueueMessage(message, true);
        }

        /// <summary>
        /// Clears all messages still in the queue and returns them.
        /// </summary>
        /// <returns>
        /// All messages that were cleared from the queue.
        /// </returns>
        public object[] ClearMessages()
        {
            lock (this.messageQueue)
            {
                object[] messages = this.messageQueue.ToArray();
                this.messageQueue.Clear();
                return messages;
            }
        }

        /// <summary>
        /// Starts the message consumer thread.
        /// </summary>
        public void Start()
        {
            if (this.controlledModule == null)
            {
                throw new InvalidOperationException("Module controller is not initialized.");
            }

            // Only starts the module if it has not been started yet.
            if (this.IsAlive)
            {
                this.LogExtension.ControllerAlreadyRunning(this.controlledModule);
                return;
            }

            this.LogExtension.Starting(this.controlledModule);
            
            this.OnBeforeModuleStart();

            // start worker threads
            for (int i = 0; i < this.numberOfThreads; i++)
            {
                Thread thread = new Thread(this.Run)
                                    {
                                        IsBackground = this.runWithBackgroundThread,
                                        Name = this.controlledModule + (this.numberOfThreads > 1 ? i.ToString() : string.Empty)
                                    };
                this.messageConsumerThreads.Add(thread);
                thread.Start();
            }

            this.OnAfterModuleStart();

            this.LogExtension.Started(this.controlledModule, this.numberOfThreads);
        }

        /// <summary>
        /// Stops the message consumer thread. If a message is currently processed, this is finished first.
        /// If the thread does not response in a given time, it is aborted.
        /// If the thread is not started then nothing happens but a log entry.
        /// </summary>
        public void StopAsync()
        {
            this.LogExtension.StoppingAsync(this.controlledModule);
            
            if (!this.IsAlive)
            {
                this.LogExtension.AlreadyStopped(this.controlledModule);
                
                return;
            }

            this.OnBeforeModuleStop();

            Thread stopThread = new Thread(this.WaitUntilAllThreadHaveStopped);
            stopThread.Start();

            lock (this.lockingObject)
            {
                this.ThreadStopping = true;
                Monitor.PulseAll(this.lockingObject);
            }
        }

        /// <summary>
        /// Stops the message consumer thread(s) with default timeout.
        /// This method must not be called on the worker thread of the module.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when this method is called from the worker thread itself. This would lead to a <see cref="ThreadAbortException"/>.
        /// </exception>
        public void Stop()
        {
            this.Stop(DefaultTimeOut);
        }

        /// <summary>
        /// Stops the message consumer thread(s) after finishing
        /// an message currently processed. If the thread is
        /// not responding anymore it is killed.
        /// This method must not be called on the worker thread of the module.
        /// </summary>
        /// <param name="timeout">The timeout that defines how long we will wait until the worker threads are killed, i.e. how long a currently
        /// processing message is tolerated.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when this method is called from the worker thread itself. This would lead to a <see cref="ThreadAbortException"/>.
        /// </exception>
        public void Stop(TimeSpan timeout)
        {
            this.LogExtension.Stopping(this.controlledModule, timeout);
            
            if (!this.IsAlive)
            {
                this.LogExtension.AlreadyStopped(this.controlledModule);

                return;
            }

            if (this.IsCurrentThreadAWorkerThread())
            {
                throw new InvalidOperationException("Async controller can not be stopped on its own worker thread. Either call Stop from a different thread or use a StopMessage.");
            }

            this.OnBeforeModuleStop();

            lock (this.lockingObject)
            {
                this.ThreadStopping = true;
            }

            Thread stopThread = new Thread(this.WaitUntilAllThreadHaveStopped);
            stopThread.Start();

            lock (this.lockingObject)
            {
                Monitor.PulseAll(this.lockingObject);
            }

            stopThread.Join(timeout);

            var runningThreads = this.messageConsumerThreads.Where(thread => thread.ThreadState == ThreadState.Running);
            foreach (Thread thread in runningThreads)
            {
                this.LogExtension.AbortingThread(this.controlledModule, thread.Name, timeout);
                
                thread.Abort();
            }
            
            this.NotifyStopped();
        }

        /// <summary>
        /// Checks the module for MessageConsumer attributes with reflection.
        /// The corresponding methods are added to the list of message consumer
        /// methods. If there are no valid MessageConsumer attributes, an exception is thrown.
        /// </summary>
        /// <param name="module">
        /// The module to inspect.
        /// </param>
        /// <returns>
        /// A list of methods which are marked with the MessageConsumer attribute.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if no message handle method is found.</exception>
        private static List<MethodInfo> GetConsumeMessageMethodInfos(object module)
        {
            List<MethodInfo> methodInfos = new List<MethodInfo>();

            foreach (MethodInfo methodInfo in module.GetType().GetMethods())
            {
                if (Attribute.IsDefined(methodInfo, typeof(MessageConsumerAttribute), true))
                {
                    if (methodInfo.GetParameters().Length != 1)
                    {
                        throw new ArgumentException(
                            string.Format(
                                "Module has message handler method with wrong signature. Expected signature is void <MethodName>(<MessageType> message). Method: {0}.{1}",
                                methodInfo.DeclaringType.FullName,
                                methodInfo.Name));
                    }

                    methodInfos.Add(methodInfo);
                }
            }

            if (methodInfos.Count == 0)
            {
                throw new ArgumentException("Module has no message consumer. Mark method with [MessageConsumer].", "module");
            }

            return methodInfos;
        }

        /// <summary>
        /// Enqueues the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="priority">if set to <c>true</c> then the message is enqueued at the front; otherwise at the end of the queue.</param>
        private void EnqueueMessage(object message, bool priority)
        {
            lock (this.lockingObject)
            {
                bool cancel;
                this.OnBeforeEnqueueMessage(message, out cancel);
                if (cancel)
                {
                    return;
                }

                if (priority)
                {
                    this.messageQueue.AddFirst(message);
                }
                else
                {
                    this.messageQueue.AddLast(message);
                }

                this.OnAfterEnqueueMessage(message);

                Monitor.PulseAll(this.lockingObject);
            }

            this.LogExtension.EnqueuedMessage(this.controlledModule, message);
        }

        /// <summary>
        /// Fires the <see cref="BeforeModuleStart"/> event.
        /// </summary>
        private void OnBeforeModuleStart()
        {
            if (this.BeforeModuleStart != null)
            {
                this.BeforeModuleStart(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Fires the <see cref="AfterModuleStart"/> event.
        /// </summary>
        private void OnAfterModuleStart()
        {
            if (this.AfterModuleStart != null)
            {
                this.AfterModuleStart(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Fires the <see cref="BeforeModuleStop"/> event.
        /// </summary>
        private void OnBeforeModuleStop()
        {
            if (this.BeforeModuleStop != null)
            {
                this.BeforeModuleStop(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Fires the <see cref="AfterModuleStop"/> event.
        /// </summary>
        private void OnAfterModuleStop()
        {
            if (this.AfterModuleStop != null)
            {
                this.AfterModuleStop(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Fires the <see cref="BeforeConsumeMessage"/> event.
        /// </summary>
        /// <param name="message">The message that will be consumed.</param>
        /// <returns>False if the handler canceled to consume of the message.</returns>
        private bool OnBeforeConsumeMessage(object message)
        {
            if (this.BeforeConsumeMessage == null)
            {
                return true;
            }

            BeforeConsumeMessageEventArgs e = new BeforeConsumeMessageEventArgs(this.controlledModule, message);
            this.BeforeConsumeMessage(this, e);

            return !e.Cancel;
        }

        /// <summary>
        /// Fires the <see cref="AfterConsumeMessage"/> event.
        /// </summary>
        /// <param name="message">The message that was consumed.</param>
        /// <param name="notSkipped">True if message was not skipped (<see cref="BeforeConsumeMessageEventArgs.Cancel"/> was false.), otherwise false (message not not passed to handler).</param>
        private void OnAfterConsumeMessage(object message, bool notSkipped)
        {
            if (this.AfterConsumeMessage != null)
            {
                this.AfterConsumeMessage(this, new AfterConsumeMessageEventArgs(this.controlledModule, message, notSkipped));
            }
        }

        /// <summary>
        /// Fires the <see cref="BeforeEnqueueMessage"/> event.
        /// </summary>
        /// <param name="message">The message that wants to be enqueued.</param>
        /// <param name="cancel"><c>true</c> to cancel enqueueing of message (message will not be enqueued).</param>
        private void OnBeforeEnqueueMessage(object message, out bool cancel)
        {
            if (this.BeforeEnqueueMessage != null)
            {
                EnqueueMessageEventArgs eventArgs = new EnqueueMessageEventArgs(this.controlledModule, message);
                this.BeforeEnqueueMessage(this, eventArgs);
                cancel = eventArgs.Cancel;
            }
            else
            {
                cancel = false; // do not cancel if there is no listener.
            }
        }

        /// <summary>
        /// Fires the <see cref="AfterEnqueueMessage"/> event.
        /// </summary>
        /// <param name="message">The message that was enqueued.</param>
        private void OnAfterEnqueueMessage(object message)
        {
            if (this.AfterEnqueueMessage != null)
            {
                this.AfterEnqueueMessage(this, new EnqueueMessageEventArgs(this.controlledModule, message));
            }
        }

        /// <summary>
        /// Fires the <see cref="ConsumeMessageExceptionOccurred"/> event.
        /// </summary>
        /// <param name="message">The message that caused the exception.</param>
        /// <param name="exception">The exception that was thrown while consuming the message.</param>
        /// <returns>Whether the exception was handled.</returns>
        private bool OnConsumeMessageExceptionOccurred(object message, Exception exception)
        {
            bool exceptionHandled = false;
            if (this.ConsumeMessageExceptionOccurred != null)
            {
                ConsumeMessageExceptionEventArgs eventArgs = new ConsumeMessageExceptionEventArgs(this.controlledModule, message, exception);
                this.ConsumeMessageExceptionOccurred(this, eventArgs);
                exceptionHandled = eventArgs.ExceptionHandled;
            }

            return !exceptionHandled;
        }

        /// <summary>
        /// Fires the <see cref="UnhandledModuleExceptionOccured"/> event.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        private void OnUnhandledModuleExceptionOccured(object message, Exception exception)
        {
            this.LogExtension.UnhandledException(this.controlledModule, message, exception);
            
            if (this.UnhandledModuleExceptionOccured != null)
            {
                this.UnhandledModuleExceptionOccured(this, new UnhandledModuleExceptionEventArgs(this.controlledModule, message, exception));
            }
        }

        /// <summary>
        /// Determines whether the current thread is a worker thread of this module controller.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the current thread is a worker thread of this module controller; otherwise, <c>false</c>.
        /// </returns>
        private bool IsCurrentThreadAWorkerThread()
        {
            foreach (Thread thread in this.messageConsumerThreads)
            {
                if (Thread.CurrentThread == thread)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Waits until all worker thread have stopped.
        /// </summary>
        private void WaitUntilAllThreadHaveStopped()
        {
            foreach (Thread thread in this.messageConsumerThreads)
            {
                if (thread.IsAlive)
                {
                    thread.Join();
                }
            }

            this.NotifyStopped();
        }

        /// <summary>
        /// Notifies the module controller that all worker threads are stopped.
        /// </summary>
        private void NotifyStopped()
        {
            lock (this.lockingObject)
            {
                if (!this.ThreadStopping)
                {
                    // Already stopped (probably due to time-out)
                    return;
                }

                this.ThreadStopping = false;
                Monitor.PulseAll(this.lockingObject);
            }

            this.messageConsumerThreads.Clear();
            this.OnAfterModuleStop();

            this.LogExtension.Stopped(this.controlledModule);
        }

        /// <summary>
        /// The main method of the message consumer thread.
        /// </summary>
        /// <param name="o">The exit signal that </param>
        private void Run(object o)
        {
            // Wait for new messages or the exit thread event.
            while (!this.ThreadStopping)
            {
                try
                {
                    object message;
                    lock (this.lockingObject)
                    {
                        this.LogExtension.NumberOfMessagesInQueue(this.messageQueue.Count, this.controlledModule);
                        
                        while (this.messageQueue.Count == 0 && !this.ThreadStopping)
                        {
                            Monitor.Wait(this.lockingObject);
                        }

                        if (this.ThreadStopping)
                        {
                            continue;
                        }

                        if (this.messageQueue.Count > 0)
                        {
                            message = this.messageQueue.First();
                            this.messageQueue.RemoveFirst();
                        }
                        else
                        {
                            continue;
                        }
                    }

                    try
                    {
                        // Call the configured consumer method.
                        this.ConsumeMessage(message);
                    }
                    catch (ThreadAbortException)
                    {
                        // ThreadAbort Exception has to be handled in outer catch otherwise two events are risen.
                        throw;
                    }
                    catch (Exception e)
                    {
                        // when an message consumption throws an error then continue with the next and report error
                        this.OnUnhandledModuleExceptionOccured(message, e);
                    }
                }
                catch (Exception e)
                {
                    // something with the queue is propably wrong.
                    this.OnUnhandledModuleExceptionOccured(null, e);
                }
            }

            this.LogExtension.WorkerThreadExit(Thread.CurrentThread.Name);
        }

        /// <summary>
        /// Calls the method marked with the MessageConsumer attribute, which match 
        /// the type of the message.
        /// </summary>
        /// <param name="message">
        /// Message to be consumed.
        /// </param>
        private void ConsumeMessage(object message)
        {
            if (message == null)
            {
                this.LogExtension.SkippingNullMessage(this.controlledModule);
                
                return;
            }

            this.LogExtension.ConsumingMessage(message, this.controlledModule);
            
            bool foundHandler = false;
            foreach (MethodInfo methodInfo in this.consumeMessageMethodInfos)
            {
                if (methodInfo.GetParameters()[0].ParameterType.IsAssignableFrom(message.GetType()))
                {
                    foundHandler = true;

                    this.LogExtension.RelayingMessage(message, this.controlledModule, methodInfo.Name);
                    
                    try
                    {
                        bool donotskip = this.OnBeforeConsumeMessage(message);
                        if (donotskip)
                        {
                            methodInfo.Invoke(this.controlledModule, new[] { message });

                            this.LogExtension.ConsumedMessage(message, this.controlledModule);
                        }
                        else
                        {
                            this.LogExtension.SkippedMessage(message, this.controlledModule);
                        }

                        this.OnAfterConsumeMessage(message, donotskip);
                    }
                    catch (TargetInvocationException e)
                    {
                        bool rethrowException = this.OnConsumeMessageExceptionOccurred(message, e.InnerException);
                        if (rethrowException)
                        {
                            throw;
                        }

                        this.LogExtension.SwallowedException(e, message, this.controlledModule);
                    }
                }
            }

            if (!foundHandler)
            {
                this.LogExtension.NoHandlerFound(message, this.controlledModule);
            }
        }
    }
}
