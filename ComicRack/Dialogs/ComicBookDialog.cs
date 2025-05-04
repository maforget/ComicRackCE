using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using cYo.Common;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Text;
using cYo.Common.Win32;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Controls;
using cYo.Projects.ComicRack.Engine.Display;
using cYo.Projects.ComicRack.Engine.Drawing;
using cYo.Projects.ComicRack.Engine.IO;
using cYo.Projects.ComicRack.Plugins;
using cYo.Projects.ComicRack.Viewer.Controls;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class ComicBookDialog : Form
    {
        private static int lastActivePage = -1;

        private static int lastActiveTextPage = -1;

        private static bool showColorControls = false;

        private ComicBook current;

        private ComicBook displayComic;

        private readonly ComicBook[] allBooks;

        private Func<ComicBook, bool> selectComicHandler;

        private Control currentTextBox;

        private int pendingPageViewerPage = -1;

        private int pageViewPage = -1;

        private string customThumbnailKey;

        private static int currentScript;

        private static ItemViewConfig pagesConfig = new ItemViewConfig();

        private static PluginEngine scriptEngine;

        public string CustomThumbnailKey
        {
            get
            {
                return customThumbnailKey;
            }
            set
            {
                if (!displayComic.IsLinked && !(value == customThumbnailKey))
                {
                    ComicBook comicBook = new ComicBook(displayComic);
                    string text2 = (customThumbnailKey = (comicBook.CustomThumbnailKey = value));
                    SetCoverThumbnailImage(coverThumbnail, comicBook);
                }
            }
        }

        public static ItemViewConfig PagesConfig
        {
            get
            {
                return pagesConfig;
            }
            set
            {
                pagesConfig = value;
            }
        }

        public static PluginEngine ScriptEngine
        {
            get
            {
                return scriptEngine;
            }
            set
            {
                scriptEngine = value;
            }
        }

        private ComicBookDialog(ComicBook current, ComicBook[] allBooks)
        {
            LocalizeUtility.UpdateRightToLeft(this);
            InitializeComponent();
            btFirstPage.Image = ((Bitmap)btFirstPage.Image).ScaleDpi();
            btLastPage.Image = ((Bitmap)btLastPage.Image).ScaleDpi();
            btNextPage.Image = ((Bitmap)btNextPage.Image).ScaleDpi();
            btPrevPage.Image = ((Bitmap)btPrevPage.Image).ScaleDpi();
            chkShowImageControls.Image = ((Bitmap)chkShowImageControls.Image).ScaleDpi();
            btPageView.Image = ((Bitmap)btPageView.Image).ScaleDpi();
            this.RestorePosition();
            coverThumbnail.MouseWheel += coverThumbnail_MouseWheel;
            TextBoxContextMenu.Register(this, TextBoxContextMenu.AddSearchLinks(SearchEngines.Engines));
            LocalizeUtility.Localize(this, components);
            pageViewer.Text = TR.Load(base.Name)[pageViewer.Name, pageViewer.Text];
            ComicListField.TranslateColumns(pagesView.ItemView.Columns);
            if (PagesConfig != null)
            {
                pagesView.ViewConfig = PagesConfig;
            }
            new ComboBoxSkinner(cbImprint, ComicBook.PublisherIcons) { MaxHeightScale = 2 };
            new ComboBoxSkinner(cbPublisher, ComicBook.PublisherIcons) { MaxHeightScale = 2};
            new ComboBoxSkinner(cbFormat, ComicBook.FormatIcons) { MaxHeightScale = 2 };
            new ComboBoxSkinner(cbAgeRating, ComicBook.AgeRatingIcons) { MaxHeightScale = 2 };
            new ComboBoxSkinner(cbBookPrice);
            new ComboBoxSkinner(cbBookOwner);
            new ComboBoxSkinner(cbBookStore);
            new ComboBoxSkinner(cbBookAge);
            new ComboBoxSkinner(cbBookCondition);
            new ComboBoxSkinner(cbBookLocation);
            //ListSelectorControl.Register(SearchEngines.Engines, txWriter, txPenciller, txInker, txColorist, txEditor, txTranslator, txCoverArtist, txLetterer, txGenre, txTags, txCharacters, txTeams, txLocations, txCollectionStatus);
            ListSelectorControl.Register(SearchEngines.Engines, txWriter, txPenciller, txInker, txColorist, txEditor, txTranslator, txCoverArtist, txLetterer, txGenre, txTags, txCollectionStatus);
            ListSelectorControl.Register(SearchEngines.Engines, anchorStyles: AnchorStyles.Bottom | AnchorStyles.Right, txCharacters, txTeams, txLocations);
            EditControlUtility.InitializeMangaYesNo(cbManga);
            EditControlUtility.InitializeYesNo(cbBlackAndWhite);
            EditControlUtility.InitializeYesNo(cbSeriesComplete);
            EditControlUtility.InitializeYesNo(cbEnableProposed, withEmpty: false);
            EditControlUtility.InitializeYesNo(cbEnableDynamicUpdate, withEmpty: false);
            if (allBooks == null)
            {
                allBooks = new ComicBook[1]
                {
                    current
                };
            }
            this.allBooks = allBooks;
            pagesView.PageFilter = ComicPageType.AllWithDeleted;
            pagesView.ItemView.SelectedIndexChanged += PagesViewSelectedIndexChanged;
            pagesView.ItemView.ItemActivate += PagesViewItemActivate;
            coverThumbnail.TextElements = ComicTextElements.CaptionWithoutTitle | ComicTextElements.Title | ComicTextElements.ArtistInfo | ComicTextElements.Summary | ComicTextElements.Opened | ComicTextElements.Added | ComicTextElements.Released | ComicTextElements.NoEmptyDates;
            coverThumbnail.DrawingFlags &= ~ThumbnailDrawingOptions.EnableRating;
            coverThumbnail.DrawingFlags &= ~ThumbnailDrawingOptions.EnableBackground;
            coverThumbnail.HighQuality = (Program.Settings.PageImageDisplayOptions & ImageDisplayOptions.HighQuality) != 0;
            EditControlUtility.SetText(txTitle, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.ShadowTitle));
            EditControlUtility.SetText(txSeries, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.ShadowSeries));
            EditControlUtility.SetText(txWriter, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Writer));
            EditControlUtility.SetText(txGenre, null, () => Program.Lists.GetGenreList(withSeparator: false));
            EditControlUtility.SetText(txTags, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Tags));
            EditControlUtility.SetText(txAlternateSeries, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.AlternateSeries));
            EditControlUtility.SetText(txStoryArc, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.StoryArc));
            EditControlUtility.SetText(txSeriesGroup, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.SeriesGroup));
            EditControlUtility.SetText(txPenciller, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Penciller));
            EditControlUtility.SetText(txColorist, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Colorist));
            EditControlUtility.SetText(txInker, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Inker));
            EditControlUtility.SetText(txLetterer, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Letterer));
            EditControlUtility.SetText(txCoverArtist, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.CoverArtist));
            EditControlUtility.SetText(txEditor, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Editor));
            EditControlUtility.SetText(txTranslator, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Translator));
			EditControlUtility.SetText(txCharacters, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Characters));
            EditControlUtility.SetText(txTeams, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Teams));
            EditControlUtility.SetText(txMainCharacterOrTeam, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.MainCharacterOrTeam));
            EditControlUtility.SetText(txLocations, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Locations));
            EditControlUtility.SetText(txScanInformation, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.ScanInformation));
            EditControlUtility.SetText(cbAgeRating, null, () => Program.Lists.GetAgeRatingList());
            EditControlUtility.SetText(cbFormat, null, () => Program.Lists.GetFormatList());
            EditControlUtility.SetText(cbPublisher, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Publisher, sort: true));
            EditControlUtility.SetText(cbImprint, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Imprint, sort: true));
            EditControlUtility.SetText(cbBookAge, null, () => Program.Lists.GetBookAgeList());
            EditControlUtility.SetText(cbBookStore, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.BookStore, sort: true));
            EditControlUtility.SetText(cbBookOwner, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.BookOwner, sort: true));
            EditControlUtility.SetText(cbBookCondition, null, () => Program.Lists.GetBookConditionList());
            EditControlUtility.SetText(cbBookPrice, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.BookPriceAsText, sort: true));
            EditControlUtility.SetText(cbBookLocation, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.BookLocation, sort: true));
            EditControlUtility.SetText(txCollectionStatus, null, Program.Lists.GetBookCollectionStatusList);
            coverThumbnail.ThreeD = Program.Settings.InformationCover3D;
            InitializeScriptButton();
            this.ForEachControl(delegate(TextBox tb)
            {
                tb.Enter += delegate
                {
                    currentTextBox = tb;
                };
            });
			this.ForEachControl(delegate(ComboBox tb)
            {
                tb.Enter += delegate
                {
                    currentTextBox = tb;
                };
            });
			AnchorStyles anchorStyles = AnchorStyles.Top | AnchorStyles.Right;
			SpinButton.AddUpDown(txVolume, anchorStyles: anchorStyles);
            SpinButton.AddUpDown(txCount, 1, 0, anchorStyles: anchorStyles);
            SpinButton.AddUpDown(txNumber, anchorStyles: anchorStyles);
            SpinButton.AddUpDown(txYear, DateTime.Now.Year, anchorStyles: anchorStyles);
            SpinButton.AddUpDown(txMonth, DateTime.Now.Month, 1, 12, anchorStyles: anchorStyles);
            SpinButton.AddUpDown(txDay, DateTime.Now.Month, 1, 31, anchorStyles: anchorStyles);
            SpinButton.AddUpDown(txAlternateCount, 1, 0, anchorStyles: anchorStyles);
            SpinButton.AddUpDown(txAlternateNumber, anchorStyles: anchorStyles);
            SpinButton.AddUpDown(txPagesAsTextSimple, 1, 1, int.MaxValue, 1, registerKeys: false, hidden: true, anchorStyles: anchorStyles);
            txVolume.EnableOnlyNumberKeys();
            txCount.EnableOnlyNumberKeys();
            txYear.EnableOnlyNumberKeys();
            txMonth.EnableOnlyNumberKeys();
            txDay.EnableOnlyNumberKeys();
            txAlternateCount.EnableOnlyNumberKeys();
            txPagesAsTextSimple.EnableOnlyNumberKeys();
            SetCurrentBook(current);
            IdleProcess.Idle += IdleProcess_Idle;
        }

        private void SetCurrentBook(ComicBook comic, bool withRefresh = true)
        {
            selectComicHandler?.Invoke(comic);
            if (withRefresh)
            {
                comic.RefreshInfoFromFile();
            }
            SetComicToEditor(current = comic);
            btPrev.Visible = allBooks.Length > 1;
            btNext.Visible = allBooks.Length > 1;
            btPrev.Enabled = Array.IndexOf(allBooks, comic) > 0;
            btNext.Enabled = Array.IndexOf(allBooks, comic) < allBooks.Length - 1;
        }

        private void SetComicToEditor(ComicBook comic)
        {
            displayComic = comic;
            Text = comic.Caption;
            new Control[4] { labelType, lblType, labelPages, lblPages }.ForEach((Control c) =>
            {
                c.Visible = comic.IsLinked;
            });
            AllowDrop = btThumbnail.Visible = btLinkFile.Visible = !comic.IsLinked && comic.EditMode.IsLocalComic();
            labelWhere.Visible = whereSeparator.Visible = lblPath.Visible = comic.IsLinked && comic.EditMode.IsLocalComic();
            customThumbnailKey = comic.CustomThumbnailKey;
            coverThumbnail.ComicBook = comic;
            pageViewer.SetBitmap(null);
            coverThumbnail.SetBitmap(null);
            SetCoverThumbnailImage(coverThumbnail, comic);
            SetPageView(comic.CurrentPage);
            SetDataToEditor(comic);
            ComicBookNavigator book = pagesView.Book;
            ComicBook comicBook = new ComicBook(comic);
            pagesView.Book = comicBook.CreateNavigator();
            if (pagesView.Book != null)
            {
                if (comicBook.IsDynamicSource)
                {
                    Program.ImagePool.RefreshLastImage(comicBook.FilePath);
                }
                pagesView.Book.Open(async: true, 0);
            }
            book?.Dispose();
            bool isFilelessOrInContainer = !comic.IsLinked || comic.IsInContainer;
            bool canEdit = isFilelessOrInContainer || Program.Settings.UpdateComicFiles;
            bool canEditProperties = comic.EditMode.CanEditProperties();
            tabDetails.Enabled = tabPlot.Enabled = tabColors.Enabled = canEdit && canEditProperties;
            tabPages.Enabled = canEdit && comic.EditMode.CanEditPages();
            tabCatalog.Enabled = tabCustom.Enabled = isFilelessOrInContainer && canEditProperties;
            EnableTabPage(tabPages, comic.IsLinked);
            EnableTabPage(tabColors, comic.IsLinked);
            EnableTabPage(tabCatalog, (!comic.IsLinked || !Program.Settings.CatalogOnlyForFileless) && isFilelessOrInContainer);
            EnableTabPage(tabCustom, Program.Settings.ShowCustomBookFields && isFilelessOrInContainer);
            labelEnableProposed.Visible = cbEnableProposed.Visible = labelScanInformation.Visible = txScanInformation.Visible = comic.IsLinked;
            labelOpenedTime.Visible = dtpOpenedTime.Visible = labelPagesAsTextSimple.Visible = txPagesAsTextSimple.Visible = !comic.IsLinked;
            txCommunityRating.Enabled = txRating.Enabled = cbEnableProposed.Enabled = cbSeriesComplete.Enabled = isFilelessOrInContainer;
            if (!canEditProperties)
            {
                txCommunityRating.Enabled = txRating.Enabled = false;
            }
            linkLabel.Text = comic.Web;
        }

        private void EnableTabPage(TabPage tabPage, bool enable)
        {
            if (enable)
            {
                if (!tabControl.TabPages.Contains(tabPage))
                {
                    tabControl.TabPages.Add(tabPage);
                }
            }
            else
            {
                tabControl.TabPages.Remove(tabPage);
            }
        }

        private void SetDataToEditor(ComicBook comic)
        {
            string text = comic.PagesAsText;
            if (comic.LastPageRead > 0)
                //HACK: When a book contains only one page, display "1/1 Page(s)." Otherwise, it will incorrectly display "Page 2/1 Page(s)."
                text = $"{comic.LastPageRead + (comic.PageCount == 1 ? 0 : 1)}/{text}";

            string fileFormat = comic.ActualFileFormat != comic.FileFormat ? $"{comic.ActualFileFormat} (Actual){Environment.NewLine}{comic.FileFormat}" : comic.FileFormat;

            EditControlUtility.SetLabel(lblPages, text);
            EditControlUtility.SetLabel(lblType, $"{fileFormat}/{comic.FileSizeAsText}");
            EditControlUtility.SetLabel(lblPath, comic.FilePath);
            EditControlUtility.SetText(txRating, comic.Rating);
            EditControlUtility.SetText(txCommunityRating, comic.CommunityRating);
            EditControlUtility.SetText(txTitle, comic.Title);
            EditControlUtility.SetText(txSeries, comic.Series);
            EditControlUtility.SetText(txVolume, comic.Volume);
            EditControlUtility.SetText(txYear, comic.Year);
            EditControlUtility.SetText(txMonth, comic.Month);
            EditControlUtility.SetText(txDay, comic.Day);
            EditControlUtility.SetText(txWriter, comic.Writer);
            EditControlUtility.SetText(txNumber, comic.Number);
            EditControlUtility.SetText(txCount, comic.Count);
            EditControlUtility.SetText(txGenre, comic.Genre);
            EditControlUtility.SetText(cbSeriesComplete, comic.SeriesComplete);
            EditControlUtility.SetText(txTags, comic.Tags);
            EditControlUtility.SetText(cbManga, comic.Manga);
            EditControlUtility.SetText(cbBlackAndWhite, comic.BlackAndWhite);
            EditControlUtility.SetText(cbEnableProposed, comic.EnableProposed ? YesNo.Yes : YesNo.No);
            ComboBox comboBox = cbEnableDynamicUpdate;
            bool visible = (labelEnableDynamicUpdate.Visible = comic.IsDynamicSource);
            comboBox.Visible = visible;
            EditControlUtility.SetText(cbEnableDynamicUpdate, comic.EnableDynamicUpdate ? YesNo.Yes : YesNo.No);
            EditControlUtility.SetText(txAlternateSeries, comic.AlternateSeries);
            txAlternateNumber.Text = comic.AlternateNumber;
            EditControlUtility.SetText(txAlternateCount, comic.AlternateCount);
            EditControlUtility.SetText(txStoryArc, comic.StoryArc);
            EditControlUtility.SetText(txSeriesGroup, comic.SeriesGroup);
            EditControlUtility.SetText(txPenciller, comic.Penciller);
            EditControlUtility.SetText(txColorist, comic.Colorist);
            EditControlUtility.SetText(txInker, comic.Inker);
            EditControlUtility.SetText(txLetterer, comic.Letterer);
            EditControlUtility.SetText(txCoverArtist, comic.CoverArtist);
            EditControlUtility.SetText(txEditor, comic.Editor);
            EditControlUtility.SetText(txTranslator, comic.Translator);
			EditControlUtility.SetText(cbFormat, comic.Format);
            EditControlUtility.SetText(cbAgeRating, comic.AgeRating);
            EditControlUtility.SetText(cbPublisher, comic.Publisher);
            EditControlUtility.SetText(cbImprint, comic.Imprint);
            EditControlUtility.SetText(cbBookAge, comic.BookAge);
            EditControlUtility.SetText(cbBookStore, comic.BookStore);
            EditControlUtility.SetText(cbBookOwner, comic.BookOwner);
            EditControlUtility.SetText(cbBookCondition, comic.BookCondition);
            EditControlUtility.SetText(cbBookPrice, comic.BookPriceAsText);
            EditControlUtility.SetText(cbBookLocation, comic.BookLocation);
            EditControlUtility.SetText(txCollectionStatus, comic.BookCollectionStatus);
            EditControlUtility.SetText(txBookNotes, StringUtility.MakeEditBoxMultiline(comic.BookNotes));
            EditControlUtility.SetText(txISBN, comic.ISBN);
            EditControlUtility.SetText(txPagesAsTextSimple, comic.PagesAsTextSimple);
            dtpAddedTime.Value = comic.AddedTime;
            dtpReleasedTime.Value = comic.ReleasedTime;
            dtpOpenedTime.Value = comic.OpenedTime;
            cbLanguage.TopISOLanguages = Program.Lists.GetComicFieldList((ComicBook cb) => cb.LanguageISO).Cast<string>().Distinct();
            cbLanguage.SelectedCulture = comic.LanguageISO;
            customValuesData.Rows.Clear();
            foreach (string item in Program.Database.CustomValues.OrderBy((string s) => s))
            {
                int index = customValuesData.Rows.Add(item, comic.GetCustomValue(item) ?? string.Empty);
                customValuesData.Rows[index].Visible = Program.ExtendedSettings.ShowCustomScriptValues || !item.Contains('.');
            }
            EditControlUtility.SetText(txSummary, StringUtility.MakeEditBoxMultiline(comic.Summary));
            EditControlUtility.SetText(txNotes, StringUtility.MakeEditBoxMultiline(comic.Notes));
            EditControlUtility.SetText(txReview, StringUtility.MakeEditBoxMultiline(comic.Review));
            EditControlUtility.SetText(txCharacters, comic.Characters);
            EditControlUtility.SetText(txTeams, comic.Teams);
            EditControlUtility.SetText(txMainCharacterOrTeam, comic.MainCharacterOrTeam);
            EditControlUtility.SetText(txLocations, comic.Locations);
            EditControlUtility.SetText(txScanInformation, comic.ScanInformation);
            EditControlUtility.SetText(txWeblink, comic.Web);
            tbSaturation.Value = (int)(comic.ColorAdjustment.Saturation * 100f);
            tbBrightness.Value = (int)(comic.ColorAdjustment.Brightness * 100f);
            tbContrast.Value = (int)(comic.ColorAdjustment.Contrast * 100f);
            tbGamma.Value = (int)(comic.ColorAdjustment.Gamma * 100f);
            tbSharpening.Value = comic.ColorAdjustment.Sharpen;
            SetCurrentColorAdjustment(pageViewer, comic.ColorAdjustment.WhitePointColor);
            CustomThumbnailKey = comic.CustomThumbnailKey;
            SetPromptTexts(comic, comic.EnableProposed);
            FocusTextBox();
        }

        private void FocusTextBox()
        {
            if (currentTextBox != null && currentTextBox.Visible)
            {
                currentTextBox.Focus();
                (currentTextBox as TextBox)?.SelectAll();
                (currentTextBox as ComboBox)?.SelectAll();
            }
        }

        private void SetPromptTexts(ComicBook comic, bool enabled)
        {
            if (enabled)
            {
                txSeries.PromptText = comic.ProposedSeries;
                txTitle.PromptText = comic.ProposedTitle;
                txNumber.PromptText = comic.ProposedNumber;
                txCount.PromptText = comic.ProposedCountAsText;
                txYear.PromptText = comic.ProposedYearAsText;
                txVolume.PromptText = comic.ProposedNakedVolumeAsText;
                cbFormat.PromptText = comic.ProposedFormat;
            }
            else
            {
                TextBoxEx textBoxEx = txSeries;
                TextBoxEx textBoxEx2 = txTitle;
                TextBoxEx textBoxEx3 = txNumber;
                TextBoxEx textBoxEx4 = txCount;
                TextBoxEx textBoxEx5 = txYear;
                TextBoxEx textBoxEx6 = txVolume;
                string text = (cbFormat.PromptText = string.Empty);
                string text3 = (textBoxEx6.PromptText = text);
                string text5 = (textBoxEx5.PromptText = text3);
                string text7 = (textBoxEx4.PromptText = text5);
                string text9 = (textBoxEx3.PromptText = text7);
                string text12 = (textBoxEx.PromptText = (textBoxEx2.PromptText = text9));
            }
        }

        private ComicBook GetFromEditor()
        {
            ComicBook comicBook = new ComicBook(current);
            SaveBook(comicBook);
            return comicBook;
        }

        private void SaveBook(ComicBook comic)
        {
            comic.Series = EditControlUtility.GetText(txSeries, comic.Series);
            comic.Number = EditControlUtility.GetText(txNumber, comic.Number);
            comic.Title = EditControlUtility.GetText(txTitle, comic.Title);
            comic.AlternateSeries = EditControlUtility.GetText(txAlternateSeries, comic.AlternateSeries);
            comic.AlternateNumber = EditControlUtility.GetText(txAlternateNumber, comic.AlternateNumber);
            comic.StoryArc = EditControlUtility.GetText(txStoryArc, comic.StoryArc);
            comic.SeriesGroup = EditControlUtility.GetText(txSeriesGroup, comic.SeriesGroup);
            comic.Writer = EditControlUtility.GetText(txWriter, comic.Writer);
            comic.Summary = EditControlUtility.GetText(txSummary, StringUtility.MakeEditBoxMultiline(comic.Summary));
            comic.Penciller = EditControlUtility.GetText(txPenciller, comic.Penciller);
            comic.Inker = EditControlUtility.GetText(txInker, comic.Inker);
            comic.Letterer = EditControlUtility.GetText(txLetterer, comic.Letterer);
            comic.CoverArtist = EditControlUtility.GetText(txCoverArtist, comic.CoverArtist);
            comic.Editor = EditControlUtility.GetText(txEditor, comic.Editor);
            comic.Translator = EditControlUtility.GetText(txTranslator, comic.Translator);
			comic.Colorist = EditControlUtility.GetText(txColorist, comic.Colorist);
            comic.Genre = EditControlUtility.GetText(txGenre, comic.Genre);
            comic.Characters = EditControlUtility.GetText(txCharacters, comic.Characters);
            comic.Teams = EditControlUtility.GetText(txTeams, comic.Teams);
            comic.MainCharacterOrTeam = EditControlUtility.GetText(txMainCharacterOrTeam, comic.MainCharacterOrTeam);
            comic.Locations = EditControlUtility.GetText(txLocations, comic.Locations);
            comic.Notes = EditControlUtility.GetText(txNotes, StringUtility.MakeEditBoxMultiline(comic.Notes));
            comic.Review = EditControlUtility.GetText(txReview, StringUtility.MakeEditBoxMultiline(comic.Review));
            comic.ScanInformation = EditControlUtility.GetText(txScanInformation, comic.ScanInformation);
            comic.Web = EditControlUtility.GetText(txWeblink, comic.Web);
            comic.Tags = EditControlUtility.GetText(txTags, comic.Tags);
            comic.SeriesComplete = EditControlUtility.GetYesNo(cbSeriesComplete.Text);
            comic.Count = EditControlUtility.GetNumber(txCount);
            comic.Volume = EditControlUtility.GetNumber(txVolume);
            comic.Month = EditControlUtility.GetNumber(txMonth);
            comic.Day = EditControlUtility.GetNumber(txDay);
            comic.Year = EditControlUtility.GetNumber(txYear);
            comic.AlternateCount = EditControlUtility.GetNumber(txAlternateCount);
            comic.FilePath = displayComic.FilePath;
            comic.CustomThumbnailKey = CustomThumbnailKey;
            string text = cbPublisher.Text.Trim();
            if (comic.Publisher != text)
            {
                comic.Publisher = text;
                EditControlUtility.SetText(cbPublisher, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Publisher), onlyDirectList: true);
            }
            string text2 = cbImprint.Text.Trim();
            if (comic.Imprint != text2)
            {
                comic.Imprint = text2;
                EditControlUtility.SetText(cbImprint, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Imprint), onlyDirectList: true);
            }
            string text3 = cbFormat.Text.Trim();
            if (comic.Format != text3)
            {
                comic.Format = text3;
                EditControlUtility.SetText(cbFormat, null, () => Program.Lists.GetFormatList(), onlyDirectList: true);
            }
            string text4 = cbAgeRating.Text.Trim();
            if (comic.AgeRating != text4)
            {
                comic.AgeRating = text4;
                EditControlUtility.SetText(cbAgeRating, null, () => Program.Lists.GetAgeRatingList(), onlyDirectList: true);
            }
            string text5 = cbBookAge.Text.Trim();
            if (comic.BookAge != text5)
            {
                comic.BookAge = text5;
                EditControlUtility.SetText(cbBookAge, null, () => Program.Lists.GetBookAgeList(), onlyDirectList: true);
            }
            string text6 = cbBookStore.Text.Trim();
            if (comic.BookStore != text6)
            {
                comic.BookStore = text6;
                EditControlUtility.SetText(cbBookStore, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.BookStore), onlyDirectList: true);
            }
            string text7 = cbBookOwner.Text.Trim();
            if (comic.BookOwner != text7)
            {
                comic.BookOwner = text7;
                EditControlUtility.SetText(cbBookOwner, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.BookOwner), onlyDirectList: true);
            }
            string b = cbBookPrice.Text.Trim();
            if (comic.BookPriceAsText != b)
            {
                comic.BookPrice = EditControlUtility.GetRealNumber(cbBookPrice);
                EditControlUtility.SetText(cbBookPrice, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.BookPriceAsText), onlyDirectList: true);
            }
            string text8 = cbBookCondition.Text.Trim();
            if (comic.BookCondition != text8)
            {
                comic.BookCondition = text8;
                EditControlUtility.SetText(cbBookCondition, null, () => Program.Lists.GetBookConditionList(), onlyDirectList: true);
            }
            string text9 = cbBookLocation.Text.Trim();
            if (comic.BookLocation != text9)
            {
                comic.BookLocation = text9;
                EditControlUtility.SetText(cbBookLocation, null, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.BookLocation), onlyDirectList: true);
            }
            comic.BookNotes = txBookNotes.Text.Trim();
            comic.BookCollectionStatus = EditControlUtility.GetText(txCollectionStatus, comic.BookCollectionStatus);
            comic.ISBN = txISBN.Text.Trim();
            if (!comic.IsLinked)
            {
                comic.PagesAsTextSimple = txPagesAsTextSimple.Text;
                comic.OpenedTime = dtpOpenedTime.Value;
            }
            comic.AddedTime = dtpAddedTime.Value;
            comic.ReleasedTime = dtpReleasedTime.Value;
            comic.LanguageISO = cbLanguage.SelectedCulture;
            comic.Rating = txRating.Rating;
            comic.CommunityRating = txCommunityRating.Rating;
            comic.ColorAdjustment = pageViewer.ColorAdjustment;
            comic.Manga = EditControlUtility.GetMangaYesNo(cbManga.Text);
            comic.BlackAndWhite = EditControlUtility.GetYesNo(cbBlackAndWhite.Text);
            comic.EnableProposed = EditControlUtility.GetYesNo(cbEnableProposed.Text) == YesNo.Yes;
            comic.EnableDynamicUpdate = EditControlUtility.GetYesNo(cbEnableDynamicUpdate.Text) == YesNo.Yes;
            if (pagesView.Book != null && pagesView.Book.IsIndexRetrievalCompleted && pagesView.Book.Comic.Pages.Count > 0)
            {
                comic.SetPages(pagesView.Book.Comic.Pages);
                comic.PageCount = pagesView.Book.Count;
                comic.LastPageRead = Math.Min(comic.PageCount - 1, comic.LastPageRead);
            }
            comic.CustomValuesStore = string.Empty;
            foreach (DataGridViewRow item in (IEnumerable)customValuesData.Rows)
            {
                string text10 = (string)item.Cells[0].Value;
                string value = (string)item.Cells[1].Value;
                if (!string.IsNullOrEmpty(text10.SafeTrim()) && !string.IsNullOrEmpty(value))
                {
                    comic.SetCustomValue(text10, value);
                }
            }
        }

        private void SaveBook()
        {
            SaveBook(current);
        }

        private void PagesViewItemActivate(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabColors;
        }

        private void PagesViewSelectedIndexChanged(object sender, EventArgs e)
        {
            if (current != null)
            {
                IEnumerable<ComicPageInfo> selectedPages = pagesView.GetSelectedPages();
                int pageView = ((!selectedPages.IsEmpty()) ? current.TranslateImageIndexToPage(selectedPages.First().ImageIndex) : current.FrontCoverPageIndex);
                SetPageView(pageView);
            }
        }

        private void pageViewer_VisibleChanged(object sender, EventArgs e)
        {
            if (pageViewer.Visible && pendingPageViewerPage != -1)
            {
                SetImage(pageViewer, displayComic, pendingPageViewerPage);
                pendingPageViewerPage = -1;
            }
        }

        private void SetPageView(int page)
        {
            pageViewPage = page;
            labelCurrentPage.Text = TR.Default["Page"] + " " + (page + 1);
            if (pageViewer.Visible)
            {
                SetImage(pageViewer, displayComic, page);
            }
            else
            {
                pendingPageViewerPage = page;
            }
        }

        private static void SetCoverThumbnailImage(IBitmapDisplayControl iv, ComicBook cb)
        {
            SetThumbnailImage(iv, cb, cb.FrontCoverPageIndex);
        }

        private static void SetThumbnailImage(IBitmapDisplayControl iv, ComicBook cb, int page)
        {
            try
            {
                ThumbnailKey key = cb.GetThumbnailKey(page);
                iv.Tag = key;
                using (IItemLock<ThumbnailImage> itemLock = Program.ImagePool.GetThumbnail(key, onlyMemory: true))
                {
                    if (itemLock != null)
                    {
                        iv.SetBitmap(itemLock.Item.Bitmap.CreateCopy());
                        return;
                    }
                    Program.ImagePool.SlowThumbnailQueue.AddItem(key, iv, delegate
                    {
                        try
                        {
                            using (IItemLock<ThumbnailImage> itemLock2 = Program.ImagePool.GetThumbnail(key, cb))
                            {
                                if (object.Equals(key, iv.Tag))
                                {
                                    iv.SetBitmap(itemLock2.Item.Bitmap.CreateCopy());
                                }
                            }
                        }
                        catch
                        {
                        }
                    });
                }
            }
            catch
            {
            }
        }


        private void SetImage(IBitmapDisplayControl iv, ComicBook cb, int page)
        {
            try
            {
                PageKey key = cb.GetPageKey(page, BitmapAdjustment.Empty);
                iv.Tag = key;
                using (IItemLock<PageImage> itemLock = Program.ImagePool.GetPage(key, onlyMemory: true))
                {
                    if (itemLock != null && itemLock.Item != null)
                    {
                        iv.SetBitmap(itemLock.Item.Bitmap.Scale(pageViewer.Width, 0));
                        return;
                    }
                    Program.ImagePool.AddPageToQueue(key, iv, delegate
                    {
                        try
                        {
                            using (IItemLock<PageImage> itemLock2 = Program.ImagePool.GetPage(key, cb))
                            {
                                if (object.Equals(key, iv.Tag))
                                {
                                    iv.SetBitmap(itemLock2.Item.Bitmap.Scale(pageViewer.Width, 0));
                                }
                            }
                        }
                        catch
                        {
                        }
                    }, bottom: false);
                }
            }
            catch
            {
            }
        }


        private void SetCurrentColorAdjustment(BitmapViewer iv, Color whitePoint)
        {
            iv.ColorAdjustment = new BitmapAdjustment((float)tbSaturation.Value / 100f, (float)tbBrightness.Value / 100f, (float)tbContrast.Value / 100f, (float)tbGamma.Value / 100f, whitePoint, BitmapAdjustmentOptions.None, tbSharpening.Value);
        }

        private void SetCurrentColorAdjustment(BitmapViewer iv)
        {
            SetCurrentColorAdjustment(iv, iv.ColorAdjustment.WhitePointColor);
        }

        private void customValuesData_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (customValuesData.CurrentCell.ColumnIndex != 1)
            {
                return;
            }
            TextBox textBox = e.Control as TextBox;
            if (textBox == null)
            {
                return;
            }
            AutoCompleteStringCollection autoCompleteStringCollection = new AutoCompleteStringCollection();
            string key = (string)customValuesData.Rows[customValuesData.CurrentRow.Index].Cells[0].Value;
            if (!string.IsNullOrEmpty(key))
            {
                autoCompleteStringCollection.AddRange((from p in Program.Database.GetBooks().SelectMany((ComicBook cb) => cb.GetCustomValues())
                                                       where p.Key.Equals(key, StringComparison.OrdinalIgnoreCase)
                                                       select p.Value).ToArray());
            }
            textBox.AutoCompleteCustomSource = autoCompleteStringCollection;
            textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void coverThumbnail_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                SelectPreviousBook();
            }
            else
            {
                SelectNextBook();
            }
        }

        private void txCharacters_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\n')
            {
                e.Handled = true;
            }
        }

        private void IdleProcess_Idle(object sender, EventArgs e)
        {
            Button button = btFirstPage;
            bool enabled = (btPrevPage.Enabled = displayComic != null && displayComic.PageCount > 0 && pageViewPage > 0);
            button.Enabled = enabled;
            Button button2 = btLastPage;
            enabled = (btNextPage.Enabled = displayComic != null && displayComic.PageCount > 0 && pageViewPage < displayComic.PageCount - 1);
            button2.Enabled = enabled;
        }

        private void chkShowColorControls_CheckedChanged(object sender, EventArgs e)
        {
            panelImageControls.Visible = chkShowImageControls.Checked;
        }

        private void btApply_Click(object sender, EventArgs e)
        {
            SaveBook();
            SetCurrentBook(current);
        }

        private void btPrev_Click(object sender, EventArgs e)
        {
            btPrev.Focus();
            SelectPreviousBook();
            FocusTextBox();
        }

        private void btNext_Click(object sender, EventArgs e)
        {
            btNext.Focus();
            SelectNextBook();
            FocusTextBox();
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            SaveBook();
        }

        private void ColorAdjustment_Scroll(object sender, EventArgs e)
        {
            SetCurrentColorAdjustment(pageViewer);
        }

        private void btReset_Click(object sender, EventArgs e)
        {
            TrackBarLite trackBarLite = tbSaturation;
            TrackBarLite trackBarLite2 = tbBrightness;
            TrackBarLite trackBarLite3 = tbContrast;
            int num2 = (tbGamma.Value = 0);
            int num4 = (trackBarLite3.Value = num2);
            int num7 = (trackBarLite.Value = (trackBarLite2.Value = num4));
            tbSharpening.Value = 0;
            SetCurrentColorAdjustment(pageViewer, Color.White);
        }

        private void pageViewer_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Color pixel = pageViewer.GetPixel(pageViewer.PointToClient(Cursor.Position));
                SetCurrentColorAdjustment(pageViewer, pixel);
            }
            catch
            {
            }
        }

        private void btResetPages_Click(object sender, EventArgs e)
        {
            if (pagesView.Book != null)
            {
                pagesView.Book.Comic.ResetPageSequence();
                pagesView.UpdateList(now: true);
            }
        }

        private void btResetPages_ShowContextMenu(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = miOrderByName;
            bool enabled = (miOrderByNameNumeric.Enabled = pagesView.Book != null && pagesView.Book.IsIndexRetrievalCompleted);
            toolStripMenuItem.Enabled = enabled;
        }

        private void cmResetPages_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (pagesView.Book == null)
            {
                return;
            }
            Comparison<ComicPageInfo> comparison = null;
            Dictionary<int, PageViewItem> map = pagesView.GetItems().ToDictionary((PageViewItem item) => item.PageInfo.ImageIndex);
            if (e.ClickedItem == miOrderByName)
            {
                comparison = (ComicPageInfo a, ComicPageInfo b) => string.CompareOrdinal(map[a.ImageIndex].Key, map[b.ImageIndex].Key);
            }
            else if (e.ClickedItem == miOrderByNameNumeric)
            {
                comparison = (ComicPageInfo a, ComicPageInfo b) => ExtendedStringComparer.Compare(map[a.ImageIndex].Key, map[b.ImageIndex].Key);
            }
            if (comparison != null)
            {
                try
                {
                    pagesView.Book.Comic.SortPages(comparison);
                }
                catch
                {
                    pagesView.Book.Comic.ResetPageSequence();
                }
                pagesView.UpdateList(now: true);
            }
        }

        private void cbEnableShadowValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPromptTexts(current, EditControlUtility.GetYesNo(cbEnableProposed.Text) == YesNo.Yes);
        }

        private void btPageViews_Click(object sender, EventArgs e)
        {
            pagesView.ItemView.GetViewMenu().Show(btPageView, new Point(btPageView.Width, btPageView.Height), ToolStripDropDownDirection.BelowLeft);
        }

        private void AdjustmentSliderChanged(object sender, EventArgs e)
        {
            TrackBarLite trackBarLite = (TrackBarLite)sender;
            toolTip.SetToolTip(trackBarLite, string.Format("{1}{0}%", trackBarLite.Value, (trackBarLite.Value > 0) ? "+" : string.Empty));
        }

        private void coverThumbnail_Click(object sender, EventArgs e)
        {
            coverThumbnail.ThreeD = !coverThumbnail.ThreeD;
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.StartDocument(linkLabel.Text);
        }

        private void btLinkFile_Click(object sender, EventArgs e)
        {
            string text = Program.ShowComicOpenDialog(this, btLinkFile.Text.Replace("&", string.Empty), includeReadingLists: false);
            if (text != null)
            {
                LinkFile(text);
            }
        }

        private void LinkFile(string file)
        {
            if (current.IsLinked)
            {
                return;
            }
            if (Program.Database.Books[file] != null)
            {
                MessageBox.Show(this, TR.Messages["ErrorFileUsed", "This file is already used in the Library"], TR.Messages["Attention", "Attention"], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ComicBook comicBook = Program.BookFactory.Create(file, CreateBookOption.DoNotAdd, RefreshInfoOptions.ForceRefresh);
            if (comicBook != null)
            {
                comicBook.SetInfo(current.GetInfo());
                SetComicToEditor(comicBook);
            }
        }

        private void ComicBookDialog_DragDrop(object sender, DragEventArgs e)
        {
            string[] array = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (array != null && array.Length != 0)
            {
                LinkFile(array[0]);
            }
        }

        private void ComicBookDialog_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = (e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None);
        }

        private void coverThumbnail_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                Bitmap bitmap = e.Data.GetData(DataFormats.Bitmap) as Bitmap;
                string[] array = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (bitmap != null)
                {
                    CustomThumbnailKey = Program.ImagePool.AddCustomThumbnail(bitmap);
                }
                else if (array != null)
                {
                    LoadThumbnail(array[0]);
                }
            }
            catch (Exception)
            {
            }
        }

        private void coverThumbnail_DragOver(object sender, DragEventArgs e)
        {
            bool flag = !displayComic.IsLinked && (e.Data.GetDataPresent(DataFormats.FileDrop) || e.Data.GetDataPresent(DataFormats.Bitmap));
            e.Effect = (flag ? DragDropEffects.Copy : DragDropEffects.None);
        }

        private void btThumbnail_Click(object sender, EventArgs e)
        {
            string value = Program.LoadCustomThumbnail(null, this, btThumbnail.Text.Replace("&", string.Empty));
            if (!string.IsNullOrEmpty(value))
            {
                CustomThumbnailKey = value;
            }
        }

        private void LoadThumbnail(string file)
        {
            string value = Program.LoadCustomThumbnail(file);
            if (!string.IsNullOrEmpty(value))
            {
                CustomThumbnailKey = value;
            }
        }

        private void btThumbnail_ShowContextMenu(object sender, EventArgs e)
        {
            FormUtility.SafeToolStripClear(cmThumbnail.Items, 2);
            foreach (string thumbnailFile in Program.Settings.ThumbnailFiles)
            {
                string file = thumbnailFile;
                cmThumbnail.Items.Add(thumbnailFile, null, delegate
                {
                    LoadThumbnail(file);
                });
            }
            cmThumbnail.Items[1].Visible = cmThumbnail.Items.Count > 2;
        }

        private void miResetThumbnail_Click(object sender, EventArgs e)
        {
            CustomThumbnailKey = null;
        }

        private void btFirstPage_Click(object sender, EventArgs e)
        {
            if (displayComic.PageCount > 0)
            {
                SetPageView(0);
            }
        }

        private void btPrevPage_Click(object sender, EventArgs e)
        {
            int num = pageViewPage - 1;
            if (displayComic.PageCount >= 0 && num >= 0)
            {
                SetPageView(num);
            }
        }

        private void btNextPage_Click(object sender, EventArgs e)
        {
            int num = pageViewPage + 1;
            if (displayComic.PageCount >= 0 && num < displayComic.PageCount)
            {
                SetPageView(num);
            }
        }

        private void btLastPage_Click(object sender, EventArgs e)
        {
            if (displayComic.PageCount > 0)
            {
                SetPageView(current.PageCount - 1);
            }
        }

        private void IconTextsChanged(object sender, EventArgs e)
        {
            coverThumbnail.FormatIcon = cbFormat.Text;
            coverThumbnail.AgeRatingIcon = cbAgeRating.Text;
            coverThumbnail.PublisherIcon = cbPublisher.Text;
            coverThumbnail.ImprintIcon = cbImprint.Text;
            coverThumbnail.PublishedYear = txYear.Text;
            coverThumbnail.BlackAndWhiteIcon = EditControlUtility.GetYesNo(cbBlackAndWhite.Text);
            coverThumbnail.MangaIcon = EditControlUtility.GetMangaYesNo(cbManga.Text);
            coverThumbnail.SeriesCompleteIcon = EditControlUtility.GetYesNo(cbSeriesComplete.Text);
        }

        private void SelectPreviousBook()
        {
            int num = Array.IndexOf(allBooks, current) - 1;
            if (num >= 0)
            {
                SaveBook();
                SetCurrentBook(allBooks[num]);
            }
        }

        private void SelectNextBook()
        {
            int num = Array.IndexOf(allBooks, current) + 1;
            if (num < allBooks.Length)
            {
                SaveBook();
                SetCurrentBook(allBooks[num]);
            }
        }

        private void InitializeScriptButton()
        {
            if (scriptEngine == null)
            {
                return;
            }
            foreach (Command command in scriptEngine.GetCommands(PluginEngine.ScriptTypeEditor))
            {
                if (btScript.ContextMenuStrip == null)
                {
                    btScript.ContextMenuStrip = new ContextMenuStrip();
                }
                string localizedName = command.GetLocalizedName();
                ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(localizedName)
                {
                    Tag = command
                };
                toolStripMenuItem.Click += DropDownMenuItemClick;
                btScript.ContextMenuStrip.Items.Add(toolStripMenuItem);
            }
            if (btScript.ContextMenuStrip != null)
            {
                currentScript = currentScript.Clamp(0, btScript.ContextMenuStrip.Items.Count - 1);
                btScript.Visible = true;
                btScript.Text = btScript.ContextMenuStrip.Items[currentScript].Text;
                btScript.Tag = btScript.ContextMenuStrip.Items[currentScript].Tag;
                btScript.Click += ScriptButtonClick;
            }
        }

        private void DropDownMenuItemClick(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;
            currentScript = btScript.ContextMenuStrip.Items.IndexOf(toolStripMenuItem);
            ExecuteScript(toolStripMenuItem.Text, toolStripMenuItem.Tag as Command);
        }

        private void ScriptButtonClick(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            ExecuteScript(control.Text, control.Tag as Command);
        }

        private void ExecuteScript(string name, Command cmd)
        {
            try
            {
                btScript.Text = name;
                btScript.Tag = cmd;
                ComicBook fromEditor = GetFromEditor();
                using (new WaitCursor(this))
                {
                    cmd.Invoke(new object[1]
                    {
                        new ComicBook[1]
                        {
                            fromEditor
                        }
                    });
                }
                SetDataToEditor(fromEditor);
            }
            catch (Exception ex)
            {
                ScriptUtility.ShowError(this, ex);
            }
        }

        public static bool Show(IWin32Window parent, ComicBook comicBook, ComicBook[] books, Func<ComicBook, bool> selHandler)
        {
            using (ComicBookDialog comicBookDialog = new ComicBookDialog(comicBook, books))
            {
                comicBookDialog.selectComicHandler = selHandler;
                if (lastActivePage != -1)
                {
                    comicBookDialog.tabControl.SelectedIndex = lastActivePage;
                }
                if (lastActiveTextPage != -1)
                {
                    comicBookDialog.tabNotes.SelectedIndex = lastActiveTextPage;
                }
                comicBookDialog.chkShowImageControls.Checked = showColorControls;
                bool result = comicBookDialog.ShowDialog(parent) == DialogResult.OK;
                lastActivePage = comicBookDialog.tabControl.SelectedIndex;
                lastActiveTextPage = comicBookDialog.tabNotes.SelectedIndex;
                showColorControls = comicBookDialog.chkShowImageControls.Checked;
                return result;
            }
        }

        public static bool Show(IWin32Window parent, ComicBook comicBook, ComicBook[] books)
        {
            return Show(parent, comicBook, books, null);
        }

        public static bool Show(IWin32Window parent, ComicBook comicBook)
        {
            return Show(parent, comicBook, null);
        }
    }
}
