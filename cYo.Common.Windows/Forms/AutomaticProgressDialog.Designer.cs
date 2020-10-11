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
			components = new System.ComponentModel.Container();
			progressBar = new System.Windows.Forms.ProgressBar();
			labelCaption = new System.Windows.Forms.Label();
			threadCheckTimer = new System.Windows.Forms.Timer(components);
			flowLayout = new System.Windows.Forms.FlowLayoutPanel();
			btCancel = new System.Windows.Forms.Button();
			flowLayout.SuspendLayout();
			SuspendLayout();
			progressBar.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			progressBar.Location = new System.Drawing.Point(0, 29);
			progressBar.Margin = new System.Windows.Forms.Padding(0, 16, 0, 0);
			progressBar.Name = "progressBar";
			progressBar.Size = new System.Drawing.Size(306, 24);
			progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			progressBar.TabIndex = 0;
			labelCaption.AutoSize = true;
			labelCaption.Location = new System.Drawing.Point(3, 0);
			labelCaption.MinimumSize = new System.Drawing.Size(300, 0);
			labelCaption.Name = "labelCaption";
			labelCaption.Size = new System.Drawing.Size(300, 13);
			labelCaption.TabIndex = 0;
			labelCaption.Text = "This is the description of the action";
			labelCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			threadCheckTimer.Enabled = true;
			threadCheckTimer.Interval = 500;
			threadCheckTimer.Tick += new System.EventHandler(threadCheckTimer_Tick);
			flowLayout.AutoSize = true;
			flowLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			flowLayout.Controls.Add(labelCaption);
			flowLayout.Controls.Add(progressBar);
			flowLayout.Controls.Add(btCancel);
			flowLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			flowLayout.Location = new System.Drawing.Point(8, 9);
			flowLayout.Margin = new System.Windows.Forms.Padding(8);
			flowLayout.Name = "flowLayout";
			flowLayout.Size = new System.Drawing.Size(306, 85);
			flowLayout.TabIndex = 1;
			btCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(226, 61);
			btCancel.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 7;
			btCancel.Text = "&Cancel";
			btCancel.Click += new System.EventHandler(btCancel_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			AutoSize = true;
			base.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			base.ClientSize = new System.Drawing.Size(322, 102);
			base.ControlBox = false;
			base.Controls.Add(flowLayout);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AutomaticProgressDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "AutomaticProgressDialog";
			flowLayout.ResumeLayout(false);
			flowLayout.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		private readonly ManualResetEvent finishEvent = new ManualResetEvent(initialState: false);

		private IContainer components;

		private ProgressBar progressBar;

		private Label labelCaption;

		private System.Windows.Forms.Timer threadCheckTimer;

		private FlowLayoutPanel flowLayout;

		private Button btCancel;
	}
}
