using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Localize;
using cYo.Common.Win32;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.IO.Network;
using cYo.Projects.ComicRack.Viewer.Controls;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public class ServerEditControl : UserControl
	{
		private IContainer components;

		private CheckBox chkClientsCanExport;

		private CheckBox chkClientsCanEdit;

		private PasswordTextBox txPassword;

		private CheckBox chkRequirePassword;

		private TextBox txSharedName;

		private Label labelSharedName;

		private TextBox txPublicServerComment;

		private Label labelShareDescription;

		private CheckBox chkShareInternet;

		private CheckBox chkPrivate;

		private TrackBarLite tbPageQuality;

		private Label labelPageQuality;

		private Label labelThumbQuality;

		private TrackBarLite tbThumbQuality;

		private ToolTip toolTip;

		private TreeViewEx tvSharedLists;

		private ComboBox cbShare;

		private ImageList ilShares;

		private GroupBox grpShareSettings;

		public ComicLibraryServerConfig Config
		{
			get
			{
				LibraryShareMode selectedIndex = (LibraryShareMode)cbShare.SelectedIndex;
				ComicLibraryServerConfig comicLibraryServerConfig = new ComicLibraryServerConfig
				{
					Name = txSharedName.Text,
					LibraryShareMode = selectedIndex,
					IsProtected = chkRequirePassword.Checked,
					IsEditable = chkClientsCanEdit.Checked,
					IsExportable = chkClientsCanExport.Checked,
					IsInternet = chkShareInternet.Checked,
					IsPrivate = chkPrivate.Checked,
					Description = txPublicServerComment.Text,
					Password = txPassword.Password,
					ThumbnailQuality = tbThumbQuality.Value,
					PageQuality = tbPageQuality.Value
				};
				comicLibraryServerConfig.SharedItems.AddRange(from clic in (from tn in tvSharedLists.AllNodes()
						where tn.Checked
						select (ComicListItem)tn.Tag).OfType<ShareableComicListItem>()
					select clic.Id);
				return comicLibraryServerConfig;
			}
			set
			{
				txSharedName.Text = value.Name;
				chkClientsCanEdit.Checked = value.IsEditable;
				chkClientsCanExport.Checked = value.IsExportable;
				txPassword.Password = value.Password;
				chkRequirePassword.Checked = value.IsProtected;
				cbShare.SelectedIndex = (int)value.LibraryShareMode;
				chkShareInternet.Checked = value.IsInternet;
				chkPrivate.Checked = value.IsPrivate;
				txPublicServerComment.Text = value.Description;
				foreach (TreeNode item in tvSharedLists.AllNodes())
				{
					item.Checked = value.SharedItems.Contains(((ComicListItem)item.Tag).Id);
				}
				tbThumbQuality.Value = value.ThumbnailQuality;
				tbPageQuality.Value = value.PageQuality;
			}
		}

		public string ShareName => txSharedName.Text;

		public event EventHandler ShareNameChanged;

		public ServerEditControl()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, value: true);
			InitializeComponent();
			TR tr = TR.Load("PreferencesDialog");
			LocalizeUtility.Localize(tr, this);
			LocalizeUtility.Localize(tr, cbShare);
			ilShares.Images.Add("Library", Resources.Library);
			ilShares.Images.Add("Folder", Resources.SearchFolder);
			ilShares.Images.Add("Search", Resources.SearchDocument);
			ilShares.Images.Add("List", Resources.List);
			ilShares.Images.Add("TempFolder", Resources.TempFolder);
			FillListTree(tvSharedLists.Nodes, Program.Database.ComicLists);
			IdleProcess.Idle += IdleProcess_Idle;
			new LibraryTreeSkin().TreeView = tvSharedLists;
		}

		private void FillListTree(TreeNodeCollection tnc, IEnumerable<ComicListItem> clic)
		{
			foreach (ComicListItem item in clic)
			{
				TreeNode treeNode = tnc.Add(item.Name);
				treeNode.Tag = item;
				string text2 = (treeNode.ImageKey = (treeNode.SelectedImageKey = item.ImageKey));
				if (item is ComicListItemFolder)
				{
					FillListTree(treeNode.Nodes, ((ComicListItemFolder)item).Items);
					treeNode.ExpandAll();
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				IdleProcess.Idle -= IdleProcess_Idle;
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private void IdleProcess_Idle(object sender, EventArgs e)
		{
			tvSharedLists.Enabled = cbShare.SelectedIndex == 2;
			txPassword.Enabled = chkRequirePassword.Checked;
			chkPrivate.Enabled = chkShareInternet.Checked;
		}

		private void txSharedName_TextChanged(object sender, EventArgs e)
		{
			if (this.ShareNameChanged != null)
			{
				this.ShareNameChanged(this, e);
			}
		}

		private void tbPageQuality_ValueChanged(object sender, EventArgs e)
		{
			TrackBarLite trackBarLite = sender as TrackBarLite;
			toolTip.SetToolTip(trackBarLite, trackBarLite.Value.ToString());
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			chkClientsCanExport = new System.Windows.Forms.CheckBox();
			chkClientsCanEdit = new System.Windows.Forms.CheckBox();
			txPassword = new cYo.Common.Windows.Forms.PasswordTextBox();
			chkRequirePassword = new System.Windows.Forms.CheckBox();
			txSharedName = new System.Windows.Forms.TextBox();
			labelSharedName = new System.Windows.Forms.Label();
			txPublicServerComment = new System.Windows.Forms.TextBox();
			labelShareDescription = new System.Windows.Forms.Label();
			chkShareInternet = new System.Windows.Forms.CheckBox();
			chkPrivate = new System.Windows.Forms.CheckBox();
			tbPageQuality = new cYo.Common.Windows.Forms.TrackBarLite();
			labelPageQuality = new System.Windows.Forms.Label();
			labelThumbQuality = new System.Windows.Forms.Label();
			tbThumbQuality = new cYo.Common.Windows.Forms.TrackBarLite();
			toolTip = new System.Windows.Forms.ToolTip(components);
			tvSharedLists = new cYo.Common.Windows.Forms.TreeViewEx();
			ilShares = new System.Windows.Forms.ImageList(components);
			cbShare = new System.Windows.Forms.ComboBox();
			grpShareSettings = new System.Windows.Forms.GroupBox();
			grpShareSettings.SuspendLayout();
			SuspendLayout();
			chkClientsCanExport.AutoSize = true;
			chkClientsCanExport.Location = new System.Drawing.Point(5, 138);
			chkClientsCanExport.Name = "chkClientsCanExport";
			chkClientsCanExport.Size = new System.Drawing.Size(143, 17);
			chkClientsCanExport.TabIndex = 11;
			chkClientsCanExport.Text = "Clients can export Books";
			chkClientsCanExport.UseVisualStyleBackColor = true;
			chkClientsCanEdit.AutoSize = true;
			chkClientsCanEdit.Location = new System.Drawing.Point(5, 120);
			chkClientsCanEdit.Name = "chkClientsCanEdit";
			chkClientsCanEdit.Size = new System.Drawing.Size(131, 17);
			chkClientsCanEdit.TabIndex = 10;
			chkClientsCanEdit.Text = "Clients can edit Books";
			chkClientsCanEdit.UseVisualStyleBackColor = true;
			txPassword.Location = new System.Drawing.Point(24, 45);
			txPassword.Name = "txPassword";
			txPassword.Password = null;
			txPassword.Size = new System.Drawing.Size(153, 20);
			txPassword.TabIndex = 7;
			txPassword.UseSystemPasswordChar = true;
			chkRequirePassword.AutoSize = true;
			chkRequirePassword.Location = new System.Drawing.Point(6, 23);
			chkRequirePassword.Name = "chkRequirePassword";
			chkRequirePassword.Size = new System.Drawing.Size(115, 17);
			chkRequirePassword.TabIndex = 6;
			chkRequirePassword.Text = "Require Password:";
			chkRequirePassword.UseVisualStyleBackColor = true;
			txSharedName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txSharedName.Location = new System.Drawing.Point(106, 7);
			txSharedName.Name = "txSharedName";
			txSharedName.Size = new System.Drawing.Size(280, 20);
			txSharedName.TabIndex = 1;
			txSharedName.TextChanged += new System.EventHandler(txSharedName_TextChanged);
			labelSharedName.AutoSize = true;
			labelSharedName.Location = new System.Drawing.Point(3, 10);
			labelSharedName.Name = "labelSharedName";
			labelSharedName.Size = new System.Drawing.Size(75, 13);
			labelSharedName.TabIndex = 0;
			labelSharedName.Text = "Shared Name:";
			txPublicServerComment.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txPublicServerComment.Location = new System.Drawing.Point(106, 29);
			txPublicServerComment.Name = "txPublicServerComment";
			txPublicServerComment.Size = new System.Drawing.Size(280, 20);
			txPublicServerComment.TabIndex = 3;
			labelShareDescription.AutoSize = true;
			labelShareDescription.Location = new System.Drawing.Point(3, 32);
			labelShareDescription.Name = "labelShareDescription";
			labelShareDescription.Size = new System.Drawing.Size(63, 13);
			labelShareDescription.TabIndex = 2;
			labelShareDescription.Text = "Description:";
			chkShareInternet.AutoSize = true;
			chkShareInternet.Location = new System.Drawing.Point(6, 78);
			chkShareInternet.Name = "chkShareInternet";
			chkShareInternet.Size = new System.Drawing.Size(126, 17);
			chkShareInternet.TabIndex = 8;
			chkShareInternet.Text = "Share on the Internet";
			chkShareInternet.UseVisualStyleBackColor = true;
			chkPrivate.AutoSize = true;
			chkPrivate.Location = new System.Drawing.Point(25, 97);
			chkPrivate.Name = "chkPrivate";
			chkPrivate.Size = new System.Drawing.Size(59, 17);
			chkPrivate.TabIndex = 9;
			chkPrivate.Text = "Private";
			chkPrivate.UseVisualStyleBackColor = true;
			tbPageQuality.Location = new System.Drawing.Point(82, 175);
			tbPageQuality.Minimum = 15;
			tbPageQuality.Name = "tbPageQuality";
			tbPageQuality.Size = new System.Drawing.Size(95, 19);
			tbPageQuality.TabIndex = 13;
			tbPageQuality.ThumbSize = new System.Drawing.Size(8, 12);
			tbPageQuality.Value = 15;
			tbPageQuality.ValueChanged += new System.EventHandler(tbPageQuality_ValueChanged);
			labelPageQuality.AutoSize = true;
			labelPageQuality.Location = new System.Drawing.Point(3, 175);
			labelPageQuality.Name = "labelPageQuality";
			labelPageQuality.Size = new System.Drawing.Size(70, 13);
			labelPageQuality.TabIndex = 12;
			labelPageQuality.Text = "Page Quality:";
			labelThumbQuality.AutoSize = true;
			labelThumbQuality.Location = new System.Drawing.Point(3, 197);
			labelThumbQuality.Name = "labelThumbQuality";
			labelThumbQuality.Size = new System.Drawing.Size(78, 13);
			labelThumbQuality.TabIndex = 14;
			labelThumbQuality.Text = "Thumb Quality:";
			tbThumbQuality.Location = new System.Drawing.Point(82, 197);
			tbThumbQuality.Minimum = 15;
			tbThumbQuality.Name = "tbThumbQuality";
			tbThumbQuality.Size = new System.Drawing.Size(95, 19);
			tbThumbQuality.TabIndex = 15;
			tbThumbQuality.ThumbSize = new System.Drawing.Size(8, 12);
			tbThumbQuality.Value = 15;
			tbThumbQuality.ValueChanged += new System.EventHandler(tbPageQuality_ValueChanged);
			tvSharedLists.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			tvSharedLists.CheckBoxes = true;
			tvSharedLists.ImageIndex = 0;
			tvSharedLists.ImageList = ilShares;
			tvSharedLists.ItemHeight = 16;
			tvSharedLists.Location = new System.Drawing.Point(6, 90);
			tvSharedLists.Name = "tvSharedLists";
			tvSharedLists.SelectedImageIndex = 0;
			tvSharedLists.ShowLines = false;
			tvSharedLists.ShowPlusMinus = false;
			tvSharedLists.ShowRootLines = false;
			tvSharedLists.Size = new System.Drawing.Size(191, 240);
			tvSharedLists.TabIndex = 5;
			ilShares.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			ilShares.ImageSize = new System.Drawing.Size(16, 16);
			ilShares.TransparentColor = System.Drawing.Color.Transparent;
			cbShare.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			cbShare.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbShare.FormattingEnabled = true;
			cbShare.Items.AddRange(new object[3]
			{
				"Share None",
				"Share All",
				"Share Selected"
			});
			cbShare.Location = new System.Drawing.Point(6, 67);
			cbShare.Name = "cbShare";
			cbShare.Size = new System.Drawing.Size(191, 21);
			cbShare.TabIndex = 4;
			grpShareSettings.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			grpShareSettings.Controls.Add(chkRequirePassword);
			grpShareSettings.Controls.Add(txPassword);
			grpShareSettings.Controls.Add(chkClientsCanEdit);
			grpShareSettings.Controls.Add(labelThumbQuality);
			grpShareSettings.Controls.Add(chkClientsCanExport);
			grpShareSettings.Controls.Add(tbThumbQuality);
			grpShareSettings.Controls.Add(chkShareInternet);
			grpShareSettings.Controls.Add(labelPageQuality);
			grpShareSettings.Controls.Add(chkPrivate);
			grpShareSettings.Controls.Add(tbPageQuality);
			grpShareSettings.Location = new System.Drawing.Point(203, 67);
			grpShareSettings.Name = "grpShareSettings";
			grpShareSettings.Size = new System.Drawing.Size(183, 263);
			grpShareSettings.TabIndex = 16;
			grpShareSettings.TabStop = false;
			grpShareSettings.Text = "Settings";
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(grpShareSettings);
			base.Controls.Add(cbShare);
			base.Controls.Add(tvSharedLists);
			base.Controls.Add(txPublicServerComment);
			base.Controls.Add(labelShareDescription);
			base.Controls.Add(txSharedName);
			base.Controls.Add(labelSharedName);
			base.Name = "ServerEditControl";
			base.Size = new System.Drawing.Size(389, 333);
			grpShareSettings.ResumeLayout(false);
			grpShareSettings.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
