using cYo.Common.Win32;
using cYo.Common.Windows.Forms;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class ExportComicsDialog
	{
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
			components = new System.ComponentModel.Container();
			contextRemovePageFilter = new System.Windows.Forms.ContextMenuStrip(components);
			btCancel = new System.Windows.Forms.Button();
			btOK = new System.Windows.Forms.Button();
			tvPresets = new System.Windows.Forms.TreeView();
			btSavePreset = new System.Windows.Forms.Button();
			btRemovePreset = new System.Windows.Forms.Button();
			exportSettings = new System.Windows.Forms.Panel();
			grpImageProcessing = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			grpCustomProcessing = new System.Windows.Forms.GroupBox();
			labelGamma = new System.Windows.Forms.Label();
			tbGamma = new cYo.Common.Windows.Forms.TrackBarLite();
			tbSaturation = new cYo.Common.Windows.Forms.TrackBarLite();
			labelContrast = new System.Windows.Forms.Label();
			tbBrightness = new cYo.Common.Windows.Forms.TrackBarLite();
			labelSaturation = new System.Windows.Forms.Label();
			tbSharpening = new cYo.Common.Windows.Forms.TrackBarLite();
			tbContrast = new cYo.Common.Windows.Forms.TrackBarLite();
			labelSharpening = new System.Windows.Forms.Label();
			labelBrightness = new System.Windows.Forms.Label();
			btResetColors = new System.Windows.Forms.Button();
			labelImageProcessingCustom = new System.Windows.Forms.Label();
			cbImageProcessingSource = new System.Windows.Forms.ComboBox();
			labelImagProcessingSource = new System.Windows.Forms.Label();
			chkAutoContrast = new System.Windows.Forms.CheckBox();
			grpPageFormat = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			labelDoublePages = new System.Windows.Forms.Label();
			cbDoublePages = new System.Windows.Forms.ComboBox();
			chkIgnoreErrorPages = new System.Windows.Forms.CheckBox();
			txHeight = new System.Windows.Forms.NumericUpDown();
			txWidth = new System.Windows.Forms.NumericUpDown();
			chkDontEnlarge = new System.Windows.Forms.CheckBox();
			labelPageResize = new System.Windows.Forms.Label();
			cbPageResize = new System.Windows.Forms.ComboBox();
			labelPageFormat = new System.Windows.Forms.Label();
			cbPageFormat = new System.Windows.Forms.ComboBox();
			labelPageQuality = new System.Windows.Forms.Label();
			tbQuality = new cYo.Common.Windows.Forms.TrackBarLite();
			labelX = new System.Windows.Forms.Label();
			grpFileFormat = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			txIncludePages = new System.Windows.Forms.TextBox();
			labelIncludePages = new System.Windows.Forms.Label();
			btRemovePageFilter = new System.Windows.Forms.Button();
			cbCompression = new System.Windows.Forms.ComboBox();
			labelCompression = new System.Windows.Forms.Label();
			chkEmbedComicInfo = new System.Windows.Forms.CheckBox();
			cbComicFormat = new System.Windows.Forms.ComboBox();
			labelComicFormat = new System.Windows.Forms.Label();
			txRemovedPages = new System.Windows.Forms.Label();
			labelRemovePageFilter = new System.Windows.Forms.Label();
			grpFileNaming = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			txCustomStartIndex = new System.Windows.Forms.NumericUpDown();
			labelCustomStartIndex = new System.Windows.Forms.Label();
			txCustomName = new System.Windows.Forms.TextBox();
			labelCustomNaming = new System.Windows.Forms.Label();
			cbNamingTemplate = new System.Windows.Forms.ComboBox();
			labelNamingTemplate = new System.Windows.Forms.Label();
			grpExportLocation = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			chkCombine = new System.Windows.Forms.CheckBox();
			chkOverwrite = new System.Windows.Forms.CheckBox();
			chkAddNewToLibrary = new System.Windows.Forms.CheckBox();
			chkDeleteOriginal = new System.Windows.Forms.CheckBox();
			btChooseFolder = new System.Windows.Forms.Button();
			txFolder = new System.Windows.Forms.Label();
			labelFolder = new System.Windows.Forms.Label();
			cbExport = new System.Windows.Forms.ComboBox();
			labelExportTo = new System.Windows.Forms.Label();
			toolTip = new System.Windows.Forms.ToolTip(components);
			chkKeepOriginalNames = new System.Windows.Forms.CheckBox();
			exportSettings.SuspendLayout();
			grpImageProcessing.SuspendLayout();
			grpCustomProcessing.SuspendLayout();
			grpPageFormat.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)txHeight).BeginInit();
			((System.ComponentModel.ISupportInitialize)txWidth).BeginInit();
			grpFileFormat.SuspendLayout();
			grpFileNaming.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)txCustomStartIndex).BeginInit();
			grpExportLocation.SuspendLayout();
			SuspendLayout();
			contextRemovePageFilter.Name = "contextRemovePageFilter";
			contextRemovePageFilter.Size = new System.Drawing.Size(61, 4);
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(660, 440);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 7;
			btCancel.Text = "&Cancel";
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(574, 440);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(80, 24);
			btOK.TabIndex = 6;
			btOK.Text = "&OK";
			tvPresets.Location = new System.Drawing.Point(14, 18);
			tvPresets.Name = "tvPresets";
			tvPresets.ShowNodeToolTips = true;
			tvPresets.Size = new System.Drawing.Size(220, 387);
			tvPresets.TabIndex = 3;
			tvPresets.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(tvPresets_AfterSelect);
			btSavePreset.Location = new System.Drawing.Point(14, 411);
			btSavePreset.Name = "btSavePreset";
			btSavePreset.Size = new System.Drawing.Size(107, 23);
			btSavePreset.TabIndex = 4;
			btSavePreset.Text = "Save,,,";
			btSavePreset.UseVisualStyleBackColor = true;
			btSavePreset.Click += new System.EventHandler(btAddPreset_Click);
			btRemovePreset.Location = new System.Drawing.Point(127, 411);
			btRemovePreset.Name = "btRemovePreset";
			btRemovePreset.Size = new System.Drawing.Size(107, 23);
			btRemovePreset.TabIndex = 5;
			btRemovePreset.Text = "Remove";
			btRemovePreset.UseVisualStyleBackColor = true;
			btRemovePreset.Click += new System.EventHandler(btRemovePreset_Click);
			exportSettings.AutoScroll = true;
			exportSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			exportSettings.Controls.Add(grpImageProcessing);
			exportSettings.Controls.Add(grpPageFormat);
			exportSettings.Controls.Add(grpFileFormat);
			exportSettings.Controls.Add(grpFileNaming);
			exportSettings.Controls.Add(grpExportLocation);
			exportSettings.Location = new System.Drawing.Point(240, 18);
			exportSettings.Name = "exportSettings";
			exportSettings.Padding = new System.Windows.Forms.Padding(4);
			exportSettings.Size = new System.Drawing.Size(500, 416);
			exportSettings.TabIndex = 8;
			grpImageProcessing.Controls.Add(grpCustomProcessing);
			grpImageProcessing.Controls.Add(labelImageProcessingCustom);
			grpImageProcessing.Controls.Add(cbImageProcessingSource);
			grpImageProcessing.Controls.Add(labelImagProcessingSource);
			grpImageProcessing.Controls.Add(chkAutoContrast);
			grpImageProcessing.Dock = System.Windows.Forms.DockStyle.Top;
			grpImageProcessing.Location = new System.Drawing.Point(4, 737);
			grpImageProcessing.Name = "grpImageProcessing";
			grpImageProcessing.Size = new System.Drawing.Size(473, 290);
			grpImageProcessing.TabIndex = 9;
			grpImageProcessing.Text = "Image Processing";
			grpCustomProcessing.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			grpCustomProcessing.Controls.Add(labelGamma);
			grpCustomProcessing.Controls.Add(tbGamma);
			grpCustomProcessing.Controls.Add(tbSaturation);
			grpCustomProcessing.Controls.Add(labelContrast);
			grpCustomProcessing.Controls.Add(tbBrightness);
			grpCustomProcessing.Controls.Add(labelSaturation);
			grpCustomProcessing.Controls.Add(tbSharpening);
			grpCustomProcessing.Controls.Add(tbContrast);
			grpCustomProcessing.Controls.Add(labelSharpening);
			grpCustomProcessing.Controls.Add(labelBrightness);
			grpCustomProcessing.Controls.Add(btResetColors);
			grpCustomProcessing.Location = new System.Drawing.Point(110, 66);
			grpCustomProcessing.Name = "grpCustomProcessing";
			grpCustomProcessing.Size = new System.Drawing.Size(346, 183);
			grpCustomProcessing.TabIndex = 3;
			grpCustomProcessing.TabStop = false;
			labelGamma.AutoSize = true;
			labelGamma.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold);
			labelGamma.Location = new System.Drawing.Point(6, 97);
			labelGamma.Name = "labelGamma";
			labelGamma.Size = new System.Drawing.Size(43, 12);
			labelGamma.TabIndex = 6;
			labelGamma.Text = "Gamma";
			tbGamma.Location = new System.Drawing.Point(95, 91);
			tbGamma.Minimum = -100;
			tbGamma.Name = "tbGamma";
			tbGamma.Size = new System.Drawing.Size(245, 18);
			tbGamma.TabIndex = 7;
			tbGamma.Text = "tbSaturation";
			tbGamma.ThumbSize = new System.Drawing.Size(8, 16);
			tbGamma.TickFrequency = 16;
			tbGamma.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			tbGamma.ValueChanged += new System.EventHandler(AdjustmentSliderChanged);
			tbSaturation.Location = new System.Drawing.Point(95, 19);
			tbSaturation.Minimum = -100;
			tbSaturation.Name = "tbSaturation";
			tbSaturation.Size = new System.Drawing.Size(245, 18);
			tbSaturation.TabIndex = 1;
			tbSaturation.ThumbSize = new System.Drawing.Size(8, 16);
			tbSaturation.TickFrequency = 16;
			tbSaturation.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			tbSaturation.ValueChanged += new System.EventHandler(AdjustmentSliderChanged);
			labelContrast.AutoSize = true;
			labelContrast.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold);
			labelContrast.Location = new System.Drawing.Point(6, 73);
			labelContrast.Name = "labelContrast";
			labelContrast.Size = new System.Drawing.Size(49, 12);
			labelContrast.TabIndex = 4;
			labelContrast.Text = "Contrast";
			tbBrightness.Location = new System.Drawing.Point(95, 43);
			tbBrightness.Minimum = -100;
			tbBrightness.Name = "tbBrightness";
			tbBrightness.Size = new System.Drawing.Size(245, 18);
			tbBrightness.TabIndex = 3;
			tbBrightness.Text = "trackBarLite3";
			tbBrightness.ThumbSize = new System.Drawing.Size(8, 16);
			tbBrightness.TickFrequency = 16;
			tbBrightness.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			tbBrightness.ValueChanged += new System.EventHandler(AdjustmentSliderChanged);
			labelSaturation.AutoSize = true;
			labelSaturation.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold);
			labelSaturation.Location = new System.Drawing.Point(6, 25);
			labelSaturation.Name = "labelSaturation";
			labelSaturation.Size = new System.Drawing.Size(57, 12);
			labelSaturation.TabIndex = 0;
			labelSaturation.Text = "Saturation";
			tbSharpening.LargeChange = 1;
			tbSharpening.Location = new System.Drawing.Point(95, 117);
			tbSharpening.Maximum = 3;
			tbSharpening.Name = "tbSharpening";
			tbSharpening.Size = new System.Drawing.Size(245, 18);
			tbSharpening.TabIndex = 9;
			tbSharpening.Text = "tbSaturation";
			tbSharpening.ThumbSize = new System.Drawing.Size(8, 16);
			tbSharpening.TickFrequency = 1;
			tbSharpening.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			tbContrast.Location = new System.Drawing.Point(95, 67);
			tbContrast.Minimum = -100;
			tbContrast.Name = "tbContrast";
			tbContrast.Size = new System.Drawing.Size(245, 18);
			tbContrast.TabIndex = 5;
			tbContrast.Text = "tbSaturation";
			tbContrast.ThumbSize = new System.Drawing.Size(8, 16);
			tbContrast.TickFrequency = 16;
			tbContrast.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			tbContrast.ValueChanged += new System.EventHandler(AdjustmentSliderChanged);
			labelSharpening.AutoSize = true;
			labelSharpening.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold);
			labelSharpening.Location = new System.Drawing.Point(6, 123);
			labelSharpening.Name = "labelSharpening";
			labelSharpening.Size = new System.Drawing.Size(61, 12);
			labelSharpening.TabIndex = 8;
			labelSharpening.Text = "Sharpening";
			labelBrightness.AutoSize = true;
			labelBrightness.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold);
			labelBrightness.Location = new System.Drawing.Point(6, 49);
			labelBrightness.Name = "labelBrightness";
			labelBrightness.Size = new System.Drawing.Size(59, 12);
			labelBrightness.TabIndex = 2;
			labelBrightness.Text = "Brightness";
			btResetColors.Location = new System.Drawing.Point(257, 149);
			btResetColors.Name = "btResetColors";
			btResetColors.Size = new System.Drawing.Size(77, 24);
			btResetColors.TabIndex = 10;
			btResetColors.Text = "Reset";
			btResetColors.UseVisualStyleBackColor = true;
			btResetColors.Click += new System.EventHandler(btResetColors_Click);
			labelImageProcessingCustom.Location = new System.Drawing.Point(0, 66);
			labelImageProcessingCustom.Name = "labelImageProcessingCustom";
			labelImageProcessingCustom.Size = new System.Drawing.Size(104, 21);
			labelImageProcessingCustom.TabIndex = 2;
			labelImageProcessingCustom.Text = "Custom:";
			labelImageProcessingCustom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			cbImageProcessingSource.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			cbImageProcessingSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbImageProcessingSource.FormattingEnabled = true;
			cbImageProcessingSource.Items.AddRange(new object[2]
			{
				"Custom Settings",
				"Book Settings are applied"
			});
			cbImageProcessingSource.Location = new System.Drawing.Point(110, 40);
			cbImageProcessingSource.Name = "cbImageProcessingSource";
			cbImageProcessingSource.Size = new System.Drawing.Size(346, 21);
			cbImageProcessingSource.TabIndex = 1;
			labelImagProcessingSource.Location = new System.Drawing.Point(3, 39);
			labelImagProcessingSource.Name = "labelImagProcessingSource";
			labelImagProcessingSource.Size = new System.Drawing.Size(101, 21);
			labelImagProcessingSource.TabIndex = 0;
			labelImagProcessingSource.Text = "Source:";
			labelImagProcessingSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			chkAutoContrast.AutoSize = true;
			chkAutoContrast.Location = new System.Drawing.Point(110, 255);
			chkAutoContrast.Name = "chkAutoContrast";
			chkAutoContrast.Size = new System.Drawing.Size(184, 17);
			chkAutoContrast.TabIndex = 4;
			chkAutoContrast.Text = "Automatic Contrast Enhancement";
			chkAutoContrast.UseVisualStyleBackColor = true;
			grpPageFormat.Controls.Add(chkKeepOriginalNames);
			grpPageFormat.Controls.Add(labelDoublePages);
			grpPageFormat.Controls.Add(cbDoublePages);
			grpPageFormat.Controls.Add(chkIgnoreErrorPages);
			grpPageFormat.Controls.Add(txHeight);
			grpPageFormat.Controls.Add(txWidth);
			grpPageFormat.Controls.Add(chkDontEnlarge);
			grpPageFormat.Controls.Add(labelPageResize);
			grpPageFormat.Controls.Add(cbPageResize);
			grpPageFormat.Controls.Add(labelPageFormat);
			grpPageFormat.Controls.Add(cbPageFormat);
			grpPageFormat.Controls.Add(labelPageQuality);
			grpPageFormat.Controls.Add(tbQuality);
			grpPageFormat.Controls.Add(labelX);
			grpPageFormat.Dock = System.Windows.Forms.DockStyle.Top;
			grpPageFormat.Location = new System.Drawing.Point(4, 532);
			grpPageFormat.Name = "grpPageFormat";
			grpPageFormat.Size = new System.Drawing.Size(473, 205);
			grpPageFormat.TabIndex = 2;
			grpPageFormat.TabStop = false;
			grpPageFormat.Text = "Page Format";
			labelDoublePages.Location = new System.Drawing.Point(6, 96);
			labelDoublePages.Name = "labelDoublePages";
			labelDoublePages.Size = new System.Drawing.Size(98, 21);
			labelDoublePages.TabIndex = 9;
			labelDoublePages.Text = "Double Pages:";
			labelDoublePages.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			cbDoublePages.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			cbDoublePages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbDoublePages.FormattingEnabled = true;
			cbDoublePages.Items.AddRange(new object[4]
			{
				"Keep",
				"Split",
				"Rotate 90°",
				"Adapt Width"
			});
			cbDoublePages.Location = new System.Drawing.Point(110, 96);
			cbDoublePages.Name = "cbDoublePages";
			cbDoublePages.Size = new System.Drawing.Size(157, 21);
			cbDoublePages.TabIndex = 10;
			chkIgnoreErrorPages.AutoSize = true;
			chkIgnoreErrorPages.Location = new System.Drawing.Point(110, 155);
			chkIgnoreErrorPages.Name = "chkIgnoreErrorPages";
			chkIgnoreErrorPages.Size = new System.Drawing.Size(237, 17);
			chkIgnoreErrorPages.TabIndex = 12;
			chkIgnoreErrorPages.Text = "Ignore Pages with errors and continue export";
			chkIgnoreErrorPages.UseVisualStyleBackColor = true;
			txHeight.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			txHeight.Location = new System.Drawing.Point(391, 71);
			txHeight.Maximum = new decimal(new int[4]
			{
				8000,
				0,
				0,
				0
			});
			txHeight.Name = "txHeight";
			txHeight.Size = new System.Drawing.Size(65, 20);
			txHeight.TabIndex = 8;
			txHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			txHeight.Value = new decimal(new int[4]
			{
				1000,
				0,
				0,
				0
			});
			txWidth.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			txWidth.Location = new System.Drawing.Point(291, 72);
			txWidth.Maximum = new decimal(new int[4]
			{
				8000,
				0,
				0,
				0
			});
			txWidth.Name = "txWidth";
			txWidth.Size = new System.Drawing.Size(67, 20);
			txWidth.TabIndex = 6;
			txWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			txWidth.Value = new decimal(new int[4]
			{
				1000,
				0,
				0,
				0
			});
			chkDontEnlarge.AutoSize = true;
			chkDontEnlarge.Location = new System.Drawing.Point(110, 132);
			chkDontEnlarge.Name = "chkDontEnlarge";
			chkDontEnlarge.Size = new System.Drawing.Size(89, 17);
			chkDontEnlarge.TabIndex = 11;
			chkDontEnlarge.Text = "Don't enlarge";
			chkDontEnlarge.UseVisualStyleBackColor = true;
			labelPageResize.Location = new System.Drawing.Point(6, 69);
			labelPageResize.Name = "labelPageResize";
			labelPageResize.Size = new System.Drawing.Size(98, 21);
			labelPageResize.TabIndex = 4;
			labelPageResize.Text = "Resize Pages:";
			labelPageResize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			cbPageResize.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			cbPageResize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbPageResize.FormattingEnabled = true;
			cbPageResize.Items.AddRange(new object[4]
			{
				"Preserve Original",
				"Best fit Width & Height",
				"Set Width",
				"Set Height"
			});
			cbPageResize.Location = new System.Drawing.Point(110, 69);
			cbPageResize.Name = "cbPageResize";
			cbPageResize.Size = new System.Drawing.Size(157, 21);
			cbPageResize.TabIndex = 5;
			labelPageFormat.Location = new System.Drawing.Point(3, 42);
			labelPageFormat.Name = "labelPageFormat";
			labelPageFormat.Size = new System.Drawing.Size(101, 21);
			labelPageFormat.TabIndex = 0;
			labelPageFormat.Text = "Format:";
			labelPageFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			cbPageFormat.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			cbPageFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbPageFormat.FormattingEnabled = true;
			cbPageFormat.Items.AddRange(new object[8]
			{
				"Preserve Original",
				"JPEG",
				"PNG",
				"GIF",
				"TIFF",
				"BMP",
				"DJVU",
				"WEBP"
			});
			cbPageFormat.Location = new System.Drawing.Point(110, 42);
			cbPageFormat.Name = "cbPageFormat";
			cbPageFormat.Size = new System.Drawing.Size(157, 21);
			cbPageFormat.TabIndex = 1;
			labelPageQuality.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			labelPageQuality.Location = new System.Drawing.Point(273, 41);
			labelPageQuality.Name = "labelPageQuality";
			labelPageQuality.Size = new System.Drawing.Size(77, 21);
			labelPageQuality.TabIndex = 2;
			labelPageQuality.Text = "Quality:";
			labelPageQuality.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			tbQuality.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			tbQuality.Location = new System.Drawing.Point(349, 41);
			tbQuality.Name = "tbQuality";
			tbQuality.Size = new System.Drawing.Size(107, 21);
			tbQuality.TabIndex = 3;
			tbQuality.ThumbSize = new System.Drawing.Size(8, 14);
			tbQuality.ValueChanged += new System.EventHandler(tbQuality_ValueChanged);
			labelX.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			labelX.Location = new System.Drawing.Point(364, 71);
			labelX.Name = "labelX";
			labelX.Size = new System.Drawing.Size(21, 21);
			labelX.TabIndex = 7;
			labelX.Text = "x";
			labelX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			grpFileFormat.Controls.Add(txIncludePages);
			grpFileFormat.Controls.Add(labelIncludePages);
			grpFileFormat.Controls.Add(btRemovePageFilter);
			grpFileFormat.Controls.Add(cbCompression);
			grpFileFormat.Controls.Add(labelCompression);
			grpFileFormat.Controls.Add(chkEmbedComicInfo);
			grpFileFormat.Controls.Add(cbComicFormat);
			grpFileFormat.Controls.Add(labelComicFormat);
			grpFileFormat.Controls.Add(txRemovedPages);
			grpFileFormat.Controls.Add(labelRemovePageFilter);
			grpFileFormat.Dock = System.Windows.Forms.DockStyle.Top;
			grpFileFormat.Location = new System.Drawing.Point(4, 356);
			grpFileFormat.Name = "grpFileFormat";
			grpFileFormat.Size = new System.Drawing.Size(473, 176);
			grpFileFormat.TabIndex = 1;
			grpFileFormat.TabStop = false;
			grpFileFormat.Text = "File Format";
			txIncludePages.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txIncludePages.Location = new System.Drawing.Point(116, 112);
			txIncludePages.Name = "txIncludePages";
			txIncludePages.Size = new System.Drawing.Size(340, 20);
			txIncludePages.TabIndex = 6;
			labelIncludePages.Location = new System.Drawing.Point(15, 111);
			labelIncludePages.Name = "labelIncludePages";
			labelIncludePages.Size = new System.Drawing.Size(98, 21);
			labelIncludePages.TabIndex = 5;
			labelIncludePages.Text = "Include Pages:";
			labelIncludePages.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			btRemovePageFilter.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btRemovePageFilter.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.SmallArrowDown;
			btRemovePageFilter.Location = new System.Drawing.Point(434, 139);
			btRemovePageFilter.Name = "btRemovePageFilter";
			btRemovePageFilter.Size = new System.Drawing.Size(22, 23);
			btRemovePageFilter.TabIndex = 9;
			btRemovePageFilter.UseVisualStyleBackColor = true;
			btRemovePageFilter.Click += new System.EventHandler(btRemovePageFilter_Click);
			cbCompression.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			cbCompression.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbCompression.FormattingEnabled = true;
			cbCompression.Items.AddRange(new object[3]
			{
				"None",
				"Medium",
				"Strong"
			});
			cbCompression.Location = new System.Drawing.Point(116, 70);
			cbCompression.Name = "cbCompression";
			cbCompression.Size = new System.Drawing.Size(151, 21);
			cbCompression.TabIndex = 3;
			labelCompression.Location = new System.Drawing.Point(12, 70);
			labelCompression.Name = "labelCompression";
			labelCompression.Size = new System.Drawing.Size(98, 21);
			labelCompression.TabIndex = 2;
			labelCompression.Text = "Compression:";
			labelCompression.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			chkEmbedComicInfo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkEmbedComicInfo.AutoSize = true;
			chkEmbedComicInfo.Location = new System.Drawing.Point(295, 73);
			chkEmbedComicInfo.Name = "chkEmbedComicInfo";
			chkEmbedComicInfo.Size = new System.Drawing.Size(108, 17);
			chkEmbedComicInfo.TabIndex = 4;
			chkEmbedComicInfo.Text = "Embed Book Info";
			chkEmbedComicInfo.UseVisualStyleBackColor = true;
			cbComicFormat.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			cbComicFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbComicFormat.FormattingEnabled = true;
			cbComicFormat.Items.AddRange(new object[1]
			{
				"Same as Original"
			});
			cbComicFormat.Location = new System.Drawing.Point(116, 43);
			cbComicFormat.Name = "cbComicFormat";
			cbComicFormat.Size = new System.Drawing.Size(340, 21);
			cbComicFormat.TabIndex = 1;
			labelComicFormat.Location = new System.Drawing.Point(9, 43);
			labelComicFormat.Name = "labelComicFormat";
			labelComicFormat.Size = new System.Drawing.Size(101, 21);
			labelComicFormat.TabIndex = 0;
			labelComicFormat.Text = "Format:";
			labelComicFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			txRemovedPages.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txRemovedPages.AutoEllipsis = true;
			txRemovedPages.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			txRemovedPages.Location = new System.Drawing.Point(116, 141);
			txRemovedPages.Name = "txRemovedPages";
			txRemovedPages.Size = new System.Drawing.Size(312, 21);
			txRemovedPages.TabIndex = 8;
			txRemovedPages.Text = "Lorem Ipsum";
			txRemovedPages.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			txRemovedPages.UseMnemonic = false;
			labelRemovePageFilter.Location = new System.Drawing.Point(9, 141);
			labelRemovePageFilter.Name = "labelRemovePageFilter";
			labelRemovePageFilter.Size = new System.Drawing.Size(101, 21);
			labelRemovePageFilter.TabIndex = 7;
			labelRemovePageFilter.Text = "Remove Pages:";
			labelRemovePageFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			grpFileNaming.Controls.Add(txCustomStartIndex);
			grpFileNaming.Controls.Add(labelCustomStartIndex);
			grpFileNaming.Controls.Add(txCustomName);
			grpFileNaming.Controls.Add(labelCustomNaming);
			grpFileNaming.Controls.Add(cbNamingTemplate);
			grpFileNaming.Controls.Add(labelNamingTemplate);
			grpFileNaming.Dock = System.Windows.Forms.DockStyle.Top;
			grpFileNaming.Location = new System.Drawing.Point(4, 226);
			grpFileNaming.Name = "grpFileNaming";
			grpFileNaming.Size = new System.Drawing.Size(473, 130);
			grpFileNaming.TabIndex = 9;
			grpFileNaming.Text = "File Naming";
			txCustomStartIndex.Location = new System.Drawing.Point(116, 98);
			txCustomStartIndex.Maximum = new decimal(new int[4]
			{
				100000,
				0,
				0,
				0
			});
			txCustomStartIndex.Name = "txCustomStartIndex";
			txCustomStartIndex.Size = new System.Drawing.Size(67, 20);
			txCustomStartIndex.TabIndex = 5;
			txCustomStartIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			txCustomStartIndex.Value = new decimal(new int[4]
			{
				1000,
				0,
				0,
				0
			});
			labelCustomStartIndex.Location = new System.Drawing.Point(6, 98);
			labelCustomStartIndex.Name = "labelCustomStartIndex";
			labelCustomStartIndex.Size = new System.Drawing.Size(104, 21);
			labelCustomStartIndex.TabIndex = 4;
			labelCustomStartIndex.Text = "Start:";
			labelCustomStartIndex.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			txCustomName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txCustomName.Location = new System.Drawing.Point(116, 72);
			txCustomName.Name = "txCustomName";
			txCustomName.Size = new System.Drawing.Size(340, 20);
			txCustomName.TabIndex = 3;
			labelCustomNaming.Location = new System.Drawing.Point(6, 71);
			labelCustomNaming.Name = "labelCustomNaming";
			labelCustomNaming.Size = new System.Drawing.Size(104, 21);
			labelCustomNaming.TabIndex = 2;
			labelCustomNaming.Text = "Custom:";
			labelCustomNaming.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			cbNamingTemplate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			cbNamingTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbNamingTemplate.FormattingEnabled = true;
			cbNamingTemplate.Items.AddRange(new object[3]
			{
				"Filename",
				"Book Caption",
				"Custom"
			});
			cbNamingTemplate.Location = new System.Drawing.Point(116, 45);
			cbNamingTemplate.Name = "cbNamingTemplate";
			cbNamingTemplate.Size = new System.Drawing.Size(340, 21);
			cbNamingTemplate.TabIndex = 1;
			cbNamingTemplate.SelectedIndexChanged += new System.EventHandler(cbNamingTemplate_SelectedIndexChanged);
			labelNamingTemplate.Location = new System.Drawing.Point(6, 45);
			labelNamingTemplate.Name = "labelNamingTemplate";
			labelNamingTemplate.Size = new System.Drawing.Size(104, 21);
			labelNamingTemplate.TabIndex = 0;
			labelNamingTemplate.Text = "Template:";
			labelNamingTemplate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			grpExportLocation.Controls.Add(chkCombine);
			grpExportLocation.Controls.Add(chkOverwrite);
			grpExportLocation.Controls.Add(chkAddNewToLibrary);
			grpExportLocation.Controls.Add(chkDeleteOriginal);
			grpExportLocation.Controls.Add(btChooseFolder);
			grpExportLocation.Controls.Add(txFolder);
			grpExportLocation.Controls.Add(labelFolder);
			grpExportLocation.Controls.Add(cbExport);
			grpExportLocation.Controls.Add(labelExportTo);
			grpExportLocation.Dock = System.Windows.Forms.DockStyle.Top;
			grpExportLocation.Location = new System.Drawing.Point(4, 4);
			grpExportLocation.Name = "grpExportLocation";
			grpExportLocation.Size = new System.Drawing.Size(473, 222);
			grpExportLocation.TabIndex = 0;
			grpExportLocation.TabStop = false;
			grpExportLocation.Text = "Export Location";
			chkCombine.AutoSize = true;
			chkCombine.Location = new System.Drawing.Point(117, 113);
			chkCombine.Name = "chkCombine";
			chkCombine.Size = new System.Drawing.Size(147, 17);
			chkCombine.TabIndex = 5;
			chkCombine.Text = "Combine all selected Files";
			chkCombine.UseVisualStyleBackColor = true;
			chkOverwrite.AutoSize = true;
			chkOverwrite.Location = new System.Drawing.Point(117, 136);
			chkOverwrite.Name = "chkOverwrite";
			chkOverwrite.Size = new System.Drawing.Size(133, 17);
			chkOverwrite.TabIndex = 6;
			chkOverwrite.Text = "Overwrite existing Files";
			chkOverwrite.UseVisualStyleBackColor = true;
			chkAddNewToLibrary.AutoSize = true;
			chkAddNewToLibrary.Location = new System.Drawing.Point(117, 182);
			chkAddNewToLibrary.Name = "chkAddNewToLibrary";
			chkAddNewToLibrary.Size = new System.Drawing.Size(188, 17);
			chkAddNewToLibrary.TabIndex = 8;
			chkAddNewToLibrary.Text = "Add newly created Book to Library";
			chkAddNewToLibrary.UseVisualStyleBackColor = true;
			chkDeleteOriginal.AutoSize = true;
			chkDeleteOriginal.Location = new System.Drawing.Point(117, 159);
			chkDeleteOriginal.Name = "chkDeleteOriginal";
			chkDeleteOriginal.Size = new System.Drawing.Size(231, 17);
			chkDeleteOriginal.TabIndex = 7;
			chkDeleteOriginal.Text = "Delete original Book after successful Export";
			chkDeleteOriginal.UseVisualStyleBackColor = true;
			btChooseFolder.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btChooseFolder.Location = new System.Drawing.Point(381, 70);
			btChooseFolder.Name = "btChooseFolder";
			btChooseFolder.Size = new System.Drawing.Size(75, 23);
			btChooseFolder.TabIndex = 4;
			btChooseFolder.Text = "Choose...";
			btChooseFolder.UseVisualStyleBackColor = true;
			btChooseFolder.Click += new System.EventHandler(btChooseFolder_Click);
			txFolder.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txFolder.AutoEllipsis = true;
			txFolder.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			txFolder.Location = new System.Drawing.Point(116, 72);
			txFolder.Name = "txFolder";
			txFolder.Size = new System.Drawing.Size(259, 21);
			txFolder.TabIndex = 3;
			txFolder.Text = "Lorem Ipsum";
			txFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			txFolder.UseMnemonic = false;
			labelFolder.Location = new System.Drawing.Point(9, 73);
			labelFolder.Name = "labelFolder";
			labelFolder.Size = new System.Drawing.Size(101, 21);
			labelFolder.TabIndex = 2;
			labelFolder.Text = "Folder:";
			labelFolder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			cbExport.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			cbExport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbExport.FormattingEnabled = true;
			cbExport.Items.AddRange(new object[4]
			{
				"Select a Folder",
				"Same Folder as the original Book",
				"Same Folder and replace in Library",
				"Ask before Export"
			});
			cbExport.Location = new System.Drawing.Point(116, 43);
			cbExport.Name = "cbExport";
			cbExport.Size = new System.Drawing.Size(340, 21);
			cbExport.TabIndex = 1;
			labelExportTo.Location = new System.Drawing.Point(6, 43);
			labelExportTo.Name = "labelExportTo";
			labelExportTo.Size = new System.Drawing.Size(104, 21);
			labelExportTo.TabIndex = 0;
			labelExportTo.Text = "Export To:";
			labelExportTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			chkKeepOriginalNames.AutoSize = true;
			chkKeepOriginalNames.Location = new System.Drawing.Point(110, 178);
			chkKeepOriginalNames.Name = "chkKeepOriginalNames";
			chkKeepOriginalNames.Size = new System.Drawing.Size(148, 17);
			chkKeepOriginalNames.TabIndex = 13;
			chkKeepOriginalNames.Text = "Keep original page names";
			chkKeepOriginalNames.UseVisualStyleBackColor = true;
			base.AcceptButton = btOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(752, 472);
			base.Controls.Add(exportSettings);
			base.Controls.Add(btRemovePreset);
			base.Controls.Add(btSavePreset);
			base.Controls.Add(tvPresets);
			base.Controls.Add(btOK);
			base.Controls.Add(btCancel);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ExportComicsDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Export Books";
			exportSettings.ResumeLayout(false);
			grpImageProcessing.ResumeLayout(false);
			grpImageProcessing.PerformLayout();
			grpCustomProcessing.ResumeLayout(false);
			grpCustomProcessing.PerformLayout();
			grpPageFormat.ResumeLayout(false);
			grpPageFormat.PerformLayout();
			((System.ComponentModel.ISupportInitialize)txHeight).EndInit();
			((System.ComponentModel.ISupportInitialize)txWidth).EndInit();
			grpFileFormat.ResumeLayout(false);
			grpFileFormat.PerformLayout();
			grpFileNaming.ResumeLayout(false);
			grpFileNaming.PerformLayout();
			((System.ComponentModel.ISupportInitialize)txCustomStartIndex).EndInit();
			grpExportLocation.ResumeLayout(false);
			grpExportLocation.PerformLayout();
			ResumeLayout(false);
		}
		
		private IContainer components;

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
	}
}
