//-------------------------------------------------------------------------------
// <copyright file="SkipIfDuplicateInQueueModuleExtension.cs" company="Appccelerate">
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
    /// <summary>
    /// Extension that cancels the consumption of a message if there is a message in the queue that is equal to it.
    /// </summary>
    public class SkipIfDuplicateInQueueModuleExtension : IModuleExtension
    {
        /// <summary>
        /// Module controller this extension is registered to.
        /// </summary>
        private IModuleController moduleController;

        /// <summary>
        /// Sets the controller this extension belongs to.
        /// </summary>
        /// <value>The controller this extension was added to.</value>
        public IModuleController ModuleController
        {
            set
            {
                this.moduleController = value;
            }
        }

        /// <summary>
        /// Called by the module controller to allow the extension
        /// to register itself for events.
        /// </summary>
        public void Attach()
        {
            this.moduleController.BeforeConsumeMessage += this.ModuleController_BeforeConsumeMessage;
        }

        /// <summary>
        /// Called by the module controller to allow the extension
        /// to remove its event handlers.
        /// </summary>
        public void Detach()
        {
            this.moduleController.BeforeConsumeMessage -= this.ModuleController_BeforeConsumeMessage;
        }

        /// <summary>
        /// Handles the BeforeConsumeMessage event of the ModuleController.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Appccelerate.AsyncModule.Events.BeforeConsumeMessageEventArgs"/> instance containing the event data.</param>
        private void ModuleController_BeforeConsumeMessage(object sender, Events.BeforeConsumeMessageEventArgs e)
        {
            bool foundDuplicate = false;
            foreach (object message in this.moduleController.Messages)
            {
                if (message.Equals(e.Message))
                {
                    foundDuplicate = true;
                    break;
                }
            }

            e.Cancel = foundDuplicate;
        }
    }
}