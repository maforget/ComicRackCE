using cYo.Common.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class OpenRemoteDialog
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Local Shares", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Internet Shares", System.Windows.Forms.HorizontalAlignment.Left);
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.labelServerAddress = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cbServer = new System.Windows.Forms.ComboBox();
            this.txFilter = new System.Windows.Forms.TextBox();
            this.panelList = new System.Windows.Forms.Panel();
            this.txPassword = new cYo.Common.Windows.Forms.PasswordTextBox();
            this.labelListPassword = new System.Windows.Forms.Label();
            this.labelFailedServerList = new System.Windows.Forms.Label();
            this.lvServers = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEdit = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colExport = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.btPublic = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelList.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btCancel.Location = new System.Drawing.Point(473, 3);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(80, 24);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "&Cancel";
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btOK.Location = new System.Drawing.Point(387, 3);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(80, 24);
            this.btOK.TabIndex = 1;
            this.btOK.Text = "&OK";
            // 
            // labelServerAddress
            // 
            this.labelServerAddress.AutoSize = true;
            this.labelServerAddress.Location = new System.Drawing.Point(3, 6);
            this.labelServerAddress.Name = "labelServerAddress";
            this.labelServerAddress.Size = new System.Drawing.Size(82, 13);
            this.labelServerAddress.TabIndex = 0;
            this.labelServerAddress.Text = "Library Address:";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.panelList);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(8, 7);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(553, 331);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.cbServer);
            this.panel1.Controls.Add(this.labelServerAddress);
            this.panel1.Controls.Add(this.txFilter);
            this.panel1.Location = new System.Drawing.Point(0, 3);
            this.panel1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(553, 27);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Search;
            this.pictureBox1.Location = new System.Drawing.Point(422, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // cbServer
            // 
            this.cbServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbServer.FormattingEnabled = true;
            this.cbServer.Location = new System.Drawing.Point(96, 3);
            this.cbServer.Name = "cbServer";
            this.cbServer.Size = new System.Drawing.Size(304, 21);
            this.cbServer.TabIndex = 1;
            this.cbServer.SelectedIndexChanged += new System.EventHandler(this.cbServer_SelectedIndexChanged);
            this.cbServer.TextUpdate += new System.EventHandler(this.cbServer_TextUpdate);
            // 
            // txFilter
            // 
            this.txFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txFilter.Location = new System.Drawing.Point(444, 3);
            this.txFilter.Name = "txFilter";
            this.txFilter.Size = new System.Drawing.Size(109, 20);
            this.txFilter.TabIndex = 2;
            this.txFilter.TextChanged += new System.EventHandler(this.txFilter_TextChanged);
            // 
            // panelList
            // 
            this.panelList.Controls.Add(this.txPassword);
            this.panelList.Controls.Add(this.labelListPassword);
            this.panelList.Controls.Add(this.labelFailedServerList);
            this.panelList.Controls.Add(this.lvServers);
            this.panelList.Location = new System.Drawing.Point(0, 36);
            this.panelList.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.panelList.Name = "panelList";
            this.panelList.Size = new System.Drawing.Size(553, 256);
            this.panelList.TabIndex = 8;
            // 
            // txPassword
            // 
            this.txPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txPassword.Location = new System.Drawing.Point(423, 229);
            this.txPassword.Name = "txPassword";
            this.txPassword.Password = null;
            this.txPassword.Size = new System.Drawing.Size(130, 20);
            this.txPassword.TabIndex = 3;
            this.txPassword.UseSystemPasswordChar = true;
            // 
            // labelListPassword
            // 
            this.labelListPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelListPassword.Location = new System.Drawing.Point(245, 232);
            this.labelListPassword.Name = "labelListPassword";
            this.labelListPassword.Size = new System.Drawing.Size(172, 13);
            this.labelListPassword.TabIndex = 2;
            this.labelListPassword.Text = "Private List Password:";
            this.labelListPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelFailedServerList
            // 
            this.labelFailedServerList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFailedServerList.BackColor = SystemColors.Window;
            this.labelFailedServerList.ForeColor = SystemColors.GrayText;
            this.labelFailedServerList.Location = new System.Drawing.Point(184, 96);
            this.labelFailedServerList.Name = "labelFailedServerList";
            this.labelFailedServerList.Size = new System.Drawing.Size(186, 68);
            this.labelFailedServerList.TabIndex = 1;
            this.labelFailedServerList.Text = "Failed to get the Server List!";
            this.labelFailedServerList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelFailedServerList.Visible = false;
            // 
            // lvServers
            // 
            this.lvServers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colDescription,
            this.colEdit,
            this.colExport});
            this.lvServers.FullRowSelect = true;
            listViewGroup1.Header = "Local Shares";
            listViewGroup1.Name = "groupLocal";
            listViewGroup2.Header = "Internet Shares";
            listViewGroup2.Name = "groupInternet";
            this.lvServers.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.lvServers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvServers.HideSelection = false;
            this.lvServers.Location = new System.Drawing.Point(0, 3);
            this.lvServers.Name = "lvServers";
            this.lvServers.Size = new System.Drawing.Size(553, 224);
            this.lvServers.SmallImageList = this.imageList;
            this.lvServers.TabIndex = 0;
            this.lvServers.UseCompatibleStateImageBehavior = false;
            this.lvServers.View = System.Windows.Forms.View.Details;
            this.lvServers.ItemActivate += new System.EventHandler(this.lvServers_ItemActivate);
            this.lvServers.SelectedIndexChanged += new System.EventHandler(this.lvServers_SelectedIndexChanged);
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 119;
            // 
            // colDescription
            // 
            this.colDescription.Text = "Description";
            this.colDescription.Width = 310;
            // 
            // colEdit
            // 
            this.colEdit.Text = "Edit";
            this.colEdit.Width = 41;
            // 
            // colExport
            // 
            this.colExport.Text = "Export";
            this.colExport.Width = 49;
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.btPublic);
            this.panel2.Controls.Add(this.btCancel);
            this.panel2.Controls.Add(this.btOK);
            this.panel2.Location = new System.Drawing.Point(0, 298);
            this.panel2.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(553, 30);
            this.panel2.TabIndex = 2;
            // 
            // btPublic
            // 
            this.btPublic.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btPublic.Location = new System.Drawing.Point(0, 3);
            this.btPublic.Name = "btPublic";
            this.btPublic.Size = new System.Drawing.Size(117, 24);
            this.btPublic.TabIndex = 0;
            this.btPublic.Text = "Show List";
            this.btPublic.Click += new System.EventHandler(this.btPublic_Click);
            // 
            // OpenRemoteDialog
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(570, 348);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenRemoteDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Open Remote Library";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelList.ResumeLayout(false);
            this.panelList.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		
		private Button btCancel;
		private Button btOK;
		private Label labelServerAddress;
		private FlowLayoutPanel flowLayoutPanel1;
		private Panel panel1;
		private ComboBox cbServer;
		private ListView lvServers;
		private Panel panel2;
		private Button btPublic;
		private ColumnHeader colName;
		private ColumnHeader colDescription;
		private Panel panelList;
		private TextBox txFilter;
		private ImageList imageList;
		private Label labelFailedServerList;
		private PasswordTextBox txPassword;
		private Label labelListPassword;
		private ColumnHeader colEdit;
		private ColumnHeader colExport;
		private PictureBox pictureBox1;
	}
}
