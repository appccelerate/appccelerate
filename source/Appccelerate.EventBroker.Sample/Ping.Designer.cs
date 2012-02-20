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
            this.SuspendLayout();
            // 
            // CallUIFromUIButton
            // 
            this.CallUIFromUIButton.Location = new System.Drawing.Point(12, 12);
            this.CallUIFromUIButton.Name = "CallUIFromUIButton";
            this.CallUIFromUIButton.Size = new System.Drawing.Size(203, 23);
            this.CallUIFromUIButton.TabIndex = 0;
            this.CallUIFromUIButton.Text = "Call  UI thread from UI thread";
            this.CallUIFromUIButton.UseVisualStyleBackColor = true;
            this.CallUIFromUIButton.Click += new System.EventHandler(this.CallUIFromUIButton_Click);
            // 
            // FeedbackLabel
            // 
            this.FeedbackLabel.AutoSize = true;
            this.FeedbackLabel.Location = new System.Drawing.Point(13, 335);
            this.FeedbackLabel.Name = "FeedbackLabel";
            this.FeedbackLabel.Size = new System.Drawing.Size(16, 13);
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
            this.Time.Location = new System.Drawing.Point(12, 319);
            this.Time.Name = "Time";
            this.Time.Size = new System.Drawing.Size(35, 13);
            this.Time.TabIndex = 2;
            this.Time.Text = "label1";
            // 
            // UIFromAsyncButton
            // 
            this.UIFromAsyncButton.Location = new System.Drawing.Point(12, 41);
            this.UIFromAsyncButton.Name = "UIFromAsyncButton";
            this.UIFromAsyncButton.Size = new System.Drawing.Size(203, 23);
            this.UIFromAsyncButton.TabIndex = 3;
            this.UIFromAsyncButton.Text = "Call  UI thread from async thread";
            this.UIFromAsyncButton.UseVisualStyleBackColor = true;
            this.UIFromAsyncButton.Click += new System.EventHandler(this.UIFromAsyncButton_Click);
            // 
            // Ping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 360);
            this.Controls.Add(this.UIFromAsyncButton);
            this.Controls.Add(this.Time);
            this.Controls.Add(this.FeedbackLabel);
            this.Controls.Add(this.CallUIFromUIButton);
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
    }
}

