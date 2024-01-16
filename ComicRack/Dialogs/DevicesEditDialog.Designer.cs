using cYo.Common.Windows.Forms;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class DevicesEditDialog
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
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.tabDevices = new cYo.Common.Windows.Forms.TabControlEx();
            this.btPairDevice = new System.Windows.Forms.Button();
            this.labelHint = new System.Windows.Forms.Label();
            this.btDevice = new System.Windows.Forms.Button();
            this.cmDevice = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miDeviceCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.miDevicePaste = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeviceCopyToAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miDeviceRename = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.miDeviceUnpair = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDevice.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btCancel.Location = new System.Drawing.Point(512, 425);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(80, 24);
            this.btCancel.TabIndex = 4;
            this.btCancel.Text = "&Cancel";
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btOK.Location = new System.Drawing.Point(426, 425);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(80, 24);
            this.btOK.TabIndex = 3;
            this.btOK.Text = "&OK";
            // 
            // tabDevices
            // 
            this.tabDevices.AllowDrop = true;
            this.tabDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabDevices.Location = new System.Drawing.Point(12, 12);
            this.tabDevices.Name = "tabDevices";
            this.tabDevices.SelectedIndex = 0;
            this.tabDevices.Size = new System.Drawing.Size(580, 407);
            this.tabDevices.TabIndex = 5;
            // 
            // btPairDevice
            // 
            this.btPairDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btPairDevice.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btPairDevice.Location = new System.Drawing.Point(12, 425);
            this.btPairDevice.Name = "btPairDevice";
            this.btPairDevice.Size = new System.Drawing.Size(123, 24);
            this.btPairDevice.TabIndex = 6;
            this.btPairDevice.Text = "Pair with Device...";
            this.btPairDevice.Click += new System.EventHandler(this.btPair_Click);
            // 
            // labelHint
            // 
            this.labelHint.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHint.Location = new System.Drawing.Point(12, 12);
            this.labelHint.Name = "labelHint";
            this.labelHint.Size = new System.Drawing.Size(580, 407);
            this.labelHint.TabIndex = 7;
            this.labelHint.Text = "Connect your Device with your Computer and press \'Pair Device...\'";
            this.labelHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btDevice
            // 
            this.btDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btDevice.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.SmallArrowDown;
            this.btDevice.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btDevice.Location = new System.Drawing.Point(141, 425);
            this.btDevice.Name = "btDevice";
            this.btDevice.Size = new System.Drawing.Size(100, 24);
            this.btDevice.TabIndex = 8;
            this.btDevice.Text = "Device";
            this.btDevice.UseVisualStyleBackColor = true;
            this.btDevice.Click += new System.EventHandler(this.btDevice_Click);
            // 
            // cmDevice
            // 
            this.cmDevice.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDeviceCopy,
            this.miDevicePaste,
            this.miDeviceCopyToAll,
            this.toolStripMenuItem1,
            this.miDeviceRename,
            this.toolStripMenuItem2,
            this.miDeviceUnpair});
            this.cmDevice.Name = "cmDevice";
            this.cmDevice.Size = new System.Drawing.Size(134, 126);
            this.cmDevice.Opening += new System.ComponentModel.CancelEventHandler(this.cmDevice_Opening);
            // 
            // miDeviceCopy
            // 
            this.miDeviceCopy.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.EditCopy;
            this.miDeviceCopy.Name = "miDeviceCopy";
            this.miDeviceCopy.Size = new System.Drawing.Size(133, 22);
            this.miDeviceCopy.Text = "Copy";
            this.miDeviceCopy.Click += new System.EventHandler(this.miDeviceCopy_Click);
            // 
            // miDevicePaste
            // 
            this.miDevicePaste.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.EditPaste;
            this.miDevicePaste.Name = "miDevicePaste";
            this.miDevicePaste.Size = new System.Drawing.Size(133, 22);
            this.miDevicePaste.Text = "Paste";
            this.miDevicePaste.Click += new System.EventHandler(this.miDevicePaste_Click);
            // 
            // miDeviceCopyToAll
            // 
            this.miDeviceCopyToAll.Name = "miDeviceCopyToAll";
            this.miDeviceCopyToAll.Size = new System.Drawing.Size(133, 22);
            this.miDeviceCopyToAll.Text = "Copy to All";
            this.miDeviceCopyToAll.Click += new System.EventHandler(this.miDeviceCopyToAll_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(130, 6);
            // 
            // miDeviceRename
            // 
            this.miDeviceRename.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Rename;
            this.miDeviceRename.Name = "miDeviceRename";
            this.miDeviceRename.Size = new System.Drawing.Size(133, 22);
            this.miDeviceRename.Text = "Rename...";
            this.miDeviceRename.Click += new System.EventHandler(this.miDeviceRename_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(130, 6);
            // 
            // miDeviceUnpair
            // 
            this.miDeviceUnpair.Name = "miDeviceUnpair";
            this.miDeviceUnpair.Size = new System.Drawing.Size(133, 22);
            this.miDeviceUnpair.Text = "Unpair";
            this.miDeviceUnpair.Click += new System.EventHandler(this.miDeviceUnpair_Click);
            // 
            // DevicesEditDialog
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(604, 461);
            this.Controls.Add(this.btDevice);
            this.Controls.Add(this.btPairDevice);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.labelHint);
            this.Controls.Add(this.tabDevices);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 460);
            this.Name = "DevicesEditDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Devices";
            this.cmDevice.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		
		private Button btCancel;
		private Button btOK;
		private TabControlEx tabDevices;
		private Button btPairDevice;
		private Label labelHint;
		private Button btDevice;
		private ContextMenuStrip cmDevice;
		private ToolStripMenuItem miDeviceCopy;
		private ToolStripMenuItem miDevicePaste;
		private ToolStripMenuItem miDeviceCopyToAll;
		private ToolStripSeparator toolStripMenuItem1;
		private ToolStripMenuItem miDeviceRename;
		private ToolStripSeparator toolStripMenuItem2;
		private ToolStripMenuItem miDeviceUnpair;
	}
}
