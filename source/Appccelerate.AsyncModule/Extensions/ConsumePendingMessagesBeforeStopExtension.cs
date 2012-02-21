//-------------------------------------------------------------------------------
// <copyright file="ConsumePendingMessagesBeforeStopExtension.cs" company="Appccelerate">
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

namespace Appccelerate.AsyncModule.Extensions
{
    using System;
    using Events;

    /// <summary>
    /// This extension consums all pending messages before stopping the 
    /// module controller.
    /// </summary>
    public class ConsumePendingMessagesBeforeStopExtension : IModuleExtension
    {
        /// <summary>
        /// Time to wait (ms) for pending messages before the module controller is stopped.
        /// </summary>
        private readonly int pendingMessagesWaitTime;

        /// <summary>
        /// In this interval it is checked if there are pending messages.
        /// </summary>
        private readonly int pendingMessagesCheckInterval;

        /// <summary>
        /// True if the module controller of this extension is stopping.
        /// </summary>
        private bool controllerIsStopping;

        /// <summary>
        /// The module controller the extension belongs to. This 
        /// field is set through the property ModuleController with 
        /// dependency injection.
        /// </summary>
        private IModuleController moduleController;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumePendingMessagesBeforeStopExtension"/> class.
        /// </summary>
        /// <param name="pendingMessagesWaitTime">Time to wait (ms) for pending messages before the module controller is stopped.</param>
        /// <param name="pendingMessagesCheckInterval">In this interval it is checked if there are pending messages.</param>
        public ConsumePendingMessagesBeforeStopExtension(int pendingMessagesWaitTime, int pendingMessagesCheckInterval)
        {
            this.pendingMessagesWaitTime = pendingMessagesWaitTime;
            this.pendingMessagesCheckInterval = pendingMessagesCheckInterval;
        }

        /// <summary>
        /// Sets the module controller this extension belongs to.
        /// </summary>
        public IModuleController ModuleController
        {
            set { this.moduleController = value; }
        }

        /// <summary>
        /// Attaches the extension to the module controller.
        /// </summary>
        public void Attach()
        {
            this.moduleController.BeforeEnqueueMessage += this.OnBeforeEnqueueMessage;
            this.moduleController.BeforeModuleStop += this.OnBeforeModuleStop;
            this.moduleController.AfterModuleStop += this.OnAfterModuleStop;
        }

        /// <summary>
        /// Removes the connection to the module controller.
        /// </summary>
        public void Detach()
        {
            this.moduleController.BeforeEnqueueMessage -= this.OnBeforeEnqueueMessage;
            this.moduleController.BeforeModuleStop -= this.OnBeforeModuleStop;
            this.moduleController.AfterModuleStop -= this.OnAfterModuleStop;
        }

        /// <summary>
        /// If the module controller is stopping and messages are still enqueue, we ignore them.
        /// </summary>
        /// <param name="sender">Sender is ignored.</param>
        /// <param name="e">Used to prevent the message from being enqueued.</param>
        private void OnBeforeEnqueueMessage(object sender, EnqueueMessageEventArgs e)
        {
            if (this.controllerIsStopping)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Waits until all messages are consumed or until the PendingMessageWaitTime has passed.
        /// </summary>
        /// <param name="sender">Sender is ignored.</param>
        /// <param name="e">Event args are ignored.</param>
        private void OnBeforeModuleStop(object sender, EventArgs e)
        {
            this.controllerIsStopping = true;
            DateTime stopDateTime = DateTime.Now.AddMilliseconds(this.pendingMessagesWaitTime);

            while ((DateTime.Now < stopDateTime) && (this.moduleController.MessageCount > 0))
            {
                System.Threading.Thread.Sleep(this.pendingMessagesCheckInterval);
            }
        }

        /// <summary>
        /// Resets the flag, which indicates that the module controller is stopping.
        /// </summary>
        /// <param name="sender">Sender is ignored.</param>
        /// <param name="e">Event args are ignored.</param>
        private void OnAfterModuleStop(object sender, EventArgs e)
        {
            this.controllerIsStopping = false;
        }
    }
}
