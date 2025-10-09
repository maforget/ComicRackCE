using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.Sync;
using cYo.Projects.ComicRack.Viewer.Controls;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class DeviceEditControl : UserControlEx
	{
		private class TagElement
		{
			public ComicListItem Item;

			public DeviceSyncSettings.SharedList List;
		}

		private ComicLibrary library;

		private string deviceName;

		private bool blockCheck;

		private bool blockListUpdate;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DeviceSyncSettings Settings
		{
			get
			{
				DeviceSyncSettings deviceSyncSettings = new DeviceSyncSettings
				{
					DeviceName = DeviceName,
					DeviceKey = DeviceKey,
					DefaultListSettings = DefaultListSettings
				};
				deviceSyncSettings.Lists.AddRange(from tn in tvSharedLists.AllNodes()
					where tn.Checked
					select (TagElement)tn.Tag into te
					select te.List ?? CreateDefaultSharedList(te.Item.Id));
				return deviceSyncSettings;
			}
			set
			{
				DeviceName = value.DeviceName;
				DeviceKey = value.DeviceKey;
				DefaultListSettings = new DeviceSyncSettings.SharedListSettings(value.DefaultListSettings);
				UpdateTree(value);
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string DeviceName
		{
			get
			{
				return deviceName;
			}
			set
			{
				if (!(deviceName == value))
				{
					deviceName = value;
					if (this.DeviceNameChanged != null)
					{
						this.DeviceNameChanged(this, EventArgs.Empty);
					}
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string DeviceKey
		{
			get;
			set;
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DeviceSyncSettings.SharedListSettings DefaultListSettings
		{
			get;
			set;
		}

		public bool CanPaste => Clipboard.ContainsData(DeviceSyncSettings.ClipboardFormat);

		public event EventHandler DeviceNameChanged;

		public DeviceEditControl()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, value: true);
			InitializeComponent();
			TR tr = TR.Load("DevicesEditDialog");
			LocalizeUtility.Localize(tr, this);
			LocalizeUtility.Localize(tr, cbLimitType);
			LocalizeUtility.Localize(tr, cbLimitSort);
			ilShares.ImageSize = ilShares.ImageSize.ScaleDpi();
			ilShares.Images.Add("Library", Resources.Library);
			ilShares.Images.Add("Folder", Resources.SearchFolder);
			ilShares.Images.Add("Search", Resources.SearchDocument);
			ilShares.Images.Add("List", Resources.List);
			ilShares.Images.Add("TempFolder", Resources.TempFolder);
			new LibraryTreeSkin
			{
				TreeView = tvSharedLists,
				GetNodeItem = (TreeNode n) => ((TagElement)n.Tag).Item,
				DisableDeviceIcon = true
			};
			txLimit.EnableOnlyNumberKeys();
			SpinButton.AddUpDown(txLimit, 1, 1);
			library = Program.Database;
			if (library != null)
			{
				library.ComicListCachesUpdated += Database_ComicListCachesUpdated;
				library.ComicListsChanged += library_ComicListsChanged;
			}
		}

		private void Database_ComicListCachesUpdated(object sender, EventArgs e)
		{
			tvSharedLists.Invalidate();
		}

		private void library_ComicListsChanged(object sender, ComicListItemChangedEventArgs e)
		{
			tvSharedLists.Invalidate();
		}

		public void SelectList(Guid id)
		{
			tvSharedLists.SelectedNode = tvSharedLists.Nodes.Find((TreeNode tn) => ((TagElement)tn.Tag).Item.Id == id);
		}

		public void CopyShareSettings()
		{
			Clipboard.SetData(DeviceSyncSettings.ClipboardFormat, Settings);
		}

		public void PasteSharedSettings()
		{
			try
			{
				DeviceSyncSettings deviceSyncSettings = Clipboard.GetData(DeviceSyncSettings.ClipboardFormat) as DeviceSyncSettings;
				if (deviceSyncSettings != null)
				{
					UpdateTree(deviceSyncSettings);
					SetEditor(tvSharedLists.SelectedNode);
					SetButtonStates();
				}
			}
			catch
			{
			}
		}

		private void btSelectAll_Click(object sender, EventArgs e)
		{
			blockCheck = true;
			tvSharedLists.Nodes.All().ForEach(delegate(TreeNode tn)
			{
				tn.Checked = true;
			});
			blockCheck = false;
		}

		private void btSelectNone_Click(object sender, EventArgs e)
		{
			blockCheck = true;
			tvSharedLists.Nodes.All().ForEach(delegate(TreeNode tn)
			{
				tn.Checked = false;
			});
			blockCheck = false;
		}

		private void btSelectList_Click(object sender, EventArgs e)
		{
			if (tvSharedLists.SelectedNode != null)
			{
				blockCheck = true;
				tvSharedLists.SelectedNode.Checked = true;
				tvSharedLists.SelectedNode.Nodes.All().ForEach(delegate(TreeNode tn)
				{
					tn.Checked = true;
				});
				blockCheck = false;
			}
		}

		private void btUnselectList_Click(object sender, EventArgs e)
		{
			if (tvSharedLists.SelectedNode != null)
			{
				blockCheck = true;
				tvSharedLists.SelectedNode.Checked = false;
				tvSharedLists.SelectedNode.Nodes.All().ForEach(delegate(TreeNode tn)
				{
					tn.Checked = false;
				});
				blockCheck = false;
			}
		}

		private void tvSharedLists_AfterSelect(object sender, TreeViewEventArgs e)
		{
			SetEditor(e.Node);
			SetButtonStates();
		}

		private void tvSharedLists_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if (!blockCheck)
			{
				tvSharedLists.SelectedNode = e.Node;
			}
			if (e.Node == tvSharedLists.SelectedNode)
			{
				SetEditor(e.Node);
			}
			SetButtonStates();
		}

		private void chkOnlyShowSelected_CheckedChanged(object sender, EventArgs e)
		{
			DeviceSyncSettings settings = Settings;
			ComicListItem cli = GetSelectedComicListItem();
			UpdateTree(settings, clear: true);
			TreeNode treeNode = ((cli == null) ? null : tvSharedLists.Nodes.Find((TreeNode n) => GetSharedList(n) != null && GetComicListItem(n).Id == cli.Id));
			tvSharedLists.SelectedNode = treeNode;
			SetEditor(treeNode);
		}

		private void chkOptimizeSize_CheckedChanged(object sender, EventArgs e)
		{
			SetSelectedListProperty(delegate(DeviceSyncSettings.SharedListSettings l)
			{
				l.OptimizePortable = chkOptimizeSize.Checked;
			});
		}

		private void chkOnlyUnread_CheckedChanged(object sender, EventArgs e)
		{
			SetSelectedListProperty(delegate(DeviceSyncSettings.SharedListSettings l)
			{
				bool onlyUnread = (chkKeepLastRead.Enabled = chkOnlyUnread.Checked);
				l.OnlyUnread = onlyUnread;
			});
		}

		private void chkKeepLastRead_CheckedChanged(object sender, EventArgs e)
		{
			SetSelectedListProperty(delegate(DeviceSyncSettings.SharedListSettings l)
			{
				l.KeepLastRead = chkKeepLastRead.Checked;
			});
		}

		private void chkOnlyChecked_CheckedChanged(object sender, EventArgs e)
		{
			SetSelectedListProperty(delegate(DeviceSyncSettings.SharedListSettings l)
			{
				l.OnlyChecked = chkOnlyChecked.Checked;
			});
		}

		private void chkLimit_CheckedChanged(object sender, EventArgs e)
		{
			TextBox textBox = txLimit;
			bool enabled = (cbLimitType.Enabled = chkLimit.Checked);
			textBox.Enabled = enabled;
			SetSelectedListProperty(delegate(DeviceSyncSettings.SharedListSettings l)
			{
				l.Limit = chkLimit.Checked;
			});
		}

		private void chkSortBy_CheckedChanged(object sender, EventArgs e)
		{
			cbLimitSort.Enabled = chkSortBy.Checked;
			SetSelectedListProperty(delegate(DeviceSyncSettings.SharedListSettings l)
			{
				l.Sort = chkSortBy.Checked;
			});
		}

		private void txLimit_TextChanged(object sender, EventArgs e)
		{
			TagElement tagElement = tvSharedLists.SelectedNode.Tag as TagElement;
			if (int.TryParse(txLimit.Text, out var i))
			{
				SetSelectedListProperty(delegate(DeviceSyncSettings.SharedListSettings l)
				{
					l.LimitValue = i.Clamp(0, 10000);
				});
			}
		}

		private void cbLimitType_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetSelectedListProperty(delegate(DeviceSyncSettings.SharedListSettings l)
			{
				l.LimitValueType = (DeviceSyncSettings.LimitType)cbLimitType.SelectedIndex;
			});
		}

		private void cbLimitSort_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetSelectedListProperty(delegate(DeviceSyncSettings.SharedListSettings l)
			{
				l.ListSortType = (DeviceSyncSettings.ListSort)cbLimitSort.SelectedIndex;
			});
		}

		private void UpdateTree(DeviceSyncSettings settings, bool clear = false)
		{
			blockCheck = true;
			tvSharedLists.BeginUpdate();
			if (clear)
			{
				tvSharedLists.Nodes.Clear();
				tvSharedLists.Sorted = chkOnlyShowSelected.Checked;
			}
			FillListTree(tvSharedLists.Nodes, settings, Program.Database.ComicLists, !chkOnlyShowSelected.Checked, chkOnlyShowSelected.Checked);
			tvSharedLists.EndUpdate();
			blockCheck = false;
			SetButtonStates();
			if (library != null)
			{
				library.CommitComicListCacheChanges();
			}
		}

		private void SetButtonStates()
		{
			IEnumerable<TreeNode> source = tvSharedLists.AllNodes();
			IEnumerable<TreeNode> source2 = ((tvSharedLists.SelectedNode == null) ? Enumerable.Empty<TreeNode>() : tvSharedLists.SelectedNode.Nodes.All().AddFirst(tvSharedLists.SelectedNode));
			btSelectAll.Enabled = source.Any((TreeNode n) => !n.Checked);
			btSelectNone.Enabled = source.Any((TreeNode n) => n.Checked);
			btSelectList.Enabled = source2.Any((TreeNode n) => !n.Checked);
			btDeselectList.Enabled = source2.Any((TreeNode n) => n.Checked);
			chkOnlyShowSelected.Enabled = chkOnlyShowSelected.Checked || chkOnlyChecked.Checked || btSelectNone.Enabled;
			chkOnlyChecked.Visible = chkOnlyChecked.Checked || tvSharedLists.Height > 12;
		}

		private void SetEditor(TreeNode node)
		{
			bool flag = node != null;
			grpListOptions.Enabled = flag && node.Checked;
			grpListOptions.Text = (flag ? node.Text : string.Empty);
			if (flag)
			{
				bool @checked = node.Checked;
				ComicListItem comicListItem = GetComicListItem(node);
				DeviceSyncSettings.SharedList sharedList = GetSharedList(node);
				if (sharedList == null && @checked)
				{
					sharedList = CreateDefaultSharedList(comicListItem.Id);
					SetSharedList(node, sharedList);
				}
				if (sharedList == null)
				{
					sharedList = CreateDefaultSharedList(Guid.Empty);
				}
				blockListUpdate = true;
				chkOnlyUnread.Checked = sharedList.OnlyUnread;
				chkKeepLastRead.Checked = sharedList.KeepLastRead;
				chkOptimizeSize.Checked = sharedList.OptimizePortable;
				chkOnlyChecked.Checked = sharedList.OnlyChecked;
				chkLimit.Checked = sharedList.Limit;
				chkSortBy.Checked = sharedList.Sort;
				txLimit.Text = sharedList.LimitValue.ToString();
				cbLimitType.SelectedIndex = (int)sharedList.LimitValueType;
				cbLimitSort.SelectedIndex = (int)sharedList.ListSortType;
				blockListUpdate = false;
			}
		}

		private static int FillListTree(TreeNodeCollection tnc, DeviceSyncSettings settings, IEnumerable<ComicListItem> clic, bool fillAll, bool flat)
		{
			int num = 0;
			foreach (ComicListItem cli in clic)
			{
				num++;
				DeviceSyncSettings.SharedList sharedList = settings.Lists.FirstOrDefault((DeviceSyncSettings.SharedList sl) => sl.ListId == cli.Id);
				bool flag = sharedList != null;
				TreeNode treeNode = tnc.Find((TreeNode n) => ((TagElement)n.Tag).Item == cli, all: false);
				if (flag || fillAll)
				{
					if (treeNode == null)
					{
						treeNode = tnc.Add(cli.Name);
					}
					treeNode.ImageKey = cli.ImageKey;
					treeNode.SelectedImageKey = cli.ImageKey;
					treeNode.Tag = new TagElement
					{
						Item = cli,
						List = (flag ? new DeviceSyncSettings.SharedList(sharedList) : null)
					};
				}
				ComicListItemFolder comicListItemFolder = cli as ComicListItemFolder;
				if (comicListItemFolder != null)
				{
					num += FillListTree(flat ? tnc : treeNode.Nodes, settings, comicListItemFolder.Items, fillAll, flat);
				}
				if (treeNode != null)
				{
					treeNode.Checked = flag;
					treeNode.Expand();
				}
			}
			return num;
		}

		private static DeviceSyncSettings.SharedList GetSharedList(TreeNode node)
		{
			if (node == null)
			{
				return null;
			}
			return (node.Tag as TagElement)?.List;
		}

		private static void SetSharedList(TreeNode node, DeviceSyncSettings.SharedList list)
		{
			if (node != null)
			{
				TagElement tagElement = node.Tag as TagElement;
				if (tagElement != null)
				{
					tagElement.List = list;
				}
			}
		}

		private static ComicListItem GetComicListItem(TreeNode node)
		{
			if (node == null)
			{
				return null;
			}
			return (node.Tag as TagElement)?.Item;
		}

		private ComicListItem GetSelectedComicListItem()
		{
			return GetComicListItem(tvSharedLists.SelectedNode);
		}

		private DeviceSyncSettings.SharedList GetSelectedSharedList()
		{
			return GetSharedList(tvSharedLists.SelectedNode);
		}

		private void SetListProperty(TreeNode node, Action<DeviceSyncSettings.SharedListSettings> set)
		{
			if (!blockListUpdate)
			{
				DeviceSyncSettings.SharedList sharedList = GetSharedList(node);
				if (sharedList != null)
				{
					set(sharedList);
				}
				set(Settings.DefaultListSettings);
			}
		}

		private void SetSelectedListProperty(Action<DeviceSyncSettings.SharedListSettings> set)
		{
			SetListProperty(tvSharedLists.SelectedNode, set);
		}

		private DeviceSyncSettings.SharedList CreateDefaultSharedList(Guid id)
		{
			return new DeviceSyncSettings.SharedList(id, DefaultListSettings);
		}
	}
}
