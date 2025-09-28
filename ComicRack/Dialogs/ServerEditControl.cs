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
	public partial class ServerEditControl : UserControlEx
	{
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
            this.chkShareInternet.Enabled = false;
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

		private void IdleProcess_Idle(object sender, EventArgs e)
		{
			tvSharedLists.Enabled = cbShare.SelectedIndex == 2;
			txPassword.Enabled = chkRequirePassword.Checked;
			chkPrivate.Enabled = chkShareInternet.Enabled && chkShareInternet.Checked;
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
	}
}
