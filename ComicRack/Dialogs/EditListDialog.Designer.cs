using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class EditListDialog
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
			labelName = new System.Windows.Forms.Label();
			txtName = new System.Windows.Forms.TextBox();
			cbCombineMode = new System.Windows.Forms.ComboBox();
			labelBooks = new System.Windows.Forms.Label();
			txtNotes = new System.Windows.Forms.TextBox();
			labelNotes = new System.Windows.Forms.Label();
			chkShowNotes = new System.Windows.Forms.CheckBox();
			flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			panel1 = new System.Windows.Forms.Panel();
			panelBooks = new System.Windows.Forms.Panel();
			panelNotes = new System.Windows.Forms.Panel();
			chkQuickOpen = new System.Windows.Forms.CheckBox();
			bottomPanel = new System.Windows.Forms.Panel();
			btOK = new System.Windows.Forms.Button();
			btCancel = new System.Windows.Forms.Button();
			flowLayoutPanel1.SuspendLayout();
			panel1.SuspendLayout();
			panelBooks.SuspendLayout();
			panelNotes.SuspendLayout();
			bottomPanel.SuspendLayout();
			SuspendLayout();
			labelName.Location = new System.Drawing.Point(0, 10);
			labelName.Name = "labelName";
			labelName.Size = new System.Drawing.Size(62, 13);
			labelName.TabIndex = 0;
			labelName.Text = "Name:";
			labelName.TextAlign = System.Drawing.ContentAlignment.TopRight;
			txtName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txtName.Location = new System.Drawing.Point(68, 7);
			txtName.Name = "txtName";
			txtName.Size = new System.Drawing.Size(526, 20);
			txtName.TabIndex = 1;
			cbCombineMode.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			cbCombineMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbCombineMode.FormattingEnabled = true;
			cbCombineMode.Location = new System.Drawing.Point(68, 3);
			cbCombineMode.Name = "cbCombineMode";
			cbCombineMode.Size = new System.Drawing.Size(554, 21);
			cbCombineMode.TabIndex = 1;
			labelBooks.Location = new System.Drawing.Point(0, 9);
			labelBooks.Name = "labelBooks";
			labelBooks.Size = new System.Drawing.Size(62, 13);
			labelBooks.TabIndex = 0;
			labelBooks.Text = "Books:";
			labelBooks.TextAlign = System.Drawing.ContentAlignment.TopRight;
			txtNotes.AcceptsReturn = true;
			txtNotes.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txtNotes.Location = new System.Drawing.Point(68, 0);
			txtNotes.Multiline = true;
			txtNotes.Name = "txtNotes";
			txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			txtNotes.Size = new System.Drawing.Size(554, 101);
			txtNotes.TabIndex = 1;
			labelNotes.Location = new System.Drawing.Point(0, 0);
			labelNotes.Name = "labelNotes";
			labelNotes.Size = new System.Drawing.Size(62, 13);
			labelNotes.TabIndex = 0;
			labelNotes.Text = "Notes:";
			labelNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
			chkShowNotes.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkShowNotes.Appearance = System.Windows.Forms.Appearance.Button;
			chkShowNotes.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.DoubleArrow;
			chkShowNotes.Location = new System.Drawing.Point(600, 6);
			chkShowNotes.Name = "chkShowNotes";
			chkShowNotes.Size = new System.Drawing.Size(22, 22);
			chkShowNotes.TabIndex = 2;
			chkShowNotes.UseVisualStyleBackColor = true;
			chkShowNotes.CheckedChanged += new System.EventHandler(chkShowNotes_CheckedChanged);
			flowLayoutPanel1.AutoSize = true;
			flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			flowLayoutPanel1.Controls.Add(panel1);
			flowLayoutPanel1.Controls.Add(panelBooks);
			flowLayoutPanel1.Controls.Add(panelNotes);
			flowLayoutPanel1.Controls.Add(bottomPanel);
			flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			flowLayoutPanel1.Name = "flowLayoutPanel1";
			flowLayoutPanel1.Size = new System.Drawing.Size(625, 217);
			flowLayoutPanel1.TabIndex = 9;
			panel1.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			panel1.AutoSize = true;
			panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel1.Controls.Add(labelName);
			panel1.Controls.Add(chkShowNotes);
			panel1.Controls.Add(txtName);
			panel1.Location = new System.Drawing.Point(0, 0);
			panel1.Margin = new System.Windows.Forms.Padding(0);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(625, 31);
			panel1.TabIndex = 0;
			panelBooks.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			panelBooks.Controls.Add(labelBooks);
			panelBooks.Controls.Add(cbCombineMode);
			panelBooks.Location = new System.Drawing.Point(0, 31);
			panelBooks.Margin = new System.Windows.Forms.Padding(0);
			panelBooks.Name = "panelBooks";
			panelBooks.Size = new System.Drawing.Size(625, 30);
			panelBooks.TabIndex = 1;
			panelNotes.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			panelNotes.AutoSize = true;
			panelNotes.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panelNotes.Controls.Add(chkQuickOpen);
			panelNotes.Controls.Add(labelNotes);
			panelNotes.Controls.Add(txtNotes);
			panelNotes.Location = new System.Drawing.Point(0, 61);
			panelNotes.Margin = new System.Windows.Forms.Padding(0);
			panelNotes.Name = "panelNotes";
			panelNotes.Size = new System.Drawing.Size(625, 127);
			panelNotes.TabIndex = 2;
			chkQuickOpen.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkQuickOpen.Location = new System.Drawing.Point(68, 107);
			chkQuickOpen.Name = "chkQuickOpen";
			chkQuickOpen.Size = new System.Drawing.Size(129, 17);
			chkQuickOpen.TabIndex = 2;
			chkQuickOpen.Text = "Show in Quick Open";
			chkQuickOpen.UseVisualStyleBackColor = true;
			chkQuickOpen.Visible = false;
			bottomPanel.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			bottomPanel.Controls.Add(btOK);
			bottomPanel.Controls.Add(btCancel);
			bottomPanel.Location = new System.Drawing.Point(0, 188);
			bottomPanel.Margin = new System.Windows.Forms.Padding(0);
			bottomPanel.Name = "bottomPanel";
			bottomPanel.Size = new System.Drawing.Size(625, 29);
			bottomPanel.TabIndex = 2;
			btOK.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(456, 3);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(80, 24);
			btOK.TabIndex = 0;
			btOK.Text = "&OK";
			btCancel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(542, 3);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 1;
			btCancel.Text = "&Cancel";
			base.AcceptButton = btOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			AutoSize = true;
			base.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(631, 223);
			base.Controls.Add(flowLayoutPanel1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "EditListDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Edit List";
			flowLayoutPanel1.ResumeLayout(false);
			flowLayoutPanel1.PerformLayout();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			panelBooks.ResumeLayout(false);
			panelNotes.ResumeLayout(false);
			panelNotes.PerformLayout();
			bottomPanel.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}
		
		private IContainer components;

		private Label labelName;

		private TextBox txtName;

		private ComboBox cbCombineMode;

		private Label labelBooks;

		private TextBox txtNotes;

		private Label labelNotes;

		private CheckBox chkShowNotes;

		private FlowLayoutPanel flowLayoutPanel1;

		private Panel panel1;

		private Panel panelBooks;

		private Panel panelNotes;

		private Panel bottomPanel;

		private Button btOK;

		private Button btCancel;

		private CheckBox chkQuickOpen;
	}
}
