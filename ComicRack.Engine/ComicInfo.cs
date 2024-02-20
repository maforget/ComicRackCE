using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using cYo.Common.Collections;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Reflection;
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Common.Xml;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	public class ComicInfo
	{
		private static volatile string[] coverKeyFilter = new string[0];

		private string title = string.Empty;

		private string series = string.Empty;

		private string number = string.Empty;

		private int count = -1;

		private int volume = -1;

		private string alternateSeries = string.Empty;

		private string alternateNumber = string.Empty;

		private string storyArc = string.Empty;

		private string seriesGroup = string.Empty;

		private int alternateCount = -1;

		private string summary = string.Empty;

		private string notes = string.Empty;

		private string review = string.Empty;

		private int year = -1;

		private int month = -1;

		private int day = -1;

		private string writer = string.Empty;

		private string penciller = string.Empty;

		private string inker = string.Empty;

		private string colorist = string.Empty;

		private string letterer = string.Empty;

		private string coverArtist = string.Empty;

		private string editor = string.Empty;

		private string publisher = string.Empty;

		private string imprint = string.Empty;

		private string genre = string.Empty;

		private string web = string.Empty;

		private int pageCount;

		private string languageISO = string.Empty;

		private string format = string.Empty;

		private string ageRating = string.Empty;

		private YesNo blackAndWhite = YesNo.Unknown;

		private MangaYesNo manga = MangaYesNo.Unknown;

		private int preferredFrontCover;

		private string characters = string.Empty;

		private string teams = string.Empty;

		private string mainCharacterOrTeam = string.Empty;

		private string locations = string.Empty;

		private float communityRating;

		private string scanInformation = string.Empty;

		private string tags = string.Empty;

		private volatile int cachedFrontCoverPageIndex = -1;

		private volatile int cachedFrontCoverCount = -1;

		private ComicPageInfoCollection pages;

		private int cachedBookmarkCount = -1;

		private static readonly Lazy<string> yesText = new Lazy<string>(() => TR.Default["Yes"]);

		private static readonly Lazy<string> noText = new Lazy<string>(() => TR.Default["No"]);

		private static readonly Lazy<string> yesRightToLeftText = new Lazy<string>(() => TR.Load("ComicInfo")["YesRightToLeft", "Yes (Right to Left)"]);

		private static readonly Regex rxVolume = new Regex("\\bv(ol(ume)?)?\\.?\\s?\\d+\\b\\s*", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Regex rxSpecial = new Regex("[^a-z0-9]|\\bthe\\b|\\band\\b|", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public static string CoverKeyFilter
		{
			get
			{
				return coverKeyFilter.ToListString(";");
			}
			set
			{
				coverKeyFilter = ((value == null) ? new string[0] : (from s in value.Split(';')
					select s.Trim()).RemoveEmpty().ToArray());
			}
		}

		[Browsable(true)]
		[Searchable]
		[DefaultValue("")]
		[ResetValue(0)]
		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				SetProperty("Title", ref title, value);
			}
		}

		[Browsable(true)]
		[Searchable]
		[DefaultValue("")]
		[ResetValue(0)]
		public string Series
		{
			get
			{
				return series;
			}
			set
			{
				SetProperty("Series", ref series, value);
			}
		}

		[Browsable(true)]
		[DefaultValue("")]
		[ResetValue(0)]
		public string Number
		{
			get
			{
				return number;
			}
			set
			{
				SetProperty("Number", ref number, value);
			}
		}

		[Browsable(true)]
		[DefaultValue(-1)]
		[ResetValue(0)]
		public int Count
		{
			get
			{
				return count;
			}
			set
			{
				SetProperty("Count", ref count, value);
			}
		}

		[Browsable(true)]
		[DefaultValue(-1)]
		[ResetValue(0)]
		public int Volume
		{
			get
			{
				return volume;
			}
			set
			{
				SetProperty("Volume", ref volume, value);
			}
		}

		[Browsable(true)]
		[Searchable]
		[DefaultValue("")]
		[ResetValue(0)]
		public string AlternateSeries
		{
			get
			{
				return alternateSeries;
			}
			set
			{
				SetProperty("AlternateSeries", ref alternateSeries, value);
			}
		}

		[Browsable(true)]
		[DefaultValue("")]
		[ResetValue(0)]
		public string AlternateNumber
		{
			get
			{
				return alternateNumber;
			}
			set
			{
				SetProperty("AlternateNumber", ref alternateNumber, value);
			}
		}

		[Browsable(true)]
		[Searchable]
		[DefaultValue("")]
		[ResetValue(0)]
		public string StoryArc
		{
			get
			{
				return storyArc;
			}
			set
			{
				SetProperty("StoryArc", ref storyArc, value);
			}
		}

		[Browsable(true)]
		[Searchable]
		[DefaultValue("")]
		[ResetValue(0)]
		public string SeriesGroup
		{
			get
			{
				return seriesGroup;
			}
			set
			{
				SetProperty("SeriesGroup", ref seriesGroup, value);
			}
		}

		[Browsable(true)]
		[DefaultValue(-1)]
		[ResetValue(0)]
		public int AlternateCount
		{
			get
			{
				return alternateCount;
			}
			set
			{
				SetProperty("AlternateCount", ref alternateCount, value);
			}
		}

		[Browsable(true)]
		[DefaultValue("")]
		[ResetValue(0)]
		public string Summary
		{
			get
			{
				return summary;
			}
			set
			{
				SetProperty("Summary", ref summary, value);
			}
		}

		[Browsable(true)]
		[DefaultValue("")]
		[ResetValue(0)]
		public string Notes
		{
			get
			{
				return notes;
			}
			set
			{
				SetProperty("Notes", ref notes, value);
			}
		}

		[Browsable(true)]
		[DefaultValue("")]
		[ResetValue(0)]
		public string Review
		{
			get
			{
				return review;
			}
			set
			{
				SetProperty("Review", ref review, value);
			}
		}

		[Browsable(true)]
		[DefaultValue(-1)]
		[ResetValue(0)]
		public int Year
		{
			get
			{
				return year;
			}
			set
			{
				SetProperty("Year", ref year, value);
			}
		}

		[Browsable(true)]
		[DefaultValue(-1)]
		[ResetValue(0)]
		public int Month
		{
			get
			{
				return month;
			}
			set
			{
				SetProperty("Month", ref month, value);
			}
		}

		[Browsable(true)]
		[DefaultValue(-1)]
		[ResetValue(0)]
		public int Day
		{
			get
			{
				return day;
			}
			set
			{
				SetProperty("Day", ref day, value);
			}
		}

		[Browsable(true)]
		[Searchable]
		[DefaultValue("")]
		[ResetValue(0)]
		public string Writer
		{
			get
			{
				return writer;
			}
			set
			{
				SetProperty("Writer", ref writer, value);
			}
		}

		[Browsable(true)]
		[Searchable]
		[DefaultValue("")]
		[ResetValue(0)]
		public string Penciller
		{
			get
			{
				return penciller;
			}
			set
			{
				SetProperty("Penciller", ref penciller, value);
			}
		}

		[Browsable(true)]
		[Searchable]
		[DefaultValue("")]
		[ResetValue(0)]
		public string Inker
		{
			get
			{
				return inker;
			}
			set
			{
				SetProperty("Inker", ref inker, value);
			}
		}

		[Browsable(true)]
		[Searchable]
		[DefaultValue("")]
		[ResetValue(0)]
		public string Colorist
		{
			get
			{
				return colorist;
			}
			set
			{
				SetProperty("Colorist", ref colorist, value);
			}
		}

		[Browsable(true)]
		[Searchable]
		[DefaultValue("")]
		[ResetValue(0)]
		public string Letterer
		{
			get
			{
				return letterer;
			}
			set
			{
				SetProperty("Letterer", ref letterer, value);
			}
		}

		[Browsable(true)]
		[Searchable]
		[DefaultValue("")]
		[ResetValue(0)]
		public string CoverArtist
		{
			get
			{
				return coverArtist;
			}
			set
			{
				SetProperty("CoverArtist", ref coverArtist, value);
			}
		}

		[Browsable(true)]
		[Searchable]
		[DefaultValue("")]
		[ResetValue(0)]
		public string Editor
		{
			get
			{
				return editor;
			}
			set
			{
				SetProperty("Editor", ref editor, value);
			}
		}

		[Browsable(true)]
		[Searchable]
		[DefaultValue("")]
		[ResetValue(0)]
		public string Publisher
		{
			get
			{
				return publisher;
			}
			set
			{
				SetProperty("Publisher", ref publisher, value);
			}
		}

		[Browsable(true)]
		[Searchable]
		[DefaultValue("")]
		[ResetValue(0)]
		public string Imprint
		{
			get
			{
				return imprint;
			}
			set
			{
				SetProperty("Imprint", ref imprint, value);
			}
		}

		[Browsable(true)]
		[Searchable]
		[DefaultValue("")]
		[ResetValue(0)]
		public string Genre
		{
			get
			{
				return genre;
			}
			set
			{
				SetProperty("Genre", ref genre, value);
			}
		}

		[Browsable(true)]
		[DefaultValue("")]
		[ResetValue(0)]
		public string Web
		{
			get
			{
				return web;
			}
			set
			{
				SetProperty("Web", ref web, value);
			}
		}

		[Browsable(true)]
		[DefaultValue(0)]
		[ResetValue(0)]
		public int PageCount
		{
			get
			{
				return pageCount;
			}
			set
			{
				SetProperty("PageCount", ref pageCount, value);
			}
		}

		[DefaultValue("")]
		[ResetValue(0)]
		public string LanguageISO
		{
			get
			{
				return languageISO;
			}
			set
			{
				SetProperty("LanguageISO", ref languageISO, value);
			}
		}

		[Browsable(true)]
		[Searchable]
		[DefaultValue("")]
		[ResetValue(0)]
		public string Format
		{
			get
			{
				return format;
			}
			set
			{
				SetProperty("Format", ref format, value);
			}
		}

		[Browsable(true)]
		[DefaultValue("")]
		[ResetValue(0)]
		public string AgeRating
		{
			get
			{
				return ageRating;
			}
			set
			{
				SetProperty("AgeRating", ref ageRating, value);
			}
		}

		[Browsable(true)]
		[DefaultValue(YesNo.Unknown)]
		[ResetValue(0)]
		public YesNo BlackAndWhite
		{
			get
			{
				return blackAndWhite;
			}
			set
			{
				SetProperty("BlackAndWhite", ref blackAndWhite, value);
			}
		}

		[Browsable(true)]
		[DefaultValue(MangaYesNo.Unknown)]
		[ResetValue(0)]
		public MangaYesNo Manga
		{
			get
			{
				return manga;
			}
			set
			{
				SetProperty("Manga", ref manga, value);
			}
		}

		[Browsable(true)]
		[DefaultValue(0)]
		[ResetValue(1)]
		public int PreferredFrontCover
		{
			get
			{
				return preferredFrontCover;
			}
			set
			{
				if (SetProperty("PreferredFrontCover", ref preferredFrontCover, value))
				{
					cachedFrontCoverPageIndex = -1;
				}
			}
		}

		[Browsable(true)]
		[Searchable]
		[DefaultValue("")]
		[ResetValue(0)]
		public string Characters
		{
			get
			{
				return characters;
			}
			set
			{
				SetProperty("Characters", ref characters, value);
			}
		}

		[Browsable(true)]
		[Searchable]
		[DefaultValue("")]
		[ResetValue(0)]
		public string Teams
		{
			get
			{
				return teams;
			}
			set
			{
				SetProperty("Teams", ref teams, value);
			}
		}

		[Browsable(true)]
		[Searchable]
		[DefaultValue("")]
		[ResetValue(0)]
		public string MainCharacterOrTeam
		{
			get
			{
				return mainCharacterOrTeam;
			}
			set
			{
				SetProperty("MainCharacterOrTeam", ref mainCharacterOrTeam, value);
			}
		}

		[Browsable(true)]
		[Searchable]
		[DefaultValue("")]
		[ResetValue(0)]
		public string Locations
		{
			get
			{
				return locations;
			}
			set
			{
				SetProperty("Locations", ref locations, value);
			}
		}

		[Browsable(true)]
		[DefaultValue(0)]
		[ResetValue(0)]
		public float CommunityRating
		{
			get
			{
				return communityRating;
			}
			set
			{
				SetProperty("CommunityRating", ref communityRating, value.Clamp(0f, 5f));
			}
		}

		[Browsable(true)]
		[Searchable]
		[DefaultValue("")]
		[ResetValue(0)]
		public string ScanInformation
		{
			get
			{
				return scanInformation;
			}
			set
			{
				SetProperty("ScanInformation", ref scanInformation, value);
			}
		}

        [Browsable(true)]
        [Searchable]
        [DefaultValue("")]
        [ResetValue(0)]
        public string Tags
        {
            get
            {
                return tags;
            }
            set
            {
                SetProperty("Tags", ref tags, value);
            }
        }

        public int FrontCoverPageIndex
		{
			get
			{
				if (cachedFrontCoverPageIndex != -1)
				{
					return cachedFrontCoverPageIndex;
				}
				ComicPageInfo[] source = GetPageList().ToArray();
				ComicPageInfo[] array = source.Where((ComicPageInfo pi) => pi.PageType == ComicPageType.FrontCover).ToArray();
				int num = PreferredFrontCover.Clamp(0, array.Length - 1);
				ComicPageInfo comicPageInfo = ((num == -1 || array.Length == 0) ? ComicPageInfo.Empty : array[num]);
				if (comicPageInfo.IsEmpty)
				{
					comicPageInfo = source.Where((ComicPageInfo p) => p.PageType != ComicPageType.Other).FirstOrDefault();
				}
				cachedFrontCoverPageIndex = ((!comicPageInfo.IsEmpty) ? TranslateImageIndexToPage(comicPageInfo.ImageIndex) : 0);
				return cachedFrontCoverPageIndex;
			}
		}

		public int FrontCoverCount
		{
			get
			{
				int num = cachedFrontCoverCount;
				if (num == -1)
				{
					num = (from cpi in GetPageList()
						where cpi.PageType == ComicPageType.FrontCover
						select cpi).Count();
				}
				cachedFrontCoverCount = num;
				return num;
			}
		}

		public int FirstNonCoverPageIndex => FrontCoverPageIndex + 1;

		[XmlArrayItem("Page")]
		[Browsable(false)]
		public ComicPageInfoCollection Pages => pages ?? (pages = new ComicPageInfoCollection());

		[XmlIgnore]
		public int BookmarkCount
		{
			get
			{
				if (cachedBookmarkCount == -1)
				{
					cachedBookmarkCount = ((pages != null) ? pages.Lock().Count((ComicPageInfo pi) => !string.IsNullOrEmpty(pi.Bookmark)) : 0);
				}
				return cachedBookmarkCount;
			}
		}

		public IEnumerable<string> Bookmarks
		{
			get
			{
				if (pages != null)
				{
					return from pi in pages.Lock()
						where !string.IsNullOrEmpty(pi.Bookmark)
						select pi.Bookmark;
				}
				return Enumerable.Empty<string>();
			}
		}

		public static string YesText => yesText.Value;

		public static string NoText => noText.Value;

		public static string YesRightToLeftText => yesRightToLeftText.Value;

		[field: NonSerialized]
		public event EventHandler<BookChangedEventArgs> BookChanged;

		public ComicInfo()
		{
		}

		public ComicInfo(ComicInfo ci)
			: this()
		{
			SetInfo(ci, onlyUpdateEmpty: false);
		}

		public void UpdatePageType(int page, ComicPageType value)
		{
			ComicPageInfo page2 = GetPage(page, add: true);
			if (page2.PageType != value)
			{
				page2.PageType = value;
				Pages[page] = page2;
				FirePageChanged(page);
			}
		}

		public void UpdatePageType(ComicPageInfo cpi, ComicPageType value)
		{
			int num = Pages.IndexOf(cpi);
			if (num >= 0)
			{
				UpdatePageType(num, value);
			}
		}

		public void UpdatePageRotation(int page, ImageRotation value)
		{
			ComicPageInfo page2 = GetPage(page, add: true);
			if (page2.Rotation != value)
			{
				page2.Rotation = value;
				Pages[page] = page2;
				FirePageChanged(page);
			}
		}

		public void UpdatePageRotation(ComicPageInfo cpi, ImageRotation value)
		{
			int num = Pages.IndexOf(cpi);
			if (num >= 0)
			{
				UpdatePageRotation(num, value);
			}
		}

		public void UpdatePagePosition(int page, ComicPagePosition value)
		{
			ComicPageInfo page2 = GetPage(page, add: true);
			if (page2.PagePosition != value)
			{
				page2.PagePosition = value;
				Pages[page] = page2;
				FirePageChanged(page);
			}
		}

		public void UpdateBookmark(int page, string bookmark)
		{
			ComicPageInfo page2 = GetPage(page, add: true);
			if ((!string.IsNullOrEmpty(page2.Bookmark) || !string.IsNullOrEmpty(bookmark)) && !(page2.Bookmark == bookmark))
			{
				page2.Bookmark = bookmark;
				Pages[page] = page2;
				cachedBookmarkCount = -1;
				FirePageChanged(page);
			}
		}

		public void SetPages(IEnumerable<ComicPageInfo> comicPages)
		{
			using (ItemMonitor.Lock(Pages))
			{
				if (Pages.SequenceEqual(comicPages))
				{
					return;
				}
				Pages.Clear();
				Pages.AddRange(comicPages);
				cachedBookmarkCount = -1;
			}
			FirePageChanged(-1);
		}

		public void UpdatePageSize(int page, int width, int height)
		{
			ComicPageInfo page2 = GetPage(page, add: true);
			if (page2.ImageHeight != height || page2.ImageWidth != width)
			{
				page2.ImageHeight = height;
				page2.ImageWidth = width;
				Pages[page] = page2;
				FirePageChanged(page, updateComicInfo: false);
			}
		}

		public void UpdatePageFileSize(int page, int size)
		{
			ComicPageInfo page2 = GetPage(page, add: true);
			if (page2.ImageFileSize != size)
			{
				page2.ImageFileSize = size;
				Pages[page] = page2;
				FirePageChanged(page, updateComicInfo: false);
			}
		}

		public int TranslatePageToImageIndex(int page)
		{
			if (page < 0)
			{
				return page;
			}
			return GetPage(page).ImageIndex;
		}

		public int TranslateImageIndexToPage(int imageIndex)
		{
			using (ItemMonitor.Lock(Pages))
			{
				int num = Pages.FindIndex((ComicPageInfo cpi) => cpi.ImageIndex == imageIndex);
				return (num == -1) ? imageIndex : num;
			}
		}

		public ComicPageInfo GetPage(int page, bool add = false)
		{
			bool updateComicInfo = false;
			using (ItemMonitor.Lock(Pages))
			{
				if (page < 0)
				{
					return ComicPageInfo.Empty;
				}
				if (page < Pages.Count)
				{
					return Pages[page];
				}
				if (!add)
				{
					return new ComicPageInfo(page);
				}
				while (Pages.Count <= page)
				{
					ComicPageInfo item = OnNewComicPageAdded(new ComicPageInfo(Pages.Count));
					if (!item.IsDefaultContent(-1))
					{
						updateComicInfo = true;
					}
					Pages.Add(item);
				}
			}
			FirePageChanged(-1, updateComicInfo);
			return GetPage(page);
		}

		public ComicPageInfo GetPageByImageIndex(int imageIndex)
		{
			ComicPageInfo result = Pages.FindByImageIndex(imageIndex);
			if (result.IsEmpty)
			{
				return GetPage(imageIndex);
			}
			return result;
		}

		public IEnumerable<ComicPageInfo> GetPageList()
		{
			return from n in Enumerable.Range(0, PageCount)
				select GetPage(n);
		}

		public void MovePages(int position, IEnumerable<ComicPageInfo> pages)
		{
			using (ItemMonitor.Lock(Pages))
			{
				foreach (ComicPageInfo page in Pages)
				{
					if (Pages.IndexOf(page) == -1)
					{
						for (int i = Count; i < page.ImageIndex - 1; i++)
						{
							Pages.Add(new ComicPageInfo(i));
						}
						Pages.Add(page);
					}
				}
				foreach (ComicPageInfo page2 in pages)
				{
					int num = Pages.IndexOf(page2);
					Pages.RemoveAt(num);
					if (num < position)
					{
						position--;
					}
					if (position == -1)
					{
						Pages.Add(page2);
						continue;
					}
					try
					{
						Pages.Insert(position++, page2);
					}
					catch
					{
						Pages.Add(page2);
					}
				}
			}
			FirePageChanged(-1);
		}

		public void ResetPageSequence()
		{
			if (pages != null)
			{
				pages.ResetPageSequence();
				FirePageChanged(-1);
			}
		}

		public void SortPages(Comparison<ComicPageInfo> comparison)
		{
			if (pages != null)
			{
				pages.Sort(comparison);
				FirePageChanged(-1);
			}
		}

		public void TrimExcessPageInfo()
		{
			if (pages == null)
			{
				return;
			}
			if (pages.Count == 0)
			{
				pages = null;
				return;
			}
			using (ItemMonitor.Lock(pages))
			{
				int num = pages.Count - 1;
				while (num >= 0 && (num >= PageCount || pages[num].IsDefaultContent(num)))
				{
					pages.RemoveAt(num);
					num--;
				}
				pages.TrimExcess();
			}
		}

		public void ClearBookmarks()
		{
			using (ItemMonitor.Lock(pages))
			{
				for (int i = 0; i < pages.Count; i++)
				{
					UpdateBookmark(i, string.Empty);
				}
			}
		}

		private bool SetProperty<T>(string name, ref T property, T value)
		{
			if (object.Equals(property, value))
			{
				return false;
			}
			T val = property;
			property = value;
			FireBookChanged(name, val, value);
			return true;
		}

		private void FireBookChanged(string name, object oldValue, object newValue)
		{
			OnBookChanged(new BookChangedEventArgs(name, isComicInfo: true, oldValue, newValue));
		}

		private void FirePageChanged(int page, bool updateComicInfo = true)
		{
			cachedFrontCoverPageIndex = (cachedFrontCoverCount = -1);
			OnBookChanged(new BookChangedEventArgs("Pages", page, updateComicInfo));
		}

		protected virtual void OnBookChanged(BookChangedEventArgs e)
		{
			if (this.BookChanged != null)
			{
				this.BookChanged(this, e);
			}
		}

		protected virtual ComicPageInfo OnNewComicPageAdded(ComicPageInfo info)
		{
			return info;
		}

		public void AppendArtistInfo(ComicInfo ci)
		{
			Penciller = Penciller.AppendWithSeparator(", ", ci.Penciller);
			Writer = Writer.AppendWithSeparator(", ", ci.Writer);
			Inker = Inker.AppendWithSeparator(", ", ci.Inker);
			Colorist = Colorist.AppendWithSeparator(", ", ci.Colorist);
			Letterer = Letterer.AppendWithSeparator(", ", ci.Letterer);
			CoverArtist = CoverArtist.AppendWithSeparator(", ", ci.CoverArtist);
		}

		public virtual void SetInfo(ComicInfo ci, bool onlyUpdateEmpty = true, bool updatePages = true)
		{
			if (ci == null)
			{
				return;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(Writer))
			{
				Writer = ci.Writer;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(Publisher))
			{
				Publisher = ci.Publisher;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(Imprint))
			{
				Imprint = ci.Imprint;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(Penciller))
			{
				Penciller = ci.Penciller;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(Inker))
			{
				Inker = ci.Inker;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(Title))
			{
				Title = ci.Title;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(Series))
			{
				Series = ci.Series;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(AlternateSeries))
			{
				AlternateSeries = ci.AlternateSeries;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(StoryArc))
			{
				StoryArc = ci.StoryArc;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(SeriesGroup))
			{
				SeriesGroup = ci.SeriesGroup;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(Summary))
			{
				Summary = ci.Summary;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(Notes))
			{
				Notes = ci.Notes;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(Review))
			{
				Review = ci.Review;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(Genre))
			{
				Genre = ci.Genre;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(Colorist))
			{
				Colorist = ci.Colorist;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(Editor))
			{
				Editor = ci.Editor;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(Letterer))
			{
				Letterer = ci.Letterer;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(CoverArtist))
			{
				CoverArtist = ci.CoverArtist;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(Web))
			{
				Web = ci.Web;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(LanguageISO))
			{
				LanguageISO = ci.LanguageISO;
			}
			if (!onlyUpdateEmpty || BlackAndWhite == YesNo.Unknown)
			{
				BlackAndWhite = ci.BlackAndWhite;
			}
			if (!onlyUpdateEmpty || Manga == MangaYesNo.Unknown)
			{
				Manga = ci.Manga;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(Format))
			{
				Format = ci.Format;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(AgeRating))
			{
				AgeRating = ci.AgeRating;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(Characters))
			{
				Characters = ci.Characters;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(Teams))
			{
				Teams = ci.Teams;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(MainCharacterOrTeam))
			{
				MainCharacterOrTeam = ci.MainCharacterOrTeam;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(Locations))
			{
				Locations = ci.Locations;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(ScanInformation))
			{
				ScanInformation = ci.ScanInformation;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(Number))
			{
				Number = ci.Number;
			}
            if (!onlyUpdateEmpty || string.IsNullOrEmpty(Tags))
            {
                Tags = ci.Tags;
            }
            if (!onlyUpdateEmpty || Count == -1)
			{
				Count = ci.Count;
			}
			if (!onlyUpdateEmpty || string.IsNullOrEmpty(AlternateNumber))
			{
				AlternateNumber = ci.AlternateNumber;
			}
			if (!onlyUpdateEmpty || AlternateCount == -1)
			{
				AlternateCount = ci.AlternateCount;
			}
			if (!onlyUpdateEmpty || Volume == -1)
			{
				Volume = ci.Volume;
			}
			if (!onlyUpdateEmpty || Year == -1)
			{
				Year = ci.Year;
			}
			if (!onlyUpdateEmpty || Month == -1)
			{
				Month = ci.Month;
			}
			if (!onlyUpdateEmpty || Day == -1)
			{
				Day = ci.Day;
			}
			if (!onlyUpdateEmpty || CommunityRating == 0f)
			{
				CommunityRating = ci.CommunityRating;
			}
			if (!updatePages || ci.PageCount == 0)
			{
				return;
			}
			if (onlyUpdateEmpty)
			{
				if (Pages.Count < ci.Pages.Count)
				{
					Pages.Clear();
				}
				if (PageCount < ci.PageCount)
				{
					PageCount = ci.PageCount;
				}
			}
			else
			{
				PageCount = ci.PageCount;
				Pages.Clear();
			}
			using (ItemMonitor.Lock(ci.Pages))
			{
				if (Pages.Count == 0)
				{
					ci.Pages.ForEach(delegate(ComicPageInfo cpi)
					{
						Pages.Add(cpi);
					});
					return;
				}
				for (int i = 0; i < Math.Min(ci.Pages.Count, Pages.Count); i++)
				{
					ComicPageInfo comicPageInfo = Pages[i];
					UpdatePageType(i, comicPageInfo.PageType);
					UpdateBookmark(i, comicPageInfo.Bookmark);
					UpdatePageRotation(i, comicPageInfo.Rotation);
				}
			}
		}

		public ComicInfo GetInfo()
		{
			using (ItemMonitor.Lock(this))
			{
				ComicInfo ci = new ComicInfo
				{
					Writer = Writer,
					Publisher = Publisher,
					Imprint = Imprint,
					Penciller = Penciller,
					Inker = Inker,
					Series = Series,
					Number = Number,
					Count = Count,
					AlternateSeries = AlternateSeries,
					AlternateNumber = AlternateNumber,
					AlternateCount = AlternateCount,
					SeriesGroup = SeriesGroup,
					StoryArc = StoryArc,
					Title = Title,
					Summary = Summary,
					Volume = Volume,
					Year = Year,
					Month = Month,
					Day = Day,
					Notes = Notes,
					Genre = Genre,
					Colorist = Colorist,
					Editor = Editor,
					Letterer = Letterer,
					CoverArtist = CoverArtist,
					Web = Web,
					PageCount = PageCount,
					LanguageISO = LanguageISO,
					BlackAndWhite = BlackAndWhite,
					Manga = Manga,
					Format = Format,
					AgeRating = AgeRating,
					Characters = Characters,
					Teams = Teams,
					Locations = Locations,
					ScanInformation = ScanInformation,
					Tags = Tags,
					MainCharacterOrTeam = MainCharacterOrTeam,
					Review = Review,
					CommunityRating = CommunityRating,
				};
				Pages.ForEach(delegate(ComicPageInfo cpi)
				{
					ci.Pages.Add(cpi);
				});
				return ci;
			}
		}

		public bool IsSameContent(ComicInfo ci, bool withPages = true)
		{
			if (ci != null && ci.Writer == Writer && ci.Publisher == Publisher && ci.Imprint == Imprint && ci.Inker == Inker && ci.Penciller == Penciller && ci.Title == Title && ci.Number == Number && ci.Count == Count && ci.Summary == Summary && ci.Series == Series && ci.Volume == Volume && ci.AlternateSeries == AlternateSeries && ci.AlternateNumber == AlternateNumber && ci.AlternateCount == AlternateCount && ci.StoryArc == StoryArc && ci.SeriesGroup == SeriesGroup && ci.Year == Year && ci.Month == Month && ci.Day == Day && ci.Notes == Notes && ci.Review == Review && ci.Genre == Genre && ci.Colorist == Colorist && ci.Editor == Editor && ci.Letterer == Letterer && ci.CoverArtist == CoverArtist && ci.Web == Web && ci.LanguageISO == LanguageISO && ci.PageCount == PageCount && ci.Format == Format && ci.AgeRating == AgeRating && ci.BlackAndWhite == BlackAndWhite && ci.Manga == Manga && ci.Characters == Characters && ci.Teams == Teams && ci.MainCharacterOrTeam == MainCharacterOrTeam && ci.Locations == Locations && ci.ScanInformation == ScanInformation && ci.Tags == Tags)
			{
				if (withPages)
				{
					return ci.Pages.PagesAreEqual(Pages);
				}
				return true;
			}
			return false;
		}

		public void Serialize(Stream outStream)
		{
			try
			{
				XmlUtility.GetSerializer<ComicInfo>().Serialize(outStream, this);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static ComicInfo Deserialize(Stream inStream)
		{
			try
			{
				return XmlUtility.GetSerializer<ComicInfo>().Deserialize(inStream) as ComicInfo;
			}
			catch
			{
				return null;
			}
		}

		public static ComicInfo LoadFromSidecar(string file)
		{
			try
			{
				using (FileStream inStream = File.OpenRead(file + ".xml"))
				{
					return Deserialize(inStream);
				}
			}
			catch (Exception)
			{
				return null;
			}
		}

		public byte[] ToArray()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Serialize(memoryStream);
				return memoryStream.ToArray();
			}
		}

		public string ToXml()
		{
			return Encoding.Default.GetString(ToArray());
		}

		public static string GetYesNoAsText(YesNo yn)
		{
			switch (yn)
			{
			case YesNo.Yes:
				return yesText.Value;
			case YesNo.No:
				return noText.Value;
			default:
				return string.Empty;
			}
		}

		public static string GetYesNoAsText(MangaYesNo yn)
		{
			if (yn == MangaYesNo.YesAndRightToLeft)
			{
				return yesRightToLeftText.Value;
			}
			return GetYesNoAsText((YesNo)yn);
		}

		public static string GetYesNoAsText(bool b)
		{
			return GetYesNoAsText(b ? YesNo.Yes : YesNo.No);
		}

		public static bool IsValidCoverKey(string fileKey)
		{
			string file = Path.GetFileName(fileKey);
			return !coverKeyFilter.Any((string f) => file.Contains(f, StringComparison.OrdinalIgnoreCase));
		}

		public static bool SeriesEquals(string a, string b, CompareSeriesOptions options)
		{
			if (options.HasFlag(CompareSeriesOptions.IgnoreVolumeInName))
			{
				a = rxVolume.Replace(a, string.Empty).Trim();
				b = rxVolume.Replace(b, string.Empty).Trim();
			}
			if (options.HasFlag(CompareSeriesOptions.StripDown))
			{
				a = rxSpecial.Replace(a, string.Empty);
				b = rxSpecial.Replace(b, string.Empty);
			}
			return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
		}
	}
}
