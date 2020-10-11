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
	public class MatcherGroupEditor : UserControl, IMatcherEditor
	{
		private static TR TR = TR.Load("SmartListDialog");

		private ComicBookGroupMatcher currentComicBookMatcher;

		private readonly ComicBookMatcherCollection matchers;

		private readonly int level;

		private readonly string rulesText;

		private IContainer components;

		private FlowLayoutPanel matcherControls;

		private ComboBox cbMatchMode;

		private CheckBox chkNot;

		private Label labelSubRules;

		private ToolTip toolTip;

		private Button btEdit;

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

		private ToolStripMenuItem miDelete;

		private CheckBox chkExpanded;

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

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				currentComicBookMatcher.Matchers.Changed -= OwnMatchers_Changed;
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private void cmEdit_Opening(object sender, CancelEventArgs e)
		{
			miNewGroup.Enabled = level <= 5;
			ToolStripMenuItem toolStripMenuItem = miDelete;
			bool enabled = (miCut.Enabled = matchers.Count > 1);
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
			Clipboard.SetData("ComicBookMatcher", currentComicBookMatcher);
		}

		public void CutClipboard()
		{
			Clipboard.SetData("ComicBookMatcher", currentComicBookMatcher);
			matchers.Remove(currentComicBookMatcher);
		}

		public void PasteClipboard()
		{
			ComicBookMatcher comicBookMatcher = Clipboard.GetData("ComicBookMatcher") as ComicBookMatcher;
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

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			matcherControls = new System.Windows.Forms.FlowLayoutPanel();
			cbMatchMode = new System.Windows.Forms.ComboBox();
			chkNot = new System.Windows.Forms.CheckBox();
			labelSubRules = new System.Windows.Forms.Label();
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
			chkExpanded = new System.Windows.Forms.CheckBox();
			cmEdit.SuspendLayout();
			SuspendLayout();
			matcherControls.AutoSize = true;
			matcherControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			matcherControls.BackColor = System.Drawing.SystemColors.Control;
			matcherControls.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			matcherControls.Location = new System.Drawing.Point(10, 25);
			matcherControls.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			matcherControls.MinimumSize = new System.Drawing.Size(400, 20);
			matcherControls.Name = "matcherControls";
			matcherControls.Size = new System.Drawing.Size(400, 20);
			matcherControls.TabIndex = 6;
			cbMatchMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbMatchMode.FormattingEnabled = true;
			cbMatchMode.Location = new System.Drawing.Point(26, 0);
			cbMatchMode.Name = "cbMatchMode";
			cbMatchMode.Size = new System.Drawing.Size(137, 21);
			cbMatchMode.TabIndex = 1;
			cbMatchMode.SelectedIndexChanged += new System.EventHandler(cbMatchMode_SelectedIndexChanged);
			chkNot.Appearance = System.Windows.Forms.Appearance.Button;
			chkNot.Location = new System.Drawing.Point(0, 0);
			chkNot.Name = "chkNot";
			chkNot.Size = new System.Drawing.Size(21, 21);
			chkNot.TabIndex = 0;
			chkNot.Text = "!";
			chkNot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			chkNot.UseVisualStyleBackColor = true;
			chkNot.Click += new System.EventHandler(chkNot_CheckedChanged);
			labelSubRules.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			labelSubRules.AutoEllipsis = true;
			labelSubRules.Location = new System.Drawing.Point(169, 4);
			labelSubRules.Name = "labelSubRules";
			labelSubRules.Size = new System.Drawing.Size(366, 13);
			labelSubRules.TabIndex = 2;
			labelSubRules.Text = "of the following rules:";
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
			miPaste.ShortcutKeys = System.Windows.Forms.Keys.P | System.Windows.Forms.Keys.Control;
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
			btEdit.Size = new System.Drawing.Size(21, 22);
			btEdit.TabIndex = 11;
			btEdit.UseVisualStyleBackColor = true;
			btEdit.Click += new System.EventHandler(btEdit_Click);
			chkExpanded.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkExpanded.Appearance = System.Windows.Forms.Appearance.Button;
			chkExpanded.Checked = true;
			chkExpanded.CheckState = System.Windows.Forms.CheckState.Checked;
			chkExpanded.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.DoubleArrow;
			chkExpanded.Location = new System.Drawing.Point(543, 0);
			chkExpanded.Name = "chkExpanded";
			chkExpanded.Size = new System.Drawing.Size(22, 22);
			chkExpanded.TabIndex = 12;
			chkExpanded.UseVisualStyleBackColor = true;
			chkExpanded.CheckedChanged += new System.EventHandler(chkCollapse_CheckedChanged);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			AutoSize = true;
			BackColor = System.Drawing.SystemColors.Control;
			base.Controls.Add(chkExpanded);
			base.Controls.Add(matcherControls);
			base.Controls.Add(labelSubRules);
			base.Controls.Add(chkNot);
			base.Controls.Add(cbMatchMode);
			base.Controls.Add(btEdit);
			base.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			MinimumSize = new System.Drawing.Size(400, 0);
			base.Name = "MatcherGroupEditor";
			base.Size = new System.Drawing.Size(589, 48);
			cmEdit.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
