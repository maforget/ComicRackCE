using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class CrashDialog
	{
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
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
            this.components = new System.ComponentModel.Container();
            this.btRestart = new System.Windows.Forms.Button();
            this.btResume = new System.Windows.Forms.Button();
            this.labelMessage = new System.Windows.Forms.Label();
            this.btDetails = new System.Windows.Forms.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.lockTimer = new System.Windows.Forms.Timer(this.components);
            this.btExit = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btRestart
            // 
            this.btRestart.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btRestart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btRestart.Location = new System.Drawing.Point(355, 7);
            this.btRestart.Name = "btRestart";
            this.btRestart.Size = new System.Drawing.Size(68, 24);
            this.btRestart.TabIndex = 4;
            this.btRestart.Text = "&Restart";
            // 
            // btResume
            // 
            this.btResume.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.btResume.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btResume.Location = new System.Drawing.Point(276, 7);
            this.btResume.Name = "btResume";
            this.btResume.Size = new System.Drawing.Size(73, 24);
            this.btResume.TabIndex = 3;
            this.btResume.Text = "&Try to resume";
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new System.Drawing.Point(3, 0);
            this.labelMessage.MaximumSize = new System.Drawing.Size(500, 0);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(489, 26);
            this.labelMessage.TabIndex = 0;
            this.labelMessage.Text = "Please help to make ComicRack a better application by submitting the follwing rep" +
    "ort. This report does not contain any data that could identify you.";
            // 
            // btDetails
            // 
            this.btDetails.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btDetails.Location = new System.Drawing.Point(197, 7);
            this.btDetails.Name = "btDetails";
            this.btDetails.Size = new System.Drawing.Size(73, 24);
            this.btDetails.TabIndex = 2;
            this.btDetails.Text = "&Details";
            this.btDetails.Click += new System.EventHandler(this.btDetails_Click);
            // 
            // tbLog
            // 
            this.tbLog.Font = new System.Drawing.Font("Courier New", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbLog.Location = new System.Drawing.Point(3, 69);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLog.Size = new System.Drawing.Size(500, 250);
            this.tbLog.TabIndex = 6;
            this.tbLog.Visible = false;
            this.tbLog.WordWrap = false;
            // 
            // lockTimer
            // 
            this.lockTimer.Enabled = true;
            this.lockTimer.Tick += new System.EventHandler(this.lockTimer_Tick);
            // 
            // btExit
            // 
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btExit.Location = new System.Drawing.Point(429, 7);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(68, 24);
            this.btExit.TabIndex = 5;
            this.btExit.Text = "&Exit";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.labelMessage);
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.tbLog);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(506, 322);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btExit);
            this.panel1.Controls.Add(this.btDetails);
            this.panel1.Controls.Add(this.btResume);
            this.panel1.Controls.Add(this.btRestart);
            this.panel1.Location = new System.Drawing.Point(3, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(500, 34);
            this.panel1.TabIndex = 1;
            // 
            // CrashDialog
            // 
            this.AcceptButton = this.btResume;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScrollMargin = new System.Drawing.Size(8, 8);
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(525, 352);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CrashDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ComicRack encountered a Problem...";
            this.TopMost = true;
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		
		private Button btRestart;
		private Button btResume;
		private Label labelMessage;
		private Button btDetails;
		private TextBox tbLog;
		private Timer lockTimer;
		private Button btExit;
		private FlowLayoutPanel flowLayoutPanel1;
		private Panel panel1;
	}
}
