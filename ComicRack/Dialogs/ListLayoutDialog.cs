using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common;
using cYo.Common.Localize;
using cYo.Common.Win32;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.Drawing;
using cYo.Projects.ComicRack.Viewer.Controls;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class ListLayoutDialog : FormEx
	{
		public class TileTextItem
		{
			public int Value
			{
				get;
				set;
			}

			public string Text
			{
				get;
				set;
			}

			public override string ToString()
			{
				return Text;
			}
		}

		private class CaptionData
		{
			public int Id
			{
				get;
				set;
			}

			public string Text
			{
				get;
				set;
			}

			public CaptionData(int id, string text)
			{
				Id = id;
				Text = text;
			}

			public override string ToString()
			{
				return Text;
			}
		}

		private Action<DisplayListConfig> apply;

		public DisplayListConfig DisplayListConfig
		{
			get;
			set;
		}

		public ItemViewConfig Config
		{
			get
			{
				return DisplayListConfig.View;
			}
			set
			{
				DisplayListConfig.View = value;
			}
		}

		public ListLayoutDialog()
		{
			LocalizeUtility.UpdateRightToLeft(this);
			InitializeComponent();
			LocalizeUtility.Localize(this, null);
			lvColumns.Columns.ScaleDpi();
			this.RestorePosition();
			IdleProcess.Idle += Application_Idle;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			string text = string.Format("<{0}>", TR.Default["None"]);
			foreach (ItemViewColumnInfo column in Config.Columns)
			{
				AddItem(column);
				if (DisplayListConfig.Thumbnail != null)
				{
					cbFirstLine.Items.Add(new CaptionData(column.Id, column.Name));
					cbSecondLine.Items.Add(new CaptionData(column.Id, column.Name));
					cbThirdLine.Items.Add(new CaptionData(column.Id, column.Name));
				}
			}
			if (DisplayListConfig.Thumbnail == null)
			{
				tab.TabPages.Remove(tabThumbnails);
				tab.TabPages.Remove(tabTiles);
				return;
			}
			cbFirstLine.Items.Add(new CaptionData(-1, text));
			cbSecondLine.Items.Add(new CaptionData(-1, text));
			cbThirdLine.Items.Add(new CaptionData(-1, text));
			SelectCaption(cbFirstLine, (DisplayListConfig.Thumbnail.CaptionIds.Count > 0) ? DisplayListConfig.Thumbnail.CaptionIds[0] : (-1));
			SelectCaption(cbSecondLine, (DisplayListConfig.Thumbnail.CaptionIds.Count > 1) ? DisplayListConfig.Thumbnail.CaptionIds[1] : (-1));
			SelectCaption(cbThirdLine, (DisplayListConfig.Thumbnail.CaptionIds.Count > 2) ? DisplayListConfig.Thumbnail.CaptionIds[2] : (-1));
			chkHideCaption.Checked = DisplayListConfig.Thumbnail.HideCaptions;
			foreach (int bitValue in BitUtility.GetBitValues(4194303))
			{
				lbTileItems.Items.Add(new TileTextItem
				{
					Value = bitValue,
					Text = LocalizeUtility.LocalizeEnum(typeof(ComicTextElements), bitValue)
				});
			}
			SetTileTextElements(DisplayListConfig.Thumbnail.TextElements);
		}

		private void Application_Idle(object sender, EventArgs e)
		{
			btMoveUp.Enabled = SelectedColumnIndex() >= 1;
			btMoveDown.Enabled = SelectedColumnIndex() >= 0 && SelectedColumnIndex() <= lvColumns.Items.Count - 2;
		}

		private void btMoveUp_Click(object sender, EventArgs e)
		{
			int num = SelectedColumnIndex();
			if (num > 0)
			{
				lvColumns.BeginUpdate();
				ListViewItem listViewItem = lvColumns.SelectedItems[0];
				try
				{
					lvColumns.Items.Remove(listViewItem);
					lvColumns.Items.Insert(--num, listViewItem);
					listViewItem.Selected = true;
					listViewItem.EnsureVisible();
				}
				finally
				{
					lvColumns.EndUpdate();
				}
			}
		}

		private void btMoveDown_Click(object sender, EventArgs e)
		{
			int num = SelectedColumnIndex();
			if (num >= 0 && num < lvColumns.Items.Count - 1)
			{
				lvColumns.BeginUpdate();
				ListViewItem listViewItem = lvColumns.SelectedItems[0];
				try
				{
					lvColumns.Items.Remove(listViewItem);
					lvColumns.Items.Insert(++num, listViewItem);
					listViewItem.Selected = true;
					listViewItem.EnsureVisible();
				}
				finally
				{
					lvColumns.EndUpdate();
				}
			}
		}

		private void btShowAll_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem item in lvColumns.Items)
			{
				item.Checked = true;
			}
		}

		private void btHideAll_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem item in lvColumns.Items)
			{
				item.Checked = false;
			}
		}

		private void btDefaultTile_Click(object sender, EventArgs e)
		{
			SetTileTextElements(ComicTextElements.DefaultFileComic);
		}

		private void btApply_Click(object sender, EventArgs e)
		{
			Apply();
			if (apply != null)
			{
				apply(DisplayListConfig);
			}
		}

		private static int GetCaptionId(ComboBox cb)
		{
			return (cb.SelectedItem as CaptionData)?.Id ?? (-1);
		}

		private void Apply()
		{
			Config.Columns.Clear();
			foreach (ListViewItem item in lvColumns.Items)
			{
				ItemViewColumnInfo itemViewColumnInfo = (ItemViewColumnInfo)item.Tag;
				itemViewColumnInfo.Visible = item.Checked;
				Config.Columns.Add(itemViewColumnInfo);
			}
			DisplayListConfig.Thumbnail.HideCaptions = chkHideCaption.Checked;
			DisplayListConfig.Thumbnail.CaptionIds.Clear();
			if (GetCaptionId(cbFirstLine) != -1)
			{
				DisplayListConfig.Thumbnail.CaptionIds.Add(GetCaptionId(cbFirstLine));
			}
			if (GetCaptionId(cbSecondLine) != -1)
			{
				DisplayListConfig.Thumbnail.CaptionIds.Add(GetCaptionId(cbSecondLine));
			}
			if (GetCaptionId(cbThirdLine) != -1)
			{
				DisplayListConfig.Thumbnail.CaptionIds.Add(GetCaptionId(cbThirdLine));
			}
			DisplayListConfig.Thumbnail.TextElements = (ComicTextElements)(from tti in lbTileItems.Items.OfType<TileTextItem>()
				where lbTileItems.GetItemChecked(lbTileItems.Items.IndexOf(tti))
				select tti).Sum((TileTextItem tti) => tti.Value);
		}

		private void AddItem(ItemViewColumnInfo ci)
		{
			ListViewItem listViewItem = lvColumns.Items.Add(ci.Name);
			listViewItem.Checked = ci.Visible;
			listViewItem.Tag = ci;
			ComicListField comicListField = ci.Tag as ComicListField;
			if (comicListField != null)
			{
				listViewItem.SubItems.Add(comicListField.Description);
			}
		}

		private int SelectedColumnIndex()
		{
			if (lvColumns.SelectedIndices.Count <= 0)
			{
				return -1;
			}
			return lvColumns.SelectedIndices[0];
		}

		private void SetTileTextElements(ComicTextElements texts)
		{
			for (int i = 0; i < lbTileItems.Items.Count; i++)
			{
				int value = ((TileTextItem)lbTileItems.Items[i]).Value;
				lbTileItems.SetItemChecked(i, ((uint)value & (uint)texts) != 0);
			}
		}

		private static void SelectCaption(ComboBox cb, int id)
		{
			for (int i = 0; i < cb.Items.Count; i++)
			{
				CaptionData captionData = (CaptionData)cb.Items[i];
				if (captionData.Id == id)
				{
					cb.SelectedIndex = i;
					break;
				}
			}
		}

		public static bool Show(IWin32Window parent, DisplayListConfig displayListConfig, ItemViewMode mode, Action<DisplayListConfig> apply = null)
		{
			using (ListLayoutDialog listLayoutDialog = new ListLayoutDialog())
			{
				listLayoutDialog.apply = apply;
				listLayoutDialog.DisplayListConfig = displayListConfig;
				switch (mode)
				{
					case ItemViewMode.Thumbnail:
						listLayoutDialog.tab.SelectedIndex = 1;
						break;
					case ItemViewMode.Tile:
						listLayoutDialog.tab.SelectedIndex = 2;
						break;
				}
				if (listLayoutDialog.ShowDialog(parent) == DialogResult.OK)
				{
					listLayoutDialog.Apply();
					return true;
				}
				return false;
			}
		}

	}
}
