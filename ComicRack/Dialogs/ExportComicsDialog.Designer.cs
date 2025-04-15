using cYo.Common.Win32;
using cYo.Common.Windows.Forms;
using System.ComponentModel;
using System;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class ExportComicsDialog
	{
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				IdleProcess.Idle -= OnIdle;
				components.Dispose();
			}
			base.Dispose(disposing);
		}


		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.contextRemovePageFilter = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.btCancel = new System.Windows.Forms.Button();
			this.btOK = new System.Windows.Forms.Button();
			this.tvPresets = new System.Windows.Forms.TreeView();
			this.btSavePreset = new System.Windows.Forms.Button();
			this.btRemovePreset = new System.Windows.Forms.Button();
			this.exportSettings = new System.Windows.Forms.Panel();
			this.grpImageProcessing = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			this.grpCustomProcessing = new System.Windows.Forms.GroupBox();
			this.labelGamma = new System.Windows.Forms.Label();
			this.tbGamma = new cYo.Common.Windows.Forms.TrackBarLite();
			this.tbSaturation = new cYo.Common.Windows.Forms.TrackBarLite();
			this.labelContrast = new System.Windows.Forms.Label();
			this.tbBrightness = new cYo.Common.Windows.Forms.TrackBarLite();
			this.labelSaturation = new System.Windows.Forms.Label();
			this.tbSharpening = new cYo.Common.Windows.Forms.TrackBarLite();
			this.tbContrast = new cYo.Common.Windows.Forms.TrackBarLite();
			this.labelSharpening = new System.Windows.Forms.Label();
			this.labelBrightness = new System.Windows.Forms.Label();
			this.btResetColors = new System.Windows.Forms.Button();
			this.labelImageProcessingCustom = new System.Windows.Forms.Label();
			this.cbImageProcessingSource = new System.Windows.Forms.ComboBox();
			this.labelImagProcessingSource = new System.Windows.Forms.Label();
			this.chkAutoContrast = new System.Windows.Forms.CheckBox();
			this.grpPageFormat = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			this.chkKeepOriginalNames = new System.Windows.Forms.CheckBox();
			this.labelDoublePages = new System.Windows.Forms.Label();
			this.cbDoublePages = new System.Windows.Forms.ComboBox();
			this.chkIgnoreErrorPages = new System.Windows.Forms.CheckBox();
			this.txHeight = new System.Windows.Forms.NumericUpDown();
			this.txWidth = new System.Windows.Forms.NumericUpDown();
			this.chkDontEnlarge = new System.Windows.Forms.CheckBox();
			this.labelPageResize = new System.Windows.Forms.Label();
			this.cbPageResize = new System.Windows.Forms.ComboBox();
			this.labelPageFormat = new System.Windows.Forms.Label();
			this.cbPageFormat = new System.Windows.Forms.ComboBox();
			this.labelPageQuality = new System.Windows.Forms.Label();
			this.tbQuality = new cYo.Common.Windows.Forms.TrackBarLite();
			this.labelX = new System.Windows.Forms.Label();
			this.grpFileFormat = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			this.txTagsToAppend = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelTagToAppend = new System.Windows.Forms.Label();
			this.txIncludePages = new System.Windows.Forms.TextBox();
			this.labelIncludePages = new System.Windows.Forms.Label();
			this.btRemovePageFilter = new System.Windows.Forms.Button();
			this.cbCompression = new System.Windows.Forms.ComboBox();
			this.labelCompression = new System.Windows.Forms.Label();
			this.chkEmbedComicInfo = new System.Windows.Forms.CheckBox();
			this.cbComicFormat = new System.Windows.Forms.ComboBox();
			this.labelComicFormat = new System.Windows.Forms.Label();
			this.txRemovedPages = new System.Windows.Forms.Label();
			this.labelRemovePageFilter = new System.Windows.Forms.Label();
			this.grpFileNaming = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			this.txCustomStartIndex = new System.Windows.Forms.NumericUpDown();
			this.labelCustomStartIndex = new System.Windows.Forms.Label();
			this.txCustomName = new System.Windows.Forms.TextBox();
			this.labelCustomNaming = new System.Windows.Forms.Label();
			this.cbNamingTemplate = new System.Windows.Forms.ComboBox();
			this.labelNamingTemplate = new System.Windows.Forms.Label();
			this.grpExportLocation = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			this.chkCombine = new System.Windows.Forms.CheckBox();
			this.chkOverwrite = new System.Windows.Forms.CheckBox();
			this.chkAddNewToLibrary = new System.Windows.Forms.CheckBox();
			this.chkDeleteOriginal = new System.Windows.Forms.CheckBox();
			this.btChooseFolder = new System.Windows.Forms.Button();
			this.txFolder = new System.Windows.Forms.Label();
			this.labelFolder = new System.Windows.Forms.Label();
			this.cbExport = new System.Windows.Forms.ComboBox();
			this.labelExportTo = new System.Windows.Forms.Label();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.exportSettings.SuspendLayout();
			this.grpImageProcessing.SuspendLayout();
			this.grpCustomProcessing.SuspendLayout();
			this.grpPageFormat.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.txHeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txWidth)).BeginInit();
			this.grpFileFormat.SuspendLayout();
			this.grpFileNaming.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.txCustomStartIndex)).BeginInit();
			this.grpExportLocation.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextRemovePageFilter
			// 
			this.contextRemovePageFilter.Name = "contextRemovePageFilter";
			this.contextRemovePageFilter.Size = new System.Drawing.Size(61, 4);
			// 
			// btCancel
			// 
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btCancel.Location = new System.Drawing.Point(660, 440);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(80, 24);
			this.btCancel.TabIndex = 7;
			this.btCancel.Text = "&Cancel";
			// 
			// btOK
			// 
			this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btOK.Location = new System.Drawing.Point(574, 440);
			this.btOK.Name = "btOK";
			this.btOK.Size = new System.Drawing.Size(80, 24);
			this.btOK.TabIndex = 6;
			this.btOK.Text = "&OK";
			// 
			// tvPresets
			// 
			this.tvPresets.Location = new System.Drawing.Point(14, 18);
			this.tvPresets.Name = "tvPresets";
			this.tvPresets.ShowNodeToolTips = true;
			this.tvPresets.Size = new System.Drawing.Size(220, 387);
			this.tvPresets.TabIndex = 3;
			this.tvPresets.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvPresets_AfterSelect);
			// 
			// btSavePreset
			// 
			this.btSavePreset.Location = new System.Drawing.Point(14, 411);
			this.btSavePreset.Name = "btSavePreset";
			this.btSavePreset.Size = new System.Drawing.Size(107, 23);
			this.btSavePreset.TabIndex = 4;
			this.btSavePreset.Text = "Save,,,";
			this.btSavePreset.UseVisualStyleBackColor = true;
			this.btSavePreset.Click += new System.EventHandler(this.btAddPreset_Click);
			// 
			// btRemovePreset
			// 
			this.btRemovePreset.Location = new System.Drawing.Point(127, 411);
			this.btRemovePreset.Name = "btRemovePreset";
			this.btRemovePreset.Size = new System.Drawing.Size(107, 23);
			this.btRemovePreset.TabIndex = 5;
			this.btRemovePreset.Text = "Remove";
			this.btRemovePreset.UseVisualStyleBackColor = true;
			this.btRemovePreset.Click += new System.EventHandler(this.btRemovePreset_Click);
			// 
			// exportSettings
			// 
			this.exportSettings.AutoScroll = true;
			this.exportSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.exportSettings.Controls.Add(this.grpImageProcessing);
			this.exportSettings.Controls.Add(this.grpPageFormat);
			this.exportSettings.Controls.Add(this.grpFileFormat);
			this.exportSettings.Controls.Add(this.grpFileNaming);
			this.exportSettings.Controls.Add(this.grpExportLocation);
			this.exportSettings.Location = new System.Drawing.Point(240, 18);
			this.exportSettings.Name = "exportSettings";
			this.exportSettings.Padding = new System.Windows.Forms.Padding(4);
			this.exportSettings.Size = new System.Drawing.Size(500, 416);
			this.exportSettings.TabIndex = 8;
			// 
			// grpImageProcessing
			// 
			this.grpImageProcessing.Controls.Add(this.grpCustomProcessing);
			this.grpImageProcessing.Controls.Add(this.labelImageProcessingCustom);
			this.grpImageProcessing.Controls.Add(this.cbImageProcessingSource);
			this.grpImageProcessing.Controls.Add(this.labelImagProcessingSource);
			this.grpImageProcessing.Controls.Add(this.chkAutoContrast);
			this.grpImageProcessing.Dock = System.Windows.Forms.DockStyle.Top;
			this.grpImageProcessing.Location = new System.Drawing.Point(4, 757);
			this.grpImageProcessing.Name = "grpImageProcessing";
			this.grpImageProcessing.Size = new System.Drawing.Size(473, 290);
			this.grpImageProcessing.TabIndex = 9;
			this.grpImageProcessing.Text = "Image Processing";
			// 
			// grpCustomProcessing
			// 
			this.grpCustomProcessing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpCustomProcessing.Controls.Add(this.labelGamma);
			this.grpCustomProcessing.Controls.Add(this.tbGamma);
			this.grpCustomProcessing.Controls.Add(this.tbSaturation);
			this.grpCustomProcessing.Controls.Add(this.labelContrast);
			this.grpCustomProcessing.Controls.Add(this.tbBrightness);
			this.grpCustomProcessing.Controls.Add(this.labelSaturation);
			this.grpCustomProcessing.Controls.Add(this.tbSharpening);
			this.grpCustomProcessing.Controls.Add(this.tbContrast);
			this.grpCustomProcessing.Controls.Add(this.labelSharpening);
			this.grpCustomProcessing.Controls.Add(this.labelBrightness);
			this.grpCustomProcessing.Controls.Add(this.btResetColors);
			this.grpCustomProcessing.Location = new System.Drawing.Point(110, 66);
			this.grpCustomProcessing.Name = "grpCustomProcessing";
			this.grpCustomProcessing.Size = new System.Drawing.Size(346, 183);
			this.grpCustomProcessing.TabIndex = 3;
			this.grpCustomProcessing.TabStop = false;
			// 
			// labelGamma
			// 
			this.labelGamma.AutoSize = true;
			this.labelGamma.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
			this.labelGamma.Location = new System.Drawing.Point(6, 97);
			this.labelGamma.Name = "labelGamma";
			this.labelGamma.Size = new System.Drawing.Size(43, 12);
			this.labelGamma.TabIndex = 6;
			this.labelGamma.Text = "Gamma";
			// 
			// tbGamma
			// 
			this.tbGamma.Location = new System.Drawing.Point(95, 91);
			this.tbGamma.Minimum = -100;
			this.tbGamma.Name = "tbGamma";
			this.tbGamma.Size = new System.Drawing.Size(245, 18);
			this.tbGamma.TabIndex = 7;
			this.tbGamma.Text = "tbSaturation";
			this.tbGamma.ThumbSize = new System.Drawing.Size(8, 16);
			this.tbGamma.TickFrequency = 16;
			this.tbGamma.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			this.tbGamma.ValueChanged += new System.EventHandler(this.AdjustmentSliderChanged);
			// 
			// tbSaturation
			// 
			this.tbSaturation.Location = new System.Drawing.Point(95, 19);
			this.tbSaturation.Minimum = -100;
			this.tbSaturation.Name = "tbSaturation";
			this.tbSaturation.Size = new System.Drawing.Size(245, 18);
			this.tbSaturation.TabIndex = 1;
			this.tbSaturation.ThumbSize = new System.Drawing.Size(8, 16);
			this.tbSaturation.TickFrequency = 16;
			this.tbSaturation.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			this.tbSaturation.ValueChanged += new System.EventHandler(this.AdjustmentSliderChanged);
			// 
			// labelContrast
			// 
			this.labelContrast.AutoSize = true;
			this.labelContrast.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
			this.labelContrast.Location = new System.Drawing.Point(6, 73);
			this.labelContrast.Name = "labelContrast";
			this.labelContrast.Size = new System.Drawing.Size(49, 12);
			this.labelContrast.TabIndex = 4;
			this.labelContrast.Text = "Contrast";
			// 
			// tbBrightness
			// 
			this.tbBrightness.Location = new System.Drawing.Point(95, 43);
			this.tbBrightness.Minimum = -100;
			this.tbBrightness.Name = "tbBrightness";
			this.tbBrightness.Size = new System.Drawing.Size(245, 18);
			this.tbBrightness.TabIndex = 3;
			this.tbBrightness.Text = "trackBarLite3";
			this.tbBrightness.ThumbSize = new System.Drawing.Size(8, 16);
			this.tbBrightness.TickFrequency = 16;
			this.tbBrightness.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			this.tbBrightness.ValueChanged += new System.EventHandler(this.AdjustmentSliderChanged);
			// 
			// labelSaturation
			// 
			this.labelSaturation.AutoSize = true;
			this.labelSaturation.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
			this.labelSaturation.Location = new System.Drawing.Point(6, 25);
			this.labelSaturation.Name = "labelSaturation";
			this.labelSaturation.Size = new System.Drawing.Size(57, 12);
			this.labelSaturation.TabIndex = 0;
			this.labelSaturation.Text = "Saturation";
			// 
			// tbSharpening
			// 
			this.tbSharpening.LargeChange = 1;
			this.tbSharpening.Location = new System.Drawing.Point(95, 117);
			this.tbSharpening.Maximum = 3;
			this.tbSharpening.Name = "tbSharpening";
			this.tbSharpening.Size = new System.Drawing.Size(245, 18);
			this.tbSharpening.TabIndex = 9;
			this.tbSharpening.Text = "tbSaturation";
			this.tbSharpening.ThumbSize = new System.Drawing.Size(8, 16);
			this.tbSharpening.TickFrequency = 1;
			this.tbSharpening.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			// 
			// tbContrast
			// 
			this.tbContrast.Location = new System.Drawing.Point(95, 67);
			this.tbContrast.Minimum = -100;
			this.tbContrast.Name = "tbContrast";
			this.tbContrast.Size = new System.Drawing.Size(245, 18);
			this.tbContrast.TabIndex = 5;
			this.tbContrast.Text = "tbSaturation";
			this.tbContrast.ThumbSize = new System.Drawing.Size(8, 16);
			this.tbContrast.TickFrequency = 16;
			this.tbContrast.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			this.tbContrast.ValueChanged += new System.EventHandler(this.AdjustmentSliderChanged);
			// 
			// labelSharpening
			// 
			this.labelSharpening.AutoSize = true;
			this.labelSharpening.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
			this.labelSharpening.Location = new System.Drawing.Point(6, 123);
			this.labelSharpening.Name = "labelSharpening";
			this.labelSharpening.Size = new System.Drawing.Size(61, 12);
			this.labelSharpening.TabIndex = 8;
			this.labelSharpening.Text = "Sharpening";
			// 
			// labelBrightness
			// 
			this.labelBrightness.AutoSize = true;
			this.labelBrightness.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
			this.labelBrightness.Location = new System.Drawing.Point(6, 49);
			this.labelBrightness.Name = "labelBrightness";
			this.labelBrightness.Size = new System.Drawing.Size(59, 12);
			this.labelBrightness.TabIndex = 2;
			this.labelBrightness.Text = "Brightness";
			// 
			// btResetColors
			// 
			this.btResetColors.Location = new System.Drawing.Point(257, 149);
			this.btResetColors.Name = "btResetColors";
			this.btResetColors.Size = new System.Drawing.Size(77, 24);
			this.btResetColors.TabIndex = 10;
			this.btResetColors.Text = "Reset";
			this.btResetColors.UseVisualStyleBackColor = true;
			this.btResetColors.Click += new System.EventHandler(this.btResetColors_Click);
			// 
			// labelImageProcessingCustom
			// 
			this.labelImageProcessingCustom.Location = new System.Drawing.Point(0, 66);
			this.labelImageProcessingCustom.Name = "labelImageProcessingCustom";
			this.labelImageProcessingCustom.Size = new System.Drawing.Size(104, 21);
			this.labelImageProcessingCustom.TabIndex = 2;
			this.labelImageProcessingCustom.Text = "Custom:";
			this.labelImageProcessingCustom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cbImageProcessingSource
			// 
			this.cbImageProcessingSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbImageProcessingSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbImageProcessingSource.FormattingEnabled = true;
			this.cbImageProcessingSource.Items.AddRange(new object[] {
            "Custom Settings",
            "Book Settings are applied"});
			this.cbImageProcessingSource.Location = new System.Drawing.Point(110, 40);
			this.cbImageProcessingSource.Name = "cbImageProcessingSource";
			this.cbImageProcessingSource.Size = new System.Drawing.Size(346, 21);
			this.cbImageProcessingSource.TabIndex = 1;
			// 
			// labelImagProcessingSource
			// 
			this.labelImagProcessingSource.Location = new System.Drawing.Point(3, 39);
			this.labelImagProcessingSource.Name = "labelImagProcessingSource";
			this.labelImagProcessingSource.Size = new System.Drawing.Size(101, 21);
			this.labelImagProcessingSource.TabIndex = 0;
			this.labelImagProcessingSource.Text = "Source:";
			this.labelImagProcessingSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// chkAutoContrast
			// 
			this.chkAutoContrast.AutoSize = true;
			this.chkAutoContrast.Location = new System.Drawing.Point(110, 255);
			this.chkAutoContrast.Name = "chkAutoContrast";
			this.chkAutoContrast.Size = new System.Drawing.Size(184, 17);
			this.chkAutoContrast.TabIndex = 4;
			this.chkAutoContrast.Text = "Automatic Contrast Enhancement";
			this.chkAutoContrast.UseVisualStyleBackColor = true;
			// 
			// grpPageFormat
			// 
			this.grpPageFormat.Controls.Add(this.chkKeepOriginalNames);
			this.grpPageFormat.Controls.Add(this.labelDoublePages);
			this.grpPageFormat.Controls.Add(this.cbDoublePages);
			this.grpPageFormat.Controls.Add(this.chkIgnoreErrorPages);
			this.grpPageFormat.Controls.Add(this.txHeight);
			this.grpPageFormat.Controls.Add(this.txWidth);
			this.grpPageFormat.Controls.Add(this.chkDontEnlarge);
			this.grpPageFormat.Controls.Add(this.labelPageResize);
			this.grpPageFormat.Controls.Add(this.cbPageResize);
			this.grpPageFormat.Controls.Add(this.labelPageFormat);
			this.grpPageFormat.Controls.Add(this.cbPageFormat);
			this.grpPageFormat.Controls.Add(this.labelPageQuality);
			this.grpPageFormat.Controls.Add(this.tbQuality);
			this.grpPageFormat.Controls.Add(this.labelX);
			this.grpPageFormat.Dock = System.Windows.Forms.DockStyle.Top;
			this.grpPageFormat.Location = new System.Drawing.Point(4, 552);
			this.grpPageFormat.Name = "grpPageFormat";
			this.grpPageFormat.Size = new System.Drawing.Size(473, 205);
			this.grpPageFormat.TabIndex = 2;
			this.grpPageFormat.TabStop = false;
			this.grpPageFormat.Text = "Page Format";
			// 
			// chkKeepOriginalNames
			// 
			this.chkKeepOriginalNames.AutoSize = true;
			this.chkKeepOriginalNames.Location = new System.Drawing.Point(110, 178);
			this.chkKeepOriginalNames.Name = "chkKeepOriginalNames";
			this.chkKeepOriginalNames.Size = new System.Drawing.Size(148, 17);
			this.chkKeepOriginalNames.TabIndex = 13;
			this.chkKeepOriginalNames.Text = "Keep original page names";
			this.chkKeepOriginalNames.UseVisualStyleBackColor = true;
			// 
			// labelDoublePages
			// 
			this.labelDoublePages.Location = new System.Drawing.Point(6, 96);
			this.labelDoublePages.Name = "labelDoublePages";
			this.labelDoublePages.Size = new System.Drawing.Size(98, 21);
			this.labelDoublePages.TabIndex = 9;
			this.labelDoublePages.Text = "Double Pages:";
			this.labelDoublePages.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cbDoublePages
			// 
			this.cbDoublePages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbDoublePages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDoublePages.FormattingEnabled = true;
			this.cbDoublePages.Items.AddRange(new object[] {
            "Keep",
            "Split",
            "Rotate 90°",
            "Adapt Width"});
			this.cbDoublePages.Location = new System.Drawing.Point(110, 96);
			this.cbDoublePages.Name = "cbDoublePages";
			this.cbDoublePages.Size = new System.Drawing.Size(157, 21);
			this.cbDoublePages.TabIndex = 10;
			// 
			// chkIgnoreErrorPages
			// 
			this.chkIgnoreErrorPages.AutoSize = true;
			this.chkIgnoreErrorPages.Location = new System.Drawing.Point(110, 155);
			this.chkIgnoreErrorPages.Name = "chkIgnoreErrorPages";
			this.chkIgnoreErrorPages.Size = new System.Drawing.Size(237, 17);
			this.chkIgnoreErrorPages.TabIndex = 12;
			this.chkIgnoreErrorPages.Text = "Ignore Pages with errors and continue export";
			this.chkIgnoreErrorPages.UseVisualStyleBackColor = true;
			// 
			// txHeight
			// 
			this.txHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txHeight.Location = new System.Drawing.Point(391, 71);
			this.txHeight.Maximum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
			this.txHeight.Name = "txHeight";
			this.txHeight.Size = new System.Drawing.Size(65, 20);
			this.txHeight.TabIndex = 8;
			this.txHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txHeight.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			// 
			// txWidth
			// 
			this.txWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txWidth.Location = new System.Drawing.Point(291, 72);
			this.txWidth.Maximum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
			this.txWidth.Name = "txWidth";
			this.txWidth.Size = new System.Drawing.Size(67, 20);
			this.txWidth.TabIndex = 6;
			this.txWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txWidth.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			// 
			// chkDontEnlarge
			// 
			this.chkDontEnlarge.AutoSize = true;
			this.chkDontEnlarge.Location = new System.Drawing.Point(110, 132);
			this.chkDontEnlarge.Name = "chkDontEnlarge";
			this.chkDontEnlarge.Size = new System.Drawing.Size(89, 17);
			this.chkDontEnlarge.TabIndex = 11;
			this.chkDontEnlarge.Text = "Don\'t enlarge";
			this.chkDontEnlarge.UseVisualStyleBackColor = true;
			// 
			// labelPageResize
			// 
			this.labelPageResize.Location = new System.Drawing.Point(6, 69);
			this.labelPageResize.Name = "labelPageResize";
			this.labelPageResize.Size = new System.Drawing.Size(98, 21);
			this.labelPageResize.TabIndex = 4;
			this.labelPageResize.Text = "Resize Pages:";
			this.labelPageResize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cbPageResize
			// 
			this.cbPageResize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbPageResize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbPageResize.FormattingEnabled = true;
			this.cbPageResize.Items.AddRange(new object[] {
            "Preserve Original",
            "Best fit Width & Height",
            "Set Width",
            "Set Height"});
			this.cbPageResize.Location = new System.Drawing.Point(110, 69);
			this.cbPageResize.Name = "cbPageResize";
			this.cbPageResize.Size = new System.Drawing.Size(157, 21);
			this.cbPageResize.TabIndex = 5;
			// 
			// labelPageFormat
			// 
			this.labelPageFormat.Location = new System.Drawing.Point(3, 42);
			this.labelPageFormat.Name = "labelPageFormat";
			this.labelPageFormat.Size = new System.Drawing.Size(101, 21);
			this.labelPageFormat.TabIndex = 0;
			this.labelPageFormat.Text = "Format:";
			this.labelPageFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cbPageFormat
			// 
			this.cbPageFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbPageFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbPageFormat.FormattingEnabled = true;
			this.cbPageFormat.Items.AddRange(new object[] {
            "Preserve Original",
            "JPEG",
            "PNG",
            "GIF",
            "TIFF",
            "BMP",
            "DJVU",
            "WEBP"});
			this.cbPageFormat.Location = new System.Drawing.Point(110, 42);
			this.cbPageFormat.Name = "cbPageFormat";
			this.cbPageFormat.Size = new System.Drawing.Size(157, 21);
			this.cbPageFormat.TabIndex = 1;
			// 
			// labelPageQuality
			// 
			this.labelPageQuality.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelPageQuality.Location = new System.Drawing.Point(273, 41);
			this.labelPageQuality.Name = "labelPageQuality";
			this.labelPageQuality.Size = new System.Drawing.Size(77, 21);
			this.labelPageQuality.TabIndex = 2;
			this.labelPageQuality.Text = "Quality:";
			this.labelPageQuality.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbQuality
			// 
			this.tbQuality.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tbQuality.Location = new System.Drawing.Point(349, 41);
			this.tbQuality.Name = "tbQuality";
			this.tbQuality.Size = new System.Drawing.Size(107, 21);
			this.tbQuality.TabIndex = 3;
			this.tbQuality.ThumbSize = new System.Drawing.Size(8, 14);
			this.tbQuality.ValueChanged += new System.EventHandler(this.tbQuality_ValueChanged);
			// 
			// labelX
			// 
			this.labelX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelX.Location = new System.Drawing.Point(364, 71);
			this.labelX.Name = "labelX";
			this.labelX.Size = new System.Drawing.Size(21, 21);
			this.labelX.TabIndex = 7;
			this.labelX.Text = "x";
			this.labelX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// grpFileFormat
			// 
			this.grpFileFormat.Controls.Add(this.txTagsToAppend);
			this.grpFileFormat.Controls.Add(this.labelTagToAppend);
			this.grpFileFormat.Controls.Add(this.txIncludePages);
			this.grpFileFormat.Controls.Add(this.labelIncludePages);
			this.grpFileFormat.Controls.Add(this.btRemovePageFilter);
			this.grpFileFormat.Controls.Add(this.cbCompression);
			this.grpFileFormat.Controls.Add(this.labelCompression);
			this.grpFileFormat.Controls.Add(this.chkEmbedComicInfo);
			this.grpFileFormat.Controls.Add(this.cbComicFormat);
			this.grpFileFormat.Controls.Add(this.labelComicFormat);
			this.grpFileFormat.Controls.Add(this.txRemovedPages);
			this.grpFileFormat.Controls.Add(this.labelRemovePageFilter);
			this.grpFileFormat.Dock = System.Windows.Forms.DockStyle.Top;
			this.grpFileFormat.Location = new System.Drawing.Point(4, 356);
			this.grpFileFormat.Name = "grpFileFormat";
			this.grpFileFormat.Size = new System.Drawing.Size(473, 206);
			this.grpFileFormat.TabIndex = 1;
			this.grpFileFormat.TabStop = false;
			this.grpFileFormat.Text = "File Format";
			// 
			// txTagsToAppend
			// 
			this.txTagsToAppend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txTagsToAppend.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txTagsToAppend.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txTagsToAppend.Location = new System.Drawing.Point(116, 176);
			this.txTagsToAppend.Name = "txTagsToAppend";
			this.txTagsToAppend.Size = new System.Drawing.Size(340, 20);
			this.txTagsToAppend.TabIndex = 11;
			this.txTagsToAppend.Tag = "";
			// 
			// labelTagToAppend
			// 
			this.labelTagToAppend.Location = new System.Drawing.Point(15, 176);
			this.labelTagToAppend.Name = "labelTagToAppend";
			this.labelTagToAppend.Size = new System.Drawing.Size(98, 21);
			this.labelTagToAppend.TabIndex = 10;
			this.labelTagToAppend.Text = "Tags to Append: ";
			this.labelTagToAppend.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txIncludePages
			// 
			this.txIncludePages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txIncludePages.Location = new System.Drawing.Point(116, 112);
			this.txIncludePages.Name = "txIncludePages";
			this.txIncludePages.Size = new System.Drawing.Size(340, 20);
			this.txIncludePages.TabIndex = 6;
			// 
			// labelIncludePages
			// 
			this.labelIncludePages.Location = new System.Drawing.Point(15, 111);
			this.labelIncludePages.Name = "labelIncludePages";
			this.labelIncludePages.Size = new System.Drawing.Size(98, 21);
			this.labelIncludePages.TabIndex = 5;
			this.labelIncludePages.Text = "Include Pages:";
			this.labelIncludePages.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// btRemovePageFilter
			// 
			this.btRemovePageFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btRemovePageFilter.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.SmallArrowDown;
			this.btRemovePageFilter.Location = new System.Drawing.Point(434, 139);
			this.btRemovePageFilter.Name = "btRemovePageFilter";
			this.btRemovePageFilter.Size = new System.Drawing.Size(22, 23);
			this.btRemovePageFilter.TabIndex = 9;
			this.btRemovePageFilter.UseVisualStyleBackColor = true;
			this.btRemovePageFilter.Click += new System.EventHandler(this.btRemovePageFilter_Click);
			// 
			// cbCompression
			// 
			this.cbCompression.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbCompression.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbCompression.FormattingEnabled = true;
			this.cbCompression.Items.AddRange(new object[] {
            "None",
            "Medium",
            "Strong"});
			this.cbCompression.Location = new System.Drawing.Point(116, 70);
			this.cbCompression.Name = "cbCompression";
			this.cbCompression.Size = new System.Drawing.Size(151, 21);
			this.cbCompression.TabIndex = 3;
			// 
			// labelCompression
			// 
			this.labelCompression.Location = new System.Drawing.Point(12, 70);
			this.labelCompression.Name = "labelCompression";
			this.labelCompression.Size = new System.Drawing.Size(98, 21);
			this.labelCompression.TabIndex = 2;
			this.labelCompression.Text = "Compression:";
			this.labelCompression.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// chkEmbedComicInfo
			// 
			this.chkEmbedComicInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkEmbedComicInfo.AutoSize = true;
			this.chkEmbedComicInfo.Location = new System.Drawing.Point(295, 73);
			this.chkEmbedComicInfo.Name = "chkEmbedComicInfo";
			this.chkEmbedComicInfo.Size = new System.Drawing.Size(108, 17);
			this.chkEmbedComicInfo.TabIndex = 4;
			this.chkEmbedComicInfo.Text = "Embed Book Info";
			this.chkEmbedComicInfo.UseVisualStyleBackColor = true;
			// 
			// cbComicFormat
			// 
			this.cbComicFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbComicFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbComicFormat.FormattingEnabled = true;
			this.cbComicFormat.Items.AddRange(new object[] {
            "Same as Original"});
			this.cbComicFormat.Location = new System.Drawing.Point(116, 43);
			this.cbComicFormat.Name = "cbComicFormat";
			this.cbComicFormat.Size = new System.Drawing.Size(340, 21);
			this.cbComicFormat.TabIndex = 1;
			// 
			// labelComicFormat
			// 
			this.labelComicFormat.Location = new System.Drawing.Point(9, 43);
			this.labelComicFormat.Name = "labelComicFormat";
			this.labelComicFormat.Size = new System.Drawing.Size(101, 21);
			this.labelComicFormat.TabIndex = 0;
			this.labelComicFormat.Text = "Format:";
			this.labelComicFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txRemovedPages
			// 
			this.txRemovedPages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txRemovedPages.AutoEllipsis = true;
			this.txRemovedPages.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.txRemovedPages.Location = new System.Drawing.Point(116, 141);
			this.txRemovedPages.Name = "txRemovedPages";
			this.txRemovedPages.Size = new System.Drawing.Size(312, 21);
			this.txRemovedPages.TabIndex = 8;
			this.txRemovedPages.Text = "Lorem Ipsum";
			this.txRemovedPages.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txRemovedPages.UseMnemonic = false;
			// 
			// labelRemovePageFilter
			// 
			this.labelRemovePageFilter.Location = new System.Drawing.Point(9, 141);
			this.labelRemovePageFilter.Name = "labelRemovePageFilter";
			this.labelRemovePageFilter.Size = new System.Drawing.Size(101, 21);
			this.labelRemovePageFilter.TabIndex = 7;
			this.labelRemovePageFilter.Text = "Remove Pages:";
			this.labelRemovePageFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// grpFileNaming
			// 
			this.grpFileNaming.Controls.Add(this.txCustomStartIndex);
			this.grpFileNaming.Controls.Add(this.labelCustomStartIndex);
			this.grpFileNaming.Controls.Add(this.txCustomName);
			this.grpFileNaming.Controls.Add(this.labelCustomNaming);
			this.grpFileNaming.Controls.Add(this.cbNamingTemplate);
			this.grpFileNaming.Controls.Add(this.labelNamingTemplate);
			this.grpFileNaming.Dock = System.Windows.Forms.DockStyle.Top;
			this.grpFileNaming.Location = new System.Drawing.Point(4, 226);
			this.grpFileNaming.Name = "grpFileNaming";
			this.grpFileNaming.Size = new System.Drawing.Size(473, 130);
			this.grpFileNaming.TabIndex = 9;
			this.grpFileNaming.Text = "File Naming";
			// 
			// txCustomStartIndex
			// 
			this.txCustomStartIndex.Location = new System.Drawing.Point(116, 98);
			this.txCustomStartIndex.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
			this.txCustomStartIndex.Name = "txCustomStartIndex";
			this.txCustomStartIndex.Size = new System.Drawing.Size(67, 20);
			this.txCustomStartIndex.TabIndex = 5;
			this.txCustomStartIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txCustomStartIndex.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			// 
			// labelCustomStartIndex
			// 
			this.labelCustomStartIndex.Location = new System.Drawing.Point(6, 98);
			this.labelCustomStartIndex.Name = "labelCustomStartIndex";
			this.labelCustomStartIndex.Size = new System.Drawing.Size(104, 21);
			this.labelCustomStartIndex.TabIndex = 4;
			this.labelCustomStartIndex.Text = "Start:";
			this.labelCustomStartIndex.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txCustomName
			// 
			this.txCustomName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txCustomName.Location = new System.Drawing.Point(116, 72);
			this.txCustomName.Name = "txCustomName";
			this.txCustomName.Size = new System.Drawing.Size(340, 20);
			this.txCustomName.TabIndex = 3;
			// 
			// labelCustomNaming
			// 
			this.labelCustomNaming.Location = new System.Drawing.Point(6, 71);
			this.labelCustomNaming.Name = "labelCustomNaming";
			this.labelCustomNaming.Size = new System.Drawing.Size(104, 21);
			this.labelCustomNaming.TabIndex = 2;
			this.labelCustomNaming.Text = "Custom:";
			this.labelCustomNaming.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cbNamingTemplate
			// 
			this.cbNamingTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbNamingTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbNamingTemplate.FormattingEnabled = true;
			this.cbNamingTemplate.Items.AddRange(new object[] {
            "Filename",
            "Book Caption",
            "Custom"});
			this.cbNamingTemplate.Location = new System.Drawing.Point(116, 45);
			this.cbNamingTemplate.Name = "cbNamingTemplate";
			this.cbNamingTemplate.Size = new System.Drawing.Size(340, 21);
			this.cbNamingTemplate.TabIndex = 1;
			this.cbNamingTemplate.SelectedIndexChanged += new System.EventHandler(this.cbNamingTemplate_SelectedIndexChanged);
			// 
			// labelNamingTemplate
			// 
			this.labelNamingTemplate.Location = new System.Drawing.Point(6, 45);
			this.labelNamingTemplate.Name = "labelNamingTemplate";
			this.labelNamingTemplate.Size = new System.Drawing.Size(104, 21);
			this.labelNamingTemplate.TabIndex = 0;
			this.labelNamingTemplate.Text = "Template:";
			this.labelNamingTemplate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// grpExportLocation
			// 
			this.grpExportLocation.Controls.Add(this.chkCombine);
			this.grpExportLocation.Controls.Add(this.chkOverwrite);
			this.grpExportLocation.Controls.Add(this.chkAddNewToLibrary);
			this.grpExportLocation.Controls.Add(this.chkDeleteOriginal);
			this.grpExportLocation.Controls.Add(this.btChooseFolder);
			this.grpExportLocation.Controls.Add(this.txFolder);
			this.grpExportLocation.Controls.Add(this.labelFolder);
			this.grpExportLocation.Controls.Add(this.cbExport);
			this.grpExportLocation.Controls.Add(this.labelExportTo);
			this.grpExportLocation.Dock = System.Windows.Forms.DockStyle.Top;
			this.grpExportLocation.Location = new System.Drawing.Point(4, 4);
			this.grpExportLocation.Name = "grpExportLocation";
			this.grpExportLocation.Size = new System.Drawing.Size(473, 222);
			this.grpExportLocation.TabIndex = 0;
			this.grpExportLocation.TabStop = false;
			this.grpExportLocation.Text = "Export Location";
			// 
			// chkCombine
			// 
			this.chkCombine.AutoSize = true;
			this.chkCombine.Location = new System.Drawing.Point(117, 113);
			this.chkCombine.Name = "chkCombine";
			this.chkCombine.Size = new System.Drawing.Size(147, 17);
			this.chkCombine.TabIndex = 5;
			this.chkCombine.Text = "Combine all selected Files";
			this.chkCombine.UseVisualStyleBackColor = true;
			// 
			// chkOverwrite
			// 
			this.chkOverwrite.AutoSize = true;
			this.chkOverwrite.Location = new System.Drawing.Point(117, 136);
			this.chkOverwrite.Name = "chkOverwrite";
			this.chkOverwrite.Size = new System.Drawing.Size(133, 17);
			this.chkOverwrite.TabIndex = 6;
			this.chkOverwrite.Text = "Overwrite existing Files";
			this.chkOverwrite.UseVisualStyleBackColor = true;
			// 
			// chkAddNewToLibrary
			// 
			this.chkAddNewToLibrary.AutoSize = true;
			this.chkAddNewToLibrary.Location = new System.Drawing.Point(117, 182);
			this.chkAddNewToLibrary.Name = "chkAddNewToLibrary";
			this.chkAddNewToLibrary.Size = new System.Drawing.Size(188, 17);
			this.chkAddNewToLibrary.TabIndex = 8;
			this.chkAddNewToLibrary.Text = "Add newly created Book to Library";
			this.chkAddNewToLibrary.UseVisualStyleBackColor = true;
			// 
			// chkDeleteOriginal
			// 
			this.chkDeleteOriginal.AutoSize = true;
			this.chkDeleteOriginal.Location = new System.Drawing.Point(117, 159);
			this.chkDeleteOriginal.Name = "chkDeleteOriginal";
			this.chkDeleteOriginal.Size = new System.Drawing.Size(231, 17);
			this.chkDeleteOriginal.TabIndex = 7;
			this.chkDeleteOriginal.Text = "Delete original Book after successful Export";
			this.chkDeleteOriginal.UseVisualStyleBackColor = true;
			// 
			// btChooseFolder
			// 
			this.btChooseFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btChooseFolder.Location = new System.Drawing.Point(381, 70);
			this.btChooseFolder.Name = "btChooseFolder";
			this.btChooseFolder.Size = new System.Drawing.Size(75, 23);
			this.btChooseFolder.TabIndex = 4;
			this.btChooseFolder.Text = "Choose...";
			this.btChooseFolder.UseVisualStyleBackColor = true;
			this.btChooseFolder.Click += new System.EventHandler(this.btChooseFolder_Click);
			// 
			// txFolder
			// 
			this.txFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txFolder.AutoEllipsis = true;
			this.txFolder.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.txFolder.Location = new System.Drawing.Point(116, 72);
			this.txFolder.Name = "txFolder";
			this.txFolder.Size = new System.Drawing.Size(259, 21);
			this.txFolder.TabIndex = 3;
			this.txFolder.Text = "Lorem Ipsum";
			this.txFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txFolder.UseMnemonic = false;
			// 
			// labelFolder
			// 
			this.labelFolder.Location = new System.Drawing.Point(9, 73);
			this.labelFolder.Name = "labelFolder";
			this.labelFolder.Size = new System.Drawing.Size(101, 21);
			this.labelFolder.TabIndex = 2;
			this.labelFolder.Text = "Folder:";
			this.labelFolder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cbExport
			// 
			this.cbExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbExport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbExport.FormattingEnabled = true;
			this.cbExport.Items.AddRange(new object[] {
            "Select a Folder",
            "Same Folder as the original Book",
            "Same Folder and replace in Library",
            "Ask before Export"});
			this.cbExport.Location = new System.Drawing.Point(116, 43);
			this.cbExport.Name = "cbExport";
			this.cbExport.Size = new System.Drawing.Size(340, 21);
			this.cbExport.TabIndex = 1;
			// 
			// labelExportTo
			// 
			this.labelExportTo.Location = new System.Drawing.Point(6, 43);
			this.labelExportTo.Name = "labelExportTo";
			this.labelExportTo.Size = new System.Drawing.Size(104, 21);
			this.labelExportTo.TabIndex = 0;
			this.labelExportTo.Text = "Export To:";
			this.labelExportTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ExportComicsDialog
			// 
			this.AcceptButton = this.btOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(752, 472);
			this.Controls.Add(this.exportSettings);
			this.Controls.Add(this.btRemovePreset);
			this.Controls.Add(this.btSavePreset);
			this.Controls.Add(this.tvPresets);
			this.Controls.Add(this.btOK);
			this.Controls.Add(this.btCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExportComicsDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Export Books";
			this.exportSettings.ResumeLayout(false);
			this.grpImageProcessing.ResumeLayout(false);
			this.grpImageProcessing.PerformLayout();
			this.grpCustomProcessing.ResumeLayout(false);
			this.grpCustomProcessing.PerformLayout();
			this.grpPageFormat.ResumeLayout(false);
			this.grpPageFormat.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.txHeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txWidth)).EndInit();
			this.grpFileFormat.ResumeLayout(false);
			this.grpFileFormat.PerformLayout();
			this.grpFileNaming.ResumeLayout(false);
			this.grpFileNaming.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.txCustomStartIndex)).EndInit();
			this.grpExportLocation.ResumeLayout(false);
			this.grpExportLocation.PerformLayout();
			this.ResumeLayout(false);

		}

		private CollapsibleGroupBox grpExportLocation;
		private ComboBox cbExport;
		private Label labelExportTo;
		private Button btChooseFolder;
		private Label txFolder;
		private Label labelFolder;
		private CollapsibleGroupBox grpFileFormat;
		private ComboBox cbPageFormat;
		private Label labelPageFormat;
		private Label labelPageQuality;
		private Label labelRemovePageFilter;
		private TrackBarLite tbQuality;
		private ComboBox cbComicFormat;
		private Label labelComicFormat;
		private Label txRemovedPages;
		private CheckBox chkEmbedComicInfo;
		private CollapsibleGroupBox grpPageFormat;
		private Label labelPageResize;
		private ComboBox cbPageResize;
		private CheckBox chkDontEnlarge;
		private NumericUpDown txHeight;
		private NumericUpDown txWidth;
		private CheckBox chkAddNewToLibrary;
		private CheckBox chkDeleteOriginal;
		private ComboBox cbCompression;
		private Label labelCompression;
		private Button btCancel;
		private Button btOK;
		private Label labelX;
		private TreeView tvPresets;
		private Button btSavePreset;
		private Button btRemovePreset;
		private ContextMenuStrip contextRemovePageFilter;
		private Button btRemovePageFilter;
		private Panel exportSettings;
		private CollapsibleGroupBox grpFileNaming;
		private ComboBox cbNamingTemplate;
		private Label labelNamingTemplate;
		private Label labelCustomNaming;
		private TextBox txCustomName;
		private NumericUpDown txCustomStartIndex;
		private Label labelCustomStartIndex;
		private CheckBox chkOverwrite;
		private CollapsibleGroupBox grpImageProcessing;
		private CheckBox chkAutoContrast;
		private TrackBarLite tbSharpening;
		private Label labelSharpening;
		private Button btResetColors;
		private Label labelBrightness;
		private TrackBarLite tbContrast;
		private Label labelSaturation;
		private TrackBarLite tbBrightness;
		private TrackBarLite tbSaturation;
		private Label labelContrast;
		private ComboBox cbImageProcessingSource;
		private Label labelImagProcessingSource;
		private GroupBox grpCustomProcessing;
		private Label labelImageProcessingCustom;
		private CheckBox chkIgnoreErrorPages;
		private ToolTip toolTip;
		private CheckBox chkCombine;
		private TextBox txIncludePages;
		private Label labelIncludePages;
		private Label labelGamma;
		private TrackBarLite tbGamma;
		private Label labelDoublePages;
		private ComboBox cbDoublePages;
		private CheckBox chkKeepOriginalNames;
		private TextBoxEx txTagsToAppend;
		private Label labelTagToAppend;
	}
}
