using cYo.Common.Windows.Forms;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class ComicDisplaySettingsDialog
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
			btBrowseTexture = new System.Windows.Forms.Button();
			cbBackgroundTexture = new System.Windows.Forms.ComboBox();
			labelBackgroundTexture = new System.Windows.Forms.Label();
			cbBackgroundType = new System.Windows.Forms.ComboBox();
			labelBackgroundType = new System.Windows.Forms.Label();
			cpBackgroundColor = new cYo.Common.Windows.Forms.SimpleColorPicker();
			labelBackgroundColor = new System.Windows.Forms.Label();
			chkPageMargin = new System.Windows.Forms.CheckBox();
			chkRealisticPages = new System.Windows.Forms.CheckBox();
			cbPageTransition = new System.Windows.Forms.ComboBox();
			btCancel = new System.Windows.Forms.Button();
			btOK = new System.Windows.Forms.Button();
			labelPaging = new System.Windows.Forms.Label();
			grpGeneral = new System.Windows.Forms.GroupBox();
			tbMargin = new cYo.Common.Windows.Forms.TrackBarLite();
			grpEffects = new System.Windows.Forms.GroupBox();
			cbPaperLayout = new System.Windows.Forms.ComboBox();
			labelPaperStrength = new System.Windows.Forms.Label();
			tbPaperStrength = new cYo.Common.Windows.Forms.TrackBarLite();
			btBrowsePaper = new System.Windows.Forms.Button();
			cbPaperTexture = new System.Windows.Forms.ComboBox();
			labelPaper = new System.Windows.Forms.Label();
			grpBackground = new System.Windows.Forms.GroupBox();
			cbTextureLayout = new System.Windows.Forms.ComboBox();
			btApply = new System.Windows.Forms.Button();
			flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			panel1 = new System.Windows.Forms.Panel();
			toolTip = new System.Windows.Forms.ToolTip(components);
			grpGeneral.SuspendLayout();
			grpEffects.SuspendLayout();
			grpBackground.SuspendLayout();
			flowLayoutPanel1.SuspendLayout();
			panel1.SuspendLayout();
			SuspendLayout();
			btBrowseTexture.Location = new System.Drawing.Point(357, 51);
			btBrowseTexture.Name = "btBrowseTexture";
			btBrowseTexture.Size = new System.Drawing.Size(32, 22);
			btBrowseTexture.TabIndex = 6;
			btBrowseTexture.Text = "...";
			btBrowseTexture.UseVisualStyleBackColor = true;
			btBrowseTexture.Click += new System.EventHandler(btBroweTexture_Click);
			cbBackgroundTexture.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbBackgroundTexture.FormattingEnabled = true;
			cbBackgroundTexture.Location = new System.Drawing.Point(103, 52);
			cbBackgroundTexture.Name = "cbBackgroundTexture";
			cbBackgroundTexture.Size = new System.Drawing.Size(248, 21);
			cbBackgroundTexture.TabIndex = 4;
			cbBackgroundTexture.SelectedIndexChanged += new System.EventHandler(cbBackgroundTexture_SelectedIndexChanged);
			labelBackgroundTexture.Location = new System.Drawing.Point(15, 55);
			labelBackgroundTexture.Name = "labelBackgroundTexture";
			labelBackgroundTexture.Size = new System.Drawing.Size(67, 13);
			labelBackgroundTexture.TabIndex = 3;
			labelBackgroundTexture.Text = "Texture:";
			labelBackgroundTexture.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			cbBackgroundType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbBackgroundType.FormattingEnabled = true;
			cbBackgroundType.Items.AddRange(new object[3]
			{
				"Adjust Color to current Page",
				"Solid Color",
				"Texture"
			});
			cbBackgroundType.Location = new System.Drawing.Point(103, 27);
			cbBackgroundType.Name = "cbBackgroundType";
			cbBackgroundType.Size = new System.Drawing.Size(286, 21);
			cbBackgroundType.TabIndex = 1;
			cbBackgroundType.SelectedIndexChanged += new System.EventHandler(cbBackgroundType_SelectedIndexChanged);
			labelBackgroundType.Location = new System.Drawing.Point(15, 30);
			labelBackgroundType.Name = "labelBackgroundType";
			labelBackgroundType.Size = new System.Drawing.Size(67, 13);
			labelBackgroundType.TabIndex = 0;
			labelBackgroundType.Text = "Type:";
			labelBackgroundType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			cpBackgroundColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			cpBackgroundColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cpBackgroundColor.FormattingEnabled = true;
			cpBackgroundColor.Location = new System.Drawing.Point(103, 52);
			cpBackgroundColor.Name = "cpBackgroundColor";
			cpBackgroundColor.SelectedColor = System.Drawing.Color.Empty;
			cpBackgroundColor.SelectedColorName = "0";
			cpBackgroundColor.Size = new System.Drawing.Size(286, 21);
			cpBackgroundColor.TabIndex = 5;
			labelBackgroundColor.Location = new System.Drawing.Point(15, 56);
			labelBackgroundColor.Name = "labelBackgroundColor";
			labelBackgroundColor.Size = new System.Drawing.Size(67, 13);
			labelBackgroundColor.TabIndex = 2;
			labelBackgroundColor.Text = "Color:";
			labelBackgroundColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			chkPageMargin.AutoSize = true;
			chkPageMargin.Location = new System.Drawing.Point(15, 51);
			chkPageMargin.Name = "chkPageMargin";
			chkPageMargin.Size = new System.Drawing.Size(181, 17);
			chkPageMargin.TabIndex = 1;
			chkPageMargin.Text = "Leave margins around the pages";
			chkPageMargin.UseVisualStyleBackColor = true;
			chkRealisticPages.AutoSize = true;
			chkRealisticPages.Location = new System.Drawing.Point(15, 28);
			chkRealisticPages.Name = "chkRealisticPages";
			chkRealisticPages.Size = new System.Drawing.Size(131, 17);
			chkRealisticPages.TabIndex = 0;
			chkRealisticPages.Text = "Realistic Book Display";
			chkRealisticPages.UseVisualStyleBackColor = true;
			cbPageTransition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbPageTransition.FormattingEnabled = true;
			cbPageTransition.Items.AddRange(new object[5]
			{
				"No Page Transition Effect",
				"New Page fades in",
				"New Page scrolls in horizontally",
				"New Page scrolls in vertically",
				"Page Turn Effect"
			});
			cbPageTransition.Location = new System.Drawing.Point(103, 26);
			cbPageTransition.Name = "cbPageTransition";
			cbPageTransition.Size = new System.Drawing.Size(286, 21);
			cbPageTransition.TabIndex = 1;
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(89, 3);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 4;
			btCancel.Text = "&Cancel";
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(3, 3);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(80, 24);
			btOK.TabIndex = 3;
			btOK.Text = "&OK";
			labelPaging.AutoSize = true;
			labelPaging.Location = new System.Drawing.Point(15, 29);
			labelPaging.Name = "labelPaging";
			labelPaging.Size = new System.Drawing.Size(84, 13);
			labelPaging.TabIndex = 0;
			labelPaging.Text = "Page Transition:";
			labelPaging.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			grpGeneral.Controls.Add(tbMargin);
			grpGeneral.Controls.Add(chkRealisticPages);
			grpGeneral.Controls.Add(chkPageMargin);
			grpGeneral.Location = new System.Drawing.Point(3, 3);
			grpGeneral.Name = "grpGeneral";
			grpGeneral.Size = new System.Drawing.Size(396, 84);
			grpGeneral.TabIndex = 0;
			grpGeneral.TabStop = false;
			grpGeneral.Text = "General";
			tbMargin.Location = new System.Drawing.Point(202, 50);
			tbMargin.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			tbMargin.Maximum = 50;
			tbMargin.Name = "tbMargin";
			tbMargin.Size = new System.Drawing.Size(187, 18);
			tbMargin.TabIndex = 2;
			tbMargin.ThumbSize = new System.Drawing.Size(8, 16);
			tbMargin.ValueChanged += new System.EventHandler(PercentTrackbarValueChanged);
			grpEffects.AutoSize = true;
			grpEffects.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			grpEffects.Controls.Add(cbPaperLayout);
			grpEffects.Controls.Add(labelPaperStrength);
			grpEffects.Controls.Add(tbPaperStrength);
			grpEffects.Controls.Add(btBrowsePaper);
			grpEffects.Controls.Add(cbPaperTexture);
			grpEffects.Controls.Add(labelPaper);
			grpEffects.Controls.Add(labelPaging);
			grpEffects.Controls.Add(cbPageTransition);
			grpEffects.Location = new System.Drawing.Point(3, 93);
			grpEffects.Name = "grpEffects";
			grpEffects.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
			grpEffects.Size = new System.Drawing.Size(395, 138);
			grpEffects.TabIndex = 0;
			grpEffects.TabStop = false;
			grpEffects.Text = "Effects";
			cbPaperLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbPaperLayout.FormattingEnabled = true;
			cbPaperLayout.Items.AddRange(new object[5]
			{
				"None",
				"Tile",
				"Center",
				"Stretch",
				"Zoom"
			});
			cbPaperLayout.Location = new System.Drawing.Point(103, 80);
			cbPaperLayout.Name = "cbPaperLayout";
			cbPaperLayout.Size = new System.Drawing.Size(248, 21);
			cbPaperLayout.TabIndex = 5;
			labelPaperStrength.AutoSize = true;
			labelPaperStrength.Location = new System.Drawing.Point(100, 107);
			labelPaperStrength.Name = "labelPaperStrength";
			labelPaperStrength.Size = new System.Drawing.Size(50, 13);
			labelPaperStrength.TabIndex = 6;
			labelPaperStrength.Text = "Strength:";
			tbPaperStrength.Location = new System.Drawing.Point(156, 107);
			tbPaperStrength.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			tbPaperStrength.Name = "tbPaperStrength";
			tbPaperStrength.Size = new System.Drawing.Size(195, 18);
			tbPaperStrength.TabIndex = 7;
			tbPaperStrength.ThumbSize = new System.Drawing.Size(8, 16);
			tbPaperStrength.ValueChanged += new System.EventHandler(PercentTrackbarValueChanged);
			btBrowsePaper.Location = new System.Drawing.Point(357, 53);
			btBrowsePaper.Name = "btBrowsePaper";
			btBrowsePaper.Size = new System.Drawing.Size(32, 21);
			btBrowsePaper.TabIndex = 4;
			btBrowsePaper.Text = "...";
			btBrowsePaper.UseVisualStyleBackColor = true;
			btBrowsePaper.Click += new System.EventHandler(btBrowsePaper_Click);
			cbPaperTexture.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbPaperTexture.FormattingEnabled = true;
			cbPaperTexture.Location = new System.Drawing.Point(103, 53);
			cbPaperTexture.Name = "cbPaperTexture";
			cbPaperTexture.Size = new System.Drawing.Size(248, 21);
			cbPaperTexture.TabIndex = 3;
			cbPaperTexture.SelectedIndexChanged += new System.EventHandler(cbPaperTexture_SelectedIndexChanged);
			labelPaper.AutoSize = true;
			labelPaper.Location = new System.Drawing.Point(15, 55);
			labelPaper.Name = "labelPaper";
			labelPaper.Size = new System.Drawing.Size(38, 13);
			labelPaper.TabIndex = 2;
			labelPaper.Text = "Paper:";
			labelPaper.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			grpBackground.AutoSize = true;
			grpBackground.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			grpBackground.Controls.Add(labelBackgroundType);
			grpBackground.Controls.Add(labelBackgroundColor);
			grpBackground.Controls.Add(cbBackgroundType);
			grpBackground.Controls.Add(cbTextureLayout);
			grpBackground.Controls.Add(labelBackgroundTexture);
			grpBackground.Controls.Add(btBrowseTexture);
			grpBackground.Controls.Add(cbBackgroundTexture);
			grpBackground.Controls.Add(cpBackgroundColor);
			grpBackground.Location = new System.Drawing.Point(3, 237);
			grpBackground.Name = "grpBackground";
			grpBackground.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
			grpBackground.Size = new System.Drawing.Size(395, 102);
			grpBackground.TabIndex = 0;
			grpBackground.TabStop = false;
			grpBackground.Text = "Background";
			cbTextureLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbTextureLayout.FormattingEnabled = true;
			cbTextureLayout.Items.AddRange(new object[5]
			{
				"None",
				"Tile",
				"Center",
				"Stretch",
				"Zoom"
			});
			cbTextureLayout.Location = new System.Drawing.Point(103, 79);
			cbTextureLayout.Name = "cbTextureLayout";
			cbTextureLayout.Size = new System.Drawing.Size(248, 21);
			cbTextureLayout.TabIndex = 7;
			btApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btApply.Location = new System.Drawing.Point(175, 3);
			btApply.Name = "btApply";
			btApply.Size = new System.Drawing.Size(80, 24);
			btApply.TabIndex = 5;
			btApply.Text = "&Apply";
			btApply.Click += new System.EventHandler(btApply_Click);
			flowLayoutPanel1.AutoSize = true;
			flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			flowLayoutPanel1.Controls.Add(grpGeneral);
			flowLayoutPanel1.Controls.Add(grpEffects);
			flowLayoutPanel1.Controls.Add(grpBackground);
			flowLayoutPanel1.Controls.Add(panel1);
			flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
			flowLayoutPanel1.Name = "flowLayoutPanel1";
			flowLayoutPanel1.Size = new System.Drawing.Size(402, 378);
			flowLayoutPanel1.TabIndex = 6;
			panel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			panel1.AutoSize = true;
			panel1.Controls.Add(btOK);
			panel1.Controls.Add(btApply);
			panel1.Controls.Add(btCancel);
			panel1.Location = new System.Drawing.Point(141, 345);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(258, 30);
			panel1.TabIndex = 3;
			base.AcceptButton = btOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			AutoSize = true;
			base.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(421, 413);
			base.Controls.Add(flowLayoutPanel1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ComicDisplaySettingsDialog";
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Book Display Settings";
			grpGeneral.ResumeLayout(false);
			grpGeneral.PerformLayout();
			grpEffects.ResumeLayout(false);
			grpEffects.PerformLayout();
			grpBackground.ResumeLayout(false);
			flowLayoutPanel1.ResumeLayout(false);
			flowLayoutPanel1.PerformLayout();
			panel1.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		private IContainer components;

		private Button btBrowseTexture;

		private ComboBox cbBackgroundTexture;

		private Label labelBackgroundTexture;

		private ComboBox cbBackgroundType;

		private Label labelBackgroundType;

		private SimpleColorPicker cpBackgroundColor;

		private Label labelBackgroundColor;

		private CheckBox chkPageMargin;

		private CheckBox chkRealisticPages;

		private ComboBox cbPageTransition;

		private Button btCancel;

		private Button btOK;

		private Label labelPaging;

		private GroupBox grpGeneral;

		private GroupBox grpEffects;

		private GroupBox grpBackground;

		private Button btApply;

		private ComboBox cbPaperTexture;

		private Label labelPaper;

		private Button btBrowsePaper;

		private TrackBarLite tbPaperStrength;

		private Label labelPaperStrength;

		private FlowLayoutPanel flowLayoutPanel1;

		private Panel panel1;

		private ComboBox cbPaperLayout;

		private ComboBox cbTextureLayout;

		private TrackBarLite tbMargin;

		private ToolTip toolTip;
	}
}
