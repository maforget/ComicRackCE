using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Text;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class SmartListDialog : Form, ISmartListDialog
	{
		private class ReferenceItem : ComboBoxSkinner.ComboBoxItem<string>
		{
			private const int ImageSpacing = 4;

			public int Level
			{
				get;
				set;
			}

			public Guid Id
			{
				get;
				set;
			}

			public Image Image
			{
				get;
				set;
			}

			public ReferenceItem(int level, string name, Guid id, Image image)
				: base(name)
			{
				base.IsOwnerDrawn = true;
				Level = level;
				Id = id;
				Image = image;
			}

			public override void Draw(Graphics gr, Rectangle bounds, Color foreColor, Font font)
			{
				bounds = bounds.Pad(Level * Image.Width, 0);
				gr.DrawImage(Image, Image.Size.Align(bounds, ContentAlignment.MiddleLeft));
				bounds = bounds.Pad(Image.Width + ImageSpacing, 0);
				using (StringFormat format = new StringFormat(StringFormatFlags.NoWrap)
				{
					LineAlignment = StringAlignment.Center
				})
				{
					using (SolidBrush brush = new SolidBrush(foreColor))
					{
						gr.DrawString(base.Item, font, brush, bounds, format);
					}
				}
			}

			public override Size Measure(Graphics gr, Font font)
			{
				Size result = gr.MeasureString(base.Item, font).ToSize();
				result.Width += Level * Image.Width + Image.Width + ImageSpacing;
				result.Height = Math.Max(result.Height, Image.Height);
				return result;
			}
		}

		private ComicSmartListItem smartComicList;

		public ComicLibrary Library
		{
			get;
			set;
		}

		public Guid EditId
		{
			get;
			set;
		}

		public ComicSmartListItem SmartComicList
		{
			get
			{
				return smartComicList;
			}
			set
			{
				matcherControls.SuspendLayout();
				try
				{
					if (smartComicList != null)
					{
						smartComicList.Matchers.Changed -= Matchers_Changed;
					}
					smartComicList = null;
					FillBaseCombo(value);
					smartComicList = value;
					txtName.Text = smartComicList.Name;
					txtNotes.Text = StringUtility.MakeEditBoxMultiline(smartComicList.Description);
					cbMatchMode.SelectedIndex = ((smartComicList.MatcherMode != 0) ? 1 : 0);
					chkNotBaseList.Checked = smartComicList.NotInBaseList;
					chkLimit.Checked = smartComicList.Limit;
					cbLimitType.SelectedIndex = (int)smartComicList.LimitType;
					txLimit.Text = smartComicList.LimitValue.ToString();
					chkQuickOpen.Checked = smartComicList.QuickOpen;
					chkShowNotes.Checked = !string.IsNullOrEmpty(txtNotes.Text) || chkLimit.Checked || chkQuickOpen.Checked;
					Button button = btFilterReset;
					bool visible = (labelFilterReset.Visible = smartComicList.ShouldSerializeFilteredIds() && chkShowNotes.Checked);
					button.Visible = visible;
					smartComicList.Matchers.Changed += Matchers_Changed;
					matcherControls.Clear(withDispose: true);
					foreach (ComicBookMatcher matcher in smartComicList.Matchers)
					{
						AddMatcherControl(matcher);
					}
				}
				finally
				{
					matcherControls.ResumeLayout(performLayout: true);
				}
			}
		}

		public bool EnableNavigation
		{
			get
			{
				return btPrev.Visible;
			}
			set
			{
				Button button = btPrev;
				bool visible = (btNext.Visible = value);
				button.Visible = visible;
				if (value)
				{
					btQuery.Left = btNext.Right + (btNext.Left - btPrev.Right);
				}
				else
				{
					btQuery.Left = btPrev.Left;
				}
			}
		}

		public bool PreviousEnabled
		{
			get
			{
				return btPrev.Enabled;
			}
			set
			{
				btPrev.Enabled = value;
			}
		}

		public bool NextEnabled
		{
			get
			{
				return btNext.Enabled;
			}
			set
			{
				btNext.Enabled = value;
			}
		}

		public int DialogEditorOffset
		{
			get
			{
				return -matcherControls.AutoScrollPosition.Y;
			}
			set
			{
				matcherControls.AutoScrollPosition = new Point(0, value);
			}
		}

		public event EventHandler Apply;

		public event EventHandler Next;

		public event EventHandler Previous;

		public SmartListDialog()
		{
			LocalizeUtility.UpdateRightToLeft(this);
			InitializeComponent();
			chkShowNotes.Image = ((Bitmap)chkShowNotes.Image).ScaleDpi();
			baseImages.ImageSize = baseImages.ImageSize.ScaleDpi();
			baseImages.Images.Add("Library", Resources.Library);
			baseImages.Images.Add("Folder", Resources.SearchFolder);
			baseImages.Images.Add("Search", Resources.SearchDocument);
			baseImages.Images.Add("List", Resources.List);
			baseImages.Images.Add("TempFolder", Resources.TempFolder);
			this.RestorePosition();
			LocalizeUtility.Localize(this, null);
			LocalizeUtility.Localize(TR.Load(base.Name), cbLimitType);
			new ComboBoxSkinner(cbBaseList);
			txLimit.EnableOnlyNumberKeys();
			SpinButton.AddUpDown(txLimit, 1, 1);
			cbMatchMode.Items.AddRange(TR.Load(base.Name).GetStrings("MatchMode", "All|Any", '|'));
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			IMatcherEditor matcherEditor = this.FindActiveService<IMatcherEditor>() ?? this.FindFirstService<IMatcherEditor>();
			if (matcherEditor == null)
			{
				return;
			}
			if (e.Control)
			{
				e.Handled = HandleControlSequences(matcherEditor, e.KeyCode);
			}
			else
			{
				if (base.ActiveControl is ComboBox)
				{
					return;
				}
				switch (e.KeyCode)
				{
				case Keys.Down:
				{
					List<IMatcherEditor> list2 = this.FindServices<IMatcherEditor>().ToList();
					int num2 = list2.IndexOf(matcherEditor) + 1;
					if (num2 < list2.Count)
					{
						list2[num2].SetFocus();
						e.Handled = true;
					}
					break;
				}
				case Keys.Up:
				{
					List<IMatcherEditor> list = this.FindServices<IMatcherEditor>().ToList();
					int num = list.IndexOf(matcherEditor) - 1;
					if (num >= 0)
					{
						list[num].SetFocus();
						e.Handled = true;
					}
					break;
				}
				}
			}
		}

		private bool HandleControlSequences(IMatcherEditor im, Keys keyCode)
		{
			switch (keyCode)
			{
			case Keys.R:
				im.AddRule();
				break;
			case Keys.G:
				im.AddGroup();
				break;
			case Keys.X:
				im.CutClipboard();
				break;
			case Keys.C:
				im.CopyClipboard();
				break;
			case Keys.V:
				im.PasteClipboard();
				break;
			case Keys.U:
				im.MoveUp();
				break;
			case Keys.D:
				im.MoveDown();
				break;
			default:
				return false;
			}
			return true;
		}

		private void Matchers_Changed(object sender, SmartListChangedEventArgs<ComicBookMatcher> e)
		{
			int dialogEditorOffset = DialogEditorOffset;
			switch (e.Action)
			{
			case SmartListAction.Insert:
				if (matcherControls.Controls.OfType<Control>().FirstOrDefault((Control c) => c.Tag == e.Item) == null)
				{
					AddMatcherControl(e.Item);
				}
				break;
			case SmartListAction.Remove:
			{
				Control control2 = matcherControls.Controls.OfType<Control>().FirstOrDefault((Control cc) => cc.Tag == e.Item);
				if (control2 != null)
				{
					matcherControls.Controls.Remove(control2);
				}
				break;
			}
			case SmartListAction.Move:
			{
				Control control = matcherControls.Controls.OfType<Control>().FirstOrDefault((Control cc) => cc.Tag == e.Item);
				if (control != null)
				{
					matcherControls.Controls.SetChildIndex(control, e.Index);
				}
				break;
			}
			}
			DialogEditorOffset = dialogEditorOffset;
		}

		private void cbMatchMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			SmartComicList.MatcherMode = ((cbMatchMode.SelectedIndex != 0) ? MatcherMode.Or : MatcherMode.And);
		}

		private void chkLimit_CheckedChanged(object sender, EventArgs e)
		{
			ComicSmartListItem comicSmartListItem = SmartComicList;
			TextBox textBox = txLimit;
			bool flag = (cbLimitType.Enabled = chkLimit.Checked);
			bool limit = (textBox.Enabled = flag);
			comicSmartListItem.Limit = limit;
		}

		private void cbLimitType_SelectedIndexChanged(object sender, EventArgs e)
		{
			SmartComicList.LimitType = (ComicSmartListLimitType)cbLimitType.SelectedIndex;
		}

		private void txLimit_TextChanged(object sender, EventArgs e)
		{
			int.TryParse(txLimit.Text, out var result);
			if (result < 0)
			{
				result = 0;
			}
			SmartComicList.LimitValue = result;
		}

		private void btApply_Click(object sender, EventArgs e)
		{
			if (this.Apply != null)
			{
				this.Apply(this, EventArgs.Empty);
			}
		}

		private void btOK_Click(object sender, EventArgs e)
		{
			if (this.Apply != null)
			{
				this.Apply(this, EventArgs.Empty);
			}
		}

		private void btPrev_Click(object sender, EventArgs e)
		{
			if (this.Apply != null)
			{
				this.Apply(this, EventArgs.Empty);
			}
			if (this.Previous != null)
			{
				this.Previous(this, EventArgs.Empty);
			}
		}

		private void btNext_Click(object sender, EventArgs e)
		{
			if (this.Apply != null)
			{
				this.Apply(this, EventArgs.Empty);
			}
			if (this.Next != null)
			{
				this.Next(this, EventArgs.Empty);
			}
		}

		private void cbBaseList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (smartComicList != null)
			{
				smartComicList.BaseListId = ((ReferenceItem)cbBaseList.SelectedItem).Id;
			}
		}

		private void txtName_TextChanged(object sender, EventArgs e)
		{
			SmartComicList.Name = txtName.Text.Trim();
		}

		private void txtNotes_TextChanged(object sender, EventArgs e)
		{
			SmartComicList.Description = txtNotes.Text.Trim();
		}

		private void chkNotBaseList_CheckedChanged(object sender, EventArgs e)
		{
			SmartComicList.NotInBaseList = chkNotBaseList.Checked;
		}

		private void chkQuickOpen_CheckedChanged(object sender, EventArgs e)
		{
			SmartComicList.QuickOpen = chkQuickOpen.Checked;
		}

		private void chkShowNotes_CheckedChanged(object sender, EventArgs e)
		{
			ShowNotes(chkShowNotes.Checked);
		}

		private void btFilterReset_Click(object sender, EventArgs e)
		{
			SmartComicList.ClearFiltered();
			Button button = btFilterReset;
			bool visible = (labelFilterReset.Visible = false);
			button.Visible = visible;
		}

		private void ShowNotes(bool show)
		{
			CheckBox checkBox = chkQuickOpen;
			ComboBox comboBox = cbLimitType;
			TextBox textBox = txLimit;
			CheckBox checkBox2 = chkLimit;
			Label label = labelNotes;
			bool flag2 = (txtNotes.Visible = show);
			bool flag4 = (label.Visible = flag2);
			bool flag6 = (checkBox2.Visible = flag4);
			bool flag8 = (textBox.Visible = flag6);
			bool visible = (comboBox.Visible = flag8);
			checkBox.Visible = visible;
			Label label2 = labelFilterReset;
			visible = (btFilterReset.Visible = SmartComicList.ShouldSerializeFilteredIds() && show);
			label2.Visible = visible;
		}

		private void FillBaseCombo(ComicSmartListItem scl)
		{
            cbBaseList.Items.Clear();
            //RecursionCache.Items.Remove(EditId);

            foreach (ComicListItem item in Library.ComicLists.GetItems<ComicListItem>())
			{
				Guid guid = ((item is ComicLibraryListItem) ? Guid.Empty : item.Id);
                if (!item.RecursionTest(EditId))
                {
					cbBaseList.Items.Add(new ReferenceItem(item.GetLevel(), item.Name, guid, baseImages.Images[item.ImageKey]));
                    if (guid == scl.BaseListId)
                    {
						cbBaseList.SelectedIndex = cbBaseList.Items.Count - 1;
                    }
				}
            }
			if (cbBaseList.Items.Count > 0 && cbBaseList.SelectedIndex == -1)
            {
				cbBaseList.SelectedIndex = 0;
            }
        }

        private void AddMatcherControl(ComicBookMatcher icbm)
		{
			int width = cbBaseList.Right - matcherControls.Left;
			Control control = ((icbm is ComicBookGroupMatcher) ? CreateGroupMatchPanel(icbm as ComicBookGroupMatcher, width) : CreateMatchPanel(icbm as ComicBookValueMatcher, width));
			matcherControls.Controls.Add(control);
			matcherControls.Controls.SetChildIndex(control, smartComicList.Matchers.IndexOf(icbm));
			matcherControls.AutoTabIndex();
		}

		private Control CreateMatchPanel(ComicBookValueMatcher matcher, int width)
		{
			return new MatcherEditor(smartComicList.Matchers, matcher, 0, width);
		}

		private Control CreateGroupMatchPanel(ComicBookGroupMatcher matcher, int width)
		{
			return new MatcherGroupEditor(smartComicList.Matchers, matcher, 0, width);
		}
	}
}
