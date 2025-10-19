using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using cYo.Common;
using cYo.Common.Localize;
using cYo.Common.Text;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class MatcherEditor : UserControlEx, IMatcherEditor
	{
		private class MatcherEntry : ComboBoxSkinner.ComboBoxItem<ComicBookValueMatcher>
		{
			public MatcherEntry(ComicBookValueMatcher item)
				: base(item)
			{
			}

			public override string ToString()
			{
				return base.Item.Description;
			}
		}

		public const int MaxLevel = 5;

		public static readonly ValuePair<Color, Regex>[] ColorRegex = new ValuePair<Color, Regex>[2]
		{
			new ValuePair<Color, Regex>(Color.Red, new Regex("\\{[a-z]+\\}", RegexOptions.IgnoreCase | RegexOptions.Compiled)),
			new ValuePair<Color, Regex>(Color.Blue, new Regex("\\{(" + ComicBookMatcher.ComicProperties.Concat(ComicBookMatcher.SeriesStatsProperties).ToListString("|") + ")\\}", RegexOptions.Compiled))
		};

        private ComicBookValueMatcher currentComicBookMatcher;
        private readonly ComicBookMatcherCollection matchers;
        private readonly int spacing = 10;
        private readonly int level;

        public string Description
		{
			get
			{
				string text = rtfMatchValue.Text.Trim();
				if (string.IsNullOrEmpty(text))
				{
					text = btMatcher.Text;
				}
				return text;
			}
		}

		public MatcherEditor(ComicBookMatcherCollection matchers, ComicBookValueMatcher comicBookMatcher, int level, int width)
		{
			InitializeComponent();
			toolTip.SetToolTip(chkNot, TR.Default["LogicNot", "Negation"]);
			this.matchers = matchers;
			this.level = level;
			base.Height = btEdit.Bottom + 2;
			base.Width = width;
			spacing = rtfMatchValue2.Left - rtfMatchValue.Right;
			InitializeMatcher(comicBookMatcher);
            // a button pretending to be a dropdown combobox. lovely.
            // let's dress it up as one. Except for dropdown arrow to carot - that's asking too much
            // we also have to make it a bit smaller as otherwise borders are out of bounds
            this.btMatcher.SetComboBoxButton();
        }

		private void cmEdit_Opening(object sender, CancelEventArgs e)
		{
			miNewGroup.Enabled = level <= MaxLevel;
			ToolStripMenuItem toolStripMenuItem = miCut;
			bool enabled = (miDelete.Enabled = matchers.Count > 1);
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

		private void cbOperator_SelectedIndexChanged(object sender, EventArgs e)
		{
			currentComicBookMatcher.MatchOperator = cbOperator.SelectedIndex;
			SetVisiblity();
			rtfMatchValue.Text = currentComicBookMatcher.MatchValue;
			rtfMatchValue2.Text = currentComicBookMatcher.MatchValue2;
		}

		private void rtfMatchValue_Leave(object sender, EventArgs e)
		{
			currentComicBookMatcher.MatchValue = rtfMatchValue.Text;
		}

		private void rtfMatchValue2_Leave(object sender, EventArgs e)
		{
			currentComicBookMatcher.MatchValue2 = rtfMatchValue2.Text;
		}

		private void chkNot_CheckedChanged(object sender, EventArgs e)
		{
			currentComicBookMatcher.Not = chkNot.Checked;
		}

		private void btMatcher_Click(object sender, EventArgs e)
		{
			Program.CreateComicBookMatchersMenu(delegate(ComicBookValueMatcher newMatcher)
			{
				if (newMatcher != null && !(newMatcher.GetType() == currentComicBookMatcher.GetType()))
				{
					int num = matchers.IndexOf(currentComicBookMatcher);
					newMatcher.Set(currentComicBookMatcher);
					InitializeMatcher(newMatcher);
					if (num != -1)
					{
						matchers[num] = newMatcher;
					}
				}
			}).Show(btMatcher, new Point(0, btMatcher.Height), ToolStripDropDownDirection.BelowRight);
		}

		private void btEdit_Click(object sender, EventArgs e)
		{
			cmEdit.Show(btEdit, new Point(btEdit.Width, btEdit.Height), ToolStripDropDownDirection.BelowLeft);
		}

		private void rtfMatchValue_DoubleClick(object sender, EventArgs e)
		{
			TextBoxBase textBoxBase = sender as TextBoxBase;
			using (ValueEditorDialog valueEditorDialog = new ValueEditorDialog())
			{
				valueEditorDialog.SyntaxColoring(ColorRegex);
				valueEditorDialog.MatchValue = textBoxBase.Text;
				if (valueEditorDialog.ShowDialog(this) == DialogResult.OK)
				{
					textBoxBase.Text = valueEditorDialog.MatchValue;
				}
			}
		}


		protected override void OnLayout(LayoutEventArgs e)
		{
			base.OnLayout(e);
			if (base.IsHandleCreated)
			{
				SetVisiblity();
			}
		}

		private void InitializeMatcher(ComicBookValueMatcher comicBookMatcher)
		{
			currentComicBookMatcher = comicBookMatcher;
			base.Tag = comicBookMatcher;
			chkNot.Checked = comicBookMatcher.Not;
			btMatcher.Text = comicBookMatcher.Description;
			btMatcher.Tag = comicBookMatcher;
			cbOperator.Items.Clear();
			cbOperator.Items.AddRange(comicBookMatcher.OperatorsList);
			cbOperator.SelectedIndex = comicBookMatcher.MatchOperator;
		}

		private void SetVisiblity()
		{
			Control control = cbOperator;
			if (cbOperator.Left > rtfMatchValue.Left)
			{
				int left = cbOperator.Left;
				cbOperator.Left = rtfMatchValue.Left;
				rtfMatchValue.Left = left;
			}
			rtfMatchValue.Width = btEdit.Left - rtfMatchValue.Left - 8;
			rtfMatchValue2.Width = btEdit.Left - rtfMatchValue2.Left - 8;
			switch (currentComicBookMatcher.ArgumentCount)
			{
			case 0:
			{
				TextBox textBox2 = rtfMatchValue;
				TextBox textBox3 = rtfMatchValue2;
				bool flag3 = (lblDescription.Visible = false);
				bool visible = (textBox3.Visible = flag3);
				textBox2.Visible = visible;
				control.Width = rtfMatchValue2.Right - cbOperator.Left;
				break;
			}
			case 1:
				rtfMatchValue.Visible = true;
				rtfMatchValue.Width = rtfMatchValue2.Right - rtfMatchValue.Left;
				rtfMatchValue2.Visible = false;
				control.Width = rtfMatchValue.Left - 8 - control.Left;
				if (!string.IsNullOrEmpty(currentComicBookMatcher.UnitDescription))
				{
					lblDescription.Text = currentComicBookMatcher.UnitDescription;
					lblDescription.Visible = !string.IsNullOrEmpty(currentComicBookMatcher.UnitDescription);
					lblDescription.Left = rtfMatchValue2.Right - lblDescription.Width;
					lblDescription.Top = rtfMatchValue2.Top + (rtfMatchValue2.Height - lblDescription.Height) / 2;
					rtfMatchValue.Width = lblDescription.Left - spacing - rtfMatchValue.Left;
				}
				break;
			case 2:
			{
				rtfMatchValue.Width = rtfMatchValue2.Left - spacing - rtfMatchValue.Left;
				TextBox textBox = rtfMatchValue;
				bool visible = (rtfMatchValue2.Visible = true);
				textBox.Visible = visible;
				lblDescription.Visible = false;
				control.Width = rtfMatchValue.Left - 8 - control.Left;
				break;
			}
			}
			bool flag5 = cbOperator.Left > rtfMatchValue.Left;
			if (currentComicBookMatcher.SwapOperatorArgument)
			{
				int left2 = cbOperator.Left;
				cbOperator.Left = rtfMatchValue.Left;
				rtfMatchValue.Left = left2;
			}
		}

		public void AddRule()
		{
			matchers.Insert(matchers.IndexOf(currentComicBookMatcher) + 1, (ComicBookMatcher)currentComicBookMatcher.Clone());
		}

		public void AddGroup()
		{
			if (level <= MaxLevel)
			{
				ComicBookGroupMatcher comicBookGroupMatcher = new ComicBookGroupMatcher();
				comicBookGroupMatcher.Matchers.Add(currentComicBookMatcher.Clone() as ComicBookMatcher);
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
			ComicBookMatcher comicBookMatcher = null;
			try
			{
				comicBookMatcher = Clipboard.GetData(ComicBookMatcher.ClipboardFormat) as ComicBookMatcher;
			}
			catch
			{
			}
			if (comicBookMatcher != null && (!(comicBookMatcher is ComicBookGroupMatcher) || level <= MaxLevel))
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
			if (rtfMatchValue.Visible)
			{
				rtfMatchValue.Focus();
			}
			else
			{
				cbOperator.Focus();
			}
		}
	}
}
