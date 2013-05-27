//-------------------------------------------------------------------------------
// <copyright file="AsyncModuleLogExtension.cs" company="Appccelerate">
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

namespace Appccelerate.SourceTemplates.Log4Net
{
    using System;
    using System.Reflection;

    using Appccelerate.AsyncModule;
    using Appccelerate.AsyncModule.Extensions;

    using log4net;

    /// <summary>
    /// log4net logging for the <see cref="ModuleController"/>.
    /// </summary>
    public class AsyncModuleLogExtension : IAsyncModuleLogExtension
    {
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncModuleLogExtension"/> class.
        /// </summary>
        public AsyncModuleLogExtension()
        {
            this.log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.FullName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncModuleLogExtension"/> class.
        /// </summary>
        /// <param name="logger">The logger name.</param>
        public AsyncModuleLogExtension(string logger)
        {
            this.log = LogManager.GetLogger(logger);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncModuleLogExtension"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public AsyncModuleLogExtension(ILog logger)
        {
            this.log = logger;
        }

        /// <summary>
        /// Called when an extension was added.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <param name="controlledModule">The controlled module.</param>
        public void AddedExtension(IModuleExtension extension, object controlledModule)
        {
            this.log.DebugFormat("Added extension {0} to module {1}.", extension, controlledModule);
        }

        /// <summary>
        /// Called when the controller Controllers the already running.
        /// </summary>
        /// <param name="controlledModule">The controlled module.</param>
        public void ControllerAlreadyRunning(object controlledModule)
        {
            this.log.DebugFormat("Asynchronous controller of module {0} is already started.", controlledModule);
        }

        /// <summary>
        /// Called when the controller is starting.
        /// </summary>
        /// <param name="controlledModule">The controlled module.</param>
        public void Starting(object controlledModule)
        {
            this.log.DebugFormat("Starting asynchronous controller of module {0}.", controlledModule);
        }

        /// <summary>
        /// Called when the controller is started.
        /// </summary>
        /// <param name="controlledModule">The controlled module.</param>
        /// <param name="numberOfThreads">The number of threads.</param>
        public void Started(object controlledModule, int numberOfThreads)
        {
            this.log.DebugFormat("Started asynchronous controller of module {0} with {1} worker thread(s).", controlledModule, numberOfThreads);
        }

        /// <summary>
        /// Called when controller is stopped asynchronously.
        /// </summary>
        /// <param name="controlledModule">The controlled module.</param>
        public void StoppingAsync(object controlledModule)
        {
            this.log.DebugFormat("Stopping asynchronous controller of module {0}.", controlledModule);
        }

        /// <summary>
        /// Called when controller is stopped but it is already stopped.
        /// </summary>
        /// <param name="controlledModule">The controlled module.</param>
        public void AlreadyStopped(object controlledModule)
        {
            this.log.DebugFormat("Asynchronous controller of module {0} is already stopped.", controlledModule);
        }

        /// <summary>
        /// Called when control is stopping.
        /// </summary>
        /// <param name="controlledModule">The controlled module.</param>
        /// <param name="timeout">The timeout.</param>
        public void Stopping(object controlledModule, TimeSpan timeout)
        {
            this.log.DebugFormat("Stopping asynchronous controller of module {0} immediately with timeout {1}.", controlledModule, timeout);
        }

        /// <summary>
        /// Called when a thread is aborted (did not finish in timeout).
        /// </summary>
        /// <param name="controlledModule">The controlled module.</param>
        /// <param name="threadName">Name of the thread.</param>
        /// <param name="timeout">The timeout.</param>
        public void AbortingThread(object controlledModule, string threadName, TimeSpan timeout)
        {
            this.log.InfoFormat("Aborting thread {0} because it did not terminate within the given time-out ({1} milliseconds)", threadName, timeout);
        }

        /// <summary>
        /// Called when a message is queued.
        /// </summary>
        /// <param name="controlledModule">The controlled module.</param>
        /// <param name="message">The message.</param>
        public void EnqueuedMessage(object controlledModule, object message)
        {
            this.log.DebugFormat("Enqueued message {0}", message);
        }

        /// <summary>
        /// Called when an unhandled exception occurs.
        /// </summary>
        /// <param name="controlledModule">The controlled module.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void UnhandledException(object controlledModule, object message, Exception exception)
        {
            this.log.ErrorFormat("Unhandled exception in module {0}: {1} {2}", controlledModule, message, exception);
        }

        /// <summary>
        /// Called when controller was stopped.
        /// </summary>
        /// <param name="controlledModule">The controlled module.</param>
        public void Stopped(object controlledModule)
        {
            this.log.DebugFormat("Stopped asynchronous controller of module {0}.", controlledModule);
        }

        /// <summary>
        /// Called to report number of remaining messages in queue.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="controlledModule">The controlled module.</param>
        public void NumberOfMessagesInQueue(int count, object controlledModule)
        {
            this.log.DebugFormat("{0} messages in queue of module {1}.", count, controlledModule);
        }

        /// <summary>
        /// Called when a worker thread exits.
        /// </summary>
        /// <param name="threadName">Name of the thread.</param>
        public void WorkerThreadExit(string threadName)
        {
            this.log.DebugFormat("Worker thread '{0}' exited.", threadName);
        }

        /// <summary>
        /// Called when skipping a null message.
        /// </summary>
        /// <param name="controlledModule">The controlled module.</param>
        public void SkippingNullMessage(object controlledModule)
        {
            this.log.DebugFormat("Skipping null message of module {0}.", controlledModule);
        }

        /// <summary>
        /// Called when consuming a message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="controlledModule">The controlled module.</param>
        public void ConsumingMessage(object message, object controlledModule)
        {
            this.log.DebugFormat("Consuming message {0} of module {1}.", message, controlledModule);
        }

        /// <summary>
        /// Called when a message is relayed to a consumer.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="controlledModule">The controlled module.</param>
        /// <param name="methodName">Name of the method.</param>
        public void RelayingMessage(object message, object controlledModule, string methodName)
        {
            this.log.DebugFormat("Relaying message {0} of module {1} to method {2}.", message, controlledModule, methodName);
        }

        /// <summary>
        /// Called when a message was consumed.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="controlledModule">The controlled module.</param>
        public void ConsumedMessage(object message, object controlledModule)
        {
            this.log.DebugFormat("Consumed message {0} of module {1}.", message, controlledModule);
        }

        /// <summary>
        /// Called when a message was skipped (due to an extension).
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="controlledModule">The controlled module.</param>
        public void SkippedMessage(object message, object controlledModule)
        {
            this.log.DebugFormat("Skipped message {0} of module {1}", message, controlledModule);
        }

        /// <summary>
        /// Called when an exception was swallowed.
        /// </summary>
        /// <param name="targetInvocationException">The target invocation exception.</param>
        /// <param name="message">The message.</param>
        /// <param name="controlledModule">The controlled module.</param>
        public void SwallowedException(TargetInvocationException targetInvocationException, object message, object controlledModule)
        {
            this.log.DebugFormat("Swallowing exception {0} that occurred consuming message {1} of module {2}.", targetInvocationException, message, controlledModule);
        }

        /// <summary>
        /// Called when no handler method was found for a message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="controlledModule">The controlled module.</param>
        public void NoHandlerFound(object message, object controlledModule)
        {
            this.log.DebugFormat("No handler method found for message {0} on module {1}.", message, controlledModule);
        }
    }
}