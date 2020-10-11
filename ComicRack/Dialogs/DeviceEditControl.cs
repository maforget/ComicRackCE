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
	public class DeviceEditControl : UserControl
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

		private IContainer components;

		private TreeViewEx tvSharedLists;

		private CheckBox chkOptimizeSize;

		private ImageList ilShares;

		private ToolTip toolTip;

		private Button btSelectAll;

		private Button btSelectNone;

		private CheckBox chkOnlyUnread;

		private GroupBox grpListOptions;

		private CheckBox chkOnlyChecked;

		private Button btSelectList;

		private Button btDeselectList;

		private ComboBox cbLimitSort;

		private TextBox txLimit;

		private CheckBox chkLimit;

		private ComboBox cbLimitType;

		private CheckBox chkKeepLastRead;

		private CheckBox chkSortBy;

		private WrappingCheckBox chkOnlyShowSelected;

		private Panel panel1;

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

		public bool CanPaste => Clipboard.ContainsData("DeviceSyncSettings");

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

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				components.SafeDispose();
				if (library != null)
				{
					library.ComicListCachesUpdated -= Database_ComicListCachesUpdated;
					library.ComicListsChanged -= library_ComicListsChanged;
				}
				library = null;
			}
			base.Dispose(disposing);
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
			Clipboard.SetData("DeviceSyncSettings", Settings);
		}

		public void PasteSharedSettings()
		{
			try
			{
				DeviceSyncSettings deviceSyncSettings = Clipboard.GetData("DeviceSyncSettings") as DeviceSyncSettings;
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

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			ilShares = new System.Windows.Forms.ImageList(components);
			chkOptimizeSize = new System.Windows.Forms.CheckBox();
			toolTip = new System.Windows.Forms.ToolTip(components);
			btSelectAll = new System.Windows.Forms.Button();
			btSelectNone = new System.Windows.Forms.Button();
			chkOnlyUnread = new System.Windows.Forms.CheckBox();
			grpListOptions = new System.Windows.Forms.GroupBox();
			chkSortBy = new System.Windows.Forms.CheckBox();
			chkKeepLastRead = new System.Windows.Forms.CheckBox();
			cbLimitSort = new System.Windows.Forms.ComboBox();
			txLimit = new System.Windows.Forms.TextBox();
			chkLimit = new System.Windows.Forms.CheckBox();
			cbLimitType = new System.Windows.Forms.ComboBox();
			chkOnlyChecked = new System.Windows.Forms.CheckBox();
			tvSharedLists = new cYo.Common.Windows.Forms.TreeViewEx();
			btSelectList = new System.Windows.Forms.Button();
			btDeselectList = new System.Windows.Forms.Button();
			chkOnlyShowSelected = new cYo.Common.Windows.Forms.WrappingCheckBox();
			panel1 = new System.Windows.Forms.Panel();
			grpListOptions.SuspendLayout();
			panel1.SuspendLayout();
			SuspendLayout();
			ilShares.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			ilShares.ImageSize = new System.Drawing.Size(16, 16);
			ilShares.TransparentColor = System.Drawing.Color.Transparent;
			chkOptimizeSize.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkOptimizeSize.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			chkOptimizeSize.Location = new System.Drawing.Point(398, 17);
			chkOptimizeSize.Name = "chkOptimizeSize";
			chkOptimizeSize.Size = new System.Drawing.Size(112, 20);
			chkOptimizeSize.TabIndex = 8;
			chkOptimizeSize.Text = "Optimize Size";
			chkOptimizeSize.UseVisualStyleBackColor = true;
			chkOptimizeSize.CheckedChanged += new System.EventHandler(chkOptimizeSize_CheckedChanged);
			btSelectAll.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			btSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btSelectAll.Location = new System.Drawing.Point(3, 3);
			btSelectAll.Name = "btSelectAll";
			btSelectAll.Size = new System.Drawing.Size(113, 24);
			btSelectAll.TabIndex = 1;
			btSelectAll.Text = "Select All";
			btSelectAll.Click += new System.EventHandler(btSelectAll_Click);
			btSelectNone.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			btSelectNone.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btSelectNone.Location = new System.Drawing.Point(3, 33);
			btSelectNone.Name = "btSelectNone";
			btSelectNone.Size = new System.Drawing.Size(113, 24);
			btSelectNone.TabIndex = 2;
			btSelectNone.Text = "Select None";
			btSelectNone.Click += new System.EventHandler(btSelectNone_Click);
			chkOnlyUnread.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			chkOnlyUnread.Location = new System.Drawing.Point(15, 68);
			chkOnlyUnread.Name = "chkOnlyUnread";
			chkOnlyUnread.Size = new System.Drawing.Size(102, 17);
			chkOnlyUnread.TabIndex = 5;
			chkOnlyUnread.Text = "Only Unread";
			chkOnlyUnread.UseVisualStyleBackColor = true;
			chkOnlyUnread.CheckedChanged += new System.EventHandler(chkOnlyUnread_CheckedChanged);
			grpListOptions.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			grpListOptions.Controls.Add(chkSortBy);
			grpListOptions.Controls.Add(chkKeepLastRead);
			grpListOptions.Controls.Add(cbLimitSort);
			grpListOptions.Controls.Add(txLimit);
			grpListOptions.Controls.Add(chkLimit);
			grpListOptions.Controls.Add(cbLimitType);
			grpListOptions.Controls.Add(chkOnlyChecked);
			grpListOptions.Controls.Add(chkOptimizeSize);
			grpListOptions.Controls.Add(chkOnlyUnread);
			grpListOptions.Enabled = false;
			grpListOptions.Location = new System.Drawing.Point(0, 301);
			grpListOptions.Name = "grpListOptions";
			grpListOptions.Size = new System.Drawing.Size(516, 122);
			grpListOptions.TabIndex = 6;
			grpListOptions.TabStop = false;
			chkSortBy.Location = new System.Drawing.Point(15, 19);
			chkSortBy.Name = "chkSortBy";
			chkSortBy.Size = new System.Drawing.Size(102, 18);
			chkSortBy.TabIndex = 0;
			chkSortBy.Text = "Sort by";
			chkSortBy.UseVisualStyleBackColor = true;
			chkSortBy.CheckedChanged += new System.EventHandler(chkSortBy_CheckedChanged);
			chkKeepLastRead.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			chkKeepLastRead.Location = new System.Drawing.Point(126, 68);
			chkKeepLastRead.Name = "chkKeepLastRead";
			chkKeepLastRead.Size = new System.Drawing.Size(144, 17);
			chkKeepLastRead.TabIndex = 6;
			chkKeepLastRead.Text = "But keep last read";
			chkKeepLastRead.UseVisualStyleBackColor = true;
			chkKeepLastRead.CheckedChanged += new System.EventHandler(chkKeepLastRead_CheckedChanged);
			cbLimitSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbLimitSort.Enabled = false;
			cbLimitSort.FormattingEnabled = true;
			cbLimitSort.Items.AddRange(new object[6]
			{
				"Random",
				"Series",
				"Alternate Series",
				"Published",
				"Added",
				"Story Arc"
			});
			cbLimitSort.Location = new System.Drawing.Point(126, 18);
			cbLimitSort.Name = "cbLimitSort";
			cbLimitSort.Size = new System.Drawing.Size(144, 21);
			cbLimitSort.TabIndex = 1;
			cbLimitSort.SelectedIndexChanged += new System.EventHandler(cbLimitSort_SelectedIndexChanged);
			txLimit.Enabled = false;
			txLimit.Location = new System.Drawing.Point(126, 45);
			txLimit.Name = "txLimit";
			txLimit.Size = new System.Drawing.Size(61, 20);
			txLimit.TabIndex = 3;
			txLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			txLimit.TextChanged += new System.EventHandler(txLimit_TextChanged);
			chkLimit.Location = new System.Drawing.Point(15, 44);
			chkLimit.Name = "chkLimit";
			chkLimit.Size = new System.Drawing.Size(102, 18);
			chkLimit.TabIndex = 2;
			chkLimit.Text = "Limit to ";
			chkLimit.UseVisualStyleBackColor = true;
			chkLimit.CheckedChanged += new System.EventHandler(chkLimit_CheckedChanged);
			cbLimitType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbLimitType.Enabled = false;
			cbLimitType.FormattingEnabled = true;
			cbLimitType.Items.AddRange(new object[3]
			{
				"Books",
				"MB",
				"GB"
			});
			cbLimitType.Location = new System.Drawing.Point(193, 45);
			cbLimitType.Name = "cbLimitType";
			cbLimitType.Size = new System.Drawing.Size(77, 21);
			cbLimitType.TabIndex = 4;
			cbLimitType.SelectedIndexChanged += new System.EventHandler(cbLimitType_SelectedIndexChanged);
			chkOnlyChecked.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			chkOnlyChecked.Location = new System.Drawing.Point(15, 91);
			chkOnlyChecked.Name = "chkOnlyChecked";
			chkOnlyChecked.Size = new System.Drawing.Size(102, 17);
			chkOnlyChecked.TabIndex = 7;
			chkOnlyChecked.Text = "Only Checked";
			chkOnlyChecked.UseVisualStyleBackColor = true;
			chkOnlyChecked.CheckedChanged += new System.EventHandler(chkOnlyChecked_CheckedChanged);
			tvSharedLists.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			tvSharedLists.CheckBoxes = true;
			tvSharedLists.FullRowSelect = true;
			tvSharedLists.HideSelection = false;
			tvSharedLists.ImageIndex = 0;
			tvSharedLists.ImageList = ilShares;
			tvSharedLists.ItemHeight = 16;
			tvSharedLists.Location = new System.Drawing.Point(0, 3);
			tvSharedLists.Name = "tvSharedLists";
			tvSharedLists.SelectedImageIndex = 0;
			tvSharedLists.ShowLines = false;
			tvSharedLists.ShowPlusMinus = false;
			tvSharedLists.ShowRootLines = false;
			tvSharedLists.Size = new System.Drawing.Size(391, 292);
			tvSharedLists.TabIndex = 0;
			tvSharedLists.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(tvSharedLists_AfterCheck);
			tvSharedLists.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(tvSharedLists_AfterSelect);
			btSelectList.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			btSelectList.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btSelectList.Location = new System.Drawing.Point(3, 72);
			btSelectList.Name = "btSelectList";
			btSelectList.Size = new System.Drawing.Size(113, 24);
			btSelectList.TabIndex = 3;
			btSelectList.Text = "Select List";
			btSelectList.Click += new System.EventHandler(btSelectList_Click);
			btDeselectList.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			btDeselectList.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btDeselectList.Location = new System.Drawing.Point(3, 102);
			btDeselectList.Name = "btDeselectList";
			btDeselectList.Size = new System.Drawing.Size(113, 24);
			btDeselectList.TabIndex = 4;
			btDeselectList.Text = "Deselect List";
			btDeselectList.Click += new System.EventHandler(btUnselectList_Click);
			chkOnlyShowSelected.Appearance = System.Windows.Forms.Appearance.Button;
			chkOnlyShowSelected.AutoSize = true;
			chkOnlyShowSelected.Dock = System.Windows.Forms.DockStyle.Bottom;
			chkOnlyShowSelected.Location = new System.Drawing.Point(0, 269);
			chkOnlyShowSelected.Name = "chkOnlyShowSelected";
			chkOnlyShowSelected.Size = new System.Drawing.Size(119, 23);
			chkOnlyShowSelected.TabIndex = 7;
			chkOnlyShowSelected.Text = "Only show selected";
			chkOnlyShowSelected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			chkOnlyShowSelected.UseVisualStyleBackColor = true;
			chkOnlyShowSelected.CheckedChanged += new System.EventHandler(chkOnlyShowSelected_CheckedChanged);
			panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			panel1.Controls.Add(btSelectAll);
			panel1.Controls.Add(chkOnlyShowSelected);
			panel1.Controls.Add(btSelectNone);
			panel1.Controls.Add(btDeselectList);
			panel1.Controls.Add(btSelectList);
			panel1.Location = new System.Drawing.Point(397, 3);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(119, 292);
			panel1.TabIndex = 8;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.Transparent;
			base.Controls.Add(panel1);
			base.Controls.Add(grpListOptions);
			base.Controls.Add(tvSharedLists);
			MinimumSize = new System.Drawing.Size(410, 320);
			base.Name = "DeviceEditControl";
			base.Size = new System.Drawing.Size(519, 426);
			grpListOptions.ResumeLayout(false);
			grpListOptions.PerformLayout();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			ResumeLayout(false);
		}
	}
}
