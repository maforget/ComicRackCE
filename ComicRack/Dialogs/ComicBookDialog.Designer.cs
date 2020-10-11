using cYo.Common.Win32;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine.Controls;
using cYo.Projects.ComicRack.Viewer.Controls;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class ComicBookDialog
	{
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Program.Settings.InformationCover3D = coverThumbnail.ThreeD;
				IdleProcess.Idle -= IdleProcess_Idle;
				PagesConfig = pagesView.ViewConfig;
				pageViewer.SetBitmap(null);
				coverThumbnail.SetBitmap(null);
				if (pagesView.Book != null)
				{
					pagesView.Book.Dispose();
				}
				if (btScript.ContextMenuStrip != null)
				{
					FormUtility.SafeToolStripClear(btScript.ContextMenuStrip.Items);
				}
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			btCancel = new System.Windows.Forms.Button();
			btOK = new System.Windows.Forms.Button();
			tabControl = new System.Windows.Forms.TabControl();
			tabSummary = new System.Windows.Forms.TabPage();
			btThumbnail = new cYo.Common.Windows.Forms.SplitButton();
			cmThumbnail = new System.Windows.Forms.ContextMenuStrip(components);
			miResetThumbnail = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			labelCommunityRating = new System.Windows.Forms.Label();
			txCommunityRating = new cYo.Projects.ComicRack.Engine.Controls.RatingControl();
			labelMyRating = new System.Windows.Forms.Label();
			btLinkFile = new System.Windows.Forms.Button();
			linkLabel = new System.Windows.Forms.LinkLabel();
			coverThumbnail = new cYo.Projects.ComicRack.Engine.Controls.ThumbnailControl();
			whereSeparator = new System.Windows.Forms.Panel();
			panel1 = new System.Windows.Forms.Panel();
			labelWhere = new System.Windows.Forms.Label();
			labelType = new System.Windows.Forms.Label();
			txRating = new cYo.Projects.ComicRack.Engine.Controls.RatingControl();
			labelPages = new System.Windows.Forms.Label();
			lblType = new System.Windows.Forms.Label();
			lblPath = new System.Windows.Forms.Label();
			lblPages = new System.Windows.Forms.Label();
			tabDetails = new System.Windows.Forms.TabPage();
			txDay = new cYo.Common.Windows.Forms.TextBoxEx();
			labelDay = new System.Windows.Forms.Label();
			labelSeriesGroup = new System.Windows.Forms.Label();
			txSeriesGroup = new cYo.Common.Windows.Forms.TextBoxEx();
			labelStoryArc = new System.Windows.Forms.Label();
			txStoryArc = new cYo.Common.Windows.Forms.TextBoxEx();
			cbSeriesComplete = new System.Windows.Forms.ComboBox();
			labelSeriesComplete = new System.Windows.Forms.Label();
			cbEnableDynamicUpdate = new System.Windows.Forms.ComboBox();
			labelEnableDynamicUpdate = new System.Windows.Forms.Label();
			txGenre = new cYo.Common.Windows.Forms.TextBoxEx();
			labelTags = new System.Windows.Forms.Label();
			cbEnableProposed = new System.Windows.Forms.ComboBox();
			txTags = new cYo.Common.Windows.Forms.TextBoxEx();
			labelEnableProposed = new System.Windows.Forms.Label();
			txVolume = new cYo.Common.Windows.Forms.TextBoxEx();
			cbAgeRating = new System.Windows.Forms.ComboBox();
			txEditor = new cYo.Common.Windows.Forms.TextBoxEx();
			labelEditor = new System.Windows.Forms.Label();
			txMonth = new cYo.Common.Windows.Forms.TextBoxEx();
			txYear = new cYo.Common.Windows.Forms.TextBoxEx();
			labelAgeRating = new System.Windows.Forms.Label();
			cbFormat = new cYo.Common.Windows.Forms.ComboBoxEx();
			txColorist = new cYo.Common.Windows.Forms.TextBoxEx();
			txSeries = new cYo.Common.Windows.Forms.TextBoxEx();
			labelFormat = new System.Windows.Forms.Label();
			labelAlternateSeries = new System.Windows.Forms.Label();
			txAlternateSeries = new cYo.Common.Windows.Forms.TextBoxEx();
			cbImprint = new System.Windows.Forms.ComboBox();
			cbBlackAndWhite = new System.Windows.Forms.ComboBox();
			labelVolume = new System.Windows.Forms.Label();
			txInker = new cYo.Common.Windows.Forms.TextBoxEx();
			cbManga = new System.Windows.Forms.ComboBox();
			labelYear = new System.Windows.Forms.Label();
			labelMonth = new System.Windows.Forms.Label();
			labelBlackAndWhite = new System.Windows.Forms.Label();
			txAlternateCount = new cYo.Common.Windows.Forms.TextBoxEx();
			labelManga = new System.Windows.Forms.Label();
			labelSeries = new System.Windows.Forms.Label();
			labelLanguage = new System.Windows.Forms.Label();
			labelImprint = new System.Windows.Forms.Label();
			labelGenre = new System.Windows.Forms.Label();
			labelColorist = new System.Windows.Forms.Label();
			txCount = new cYo.Common.Windows.Forms.TextBoxEx();
			cbLanguage = new cYo.Common.Windows.Forms.LanguageComboBox();
			cbPublisher = new System.Windows.Forms.ComboBox();
			txPenciller = new cYo.Common.Windows.Forms.TextBoxEx();
			txAlternateNumber = new cYo.Common.Windows.Forms.TextBoxEx();
			txNumber = new cYo.Common.Windows.Forms.TextBoxEx();
			labelPublisher = new System.Windows.Forms.Label();
			labelCoverArtist = new System.Windows.Forms.Label();
			txCoverArtist = new cYo.Common.Windows.Forms.TextBoxEx();
			labelInker = new System.Windows.Forms.Label();
			labelAlternateCount = new System.Windows.Forms.Label();
			txTitle = new cYo.Common.Windows.Forms.TextBoxEx();
			labelCount = new System.Windows.Forms.Label();
			labelAlternateNumber = new System.Windows.Forms.Label();
			labelNumber = new System.Windows.Forms.Label();
			labelLetterer = new System.Windows.Forms.Label();
			labelPenciller = new System.Windows.Forms.Label();
			txLetterer = new cYo.Common.Windows.Forms.TextBoxEx();
			labelTitle = new System.Windows.Forms.Label();
			labelWriter = new System.Windows.Forms.Label();
			txWriter = new cYo.Common.Windows.Forms.TextBoxEx();
			tabPlot = new System.Windows.Forms.TabPage();
			tabNotes = new System.Windows.Forms.TabControl();
			tabPageSummary = new System.Windows.Forms.TabPage();
			txSummary = new cYo.Common.Windows.Forms.TextBoxEx();
			tabPageNotes = new System.Windows.Forms.TabPage();
			txNotes = new cYo.Common.Windows.Forms.TextBoxEx();
			tabPageReview = new System.Windows.Forms.TabPage();
			txReview = new cYo.Common.Windows.Forms.TextBoxEx();
			txMainCharacterOrTeam = new cYo.Common.Windows.Forms.TextBoxEx();
			labelMainCharacterOrTeam = new System.Windows.Forms.Label();
			txScanInformation = new cYo.Common.Windows.Forms.TextBoxEx();
			labelScanInformation = new System.Windows.Forms.Label();
			txLocations = new cYo.Common.Windows.Forms.TextBoxEx();
			labelLocations = new System.Windows.Forms.Label();
			txTeams = new cYo.Common.Windows.Forms.TextBoxEx();
			labelTeams = new System.Windows.Forms.Label();
			txWeblink = new cYo.Common.Windows.Forms.TextBoxEx();
			labelWeb = new System.Windows.Forms.Label();
			txCharacters = new cYo.Common.Windows.Forms.TextBoxEx();
			labelCharacters = new System.Windows.Forms.Label();
			tabCatalog = new System.Windows.Forms.TabPage();
			labelReleasedTime = new System.Windows.Forms.Label();
			dtpReleasedTime = new cYo.Common.Windows.Forms.NullableDateTimePicker();
			labelOpenedTime = new System.Windows.Forms.Label();
			dtpOpenedTime = new cYo.Common.Windows.Forms.NullableDateTimePicker();
			labelAddedTime = new System.Windows.Forms.Label();
			dtpAddedTime = new cYo.Common.Windows.Forms.NullableDateTimePicker();
			txPagesAsTextSimple = new cYo.Common.Windows.Forms.TextBoxEx();
			labelPagesAsTextSimple = new System.Windows.Forms.Label();
			txISBN = new cYo.Common.Windows.Forms.TextBoxEx();
			labelISBN = new System.Windows.Forms.Label();
			cbBookLocation = new System.Windows.Forms.ComboBox();
			labelBookLocation = new System.Windows.Forms.Label();
			txCollectionStatus = new cYo.Common.Windows.Forms.TextBoxEx();
			cbBookPrice = new System.Windows.Forms.ComboBox();
			labelBookPrice = new System.Windows.Forms.Label();
			txBookNotes = new cYo.Common.Windows.Forms.TextBoxEx();
			labelBookNotes = new System.Windows.Forms.Label();
			cbBookAge = new System.Windows.Forms.ComboBox();
			labelBookAge = new System.Windows.Forms.Label();
			labelBookCollectionStatus = new System.Windows.Forms.Label();
			cbBookCondition = new System.Windows.Forms.ComboBox();
			labelBookCondition = new System.Windows.Forms.Label();
			cbBookStore = new cYo.Common.Windows.Forms.ComboBoxEx();
			labelBookStore = new System.Windows.Forms.Label();
			cbBookOwner = new System.Windows.Forms.ComboBox();
			labelBookOwner = new System.Windows.Forms.Label();
			tabCustom = new System.Windows.Forms.TabPage();
			customValuesData = new System.Windows.Forms.DataGridView();
			CustomValueName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			CustomValueValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			tabPages = new System.Windows.Forms.TabPage();
			btResetPages = new cYo.Common.Windows.Forms.SplitButton();
			cmResetPages = new System.Windows.Forms.ContextMenuStrip(components);
			miOrderByName = new System.Windows.Forms.ToolStripMenuItem();
			miOrderByNameNumeric = new System.Windows.Forms.ToolStripMenuItem();
			btPageView = new System.Windows.Forms.Button();
			labelPagesInfo = new System.Windows.Forms.Label();
			pagesView = new cYo.Projects.ComicRack.Viewer.Controls.PagesView();
			tabColors = new System.Windows.Forms.TabPage();
			panelImage = new System.Windows.Forms.Panel();
			labelCurrentPage = new System.Windows.Forms.Label();
			chkShowImageControls = new System.Windows.Forms.CheckBox();
			btLastPage = new System.Windows.Forms.Button();
			btFirstPage = new System.Windows.Forms.Button();
			btNextPage = new cYo.Common.Windows.Forms.AutoRepeatButton();
			btPrevPage = new cYo.Common.Windows.Forms.AutoRepeatButton();
			pageViewer = new cYo.Common.Windows.Forms.BitmapViewer();
			panelImageControls = new System.Windows.Forms.Panel();
			labelSaturation = new System.Windows.Forms.Label();
			labelContrast = new System.Windows.Forms.Label();
			tbGamma = new cYo.Common.Windows.Forms.TrackBarLite();
			tbSaturation = new cYo.Common.Windows.Forms.TrackBarLite();
			labelGamma = new System.Windows.Forms.Label();
			tbBrightness = new cYo.Common.Windows.Forms.TrackBarLite();
			tbSharpening = new cYo.Common.Windows.Forms.TrackBarLite();
			tbContrast = new cYo.Common.Windows.Forms.TrackBarLite();
			labelSharpening = new System.Windows.Forms.Label();
			labelBrightness = new System.Windows.Forms.Label();
			btResetColors = new System.Windows.Forms.Button();
			btPrev = new System.Windows.Forms.Button();
			btNext = new System.Windows.Forms.Button();
			btScript = new cYo.Common.Windows.Forms.SplitButton();
			toolTip = new System.Windows.Forms.ToolTip(components);
			btApply = new System.Windows.Forms.Button();
			tabControl.SuspendLayout();
			tabSummary.SuspendLayout();
			cmThumbnail.SuspendLayout();
			tabDetails.SuspendLayout();
			tabPlot.SuspendLayout();
			tabNotes.SuspendLayout();
			tabPageSummary.SuspendLayout();
			tabPageNotes.SuspendLayout();
			tabPageReview.SuspendLayout();
			tabCatalog.SuspendLayout();
			tabCustom.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)customValuesData).BeginInit();
			tabPages.SuspendLayout();
			cmResetPages.SuspendLayout();
			tabColors.SuspendLayout();
			panelImage.SuspendLayout();
			panelImageControls.SuspendLayout();
			SuspendLayout();
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(415, 483);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 5;
			btCancel.Text = "&Cancel";
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(329, 483);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(80, 24);
			btOK.TabIndex = 4;
			btOK.Text = "&OK";
			btOK.Click += new System.EventHandler(btOK_Click);
			tabControl.Controls.Add(tabSummary);
			tabControl.Controls.Add(tabDetails);
			tabControl.Controls.Add(tabPlot);
			tabControl.Controls.Add(tabCatalog);
			tabControl.Controls.Add(tabCustom);
			tabControl.Controls.Add(tabPages);
			tabControl.Controls.Add(tabColors);
			tabControl.Location = new System.Drawing.Point(8, 9);
			tabControl.Name = "tabControl";
			tabControl.SelectedIndex = 0;
			tabControl.Size = new System.Drawing.Size(574, 468);
			tabControl.TabIndex = 0;
			tabSummary.Controls.Add(btThumbnail);
			tabSummary.Controls.Add(labelCommunityRating);
			tabSummary.Controls.Add(txCommunityRating);
			tabSummary.Controls.Add(labelMyRating);
			tabSummary.Controls.Add(btLinkFile);
			tabSummary.Controls.Add(linkLabel);
			tabSummary.Controls.Add(coverThumbnail);
			tabSummary.Controls.Add(whereSeparator);
			tabSummary.Controls.Add(panel1);
			tabSummary.Controls.Add(labelWhere);
			tabSummary.Controls.Add(labelType);
			tabSummary.Controls.Add(txRating);
			tabSummary.Controls.Add(labelPages);
			tabSummary.Controls.Add(lblType);
			tabSummary.Controls.Add(lblPath);
			tabSummary.Controls.Add(lblPages);
			tabSummary.Location = new System.Drawing.Point(4, 22);
			tabSummary.Name = "tabSummary";
			tabSummary.Padding = new System.Windows.Forms.Padding(3);
			tabSummary.Size = new System.Drawing.Size(566, 442);
			tabSummary.TabIndex = 0;
			tabSummary.Text = "Summary";
			tabSummary.UseVisualStyleBackColor = true;
			btThumbnail.ContextMenuStrip = cmThumbnail;
			btThumbnail.Location = new System.Drawing.Point(372, 347);
			btThumbnail.Name = "btThumbnail";
			btThumbnail.Size = new System.Drawing.Size(152, 23);
			btThumbnail.TabIndex = 13;
			btThumbnail.Text = "Thumbnail...";
			btThumbnail.UseVisualStyleBackColor = true;
			btThumbnail.Visible = false;
			btThumbnail.ShowContextMenu += new System.EventHandler(btThumbnail_ShowContextMenu);
			btThumbnail.Click += new System.EventHandler(btThumbnail_Click);
			cmThumbnail.Items.AddRange(new System.Windows.Forms.ToolStripItem[2]
			{
				miResetThumbnail,
				toolStripMenuItem1
			});
			cmThumbnail.Name = "cmKeyboardLayout";
			cmThumbnail.Size = new System.Drawing.Size(103, 32);
			miResetThumbnail.Name = "miResetThumbnail";
			miResetThumbnail.Size = new System.Drawing.Size(102, 22);
			miResetThumbnail.Text = "&Reset";
			miResetThumbnail.Click += new System.EventHandler(miResetThumbnail_Click);
			toolStripMenuItem1.Name = "toolStripMenuItem1";
			toolStripMenuItem1.Size = new System.Drawing.Size(99, 6);
			labelCommunityRating.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelCommunityRating.Location = new System.Drawing.Point(217, 313);
			labelCommunityRating.Name = "labelCommunityRating";
			labelCommunityRating.Size = new System.Drawing.Size(149, 20);
			labelCommunityRating.TabIndex = 7;
			labelCommunityRating.Text = "Community Rating:";
			labelCommunityRating.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			txCommunityRating.DrawText = true;
			txCommunityRating.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
			txCommunityRating.ForeColor = System.Drawing.SystemColors.GrayText;
			txCommunityRating.Location = new System.Drawing.Point(372, 312);
			txCommunityRating.Name = "txCommunityRating";
			txCommunityRating.Rating = 3f;
			txCommunityRating.RatingImage = cYo.Projects.ComicRack.Viewer.Properties.Resources.StarBlue;
			txCommunityRating.Size = new System.Drawing.Size(152, 21);
			txCommunityRating.TabIndex = 8;
			txCommunityRating.Text = "3";
			labelMyRating.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelMyRating.Location = new System.Drawing.Point(219, 285);
			labelMyRating.Name = "labelMyRating";
			labelMyRating.Size = new System.Drawing.Size(147, 20);
			labelMyRating.TabIndex = 5;
			labelMyRating.Text = "My Rating:";
			labelMyRating.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			btLinkFile.Location = new System.Drawing.Point(372, 373);
			btLinkFile.Name = "btLinkFile";
			btLinkFile.Size = new System.Drawing.Size(152, 23);
			btLinkFile.TabIndex = 14;
			btLinkFile.Text = "Link to File...";
			btLinkFile.UseVisualStyleBackColor = true;
			btLinkFile.Visible = false;
			btLinkFile.Click += new System.EventHandler(btLinkFile_Click);
			linkLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			linkLabel.LinkColor = System.Drawing.Color.SteelBlue;
			linkLabel.Location = new System.Drawing.Point(3, 416);
			linkLabel.Name = "linkLabel";
			linkLabel.Size = new System.Drawing.Size(560, 23);
			linkLabel.TabIndex = 12;
			linkLabel.TabStop = true;
			linkLabel.Text = "linkLabel";
			linkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			linkLabel.VisitedLinkColor = System.Drawing.Color.MediumOrchid;
			linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel_LinkClicked);
			coverThumbnail.AllowDrop = true;
			coverThumbnail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			coverThumbnail.Location = new System.Drawing.Point(21, 19);
			coverThumbnail.Name = "coverThumbnail";
			coverThumbnail.Size = new System.Drawing.Size(520, 243);
			coverThumbnail.TabIndex = 0;
			coverThumbnail.ThreeD = true;
			coverThumbnail.Tile = true;
			coverThumbnail.Click += new System.EventHandler(coverThumbnail_Click);
			coverThumbnail.DragDrop += new System.Windows.Forms.DragEventHandler(coverThumbnail_DragDrop);
			coverThumbnail.DragOver += new System.Windows.Forms.DragEventHandler(coverThumbnail_DragOver);
			whereSeparator.BackColor = System.Drawing.SystemColors.ButtonShadow;
			whereSeparator.Location = new System.Drawing.Point(19, 360);
			whereSeparator.Name = "whereSeparator";
			whereSeparator.Size = new System.Drawing.Size(520, 1);
			whereSeparator.TabIndex = 9;
			panel1.BackColor = System.Drawing.SystemColors.ButtonShadow;
			panel1.Location = new System.Drawing.Point(23, 268);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(520, 1);
			panel1.TabIndex = 0;
			labelWhere.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelWhere.Location = new System.Drawing.Point(19, 373);
			labelWhere.Name = "labelWhere";
			labelWhere.Size = new System.Drawing.Size(68, 17);
			labelWhere.TabIndex = 10;
			labelWhere.Text = "Where:";
			labelWhere.TextAlign = System.Drawing.ContentAlignment.TopRight;
			labelType.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelType.Location = new System.Drawing.Point(19, 285);
			labelType.Name = "labelType";
			labelType.Size = new System.Drawing.Size(68, 20);
			labelType.TabIndex = 1;
			labelType.Text = "Type:";
			labelType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			txRating.DrawText = true;
			txRating.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
			txRating.ForeColor = System.Drawing.SystemColors.GrayText;
			txRating.Location = new System.Drawing.Point(372, 285);
			txRating.Name = "txRating";
			txRating.Rating = 3f;
			txRating.RatingImage = cYo.Projects.ComicRack.Viewer.Properties.Resources.StarYellow;
			txRating.Size = new System.Drawing.Size(152, 21);
			txRating.TabIndex = 6;
			txRating.Text = "3";
			labelPages.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelPages.Location = new System.Drawing.Point(21, 313);
			labelPages.Name = "labelPages";
			labelPages.Size = new System.Drawing.Size(66, 20);
			labelPages.TabIndex = 3;
			labelPages.Text = "Pages:";
			labelPages.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			lblType.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lblType.Location = new System.Drawing.Point(93, 285);
			lblType.Name = "lblType";
			lblType.Size = new System.Drawing.Size(145, 20);
			lblType.TabIndex = 2;
			lblType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			lblPath.AutoEllipsis = true;
			lblPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lblPath.Location = new System.Drawing.Point(93, 373);
			lblPath.Name = "lblPath";
			lblPath.Size = new System.Drawing.Size(431, 35);
			lblPath.TabIndex = 11;
			lblPath.UseMnemonic = false;
			lblPages.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lblPages.Location = new System.Drawing.Point(93, 313);
			lblPages.Name = "lblPages";
			lblPages.Size = new System.Drawing.Size(105, 20);
			lblPages.TabIndex = 4;
			lblPages.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			tabDetails.Controls.Add(txDay);
			tabDetails.Controls.Add(labelDay);
			tabDetails.Controls.Add(labelSeriesGroup);
			tabDetails.Controls.Add(txSeriesGroup);
			tabDetails.Controls.Add(labelStoryArc);
			tabDetails.Controls.Add(txStoryArc);
			tabDetails.Controls.Add(cbSeriesComplete);
			tabDetails.Controls.Add(labelSeriesComplete);
			tabDetails.Controls.Add(cbEnableDynamicUpdate);
			tabDetails.Controls.Add(labelEnableDynamicUpdate);
			tabDetails.Controls.Add(txGenre);
			tabDetails.Controls.Add(labelTags);
			tabDetails.Controls.Add(cbEnableProposed);
			tabDetails.Controls.Add(txTags);
			tabDetails.Controls.Add(labelEnableProposed);
			tabDetails.Controls.Add(txVolume);
			tabDetails.Controls.Add(cbAgeRating);
			tabDetails.Controls.Add(txEditor);
			tabDetails.Controls.Add(labelEditor);
			tabDetails.Controls.Add(txMonth);
			tabDetails.Controls.Add(txYear);
			tabDetails.Controls.Add(labelAgeRating);
			tabDetails.Controls.Add(cbFormat);
			tabDetails.Controls.Add(txColorist);
			tabDetails.Controls.Add(txSeries);
			tabDetails.Controls.Add(labelFormat);
			tabDetails.Controls.Add(labelAlternateSeries);
			tabDetails.Controls.Add(txAlternateSeries);
			tabDetails.Controls.Add(cbImprint);
			tabDetails.Controls.Add(cbBlackAndWhite);
			tabDetails.Controls.Add(labelVolume);
			tabDetails.Controls.Add(txInker);
			tabDetails.Controls.Add(cbManga);
			tabDetails.Controls.Add(labelYear);
			tabDetails.Controls.Add(labelMonth);
			tabDetails.Controls.Add(labelBlackAndWhite);
			tabDetails.Controls.Add(txAlternateCount);
			tabDetails.Controls.Add(labelManga);
			tabDetails.Controls.Add(labelSeries);
			tabDetails.Controls.Add(labelLanguage);
			tabDetails.Controls.Add(labelImprint);
			tabDetails.Controls.Add(labelGenre);
			tabDetails.Controls.Add(labelColorist);
			tabDetails.Controls.Add(txCount);
			tabDetails.Controls.Add(cbLanguage);
			tabDetails.Controls.Add(cbPublisher);
			tabDetails.Controls.Add(txPenciller);
			tabDetails.Controls.Add(txAlternateNumber);
			tabDetails.Controls.Add(txNumber);
			tabDetails.Controls.Add(labelPublisher);
			tabDetails.Controls.Add(labelCoverArtist);
			tabDetails.Controls.Add(txCoverArtist);
			tabDetails.Controls.Add(labelInker);
			tabDetails.Controls.Add(labelAlternateCount);
			tabDetails.Controls.Add(txTitle);
			tabDetails.Controls.Add(labelCount);
			tabDetails.Controls.Add(labelAlternateNumber);
			tabDetails.Controls.Add(labelNumber);
			tabDetails.Controls.Add(labelLetterer);
			tabDetails.Controls.Add(labelPenciller);
			tabDetails.Controls.Add(txLetterer);
			tabDetails.Controls.Add(labelTitle);
			tabDetails.Controls.Add(labelWriter);
			tabDetails.Controls.Add(txWriter);
			tabDetails.Location = new System.Drawing.Point(4, 22);
			tabDetails.Name = "tabDetails";
			tabDetails.Padding = new System.Windows.Forms.Padding(3);
			tabDetails.Size = new System.Drawing.Size(566, 442);
			tabDetails.TabIndex = 1;
			tabDetails.Text = "Details";
			tabDetails.UseVisualStyleBackColor = true;
			txDay.Location = new System.Drawing.Point(349, 65);
			txDay.MaxLength = 4;
			txDay.Name = "txDay";
			txDay.PromptText = "";
			txDay.Size = new System.Drawing.Size(55, 20);
			txDay.TabIndex = 17;
			labelDay.AutoSize = true;
			labelDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelDay.Location = new System.Drawing.Point(349, 51);
			labelDay.Name = "labelDay";
			labelDay.Size = new System.Drawing.Size(29, 12);
			labelDay.TabIndex = 16;
			labelDay.Text = "Day:";
			labelSeriesGroup.AutoSize = true;
			labelSeriesGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelSeriesGroup.Location = new System.Drawing.Point(214, 129);
			labelSeriesGroup.Name = "labelSeriesGroup";
			labelSeriesGroup.Size = new System.Drawing.Size(74, 12);
			labelSeriesGroup.TabIndex = 30;
			labelSeriesGroup.Text = "Series Group:";
			txSeriesGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txSeriesGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txSeriesGroup.Location = new System.Drawing.Point(216, 142);
			txSeriesGroup.Name = "txSeriesGroup";
			txSeriesGroup.PromptText = "";
			txSeriesGroup.Size = new System.Drawing.Size(188, 20);
			txSeriesGroup.TabIndex = 31;
			txSeriesGroup.Tag = "SeriesGroup";
			labelStoryArc.AutoSize = true;
			labelStoryArc.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelStoryArc.Location = new System.Drawing.Point(8, 129);
			labelStoryArc.Name = "labelStoryArc";
			labelStoryArc.Size = new System.Drawing.Size(57, 12);
			labelStoryArc.TabIndex = 28;
			labelStoryArc.Text = "Story Arc:";
			txStoryArc.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txStoryArc.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txStoryArc.Location = new System.Drawing.Point(10, 142);
			txStoryArc.Name = "txStoryArc";
			txStoryArc.PromptText = "";
			txStoryArc.Size = new System.Drawing.Size(200, 20);
			txStoryArc.TabIndex = 29;
			txStoryArc.Tag = "StoryArc";
			cbSeriesComplete.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbSeriesComplete.FormattingEnabled = true;
			cbSeriesComplete.Location = new System.Drawing.Point(416, 141);
			cbSeriesComplete.Name = "cbSeriesComplete";
			cbSeriesComplete.Size = new System.Drawing.Size(139, 21);
			cbSeriesComplete.TabIndex = 33;
			cbSeriesComplete.TextChanged += new System.EventHandler(IconTextsChanged);
			labelSeriesComplete.AutoSize = true;
			labelSeriesComplete.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelSeriesComplete.Location = new System.Drawing.Point(414, 129);
			labelSeriesComplete.Name = "labelSeriesComplete";
			labelSeriesComplete.Size = new System.Drawing.Size(90, 12);
			labelSeriesComplete.TabIndex = 32;
			labelSeriesComplete.Text = "Series complete:";
			cbEnableDynamicUpdate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbEnableDynamicUpdate.FormattingEnabled = true;
			cbEnableDynamicUpdate.Location = new System.Drawing.Point(416, 371);
			cbEnableDynamicUpdate.Name = "cbEnableDynamicUpdate";
			cbEnableDynamicUpdate.Size = new System.Drawing.Size(139, 21);
			cbEnableDynamicUpdate.TabIndex = 59;
			labelEnableDynamicUpdate.AutoSize = true;
			labelEnableDynamicUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelEnableDynamicUpdate.Location = new System.Drawing.Point(414, 357);
			labelEnableDynamicUpdate.Name = "labelEnableDynamicUpdate";
			labelEnableDynamicUpdate.Size = new System.Drawing.Size(103, 12);
			labelEnableDynamicUpdate.TabIndex = 58;
			labelEnableDynamicUpdate.Text = "Include in Updates:";
			txGenre.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
			txGenre.Location = new System.Drawing.Point(10, 372);
			txGenre.Name = "txGenre";
			txGenre.Size = new System.Drawing.Size(392, 20);
			txGenre.TabIndex = 57;
			labelTags.AutoSize = true;
			labelTags.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelTags.Location = new System.Drawing.Point(11, 395);
			labelTags.Name = "labelTags";
			labelTags.Size = new System.Drawing.Size(33, 12);
			labelTags.TabIndex = 60;
			labelTags.Text = "Tags:";
			cbEnableProposed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbEnableProposed.FormattingEnabled = true;
			cbEnableProposed.Location = new System.Drawing.Point(416, 410);
			cbEnableProposed.Name = "cbEnableProposed";
			cbEnableProposed.Size = new System.Drawing.Size(139, 21);
			cbEnableProposed.TabIndex = 63;
			cbEnableProposed.SelectedIndexChanged += new System.EventHandler(cbEnableShadowValues_SelectedIndexChanged);
			txTags.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
			txTags.Location = new System.Drawing.Point(11, 411);
			txTags.Name = "txTags";
			txTags.Size = new System.Drawing.Size(392, 20);
			txTags.TabIndex = 61;
			txTags.TextChanged += new System.EventHandler(IconTextsChanged);
			labelEnableProposed.AutoSize = true;
			labelEnableProposed.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelEnableProposed.Location = new System.Drawing.Point(414, 395);
			labelEnableProposed.Name = "labelEnableProposed";
			labelEnableProposed.Size = new System.Drawing.Size(94, 12);
			labelEnableProposed.TabIndex = 62;
			labelEnableProposed.Text = "Proposed Values:";
			txVolume.Location = new System.Drawing.Point(216, 28);
			txVolume.Name = "txVolume";
			txVolume.PromptText = "";
			txVolume.Size = new System.Drawing.Size(57, 20);
			txVolume.TabIndex = 3;
			cbAgeRating.FormattingEnabled = true;
			cbAgeRating.Location = new System.Drawing.Point(416, 200);
			cbAgeRating.Name = "cbAgeRating";
			cbAgeRating.Size = new System.Drawing.Size(139, 21);
			cbAgeRating.TabIndex = 39;
			cbAgeRating.TextChanged += new System.EventHandler(IconTextsChanged);
			txEditor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txEditor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txEditor.Location = new System.Drawing.Point(11, 315);
			txEditor.Name = "txEditor";
			txEditor.Size = new System.Drawing.Size(200, 20);
			txEditor.TabIndex = 53;
			txEditor.Tag = "Editor";
			labelEditor.AutoSize = true;
			labelEditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelEditor.Location = new System.Drawing.Point(9, 300);
			labelEditor.Name = "labelEditor";
			labelEditor.Size = new System.Drawing.Size(39, 12);
			labelEditor.TabIndex = 52;
			labelEditor.Text = "Editor:";
			txMonth.Location = new System.Drawing.Point(278, 66);
			txMonth.MaxLength = 2;
			txMonth.Name = "txMonth";
			txMonth.PromptText = "";
			txMonth.Size = new System.Drawing.Size(65, 20);
			txMonth.TabIndex = 15;
			txYear.Location = new System.Drawing.Point(216, 66);
			txYear.MaxLength = 4;
			txYear.Name = "txYear";
			txYear.PromptText = "";
			txYear.Size = new System.Drawing.Size(57, 20);
			txYear.TabIndex = 13;
			txYear.TextChanged += new System.EventHandler(IconTextsChanged);
			labelAgeRating.AutoSize = true;
			labelAgeRating.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelAgeRating.Location = new System.Drawing.Point(414, 187);
			labelAgeRating.Name = "labelAgeRating";
			labelAgeRating.Size = new System.Drawing.Size(65, 12);
			labelAgeRating.TabIndex = 38;
			labelAgeRating.Text = "Age Rating:";
			cbFormat.FormattingEnabled = true;
			cbFormat.Location = new System.Drawing.Point(416, 27);
			cbFormat.Name = "cbFormat";
			cbFormat.PromptText = null;
			cbFormat.Size = new System.Drawing.Size(139, 21);
			cbFormat.TabIndex = 9;
			cbFormat.TextChanged += new System.EventHandler(IconTextsChanged);
			txColorist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txColorist.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txColorist.Location = new System.Drawing.Point(216, 239);
			txColorist.Name = "txColorist";
			txColorist.Size = new System.Drawing.Size(187, 20);
			txColorist.TabIndex = 43;
			txColorist.Tag = "Colorist";
			txSeries.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txSeries.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txSeries.Location = new System.Drawing.Point(11, 28);
			txSeries.Name = "txSeries";
			txSeries.PromptText = "";
			txSeries.Size = new System.Drawing.Size(201, 20);
			txSeries.TabIndex = 1;
			txSeries.Tag = "Series";
			labelFormat.AutoSize = true;
			labelFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelFormat.Location = new System.Drawing.Point(414, 13);
			labelFormat.Name = "labelFormat";
			labelFormat.Size = new System.Drawing.Size(45, 12);
			labelFormat.TabIndex = 8;
			labelFormat.Text = "Format:";
			labelAlternateSeries.AutoSize = true;
			labelAlternateSeries.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelAlternateSeries.Location = new System.Drawing.Point(9, 89);
			labelAlternateSeries.Name = "labelAlternateSeries";
			labelAlternateSeries.Size = new System.Drawing.Size(177, 12);
			labelAlternateSeries.TabIndex = 20;
			labelAlternateSeries.Text = "Alternate Series or Storyline Title:";
			txAlternateSeries.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txAlternateSeries.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txAlternateSeries.Location = new System.Drawing.Point(11, 104);
			txAlternateSeries.Name = "txAlternateSeries";
			txAlternateSeries.PromptText = "";
			txAlternateSeries.Size = new System.Drawing.Size(262, 20);
			txAlternateSeries.TabIndex = 21;
			txAlternateSeries.Tag = "AlternateSeries";
			cbImprint.FormattingEnabled = true;
			cbImprint.Location = new System.Drawing.Point(416, 103);
			cbImprint.Name = "cbImprint";
			cbImprint.Size = new System.Drawing.Size(139, 21);
			cbImprint.TabIndex = 27;
			cbImprint.Tag = "Imprint";
			cbImprint.TextChanged += new System.EventHandler(IconTextsChanged);
			cbBlackAndWhite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbBlackAndWhite.FormattingEnabled = true;
			cbBlackAndWhite.Location = new System.Drawing.Point(416, 312);
			cbBlackAndWhite.Name = "cbBlackAndWhite";
			cbBlackAndWhite.Size = new System.Drawing.Size(139, 21);
			cbBlackAndWhite.TabIndex = 55;
			cbBlackAndWhite.TextChanged += new System.EventHandler(IconTextsChanged);
			labelVolume.AutoSize = true;
			labelVolume.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelVolume.Location = new System.Drawing.Point(216, 13);
			labelVolume.Name = "labelVolume";
			labelVolume.Size = new System.Drawing.Size(47, 12);
			labelVolume.TabIndex = 2;
			labelVolume.Text = "Volume:";
			txInker.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txInker.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txInker.Location = new System.Drawing.Point(10, 239);
			txInker.Name = "txInker";
			txInker.Size = new System.Drawing.Size(201, 20);
			txInker.TabIndex = 41;
			txInker.Tag = "Inker";
			cbManga.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbManga.FormattingEnabled = true;
			cbManga.Location = new System.Drawing.Point(416, 237);
			cbManga.Name = "cbManga";
			cbManga.Size = new System.Drawing.Size(139, 21);
			cbManga.TabIndex = 45;
			cbManga.TextChanged += new System.EventHandler(IconTextsChanged);
			labelYear.AutoSize = true;
			labelYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelYear.Location = new System.Drawing.Point(216, 52);
			labelYear.Name = "labelYear";
			labelYear.Size = new System.Drawing.Size(31, 12);
			labelYear.TabIndex = 12;
			labelYear.Text = "Year:";
			labelMonth.AutoSize = true;
			labelMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelMonth.Location = new System.Drawing.Point(276, 51);
			labelMonth.Name = "labelMonth";
			labelMonth.Size = new System.Drawing.Size(41, 12);
			labelMonth.TabIndex = 14;
			labelMonth.Text = "Month:";
			labelBlackAndWhite.AutoSize = true;
			labelBlackAndWhite.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelBlackAndWhite.Location = new System.Drawing.Point(414, 297);
			labelBlackAndWhite.Name = "labelBlackAndWhite";
			labelBlackAndWhite.Size = new System.Drawing.Size(90, 12);
			labelBlackAndWhite.TabIndex = 54;
			labelBlackAndWhite.Text = "Black and White:";
			txAlternateCount.Location = new System.Drawing.Point(349, 104);
			txAlternateCount.Name = "txAlternateCount";
			txAlternateCount.PromptText = "";
			txAlternateCount.Size = new System.Drawing.Size(55, 20);
			txAlternateCount.TabIndex = 25;
			labelManga.AutoSize = true;
			labelManga.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelManga.Location = new System.Drawing.Point(414, 222);
			labelManga.Name = "labelManga";
			labelManga.Size = new System.Drawing.Size(43, 12);
			labelManga.TabIndex = 44;
			labelManga.Text = "Manga:";
			labelSeries.AutoSize = true;
			labelSeries.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelSeries.Location = new System.Drawing.Point(9, 13);
			labelSeries.Name = "labelSeries";
			labelSeries.Size = new System.Drawing.Size(41, 12);
			labelSeries.TabIndex = 0;
			labelSeries.Text = "Series:";
			labelLanguage.AutoSize = true;
			labelLanguage.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelLanguage.Location = new System.Drawing.Point(414, 261);
			labelLanguage.Name = "labelLanguage";
			labelLanguage.Size = new System.Drawing.Size(57, 12);
			labelLanguage.TabIndex = 50;
			labelLanguage.Text = "Language:";
			labelImprint.AutoSize = true;
			labelImprint.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelImprint.Location = new System.Drawing.Point(414, 88);
			labelImprint.Name = "labelImprint";
			labelImprint.Size = new System.Drawing.Size(45, 12);
			labelImprint.TabIndex = 26;
			labelImprint.Text = "Imprint:";
			labelGenre.AutoSize = true;
			labelGenre.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelGenre.Location = new System.Drawing.Point(11, 357);
			labelGenre.Name = "labelGenre";
			labelGenre.Size = new System.Drawing.Size(39, 12);
			labelGenre.TabIndex = 56;
			labelGenre.Text = "Genre:";
			labelColorist.AutoSize = true;
			labelColorist.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelColorist.Location = new System.Drawing.Point(214, 224);
			labelColorist.Name = "labelColorist";
			labelColorist.Size = new System.Drawing.Size(49, 12);
			labelColorist.TabIndex = 42;
			labelColorist.Text = "Colorist:";
			txCount.Location = new System.Drawing.Point(349, 28);
			txCount.Name = "txCount";
			txCount.PromptText = "";
			txCount.Size = new System.Drawing.Size(55, 20);
			txCount.TabIndex = 7;
			cbLanguage.CultureTypes = System.Globalization.CultureTypes.NeutralCultures;
			cbLanguage.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			cbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbLanguage.FormattingEnabled = true;
			cbLanguage.IntegralHeight = false;
			cbLanguage.Location = new System.Drawing.Point(416, 276);
			cbLanguage.Name = "cbLanguage";
			cbLanguage.SelectedCulture = "";
			cbLanguage.SelectedTwoLetterISOLanguage = "";
			cbLanguage.Size = new System.Drawing.Size(139, 21);
			cbLanguage.TabIndex = 51;
			cbLanguage.TopTwoLetterISOLanguages = null;
			cbPublisher.FormattingEnabled = true;
			cbPublisher.Location = new System.Drawing.Point(416, 65);
			cbPublisher.Name = "cbPublisher";
			cbPublisher.Size = new System.Drawing.Size(139, 21);
			cbPublisher.TabIndex = 19;
			cbPublisher.Tag = "Publisher";
			cbPublisher.TextChanged += new System.EventHandler(IconTextsChanged);
			txPenciller.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txPenciller.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txPenciller.Location = new System.Drawing.Point(216, 200);
			txPenciller.Name = "txPenciller";
			txPenciller.Size = new System.Drawing.Size(187, 20);
			txPenciller.TabIndex = 37;
			txPenciller.Tag = "Penciller";
			txAlternateNumber.Location = new System.Drawing.Point(278, 104);
			txAlternateNumber.Name = "txAlternateNumber";
			txAlternateNumber.PromptText = "";
			txAlternateNumber.Size = new System.Drawing.Size(65, 20);
			txAlternateNumber.TabIndex = 23;
			txNumber.Location = new System.Drawing.Point(278, 28);
			txNumber.Name = "txNumber";
			txNumber.PromptText = "";
			txNumber.Size = new System.Drawing.Size(65, 20);
			txNumber.TabIndex = 5;
			labelPublisher.AutoSize = true;
			labelPublisher.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelPublisher.Location = new System.Drawing.Point(414, 51);
			labelPublisher.Name = "labelPublisher";
			labelPublisher.Size = new System.Drawing.Size(56, 12);
			labelPublisher.TabIndex = 18;
			labelPublisher.Text = "Publisher:";
			labelCoverArtist.AutoSize = true;
			labelCoverArtist.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelCoverArtist.Location = new System.Drawing.Point(214, 262);
			labelCoverArtist.Name = "labelCoverArtist";
			labelCoverArtist.Size = new System.Drawing.Size(72, 12);
			labelCoverArtist.TabIndex = 48;
			labelCoverArtist.Text = "Cover Artist:";
			txCoverArtist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txCoverArtist.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txCoverArtist.Location = new System.Drawing.Point(216, 277);
			txCoverArtist.Name = "txCoverArtist";
			txCoverArtist.Size = new System.Drawing.Size(187, 20);
			txCoverArtist.TabIndex = 49;
			txCoverArtist.Tag = "CoverArtist";
			labelInker.AutoSize = true;
			labelInker.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelInker.Location = new System.Drawing.Point(11, 224);
			labelInker.Name = "labelInker";
			labelInker.Size = new System.Drawing.Size(35, 12);
			labelInker.TabIndex = 40;
			labelInker.Text = "Inker:";
			labelAlternateCount.AutoSize = true;
			labelAlternateCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelAlternateCount.Location = new System.Drawing.Point(347, 89);
			labelAlternateCount.Name = "labelAlternateCount";
			labelAlternateCount.Size = new System.Drawing.Size(19, 12);
			labelAlternateCount.TabIndex = 24;
			labelAlternateCount.Text = "of:";
			txTitle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txTitle.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txTitle.Location = new System.Drawing.Point(11, 66);
			txTitle.Name = "txTitle";
			txTitle.Size = new System.Drawing.Size(201, 20);
			txTitle.TabIndex = 11;
			txTitle.Tag = "Title";
			labelCount.AutoSize = true;
			labelCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelCount.Location = new System.Drawing.Point(347, 13);
			labelCount.Name = "labelCount";
			labelCount.Size = new System.Drawing.Size(19, 12);
			labelCount.TabIndex = 6;
			labelCount.Text = "of:";
			labelAlternateNumber.AutoSize = true;
			labelAlternateNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelAlternateNumber.Location = new System.Drawing.Point(276, 89);
			labelAlternateNumber.Name = "labelAlternateNumber";
			labelAlternateNumber.Size = new System.Drawing.Size(48, 12);
			labelAlternateNumber.TabIndex = 22;
			labelAlternateNumber.Text = "Number:";
			labelNumber.AutoSize = true;
			labelNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelNumber.Location = new System.Drawing.Point(276, 13);
			labelNumber.Name = "labelNumber";
			labelNumber.Size = new System.Drawing.Size(48, 12);
			labelNumber.TabIndex = 4;
			labelNumber.Text = "Number:";
			labelLetterer.AutoSize = true;
			labelLetterer.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelLetterer.Location = new System.Drawing.Point(9, 262);
			labelLetterer.Name = "labelLetterer";
			labelLetterer.Size = new System.Drawing.Size(49, 12);
			labelLetterer.TabIndex = 46;
			labelLetterer.Text = "Letterer:";
			labelPenciller.AutoSize = true;
			labelPenciller.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelPenciller.Location = new System.Drawing.Point(214, 187);
			labelPenciller.Name = "labelPenciller";
			labelPenciller.Size = new System.Drawing.Size(53, 12);
			labelPenciller.TabIndex = 36;
			labelPenciller.Text = "Penciller:";
			txLetterer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txLetterer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txLetterer.Location = new System.Drawing.Point(11, 277);
			txLetterer.Name = "txLetterer";
			txLetterer.Size = new System.Drawing.Size(200, 20);
			txLetterer.TabIndex = 47;
			txLetterer.Tag = "Letterer";
			labelTitle.AutoSize = true;
			labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelTitle.Location = new System.Drawing.Point(9, 51);
			labelTitle.Name = "labelTitle";
			labelTitle.Size = new System.Drawing.Size(31, 12);
			labelTitle.TabIndex = 10;
			labelTitle.Text = "Title:";
			labelWriter.AutoSize = true;
			labelWriter.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelWriter.Location = new System.Drawing.Point(9, 187);
			labelWriter.Name = "labelWriter";
			labelWriter.Size = new System.Drawing.Size(40, 12);
			labelWriter.TabIndex = 34;
			labelWriter.Text = "Writer:";
			txWriter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txWriter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txWriter.Location = new System.Drawing.Point(11, 200);
			txWriter.Name = "txWriter";
			txWriter.Size = new System.Drawing.Size(200, 20);
			txWriter.TabIndex = 35;
			txWriter.Tag = "Writer";
			tabPlot.Controls.Add(tabNotes);
			tabPlot.Controls.Add(txMainCharacterOrTeam);
			tabPlot.Controls.Add(labelMainCharacterOrTeam);
			tabPlot.Controls.Add(txScanInformation);
			tabPlot.Controls.Add(labelScanInformation);
			tabPlot.Controls.Add(txLocations);
			tabPlot.Controls.Add(labelLocations);
			tabPlot.Controls.Add(txTeams);
			tabPlot.Controls.Add(labelTeams);
			tabPlot.Controls.Add(txWeblink);
			tabPlot.Controls.Add(labelWeb);
			tabPlot.Controls.Add(txCharacters);
			tabPlot.Controls.Add(labelCharacters);
			tabPlot.Location = new System.Drawing.Point(4, 22);
			tabPlot.Name = "tabPlot";
			tabPlot.Size = new System.Drawing.Size(566, 442);
			tabPlot.TabIndex = 8;
			tabPlot.Text = "Plot & Notes";
			tabPlot.UseVisualStyleBackColor = true;
			tabNotes.Controls.Add(tabPageSummary);
			tabNotes.Controls.Add(tabPageNotes);
			tabNotes.Controls.Add(tabPageReview);
			tabNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold);
			tabNotes.Location = new System.Drawing.Point(11, 15);
			tabNotes.Multiline = true;
			tabNotes.Name = "tabNotes";
			tabNotes.SelectedIndex = 0;
			tabNotes.Size = new System.Drawing.Size(539, 233);
			tabNotes.TabIndex = 0;
			tabPageSummary.Controls.Add(txSummary);
			tabPageSummary.Location = new System.Drawing.Point(4, 21);
			tabPageSummary.Name = "tabPageSummary";
			tabPageSummary.Padding = new System.Windows.Forms.Padding(3);
			tabPageSummary.Size = new System.Drawing.Size(531, 208);
			tabPageSummary.TabIndex = 0;
			tabPageSummary.Text = "Summary";
			tabPageSummary.UseVisualStyleBackColor = true;
			txSummary.AcceptsReturn = true;
			txSummary.Dock = System.Windows.Forms.DockStyle.Fill;
			txSummary.FocusSelect = false;
			txSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
			txSummary.Location = new System.Drawing.Point(3, 3);
			txSummary.Multiline = true;
			txSummary.Name = "txSummary";
			txSummary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			txSummary.Size = new System.Drawing.Size(525, 202);
			txSummary.TabIndex = 2;
			tabPageNotes.Controls.Add(txNotes);
			tabPageNotes.Location = new System.Drawing.Point(4, 21);
			tabPageNotes.Name = "tabPageNotes";
			tabPageNotes.Padding = new System.Windows.Forms.Padding(3);
			tabPageNotes.Size = new System.Drawing.Size(521, 208);
			tabPageNotes.TabIndex = 1;
			tabPageNotes.Text = "Notes";
			tabPageNotes.UseVisualStyleBackColor = true;
			txNotes.AcceptsReturn = true;
			txNotes.Dock = System.Windows.Forms.DockStyle.Fill;
			txNotes.FocusSelect = false;
			txNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
			txNotes.Location = new System.Drawing.Point(3, 3);
			txNotes.Multiline = true;
			txNotes.Name = "txNotes";
			txNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			txNotes.Size = new System.Drawing.Size(515, 202);
			txNotes.TabIndex = 10;
			tabPageReview.Controls.Add(txReview);
			tabPageReview.Location = new System.Drawing.Point(4, 21);
			tabPageReview.Name = "tabPageReview";
			tabPageReview.Padding = new System.Windows.Forms.Padding(3);
			tabPageReview.Size = new System.Drawing.Size(521, 208);
			tabPageReview.TabIndex = 2;
			tabPageReview.Text = "Review";
			tabPageReview.UseVisualStyleBackColor = true;
			txReview.AcceptsReturn = true;
			txReview.Dock = System.Windows.Forms.DockStyle.Fill;
			txReview.FocusSelect = false;
			txReview.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
			txReview.Location = new System.Drawing.Point(3, 3);
			txReview.Multiline = true;
			txReview.Name = "txReview";
			txReview.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			txReview.Size = new System.Drawing.Size(515, 202);
			txReview.TabIndex = 10;
			txMainCharacterOrTeam.AcceptsReturn = true;
			txMainCharacterOrTeam.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
			txMainCharacterOrTeam.Location = new System.Drawing.Point(286, 278);
			txMainCharacterOrTeam.Name = "txMainCharacterOrTeam";
			txMainCharacterOrTeam.Size = new System.Drawing.Size(264, 20);
			txMainCharacterOrTeam.TabIndex = 4;
			txMainCharacterOrTeam.Tag = "Teams";
			labelMainCharacterOrTeam.AutoSize = true;
			labelMainCharacterOrTeam.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelMainCharacterOrTeam.Location = new System.Drawing.Point(284, 263);
			labelMainCharacterOrTeam.Name = "labelMainCharacterOrTeam";
			labelMainCharacterOrTeam.Size = new System.Drawing.Size(130, 12);
			labelMainCharacterOrTeam.TabIndex = 3;
			labelMainCharacterOrTeam.Text = "Main Character or Team:";
			txScanInformation.AcceptsReturn = true;
			txScanInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
			txScanInformation.Location = new System.Drawing.Point(11, 412);
			txScanInformation.Name = "txScanInformation";
			txScanInformation.Size = new System.Drawing.Size(260, 20);
			txScanInformation.TabIndex = 10;
			txScanInformation.Tag = "";
			labelScanInformation.AutoSize = true;
			labelScanInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelScanInformation.Location = new System.Drawing.Point(9, 397);
			labelScanInformation.Name = "labelScanInformation";
			labelScanInformation.Size = new System.Drawing.Size(95, 12);
			labelScanInformation.TabIndex = 9;
			labelScanInformation.Text = "Scan Information:";
			txLocations.AcceptsReturn = true;
			txLocations.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
			txLocations.Location = new System.Drawing.Point(287, 351);
			txLocations.Name = "txLocations";
			txLocations.Size = new System.Drawing.Size(263, 20);
			txLocations.TabIndex = 8;
			txLocations.Tag = "Locations";
			labelLocations.AutoSize = true;
			labelLocations.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelLocations.Location = new System.Drawing.Point(285, 336);
			labelLocations.Name = "labelLocations";
			labelLocations.Size = new System.Drawing.Size(58, 12);
			labelLocations.TabIndex = 7;
			labelLocations.Text = "Locations:";
			txTeams.AcceptsReturn = true;
			txTeams.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
			txTeams.Location = new System.Drawing.Point(286, 316);
			txTeams.Name = "txTeams";
			txTeams.Size = new System.Drawing.Size(264, 20);
			txTeams.TabIndex = 6;
			txTeams.Tag = "Teams";
			labelTeams.AutoSize = true;
			labelTeams.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelTeams.Location = new System.Drawing.Point(284, 301);
			labelTeams.Name = "labelTeams";
			labelTeams.Size = new System.Drawing.Size(42, 12);
			labelTeams.TabIndex = 5;
			labelTeams.Text = "Teams:";
			txWeblink.AcceptsReturn = true;
			txWeblink.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
			txWeblink.Location = new System.Drawing.Point(287, 412);
			txWeblink.Name = "txWeblink";
			txWeblink.Size = new System.Drawing.Size(263, 20);
			txWeblink.TabIndex = 12;
			txWeblink.Tag = "";
			labelWeb.AutoSize = true;
			labelWeb.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelWeb.Location = new System.Drawing.Point(285, 397);
			labelWeb.Name = "labelWeb";
			labelWeb.Size = new System.Drawing.Size(31, 12);
			labelWeb.TabIndex = 11;
			labelWeb.Text = "Web:";
			txCharacters.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
			txCharacters.Location = new System.Drawing.Point(11, 278);
			txCharacters.Multiline = true;
			txCharacters.Name = "txCharacters";
			txCharacters.Size = new System.Drawing.Size(260, 93);
			txCharacters.TabIndex = 2;
			txCharacters.Tag = "Characters";
			txCharacters.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txCharacters_KeyPress);
			labelCharacters.AutoSize = true;
			labelCharacters.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelCharacters.Location = new System.Drawing.Point(9, 263);
			labelCharacters.Name = "labelCharacters";
			labelCharacters.Size = new System.Drawing.Size(65, 12);
			labelCharacters.TabIndex = 1;
			labelCharacters.Text = "Characters:";
			tabCatalog.Controls.Add(labelReleasedTime);
			tabCatalog.Controls.Add(dtpReleasedTime);
			tabCatalog.Controls.Add(labelOpenedTime);
			tabCatalog.Controls.Add(dtpOpenedTime);
			tabCatalog.Controls.Add(labelAddedTime);
			tabCatalog.Controls.Add(dtpAddedTime);
			tabCatalog.Controls.Add(txPagesAsTextSimple);
			tabCatalog.Controls.Add(labelPagesAsTextSimple);
			tabCatalog.Controls.Add(txISBN);
			tabCatalog.Controls.Add(labelISBN);
			tabCatalog.Controls.Add(cbBookLocation);
			tabCatalog.Controls.Add(labelBookLocation);
			tabCatalog.Controls.Add(txCollectionStatus);
			tabCatalog.Controls.Add(cbBookPrice);
			tabCatalog.Controls.Add(labelBookPrice);
			tabCatalog.Controls.Add(txBookNotes);
			tabCatalog.Controls.Add(labelBookNotes);
			tabCatalog.Controls.Add(cbBookAge);
			tabCatalog.Controls.Add(labelBookAge);
			tabCatalog.Controls.Add(labelBookCollectionStatus);
			tabCatalog.Controls.Add(cbBookCondition);
			tabCatalog.Controls.Add(labelBookCondition);
			tabCatalog.Controls.Add(cbBookStore);
			tabCatalog.Controls.Add(labelBookStore);
			tabCatalog.Controls.Add(cbBookOwner);
			tabCatalog.Controls.Add(labelBookOwner);
			tabCatalog.Location = new System.Drawing.Point(4, 22);
			tabCatalog.Name = "tabCatalog";
			tabCatalog.Padding = new System.Windows.Forms.Padding(3);
			tabCatalog.Size = new System.Drawing.Size(566, 442);
			tabCatalog.TabIndex = 9;
			tabCatalog.Text = "Catalog";
			tabCatalog.UseVisualStyleBackColor = true;
			labelReleasedTime.AutoSize = true;
			labelReleasedTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelReleasedTime.Location = new System.Drawing.Point(314, 21);
			labelReleasedTime.Name = "labelReleasedTime";
			labelReleasedTime.Size = new System.Drawing.Size(56, 12);
			labelReleasedTime.TabIndex = 12;
			labelReleasedTime.Text = "Released:";
			dtpReleasedTime.CustomFormat = " ";
			dtpReleasedTime.Location = new System.Drawing.Point(315, 36);
			dtpReleasedTime.Name = "dtpReleasedTime";
			dtpReleasedTime.Size = new System.Drawing.Size(235, 20);
			dtpReleasedTime.TabIndex = 13;
			dtpReleasedTime.Value = new System.DateTime(0L);
			labelOpenedTime.AutoSize = true;
			labelOpenedTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelOpenedTime.Location = new System.Drawing.Point(315, 98);
			labelOpenedTime.Name = "labelOpenedTime";
			labelOpenedTime.Size = new System.Drawing.Size(77, 12);
			labelOpenedTime.TabIndex = 16;
			labelOpenedTime.Text = "Opened/Read:";
			dtpOpenedTime.CustomFormat = " ";
			dtpOpenedTime.Location = new System.Drawing.Point(316, 113);
			dtpOpenedTime.Name = "dtpOpenedTime";
			dtpOpenedTime.Size = new System.Drawing.Size(234, 20);
			dtpOpenedTime.TabIndex = 17;
			dtpOpenedTime.Value = new System.DateTime(0L);
			labelAddedTime.AutoSize = true;
			labelAddedTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelAddedTime.Location = new System.Drawing.Point(314, 58);
			labelAddedTime.Name = "labelAddedTime";
			labelAddedTime.Size = new System.Drawing.Size(98, 12);
			labelAddedTime.TabIndex = 14;
			labelAddedTime.Text = "Added/Purchased:";
			dtpAddedTime.CustomFormat = " ";
			dtpAddedTime.Location = new System.Drawing.Point(315, 73);
			dtpAddedTime.Name = "dtpAddedTime";
			dtpAddedTime.Size = new System.Drawing.Size(235, 20);
			dtpAddedTime.TabIndex = 15;
			dtpAddedTime.Value = new System.DateTime(0L);
			txPagesAsTextSimple.AcceptsReturn = true;
			txPagesAsTextSimple.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
			txPagesAsTextSimple.Location = new System.Drawing.Point(167, 73);
			txPagesAsTextSimple.Name = "txPagesAsTextSimple";
			txPagesAsTextSimple.Size = new System.Drawing.Size(126, 20);
			txPagesAsTextSimple.TabIndex = 7;
			txPagesAsTextSimple.Tag = "PageCountTextSimple";
			labelPagesAsTextSimple.AutoSize = true;
			labelPagesAsTextSimple.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelPagesAsTextSimple.Location = new System.Drawing.Point(165, 58);
			labelPagesAsTextSimple.Name = "labelPagesAsTextSimple";
			labelPagesAsTextSimple.Size = new System.Drawing.Size(40, 12);
			labelPagesAsTextSimple.TabIndex = 6;
			labelPagesAsTextSimple.Text = "Pages:";
			txISBN.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
			txISBN.Location = new System.Drawing.Point(11, 73);
			txISBN.Name = "txISBN";
			txISBN.Size = new System.Drawing.Size(150, 20);
			txISBN.TabIndex = 5;
			txISBN.Tag = "ISBN";
			labelISBN.AutoSize = true;
			labelISBN.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelISBN.Location = new System.Drawing.Point(9, 58);
			labelISBN.Name = "labelISBN";
			labelISBN.Size = new System.Drawing.Size(35, 12);
			labelISBN.TabIndex = 4;
			labelISBN.Text = "ISBN:";
			cbBookLocation.FormattingEnabled = true;
			cbBookLocation.Location = new System.Drawing.Point(317, 168);
			cbBookLocation.Name = "cbBookLocation";
			cbBookLocation.Size = new System.Drawing.Size(233, 21);
			cbBookLocation.TabIndex = 21;
			cbBookLocation.Tag = "BookLocation";
			labelBookLocation.AutoSize = true;
			labelBookLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelBookLocation.Location = new System.Drawing.Point(315, 154);
			labelBookLocation.Name = "labelBookLocation";
			labelBookLocation.Size = new System.Drawing.Size(80, 12);
			labelBookLocation.TabIndex = 20;
			labelBookLocation.Text = "Book Location:";
			txCollectionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
			txCollectionStatus.Location = new System.Drawing.Point(11, 210);
			txCollectionStatus.Name = "txCollectionStatus";
			txCollectionStatus.Size = new System.Drawing.Size(539, 20);
			txCollectionStatus.TabIndex = 23;
			txCollectionStatus.Tag = "CollectionStatus";
			cbBookPrice.FormattingEnabled = true;
			cbBookPrice.Location = new System.Drawing.Point(167, 34);
			cbBookPrice.Name = "cbBookPrice";
			cbBookPrice.Size = new System.Drawing.Size(126, 21);
			cbBookPrice.TabIndex = 3;
			cbBookPrice.Tag = "BookPrice";
			labelBookPrice.AutoSize = true;
			labelBookPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelBookPrice.Location = new System.Drawing.Point(165, 19);
			labelBookPrice.Name = "labelBookPrice";
			labelBookPrice.Size = new System.Drawing.Size(35, 12);
			labelBookPrice.TabIndex = 2;
			labelBookPrice.Text = "Price:";
			txBookNotes.AcceptsReturn = true;
			txBookNotes.FocusSelect = false;
			txBookNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
			txBookNotes.Location = new System.Drawing.Point(11, 265);
			txBookNotes.Multiline = true;
			txBookNotes.Name = "txBookNotes";
			txBookNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			txBookNotes.Size = new System.Drawing.Size(539, 157);
			txBookNotes.TabIndex = 25;
			txBookNotes.Tag = "BookNotes";
			labelBookNotes.AutoSize = true;
			labelBookNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelBookNotes.Location = new System.Drawing.Point(9, 250);
			labelBookNotes.Name = "labelBookNotes";
			labelBookNotes.Size = new System.Drawing.Size(120, 12);
			labelBookNotes.TabIndex = 24;
			labelBookNotes.Text = "Notes about this Book:";
			cbBookAge.FormattingEnabled = true;
			cbBookAge.Location = new System.Drawing.Point(11, 112);
			cbBookAge.Name = "cbBookAge";
			cbBookAge.Size = new System.Drawing.Size(150, 21);
			cbBookAge.TabIndex = 9;
			cbBookAge.Tag = "BookAge";
			labelBookAge.AutoSize = true;
			labelBookAge.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelBookAge.Location = new System.Drawing.Point(9, 97);
			labelBookAge.Name = "labelBookAge";
			labelBookAge.Size = new System.Drawing.Size(29, 12);
			labelBookAge.TabIndex = 8;
			labelBookAge.Text = "Age:";
			labelBookCollectionStatus.AutoSize = true;
			labelBookCollectionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelBookCollectionStatus.Location = new System.Drawing.Point(9, 195);
			labelBookCollectionStatus.Name = "labelBookCollectionStatus";
			labelBookCollectionStatus.Size = new System.Drawing.Size(96, 12);
			labelBookCollectionStatus.TabIndex = 22;
			labelBookCollectionStatus.Text = "Collection Status:";
			cbBookCondition.FormattingEnabled = true;
			cbBookCondition.Location = new System.Drawing.Point(167, 112);
			cbBookCondition.Name = "cbBookCondition";
			cbBookCondition.Size = new System.Drawing.Size(126, 21);
			cbBookCondition.TabIndex = 11;
			cbBookCondition.Tag = "BookCondition";
			labelBookCondition.AutoSize = true;
			labelBookCondition.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelBookCondition.Location = new System.Drawing.Point(165, 98);
			labelBookCondition.Name = "labelBookCondition";
			labelBookCondition.Size = new System.Drawing.Size(57, 12);
			labelBookCondition.TabIndex = 10;
			labelBookCondition.Text = "Condition:";
			cbBookStore.FormattingEnabled = true;
			cbBookStore.Location = new System.Drawing.Point(11, 35);
			cbBookStore.Name = "cbBookStore";
			cbBookStore.PromptText = null;
			cbBookStore.Size = new System.Drawing.Size(150, 21);
			cbBookStore.TabIndex = 1;
			cbBookStore.Tag = "BookStore";
			labelBookStore.AutoSize = true;
			labelBookStore.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelBookStore.Location = new System.Drawing.Point(11, 21);
			labelBookStore.Name = "labelBookStore";
			labelBookStore.Size = new System.Drawing.Size(36, 12);
			labelBookStore.TabIndex = 0;
			labelBookStore.Text = "Store:";
			cbBookOwner.FormattingEnabled = true;
			cbBookOwner.Location = new System.Drawing.Point(11, 168);
			cbBookOwner.Name = "cbBookOwner";
			cbBookOwner.Size = new System.Drawing.Size(282, 21);
			cbBookOwner.TabIndex = 19;
			cbBookOwner.Tag = "BookOwner";
			labelBookOwner.AutoSize = true;
			labelBookOwner.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelBookOwner.Location = new System.Drawing.Point(9, 154);
			labelBookOwner.Name = "labelBookOwner";
			labelBookOwner.Size = new System.Drawing.Size(42, 12);
			labelBookOwner.TabIndex = 18;
			labelBookOwner.Text = "Owner:";
			tabCustom.Controls.Add(customValuesData);
			tabCustom.Location = new System.Drawing.Point(4, 22);
			tabCustom.Name = "tabCustom";
			tabCustom.Padding = new System.Windows.Forms.Padding(3);
			tabCustom.Size = new System.Drawing.Size(566, 442);
			tabCustom.TabIndex = 10;
			tabCustom.Text = "Custom";
			tabCustom.UseVisualStyleBackColor = true;
			customValuesData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			customValuesData.Columns.AddRange(CustomValueName, CustomValueValue);
			customValuesData.Location = new System.Drawing.Point(11, 16);
			customValuesData.Name = "customValuesData";
			customValuesData.Size = new System.Drawing.Size(537, 410);
			customValuesData.TabIndex = 1;
			customValuesData.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(customValuesData_EditingControlShowing);
			CustomValueName.HeaderText = "Name";
			CustomValueName.Name = "CustomValueName";
			CustomValueName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			CustomValueValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			CustomValueValue.HeaderText = "Value";
			CustomValueValue.Name = "CustomValueValue";
			tabPages.Controls.Add(btResetPages);
			tabPages.Controls.Add(btPageView);
			tabPages.Controls.Add(labelPagesInfo);
			tabPages.Controls.Add(pagesView);
			tabPages.Location = new System.Drawing.Point(4, 22);
			tabPages.Name = "tabPages";
			tabPages.Size = new System.Drawing.Size(566, 442);
			tabPages.TabIndex = 6;
			tabPages.Text = "Pages";
			tabPages.UseVisualStyleBackColor = true;
			btResetPages.ContextMenuStrip = cmResetPages;
			btResetPages.Location = new System.Drawing.Point(441, 407);
			btResetPages.Name = "btResetPages";
			btResetPages.Size = new System.Drawing.Size(111, 23);
			btResetPages.TabIndex = 14;
			btResetPages.Text = "Reset";
			btResetPages.UseVisualStyleBackColor = true;
			btResetPages.ShowContextMenu += new System.EventHandler(btResetPages_ShowContextMenu);
			btResetPages.Click += new System.EventHandler(btResetPages_Click);
			cmResetPages.Items.AddRange(new System.Windows.Forms.ToolStripItem[2]
			{
				miOrderByName,
				miOrderByNameNumeric
			});
			cmResetPages.Name = "cmResetPages";
			cmResetPages.Size = new System.Drawing.Size(211, 48);
			cmResetPages.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(cmResetPages_ItemClicked);
			miOrderByName.Name = "miOrderByName";
			miOrderByName.Size = new System.Drawing.Size(210, 22);
			miOrderByName.Text = "Order by Name";
			miOrderByNameNumeric.Name = "miOrderByNameNumeric";
			miOrderByNameNumeric.Size = new System.Drawing.Size(210, 22);
			miOrderByNameNumeric.Text = "Order by Name (numeric)";
			btPageView.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.SmallArrowDown;
			btPageView.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			btPageView.Location = new System.Drawing.Point(479, 9);
			btPageView.Name = "btPageView";
			btPageView.Size = new System.Drawing.Size(73, 23);
			btPageView.TabIndex = 2;
			btPageView.Text = "Pages";
			btPageView.UseVisualStyleBackColor = true;
			btPageView.Click += new System.EventHandler(btPageViews_Click);
			labelPagesInfo.AutoSize = true;
			labelPagesInfo.Location = new System.Drawing.Point(8, 14);
			labelPagesInfo.Name = "labelPagesInfo";
			labelPagesInfo.Size = new System.Drawing.Size(421, 13);
			labelPagesInfo.TabIndex = 0;
			labelPagesInfo.Text = "Change the page order with drag & drop or use the context menu to change page types:";
			labelPagesInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			labelPagesInfo.UseMnemonic = false;
			pagesView.Bookmark = null;
			pagesView.CreateBackdrop = false;
			pagesView.Location = new System.Drawing.Point(11, 38);
			pagesView.Name = "pagesView";
			pagesView.Size = new System.Drawing.Size(541, 363);
			pagesView.TabIndex = 1;
			tabColors.Controls.Add(panelImage);
			tabColors.Controls.Add(panelImageControls);
			tabColors.Location = new System.Drawing.Point(4, 22);
			tabColors.Name = "tabColors";
			tabColors.Size = new System.Drawing.Size(566, 442);
			tabColors.TabIndex = 7;
			tabColors.Text = "Colors";
			tabColors.UseVisualStyleBackColor = true;
			panelImage.Controls.Add(labelCurrentPage);
			panelImage.Controls.Add(chkShowImageControls);
			panelImage.Controls.Add(btLastPage);
			panelImage.Controls.Add(btFirstPage);
			panelImage.Controls.Add(btNextPage);
			panelImage.Controls.Add(btPrevPage);
			panelImage.Controls.Add(pageViewer);
			panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
			panelImage.Location = new System.Drawing.Point(0, 0);
			panelImage.Name = "panelImage";
			panelImage.Size = new System.Drawing.Size(566, 317);
			panelImage.TabIndex = 12;
			labelCurrentPage.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			labelCurrentPage.Location = new System.Drawing.Point(184, 291);
			labelCurrentPage.Name = "labelCurrentPage";
			labelCurrentPage.Size = new System.Drawing.Size(202, 21);
			labelCurrentPage.TabIndex = 6;
			labelCurrentPage.Text = "Page Text";
			labelCurrentPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			chkShowImageControls.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			chkShowImageControls.Appearance = System.Windows.Forms.Appearance.Button;
			chkShowImageControls.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.DoubleArrow;
			chkShowImageControls.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			chkShowImageControls.Location = new System.Drawing.Point(416, 291);
			chkShowImageControls.Name = "chkShowImageControls";
			chkShowImageControls.Size = new System.Drawing.Size(140, 23);
			chkShowImageControls.TabIndex = 5;
			chkShowImageControls.Text = "Image Control";
			chkShowImageControls.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			chkShowImageControls.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			chkShowImageControls.UseVisualStyleBackColor = true;
			chkShowImageControls.CheckedChanged += new System.EventHandler(chkShowColorControls_CheckedChanged);
			btLastPage.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btLastPage.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.GoLast;
			btLastPage.Location = new System.Drawing.Point(107, 290);
			btLastPage.Name = "btLastPage";
			btLastPage.Size = new System.Drawing.Size(32, 23);
			btLastPage.TabIndex = 4;
			btLastPage.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			btLastPage.UseVisualStyleBackColor = true;
			btLastPage.Click += new System.EventHandler(btLastPage_Click);
			btFirstPage.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btFirstPage.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.GoFirst;
			btFirstPage.Location = new System.Drawing.Point(11, 290);
			btFirstPage.Name = "btFirstPage";
			btFirstPage.Size = new System.Drawing.Size(32, 23);
			btFirstPage.TabIndex = 3;
			btFirstPage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			btFirstPage.UseVisualStyleBackColor = true;
			btFirstPage.Click += new System.EventHandler(btFirstPage_Click);
			btNextPage.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btNextPage.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.GoNext;
			btNextPage.Location = new System.Drawing.Point(75, 290);
			btNextPage.Name = "btNextPage";
			btNextPage.Size = new System.Drawing.Size(32, 23);
			btNextPage.TabIndex = 2;
			btNextPage.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			btNextPage.UseVisualStyleBackColor = true;
			btNextPage.Click += new System.EventHandler(btNextPage_Click);
			btPrevPage.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btPrevPage.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.GoPrevious;
			btPrevPage.Location = new System.Drawing.Point(43, 290);
			btPrevPage.Name = "btPrevPage";
			btPrevPage.Size = new System.Drawing.Size(32, 23);
			btPrevPage.TabIndex = 1;
			btPrevPage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			btPrevPage.UseVisualStyleBackColor = true;
			btPrevPage.Click += new System.EventHandler(btPrevPage_Click);
			pageViewer.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			pageViewer.AutoScrollMode = cYo.Common.Windows.Forms.AutoScrollMode.Pan;
			pageViewer.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			pageViewer.ForeColor = System.Drawing.Color.White;
			pageViewer.Location = new System.Drawing.Point(11, 12);
			pageViewer.Name = "pageViewer";
			pageViewer.ScaleMode = cYo.Common.Drawing.ScaleMode.FitWidth;
			pageViewer.Size = new System.Drawing.Size(545, 275);
			pageViewer.TabIndex = 0;
			pageViewer.Text = "Double Click on Color to set White Point";
			pageViewer.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
			pageViewer.VisibleChanged += new System.EventHandler(pageViewer_VisibleChanged);
			pageViewer.DoubleClick += new System.EventHandler(pageViewer_DoubleClick);
			panelImageControls.Controls.Add(labelSaturation);
			panelImageControls.Controls.Add(labelContrast);
			panelImageControls.Controls.Add(tbGamma);
			panelImageControls.Controls.Add(tbSaturation);
			panelImageControls.Controls.Add(labelGamma);
			panelImageControls.Controls.Add(tbBrightness);
			panelImageControls.Controls.Add(tbSharpening);
			panelImageControls.Controls.Add(tbContrast);
			panelImageControls.Controls.Add(labelSharpening);
			panelImageControls.Controls.Add(labelBrightness);
			panelImageControls.Controls.Add(btResetColors);
			panelImageControls.Dock = System.Windows.Forms.DockStyle.Bottom;
			panelImageControls.Location = new System.Drawing.Point(0, 317);
			panelImageControls.Name = "panelImageControls";
			panelImageControls.Size = new System.Drawing.Size(566, 125);
			panelImageControls.TabIndex = 13;
			panelImageControls.Visible = false;
			labelSaturation.AutoSize = true;
			labelSaturation.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelSaturation.Location = new System.Drawing.Point(11, 34);
			labelSaturation.Name = "labelSaturation";
			labelSaturation.Size = new System.Drawing.Size(57, 12);
			labelSaturation.TabIndex = 1;
			labelSaturation.Text = "Saturation";
			labelContrast.AutoSize = true;
			labelContrast.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold);
			labelContrast.Location = new System.Drawing.Point(296, 34);
			labelContrast.Name = "labelContrast";
			labelContrast.Size = new System.Drawing.Size(49, 12);
			labelContrast.TabIndex = 5;
			labelContrast.Text = "Contrast";
			tbGamma.Location = new System.Drawing.Point(363, 52);
			tbGamma.Minimum = -100;
			tbGamma.Name = "tbGamma";
			tbGamma.Size = new System.Drawing.Size(192, 18);
			tbGamma.TabIndex = 8;
			tbGamma.ThumbSize = new System.Drawing.Size(8, 16);
			tbGamma.TickFrequency = 16;
			tbGamma.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			tbGamma.Scroll += new System.EventHandler(ColorAdjustment_Scroll);
			tbGamma.ValueChanged += new System.EventHandler(AdjustmentSliderChanged);
			tbSaturation.Location = new System.Drawing.Point(78, 28);
			tbSaturation.Minimum = -100;
			tbSaturation.Name = "tbSaturation";
			tbSaturation.Size = new System.Drawing.Size(192, 18);
			tbSaturation.TabIndex = 2;
			tbSaturation.ThumbSize = new System.Drawing.Size(8, 16);
			tbSaturation.TickFrequency = 16;
			tbSaturation.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			tbSaturation.Scroll += new System.EventHandler(ColorAdjustment_Scroll);
			tbSaturation.ValueChanged += new System.EventHandler(AdjustmentSliderChanged);
			labelGamma.AutoSize = true;
			labelGamma.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold);
			labelGamma.Location = new System.Drawing.Point(296, 58);
			labelGamma.Name = "labelGamma";
			labelGamma.Size = new System.Drawing.Size(43, 12);
			labelGamma.TabIndex = 7;
			labelGamma.Text = "Gamma";
			tbBrightness.Location = new System.Drawing.Point(78, 52);
			tbBrightness.Minimum = -100;
			tbBrightness.Name = "tbBrightness";
			tbBrightness.Size = new System.Drawing.Size(192, 18);
			tbBrightness.TabIndex = 4;
			tbBrightness.Text = "tbBrightness";
			tbBrightness.ThumbSize = new System.Drawing.Size(8, 16);
			tbBrightness.TickFrequency = 16;
			tbBrightness.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			tbBrightness.Scroll += new System.EventHandler(ColorAdjustment_Scroll);
			tbBrightness.ValueChanged += new System.EventHandler(AdjustmentSliderChanged);
			tbSharpening.LargeChange = 1;
			tbSharpening.Location = new System.Drawing.Point(81, 86);
			tbSharpening.Maximum = 3;
			tbSharpening.Name = "tbSharpening";
			tbSharpening.Size = new System.Drawing.Size(189, 18);
			tbSharpening.TabIndex = 10;
			tbSharpening.Text = "tbSaturation";
			tbSharpening.ThumbSize = new System.Drawing.Size(8, 16);
			tbSharpening.TickFrequency = 1;
			tbSharpening.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			tbSharpening.Scroll += new System.EventHandler(ColorAdjustment_Scroll);
			tbContrast.Location = new System.Drawing.Point(363, 28);
			tbContrast.Minimum = -100;
			tbContrast.Name = "tbContrast";
			tbContrast.Size = new System.Drawing.Size(192, 18);
			tbContrast.TabIndex = 6;
			tbContrast.Text = "tbSaturation";
			tbContrast.ThumbSize = new System.Drawing.Size(8, 16);
			tbContrast.TickFrequency = 16;
			tbContrast.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			tbContrast.Scroll += new System.EventHandler(ColorAdjustment_Scroll);
			tbContrast.ValueChanged += new System.EventHandler(AdjustmentSliderChanged);
			labelSharpening.AutoSize = true;
			labelSharpening.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold);
			labelSharpening.Location = new System.Drawing.Point(11, 92);
			labelSharpening.Name = "labelSharpening";
			labelSharpening.Size = new System.Drawing.Size(61, 12);
			labelSharpening.TabIndex = 9;
			labelSharpening.Text = "Sharpening";
			labelBrightness.AutoSize = true;
			labelBrightness.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold);
			labelBrightness.Location = new System.Drawing.Point(11, 58);
			labelBrightness.Name = "labelBrightness";
			labelBrightness.Size = new System.Drawing.Size(59, 12);
			labelBrightness.TabIndex = 3;
			labelBrightness.Text = "Brightness";
			btResetColors.Location = new System.Drawing.Point(478, 86);
			btResetColors.Name = "btResetColors";
			btResetColors.Size = new System.Drawing.Size(77, 24);
			btResetColors.TabIndex = 11;
			btResetColors.Text = "Reset";
			btResetColors.UseVisualStyleBackColor = true;
			btResetColors.Click += new System.EventHandler(btReset_Click);
			btPrev.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btPrev.Location = new System.Drawing.Point(8, 483);
			btPrev.Name = "btPrev";
			btPrev.Size = new System.Drawing.Size(80, 24);
			btPrev.TabIndex = 1;
			btPrev.Text = "&Previous";
			btPrev.Click += new System.EventHandler(btPrev_Click);
			btNext.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btNext.Location = new System.Drawing.Point(92, 483);
			btNext.Name = "btNext";
			btNext.Size = new System.Drawing.Size(80, 24);
			btNext.TabIndex = 2;
			btNext.Text = "&Next";
			btNext.Click += new System.EventHandler(btNext_Click);
			btScript.AutoEllipsis = true;
			btScript.Location = new System.Drawing.Point(188, 484);
			btScript.Name = "btScript";
			btScript.Size = new System.Drawing.Size(135, 23);
			btScript.TabIndex = 3;
			btScript.Text = "Lorem Ipsum";
			btScript.UseVisualStyleBackColor = true;
			btScript.Visible = false;
			btApply.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btApply.Location = new System.Drawing.Point(501, 483);
			btApply.Name = "btApply";
			btApply.Size = new System.Drawing.Size(80, 24);
			btApply.TabIndex = 6;
			btApply.Text = "&Apply";
			btApply.Click += new System.EventHandler(btApply_Click);
			base.AcceptButton = btOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(593, 517);
			base.Controls.Add(btApply);
			base.Controls.Add(btScript);
			base.Controls.Add(tabControl);
			base.Controls.Add(btNext);
			base.Controls.Add(btCancel);
			base.Controls.Add(btPrev);
			base.Controls.Add(btOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ComicBookDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Info";
			base.DragDrop += new System.Windows.Forms.DragEventHandler(ComicBookDialog_DragDrop);
			base.DragOver += new System.Windows.Forms.DragEventHandler(ComicBookDialog_DragOver);
			tabControl.ResumeLayout(false);
			tabSummary.ResumeLayout(false);
			cmThumbnail.ResumeLayout(false);
			tabDetails.ResumeLayout(false);
			tabDetails.PerformLayout();
			tabPlot.ResumeLayout(false);
			tabPlot.PerformLayout();
			tabNotes.ResumeLayout(false);
			tabPageSummary.ResumeLayout(false);
			tabPageSummary.PerformLayout();
			tabPageNotes.ResumeLayout(false);
			tabPageNotes.PerformLayout();
			tabPageReview.ResumeLayout(false);
			tabPageReview.PerformLayout();
			tabCatalog.ResumeLayout(false);
			tabCatalog.PerformLayout();
			tabCustom.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)customValuesData).EndInit();
			tabPages.ResumeLayout(false);
			tabPages.PerformLayout();
			cmResetPages.ResumeLayout(false);
			tabColors.ResumeLayout(false);
			panelImage.ResumeLayout(false);
			panelImageControls.ResumeLayout(false);
			panelImageControls.PerformLayout();
			ResumeLayout(false);
		}

	private IContainer components;

		private TextBoxEx txVolume;

		private TextBoxEx txYear;

		private TextBoxEx txSeries;

		private TextBoxEx txCount;

		private TextBoxEx txNumber;

		private Button btCancel;

		private Button btOK;

		private TabControl tabControl;

		private TabPage tabSummary;

		private Label labelWhere;

		private Label labelPages;

		private Label lblPath;

		private Label lblPages;

		private Label labelType;

		private Label lblType;

		private Button btPrev;

		private Button btNext;

		private TabPage tabDetails;

		private Label labelVolume;

		private Label labelYear;

		private Label labelSeries;

		private RatingControl txRating;

		private TextBoxEx txWriter;

		private TextBoxEx txColorist;

		private TextBoxEx txInker;

		private TextBoxEx txPenciller;

		private Label labelGenre;

		private Label labelColorist;

		private Label labelPublisher;

		private TextBoxEx txTitle;

		private Label labelInker;

		private Label labelCount;

		private Label labelNumber;

		private Label labelWriter;

		private Label labelPenciller;

		private Label labelTitle;

		private Panel panel1;

		private TextBoxEx txLetterer;

		private Label labelLetterer;

		private TextBoxEx txEditor;

		private TextBoxEx txCoverArtist;

		private Label labelEditor;

		private Label labelCoverArtist;

		private ComboBox cbPublisher;

		private TrackBarLite tbBrightness;

		private TrackBarLite tbSaturation;

		private TrackBarLite tbContrast;

		private Label labelSaturation;

		private Label labelContrast;

		private Label labelBrightness;

		private ThumbnailControl coverThumbnail;

		private Button btResetColors;

		private TextBoxEx txAlternateSeries;

		private Label labelAlternateSeries;

		private TextBoxEx txAlternateCount;

		private TextBoxEx txAlternateNumber;

		private Label labelAlternateCount;

		private Label labelAlternateNumber;

		private TextBoxEx txMonth;

		private Label labelMonth;

		private TextBoxEx txTags;

		private Label labelImprint;

		private ComboBox cbImprint;

		private TabPage tabPages;

		private LanguageComboBox cbLanguage;

		private Label labelLanguage;

		private BitmapViewer pageViewer;

		private TabPage tabColors;

		private PagesView pagesView;

		private Label labelPagesInfo;

		private ComboBoxEx cbFormat;

		private Label labelFormat;

		private ComboBox cbBlackAndWhite;

		private ComboBox cbManga;

		private Label labelBlackAndWhite;

		private Label labelManga;

		private ComboBox cbEnableProposed;

		private Label labelEnableProposed;

		private Button btPageView;

		private ComboBox cbAgeRating;

		private Label labelAgeRating;

		private Label labelTags;

		private TabPage tabPlot;

		private TextBoxEx txCharacters;

		private Label labelCharacters;

		private TextBoxEx txGenre;

		private SplitButton btScript;

		private TrackBarLite tbSharpening;

		private Label labelSharpening;

		private ToolTip toolTip;

		private TextBoxEx txWeblink;

		private Label labelWeb;

		private LinkLabel linkLabel;

		private ComboBox cbEnableDynamicUpdate;

		private Label labelEnableDynamicUpdate;

		private TextBoxEx txLocations;

		private Label labelLocations;

		private TextBoxEx txTeams;

		private Label labelTeams;

		private Button btLinkFile;

		private Label labelCommunityRating;

		private RatingControl txCommunityRating;

		private Label labelMyRating;

		private Panel whereSeparator;

		private SplitButton btThumbnail;

		private ContextMenuStrip cmThumbnail;

		private ToolStripMenuItem miResetThumbnail;

		private ToolStripSeparator toolStripMenuItem1;

		private TabPage tabCatalog;

		private ComboBox cbBookPrice;

		private Label labelBookPrice;

		private TextBoxEx txBookNotes;

		private Label labelBookNotes;

		private ComboBox cbBookAge;

		private Label labelBookAge;

		private Label labelBookCollectionStatus;

		private ComboBox cbBookCondition;

		private Label labelBookCondition;

		private ComboBoxEx cbBookStore;

		private Label labelBookStore;

		private ComboBox cbBookOwner;

		private Label labelBookOwner;

		private TextBoxEx txCollectionStatus;

		private ComboBox cbBookLocation;

		private Label labelBookLocation;

		private TextBoxEx txISBN;

		private Label labelISBN;

		private TextBoxEx txPagesAsTextSimple;

		private Label labelPagesAsTextSimple;

		private Label labelOpenedTime;

		private NullableDateTimePicker dtpOpenedTime;

		private Label labelAddedTime;

		private NullableDateTimePicker dtpAddedTime;

		private ComboBox cbSeriesComplete;

		private Label labelSeriesComplete;

		private TrackBarLite tbGamma;

		private Label labelGamma;

		private Panel panelImage;

		private CheckBox chkShowImageControls;

		private Button btLastPage;

		private Button btFirstPage;

		private AutoRepeatButton btNextPage;

		private AutoRepeatButton btPrevPage;

		private Panel panelImageControls;

		private Label labelCurrentPage;

		private TextBoxEx txScanInformation;

		private Label labelScanInformation;

		private Label labelSeriesGroup;

		private TextBoxEx txSeriesGroup;

		private Label labelStoryArc;

		private TextBoxEx txStoryArc;

		private TabControl tabNotes;

		private TabPage tabPageSummary;

		private TextBoxEx txSummary;

		private TabPage tabPageNotes;

		private TextBoxEx txNotes;

		private TabPage tabPageReview;

		private TextBoxEx txReview;

		private TextBoxEx txMainCharacterOrTeam;

		private Label labelMainCharacterOrTeam;

		private SplitButton btResetPages;

		private ContextMenuStrip cmResetPages;

		private ToolStripMenuItem miOrderByName;

		private ToolStripMenuItem miOrderByNameNumeric;

		private Button btApply;

		private TextBoxEx txDay;

		private Label labelDay;

		private Label labelReleasedTime;

		private NullableDateTimePicker dtpReleasedTime;

		private TabPage tabCustom;

		private DataGridView customValuesData;

		private DataGridViewTextBoxColumn CustomValueName;

		private DataGridViewTextBoxColumn CustomValueValue;
	}
}
