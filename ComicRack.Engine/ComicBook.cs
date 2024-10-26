using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using cYo.Common;
using cYo.Common.Collections;
using cYo.Common.Drawing;
using cYo.Common.IO;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Net.Search;
using cYo.Common.Reflection;
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Common.Xml;
using cYo.Projects.ComicRack.Engine.IO;
using cYo.Projects.ComicRack.Engine.IO.Provider;
using cYo.Projects.ComicRack.Engine.Sync;
using SharpCompress.Common;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[ComVisible(true)]
	public class ComicBook : ComicInfo, IImageKeyProvider, ICloneable
	{
		private class ComicTextNumberFloat : TextNumberFloat
		{
			public ComicTextNumberFloat(string text)
				: base(text)
			{
			}

			protected override void OnParseText(string s)
			{
				if (s == "1/2")
				{
					base.Number = 0.5f;
				}
				else
				{
					base.OnParseText(s);
				}
			}
		}

		public class ParseFilePathEventArgs : EventArgs
		{
			private readonly string path;

			private readonly ComicNameInfo nameInfo;

			public string Path => path;

			public ComicNameInfo NameInfo => nameInfo;

			public ParseFilePathEventArgs(string path)
			{
				this.path = path;
				nameInfo = ComicNameInfo.FromFilePath(path);
			}
		}

		public const int ReadPercentageAsRead = 95;

		public const string ClipboardFormat = "ComicBook";

		public const string DefaultCaptionFormat = "[{format} ][{series}][ {volume}][ #{number}][ - {title}][ ({year}[/{month}[/{day}]])]";

		public const string DefaultAlternateCaptionFormat = "[{alternateseries}][ #{alternatenumber}]";

		public const string DefaultComicExportFileNameFormat = "[{format} ][{series}][ {volume}][ #{number}][ ({year}[/{month}])]";

		public static readonly Equality<ComicBook> GuidEquality;

		public static bool EnableGroupNameCompression;

		private static TR tr;

		private static readonly Lazy<string> unkownText;

		private static readonly Lazy<string> pagesText;

		private static readonly Lazy<string> lastTimeOpenedAtText;

		private static readonly Lazy<string> readingAtPageText;

		private static readonly Lazy<string> lastPageReadIsText;

		private static readonly Lazy<string> noneText;

		private static readonly Lazy<string> notFoundText;

		private static readonly Lazy<string> neverText;

		private static readonly Lazy<string> volumeFormat;

		private static readonly Lazy<string> ofText;

		[NonSerialized]
		private volatile ComicBookContainer container;

		private Guid id = Guid.NewGuid();

		private DateTime addedTime = DateTime.MinValue;

		private DateTime releasedTime = DateTime.MinValue;

		private DateTime openedTime = DateTime.MinValue;

		private volatile int openCount;

		private volatile int currentPage;

		private volatile int lastPage;

		private float rating;

		private BitmapAdjustment colorAdjustment = BitmapAdjustment.Empty;

		private bool enableProposed = true;

		private YesNo seriesComplete = YesNo.Unknown;

		private bool enableDynamicUpdate = true;

		private bool check = NewBooksChecked;

		[NonSerialized]
		private volatile bool fileInfoRetrieved;

		private volatile bool comicInfoIsDirty;

		private volatile string filePath = string.Empty;

		private long fileSize = -1L;

		private volatile bool fileIsMissing;

		private DateTime fileModifiedTime = DateTime.MinValue;

		private DateTime fileCreationTime = DateTime.MinValue;

		private string customThumbnailKey;

		private float bookPrice = -1f;

		private string bookAge = string.Empty;

		private string bookCondition = string.Empty;

		private string bookStore = string.Empty;

		private string bookOwner = string.Empty;

		private string bookCollectionStatus = string.Empty;

		private string bookNotes = string.Empty;

		private string bookLocation = string.Empty;

		private string isbn = string.Empty;

		private volatile string fileName;

		private volatile string fileNameWithExtension;

		private volatile string fileFormat;

		private volatile string actualFileFormat;

		private volatile string fileDirectory;

		private static readonly Calendar weekCalendar;

		private string fileLocation;

		private int newPages;

		private ComicNameInfo proposed;

		[NonSerialized]
		private TextNumberFloat compareNumber;

		[NonSerialized]
		private TextNumberFloat compareAlternateNumber;

		private static readonly Dictionary<ComicBook, ExtraSyncInformation> syncInfo;

		private string customValuesStore = string.Empty;

		private static HashSet<string> searchableProperties;

		private static readonly Regex rxField;

		private static Dictionary<string, CultureInfo> languages;

		public static readonly ComicBook Default;

		private static readonly Dictionary<string, string> hasAsText;

		private static readonly ImagePackage publisherIcons;

		private static readonly ImagePackage ageRatingIcons;

		private static readonly ImagePackage formatIcons;

		private static readonly ImagePackage specialIcons;

		public static TR TR
		{
			get
			{
				if (tr == null)
				{
					tr = TR.Load("ComicBook");
				}
				return tr;
			}
		}

		[XmlIgnore]
		public ComicBookContainer Container
		{
			get
			{
				return container;
			}
			internal set
			{
				container = value;
			}
		}

		[XmlAttribute]
		public Guid Id
		{
			get
			{
				using (ItemMonitor.Lock(this))
				{
					return id;
				}
			}
			set
			{
				using (ItemMonitor.Lock(this))
				{
					if (id == value)
					{
						return;
					}
					id = value;
				}
				FireBookChanged("Id");
			}
		}

		[Browsable(true)]
		[XmlElement("Added")]
		[DefaultValue(typeof(DateTime), "01.01.0001")]
		[ResetValue(1)]
		public DateTime AddedTime
		{
			get
			{
				using (ItemMonitor.Lock(this))
				{
					return addedTime;
				}
			}
			set
			{
				SetProperty("AddedTime", ref addedTime, value, lockItem: true, !IsLinked);
			}
		}

		[Browsable(true)]
		[XmlElement("Released")]
		[DefaultValue(typeof(DateTime), "01.01.0001")]
		[ResetValue(1)]
		public DateTime ReleasedTime
		{
			get
			{
				using (ItemMonitor.Lock(this))
				{
					return releasedTime.DateOnly();
				}
			}
			set
			{
				SetProperty("ReleasedTime", ref releasedTime, value, lockItem: true);
			}
		}

		[Browsable(true)]
		[XmlElement("Opened")]
		[DefaultValue(typeof(DateTime), "01.01.0001")]
		[ResetValue(1)]
		public DateTime OpenedTime
		{
			get
			{
				using (ItemMonitor.Lock(this))
				{
					return openedTime;
				}
			}
			set
			{
				SetProperty("OpenedTime", ref openedTime, value, lockItem: true, !IsLinked);
			}
		}

		[Browsable(true)]
		[XmlElement("OpenCount")]
		[DefaultValue(0)]
		[ResetValue(1)]
		public int OpenedCount
		{
			get
			{
				return openCount;
			}
			set
			{
				if (openCount != value)
				{
					openCount = value;
					FireBookChanged("OpenedCount");
				}
			}
		}

		[Browsable(true)]
		[DefaultValue(0)]
		[ResetValue(1)]
		public int CurrentPage
		{
			get
			{
				return currentPage;
			}
			set
			{
				value = Math.Max(0, value);
				if (currentPage != value)
				{
					currentPage = value;
					FireBookChanged("CurrentPage");
					if (currentPage > LastPageRead)
					{
						LastPageRead = value;
					}
				}
			}
		}

		[Browsable(true)]
		[DefaultValue(0)]
		[ResetValue(1)]
		public int LastPageRead
		{
			get
			{
				return lastPage;
			}
			set
			{
				value = Math.Max(0, value);
				if (lastPage != value)
				{
					lastPage = value;
					FireBookChanged("LastPageRead");
				}
			}
		}

		[Browsable(true)]
		[DefaultValue(0)]
		[ResetValue(0)]
		public float Rating
		{
			get
			{
				return rating;
			}
			set
			{
				SetProperty("Rating", ref rating, value.Clamp(0f, 5f));
			}
		}

		[DefaultValue(typeof(BitmapAdjustment), "Empty")]
		public BitmapAdjustment ColorAdjustment
		{
			get
			{
				using (ItemMonitor.Lock(this))
				{
					return colorAdjustment;
				}
			}
			set
			{
				BitmapAdjustment bitmapAdjustment;
				using (ItemMonitor.Lock(this))
				{
					if (colorAdjustment == value)
					{
						return;
					}
					bitmapAdjustment = colorAdjustment;
					colorAdjustment = value;
				}
				FireBookChanged("ColorAdjustment", bitmapAdjustment, colorAdjustment);
			}
		}

		public bool ColorAdjustmentSpecified => ColorAdjustment != BitmapAdjustment.Empty;

		[Browsable(true)]
		[DefaultValue(true)]
		[ResetValue(0)]
		public bool EnableProposed
		{
			get
			{
				return enableProposed;
			}
			set
			{
				SetProperty("EnableProposed", ref enableProposed, value);
			}
		}

		[Browsable(true)]
		[DefaultValue(YesNo.Unknown)]
		[ResetValue(0)]
		public YesNo SeriesComplete
		{
			get
			{
				return seriesComplete;
			}
			set
			{
				SetProperty("SeriesComplete", ref seriesComplete, value);
			}
		}

		[Browsable(true)]
		[DefaultValue(true)]
		[ResetValue(1)]
		public bool EnableDynamicUpdate
		{
			get
			{
				return enableDynamicUpdate;
			}
			set
			{
				SetProperty("EnableDynamicUpdate", ref enableDynamicUpdate, value);
			}
		}

		public Guid LastOpenedFromListId
		{
			get;
			set;
		}

		public bool LastOpenedFromListIdSpecified => LastOpenedFromListId != Guid.Empty;

		[XmlAttribute]
		[DefaultValue(true)]
		public bool Checked
		{
			get
			{
				return check;
			}
			set
			{
				SetProperty("Checked", ref check, value);
			}
		}

		[Browsable(true)]
		[XmlIgnore]
		public bool FileInfoRetrieved
		{
			get
			{
				return fileInfoRetrieved;
			}
			set
			{
				fileInfoRetrieved = value;
			}
		}

		[Browsable(true)]
		[DefaultValue(false)]
		public bool ComicInfoIsDirty
		{
			get
			{
				return comicInfoIsDirty;
			}
			set
			{
				if (comicInfoIsDirty != value)
				{
					comicInfoIsDirty = value;
					FireBookChanged("ComicInfoIsDirty");
				}
			}
		}

		[Browsable(true)]
		[XmlAttribute("File")]
		[DefaultValue("")]
		public string FilePath
		{
			get
			{
				return filePath;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				if (!(filePath == value))
				{
					string text = FilePath;
					filePath = value;
					fileName = fileNameWithExtension = actualFileFormat = fileFormat = fileDirectory = null;
					proposed = null;
					FireBookChanged("FilePath", text, filePath);
					if (!string.IsNullOrEmpty(text))
					{
						OnFileRenamed(new ComicBookFileRenameEventArgs(text, filePath));
					}
				}
			}
		}

		[Browsable(true)]
		[DefaultValue(-1)]
		public long FileSize
		{
			get
			{
				return Interlocked.Read(ref fileSize);
			}
			set
			{
				if (Interlocked.Read(ref fileSize) != value)
				{
					Interlocked.Exchange(ref fileSize, value);
					FireBookChanged("FileSize");
				}
			}
		}

		[Browsable(true)]
		[XmlElement("Missing")]
		[DefaultValue(false)]
		public bool FileIsMissing
		{
			get
			{
				return fileIsMissing;
			}
			set
			{
				if (fileIsMissing != value)
				{
					fileIsMissing = value;
					FireBookChanged("FileIsMissing");
				}
			}
		}

		[Browsable(true)]
		[DefaultValue(typeof(DateTime), "01.01.0001")]
		public DateTime FileModifiedTime
		{
			get
			{
				using (ItemMonitor.Lock(this))
				{
					return fileModifiedTime;
				}
			}
			set
			{
				using (ItemMonitor.Lock(this))
				{
					if (fileModifiedTime == value)
					{
						return;
					}
					fileModifiedTime = value;
				}
				FireBookChanged("FileModifiedTime");
			}
		}

		[Browsable(true)]
		[DefaultValue(typeof(DateTime), "01.01.0001")]
		public DateTime FileCreationTime
		{
			get
			{
				using (ItemMonitor.Lock(this))
				{
					return fileCreationTime;
				}
			}
			set
			{
				using (ItemMonitor.Lock(this))
				{
					if (fileCreationTime == value)
					{
						return;
					}
					fileCreationTime = value;
				}
				FireBookChanged("FileCreationTime");
			}
		}

		[DefaultValue(null)]
		[ResetValue(0)]
		public string CustomThumbnailKey
		{
			get
			{
				return customThumbnailKey;
			}
			set
			{
				if (!(customThumbnailKey == value))
				{
					customThumbnailKey = value;
					FireBookChanged("CustomThumbnailKey");
				}
			}
		}

		[Browsable(true)]
		[DefaultValue(-1f)]
		[ResetValue(0)]
		public float BookPrice
		{
			get
			{
				return bookPrice;
			}
			set
			{
				SetProperty("BookPrice", ref bookPrice, value);
			}
		}

		[Browsable(true)]
		[DefaultValue("")]
		[ResetValue(0)]
		public string BookAge
		{
			get
			{
				return bookAge;
			}
			set
			{
				SetProperty("BookAge", ref bookAge, value);
			}
		}

		[Browsable(true)]
		[DefaultValue("")]
		[ResetValue(0)]
		public string BookCondition
		{
			get
			{
				return bookCondition;
			}
			set
			{
				SetProperty("BookCondition", ref bookCondition, value);
			}
		}

		[Browsable(true)]
		[DefaultValue("")]
		[ResetValue(0)]
		public string BookStore
		{
			get
			{
				return bookStore;
			}
			set
			{
				SetProperty("BookStore", ref bookStore, value);
			}
		}

		[Browsable(true)]
		[DefaultValue("")]
		[ResetValue(0)]
		public string BookOwner
		{
			get
			{
				return bookOwner;
			}
			set
			{
				SetProperty("BookOwner", ref bookOwner, value);
			}
		}

		[Browsable(true)]
		[DefaultValue("")]
		[ResetValue(0)]
		public string BookCollectionStatus
		{
			get
			{
				return bookCollectionStatus;
			}
			set
			{
				SetProperty("BookCollectionStatus", ref bookCollectionStatus, value);
			}
		}

		[Browsable(true)]
		[DefaultValue("")]
		[ResetValue(0)]
		public string BookNotes
		{
			get
			{
				return bookNotes;
			}
			set
			{
				SetProperty("BookNotes", ref bookNotes, value);
			}
		}

		[Browsable(true)]
		[DefaultValue("")]
		[ResetValue(0)]
		public string BookLocation
		{
			get
			{
				return bookLocation;
			}
			set
			{
				SetProperty("BookLocation", ref bookLocation, value);
			}
		}

		[Browsable(true)]
		[DefaultValue("")]
		[ResetValue(0)]
		public string ISBN
		{
			get
			{
				return isbn;
			}
			set
			{
				SetProperty("ISBN", ref isbn, value);
			}
		}

		[XmlIgnore]
		public string PagesAsTextSimple
		{
			get
			{
				if (base.PageCount > 0)
				{
					return base.PageCount.ToString();
				}
				return "-";
			}
			set
			{
				if (int.TryParse(value, out var result) && result >= 0)
				{
					base.PageCount = result;
				}
			}
		}

		[Browsable(true)]
		public string FileName
		{
			get
			{
				if (fileName != null)
				{
					return fileName;
				}
				try
				{
					fileName = Path.GetFileNameWithoutExtension(filePath);
				}
				catch (Exception)
				{
					fileName = string.Empty;
				}
				return fileName;
			}
		}

		[Browsable(true)]
		public string FileNameWithExtension
		{
			get
			{
				if (fileNameWithExtension != null)
				{
					return fileNameWithExtension;
				}
				try
				{
					fileNameWithExtension = Path.GetFileName(filePath);
				}
				catch (Exception)
				{
					fileNameWithExtension = string.Empty;
				}
				return fileNameWithExtension;
			}
		}

		[Browsable(true)]
		public string FileFormat
		{
			get
			{
				if (fileFormat != null)
				{
					return fileFormat;
				}
				try
				{
					fileFormat = Providers.Readers.GetSourceFormatName(filePath);
				}
				catch (Exception)
				{
					fileFormat = string.Empty;
				}
				return fileFormat;
			}
		}

		[Browsable(true)]
		public string ActualFileFormat
		{
			get
			{
				if (actualFileFormat != null)
				{
					return actualFileFormat;
				}
				try
				{
					actualFileFormat = Providers.Readers.GetSourceFormatName(filePath, true);
				}
				catch (Exception)
				{
					actualFileFormat = string.Empty;
				}
				return actualFileFormat;
			}
		}

		[Browsable(true)]
		public string FileDirectory
		{
			get
			{
				if (fileDirectory != null)
				{
					return fileDirectory;
				}
				try
				{
					fileDirectory = Path.GetDirectoryName(filePath);
				}
				catch (Exception)
				{
					fileDirectory = string.Empty;
				}
				return fileDirectory;
			}
		}

		[Browsable(true)]
		public bool IsValidComicBook
		{
			get
			{
				if (!string.IsNullOrEmpty(FilePath))
				{
					return Providers.Readers.GetSourceProviderType(FilePath) != null;
				}
				return false;
			}
		}

		//TODO: check to update Caption to a Virtual Tag
		[Browsable(true)]
		public string Caption => GetFullTitle(EngineConfiguration.Default.ComicCaptionFormat);

		[Browsable(true)]
		public string CaptionWithoutTitle => GetFullTitle(EngineConfiguration.Default.ComicCaptionFormat, "title");

		[Browsable(true)]
		public string CaptionWithoutFormat => GetFullTitle(EngineConfiguration.Default.ComicCaptionFormat, "format");

		[Browsable(true)]
		public string AlternateCaption => GetFullTitle(DefaultAlternateCaptionFormat);

		public string TargetFilename => GetFullTitle(EngineConfiguration.Default.ComicExportFileNameFormat);

		[Browsable(true)]
		public int ReadPercentage
		{
			get
			{
				if (base.PageCount <= 0 || LastPageRead <= 0)
				{
					return 0;
				}
				return ((LastPageRead + 1) * 100 / base.PageCount).Clamp(1, 100);
			}
		}

		public string ReadPercentageAsText => $"{ReadPercentage}%";

		[Browsable(true)]
		public bool HasBeenOpened => OpenedTime != DateTime.MinValue;

		[Browsable(true)]
		[XmlIgnore]
		public bool HasBeenRead
		{
			get
			{
				return ReadPercentage >= ReadPercentageAsRead;
			}
			set
			{
				if (value)
				{
					MarkAsRead();
				}
				else
				{
					MarkAsNotRead();
				}
			}
		}

		public string PagesAsText => FormatPages(base.PageCount);

		public string OpenedCountAsText => OpenedCount.ToString();

		public string InfoText
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("{0}\n", FileName);
				stringBuilder.AppendFormat("{0} ({1})\n\n", FileSizeAsText, PagesAsText);
				stringBuilder.AppendFormat("{0}\n", ActualFileFormat);
				stringBuilder.AppendFormat("{0}\n\n", FileDirectory);
				stringBuilder.Append(StringUtility.Format(lastTimeOpenedAtText.Value, OpenedTimeAsText));
				stringBuilder.AppendLine();
				stringBuilder.Append(StringUtility.Format(readingAtPageText.Value, CurrentPage + 1));
				stringBuilder.AppendLine();
				stringBuilder.Append(StringUtility.Format(lastPageReadIsText.Value, LastPageRead + 1));
				return stringBuilder.ToString();
			}
		}

		public string NumberAsText => FormatNumber(base.Number, ShadowCount);

		[Browsable(true)]
		public string NumberOnly => ShadowNumber;

		public string AlternateNumberAsText => FormatNumber(base.AlternateNumber, base.AlternateCount);

		public string VolumeAsText => FormatVolume(base.Volume);

		[Browsable(true)]
		public string VolumeOnly
		{
			get
			{
				if (base.Volume >= 0)
				{
					return base.Volume.ToString();
				}
				return string.Empty;
			}
		}

		public string LastPageReadAsText
		{
			get
			{
				if (LastPageRead > 0)
				{
					return (LastPageRead + 1).ToString();
				}
				return noneText.Value;
			}
		}

		public string LanguageAsText
		{
			get
			{
				if (!string.IsNullOrEmpty(base.LanguageISO))
				{
					return GetLanguageName(base.LanguageISO);
				}
				return string.Empty;
			}
		}

		public string ArtistInfo
		{
			get
			{
				HashSet<string> uniqueNames = new HashSet<string>();
				StringBuilder stringBuilder = new StringBuilder();
				AppendUniqueName(stringBuilder, "/", base.Writer, uniqueNames);
				AppendUniqueName(stringBuilder, "/", base.Penciller, uniqueNames);
				AppendUniqueName(stringBuilder, "/", base.Inker, uniqueNames);
				AppendUniqueName(stringBuilder, "/", base.Colorist, uniqueNames);
				AppendUniqueName(stringBuilder, "/", base.Letterer, uniqueNames);
				AppendUniqueName(stringBuilder, "/", base.CoverArtist, uniqueNames);

				return stringBuilder.ToString();
			}
		}

		public string YearAsText => FormatYear(base.Year);

		public string MonthAsText
		{
			get
			{
				if (base.Month != -1)
				{
					return base.Month.ToString();
				}
				return string.Empty;
			}
		}

		public string DayAsText
		{
			get
			{
				if (base.Day != -1)
				{
					return base.Day.ToString();
				}
				return string.Empty;
			}
		}

		public int Week
		{
			get
			{
				DateTime published = Published;
				if (published == DateTime.MinValue)
				{
					return -1;
				}
				return weekCalendar.GetWeekOfYear(published, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
			}
		}

		public string WeekAsText
		{
			get
			{
				int week = Week;
				if (week != -1)
				{
					return week.ToString();
				}
				return string.Empty;
			}
		}

		public string PublishedRegional => FormatDate(Published, ComicDateFormat.Short, toLocal: false, unkownText.Value);

		public string PublishedAsText
		{
			get
			{
				string shadowYearAsText = ShadowYearAsText;
				string text = string.Empty;
				if (!string.IsNullOrEmpty(shadowYearAsText))
				{
					string monthAsText = MonthAsText;
					if (!string.IsNullOrEmpty(monthAsText))
					{
						string dayAsText = DayAsText;
						if (!string.IsNullOrEmpty(dayAsText))
						{
							text = text + dayAsText + "/";
						}
						text = text + monthAsText + "/";
					}
					text += shadowYearAsText;
				}
				return text;
			}
		}

		public DateTime Published
		{
			get
			{
				if (ShadowYear <= 0)
				{
					return DateTime.MinValue;
				}
				int year = ShadowYear.Clamp(1, 10000);
				int month = base.Month.Clamp(1, 12);
				int day = base.Day.Clamp(1, DateTime.DaysInMonth(year, month));
				return new DateTime(year, month, day);
			}
		}

		public string CountAsText
		{
			get
			{
				if (base.Count != -1)
				{
					return base.Count.ToString();
				}
				return string.Empty;
			}
		}

		public string NewPagesAsText
		{
			get
			{
				if (!IsDynamicSource || NewPages <= 0)
				{
					return string.Empty;
				}
				return NewPages.ToString();
			}
		}

		public string AlternateCountAsText
		{
			get
			{
				if (base.AlternateCount != -1)
				{
					return base.AlternateCount.ToString();
				}
				return string.Empty;
			}
		}

		public string RatingAsText => FormatRating(Rating);

		public string CommunityRatingAsText => FormatRating(base.CommunityRating);

		public string CoverAsText => ComicInfo.GetYesNoAsText((base.FrontCoverCount != 0) ? YesNo.Yes : YesNo.No);

		public string FileSizeAsText
		{
			get
			{
				long num = FileSize;
				if (num == -1)
				{
					return notFoundText.Value;
				}
				return string.Format(new FileLengthFormat(), "{0}", new object[1]
				{
					num
				});
			}
		}

		public string MangaAsText => ComicInfo.GetYesNoAsText(base.Manga);

		public string SeriesCompleteAsText => ComicInfo.GetYesNoAsText(SeriesComplete);

		public string EnableProposedAsText => ComicInfo.GetYesNoAsText(EnableProposed ? YesNo.Yes : YesNo.No);

		public string HasBeenReadAsText => ComicInfo.GetYesNoAsText(HasBeenRead ? YesNo.Yes : YesNo.No);

		public string IsLinkedAsText => ComicInfo.GetYesNoAsText(IsLinked);

		public string BlackAndWhiteAsText => ComicInfo.GetYesNoAsText(base.BlackAndWhite);

		public string BookmarkCountAsText
		{
			get
			{
				if (base.BookmarkCount > 0)
				{
					return base.BookmarkCount.ToString();
				}
				return noneText.Value;
			}
		}

		public ComicPageInfo CurrentPageInfo => GetPage(CurrentPage);

		public string FileLocation
		{
			get
			{
				if (!string.IsNullOrEmpty(fileLocation))
				{
					return fileLocation;
				}
				return FilePath;
			}
		}

		public string DisplayFileLocation
		{
			get
			{
				if (!IsInContainer)
				{
					return FilePath;
				}
				return Caption;
			}
		}

		public int Status
		{
			get
			{
				int num = 0;
				if (FileIsMissing && IsLinked)
				{
					num |= 1;
				}
				if (ComicInfoIsDirty)
				{
					num |= 2;
				}
				return num;
			}
		}

		[Browsable(true)]
		public bool IsLinked => !string.IsNullOrEmpty(filePath);

		[XmlIgnore]
		public string BookPriceAsText
		{
			get
			{
				if (!(bookPrice < 0f))
				{
					return $"{bookPrice:0.00}";
				}
				return unkownText.Value;
			}
			set
			{
				if (float.TryParse(value, out var result))
				{
					BookPrice = result;
				}
				else
				{
					BookPrice = -1f;
				}
			}
		}

		public string OpenedTimeAsText => FormatDate(OpenedTime);

		public string AddedTimeAsText => FormatDate(AddedTime, ComicDateFormat.Long, toLocal: false, unkownText.Value);

		public string ReleasedTimeAsText => FormatDate(ReleasedTime, ComicDateFormat.Short, toLocal: false, unkownText.Value);

		public string FileCreationTimeAsText => FormatFileDate(FileCreationTime);

		public string FileModifiedTimeAsText => FormatFileDate(FileModifiedTime);

		public bool IsInContainer => container != null;

		public ComicsEditModes EditMode
		{
			get
			{
				if (!IsInContainer)
				{
					return ComicsEditModes.Default;
				}
				return Container.EditMode;
			}
		}

		[DefaultValue(false)]
		[XmlAttribute]
		public bool IsDynamicSource
		{
			get;
			set;
		}

		[DefaultValue(0)]
		public int NewPages
		{
			get
			{
				return newPages;
			}
			set
			{
				SetProperty("NewPages", ref newPages, value);
			}
		}

		public string ProposedSeries
		{
			get
			{
				UpdateProposed();
				return proposed.Series;
			}
		}

		public string ProposedTitle
		{
			get
			{
				UpdateProposed();
				return proposed.Title;
			}
		}

		public string ProposedFormat
		{
			get
			{
				UpdateProposed();
				return proposed.Format;
			}
		}

		public int ProposedVolume
		{
			get
			{
				UpdateProposed();
				return proposed.Volume;
			}
		}

		public string ProposedNumber
		{
			get
			{
				UpdateProposed();
				if (!(base.Number == "-"))
				{
					return proposed.Number;
				}
				return string.Empty;
			}
		}

		public int ProposedCount
		{
			get
			{
				UpdateProposed();
				return proposed.Count;
			}
		}

		public int ProposedYear
		{
			get
			{
				UpdateProposed();
				return proposed.Year;
			}
		}

		public string ProposedYearAsText => FormatYear(ProposedYear);

		public string ProposedNumberAsText => FormatNumber(ProposedNumber, ProposedCount);

		public string ProposedVolumeAsText => FormatVolume(ProposedVolume);

		public string ProposedNakedVolumeAsText
		{
			get
			{
				if (ProposedVolume != -1)
				{
					return ProposedVolume.ToString();
				}
				return string.Empty;
			}
		}

		public string ProposedCountAsText
		{
			get
			{
				if (ProposedCount != -1)
				{
					return ProposedCount.ToString();
				}
				return string.Empty;
			}
		}

		public string ShadowSeries
		{
			get
			{
				if (!EnableProposed || !string.IsNullOrEmpty(base.Series))
				{
					return base.Series;
				}
				return ProposedSeries;
			}
		}

		public string ShadowTitle
		{
			get
			{
				if (!EnableProposed || !string.IsNullOrEmpty(base.Title))
				{
					return base.Title;
				}
				return ProposedTitle;
			}
		}

		public string ShadowFormat
		{
			get
			{
				if (!EnableProposed || !string.IsNullOrEmpty(base.Format))
				{
					return base.Format;
				}
				return ProposedFormat;
			}
		}

		public int ShadowVolume
		{
			get
			{
				if (!EnableProposed || base.Volume != -1)
				{
					return base.Volume;
				}
				return ProposedVolume;
			}
		}

		public string ShadowNumber
		{
			get
			{
				if (!EnableProposed || !string.IsNullOrEmpty(base.Number))
				{
					return base.Number;
				}
				return ProposedNumber;
			}
		}

		public int ShadowCount
		{
			get
			{
				if (!EnableProposed || base.Count != -1)
				{
					return base.Count;
				}
				return ProposedCount;
			}
		}

		public int ShadowYear
		{
			get
			{
				if (!EnableProposed || base.Year != -1)
				{
					return base.Year;
				}
				return ProposedYear;
			}
		}

		[Browsable(true)]
		public string ShadowYearAsText => FormatYear(ShadowYear);

		public string ShadowNumberAsText => FormatNumber(ShadowNumber, ShadowCount);

		public string ShadowVolumeAsText => FormatVolume(ShadowVolume);

		public string ShadowCountAsText
		{
			get
			{
				if (ShadowCount != -1)
				{
					return ShadowCount.ToString();
				}
				return string.Empty;
			}
		}

		public TextNumberFloat CompareNumber => compareNumber ?? (compareNumber = new ComicTextNumberFloat(ShadowNumber));

		public TextNumberFloat CompareAlternateNumber => compareAlternateNumber ?? (compareAlternateNumber = new ComicTextNumberFloat(base.AlternateNumber));

		[DefaultValue(null)]
		public ExtraSyncInformation ExtraSyncInformation
		{
			get
			{
				using (ItemMonitor.Lock(syncInfo))
				{
					ExtraSyncInformation value;
					return syncInfo.TryGetValue(this, out value) ? value : null;
				}
			}
			set
			{
				using (ItemMonitor.Lock(syncInfo))
				{
					syncInfo[this] = value;
				}
			}
		}

		[Browsable(true)]
		[DefaultValue("")]
		[ResetValue(0)]
		public string CustomValuesStore
		{
			get
			{
				return customValuesStore;
			}
			set
			{
				SetProperty("CustomValuesStore", ref customValuesStore, value);
			}
		}

		public static ImagePackage PublisherIcons => publisherIcons;

		public static ImagePackage AgeRatingIcons => ageRatingIcons;

		public static ImagePackage FormatIcons => formatIcons;

		public static ImagePackage SpecialIcons => specialIcons;

		public static bool NewBooksChecked
		{
			get;
			set;
		}

		[field: NonSerialized]
		public event EventHandler<ComicBookFileRenameEventArgs> FileRenamed;

		[field: NonSerialized]
		public event EventHandler<CreateComicProviderEventArgs> CreateComicProvider;

		[field: NonSerialized]
		public event EventHandler<CreateComicProviderEventArgs> ComicProviderCreated;

		public static event EventHandler<ParseFilePathEventArgs> ParseFilePath;

		Dictionary<int, string> CachedVirtualTags = new Dictionary<int, string>();
		private string GetVirtualTagValue(int id)
		{
			if (CachedVirtualTags.TryGetValue(id, out var value))
				return value;

			string captionFormat = VirtualTagsCollection.Tags.GetValue(id).CaptionFormat;
			string saved = (!string.IsNullOrEmpty(captionFormat)) ? GetFullTitle(captionFormat) : string.Empty;

			CachedVirtualTags[id] = saved;
			return saved;
		}

		private void VirtualTagsCollection_TagsRefresh(object sender, EventArgs e)
		{
			ClearVirtualTagsCache();
		}

		private void ClearVirtualTagsCache()
		{
			CachedVirtualTags.Clear();
		}

		public string VirtualTag01 => GetVirtualTagValue(1);
		public string VirtualTag02 => GetVirtualTagValue(2);
		public string VirtualTag03 => GetVirtualTagValue(3);
		public string VirtualTag04 => GetVirtualTagValue(4);
		public string VirtualTag05 => GetVirtualTagValue(5);
		public string VirtualTag06 => GetVirtualTagValue(6);
		public string VirtualTag07 => GetVirtualTagValue(7);
		public string VirtualTag08 => GetVirtualTagValue(8);
		public string VirtualTag09 => GetVirtualTagValue(9);
		public string VirtualTag10 => GetVirtualTagValue(10);
		public string VirtualTag11 => GetVirtualTagValue(11);
		public string VirtualTag12 => GetVirtualTagValue(12);
		public string VirtualTag13 => GetVirtualTagValue(13);
		public string VirtualTag14 => GetVirtualTagValue(14);
		public string VirtualTag15 => GetVirtualTagValue(15);
		public string VirtualTag16 => GetVirtualTagValue(16);
		public string VirtualTag17 => GetVirtualTagValue(17);
		public string VirtualTag18 => GetVirtualTagValue(18);
		public string VirtualTag19 => GetVirtualTagValue(19);
		public string VirtualTag20 => GetVirtualTagValue(20);

		static ComicBook()
		{
			GuidEquality = new Equality<ComicBook>((ComicBook a, ComicBook b) => a.Id == b.Id, (ComicBook a) => a.Id.GetHashCode());
			EnableGroupNameCompression = false;
			unkownText = new Lazy<string>(() => TR["Unknown"]);
			pagesText = new Lazy<string>(() => TR["Pages", "{0} Page(s)"]);
			lastTimeOpenedAtText = new Lazy<string>(() => TR["LastTimeOpenedAt", "Last time opened at {0}"]);
			readingAtPageText = new Lazy<string>(() => TR["ReadingAtPage", "Reading at page {0}"]);
			lastPageReadIsText = new Lazy<string>(() => TR["LastPageReadIs", "Last page read is {0}"]);
			noneText = new Lazy<string>(() => TR["None", "None"]);
			notFoundText = new Lazy<string>(() => TR["NotFound", "not found"]);
			neverText = new Lazy<string>(() => TR["Never", "never"]);
			volumeFormat = new Lazy<string>(() => TR["Volume", "V{0}"]);
			ofText = new Lazy<string>(() => TR["Of", "of"]);
			weekCalendar = new GregorianCalendar();
			syncInfo = new Dictionary<ComicBook, ExtraSyncInformation>();
			rxField = new Regex("{(?<name>[a-z]+)}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			Default = new ComicBook();
			hasAsText = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			publisherIcons = new ImagePackage
			{
				EnableWidthCropping = true
			};
			ageRatingIcons = new ImagePackage
			{
				EnableWidthCropping = true
			};
			formatIcons = new ImagePackage
			{
				EnableWidthCropping = true
			};
			specialIcons = new ImagePackage
			{
				EnableWidthCropping = true
			};
			NewBooksChecked = true;
		}

		public ComicBook()
		{
			VirtualTagsCollection.TagsRefresh += VirtualTagsCollection_TagsRefresh;
		}

		public ComicBook(ComicBook cb)
		{
			CopyFrom(cb);
			if (cb.CreateComicProvider != null)
			{
				CreateComicProvider += cb.CreateComicProvider;
			}
			if (cb.ComicProviderCreated != null)
			{
				ComicProviderCreated += cb.ComicProviderCreated;
			}
		}

		public static ComicBook Create(string file, RefreshInfoOptions options)
		{
			ComicBook comicBook = new ComicBook
			{
				FilePath = file
			};
			comicBook.RefreshInfoFromFile(options);
			return comicBook;
		}

		private void ResetOptimizedNumbers()
		{
			compareNumber = (compareAlternateNumber = null);
		}

		public static void ClearExtraSyncInformation()
		{
			using (ItemMonitor.Lock(syncInfo))
			{
				syncInfo.Clear();
			}
		}

		public IEnumerable<StringPair> GetCustomValues()
		{
			return ValuesStore.GetValues(CustomValuesStore);
		}

		public void SetCustomValue(string key, string value)
		{
			if (!string.IsNullOrEmpty(key))
			{
				if (string.IsNullOrEmpty(value))
				{
					DeleteCustomValue(key);
				}
				else
				{
					CustomValuesStore = new ValuesStore(CustomValuesStore).Add(key, value).ToString();
				}
			}
		}

		public string GetCustomValue(string key)
		{
			return ValuesStore.GetValue(CustomValuesStore, key);
		}

		public void DeleteCustomValue(string key)
		{
			CustomValuesStore = new ValuesStore(CustomValuesStore).Remove(key).ToString();
		}

		public ThumbnailKey GetThumbnailKey(int page)
		{
			string locationKey = ((!IsLinked) ? (string.IsNullOrEmpty(CustomThumbnailKey) ? ThumbnailKey.GetResource(ThumbnailKey.ResourceKey, "Unknown") : ThumbnailKey.GetResource(ThumbnailKey.CustomKey, CustomThumbnailKey)) : FileLocation);
			return GetThumbnailKey(page, locationKey);
		}

		public ThumbnailKey GetThumbnailKey(int page, string locationKey)
		{
			return new ThumbnailKey(this, locationKey, FileSize, FileModifiedTime, TranslatePageToImageIndex(page), GetPage(page).Rotation);
		}

		public ThumbnailKey GetFrontCoverThumbnailKey()
		{
			return GetThumbnailKey(base.FrontCoverPageIndex);
		}

		public PageKey GetFrontCoverKey(BitmapAdjustment bitmapAdjustment)
		{
			return GetPageKey(base.FrontCoverPageIndex, bitmapAdjustment);
		}

		public PageKey GetPageKey(int page, BitmapAdjustment bitmapAdjustment)
		{
			return new PageKey(this, FileLocation, FileSize, FileModifiedTime, TranslatePageToImageIndex(page), GetPage(page).Rotation, bitmapAdjustment);
		}

		public ImageKey GetImageKey(int image)
		{
			return new PageKey(this, FileLocation, FileSize, FileModifiedTime, image, ImageRotation.None, BitmapAdjustment.Empty);
		}

		public ImageProvider CreateImageProvider()
		{
			CreateComicProviderEventArgs createComicProviderEventArgs = new CreateComicProviderEventArgs();
			OnCreateComicProvider(createComicProviderEventArgs);
			createComicProviderEventArgs.Provider = createComicProviderEventArgs.Provider ?? Providers.Readers.CreateSourceProvider(FilePath);
			OnComicProviderCreated(createComicProviderEventArgs);
			return createComicProviderEventArgs.Provider;
		}

		public ImageProvider OpenProvider(int lastPageIndexToRead = -1)
		{
			ImageProvider imageProvider = CreateImageProvider();
			try
			{
				if (lastPageIndexToRead != -1)
				{
					int imageIndex = TranslatePageToImageIndex(lastPageIndexToRead);
                    imageProvider.ImageReady += delegate(object s, ImageIndexReadyEventArgs e)
					{
						e.Cancel = e.ImageNumber == imageIndex;
					};
				}
				imageProvider.Open(async: false);
				return imageProvider;
			}
			catch
			{
				imageProvider?.Dispose();
				return null;
			}
		}

		public string GetPublisherIconKey(bool yearOnly = true)
		{
			string text = base.Publisher;
			if (base.Year >= 0 && base.Month >= 0 && !yearOnly)
				return $"{text}({YearAsText}_{Month:00})";

			if (base.Year >= 0)
				return $"{text}({YearAsText})";

			return text;
		}

		public string GetImprintIconKey(bool yearOnly = true)
		{
			string text = base.Imprint;
			if (base.Year >= 0 && base.Month >= 0 && !yearOnly)
				return $"{text}({YearAsText}_{Month:00})";

			if (base.Year >= 0)
				return $"{text}({YearAsText})";

			return text;
		}

		private IEnumerable<Image> GetIconsInternal()
		{
			Image image = PublisherIcons.GetImage(GetPublisherIconKey(false));//Year_Month
			if (image != null)
			{
				yield return image;
			}	
			else if ((image = PublisherIcons.GetImage(GetPublisherIconKey(true))) != null)//Year Only
			{
				yield return image;
			}
			else
			{
				image = PublisherIcons.GetImage(Publisher);
				if (image != null)
					yield return image;
			}

			image = PublisherIcons.GetImage(GetImprintIconKey(false));//Year_Month
			if (image != null)
			{
				yield return image;
			}
			else if ((image = PublisherIcons.GetImage(GetImprintIconKey(true))) != null)//Year Only
			{
				yield return image;
			}
			else
			{
				image = PublisherIcons.GetImage(Imprint);
				if (image != null)
					yield return image;
			}

			image = AgeRatingIcons.GetImage(AgeRating);
			if (image != null)
			{
				yield return image;
			}

			image = FormatIcons.GetImage(ShadowFormat);
			if (image != null)
			{
				yield return image;
			}

			if (!string.IsNullOrEmpty(Tags))
			{
				foreach (string item in Tags.ListStringToSet(','))
				{
					image = (image = SpecialIcons.GetImage(item));
					if (image != null)
						yield return image;
				}
			}
			if (SeriesComplete == YesNo.Yes)
			{
				image = SpecialIcons.GetImage("SeriesComplete");
				if (image != null)
					yield return image;
			}
			if (BlackAndWhite == YesNo.Yes)
			{
				image = SpecialIcons.GetImage("BlackAndWhite");
				if (image != null)
					yield return image;
			}
			if (Manga == MangaYesNo.Yes)
			{
				image = SpecialIcons.GetImage("Manga");
				if (image != null)
					yield return image;
			}
			if (Manga == MangaYesNo.YesAndRightToLeft)
			{
				image = SpecialIcons.GetImage("MangaRightToLeft");
				if (image != null)
					yield return image;
			}
		}

		public IEnumerable<Image> GetIcons()
		{
			return GetIconsInternal();
		}

		public void CopyFrom(ComicBook cb)
		{
			SetInfo(cb, onlyUpdateEmpty: false);
			Id = cb.Id;
			AddedTime = cb.AddedTime;
			ReleasedTime = cb.ReleasedTime;
			OpenedTime = cb.OpenedTime;
			OpenedCount = cb.OpenedCount;
			CurrentPage = cb.CurrentPage;
			LastPageRead = cb.LastPageRead;
			Rating = cb.Rating;
			ColorAdjustment = cb.ColorAdjustment;
			EnableDynamicUpdate = cb.EnableDynamicUpdate;
			EnableProposed = cb.EnableProposed;
			SeriesComplete = cb.SeriesComplete;
			Checked = cb.Checked;
			FilePath = cb.FilePath;
			FileSize = cb.FileSize;
			FileModifiedTime = cb.FileModifiedTime;
			FileCreationTime = cb.FileCreationTime;
			fileLocation = cb.FileLocation;
			customThumbnailKey = cb.CustomThumbnailKey;
			LastOpenedFromListId = cb.LastOpenedFromListId;
			CustomValuesStore = cb.CustomValuesStore;
		}

		public void CopyTo(ComicBook cb)
		{
			cb.CopyFrom(this);
		}

		public string GetFullTitle(string textFormat, params string[] ignore)
		{
			try
			{
                return ExtendedStringFormater.Format(textFormat, delegate(string s)
				{
					try
					{
						if (ignore != null && ignore.Length != 0 && ignore.Contains(s, StringComparer.OrdinalIgnoreCase))
						{
							return null;
						}
						MapPropertyNameToAsText(s, out var newName);
						return GetPropertyValue<string>(newName, ComicValueType.Shadow);
					}
					catch
					{
						return null;
					}
				});
			}
			catch
			{
				return string.Empty;
			}
		}

		public void SetShadowValues(ComicInfo ci)
		{
			ci.Series = ShadowSeries;
			ci.Count = ShadowCount;
			ci.Title = ShadowTitle;
			ci.Year = ShadowYear;
			ci.Number = ShadowNumber;
			ci.Format = ShadowFormat;
			ci.Volume = ShadowVolume;
		}

		public void SetFileLocation(string fileLocation)
		{
			this.fileLocation = fileLocation;
		}

		public void MarkAsNotRead()
		{
			OpenedTime = DateTime.MinValue;
			int num2 = (CurrentPage = 0);
			int num5 = (OpenedCount = (LastPageRead = num2));
		}

		public void MarkAsRead()
		{
			if (OpenedCount == 0)
			{
				OpenedCount = 1;
			}
			OpenedTime = DateTime.Now;
			if (base.PageCount > 0)
			{
				//HACK: When marking as read a book with only 1 page, set it to 1 (Page 2)
				int currentPage = base.PageCount == 1 ? 1 : base.PageCount - 1;
				CurrentPage = LastPageRead = currentPage;
			}
		}

		public void ResetProperties(int level = 0)
		{
			ResetValueAttribute.ResetProperties(this, level);
		}

		public bool RemoveFromContainer()
		{
			return Container?.Books.Remove(this) ?? false;
		}

		public bool IsSearchable(string propName)
		{
			if (searchableProperties == null)
			{
				searchableProperties = new HashSet<string>(from pi in GetType().GetProperties().Where(SearchableAttribute.IsSearchable)
														   select pi.Name);
			}
			return searchableProperties.Contains(propName);
		}

		public object GetUntypedPropertyValue(string propName)
		{
			return GetType().GetProperty(propName).GetValue(this, null);
		}

		public T GetPropertyValue<T>(string propName, ComicValueType cvt = ComicValueType.Standard)
		{
			MapPropertyName(propName, out propName, cvt);
			try
			{
				if (!propName.StartsWith("{"))
				{
					return PropertyCaller.CreateGetMethod<ComicBook, T>(propName)(this);
				}
				propName = propName.Substring(1, propName.Length - 2);
				return (T)(object)GetCustomValue(propName);
			}
			catch (Exception)
			{
				return default(T);
			}
		}

		public string GetStringPropertyValue(string propName, ComicValueType cvt = ComicValueType.Standard)
		{
			return GetPropertyValue<string>(propName, cvt) ?? string.Empty;
		}

		public T GetPropertyValue<T>(string propName, bool proposed)
		{
			T propertyValue = GetPropertyValue<T>(propName);
			if (!proposed || !EnableProposed || !IsDefaultPropertyValue(propertyValue) || !MapPropertyName(propName, out propName, ComicValueType.Proposed))
			{
				return propertyValue;
			}
			return GetPropertyValue<T>(propName);
		}

		public string FormatString(string format)
		{
			return rxField.Replace(format, delegate (Match m)
			{
				try
				{
					return PropertyCaller.CreateGetMethod<ComicBook, string>(m.Groups[1].Value)(this) ?? string.Empty;
				}
				catch (Exception)
				{
					return m.Value;
				}
			});
		}

		public void WriteProposedValues(bool overwriteAll)
		{
			if (overwriteAll || string.IsNullOrEmpty(base.Series))
			{
				base.Series = ProposedSeries;
			}
			if (overwriteAll || string.IsNullOrEmpty(base.Title))
			{
				base.Title = ProposedTitle;
			}
			if (overwriteAll || base.Year == -1)
			{
				base.Year = ProposedYear;
			}
			if (overwriteAll || string.IsNullOrEmpty(base.Number))
			{
				base.Number = ProposedNumber;
			}
			if (overwriteAll || base.Volume == -1)
			{
				base.Volume = ProposedVolume;
			}
			if (overwriteAll || base.Count == -1)
			{
				base.Count = ProposedCount;
			}
			if (overwriteAll || string.IsNullOrEmpty(base.Format))
			{
				base.Format = ProposedFormat;
			}
			EnableProposed = false;
		}

		public bool RenameFile(string newName)
		{
			if (!IsLinked || !EditMode.IsLocalComic())
			{
				return false;
			}
			try
			{
				newName = FileUtility.MakeValidFilename(newName);
				string text = Path.Combine(FileDirectory, newName + Path.GetExtension(FilePath));
				if (string.Equals(FilePath, text, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
				File.Move(FilePath, text);
				FilePath = text;
				return true;
			}
			catch
			{
				return false;
			}
		}

		public ComicBookNavigator CreateNavigator()
		{
			try
			{
				ComicBookNavigator comicBookNavigator = new ComicBookNavigator(this);
				switch (base.Manga)
				{
					case MangaYesNo.Unknown:
						comicBookNavigator.RightToLeftReading = YesNo.Unknown;
						break;
					case MangaYesNo.No:
					case MangaYesNo.Yes:
						comicBookNavigator.RightToLeftReading = YesNo.No;
						break;
					case MangaYesNo.YesAndRightToLeft:
						comicBookNavigator.RightToLeftReading = YesNo.Yes;
						break;
				}
				return comicBookNavigator;
			}
			catch
			{
				return null;
			}
		}

		public void SetValue(string propertyName, object value)
		{
			try
			{
				GetType().GetProperty(propertyName).SetValue(this, value, null);
			}
			catch (Exception)
			{
			}
		}

		public string Hash()
		{
			try
			{
				if (EditMode.IsLocalComic() && !string.IsNullOrEmpty(FilePath) && File.Exists(FilePath))
				{
					return CreateFileHash();
				}
			}
			catch
			{
			}
			return null;
		}

		public void CopyDataFrom(ComicBook data, IEnumerable<string> properties)
		{
			Type type = data.GetType();
			foreach (string property2 in properties)
			{
				try
				{
					if (property2.StartsWith("{"))
					{
						string key = property2.Substring(1, property2.Length - 2);
						SetCustomValue(key, data.GetCustomValue(key));
					}
					else
					{
						PropertyInfo property = type.GetProperty(property2);
						property.SetValue(this, property.GetValue(data, null), null);
					}
				}
				catch
				{
				}
			}
		}

		public void ToClipboard()
		{
			ComicBook comicBook = CloneUtility.Clone(this);
			comicBook.Id = Guid.NewGuid();
			DataObject dataObject = new DataObject();
			dataObject.SetData(DataFormats.UnicodeText, GetInfo().ToXml());
			dataObject.SetData(ClipboardFormat, comicBook);
			Clipboard.SetDataObject(dataObject);
		}

		public bool IsDefaultValue(string property)
		{
			PropertyInfo property2 = GetType().GetProperty(property);
			return object.Equals(property2.GetValue(Default, null), property2.GetValue(this, null));
		}

		public void ValidateData()
		{
			TrimExcessPageInfo();
			if (!FileIsMissing && FileCreationTime == DateTime.MinValue)
			{
				try
				{
					FileCreationTime = File.GetCreationTimeUtc(FilePath);
				}
				catch (Exception)
				{
				}
			}
		}

		public void UpdateDynamicPageCount(bool refresh, IProgressState ps = null)
		{
			try
			{
				using (ImageProvider imageProvider = Providers.Readers.CreateSourceProvider(FilePath))
				{
					IDynamicImages dynamicImages = imageProvider as IDynamicImages;
					if (dynamicImages != null)
					{
						dynamicImages.RefreshMode = refresh;
					}
					if (ps != null)
					{
						imageProvider.ImageReady += delegate (object s, ImageIndexReadyEventArgs e)
						{
							ps.ProgressAvailable = base.PageCount > 0;
							if (ps.ProgressAvailable)
							{
								ps.ProgressPercentage = 100 * e.ImageNumber / base.PageCount;
							}
						};
					}
					imageProvider.Open(async: false);
					NewPages = Math.Max(imageProvider.Count - base.PageCount, 0);
				}
			}
			catch (Exception)
			{
			}
		}

		public bool WriteInfoToFile(bool withRefreshFileProperties = true)
		{
			if (!EditMode.IsLocalComic())
			{
				return false;
			}
			FileInfo fileInfo = new FileInfo(FilePath);
			if (!fileInfo.Exists || fileInfo.IsReadOnly)
			{
				return false;
			}
			using (ImageProvider imageProvider = CreateImageProvider())
			{
				IInfoStorage infoStorage = imageProvider as IInfoStorage;
				if (infoStorage == null)
				{
					return false;
				}
				infoStorage.StoreInfo(GetInfo());
				FileInfoRetrieved = true;
			}
			if (withRefreshFileProperties)
			{
				RefreshFileProperties();
			}
			return true;
		}

		public void ResetInfoRetrieved()
		{
			FileInfoRetrieved = false;
		}

		public void RefreshInfoFromFile(RefreshInfoOptions options)
		{
			if ((options & RefreshInfoOptions.DontReadInformation) != 0 || !EditMode.IsLocalComic() || !IsLinked)
			{
				return;
			}
			DateTime d = FileModifiedTime;
			long num = FileSize;
			RefreshFileProperties();
			if (FileIsMissing)
			{
				return;
			}
			bool flag = FileModifiedTime != d;
			try
			{
				IsDynamicSource = Providers.Readers.GetSourceProviderInfo(FilePath).Formats.All((FileFormat f) => f.Dynamic);
			}
			catch (Exception)
			{
				IsDynamicSource = false;
			}
			if (!(!FileInfoRetrieved || flag) && (options & RefreshInfoOptions.ForceRefresh) == 0 && ((options & (RefreshInfoOptions.GetFastPageCount | RefreshInfoOptions.GetPageCount)) == 0 || base.PageCount != 0))
			{
				return;
			}
			using (ImageProvider imageProvider = CreateImageProvider())
			{
				IInfoStorage infoStorage = imageProvider as IInfoStorage;
				if (infoStorage == null)
				{
					return;
				}
				if (!ComicInfoIsDirty)
				{
					SetInfo(infoStorage.LoadInfo((flag || !FileInfoRetrieved) ? InfoLoadingMethod.Complete : InfoLoadingMethod.Fast));
				}
				if (!imageProvider.IsSlow && (base.PageCount == 0 || num != FileSize) && (((options & RefreshInfoOptions.GetFastPageCount) != 0 && (imageProvider.Capabilities & ImageProviderCapabilities.FastPageInfo) != 0) || (options & RefreshInfoOptions.GetPageCount) != 0))
				{
					try
					{
						imageProvider.Open(async: false);
						if (imageProvider.Count > 0)
						{
							base.PageCount = imageProvider.Count;
						}
					}
					catch
					{
					}
				}
			}
			FileInfoRetrieved = true;
		}

		public void RefreshInfoFromFile()
		{
			RefreshInfoFromFile(RefreshInfoOptions.GetFastPageCount);
		}

		public string CreateFileHash()
		{
			using (ImageProvider imageProvider = CreateImageProvider())
			{
				imageProvider.Open(async: false);
				return imageProvider.CreateHash();
			}
		}

		public void RefreshFileProperties()
		{
			if (!EditMode.IsLocalComic())
			{
				return;
			}
			try
			{
				FileInfo fileInfo = new FileInfo(FilePath);
				FileIsMissing = !fileInfo.Exists;
				if (fileInfo.Exists)
				{
					bool flag = fileSize != fileInfo.Length;
					bool flag2 = fileModifiedTime != fileInfo.LastWriteTimeUtc;
					fileSize = fileInfo.Length;
					fileModifiedTime = fileInfo.LastWriteTimeUtc;
					if (flag)
					{
						FireBookChanged("FileSize");
					}
					if (flag2)
					{
						FireBookChanged("FileModifiedTime");
					}
					FileCreationTime = fileInfo.CreationTimeUtc;
				}
			}
			catch
			{
				FileIsMissing = true;
			}
		}

		private bool SetProperty<T>(string name, ref T property, T value, bool lockItem = false, bool addUndo = true)
		{
			if (object.Equals(property, value))
			{
				return false;
			}
			T val = property;
			using (lockItem ? ItemMonitor.Lock(this) : null)
			{
				property = value;
			}
			if (addUndo)
			{
				FireBookChanged(name, val, value);
			}
			else
			{
				FireBookChanged(name);
			}
			return true;
		}

		private void FireBookChanged(string name)
		{
			OnBookChanged(new BookChangedEventArgs(name, isComicInfo: false));
		}

		private void FireBookChanged(string name, object oldValue, object newValue)
		{
			OnBookChanged(new BookChangedEventArgs(name, isComicInfo: false, oldValue, newValue));
		}

		private void UpdateProposed()
		{
			if (proposed == null)
			{
				OnParseFilePath();
			}
		}

		protected virtual void OnFileRenamed(ComicBookFileRenameEventArgs e)
		{
			if (this.FileRenamed != null)
			{
				this.FileRenamed(this, e);
			}
		}

		protected virtual void OnCreateComicProvider(CreateComicProviderEventArgs cpea)
		{
			if (this.CreateComicProvider != null)
			{
				this.CreateComicProvider(this, cpea);
			}
		}

		protected virtual void OnComicProviderCreated(CreateComicProviderEventArgs cpea)
		{
			if (this.ComicProviderCreated != null)
			{
				this.ComicProviderCreated(this, cpea);
			}
		}

		protected override void OnBookChanged(BookChangedEventArgs e)
		{
			base.OnBookChanged(e);
			if (e.PropertyName == "FilePath" || e.PropertyName == "Number" || e.PropertyName == "EnableProposed")
			{
				compareNumber = null;
			}
			else if (e.PropertyName == "AlternateNumber")
			{
				compareAlternateNumber = null;
			}
			ClearVirtualTagsCache();
		}

		public override void SetInfo(ComicInfo ci, bool onlyUpdateEmpty = true, bool updatePages = true)
		{
			base.SetInfo(ci, onlyUpdateEmpty, updatePages);
			LastPageRead = Math.Min(base.PageCount - 1, LastPageRead);
			CurrentPage = Math.Min(base.PageCount - 1, CurrentPage);
		}

		protected override ComicPageInfo OnNewComicPageAdded(ComicPageInfo info)
		{
			if (proposed != null && info.ImageIndex < proposed.CoverCount)
			{
				info.PageType = ComicPageType.FrontCover;
			}
			return info;
		}

		public static ComicBook DeserializeFull(Stream stream)
		{
			return XmlUtility.Load<ComicBook>(stream, compressed: false);
		}

		public static ComicBook DeserializeFull(string file)
		{
			return XmlUtility.Load<ComicBook>(file, compressed: false);
		}

		public void SerializeFull(Stream stream)
		{
			XmlUtility.Store(stream, this, compressed: false);
		}

		public void SerializeFull(string file)
		{
			XmlUtility.Store(file, this, compressed: false);
		}

		public static string FormatPages(int pages)
		{
			if (pages <= 0)
			{
				return unkownText.Value;
			}
			return StringUtility.Format(pagesText.Value, pages);
		}

		public static string FormatRating(float rating)
		{
			if (!(rating <= 0f))
			{
				return rating.ToString("0.0");
			}
			return noneText.Value;
		}

		public static string FormatYear(int year)
		{
			if (year != -1)
			{
				return year.ToString();
			}
			return string.Empty;
		}

		public static string FormatNumber(string number, int count)
		{
			if (string.IsNullOrEmpty(number))
			{
				return string.Empty;
			}
			string text = ((number == "-") ? string.Empty : number);
			if (count >= 0)
			{
				text += StringUtility.Format(" ({0} {1})", ofText.Value, count);
			}
			return text;
		}

		public static string FormatVolume(int volume)
		{
			if (volume != -1)
			{
				return StringUtility.Format(volumeFormat.Value, volume);
			}
			return string.Empty;
		}

		public static string FormatTitle(string textFormat, string series, string title = null, string volumeText = null, string numberText = null, string yearText = null, string monthText = null, string dayText = null, string format = null, string fileName = null)
		{
			if (!string.IsNullOrEmpty(series))
			{
				try
				{
					return ExtendedStringFormater.Format(textFormat, delegate (string s)
					{
						switch (s)
						{
							case "filename":
								return fileName;
							case "series":
								return series;
							case "title":
								return title;
							case "volume":
								return volumeText;
							case "number":
								return numberText;
							case "year":
								return yearText;
							case "month":
								return monthText;
							case "day":
								return dayText;
							case "format":
								if (!series.Contains(format, StringComparison.OrdinalIgnoreCase))
								{
									return format;
								}
								return string.Empty;
							default:
								return null;
						}
					}).Trim();
				}
				catch
				{
				}
			}
			return fileName ?? string.Empty;
		}

		private static void AppendUniqueName(StringBuilder s, string delimiter, string text, HashSet<string> uniqueNames)
		{
			if (!string.IsNullOrEmpty(text))
			{
				var names = text.Split(',', StringSplitOptions.RemoveEmptyEntries);
				foreach (var name in names)
				{
					var trimmedName = name.Trim();
					if (!string.IsNullOrEmpty(trimmedName) && uniqueNames.Add(trimmedName))
					{
						if (s.Length != 0)
						{
							s.Append(delimiter);
						}
						s.Append(trimmedName);
					}
				}
			}
		}

		public static string FormatDate(DateTime date, ComicDateFormat dateFormat = ComicDateFormat.Long, bool toLocal = false, string missingText = null)
		{
			if (date == DateTime.MinValue)
			{
				return missingText ?? neverText.Value;
			}
			if (toLocal)
			{
				date = date.ToLocalTime();
			}
			switch (dateFormat)
			{
				default:
					if (!date.IsDateOnly())
					{
						return date.ToString();
					}
					return date.ToShortDateString();
				case ComicDateFormat.Short:
					return date.ToShortDateString();
				case ComicDateFormat.Relative:
					return date.ToRelativeDateString(DateTime.Now);
			}
		}

		public static string FormatFileDate(DateTime date, ComicDateFormat dateFormat = ComicDateFormat.Long)
		{
			return FormatDate(date, dateFormat, toLocal: true, notFoundText.Value);
		}

		public static string GetLanguageName(string iso)
		{
			try
			{
				return GetIsoCulture(iso).DisplayName;
			}
			catch
			{
				return string.Empty;
			}
		}

		public static CultureInfo GetIsoCulture(string iso)
		{
			iso = iso.ToLower();
			if (languages == null)
			{
				languages = new Dictionary<string, CultureInfo>();
			}
			new Dictionary<string, CultureInfo>();
			if (languages.TryGetValue(iso, out var value))
			{
				return value;
			}
			CultureInfo cultureInfo = CultureInfo.GetCultures(CultureTypes.NeutralCultures).FirstOrDefault((CultureInfo info) => info.TwoLetterISOLanguageName == iso);
			if (cultureInfo != null)
			{
				return languages[iso] = cultureInfo;
			}
			return new CultureInfo(string.Empty);
		}

		public static IEnumerable<string> GetProperties(bool onlyWritable, Type t = null)
		{
			return from pi in typeof(ComicBook).GetProperties(BindingFlags.Instance | BindingFlags.Public)
				   where pi.CanRead && (pi.CanWrite || !onlyWritable) && (t == null || pi.PropertyType == t) && pi.Browsable(forced: true)
				   select pi.Name;
		}

		public static IEnumerable<string> GetWritableStringProperties()
		{
			return GetProperties(onlyWritable: true, typeof(string));
		}

		public static IDictionary<string, string> GetTranslatedWritableStringProperties()
		{
			TR tr = TR.Load("Columns");
			return GetWritableStringProperties().ToDictionary((string s) => tr[s].PascalToSpaced());
		}

		public static bool MapPropertyName(string propName, out string newName, ComicValueType cvt)
		{
			string text = propName.ToLower();
			if (text == "cover" || text == "rating")
			{
				propName += "AsText";
			}
			if (cvt != 0)
			{
				switch (text)
				{
					case "series":
					case "title":
					case "format":
					case "count":
					case "year":
					case "number":
					case "yearastext":
					case "numberastext":
					case "volumeastext":
					case "countastext":
						newName = ((cvt == ComicValueType.Proposed) ? "Proposed" : "Shadow") + propName;
						return true;
				}
			}
			newName = propName;
			return false;
		}

		public static bool MapPropertyNameToAsText(string propName, out string newName)
		{
			if (hasAsText.TryGetValue(propName, out newName))
			{
				return true;
			}
			string text = propName + "AsText";
			if (typeof(ComicBook).GetProperty(text, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public) != null)
			{
				propName = text;
			}
			hasAsText[propName] = propName;
			newName = propName;
			return true;
		}

		public static bool IsDefaultPropertyValue(object value)
		{
			if (value == null)
			{
				return true;
			}
			if (value is string)
			{
				return string.IsNullOrEmpty((string)value);
			}
			if (value is int || value is double || value is float)
			{
				return (int)value == -1;
			}
			if (value is DateTime)
			{
				return (DateTime)value == DateTime.MinValue;
			}
			return false;
		}

		protected virtual void OnParseFilePath()
		{
			ParseFilePathEventArgs parseFilePathEventArgs = new ParseFilePathEventArgs(FilePath);
			if (ComicBook.ParseFilePath != null)
			{
				ComicBook.ParseFilePath(this, parseFilePathEventArgs);
			}
			proposed = parseFilePathEventArgs.NameInfo;
		}

		public object Clone()
		{
			return new ComicBook(this);
		}
	}
}
