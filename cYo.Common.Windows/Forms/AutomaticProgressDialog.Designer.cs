using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Threading;

namespace cYo.Common.Windows.Forms
{
	public partial class AutomaticProgressDialog
	{
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				finishEvent.Close();
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelCaption = new System.Windows.Forms.Label();
            this.threadCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.flowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.btCancel = new System.Windows.Forms.Button();
            this.flowLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(0, 29);
            this.progressBar.Margin = new System.Windows.Forms.Padding(0, 16, 0, 0);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(306, 24);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 0;
            // 
            // labelCaption
            // 
            this.labelCaption.AutoSize = true;
            this.labelCaption.Location = new System.Drawing.Point(3, 0);
            this.labelCaption.MinimumSize = new System.Drawing.Size(300, 0);
            this.labelCaption.Name = "labelCaption";
            this.labelCaption.Size = new System.Drawing.Size(300, 13);
            this.labelCaption.TabIndex = 0;
            this.labelCaption.Text = "This is the description of the action";
            this.labelCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // threadCheckTimer
            // 
            this.threadCheckTimer.Enabled = true;
            this.threadCheckTimer.Interval = 500;
            this.threadCheckTimer.Tick += new System.EventHandler(this.threadCheckTimer_Tick);
            // 
            // flowLayout
            // 
            this.flowLayout.AutoSize = true;
            this.flowLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayout.Controls.Add(this.labelCaption);
            this.flowLayout.Controls.Add(this.progressBar);
            this.flowLayout.Controls.Add(this.btCancel);
            this.flowLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayout.Location = new System.Drawing.Point(8, 9);
            this.flowLayout.Margin = new System.Windows.Forms.Padding(8);
            this.flowLayout.Name = "flowLayout";
            this.flowLayout.Size = new System.Drawing.Size(306, 85);
            this.flowLayout.TabIndex = 1;
            // 
            // btCancel
            // 
            this.btCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btCancel.Location = new System.Drawing.Point(226, 61);
            this.btCancel.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(80, 24);
            this.btCancel.TabIndex = 7;
            this.btCancel.Text = "&Cancel";
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // AutomaticProgressDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(322, 102);
            this.ControlBox = false;
            this.Controls.Add(this.flowLayout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AutomaticProgressDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AutomaticProgressDialog";
            this.flowLayout.ResumeLayout(false);
            this.flowLayout.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		private ProgressBar progressBar;
		private Label labelCaption;
		private System.Windows.Forms.Timer threadCheckTimer;
		private FlowLayoutPanel flowLayout;
		private Button btCancel;
	}
}
