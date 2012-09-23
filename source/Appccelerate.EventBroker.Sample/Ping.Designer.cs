namespace Appccelerate.EventBroker.Sample
{
    partial class Ping
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.CallUIFromUIButton = new System.Windows.Forms.Button();
            this.FeedbackLabel = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.Time = new System.Windows.Forms.Label();
            this.UIFromAsyncButton = new System.Windows.Forms.Button();
            this.BurstButton = new System.Windows.Forms.Button();
            this.BurstText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // CallUIFromUIButton
            // 
            this.CallUIFromUIButton.Location = new System.Drawing.Point(22, 22);
            this.CallUIFromUIButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.CallUIFromUIButton.Name = "CallUIFromUIButton";
            this.CallUIFromUIButton.Size = new System.Drawing.Size(372, 42);
            this.CallUIFromUIButton.TabIndex = 0;
            this.CallUIFromUIButton.Text = "Call  UI thread from UI thread";
            this.CallUIFromUIButton.UseVisualStyleBackColor = true;
            this.CallUIFromUIButton.Click += new System.EventHandler(this.CallUIFromUIButton_Click);
            // 
            // FeedbackLabel
            // 
            this.FeedbackLabel.AutoSize = true;
            this.FeedbackLabel.Location = new System.Drawing.Point(22, 346);
            this.FeedbackLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.FeedbackLabel.Name = "FeedbackLabel";
            this.FeedbackLabel.Size = new System.Drawing.Size(27, 25);
            this.FeedbackLabel.TabIndex = 1;
            this.FeedbackLabel.Text = "...";
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // Time
            // 
            this.Time.AutoSize = true;
            this.Time.Location = new System.Drawing.Point(22, 311);
            this.Time.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Time.Name = "Time";
            this.Time.Size = new System.Drawing.Size(64, 25);
            this.Time.TabIndex = 2;
            this.Time.Text = "label1";
            // 
            // UIFromAsyncButton
            // 
            this.UIFromAsyncButton.Location = new System.Drawing.Point(22, 76);
            this.UIFromAsyncButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.UIFromAsyncButton.Name = "UIFromAsyncButton";
            this.UIFromAsyncButton.Size = new System.Drawing.Size(372, 42);
            this.UIFromAsyncButton.TabIndex = 3;
            this.UIFromAsyncButton.Text = "Call  UI thread from async thread";
            this.UIFromAsyncButton.UseVisualStyleBackColor = true;
            this.UIFromAsyncButton.Click += new System.EventHandler(this.UIFromAsyncButton_Click);
            // 
            // BurstButton
            // 
            this.BurstButton.Location = new System.Drawing.Point(22, 162);
            this.BurstButton.Margin = new System.Windows.Forms.Padding(6);
            this.BurstButton.Name = "BurstButton";
            this.BurstButton.Size = new System.Drawing.Size(372, 42);
            this.BurstButton.TabIndex = 4;
            this.BurstButton.Text = "Burst";
            this.BurstButton.UseVisualStyleBackColor = true;
            this.BurstButton.Click += new System.EventHandler(this.BurstButton_Click);
            // 
            // BurstText
            // 
            this.BurstText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BurstText.Location = new System.Drawing.Point(22, 396);
            this.BurstText.Multiline = true;
            this.BurstText.Name = "BurstText";
            this.BurstText.ReadOnly = true;
            this.BurstText.Size = new System.Drawing.Size(809, 242);
            this.BurstText.TabIndex = 5;
            // 
            // Ping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 665);
            this.Controls.Add(this.BurstText);
            this.Controls.Add(this.BurstButton);
            this.Controls.Add(this.UIFromAsyncButton);
            this.Controls.Add(this.Time);
            this.Controls.Add(this.FeedbackLabel);
            this.Controls.Add(this.CallUIFromUIButton);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "Ping";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CallUIFromUIButton;
        private System.Windows.Forms.Label FeedbackLabel;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label Time;
        private System.Windows.Forms.Button UIFromAsyncButton;
        private System.Windows.Forms.Button BurstButton;
        private System.Windows.Forms.TextBox BurstText;
    }
}

