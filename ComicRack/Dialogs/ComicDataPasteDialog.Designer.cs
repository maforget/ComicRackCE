using cYo.Common.Windows.Forms;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class ComicDataPasteDialog
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
			btCancel = new System.Windows.Forms.Button();
			btOK = new System.Windows.Forms.Button();
			chkSeries = new System.Windows.Forms.CheckBox();
			chkTitle = new System.Windows.Forms.CheckBox();
			chkVolume = new System.Windows.Forms.CheckBox();
			chkYear = new System.Windows.Forms.CheckBox();
			chkNumber = new System.Windows.Forms.CheckBox();
			chkCount = new System.Windows.Forms.CheckBox();
			chkMonth = new System.Windows.Forms.CheckBox();
			chkWriter = new System.Windows.Forms.CheckBox();
			chkPenciller = new System.Windows.Forms.CheckBox();
			chkPublisher = new System.Windows.Forms.CheckBox();
			chkInker = new System.Windows.Forms.CheckBox();
			chkColorist = new System.Windows.Forms.CheckBox();
			chkLetterer = new System.Windows.Forms.CheckBox();
			chkCover = new System.Windows.Forms.CheckBox();
			chkEditor = new System.Windows.Forms.CheckBox();
			chkGenre = new System.Windows.Forms.CheckBox();
			chkLanguage = new System.Windows.Forms.CheckBox();
			chkImprint = new System.Windows.Forms.CheckBox();
			chkRating = new System.Windows.Forms.CheckBox();
			chkSummary = new System.Windows.Forms.CheckBox();
			chkTags = new System.Windows.Forms.CheckBox();
			chkNotes = new System.Windows.Forms.CheckBox();
			chkManga = new System.Windows.Forms.CheckBox();
			chkAlternateSeries = new System.Windows.Forms.CheckBox();
			chkAlternateNumber = new System.Windows.Forms.CheckBox();
			chkAlternateCount = new System.Windows.Forms.CheckBox();
			chkColor = new System.Windows.Forms.CheckBox();
			chkCommunityRating = new System.Windows.Forms.CheckBox();
			chkAgeRating = new System.Windows.Forms.CheckBox();
			chkFormat = new System.Windows.Forms.CheckBox();
			chkBlackAndWhite = new System.Windows.Forms.CheckBox();
			chkLocations = new System.Windows.Forms.CheckBox();
			chkTeams = new System.Windows.Forms.CheckBox();
			chkWeb = new System.Windows.Forms.CheckBox();
			chkCharacters = new System.Windows.Forms.CheckBox();
			chkBookCollectionStatus = new System.Windows.Forms.CheckBox();
			chkBookNotes = new System.Windows.Forms.CheckBox();
			chkBookCondition = new System.Windows.Forms.CheckBox();
			chkBookLocation = new System.Windows.Forms.CheckBox();
			chkBookOwner = new System.Windows.Forms.CheckBox();
			chkBookStore = new System.Windows.Forms.CheckBox();
			chkBookPrice = new System.Windows.Forms.CheckBox();
			btMarkDefined = new System.Windows.Forms.Button();
			btMarkAll = new System.Windows.Forms.Button();
			btMarkNone = new System.Windows.Forms.Button();
			pageData = new System.Windows.Forms.Panel();
			grpCustom = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			grpCatalog = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			chkReleasedTime = new System.Windows.Forms.CheckBox();
			chkAddedTime = new System.Windows.Forms.CheckBox();
			chkOpenedTime = new System.Windows.Forms.CheckBox();
			chkPageCount = new System.Windows.Forms.CheckBox();
			chkISBN = new System.Windows.Forms.CheckBox();
			chkBookAge = new System.Windows.Forms.CheckBox();
			grpPlotNotes = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			chkScanInformation = new System.Windows.Forms.CheckBox();
			chkReview = new System.Windows.Forms.CheckBox();
			chkMainCharacterOrTeam = new System.Windows.Forms.CheckBox();
			grpArtists = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			grpMain = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			chkDay = new System.Windows.Forms.CheckBox();
			chkSeriesGroup = new System.Windows.Forms.CheckBox();
			chkStoryArc = new System.Windows.Forms.CheckBox();
			chkSeriesComplete = new System.Windows.Forms.CheckBox();
			pageData.SuspendLayout();
			grpCatalog.SuspendLayout();
			grpPlotNotes.SuspendLayout();
			grpArtists.SuspendLayout();
			grpMain.SuspendLayout();
			SuspendLayout();
			btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(473, 389);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 1;
			btCancel.Text = "&Cancel";
			btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(387, 389);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(80, 24);
			btOK.TabIndex = 0;
			btOK.Text = "&OK";
			chkSeries.AutoEllipsis = true;
			chkSeries.Location = new System.Drawing.Point(12, 35);
			chkSeries.Name = "chkSeries";
			chkSeries.Size = new System.Drawing.Size(120, 17);
			chkSeries.TabIndex = 0;
			chkSeries.Tag = "Series";
			chkSeries.Text = "Series";
			chkSeries.UseVisualStyleBackColor = true;
			chkTitle.AutoEllipsis = true;
			chkTitle.Location = new System.Drawing.Point(12, 58);
			chkTitle.Name = "chkTitle";
			chkTitle.Size = new System.Drawing.Size(120, 17);
			chkTitle.TabIndex = 4;
			chkTitle.Tag = "Title";
			chkTitle.Text = "Title";
			chkTitle.UseVisualStyleBackColor = true;
			chkVolume.AutoEllipsis = true;
			chkVolume.Location = new System.Drawing.Point(138, 35);
			chkVolume.Name = "chkVolume";
			chkVolume.Size = new System.Drawing.Size(120, 17);
			chkVolume.TabIndex = 1;
			chkVolume.Tag = "Volume";
			chkVolume.Text = "Volume";
			chkVolume.UseVisualStyleBackColor = true;
			chkYear.AutoEllipsis = true;
			chkYear.Location = new System.Drawing.Point(138, 58);
			chkYear.Name = "chkYear";
			chkYear.Size = new System.Drawing.Size(120, 17);
			chkYear.TabIndex = 5;
			chkYear.Tag = "Year";
			chkYear.Text = "Year";
			chkYear.UseVisualStyleBackColor = true;
			chkNumber.AutoEllipsis = true;
			chkNumber.Location = new System.Drawing.Point(264, 35);
			chkNumber.Name = "chkNumber";
			chkNumber.Size = new System.Drawing.Size(120, 17);
			chkNumber.TabIndex = 2;
			chkNumber.Tag = "Number";
			chkNumber.Text = "Number";
			chkNumber.UseVisualStyleBackColor = true;
			chkCount.AutoEllipsis = true;
			chkCount.Location = new System.Drawing.Point(398, 35);
			chkCount.Name = "chkCount";
			chkCount.Size = new System.Drawing.Size(120, 17);
			chkCount.TabIndex = 3;
			chkCount.Tag = "Count";
			chkCount.Text = "Total Number";
			chkCount.UseVisualStyleBackColor = true;
			chkMonth.AutoEllipsis = true;
			chkMonth.Location = new System.Drawing.Point(264, 58);
			chkMonth.Name = "chkMonth";
			chkMonth.Size = new System.Drawing.Size(120, 17);
			chkMonth.TabIndex = 6;
			chkMonth.Tag = "Month";
			chkMonth.Text = "Month";
			chkMonth.UseVisualStyleBackColor = true;
			chkWriter.AutoEllipsis = true;
			chkWriter.Location = new System.Drawing.Point(12, 38);
			chkWriter.Name = "chkWriter";
			chkWriter.Size = new System.Drawing.Size(120, 17);
			chkWriter.TabIndex = 0;
			chkWriter.Tag = "Writer";
			chkWriter.Text = "Writer";
			chkWriter.UseVisualStyleBackColor = true;
			chkPenciller.AutoEllipsis = true;
			chkPenciller.Location = new System.Drawing.Point(138, 38);
			chkPenciller.Name = "chkPenciller";
			chkPenciller.Size = new System.Drawing.Size(120, 17);
			chkPenciller.TabIndex = 1;
			chkPenciller.Tag = "Penciller";
			chkPenciller.Text = "Penciller";
			chkPenciller.UseVisualStyleBackColor = true;
			chkPublisher.AutoEllipsis = true;
			chkPublisher.Location = new System.Drawing.Point(264, 128);
			chkPublisher.Name = "chkPublisher";
			chkPublisher.Size = new System.Drawing.Size(120, 17);
			chkPublisher.TabIndex = 16;
			chkPublisher.Tag = "Publisher";
			chkPublisher.Text = "Publisher";
			chkPublisher.UseVisualStyleBackColor = true;
			chkInker.AutoEllipsis = true;
			chkInker.Location = new System.Drawing.Point(264, 38);
			chkInker.Name = "chkInker";
			chkInker.Size = new System.Drawing.Size(120, 17);
			chkInker.TabIndex = 2;
			chkInker.Tag = "Inker";
			chkInker.Text = "Inker";
			chkInker.UseVisualStyleBackColor = true;
			chkColorist.AutoEllipsis = true;
			chkColorist.Location = new System.Drawing.Point(398, 38);
			chkColorist.Name = "chkColorist";
			chkColorist.Size = new System.Drawing.Size(120, 17);
			chkColorist.TabIndex = 3;
			chkColorist.Tag = "Colorist";
			chkColorist.Text = "Colorist";
			chkColorist.UseVisualStyleBackColor = true;
			chkLetterer.AutoEllipsis = true;
			chkLetterer.Location = new System.Drawing.Point(12, 61);
			chkLetterer.Name = "chkLetterer";
			chkLetterer.Size = new System.Drawing.Size(120, 17);
			chkLetterer.TabIndex = 4;
			chkLetterer.Tag = "Letterer";
			chkLetterer.Text = "Letterer";
			chkLetterer.UseVisualStyleBackColor = true;
			chkCover.AutoEllipsis = true;
			chkCover.Location = new System.Drawing.Point(138, 61);
			chkCover.Name = "chkCover";
			chkCover.Size = new System.Drawing.Size(120, 17);
			chkCover.TabIndex = 5;
			chkCover.Tag = "CoverArtist";
			chkCover.Text = "Cover Artist";
			chkCover.UseVisualStyleBackColor = true;
			chkEditor.AutoEllipsis = true;
			chkEditor.Location = new System.Drawing.Point(264, 61);
			chkEditor.Name = "chkEditor";
			chkEditor.Size = new System.Drawing.Size(120, 17);
			chkEditor.TabIndex = 6;
			chkEditor.Tag = "Editor";
			chkEditor.Text = "Editor";
			chkEditor.UseVisualStyleBackColor = true;
			chkGenre.AutoEllipsis = true;
			chkGenre.Location = new System.Drawing.Point(264, 151);
			chkGenre.Name = "chkGenre";
			chkGenre.Size = new System.Drawing.Size(120, 17);
			chkGenre.TabIndex = 20;
			chkGenre.Tag = "Genre";
			chkGenre.Text = "Genre";
			chkGenre.UseVisualStyleBackColor = true;
			chkLanguage.AutoEllipsis = true;
			chkLanguage.Location = new System.Drawing.Point(138, 151);
			chkLanguage.Name = "chkLanguage";
			chkLanguage.Size = new System.Drawing.Size(120, 17);
			chkLanguage.TabIndex = 19;
			chkLanguage.Tag = "LanguageISO";
			chkLanguage.Text = "Language";
			chkLanguage.UseVisualStyleBackColor = true;
			chkImprint.AutoEllipsis = true;
			chkImprint.Location = new System.Drawing.Point(12, 151);
			chkImprint.Name = "chkImprint";
			chkImprint.Size = new System.Drawing.Size(120, 17);
			chkImprint.TabIndex = 18;
			chkImprint.Tag = "Imprint";
			chkImprint.Text = "Imprint";
			chkImprint.UseVisualStyleBackColor = true;
			chkRating.AutoEllipsis = true;
			chkRating.Location = new System.Drawing.Point(138, 174);
			chkRating.Name = "chkRating";
			chkRating.Size = new System.Drawing.Size(120, 17);
			chkRating.TabIndex = 23;
			chkRating.Tag = "Rating";
			chkRating.Text = "My Rating";
			chkRating.UseVisualStyleBackColor = true;
			chkSummary.AutoEllipsis = true;
			chkSummary.Location = new System.Drawing.Point(12, 59);
			chkSummary.Name = "chkSummary";
			chkSummary.Size = new System.Drawing.Size(120, 17);
			chkSummary.TabIndex = 4;
			chkSummary.Tag = "Summary";
			chkSummary.Text = "Summary";
			chkSummary.UseVisualStyleBackColor = true;
			chkTags.AutoEllipsis = true;
			chkTags.Location = new System.Drawing.Point(12, 174);
			chkTags.Name = "chkTags";
			chkTags.Size = new System.Drawing.Size(120, 17);
			chkTags.TabIndex = 22;
			chkTags.Tag = "Tags";
			chkTags.Text = "Tags";
			chkTags.UseVisualStyleBackColor = true;
			chkNotes.AutoEllipsis = true;
			chkNotes.Location = new System.Drawing.Point(138, 59);
			chkNotes.Name = "chkNotes";
			chkNotes.Size = new System.Drawing.Size(120, 17);
			chkNotes.TabIndex = 5;
			chkNotes.Tag = "Notes";
			chkNotes.Text = "Notes";
			chkNotes.UseVisualStyleBackColor = true;
			chkManga.AutoEllipsis = true;
			chkManga.Location = new System.Drawing.Point(398, 128);
			chkManga.Name = "chkManga";
			chkManga.Size = new System.Drawing.Size(120, 17);
			chkManga.TabIndex = 17;
			chkManga.Tag = "Manga";
			chkManga.Text = "Manga";
			chkManga.UseVisualStyleBackColor = true;
			chkAlternateSeries.AutoEllipsis = true;
			chkAlternateSeries.Location = new System.Drawing.Point(138, 81);
			chkAlternateSeries.Name = "chkAlternateSeries";
			chkAlternateSeries.Size = new System.Drawing.Size(120, 17);
			chkAlternateSeries.TabIndex = 9;
			chkAlternateSeries.Tag = "AlternateSeries";
			chkAlternateSeries.Text = "Alt. Series";
			chkAlternateSeries.UseVisualStyleBackColor = true;
			chkAlternateNumber.AutoEllipsis = true;
			chkAlternateNumber.Location = new System.Drawing.Point(264, 81);
			chkAlternateNumber.Name = "chkAlternateNumber";
			chkAlternateNumber.Size = new System.Drawing.Size(120, 17);
			chkAlternateNumber.TabIndex = 10;
			chkAlternateNumber.Tag = "AlternateNumber";
			chkAlternateNumber.Text = "Alt. Number";
			chkAlternateNumber.UseVisualStyleBackColor = true;
			chkAlternateCount.AutoEllipsis = true;
			chkAlternateCount.Location = new System.Drawing.Point(398, 81);
			chkAlternateCount.Name = "chkAlternateCount";
			chkAlternateCount.Size = new System.Drawing.Size(120, 17);
			chkAlternateCount.TabIndex = 11;
			chkAlternateCount.Tag = "AlternateCount";
			chkAlternateCount.Text = "Alt. Total Number";
			chkAlternateCount.UseVisualStyleBackColor = true;
			chkColor.AutoEllipsis = true;
			chkColor.Location = new System.Drawing.Point(398, 174);
			chkColor.Name = "chkColor";
			chkColor.Size = new System.Drawing.Size(120, 17);
			chkColor.TabIndex = 25;
			chkColor.Tag = "ColorAdjustment";
			chkColor.Text = "Color Adjustment";
			chkColor.UseVisualStyleBackColor = true;
			chkCommunityRating.AutoEllipsis = true;
			chkCommunityRating.Location = new System.Drawing.Point(264, 174);
			chkCommunityRating.Name = "chkCommunityRating";
			chkCommunityRating.Size = new System.Drawing.Size(120, 17);
			chkCommunityRating.TabIndex = 24;
			chkCommunityRating.Tag = "CommunityRating";
			chkCommunityRating.Text = "Community Rating";
			chkCommunityRating.UseVisualStyleBackColor = true;
			chkAgeRating.AutoEllipsis = true;
			chkAgeRating.Location = new System.Drawing.Point(138, 128);
			chkAgeRating.Name = "chkAgeRating";
			chkAgeRating.Size = new System.Drawing.Size(120, 17);
			chkAgeRating.TabIndex = 15;
			chkAgeRating.Tag = "AgeRating";
			chkAgeRating.Text = "Age Rating";
			chkAgeRating.UseVisualStyleBackColor = true;
			chkFormat.AutoEllipsis = true;
			chkFormat.Location = new System.Drawing.Point(12, 128);
			chkFormat.Name = "chkFormat";
			chkFormat.Size = new System.Drawing.Size(120, 17);
			chkFormat.TabIndex = 14;
			chkFormat.Tag = "Format";
			chkFormat.Text = "Format";
			chkFormat.UseVisualStyleBackColor = true;
			chkBlackAndWhite.AutoEllipsis = true;
			chkBlackAndWhite.Location = new System.Drawing.Point(398, 151);
			chkBlackAndWhite.Name = "chkBlackAndWhite";
			chkBlackAndWhite.Size = new System.Drawing.Size(120, 17);
			chkBlackAndWhite.TabIndex = 21;
			chkBlackAndWhite.Tag = "BlackAndWhite";
			chkBlackAndWhite.Text = "Black and White";
			chkBlackAndWhite.UseVisualStyleBackColor = true;
			chkLocations.AutoEllipsis = true;
			chkLocations.Location = new System.Drawing.Point(398, 36);
			chkLocations.Name = "chkLocations";
			chkLocations.Size = new System.Drawing.Size(120, 17);
			chkLocations.TabIndex = 3;
			chkLocations.Tag = "Locations";
			chkLocations.Text = "Locations";
			chkLocations.UseVisualStyleBackColor = true;
			chkTeams.AutoEllipsis = true;
			chkTeams.Location = new System.Drawing.Point(264, 36);
			chkTeams.Name = "chkTeams";
			chkTeams.Size = new System.Drawing.Size(120, 17);
			chkTeams.TabIndex = 2;
			chkTeams.Tag = "Teams";
			chkTeams.Text = "Teams";
			chkTeams.UseVisualStyleBackColor = true;
			chkWeb.AutoEllipsis = true;
			chkWeb.Location = new System.Drawing.Point(138, 82);
			chkWeb.Name = "chkWeb";
			chkWeb.Size = new System.Drawing.Size(120, 17);
			chkWeb.TabIndex = 8;
			chkWeb.Tag = "Web";
			chkWeb.Text = "Web";
			chkWeb.UseVisualStyleBackColor = true;
			chkCharacters.AutoEllipsis = true;
			chkCharacters.Location = new System.Drawing.Point(138, 36);
			chkCharacters.Name = "chkCharacters";
			chkCharacters.Size = new System.Drawing.Size(120, 17);
			chkCharacters.TabIndex = 1;
			chkCharacters.Tag = "Characters";
			chkCharacters.Text = "Characters";
			chkCharacters.UseVisualStyleBackColor = true;
			chkBookCollectionStatus.AutoEllipsis = true;
			chkBookCollectionStatus.Location = new System.Drawing.Point(264, 59);
			chkBookCollectionStatus.Name = "chkBookCollectionStatus";
			chkBookCollectionStatus.Size = new System.Drawing.Size(120, 17);
			chkBookCollectionStatus.TabIndex = 6;
			chkBookCollectionStatus.Tag = "BookCollectionStatus";
			chkBookCollectionStatus.Text = "Collection Status";
			chkBookCollectionStatus.UseVisualStyleBackColor = true;
			chkBookNotes.AutoEllipsis = true;
			chkBookNotes.Location = new System.Drawing.Point(138, 59);
			chkBookNotes.Name = "chkBookNotes";
			chkBookNotes.Size = new System.Drawing.Size(120, 17);
			chkBookNotes.TabIndex = 5;
			chkBookNotes.Tag = "BookNotes";
			chkBookNotes.Text = "Notes";
			chkBookNotes.UseVisualStyleBackColor = true;
			chkBookCondition.AutoEllipsis = true;
			chkBookCondition.Location = new System.Drawing.Point(12, 59);
			chkBookCondition.Name = "chkBookCondition";
			chkBookCondition.Size = new System.Drawing.Size(120, 17);
			chkBookCondition.TabIndex = 4;
			chkBookCondition.Tag = "BookCondition";
			chkBookCondition.Text = "Condition";
			chkBookCondition.UseVisualStyleBackColor = true;
			chkBookLocation.AutoEllipsis = true;
			chkBookLocation.Location = new System.Drawing.Point(264, 36);
			chkBookLocation.Name = "chkBookLocation";
			chkBookLocation.Size = new System.Drawing.Size(120, 17);
			chkBookLocation.TabIndex = 2;
			chkBookLocation.Tag = "BookLocation";
			chkBookLocation.Text = "Location";
			chkBookLocation.UseVisualStyleBackColor = true;
			chkBookOwner.AutoEllipsis = true;
			chkBookOwner.Location = new System.Drawing.Point(398, 59);
			chkBookOwner.Name = "chkBookOwner";
			chkBookOwner.Size = new System.Drawing.Size(120, 17);
			chkBookOwner.TabIndex = 7;
			chkBookOwner.Tag = "BookOwner";
			chkBookOwner.Text = "Owner";
			chkBookOwner.UseVisualStyleBackColor = true;
			chkBookStore.AutoEllipsis = true;
			chkBookStore.Location = new System.Drawing.Point(12, 36);
			chkBookStore.Name = "chkBookStore";
			chkBookStore.Size = new System.Drawing.Size(120, 17);
			chkBookStore.TabIndex = 0;
			chkBookStore.Tag = "BookStore";
			chkBookStore.Text = "Store";
			chkBookStore.UseVisualStyleBackColor = true;
			chkBookPrice.AutoEllipsis = true;
			chkBookPrice.Location = new System.Drawing.Point(138, 36);
			chkBookPrice.Name = "chkBookPrice";
			chkBookPrice.Size = new System.Drawing.Size(120, 17);
			chkBookPrice.TabIndex = 1;
			chkBookPrice.Tag = "BookPrice";
			chkBookPrice.Text = "Price";
			chkBookPrice.UseVisualStyleBackColor = true;
			btMarkDefined.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btMarkDefined.Location = new System.Drawing.Point(87, 389);
			btMarkDefined.Name = "btMarkDefined";
			btMarkDefined.Size = new System.Drawing.Size(70, 23);
			btMarkDefined.TabIndex = 3;
			btMarkDefined.Text = "Only &Set";
			btMarkDefined.UseVisualStyleBackColor = true;
			btMarkDefined.Click += new System.EventHandler(btMarkDefined_Click);
			btMarkAll.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btMarkAll.Location = new System.Drawing.Point(11, 389);
			btMarkAll.Name = "btMarkAll";
			btMarkAll.Size = new System.Drawing.Size(70, 23);
			btMarkAll.TabIndex = 2;
			btMarkAll.Text = "&All";
			btMarkAll.UseVisualStyleBackColor = true;
			btMarkAll.Click += new System.EventHandler(btMarkAll_Click);
			btMarkNone.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btMarkNone.Location = new System.Drawing.Point(163, 389);
			btMarkNone.Name = "btMarkNone";
			btMarkNone.Size = new System.Drawing.Size(70, 23);
			btMarkNone.TabIndex = 4;
			btMarkNone.Text = "&Clear";
			btMarkNone.UseVisualStyleBackColor = true;
			btMarkNone.Click += new System.EventHandler(btMarkNone_Click);
			pageData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			pageData.AutoScroll = true;
			pageData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			pageData.Controls.Add(grpCustom);
			pageData.Controls.Add(grpCatalog);
			pageData.Controls.Add(grpPlotNotes);
			pageData.Controls.Add(grpArtists);
			pageData.Controls.Add(grpMain);
			pageData.Location = new System.Drawing.Point(12, 12);
			pageData.Name = "pageData";
			pageData.Size = new System.Drawing.Size(541, 371);
			pageData.TabIndex = 0;
			grpCustom.Dock = System.Windows.Forms.DockStyle.Top;
			grpCustom.Location = new System.Drawing.Point(0, 548);
			grpCustom.Name = "grpCustom";
			grpCustom.Size = new System.Drawing.Size(522, 90);
			grpCustom.TabIndex = 4;
			grpCustom.Text = "Custom";
			grpCatalog.Controls.Add(chkReleasedTime);
			grpCatalog.Controls.Add(chkAddedTime);
			grpCatalog.Controls.Add(chkOpenedTime);
			grpCatalog.Controls.Add(chkPageCount);
			grpCatalog.Controls.Add(chkISBN);
			grpCatalog.Controls.Add(chkBookAge);
			grpCatalog.Controls.Add(chkBookNotes);
			grpCatalog.Controls.Add(chkBookCollectionStatus);
			grpCatalog.Controls.Add(chkBookStore);
			grpCatalog.Controls.Add(chkBookPrice);
			grpCatalog.Controls.Add(chkBookCondition);
			grpCatalog.Controls.Add(chkBookOwner);
			grpCatalog.Controls.Add(chkBookLocation);
			grpCatalog.Dock = System.Windows.Forms.DockStyle.Top;
			grpCatalog.Location = new System.Drawing.Point(0, 415);
			grpCatalog.Name = "grpCatalog";
			grpCatalog.Size = new System.Drawing.Size(522, 133);
			grpCatalog.TabIndex = 3;
			grpCatalog.Text = "Catalog";
			chkReleasedTime.AutoEllipsis = true;
			chkReleasedTime.Location = new System.Drawing.Point(12, 103);
			chkReleasedTime.Name = "chkReleasedTime";
			chkReleasedTime.Size = new System.Drawing.Size(120, 17);
			chkReleasedTime.TabIndex = 10;
			chkReleasedTime.Tag = "ReleasedTime";
			chkReleasedTime.Text = "Released";
			chkReleasedTime.UseVisualStyleBackColor = true;
			chkAddedTime.AutoEllipsis = true;
			chkAddedTime.Location = new System.Drawing.Point(138, 103);
			chkAddedTime.Name = "chkAddedTime";
			chkAddedTime.Size = new System.Drawing.Size(120, 17);
			chkAddedTime.TabIndex = 11;
			chkAddedTime.Tag = "AddedTime";
			chkAddedTime.Text = "Added/Purchased";
			chkAddedTime.UseVisualStyleBackColor = true;
			chkOpenedTime.AutoEllipsis = true;
			chkOpenedTime.Location = new System.Drawing.Point(264, 103);
			chkOpenedTime.Name = "chkOpenedTime";
			chkOpenedTime.Size = new System.Drawing.Size(120, 17);
			chkOpenedTime.TabIndex = 12;
			chkOpenedTime.Tag = "OpenedTime";
			chkOpenedTime.Text = "Opened/Read";
			chkOpenedTime.UseVisualStyleBackColor = true;
			chkPageCount.AutoEllipsis = true;
			chkPageCount.Location = new System.Drawing.Point(138, 82);
			chkPageCount.Name = "chkPageCount";
			chkPageCount.Size = new System.Drawing.Size(120, 17);
			chkPageCount.TabIndex = 9;
			chkPageCount.Tag = "PagesCount";
			chkPageCount.Text = "Pages";
			chkPageCount.UseVisualStyleBackColor = true;
			chkISBN.AutoEllipsis = true;
			chkISBN.Location = new System.Drawing.Point(12, 82);
			chkISBN.Name = "chkISBN";
			chkISBN.Size = new System.Drawing.Size(120, 17);
			chkISBN.TabIndex = 8;
			chkISBN.Tag = "ISBN";
			chkISBN.Text = "ISBN";
			chkISBN.UseVisualStyleBackColor = true;
			chkBookAge.AutoEllipsis = true;
			chkBookAge.Location = new System.Drawing.Point(398, 36);
			chkBookAge.Name = "chkBookAge";
			chkBookAge.Size = new System.Drawing.Size(120, 17);
			chkBookAge.TabIndex = 3;
			chkBookAge.Tag = "BookAge";
			chkBookAge.Text = "Age";
			chkBookAge.UseVisualStyleBackColor = true;
			grpPlotNotes.Controls.Add(chkScanInformation);
			grpPlotNotes.Controls.Add(chkReview);
			grpPlotNotes.Controls.Add(chkNotes);
			grpPlotNotes.Controls.Add(chkSummary);
			grpPlotNotes.Controls.Add(chkLocations);
			grpPlotNotes.Controls.Add(chkWeb);
			grpPlotNotes.Controls.Add(chkMainCharacterOrTeam);
			grpPlotNotes.Controls.Add(chkCharacters);
			grpPlotNotes.Controls.Add(chkTeams);
			grpPlotNotes.Dock = System.Windows.Forms.DockStyle.Top;
			grpPlotNotes.Location = new System.Drawing.Point(0, 297);
			grpPlotNotes.Name = "grpPlotNotes";
			grpPlotNotes.Size = new System.Drawing.Size(522, 118);
			grpPlotNotes.TabIndex = 2;
			grpPlotNotes.Text = "Plot & Notes";
			chkScanInformation.AutoEllipsis = true;
			chkScanInformation.Location = new System.Drawing.Point(12, 82);
			chkScanInformation.Name = "chkScanInformation";
			chkScanInformation.Size = new System.Drawing.Size(120, 17);
			chkScanInformation.TabIndex = 7;
			chkScanInformation.Tag = "ScanInformation";
			chkScanInformation.Text = "Scan Information";
			chkScanInformation.UseVisualStyleBackColor = true;
			chkReview.AutoEllipsis = true;
			chkReview.Location = new System.Drawing.Point(264, 59);
			chkReview.Name = "chkReview";
			chkReview.Size = new System.Drawing.Size(120, 17);
			chkReview.TabIndex = 6;
			chkReview.Tag = "Review";
			chkReview.Text = "Review";
			chkReview.UseVisualStyleBackColor = true;
			chkMainCharacterOrTeam.AutoEllipsis = true;
			chkMainCharacterOrTeam.Location = new System.Drawing.Point(12, 36);
			chkMainCharacterOrTeam.Name = "chkMainCharacterOrTeam";
			chkMainCharacterOrTeam.Size = new System.Drawing.Size(120, 17);
			chkMainCharacterOrTeam.TabIndex = 0;
			chkMainCharacterOrTeam.Tag = "MainCharacterOrTeam";
			chkMainCharacterOrTeam.Text = "Main Character";
			chkMainCharacterOrTeam.UseVisualStyleBackColor = true;
			grpArtists.Controls.Add(chkEditor);
			grpArtists.Controls.Add(chkCover);
			grpArtists.Controls.Add(chkLetterer);
			grpArtists.Controls.Add(chkColorist);
			grpArtists.Controls.Add(chkInker);
			grpArtists.Controls.Add(chkPenciller);
			grpArtists.Controls.Add(chkWriter);
			grpArtists.Dock = System.Windows.Forms.DockStyle.Top;
			grpArtists.Location = new System.Drawing.Point(0, 203);
			grpArtists.Name = "grpArtists";
			grpArtists.Size = new System.Drawing.Size(522, 94);
			grpArtists.TabIndex = 1;
			grpArtists.Text = "Artists / People Involved";
			grpMain.Controls.Add(chkDay);
			grpMain.Controls.Add(chkSeriesGroup);
			grpMain.Controls.Add(chkStoryArc);
			grpMain.Controls.Add(chkSeriesComplete);
			grpMain.Controls.Add(chkCommunityRating);
			grpMain.Controls.Add(chkColor);
			grpMain.Controls.Add(chkRating);
			grpMain.Controls.Add(chkSeries);
			grpMain.Controls.Add(chkVolume);
			grpMain.Controls.Add(chkTags);
			grpMain.Controls.Add(chkBlackAndWhite);
			grpMain.Controls.Add(chkNumber);
			grpMain.Controls.Add(chkLanguage);
			grpMain.Controls.Add(chkAlternateCount);
			grpMain.Controls.Add(chkAgeRating);
			grpMain.Controls.Add(chkImprint);
			grpMain.Controls.Add(chkGenre);
			grpMain.Controls.Add(chkManga);
			grpMain.Controls.Add(chkPublisher);
			grpMain.Controls.Add(chkCount);
			grpMain.Controls.Add(chkFormat);
			grpMain.Controls.Add(chkAlternateSeries);
			grpMain.Controls.Add(chkAlternateNumber);
			grpMain.Controls.Add(chkTitle);
			grpMain.Controls.Add(chkMonth);
			grpMain.Controls.Add(chkYear);
			grpMain.Dock = System.Windows.Forms.DockStyle.Top;
			grpMain.Location = new System.Drawing.Point(0, 0);
			grpMain.Name = "grpMain";
			grpMain.Size = new System.Drawing.Size(522, 203);
			grpMain.TabIndex = 0;
			grpMain.Text = "Main";
			chkDay.AutoEllipsis = true;
			chkDay.Location = new System.Drawing.Point(398, 58);
			chkDay.Name = "chkDay";
			chkDay.Size = new System.Drawing.Size(107, 17);
			chkDay.TabIndex = 7;
			chkDay.Tag = "Day";
			chkDay.Text = "Day";
			chkDay.UseVisualStyleBackColor = true;
			chkSeriesGroup.AutoEllipsis = true;
			chkSeriesGroup.Location = new System.Drawing.Point(138, 104);
			chkSeriesGroup.Name = "chkSeriesGroup";
			chkSeriesGroup.Size = new System.Drawing.Size(120, 17);
			chkSeriesGroup.TabIndex = 13;
			chkSeriesGroup.Tag = "SeriesGroup";
			chkSeriesGroup.Text = "Series Group";
			chkSeriesGroup.UseVisualStyleBackColor = true;
			chkStoryArc.AutoEllipsis = true;
			chkStoryArc.Location = new System.Drawing.Point(12, 104);
			chkStoryArc.Name = "chkStoryArc";
			chkStoryArc.Size = new System.Drawing.Size(120, 17);
			chkStoryArc.TabIndex = 12;
			chkStoryArc.Tag = "StoryArc";
			chkStoryArc.Text = "Story Arc";
			chkStoryArc.UseVisualStyleBackColor = true;
			chkSeriesComplete.AutoEllipsis = true;
			chkSeriesComplete.Location = new System.Drawing.Point(12, 81);
			chkSeriesComplete.Name = "chkSeriesComplete";
			chkSeriesComplete.Size = new System.Drawing.Size(120, 17);
			chkSeriesComplete.TabIndex = 8;
			chkSeriesComplete.Tag = "SeriesComplete";
			chkSeriesComplete.Text = "Series complete";
			chkSeriesComplete.UseVisualStyleBackColor = true;
			base.AcceptButton = btOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(563, 422);
			base.Controls.Add(pageData);
			base.Controls.Add(btMarkNone);
			base.Controls.Add(btMarkAll);
			base.Controls.Add(btMarkDefined);
			base.Controls.Add(btCancel);
			base.Controls.Add(btOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ComicDataPasteDialog";
			RightToLeftLayout = true;
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Paste Data to {0} Books";
			pageData.ResumeLayout(false);
			grpCatalog.ResumeLayout(false);
			grpPlotNotes.ResumeLayout(false);
			grpArtists.ResumeLayout(false);
			grpMain.ResumeLayout(false);
			ResumeLayout(false);
		}

		private IContainer components;

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
	}
}
