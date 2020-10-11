using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class DeviceSelectDialog
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
			btCancel = new System.Windows.Forms.Button();
			btOK = new System.Windows.Forms.Button();
			lvDevices = new System.Windows.Forms.ListView();
			colName = new System.Windows.Forms.ColumnHeader();
			colModel = new System.Windows.Forms.ColumnHeader();
			colSerial = new System.Windows.Forms.ColumnHeader();
			SuspendLayout();
			btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(303, 170);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 6;
			btCancel.Text = "&Cancel";
			btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(217, 170);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(80, 24);
			btOK.TabIndex = 5;
			btOK.Text = "&OK";
			lvDevices.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			lvDevices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[3]
			{
				colName,
				colModel,
				colSerial
			});
			lvDevices.FullRowSelect = true;
			lvDevices.Location = new System.Drawing.Point(12, 12);
			lvDevices.MultiSelect = false;
			lvDevices.Name = "lvDevices";
			lvDevices.Size = new System.Drawing.Size(371, 149);
			lvDevices.TabIndex = 7;
			lvDevices.UseCompatibleStateImageBehavior = false;
			lvDevices.View = System.Windows.Forms.View.Details;
			lvDevices.DoubleClick += new System.EventHandler(lvDevices_DoubleClick);
			colName.Text = "Name";
			colName.Width = 128;
			colModel.Text = "Model";
			colModel.Width = 109;
			colSerial.Text = "Serial";
			colSerial.Width = 102;
			base.AcceptButton = btOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(395, 206);
			base.Controls.Add(lvDevices);
			base.Controls.Add(btCancel);
			base.Controls.Add(btOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DeviceSelectDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Select Device";
			ResumeLayout(false);
		}
		
		private IContainer components;

		private Button btCancel;

		private Button btOK;

		private ListView lvDevices;

		private ColumnHeader colName;

		private ColumnHeader colModel;

		private ColumnHeader colSerial;
	}
}
