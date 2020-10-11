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
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public class MatcherEditor : UserControl, IMatcherEditor
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

		private IContainer components;

		private ComboBox cbOperator;

		private Label lblDescription;

		private CheckBox chkNot;

		private ToolTip toolTip;

		private ContextMenuStrip cmEdit;

		private ToolStripMenuItem miNewRule;

		private ToolStripMenuItem miNewGroup;

		private ToolStripSeparator toolStripMenuItem1;

		private ToolStripMenuItem miCopy;

		private ToolStripMenuItem miCut;

		private ToolStripMenuItem miPaste;

		private ToolStripSeparator toolStripMenuItem2;

		private ToolStripMenuItem miMoveUp;

		private ToolStripMenuItem miMoveDown;

		private Button btEdit;

		private TextBox rtfMatchValue;

		private TextBox rtfMatchValue2;

		private ToolStripMenuItem miDelete;

		private Button btMatcher;

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
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void cmEdit_Opening(object sender, CancelEventArgs e)
		{
			miNewGroup.Enabled = level <= 5;
			ToolStripMenuItem toolStripMenuItem = miCut;
			bool enabled = (miDelete.Enabled = matchers.Count > 1);
			toolStripMenuItem.Enabled = enabled;
			miPaste.Enabled = Clipboard.ContainsData("ComicBookMatcher");
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
			if (level <= 5)
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
			Clipboard.SetData("ComicBookMatcher", currentComicBookMatcher);
		}

		public void CutClipboard()
		{
			Clipboard.SetData("ComicBookMatcher", currentComicBookMatcher);
			matchers.Remove(currentComicBookMatcher);
		}

		public void PasteClipboard()
		{
			ComicBookMatcher comicBookMatcher = null;
			try
			{
				comicBookMatcher = Clipboard.GetData("ComicBookMatcher") as ComicBookMatcher;
			}
			catch
			{
			}
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
			if (rtfMatchValue.Visible)
			{
				rtfMatchValue.Focus();
			}
			else
			{
				cbOperator.Focus();
			}
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			cbOperator = new System.Windows.Forms.ComboBox();
			lblDescription = new System.Windows.Forms.Label();
			chkNot = new System.Windows.Forms.CheckBox();
			toolTip = new System.Windows.Forms.ToolTip(components);
			cmEdit = new System.Windows.Forms.ContextMenuStrip(components);
			miNewRule = new System.Windows.Forms.ToolStripMenuItem();
			miNewGroup = new System.Windows.Forms.ToolStripMenuItem();
			miDelete = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			miCut = new System.Windows.Forms.ToolStripMenuItem();
			miCopy = new System.Windows.Forms.ToolStripMenuItem();
			miPaste = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			miMoveUp = new System.Windows.Forms.ToolStripMenuItem();
			miMoveDown = new System.Windows.Forms.ToolStripMenuItem();
			btEdit = new System.Windows.Forms.Button();
			rtfMatchValue = new System.Windows.Forms.TextBox();
			rtfMatchValue2 = new System.Windows.Forms.TextBox();
			btMatcher = new System.Windows.Forms.Button();
			cmEdit.SuspendLayout();
			SuspendLayout();
			cbOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbOperator.FormattingEnabled = true;
			cbOperator.Location = new System.Drawing.Point(169, 0);
			cbOperator.Name = "cbOperator";
			cbOperator.Size = new System.Drawing.Size(135, 21);
			cbOperator.TabIndex = 2;
			cbOperator.SelectedIndexChanged += new System.EventHandler(cbOperator_SelectedIndexChanged);
			lblDescription.AutoSize = true;
			lblDescription.Location = new System.Drawing.Point(448, 25);
			lblDescription.Name = "lblDescription";
			lblDescription.Size = new System.Drawing.Size(60, 13);
			lblDescription.TabIndex = 7;
			lblDescription.Text = "Description";
			chkNot.Appearance = System.Windows.Forms.Appearance.Button;
			chkNot.Location = new System.Drawing.Point(0, 0);
			chkNot.Name = "chkNot";
			chkNot.Size = new System.Drawing.Size(21, 21);
			chkNot.TabIndex = 0;
			chkNot.Text = "!";
			chkNot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			chkNot.UseVisualStyleBackColor = true;
			chkNot.CheckedChanged += new System.EventHandler(chkNot_CheckedChanged);
			cmEdit.Items.AddRange(new System.Windows.Forms.ToolStripItem[10]
			{
				miNewRule,
				miNewGroup,
				miDelete,
				toolStripMenuItem1,
				miCut,
				miCopy,
				miPaste,
				toolStripMenuItem2,
				miMoveUp,
				miMoveDown
			});
			cmEdit.Name = "cmEdit";
			cmEdit.Size = new System.Drawing.Size(181, 192);
			cmEdit.Opening += new System.ComponentModel.CancelEventHandler(cmEdit_Opening);
			miNewRule.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.AddTab;
			miNewRule.Name = "miNewRule";
			miNewRule.ShortcutKeys = System.Windows.Forms.Keys.R | System.Windows.Forms.Keys.Control;
			miNewRule.Size = new System.Drawing.Size(180, 22);
			miNewRule.Text = "New Rule";
			miNewRule.Click += new System.EventHandler(miNewRule_Click);
			miNewGroup.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Group;
			miNewGroup.Name = "miNewGroup";
			miNewGroup.ShortcutKeys = System.Windows.Forms.Keys.G | System.Windows.Forms.Keys.Control;
			miNewGroup.Size = new System.Drawing.Size(180, 22);
			miNewGroup.Text = "New Group";
			miNewGroup.Click += new System.EventHandler(miNewGroup_Click);
			miDelete.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.EditDelete;
			miDelete.Name = "miDelete";
			miDelete.Size = new System.Drawing.Size(180, 22);
			miDelete.Text = "Delete";
			miDelete.Click += new System.EventHandler(miDelete_Click);
			toolStripMenuItem1.Name = "toolStripMenuItem1";
			toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
			miCut.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Cut;
			miCut.Name = "miCut";
			miCut.ShortcutKeys = System.Windows.Forms.Keys.X | System.Windows.Forms.Keys.Control;
			miCut.Size = new System.Drawing.Size(180, 22);
			miCut.Text = "Cut";
			miCut.Click += new System.EventHandler(miCut_Click);
			miCopy.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.EditCopy;
			miCopy.Name = "miCopy";
			miCopy.ShortcutKeys = System.Windows.Forms.Keys.C | System.Windows.Forms.Keys.Control;
			miCopy.Size = new System.Drawing.Size(180, 22);
			miCopy.Text = "Copy";
			miCopy.Click += new System.EventHandler(miCopy_Click);
			miPaste.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.EditPaste;
			miPaste.Name = "miPaste";
			miPaste.ShortcutKeys = System.Windows.Forms.Keys.V | System.Windows.Forms.Keys.Control;
			miPaste.Size = new System.Drawing.Size(180, 22);
			miPaste.Text = "Paste";
			miPaste.Click += new System.EventHandler(miPaste_Click);
			toolStripMenuItem2.Name = "toolStripMenuItem2";
			toolStripMenuItem2.Size = new System.Drawing.Size(177, 6);
			miMoveUp.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.GroupUp;
			miMoveUp.Name = "miMoveUp";
			miMoveUp.ShortcutKeys = System.Windows.Forms.Keys.U | System.Windows.Forms.Keys.Control;
			miMoveUp.Size = new System.Drawing.Size(180, 22);
			miMoveUp.Text = "Move Up";
			miMoveUp.Click += new System.EventHandler(miMoveUp_Click);
			miMoveDown.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.GroupDown;
			miMoveDown.Name = "miMoveDown";
			miMoveDown.ShortcutKeys = System.Windows.Forms.Keys.D | System.Windows.Forms.Keys.Control;
			miMoveDown.Size = new System.Drawing.Size(180, 22);
			miMoveDown.Text = "Move Down";
			miMoveDown.Click += new System.EventHandler(miMoveDown_Click);
			btEdit.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btEdit.ContextMenuStrip = cmEdit;
			btEdit.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.SmallArrowDown;
			btEdit.Location = new System.Drawing.Point(568, 0);
			btEdit.Name = "btEdit";
			btEdit.Size = new System.Drawing.Size(21, 21);
			btEdit.TabIndex = 10;
			btEdit.UseVisualStyleBackColor = true;
			btEdit.Click += new System.EventHandler(btEdit_Click);
			rtfMatchValue.Location = new System.Drawing.Point(310, 1);
			rtfMatchValue.Name = "rtfMatchValue";
			rtfMatchValue.Size = new System.Drawing.Size(135, 20);
			rtfMatchValue.TabIndex = 11;
			rtfMatchValue.DoubleClick += new System.EventHandler(rtfMatchValue_DoubleClick);
			rtfMatchValue.Validated += new System.EventHandler(rtfMatchValue_Leave);
			rtfMatchValue2.Location = new System.Drawing.Point(451, 1);
			rtfMatchValue2.Name = "rtfMatchValue2";
			rtfMatchValue2.Size = new System.Drawing.Size(100, 20);
			rtfMatchValue2.TabIndex = 12;
			rtfMatchValue2.DoubleClick += new System.EventHandler(rtfMatchValue_DoubleClick);
			rtfMatchValue2.Validated += new System.EventHandler(rtfMatchValue2_Leave);
			btMatcher.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.SmallArrowDown;
			btMatcher.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			btMatcher.Location = new System.Drawing.Point(27, -1);
			btMatcher.Name = "btMatcher";
			btMatcher.Size = new System.Drawing.Size(136, 23);
			btMatcher.TabIndex = 13;
			btMatcher.Text = "Pages";
			btMatcher.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btMatcher.UseVisualStyleBackColor = true;
			btMatcher.Click += new System.EventHandler(btMatcher_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(btMatcher);
			base.Controls.Add(btEdit);
			base.Controls.Add(rtfMatchValue2);
			base.Controls.Add(rtfMatchValue);
			base.Controls.Add(chkNot);
			base.Controls.Add(cbOperator);
			base.Controls.Add(lblDescription);
			base.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			base.Name = "MatcherEditor";
			base.Size = new System.Drawing.Size(589, 50);
			cmEdit.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
