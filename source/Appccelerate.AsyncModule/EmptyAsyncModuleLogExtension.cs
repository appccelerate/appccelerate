//-------------------------------------------------------------------------------
// <copyright file="EmptyAsyncModuleLogExtension.cs" company="Appccelerate">
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
    using System.Reflection;

    using Appccelerate.AsyncModule.Extensions;

    /// <summary>
    /// Log extension that does nothing. Can be used as a base class for custom loggers.
    /// </summary>
    public class EmptyAsyncModuleLogExtension : IAsyncModuleLogExtension
    {
        /// <summary>
        /// Called when an extension was added.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <param name="controlledModule">The controlled module.</param>
        public virtual void AddedExtension(IModuleExtension extension, object controlledModule)
        {
        }

        /// <summary>
        /// Called when the controller Controllers the already running.
        /// </summary>
        /// <param name="controlledModule">The controlled module.</param>
        public virtual void ControllerAlreadyRunning(object controlledModule)
        {
        }

        /// <summary>
        /// Called when the controller is starting.
        /// </summary>
        /// <param name="controlledModule">The controlled module.</param>
        public virtual void Starting(object controlledModule)
        {
        }

        /// <summary>
        /// Called when the controller is started.
        /// </summary>
        /// <param name="controlledModule">The controlled module.</param>
        /// <param name="numberOfThreads">The number of threads.</param>
        public virtual void Started(object controlledModule, int numberOfThreads)
        {
        }

        /// <summary>
        /// Called when controller is stopped asynchronously.
        /// </summary>
        /// <param name="controlledModule">The controlled module.</param>
        public virtual void StoppingAsync(object controlledModule)
        {
        }

        /// <summary>
        /// Called when controller is stopped but it is already stopped.
        /// </summary>
        /// <param name="controlledModule">The controlled module.</param>
        public virtual void AlreadyStopped(object controlledModule)
        {
        }

        /// <summary>
        /// Called when control is stopping.
        /// </summary>
        /// <param name="controlledModule">The controlled module.</param>
        /// <param name="timeout">The timeout.</param>
        public virtual void Stopping(object controlledModule, TimeSpan timeout)
        {
        }

        /// <summary>
        /// Called when a thread is aborted (did not finish in timeout).
        /// </summary>
        /// <param name="controlledModule">The controlled module.</param>
        /// <param name="threadName">Name of the thread.</param>
        /// <param name="timeout">The timeout.</param>
        public virtual void AbortingThread(object controlledModule, string threadName, TimeSpan timeout)
        {
        }

        /// <summary>
        /// Called when a message is queued.
        /// </summary>
        /// <param name="controlledModule">The controlled module.</param>
        /// <param name="message">The message.</param>
        public virtual void EnqueuedMessage(object controlledModule, object message)
        {
        }

        /// <summary>
        /// Called when an unhandled exception occurs.
        /// </summary>
        /// <param name="controlledModule">The controlled module.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public virtual void UnhandledException(object controlledModule, object message, Exception exception)
        {
        }

        /// <summary>
        /// Called when controller was stopped.
        /// </summary>
        /// <param name="controlledModule">The controlled module.</param>
        public virtual void Stopped(object controlledModule)
        {
        }

        /// <summary>
        /// Called to report number of remaining messages in queue.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="controlledModule">The controlled module.</param>
        public virtual void NumberOfMessagesInQueue(int count, object controlledModule)
        {
        }

        /// <summary>
        /// Called when a worker thread exits.
        /// </summary>
        /// <param name="threadName">Name of the thread.</param>
        public virtual void WorkerThreadExit(string threadName)
        {
        }

        /// <summary>
        /// Called when skipping a null message.
        /// </summary>
        /// <param name="controlledModule">The controlled module.</param>
        public virtual void SkippingNullMessage(object controlledModule)
        {
        }

        /// <summary>
        /// Called when consuming a message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="controlledModule">The controlled module.</param>
        public virtual void ConsumingMessage(object message, object controlledModule)
        {
        }

        /// <summary>
        /// Called when a message is relayed to a consumer.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="controlledModule">The controlled module.</param>
        /// <param name="methodName">Name of the method.</param>
        public virtual void RelayingMessage(object message, object controlledModule, string methodName)
        {
        }

        /// <summary>
        /// Called when a message was consumed.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="controlledModule">The controlled module.</param>
        public virtual void ConsumedMessage(object message, object controlledModule)
        {
        }

        /// <summary>
        /// Called when a message was skipped (due to an extension).
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="controlledModule">The controlled module.</param>
        public virtual void SkippedMessage(object message, object controlledModule)
        {
        }

        /// <summary>
        /// Called when an exception was swallowed.
        /// </summary>
        /// <param name="targetInvocationException">The target invocation exception.</param>
        /// <param name="message">The message.</param>
        /// <param name="controlledModule">The controlled module.</param>
        public virtual void SwallowedException(TargetInvocationException targetInvocationException, object message, object controlledModule)
        {
        }

        /// <summary>
        /// Called when no handler method was found for a message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="controlledModule">The controlled module.</param>
        public virtual void NoHandlerFound(object message, object controlledModule)
        {
        }
    }
}