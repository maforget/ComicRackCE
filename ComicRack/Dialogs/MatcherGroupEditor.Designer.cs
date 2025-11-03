using System.Windows.Forms;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme.Resources;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class MatcherGroupEditor : UserControlEx, IMatcherEditor
	{
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.matcherControls = new System.Windows.Forms.FlowLayoutPanel();
            this.cbMatchMode = new System.Windows.Forms.ComboBox();
            this.chkNot = new System.Windows.Forms.CheckBox();
            this.labelSubRules = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.cmEdit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.chkExpanded = new System.Windows.Forms.CheckBox();
            this.btEdit = new System.Windows.Forms.Button();
            this.miNewRule = new System.Windows.Forms.ToolStripMenuItem();
            this.miNewGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.miDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.miCut = new System.Windows.Forms.ToolStripMenuItem();
            this.miCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.miPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.miMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            this.miMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            this.cmEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // matcherControls
            // 
            this.matcherControls.AutoSize = true;
            this.matcherControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.matcherControls.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.matcherControls.Location = new System.Drawing.Point(10, 25);
            this.matcherControls.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.matcherControls.MinimumSize = new System.Drawing.Size(400, 20);
            this.matcherControls.Name = "matcherControls";
            this.matcherControls.Size = new System.Drawing.Size(400, 20);
            this.matcherControls.TabIndex = 6;
            // 
            // cbMatchMode
            // 
            this.cbMatchMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMatchMode.FormattingEnabled = true;
            this.cbMatchMode.Location = new System.Drawing.Point(26, 0);
            this.cbMatchMode.Name = "cbMatchMode";
            this.cbMatchMode.Size = new System.Drawing.Size(137, 21);
            this.cbMatchMode.TabIndex = 1;
            this.cbMatchMode.SelectedIndexChanged += new System.EventHandler(this.cbMatchMode_SelectedIndexChanged);
            // 
            // chkNot
            // 
            this.chkNot.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkNot.Location = new System.Drawing.Point(0, 0);
            this.chkNot.Name = "chkNot";
            this.chkNot.Size = new System.Drawing.Size(21, 21);
            this.chkNot.TabIndex = 0;
            this.chkNot.Text = "!";
            this.chkNot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkNot.UseVisualStyleBackColor = true;
            this.chkNot.Click += new System.EventHandler(this.chkNot_CheckedChanged);
            // 
            // labelSubRules
            // 
            this.labelSubRules.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSubRules.AutoEllipsis = true;
            this.labelSubRules.Location = new System.Drawing.Point(169, 4);
            this.labelSubRules.Name = "labelSubRules";
            this.labelSubRules.Size = new System.Drawing.Size(366, 13);
            this.labelSubRules.TabIndex = 2;
            this.labelSubRules.Text = "of the following rules:";
            // 
            // cmEdit
            // 
            this.cmEdit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miNewRule,
            this.miNewGroup,
            this.miDelete,
            this.toolStripMenuItem1,
            this.miCut,
            this.miCopy,
            this.miPaste,
            this.toolStripMenuItem2,
            this.miMoveUp,
            this.miMoveDown});
            this.cmEdit.Name = "cmEdit";
            this.cmEdit.Size = new System.Drawing.Size(181, 192);
            this.cmEdit.Opening += new System.ComponentModel.CancelEventHandler(this.cmEdit_Opening);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(177, 6);
            // 
            // chkExpanded
            // 
            this.chkExpanded.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkExpanded.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkExpanded.Checked = true;
            this.chkExpanded.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExpanded.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.DoubleArrow;
            this.chkExpanded.Location = new System.Drawing.Point(543, 0);
            this.chkExpanded.Name = "chkExpanded";
            this.chkExpanded.Size = new System.Drawing.Size(22, 22);
            this.chkExpanded.TabIndex = 12;
            this.chkExpanded.UseVisualStyleBackColor = true;
            this.chkExpanded.CheckedChanged += new System.EventHandler(this.chkCollapse_CheckedChanged);
            // 
            // btEdit
            // 
            this.btEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btEdit.ContextMenuStrip = this.cmEdit;
            this.btEdit.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.SmallArrowDown;
            this.btEdit.Location = new System.Drawing.Point(568, 0);
            this.btEdit.Name = "btEdit";
            this.btEdit.Size = new System.Drawing.Size(21, 22);
            this.btEdit.TabIndex = 11;
            this.btEdit.UseVisualStyleBackColor = true;
            this.btEdit.Click += new System.EventHandler(this.btEdit_Click);
            // 
            // miNewRule
            // 
            this.miNewRule.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.AddTab;
            this.miNewRule.Name = "miNewRule";
            this.miNewRule.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.miNewRule.Size = new System.Drawing.Size(180, 22);
            this.miNewRule.Text = "New Rule";
            this.miNewRule.Click += new System.EventHandler(this.miNewRule_Click);
            // 
            // miNewGroup
            // 
            this.miNewGroup.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Group;
            this.miNewGroup.Name = "miNewGroup";
            this.miNewGroup.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.miNewGroup.Size = new System.Drawing.Size(180, 22);
            this.miNewGroup.Text = "New Group";
            this.miNewGroup.Click += new System.EventHandler(this.miNewGroup_Click);
            // 
            // miDelete
            // 
            this.miDelete.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.EditDelete;
            this.miDelete.Name = "miDelete";
            this.miDelete.Size = new System.Drawing.Size(180, 22);
            this.miDelete.Text = "Delete";
            this.miDelete.Click += new System.EventHandler(this.miDelete_Click);
            // 
            // miCut
            // 
            this.miCut.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Cut;
            this.miCut.Name = "miCut";
            this.miCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.miCut.Size = new System.Drawing.Size(180, 22);
            this.miCut.Text = "Cut";
            this.miCut.Click += new System.EventHandler(this.miCut_Click);
            // 
            // miCopy
            // 
            this.miCopy.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.EditCopy;
            this.miCopy.Name = "miCopy";
            this.miCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.miCopy.Size = new System.Drawing.Size(180, 22);
            this.miCopy.Text = "Copy";
            this.miCopy.Click += new System.EventHandler(this.miCopy_Click);
            // 
            // miPaste
            // 
            this.miPaste.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.EditPaste;
            this.miPaste.Name = "miPaste";
            this.miPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.miPaste.Size = new System.Drawing.Size(180, 22);
            this.miPaste.Text = "Paste";
            this.miPaste.Click += new System.EventHandler(this.miPaste_Click);
            // 
            // miMoveUp
            // 
            this.miMoveUp.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GroupUp;
            this.miMoveUp.Name = "miMoveUp";
            this.miMoveUp.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.miMoveUp.Size = new System.Drawing.Size(180, 22);
            this.miMoveUp.Text = "Move Up";
            this.miMoveUp.Click += new System.EventHandler(this.miMoveUp_Click);
            // 
            // miMoveDown
            // 
            this.miMoveDown.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GroupDown;
            this.miMoveDown.Name = "miMoveDown";
            this.miMoveDown.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.miMoveDown.Size = new System.Drawing.Size(180, 22);
            this.miMoveDown.Text = "Move Down";
            this.miMoveDown.Click += new System.EventHandler(this.miMoveDown_Click);
            // 
            // MatcherGroupEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.chkExpanded);
            this.Controls.Add(this.matcherControls);
            this.Controls.Add(this.labelSubRules);
            this.Controls.Add(this.chkNot);
            this.Controls.Add(this.cbMatchMode);
            this.Controls.Add(this.btEdit);
            this.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.MinimumSize = new System.Drawing.Size(400, 0);
            this.Name = "MatcherGroupEditor";
            this.Size = new System.Drawing.Size(589, 48);
            this.cmEdit.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

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
    }
}
