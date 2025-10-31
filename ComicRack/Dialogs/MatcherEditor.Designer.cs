using System.Windows.Forms;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class MatcherEditor : UserControlEx, IMatcherEditor
	{
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.cbOperator = new System.Windows.Forms.ComboBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.chkNot = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.cmEdit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miNewRule = new System.Windows.Forms.ToolStripMenuItem();
            this.miNewGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.miDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miCut = new System.Windows.Forms.ToolStripMenuItem();
            this.miCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.miPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.miMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            this.miMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            this.btEdit = new System.Windows.Forms.Button();
            this.rtfMatchValue = new System.Windows.Forms.TextBox();
            this.rtfMatchValue2 = new System.Windows.Forms.TextBox();
            this.btMatcher = new System.Windows.Forms.Button();
            this.cmEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbOperator
            // 
            this.cbOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOperator.FormattingEnabled = true;
            this.cbOperator.Location = new System.Drawing.Point(169, 0);
            this.cbOperator.Name = "cbOperator";
            this.cbOperator.Size = new System.Drawing.Size(135, 21);
            this.cbOperator.TabIndex = 2;
            this.cbOperator.SelectedIndexChanged += new System.EventHandler(this.cbOperator_SelectedIndexChanged);
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(448, 25);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(60, 13);
            this.lblDescription.TabIndex = 7;
            this.lblDescription.Text = "Description";
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
            this.chkNot.CheckedChanged += new System.EventHandler(this.chkNot_CheckedChanged);
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
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
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
            this.miPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.miPaste.Size = new System.Drawing.Size(180, 22);
            this.miPaste.Text = "Paste";
            this.miPaste.Click += new System.EventHandler(this.miPaste_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(177, 6);
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
            // btEdit
            // 
            this.btEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btEdit.ContextMenuStrip = this.cmEdit;
            this.btEdit.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.SmallArrowDown;
            this.btEdit.Location = new System.Drawing.Point(568, 0);
            this.btEdit.Name = "btEdit";
            this.btEdit.Size = new System.Drawing.Size(21, 21);
            this.btEdit.TabIndex = 10;
            this.btEdit.UseVisualStyleBackColor = true;
            this.btEdit.Click += new System.EventHandler(this.btEdit_Click);
            // 
            // rtfMatchValue
            // 
            this.rtfMatchValue.Location = new System.Drawing.Point(310, 1);
            this.rtfMatchValue.Name = "rtfMatchValue";
            this.rtfMatchValue.Size = new System.Drawing.Size(135, 20);
            this.rtfMatchValue.TabIndex = 11;
            this.rtfMatchValue.DoubleClick += new System.EventHandler(this.rtfMatchValue_DoubleClick);
            this.rtfMatchValue.Validated += new System.EventHandler(this.rtfMatchValue_Leave);
            // 
            // rtfMatchValue2
            // 
            this.rtfMatchValue2.Location = new System.Drawing.Point(451, 1);
            this.rtfMatchValue2.Name = "rtfMatchValue2";
            this.rtfMatchValue2.Size = new System.Drawing.Size(100, 20);
            this.rtfMatchValue2.TabIndex = 12;
            this.rtfMatchValue2.DoubleClick += new System.EventHandler(this.rtfMatchValue_DoubleClick);
            this.rtfMatchValue2.Validated += new System.EventHandler(this.rtfMatchValue2_Leave);
            // 
            // btMatcher
            // 
            this.btMatcher.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.SmallArrowDown;
            this.btMatcher.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btMatcher.Location = new System.Drawing.Point(27, -1);
            this.btMatcher.Name = "btMatcher";
            this.btMatcher.Size = new System.Drawing.Size(136, 23);
            this.btMatcher.TabIndex = 13;
            this.btMatcher.Text = "Pages";
            this.btMatcher.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btMatcher.UseVisualStyleBackColor = true;
            this.btMatcher.Click += new System.EventHandler(this.btMatcher_Click);
            // 
            // MatcherEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btMatcher);
            this.Controls.Add(this.btEdit);
            this.Controls.Add(this.rtfMatchValue2);
            this.Controls.Add(this.rtfMatchValue);
            this.Controls.Add(this.chkNot);
            this.Controls.Add(this.cbOperator);
            this.Controls.Add(this.lblDescription);
            this.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.Name = "MatcherEditor";
            this.Size = new System.Drawing.Size(589, 50);
            this.cmEdit.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

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
    }
}
