using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class SmartListQueryDialog
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.rtfQuery = new System.Windows.Forms.RichTextBox();
            this.cmEdit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.miRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.miCut = new System.Windows.Forms.ToolStripMenuItem();
            this.miCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.miPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.miQuickFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.miInsertMatch = new System.Windows.Forms.ToolStripMenuItem();
            this.miInsertValue = new System.Windows.Forms.ToolStripMenuItem();
            this.btNext = new System.Windows.Forms.Button();
            this.btPrev = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.btApply = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelPosition = new System.Windows.Forms.Label();
            this.colorizeTimer = new System.Windows.Forms.Timer(this.components);
            this.undoTimer = new System.Windows.Forms.Timer(this.components);
            this.btDesigner = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.cmEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.rtfQuery);
            this.panel1.Location = new System.Drawing.Point(8, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(573, 408);
            this.panel1.TabIndex = 0;
            // 
            // rtfQuery
            // 
            this.rtfQuery.AcceptsTab = true;
            this.rtfQuery.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtfQuery.ContextMenuStrip = this.cmEdit;
            this.rtfQuery.DetectUrls = false;
            this.rtfQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtfQuery.Font = queryFont.Default;
            this.rtfQuery.HideSelection = false;
            this.rtfQuery.Location = new System.Drawing.Point(0, 0);
            this.rtfQuery.Name = "rtfQuery";
            this.rtfQuery.Size = new System.Drawing.Size(571, 406);
            this.rtfQuery.TabIndex = 0;
            this.rtfQuery.Text = "";
            this.rtfQuery.WordWrap = false;
            this.rtfQuery.SelectionChanged += new System.EventHandler(this.rtfQuery_SelectionChanged);
            this.rtfQuery.TextChanged += new System.EventHandler(this.rtfQuery_TextChanged);
            this.rtfQuery.DoubleClick += new System.EventHandler(this.rtfQuery_DoubleClick);
            this.rtfQuery.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rtfQuery_KeyDown);
            this.rtfQuery.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rtfQuery_KeyPress);
            // 
            // cmEdit
            // 
            this.cmEdit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miUndo,
            this.miRedo,
            this.toolStripMenuItem3,
            this.miCut,
            this.miCopy,
            this.miPaste,
            this.toolStripMenuItem1,
            this.miSelectAll,
            this.toolStripMenuItem2,
            this.miQuickFormat,
            this.toolStripMenuItem4,
            this.miInsertMatch,
            this.miInsertValue});
            this.cmEdit.Name = "cmEdit";
            this.cmEdit.Size = new System.Drawing.Size(187, 226);
            this.cmEdit.Opening += new System.ComponentModel.CancelEventHandler(this.cmEdit_Opening);
            // 
            // miUndo
            // 
            this.miUndo.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Undo;
            this.miUndo.Name = "miUndo";
            this.miUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.miUndo.Size = new System.Drawing.Size(186, 22);
            this.miUndo.Text = "Undo";
            this.miUndo.Click += new System.EventHandler(this.miUndo_Click);
            // 
            // miRedo
            // 
            this.miRedo.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Redo;
            this.miRedo.Name = "miRedo";
            this.miRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.miRedo.Size = new System.Drawing.Size(186, 22);
            this.miRedo.Text = "Redo";
            this.miRedo.Click += new System.EventHandler(this.miRedo_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(183, 6);
            // 
            // miCut
            // 
            this.miCut.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Cut;
            this.miCut.Name = "miCut";
            this.miCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.miCut.Size = new System.Drawing.Size(186, 22);
            this.miCut.Text = "Cut";
            this.miCut.Click += new System.EventHandler(this.miCut_Click);
            // 
            // miCopy
            // 
            this.miCopy.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Copy;
            this.miCopy.Name = "miCopy";
            this.miCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.miCopy.Size = new System.Drawing.Size(186, 22);
            this.miCopy.Text = "Copy";
            this.miCopy.Click += new System.EventHandler(this.miCopy_Click);
            // 
            // miPaste
            // 
            this.miPaste.Name = "miPaste";
            this.miPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.miPaste.Size = new System.Drawing.Size(186, 22);
            this.miPaste.Text = "Paste";
            this.miPaste.Click += new System.EventHandler(this.miPaste_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(183, 6);
            // 
            // miSelectAll
            // 
            this.miSelectAll.Name = "miSelectAll";
            this.miSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.miSelectAll.Size = new System.Drawing.Size(186, 22);
            this.miSelectAll.Text = "Select All";
            this.miSelectAll.Click += new System.EventHandler(this.miSelectAll_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(183, 6);
            // 
            // miQuickFormat
            // 
            this.miQuickFormat.Name = "miQuickFormat";
            this.miQuickFormat.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.miQuickFormat.Size = new System.Drawing.Size(186, 22);
            this.miQuickFormat.Text = "Quick Format";
            this.miQuickFormat.Click += new System.EventHandler(this.miQuickFormat_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(183, 6);
            // 
            // miInsertMatch
            // 
            this.miInsertMatch.Name = "miInsertMatch";
            this.miInsertMatch.Size = new System.Drawing.Size(186, 22);
            this.miInsertMatch.Text = "Insert Match";
            // 
            // miInsertValue
            // 
            this.miInsertValue.Name = "miInsertValue";
            this.miInsertValue.Size = new System.Drawing.Size(186, 22);
            this.miInsertValue.Text = "Insert Value";
            // 
            // btNext
            // 
            this.btNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btNext.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btNext.Location = new System.Drawing.Point(95, 451);
            this.btNext.Name = "btNext";
            this.btNext.Size = new System.Drawing.Size(80, 24);
            this.btNext.TabIndex = 3;
            this.btNext.Text = "&Next";
            this.btNext.Click += new System.EventHandler(this.btNext_Click);
            // 
            // btPrev
            // 
            this.btPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btPrev.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btPrev.Location = new System.Drawing.Point(9, 451);
            this.btPrev.Name = "btPrev";
            this.btPrev.Size = new System.Drawing.Size(80, 24);
            this.btPrev.TabIndex = 2;
            this.btPrev.Text = "&Previous";
            this.btPrev.Click += new System.EventHandler(this.btPrev_Click);
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btOK.Location = new System.Drawing.Point(331, 451);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(80, 24);
            this.btOK.TabIndex = 5;
            this.btOK.Text = "&OK";
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btCancel.Location = new System.Drawing.Point(417, 451);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(80, 24);
            this.btCancel.TabIndex = 6;
            this.btCancel.Text = "&Cancel";
            // 
            // btApply
            // 
            this.btApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btApply.Location = new System.Drawing.Point(503, 451);
            this.btApply.Name = "btApply";
            this.btApply.Size = new System.Drawing.Size(80, 24);
            this.btApply.TabIndex = 7;
            this.btApply.Text = "&Apply";
            this.btApply.Click += new System.EventHandler(this.btApply_Click);
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelStatus.Location = new System.Drawing.Point(8, 421);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(523, 20);
            this.labelStatus.TabIndex = 0;
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelPosition
            // 
            this.labelPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPosition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelPosition.Location = new System.Drawing.Point(532, 421);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(49, 20);
            this.labelPosition.TabIndex = 1;
            this.labelPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // colorizeTimer
            // 
            this.colorizeTimer.Interval = 500;
            this.colorizeTimer.Tick += new System.EventHandler(this.colorizeTimer_Tick);
            // 
            // undoTimer
            // 
            this.undoTimer.Enabled = true;
            this.undoTimer.Interval = 1000;
            this.undoTimer.Tick += new System.EventHandler(this.undoTimer_Tick);
            // 
            // btDesigner
            // 
            this.btDesigner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btDesigner.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.btDesigner.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btDesigner.Location = new System.Drawing.Point(181, 451);
            this.btDesigner.Name = "btDesigner";
            this.btDesigner.Size = new System.Drawing.Size(80, 24);
            this.btDesigner.TabIndex = 4;
            this.btDesigner.Text = "&Designer";
            // 
            // SmartListQueryDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(591, 487);
            this.Controls.Add(this.btDesigner);
            this.Controls.Add(this.labelPosition);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.btNext);
            this.Controls.Add(this.btPrev);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btApply);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "SmartListQueryDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Smart List";
            this.panel1.ResumeLayout(false);
            this.cmEdit.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		
		private Panel panel1;
		private RichTextBox rtfQuery;
		private Button btNext;
		private Button btPrev;
		private Button btOK;
		private Button btCancel;
		private Button btApply;
		private Label labelStatus;
		private Label labelPosition;
		private Timer colorizeTimer;
		private Timer undoTimer;
		private ContextMenuStrip cmEdit;
		private ToolStripMenuItem miUndo;
		private ToolStripMenuItem miRedo;
		private ToolStripSeparator toolStripMenuItem3;
		private ToolStripMenuItem miCut;
		private ToolStripMenuItem miCopy;
		private ToolStripMenuItem miPaste;
		private ToolStripSeparator toolStripMenuItem1;
		private ToolStripMenuItem miSelectAll;
		private ToolStripSeparator toolStripMenuItem2;
		private ToolStripMenuItem miQuickFormat;
		private ToolStripSeparator toolStripMenuItem4;
		private ToolStripMenuItem miInsertMatch;
		private ToolStripMenuItem miInsertValue;
		private Button btDesigner;
	}
}
