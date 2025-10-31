using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine.IO.Network;
using cYo.Projects.ComicRack.Viewer.Config;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class OpenRemoteDialog : FormEx
	{
		private bool showPublic;

		private List<ShareInformation> servers;

		public bool ShowPublic
		{
			get
			{
				return showPublic;
			}
			set
			{
				showPublic = value;
				if (showPublic)
				{
					FillServers();
				}
				panelList.Visible = showPublic;
				btPublic.Text = (showPublic ? TR.Load(base.Name)["RefreshList", "Refresh List"] : TR.Load(base.Name)["ShowList", "Show List"]);
			}
		}

		private RemoteShareItem CurrentItem
		{
			get;
			set;
		}

		public OpenRemoteDialog()
		{
			InitializeComponent();
			imageList.Images.Add(Resources.RemoteDatabase);
			imageList.Images.Add(Resources.RemoteDatabaseLocked);
			lvServers.Columns.ScaleDpi();
			LocalizeUtility.Localize(this, null);
			this.RestorePosition();
			txFilter.Text = Program.Settings.OpenRemoteFilter;
			txPassword.Password = Program.Settings.OpenRemotePassword;
		}

		private void lvServers_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateServer();
		}

		private void lvServers_ItemActivate(object sender, EventArgs e)
		{
			UpdateServer();
			if (!string.IsNullOrEmpty(cbServer.Text.Trim()))
			{
				base.DialogResult = DialogResult.OK;
				Hide();
			}
		}

		private void btPublic_Click(object sender, EventArgs e)
		{
			if (!ShowPublic)
			{
				ShowPublic = true;
				return;
			}
			servers = null;
			FillServers();
		}

		private void txFilter_TextChanged(object sender, EventArgs e)
		{
			if (!ShowPublic)
			{
				ShowPublic = true;
			}
			else
			{
				FillServers();
			}
		}

		private void cbServer_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbServer.SelectedIndex == -1)
			{
				CurrentItem = null;
			}
			else
			{
				CurrentItem = cbServer.SelectedItem as RemoteShareItem;
			}
		}

		private void cbServer_TextUpdate(object sender, EventArgs e)
		{
			RemoteShareItem remoteShareItem = cbServer.Items.OfType<RemoteShareItem>().FirstOrDefault((RemoteShareItem n) => n.Name == cbServer.Text);
			CurrentItem = remoteShareItem ?? new RemoteShareItem(cbServer.Text);
		}

		private void UpdateServer()
		{
			if (lvServers.SelectedItems.Count == 0)
			{
				return;
			}
			ListViewItem listViewItem = lvServers.SelectedItems[0];
			if (listViewItem.Tag == null)
			{
				cbServer.Text = string.Empty;
				CurrentItem = null;
				return;
			}
			CurrentItem = new RemoteShareItem(listViewItem.Tag as ShareInformation);
			if (!cbServer.Items.Contains(CurrentItem))
			{
				cbServer.Items.Insert(0, CurrentItem);
			}
			cbServer.SelectedItem = CurrentItem;
		}

		private void FillServers()
		{
			if (!showPublic)
			{
				return;
			}
			string filter = txFilter.Text.Trim();
			string password = txPassword.Password;
			if (servers == null)
			{
				using (new WaitCursor(this))
				{
					labelFailedServerList.Visible = false;
					servers = new List<ShareInformation>();
					using (ItemMonitor.Lock(Program.NetworkManager.LocalShares))
					{
						servers.AddRange(Program.NetworkManager.LocalShares.Values);
					}
					try
					{
						servers.AddRange((from s in ComicLibraryServer.GetPublicServers(ServerOptions.None, password)
							orderby s.Name
							select s).ToArray());
					}
					catch
					{
						labelFailedServerList.Visible = servers.Count == 0;
					}
				}
			}
			lvServers.Items.Clear();
			foreach (ShareInformation item in servers.Where((ShareInformation s) => string.IsNullOrEmpty(filter) || s.Name.Contains(filter, StringComparison.OrdinalIgnoreCase) || s.Comment.Contains(filter, StringComparison.OrdinalIgnoreCase)))
			{
				ListViewItem listViewItem = lvServers.Items.Add(item.Name, item.IsProtected ? 1 : 0);
				listViewItem.SubItems.Add(item.Comment);
				listViewItem.SubItems.Add(item.IsEditable ? TR.Default["Yes"] : TR.Default["No"]);
				listViewItem.SubItems.Add(item.IsExportable ? TR.Default["Yes"] : TR.Default["No"]);
				listViewItem.Group = lvServers.Groups[item.IsLocal ? "groupLocal" : "groupInternet"];
				listViewItem.Tag = item;
				if (Program.NetworkManager.IsOwnServer(item.Uri))
				{
					listViewItem.ForeColor = SystemColors.GrayText;
					listViewItem.Tag = null;
				}
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			Program.Settings.OpenRemoteFilter = txFilter.Text;
			Program.Settings.OpenRemotePassword = txPassword.Password;
			base.OnClosing(e);
		}

		public static RemoteShareItem GetShare(IWin32Window parent, RemoteShareItem share, IEnumerable<RemoteShareItem> list, bool showPublic)
		{
			using (OpenRemoteDialog openRemoteDialog = new OpenRemoteDialog())
			{
				openRemoteDialog.cbServer.Items.AddRange(list.ToArray());
				if (share != null)
				{
					openRemoteDialog.cbServer.Text = share.Name;
					openRemoteDialog.CurrentItem = share;
				}
				openRemoteDialog.ShowPublic = showPublic;
				return (openRemoteDialog.ShowDialog(parent) == DialogResult.OK) ? openRemoteDialog.CurrentItem : null;
			}
		}
	}
}
