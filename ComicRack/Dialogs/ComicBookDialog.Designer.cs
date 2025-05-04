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
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
			this.components = new System.ComponentModel.Container();
			this.btCancel = new System.Windows.Forms.Button();
			this.btOK = new System.Windows.Forms.Button();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabSummary = new System.Windows.Forms.TabPage();
			this.btThumbnail = new cYo.Common.Windows.Forms.SplitButton();
			this.cmThumbnail = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.miResetThumbnail = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.labelCommunityRating = new System.Windows.Forms.Label();
			this.txCommunityRating = new cYo.Projects.ComicRack.Engine.Controls.RatingControl();
			this.labelMyRating = new System.Windows.Forms.Label();
			this.btLinkFile = new System.Windows.Forms.Button();
			this.linkLabel = new System.Windows.Forms.LinkLabel();
			this.coverThumbnail = new cYo.Projects.ComicRack.Engine.Controls.ThumbnailControl();
			this.whereSeparator = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.labelWhere = new System.Windows.Forms.Label();
			this.labelType = new System.Windows.Forms.Label();
			this.txRating = new cYo.Projects.ComicRack.Engine.Controls.RatingControl();
			this.labelPages = new System.Windows.Forms.Label();
			this.lblType = new System.Windows.Forms.Label();
			this.lblPath = new System.Windows.Forms.Label();
			this.lblPages = new System.Windows.Forms.Label();
			this.tabDetails = new System.Windows.Forms.TabPage();
			this.txDay = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelDay = new System.Windows.Forms.Label();
			this.labelSeriesGroup = new System.Windows.Forms.Label();
			this.txSeriesGroup = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelStoryArc = new System.Windows.Forms.Label();
			this.txStoryArc = new cYo.Common.Windows.Forms.TextBoxEx();
			this.cbSeriesComplete = new System.Windows.Forms.ComboBox();
			this.labelSeriesComplete = new System.Windows.Forms.Label();
			this.cbEnableDynamicUpdate = new System.Windows.Forms.ComboBox();
			this.labelEnableDynamicUpdate = new System.Windows.Forms.Label();
			this.txGenre = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelTags = new System.Windows.Forms.Label();
			this.cbEnableProposed = new System.Windows.Forms.ComboBox();
			this.txTags = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelEnableProposed = new System.Windows.Forms.Label();
			this.txVolume = new cYo.Common.Windows.Forms.TextBoxEx();
			this.cbAgeRating = new System.Windows.Forms.ComboBox();
			this.txEditor = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelEditor = new System.Windows.Forms.Label();
			this.txMonth = new cYo.Common.Windows.Forms.TextBoxEx();
			this.txYear = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelAgeRating = new System.Windows.Forms.Label();
			this.cbFormat = new cYo.Common.Windows.Forms.ComboBoxEx();
			this.txColorist = new cYo.Common.Windows.Forms.TextBoxEx();
			this.txSeries = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelFormat = new System.Windows.Forms.Label();
			this.labelAlternateSeries = new System.Windows.Forms.Label();
			this.txAlternateSeries = new cYo.Common.Windows.Forms.TextBoxEx();
			this.cbImprint = new System.Windows.Forms.ComboBox();
			this.cbBlackAndWhite = new System.Windows.Forms.ComboBox();
			this.labelVolume = new System.Windows.Forms.Label();
			this.txInker = new cYo.Common.Windows.Forms.TextBoxEx();
			this.cbManga = new System.Windows.Forms.ComboBox();
			this.labelYear = new System.Windows.Forms.Label();
			this.labelMonth = new System.Windows.Forms.Label();
			this.labelBlackAndWhite = new System.Windows.Forms.Label();
			this.txAlternateCount = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelManga = new System.Windows.Forms.Label();
			this.labelSeries = new System.Windows.Forms.Label();
			this.labelLanguage = new System.Windows.Forms.Label();
			this.labelImprint = new System.Windows.Forms.Label();
			this.labelGenre = new System.Windows.Forms.Label();
			this.labelColorist = new System.Windows.Forms.Label();
			this.txCount = new cYo.Common.Windows.Forms.TextBoxEx();
			this.cbLanguage = new cYo.Common.Windows.Forms.LanguageComboBox();
			this.cbPublisher = new System.Windows.Forms.ComboBox();
			this.txPenciller = new cYo.Common.Windows.Forms.TextBoxEx();
			this.txAlternateNumber = new cYo.Common.Windows.Forms.TextBoxEx();
			this.txNumber = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelPublisher = new System.Windows.Forms.Label();
			this.labelCoverArtist = new System.Windows.Forms.Label();
			this.txCoverArtist = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelInker = new System.Windows.Forms.Label();
			this.labelAlternateCount = new System.Windows.Forms.Label();
			this.txTitle = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelCount = new System.Windows.Forms.Label();
			this.labelAlternateNumber = new System.Windows.Forms.Label();
			this.labelNumber = new System.Windows.Forms.Label();
			this.labelLetterer = new System.Windows.Forms.Label();
			this.labelPenciller = new System.Windows.Forms.Label();
			this.txLetterer = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelTitle = new System.Windows.Forms.Label();
			this.labelWriter = new System.Windows.Forms.Label();
			this.txWriter = new cYo.Common.Windows.Forms.TextBoxEx();
			this.tabPlot = new System.Windows.Forms.TabPage();
			this.tabNotes = new System.Windows.Forms.TabControl();
			this.tabPageSummary = new System.Windows.Forms.TabPage();
			this.txSummary = new cYo.Common.Windows.Forms.TextBoxEx();
			this.tabPageNotes = new System.Windows.Forms.TabPage();
			this.txNotes = new cYo.Common.Windows.Forms.TextBoxEx();
			this.tabPageReview = new System.Windows.Forms.TabPage();
			this.txReview = new cYo.Common.Windows.Forms.TextBoxEx();
			this.txMainCharacterOrTeam = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelMainCharacterOrTeam = new System.Windows.Forms.Label();
			this.txScanInformation = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelScanInformation = new System.Windows.Forms.Label();
			this.txLocations = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelLocations = new System.Windows.Forms.Label();
			this.txTeams = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelTeams = new System.Windows.Forms.Label();
			this.txWeblink = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelWeb = new System.Windows.Forms.Label();
			this.txCharacters = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelCharacters = new System.Windows.Forms.Label();
			this.tabCatalog = new System.Windows.Forms.TabPage();
			this.labelReleasedTime = new System.Windows.Forms.Label();
			this.dtpReleasedTime = new cYo.Common.Windows.Forms.NullableDateTimePicker();
			this.labelOpenedTime = new System.Windows.Forms.Label();
			this.dtpOpenedTime = new cYo.Common.Windows.Forms.NullableDateTimePicker();
			this.labelAddedTime = new System.Windows.Forms.Label();
			this.dtpAddedTime = new cYo.Common.Windows.Forms.NullableDateTimePicker();
			this.txPagesAsTextSimple = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelPagesAsTextSimple = new System.Windows.Forms.Label();
			this.txISBN = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelISBN = new System.Windows.Forms.Label();
			this.cbBookLocation = new System.Windows.Forms.ComboBox();
			this.labelBookLocation = new System.Windows.Forms.Label();
			this.txCollectionStatus = new cYo.Common.Windows.Forms.TextBoxEx();
			this.cbBookPrice = new System.Windows.Forms.ComboBox();
			this.labelBookPrice = new System.Windows.Forms.Label();
			this.txBookNotes = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelBookNotes = new System.Windows.Forms.Label();
			this.cbBookAge = new System.Windows.Forms.ComboBox();
			this.labelBookAge = new System.Windows.Forms.Label();
			this.labelBookCollectionStatus = new System.Windows.Forms.Label();
			this.cbBookCondition = new System.Windows.Forms.ComboBox();
			this.labelBookCondition = new System.Windows.Forms.Label();
			this.cbBookStore = new cYo.Common.Windows.Forms.ComboBoxEx();
			this.labelBookStore = new System.Windows.Forms.Label();
			this.cbBookOwner = new System.Windows.Forms.ComboBox();
			this.labelBookOwner = new System.Windows.Forms.Label();
			this.tabCustom = new System.Windows.Forms.TabPage();
			this.customValuesData = new System.Windows.Forms.DataGridView();
			this.CustomValueName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CustomValueValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabPages = new System.Windows.Forms.TabPage();
			this.btResetPages = new cYo.Common.Windows.Forms.SplitButton();
			this.cmResetPages = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.miOrderByName = new System.Windows.Forms.ToolStripMenuItem();
			this.miOrderByNameNumeric = new System.Windows.Forms.ToolStripMenuItem();
			this.btPageView = new System.Windows.Forms.Button();
			this.labelPagesInfo = new System.Windows.Forms.Label();
			this.pagesView = new cYo.Projects.ComicRack.Viewer.Controls.PagesView();
			this.tabColors = new System.Windows.Forms.TabPage();
			this.panelImage = new System.Windows.Forms.Panel();
			this.labelCurrentPage = new System.Windows.Forms.Label();
			this.chkShowImageControls = new System.Windows.Forms.CheckBox();
			this.btLastPage = new System.Windows.Forms.Button();
			this.btFirstPage = new System.Windows.Forms.Button();
			this.btNextPage = new cYo.Common.Windows.Forms.AutoRepeatButton();
			this.btPrevPage = new cYo.Common.Windows.Forms.AutoRepeatButton();
			this.pageViewer = new cYo.Common.Windows.Forms.BitmapViewer();
			this.panelImageControls = new System.Windows.Forms.Panel();
			this.labelSaturation = new System.Windows.Forms.Label();
			this.labelContrast = new System.Windows.Forms.Label();
			this.tbGamma = new cYo.Common.Windows.Forms.TrackBarLite();
			this.tbSaturation = new cYo.Common.Windows.Forms.TrackBarLite();
			this.labelGamma = new System.Windows.Forms.Label();
			this.tbBrightness = new cYo.Common.Windows.Forms.TrackBarLite();
			this.tbSharpening = new cYo.Common.Windows.Forms.TrackBarLite();
			this.tbContrast = new cYo.Common.Windows.Forms.TrackBarLite();
			this.labelSharpening = new System.Windows.Forms.Label();
			this.labelBrightness = new System.Windows.Forms.Label();
			this.btResetColors = new System.Windows.Forms.Button();
			this.btPrev = new System.Windows.Forms.Button();
			this.btNext = new System.Windows.Forms.Button();
			this.btScript = new cYo.Common.Windows.Forms.SplitButton();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.btApply = new System.Windows.Forms.Button();
			this.labelTranslator = new System.Windows.Forms.Label();
			this.txTranslator = new cYo.Common.Windows.Forms.TextBoxEx();
			this.tabControl.SuspendLayout();
			this.tabSummary.SuspendLayout();
			this.cmThumbnail.SuspendLayout();
			this.tabDetails.SuspendLayout();
			this.tabPlot.SuspendLayout();
			this.tabNotes.SuspendLayout();
			this.tabPageSummary.SuspendLayout();
			this.tabPageNotes.SuspendLayout();
			this.tabPageReview.SuspendLayout();
			this.tabCatalog.SuspendLayout();
			this.tabCustom.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.customValuesData)).BeginInit();
			this.tabPages.SuspendLayout();
			this.cmResetPages.SuspendLayout();
			this.tabColors.SuspendLayout();
			this.panelImage.SuspendLayout();
			this.panelImageControls.SuspendLayout();
			this.SuspendLayout();
			// 
			// btCancel
			// 
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btCancel.Location = new System.Drawing.Point(415, 483);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(80, 24);
			this.btCancel.TabIndex = 5;
			this.btCancel.Text = "&Cancel";
			// 
			// btOK
			// 
			this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btOK.Location = new System.Drawing.Point(329, 483);
			this.btOK.Name = "btOK";
			this.btOK.Size = new System.Drawing.Size(80, 24);
			this.btOK.TabIndex = 4;
			this.btOK.Text = "&OK";
			this.btOK.Click += new System.EventHandler(this.btOK_Click);
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabSummary);
			this.tabControl.Controls.Add(this.tabDetails);
			this.tabControl.Controls.Add(this.tabPlot);
			this.tabControl.Controls.Add(this.tabCatalog);
			this.tabControl.Controls.Add(this.tabCustom);
			this.tabControl.Controls.Add(this.tabPages);
			this.tabControl.Controls.Add(this.tabColors);
			this.tabControl.Location = new System.Drawing.Point(8, 9);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(574, 468);
			this.tabControl.TabIndex = 0;
			// 
			// tabSummary
			// 
			this.tabSummary.Controls.Add(this.btThumbnail);
			this.tabSummary.Controls.Add(this.labelCommunityRating);
			this.tabSummary.Controls.Add(this.txCommunityRating);
			this.tabSummary.Controls.Add(this.labelMyRating);
			this.tabSummary.Controls.Add(this.btLinkFile);
			this.tabSummary.Controls.Add(this.linkLabel);
			this.tabSummary.Controls.Add(this.coverThumbnail);
			this.tabSummary.Controls.Add(this.whereSeparator);
			this.tabSummary.Controls.Add(this.panel1);
			this.tabSummary.Controls.Add(this.labelWhere);
			this.tabSummary.Controls.Add(this.labelType);
			this.tabSummary.Controls.Add(this.txRating);
			this.tabSummary.Controls.Add(this.labelPages);
			this.tabSummary.Controls.Add(this.lblType);
			this.tabSummary.Controls.Add(this.lblPath);
			this.tabSummary.Controls.Add(this.lblPages);
			this.tabSummary.Location = new System.Drawing.Point(4, 22);
			this.tabSummary.Name = "tabSummary";
			this.tabSummary.Padding = new System.Windows.Forms.Padding(3);
			this.tabSummary.Size = new System.Drawing.Size(566, 442);
			this.tabSummary.TabIndex = 0;
			this.tabSummary.Text = "Summary";
			this.tabSummary.UseVisualStyleBackColor = true;
			// 
			// btThumbnail
			// 
			this.btThumbnail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btThumbnail.ContextMenuStrip = this.cmThumbnail;
			this.btThumbnail.Location = new System.Drawing.Point(372, 347);
			this.btThumbnail.Name = "btThumbnail";
			this.btThumbnail.Size = new System.Drawing.Size(152, 23);
			this.btThumbnail.TabIndex = 13;
			this.btThumbnail.Text = "Thumbnail...";
			this.btThumbnail.UseVisualStyleBackColor = true;
			this.btThumbnail.Visible = false;
			this.btThumbnail.ShowContextMenu += new System.EventHandler(this.btThumbnail_ShowContextMenu);
			this.btThumbnail.Click += new System.EventHandler(this.btThumbnail_Click);
			// 
			// cmThumbnail
			// 
			this.cmThumbnail.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.miResetThumbnail,
			this.toolStripMenuItem1});
			this.cmThumbnail.Name = "cmKeyboardLayout";
			this.cmThumbnail.Size = new System.Drawing.Size(103, 32);
			// 
			// miResetThumbnail
			// 
			this.miResetThumbnail.Name = "miResetThumbnail";
			this.miResetThumbnail.Size = new System.Drawing.Size(102, 22);
			this.miResetThumbnail.Text = "&Reset";
			this.miResetThumbnail.Click += new System.EventHandler(this.miResetThumbnail_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(99, 6);
			// 
			// labelCommunityRating
			// 
			this.labelCommunityRating.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelCommunityRating.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelCommunityRating.Location = new System.Drawing.Point(217, 313);
			this.labelCommunityRating.Name = "labelCommunityRating";
			this.labelCommunityRating.Size = new System.Drawing.Size(149, 20);
			this.labelCommunityRating.TabIndex = 7;
			this.labelCommunityRating.Text = "Community Rating:";
			this.labelCommunityRating.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txCommunityRating
			// 
			this.txCommunityRating.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.txCommunityRating.DrawText = true;
			this.txCommunityRating.Font = new System.Drawing.Font("Arial", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txCommunityRating.ForeColor = System.Drawing.SystemColors.GrayText;
			this.txCommunityRating.Location = new System.Drawing.Point(372, 312);
			this.txCommunityRating.Name = "txCommunityRating";
			this.txCommunityRating.Rating = 3F;
			this.txCommunityRating.RatingImage = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.StarBlue;
			this.txCommunityRating.Size = new System.Drawing.Size(152, 21);
			this.txCommunityRating.TabIndex = 8;
			this.txCommunityRating.Text = "3";
			// 
			// labelMyRating
			// 
			this.labelMyRating.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelMyRating.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelMyRating.Location = new System.Drawing.Point(219, 285);
			this.labelMyRating.Name = "labelMyRating";
			this.labelMyRating.Size = new System.Drawing.Size(147, 20);
			this.labelMyRating.TabIndex = 5;
			this.labelMyRating.Text = "My Rating:";
			this.labelMyRating.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// btLinkFile
			// 
			this.btLinkFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btLinkFile.Location = new System.Drawing.Point(372, 373);
			this.btLinkFile.Name = "btLinkFile";
			this.btLinkFile.Size = new System.Drawing.Size(152, 23);
			this.btLinkFile.TabIndex = 14;
			this.btLinkFile.Text = "Link to File...";
			this.btLinkFile.UseVisualStyleBackColor = true;
			this.btLinkFile.Visible = false;
			this.btLinkFile.Click += new System.EventHandler(this.btLinkFile_Click);
			// 
			// linkLabel
			// 
			this.linkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.linkLabel.LinkColor = System.Drawing.Color.SteelBlue;
			this.linkLabel.Location = new System.Drawing.Point(3, 416);
			this.linkLabel.Name = "linkLabel";
			this.linkLabel.Size = new System.Drawing.Size(560, 23);
			this.linkLabel.TabIndex = 12;
			this.linkLabel.TabStop = true;
			this.linkLabel.Text = "linkLabel";
			this.linkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.linkLabel.VisitedLinkColor = System.Drawing.Color.MediumOrchid;
			this.linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_LinkClicked);
			// 
			// coverThumbnail
			// 
			this.coverThumbnail.AllowDrop = true;
			this.coverThumbnail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.coverThumbnail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.coverThumbnail.Location = new System.Drawing.Point(21, 19);
			this.coverThumbnail.Name = "coverThumbnail";
			this.coverThumbnail.Size = new System.Drawing.Size(520, 243);
			this.coverThumbnail.TabIndex = 0;
			this.coverThumbnail.ThreeD = true;
			this.coverThumbnail.Tile = true;
			this.coverThumbnail.Click += new System.EventHandler(this.coverThumbnail_Click);
			this.coverThumbnail.DragDrop += new System.Windows.Forms.DragEventHandler(this.coverThumbnail_DragDrop);
			this.coverThumbnail.DragOver += new System.Windows.Forms.DragEventHandler(this.coverThumbnail_DragOver);
			// 
			// whereSeparator
			// 
			this.whereSeparator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.whereSeparator.BackColor = System.Drawing.SystemColors.ButtonShadow;
			this.whereSeparator.Location = new System.Drawing.Point(19, 360);
			this.whereSeparator.Name = "whereSeparator";
			this.whereSeparator.Size = new System.Drawing.Size(520, 1);
			this.whereSeparator.TabIndex = 9;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BackColor = System.Drawing.SystemColors.ButtonShadow;
			this.panel1.Location = new System.Drawing.Point(23, 268);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(520, 1);
			this.panel1.TabIndex = 0;
			// 
			// labelWhere
			// 
			this.labelWhere.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelWhere.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelWhere.Location = new System.Drawing.Point(19, 373);
			this.labelWhere.Name = "labelWhere";
			this.labelWhere.Size = new System.Drawing.Size(68, 17);
			this.labelWhere.TabIndex = 10;
			this.labelWhere.Text = "Where:";
			this.labelWhere.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelType
			// 
			this.labelType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelType.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelType.Location = new System.Drawing.Point(19, 285);
			this.labelType.Name = "labelType";
			this.labelType.Size = new System.Drawing.Size(68, 20);
			this.labelType.TabIndex = 1;
			this.labelType.Text = "Type:";
			this.labelType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txRating
			// 
			this.txRating.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.txRating.DrawText = true;
			this.txRating.Font = new System.Drawing.Font("Arial", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txRating.ForeColor = System.Drawing.SystemColors.GrayText;
			this.txRating.Location = new System.Drawing.Point(372, 285);
			this.txRating.Name = "txRating";
			this.txRating.Rating = 3F;
			this.txRating.RatingImage = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.StarYellow;
			this.txRating.Size = new System.Drawing.Size(152, 21);
			this.txRating.TabIndex = 6;
			this.txRating.Text = "3";
			// 
			// labelPages
			// 
			this.labelPages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelPages.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelPages.Location = new System.Drawing.Point(21, 313);
			this.labelPages.Name = "labelPages";
			this.labelPages.Size = new System.Drawing.Size(66, 20);
			this.labelPages.TabIndex = 3;
			this.labelPages.Text = "Pages:";
			this.labelPages.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblType
			// 
			this.lblType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblType.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblType.Location = new System.Drawing.Point(93, 272);
			this.lblType.Name = "lblType";
			this.lblType.Size = new System.Drawing.Size(160, 41);
			this.lblType.TabIndex = 2;
			this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblPath
			// 
			this.lblPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblPath.AutoEllipsis = true;
			this.lblPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPath.Location = new System.Drawing.Point(93, 373);
			this.lblPath.Name = "lblPath";
			this.lblPath.Size = new System.Drawing.Size(431, 35);
			this.lblPath.TabIndex = 11;
			this.lblPath.UseMnemonic = false;
			// 
			// lblPages
			// 
			this.lblPages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblPages.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPages.Location = new System.Drawing.Point(93, 313);
			this.lblPages.Name = "lblPages";
			this.lblPages.Size = new System.Drawing.Size(105, 20);
			this.lblPages.TabIndex = 4;
			this.lblPages.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tabDetails
			// 
			this.tabDetails.Controls.Add(this.labelTranslator);
			this.tabDetails.Controls.Add(this.txTranslator);
			this.tabDetails.Controls.Add(this.txDay);
			this.tabDetails.Controls.Add(this.labelDay);
			this.tabDetails.Controls.Add(this.labelSeriesGroup);
			this.tabDetails.Controls.Add(this.txSeriesGroup);
			this.tabDetails.Controls.Add(this.labelStoryArc);
			this.tabDetails.Controls.Add(this.txStoryArc);
			this.tabDetails.Controls.Add(this.cbSeriesComplete);
			this.tabDetails.Controls.Add(this.labelSeriesComplete);
			this.tabDetails.Controls.Add(this.cbEnableDynamicUpdate);
			this.tabDetails.Controls.Add(this.labelEnableDynamicUpdate);
			this.tabDetails.Controls.Add(this.txGenre);
			this.tabDetails.Controls.Add(this.labelTags);
			this.tabDetails.Controls.Add(this.cbEnableProposed);
			this.tabDetails.Controls.Add(this.txTags);
			this.tabDetails.Controls.Add(this.labelEnableProposed);
			this.tabDetails.Controls.Add(this.txVolume);
			this.tabDetails.Controls.Add(this.cbAgeRating);
			this.tabDetails.Controls.Add(this.txEditor);
			this.tabDetails.Controls.Add(this.labelEditor);
			this.tabDetails.Controls.Add(this.txMonth);
			this.tabDetails.Controls.Add(this.txYear);
			this.tabDetails.Controls.Add(this.labelAgeRating);
			this.tabDetails.Controls.Add(this.cbFormat);
			this.tabDetails.Controls.Add(this.txColorist);
			this.tabDetails.Controls.Add(this.txSeries);
			this.tabDetails.Controls.Add(this.labelFormat);
			this.tabDetails.Controls.Add(this.labelAlternateSeries);
			this.tabDetails.Controls.Add(this.txAlternateSeries);
			this.tabDetails.Controls.Add(this.cbImprint);
			this.tabDetails.Controls.Add(this.cbBlackAndWhite);
			this.tabDetails.Controls.Add(this.labelVolume);
			this.tabDetails.Controls.Add(this.txInker);
			this.tabDetails.Controls.Add(this.cbManga);
			this.tabDetails.Controls.Add(this.labelYear);
			this.tabDetails.Controls.Add(this.labelMonth);
			this.tabDetails.Controls.Add(this.labelBlackAndWhite);
			this.tabDetails.Controls.Add(this.txAlternateCount);
			this.tabDetails.Controls.Add(this.labelManga);
			this.tabDetails.Controls.Add(this.labelSeries);
			this.tabDetails.Controls.Add(this.labelLanguage);
			this.tabDetails.Controls.Add(this.labelImprint);
			this.tabDetails.Controls.Add(this.labelGenre);
			this.tabDetails.Controls.Add(this.labelColorist);
			this.tabDetails.Controls.Add(this.txCount);
			this.tabDetails.Controls.Add(this.cbLanguage);
			this.tabDetails.Controls.Add(this.cbPublisher);
			this.tabDetails.Controls.Add(this.txPenciller);
			this.tabDetails.Controls.Add(this.txAlternateNumber);
			this.tabDetails.Controls.Add(this.txNumber);
			this.tabDetails.Controls.Add(this.labelPublisher);
			this.tabDetails.Controls.Add(this.labelCoverArtist);
			this.tabDetails.Controls.Add(this.txCoverArtist);
			this.tabDetails.Controls.Add(this.labelInker);
			this.tabDetails.Controls.Add(this.labelAlternateCount);
			this.tabDetails.Controls.Add(this.txTitle);
			this.tabDetails.Controls.Add(this.labelCount);
			this.tabDetails.Controls.Add(this.labelAlternateNumber);
			this.tabDetails.Controls.Add(this.labelNumber);
			this.tabDetails.Controls.Add(this.labelLetterer);
			this.tabDetails.Controls.Add(this.labelPenciller);
			this.tabDetails.Controls.Add(this.txLetterer);
			this.tabDetails.Controls.Add(this.labelTitle);
			this.tabDetails.Controls.Add(this.labelWriter);
			this.tabDetails.Controls.Add(this.txWriter);
			this.tabDetails.Location = new System.Drawing.Point(4, 22);
			this.tabDetails.Name = "tabDetails";
			this.tabDetails.Padding = new System.Windows.Forms.Padding(3);
			this.tabDetails.Size = new System.Drawing.Size(566, 442);
			this.tabDetails.TabIndex = 1;
			this.tabDetails.Text = "Details";
			this.tabDetails.UseVisualStyleBackColor = true;
			// 
			// txDay
			// 
			this.txDay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txDay.Location = new System.Drawing.Point(349, 65);
			this.txDay.MaxLength = 4;
			this.txDay.Name = "txDay";
			this.txDay.PromptText = "";
			this.txDay.Size = new System.Drawing.Size(55, 20);
			this.txDay.TabIndex = 17;
			// 
			// labelDay
			// 
			this.labelDay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelDay.AutoSize = true;
			this.labelDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelDay.Location = new System.Drawing.Point(349, 51);
			this.labelDay.Name = "labelDay";
			this.labelDay.Size = new System.Drawing.Size(29, 12);
			this.labelDay.TabIndex = 16;
			this.labelDay.Text = "Day:";
			// 
			// labelSeriesGroup
			// 
			this.labelSeriesGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelSeriesGroup.AutoSize = true;
			this.labelSeriesGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSeriesGroup.Location = new System.Drawing.Point(214, 129);
			this.labelSeriesGroup.Name = "labelSeriesGroup";
			this.labelSeriesGroup.Size = new System.Drawing.Size(74, 12);
			this.labelSeriesGroup.TabIndex = 30;
			this.labelSeriesGroup.Text = "Series Group:";
			// 
			// txSeriesGroup
			// 
			this.txSeriesGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txSeriesGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txSeriesGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txSeriesGroup.Location = new System.Drawing.Point(216, 142);
			this.txSeriesGroup.Name = "txSeriesGroup";
			this.txSeriesGroup.PromptText = "";
			this.txSeriesGroup.Size = new System.Drawing.Size(188, 20);
			this.txSeriesGroup.TabIndex = 31;
			this.txSeriesGroup.Tag = "SeriesGroup";
			// 
			// labelStoryArc
			// 
			this.labelStoryArc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.labelStoryArc.AutoSize = true;
			this.labelStoryArc.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelStoryArc.Location = new System.Drawing.Point(8, 129);
			this.labelStoryArc.Name = "labelStoryArc";
			this.labelStoryArc.Size = new System.Drawing.Size(57, 12);
			this.labelStoryArc.TabIndex = 28;
			this.labelStoryArc.Text = "Story Arc:";
			// 
			// txStoryArc
			// 
			this.txStoryArc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.txStoryArc.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txStoryArc.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txStoryArc.Location = new System.Drawing.Point(10, 142);
			this.txStoryArc.Name = "txStoryArc";
			this.txStoryArc.PromptText = "";
			this.txStoryArc.Size = new System.Drawing.Size(200, 20);
			this.txStoryArc.TabIndex = 29;
			this.txStoryArc.Tag = "StoryArc";
			// 
			// cbSeriesComplete
			// 
			this.cbSeriesComplete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbSeriesComplete.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSeriesComplete.FormattingEnabled = true;
			this.cbSeriesComplete.Location = new System.Drawing.Point(416, 141);
			this.cbSeriesComplete.Name = "cbSeriesComplete";
			this.cbSeriesComplete.Size = new System.Drawing.Size(139, 21);
			this.cbSeriesComplete.TabIndex = 33;
			this.cbSeriesComplete.TextChanged += new System.EventHandler(this.IconTextsChanged);
			// 
			// labelSeriesComplete
			// 
			this.labelSeriesComplete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelSeriesComplete.AutoSize = true;
			this.labelSeriesComplete.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSeriesComplete.Location = new System.Drawing.Point(414, 129);
			this.labelSeriesComplete.Name = "labelSeriesComplete";
			this.labelSeriesComplete.Size = new System.Drawing.Size(90, 12);
			this.labelSeriesComplete.TabIndex = 32;
			this.labelSeriesComplete.Text = "Series complete:";
			// 
			// cbEnableDynamicUpdate
			// 
			this.cbEnableDynamicUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbEnableDynamicUpdate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbEnableDynamicUpdate.FormattingEnabled = true;
			this.cbEnableDynamicUpdate.Location = new System.Drawing.Point(416, 371);
			this.cbEnableDynamicUpdate.Name = "cbEnableDynamicUpdate";
			this.cbEnableDynamicUpdate.Size = new System.Drawing.Size(139, 21);
			this.cbEnableDynamicUpdate.TabIndex = 59;
			// 
			// labelEnableDynamicUpdate
			// 
			this.labelEnableDynamicUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelEnableDynamicUpdate.AutoSize = true;
			this.labelEnableDynamicUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelEnableDynamicUpdate.Location = new System.Drawing.Point(414, 357);
			this.labelEnableDynamicUpdate.Name = "labelEnableDynamicUpdate";
			this.labelEnableDynamicUpdate.Size = new System.Drawing.Size(103, 12);
			this.labelEnableDynamicUpdate.TabIndex = 58;
			this.labelEnableDynamicUpdate.Text = "Include in Updates:";
			// 
			// txGenre
			// 
			this.txGenre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.txGenre.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.txGenre.Location = new System.Drawing.Point(10, 372);
			this.txGenre.Name = "txGenre";
			this.txGenre.Size = new System.Drawing.Size(392, 20);
			this.txGenre.TabIndex = 57;
			// 
			// labelTags
			// 
			this.labelTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.labelTags.AutoSize = true;
			this.labelTags.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTags.Location = new System.Drawing.Point(11, 395);
			this.labelTags.Name = "labelTags";
			this.labelTags.Size = new System.Drawing.Size(33, 12);
			this.labelTags.TabIndex = 60;
			this.labelTags.Text = "Tags:";
			// 
			// cbEnableProposed
			// 
			this.cbEnableProposed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbEnableProposed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbEnableProposed.FormattingEnabled = true;
			this.cbEnableProposed.Location = new System.Drawing.Point(416, 410);
			this.cbEnableProposed.Name = "cbEnableProposed";
			this.cbEnableProposed.Size = new System.Drawing.Size(139, 21);
			this.cbEnableProposed.TabIndex = 63;
			this.cbEnableProposed.SelectedIndexChanged += new System.EventHandler(this.cbEnableShadowValues_SelectedIndexChanged);
			// 
			// txTags
			// 
			this.txTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.txTags.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.txTags.Location = new System.Drawing.Point(11, 411);
			this.txTags.Name = "txTags";
			this.txTags.Size = new System.Drawing.Size(392, 20);
			this.txTags.TabIndex = 61;
			this.txTags.TextChanged += new System.EventHandler(this.IconTextsChanged);
			// 
			// labelEnableProposed
			// 
			this.labelEnableProposed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelEnableProposed.AutoSize = true;
			this.labelEnableProposed.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelEnableProposed.Location = new System.Drawing.Point(414, 395);
			this.labelEnableProposed.Name = "labelEnableProposed";
			this.labelEnableProposed.Size = new System.Drawing.Size(94, 12);
			this.labelEnableProposed.TabIndex = 62;
			this.labelEnableProposed.Text = "Proposed Values:";
			// 
			// txVolume
			// 
			this.txVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txVolume.Location = new System.Drawing.Point(216, 28);
			this.txVolume.Name = "txVolume";
			this.txVolume.PromptText = "";
			this.txVolume.Size = new System.Drawing.Size(57, 20);
			this.txVolume.TabIndex = 3;
			// 
			// cbAgeRating
			// 
			this.cbAgeRating.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbAgeRating.FormattingEnabled = true;
			this.cbAgeRating.Location = new System.Drawing.Point(416, 200);
			this.cbAgeRating.Name = "cbAgeRating";
			this.cbAgeRating.Size = new System.Drawing.Size(139, 21);
			this.cbAgeRating.TabIndex = 39;
			this.cbAgeRating.TextChanged += new System.EventHandler(this.IconTextsChanged);
			// 
			// txEditor
			// 
			this.txEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.txEditor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txEditor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txEditor.Location = new System.Drawing.Point(11, 315);
			this.txEditor.Name = "txEditor";
			this.txEditor.Size = new System.Drawing.Size(200, 20);
			this.txEditor.TabIndex = 53;
			this.txEditor.Tag = "Editor";
			// 
			// labelEditor
			// 
			this.labelEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.labelEditor.AutoSize = true;
			this.labelEditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelEditor.Location = new System.Drawing.Point(9, 300);
			this.labelEditor.Name = "labelEditor";
			this.labelEditor.Size = new System.Drawing.Size(39, 12);
			this.labelEditor.TabIndex = 52;
			this.labelEditor.Text = "Editor:";
			// 
			// txMonth
			// 
			this.txMonth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txMonth.Location = new System.Drawing.Point(278, 66);
			this.txMonth.MaxLength = 2;
			this.txMonth.Name = "txMonth";
			this.txMonth.PromptText = "";
			this.txMonth.Size = new System.Drawing.Size(65, 20);
			this.txMonth.TabIndex = 15;
			// 
			// txYear
			// 
			this.txYear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txYear.Location = new System.Drawing.Point(216, 66);
			this.txYear.MaxLength = 4;
			this.txYear.Name = "txYear";
			this.txYear.PromptText = "";
			this.txYear.Size = new System.Drawing.Size(57, 20);
			this.txYear.TabIndex = 13;
			this.txYear.TextChanged += new System.EventHandler(this.IconTextsChanged);
			// 
			// labelAgeRating
			// 
			this.labelAgeRating.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelAgeRating.AutoSize = true;
			this.labelAgeRating.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelAgeRating.Location = new System.Drawing.Point(414, 187);
			this.labelAgeRating.Name = "labelAgeRating";
			this.labelAgeRating.Size = new System.Drawing.Size(65, 12);
			this.labelAgeRating.TabIndex = 38;
			this.labelAgeRating.Text = "Age Rating:";
			// 
			// cbFormat
			// 
			this.cbFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbFormat.FormattingEnabled = true;
			this.cbFormat.Location = new System.Drawing.Point(416, 27);
			this.cbFormat.Name = "cbFormat";
			this.cbFormat.PromptText = null;
			this.cbFormat.Size = new System.Drawing.Size(139, 21);
			this.cbFormat.TabIndex = 9;
			this.cbFormat.TextChanged += new System.EventHandler(this.IconTextsChanged);
			// 
			// txColorist
			// 
			this.txColorist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txColorist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txColorist.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txColorist.Location = new System.Drawing.Point(216, 239);
			this.txColorist.Name = "txColorist";
			this.txColorist.Size = new System.Drawing.Size(187, 20);
			this.txColorist.TabIndex = 43;
			this.txColorist.Tag = "Colorist";
			// 
			// txSeries
			// 
			this.txSeries.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.txSeries.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txSeries.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txSeries.Location = new System.Drawing.Point(11, 28);
			this.txSeries.Name = "txSeries";
			this.txSeries.PromptText = "";
			this.txSeries.Size = new System.Drawing.Size(201, 20);
			this.txSeries.TabIndex = 1;
			this.txSeries.Tag = "Series";
			// 
			// labelFormat
			// 
			this.labelFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelFormat.AutoSize = true;
			this.labelFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelFormat.Location = new System.Drawing.Point(414, 13);
			this.labelFormat.Name = "labelFormat";
			this.labelFormat.Size = new System.Drawing.Size(45, 12);
			this.labelFormat.TabIndex = 8;
			this.labelFormat.Text = "Format:";
			// 
			// labelAlternateSeries
			// 
			this.labelAlternateSeries.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.labelAlternateSeries.AutoSize = true;
			this.labelAlternateSeries.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelAlternateSeries.Location = new System.Drawing.Point(9, 89);
			this.labelAlternateSeries.Name = "labelAlternateSeries";
			this.labelAlternateSeries.Size = new System.Drawing.Size(177, 12);
			this.labelAlternateSeries.TabIndex = 20;
			this.labelAlternateSeries.Text = "Alternate Series or Storyline Title:";
			// 
			// txAlternateSeries
			// 
			this.txAlternateSeries.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.txAlternateSeries.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txAlternateSeries.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txAlternateSeries.Location = new System.Drawing.Point(11, 104);
			this.txAlternateSeries.Name = "txAlternateSeries";
			this.txAlternateSeries.PromptText = "";
			this.txAlternateSeries.Size = new System.Drawing.Size(262, 20);
			this.txAlternateSeries.TabIndex = 21;
			this.txAlternateSeries.Tag = "AlternateSeries";
			// 
			// cbImprint
			// 
			this.cbImprint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbImprint.FormattingEnabled = true;
			this.cbImprint.Location = new System.Drawing.Point(416, 103);
			this.cbImprint.Name = "cbImprint";
			this.cbImprint.Size = new System.Drawing.Size(139, 21);
			this.cbImprint.TabIndex = 27;
			this.cbImprint.Tag = "Imprint";
			this.cbImprint.TextChanged += new System.EventHandler(this.IconTextsChanged);
			// 
			// cbBlackAndWhite
			// 
			this.cbBlackAndWhite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbBlackAndWhite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbBlackAndWhite.FormattingEnabled = true;
			this.cbBlackAndWhite.Location = new System.Drawing.Point(416, 312);
			this.cbBlackAndWhite.Name = "cbBlackAndWhite";
			this.cbBlackAndWhite.Size = new System.Drawing.Size(139, 21);
			this.cbBlackAndWhite.TabIndex = 55;
			this.cbBlackAndWhite.TextChanged += new System.EventHandler(this.IconTextsChanged);
			// 
			// labelVolume
			// 
			this.labelVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelVolume.AutoSize = true;
			this.labelVolume.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelVolume.Location = new System.Drawing.Point(216, 13);
			this.labelVolume.Name = "labelVolume";
			this.labelVolume.Size = new System.Drawing.Size(47, 12);
			this.labelVolume.TabIndex = 2;
			this.labelVolume.Text = "Volume:";
			// 
			// txInker
			// 
			this.txInker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.txInker.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txInker.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txInker.Location = new System.Drawing.Point(10, 239);
			this.txInker.Name = "txInker";
			this.txInker.Size = new System.Drawing.Size(201, 20);
			this.txInker.TabIndex = 41;
			this.txInker.Tag = "Inker";
			// 
			// cbManga
			// 
			this.cbManga.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbManga.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbManga.FormattingEnabled = true;
			this.cbManga.Location = new System.Drawing.Point(416, 237);
			this.cbManga.Name = "cbManga";
			this.cbManga.Size = new System.Drawing.Size(139, 21);
			this.cbManga.TabIndex = 45;
			this.cbManga.TextChanged += new System.EventHandler(this.IconTextsChanged);
			// 
			// labelYear
			// 
			this.labelYear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelYear.AutoSize = true;
			this.labelYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelYear.Location = new System.Drawing.Point(216, 52);
			this.labelYear.Name = "labelYear";
			this.labelYear.Size = new System.Drawing.Size(31, 12);
			this.labelYear.TabIndex = 12;
			this.labelYear.Text = "Year:";
			// 
			// labelMonth
			// 
			this.labelMonth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelMonth.AutoSize = true;
			this.labelMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelMonth.Location = new System.Drawing.Point(276, 51);
			this.labelMonth.Name = "labelMonth";
			this.labelMonth.Size = new System.Drawing.Size(41, 12);
			this.labelMonth.TabIndex = 14;
			this.labelMonth.Text = "Month:";
			// 
			// labelBlackAndWhite
			// 
			this.labelBlackAndWhite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelBlackAndWhite.AutoSize = true;
			this.labelBlackAndWhite.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBlackAndWhite.Location = new System.Drawing.Point(414, 297);
			this.labelBlackAndWhite.Name = "labelBlackAndWhite";
			this.labelBlackAndWhite.Size = new System.Drawing.Size(90, 12);
			this.labelBlackAndWhite.TabIndex = 54;
			this.labelBlackAndWhite.Text = "Black and White:";
			// 
			// txAlternateCount
			// 
			this.txAlternateCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txAlternateCount.Location = new System.Drawing.Point(349, 104);
			this.txAlternateCount.Name = "txAlternateCount";
			this.txAlternateCount.PromptText = "";
			this.txAlternateCount.Size = new System.Drawing.Size(55, 20);
			this.txAlternateCount.TabIndex = 25;
			// 
			// labelManga
			// 
			this.labelManga.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelManga.AutoSize = true;
			this.labelManga.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelManga.Location = new System.Drawing.Point(414, 222);
			this.labelManga.Name = "labelManga";
			this.labelManga.Size = new System.Drawing.Size(43, 12);
			this.labelManga.TabIndex = 44;
			this.labelManga.Text = "Manga:";
			// 
			// labelSeries
			// 
			this.labelSeries.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.labelSeries.AutoSize = true;
			this.labelSeries.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSeries.Location = new System.Drawing.Point(9, 13);
			this.labelSeries.Name = "labelSeries";
			this.labelSeries.Size = new System.Drawing.Size(41, 12);
			this.labelSeries.TabIndex = 0;
			this.labelSeries.Text = "Series:";
			// 
			// labelLanguage
			// 
			this.labelLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelLanguage.AutoSize = true;
			this.labelLanguage.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelLanguage.Location = new System.Drawing.Point(414, 261);
			this.labelLanguage.Name = "labelLanguage";
			this.labelLanguage.Size = new System.Drawing.Size(57, 12);
			this.labelLanguage.TabIndex = 50;
			this.labelLanguage.Text = "Language:";
			// 
			// labelImprint
			// 
			this.labelImprint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelImprint.AutoSize = true;
			this.labelImprint.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelImprint.Location = new System.Drawing.Point(414, 88);
			this.labelImprint.Name = "labelImprint";
			this.labelImprint.Size = new System.Drawing.Size(45, 12);
			this.labelImprint.TabIndex = 26;
			this.labelImprint.Text = "Imprint:";
			// 
			// labelGenre
			// 
			this.labelGenre.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.labelGenre.AutoSize = true;
			this.labelGenre.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelGenre.Location = new System.Drawing.Point(11, 357);
			this.labelGenre.Name = "labelGenre";
			this.labelGenre.Size = new System.Drawing.Size(39, 12);
			this.labelGenre.TabIndex = 56;
			this.labelGenre.Text = "Genre:";
			// 
			// labelColorist
			// 
			this.labelColorist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelColorist.AutoSize = true;
			this.labelColorist.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelColorist.Location = new System.Drawing.Point(214, 224);
			this.labelColorist.Name = "labelColorist";
			this.labelColorist.Size = new System.Drawing.Size(49, 12);
			this.labelColorist.TabIndex = 42;
			this.labelColorist.Text = "Colorist:";
			// 
			// txCount
			// 
			this.txCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txCount.Location = new System.Drawing.Point(349, 28);
			this.txCount.Name = "txCount";
			this.txCount.PromptText = "";
			this.txCount.Size = new System.Drawing.Size(55, 20);
			this.txCount.TabIndex = 7;
			// 
			// cbLanguage
			// 
			this.cbLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbLanguage.CultureTypes = System.Globalization.CultureTypes.NeutralCultures;
			this.cbLanguage.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.cbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbLanguage.FormattingEnabled = true;
			this.cbLanguage.IntegralHeight = false;
			this.cbLanguage.Location = new System.Drawing.Point(416, 276);
			this.cbLanguage.Name = "cbLanguage";
			this.cbLanguage.SelectedCulture = "";
			this.cbLanguage.Size = new System.Drawing.Size(139, 21);
			this.cbLanguage.TabIndex = 51;
			this.cbLanguage.TopISOLanguages = null;
			// 
			// cbPublisher
			// 
			this.cbPublisher.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbPublisher.FormattingEnabled = true;
			this.cbPublisher.Location = new System.Drawing.Point(416, 65);
			this.cbPublisher.Name = "cbPublisher";
			this.cbPublisher.Size = new System.Drawing.Size(139, 21);
			this.cbPublisher.TabIndex = 19;
			this.cbPublisher.Tag = "Publisher";
			this.cbPublisher.TextChanged += new System.EventHandler(this.IconTextsChanged);
			// 
			// txPenciller
			// 
			this.txPenciller.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txPenciller.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txPenciller.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txPenciller.Location = new System.Drawing.Point(216, 200);
			this.txPenciller.Name = "txPenciller";
			this.txPenciller.Size = new System.Drawing.Size(187, 20);
			this.txPenciller.TabIndex = 37;
			this.txPenciller.Tag = "Penciller";
			// 
			// txAlternateNumber
			// 
			this.txAlternateNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txAlternateNumber.Location = new System.Drawing.Point(278, 104);
			this.txAlternateNumber.Name = "txAlternateNumber";
			this.txAlternateNumber.PromptText = "";
			this.txAlternateNumber.Size = new System.Drawing.Size(65, 20);
			this.txAlternateNumber.TabIndex = 23;
			// 
			// txNumber
			// 
			this.txNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txNumber.Location = new System.Drawing.Point(278, 28);
			this.txNumber.Name = "txNumber";
			this.txNumber.PromptText = "";
			this.txNumber.Size = new System.Drawing.Size(65, 20);
			this.txNumber.TabIndex = 5;
			// 
			// labelPublisher
			// 
			this.labelPublisher.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelPublisher.AutoSize = true;
			this.labelPublisher.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelPublisher.Location = new System.Drawing.Point(414, 51);
			this.labelPublisher.Name = "labelPublisher";
			this.labelPublisher.Size = new System.Drawing.Size(56, 12);
			this.labelPublisher.TabIndex = 18;
			this.labelPublisher.Text = "Publisher:";
			// 
			// labelCoverArtist
			// 
			this.labelCoverArtist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelCoverArtist.AutoSize = true;
			this.labelCoverArtist.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelCoverArtist.Location = new System.Drawing.Point(214, 262);
			this.labelCoverArtist.Name = "labelCoverArtist";
			this.labelCoverArtist.Size = new System.Drawing.Size(72, 12);
			this.labelCoverArtist.TabIndex = 48;
			this.labelCoverArtist.Text = "Cover Artist:";
			// 
			// txCoverArtist
			// 
			this.txCoverArtist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txCoverArtist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txCoverArtist.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txCoverArtist.Location = new System.Drawing.Point(216, 277);
			this.txCoverArtist.Name = "txCoverArtist";
			this.txCoverArtist.Size = new System.Drawing.Size(187, 20);
			this.txCoverArtist.TabIndex = 49;
			this.txCoverArtist.Tag = "CoverArtist";
			// 
			// labelInker
			// 
			this.labelInker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.labelInker.AutoSize = true;
			this.labelInker.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelInker.Location = new System.Drawing.Point(11, 224);
			this.labelInker.Name = "labelInker";
			this.labelInker.Size = new System.Drawing.Size(35, 12);
			this.labelInker.TabIndex = 40;
			this.labelInker.Text = "Inker:";
			// 
			// labelAlternateCount
			// 
			this.labelAlternateCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelAlternateCount.AutoSize = true;
			this.labelAlternateCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelAlternateCount.Location = new System.Drawing.Point(347, 89);
			this.labelAlternateCount.Name = "labelAlternateCount";
			this.labelAlternateCount.Size = new System.Drawing.Size(19, 12);
			this.labelAlternateCount.TabIndex = 24;
			this.labelAlternateCount.Text = "of:";
			// 
			// txTitle
			// 
			this.txTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.txTitle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txTitle.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txTitle.Location = new System.Drawing.Point(11, 66);
			this.txTitle.Name = "txTitle";
			this.txTitle.Size = new System.Drawing.Size(201, 20);
			this.txTitle.TabIndex = 11;
			this.txTitle.Tag = "Title";
			// 
			// labelCount
			// 
			this.labelCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelCount.AutoSize = true;
			this.labelCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelCount.Location = new System.Drawing.Point(347, 13);
			this.labelCount.Name = "labelCount";
			this.labelCount.Size = new System.Drawing.Size(19, 12);
			this.labelCount.TabIndex = 6;
			this.labelCount.Text = "of:";
			// 
			// labelAlternateNumber
			// 
			this.labelAlternateNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelAlternateNumber.AutoSize = true;
			this.labelAlternateNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelAlternateNumber.Location = new System.Drawing.Point(276, 89);
			this.labelAlternateNumber.Name = "labelAlternateNumber";
			this.labelAlternateNumber.Size = new System.Drawing.Size(48, 12);
			this.labelAlternateNumber.TabIndex = 22;
			this.labelAlternateNumber.Text = "Number:";
			// 
			// labelNumber
			// 
			this.labelNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelNumber.AutoSize = true;
			this.labelNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelNumber.Location = new System.Drawing.Point(276, 13);
			this.labelNumber.Name = "labelNumber";
			this.labelNumber.Size = new System.Drawing.Size(48, 12);
			this.labelNumber.TabIndex = 4;
			this.labelNumber.Text = "Number:";
			// 
			// labelLetterer
			// 
			this.labelLetterer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.labelLetterer.AutoSize = true;
			this.labelLetterer.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelLetterer.Location = new System.Drawing.Point(9, 262);
			this.labelLetterer.Name = "labelLetterer";
			this.labelLetterer.Size = new System.Drawing.Size(49, 12);
			this.labelLetterer.TabIndex = 46;
			this.labelLetterer.Text = "Letterer:";
			// 
			// labelPenciller
			// 
			this.labelPenciller.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelPenciller.AutoSize = true;
			this.labelPenciller.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelPenciller.Location = new System.Drawing.Point(214, 187);
			this.labelPenciller.Name = "labelPenciller";
			this.labelPenciller.Size = new System.Drawing.Size(53, 12);
			this.labelPenciller.TabIndex = 36;
			this.labelPenciller.Text = "Penciller:";
			// 
			// txLetterer
			// 
			this.txLetterer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.txLetterer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txLetterer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txLetterer.Location = new System.Drawing.Point(11, 277);
			this.txLetterer.Name = "txLetterer";
			this.txLetterer.Size = new System.Drawing.Size(200, 20);
			this.txLetterer.TabIndex = 47;
			this.txLetterer.Tag = "Letterer";
			// 
			// labelTitle
			// 
			this.labelTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.labelTitle.AutoSize = true;
			this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTitle.Location = new System.Drawing.Point(9, 51);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new System.Drawing.Size(31, 12);
			this.labelTitle.TabIndex = 10;
			this.labelTitle.Text = "Title:";
			// 
			// labelWriter
			// 
			this.labelWriter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.labelWriter.AutoSize = true;
			this.labelWriter.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelWriter.Location = new System.Drawing.Point(9, 187);
			this.labelWriter.Name = "labelWriter";
			this.labelWriter.Size = new System.Drawing.Size(40, 12);
			this.labelWriter.TabIndex = 34;
			this.labelWriter.Text = "Writer:";
			// 
			// txWriter
			// 
			this.txWriter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.txWriter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txWriter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txWriter.Location = new System.Drawing.Point(11, 200);
			this.txWriter.Name = "txWriter";
			this.txWriter.Size = new System.Drawing.Size(200, 20);
			this.txWriter.TabIndex = 35;
			this.txWriter.Tag = "Writer";
			// 
			// tabPlot
			// 
			this.tabPlot.Controls.Add(this.tabNotes);
			this.tabPlot.Controls.Add(this.txMainCharacterOrTeam);
			this.tabPlot.Controls.Add(this.labelMainCharacterOrTeam);
			this.tabPlot.Controls.Add(this.txScanInformation);
			this.tabPlot.Controls.Add(this.labelScanInformation);
			this.tabPlot.Controls.Add(this.txLocations);
			this.tabPlot.Controls.Add(this.labelLocations);
			this.tabPlot.Controls.Add(this.txTeams);
			this.tabPlot.Controls.Add(this.labelTeams);
			this.tabPlot.Controls.Add(this.txWeblink);
			this.tabPlot.Controls.Add(this.labelWeb);
			this.tabPlot.Controls.Add(this.txCharacters);
			this.tabPlot.Controls.Add(this.labelCharacters);
			this.tabPlot.Location = new System.Drawing.Point(4, 22);
			this.tabPlot.Name = "tabPlot";
			this.tabPlot.Size = new System.Drawing.Size(566, 442);
			this.tabPlot.TabIndex = 8;
			this.tabPlot.Text = "Plot & Notes";
			this.tabPlot.UseVisualStyleBackColor = true;
			// 
			// tabNotes
			// 
			this.tabNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.tabNotes.Controls.Add(this.tabPageSummary);
			this.tabNotes.Controls.Add(this.tabPageNotes);
			this.tabNotes.Controls.Add(this.tabPageReview);
			this.tabNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
			this.tabNotes.Location = new System.Drawing.Point(11, 15);
			this.tabNotes.Multiline = true;
			this.tabNotes.Name = "tabNotes";
			this.tabNotes.SelectedIndex = 0;
			this.tabNotes.Size = new System.Drawing.Size(539, 233);
			this.tabNotes.TabIndex = 0;
			// 
			// tabPageSummary
			// 
			this.tabPageSummary.Controls.Add(this.txSummary);
			this.tabPageSummary.Location = new System.Drawing.Point(4, 21);
			this.tabPageSummary.Name = "tabPageSummary";
			this.tabPageSummary.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageSummary.Size = new System.Drawing.Size(531, 208);
			this.tabPageSummary.TabIndex = 0;
			this.tabPageSummary.Text = "Summary";
			this.tabPageSummary.UseVisualStyleBackColor = true;
			// 
			// txSummary
			// 
			this.txSummary.AcceptsReturn = true;
			this.txSummary.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txSummary.FocusSelect = false;
			this.txSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.txSummary.Location = new System.Drawing.Point(3, 3);
			this.txSummary.Multiline = true;
			this.txSummary.Name = "txSummary";
			this.txSummary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txSummary.Size = new System.Drawing.Size(525, 202);
			this.txSummary.TabIndex = 2;
			// 
			// tabPageNotes
			// 
			this.tabPageNotes.Controls.Add(this.txNotes);
			this.tabPageNotes.Location = new System.Drawing.Point(4, 21);
			this.tabPageNotes.Name = "tabPageNotes";
			this.tabPageNotes.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageNotes.Size = new System.Drawing.Size(531, 208);
			this.tabPageNotes.TabIndex = 1;
			this.tabPageNotes.Text = "Notes";
			this.tabPageNotes.UseVisualStyleBackColor = true;
			// 
			// txNotes
			// 
			this.txNotes.AcceptsReturn = true;
			this.txNotes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txNotes.FocusSelect = false;
			this.txNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.txNotes.Location = new System.Drawing.Point(3, 3);
			this.txNotes.Multiline = true;
			this.txNotes.Name = "txNotes";
			this.txNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txNotes.Size = new System.Drawing.Size(525, 202);
			this.txNotes.TabIndex = 10;
			// 
			// tabPageReview
			// 
			this.tabPageReview.Controls.Add(this.txReview);
			this.tabPageReview.Location = new System.Drawing.Point(4, 21);
			this.tabPageReview.Name = "tabPageReview";
			this.tabPageReview.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageReview.Size = new System.Drawing.Size(531, 208);
			this.tabPageReview.TabIndex = 2;
			this.tabPageReview.Text = "Review";
			this.tabPageReview.UseVisualStyleBackColor = true;
			// 
			// txReview
			// 
			this.txReview.AcceptsReturn = true;
			this.txReview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txReview.FocusSelect = false;
			this.txReview.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.txReview.Location = new System.Drawing.Point(3, 3);
			this.txReview.Multiline = true;
			this.txReview.Name = "txReview";
			this.txReview.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txReview.Size = new System.Drawing.Size(525, 202);
			this.txReview.TabIndex = 10;
			// 
			// txMainCharacterOrTeam
			// 
			this.txMainCharacterOrTeam.AcceptsReturn = true;
			this.txMainCharacterOrTeam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.txMainCharacterOrTeam.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.txMainCharacterOrTeam.Location = new System.Drawing.Point(286, 278);
			this.txMainCharacterOrTeam.Name = "txMainCharacterOrTeam";
			this.txMainCharacterOrTeam.Size = new System.Drawing.Size(264, 20);
			this.txMainCharacterOrTeam.TabIndex = 4;
			this.txMainCharacterOrTeam.Tag = "Teams";
			// 
			// labelMainCharacterOrTeam
			// 
			this.labelMainCharacterOrTeam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelMainCharacterOrTeam.AutoSize = true;
			this.labelMainCharacterOrTeam.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelMainCharacterOrTeam.Location = new System.Drawing.Point(284, 263);
			this.labelMainCharacterOrTeam.Name = "labelMainCharacterOrTeam";
			this.labelMainCharacterOrTeam.Size = new System.Drawing.Size(130, 12);
			this.labelMainCharacterOrTeam.TabIndex = 3;
			this.labelMainCharacterOrTeam.Text = "Main Character or Team:";
			// 
			// txScanInformation
			// 
			this.txScanInformation.AcceptsReturn = true;
			this.txScanInformation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.txScanInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.txScanInformation.Location = new System.Drawing.Point(11, 412);
			this.txScanInformation.Name = "txScanInformation";
			this.txScanInformation.Size = new System.Drawing.Size(260, 20);
			this.txScanInformation.TabIndex = 10;
			this.txScanInformation.Tag = "";
			// 
			// labelScanInformation
			// 
			this.labelScanInformation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.labelScanInformation.AutoSize = true;
			this.labelScanInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelScanInformation.Location = new System.Drawing.Point(9, 397);
			this.labelScanInformation.Name = "labelScanInformation";
			this.labelScanInformation.Size = new System.Drawing.Size(95, 12);
			this.labelScanInformation.TabIndex = 9;
			this.labelScanInformation.Text = "Scan Information:";
			// 
			// txLocations
			// 
			this.txLocations.AcceptsReturn = true;
			this.txLocations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.txLocations.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.txLocations.Location = new System.Drawing.Point(287, 351);
			this.txLocations.Name = "txLocations";
			this.txLocations.Size = new System.Drawing.Size(263, 20);
			this.txLocations.TabIndex = 8;
			this.txLocations.Tag = "Locations";
			// 
			// labelLocations
			// 
			this.labelLocations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelLocations.AutoSize = true;
			this.labelLocations.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelLocations.Location = new System.Drawing.Point(285, 336);
			this.labelLocations.Name = "labelLocations";
			this.labelLocations.Size = new System.Drawing.Size(58, 12);
			this.labelLocations.TabIndex = 7;
			this.labelLocations.Text = "Locations:";
			// 
			// txTeams
			// 
			this.txTeams.AcceptsReturn = true;
			this.txTeams.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.txTeams.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.txTeams.Location = new System.Drawing.Point(286, 316);
			this.txTeams.Name = "txTeams";
			this.txTeams.Size = new System.Drawing.Size(264, 20);
			this.txTeams.TabIndex = 6;
			this.txTeams.Tag = "Teams";
			// 
			// labelTeams
			// 
			this.labelTeams.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelTeams.AutoSize = true;
			this.labelTeams.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTeams.Location = new System.Drawing.Point(284, 301);
			this.labelTeams.Name = "labelTeams";
			this.labelTeams.Size = new System.Drawing.Size(42, 12);
			this.labelTeams.TabIndex = 5;
			this.labelTeams.Text = "Teams:";
			// 
			// txWeblink
			// 
			this.txWeblink.AcceptsReturn = true;
			this.txWeblink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.txWeblink.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.txWeblink.Location = new System.Drawing.Point(287, 412);
			this.txWeblink.Name = "txWeblink";
			this.txWeblink.Size = new System.Drawing.Size(263, 20);
			this.txWeblink.TabIndex = 12;
			this.txWeblink.Tag = "";
			// 
			// labelWeb
			// 
			this.labelWeb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelWeb.AutoSize = true;
			this.labelWeb.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelWeb.Location = new System.Drawing.Point(285, 397);
			this.labelWeb.Name = "labelWeb";
			this.labelWeb.Size = new System.Drawing.Size(31, 12);
			this.labelWeb.TabIndex = 11;
			this.labelWeb.Text = "Web:";
			// 
			// txCharacters
			// 
			this.txCharacters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.txCharacters.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.txCharacters.Location = new System.Drawing.Point(11, 278);
			this.txCharacters.Multiline = true;
			this.txCharacters.Name = "txCharacters";
			this.txCharacters.Size = new System.Drawing.Size(260, 93);
			this.txCharacters.TabIndex = 2;
			this.txCharacters.Tag = "Characters";
			this.txCharacters.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txCharacters_KeyPress);
			// 
			// labelCharacters
			// 
			this.labelCharacters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.labelCharacters.AutoSize = true;
			this.labelCharacters.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelCharacters.Location = new System.Drawing.Point(9, 263);
			this.labelCharacters.Name = "labelCharacters";
			this.labelCharacters.Size = new System.Drawing.Size(65, 12);
			this.labelCharacters.TabIndex = 1;
			this.labelCharacters.Text = "Characters:";
			// 
			// tabCatalog
			// 
			this.tabCatalog.Controls.Add(this.labelReleasedTime);
			this.tabCatalog.Controls.Add(this.dtpReleasedTime);
			this.tabCatalog.Controls.Add(this.labelOpenedTime);
			this.tabCatalog.Controls.Add(this.dtpOpenedTime);
			this.tabCatalog.Controls.Add(this.labelAddedTime);
			this.tabCatalog.Controls.Add(this.dtpAddedTime);
			this.tabCatalog.Controls.Add(this.txPagesAsTextSimple);
			this.tabCatalog.Controls.Add(this.labelPagesAsTextSimple);
			this.tabCatalog.Controls.Add(this.txISBN);
			this.tabCatalog.Controls.Add(this.labelISBN);
			this.tabCatalog.Controls.Add(this.cbBookLocation);
			this.tabCatalog.Controls.Add(this.labelBookLocation);
			this.tabCatalog.Controls.Add(this.txCollectionStatus);
			this.tabCatalog.Controls.Add(this.cbBookPrice);
			this.tabCatalog.Controls.Add(this.labelBookPrice);
			this.tabCatalog.Controls.Add(this.txBookNotes);
			this.tabCatalog.Controls.Add(this.labelBookNotes);
			this.tabCatalog.Controls.Add(this.cbBookAge);
			this.tabCatalog.Controls.Add(this.labelBookAge);
			this.tabCatalog.Controls.Add(this.labelBookCollectionStatus);
			this.tabCatalog.Controls.Add(this.cbBookCondition);
			this.tabCatalog.Controls.Add(this.labelBookCondition);
			this.tabCatalog.Controls.Add(this.cbBookStore);
			this.tabCatalog.Controls.Add(this.labelBookStore);
			this.tabCatalog.Controls.Add(this.cbBookOwner);
			this.tabCatalog.Controls.Add(this.labelBookOwner);
			this.tabCatalog.Location = new System.Drawing.Point(4, 22);
			this.tabCatalog.Name = "tabCatalog";
			this.tabCatalog.Padding = new System.Windows.Forms.Padding(3);
			this.tabCatalog.Size = new System.Drawing.Size(566, 442);
			this.tabCatalog.TabIndex = 9;
			this.tabCatalog.Text = "Catalog";
			this.tabCatalog.UseVisualStyleBackColor = true;
			// 
			// labelReleasedTime
			// 
			this.labelReleasedTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelReleasedTime.AutoSize = true;
			this.labelReleasedTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelReleasedTime.Location = new System.Drawing.Point(314, 21);
			this.labelReleasedTime.Name = "labelReleasedTime";
			this.labelReleasedTime.Size = new System.Drawing.Size(56, 12);
			this.labelReleasedTime.TabIndex = 12;
			this.labelReleasedTime.Text = "Released:";
			// 
			// dtpReleasedTime
			// 
			this.dtpReleasedTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.dtpReleasedTime.CustomFormat = " ";
			this.dtpReleasedTime.Location = new System.Drawing.Point(315, 36);
			this.dtpReleasedTime.Name = "dtpReleasedTime";
			this.dtpReleasedTime.Size = new System.Drawing.Size(235, 20);
			this.dtpReleasedTime.TabIndex = 13;
			this.dtpReleasedTime.Value = new System.DateTime(((long)(0)));
			// 
			// labelOpenedTime
			// 
			this.labelOpenedTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelOpenedTime.AutoSize = true;
			this.labelOpenedTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelOpenedTime.Location = new System.Drawing.Point(315, 98);
			this.labelOpenedTime.Name = "labelOpenedTime";
			this.labelOpenedTime.Size = new System.Drawing.Size(77, 12);
			this.labelOpenedTime.TabIndex = 16;
			this.labelOpenedTime.Text = "Opened/Read:";
			// 
			// dtpOpenedTime
			// 
			this.dtpOpenedTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.dtpOpenedTime.CustomFormat = " ";
			this.dtpOpenedTime.Location = new System.Drawing.Point(316, 113);
			this.dtpOpenedTime.Name = "dtpOpenedTime";
			this.dtpOpenedTime.Size = new System.Drawing.Size(234, 20);
			this.dtpOpenedTime.TabIndex = 17;
			this.dtpOpenedTime.Value = new System.DateTime(((long)(0)));
			// 
			// labelAddedTime
			// 
			this.labelAddedTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelAddedTime.AutoSize = true;
			this.labelAddedTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelAddedTime.Location = new System.Drawing.Point(314, 58);
			this.labelAddedTime.Name = "labelAddedTime";
			this.labelAddedTime.Size = new System.Drawing.Size(98, 12);
			this.labelAddedTime.TabIndex = 14;
			this.labelAddedTime.Text = "Added/Purchased:";
			// 
			// dtpAddedTime
			// 
			this.dtpAddedTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.dtpAddedTime.CustomFormat = " ";
			this.dtpAddedTime.Location = new System.Drawing.Point(315, 73);
			this.dtpAddedTime.Name = "dtpAddedTime";
			this.dtpAddedTime.Size = new System.Drawing.Size(235, 20);
			this.dtpAddedTime.TabIndex = 15;
			this.dtpAddedTime.Value = new System.DateTime(((long)(0)));
			// 
			// txPagesAsTextSimple
			// 
			this.txPagesAsTextSimple.AcceptsReturn = true;
			this.txPagesAsTextSimple.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txPagesAsTextSimple.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.txPagesAsTextSimple.Location = new System.Drawing.Point(167, 73);
			this.txPagesAsTextSimple.Name = "txPagesAsTextSimple";
			this.txPagesAsTextSimple.Size = new System.Drawing.Size(126, 20);
			this.txPagesAsTextSimple.TabIndex = 7;
			this.txPagesAsTextSimple.Tag = "PageCountTextSimple";
			// 
			// labelPagesAsTextSimple
			// 
			this.labelPagesAsTextSimple.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelPagesAsTextSimple.AutoSize = true;
			this.labelPagesAsTextSimple.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelPagesAsTextSimple.Location = new System.Drawing.Point(165, 58);
			this.labelPagesAsTextSimple.Name = "labelPagesAsTextSimple";
			this.labelPagesAsTextSimple.Size = new System.Drawing.Size(40, 12);
			this.labelPagesAsTextSimple.TabIndex = 6;
			this.labelPagesAsTextSimple.Text = "Pages:";
			// 
			// txISBN
			// 
			this.txISBN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.txISBN.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.txISBN.Location = new System.Drawing.Point(11, 73);
			this.txISBN.Name = "txISBN";
			this.txISBN.Size = new System.Drawing.Size(150, 20);
			this.txISBN.TabIndex = 5;
			this.txISBN.Tag = "ISBN";
			// 
			// labelISBN
			// 
			this.labelISBN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.labelISBN.AutoSize = true;
			this.labelISBN.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelISBN.Location = new System.Drawing.Point(9, 58);
			this.labelISBN.Name = "labelISBN";
			this.labelISBN.Size = new System.Drawing.Size(35, 12);
			this.labelISBN.TabIndex = 4;
			this.labelISBN.Text = "ISBN:";
			// 
			// cbBookLocation
			// 
			this.cbBookLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbBookLocation.FormattingEnabled = true;
			this.cbBookLocation.Location = new System.Drawing.Point(317, 168);
			this.cbBookLocation.Name = "cbBookLocation";
			this.cbBookLocation.Size = new System.Drawing.Size(233, 21);
			this.cbBookLocation.TabIndex = 21;
			this.cbBookLocation.Tag = "BookLocation";
			// 
			// labelBookLocation
			// 
			this.labelBookLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelBookLocation.AutoSize = true;
			this.labelBookLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBookLocation.Location = new System.Drawing.Point(315, 154);
			this.labelBookLocation.Name = "labelBookLocation";
			this.labelBookLocation.Size = new System.Drawing.Size(80, 12);
			this.labelBookLocation.TabIndex = 20;
			this.labelBookLocation.Text = "Book Location:";
			// 
			// txCollectionStatus
			// 
			this.txCollectionStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.txCollectionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.txCollectionStatus.Location = new System.Drawing.Point(11, 210);
			this.txCollectionStatus.Name = "txCollectionStatus";
			this.txCollectionStatus.Size = new System.Drawing.Size(539, 20);
			this.txCollectionStatus.TabIndex = 23;
			this.txCollectionStatus.Tag = "CollectionStatus";
			// 
			// cbBookPrice
			// 
			this.cbBookPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbBookPrice.FormattingEnabled = true;
			this.cbBookPrice.Location = new System.Drawing.Point(167, 34);
			this.cbBookPrice.Name = "cbBookPrice";
			this.cbBookPrice.Size = new System.Drawing.Size(126, 21);
			this.cbBookPrice.TabIndex = 3;
			this.cbBookPrice.Tag = "BookPrice";
			// 
			// labelBookPrice
			// 
			this.labelBookPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelBookPrice.AutoSize = true;
			this.labelBookPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBookPrice.Location = new System.Drawing.Point(165, 19);
			this.labelBookPrice.Name = "labelBookPrice";
			this.labelBookPrice.Size = new System.Drawing.Size(35, 12);
			this.labelBookPrice.TabIndex = 2;
			this.labelBookPrice.Text = "Price:";
			// 
			// txBookNotes
			// 
			this.txBookNotes.AcceptsReturn = true;
			this.txBookNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left)| System.Windows.Forms.AnchorStyles.Right)));
			this.txBookNotes.FocusSelect = false;
			this.txBookNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.txBookNotes.Location = new System.Drawing.Point(11, 265);
			this.txBookNotes.Multiline = true;
			this.txBookNotes.Name = "txBookNotes";
			this.txBookNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txBookNotes.Size = new System.Drawing.Size(539, 157);
			this.txBookNotes.TabIndex = 25;
			this.txBookNotes.Tag = "BookNotes";
			// 
			// labelBookNotes
			// 
			this.labelBookNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.labelBookNotes.AutoSize = true;
			this.labelBookNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBookNotes.Location = new System.Drawing.Point(9, 250);
			this.labelBookNotes.Name = "labelBookNotes";
			this.labelBookNotes.Size = new System.Drawing.Size(120, 12);
			this.labelBookNotes.TabIndex = 24;
			this.labelBookNotes.Text = "Notes about this Book:";
			// 
			// cbBookAge
			// 
			this.cbBookAge.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.cbBookAge.FormattingEnabled = true;
			this.cbBookAge.Location = new System.Drawing.Point(11, 112);
			this.cbBookAge.Name = "cbBookAge";
			this.cbBookAge.Size = new System.Drawing.Size(150, 21);
			this.cbBookAge.TabIndex = 9;
			this.cbBookAge.Tag = "BookAge";
			// 
			// labelBookAge
			// 
			this.labelBookAge.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.labelBookAge.AutoSize = true;
			this.labelBookAge.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBookAge.Location = new System.Drawing.Point(9, 97);
			this.labelBookAge.Name = "labelBookAge";
			this.labelBookAge.Size = new System.Drawing.Size(29, 12);
			this.labelBookAge.TabIndex = 8;
			this.labelBookAge.Text = "Age:";
			// 
			// labelBookCollectionStatus
			// 
			this.labelBookCollectionStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.labelBookCollectionStatus.AutoSize = true;
			this.labelBookCollectionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBookCollectionStatus.Location = new System.Drawing.Point(9, 195);
			this.labelBookCollectionStatus.Name = "labelBookCollectionStatus";
			this.labelBookCollectionStatus.Size = new System.Drawing.Size(96, 12);
			this.labelBookCollectionStatus.TabIndex = 22;
			this.labelBookCollectionStatus.Text = "Collection Status:";
			// 
			// cbBookCondition
			// 
			this.cbBookCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbBookCondition.FormattingEnabled = true;
			this.cbBookCondition.Location = new System.Drawing.Point(167, 112);
			this.cbBookCondition.Name = "cbBookCondition";
			this.cbBookCondition.Size = new System.Drawing.Size(126, 21);
			this.cbBookCondition.TabIndex = 11;
			this.cbBookCondition.Tag = "BookCondition";
			// 
			// labelBookCondition
			// 
			this.labelBookCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelBookCondition.AutoSize = true;
			this.labelBookCondition.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBookCondition.Location = new System.Drawing.Point(165, 98);
			this.labelBookCondition.Name = "labelBookCondition";
			this.labelBookCondition.Size = new System.Drawing.Size(57, 12);
			this.labelBookCondition.TabIndex = 10;
			this.labelBookCondition.Text = "Condition:";
			// 
			// cbBookStore
			// 
			this.cbBookStore.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.cbBookStore.FormattingEnabled = true;
			this.cbBookStore.Location = new System.Drawing.Point(11, 35);
			this.cbBookStore.Name = "cbBookStore";
			this.cbBookStore.PromptText = null;
			this.cbBookStore.Size = new System.Drawing.Size(150, 21);
			this.cbBookStore.TabIndex = 1;
			this.cbBookStore.Tag = "BookStore";
			// 
			// labelBookStore
			// 
			this.labelBookStore.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.labelBookStore.AutoSize = true;
			this.labelBookStore.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBookStore.Location = new System.Drawing.Point(11, 21);
			this.labelBookStore.Name = "labelBookStore";
			this.labelBookStore.Size = new System.Drawing.Size(36, 12);
			this.labelBookStore.TabIndex = 0;
			this.labelBookStore.Text = "Store:";
			// 
			// cbBookOwner
			// 
			this.cbBookOwner.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.cbBookOwner.FormattingEnabled = true;
			this.cbBookOwner.Location = new System.Drawing.Point(11, 168);
			this.cbBookOwner.Name = "cbBookOwner";
			this.cbBookOwner.Size = new System.Drawing.Size(282, 21);
			this.cbBookOwner.TabIndex = 19;
			this.cbBookOwner.Tag = "BookOwner";
			// 
			// labelBookOwner
			// 
			this.labelBookOwner.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.labelBookOwner.AutoSize = true;
			this.labelBookOwner.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBookOwner.Location = new System.Drawing.Point(9, 154);
			this.labelBookOwner.Name = "labelBookOwner";
			this.labelBookOwner.Size = new System.Drawing.Size(42, 12);
			this.labelBookOwner.TabIndex = 18;
			this.labelBookOwner.Text = "Owner:";
			// 
			// tabCustom
			// 
			this.tabCustom.Controls.Add(this.customValuesData);
			this.tabCustom.Location = new System.Drawing.Point(4, 22);
			this.tabCustom.Name = "tabCustom";
			this.tabCustom.Padding = new System.Windows.Forms.Padding(3);
			this.tabCustom.Size = new System.Drawing.Size(566, 442);
			this.tabCustom.TabIndex = 10;
			this.tabCustom.Text = "Custom";
			this.tabCustom.UseVisualStyleBackColor = true;
			// 
			// customValuesData
			// 
			this.customValuesData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.customValuesData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.customValuesData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
			this.CustomValueName,
			this.CustomValueValue});
			this.customValuesData.Location = new System.Drawing.Point(11, 16);
			this.customValuesData.Name = "customValuesData";
			this.customValuesData.Size = new System.Drawing.Size(537, 410);
			this.customValuesData.TabIndex = 1;
			this.customValuesData.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.customValuesData_EditingControlShowing);
			// 
			// CustomValueName
			// 
			this.CustomValueName.HeaderText = "Name";
			this.CustomValueName.Name = "CustomValueName";
			this.CustomValueName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// CustomValueValue
			// 
			this.CustomValueValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.CustomValueValue.HeaderText = "Value";
			this.CustomValueValue.Name = "CustomValueValue";
			// 
			// tabPages
			// 
			this.tabPages.Controls.Add(this.btResetPages);
			this.tabPages.Controls.Add(this.btPageView);
			this.tabPages.Controls.Add(this.labelPagesInfo);
			this.tabPages.Controls.Add(this.pagesView);
			this.tabPages.Location = new System.Drawing.Point(4, 22);
			this.tabPages.Name = "tabPages";
			this.tabPages.Size = new System.Drawing.Size(566, 442);
			this.tabPages.TabIndex = 6;
			this.tabPages.Text = "Pages";
			this.tabPages.UseVisualStyleBackColor = true;
			// 
			// btResetPages
			// 
			this.btResetPages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btResetPages.ContextMenuStrip = this.cmResetPages;
			this.btResetPages.Location = new System.Drawing.Point(441, 407);
			this.btResetPages.Name = "btResetPages";
			this.btResetPages.Size = new System.Drawing.Size(111, 23);
			this.btResetPages.TabIndex = 14;
			this.btResetPages.Text = "Reset";
			this.btResetPages.UseVisualStyleBackColor = true;
			this.btResetPages.ShowContextMenu += new System.EventHandler(this.btResetPages_ShowContextMenu);
			this.btResetPages.Click += new System.EventHandler(this.btResetPages_Click);
			// 
			// cmResetPages
			// 
			this.cmResetPages.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.miOrderByName,
			this.miOrderByNameNumeric});
			this.cmResetPages.Name = "cmResetPages";
			this.cmResetPages.Size = new System.Drawing.Size(211, 48);
			this.cmResetPages.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmResetPages_ItemClicked);
			// 
			// miOrderByName
			// 
			this.miOrderByName.Name = "miOrderByName";
			this.miOrderByName.Size = new System.Drawing.Size(210, 22);
			this.miOrderByName.Text = "Order by Name";
			// 
			// miOrderByNameNumeric
			// 
			this.miOrderByNameNumeric.Name = "miOrderByNameNumeric";
			this.miOrderByNameNumeric.Size = new System.Drawing.Size(210, 22);
			this.miOrderByNameNumeric.Text = "Order by Name (numeric)";
			// 
			// btPageView
			// 
			this.btPageView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btPageView.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.SmallArrowDown;
			this.btPageView.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btPageView.Location = new System.Drawing.Point(479, 9);
			this.btPageView.Name = "btPageView";
			this.btPageView.Size = new System.Drawing.Size(73, 23);
			this.btPageView.TabIndex = 2;
			this.btPageView.Text = "Pages";
			this.btPageView.UseVisualStyleBackColor = true;
			this.btPageView.Click += new System.EventHandler(this.btPageViews_Click);
			// 
			// labelPagesInfo
			// 
			this.labelPagesInfo.AutoSize = true;
			this.labelPagesInfo.Location = new System.Drawing.Point(8, 14);
			this.labelPagesInfo.Name = "labelPagesInfo";
			this.labelPagesInfo.Size = new System.Drawing.Size(421, 13);
			this.labelPagesInfo.TabIndex = 0;
			this.labelPagesInfo.Text = "Change the page order with drag & drop or use the context menu to change page typ" +
	"es:";
			this.labelPagesInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelPagesInfo.UseMnemonic = false;
			// 
			// pagesView
			// 
			this.pagesView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.pagesView.Bookmark = null;
			this.pagesView.CreateBackdrop = false;
			this.pagesView.Location = new System.Drawing.Point(11, 38);
			this.pagesView.Name = "pagesView";
			this.pagesView.Size = new System.Drawing.Size(541, 363);
			this.pagesView.TabIndex = 1;
			// 
			// tabColors
			// 
			this.tabColors.Controls.Add(this.panelImage);
			this.tabColors.Controls.Add(this.panelImageControls);
			this.tabColors.Location = new System.Drawing.Point(4, 22);
			this.tabColors.Name = "tabColors";
			this.tabColors.Size = new System.Drawing.Size(566, 442);
			this.tabColors.TabIndex = 7;
			this.tabColors.Text = "Colors";
			this.tabColors.UseVisualStyleBackColor = true;
			// 
			// panelImage
			// 
			this.panelImage.Controls.Add(this.labelCurrentPage);
			this.panelImage.Controls.Add(this.chkShowImageControls);
			this.panelImage.Controls.Add(this.btLastPage);
			this.panelImage.Controls.Add(this.btFirstPage);
			this.panelImage.Controls.Add(this.btNextPage);
			this.panelImage.Controls.Add(this.btPrevPage);
			this.panelImage.Controls.Add(this.pageViewer);
			this.panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelImage.Location = new System.Drawing.Point(0, 0);
			this.panelImage.Name = "panelImage";
			this.panelImage.Size = new System.Drawing.Size(566, 317);
			this.panelImage.TabIndex = 12;
			// 
			// labelCurrentPage
			// 
			this.labelCurrentPage.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.labelCurrentPage.Location = new System.Drawing.Point(184, 291);
			this.labelCurrentPage.Name = "labelCurrentPage";
			this.labelCurrentPage.Size = new System.Drawing.Size(202, 21);
			this.labelCurrentPage.TabIndex = 6;
			this.labelCurrentPage.Text = "Page Text";
			this.labelCurrentPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// chkShowImageControls
			// 
			this.chkShowImageControls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.chkShowImageControls.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkShowImageControls.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.DoubleArrow;
			this.chkShowImageControls.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkShowImageControls.Location = new System.Drawing.Point(416, 291);
			this.chkShowImageControls.Name = "chkShowImageControls";
			this.chkShowImageControls.Size = new System.Drawing.Size(140, 23);
			this.chkShowImageControls.TabIndex = 5;
			this.chkShowImageControls.Text = "Image Control";
			this.chkShowImageControls.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkShowImageControls.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.chkShowImageControls.UseVisualStyleBackColor = true;
			this.chkShowImageControls.CheckedChanged += new System.EventHandler(this.chkShowColorControls_CheckedChanged);
			// 
			// btLastPage
			// 
			this.btLastPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btLastPage.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GoLast;
			this.btLastPage.Location = new System.Drawing.Point(107, 290);
			this.btLastPage.Name = "btLastPage";
			this.btLastPage.Size = new System.Drawing.Size(32, 23);
			this.btLastPage.TabIndex = 4;
			this.btLastPage.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.btLastPage.UseVisualStyleBackColor = true;
			this.btLastPage.Click += new System.EventHandler(this.btLastPage_Click);
			// 
			// btFirstPage
			// 
			this.btFirstPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btFirstPage.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GoFirst;
			this.btFirstPage.Location = new System.Drawing.Point(11, 290);
			this.btFirstPage.Name = "btFirstPage";
			this.btFirstPage.Size = new System.Drawing.Size(32, 23);
			this.btFirstPage.TabIndex = 3;
			this.btFirstPage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btFirstPage.UseVisualStyleBackColor = true;
			this.btFirstPage.Click += new System.EventHandler(this.btFirstPage_Click);
			// 
			// btNextPage
			// 
			this.btNextPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btNextPage.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GoNext;
			this.btNextPage.Location = new System.Drawing.Point(75, 290);
			this.btNextPage.Name = "btNextPage";
			this.btNextPage.Size = new System.Drawing.Size(32, 23);
			this.btNextPage.TabIndex = 2;
			this.btNextPage.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.btNextPage.UseVisualStyleBackColor = true;
			this.btNextPage.Click += new System.EventHandler(this.btNextPage_Click);
			// 
			// btPrevPage
			// 
			this.btPrevPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btPrevPage.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GoPrevious;
			this.btPrevPage.Location = new System.Drawing.Point(43, 290);
			this.btPrevPage.Name = "btPrevPage";
			this.btPrevPage.Size = new System.Drawing.Size(32, 23);
			this.btPrevPage.TabIndex = 1;
			this.btPrevPage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btPrevPage.UseVisualStyleBackColor = true;
			this.btPrevPage.Click += new System.EventHandler(this.btPrevPage_Click);
			// 
			// pageViewer
			// 
			this.pageViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.pageViewer.AutoScrollMode = cYo.Common.Windows.Forms.AutoScrollMode.Pan;
			this.pageViewer.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.pageViewer.ForeColor = System.Drawing.Color.White;
			this.pageViewer.Location = new System.Drawing.Point(11, 12);
			this.pageViewer.Name = "pageViewer";
			this.pageViewer.ScaleMode = cYo.Common.Drawing.ScaleMode.FitWidth;
			this.pageViewer.Size = new System.Drawing.Size(545, 275);
			this.pageViewer.TabIndex = 0;
			this.pageViewer.Text = "Double Click on Color to set White Point";
			this.pageViewer.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
			this.pageViewer.VisibleChanged += new System.EventHandler(this.pageViewer_VisibleChanged);
			this.pageViewer.DoubleClick += new System.EventHandler(this.pageViewer_DoubleClick);
			// 
			// panelImageControls
			// 
			this.panelImageControls.Controls.Add(this.labelSaturation);
			this.panelImageControls.Controls.Add(this.labelContrast);
			this.panelImageControls.Controls.Add(this.tbGamma);
			this.panelImageControls.Controls.Add(this.tbSaturation);
			this.panelImageControls.Controls.Add(this.labelGamma);
			this.panelImageControls.Controls.Add(this.tbBrightness);
			this.panelImageControls.Controls.Add(this.tbSharpening);
			this.panelImageControls.Controls.Add(this.tbContrast);
			this.panelImageControls.Controls.Add(this.labelSharpening);
			this.panelImageControls.Controls.Add(this.labelBrightness);
			this.panelImageControls.Controls.Add(this.btResetColors);
			this.panelImageControls.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelImageControls.Location = new System.Drawing.Point(0, 317);
			this.panelImageControls.Name = "panelImageControls";
			this.panelImageControls.Size = new System.Drawing.Size(566, 125);
			this.panelImageControls.TabIndex = 13;
			this.panelImageControls.Visible = false;
			// 
			// labelSaturation
			// 
			this.labelSaturation.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelSaturation.AutoSize = true;
			this.labelSaturation.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSaturation.Location = new System.Drawing.Point(11, 34);
			this.labelSaturation.Name = "labelSaturation";
			this.labelSaturation.Size = new System.Drawing.Size(57, 12);
			this.labelSaturation.TabIndex = 1;
			this.labelSaturation.Text = "Saturation";
			// 
			// labelContrast
			// 
			this.labelContrast.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelContrast.AutoSize = true;
			this.labelContrast.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
			this.labelContrast.Location = new System.Drawing.Point(296, 34);
			this.labelContrast.Name = "labelContrast";
			this.labelContrast.Size = new System.Drawing.Size(49, 12);
			this.labelContrast.TabIndex = 5;
			this.labelContrast.Text = "Contrast";
			// 
			// tbGamma
			// 
			this.tbGamma.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.tbGamma.Location = new System.Drawing.Point(363, 52);
			this.tbGamma.Minimum = -100;
			this.tbGamma.Name = "tbGamma";
			this.tbGamma.Size = new System.Drawing.Size(192, 18);
			this.tbGamma.TabIndex = 8;
			this.tbGamma.ThumbSize = new System.Drawing.Size(8, 16);
			this.tbGamma.TickFrequency = 16;
			this.tbGamma.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			this.tbGamma.Scroll += new System.EventHandler(this.ColorAdjustment_Scroll);
			this.tbGamma.ValueChanged += new System.EventHandler(this.AdjustmentSliderChanged);
			// 
			// tbSaturation
			// 
			this.tbSaturation.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.tbSaturation.Location = new System.Drawing.Point(78, 28);
			this.tbSaturation.Minimum = -100;
			this.tbSaturation.Name = "tbSaturation";
			this.tbSaturation.Size = new System.Drawing.Size(192, 18);
			this.tbSaturation.TabIndex = 2;
			this.tbSaturation.ThumbSize = new System.Drawing.Size(8, 16);
			this.tbSaturation.TickFrequency = 16;
			this.tbSaturation.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			this.tbSaturation.Scroll += new System.EventHandler(this.ColorAdjustment_Scroll);
			this.tbSaturation.ValueChanged += new System.EventHandler(this.AdjustmentSliderChanged);
			// 
			// labelGamma
			// 
			this.labelGamma.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelGamma.AutoSize = true;
			this.labelGamma.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
			this.labelGamma.Location = new System.Drawing.Point(296, 58);
			this.labelGamma.Name = "labelGamma";
			this.labelGamma.Size = new System.Drawing.Size(43, 12);
			this.labelGamma.TabIndex = 7;
			this.labelGamma.Text = "Gamma";
			// 
			// tbBrightness
			// 
			this.tbBrightness.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.tbBrightness.Location = new System.Drawing.Point(78, 52);
			this.tbBrightness.Minimum = -100;
			this.tbBrightness.Name = "tbBrightness";
			this.tbBrightness.Size = new System.Drawing.Size(192, 18);
			this.tbBrightness.TabIndex = 4;
			this.tbBrightness.Text = "tbBrightness";
			this.tbBrightness.ThumbSize = new System.Drawing.Size(8, 16);
			this.tbBrightness.TickFrequency = 16;
			this.tbBrightness.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			this.tbBrightness.Scroll += new System.EventHandler(this.ColorAdjustment_Scroll);
			this.tbBrightness.ValueChanged += new System.EventHandler(this.AdjustmentSliderChanged);
			// 
			// tbSharpening
			// 
			this.tbSharpening.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.tbSharpening.LargeChange = 1;
			this.tbSharpening.Location = new System.Drawing.Point(81, 86);
			this.tbSharpening.Maximum = 3;
			this.tbSharpening.Name = "tbSharpening";
			this.tbSharpening.Size = new System.Drawing.Size(189, 18);
			this.tbSharpening.TabIndex = 10;
			this.tbSharpening.Text = "tbSaturation";
			this.tbSharpening.ThumbSize = new System.Drawing.Size(8, 16);
			this.tbSharpening.TickFrequency = 1;
			this.tbSharpening.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			this.tbSharpening.Scroll += new System.EventHandler(this.ColorAdjustment_Scroll);
			// 
			// tbContrast
			// 
			this.tbContrast.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.tbContrast.Location = new System.Drawing.Point(363, 28);
			this.tbContrast.Minimum = -100;
			this.tbContrast.Name = "tbContrast";
			this.tbContrast.Size = new System.Drawing.Size(192, 18);
			this.tbContrast.TabIndex = 6;
			this.tbContrast.Text = "tbSaturation";
			this.tbContrast.ThumbSize = new System.Drawing.Size(8, 16);
			this.tbContrast.TickFrequency = 16;
			this.tbContrast.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			this.tbContrast.Scroll += new System.EventHandler(this.ColorAdjustment_Scroll);
			this.tbContrast.ValueChanged += new System.EventHandler(this.AdjustmentSliderChanged);
			// 
			// labelSharpening
			// 
			this.labelSharpening.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelSharpening.AutoSize = true;
			this.labelSharpening.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
			this.labelSharpening.Location = new System.Drawing.Point(11, 92);
			this.labelSharpening.Name = "labelSharpening";
			this.labelSharpening.Size = new System.Drawing.Size(61, 12);
			this.labelSharpening.TabIndex = 9;
			this.labelSharpening.Text = "Sharpening";
			// 
			// labelBrightness
			// 
			this.labelBrightness.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelBrightness.AutoSize = true;
			this.labelBrightness.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
			this.labelBrightness.Location = new System.Drawing.Point(11, 58);
			this.labelBrightness.Name = "labelBrightness";
			this.labelBrightness.Size = new System.Drawing.Size(59, 12);
			this.labelBrightness.TabIndex = 3;
			this.labelBrightness.Text = "Brightness";
			// 
			// btResetColors
			// 
			this.btResetColors.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btResetColors.Location = new System.Drawing.Point(478, 86);
			this.btResetColors.Name = "btResetColors";
			this.btResetColors.Size = new System.Drawing.Size(77, 24);
			this.btResetColors.TabIndex = 11;
			this.btResetColors.Text = "Reset";
			this.btResetColors.UseVisualStyleBackColor = true;
			this.btResetColors.Click += new System.EventHandler(this.btReset_Click);
			// 
			// btPrev
			// 
			this.btPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btPrev.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btPrev.Location = new System.Drawing.Point(8, 483);
			this.btPrev.Name = "btPrev";
			this.btPrev.Size = new System.Drawing.Size(80, 24);
			this.btPrev.TabIndex = 1;
			this.btPrev.Text = "&Previous";
			this.btPrev.Click += new System.EventHandler(this.btPrev_Click);
			// 
			// btNext
			// 
			this.btNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btNext.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btNext.Location = new System.Drawing.Point(92, 483);
			this.btNext.Name = "btNext";
			this.btNext.Size = new System.Drawing.Size(80, 24);
			this.btNext.TabIndex = 2;
			this.btNext.Text = "&Next";
			this.btNext.Click += new System.EventHandler(this.btNext_Click);
			// 
			// btScript
			// 
			this.btScript.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btScript.AutoEllipsis = true;
			this.btScript.Location = new System.Drawing.Point(188, 484);
			this.btScript.Name = "btScript";
			this.btScript.Size = new System.Drawing.Size(135, 23);
			this.btScript.TabIndex = 3;
			this.btScript.Text = "Lorem Ipsum";
			this.btScript.UseVisualStyleBackColor = true;
			this.btScript.Visible = false;
			// 
			// btApply
			// 
			this.btApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btApply.Location = new System.Drawing.Point(501, 483);
			this.btApply.Name = "btApply";
			this.btApply.Size = new System.Drawing.Size(80, 24);
			this.btApply.TabIndex = 6;
			this.btApply.Text = "&Apply";
			this.btApply.Click += new System.EventHandler(this.btApply_Click);
			// 
			// labelTranslator
			// 
			this.labelTranslator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelTranslator.AutoSize = true;
			this.labelTranslator.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTranslator.Location = new System.Drawing.Point(214, 300);
			this.labelTranslator.Name = "labelTranslator";
			this.labelTranslator.Size = new System.Drawing.Size(60, 12);
			this.labelTranslator.TabIndex = 64;
			this.labelTranslator.Text = "Translator:";
			// 
			// txTranslator
			// 
			this.txTranslator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txTranslator.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txTranslator.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txTranslator.Location = new System.Drawing.Point(216, 315);
			this.txTranslator.Name = "txTranslator";
			this.txTranslator.Size = new System.Drawing.Size(187, 20);
			this.txTranslator.TabIndex = 65;
			this.txTranslator.Tag = "Translator";
			//
			// ComicBookDialog
			// 
			this.AcceptButton = this.btOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(593, 517);
			this.Controls.Add(this.btApply);
			this.Controls.Add(this.btScript);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.btNext);
			this.Controls.Add(this.btCancel);
			this.Controls.Add(this.btPrev);
			this.Controls.Add(this.btOK);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(609, 556);
			this.Name = "ComicBookDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Info";
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ComicBookDialog_DragDrop);
			this.DragOver += new System.Windows.Forms.DragEventHandler(this.ComicBookDialog_DragOver);
			this.tabControl.ResumeLayout(false);
			this.tabSummary.ResumeLayout(false);
			this.cmThumbnail.ResumeLayout(false);
			this.tabDetails.ResumeLayout(false);
			this.tabDetails.PerformLayout();
			this.tabPlot.ResumeLayout(false);
			this.tabPlot.PerformLayout();
			this.tabNotes.ResumeLayout(false);
			this.tabPageSummary.ResumeLayout(false);
			this.tabPageSummary.PerformLayout();
			this.tabPageNotes.ResumeLayout(false);
			this.tabPageNotes.PerformLayout();
			this.tabPageReview.ResumeLayout(false);
			this.tabPageReview.PerformLayout();
			this.tabCatalog.ResumeLayout(false);
			this.tabCatalog.PerformLayout();
			this.tabCustom.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.customValuesData)).EndInit();
			this.tabPages.ResumeLayout(false);
			this.tabPages.PerformLayout();
			this.cmResetPages.ResumeLayout(false);
			this.tabColors.ResumeLayout(false);
			this.panelImage.ResumeLayout(false);
			this.panelImageControls.ResumeLayout(false);
			this.panelImageControls.PerformLayout();
			this.ResumeLayout(false);

		}

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
		private Label labelTranslator;
		private TextBoxEx txTranslator;
	}
}
