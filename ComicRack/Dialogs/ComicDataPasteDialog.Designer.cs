using cYo.Common.Windows.Forms;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class ComicDataPasteDialog
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
			this.btCancel = new System.Windows.Forms.Button();
			this.btOK = new System.Windows.Forms.Button();
			this.chkSeries = new System.Windows.Forms.CheckBox();
			this.chkTitle = new System.Windows.Forms.CheckBox();
			this.chkVolume = new System.Windows.Forms.CheckBox();
			this.chkYear = new System.Windows.Forms.CheckBox();
			this.chkNumber = new System.Windows.Forms.CheckBox();
			this.chkCount = new System.Windows.Forms.CheckBox();
			this.chkMonth = new System.Windows.Forms.CheckBox();
			this.chkWriter = new System.Windows.Forms.CheckBox();
			this.chkPenciller = new System.Windows.Forms.CheckBox();
			this.chkPublisher = new System.Windows.Forms.CheckBox();
			this.chkInker = new System.Windows.Forms.CheckBox();
			this.chkColorist = new System.Windows.Forms.CheckBox();
			this.chkLetterer = new System.Windows.Forms.CheckBox();
			this.chkCover = new System.Windows.Forms.CheckBox();
			this.chkEditor = new System.Windows.Forms.CheckBox();
			this.chkGenre = new System.Windows.Forms.CheckBox();
			this.chkLanguage = new System.Windows.Forms.CheckBox();
			this.chkImprint = new System.Windows.Forms.CheckBox();
			this.chkRating = new System.Windows.Forms.CheckBox();
			this.chkSummary = new System.Windows.Forms.CheckBox();
			this.chkTags = new System.Windows.Forms.CheckBox();
			this.chkNotes = new System.Windows.Forms.CheckBox();
			this.chkManga = new System.Windows.Forms.CheckBox();
			this.chkAlternateSeries = new System.Windows.Forms.CheckBox();
			this.chkAlternateNumber = new System.Windows.Forms.CheckBox();
			this.chkAlternateCount = new System.Windows.Forms.CheckBox();
			this.chkColor = new System.Windows.Forms.CheckBox();
			this.chkCommunityRating = new System.Windows.Forms.CheckBox();
			this.chkAgeRating = new System.Windows.Forms.CheckBox();
			this.chkFormat = new System.Windows.Forms.CheckBox();
			this.chkBlackAndWhite = new System.Windows.Forms.CheckBox();
			this.chkLocations = new System.Windows.Forms.CheckBox();
			this.chkTeams = new System.Windows.Forms.CheckBox();
			this.chkWeb = new System.Windows.Forms.CheckBox();
			this.chkCharacters = new System.Windows.Forms.CheckBox();
			this.chkBookCollectionStatus = new System.Windows.Forms.CheckBox();
			this.chkBookNotes = new System.Windows.Forms.CheckBox();
			this.chkBookCondition = new System.Windows.Forms.CheckBox();
			this.chkBookLocation = new System.Windows.Forms.CheckBox();
			this.chkBookOwner = new System.Windows.Forms.CheckBox();
			this.chkBookStore = new System.Windows.Forms.CheckBox();
			this.chkBookPrice = new System.Windows.Forms.CheckBox();
			this.btMarkDefined = new System.Windows.Forms.Button();
			this.btMarkAll = new System.Windows.Forms.Button();
			this.btMarkNone = new System.Windows.Forms.Button();
			this.pageData = new System.Windows.Forms.Panel();
			this.grpCustom = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			this.grpCatalog = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			this.chkReleasedTime = new System.Windows.Forms.CheckBox();
			this.chkAddedTime = new System.Windows.Forms.CheckBox();
			this.chkOpenedTime = new System.Windows.Forms.CheckBox();
			this.chkPageCount = new System.Windows.Forms.CheckBox();
			this.chkISBN = new System.Windows.Forms.CheckBox();
			this.chkBookAge = new System.Windows.Forms.CheckBox();
			this.grpPlotNotes = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			this.chkScanInformation = new System.Windows.Forms.CheckBox();
			this.chkReview = new System.Windows.Forms.CheckBox();
			this.chkMainCharacterOrTeam = new System.Windows.Forms.CheckBox();
			this.grpArtists = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			this.grpMain = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			this.chkDay = new System.Windows.Forms.CheckBox();
			this.chkSeriesGroup = new System.Windows.Forms.CheckBox();
			this.chkStoryArc = new System.Windows.Forms.CheckBox();
			this.chkSeriesComplete = new System.Windows.Forms.CheckBox();
			this.chkTranslator = new System.Windows.Forms.CheckBox();
			this.pageData.SuspendLayout();
			this.grpCatalog.SuspendLayout();
			this.grpPlotNotes.SuspendLayout();
			this.grpArtists.SuspendLayout();
			this.grpMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// btCancel
			// 
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btCancel.Location = new System.Drawing.Point(473, 389);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(80, 24);
			this.btCancel.TabIndex = 1;
			this.btCancel.Text = "&Cancel";
			// 
			// btOK
			// 
			this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btOK.Location = new System.Drawing.Point(387, 389);
			this.btOK.Name = "btOK";
			this.btOK.Size = new System.Drawing.Size(80, 24);
			this.btOK.TabIndex = 0;
			this.btOK.Text = "&OK";
			// 
			// chkSeries
			// 
			this.chkSeries.AutoEllipsis = true;
			this.chkSeries.Location = new System.Drawing.Point(12, 35);
			this.chkSeries.Name = "chkSeries";
			this.chkSeries.Size = new System.Drawing.Size(120, 17);
			this.chkSeries.TabIndex = 0;
			this.chkSeries.Tag = "Series";
			this.chkSeries.Text = "Series";
			this.chkSeries.UseVisualStyleBackColor = true;
			// 
			// chkTitle
			// 
			this.chkTitle.AutoEllipsis = true;
			this.chkTitle.Location = new System.Drawing.Point(12, 58);
			this.chkTitle.Name = "chkTitle";
			this.chkTitle.Size = new System.Drawing.Size(120, 17);
			this.chkTitle.TabIndex = 4;
			this.chkTitle.Tag = "Title";
			this.chkTitle.Text = "Title";
			this.chkTitle.UseVisualStyleBackColor = true;
			// 
			// chkVolume
			// 
			this.chkVolume.AutoEllipsis = true;
			this.chkVolume.Location = new System.Drawing.Point(138, 35);
			this.chkVolume.Name = "chkVolume";
			this.chkVolume.Size = new System.Drawing.Size(120, 17);
			this.chkVolume.TabIndex = 1;
			this.chkVolume.Tag = "Volume";
			this.chkVolume.Text = "Volume";
			this.chkVolume.UseVisualStyleBackColor = true;
			// 
			// chkYear
			// 
			this.chkYear.AutoEllipsis = true;
			this.chkYear.Location = new System.Drawing.Point(138, 58);
			this.chkYear.Name = "chkYear";
			this.chkYear.Size = new System.Drawing.Size(120, 17);
			this.chkYear.TabIndex = 5;
			this.chkYear.Tag = "Year";
			this.chkYear.Text = "Year";
			this.chkYear.UseVisualStyleBackColor = true;
			// 
			// chkNumber
			// 
			this.chkNumber.AutoEllipsis = true;
			this.chkNumber.Location = new System.Drawing.Point(264, 35);
			this.chkNumber.Name = "chkNumber";
			this.chkNumber.Size = new System.Drawing.Size(120, 17);
			this.chkNumber.TabIndex = 2;
			this.chkNumber.Tag = "Number";
			this.chkNumber.Text = "Number";
			this.chkNumber.UseVisualStyleBackColor = true;
			// 
			// chkCount
			// 
			this.chkCount.AutoEllipsis = true;
			this.chkCount.Location = new System.Drawing.Point(398, 35);
			this.chkCount.Name = "chkCount";
			this.chkCount.Size = new System.Drawing.Size(120, 17);
			this.chkCount.TabIndex = 3;
			this.chkCount.Tag = "Count";
			this.chkCount.Text = "Total Number";
			this.chkCount.UseVisualStyleBackColor = true;
			// 
			// chkMonth
			// 
			this.chkMonth.AutoEllipsis = true;
			this.chkMonth.Location = new System.Drawing.Point(264, 58);
			this.chkMonth.Name = "chkMonth";
			this.chkMonth.Size = new System.Drawing.Size(120, 17);
			this.chkMonth.TabIndex = 6;
			this.chkMonth.Tag = "Month";
			this.chkMonth.Text = "Month";
			this.chkMonth.UseVisualStyleBackColor = true;
			// 
			// chkWriter
			// 
			this.chkWriter.AutoEllipsis = true;
			this.chkWriter.Location = new System.Drawing.Point(12, 38);
			this.chkWriter.Name = "chkWriter";
			this.chkWriter.Size = new System.Drawing.Size(120, 17);
			this.chkWriter.TabIndex = 0;
			this.chkWriter.Tag = "Writer";
			this.chkWriter.Text = "Writer";
			this.chkWriter.UseVisualStyleBackColor = true;
			// 
			// chkPenciller
			// 
			this.chkPenciller.AutoEllipsis = true;
			this.chkPenciller.Location = new System.Drawing.Point(138, 38);
			this.chkPenciller.Name = "chkPenciller";
			this.chkPenciller.Size = new System.Drawing.Size(120, 17);
			this.chkPenciller.TabIndex = 1;
			this.chkPenciller.Tag = "Penciller";
			this.chkPenciller.Text = "Penciller";
			this.chkPenciller.UseVisualStyleBackColor = true;
			// 
			// chkPublisher
			// 
			this.chkPublisher.AutoEllipsis = true;
			this.chkPublisher.Location = new System.Drawing.Point(264, 128);
			this.chkPublisher.Name = "chkPublisher";
			this.chkPublisher.Size = new System.Drawing.Size(120, 17);
			this.chkPublisher.TabIndex = 16;
			this.chkPublisher.Tag = "Publisher";
			this.chkPublisher.Text = "Publisher";
			this.chkPublisher.UseVisualStyleBackColor = true;
			// 
			// chkInker
			// 
			this.chkInker.AutoEllipsis = true;
			this.chkInker.Location = new System.Drawing.Point(264, 38);
			this.chkInker.Name = "chkInker";
			this.chkInker.Size = new System.Drawing.Size(120, 17);
			this.chkInker.TabIndex = 2;
			this.chkInker.Tag = "Inker";
			this.chkInker.Text = "Inker";
			this.chkInker.UseVisualStyleBackColor = true;
			// 
			// chkColorist
			// 
			this.chkColorist.AutoEllipsis = true;
			this.chkColorist.Location = new System.Drawing.Point(398, 38);
			this.chkColorist.Name = "chkColorist";
			this.chkColorist.Size = new System.Drawing.Size(120, 17);
			this.chkColorist.TabIndex = 3;
			this.chkColorist.Tag = "Colorist";
			this.chkColorist.Text = "Colorist";
			this.chkColorist.UseVisualStyleBackColor = true;
			// 
			// chkLetterer
			// 
			this.chkLetterer.AutoEllipsis = true;
			this.chkLetterer.Location = new System.Drawing.Point(12, 61);
			this.chkLetterer.Name = "chkLetterer";
			this.chkLetterer.Size = new System.Drawing.Size(120, 17);
			this.chkLetterer.TabIndex = 4;
			this.chkLetterer.Tag = "Letterer";
			this.chkLetterer.Text = "Letterer";
			this.chkLetterer.UseVisualStyleBackColor = true;
			// 
			// chkCover
			// 
			this.chkCover.AutoEllipsis = true;
			this.chkCover.Location = new System.Drawing.Point(138, 61);
			this.chkCover.Name = "chkCover";
			this.chkCover.Size = new System.Drawing.Size(120, 17);
			this.chkCover.TabIndex = 5;
			this.chkCover.Tag = "CoverArtist";
			this.chkCover.Text = "Cover Artist";
			this.chkCover.UseVisualStyleBackColor = true;
			// 
			// chkEditor
			// 
			this.chkEditor.AutoEllipsis = true;
			this.chkEditor.Location = new System.Drawing.Point(264, 61);
			this.chkEditor.Name = "chkEditor";
			this.chkEditor.Size = new System.Drawing.Size(120, 17);
			this.chkEditor.TabIndex = 6;
			this.chkEditor.Tag = "Editor";
			this.chkEditor.Text = "Editor";
			this.chkEditor.UseVisualStyleBackColor = true;
			// 
			// chkGenre
			// 
			this.chkGenre.AutoEllipsis = true;
			this.chkGenre.Location = new System.Drawing.Point(264, 151);
			this.chkGenre.Name = "chkGenre";
			this.chkGenre.Size = new System.Drawing.Size(120, 17);
			this.chkGenre.TabIndex = 20;
			this.chkGenre.Tag = "Genre";
			this.chkGenre.Text = "Genre";
			this.chkGenre.UseVisualStyleBackColor = true;
			// 
			// chkLanguage
			// 
			this.chkLanguage.AutoEllipsis = true;
			this.chkLanguage.Location = new System.Drawing.Point(138, 151);
			this.chkLanguage.Name = "chkLanguage";
			this.chkLanguage.Size = new System.Drawing.Size(120, 17);
			this.chkLanguage.TabIndex = 19;
			this.chkLanguage.Tag = "LanguageISO";
			this.chkLanguage.Text = "Language";
			this.chkLanguage.UseVisualStyleBackColor = true;
			// 
			// chkImprint
			// 
			this.chkImprint.AutoEllipsis = true;
			this.chkImprint.Location = new System.Drawing.Point(12, 151);
			this.chkImprint.Name = "chkImprint";
			this.chkImprint.Size = new System.Drawing.Size(120, 17);
			this.chkImprint.TabIndex = 18;
			this.chkImprint.Tag = "Imprint";
			this.chkImprint.Text = "Imprint";
			this.chkImprint.UseVisualStyleBackColor = true;
			// 
			// chkRating
			// 
			this.chkRating.AutoEllipsis = true;
			this.chkRating.Location = new System.Drawing.Point(138, 174);
			this.chkRating.Name = "chkRating";
			this.chkRating.Size = new System.Drawing.Size(120, 17);
			this.chkRating.TabIndex = 23;
			this.chkRating.Tag = "Rating";
			this.chkRating.Text = "My Rating";
			this.chkRating.UseVisualStyleBackColor = true;
			// 
			// chkSummary
			// 
			this.chkSummary.AutoEllipsis = true;
			this.chkSummary.Location = new System.Drawing.Point(12, 59);
			this.chkSummary.Name = "chkSummary";
			this.chkSummary.Size = new System.Drawing.Size(120, 17);
			this.chkSummary.TabIndex = 4;
			this.chkSummary.Tag = "Summary";
			this.chkSummary.Text = "Summary";
			this.chkSummary.UseVisualStyleBackColor = true;
			// 
			// chkTags
			// 
			this.chkTags.AutoEllipsis = true;
			this.chkTags.Location = new System.Drawing.Point(12, 174);
			this.chkTags.Name = "chkTags";
			this.chkTags.Size = new System.Drawing.Size(120, 17);
			this.chkTags.TabIndex = 22;
			this.chkTags.Tag = "Tags";
			this.chkTags.Text = "Tags";
			this.chkTags.UseVisualStyleBackColor = true;
			// 
			// chkNotes
			// 
			this.chkNotes.AutoEllipsis = true;
			this.chkNotes.Location = new System.Drawing.Point(138, 59);
			this.chkNotes.Name = "chkNotes";
			this.chkNotes.Size = new System.Drawing.Size(120, 17);
			this.chkNotes.TabIndex = 5;
			this.chkNotes.Tag = "Notes";
			this.chkNotes.Text = "Notes";
			this.chkNotes.UseVisualStyleBackColor = true;
			// 
			// chkManga
			// 
			this.chkManga.AutoEllipsis = true;
			this.chkManga.Location = new System.Drawing.Point(398, 128);
			this.chkManga.Name = "chkManga";
			this.chkManga.Size = new System.Drawing.Size(120, 17);
			this.chkManga.TabIndex = 17;
			this.chkManga.Tag = "Manga";
			this.chkManga.Text = "Manga";
			this.chkManga.UseVisualStyleBackColor = true;
			// 
			// chkAlternateSeries
			// 
			this.chkAlternateSeries.AutoEllipsis = true;
			this.chkAlternateSeries.Location = new System.Drawing.Point(138, 81);
			this.chkAlternateSeries.Name = "chkAlternateSeries";
			this.chkAlternateSeries.Size = new System.Drawing.Size(120, 17);
			this.chkAlternateSeries.TabIndex = 9;
			this.chkAlternateSeries.Tag = "AlternateSeries";
			this.chkAlternateSeries.Text = "Alt. Series";
			this.chkAlternateSeries.UseVisualStyleBackColor = true;
			// 
			// chkAlternateNumber
			// 
			this.chkAlternateNumber.AutoEllipsis = true;
			this.chkAlternateNumber.Location = new System.Drawing.Point(264, 81);
			this.chkAlternateNumber.Name = "chkAlternateNumber";
			this.chkAlternateNumber.Size = new System.Drawing.Size(120, 17);
			this.chkAlternateNumber.TabIndex = 10;
			this.chkAlternateNumber.Tag = "AlternateNumber";
			this.chkAlternateNumber.Text = "Alt. Number";
			this.chkAlternateNumber.UseVisualStyleBackColor = true;
			// 
			// chkAlternateCount
			// 
			this.chkAlternateCount.AutoEllipsis = true;
			this.chkAlternateCount.Location = new System.Drawing.Point(398, 81);
			this.chkAlternateCount.Name = "chkAlternateCount";
			this.chkAlternateCount.Size = new System.Drawing.Size(120, 17);
			this.chkAlternateCount.TabIndex = 11;
			this.chkAlternateCount.Tag = "AlternateCount";
			this.chkAlternateCount.Text = "Alt. Total Number";
			this.chkAlternateCount.UseVisualStyleBackColor = true;
			// 
			// chkColor
			// 
			this.chkColor.AutoEllipsis = true;
			this.chkColor.Location = new System.Drawing.Point(398, 174);
			this.chkColor.Name = "chkColor";
			this.chkColor.Size = new System.Drawing.Size(120, 17);
			this.chkColor.TabIndex = 25;
			this.chkColor.Tag = "ColorAdjustment";
			this.chkColor.Text = "Color Adjustment";
			this.chkColor.UseVisualStyleBackColor = true;
			// 
			// chkCommunityRating
			// 
			this.chkCommunityRating.AutoEllipsis = true;
			this.chkCommunityRating.Location = new System.Drawing.Point(264, 174);
			this.chkCommunityRating.Name = "chkCommunityRating";
			this.chkCommunityRating.Size = new System.Drawing.Size(120, 17);
			this.chkCommunityRating.TabIndex = 24;
			this.chkCommunityRating.Tag = "CommunityRating";
			this.chkCommunityRating.Text = "Community Rating";
			this.chkCommunityRating.UseVisualStyleBackColor = true;
			// 
			// chkAgeRating
			// 
			this.chkAgeRating.AutoEllipsis = true;
			this.chkAgeRating.Location = new System.Drawing.Point(138, 128);
			this.chkAgeRating.Name = "chkAgeRating";
			this.chkAgeRating.Size = new System.Drawing.Size(120, 17);
			this.chkAgeRating.TabIndex = 15;
			this.chkAgeRating.Tag = "AgeRating";
			this.chkAgeRating.Text = "Age Rating";
			this.chkAgeRating.UseVisualStyleBackColor = true;
			// 
			// chkFormat
			// 
			this.chkFormat.AutoEllipsis = true;
			this.chkFormat.Location = new System.Drawing.Point(12, 128);
			this.chkFormat.Name = "chkFormat";
			this.chkFormat.Size = new System.Drawing.Size(120, 17);
			this.chkFormat.TabIndex = 14;
			this.chkFormat.Tag = "Format";
			this.chkFormat.Text = "Format";
			this.chkFormat.UseVisualStyleBackColor = true;
			// 
			// chkBlackAndWhite
			// 
			this.chkBlackAndWhite.AutoEllipsis = true;
			this.chkBlackAndWhite.Location = new System.Drawing.Point(398, 151);
			this.chkBlackAndWhite.Name = "chkBlackAndWhite";
			this.chkBlackAndWhite.Size = new System.Drawing.Size(120, 17);
			this.chkBlackAndWhite.TabIndex = 21;
			this.chkBlackAndWhite.Tag = "BlackAndWhite";
			this.chkBlackAndWhite.Text = "Black and White";
			this.chkBlackAndWhite.UseVisualStyleBackColor = true;
			// 
			// chkLocations
			// 
			this.chkLocations.AutoEllipsis = true;
			this.chkLocations.Location = new System.Drawing.Point(398, 36);
			this.chkLocations.Name = "chkLocations";
			this.chkLocations.Size = new System.Drawing.Size(120, 17);
			this.chkLocations.TabIndex = 3;
			this.chkLocations.Tag = "Locations";
			this.chkLocations.Text = "Locations";
			this.chkLocations.UseVisualStyleBackColor = true;
			// 
			// chkTeams
			// 
			this.chkTeams.AutoEllipsis = true;
			this.chkTeams.Location = new System.Drawing.Point(264, 36);
			this.chkTeams.Name = "chkTeams";
			this.chkTeams.Size = new System.Drawing.Size(120, 17);
			this.chkTeams.TabIndex = 2;
			this.chkTeams.Tag = "Teams";
			this.chkTeams.Text = "Teams";
			this.chkTeams.UseVisualStyleBackColor = true;
			// 
			// chkWeb
			// 
			this.chkWeb.AutoEllipsis = true;
			this.chkWeb.Location = new System.Drawing.Point(138, 82);
			this.chkWeb.Name = "chkWeb";
			this.chkWeb.Size = new System.Drawing.Size(120, 17);
			this.chkWeb.TabIndex = 8;
			this.chkWeb.Tag = "Web";
			this.chkWeb.Text = "Web";
			this.chkWeb.UseVisualStyleBackColor = true;
			// 
			// chkCharacters
			// 
			this.chkCharacters.AutoEllipsis = true;
			this.chkCharacters.Location = new System.Drawing.Point(138, 36);
			this.chkCharacters.Name = "chkCharacters";
			this.chkCharacters.Size = new System.Drawing.Size(120, 17);
			this.chkCharacters.TabIndex = 1;
			this.chkCharacters.Tag = "Characters";
			this.chkCharacters.Text = "Characters";
			this.chkCharacters.UseVisualStyleBackColor = true;
			// 
			// chkBookCollectionStatus
			// 
			this.chkBookCollectionStatus.AutoEllipsis = true;
			this.chkBookCollectionStatus.Location = new System.Drawing.Point(264, 59);
			this.chkBookCollectionStatus.Name = "chkBookCollectionStatus";
			this.chkBookCollectionStatus.Size = new System.Drawing.Size(120, 17);
			this.chkBookCollectionStatus.TabIndex = 6;
			this.chkBookCollectionStatus.Tag = "BookCollectionStatus";
			this.chkBookCollectionStatus.Text = "Collection Status";
			this.chkBookCollectionStatus.UseVisualStyleBackColor = true;
			// 
			// chkBookNotes
			// 
			this.chkBookNotes.AutoEllipsis = true;
			this.chkBookNotes.Location = new System.Drawing.Point(138, 59);
			this.chkBookNotes.Name = "chkBookNotes";
			this.chkBookNotes.Size = new System.Drawing.Size(120, 17);
			this.chkBookNotes.TabIndex = 5;
			this.chkBookNotes.Tag = "BookNotes";
			this.chkBookNotes.Text = "Notes";
			this.chkBookNotes.UseVisualStyleBackColor = true;
			// 
			// chkBookCondition
			// 
			this.chkBookCondition.AutoEllipsis = true;
			this.chkBookCondition.Location = new System.Drawing.Point(12, 59);
			this.chkBookCondition.Name = "chkBookCondition";
			this.chkBookCondition.Size = new System.Drawing.Size(120, 17);
			this.chkBookCondition.TabIndex = 4;
			this.chkBookCondition.Tag = "BookCondition";
			this.chkBookCondition.Text = "Condition";
			this.chkBookCondition.UseVisualStyleBackColor = true;
			// 
			// chkBookLocation
			// 
			this.chkBookLocation.AutoEllipsis = true;
			this.chkBookLocation.Location = new System.Drawing.Point(264, 36);
			this.chkBookLocation.Name = "chkBookLocation";
			this.chkBookLocation.Size = new System.Drawing.Size(120, 17);
			this.chkBookLocation.TabIndex = 2;
			this.chkBookLocation.Tag = "BookLocation";
			this.chkBookLocation.Text = "Location";
			this.chkBookLocation.UseVisualStyleBackColor = true;
			// 
			// chkBookOwner
			// 
			this.chkBookOwner.AutoEllipsis = true;
			this.chkBookOwner.Location = new System.Drawing.Point(398, 59);
			this.chkBookOwner.Name = "chkBookOwner";
			this.chkBookOwner.Size = new System.Drawing.Size(120, 17);
			this.chkBookOwner.TabIndex = 7;
			this.chkBookOwner.Tag = "BookOwner";
			this.chkBookOwner.Text = "Owner";
			this.chkBookOwner.UseVisualStyleBackColor = true;
			// 
			// chkBookStore
			// 
			this.chkBookStore.AutoEllipsis = true;
			this.chkBookStore.Location = new System.Drawing.Point(12, 36);
			this.chkBookStore.Name = "chkBookStore";
			this.chkBookStore.Size = new System.Drawing.Size(120, 17);
			this.chkBookStore.TabIndex = 0;
			this.chkBookStore.Tag = "BookStore";
			this.chkBookStore.Text = "Store";
			this.chkBookStore.UseVisualStyleBackColor = true;
			// 
			// chkBookPrice
			// 
			this.chkBookPrice.AutoEllipsis = true;
			this.chkBookPrice.Location = new System.Drawing.Point(138, 36);
			this.chkBookPrice.Name = "chkBookPrice";
			this.chkBookPrice.Size = new System.Drawing.Size(120, 17);
			this.chkBookPrice.TabIndex = 1;
			this.chkBookPrice.Tag = "BookPrice";
			this.chkBookPrice.Text = "Price";
			this.chkBookPrice.UseVisualStyleBackColor = true;
			// 
			// btMarkDefined
			// 
			this.btMarkDefined.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btMarkDefined.Location = new System.Drawing.Point(87, 389);
			this.btMarkDefined.Name = "btMarkDefined";
			this.btMarkDefined.Size = new System.Drawing.Size(70, 23);
			this.btMarkDefined.TabIndex = 3;
			this.btMarkDefined.Text = "Only &Set";
			this.btMarkDefined.UseVisualStyleBackColor = true;
			this.btMarkDefined.Click += new System.EventHandler(this.btMarkDefined_Click);
			// 
			// btMarkAll
			// 
			this.btMarkAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btMarkAll.Location = new System.Drawing.Point(11, 389);
			this.btMarkAll.Name = "btMarkAll";
			this.btMarkAll.Size = new System.Drawing.Size(70, 23);
			this.btMarkAll.TabIndex = 2;
			this.btMarkAll.Text = "&All";
			this.btMarkAll.UseVisualStyleBackColor = true;
			this.btMarkAll.Click += new System.EventHandler(this.btMarkAll_Click);
			// 
			// btMarkNone
			// 
			this.btMarkNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btMarkNone.Location = new System.Drawing.Point(163, 389);
			this.btMarkNone.Name = "btMarkNone";
			this.btMarkNone.Size = new System.Drawing.Size(70, 23);
			this.btMarkNone.TabIndex = 4;
			this.btMarkNone.Text = "&Clear";
			this.btMarkNone.UseVisualStyleBackColor = true;
			this.btMarkNone.Click += new System.EventHandler(this.btMarkNone_Click);
			// 
			// pageData
			// 
			this.pageData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pageData.AutoScroll = true;
			this.pageData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pageData.Controls.Add(this.grpCustom);
			this.pageData.Controls.Add(this.grpCatalog);
			this.pageData.Controls.Add(this.grpPlotNotes);
			this.pageData.Controls.Add(this.grpArtists);
			this.pageData.Controls.Add(this.grpMain);
			this.pageData.Location = new System.Drawing.Point(12, 12);
			this.pageData.Name = "pageData";
			this.pageData.Size = new System.Drawing.Size(541, 371);
			this.pageData.TabIndex = 0;
			// 
			// grpCustom
			// 
			this.grpCustom.Dock = System.Windows.Forms.DockStyle.Top;
			this.grpCustom.Location = new System.Drawing.Point(0, 548);
			this.grpCustom.Name = "grpCustom";
			this.grpCustom.Size = new System.Drawing.Size(522, 90);
			this.grpCustom.TabIndex = 4;
			this.grpCustom.Text = "Custom";
			// 
			// grpCatalog
			// 
			this.grpCatalog.Controls.Add(this.chkReleasedTime);
			this.grpCatalog.Controls.Add(this.chkAddedTime);
			this.grpCatalog.Controls.Add(this.chkOpenedTime);
			this.grpCatalog.Controls.Add(this.chkPageCount);
			this.grpCatalog.Controls.Add(this.chkISBN);
			this.grpCatalog.Controls.Add(this.chkBookAge);
			this.grpCatalog.Controls.Add(this.chkBookNotes);
			this.grpCatalog.Controls.Add(this.chkBookCollectionStatus);
			this.grpCatalog.Controls.Add(this.chkBookStore);
			this.grpCatalog.Controls.Add(this.chkBookPrice);
			this.grpCatalog.Controls.Add(this.chkBookCondition);
			this.grpCatalog.Controls.Add(this.chkBookOwner);
			this.grpCatalog.Controls.Add(this.chkBookLocation);
			this.grpCatalog.Dock = System.Windows.Forms.DockStyle.Top;
			this.grpCatalog.Location = new System.Drawing.Point(0, 415);
			this.grpCatalog.Name = "grpCatalog";
			this.grpCatalog.Size = new System.Drawing.Size(522, 133);
			this.grpCatalog.TabIndex = 3;
			this.grpCatalog.Text = "Catalog";
			// 
			// chkReleasedTime
			// 
			this.chkReleasedTime.AutoEllipsis = true;
			this.chkReleasedTime.Location = new System.Drawing.Point(12, 103);
			this.chkReleasedTime.Name = "chkReleasedTime";
			this.chkReleasedTime.Size = new System.Drawing.Size(120, 17);
			this.chkReleasedTime.TabIndex = 10;
			this.chkReleasedTime.Tag = "ReleasedTime";
			this.chkReleasedTime.Text = "Released";
			this.chkReleasedTime.UseVisualStyleBackColor = true;
			// 
			// chkAddedTime
			// 
			this.chkAddedTime.AutoEllipsis = true;
			this.chkAddedTime.Location = new System.Drawing.Point(138, 103);
			this.chkAddedTime.Name = "chkAddedTime";
			this.chkAddedTime.Size = new System.Drawing.Size(120, 17);
			this.chkAddedTime.TabIndex = 11;
			this.chkAddedTime.Tag = "AddedTime";
			this.chkAddedTime.Text = "Added/Purchased";
			this.chkAddedTime.UseVisualStyleBackColor = true;
			// 
			// chkOpenedTime
			// 
			this.chkOpenedTime.AutoEllipsis = true;
			this.chkOpenedTime.Location = new System.Drawing.Point(264, 103);
			this.chkOpenedTime.Name = "chkOpenedTime";
			this.chkOpenedTime.Size = new System.Drawing.Size(120, 17);
			this.chkOpenedTime.TabIndex = 12;
			this.chkOpenedTime.Tag = "OpenedTime";
			this.chkOpenedTime.Text = "Opened/Read";
			this.chkOpenedTime.UseVisualStyleBackColor = true;
			// 
			// chkPageCount
			// 
			this.chkPageCount.AutoEllipsis = true;
			this.chkPageCount.Location = new System.Drawing.Point(138, 82);
			this.chkPageCount.Name = "chkPageCount";
			this.chkPageCount.Size = new System.Drawing.Size(120, 17);
			this.chkPageCount.TabIndex = 9;
			this.chkPageCount.Tag = "PagesCount";
			this.chkPageCount.Text = "Pages";
			this.chkPageCount.UseVisualStyleBackColor = true;
			// 
			// chkISBN
			// 
			this.chkISBN.AutoEllipsis = true;
			this.chkISBN.Location = new System.Drawing.Point(12, 82);
			this.chkISBN.Name = "chkISBN";
			this.chkISBN.Size = new System.Drawing.Size(120, 17);
			this.chkISBN.TabIndex = 8;
			this.chkISBN.Tag = "ISBN";
			this.chkISBN.Text = "ISBN";
			this.chkISBN.UseVisualStyleBackColor = true;
			// 
			// chkBookAge
			// 
			this.chkBookAge.AutoEllipsis = true;
			this.chkBookAge.Location = new System.Drawing.Point(398, 36);
			this.chkBookAge.Name = "chkBookAge";
			this.chkBookAge.Size = new System.Drawing.Size(120, 17);
			this.chkBookAge.TabIndex = 3;
			this.chkBookAge.Tag = "BookAge";
			this.chkBookAge.Text = "Age";
			this.chkBookAge.UseVisualStyleBackColor = true;
			// 
			// grpPlotNotes
			// 
			this.grpPlotNotes.Controls.Add(this.chkScanInformation);
			this.grpPlotNotes.Controls.Add(this.chkReview);
			this.grpPlotNotes.Controls.Add(this.chkNotes);
			this.grpPlotNotes.Controls.Add(this.chkSummary);
			this.grpPlotNotes.Controls.Add(this.chkLocations);
			this.grpPlotNotes.Controls.Add(this.chkWeb);
			this.grpPlotNotes.Controls.Add(this.chkMainCharacterOrTeam);
			this.grpPlotNotes.Controls.Add(this.chkCharacters);
			this.grpPlotNotes.Controls.Add(this.chkTeams);
			this.grpPlotNotes.Dock = System.Windows.Forms.DockStyle.Top;
			this.grpPlotNotes.Location = new System.Drawing.Point(0, 297);
			this.grpPlotNotes.Name = "grpPlotNotes";
			this.grpPlotNotes.Size = new System.Drawing.Size(522, 118);
			this.grpPlotNotes.TabIndex = 2;
			this.grpPlotNotes.Text = "Plot & Notes";
			// 
			// chkScanInformation
			// 
			this.chkScanInformation.AutoEllipsis = true;
			this.chkScanInformation.Location = new System.Drawing.Point(12, 82);
			this.chkScanInformation.Name = "chkScanInformation";
			this.chkScanInformation.Size = new System.Drawing.Size(120, 17);
			this.chkScanInformation.TabIndex = 7;
			this.chkScanInformation.Tag = "ScanInformation";
			this.chkScanInformation.Text = "Scan Information";
			this.chkScanInformation.UseVisualStyleBackColor = true;
			// 
			// chkReview
			// 
			this.chkReview.AutoEllipsis = true;
			this.chkReview.Location = new System.Drawing.Point(264, 59);
			this.chkReview.Name = "chkReview";
			this.chkReview.Size = new System.Drawing.Size(120, 17);
			this.chkReview.TabIndex = 6;
			this.chkReview.Tag = "Review";
			this.chkReview.Text = "Review";
			this.chkReview.UseVisualStyleBackColor = true;
			// 
			// chkMainCharacterOrTeam
			// 
			this.chkMainCharacterOrTeam.AutoEllipsis = true;
			this.chkMainCharacterOrTeam.Location = new System.Drawing.Point(12, 36);
			this.chkMainCharacterOrTeam.Name = "chkMainCharacterOrTeam";
			this.chkMainCharacterOrTeam.Size = new System.Drawing.Size(120, 17);
			this.chkMainCharacterOrTeam.TabIndex = 0;
			this.chkMainCharacterOrTeam.Tag = "MainCharacterOrTeam";
			this.chkMainCharacterOrTeam.Text = "Main Character";
			this.chkMainCharacterOrTeam.UseVisualStyleBackColor = true;
			// 
			// grpArtists
			// 
			this.grpArtists.Controls.Add(this.chkTranslator);
			this.grpArtists.Controls.Add(this.chkEditor);
			this.grpArtists.Controls.Add(this.chkCover);
			this.grpArtists.Controls.Add(this.chkLetterer);
			this.grpArtists.Controls.Add(this.chkColorist);
			this.grpArtists.Controls.Add(this.chkInker);
			this.grpArtists.Controls.Add(this.chkPenciller);
			this.grpArtists.Controls.Add(this.chkWriter);
			this.grpArtists.Dock = System.Windows.Forms.DockStyle.Top;
			this.grpArtists.Location = new System.Drawing.Point(0, 203);
			this.grpArtists.Name = "grpArtists";
			this.grpArtists.Size = new System.Drawing.Size(522, 94);
			this.grpArtists.TabIndex = 1;
			this.grpArtists.Text = "Artists / People Involved";
			// 
			// grpMain
			// 
			this.grpMain.Controls.Add(this.chkDay);
			this.grpMain.Controls.Add(this.chkSeriesGroup);
			this.grpMain.Controls.Add(this.chkStoryArc);
			this.grpMain.Controls.Add(this.chkSeriesComplete);
			this.grpMain.Controls.Add(this.chkCommunityRating);
			this.grpMain.Controls.Add(this.chkColor);
			this.grpMain.Controls.Add(this.chkRating);
			this.grpMain.Controls.Add(this.chkSeries);
			this.grpMain.Controls.Add(this.chkVolume);
			this.grpMain.Controls.Add(this.chkTags);
			this.grpMain.Controls.Add(this.chkBlackAndWhite);
			this.grpMain.Controls.Add(this.chkNumber);
			this.grpMain.Controls.Add(this.chkLanguage);
			this.grpMain.Controls.Add(this.chkAlternateCount);
			this.grpMain.Controls.Add(this.chkAgeRating);
			this.grpMain.Controls.Add(this.chkImprint);
			this.grpMain.Controls.Add(this.chkGenre);
			this.grpMain.Controls.Add(this.chkManga);
			this.grpMain.Controls.Add(this.chkPublisher);
			this.grpMain.Controls.Add(this.chkCount);
			this.grpMain.Controls.Add(this.chkFormat);
			this.grpMain.Controls.Add(this.chkAlternateSeries);
			this.grpMain.Controls.Add(this.chkAlternateNumber);
			this.grpMain.Controls.Add(this.chkTitle);
			this.grpMain.Controls.Add(this.chkMonth);
			this.grpMain.Controls.Add(this.chkYear);
			this.grpMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.grpMain.Location = new System.Drawing.Point(0, 0);
			this.grpMain.Name = "grpMain";
			this.grpMain.Size = new System.Drawing.Size(522, 203);
			this.grpMain.TabIndex = 0;
			this.grpMain.Text = "Main";
			// 
			// chkDay
			// 
			this.chkDay.AutoEllipsis = true;
			this.chkDay.Location = new System.Drawing.Point(398, 58);
			this.chkDay.Name = "chkDay";
			this.chkDay.Size = new System.Drawing.Size(107, 17);
			this.chkDay.TabIndex = 7;
			this.chkDay.Tag = "Day";
			this.chkDay.Text = "Day";
			this.chkDay.UseVisualStyleBackColor = true;
			// 
			// chkSeriesGroup
			// 
			this.chkSeriesGroup.AutoEllipsis = true;
			this.chkSeriesGroup.Location = new System.Drawing.Point(138, 104);
			this.chkSeriesGroup.Name = "chkSeriesGroup";
			this.chkSeriesGroup.Size = new System.Drawing.Size(120, 17);
			this.chkSeriesGroup.TabIndex = 13;
			this.chkSeriesGroup.Tag = "SeriesGroup";
			this.chkSeriesGroup.Text = "Series Group";
			this.chkSeriesGroup.UseVisualStyleBackColor = true;
			// 
			// chkStoryArc
			// 
			this.chkStoryArc.AutoEllipsis = true;
			this.chkStoryArc.Location = new System.Drawing.Point(12, 104);
			this.chkStoryArc.Name = "chkStoryArc";
			this.chkStoryArc.Size = new System.Drawing.Size(120, 17);
			this.chkStoryArc.TabIndex = 12;
			this.chkStoryArc.Tag = "StoryArc";
			this.chkStoryArc.Text = "Story Arc";
			this.chkStoryArc.UseVisualStyleBackColor = true;
			// 
			// chkSeriesComplete
			// 
			this.chkSeriesComplete.AutoEllipsis = true;
			this.chkSeriesComplete.Location = new System.Drawing.Point(12, 81);
			this.chkSeriesComplete.Name = "chkSeriesComplete";
			this.chkSeriesComplete.Size = new System.Drawing.Size(120, 17);
			this.chkSeriesComplete.TabIndex = 8;
			this.chkSeriesComplete.Tag = "SeriesComplete";
			this.chkSeriesComplete.Text = "Series complete";
			this.chkSeriesComplete.UseVisualStyleBackColor = true;
			// 
			// chkTranslator
			// 
			this.chkTranslator.AutoEllipsis = true;
			this.chkTranslator.Location = new System.Drawing.Point(398, 61);
			this.chkTranslator.Name = "chkTranslator";
			this.chkTranslator.Size = new System.Drawing.Size(120, 17);
			this.chkTranslator.TabIndex = 7;
			this.chkTranslator.Tag = "Translator";
			this.chkTranslator.Text = "Translator";
			this.chkTranslator.UseVisualStyleBackColor = true;
			// 
			// ComicDataPasteDialog
			// 
			this.AcceptButton = this.btOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(563, 422);
			this.Controls.Add(this.pageData);
			this.Controls.Add(this.btMarkNone);
			this.Controls.Add(this.btMarkAll);
			this.Controls.Add(this.btMarkDefined);
			this.Controls.Add(this.btCancel);
			this.Controls.Add(this.btOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ComicDataPasteDialog";
			this.RightToLeftLayout = true;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Paste Data to {0} Books";
			this.pageData.ResumeLayout(false);
			this.grpCatalog.ResumeLayout(false);
			this.grpPlotNotes.ResumeLayout(false);
			this.grpArtists.ResumeLayout(false);
			this.grpMain.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		private Button btCancel;
		private Button btOK;
		private CheckBox chkSeries;
		private CheckBox chkTitle;
		private CheckBox chkVolume;
		private CheckBox chkYear;
		private CheckBox chkNumber;
		private CheckBox chkCount;
		private CheckBox chkMonth;
		private CheckBox chkWriter;
		private CheckBox chkPenciller;
		private CheckBox chkPublisher;
		private CheckBox chkInker;
		private CheckBox chkColorist;
		private CheckBox chkLetterer;
		private CheckBox chkCover;
		private CheckBox chkEditor;
		private CheckBox chkGenre;
		private CheckBox chkLanguage;
		private CheckBox chkImprint;
		private CheckBox chkRating;
		private CheckBox chkSummary;
		private CheckBox chkTags;
		private CheckBox chkNotes;
		private CheckBox chkManga;
		private CheckBox chkAlternateSeries;
		private CheckBox chkAlternateNumber;
		private CheckBox chkAlternateCount;
		private CheckBox chkColor;
		private Button btMarkDefined;
		private Button btMarkAll;
		private Button btMarkNone;
		private CheckBox chkFormat;
		private CheckBox chkBlackAndWhite;
		private CheckBox chkAgeRating;
		private CheckBox chkCharacters;
		private CheckBox chkWeb;
		private CheckBox chkLocations;
		private CheckBox chkTeams;
		private CheckBox chkCommunityRating;
		private CheckBox chkBookCollectionStatus;
		private CheckBox chkBookNotes;
		private CheckBox chkBookCondition;
		private CheckBox chkBookLocation;
		private CheckBox chkBookOwner;
		private CheckBox chkBookStore;
		private CheckBox chkBookPrice;
		private Panel pageData;
		private CollapsibleGroupBox grpCatalog;
		private CollapsibleGroupBox grpPlotNotes;
		private CollapsibleGroupBox grpArtists;
		private CollapsibleGroupBox grpMain;
		private CheckBox chkBookAge;
		private CheckBox chkISBN;
		private CheckBox chkPageCount;
		private CheckBox chkAddedTime;
		private CheckBox chkOpenedTime;
		private CheckBox chkSeriesComplete;
		private CheckBox chkScanInformation;
		private CheckBox chkReview;
		private CheckBox chkMainCharacterOrTeam;
		private CheckBox chkSeriesGroup;
		private CheckBox chkStoryArc;
		private CheckBox chkDay;
		private CheckBox chkReleasedTime;
		private CollapsibleGroupBox grpCustom;
		private CheckBox chkTranslator;
	}
}
