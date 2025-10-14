using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using cYo.Common.Cryptography;
using cYo.Common.Localize;
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.IO.Network;
using cYo.Projects.ComicRack.Viewer.Dialogs;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public partial class RemoteConnectionView : SubView
	{
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			thread?.Abort();
			if (disposing)
			{
				if (oldImage != null)
				{
					TabImage = oldImage;
				}
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.btConnect = new System.Windows.Forms.Button();
            this.panelCenter = new System.Windows.Forms.Panel();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblServerDescription = new System.Windows.Forms.Label();
            this.lblServerName = new System.Windows.Forms.Label();
            this.connectionAnimation = new System.Windows.Forms.PictureBox();
            this.panelCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.connectionAnimation)).BeginInit();
            this.SuspendLayout();
            // 
            // btConnect
            // 
            this.btConnect.Location = new System.Drawing.Point(80, 161);
            this.btConnect.Name = "btConnect";
            this.btConnect.Size = new System.Drawing.Size(163, 31);
            this.btConnect.TabIndex = 0;
            this.btConnect.Text = "Connect";
            this.btConnect.UseVisualStyleBackColor = true;
            this.btConnect.Click += new System.EventHandler(this.btConnect_Click);
            // 
            // panelCenter
            // 
            this.panelCenter.Controls.Add(this.connectionAnimation);
            this.panelCenter.Controls.Add(this.lblMessage);
            this.panelCenter.Controls.Add(this.lblServerDescription);
            this.panelCenter.Controls.Add(this.lblServerName);
            this.panelCenter.Controls.Add(this.btConnect);
            this.panelCenter.Location = new System.Drawing.Point(16, 3);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(323, 195);
            this.panelCenter.TabIndex = 1;
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMessage.Location = new System.Drawing.Point(3, 127);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(314, 19);
            this.lblMessage.TabIndex = 3;
            this.lblMessage.Text = "Process Message";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMessage.Visible = false;
            // 
            // lblServerDescription
            // 
            this.lblServerDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblServerDescription.Location = new System.Drawing.Point(3, 20);
            this.lblServerDescription.Name = "lblServerDescription";
            this.lblServerDescription.Size = new System.Drawing.Size(314, 19);
            this.lblServerDescription.TabIndex = 2;
            this.lblServerDescription.Text = "Server Description";
            this.lblServerDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblServerName
            // 
            this.lblServerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblServerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServerName.Location = new System.Drawing.Point(3, 0);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(317, 20);
            this.lblServerName.TabIndex = 1;
            this.lblServerName.Text = "Server Name";
            this.lblServerName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // connectionAnimation
            // 
            this.connectionAnimation.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.BigBallAnimation;
            this.connectionAnimation.Location = new System.Drawing.Point(134, 57);
            this.connectionAnimation.Name = "connectionAnimation";
            this.connectionAnimation.Size = new System.Drawing.Size(54, 55);
            this.connectionAnimation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.connectionAnimation.TabIndex = 4;
            this.connectionAnimation.TabStop = false;
            this.connectionAnimation.Visible = false;
            // 
            // RemoteConnectionView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = SystemColors.Window;
            this.Controls.Add(this.panelCenter);
            this.Name = "RemoteConnectionView";
            this.Size = new System.Drawing.Size(356, 212);
            this.panelCenter.ResumeLayout(false);
            this.panelCenter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.connectionAnimation)).EndInit();
            this.ResumeLayout(false);

		}

        private Button btConnect;
        private Panel panelCenter;
        private Label lblMessage;
        private Label lblServerDescription;
        private Label lblServerName;
        private PictureBox connectionAnimation;
    }
}
