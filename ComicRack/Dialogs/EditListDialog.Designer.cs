using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class EditListDialog
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
            this.labelName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.cbCombineMode = new System.Windows.Forms.ComboBox();
            this.labelBooks = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.labelNotes = new System.Windows.Forms.Label();
            this.chkShowNotes = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelBooks = new System.Windows.Forms.Panel();
            this.panelNotes = new System.Windows.Forms.Panel();
            this.chkQuickOpen = new System.Windows.Forms.CheckBox();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelBooks.SuspendLayout();
            this.panelNotes.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.Location = new System.Drawing.Point(0, 10);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(62, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Name:";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(68, 7);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(526, 20);
            this.txtName.TabIndex = 1;
            // 
            // cbCombineMode
            // 
            this.cbCombineMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCombineMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCombineMode.FormattingEnabled = true;
            this.cbCombineMode.Location = new System.Drawing.Point(68, 3);
            this.cbCombineMode.Name = "cbCombineMode";
            this.cbCombineMode.Size = new System.Drawing.Size(554, 21);
            this.cbCombineMode.TabIndex = 1;
            // 
            // labelBooks
            // 
            this.labelBooks.Location = new System.Drawing.Point(0, 9);
            this.labelBooks.Name = "labelBooks";
            this.labelBooks.Size = new System.Drawing.Size(62, 13);
            this.labelBooks.TabIndex = 0;
            this.labelBooks.Text = "Books:";
            this.labelBooks.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtNotes
            // 
            this.txtNotes.AcceptsReturn = true;
            this.txtNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotes.Location = new System.Drawing.Point(68, 0);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNotes.Size = new System.Drawing.Size(554, 101);
            this.txtNotes.TabIndex = 1;
            // 
            // labelNotes
            // 
            this.labelNotes.Location = new System.Drawing.Point(0, 0);
            this.labelNotes.Name = "labelNotes";
            this.labelNotes.Size = new System.Drawing.Size(62, 13);
            this.labelNotes.TabIndex = 0;
            this.labelNotes.Text = "Notes:";
            this.labelNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkShowNotes
            // 
            this.chkShowNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkShowNotes.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkShowNotes.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.DoubleArrow;
            this.chkShowNotes.Location = new System.Drawing.Point(600, 6);
            this.chkShowNotes.Name = "chkShowNotes";
            this.chkShowNotes.Size = new System.Drawing.Size(22, 22);
            this.chkShowNotes.TabIndex = 2;
            this.chkShowNotes.UseVisualStyleBackColor = true;
            this.chkShowNotes.CheckedChanged += new System.EventHandler(this.chkShowNotes_CheckedChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.panelBooks);
            this.flowLayoutPanel1.Controls.Add(this.panelNotes);
            this.flowLayoutPanel1.Controls.Add(this.bottomPanel);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(625, 217);
            this.flowLayoutPanel1.TabIndex = 9;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.labelName);
            this.panel1.Controls.Add(this.chkShowNotes);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(625, 31);
            this.panel1.TabIndex = 0;
            // 
            // panelBooks
            // 
            this.panelBooks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBooks.Controls.Add(this.labelBooks);
            this.panelBooks.Controls.Add(this.cbCombineMode);
            this.panelBooks.Location = new System.Drawing.Point(0, 31);
            this.panelBooks.Margin = new System.Windows.Forms.Padding(0);
            this.panelBooks.Name = "panelBooks";
            this.panelBooks.Size = new System.Drawing.Size(625, 30);
            this.panelBooks.TabIndex = 1;
            // 
            // panelNotes
            // 
            this.panelNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.panelNotes.AutoSize = true;
            this.panelNotes.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelNotes.Controls.Add(this.chkQuickOpen);
            this.panelNotes.Controls.Add(this.labelNotes);
            this.panelNotes.Controls.Add(this.txtNotes);
            this.panelNotes.Location = new System.Drawing.Point(0, 61);
            this.panelNotes.Margin = new System.Windows.Forms.Padding(0);
            this.panelNotes.Name = "panelNotes";
            this.panelNotes.Size = new System.Drawing.Size(625, 127);
            this.panelNotes.TabIndex = 2;
            // 
            // chkQuickOpen
            // 
            this.chkQuickOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkQuickOpen.Location = new System.Drawing.Point(68, 107);
            this.chkQuickOpen.Name = "chkQuickOpen";
            this.chkQuickOpen.Size = new System.Drawing.Size(129, 17);
            this.chkQuickOpen.TabIndex = 2;
            this.chkQuickOpen.Text = "Show in Quick Open";
            this.chkQuickOpen.UseVisualStyleBackColor = true;
            this.chkQuickOpen.Visible = false;
            // 
            // bottomPanel
            // 
            this.bottomPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.bottomPanel.Controls.Add(this.btOK);
            this.bottomPanel.Controls.Add(this.btCancel);
            this.bottomPanel.Location = new System.Drawing.Point(0, 188);
            this.bottomPanel.Margin = new System.Windows.Forms.Padding(0);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(625, 29);
            this.bottomPanel.TabIndex = 2;
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btOK.Location = new System.Drawing.Point(456, 3);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(80, 24);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "&OK";
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btCancel.Location = new System.Drawing.Point(542, 3);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(80, 24);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "&Cancel";
            // 
            // EditListDialog
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(631, 223);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditListDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit List";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelBooks.ResumeLayout(false);
            this.panelNotes.ResumeLayout(false);
            this.panelNotes.PerformLayout();
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		
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
