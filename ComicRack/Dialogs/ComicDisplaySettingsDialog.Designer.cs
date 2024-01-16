using cYo.Common.Windows.Forms;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class ComicDisplaySettingsDialog
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
            this.btBrowseTexture = new System.Windows.Forms.Button();
            this.cbBackgroundTexture = new System.Windows.Forms.ComboBox();
            this.labelBackgroundTexture = new System.Windows.Forms.Label();
            this.cbBackgroundType = new System.Windows.Forms.ComboBox();
            this.labelBackgroundType = new System.Windows.Forms.Label();
            this.cpBackgroundColor = new cYo.Common.Windows.Forms.SimpleColorPicker();
            this.labelBackgroundColor = new System.Windows.Forms.Label();
            this.chkPageMargin = new System.Windows.Forms.CheckBox();
            this.chkRealisticPages = new System.Windows.Forms.CheckBox();
            this.cbPageTransition = new System.Windows.Forms.ComboBox();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.labelPaging = new System.Windows.Forms.Label();
            this.grpGeneral = new System.Windows.Forms.GroupBox();
            this.tbMargin = new cYo.Common.Windows.Forms.TrackBarLite();
            this.grpEffects = new System.Windows.Forms.GroupBox();
            this.cbPaperLayout = new System.Windows.Forms.ComboBox();
            this.labelPaperStrength = new System.Windows.Forms.Label();
            this.tbPaperStrength = new cYo.Common.Windows.Forms.TrackBarLite();
            this.btBrowsePaper = new System.Windows.Forms.Button();
            this.cbPaperTexture = new System.Windows.Forms.ComboBox();
            this.labelPaper = new System.Windows.Forms.Label();
            this.grpBackground = new System.Windows.Forms.GroupBox();
            this.cbTextureLayout = new System.Windows.Forms.ComboBox();
            this.btApply = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.grpGeneral.SuspendLayout();
            this.grpEffects.SuspendLayout();
            this.grpBackground.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btBrowseTexture
            // 
            this.btBrowseTexture.Location = new System.Drawing.Point(357, 51);
            this.btBrowseTexture.Name = "btBrowseTexture";
            this.btBrowseTexture.Size = new System.Drawing.Size(32, 22);
            this.btBrowseTexture.TabIndex = 6;
            this.btBrowseTexture.Text = "...";
            this.btBrowseTexture.UseVisualStyleBackColor = true;
            this.btBrowseTexture.Click += new System.EventHandler(this.btBroweTexture_Click);
            // 
            // cbBackgroundTexture
            // 
            this.cbBackgroundTexture.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBackgroundTexture.FormattingEnabled = true;
            this.cbBackgroundTexture.Location = new System.Drawing.Point(103, 52);
            this.cbBackgroundTexture.Name = "cbBackgroundTexture";
            this.cbBackgroundTexture.Size = new System.Drawing.Size(248, 21);
            this.cbBackgroundTexture.TabIndex = 4;
            this.cbBackgroundTexture.SelectedIndexChanged += new System.EventHandler(this.cbBackgroundTexture_SelectedIndexChanged);
            // 
            // labelBackgroundTexture
            // 
            this.labelBackgroundTexture.Location = new System.Drawing.Point(15, 55);
            this.labelBackgroundTexture.Name = "labelBackgroundTexture";
            this.labelBackgroundTexture.Size = new System.Drawing.Size(67, 13);
            this.labelBackgroundTexture.TabIndex = 3;
            this.labelBackgroundTexture.Text = "Texture:";
            this.labelBackgroundTexture.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbBackgroundType
            // 
            this.cbBackgroundType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBackgroundType.FormattingEnabled = true;
            this.cbBackgroundType.Items.AddRange(new object[] {
            "Adjust Color to current Page",
            "Solid Color",
            "Texture"});
            this.cbBackgroundType.Location = new System.Drawing.Point(103, 27);
            this.cbBackgroundType.Name = "cbBackgroundType";
            this.cbBackgroundType.Size = new System.Drawing.Size(286, 21);
            this.cbBackgroundType.TabIndex = 1;
            this.cbBackgroundType.SelectedIndexChanged += new System.EventHandler(this.cbBackgroundType_SelectedIndexChanged);
            // 
            // labelBackgroundType
            // 
            this.labelBackgroundType.Location = new System.Drawing.Point(15, 30);
            this.labelBackgroundType.Name = "labelBackgroundType";
            this.labelBackgroundType.Size = new System.Drawing.Size(67, 13);
            this.labelBackgroundType.TabIndex = 0;
            this.labelBackgroundType.Text = "Type:";
            this.labelBackgroundType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cpBackgroundColor
            // 
            this.cpBackgroundColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cpBackgroundColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cpBackgroundColor.FormattingEnabled = true;
            this.cpBackgroundColor.Location = new System.Drawing.Point(103, 52);
            this.cpBackgroundColor.Name = "cpBackgroundColor";
            this.cpBackgroundColor.SelectedColor = System.Drawing.Color.Empty;
            this.cpBackgroundColor.SelectedColorName = "0";
            this.cpBackgroundColor.Size = new System.Drawing.Size(286, 21);
            this.cpBackgroundColor.TabIndex = 5;
            // 
            // labelBackgroundColor
            // 
            this.labelBackgroundColor.Location = new System.Drawing.Point(15, 56);
            this.labelBackgroundColor.Name = "labelBackgroundColor";
            this.labelBackgroundColor.Size = new System.Drawing.Size(67, 13);
            this.labelBackgroundColor.TabIndex = 2;
            this.labelBackgroundColor.Text = "Color:";
            this.labelBackgroundColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkPageMargin
            // 
            this.chkPageMargin.AutoSize = true;
            this.chkPageMargin.Location = new System.Drawing.Point(15, 51);
            this.chkPageMargin.Name = "chkPageMargin";
            this.chkPageMargin.Size = new System.Drawing.Size(181, 17);
            this.chkPageMargin.TabIndex = 1;
            this.chkPageMargin.Text = "Leave margins around the pages";
            this.chkPageMargin.UseVisualStyleBackColor = true;
            // 
            // chkRealisticPages
            // 
            this.chkRealisticPages.AutoSize = true;
            this.chkRealisticPages.Location = new System.Drawing.Point(15, 28);
            this.chkRealisticPages.Name = "chkRealisticPages";
            this.chkRealisticPages.Size = new System.Drawing.Size(131, 17);
            this.chkRealisticPages.TabIndex = 0;
            this.chkRealisticPages.Text = "Realistic Book Display";
            this.chkRealisticPages.UseVisualStyleBackColor = true;
            // 
            // cbPageTransition
            // 
            this.cbPageTransition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPageTransition.FormattingEnabled = true;
            this.cbPageTransition.Items.AddRange(new object[] {
            "No Page Transition Effect",
            "New Page fades in",
            "New Page scrolls in horizontally",
            "New Page scrolls in vertically",
            "Page Turn Effect"});
            this.cbPageTransition.Location = new System.Drawing.Point(103, 26);
            this.cbPageTransition.Name = "cbPageTransition";
            this.cbPageTransition.Size = new System.Drawing.Size(286, 21);
            this.cbPageTransition.TabIndex = 1;
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btCancel.Location = new System.Drawing.Point(89, 3);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(80, 24);
            this.btCancel.TabIndex = 4;
            this.btCancel.Text = "&Cancel";
            // 
            // btOK
            // 
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btOK.Location = new System.Drawing.Point(3, 3);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(80, 24);
            this.btOK.TabIndex = 3;
            this.btOK.Text = "&OK";
            // 
            // labelPaging
            // 
            this.labelPaging.AutoSize = true;
            this.labelPaging.Location = new System.Drawing.Point(15, 29);
            this.labelPaging.Name = "labelPaging";
            this.labelPaging.Size = new System.Drawing.Size(84, 13);
            this.labelPaging.TabIndex = 0;
            this.labelPaging.Text = "Page Transition:";
            this.labelPaging.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpGeneral
            // 
            this.grpGeneral.Controls.Add(this.tbMargin);
            this.grpGeneral.Controls.Add(this.chkRealisticPages);
            this.grpGeneral.Controls.Add(this.chkPageMargin);
            this.grpGeneral.Location = new System.Drawing.Point(3, 3);
            this.grpGeneral.Name = "grpGeneral";
            this.grpGeneral.Size = new System.Drawing.Size(396, 84);
            this.grpGeneral.TabIndex = 0;
            this.grpGeneral.TabStop = false;
            this.grpGeneral.Text = "General";
            // 
            // tbMargin
            // 
            this.tbMargin.Location = new System.Drawing.Point(202, 50);
            this.tbMargin.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tbMargin.Maximum = 50;
            this.tbMargin.Name = "tbMargin";
            this.tbMargin.Size = new System.Drawing.Size(187, 18);
            this.tbMargin.TabIndex = 2;
            this.tbMargin.ThumbSize = new System.Drawing.Size(8, 16);
            this.tbMargin.ValueChanged += new System.EventHandler(this.PercentTrackbarValueChanged);
            // 
            // grpEffects
            // 
            this.grpEffects.AutoSize = true;
            this.grpEffects.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.grpEffects.Controls.Add(this.cbPaperLayout);
            this.grpEffects.Controls.Add(this.labelPaperStrength);
            this.grpEffects.Controls.Add(this.tbPaperStrength);
            this.grpEffects.Controls.Add(this.btBrowsePaper);
            this.grpEffects.Controls.Add(this.cbPaperTexture);
            this.grpEffects.Controls.Add(this.labelPaper);
            this.grpEffects.Controls.Add(this.labelPaging);
            this.grpEffects.Controls.Add(this.cbPageTransition);
            this.grpEffects.Location = new System.Drawing.Point(3, 93);
            this.grpEffects.Name = "grpEffects";
            this.grpEffects.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.grpEffects.Size = new System.Drawing.Size(395, 138);
            this.grpEffects.TabIndex = 0;
            this.grpEffects.TabStop = false;
            this.grpEffects.Text = "Effects";
            // 
            // cbPaperLayout
            // 
            this.cbPaperLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPaperLayout.FormattingEnabled = true;
            this.cbPaperLayout.Items.AddRange(new object[] {
            "None",
            "Tile",
            "Center",
            "Stretch",
            "Zoom"});
            this.cbPaperLayout.Location = new System.Drawing.Point(103, 80);
            this.cbPaperLayout.Name = "cbPaperLayout";
            this.cbPaperLayout.Size = new System.Drawing.Size(248, 21);
            this.cbPaperLayout.TabIndex = 5;
            // 
            // labelPaperStrength
            // 
            this.labelPaperStrength.AutoSize = true;
            this.labelPaperStrength.Location = new System.Drawing.Point(100, 107);
            this.labelPaperStrength.Name = "labelPaperStrength";
            this.labelPaperStrength.Size = new System.Drawing.Size(50, 13);
            this.labelPaperStrength.TabIndex = 6;
            this.labelPaperStrength.Text = "Strength:";
            // 
            // tbPaperStrength
            // 
            this.tbPaperStrength.Location = new System.Drawing.Point(156, 107);
            this.tbPaperStrength.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tbPaperStrength.Name = "tbPaperStrength";
            this.tbPaperStrength.Size = new System.Drawing.Size(195, 18);
            this.tbPaperStrength.TabIndex = 7;
            this.tbPaperStrength.ThumbSize = new System.Drawing.Size(8, 16);
            this.tbPaperStrength.ValueChanged += new System.EventHandler(this.PercentTrackbarValueChanged);
            // 
            // btBrowsePaper
            // 
            this.btBrowsePaper.Location = new System.Drawing.Point(357, 53);
            this.btBrowsePaper.Name = "btBrowsePaper";
            this.btBrowsePaper.Size = new System.Drawing.Size(32, 21);
            this.btBrowsePaper.TabIndex = 4;
            this.btBrowsePaper.Text = "...";
            this.btBrowsePaper.UseVisualStyleBackColor = true;
            this.btBrowsePaper.Click += new System.EventHandler(this.btBrowsePaper_Click);
            // 
            // cbPaperTexture
            // 
            this.cbPaperTexture.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPaperTexture.FormattingEnabled = true;
            this.cbPaperTexture.Location = new System.Drawing.Point(103, 53);
            this.cbPaperTexture.Name = "cbPaperTexture";
            this.cbPaperTexture.Size = new System.Drawing.Size(248, 21);
            this.cbPaperTexture.TabIndex = 3;
            this.cbPaperTexture.SelectedIndexChanged += new System.EventHandler(this.cbPaperTexture_SelectedIndexChanged);
            // 
            // labelPaper
            // 
            this.labelPaper.AutoSize = true;
            this.labelPaper.Location = new System.Drawing.Point(15, 55);
            this.labelPaper.Name = "labelPaper";
            this.labelPaper.Size = new System.Drawing.Size(38, 13);
            this.labelPaper.TabIndex = 2;
            this.labelPaper.Text = "Paper:";
            this.labelPaper.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpBackground
            // 
            this.grpBackground.AutoSize = true;
            this.grpBackground.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.grpBackground.Controls.Add(this.labelBackgroundType);
            this.grpBackground.Controls.Add(this.labelBackgroundColor);
            this.grpBackground.Controls.Add(this.cbBackgroundType);
            this.grpBackground.Controls.Add(this.cbTextureLayout);
            this.grpBackground.Controls.Add(this.labelBackgroundTexture);
            this.grpBackground.Controls.Add(this.btBrowseTexture);
            this.grpBackground.Controls.Add(this.cbBackgroundTexture);
            this.grpBackground.Controls.Add(this.cpBackgroundColor);
            this.grpBackground.Location = new System.Drawing.Point(3, 237);
            this.grpBackground.Name = "grpBackground";
            this.grpBackground.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.grpBackground.Size = new System.Drawing.Size(395, 116);
            this.grpBackground.TabIndex = 0;
            this.grpBackground.TabStop = false;
            this.grpBackground.Text = "Background";
            // 
            // cbTextureLayout
            // 
            this.cbTextureLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTextureLayout.FormattingEnabled = true;
            this.cbTextureLayout.Items.AddRange(new object[] {
            "None",
            "Tile",
            "Center",
            "Stretch",
            "Zoom"});
            this.cbTextureLayout.Location = new System.Drawing.Point(103, 79);
            this.cbTextureLayout.Name = "cbTextureLayout";
            this.cbTextureLayout.Size = new System.Drawing.Size(248, 21);
            this.cbTextureLayout.TabIndex = 7;
            // 
            // btApply
            // 
            this.btApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btApply.Location = new System.Drawing.Point(175, 3);
            this.btApply.Name = "btApply";
            this.btApply.Size = new System.Drawing.Size(80, 24);
            this.btApply.TabIndex = 5;
            this.btApply.Text = "&Apply";
            this.btApply.Click += new System.EventHandler(this.btApply_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.grpGeneral);
            this.flowLayoutPanel1.Controls.Add(this.grpEffects);
            this.flowLayoutPanel1.Controls.Add(this.grpBackground);
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(402, 392);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.btOK);
            this.panel1.Controls.Add(this.btApply);
            this.panel1.Controls.Add(this.btCancel);
            this.panel1.Location = new System.Drawing.Point(141, 359);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(258, 30);
            this.panel1.TabIndex = 3;
            // 
            // ComicDisplaySettingsDialog
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(421, 413);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ComicDisplaySettingsDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Book Display Settings";
            this.grpGeneral.ResumeLayout(false);
            this.grpGeneral.PerformLayout();
            this.grpEffects.ResumeLayout(false);
            this.grpEffects.PerformLayout();
            this.grpBackground.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

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
