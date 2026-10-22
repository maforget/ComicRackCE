using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class DeviceSelectDialog
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
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.lvDevices = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colModel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSerial = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSyncId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlSyncId = new System.Windows.Forms.Panel();
            this.lblSyncId = new System.Windows.Forms.Label();
            this.txSyncId = new System.Windows.Forms.TextBox();
            this.pnlSyncId.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btCancel.Location = new System.Drawing.Point(303, 206);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(80, 24);
            this.btCancel.TabIndex = 6;
            this.btCancel.Text = "&Cancel";
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btOK.Location = new System.Drawing.Point(217, 206);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(80, 24);
            this.btOK.TabIndex = 5;
            this.btOK.Text = "&OK";
            // 
            // lvDevices
            // 
            this.lvDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvDevices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colModel,
            this.colSerial,
            this.colSyncId});
            this.lvDevices.FullRowSelect = true;
            this.lvDevices.HideSelection = false;
            this.lvDevices.Location = new System.Drawing.Point(12, 12);
            this.lvDevices.MultiSelect = false;
            this.lvDevices.Name = "lvDevices";
            this.lvDevices.Size = new System.Drawing.Size(371, 149);
            this.lvDevices.TabIndex = 7;
            this.lvDevices.UseCompatibleStateImageBehavior = false;
            this.lvDevices.View = System.Windows.Forms.View.Details;
            this.lvDevices.SelectedIndexChanged += new System.EventHandler(this.lvDevices_SelectedIndexChanged);
            this.lvDevices.DoubleClick += new System.EventHandler(this.lvDevices_DoubleClick);
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 128;
            // 
            // colModel
            // 
            this.colModel.Text = "Model";
            this.colModel.Width = 109;
            // 
            // colSerial
            // 
            this.colSerial.Text = "Serial";
            this.colSerial.Width = 102;
            // 
            // colSyncId
            // 
            this.colSyncId.Text = "Sync ID";
            this.colSyncId.Width = 250;
            // 
            // pnlSyncId
            // 
            this.pnlSyncId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSyncId.Controls.Add(this.lblSyncId);
            this.pnlSyncId.Controls.Add(this.txSyncId);
            this.pnlSyncId.Location = new System.Drawing.Point(12, 166);
            this.pnlSyncId.Name = "pnlSyncId";
            this.pnlSyncId.Size = new System.Drawing.Size(371, 36);
            this.pnlSyncId.TabIndex = 8;
            // 
            // lblSyncId
            // 
            this.lblSyncId.AutoSize = true;
            this.lblSyncId.Location = new System.Drawing.Point(0, 0);
            this.lblSyncId.Name = "lblSyncId";
            this.lblSyncId.Size = new System.Drawing.Size(250, 13);
            this.lblSyncId.TabIndex = 1;
            this.lblSyncId.Text = "New Sync ID for '{0}' (current: {1}). Select a device above or type it:";
            // 
            // txSyncId
            // 
            this.txSyncId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txSyncId.Location = new System.Drawing.Point(0, 16);
            this.txSyncId.Name = "txSyncId";
            this.txSyncId.Size = new System.Drawing.Size(371, 20);
            this.txSyncId.TabIndex = 0;
            this.txSyncId.TextChanged += new System.EventHandler(this.txSyncId_TextChanged);
            // 
            // DeviceSelectDialog
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(395, 242);
            this.Controls.Add(this.pnlSyncId);
            this.Controls.Add(this.lvDevices);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeviceSelectDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Device";
            this.pnlSyncId.ResumeLayout(false);
            this.pnlSyncId.PerformLayout();
            this.ResumeLayout(false);

		}
		
		private Button btCancel;
		private Button btOK;
		private ListView lvDevices;
		private ColumnHeader colName;
		private ColumnHeader colModel;
		private ColumnHeader colSerial;
		private ColumnHeader colSyncId;
		private Panel pnlSyncId;
		private Label lblSyncId;
		private TextBox txSyncId;
	}
}
