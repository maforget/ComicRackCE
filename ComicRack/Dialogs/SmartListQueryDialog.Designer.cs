using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class SmartListQueryDialog
	{
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
			components = new System.ComponentModel.Container();
			panel1 = new System.Windows.Forms.Panel();
			rtfQuery = new System.Windows.Forms.RichTextBox();
			cmEdit = new System.Windows.Forms.ContextMenuStrip(components);
			miUndo = new System.Windows.Forms.ToolStripMenuItem();
			miRedo = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			miCut = new System.Windows.Forms.ToolStripMenuItem();
			miCopy = new System.Windows.Forms.ToolStripMenuItem();
			miPaste = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			miSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			miQuickFormat = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			miInsertMatch = new System.Windows.Forms.ToolStripMenuItem();
			miInsertValue = new System.Windows.Forms.ToolStripMenuItem();
			btNext = new System.Windows.Forms.Button();
			btPrev = new System.Windows.Forms.Button();
			btOK = new System.Windows.Forms.Button();
			btCancel = new System.Windows.Forms.Button();
			btApply = new System.Windows.Forms.Button();
			labelStatus = new System.Windows.Forms.Label();
			labelPosition = new System.Windows.Forms.Label();
			colorizeTimer = new System.Windows.Forms.Timer(components);
			undoTimer = new System.Windows.Forms.Timer(components);
			btDesigner = new System.Windows.Forms.Button();
			panel1.SuspendLayout();
			cmEdit.SuspendLayout();
			SuspendLayout();
			panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			panel1.Controls.Add(rtfQuery);
			panel1.Location = new System.Drawing.Point(8, 10);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(573, 408);
			panel1.TabIndex = 0;
			rtfQuery.AcceptsTab = true;
			rtfQuery.BorderStyle = System.Windows.Forms.BorderStyle.None;
			rtfQuery.ContextMenuStrip = cmEdit;
			rtfQuery.DetectUrls = false;
			rtfQuery.Dock = System.Windows.Forms.DockStyle.Fill;
			rtfQuery.Font = new System.Drawing.Font("Courier New", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			rtfQuery.HideSelection = false;
			rtfQuery.Location = new System.Drawing.Point(0, 0);
			rtfQuery.Name = "rtfQuery";
			rtfQuery.Size = new System.Drawing.Size(571, 406);
			rtfQuery.TabIndex = 0;
			rtfQuery.Text = "";
			rtfQuery.WordWrap = false;
			rtfQuery.SelectionChanged += new System.EventHandler(rtfQuery_SelectionChanged);
			rtfQuery.TextChanged += new System.EventHandler(rtfQuery_TextChanged);
			rtfQuery.DoubleClick += new System.EventHandler(rtfQuery_DoubleClick);
			rtfQuery.KeyDown += new System.Windows.Forms.KeyEventHandler(rtfQuery_KeyDown);
			rtfQuery.KeyPress += new System.Windows.Forms.KeyPressEventHandler(rtfQuery_KeyPress);
			cmEdit.Items.AddRange(new System.Windows.Forms.ToolStripItem[13]
			{
				miUndo,
				miRedo,
				toolStripMenuItem3,
				miCut,
				miCopy,
				miPaste,
				toolStripMenuItem1,
				miSelectAll,
				toolStripMenuItem2,
				miQuickFormat,
				toolStripMenuItem4,
				miInsertMatch,
				miInsertValue
			});
			cmEdit.Name = "cmEdit";
			cmEdit.Size = new System.Drawing.Size(187, 226);
			cmEdit.Opening += new System.ComponentModel.CancelEventHandler(cmEdit_Opening);
			miUndo.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Undo;
			miUndo.Name = "miUndo";
			miUndo.ShortcutKeys = System.Windows.Forms.Keys.Z | System.Windows.Forms.Keys.Control;
			miUndo.Size = new System.Drawing.Size(186, 22);
			miUndo.Text = "Undo";
			miUndo.Click += new System.EventHandler(miUndo_Click);
			miRedo.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Redo;
			miRedo.Name = "miRedo";
			miRedo.ShortcutKeys = System.Windows.Forms.Keys.Y | System.Windows.Forms.Keys.Control;
			miRedo.Size = new System.Drawing.Size(186, 22);
			miRedo.Text = "Redo";
			miRedo.Click += new System.EventHandler(miRedo_Click);
			toolStripMenuItem3.Name = "toolStripMenuItem3";
			toolStripMenuItem3.Size = new System.Drawing.Size(183, 6);
			miCut.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Cut;
			miCut.Name = "miCut";
			miCut.ShortcutKeys = System.Windows.Forms.Keys.X | System.Windows.Forms.Keys.Control;
			miCut.Size = new System.Drawing.Size(186, 22);
			miCut.Text = "Cut";
			miCut.Click += new System.EventHandler(miCut_Click);
			miCopy.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Copy;
			miCopy.Name = "miCopy";
			miCopy.ShortcutKeys = System.Windows.Forms.Keys.C | System.Windows.Forms.Keys.Control;
			miCopy.Size = new System.Drawing.Size(186, 22);
			miCopy.Text = "Copy";
			miCopy.Click += new System.EventHandler(miCopy_Click);
			miPaste.Name = "miPaste";
			miPaste.ShortcutKeys = System.Windows.Forms.Keys.V | System.Windows.Forms.Keys.Control;
			miPaste.Size = new System.Drawing.Size(186, 22);
			miPaste.Text = "Paste";
			miPaste.Click += new System.EventHandler(miPaste_Click);
			toolStripMenuItem1.Name = "toolStripMenuItem1";
			toolStripMenuItem1.Size = new System.Drawing.Size(183, 6);
			miSelectAll.Name = "miSelectAll";
			miSelectAll.ShortcutKeys = System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control;
			miSelectAll.Size = new System.Drawing.Size(186, 22);
			miSelectAll.Text = "Select All";
			miSelectAll.Click += new System.EventHandler(miSelectAll_Click);
			toolStripMenuItem2.Name = "toolStripMenuItem2";
			toolStripMenuItem2.Size = new System.Drawing.Size(183, 6);
			miQuickFormat.Name = "miQuickFormat";
			miQuickFormat.ShortcutKeys = System.Windows.Forms.Keys.F | System.Windows.Forms.Keys.Control;
			miQuickFormat.Size = new System.Drawing.Size(186, 22);
			miQuickFormat.Text = "Quick Format";
			miQuickFormat.Click += new System.EventHandler(miQuickFormat_Click);
			toolStripMenuItem4.Name = "toolStripMenuItem4";
			toolStripMenuItem4.Size = new System.Drawing.Size(183, 6);
			miInsertMatch.Name = "miInsertMatch";
			miInsertMatch.Size = new System.Drawing.Size(186, 22);
			miInsertMatch.Text = "Insert Match";
			miInsertValue.Name = "miInsertValue";
			miInsertValue.Size = new System.Drawing.Size(186, 22);
			miInsertValue.Text = "Insert Value";
			btNext.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btNext.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btNext.Location = new System.Drawing.Point(95, 451);
			btNext.Name = "btNext";
			btNext.Size = new System.Drawing.Size(80, 24);
			btNext.TabIndex = 3;
			btNext.Text = "&Next";
			btNext.Click += new System.EventHandler(btNext_Click);
			btPrev.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btPrev.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btPrev.Location = new System.Drawing.Point(9, 451);
			btPrev.Name = "btPrev";
			btPrev.Size = new System.Drawing.Size(80, 24);
			btPrev.TabIndex = 2;
			btPrev.Text = "&Previous";
			btPrev.Click += new System.EventHandler(btPrev_Click);
			btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(331, 451);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(80, 24);
			btOK.TabIndex = 5;
			btOK.Text = "&OK";
			btOK.Click += new System.EventHandler(btOK_Click);
			btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(417, 451);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 6;
			btCancel.Text = "&Cancel";
			btApply.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btApply.Location = new System.Drawing.Point(503, 451);
			btApply.Name = "btApply";
			btApply.Size = new System.Drawing.Size(80, 24);
			btApply.TabIndex = 7;
			btApply.Text = "&Apply";
			btApply.Click += new System.EventHandler(btApply_Click);
			labelStatus.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			labelStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			labelStatus.Location = new System.Drawing.Point(8, 421);
			labelStatus.Name = "labelStatus";
			labelStatus.Size = new System.Drawing.Size(523, 20);
			labelStatus.TabIndex = 0;
			labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			labelPosition.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			labelPosition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			labelPosition.Location = new System.Drawing.Point(532, 421);
			labelPosition.Name = "labelPosition";
			labelPosition.Size = new System.Drawing.Size(49, 20);
			labelPosition.TabIndex = 1;
			labelPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			colorizeTimer.Interval = 500;
			colorizeTimer.Tick += new System.EventHandler(colorizeTimer_Tick);
			undoTimer.Enabled = true;
			undoTimer.Interval = 1000;
			undoTimer.Tick += new System.EventHandler(undoTimer_Tick);
			btDesigner.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btDesigner.DialogResult = System.Windows.Forms.DialogResult.Retry;
			btDesigner.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btDesigner.Location = new System.Drawing.Point(181, 451);
			btDesigner.Name = "btDesigner";
			btDesigner.Size = new System.Drawing.Size(80, 24);
			btDesigner.TabIndex = 4;
			btDesigner.Text = "&Designer";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(591, 487);
			base.Controls.Add(btDesigner);
			base.Controls.Add(labelPosition);
			base.Controls.Add(labelStatus);
			base.Controls.Add(btNext);
			base.Controls.Add(btPrev);
			base.Controls.Add(btOK);
			base.Controls.Add(btCancel);
			base.Controls.Add(btApply);
			base.Controls.Add(panel1);
			base.MinimizeBox = false;
			MinimumSize = new System.Drawing.Size(500, 400);
			base.Name = "SmartListQueryDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Edit Smart List";
			panel1.ResumeLayout(false);
			cmEdit.ResumeLayout(false);
			ResumeLayout(false);
		}
		
		private IContainer components;

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
