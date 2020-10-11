using cYo.Common.Windows.Forms;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class OpenRemoteDialog
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
			System.Windows.Forms.ListViewGroup listViewGroup = new System.Windows.Forms.ListViewGroup("Local Shares", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Internet Shares", System.Windows.Forms.HorizontalAlignment.Left);
			btCancel = new System.Windows.Forms.Button();
			btOK = new System.Windows.Forms.Button();
			labelServerAddress = new System.Windows.Forms.Label();
			flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			panel1 = new System.Windows.Forms.Panel();
			cbServer = new System.Windows.Forms.ComboBox();
			panelList = new System.Windows.Forms.Panel();
			txPassword = new cYo.Common.Windows.Forms.PasswordTextBox();
			labelListPassword = new System.Windows.Forms.Label();
			labelFailedServerList = new System.Windows.Forms.Label();
			txFilter = new System.Windows.Forms.TextBox();
			lvServers = new System.Windows.Forms.ListView();
			colName = new System.Windows.Forms.ColumnHeader();
			colDescription = new System.Windows.Forms.ColumnHeader();
			colEdit = new System.Windows.Forms.ColumnHeader();
			colExport = new System.Windows.Forms.ColumnHeader();
			imageList = new System.Windows.Forms.ImageList(components);
			panel2 = new System.Windows.Forms.Panel();
			btPublic = new System.Windows.Forms.Button();
			pictureBox1 = new System.Windows.Forms.PictureBox();
			flowLayoutPanel1.SuspendLayout();
			panel1.SuspendLayout();
			panelList.SuspendLayout();
			panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			SuspendLayout();
			btCancel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(473, 3);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 2;
			btCancel.Text = "&Cancel";
			btOK.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(387, 3);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(80, 24);
			btOK.TabIndex = 1;
			btOK.Text = "&OK";
			labelServerAddress.AutoSize = true;
			labelServerAddress.Location = new System.Drawing.Point(3, 6);
			labelServerAddress.Name = "labelServerAddress";
			labelServerAddress.Size = new System.Drawing.Size(82, 13);
			labelServerAddress.TabIndex = 0;
			labelServerAddress.Text = "Library Address:";
			flowLayoutPanel1.AutoSize = true;
			flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			flowLayoutPanel1.Controls.Add(panel1);
			flowLayoutPanel1.Controls.Add(panelList);
			flowLayoutPanel1.Controls.Add(panel2);
			flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			flowLayoutPanel1.Location = new System.Drawing.Point(8, 7);
			flowLayoutPanel1.Name = "flowLayoutPanel1";
			flowLayoutPanel1.Size = new System.Drawing.Size(553, 331);
			flowLayoutPanel1.TabIndex = 7;
			panel1.Controls.Add(pictureBox1);
			panel1.Controls.Add(cbServer);
			panel1.Controls.Add(labelServerAddress);
			panel1.Controls.Add(txFilter);
			panel1.Location = new System.Drawing.Point(0, 3);
			panel1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(553, 27);
			panel1.TabIndex = 0;
			cbServer.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			cbServer.FormattingEnabled = true;
			cbServer.Location = new System.Drawing.Point(96, 3);
			cbServer.Name = "cbServer";
			cbServer.Size = new System.Drawing.Size(304, 21);
			cbServer.TabIndex = 1;
			cbServer.SelectedIndexChanged += new System.EventHandler(cbServer_SelectedIndexChanged);
			cbServer.TextUpdate += new System.EventHandler(cbServer_TextUpdate);
			panelList.Controls.Add(txPassword);
			panelList.Controls.Add(labelListPassword);
			panelList.Controls.Add(labelFailedServerList);
			panelList.Controls.Add(lvServers);
			panelList.Location = new System.Drawing.Point(0, 36);
			panelList.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			panelList.Name = "panelList";
			panelList.Size = new System.Drawing.Size(553, 256);
			panelList.TabIndex = 8;
			txPassword.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			txPassword.Location = new System.Drawing.Point(423, 229);
			txPassword.Name = "txPassword";
			txPassword.Password = null;
			txPassword.Size = new System.Drawing.Size(130, 20);
			txPassword.TabIndex = 3;
			txPassword.UseSystemPasswordChar = true;
			labelListPassword.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			labelListPassword.Location = new System.Drawing.Point(245, 232);
			labelListPassword.Name = "labelListPassword";
			labelListPassword.Size = new System.Drawing.Size(172, 13);
			labelListPassword.TabIndex = 2;
			labelListPassword.Text = "Private List Password:";
			labelListPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
			labelFailedServerList.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			labelFailedServerList.BackColor = System.Drawing.SystemColors.Window;
			labelFailedServerList.ForeColor = System.Drawing.SystemColors.GrayText;
			labelFailedServerList.Location = new System.Drawing.Point(184, 96);
			labelFailedServerList.Name = "labelFailedServerList";
			labelFailedServerList.Size = new System.Drawing.Size(186, 68);
			labelFailedServerList.TabIndex = 1;
			labelFailedServerList.Text = "Failed to get the Server List!";
			labelFailedServerList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			labelFailedServerList.Visible = false;
			txFilter.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			txFilter.Location = new System.Drawing.Point(444, 3);
			txFilter.Name = "txFilter";
			txFilter.Size = new System.Drawing.Size(109, 20);
			txFilter.TabIndex = 2;
			txFilter.TextChanged += new System.EventHandler(txFilter_TextChanged);
			lvServers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[4]
			{
				colName,
				colDescription,
				colEdit,
				colExport
			});
			lvServers.FullRowSelect = true;
			listViewGroup.Header = "Local Shares";
			listViewGroup.Name = "groupLocal";
			listViewGroup2.Header = "Internet Shares";
			listViewGroup2.Name = "groupInternet";
			lvServers.Groups.AddRange(new System.Windows.Forms.ListViewGroup[2]
			{
				listViewGroup,
				listViewGroup2
			});
			lvServers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			lvServers.Location = new System.Drawing.Point(0, 3);
			lvServers.Name = "lvServers";
			lvServers.Size = new System.Drawing.Size(553, 224);
			lvServers.SmallImageList = imageList;
			lvServers.TabIndex = 0;
			lvServers.UseCompatibleStateImageBehavior = false;
			lvServers.View = System.Windows.Forms.View.Details;
			lvServers.ItemActivate += new System.EventHandler(lvServers_ItemActivate);
			lvServers.SelectedIndexChanged += new System.EventHandler(lvServers_SelectedIndexChanged);
			colName.Text = "Name";
			colName.Width = 119;
			colDescription.Text = "Description";
			colDescription.Width = 310;
			colEdit.Text = "Edit";
			colEdit.Width = 41;
			colExport.Text = "Export";
			colExport.Width = 49;
			imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			imageList.ImageSize = new System.Drawing.Size(16, 16);
			imageList.TransparentColor = System.Drawing.Color.Transparent;
			panel2.AutoSize = true;
			panel2.Controls.Add(btPublic);
			panel2.Controls.Add(btCancel);
			panel2.Controls.Add(btOK);
			panel2.Location = new System.Drawing.Point(0, 298);
			panel2.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			panel2.Name = "panel2";
			panel2.Size = new System.Drawing.Size(553, 30);
			panel2.TabIndex = 2;
			btPublic.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btPublic.Location = new System.Drawing.Point(0, 3);
			btPublic.Name = "btPublic";
			btPublic.Size = new System.Drawing.Size(117, 24);
			btPublic.TabIndex = 0;
			btPublic.Text = "Show List";
			btPublic.Click += new System.EventHandler(btPublic_Click);
			pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			pictureBox1.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Search;
			pictureBox1.Location = new System.Drawing.Point(422, 6);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new System.Drawing.Size(16, 16);
			pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			pictureBox1.TabIndex = 6;
			pictureBox1.TabStop = false;
			base.AcceptButton = btOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			AutoSize = true;
			base.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(570, 348);
			base.Controls.Add(flowLayoutPanel1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "OpenRemoteDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Open Remote Library";
			flowLayoutPanel1.ResumeLayout(false);
			flowLayoutPanel1.PerformLayout();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			panelList.ResumeLayout(false);
			panelList.PerformLayout();
			panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}
		
		private IContainer components;

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
