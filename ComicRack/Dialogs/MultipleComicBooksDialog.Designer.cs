using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine.Controls;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class MultipleComicBooksDialog
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
			txCount = new cYo.Common.Windows.Forms.TextBoxEx();
			txNumber = new cYo.Common.Windows.Forms.TextBoxEx();
			txVolume = new cYo.Common.Windows.Forms.TextBoxEx();
			txYear = new cYo.Common.Windows.Forms.TextBoxEx();
			txSeries = new cYo.Common.Windows.Forms.TextBoxEx();
			txTitle = new cYo.Common.Windows.Forms.TextBoxEx();
			txWriter = new cYo.Common.Windows.Forms.TextBoxEx();
			txSummary = new cYo.Common.Windows.Forms.TextBoxEx();
			txColorist = new cYo.Common.Windows.Forms.TextBoxEx();
			txInker = new cYo.Common.Windows.Forms.TextBoxEx();
			txPenciller = new cYo.Common.Windows.Forms.TextBoxEx();
			labelSummary = new System.Windows.Forms.Label();
			labelGenre = new System.Windows.Forms.Label();
			labelColorist = new System.Windows.Forms.Label();
			labelPublisher = new System.Windows.Forms.Label();
			labelInker = new System.Windows.Forms.Label();
			labelCount = new System.Windows.Forms.Label();
			labelVolume = new System.Windows.Forms.Label();
			labelNumber = new System.Windows.Forms.Label();
			labelYear = new System.Windows.Forms.Label();
			labelSeries = new System.Windows.Forms.Label();
			labelWriter = new System.Windows.Forms.Label();
			labelPenciller = new System.Windows.Forms.Label();
			labelTitle = new System.Windows.Forms.Label();
			btCancel = new System.Windows.Forms.Button();
			btOK = new System.Windows.Forms.Button();
			txRating = new cYo.Projects.ComicRack.Engine.Controls.RatingControl();
			labelRating = new System.Windows.Forms.Label();
			txEditor = new cYo.Common.Windows.Forms.TextBoxEx();
			txCoverArtist = new cYo.Common.Windows.Forms.TextBoxEx();
			txLetterer = new cYo.Common.Windows.Forms.TextBoxEx();
			labelEditor = new System.Windows.Forms.Label();
			labelCoverArtist = new System.Windows.Forms.Label();
			labelLetterer = new System.Windows.Forms.Label();
			cbPublisher = new System.Windows.Forms.ComboBox();
			labelAlternateSeries = new System.Windows.Forms.Label();
			labelAlternateNumber = new System.Windows.Forms.Label();
			labelAlternateCount = new System.Windows.Forms.Label();
			txAlternateSeries = new cYo.Common.Windows.Forms.TextBoxEx();
			txAlternateNumber = new cYo.Common.Windows.Forms.TextBoxEx();
			txAlternateCount = new cYo.Common.Windows.Forms.TextBoxEx();
			labelMonth = new System.Windows.Forms.Label();
			txMonth = new cYo.Common.Windows.Forms.TextBoxEx();
			labelTags = new System.Windows.Forms.Label();
			txTags = new cYo.Common.Windows.Forms.TextBoxEx();
			cbImprint = new System.Windows.Forms.ComboBox();
			labelImprint = new System.Windows.Forms.Label();
			cbLanguage = new cYo.Common.Windows.Forms.LanguageComboBox();
			labelLanguage = new System.Windows.Forms.Label();
			cbManga = new System.Windows.Forms.ComboBox();
			labelManga = new System.Windows.Forms.Label();
			cbBlackAndWhite = new System.Windows.Forms.ComboBox();
			labelBlackAndWhite = new System.Windows.Forms.Label();
			cbFormat = new cYo.Common.Windows.Forms.ComboBoxEx();
			labelFormat = new System.Windows.Forms.Label();
			cbEnableProposed = new System.Windows.Forms.ComboBox();
			labelEnableProposed = new System.Windows.Forms.Label();
			cbAgeRating = new cYo.Common.Windows.Forms.ComboBoxEx();
			labelAgeRating = new System.Windows.Forms.Label();
			txNotes = new cYo.Common.Windows.Forms.TextBoxEx();
			labelNotes = new System.Windows.Forms.Label();
			txCharacters = new cYo.Common.Windows.Forms.TextBoxEx();
			labelCharacters = new System.Windows.Forms.Label();
			txGenre = new cYo.Common.Windows.Forms.TextBoxEx();
			txWeb = new cYo.Common.Windows.Forms.TextBoxEx();
			labelWeb = new System.Windows.Forms.Label();
			txTeams = new cYo.Common.Windows.Forms.TextBoxEx();
			labelTeams = new System.Windows.Forms.Label();
			txLocations = new cYo.Common.Windows.Forms.TextBoxEx();
			labelLocations = new System.Windows.Forms.Label();
			txCommunityRating = new cYo.Projects.ComicRack.Engine.Controls.RatingControl();
			labelCommunityRating = new System.Windows.Forms.Label();
			pageData = new System.Windows.Forms.Panel();
			grpCustom = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			textCustomField = new cYo.Common.Windows.Forms.TextBoxEx();
			labelCustomField = new System.Windows.Forms.Label();
			grpCatalog = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
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
			txBookNotes = new cYo.Common.Windows.Forms.TextBoxEx();
			labelBookNotes = new System.Windows.Forms.Label();
			cbBookLocation = new System.Windows.Forms.ComboBox();
			labelBookLocation = new System.Windows.Forms.Label();
			txCollectionStatus = new cYo.Common.Windows.Forms.TextBoxEx();
			cbBookPrice = new System.Windows.Forms.ComboBox();
			labelBookPrice = new System.Windows.Forms.Label();
			cbBookAge = new System.Windows.Forms.ComboBox();
			labelBookAge = new System.Windows.Forms.Label();
			labelBookCollectionStatus = new System.Windows.Forms.Label();
			cbBookCondition = new System.Windows.Forms.ComboBox();
			labelBookCondition = new System.Windows.Forms.Label();
			cbBookStore = new cYo.Common.Windows.Forms.ComboBoxEx();
			labelBookStore = new System.Windows.Forms.Label();
			cbBookOwner = new System.Windows.Forms.ComboBox();
			labelBookOwner = new System.Windows.Forms.Label();
			grpPlotNotes = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			labelMainCharacterOrTeam = new System.Windows.Forms.Label();
			txMainCharacterOrTeam = new cYo.Common.Windows.Forms.TextBoxEx();
			labelReview = new System.Windows.Forms.Label();
			txReview = new cYo.Common.Windows.Forms.TextBoxEx();
			txScanInformation = new cYo.Common.Windows.Forms.TextBoxEx();
			labelScanInformation = new System.Windows.Forms.Label();
			grpArtists = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			grpMain = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			labelDay = new System.Windows.Forms.Label();
			txDay = new cYo.Common.Windows.Forms.TextBoxEx();
			labelSeriesGroup = new System.Windows.Forms.Label();
			txSeriesGroup = new cYo.Common.Windows.Forms.TextBoxEx();
			labelStoryArc = new System.Windows.Forms.Label();
			txStoryArc = new cYo.Common.Windows.Forms.TextBoxEx();
			cbSeriesComplete = new System.Windows.Forms.ComboBox();
			labelSeriesComplete = new System.Windows.Forms.Label();
			pageData.SuspendLayout();
			grpCustom.SuspendLayout();
			grpCatalog.SuspendLayout();
			grpPlotNotes.SuspendLayout();
			grpArtists.SuspendLayout();
			grpMain.SuspendLayout();
			SuspendLayout();
			txCount.Location = new System.Drawing.Point(395, 51);
			txCount.Name = "txCount";
			txCount.PromptText = "";
			txCount.Size = new System.Drawing.Size(72, 20);
			txCount.TabIndex = 7;
			txNumber.Location = new System.Drawing.Point(318, 50);
			txNumber.Name = "txNumber";
			txNumber.PromptText = "";
			txNumber.Size = new System.Drawing.Size(71, 20);
			txNumber.TabIndex = 5;
			txVolume.Location = new System.Drawing.Point(243, 50);
			txVolume.Name = "txVolume";
			txVolume.PromptText = "";
			txVolume.Size = new System.Drawing.Size(66, 20);
			txVolume.TabIndex = 3;
			txYear.Location = new System.Drawing.Point(243, 87);
			txYear.MaxLength = 4;
			txYear.Name = "txYear";
			txYear.PromptText = "";
			txYear.Size = new System.Drawing.Size(66, 20);
			txYear.TabIndex = 11;
			txSeries.Location = new System.Drawing.Point(11, 50);
			txSeries.Name = "txSeries";
			txSeries.PromptText = "";
			txSeries.Size = new System.Drawing.Size(223, 20);
			txSeries.TabIndex = 1;
			txTitle.Location = new System.Drawing.Point(11, 87);
			txTitle.Name = "txTitle";
			txTitle.PromptText = "";
			txTitle.Size = new System.Drawing.Size(226, 20);
			txTitle.TabIndex = 9;
			txWriter.Location = new System.Drawing.Point(13, 57);
			txWriter.Name = "txWriter";
			txWriter.Size = new System.Drawing.Size(220, 20);
			txWriter.TabIndex = 1;
			txSummary.Location = new System.Drawing.Point(13, 48);
			txSummary.Multiline = true;
			txSummary.Name = "txSummary";
			txSummary.Size = new System.Drawing.Size(220, 55);
			txSummary.TabIndex = 1;
			txColorist.Location = new System.Drawing.Point(245, 93);
			txColorist.Name = "txColorist";
			txColorist.Size = new System.Drawing.Size(220, 20);
			txColorist.TabIndex = 11;
			txInker.Location = new System.Drawing.Point(13, 93);
			txInker.Name = "txInker";
			txInker.Size = new System.Drawing.Size(220, 20);
			txInker.TabIndex = 3;
			txPenciller.Location = new System.Drawing.Point(245, 57);
			txPenciller.Name = "txPenciller";
			txPenciller.Size = new System.Drawing.Size(220, 20);
			txPenciller.TabIndex = 9;
			labelSummary.AutoSize = true;
			labelSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelSummary.Location = new System.Drawing.Point(15, 33);
			labelSummary.Name = "labelSummary";
			labelSummary.Size = new System.Drawing.Size(56, 12);
			labelSummary.TabIndex = 0;
			labelSummary.Text = "Summary:";
			labelGenre.AutoSize = true;
			labelGenre.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelGenre.Location = new System.Drawing.Point(163, 270);
			labelGenre.Name = "labelGenre";
			labelGenre.Size = new System.Drawing.Size(39, 12);
			labelGenre.TabIndex = 42;
			labelGenre.Text = "Genre:";
			labelColorist.AutoSize = true;
			labelColorist.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelColorist.Location = new System.Drawing.Point(243, 80);
			labelColorist.Name = "labelColorist";
			labelColorist.Size = new System.Drawing.Size(49, 12);
			labelColorist.TabIndex = 10;
			labelColorist.Text = "Colorist:";
			labelPublisher.AutoSize = true;
			labelPublisher.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelPublisher.Location = new System.Drawing.Point(9, 236);
			labelPublisher.Name = "labelPublisher";
			labelPublisher.Size = new System.Drawing.Size(56, 12);
			labelPublisher.TabIndex = 34;
			labelPublisher.Text = "Publisher:";
			labelInker.AutoSize = true;
			labelInker.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelInker.Location = new System.Drawing.Point(13, 80);
			labelInker.Name = "labelInker";
			labelInker.Size = new System.Drawing.Size(35, 12);
			labelInker.TabIndex = 2;
			labelInker.Text = "Inker:";
			labelCount.AutoSize = true;
			labelCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelCount.Location = new System.Drawing.Point(396, 36);
			labelCount.Name = "labelCount";
			labelCount.Size = new System.Drawing.Size(19, 12);
			labelCount.TabIndex = 6;
			labelCount.Text = "of:";
			labelVolume.AutoSize = true;
			labelVolume.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelVolume.Location = new System.Drawing.Point(241, 36);
			labelVolume.Name = "labelVolume";
			labelVolume.Size = new System.Drawing.Size(47, 12);
			labelVolume.TabIndex = 2;
			labelVolume.Text = "Volume:";
			labelNumber.AutoSize = true;
			labelNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelNumber.Location = new System.Drawing.Point(316, 36);
			labelNumber.Name = "labelNumber";
			labelNumber.Size = new System.Drawing.Size(48, 12);
			labelNumber.TabIndex = 4;
			labelNumber.Text = "Number:";
			labelYear.AutoSize = true;
			labelYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelYear.Location = new System.Drawing.Point(241, 74);
			labelYear.Name = "labelYear";
			labelYear.Size = new System.Drawing.Size(31, 12);
			labelYear.TabIndex = 10;
			labelYear.Text = "Year:";
			labelSeries.AutoSize = true;
			labelSeries.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelSeries.Location = new System.Drawing.Point(11, 36);
			labelSeries.Name = "labelSeries";
			labelSeries.Size = new System.Drawing.Size(41, 12);
			labelSeries.TabIndex = 0;
			labelSeries.Text = "Series:";
			labelWriter.AutoSize = true;
			labelWriter.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelWriter.Location = new System.Drawing.Point(12, 42);
			labelWriter.Name = "labelWriter";
			labelWriter.Size = new System.Drawing.Size(40, 12);
			labelWriter.TabIndex = 0;
			labelWriter.Text = "Writer:";
			labelPenciller.AutoSize = true;
			labelPenciller.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelPenciller.Location = new System.Drawing.Point(243, 42);
			labelPenciller.Name = "labelPenciller";
			labelPenciller.Size = new System.Drawing.Size(53, 12);
			labelPenciller.TabIndex = 8;
			labelPenciller.Text = "Penciller:";
			labelTitle.AutoSize = true;
			labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelTitle.Location = new System.Drawing.Point(11, 74);
			labelTitle.Name = "labelTitle";
			labelTitle.Size = new System.Drawing.Size(31, 12);
			labelTitle.TabIndex = 8;
			labelTitle.Text = "Title:";
			btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(429, 427);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 71;
			btCancel.Text = "&Cancel";
			btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(346, 427);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(77, 24);
			btOK.TabIndex = 70;
			btOK.Text = "&OK";
			btOK.Click += new System.EventHandler(btOK_Click);
			txRating.DrawText = true;
			txRating.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
			txRating.ForeColor = System.Drawing.SystemColors.GrayText;
			txRating.Location = new System.Drawing.Point(13, 363);
			txRating.Name = "txRating";
			txRating.Rating = 3f;
			txRating.RatingImage = cYo.Projects.ComicRack.Viewer.Properties.Resources.StarYellow;
			txRating.Size = new System.Drawing.Size(223, 20);
			txRating.TabIndex = 49;
			txRating.Text = "3";
			labelRating.AutoSize = true;
			labelRating.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelRating.Location = new System.Drawing.Point(12, 348);
			labelRating.Name = "labelRating";
			labelRating.Size = new System.Drawing.Size(61, 12);
			labelRating.TabIndex = 48;
			labelRating.Text = "My Rating:";
			txEditor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txEditor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txEditor.Location = new System.Drawing.Point(13, 167);
			txEditor.Name = "txEditor";
			txEditor.Size = new System.Drawing.Size(220, 20);
			txEditor.TabIndex = 7;
			txCoverArtist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txCoverArtist.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txCoverArtist.Location = new System.Drawing.Point(245, 130);
			txCoverArtist.Name = "txCoverArtist";
			txCoverArtist.Size = new System.Drawing.Size(220, 20);
			txCoverArtist.TabIndex = 13;
			txLetterer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txLetterer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txLetterer.Location = new System.Drawing.Point(13, 129);
			txLetterer.Name = "txLetterer";
			txLetterer.Size = new System.Drawing.Size(220, 20);
			txLetterer.TabIndex = 5;
			labelEditor.AutoSize = true;
			labelEditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelEditor.Location = new System.Drawing.Point(13, 152);
			labelEditor.Name = "labelEditor";
			labelEditor.Size = new System.Drawing.Size(39, 12);
			labelEditor.TabIndex = 6;
			labelEditor.Text = "Editor:";
			labelCoverArtist.AutoSize = true;
			labelCoverArtist.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelCoverArtist.Location = new System.Drawing.Point(243, 116);
			labelCoverArtist.Name = "labelCoverArtist";
			labelCoverArtist.Size = new System.Drawing.Size(72, 12);
			labelCoverArtist.TabIndex = 12;
			labelCoverArtist.Text = "Cover Artist:";
			labelLetterer.AutoSize = true;
			labelLetterer.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelLetterer.Location = new System.Drawing.Point(12, 116);
			labelLetterer.Name = "labelLetterer";
			labelLetterer.Size = new System.Drawing.Size(49, 12);
			labelLetterer.TabIndex = 4;
			labelLetterer.Text = "Letterer:";
			cbPublisher.FormattingEnabled = true;
			cbPublisher.Location = new System.Drawing.Point(11, 248);
			cbPublisher.Name = "cbPublisher";
			cbPublisher.Size = new System.Drawing.Size(142, 21);
			cbPublisher.TabIndex = 35;
			labelAlternateSeries.AutoSize = true;
			labelAlternateSeries.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelAlternateSeries.Location = new System.Drawing.Point(9, 112);
			labelAlternateSeries.Name = "labelAlternateSeries";
			labelAlternateSeries.Size = new System.Drawing.Size(177, 12);
			labelAlternateSeries.TabIndex = 16;
			labelAlternateSeries.Text = "Alternate Series or Storyline Title:";
			labelAlternateNumber.AutoSize = true;
			labelAlternateNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelAlternateNumber.Location = new System.Drawing.Point(316, 112);
			labelAlternateNumber.Name = "labelAlternateNumber";
			labelAlternateNumber.Size = new System.Drawing.Size(48, 12);
			labelAlternateNumber.TabIndex = 18;
			labelAlternateNumber.Text = "Number:";
			labelAlternateCount.AutoSize = true;
			labelAlternateCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelAlternateCount.Location = new System.Drawing.Point(395, 112);
			labelAlternateCount.Name = "labelAlternateCount";
			labelAlternateCount.Size = new System.Drawing.Size(19, 12);
			labelAlternateCount.TabIndex = 20;
			labelAlternateCount.Text = "of:";
			txAlternateSeries.Location = new System.Drawing.Point(10, 127);
			txAlternateSeries.Name = "txAlternateSeries";
			txAlternateSeries.Size = new System.Drawing.Size(299, 20);
			txAlternateSeries.TabIndex = 17;
			txAlternateNumber.Location = new System.Drawing.Point(318, 127);
			txAlternateNumber.Name = "txAlternateNumber";
			txAlternateNumber.Size = new System.Drawing.Size(70, 20);
			txAlternateNumber.TabIndex = 19;
			txAlternateCount.Location = new System.Drawing.Point(394, 127);
			txAlternateCount.Name = "txAlternateCount";
			txAlternateCount.Size = new System.Drawing.Size(72, 20);
			txAlternateCount.TabIndex = 21;
			labelMonth.AutoSize = true;
			labelMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelMonth.Location = new System.Drawing.Point(316, 74);
			labelMonth.Name = "labelMonth";
			labelMonth.Size = new System.Drawing.Size(41, 12);
			labelMonth.TabIndex = 12;
			labelMonth.Text = "Month:";
			txMonth.Location = new System.Drawing.Point(318, 87);
			txMonth.MaxLength = 2;
			txMonth.Name = "txMonth";
			txMonth.Size = new System.Drawing.Size(71, 20);
			txMonth.TabIndex = 13;
			labelTags.AutoSize = true;
			labelTags.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelTags.Location = new System.Drawing.Point(9, 309);
			labelTags.Name = "labelTags";
			labelTags.Size = new System.Drawing.Size(33, 12);
			labelTags.TabIndex = 46;
			labelTags.Text = "Tags:";
			txTags.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txTags.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txTags.Location = new System.Drawing.Point(11, 324);
			txTags.Name = "txTags";
			txTags.Size = new System.Drawing.Size(298, 20);
			txTags.TabIndex = 47;
			cbImprint.FormattingEnabled = true;
			cbImprint.Location = new System.Drawing.Point(164, 248);
			cbImprint.Name = "cbImprint";
			cbImprint.Size = new System.Drawing.Size(145, 21);
			cbImprint.TabIndex = 37;
			labelImprint.AutoSize = true;
			labelImprint.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelImprint.Location = new System.Drawing.Point(162, 235);
			labelImprint.Name = "labelImprint";
			labelImprint.Size = new System.Drawing.Size(45, 12);
			labelImprint.TabIndex = 36;
			labelImprint.Text = "Imprint:";
			cbLanguage.CultureTypes = System.Globalization.CultureTypes.NeutralCultures;
			cbLanguage.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			cbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbLanguage.FormattingEnabled = true;
			cbLanguage.IntegralHeight = false;
			cbLanguage.Location = new System.Drawing.Point(10, 285);
			cbLanguage.Name = "cbLanguage";
			cbLanguage.SelectedCulture = "";
			cbLanguage.SelectedTwoLetterISOLanguage = "";
			cbLanguage.Size = new System.Drawing.Size(143, 21);
			cbLanguage.TabIndex = 41;
			cbLanguage.TopTwoLetterISOLanguages = null;
			labelLanguage.AutoSize = true;
			labelLanguage.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelLanguage.Location = new System.Drawing.Point(8, 270);
			labelLanguage.Name = "labelLanguage";
			labelLanguage.Size = new System.Drawing.Size(57, 12);
			labelLanguage.TabIndex = 40;
			labelLanguage.Text = "Language:";
			cbManga.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbManga.FormattingEnabled = true;
			cbManga.Location = new System.Drawing.Point(317, 210);
			cbManga.Name = "cbManga";
			cbManga.Size = new System.Drawing.Size(149, 21);
			cbManga.TabIndex = 33;
			labelManga.AutoSize = true;
			labelManga.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelManga.Location = new System.Drawing.Point(315, 198);
			labelManga.Name = "labelManga";
			labelManga.Size = new System.Drawing.Size(43, 12);
			labelManga.TabIndex = 32;
			labelManga.Text = "Manga:";
			cbBlackAndWhite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbBlackAndWhite.FormattingEnabled = true;
			cbBlackAndWhite.Location = new System.Drawing.Point(317, 246);
			cbBlackAndWhite.Name = "cbBlackAndWhite";
			cbBlackAndWhite.Size = new System.Drawing.Size(149, 21);
			cbBlackAndWhite.TabIndex = 39;
			labelBlackAndWhite.AutoSize = true;
			labelBlackAndWhite.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelBlackAndWhite.Location = new System.Drawing.Point(317, 231);
			labelBlackAndWhite.Name = "labelBlackAndWhite";
			labelBlackAndWhite.Size = new System.Drawing.Size(90, 12);
			labelBlackAndWhite.TabIndex = 38;
			labelBlackAndWhite.Text = "Black and White:";
			cbFormat.FormattingEnabled = true;
			cbFormat.Location = new System.Drawing.Point(10, 211);
			cbFormat.Name = "cbFormat";
			cbFormat.PromptText = null;
			cbFormat.Size = new System.Drawing.Size(145, 21);
			cbFormat.TabIndex = 29;
			labelFormat.AutoSize = true;
			labelFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelFormat.Location = new System.Drawing.Point(8, 197);
			labelFormat.Name = "labelFormat";
			labelFormat.Size = new System.Drawing.Size(45, 12);
			labelFormat.TabIndex = 28;
			labelFormat.Text = "Format:";
			cbEnableProposed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbEnableProposed.FormattingEnabled = true;
			cbEnableProposed.Location = new System.Drawing.Point(317, 284);
			cbEnableProposed.Name = "cbEnableProposed";
			cbEnableProposed.Size = new System.Drawing.Size(149, 21);
			cbEnableProposed.TabIndex = 45;
			labelEnableProposed.AutoSize = true;
			labelEnableProposed.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelEnableProposed.Location = new System.Drawing.Point(319, 269);
			labelEnableProposed.Name = "labelEnableProposed";
			labelEnableProposed.Size = new System.Drawing.Size(94, 12);
			labelEnableProposed.TabIndex = 44;
			labelEnableProposed.Text = "Proposed Values:";
			cbAgeRating.FormattingEnabled = true;
			cbAgeRating.Location = new System.Drawing.Point(164, 211);
			cbAgeRating.Name = "cbAgeRating";
			cbAgeRating.PromptText = null;
			cbAgeRating.Size = new System.Drawing.Size(145, 21);
			cbAgeRating.TabIndex = 31;
			labelAgeRating.AutoSize = true;
			labelAgeRating.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelAgeRating.Location = new System.Drawing.Point(161, 196);
			labelAgeRating.Name = "labelAgeRating";
			labelAgeRating.Size = new System.Drawing.Size(65, 12);
			labelAgeRating.TabIndex = 30;
			labelAgeRating.Text = "Age Rating:";
			txNotes.Location = new System.Drawing.Point(13, 123);
			txNotes.Multiline = true;
			txNotes.Name = "txNotes";
			txNotes.Size = new System.Drawing.Size(220, 55);
			txNotes.TabIndex = 7;
			labelNotes.AutoSize = true;
			labelNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelNotes.Location = new System.Drawing.Point(15, 109);
			labelNotes.Name = "labelNotes";
			labelNotes.Size = new System.Drawing.Size(39, 12);
			labelNotes.TabIndex = 6;
			labelNotes.Text = "Notes:";
			txCharacters.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txCharacters.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txCharacters.Location = new System.Drawing.Point(244, 83);
			txCharacters.Name = "txCharacters";
			txCharacters.Size = new System.Drawing.Size(223, 20);
			txCharacters.TabIndex = 5;
			labelCharacters.AutoSize = true;
			labelCharacters.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelCharacters.Location = new System.Drawing.Point(245, 69);
			labelCharacters.Name = "labelCharacters";
			labelCharacters.Size = new System.Drawing.Size(65, 12);
			labelCharacters.TabIndex = 4;
			labelCharacters.Text = "Characters:";
			txGenre.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txGenre.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txGenre.Location = new System.Drawing.Point(164, 285);
			txGenre.Name = "txGenre";
			txGenre.Size = new System.Drawing.Size(145, 20);
			txGenre.TabIndex = 43;
			txWeb.Location = new System.Drawing.Point(245, 231);
			txWeb.Name = "txWeb";
			txWeb.Size = new System.Drawing.Size(221, 20);
			txWeb.TabIndex = 17;
			labelWeb.AutoSize = true;
			labelWeb.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelWeb.Location = new System.Drawing.Point(244, 217);
			labelWeb.Name = "labelWeb";
			labelWeb.Size = new System.Drawing.Size(31, 12);
			labelWeb.TabIndex = 16;
			labelWeb.Text = "Web:";
			txTeams.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txTeams.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txTeams.Location = new System.Drawing.Point(243, 123);
			txTeams.Name = "txTeams";
			txTeams.Size = new System.Drawing.Size(224, 20);
			txTeams.TabIndex = 9;
			labelTeams.AutoSize = true;
			labelTeams.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelTeams.Location = new System.Drawing.Point(244, 110);
			labelTeams.Name = "labelTeams";
			labelTeams.Size = new System.Drawing.Size(42, 12);
			labelTeams.TabIndex = 8;
			labelTeams.Text = "Teams:";
			txLocations.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txLocations.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txLocations.Location = new System.Drawing.Point(243, 159);
			txLocations.Name = "txLocations";
			txLocations.Size = new System.Drawing.Size(223, 20);
			txLocations.TabIndex = 11;
			labelLocations.AutoSize = true;
			labelLocations.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelLocations.Location = new System.Drawing.Point(244, 146);
			labelLocations.Name = "labelLocations";
			labelLocations.Size = new System.Drawing.Size(58, 12);
			labelLocations.TabIndex = 10;
			labelLocations.Text = "Locations:";
			txCommunityRating.DrawText = true;
			txCommunityRating.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
			txCommunityRating.ForeColor = System.Drawing.SystemColors.GrayText;
			txCommunityRating.Location = new System.Drawing.Point(248, 364);
			txCommunityRating.Name = "txCommunityRating";
			txCommunityRating.Rating = 3f;
			txCommunityRating.RatingImage = cYo.Projects.ComicRack.Viewer.Properties.Resources.StarBlue;
			txCommunityRating.Size = new System.Drawing.Size(219, 20);
			txCommunityRating.TabIndex = 51;
			txCommunityRating.Text = "3";
			labelCommunityRating.AutoSize = true;
			labelCommunityRating.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelCommunityRating.Location = new System.Drawing.Point(247, 349);
			labelCommunityRating.Name = "labelCommunityRating";
			labelCommunityRating.Size = new System.Drawing.Size(102, 12);
			labelCommunityRating.TabIndex = 50;
			labelCommunityRating.Text = "Community Rating:";
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
			pageData.Size = new System.Drawing.Size(497, 407);
			pageData.TabIndex = 72;
			grpCustom.Controls.Add(textCustomField);
			grpCustom.Controls.Add(labelCustomField);
			grpCustom.Dock = System.Windows.Forms.DockStyle.Top;
			grpCustom.Location = new System.Drawing.Point(0, 1261);
			grpCustom.Name = "grpCustom";
			grpCustom.Size = new System.Drawing.Size(478, 91);
			grpCustom.TabIndex = 4;
			grpCustom.Text = "Custom Fields";
			textCustomField.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
			textCustomField.Location = new System.Drawing.Point(11, 53);
			textCustomField.Name = "textCustomField";
			textCustomField.Size = new System.Drawing.Size(451, 20);
			textCustomField.TabIndex = 25;
			textCustomField.Tag = "";
			labelCustomField.AutoSize = true;
			labelCustomField.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelCustomField.Location = new System.Drawing.Point(10, 38);
			labelCustomField.Name = "labelCustomField";
			labelCustomField.Size = new System.Drawing.Size(76, 12);
			labelCustomField.TabIndex = 24;
			labelCustomField.Text = "Custom Field:";
			grpCatalog.Controls.Add(labelReleasedTime);
			grpCatalog.Controls.Add(dtpReleasedTime);
			grpCatalog.Controls.Add(labelOpenedTime);
			grpCatalog.Controls.Add(dtpOpenedTime);
			grpCatalog.Controls.Add(labelAddedTime);
			grpCatalog.Controls.Add(dtpAddedTime);
			grpCatalog.Controls.Add(txPagesAsTextSimple);
			grpCatalog.Controls.Add(labelPagesAsTextSimple);
			grpCatalog.Controls.Add(txISBN);
			grpCatalog.Controls.Add(labelISBN);
			grpCatalog.Controls.Add(txBookNotes);
			grpCatalog.Controls.Add(labelBookNotes);
			grpCatalog.Controls.Add(cbBookLocation);
			grpCatalog.Controls.Add(labelBookLocation);
			grpCatalog.Controls.Add(txCollectionStatus);
			grpCatalog.Controls.Add(cbBookPrice);
			grpCatalog.Controls.Add(labelBookPrice);
			grpCatalog.Controls.Add(cbBookAge);
			grpCatalog.Controls.Add(labelBookAge);
			grpCatalog.Controls.Add(labelBookCollectionStatus);
			grpCatalog.Controls.Add(cbBookCondition);
			grpCatalog.Controls.Add(labelBookCondition);
			grpCatalog.Controls.Add(cbBookStore);
			grpCatalog.Controls.Add(labelBookStore);
			grpCatalog.Controls.Add(cbBookOwner);
			grpCatalog.Controls.Add(labelBookOwner);
			grpCatalog.Dock = System.Windows.Forms.DockStyle.Top;
			grpCatalog.Location = new System.Drawing.Point(0, 869);
			grpCatalog.Name = "grpCatalog";
			grpCatalog.Size = new System.Drawing.Size(478, 392);
			grpCatalog.TabIndex = 2;
			grpCatalog.Text = "Catalog";
			labelReleasedTime.AutoSize = true;
			labelReleasedTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelReleasedTime.Location = new System.Drawing.Point(14, 191);
			labelReleasedTime.Name = "labelReleasedTime";
			labelReleasedTime.Size = new System.Drawing.Size(56, 12);
			labelReleasedTime.TabIndex = 16;
			labelReleasedTime.Text = "Released:";
			dtpReleasedTime.CustomFormat = " ";
			dtpReleasedTime.Location = new System.Drawing.Point(13, 206);
			dtpReleasedTime.Name = "dtpReleasedTime";
			dtpReleasedTime.Size = new System.Drawing.Size(220, 20);
			dtpReleasedTime.TabIndex = 17;
			dtpReleasedTime.Tag = "ReleasedTime";
			dtpReleasedTime.Value = new System.DateTime(0L);
			labelOpenedTime.AutoSize = true;
			labelOpenedTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelOpenedTime.Location = new System.Drawing.Point(248, 229);
			labelOpenedTime.Name = "labelOpenedTime";
			labelOpenedTime.Size = new System.Drawing.Size(77, 12);
			labelOpenedTime.TabIndex = 20;
			labelOpenedTime.Text = "Opened/Read:";
			dtpOpenedTime.CustomFormat = " ";
			dtpOpenedTime.Location = new System.Drawing.Point(246, 244);
			dtpOpenedTime.Name = "dtpOpenedTime";
			dtpOpenedTime.Size = new System.Drawing.Size(218, 20);
			dtpOpenedTime.TabIndex = 21;
			dtpOpenedTime.Tag = "OpenedTime";
			dtpOpenedTime.Value = new System.DateTime(0L);
			labelAddedTime.AutoSize = true;
			labelAddedTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelAddedTime.Location = new System.Drawing.Point(246, 191);
			labelAddedTime.Name = "labelAddedTime";
			labelAddedTime.Size = new System.Drawing.Size(98, 12);
			labelAddedTime.TabIndex = 18;
			labelAddedTime.Text = "Added/Purchased:";
			dtpAddedTime.CustomFormat = " ";
			dtpAddedTime.Location = new System.Drawing.Point(246, 206);
			dtpAddedTime.Name = "dtpAddedTime";
			dtpAddedTime.Size = new System.Drawing.Size(218, 20);
			dtpAddedTime.TabIndex = 19;
			dtpAddedTime.Tag = "AddedTime";
			dtpAddedTime.Value = new System.DateTime(0L);
			txPagesAsTextSimple.AcceptsReturn = true;
			txPagesAsTextSimple.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
			txPagesAsTextSimple.Location = new System.Drawing.Point(246, 82);
			txPagesAsTextSimple.Name = "txPagesAsTextSimple";
			txPagesAsTextSimple.Size = new System.Drawing.Size(218, 20);
			txPagesAsTextSimple.TabIndex = 7;
			txPagesAsTextSimple.Tag = "PagesAsTextSimple";
			labelPagesAsTextSimple.AutoSize = true;
			labelPagesAsTextSimple.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelPagesAsTextSimple.Location = new System.Drawing.Point(245, 67);
			labelPagesAsTextSimple.Name = "labelPagesAsTextSimple";
			labelPagesAsTextSimple.Size = new System.Drawing.Size(40, 12);
			labelPagesAsTextSimple.TabIndex = 6;
			labelPagesAsTextSimple.Text = "Pages:";
			txISBN.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
			txISBN.Location = new System.Drawing.Point(13, 82);
			txISBN.Name = "txISBN";
			txISBN.Size = new System.Drawing.Size(220, 20);
			txISBN.TabIndex = 5;
			txISBN.Tag = "ISBN";
			labelISBN.AutoSize = true;
			labelISBN.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelISBN.Location = new System.Drawing.Point(13, 67);
			labelISBN.Name = "labelISBN";
			labelISBN.Size = new System.Drawing.Size(35, 12);
			labelISBN.TabIndex = 4;
			labelISBN.Text = "ISBN:";
			txBookNotes.AcceptsReturn = true;
			txBookNotes.FocusSelect = false;
			txBookNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
			txBookNotes.Location = new System.Drawing.Point(13, 327);
			txBookNotes.Multiline = true;
			txBookNotes.Name = "txBookNotes";
			txBookNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			txBookNotes.Size = new System.Drawing.Size(452, 50);
			txBookNotes.TabIndex = 25;
			txBookNotes.Tag = "BookNotes";
			labelBookNotes.AutoSize = true;
			labelBookNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelBookNotes.Location = new System.Drawing.Point(13, 312);
			labelBookNotes.Name = "labelBookNotes";
			labelBookNotes.Size = new System.Drawing.Size(120, 12);
			labelBookNotes.TabIndex = 24;
			labelBookNotes.Text = "Notes about this Book:";
			cbBookLocation.FormattingEnabled = true;
			cbBookLocation.Location = new System.Drawing.Point(246, 158);
			cbBookLocation.Name = "cbBookLocation";
			cbBookLocation.Size = new System.Drawing.Size(218, 21);
			cbBookLocation.TabIndex = 15;
			cbBookLocation.Tag = "BookLocation";
			labelBookLocation.AutoSize = true;
			labelBookLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelBookLocation.Location = new System.Drawing.Point(245, 144);
			labelBookLocation.Name = "labelBookLocation";
			labelBookLocation.Size = new System.Drawing.Size(80, 12);
			labelBookLocation.TabIndex = 14;
			labelBookLocation.Text = "Book Location:";
			txCollectionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
			txCollectionStatus.Location = new System.Drawing.Point(13, 286);
			txCollectionStatus.Name = "txCollectionStatus";
			txCollectionStatus.Size = new System.Drawing.Size(451, 20);
			txCollectionStatus.TabIndex = 23;
			txCollectionStatus.Tag = "CollectionStatus";
			cbBookPrice.FormattingEnabled = true;
			cbBookPrice.Location = new System.Drawing.Point(246, 44);
			cbBookPrice.Name = "cbBookPrice";
			cbBookPrice.Size = new System.Drawing.Size(218, 21);
			cbBookPrice.TabIndex = 3;
			cbBookPrice.Tag = "BookPrice";
			labelBookPrice.AutoSize = true;
			labelBookPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelBookPrice.Location = new System.Drawing.Point(244, 30);
			labelBookPrice.Name = "labelBookPrice";
			labelBookPrice.Size = new System.Drawing.Size(35, 12);
			labelBookPrice.TabIndex = 2;
			labelBookPrice.Text = "Price:";
			cbBookAge.FormattingEnabled = true;
			cbBookAge.Location = new System.Drawing.Point(13, 121);
			cbBookAge.Name = "cbBookAge";
			cbBookAge.Size = new System.Drawing.Size(220, 21);
			cbBookAge.TabIndex = 9;
			cbBookAge.Tag = "BookAge";
			labelBookAge.AutoSize = true;
			labelBookAge.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelBookAge.Location = new System.Drawing.Point(12, 106);
			labelBookAge.Name = "labelBookAge";
			labelBookAge.Size = new System.Drawing.Size(29, 12);
			labelBookAge.TabIndex = 8;
			labelBookAge.Text = "Age:";
			labelBookCollectionStatus.AutoSize = true;
			labelBookCollectionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelBookCollectionStatus.Location = new System.Drawing.Point(12, 271);
			labelBookCollectionStatus.Name = "labelBookCollectionStatus";
			labelBookCollectionStatus.Size = new System.Drawing.Size(96, 12);
			labelBookCollectionStatus.TabIndex = 22;
			labelBookCollectionStatus.Text = "Collection Status:";
			cbBookCondition.FormattingEnabled = true;
			cbBookCondition.Location = new System.Drawing.Point(246, 120);
			cbBookCondition.Name = "cbBookCondition";
			cbBookCondition.Size = new System.Drawing.Size(218, 21);
			cbBookCondition.TabIndex = 11;
			cbBookCondition.Tag = "BookCondition";
			labelBookCondition.AutoSize = true;
			labelBookCondition.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelBookCondition.Location = new System.Drawing.Point(244, 106);
			labelBookCondition.Name = "labelBookCondition";
			labelBookCondition.Size = new System.Drawing.Size(57, 12);
			labelBookCondition.TabIndex = 10;
			labelBookCondition.Text = "Condition:";
			cbBookStore.FormattingEnabled = true;
			cbBookStore.Location = new System.Drawing.Point(13, 45);
			cbBookStore.Name = "cbBookStore";
			cbBookStore.PromptText = null;
			cbBookStore.Size = new System.Drawing.Size(220, 21);
			cbBookStore.TabIndex = 1;
			cbBookStore.Tag = "BookStore";
			labelBookStore.AutoSize = true;
			labelBookStore.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelBookStore.Location = new System.Drawing.Point(11, 30);
			labelBookStore.Name = "labelBookStore";
			labelBookStore.Size = new System.Drawing.Size(36, 12);
			labelBookStore.TabIndex = 0;
			labelBookStore.Text = "Store:";
			cbBookOwner.FormattingEnabled = true;
			cbBookOwner.Location = new System.Drawing.Point(13, 158);
			cbBookOwner.Name = "cbBookOwner";
			cbBookOwner.Size = new System.Drawing.Size(220, 21);
			cbBookOwner.TabIndex = 13;
			cbBookOwner.Tag = "BookOwner";
			labelBookOwner.AutoSize = true;
			labelBookOwner.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelBookOwner.Location = new System.Drawing.Point(13, 144);
			labelBookOwner.Name = "labelBookOwner";
			labelBookOwner.Size = new System.Drawing.Size(42, 12);
			labelBookOwner.TabIndex = 12;
			labelBookOwner.Text = "Owner:";
			grpPlotNotes.Controls.Add(labelMainCharacterOrTeam);
			grpPlotNotes.Controls.Add(txMainCharacterOrTeam);
			grpPlotNotes.Controls.Add(labelReview);
			grpPlotNotes.Controls.Add(txReview);
			grpPlotNotes.Controls.Add(txScanInformation);
			grpPlotNotes.Controls.Add(labelScanInformation);
			grpPlotNotes.Controls.Add(txWeb);
			grpPlotNotes.Controls.Add(txSummary);
			grpPlotNotes.Controls.Add(labelSummary);
			grpPlotNotes.Controls.Add(labelNotes);
			grpPlotNotes.Controls.Add(txNotes);
			grpPlotNotes.Controls.Add(labelCharacters);
			grpPlotNotes.Controls.Add(txCharacters);
			grpPlotNotes.Controls.Add(labelWeb);
			grpPlotNotes.Controls.Add(labelTeams);
			grpPlotNotes.Controls.Add(txTeams);
			grpPlotNotes.Controls.Add(labelLocations);
			grpPlotNotes.Controls.Add(txLocations);
			grpPlotNotes.Dock = System.Windows.Forms.DockStyle.Top;
			grpPlotNotes.Location = new System.Drawing.Point(0, 604);
			grpPlotNotes.Name = "grpPlotNotes";
			grpPlotNotes.Size = new System.Drawing.Size(478, 265);
			grpPlotNotes.TabIndex = 3;
			grpPlotNotes.Text = "Plot & Notes";
			labelMainCharacterOrTeam.AutoSize = true;
			labelMainCharacterOrTeam.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelMainCharacterOrTeam.Location = new System.Drawing.Point(247, 33);
			labelMainCharacterOrTeam.Name = "labelMainCharacterOrTeam";
			labelMainCharacterOrTeam.Size = new System.Drawing.Size(118, 12);
			labelMainCharacterOrTeam.TabIndex = 2;
			labelMainCharacterOrTeam.Text = "Main Character/Team:";
			txMainCharacterOrTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txMainCharacterOrTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txMainCharacterOrTeam.Location = new System.Drawing.Point(243, 48);
			txMainCharacterOrTeam.Name = "txMainCharacterOrTeam";
			txMainCharacterOrTeam.Size = new System.Drawing.Size(224, 20);
			txMainCharacterOrTeam.TabIndex = 3;
			labelReview.AutoSize = true;
			labelReview.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelReview.Location = new System.Drawing.Point(16, 182);
			labelReview.Name = "labelReview";
			labelReview.Size = new System.Drawing.Size(48, 12);
			labelReview.TabIndex = 12;
			labelReview.Text = "Review:";
			txReview.Location = new System.Drawing.Point(14, 196);
			txReview.Multiline = true;
			txReview.Name = "txReview";
			txReview.Size = new System.Drawing.Size(220, 55);
			txReview.TabIndex = 13;
			txScanInformation.Location = new System.Drawing.Point(244, 196);
			txScanInformation.Name = "txScanInformation";
			txScanInformation.Size = new System.Drawing.Size(222, 20);
			txScanInformation.TabIndex = 15;
			labelScanInformation.AutoSize = true;
			labelScanInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelScanInformation.Location = new System.Drawing.Point(243, 182);
			labelScanInformation.Name = "labelScanInformation";
			labelScanInformation.Size = new System.Drawing.Size(95, 12);
			labelScanInformation.TabIndex = 14;
			labelScanInformation.Text = "Scan Information:";
			grpArtists.Controls.Add(labelWriter);
			grpArtists.Controls.Add(labelPenciller);
			grpArtists.Controls.Add(labelInker);
			grpArtists.Controls.Add(labelColorist);
			grpArtists.Controls.Add(txPenciller);
			grpArtists.Controls.Add(txInker);
			grpArtists.Controls.Add(txColorist);
			grpArtists.Controls.Add(txEditor);
			grpArtists.Controls.Add(txWriter);
			grpArtists.Controls.Add(txCoverArtist);
			grpArtists.Controls.Add(labelLetterer);
			grpArtists.Controls.Add(txLetterer);
			grpArtists.Controls.Add(labelCoverArtist);
			grpArtists.Controls.Add(labelEditor);
			grpArtists.Dock = System.Windows.Forms.DockStyle.Top;
			grpArtists.Location = new System.Drawing.Point(0, 400);
			grpArtists.Name = "grpArtists";
			grpArtists.Size = new System.Drawing.Size(478, 204);
			grpArtists.TabIndex = 1;
			grpArtists.Text = "Artists / People Involved";
			grpMain.Controls.Add(labelDay);
			grpMain.Controls.Add(txDay);
			grpMain.Controls.Add(labelSeriesGroup);
			grpMain.Controls.Add(txSeriesGroup);
			grpMain.Controls.Add(labelStoryArc);
			grpMain.Controls.Add(txStoryArc);
			grpMain.Controls.Add(cbSeriesComplete);
			grpMain.Controls.Add(labelSeriesComplete);
			grpMain.Controls.Add(cbEnableProposed);
			grpMain.Controls.Add(labelRating);
			grpMain.Controls.Add(txCommunityRating);
			grpMain.Controls.Add(labelEnableProposed);
			grpMain.Controls.Add(labelFormat);
			grpMain.Controls.Add(cbImprint);
			grpMain.Controls.Add(labelTags);
			grpMain.Controls.Add(labelAlternateSeries);
			grpMain.Controls.Add(labelImprint);
			grpMain.Controls.Add(labelCommunityRating);
			grpMain.Controls.Add(labelAlternateNumber);
			grpMain.Controls.Add(cbFormat);
			grpMain.Controls.Add(labelGenre);
			grpMain.Controls.Add(labelSeries);
			grpMain.Controls.Add(cbPublisher);
			grpMain.Controls.Add(txGenre);
			grpMain.Controls.Add(labelAlternateCount);
			grpMain.Controls.Add(labelPublisher);
			grpMain.Controls.Add(txRating);
			grpMain.Controls.Add(labelTitle);
			grpMain.Controls.Add(labelAgeRating);
			grpMain.Controls.Add(txTags);
			grpMain.Controls.Add(txAlternateSeries);
			grpMain.Controls.Add(labelLanguage);
			grpMain.Controls.Add(labelYear);
			grpMain.Controls.Add(cbLanguage);
			grpMain.Controls.Add(cbManga);
			grpMain.Controls.Add(txAlternateNumber);
			grpMain.Controls.Add(labelManga);
			grpMain.Controls.Add(labelNumber);
			grpMain.Controls.Add(labelBlackAndWhite);
			grpMain.Controls.Add(cbBlackAndWhite);
			grpMain.Controls.Add(txAlternateCount);
			grpMain.Controls.Add(cbAgeRating);
			grpMain.Controls.Add(labelMonth);
			grpMain.Controls.Add(labelVolume);
			grpMain.Controls.Add(labelCount);
			grpMain.Controls.Add(txTitle);
			grpMain.Controls.Add(txSeries);
			grpMain.Controls.Add(txYear);
			grpMain.Controls.Add(txMonth);
			grpMain.Controls.Add(txVolume);
			grpMain.Controls.Add(txNumber);
			grpMain.Controls.Add(txCount);
			grpMain.Dock = System.Windows.Forms.DockStyle.Top;
			grpMain.Location = new System.Drawing.Point(0, 0);
			grpMain.Name = "grpMain";
			grpMain.Size = new System.Drawing.Size(478, 400);
			grpMain.TabIndex = 0;
			grpMain.Text = "Main";
			labelDay.AutoSize = true;
			labelDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelDay.Location = new System.Drawing.Point(393, 74);
			labelDay.Name = "labelDay";
			labelDay.Size = new System.Drawing.Size(29, 12);
			labelDay.TabIndex = 14;
			labelDay.Text = "Day:";
			txDay.Location = new System.Drawing.Point(395, 87);
			txDay.MaxLength = 4;
			txDay.Name = "txDay";
			txDay.PromptText = "";
			txDay.Size = new System.Drawing.Size(71, 20);
			txDay.TabIndex = 15;
			labelSeriesGroup.AutoSize = true;
			labelSeriesGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelSeriesGroup.Location = new System.Drawing.Point(163, 150);
			labelSeriesGroup.Name = "labelSeriesGroup";
			labelSeriesGroup.Size = new System.Drawing.Size(74, 12);
			labelSeriesGroup.TabIndex = 24;
			labelSeriesGroup.Text = "Series Group:";
			txSeriesGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txSeriesGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txSeriesGroup.Location = new System.Drawing.Point(166, 165);
			txSeriesGroup.Name = "txSeriesGroup";
			txSeriesGroup.PromptText = "";
			txSeriesGroup.Size = new System.Drawing.Size(143, 20);
			txSeriesGroup.TabIndex = 25;
			txSeriesGroup.Tag = "";
			labelStoryArc.AutoSize = true;
			labelStoryArc.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelStoryArc.Location = new System.Drawing.Point(9, 150);
			labelStoryArc.Name = "labelStoryArc";
			labelStoryArc.Size = new System.Drawing.Size(57, 12);
			labelStoryArc.TabIndex = 22;
			labelStoryArc.Text = "Story Arc:";
			txStoryArc.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			txStoryArc.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			txStoryArc.Location = new System.Drawing.Point(11, 164);
			txStoryArc.Name = "txStoryArc";
			txStoryArc.PromptText = "";
			txStoryArc.Size = new System.Drawing.Size(144, 20);
			txStoryArc.TabIndex = 23;
			txStoryArc.Tag = "";
			cbSeriesComplete.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbSeriesComplete.FormattingEnabled = true;
			cbSeriesComplete.Location = new System.Drawing.Point(317, 165);
			cbSeriesComplete.Name = "cbSeriesComplete";
			cbSeriesComplete.Size = new System.Drawing.Size(149, 21);
			cbSeriesComplete.TabIndex = 27;
			labelSeriesComplete.AutoSize = true;
			labelSeriesComplete.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelSeriesComplete.Location = new System.Drawing.Point(315, 152);
			labelSeriesComplete.Name = "labelSeriesComplete";
			labelSeriesComplete.Size = new System.Drawing.Size(90, 12);
			labelSeriesComplete.TabIndex = 26;
			labelSeriesComplete.Text = "Series complete:";
			base.AcceptButton = btOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(525, 460);
			base.Controls.Add(pageData);
			base.Controls.Add(btCancel);
			base.Controls.Add(btOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "MultipleComicBooksDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Multiple Book Information ({0})";
			pageData.ResumeLayout(false);
			grpCustom.ResumeLayout(false);
			grpCustom.PerformLayout();
			grpCatalog.ResumeLayout(false);
			grpCatalog.PerformLayout();
			grpPlotNotes.ResumeLayout(false);
			grpPlotNotes.PerformLayout();
			grpArtists.ResumeLayout(false);
			grpArtists.PerformLayout();
			grpMain.ResumeLayout(false);
			grpMain.PerformLayout();
			ResumeLayout(false);
		}
		
		private IContainer components;

		private TextBoxEx txCount;

		private TextBoxEx txNumber;

		private TextBoxEx txVolume;

		private TextBoxEx txYear;

		private TextBoxEx txSeries;

		private TextBoxEx txTitle;

		private TextBoxEx txWriter;

		private TextBoxEx txSummary;

		private TextBoxEx txColorist;

		private TextBoxEx txInker;

		private TextBoxEx txPenciller;

		private Label labelSummary;

		private Label labelGenre;

		private Label labelColorist;

		private Label labelPublisher;

		private Label labelInker;

		private Label labelCount;

		private Label labelVolume;

		private Label labelNumber;

		private Label labelYear;

		private Label labelSeries;

		private Label labelWriter;

		private Label labelPenciller;

		private Label labelTitle;

		private Button btCancel;

		private Button btOK;

		private RatingControl txRating;

		private Label labelRating;

		private TextBoxEx txEditor;

		private TextBoxEx txCoverArtist;

		private TextBoxEx txLetterer;

		private Label labelEditor;

		private Label labelCoverArtist;

		private Label labelLetterer;

		private ComboBox cbPublisher;

		private Label labelAlternateSeries;

		private Label labelAlternateNumber;

		private Label labelAlternateCount;

		private TextBoxEx txAlternateSeries;

		private TextBoxEx txAlternateNumber;

		private TextBoxEx txAlternateCount;

		private Label labelMonth;

		private TextBoxEx txMonth;

		private Label labelTags;

		private TextBoxEx txTags;

		private ComboBox cbImprint;

		private Label labelImprint;

		private LanguageComboBox cbLanguage;

		private Label labelLanguage;

		private ComboBox cbManga;

		private Label labelManga;

		private ComboBox cbBlackAndWhite;

		private Label labelBlackAndWhite;

		private ComboBoxEx cbFormat;

		private Label labelFormat;

		private ComboBox cbEnableProposed;

		private Label labelEnableProposed;

		private ComboBoxEx cbAgeRating;

		private Label labelAgeRating;

		private TextBoxEx txNotes;

		private Label labelNotes;

		private TextBoxEx txCharacters;

		private Label labelCharacters;

		private TextBoxEx txGenre;

		private TextBoxEx txWeb;

		private Label labelWeb;

		private TextBoxEx txTeams;

		private Label labelTeams;

		private TextBoxEx txLocations;

		private Label labelLocations;

		private RatingControl txCommunityRating;

		private Label labelCommunityRating;

		private Panel pageData;

		private CollapsibleGroupBox grpCatalog;

		private CollapsibleGroupBox grpArtists;

		private CollapsibleGroupBox grpPlotNotes;

		private CollapsibleGroupBox grpMain;

		private ComboBox cbBookLocation;

		private Label labelBookLocation;

		private TextBoxEx txCollectionStatus;

		private ComboBox cbBookPrice;

		private Label labelBookPrice;

		private ComboBox cbBookAge;

		private Label labelBookAge;

		private Label labelBookCollectionStatus;

		private ComboBox cbBookCondition;

		private Label labelBookCondition;

		private ComboBoxEx cbBookStore;

		private Label labelBookStore;

		private ComboBox cbBookOwner;

		private Label labelBookOwner;

		private TextBoxEx txBookNotes;

		private Label labelBookNotes;

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

		private TextBoxEx txScanInformation;

		private Label labelScanInformation;

		private Label labelSeriesGroup;

		private TextBoxEx txSeriesGroup;

		private Label labelStoryArc;

		private TextBoxEx txStoryArc;

		private Label labelMainCharacterOrTeam;

		private TextBoxEx txMainCharacterOrTeam;

		private Label labelReview;

		private TextBoxEx txReview;

		private Label labelDay;

		private TextBoxEx txDay;

		private Label labelReleasedTime;

		private NullableDateTimePicker dtpReleasedTime;

		private CollapsibleGroupBox grpCustom;

		private TextBoxEx textCustomField;

		private Label labelCustomField;
	}
}
