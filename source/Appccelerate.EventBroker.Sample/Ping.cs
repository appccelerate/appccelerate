// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Ping.cs" company="Appccelerate">
//   Copyright (c) 2008-2013   Licensed under the Apache License, Version 2.0 (the "License");   you may not use this file except in compliance with the License.   You may obtain a copy of the License at       http://www.apache.org/licenses/LICENSE-2.0   Unless required by applicable law or agreed to in writing, software   distributed under the License is distributed on an "AS IS" BASIS,   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.   See the License for the specific language governing permissions and   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Appccelerate.EventBroker.Sample
{
    using System;
    using System.Windows.Forms;

    using Appccelerate.Events;

    /// <summary>
    /// The ping form.
    /// </summary>
    public partial class Ping : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ping"/> class.
        /// </summary>
        public Ping()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Occurs when the ping UI from UI thread is fired.
        /// </summary>
        [EventPublication(EventTopics.PingUIFromUIThread)]
        public event EventHandler UiFromUiEvent;

        /// <summary>
        /// Occurs when the ping UI from asynchronous is fired.
        /// </summary>
        [EventPublication(EventTopics.PingUIFromAsync)]
        public event EventHandler UiFromAsyncEvent;

        [EventPublication(EventTopics.BurstPing, HandlerRestriction.Asynchronous)]
        public event EventHandler<EventArgs<int>> BurstEvent;

        /// <summary>
        /// Handles the pong UI from UI thread event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments</param>
        [EventSubscription(EventTopics.PongUIFromUIThread, typeof(Handlers.OnUserInterface))]
        public void HandleUiFromUi(object sender, EventArgs e)
        {
            this.FeedbackLabel.Text = "UI from UI thread called.";
        }

        /// <summary>
        /// Handles the pong UI from asynchronous event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        [EventSubscription(EventTopics.PongUIFromAsync, typeof(Handlers.OnUserInterfaceAsync))]
        public void HandleUiFromAsync(object sender, EventArgs e)
        {
            this.FeedbackLabel.Text = "UI from asynchronous.";
        }

        [EventSubscription(EventTopics.BurstPong, typeof(Handlers.OnUserInterfaceAsync))]
        public void HandleBurst(int index)
        {
            this.BurstText.Text += " " + index;
        }

        /// <summary>
        /// Handles the load event.
        /// </summary>
        /// <param name="e">The load event argument.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.timer.Enabled = true;
        }

        private void CallUIFromUIButton_Click(object sender, EventArgs e)
        {
            this.UiFromUiEvent(this, EventArgs.Empty);
        }

        private void UIFromAsyncButton_Click(object sender, EventArgs e)
        {
            this.UiFromAsyncEvent(this, EventArgs.Empty);
        }

        private void BurstButton_Click(object sender, EventArgs e)
        {
            this.BurstText.Text = string.Empty;

            for (int i = 0; i < 50; i++)
            {
                this.BurstEvent(this, new EventArgs<int>(i));
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.Time.Text = DateTime.Now.ToString("hh:mm:ss.fff");
        }
    }
}