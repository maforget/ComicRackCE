using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class SmartListDialog
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
			btCancel = new System.Windows.Forms.Button();
			btOK = new System.Windows.Forms.Button();
			matcherControls = new System.Windows.Forms.FlowLayoutPanel();
			topPanel = new System.Windows.Forms.Panel();
			btFilterReset = new System.Windows.Forms.Button();
			labelFilterReset = new System.Windows.Forms.Label();
			chkShowNotes = new System.Windows.Forms.CheckBox();
			txLimit = new System.Windows.Forms.TextBox();
			txtNotes = new System.Windows.Forms.TextBox();
			labelNotes = new System.Windows.Forms.Label();
			chkLimit = new System.Windows.Forms.CheckBox();
			labelName = new System.Windows.Forms.Label();
			chkQuickOpen = new System.Windows.Forms.CheckBox();
			cbLimitType = new System.Windows.Forms.ComboBox();
			txtName = new System.Windows.Forms.TextBox();
			chkNotBaseList = new System.Windows.Forms.CheckBox();
			cbMatchMode = new System.Windows.Forms.ComboBox();
			cbBaseList = new System.Windows.Forms.ComboBox();
			labelMatch = new System.Windows.Forms.Label();
			labelRules = new System.Windows.Forms.Label();
			flowLayout = new System.Windows.Forms.FlowLayoutPanel();
			matcherGroup = new System.Windows.Forms.GroupBox();
			bottomPanel = new System.Windows.Forms.Panel();
			btNext = new System.Windows.Forms.Button();
			btPrev = new System.Windows.Forms.Button();
			btApply = new System.Windows.Forms.Button();
			baseImages = new System.Windows.Forms.ImageList(components);
			btQuery = new System.Windows.Forms.Button();
			topPanel.SuspendLayout();
			flowLayout.SuspendLayout();
			matcherGroup.SuspendLayout();
			bottomPanel.SuspendLayout();
			SuspendLayout();
			btCancel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(435, 3);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 4;
			btCancel.Text = "&Cancel";
			btOK.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(351, 3);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(80, 24);
			btOK.TabIndex = 3;
			btOK.Text = "&OK";
			btOK.Click += new System.EventHandler(btOK_Click);
			matcherControls.AutoScroll = true;
			matcherControls.AutoSize = true;
			matcherControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			matcherControls.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			matcherControls.Location = new System.Drawing.Point(6, 41);
			matcherControls.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			matcherControls.MaximumSize = new System.Drawing.Size(0, 400);
			matcherControls.MinimumSize = new System.Drawing.Size(588, 20);
			matcherControls.Name = "matcherControls";
			matcherControls.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
			matcherControls.Size = new System.Drawing.Size(588, 20);
			matcherControls.TabIndex = 0;
			matcherControls.WrapContents = false;
			topPanel.AutoSize = true;
			topPanel.Controls.Add(btFilterReset);
			topPanel.Controls.Add(labelFilterReset);
			topPanel.Controls.Add(chkShowNotes);
			topPanel.Controls.Add(txLimit);
			topPanel.Controls.Add(txtNotes);
			topPanel.Controls.Add(labelNotes);
			topPanel.Controls.Add(chkLimit);
			topPanel.Controls.Add(labelName);
			topPanel.Controls.Add(chkQuickOpen);
			topPanel.Controls.Add(cbLimitType);
			topPanel.Controls.Add(txtName);
			topPanel.Dock = System.Windows.Forms.DockStyle.Top;
			topPanel.Location = new System.Drawing.Point(3, 3);
			topPanel.Name = "topPanel";
			topPanel.Size = new System.Drawing.Size(602, 125);
			topPanel.TabIndex = 0;
			btFilterReset.Location = new System.Drawing.Point(344, 101);
			btFilterReset.Name = "btFilterReset";
			btFilterReset.Size = new System.Drawing.Size(90, 21);
			btFilterReset.TabIndex = 10;
			btFilterReset.Text = "Reset";
			btFilterReset.UseVisualStyleBackColor = true;
			btFilterReset.Click += new System.EventHandler(btFilterReset_Click);
			labelFilterReset.AutoSize = true;
			labelFilterReset.Location = new System.Drawing.Point(69, 105);
			labelFilterReset.Name = "labelFilterReset";
			labelFilterReset.Size = new System.Drawing.Size(269, 13);
			labelFilterReset.TabIndex = 9;
			labelFilterReset.Text = "Some Books have been manually removed from this list.";
			chkShowNotes.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkShowNotes.Appearance = System.Windows.Forms.Appearance.Button;
			chkShowNotes.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.DoubleArrow;
			chkShowNotes.Location = new System.Drawing.Point(574, 0);
			chkShowNotes.Name = "chkShowNotes";
			chkShowNotes.Size = new System.Drawing.Size(22, 22);
			chkShowNotes.TabIndex = 2;
			chkShowNotes.UseVisualStyleBackColor = true;
			chkShowNotes.CheckedChanged += new System.EventHandler(chkShowNotes_CheckedChanged);
			txLimit.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			txLimit.Enabled = false;
			txLimit.Location = new System.Drawing.Point(443, 74);
			txLimit.Name = "txLimit";
			txLimit.Size = new System.Drawing.Size(45, 20);
			txLimit.TabIndex = 7;
			txLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			txLimit.Visible = false;
			txLimit.TextChanged += new System.EventHandler(txLimit_TextChanged);
			txtNotes.AcceptsReturn = true;
			txtNotes.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txtNotes.Location = new System.Drawing.Point(68, 26);
			txtNotes.Multiline = true;
			txtNotes.Name = "txtNotes";
			txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			txtNotes.Size = new System.Drawing.Size(366, 69);
			txtNotes.TabIndex = 4;
			txtNotes.Visible = false;
			txtNotes.TextChanged += new System.EventHandler(txtNotes_TextChanged);
			labelNotes.Location = new System.Drawing.Point(0, 26);
			labelNotes.Name = "labelNotes";
			labelNotes.Size = new System.Drawing.Size(62, 13);
			labelNotes.TabIndex = 3;
			labelNotes.Text = "Notes:";
			labelNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
			labelNotes.Visible = false;
			chkLimit.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkLimit.Location = new System.Drawing.Point(443, 51);
			chkLimit.Name = "chkLimit";
			chkLimit.Size = new System.Drawing.Size(129, 17);
			chkLimit.TabIndex = 6;
			chkLimit.Text = "Limit to ";
			chkLimit.UseVisualStyleBackColor = true;
			chkLimit.Visible = false;
			chkLimit.CheckedChanged += new System.EventHandler(chkLimit_CheckedChanged);
			labelName.Location = new System.Drawing.Point(3, 4);
			labelName.Name = "labelName";
			labelName.Size = new System.Drawing.Size(59, 13);
			labelName.TabIndex = 0;
			labelName.Text = "Name:";
			labelName.TextAlign = System.Drawing.ContentAlignment.TopRight;
			chkQuickOpen.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkQuickOpen.Location = new System.Drawing.Point(443, 28);
			chkQuickOpen.Name = "chkQuickOpen";
			chkQuickOpen.Size = new System.Drawing.Size(129, 17);
			chkQuickOpen.TabIndex = 5;
			chkQuickOpen.Text = "Show in Quick Open";
			chkQuickOpen.UseVisualStyleBackColor = true;
			chkQuickOpen.Visible = false;
			chkQuickOpen.CheckedChanged += new System.EventHandler(chkQuickOpen_CheckedChanged);
			cbLimitType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			cbLimitType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbLimitType.Enabled = false;
			cbLimitType.FormattingEnabled = true;
			cbLimitType.Items.AddRange(new object[3]
			{
				"Books",
				"MB",
				"GB"
			});
			cbLimitType.Location = new System.Drawing.Point(494, 74);
			cbLimitType.Name = "cbLimitType";
			cbLimitType.Size = new System.Drawing.Size(78, 21);
			cbLimitType.TabIndex = 8;
			cbLimitType.Visible = false;
			cbLimitType.SelectedIndexChanged += new System.EventHandler(cbLimitType_SelectedIndexChanged);
			txtName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txtName.Location = new System.Drawing.Point(68, 1);
			txtName.Name = "txtName";
			txtName.Size = new System.Drawing.Size(504, 20);
			txtName.TabIndex = 1;
			txtName.TextChanged += new System.EventHandler(txtName_TextChanged);
			chkNotBaseList.Appearance = System.Windows.Forms.Appearance.Button;
			chkNotBaseList.Location = new System.Drawing.Point(6, 16);
			chkNotBaseList.Name = "chkNotBaseList";
			chkNotBaseList.Size = new System.Drawing.Size(21, 23);
			chkNotBaseList.TabIndex = 2;
			chkNotBaseList.Text = "!";
			chkNotBaseList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			chkNotBaseList.UseVisualStyleBackColor = true;
			chkNotBaseList.CheckedChanged += new System.EventHandler(chkNotBaseList_CheckedChanged);
			cbMatchMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbMatchMode.FormattingEnabled = true;
			cbMatchMode.Location = new System.Drawing.Point(103, 18);
			cbMatchMode.Name = "cbMatchMode";
			cbMatchMode.Size = new System.Drawing.Size(65, 21);
			cbMatchMode.TabIndex = 4;
			cbMatchMode.SelectedIndexChanged += new System.EventHandler(cbMatchMode_SelectedIndexChanged);
			cbBaseList.DisplayMember = "FullName";
			cbBaseList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbBaseList.Location = new System.Drawing.Point(317, 16);
			cbBaseList.Name = "cbBaseList";
			cbBaseList.Size = new System.Drawing.Size(255, 21);
			cbBaseList.TabIndex = 6;
			cbBaseList.ValueMember = "FullName";
			cbBaseList.SelectedIndexChanged += new System.EventHandler(cbBaseList_SelectedIndexChanged);
			labelMatch.Location = new System.Drawing.Point(33, 21);
			labelMatch.Name = "labelMatch";
			labelMatch.Size = new System.Drawing.Size(69, 13);
			labelMatch.TabIndex = 3;
			labelMatch.Text = "Match";
			labelRules.Location = new System.Drawing.Point(174, 21);
			labelRules.Name = "labelRules";
			labelRules.Size = new System.Drawing.Size(126, 13);
			labelRules.TabIndex = 5;
			labelRules.Text = "of the following rules on";
			flowLayout.AutoSize = true;
			flowLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			flowLayout.Controls.Add(topPanel);
			flowLayout.Controls.Add(matcherGroup);
			flowLayout.Controls.Add(bottomPanel);
			flowLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			flowLayout.Location = new System.Drawing.Point(6, 12);
			flowLayout.Name = "flowLayout";
			flowLayout.Size = new System.Drawing.Size(608, 244);
			flowLayout.TabIndex = 0;
			matcherGroup.AutoSize = true;
			matcherGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			matcherGroup.Controls.Add(chkNotBaseList);
			matcherGroup.Controls.Add(matcherControls);
			matcherGroup.Controls.Add(labelRules);
			matcherGroup.Controls.Add(cbMatchMode);
			matcherGroup.Controls.Add(labelMatch);
			matcherGroup.Controls.Add(cbBaseList);
			matcherGroup.Location = new System.Drawing.Point(3, 131);
			matcherGroup.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			matcherGroup.MinimumSize = new System.Drawing.Size(600, 0);
			matcherGroup.Name = "matcherGroup";
			matcherGroup.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
			matcherGroup.Size = new System.Drawing.Size(600, 74);
			matcherGroup.TabIndex = 1;
			matcherGroup.TabStop = false;
			bottomPanel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			bottomPanel.AutoSize = true;
			bottomPanel.Controls.Add(btQuery);
			bottomPanel.Controls.Add(btNext);
			bottomPanel.Controls.Add(btPrev);
			bottomPanel.Controls.Add(btOK);
			bottomPanel.Controls.Add(btCancel);
			bottomPanel.Controls.Add(btApply);
			bottomPanel.Location = new System.Drawing.Point(3, 211);
			bottomPanel.Name = "bottomPanel";
			bottomPanel.Size = new System.Drawing.Size(602, 30);
			bottomPanel.TabIndex = 2;
			btNext.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btNext.Location = new System.Drawing.Point(86, 3);
			btNext.Name = "btNext";
			btNext.Size = new System.Drawing.Size(80, 24);
			btNext.TabIndex = 1;
			btNext.Text = "&Next";
			btNext.Click += new System.EventHandler(btNext_Click);
			btPrev.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btPrev.Location = new System.Drawing.Point(0, 3);
			btPrev.Name = "btPrev";
			btPrev.Size = new System.Drawing.Size(80, 24);
			btPrev.TabIndex = 0;
			btPrev.Text = "&Previous";
			btPrev.Click += new System.EventHandler(btPrev_Click);
			btApply.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btApply.Location = new System.Drawing.Point(519, 3);
			btApply.Name = "btApply";
			btApply.Size = new System.Drawing.Size(80, 24);
			btApply.TabIndex = 5;
			btApply.Text = "&Apply";
			btApply.Click += new System.EventHandler(btApply_Click);
			baseImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			baseImages.ImageSize = new System.Drawing.Size(16, 16);
			baseImages.TransparentColor = System.Drawing.Color.Transparent;
			btQuery.DialogResult = System.Windows.Forms.DialogResult.Retry;
			btQuery.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btQuery.Location = new System.Drawing.Point(172, 3);
			btQuery.Name = "btQuery";
			btQuery.Size = new System.Drawing.Size(80, 24);
			btQuery.TabIndex = 2;
			btQuery.Text = "&Query";
			base.AcceptButton = btOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			AutoSize = true;
			base.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(617, 258);
			base.Controls.Add(flowLayout);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.KeyPreview = true;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SmartListDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Edit Smart List";
			topPanel.ResumeLayout(false);
			topPanel.PerformLayout();
			flowLayout.ResumeLayout(false);
			flowLayout.PerformLayout();
			matcherGroup.ResumeLayout(false);
			matcherGroup.PerformLayout();
			bottomPanel.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}
		
		private IContainer components;

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
