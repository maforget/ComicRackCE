using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Localize;
using cYo.Common.Text;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class MatcherGroupEditor : UserControlEx, IMatcherEditor
	{
		private static TR TR = TR.Load("SmartListDialog");

		private ComicBookGroupMatcher currentComicBookMatcher;

		private readonly ComicBookMatcherCollection matchers;

		private readonly int level;

		private readonly string rulesText;

		private int DialogEditorOffset
		{
			get
			{
				int result = 0;
				if (base.ParentForm is SmartListDialog)
				{
					result = ((SmartListDialog)base.ParentForm).DialogEditorOffset;
				}
				return result;
			}
			set
			{
				if (base.ParentForm is SmartListDialog)
				{
					((SmartListDialog)base.ParentForm).DialogEditorOffset = value;
				}
			}
		}

		public string Description => (from me in matcherControls.GetControls<MatcherEditor>()
			select me.Description.Ellipsis(20, "...")).ToListString(", ");

		public MatcherGroupEditor(ComicBookMatcherCollection matchers, ComicBookGroupMatcher comicBookMatcher, int level, int width)
		{
			InitializeComponent();
			chkExpanded.Image = ((Bitmap)chkExpanded.Image).ScaleDpi();
			btEdit.Image = ((Bitmap)btEdit.Image).ScaleDpi();
			base.Width = width;
			MinimumSize = new Size(width, 0);
			base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			toolTip.SetToolTip(chkNot, TR.Default["LogicNot", "Negation"]);
			base.Tag = comicBookMatcher;
			this.level = level;
			this.matchers = matchers;
			comicBookMatcher.Matchers.Changed += OwnMatchers_Changed;
			cbMatchMode.Items.AddRange(TR.GetStrings("MatchMode", "All|Any", '|'));
			LocalizeUtility.Localize(TR, labelSubRules);
			LocalizeUtility.Localize(TR.Load("MatcherEditor"), cmEdit);
			rulesText = labelSubRules.Text;
			InitializeMatcher(comicBookMatcher);
		}

		private void cmEdit_Opening(object sender, CancelEventArgs e)
		{
			miNewGroup.Enabled = level <= 5;
			ToolStripMenuItem toolStripMenuItem = miDelete;
			bool enabled = (miCut.Enabled = matchers.Count > 1);
			toolStripMenuItem.Enabled = enabled;
			miPaste.Enabled = Clipboard.ContainsData(ComicBookMatcher.ClipboardFormat);
			miMoveDown.Enabled = matchers.IndexOf(currentComicBookMatcher) < matchers.Count - 1;
			miMoveUp.Enabled = matchers.IndexOf(currentComicBookMatcher) > 0;
		}

		private void miNewRule_Click(object sender, EventArgs e)
		{
			AddRule();
		}

		private void miNewGroup_Click(object sender, EventArgs e)
		{
			AddGroup();
		}

		private void miDelete_Click(object sender, EventArgs e)
		{
			DeleteRuleOrGroup();
		}

		private void miCopy_Click(object sender, EventArgs e)
		{
			CopyClipboard();
		}

		private void miCut_Click(object sender, EventArgs e)
		{
			CutClipboard();
		}

		private void miPaste_Click(object sender, EventArgs e)
		{
			PasteClipboard();
		}

		private void miMoveUp_Click(object sender, EventArgs e)
		{
			MoveUp();
		}

		private void miMoveDown_Click(object sender, EventArgs e)
		{
			MoveDown();
		}

		private void btEdit_Click(object sender, EventArgs e)
		{
			cmEdit.Show(btEdit, new Point(btEdit.Width, btEdit.Height), ToolStripDropDownDirection.BelowLeft);
		}

		private void OwnMatchers_Changed(object sender, SmartListChangedEventArgs<ComicBookMatcher> e)
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
			currentComicBookMatcher.MatcherMode = ((cbMatchMode.SelectedIndex != 0) ? MatcherMode.Or : MatcherMode.And);
		}

		private void chkNot_CheckedChanged(object sender, EventArgs e)
		{
			currentComicBookMatcher.Not = chkNot.Checked;
		}

		private void chkCollapse_CheckedChanged(object sender, EventArgs e)
		{
			int dialogEditorOffset = DialogEditorOffset;
			currentComicBookMatcher.Collapsed = !chkExpanded.Checked;
			matcherControls.Visible = chkExpanded.Checked;
			labelSubRules.Text = (chkExpanded.Checked ? rulesText : Description);
			DialogEditorOffset = dialogEditorOffset;
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			foreach (ComicBookMatcher matcher in currentComicBookMatcher.Matchers)
			{
				AddMatcherControl(matcher);
			}
			labelSubRules.Text = (chkExpanded.Checked ? rulesText : Description);
		}

		public void AddMatcherControl(ComicBookMatcher icbm)
		{
			Size size3 = (matcherControls.MinimumSize = (matcherControls.MaximumSize = new Size(base.Width - matcherControls.Left, 0)));
			int width = matcherControls.Width;
			Control control = ((icbm is ComicBookGroupMatcher) ? CreateGroupMatchPanel(icbm as ComicBookGroupMatcher, width) : CreateMatchPanel(icbm as ComicBookValueMatcher, width));
			matcherControls.Controls.Add(control);
			matcherControls.Controls.SetChildIndex(control, currentComicBookMatcher.Matchers.IndexOf(icbm));
			matcherControls.AutoTabIndex();
		}

		public Control CreateMatchPanel(ComicBookValueMatcher matcher, int width)
		{
			return new MatcherEditor(currentComicBookMatcher.Matchers, matcher, level, width);
		}

		public Control CreateGroupMatchPanel(ComicBookGroupMatcher matcher, int width)
		{
			return new MatcherGroupEditor(currentComicBookMatcher.Matchers, matcher, level + 1, width);
		}

		private void InitializeMatcher(ComicBookGroupMatcher comicBookMatcher)
		{
			currentComicBookMatcher = comicBookMatcher;
			chkNot.Checked = comicBookMatcher.Not;
			cbMatchMode.SelectedIndex = ((comicBookMatcher.MatcherMode != 0) ? 1 : 0);
			chkExpanded.Checked = !comicBookMatcher.Collapsed;
		}

		public void AddRule()
		{
			ComicBookMatcher item = ((ICloneable)currentComicBookMatcher.Matchers.Last()).Clone<ComicBookMatcher>();
			matchers.Insert(matchers.IndexOf(currentComicBookMatcher) + 1, item);
		}

		public void AddGroup()
		{
			if (level <= 5)
			{
				ComicBookGroupMatcher comicBookGroupMatcher = ((ICloneable)currentComicBookMatcher).Clone<ComicBookGroupMatcher>();
				comicBookGroupMatcher.Matchers.Clear();
				comicBookGroupMatcher.Matchers.Add(((ICloneable)currentComicBookMatcher.Matchers.Last()).Clone<ComicBookMatcher>());
				matchers.Insert(matchers.IndexOf(currentComicBookMatcher) + 1, comicBookGroupMatcher);
			}
		}

		public void DeleteRuleOrGroup()
		{
			matchers.Remove(currentComicBookMatcher);
		}

		public void CopyClipboard()
		{
			Clipboard.SetData(ComicBookMatcher.ClipboardFormat, currentComicBookMatcher);
		}

		public void CutClipboard()
		{
			Clipboard.SetData(ComicBookMatcher.ClipboardFormat, currentComicBookMatcher);
			matchers.Remove(currentComicBookMatcher);
		}

		public void PasteClipboard()
		{
			ComicBookMatcher comicBookMatcher = Clipboard.GetData(ComicBookMatcher.ClipboardFormat) as ComicBookMatcher;
			if (comicBookMatcher != null && (!(comicBookMatcher is ComicBookGroupMatcher) || level <= 5))
			{
				matchers.Insert(matchers.IndexOf(currentComicBookMatcher) + 1, comicBookMatcher);
			}
		}

		public void MoveUp()
		{
			matchers.MoveRelative(matchers.IndexOf(currentComicBookMatcher), -1);
		}

		public void MoveDown()
		{
			matchers.MoveRelative(matchers.IndexOf(currentComicBookMatcher), 1);
		}

		public void SetFocus()
		{
			Focus();
		}
	}
}
