using cYo.Common.Windows.Forms;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class DevicesEditDialog
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
			btCancel = new System.Windows.Forms.Button();
			btOK = new System.Windows.Forms.Button();
			tabDevices = new cYo.Common.Windows.Forms.TabControlEx();
			btPairDevice = new System.Windows.Forms.Button();
			labelHint = new System.Windows.Forms.Label();
			btDevice = new System.Windows.Forms.Button();
			cmDevice = new System.Windows.Forms.ContextMenuStrip(components);
			miDeviceCopy = new System.Windows.Forms.ToolStripMenuItem();
			miDevicePaste = new System.Windows.Forms.ToolStripMenuItem();
			miDeviceCopyToAll = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			miDeviceRename = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			miDeviceUnpair = new System.Windows.Forms.ToolStripMenuItem();
			cmDevice.SuspendLayout();
			SuspendLayout();
			btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(512, 425);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 4;
			btCancel.Text = "&Cancel";
			btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(426, 425);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(80, 24);
			btOK.TabIndex = 3;
			btOK.Text = "&OK";
			tabDevices.AllowDrop = true;
			tabDevices.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			tabDevices.Location = new System.Drawing.Point(12, 12);
			tabDevices.Name = "tabDevices";
			tabDevices.SelectedIndex = 0;
			tabDevices.Size = new System.Drawing.Size(580, 407);
			tabDevices.TabIndex = 5;
			btPairDevice.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btPairDevice.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btPairDevice.Location = new System.Drawing.Point(12, 425);
			btPairDevice.Name = "btPairDevice";
			btPairDevice.Size = new System.Drawing.Size(123, 24);
			btPairDevice.TabIndex = 6;
			btPairDevice.Text = "Pair with Device...";
			btPairDevice.Click += new System.EventHandler(btPair_Click);
			labelHint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			labelHint.Location = new System.Drawing.Point(12, 12);
			labelHint.Name = "labelHint";
			labelHint.Size = new System.Drawing.Size(580, 407);
			labelHint.TabIndex = 7;
			labelHint.Text = "Connect your Device with your Computer and press 'Pair Device...'";
			labelHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			btDevice.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btDevice.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.SmallArrowDown;
			btDevice.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			btDevice.Location = new System.Drawing.Point(141, 425);
			btDevice.Name = "btDevice";
			btDevice.Size = new System.Drawing.Size(100, 24);
			btDevice.TabIndex = 8;
			btDevice.Text = "Device";
			btDevice.UseVisualStyleBackColor = true;
			btDevice.Click += new System.EventHandler(btDevice_Click);
			cmDevice.Items.AddRange(new System.Windows.Forms.ToolStripItem[7]
			{
				miDeviceCopy,
				miDevicePaste,
				miDeviceCopyToAll,
				toolStripMenuItem1,
				miDeviceRename,
				toolStripMenuItem2,
				miDeviceUnpair
			});
			cmDevice.Name = "cmDevice";
			cmDevice.Size = new System.Drawing.Size(134, 126);
			cmDevice.Opening += new System.ComponentModel.CancelEventHandler(cmDevice_Opening);
			miDeviceCopy.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.EditCopy;
			miDeviceCopy.Name = "miDeviceCopy";
			miDeviceCopy.Size = new System.Drawing.Size(133, 22);
			miDeviceCopy.Text = "Copy";
			miDeviceCopy.Click += new System.EventHandler(miDeviceCopy_Click);
			miDevicePaste.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.EditPaste;
			miDevicePaste.Name = "miDevicePaste";
			miDevicePaste.Size = new System.Drawing.Size(133, 22);
			miDevicePaste.Text = "Paste";
			miDevicePaste.Click += new System.EventHandler(miDevicePaste_Click);
			miDeviceCopyToAll.Name = "miDeviceCopyToAll";
			miDeviceCopyToAll.Size = new System.Drawing.Size(133, 22);
			miDeviceCopyToAll.Text = "Copy to All";
			miDeviceCopyToAll.Click += new System.EventHandler(miDeviceCopyToAll_Click);
			toolStripMenuItem1.Name = "toolStripMenuItem1";
			toolStripMenuItem1.Size = new System.Drawing.Size(130, 6);
			miDeviceRename.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Rename;
			miDeviceRename.Name = "miDeviceRename";
			miDeviceRename.Size = new System.Drawing.Size(133, 22);
			miDeviceRename.Text = "Rename...";
			miDeviceRename.Click += new System.EventHandler(miDeviceRename_Click);
			toolStripMenuItem2.Name = "toolStripMenuItem2";
			toolStripMenuItem2.Size = new System.Drawing.Size(130, 6);
			miDeviceUnpair.Name = "miDeviceUnpair";
			miDeviceUnpair.Size = new System.Drawing.Size(133, 22);
			miDeviceUnpair.Text = "Unpair";
			miDeviceUnpair.Click += new System.EventHandler(miDeviceUnpair_Click);
			base.AcceptButton = btOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(604, 461);
			base.Controls.Add(btDevice);
			base.Controls.Add(btPairDevice);
			base.Controls.Add(btCancel);
			base.Controls.Add(btOK);
			base.Controls.Add(labelHint);
			base.Controls.Add(tabDevices);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			MinimumSize = new System.Drawing.Size(500, 460);
			base.Name = "DevicesEditDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Devices";
			cmDevice.ResumeLayout(false);
			ResumeLayout(false);
		}
		
		private IContainer components;

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
