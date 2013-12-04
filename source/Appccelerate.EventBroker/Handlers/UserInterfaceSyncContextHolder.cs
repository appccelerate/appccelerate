//-------------------------------------------------------------------------------
// <copyright file="UserInterfaceSyncContextHolder.cs" company="Appccelerate">
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
    using System.Reflection;
    using System.Threading;

    /// <summary>
    /// Helper class to store the synchronization context.
    /// </summary>
    internal class UserInterfaceSyncContextHolder
    {
        /// <summary>
        /// Gets the synchronization context that was acquired on registration.
        /// </summary>
        /// <value>The synchronization context that was acquired on registration.</value>
        public SynchronizationContext SyncContext { get; private set; }

        /// <summary>
        /// Gets the id of the thread this instance was initialized on.
        /// </summary>
        /// <value>The thread id.</value>
        public int ThreadId { get; private set; }

        /// <summary>
        /// Initializes this instance. If the current thread is not the user interface thread then an exception is thrown.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="handlerMethod">Handler method on the subscriber.</param>
        public void Initalize(object subscriber, MethodInfo handlerMethod)
        {
            // If there's a syncronization context (i.e. the WindowsFormsSynchronizationContext 
            // created to marshal back to the thread where a control was initially created 
            // in a particular thread), capture it to marshal back to it through the 
            // context, that basically goes through a Post/Send.
            if (SynchronizationContext.Current != null)
            {
                this.SyncContext = SynchronizationContext.Current;
            }
            else
            {
                throw new NotUserInterfaceThreadException(subscriber, handlerMethod.Name);
            }

            this.ThreadId = Thread.CurrentThread.ManagedThreadId;
        }
    }
}