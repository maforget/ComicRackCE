using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine.Controls;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class MultipleComicBooksDialog
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
			this.txCount = new cYo.Common.Windows.Forms.TextBoxEx();
			this.txNumber = new cYo.Common.Windows.Forms.TextBoxEx();
			this.txVolume = new cYo.Common.Windows.Forms.TextBoxEx();
			this.txYear = new cYo.Common.Windows.Forms.TextBoxEx();
			this.txSeries = new cYo.Common.Windows.Forms.TextBoxEx();
			this.txTitle = new cYo.Common.Windows.Forms.TextBoxEx();
			this.txWriter = new cYo.Common.Windows.Forms.TextBoxEx();
			this.txSummary = new cYo.Common.Windows.Forms.TextBoxEx();
			this.txColorist = new cYo.Common.Windows.Forms.TextBoxEx();
			this.txInker = new cYo.Common.Windows.Forms.TextBoxEx();
			this.txPenciller = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelSummary = new System.Windows.Forms.Label();
			this.labelGenre = new System.Windows.Forms.Label();
			this.labelColorist = new System.Windows.Forms.Label();
			this.labelPublisher = new System.Windows.Forms.Label();
			this.labelInker = new System.Windows.Forms.Label();
			this.labelCount = new System.Windows.Forms.Label();
			this.labelVolume = new System.Windows.Forms.Label();
			this.labelNumber = new System.Windows.Forms.Label();
			this.labelYear = new System.Windows.Forms.Label();
			this.labelSeries = new System.Windows.Forms.Label();
			this.labelWriter = new System.Windows.Forms.Label();
			this.labelPenciller = new System.Windows.Forms.Label();
			this.labelTitle = new System.Windows.Forms.Label();
			this.btCancel = new System.Windows.Forms.Button();
			this.btOK = new System.Windows.Forms.Button();
			this.txRating = new cYo.Projects.ComicRack.Engine.Controls.RatingControl();
			this.labelRating = new System.Windows.Forms.Label();
			this.txEditor = new cYo.Common.Windows.Forms.TextBoxEx();
			this.txCoverArtist = new cYo.Common.Windows.Forms.TextBoxEx();
			this.txLetterer = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelEditor = new System.Windows.Forms.Label();
			this.labelCoverArtist = new System.Windows.Forms.Label();
			this.labelLetterer = new System.Windows.Forms.Label();
			this.cbPublisher = new System.Windows.Forms.ComboBox();
			this.labelAlternateSeries = new System.Windows.Forms.Label();
			this.labelAlternateNumber = new System.Windows.Forms.Label();
			this.labelAlternateCount = new System.Windows.Forms.Label();
			this.txAlternateSeries = new cYo.Common.Windows.Forms.TextBoxEx();
			this.txAlternateNumber = new cYo.Common.Windows.Forms.TextBoxEx();
			this.txAlternateCount = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelMonth = new System.Windows.Forms.Label();
			this.txMonth = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelTags = new System.Windows.Forms.Label();
			this.txTags = new cYo.Common.Windows.Forms.TextBoxEx();
			this.cbImprint = new System.Windows.Forms.ComboBox();
			this.labelImprint = new System.Windows.Forms.Label();
			this.cbLanguage = new cYo.Common.Windows.Forms.LanguageComboBox();
			this.labelLanguage = new System.Windows.Forms.Label();
			this.cbManga = new System.Windows.Forms.ComboBox();
			this.labelManga = new System.Windows.Forms.Label();
			this.cbBlackAndWhite = new System.Windows.Forms.ComboBox();
			this.labelBlackAndWhite = new System.Windows.Forms.Label();
			this.cbFormat = new cYo.Common.Windows.Forms.ComboBoxEx();
			this.labelFormat = new System.Windows.Forms.Label();
			this.cbEnableProposed = new System.Windows.Forms.ComboBox();
			this.labelEnableProposed = new System.Windows.Forms.Label();
			this.cbAgeRating = new cYo.Common.Windows.Forms.ComboBoxEx();
			this.labelAgeRating = new System.Windows.Forms.Label();
			this.txNotes = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelNotes = new System.Windows.Forms.Label();
			this.txCharacters = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelCharacters = new System.Windows.Forms.Label();
			this.txGenre = new cYo.Common.Windows.Forms.TextBoxEx();
			this.txWeb = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelWeb = new System.Windows.Forms.Label();
			this.txTeams = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelTeams = new System.Windows.Forms.Label();
			this.txLocations = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelLocations = new System.Windows.Forms.Label();
			this.txCommunityRating = new cYo.Projects.ComicRack.Engine.Controls.RatingControl();
			this.labelCommunityRating = new System.Windows.Forms.Label();
			this.pageData = new System.Windows.Forms.Panel();
			this.grpCustom = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			this.textCustomField = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelCustomField = new System.Windows.Forms.Label();
			this.grpCatalog = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
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
			this.txBookNotes = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelBookNotes = new System.Windows.Forms.Label();
			this.cbBookLocation = new System.Windows.Forms.ComboBox();
			this.labelBookLocation = new System.Windows.Forms.Label();
			this.txCollectionStatus = new cYo.Common.Windows.Forms.TextBoxEx();
			this.cbBookPrice = new System.Windows.Forms.ComboBox();
			this.labelBookPrice = new System.Windows.Forms.Label();
			this.cbBookAge = new System.Windows.Forms.ComboBox();
			this.labelBookAge = new System.Windows.Forms.Label();
			this.labelBookCollectionStatus = new System.Windows.Forms.Label();
			this.cbBookCondition = new System.Windows.Forms.ComboBox();
			this.labelBookCondition = new System.Windows.Forms.Label();
			this.cbBookStore = new cYo.Common.Windows.Forms.ComboBoxEx();
			this.labelBookStore = new System.Windows.Forms.Label();
			this.cbBookOwner = new System.Windows.Forms.ComboBox();
			this.labelBookOwner = new System.Windows.Forms.Label();
			this.grpPlotNotes = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			this.labelMainCharacterOrTeam = new System.Windows.Forms.Label();
			this.txMainCharacterOrTeam = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelReview = new System.Windows.Forms.Label();
			this.txReview = new cYo.Common.Windows.Forms.TextBoxEx();
			this.txScanInformation = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelScanInformation = new System.Windows.Forms.Label();
			this.grpArtists = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			this.labelTranslator = new System.Windows.Forms.Label();
			this.txTranslator = new cYo.Common.Windows.Forms.TextBoxEx();
			this.grpMain = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			this.labelDay = new System.Windows.Forms.Label();
			this.txDay = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelSeriesGroup = new System.Windows.Forms.Label();
			this.txSeriesGroup = new cYo.Common.Windows.Forms.TextBoxEx();
			this.labelStoryArc = new System.Windows.Forms.Label();
			this.txStoryArc = new cYo.Common.Windows.Forms.TextBoxEx();
			this.cbSeriesComplete = new System.Windows.Forms.ComboBox();
			this.labelSeriesComplete = new System.Windows.Forms.Label();
			this.pageData.SuspendLayout();
			this.grpCustom.SuspendLayout();
			this.grpCatalog.SuspendLayout();
			this.grpPlotNotes.SuspendLayout();
			this.grpArtists.SuspendLayout();
			this.grpMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// txCount
			// 
			this.txCount.Location = new System.Drawing.Point(395, 51);
			this.txCount.Name = "txCount";
			this.txCount.PromptText = "";
			this.txCount.Size = new System.Drawing.Size(72, 20);
			this.txCount.TabIndex = 7;
			// 
			// txNumber
			// 
			this.txNumber.Location = new System.Drawing.Point(318, 50);
			this.txNumber.Name = "txNumber";
			this.txNumber.PromptText = "";
			this.txNumber.Size = new System.Drawing.Size(71, 20);
			this.txNumber.TabIndex = 5;
			// 
			// txVolume
			// 
			this.txVolume.Location = new System.Drawing.Point(243, 50);
			this.txVolume.Name = "txVolume";
			this.txVolume.PromptText = "";
			this.txVolume.Size = new System.Drawing.Size(66, 20);
			this.txVolume.TabIndex = 3;
			// 
			// txYear
			// 
			this.txYear.Location = new System.Drawing.Point(243, 87);
			this.txYear.MaxLength = 4;
			this.txYear.Name = "txYear";
			this.txYear.PromptText = "";
			this.txYear.Size = new System.Drawing.Size(66, 20);
			this.txYear.TabIndex = 11;
			// 
			// txSeries
			// 
			this.txSeries.Location = new System.Drawing.Point(11, 50);
			this.txSeries.Name = "txSeries";
			this.txSeries.PromptText = "";
			this.txSeries.Size = new System.Drawing.Size(223, 20);
			this.txSeries.TabIndex = 1;
			// 
			// txTitle
			// 
			this.txTitle.Location = new System.Drawing.Point(11, 87);
			this.txTitle.Name = "txTitle";
			this.txTitle.PromptText = "";
			this.txTitle.Size = new System.Drawing.Size(226, 20);
			this.txTitle.TabIndex = 9;
			// 
			// txWriter
			// 
			this.txWriter.Location = new System.Drawing.Point(13, 57);
			this.txWriter.Name = "txWriter";
			this.txWriter.Size = new System.Drawing.Size(220, 20);
			this.txWriter.TabIndex = 1;
			// 
			// txSummary
			// 
			this.txSummary.Location = new System.Drawing.Point(13, 48);
			this.txSummary.Multiline = true;
			this.txSummary.Name = "txSummary";
			this.txSummary.Size = new System.Drawing.Size(220, 55);
			this.txSummary.TabIndex = 1;
			// 
			// txColorist
			// 
			this.txColorist.Location = new System.Drawing.Point(245, 93);
			this.txColorist.Name = "txColorist";
			this.txColorist.Size = new System.Drawing.Size(220, 20);
			this.txColorist.TabIndex = 11;
			// 
			// txInker
			// 
			this.txInker.Location = new System.Drawing.Point(13, 93);
			this.txInker.Name = "txInker";
			this.txInker.Size = new System.Drawing.Size(220, 20);
			this.txInker.TabIndex = 3;
			// 
			// txPenciller
			// 
			this.txPenciller.Location = new System.Drawing.Point(245, 57);
			this.txPenciller.Name = "txPenciller";
			this.txPenciller.Size = new System.Drawing.Size(220, 20);
			this.txPenciller.TabIndex = 9;
			// 
			// labelSummary
			// 
			this.labelSummary.AutoSize = true;
			this.labelSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSummary.Location = new System.Drawing.Point(15, 33);
			this.labelSummary.Name = "labelSummary";
			this.labelSummary.Size = new System.Drawing.Size(56, 12);
			this.labelSummary.TabIndex = 0;
			this.labelSummary.Text = "Summary:";
			// 
			// labelGenre
			// 
			this.labelGenre.AutoSize = true;
			this.labelGenre.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelGenre.Location = new System.Drawing.Point(163, 270);
			this.labelGenre.Name = "labelGenre";
			this.labelGenre.Size = new System.Drawing.Size(39, 12);
			this.labelGenre.TabIndex = 42;
			this.labelGenre.Text = "Genre:";
			// 
			// labelColorist
			// 
			this.labelColorist.AutoSize = true;
			this.labelColorist.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelColorist.Location = new System.Drawing.Point(243, 80);
			this.labelColorist.Name = "labelColorist";
			this.labelColorist.Size = new System.Drawing.Size(49, 12);
			this.labelColorist.TabIndex = 10;
			this.labelColorist.Text = "Colorist:";
			// 
			// labelPublisher
			// 
			this.labelPublisher.AutoSize = true;
			this.labelPublisher.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelPublisher.Location = new System.Drawing.Point(9, 236);
			this.labelPublisher.Name = "labelPublisher";
			this.labelPublisher.Size = new System.Drawing.Size(56, 12);
			this.labelPublisher.TabIndex = 34;
			this.labelPublisher.Text = "Publisher:";
			// 
			// labelInker
			// 
			this.labelInker.AutoSize = true;
			this.labelInker.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelInker.Location = new System.Drawing.Point(13, 80);
			this.labelInker.Name = "labelInker";
			this.labelInker.Size = new System.Drawing.Size(35, 12);
			this.labelInker.TabIndex = 2;
			this.labelInker.Text = "Inker:";
			// 
			// labelCount
			// 
			this.labelCount.AutoSize = true;
			this.labelCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelCount.Location = new System.Drawing.Point(396, 36);
			this.labelCount.Name = "labelCount";
			this.labelCount.Size = new System.Drawing.Size(19, 12);
			this.labelCount.TabIndex = 6;
			this.labelCount.Text = "of:";
			// 
			// labelVolume
			// 
			this.labelVolume.AutoSize = true;
			this.labelVolume.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelVolume.Location = new System.Drawing.Point(241, 36);
			this.labelVolume.Name = "labelVolume";
			this.labelVolume.Size = new System.Drawing.Size(47, 12);
			this.labelVolume.TabIndex = 2;
			this.labelVolume.Text = "Volume:";
			// 
			// labelNumber
			// 
			this.labelNumber.AutoSize = true;
			this.labelNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelNumber.Location = new System.Drawing.Point(316, 36);
			this.labelNumber.Name = "labelNumber";
			this.labelNumber.Size = new System.Drawing.Size(48, 12);
			this.labelNumber.TabIndex = 4;
			this.labelNumber.Text = "Number:";
			// 
			// labelYear
			// 
			this.labelYear.AutoSize = true;
			this.labelYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelYear.Location = new System.Drawing.Point(241, 74);
			this.labelYear.Name = "labelYear";
			this.labelYear.Size = new System.Drawing.Size(31, 12);
			this.labelYear.TabIndex = 10;
			this.labelYear.Text = "Year:";
			// 
			// labelSeries
			// 
			this.labelSeries.AutoSize = true;
			this.labelSeries.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSeries.Location = new System.Drawing.Point(11, 36);
			this.labelSeries.Name = "labelSeries";
			this.labelSeries.Size = new System.Drawing.Size(41, 12);
			this.labelSeries.TabIndex = 0;
			this.labelSeries.Text = "Series:";
			// 
			// labelWriter
			// 
			this.labelWriter.AutoSize = true;
			this.labelWriter.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelWriter.Location = new System.Drawing.Point(12, 42);
			this.labelWriter.Name = "labelWriter";
			this.labelWriter.Size = new System.Drawing.Size(40, 12);
			this.labelWriter.TabIndex = 0;
			this.labelWriter.Text = "Writer:";
			// 
			// labelPenciller
			// 
			this.labelPenciller.AutoSize = true;
			this.labelPenciller.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelPenciller.Location = new System.Drawing.Point(243, 42);
			this.labelPenciller.Name = "labelPenciller";
			this.labelPenciller.Size = new System.Drawing.Size(53, 12);
			this.labelPenciller.TabIndex = 8;
			this.labelPenciller.Text = "Penciller:";
			// 
			// labelTitle
			// 
			this.labelTitle.AutoSize = true;
			this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTitle.Location = new System.Drawing.Point(11, 74);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new System.Drawing.Size(31, 12);
			this.labelTitle.TabIndex = 8;
			this.labelTitle.Text = "Title:";
			// 
			// btCancel
			// 
			this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btCancel.Location = new System.Drawing.Point(429, 427);
			this.btCancel.Name = "btCancel";
			this.btCancel.Size = new System.Drawing.Size(80, 24);
			this.btCancel.TabIndex = 71;
			this.btCancel.Text = "&Cancel";
			// 
			// btOK
			// 
			this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btOK.Location = new System.Drawing.Point(346, 427);
			this.btOK.Name = "btOK";
			this.btOK.Size = new System.Drawing.Size(77, 24);
			this.btOK.TabIndex = 70;
			this.btOK.Text = "&OK";
			this.btOK.Click += new System.EventHandler(this.btOK_Click);
			// 
			// txRating
			// 
			this.txRating.DrawText = true;
			this.txRating.Font = new System.Drawing.Font("Arial", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txRating.ForeColor = System.Drawing.SystemColors.GrayText;
			this.txRating.Location = new System.Drawing.Point(13, 363);
			this.txRating.Name = "txRating";
			this.txRating.Rating = 3F;
			this.txRating.RatingImage = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.StarYellow;
			this.txRating.Size = new System.Drawing.Size(223, 20);
			this.txRating.TabIndex = 49;
			this.txRating.Text = "3";
			// 
			// labelRating
			// 
			this.labelRating.AutoSize = true;
			this.labelRating.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelRating.Location = new System.Drawing.Point(12, 348);
			this.labelRating.Name = "labelRating";
			this.labelRating.Size = new System.Drawing.Size(61, 12);
			this.labelRating.TabIndex = 48;
			this.labelRating.Text = "My Rating:";
			// 
			// txEditor
			// 
			this.txEditor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txEditor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txEditor.Location = new System.Drawing.Point(13, 167);
			this.txEditor.Name = "txEditor";
			this.txEditor.Size = new System.Drawing.Size(220, 20);
			this.txEditor.TabIndex = 7;
			// 
			// txCoverArtist
			// 
			this.txCoverArtist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txCoverArtist.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txCoverArtist.Location = new System.Drawing.Point(245, 130);
			this.txCoverArtist.Name = "txCoverArtist";
			this.txCoverArtist.Size = new System.Drawing.Size(220, 20);
			this.txCoverArtist.TabIndex = 13;
			// 
			// txLetterer
			// 
			this.txLetterer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txLetterer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txLetterer.Location = new System.Drawing.Point(13, 129);
			this.txLetterer.Name = "txLetterer";
			this.txLetterer.Size = new System.Drawing.Size(220, 20);
			this.txLetterer.TabIndex = 5;
			// 
			// labelEditor
			// 
			this.labelEditor.AutoSize = true;
			this.labelEditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelEditor.Location = new System.Drawing.Point(13, 152);
			this.labelEditor.Name = "labelEditor";
			this.labelEditor.Size = new System.Drawing.Size(39, 12);
			this.labelEditor.TabIndex = 6;
			this.labelEditor.Text = "Editor:";
			// 
			// labelCoverArtist
			// 
			this.labelCoverArtist.AutoSize = true;
			this.labelCoverArtist.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelCoverArtist.Location = new System.Drawing.Point(243, 116);
			this.labelCoverArtist.Name = "labelCoverArtist";
			this.labelCoverArtist.Size = new System.Drawing.Size(72, 12);
			this.labelCoverArtist.TabIndex = 12;
			this.labelCoverArtist.Text = "Cover Artist:";
			// 
			// labelLetterer
			// 
			this.labelLetterer.AutoSize = true;
			this.labelLetterer.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelLetterer.Location = new System.Drawing.Point(12, 116);
			this.labelLetterer.Name = "labelLetterer";
			this.labelLetterer.Size = new System.Drawing.Size(49, 12);
			this.labelLetterer.TabIndex = 4;
			this.labelLetterer.Text = "Letterer:";
			// 
			// cbPublisher
			// 
			this.cbPublisher.FormattingEnabled = true;
			this.cbPublisher.Location = new System.Drawing.Point(11, 248);
			this.cbPublisher.Name = "cbPublisher";
			this.cbPublisher.Size = new System.Drawing.Size(142, 21);
			this.cbPublisher.TabIndex = 35;
			// 
			// labelAlternateSeries
			// 
			this.labelAlternateSeries.AutoSize = true;
			this.labelAlternateSeries.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelAlternateSeries.Location = new System.Drawing.Point(9, 112);
			this.labelAlternateSeries.Name = "labelAlternateSeries";
			this.labelAlternateSeries.Size = new System.Drawing.Size(177, 12);
			this.labelAlternateSeries.TabIndex = 16;
			this.labelAlternateSeries.Text = "Alternate Series or Storyline Title:";
			// 
			// labelAlternateNumber
			// 
			this.labelAlternateNumber.AutoSize = true;
			this.labelAlternateNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelAlternateNumber.Location = new System.Drawing.Point(316, 112);
			this.labelAlternateNumber.Name = "labelAlternateNumber";
			this.labelAlternateNumber.Size = new System.Drawing.Size(48, 12);
			this.labelAlternateNumber.TabIndex = 18;
			this.labelAlternateNumber.Text = "Number:";
			// 
			// labelAlternateCount
			// 
			this.labelAlternateCount.AutoSize = true;
			this.labelAlternateCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelAlternateCount.Location = new System.Drawing.Point(395, 112);
			this.labelAlternateCount.Name = "labelAlternateCount";
			this.labelAlternateCount.Size = new System.Drawing.Size(19, 12);
			this.labelAlternateCount.TabIndex = 20;
			this.labelAlternateCount.Text = "of:";
			// 
			// txAlternateSeries
			// 
			this.txAlternateSeries.Location = new System.Drawing.Point(10, 127);
			this.txAlternateSeries.Name = "txAlternateSeries";
			this.txAlternateSeries.Size = new System.Drawing.Size(299, 20);
			this.txAlternateSeries.TabIndex = 17;
			// 
			// txAlternateNumber
			// 
			this.txAlternateNumber.Location = new System.Drawing.Point(318, 127);
			this.txAlternateNumber.Name = "txAlternateNumber";
			this.txAlternateNumber.Size = new System.Drawing.Size(70, 20);
			this.txAlternateNumber.TabIndex = 19;
			// 
			// txAlternateCount
			// 
			this.txAlternateCount.Location = new System.Drawing.Point(394, 127);
			this.txAlternateCount.Name = "txAlternateCount";
			this.txAlternateCount.Size = new System.Drawing.Size(72, 20);
			this.txAlternateCount.TabIndex = 21;
			// 
			// labelMonth
			// 
			this.labelMonth.AutoSize = true;
			this.labelMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelMonth.Location = new System.Drawing.Point(316, 74);
			this.labelMonth.Name = "labelMonth";
			this.labelMonth.Size = new System.Drawing.Size(41, 12);
			this.labelMonth.TabIndex = 12;
			this.labelMonth.Text = "Month:";
			// 
			// txMonth
			// 
			this.txMonth.Location = new System.Drawing.Point(318, 87);
			this.txMonth.MaxLength = 2;
			this.txMonth.Name = "txMonth";
			this.txMonth.Size = new System.Drawing.Size(71, 20);
			this.txMonth.TabIndex = 13;
			// 
			// labelTags
			// 
			this.labelTags.AutoSize = true;
			this.labelTags.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTags.Location = new System.Drawing.Point(9, 309);
			this.labelTags.Name = "labelTags";
			this.labelTags.Size = new System.Drawing.Size(33, 12);
			this.labelTags.TabIndex = 46;
			this.labelTags.Text = "Tags:";
			// 
			// txTags
			// 
			this.txTags.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txTags.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txTags.Location = new System.Drawing.Point(11, 324);
			this.txTags.Name = "txTags";
			this.txTags.Size = new System.Drawing.Size(298, 20);
			this.txTags.TabIndex = 47;
			// 
			// cbImprint
			// 
			this.cbImprint.FormattingEnabled = true;
			this.cbImprint.Location = new System.Drawing.Point(164, 248);
			this.cbImprint.Name = "cbImprint";
			this.cbImprint.Size = new System.Drawing.Size(145, 21);
			this.cbImprint.TabIndex = 37;
			// 
			// labelImprint
			// 
			this.labelImprint.AutoSize = true;
			this.labelImprint.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelImprint.Location = new System.Drawing.Point(162, 235);
			this.labelImprint.Name = "labelImprint";
			this.labelImprint.Size = new System.Drawing.Size(45, 12);
			this.labelImprint.TabIndex = 36;
			this.labelImprint.Text = "Imprint:";
			// 
			// cbLanguage
			// 
			this.cbLanguage.CultureTypes = System.Globalization.CultureTypes.NeutralCultures;
			this.cbLanguage.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.cbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbLanguage.FormattingEnabled = true;
			this.cbLanguage.IntegralHeight = false;
			this.cbLanguage.Location = new System.Drawing.Point(10, 285);
			this.cbLanguage.Name = "cbLanguage";
			this.cbLanguage.SelectedCulture = "";
			this.cbLanguage.SelectedTwoLetterISOLanguage = "";
			this.cbLanguage.Size = new System.Drawing.Size(143, 21);
			this.cbLanguage.TabIndex = 41;
			this.cbLanguage.TopTwoLetterISOLanguages = null;
			// 
			// labelLanguage
			// 
			this.labelLanguage.AutoSize = true;
			this.labelLanguage.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelLanguage.Location = new System.Drawing.Point(8, 270);
			this.labelLanguage.Name = "labelLanguage";
			this.labelLanguage.Size = new System.Drawing.Size(57, 12);
			this.labelLanguage.TabIndex = 40;
			this.labelLanguage.Text = "Language:";
			// 
			// cbManga
			// 
			this.cbManga.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbManga.FormattingEnabled = true;
			this.cbManga.Location = new System.Drawing.Point(317, 210);
			this.cbManga.Name = "cbManga";
			this.cbManga.Size = new System.Drawing.Size(149, 21);
			this.cbManga.TabIndex = 33;
			// 
			// labelManga
			// 
			this.labelManga.AutoSize = true;
			this.labelManga.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelManga.Location = new System.Drawing.Point(315, 198);
			this.labelManga.Name = "labelManga";
			this.labelManga.Size = new System.Drawing.Size(43, 12);
			this.labelManga.TabIndex = 32;
			this.labelManga.Text = "Manga:";
			// 
			// cbBlackAndWhite
			// 
			this.cbBlackAndWhite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbBlackAndWhite.FormattingEnabled = true;
			this.cbBlackAndWhite.Location = new System.Drawing.Point(317, 246);
			this.cbBlackAndWhite.Name = "cbBlackAndWhite";
			this.cbBlackAndWhite.Size = new System.Drawing.Size(149, 21);
			this.cbBlackAndWhite.TabIndex = 39;
			// 
			// labelBlackAndWhite
			// 
			this.labelBlackAndWhite.AutoSize = true;
			this.labelBlackAndWhite.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBlackAndWhite.Location = new System.Drawing.Point(317, 231);
			this.labelBlackAndWhite.Name = "labelBlackAndWhite";
			this.labelBlackAndWhite.Size = new System.Drawing.Size(90, 12);
			this.labelBlackAndWhite.TabIndex = 38;
			this.labelBlackAndWhite.Text = "Black and White:";
			// 
			// cbFormat
			// 
			this.cbFormat.FormattingEnabled = true;
			this.cbFormat.Location = new System.Drawing.Point(10, 211);
			this.cbFormat.Name = "cbFormat";
			this.cbFormat.PromptText = null;
			this.cbFormat.Size = new System.Drawing.Size(145, 21);
			this.cbFormat.TabIndex = 29;
			// 
			// labelFormat
			// 
			this.labelFormat.AutoSize = true;
			this.labelFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelFormat.Location = new System.Drawing.Point(8, 197);
			this.labelFormat.Name = "labelFormat";
			this.labelFormat.Size = new System.Drawing.Size(45, 12);
			this.labelFormat.TabIndex = 28;
			this.labelFormat.Text = "Format:";
			// 
			// cbEnableProposed
			// 
			this.cbEnableProposed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbEnableProposed.FormattingEnabled = true;
			this.cbEnableProposed.Location = new System.Drawing.Point(317, 284);
			this.cbEnableProposed.Name = "cbEnableProposed";
			this.cbEnableProposed.Size = new System.Drawing.Size(149, 21);
			this.cbEnableProposed.TabIndex = 45;
			// 
			// labelEnableProposed
			// 
			this.labelEnableProposed.AutoSize = true;
			this.labelEnableProposed.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelEnableProposed.Location = new System.Drawing.Point(319, 269);
			this.labelEnableProposed.Name = "labelEnableProposed";
			this.labelEnableProposed.Size = new System.Drawing.Size(94, 12);
			this.labelEnableProposed.TabIndex = 44;
			this.labelEnableProposed.Text = "Proposed Values:";
			// 
			// cbAgeRating
			// 
			this.cbAgeRating.FormattingEnabled = true;
			this.cbAgeRating.Location = new System.Drawing.Point(164, 211);
			this.cbAgeRating.Name = "cbAgeRating";
			this.cbAgeRating.PromptText = null;
			this.cbAgeRating.Size = new System.Drawing.Size(145, 21);
			this.cbAgeRating.TabIndex = 31;
			// 
			// labelAgeRating
			// 
			this.labelAgeRating.AutoSize = true;
			this.labelAgeRating.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelAgeRating.Location = new System.Drawing.Point(161, 196);
			this.labelAgeRating.Name = "labelAgeRating";
			this.labelAgeRating.Size = new System.Drawing.Size(65, 12);
			this.labelAgeRating.TabIndex = 30;
			this.labelAgeRating.Text = "Age Rating:";
			// 
			// txNotes
			// 
			this.txNotes.Location = new System.Drawing.Point(13, 123);
			this.txNotes.Multiline = true;
			this.txNotes.Name = "txNotes";
			this.txNotes.Size = new System.Drawing.Size(220, 55);
			this.txNotes.TabIndex = 7;
			// 
			// labelNotes
			// 
			this.labelNotes.AutoSize = true;
			this.labelNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelNotes.Location = new System.Drawing.Point(15, 109);
			this.labelNotes.Name = "labelNotes";
			this.labelNotes.Size = new System.Drawing.Size(39, 12);
			this.labelNotes.TabIndex = 6;
			this.labelNotes.Text = "Notes:";
			// 
			// txCharacters
			// 
			this.txCharacters.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txCharacters.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txCharacters.Location = new System.Drawing.Point(244, 83);
			this.txCharacters.Name = "txCharacters";
			this.txCharacters.Size = new System.Drawing.Size(223, 20);
			this.txCharacters.TabIndex = 5;
			// 
			// labelCharacters
			// 
			this.labelCharacters.AutoSize = true;
			this.labelCharacters.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelCharacters.Location = new System.Drawing.Point(245, 69);
			this.labelCharacters.Name = "labelCharacters";
			this.labelCharacters.Size = new System.Drawing.Size(65, 12);
			this.labelCharacters.TabIndex = 4;
			this.labelCharacters.Text = "Characters:";
			// 
			// txGenre
			// 
			this.txGenre.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txGenre.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txGenre.Location = new System.Drawing.Point(164, 285);
			this.txGenre.Name = "txGenre";
			this.txGenre.Size = new System.Drawing.Size(145, 20);
			this.txGenre.TabIndex = 43;
			// 
			// txWeb
			// 
			this.txWeb.Location = new System.Drawing.Point(245, 231);
			this.txWeb.Name = "txWeb";
			this.txWeb.Size = new System.Drawing.Size(221, 20);
			this.txWeb.TabIndex = 17;
			// 
			// labelWeb
			// 
			this.labelWeb.AutoSize = true;
			this.labelWeb.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelWeb.Location = new System.Drawing.Point(244, 217);
			this.labelWeb.Name = "labelWeb";
			this.labelWeb.Size = new System.Drawing.Size(31, 12);
			this.labelWeb.TabIndex = 16;
			this.labelWeb.Text = "Web:";
			// 
			// txTeams
			// 
			this.txTeams.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txTeams.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txTeams.Location = new System.Drawing.Point(243, 123);
			this.txTeams.Name = "txTeams";
			this.txTeams.Size = new System.Drawing.Size(224, 20);
			this.txTeams.TabIndex = 9;
			// 
			// labelTeams
			// 
			this.labelTeams.AutoSize = true;
			this.labelTeams.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTeams.Location = new System.Drawing.Point(244, 110);
			this.labelTeams.Name = "labelTeams";
			this.labelTeams.Size = new System.Drawing.Size(42, 12);
			this.labelTeams.TabIndex = 8;
			this.labelTeams.Text = "Teams:";
			// 
			// txLocations
			// 
			this.txLocations.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txLocations.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txLocations.Location = new System.Drawing.Point(243, 159);
			this.txLocations.Name = "txLocations";
			this.txLocations.Size = new System.Drawing.Size(223, 20);
			this.txLocations.TabIndex = 11;
			// 
			// labelLocations
			// 
			this.labelLocations.AutoSize = true;
			this.labelLocations.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelLocations.Location = new System.Drawing.Point(244, 146);
			this.labelLocations.Name = "labelLocations";
			this.labelLocations.Size = new System.Drawing.Size(58, 12);
			this.labelLocations.TabIndex = 10;
			this.labelLocations.Text = "Locations:";
			// 
			// txCommunityRating
			// 
			this.txCommunityRating.DrawText = true;
			this.txCommunityRating.Font = new System.Drawing.Font("Arial", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txCommunityRating.ForeColor = System.Drawing.SystemColors.GrayText;
			this.txCommunityRating.Location = new System.Drawing.Point(248, 364);
			this.txCommunityRating.Name = "txCommunityRating";
			this.txCommunityRating.Rating = 3F;
			this.txCommunityRating.RatingImage = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.StarBlue;
			this.txCommunityRating.Size = new System.Drawing.Size(219, 20);
			this.txCommunityRating.TabIndex = 51;
			this.txCommunityRating.Text = "3";
			// 
			// labelCommunityRating
			// 
			this.labelCommunityRating.AutoSize = true;
			this.labelCommunityRating.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelCommunityRating.Location = new System.Drawing.Point(247, 349);
			this.labelCommunityRating.Name = "labelCommunityRating";
			this.labelCommunityRating.Size = new System.Drawing.Size(102, 12);
			this.labelCommunityRating.TabIndex = 50;
			this.labelCommunityRating.Text = "Community Rating:";
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
			this.pageData.Size = new System.Drawing.Size(497, 407);
			this.pageData.TabIndex = 72;
			// 
			// grpCustom
			// 
			this.grpCustom.Controls.Add(this.textCustomField);
			this.grpCustom.Controls.Add(this.labelCustomField);
			this.grpCustom.Dock = System.Windows.Forms.DockStyle.Top;
			this.grpCustom.Location = new System.Drawing.Point(0, 1261);
			this.grpCustom.Name = "grpCustom";
			this.grpCustom.Size = new System.Drawing.Size(478, 91);
			this.grpCustom.TabIndex = 4;
			this.grpCustom.Text = "Custom Fields";
			// 
			// textCustomField
			// 
			this.textCustomField.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.textCustomField.Location = new System.Drawing.Point(11, 53);
			this.textCustomField.Name = "textCustomField";
			this.textCustomField.Size = new System.Drawing.Size(451, 20);
			this.textCustomField.TabIndex = 25;
			this.textCustomField.Tag = "";
			// 
			// labelCustomField
			// 
			this.labelCustomField.AutoSize = true;
			this.labelCustomField.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelCustomField.Location = new System.Drawing.Point(10, 38);
			this.labelCustomField.Name = "labelCustomField";
			this.labelCustomField.Size = new System.Drawing.Size(76, 12);
			this.labelCustomField.TabIndex = 24;
			this.labelCustomField.Text = "Custom Field:";
			// 
			// grpCatalog
			// 
			this.grpCatalog.Controls.Add(this.labelReleasedTime);
			this.grpCatalog.Controls.Add(this.dtpReleasedTime);
			this.grpCatalog.Controls.Add(this.labelOpenedTime);
			this.grpCatalog.Controls.Add(this.dtpOpenedTime);
			this.grpCatalog.Controls.Add(this.labelAddedTime);
			this.grpCatalog.Controls.Add(this.dtpAddedTime);
			this.grpCatalog.Controls.Add(this.txPagesAsTextSimple);
			this.grpCatalog.Controls.Add(this.labelPagesAsTextSimple);
			this.grpCatalog.Controls.Add(this.txISBN);
			this.grpCatalog.Controls.Add(this.labelISBN);
			this.grpCatalog.Controls.Add(this.txBookNotes);
			this.grpCatalog.Controls.Add(this.labelBookNotes);
			this.grpCatalog.Controls.Add(this.cbBookLocation);
			this.grpCatalog.Controls.Add(this.labelBookLocation);
			this.grpCatalog.Controls.Add(this.txCollectionStatus);
			this.grpCatalog.Controls.Add(this.cbBookPrice);
			this.grpCatalog.Controls.Add(this.labelBookPrice);
			this.grpCatalog.Controls.Add(this.cbBookAge);
			this.grpCatalog.Controls.Add(this.labelBookAge);
			this.grpCatalog.Controls.Add(this.labelBookCollectionStatus);
			this.grpCatalog.Controls.Add(this.cbBookCondition);
			this.grpCatalog.Controls.Add(this.labelBookCondition);
			this.grpCatalog.Controls.Add(this.cbBookStore);
			this.grpCatalog.Controls.Add(this.labelBookStore);
			this.grpCatalog.Controls.Add(this.cbBookOwner);
			this.grpCatalog.Controls.Add(this.labelBookOwner);
			this.grpCatalog.Dock = System.Windows.Forms.DockStyle.Top;
			this.grpCatalog.Location = new System.Drawing.Point(0, 869);
			this.grpCatalog.Name = "grpCatalog";
			this.grpCatalog.Size = new System.Drawing.Size(478, 392);
			this.grpCatalog.TabIndex = 2;
			this.grpCatalog.Text = "Catalog";
			// 
			// labelReleasedTime
			// 
			this.labelReleasedTime.AutoSize = true;
			this.labelReleasedTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelReleasedTime.Location = new System.Drawing.Point(14, 191);
			this.labelReleasedTime.Name = "labelReleasedTime";
			this.labelReleasedTime.Size = new System.Drawing.Size(56, 12);
			this.labelReleasedTime.TabIndex = 16;
			this.labelReleasedTime.Text = "Released:";
			// 
			// dtpReleasedTime
			// 
			this.dtpReleasedTime.CustomFormat = " ";
			this.dtpReleasedTime.Location = new System.Drawing.Point(13, 206);
			this.dtpReleasedTime.Name = "dtpReleasedTime";
			this.dtpReleasedTime.Size = new System.Drawing.Size(220, 20);
			this.dtpReleasedTime.TabIndex = 17;
			this.dtpReleasedTime.Tag = "ReleasedTime";
			this.dtpReleasedTime.Value = new System.DateTime(((long)(0)));
			// 
			// labelOpenedTime
			// 
			this.labelOpenedTime.AutoSize = true;
			this.labelOpenedTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelOpenedTime.Location = new System.Drawing.Point(248, 229);
			this.labelOpenedTime.Name = "labelOpenedTime";
			this.labelOpenedTime.Size = new System.Drawing.Size(77, 12);
			this.labelOpenedTime.TabIndex = 20;
			this.labelOpenedTime.Text = "Opened/Read:";
			// 
			// dtpOpenedTime
			// 
			this.dtpOpenedTime.CustomFormat = " ";
			this.dtpOpenedTime.Location = new System.Drawing.Point(246, 244);
			this.dtpOpenedTime.Name = "dtpOpenedTime";
			this.dtpOpenedTime.Size = new System.Drawing.Size(218, 20);
			this.dtpOpenedTime.TabIndex = 21;
			this.dtpOpenedTime.Tag = "OpenedTime";
			this.dtpOpenedTime.Value = new System.DateTime(((long)(0)));
			// 
			// labelAddedTime
			// 
			this.labelAddedTime.AutoSize = true;
			this.labelAddedTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelAddedTime.Location = new System.Drawing.Point(246, 191);
			this.labelAddedTime.Name = "labelAddedTime";
			this.labelAddedTime.Size = new System.Drawing.Size(98, 12);
			this.labelAddedTime.TabIndex = 18;
			this.labelAddedTime.Text = "Added/Purchased:";
			// 
			// dtpAddedTime
			// 
			this.dtpAddedTime.CustomFormat = " ";
			this.dtpAddedTime.Location = new System.Drawing.Point(246, 206);
			this.dtpAddedTime.Name = "dtpAddedTime";
			this.dtpAddedTime.Size = new System.Drawing.Size(218, 20);
			this.dtpAddedTime.TabIndex = 19;
			this.dtpAddedTime.Tag = "AddedTime";
			this.dtpAddedTime.Value = new System.DateTime(((long)(0)));
			// 
			// txPagesAsTextSimple
			// 
			this.txPagesAsTextSimple.AcceptsReturn = true;
			this.txPagesAsTextSimple.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.txPagesAsTextSimple.Location = new System.Drawing.Point(246, 82);
			this.txPagesAsTextSimple.Name = "txPagesAsTextSimple";
			this.txPagesAsTextSimple.Size = new System.Drawing.Size(218, 20);
			this.txPagesAsTextSimple.TabIndex = 7;
			this.txPagesAsTextSimple.Tag = "PagesAsTextSimple";
			// 
			// labelPagesAsTextSimple
			// 
			this.labelPagesAsTextSimple.AutoSize = true;
			this.labelPagesAsTextSimple.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelPagesAsTextSimple.Location = new System.Drawing.Point(245, 67);
			this.labelPagesAsTextSimple.Name = "labelPagesAsTextSimple";
			this.labelPagesAsTextSimple.Size = new System.Drawing.Size(40, 12);
			this.labelPagesAsTextSimple.TabIndex = 6;
			this.labelPagesAsTextSimple.Text = "Pages:";
			// 
			// txISBN
			// 
			this.txISBN.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.txISBN.Location = new System.Drawing.Point(13, 82);
			this.txISBN.Name = "txISBN";
			this.txISBN.Size = new System.Drawing.Size(220, 20);
			this.txISBN.TabIndex = 5;
			this.txISBN.Tag = "ISBN";
			// 
			// labelISBN
			// 
			this.labelISBN.AutoSize = true;
			this.labelISBN.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelISBN.Location = new System.Drawing.Point(13, 67);
			this.labelISBN.Name = "labelISBN";
			this.labelISBN.Size = new System.Drawing.Size(35, 12);
			this.labelISBN.TabIndex = 4;
			this.labelISBN.Text = "ISBN:";
			// 
			// txBookNotes
			// 
			this.txBookNotes.AcceptsReturn = true;
			this.txBookNotes.FocusSelect = false;
			this.txBookNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.txBookNotes.Location = new System.Drawing.Point(13, 327);
			this.txBookNotes.Multiline = true;
			this.txBookNotes.Name = "txBookNotes";
			this.txBookNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txBookNotes.Size = new System.Drawing.Size(452, 50);
			this.txBookNotes.TabIndex = 25;
			this.txBookNotes.Tag = "BookNotes";
			// 
			// labelBookNotes
			// 
			this.labelBookNotes.AutoSize = true;
			this.labelBookNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBookNotes.Location = new System.Drawing.Point(13, 312);
			this.labelBookNotes.Name = "labelBookNotes";
			this.labelBookNotes.Size = new System.Drawing.Size(120, 12);
			this.labelBookNotes.TabIndex = 24;
			this.labelBookNotes.Text = "Notes about this Book:";
			// 
			// cbBookLocation
			// 
			this.cbBookLocation.FormattingEnabled = true;
			this.cbBookLocation.Location = new System.Drawing.Point(246, 158);
			this.cbBookLocation.Name = "cbBookLocation";
			this.cbBookLocation.Size = new System.Drawing.Size(218, 21);
			this.cbBookLocation.TabIndex = 15;
			this.cbBookLocation.Tag = "BookLocation";
			// 
			// labelBookLocation
			// 
			this.labelBookLocation.AutoSize = true;
			this.labelBookLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBookLocation.Location = new System.Drawing.Point(245, 144);
			this.labelBookLocation.Name = "labelBookLocation";
			this.labelBookLocation.Size = new System.Drawing.Size(80, 12);
			this.labelBookLocation.TabIndex = 14;
			this.labelBookLocation.Text = "Book Location:";
			// 
			// txCollectionStatus
			// 
			this.txCollectionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.txCollectionStatus.Location = new System.Drawing.Point(13, 286);
			this.txCollectionStatus.Name = "txCollectionStatus";
			this.txCollectionStatus.Size = new System.Drawing.Size(451, 20);
			this.txCollectionStatus.TabIndex = 23;
			this.txCollectionStatus.Tag = "CollectionStatus";
			// 
			// cbBookPrice
			// 
			this.cbBookPrice.FormattingEnabled = true;
			this.cbBookPrice.Location = new System.Drawing.Point(246, 44);
			this.cbBookPrice.Name = "cbBookPrice";
			this.cbBookPrice.Size = new System.Drawing.Size(218, 21);
			this.cbBookPrice.TabIndex = 3;
			this.cbBookPrice.Tag = "BookPrice";
			// 
			// labelBookPrice
			// 
			this.labelBookPrice.AutoSize = true;
			this.labelBookPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBookPrice.Location = new System.Drawing.Point(244, 30);
			this.labelBookPrice.Name = "labelBookPrice";
			this.labelBookPrice.Size = new System.Drawing.Size(35, 12);
			this.labelBookPrice.TabIndex = 2;
			this.labelBookPrice.Text = "Price:";
			// 
			// cbBookAge
			// 
			this.cbBookAge.FormattingEnabled = true;
			this.cbBookAge.Location = new System.Drawing.Point(13, 121);
			this.cbBookAge.Name = "cbBookAge";
			this.cbBookAge.Size = new System.Drawing.Size(220, 21);
			this.cbBookAge.TabIndex = 9;
			this.cbBookAge.Tag = "BookAge";
			// 
			// labelBookAge
			// 
			this.labelBookAge.AutoSize = true;
			this.labelBookAge.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBookAge.Location = new System.Drawing.Point(12, 106);
			this.labelBookAge.Name = "labelBookAge";
			this.labelBookAge.Size = new System.Drawing.Size(29, 12);
			this.labelBookAge.TabIndex = 8;
			this.labelBookAge.Text = "Age:";
			// 
			// labelBookCollectionStatus
			// 
			this.labelBookCollectionStatus.AutoSize = true;
			this.labelBookCollectionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBookCollectionStatus.Location = new System.Drawing.Point(12, 271);
			this.labelBookCollectionStatus.Name = "labelBookCollectionStatus";
			this.labelBookCollectionStatus.Size = new System.Drawing.Size(96, 12);
			this.labelBookCollectionStatus.TabIndex = 22;
			this.labelBookCollectionStatus.Text = "Collection Status:";
			// 
			// cbBookCondition
			// 
			this.cbBookCondition.FormattingEnabled = true;
			this.cbBookCondition.Location = new System.Drawing.Point(246, 120);
			this.cbBookCondition.Name = "cbBookCondition";
			this.cbBookCondition.Size = new System.Drawing.Size(218, 21);
			this.cbBookCondition.TabIndex = 11;
			this.cbBookCondition.Tag = "BookCondition";
			// 
			// labelBookCondition
			// 
			this.labelBookCondition.AutoSize = true;
			this.labelBookCondition.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBookCondition.Location = new System.Drawing.Point(244, 106);
			this.labelBookCondition.Name = "labelBookCondition";
			this.labelBookCondition.Size = new System.Drawing.Size(57, 12);
			this.labelBookCondition.TabIndex = 10;
			this.labelBookCondition.Text = "Condition:";
			// 
			// cbBookStore
			// 
			this.cbBookStore.FormattingEnabled = true;
			this.cbBookStore.Location = new System.Drawing.Point(13, 45);
			this.cbBookStore.Name = "cbBookStore";
			this.cbBookStore.PromptText = null;
			this.cbBookStore.Size = new System.Drawing.Size(220, 21);
			this.cbBookStore.TabIndex = 1;
			this.cbBookStore.Tag = "BookStore";
			// 
			// labelBookStore
			// 
			this.labelBookStore.AutoSize = true;
			this.labelBookStore.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBookStore.Location = new System.Drawing.Point(11, 30);
			this.labelBookStore.Name = "labelBookStore";
			this.labelBookStore.Size = new System.Drawing.Size(36, 12);
			this.labelBookStore.TabIndex = 0;
			this.labelBookStore.Text = "Store:";
			// 
			// cbBookOwner
			// 
			this.cbBookOwner.FormattingEnabled = true;
			this.cbBookOwner.Location = new System.Drawing.Point(13, 158);
			this.cbBookOwner.Name = "cbBookOwner";
			this.cbBookOwner.Size = new System.Drawing.Size(220, 21);
			this.cbBookOwner.TabIndex = 13;
			this.cbBookOwner.Tag = "BookOwner";
			// 
			// labelBookOwner
			// 
			this.labelBookOwner.AutoSize = true;
			this.labelBookOwner.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelBookOwner.Location = new System.Drawing.Point(13, 144);
			this.labelBookOwner.Name = "labelBookOwner";
			this.labelBookOwner.Size = new System.Drawing.Size(42, 12);
			this.labelBookOwner.TabIndex = 12;
			this.labelBookOwner.Text = "Owner:";
			// 
			// grpPlotNotes
			// 
			this.grpPlotNotes.Controls.Add(this.labelMainCharacterOrTeam);
			this.grpPlotNotes.Controls.Add(this.txMainCharacterOrTeam);
			this.grpPlotNotes.Controls.Add(this.labelReview);
			this.grpPlotNotes.Controls.Add(this.txReview);
			this.grpPlotNotes.Controls.Add(this.txScanInformation);
			this.grpPlotNotes.Controls.Add(this.labelScanInformation);
			this.grpPlotNotes.Controls.Add(this.txWeb);
			this.grpPlotNotes.Controls.Add(this.txSummary);
			this.grpPlotNotes.Controls.Add(this.labelSummary);
			this.grpPlotNotes.Controls.Add(this.labelNotes);
			this.grpPlotNotes.Controls.Add(this.txNotes);
			this.grpPlotNotes.Controls.Add(this.labelCharacters);
			this.grpPlotNotes.Controls.Add(this.txCharacters);
			this.grpPlotNotes.Controls.Add(this.labelWeb);
			this.grpPlotNotes.Controls.Add(this.labelTeams);
			this.grpPlotNotes.Controls.Add(this.txTeams);
			this.grpPlotNotes.Controls.Add(this.labelLocations);
			this.grpPlotNotes.Controls.Add(this.txLocations);
			this.grpPlotNotes.Dock = System.Windows.Forms.DockStyle.Top;
			this.grpPlotNotes.Location = new System.Drawing.Point(0, 604);
			this.grpPlotNotes.Name = "grpPlotNotes";
			this.grpPlotNotes.Size = new System.Drawing.Size(478, 265);
			this.grpPlotNotes.TabIndex = 3;
			this.grpPlotNotes.Text = "Plot & Notes";
			// 
			// labelMainCharacterOrTeam
			// 
			this.labelMainCharacterOrTeam.AutoSize = true;
			this.labelMainCharacterOrTeam.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelMainCharacterOrTeam.Location = new System.Drawing.Point(247, 33);
			this.labelMainCharacterOrTeam.Name = "labelMainCharacterOrTeam";
			this.labelMainCharacterOrTeam.Size = new System.Drawing.Size(118, 12);
			this.labelMainCharacterOrTeam.TabIndex = 2;
			this.labelMainCharacterOrTeam.Text = "Main Character/Team:";
			// 
			// txMainCharacterOrTeam
			// 
			this.txMainCharacterOrTeam.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txMainCharacterOrTeam.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txMainCharacterOrTeam.Location = new System.Drawing.Point(243, 48);
			this.txMainCharacterOrTeam.Name = "txMainCharacterOrTeam";
			this.txMainCharacterOrTeam.Size = new System.Drawing.Size(224, 20);
			this.txMainCharacterOrTeam.TabIndex = 3;
			// 
			// labelReview
			// 
			this.labelReview.AutoSize = true;
			this.labelReview.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelReview.Location = new System.Drawing.Point(16, 182);
			this.labelReview.Name = "labelReview";
			this.labelReview.Size = new System.Drawing.Size(48, 12);
			this.labelReview.TabIndex = 12;
			this.labelReview.Text = "Review:";
			// 
			// txReview
			// 
			this.txReview.Location = new System.Drawing.Point(14, 196);
			this.txReview.Multiline = true;
			this.txReview.Name = "txReview";
			this.txReview.Size = new System.Drawing.Size(220, 55);
			this.txReview.TabIndex = 13;
			// 
			// txScanInformation
			// 
			this.txScanInformation.Location = new System.Drawing.Point(244, 196);
			this.txScanInformation.Name = "txScanInformation";
			this.txScanInformation.Size = new System.Drawing.Size(222, 20);
			this.txScanInformation.TabIndex = 15;
			// 
			// labelScanInformation
			// 
			this.labelScanInformation.AutoSize = true;
			this.labelScanInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelScanInformation.Location = new System.Drawing.Point(243, 182);
			this.labelScanInformation.Name = "labelScanInformation";
			this.labelScanInformation.Size = new System.Drawing.Size(95, 12);
			this.labelScanInformation.TabIndex = 14;
			this.labelScanInformation.Text = "Scan Information:";
			// 
			// grpArtists
			// 
			this.grpArtists.Controls.Add(this.labelTranslator);
			this.grpArtists.Controls.Add(this.labelWriter);
			this.grpArtists.Controls.Add(this.labelPenciller);
			this.grpArtists.Controls.Add(this.labelInker);
			this.grpArtists.Controls.Add(this.labelColorist);
			this.grpArtists.Controls.Add(this.txPenciller);
			this.grpArtists.Controls.Add(this.txInker);
			this.grpArtists.Controls.Add(this.txColorist);
			this.grpArtists.Controls.Add(this.txEditor);
			this.grpArtists.Controls.Add(this.txTranslator);
			this.grpArtists.Controls.Add(this.txWriter);
			this.grpArtists.Controls.Add(this.txCoverArtist);
			this.grpArtists.Controls.Add(this.labelLetterer);
			this.grpArtists.Controls.Add(this.txLetterer);
			this.grpArtists.Controls.Add(this.labelCoverArtist);
			this.grpArtists.Controls.Add(this.labelEditor);
			this.grpArtists.Dock = System.Windows.Forms.DockStyle.Top;
			this.grpArtists.Location = new System.Drawing.Point(0, 400);
			this.grpArtists.Name = "grpArtists";
			this.grpArtists.Size = new System.Drawing.Size(478, 204);
			this.grpArtists.TabIndex = 1;
			this.grpArtists.Text = "Artists / People Involved";
			// 
			// labelTranslator
			// 
			this.labelTranslator.AutoSize = true;
			this.labelTranslator.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTranslator.Location = new System.Drawing.Point(243, 153);
			this.labelTranslator.Name = "labelTranslator";
			this.labelTranslator.Size = new System.Drawing.Size(60, 12);
			this.labelTranslator.TabIndex = 14;
			this.labelTranslator.Text = "Translator:";
			// 
			// txTranslator
			// 
			this.txTranslator.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txTranslator.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txTranslator.Location = new System.Drawing.Point(245, 167);
			this.txTranslator.Name = "txTranslator";
			this.txTranslator.Size = new System.Drawing.Size(220, 20);
			this.txTranslator.TabIndex = 8;
			// 
			// grpMain
			// 
			this.grpMain.Controls.Add(this.labelDay);
			this.grpMain.Controls.Add(this.txDay);
			this.grpMain.Controls.Add(this.labelSeriesGroup);
			this.grpMain.Controls.Add(this.txSeriesGroup);
			this.grpMain.Controls.Add(this.labelStoryArc);
			this.grpMain.Controls.Add(this.txStoryArc);
			this.grpMain.Controls.Add(this.cbSeriesComplete);
			this.grpMain.Controls.Add(this.labelSeriesComplete);
			this.grpMain.Controls.Add(this.cbEnableProposed);
			this.grpMain.Controls.Add(this.labelRating);
			this.grpMain.Controls.Add(this.txCommunityRating);
			this.grpMain.Controls.Add(this.labelEnableProposed);
			this.grpMain.Controls.Add(this.labelFormat);
			this.grpMain.Controls.Add(this.cbImprint);
			this.grpMain.Controls.Add(this.labelTags);
			this.grpMain.Controls.Add(this.labelAlternateSeries);
			this.grpMain.Controls.Add(this.labelImprint);
			this.grpMain.Controls.Add(this.labelCommunityRating);
			this.grpMain.Controls.Add(this.labelAlternateNumber);
			this.grpMain.Controls.Add(this.cbFormat);
			this.grpMain.Controls.Add(this.labelGenre);
			this.grpMain.Controls.Add(this.labelSeries);
			this.grpMain.Controls.Add(this.cbPublisher);
			this.grpMain.Controls.Add(this.txGenre);
			this.grpMain.Controls.Add(this.labelAlternateCount);
			this.grpMain.Controls.Add(this.labelPublisher);
			this.grpMain.Controls.Add(this.txRating);
			this.grpMain.Controls.Add(this.labelTitle);
			this.grpMain.Controls.Add(this.labelAgeRating);
			this.grpMain.Controls.Add(this.txTags);
			this.grpMain.Controls.Add(this.txAlternateSeries);
			this.grpMain.Controls.Add(this.labelLanguage);
			this.grpMain.Controls.Add(this.labelYear);
			this.grpMain.Controls.Add(this.cbLanguage);
			this.grpMain.Controls.Add(this.cbManga);
			this.grpMain.Controls.Add(this.txAlternateNumber);
			this.grpMain.Controls.Add(this.labelManga);
			this.grpMain.Controls.Add(this.labelNumber);
			this.grpMain.Controls.Add(this.labelBlackAndWhite);
			this.grpMain.Controls.Add(this.cbBlackAndWhite);
			this.grpMain.Controls.Add(this.txAlternateCount);
			this.grpMain.Controls.Add(this.cbAgeRating);
			this.grpMain.Controls.Add(this.labelMonth);
			this.grpMain.Controls.Add(this.labelVolume);
			this.grpMain.Controls.Add(this.labelCount);
			this.grpMain.Controls.Add(this.txTitle);
			this.grpMain.Controls.Add(this.txSeries);
			this.grpMain.Controls.Add(this.txYear);
			this.grpMain.Controls.Add(this.txMonth);
			this.grpMain.Controls.Add(this.txVolume);
			this.grpMain.Controls.Add(this.txNumber);
			this.grpMain.Controls.Add(this.txCount);
			this.grpMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.grpMain.Location = new System.Drawing.Point(0, 0);
			this.grpMain.Name = "grpMain";
			this.grpMain.Size = new System.Drawing.Size(478, 400);
			this.grpMain.TabIndex = 0;
			this.grpMain.Text = "Main";
			// 
			// labelDay
			// 
			this.labelDay.AutoSize = true;
			this.labelDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelDay.Location = new System.Drawing.Point(393, 74);
			this.labelDay.Name = "labelDay";
			this.labelDay.Size = new System.Drawing.Size(29, 12);
			this.labelDay.TabIndex = 14;
			this.labelDay.Text = "Day:";
			// 
			// txDay
			// 
			this.txDay.Location = new System.Drawing.Point(395, 87);
			this.txDay.MaxLength = 4;
			this.txDay.Name = "txDay";
			this.txDay.PromptText = "";
			this.txDay.Size = new System.Drawing.Size(71, 20);
			this.txDay.TabIndex = 15;
			// 
			// labelSeriesGroup
			// 
			this.labelSeriesGroup.AutoSize = true;
			this.labelSeriesGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSeriesGroup.Location = new System.Drawing.Point(163, 150);
			this.labelSeriesGroup.Name = "labelSeriesGroup";
			this.labelSeriesGroup.Size = new System.Drawing.Size(74, 12);
			this.labelSeriesGroup.TabIndex = 24;
			this.labelSeriesGroup.Text = "Series Group:";
			// 
			// txSeriesGroup
			// 
			this.txSeriesGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txSeriesGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txSeriesGroup.Location = new System.Drawing.Point(166, 165);
			this.txSeriesGroup.Name = "txSeriesGroup";
			this.txSeriesGroup.PromptText = "";
			this.txSeriesGroup.Size = new System.Drawing.Size(143, 20);
			this.txSeriesGroup.TabIndex = 25;
			this.txSeriesGroup.Tag = "";
			// 
			// labelStoryArc
			// 
			this.labelStoryArc.AutoSize = true;
			this.labelStoryArc.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelStoryArc.Location = new System.Drawing.Point(9, 150);
			this.labelStoryArc.Name = "labelStoryArc";
			this.labelStoryArc.Size = new System.Drawing.Size(57, 12);
			this.labelStoryArc.TabIndex = 22;
			this.labelStoryArc.Text = "Story Arc:";
			// 
			// txStoryArc
			// 
			this.txStoryArc.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.txStoryArc.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.txStoryArc.Location = new System.Drawing.Point(11, 164);
			this.txStoryArc.Name = "txStoryArc";
			this.txStoryArc.PromptText = "";
			this.txStoryArc.Size = new System.Drawing.Size(144, 20);
			this.txStoryArc.TabIndex = 23;
			this.txStoryArc.Tag = "";
			// 
			// cbSeriesComplete
			// 
			this.cbSeriesComplete.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSeriesComplete.FormattingEnabled = true;
			this.cbSeriesComplete.Location = new System.Drawing.Point(317, 165);
			this.cbSeriesComplete.Name = "cbSeriesComplete";
			this.cbSeriesComplete.Size = new System.Drawing.Size(149, 21);
			this.cbSeriesComplete.TabIndex = 27;
			// 
			// labelSeriesComplete
			// 
			this.labelSeriesComplete.AutoSize = true;
			this.labelSeriesComplete.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSeriesComplete.Location = new System.Drawing.Point(315, 152);
			this.labelSeriesComplete.Name = "labelSeriesComplete";
			this.labelSeriesComplete.Size = new System.Drawing.Size(90, 12);
			this.labelSeriesComplete.TabIndex = 26;
			this.labelSeriesComplete.Text = "Series complete:";
			// 
			// MultipleComicBooksDialog
			// 
			this.AcceptButton = this.btOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btCancel;
			this.ClientSize = new System.Drawing.Size(525, 460);
			this.Controls.Add(this.pageData);
			this.Controls.Add(this.btCancel);
			this.Controls.Add(this.btOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MultipleComicBooksDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Multiple Book Information ({0})";
			this.pageData.ResumeLayout(false);
			this.grpCustom.ResumeLayout(false);
			this.grpCustom.PerformLayout();
			this.grpCatalog.ResumeLayout(false);
			this.grpCatalog.PerformLayout();
			this.grpPlotNotes.ResumeLayout(false);
			this.grpPlotNotes.PerformLayout();
			this.grpArtists.ResumeLayout(false);
			this.grpArtists.PerformLayout();
			this.grpMain.ResumeLayout(false);
			this.grpMain.PerformLayout();
			this.ResumeLayout(false);

		}
		
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
		private TextBoxEx txTranslator;
		private Label labelTranslator;
	}
}
