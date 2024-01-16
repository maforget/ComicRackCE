using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class SmartListDialog
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
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.matcherControls = new System.Windows.Forms.FlowLayoutPanel();
            this.topPanel = new System.Windows.Forms.Panel();
            this.btFilterReset = new System.Windows.Forms.Button();
            this.labelFilterReset = new System.Windows.Forms.Label();
            this.chkShowNotes = new System.Windows.Forms.CheckBox();
            this.txLimit = new System.Windows.Forms.TextBox();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.labelNotes = new System.Windows.Forms.Label();
            this.chkLimit = new System.Windows.Forms.CheckBox();
            this.labelName = new System.Windows.Forms.Label();
            this.chkQuickOpen = new System.Windows.Forms.CheckBox();
            this.cbLimitType = new System.Windows.Forms.ComboBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.chkNotBaseList = new System.Windows.Forms.CheckBox();
            this.cbMatchMode = new System.Windows.Forms.ComboBox();
            this.cbBaseList = new System.Windows.Forms.ComboBox();
            this.labelMatch = new System.Windows.Forms.Label();
            this.labelRules = new System.Windows.Forms.Label();
            this.flowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.matcherGroup = new System.Windows.Forms.GroupBox();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.btQuery = new System.Windows.Forms.Button();
            this.btNext = new System.Windows.Forms.Button();
            this.btPrev = new System.Windows.Forms.Button();
            this.btApply = new System.Windows.Forms.Button();
            this.baseImages = new System.Windows.Forms.ImageList(this.components);
            this.topPanel.SuspendLayout();
            this.flowLayout.SuspendLayout();
            this.matcherGroup.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btCancel.Location = new System.Drawing.Point(435, 3);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(80, 24);
            this.btCancel.TabIndex = 4;
            this.btCancel.Text = "&Cancel";
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btOK.Location = new System.Drawing.Point(351, 3);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(80, 24);
            this.btOK.TabIndex = 3;
            this.btOK.Text = "&OK";
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // matcherControls
            // 
            this.matcherControls.AutoScroll = true;
            this.matcherControls.AutoSize = true;
            this.matcherControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.matcherControls.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.matcherControls.Location = new System.Drawing.Point(6, 41);
            this.matcherControls.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.matcherControls.MaximumSize = new System.Drawing.Size(0, 400);
            this.matcherControls.MinimumSize = new System.Drawing.Size(588, 20);
            this.matcherControls.Name = "matcherControls";
            this.matcherControls.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.matcherControls.Size = new System.Drawing.Size(588, 20);
            this.matcherControls.TabIndex = 0;
            this.matcherControls.WrapContents = false;
            // 
            // topPanel
            // 
            this.topPanel.AutoSize = true;
            this.topPanel.Controls.Add(this.btFilterReset);
            this.topPanel.Controls.Add(this.labelFilterReset);
            this.topPanel.Controls.Add(this.chkShowNotes);
            this.topPanel.Controls.Add(this.txLimit);
            this.topPanel.Controls.Add(this.txtNotes);
            this.topPanel.Controls.Add(this.labelNotes);
            this.topPanel.Controls.Add(this.chkLimit);
            this.topPanel.Controls.Add(this.labelName);
            this.topPanel.Controls.Add(this.chkQuickOpen);
            this.topPanel.Controls.Add(this.cbLimitType);
            this.topPanel.Controls.Add(this.txtName);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(3, 3);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(602, 125);
            this.topPanel.TabIndex = 0;
            // 
            // btFilterReset
            // 
            this.btFilterReset.Location = new System.Drawing.Point(344, 101);
            this.btFilterReset.Name = "btFilterReset";
            this.btFilterReset.Size = new System.Drawing.Size(90, 21);
            this.btFilterReset.TabIndex = 10;
            this.btFilterReset.Text = "Reset";
            this.btFilterReset.UseVisualStyleBackColor = true;
            this.btFilterReset.Click += new System.EventHandler(this.btFilterReset_Click);
            // 
            // labelFilterReset
            // 
            this.labelFilterReset.AutoSize = true;
            this.labelFilterReset.Location = new System.Drawing.Point(69, 105);
            this.labelFilterReset.Name = "labelFilterReset";
            this.labelFilterReset.Size = new System.Drawing.Size(269, 13);
            this.labelFilterReset.TabIndex = 9;
            this.labelFilterReset.Text = "Some Books have been manually removed from this list.";
            // 
            // chkShowNotes
            // 
            this.chkShowNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkShowNotes.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkShowNotes.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.DoubleArrow;
            this.chkShowNotes.Location = new System.Drawing.Point(574, 0);
            this.chkShowNotes.Name = "chkShowNotes";
            this.chkShowNotes.Size = new System.Drawing.Size(22, 22);
            this.chkShowNotes.TabIndex = 2;
            this.chkShowNotes.UseVisualStyleBackColor = true;
            this.chkShowNotes.CheckedChanged += new System.EventHandler(this.chkShowNotes_CheckedChanged);
            // 
            // txLimit
            // 
            this.txLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txLimit.Enabled = false;
            this.txLimit.Location = new System.Drawing.Point(443, 74);
            this.txLimit.Name = "txLimit";
            this.txLimit.Size = new System.Drawing.Size(45, 20);
            this.txLimit.TabIndex = 7;
            this.txLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txLimit.Visible = false;
            this.txLimit.TextChanged += new System.EventHandler(this.txLimit_TextChanged);
            // 
            // txtNotes
            // 
            this.txtNotes.AcceptsReturn = true;
            this.txtNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotes.Location = new System.Drawing.Point(68, 26);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNotes.Size = new System.Drawing.Size(366, 69);
            this.txtNotes.TabIndex = 4;
            this.txtNotes.Visible = false;
            this.txtNotes.TextChanged += new System.EventHandler(this.txtNotes_TextChanged);
            // 
            // labelNotes
            // 
            this.labelNotes.Location = new System.Drawing.Point(0, 26);
            this.labelNotes.Name = "labelNotes";
            this.labelNotes.Size = new System.Drawing.Size(62, 13);
            this.labelNotes.TabIndex = 3;
            this.labelNotes.Text = "Notes:";
            this.labelNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelNotes.Visible = false;
            // 
            // chkLimit
            // 
            this.chkLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkLimit.Location = new System.Drawing.Point(443, 51);
            this.chkLimit.Name = "chkLimit";
            this.chkLimit.Size = new System.Drawing.Size(129, 17);
            this.chkLimit.TabIndex = 6;
            this.chkLimit.Text = "Limit to ";
            this.chkLimit.UseVisualStyleBackColor = true;
            this.chkLimit.Visible = false;
            this.chkLimit.CheckedChanged += new System.EventHandler(this.chkLimit_CheckedChanged);
            // 
            // labelName
            // 
            this.labelName.Location = new System.Drawing.Point(3, 4);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(59, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Name:";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkQuickOpen
            // 
            this.chkQuickOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkQuickOpen.Location = new System.Drawing.Point(443, 28);
            this.chkQuickOpen.Name = "chkQuickOpen";
            this.chkQuickOpen.Size = new System.Drawing.Size(129, 17);
            this.chkQuickOpen.TabIndex = 5;
            this.chkQuickOpen.Text = "Show in Quick Open";
            this.chkQuickOpen.UseVisualStyleBackColor = true;
            this.chkQuickOpen.Visible = false;
            this.chkQuickOpen.CheckedChanged += new System.EventHandler(this.chkQuickOpen_CheckedChanged);
            // 
            // cbLimitType
            // 
            this.cbLimitType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLimitType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLimitType.Enabled = false;
            this.cbLimitType.FormattingEnabled = true;
            this.cbLimitType.Items.AddRange(new object[] {
            "Books",
            "MB",
            "GB"});
            this.cbLimitType.Location = new System.Drawing.Point(494, 74);
            this.cbLimitType.Name = "cbLimitType";
            this.cbLimitType.Size = new System.Drawing.Size(78, 21);
            this.cbLimitType.TabIndex = 8;
            this.cbLimitType.Visible = false;
            this.cbLimitType.SelectedIndexChanged += new System.EventHandler(this.cbLimitType_SelectedIndexChanged);
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(68, 1);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(504, 20);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // chkNotBaseList
            // 
            this.chkNotBaseList.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkNotBaseList.Location = new System.Drawing.Point(6, 16);
            this.chkNotBaseList.Name = "chkNotBaseList";
            this.chkNotBaseList.Size = new System.Drawing.Size(21, 23);
            this.chkNotBaseList.TabIndex = 2;
            this.chkNotBaseList.Text = "!";
            this.chkNotBaseList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkNotBaseList.UseVisualStyleBackColor = true;
            this.chkNotBaseList.CheckedChanged += new System.EventHandler(this.chkNotBaseList_CheckedChanged);
            // 
            // cbMatchMode
            // 
            this.cbMatchMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMatchMode.FormattingEnabled = true;
            this.cbMatchMode.Location = new System.Drawing.Point(103, 18);
            this.cbMatchMode.Name = "cbMatchMode";
            this.cbMatchMode.Size = new System.Drawing.Size(65, 21);
            this.cbMatchMode.TabIndex = 4;
            this.cbMatchMode.SelectedIndexChanged += new System.EventHandler(this.cbMatchMode_SelectedIndexChanged);
            // 
            // cbBaseList
            // 
            this.cbBaseList.DisplayMember = "FullName";
            this.cbBaseList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBaseList.Location = new System.Drawing.Point(317, 16);
            this.cbBaseList.Name = "cbBaseList";
            this.cbBaseList.Size = new System.Drawing.Size(255, 21);
            this.cbBaseList.TabIndex = 6;
            this.cbBaseList.ValueMember = "FullName";
            this.cbBaseList.SelectedIndexChanged += new System.EventHandler(this.cbBaseList_SelectedIndexChanged);
            // 
            // labelMatch
            // 
            this.labelMatch.Location = new System.Drawing.Point(33, 21);
            this.labelMatch.Name = "labelMatch";
            this.labelMatch.Size = new System.Drawing.Size(69, 13);
            this.labelMatch.TabIndex = 3;
            this.labelMatch.Text = "Match";
            // 
            // labelRules
            // 
            this.labelRules.Location = new System.Drawing.Point(174, 21);
            this.labelRules.Name = "labelRules";
            this.labelRules.Size = new System.Drawing.Size(126, 13);
            this.labelRules.TabIndex = 5;
            this.labelRules.Text = "of the following rules on";
            // 
            // flowLayout
            // 
            this.flowLayout.AutoSize = true;
            this.flowLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayout.Controls.Add(this.topPanel);
            this.flowLayout.Controls.Add(this.matcherGroup);
            this.flowLayout.Controls.Add(this.bottomPanel);
            this.flowLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayout.Location = new System.Drawing.Point(6, 12);
            this.flowLayout.Name = "flowLayout";
            this.flowLayout.Size = new System.Drawing.Size(608, 244);
            this.flowLayout.TabIndex = 0;
            // 
            // matcherGroup
            // 
            this.matcherGroup.AutoSize = true;
            this.matcherGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.matcherGroup.Controls.Add(this.chkNotBaseList);
            this.matcherGroup.Controls.Add(this.matcherControls);
            this.matcherGroup.Controls.Add(this.labelRules);
            this.matcherGroup.Controls.Add(this.cbMatchMode);
            this.matcherGroup.Controls.Add(this.labelMatch);
            this.matcherGroup.Controls.Add(this.cbBaseList);
            this.matcherGroup.Location = new System.Drawing.Point(3, 131);
            this.matcherGroup.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.matcherGroup.MinimumSize = new System.Drawing.Size(600, 0);
            this.matcherGroup.Name = "matcherGroup";
            this.matcherGroup.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.matcherGroup.Size = new System.Drawing.Size(600, 74);
            this.matcherGroup.TabIndex = 1;
            this.matcherGroup.TabStop = false;
            // 
            // bottomPanel
            // 
            this.bottomPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bottomPanel.AutoSize = true;
            this.bottomPanel.Controls.Add(this.btQuery);
            this.bottomPanel.Controls.Add(this.btNext);
            this.bottomPanel.Controls.Add(this.btPrev);
            this.bottomPanel.Controls.Add(this.btOK);
            this.bottomPanel.Controls.Add(this.btCancel);
            this.bottomPanel.Controls.Add(this.btApply);
            this.bottomPanel.Location = new System.Drawing.Point(3, 211);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(602, 30);
            this.bottomPanel.TabIndex = 2;
            // 
            // btQuery
            // 
            this.btQuery.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.btQuery.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btQuery.Location = new System.Drawing.Point(172, 3);
            this.btQuery.Name = "btQuery";
            this.btQuery.Size = new System.Drawing.Size(80, 24);
            this.btQuery.TabIndex = 2;
            this.btQuery.Text = "&Query";
            // 
            // btNext
            // 
            this.btNext.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btNext.Location = new System.Drawing.Point(86, 3);
            this.btNext.Name = "btNext";
            this.btNext.Size = new System.Drawing.Size(80, 24);
            this.btNext.TabIndex = 1;
            this.btNext.Text = "&Next";
            this.btNext.Click += new System.EventHandler(this.btNext_Click);
            // 
            // btPrev
            // 
            this.btPrev.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btPrev.Location = new System.Drawing.Point(0, 3);
            this.btPrev.Name = "btPrev";
            this.btPrev.Size = new System.Drawing.Size(80, 24);
            this.btPrev.TabIndex = 0;
            this.btPrev.Text = "&Previous";
            this.btPrev.Click += new System.EventHandler(this.btPrev_Click);
            // 
            // btApply
            // 
            this.btApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btApply.Location = new System.Drawing.Point(519, 3);
            this.btApply.Name = "btApply";
            this.btApply.Size = new System.Drawing.Size(80, 24);
            this.btApply.TabIndex = 5;
            this.btApply.Text = "&Apply";
            this.btApply.Click += new System.EventHandler(this.btApply_Click);
            // 
            // baseImages
            // 
            this.baseImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.baseImages.ImageSize = new System.Drawing.Size(16, 16);
            this.baseImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // SmartListDialog
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(617, 258);
            this.Controls.Add(this.flowLayout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SmartListDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Smart List";
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.flowLayout.ResumeLayout(false);
            this.flowLayout.PerformLayout();
            this.matcherGroup.ResumeLayout(false);
            this.matcherGroup.PerformLayout();
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		private Button btCancel;
		private Button btOK;
		private FlowLayoutPanel matcherControls;
		private Panel topPanel;
		private Label labelRules;
		private FlowLayoutPanel flowLayout;
		private GroupBox matcherGroup;
		private Panel bottomPanel;
		private ComboBox cbMatchMode;
		private Label labelMatch;
		private Button btApply;
		private ComboBox cbLimitType;
		private TextBox txLimit;
		private CheckBox chkLimit;
		private ComboBox cbBaseList;
		private ImageList baseImages;
		private Label labelName;
		private TextBox txtName;
		private Button btNext;
		private Button btPrev;
		private CheckBox chkNotBaseList;
		private CheckBox chkQuickOpen;
		private TextBox txtNotes;
		private Label labelNotes;
		private CheckBox chkShowNotes;
		private Button btFilterReset;
		private Label labelFilterReset;
		private Button btQuery;
	}
}
