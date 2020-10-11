using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class CrashDialog
	{
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}


		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			btRestart = new System.Windows.Forms.Button();
			btResume = new System.Windows.Forms.Button();
			labelMessage = new System.Windows.Forms.Label();
			chkSubmit = new System.Windows.Forms.CheckBox();
			btDetails = new System.Windows.Forms.Button();
			tbLog = new System.Windows.Forms.TextBox();
			lockTimer = new System.Windows.Forms.Timer(components);
			btExit = new System.Windows.Forms.Button();
			flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			panel1 = new System.Windows.Forms.Panel();
			flowLayoutPanel1.SuspendLayout();
			panel1.SuspendLayout();
			SuspendLayout();
			btRestart.DialogResult = System.Windows.Forms.DialogResult.OK;
			btRestart.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btRestart.Location = new System.Drawing.Point(355, 7);
			btRestart.Name = "btRestart";
			btRestart.Size = new System.Drawing.Size(68, 24);
			btRestart.TabIndex = 4;
			btRestart.Text = "&Restart";
			btResume.DialogResult = System.Windows.Forms.DialogResult.Retry;
			btResume.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btResume.Location = new System.Drawing.Point(276, 7);
			btResume.Name = "btResume";
			btResume.Size = new System.Drawing.Size(73, 24);
			btResume.TabIndex = 3;
			btResume.Text = "&Try to resume";
			labelMessage.AutoSize = true;
			labelMessage.Location = new System.Drawing.Point(3, 0);
			labelMessage.MaximumSize = new System.Drawing.Size(500, 0);
			labelMessage.Name = "labelMessage";
			labelMessage.Size = new System.Drawing.Size(489, 26);
			labelMessage.TabIndex = 0;
			labelMessage.Text = "Please help to make ComicRack a better application by submitting the follwing report. This report does not contain any data that could identify you.";
			chkSubmit.AutoSize = true;
			chkSubmit.Checked = true;
			chkSubmit.CheckState = System.Windows.Forms.CheckState.Checked;
			chkSubmit.Location = new System.Drawing.Point(3, 12);
			chkSubmit.Name = "chkSubmit";
			chkSubmit.Size = new System.Drawing.Size(88, 17);
			chkSubmit.TabIndex = 1;
			chkSubmit.Text = "Submit report";
			chkSubmit.UseVisualStyleBackColor = true;
			btDetails.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btDetails.Location = new System.Drawing.Point(197, 7);
			btDetails.Name = "btDetails";
			btDetails.Size = new System.Drawing.Size(73, 24);
			btDetails.TabIndex = 2;
			btDetails.Text = "&Details";
			btDetails.Click += new System.EventHandler(btDetails_Click);
			tbLog.Font = new System.Drawing.Font("Courier New", 6.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			tbLog.Location = new System.Drawing.Point(3, 69);
			tbLog.Multiline = true;
			tbLog.Name = "tbLog";
			tbLog.ReadOnly = true;
			tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			tbLog.Size = new System.Drawing.Size(500, 250);
			tbLog.TabIndex = 6;
			tbLog.Visible = false;
			tbLog.WordWrap = false;
			lockTimer.Enabled = true;
			lockTimer.Tick += new System.EventHandler(lockTimer_Tick);
			btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btExit.Location = new System.Drawing.Point(429, 7);
			btExit.Name = "btExit";
			btExit.Size = new System.Drawing.Size(68, 24);
			btExit.TabIndex = 5;
			btExit.Text = "&Exit";
			flowLayoutPanel1.AutoSize = true;
			flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			flowLayoutPanel1.Controls.Add(labelMessage);
			flowLayoutPanel1.Controls.Add(panel1);
			flowLayoutPanel1.Controls.Add(tbLog);
			flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
			flowLayoutPanel1.Name = "flowLayoutPanel1";
			flowLayoutPanel1.Size = new System.Drawing.Size(506, 322);
			flowLayoutPanel1.TabIndex = 7;
			panel1.Controls.Add(chkSubmit);
			panel1.Controls.Add(btExit);
			panel1.Controls.Add(btDetails);
			panel1.Controls.Add(btResume);
			panel1.Controls.Add(btRestart);
			panel1.Location = new System.Drawing.Point(3, 29);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(500, 34);
			panel1.TabIndex = 1;
			base.AcceptButton = btResume;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.AutoScrollMargin = new System.Drawing.Size(8, 8);
			AutoSize = true;
			base.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			base.CancelButton = btExit;
			base.ClientSize = new System.Drawing.Size(525, 352);
			base.Controls.Add(flowLayoutPanel1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "CrashDialog";
			base.ShowIcon = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "ComicRack encountered a Problem...";
			base.TopMost = true;
			flowLayoutPanel1.ResumeLayout(false);
			flowLayoutPanel1.PerformLayout();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}
		
		private IContainer components;

		private Button btRestart;

		private Button btResume;

		private Label labelMessage;

		private CheckBox chkSubmit;

		private Button btDetails;

		private TextBox tbLog;

		private Timer lockTimer;

		private Button btExit;

		private FlowLayoutPanel flowLayoutPanel1;

		private Panel panel1;
	}
}
