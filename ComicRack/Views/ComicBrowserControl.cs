using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using cYo.Common;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Runtime;
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Common.Win32;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Controls;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.Drawing;
using cYo.Projects.ComicRack.Engine.IO;
using cYo.Projects.ComicRack.Plugins;
using cYo.Projects.ComicRack.Viewer.Config;
using cYo.Projects.ComicRack.Viewer.Controls;
using cYo.Projects.ComicRack.Viewer.Dialogs;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public partial class ComicBrowserControl : SubView, IComicBrowser, IGetBookList, IRefreshDisplay, ISearchOptions, IDisplayWorkspace, IItemSize, ISettingsChanged
	{
		private class StackMatcher : ComicBookMatcher
		{
			private readonly HashSet<ComicBook> items;

			public string Caption
			{
				get;
				private set;
			}

			public IGrouper<IViewableItem> Grouper
			{
				get;
				private set;
			}

			public StackMatcher(IGrouper<IViewableItem> grouper, string caption, HashSet<ComicBook> items)
			{
				this.items = items;
				Grouper = grouper;
				Caption = caption;
			}

			public bool Match(ComicBook item)
			{
				return items.Contains(item);
			}

			public override IEnumerable<ComicBook> Match(IEnumerable<ComicBook> items)
			{
				return items.Where(Match);
			}

			public override object Clone()
			{
				return new StackMatcher(Grouper, Caption, items);
			}
		}

		private class CoverViewItemPropertyComparer : CoverViewItemComparer
		{
			private readonly string property;

			public CoverViewItemPropertyComparer(string property)
			{
				this.property = property;
			}

			protected override int OnCompare(CoverViewItem x, CoverViewItem y)
			{
				return ExtendedStringComparer.Compare(x.Comic.GetStringPropertyValue(property), y.Comic.GetStringPropertyValue(property));
			}
		}

		private class CoverViewItemPropertyGrouper : IGrouper<IViewableItem>
		{
			private readonly string property;

			public bool IsMultiGroup => false;

			public CoverViewItemPropertyGrouper(string property)
			{
				this.property = property;
			}

			public IEnumerable<IGroupInfo> GetGroups(IViewableItem item)
			{
				throw new NotImplementedException();
			}

			public IGroupInfo GetGroup(IViewableItem item)
			{
				CoverViewItem coverViewItem = (CoverViewItem)item;
				return SingleComicGrouper.GetNameGroup(coverViewItem.Comic.GetStringPropertyValue(property));
			}
		}

		private readonly string noneText;

		private readonly string arrangedByText;

		private readonly string notArrangedText;

		private readonly string groupedByText;

		private readonly string notGroupedText;

		private readonly string stackedByText;

		private readonly string notStackedText;

		private readonly string eComicText;

		private readonly string eComicsText;

		private readonly string selectedText;

		private readonly string filteredText;

		private readonly string customFieldDescription;

		private volatile bool bookListDirty;

		private volatile bool updateGroupList;

		private volatile int newGroupListWidth;

		private volatile int totalCount;

		private long totalSize;

		private long selectedSize;

		private readonly CommandMapper commands = new CommandMapper();

		private readonly Image groupUp = Resources.GroupUp;

		private readonly Image groupDown = Resources.GroupDown;

		private readonly Image sortUp = Resources.SortUp;

		private readonly Image sortDown = Resources.SortDown;

		private ToolStripMenuItem miCopyListSetup;

		private ToolStripMenuItem miPasteListSetup;

		private ComicBookMatcher quickFilter;

		private ComicBookMatcher stackFilter;

		private IViewableItem stackItem;

		private StacksConfig stacksConfig;

		private ItemViewConfig preStackConfig;

		private Point preStackScrollPosition;

		private Guid preStackFocusedId;

		private string currentStackName;

		private IComicBookListProvider bookList;

		private string quickSearch;

		private ComicBookAllPropertiesMatcher.MatcherOption quickSearchType;

		private ComicBookAllPropertiesMatcher.ShowOptionType showOptionType;

		private ComicBookAllPropertiesMatcher.ShowComicType showComicType;

		private bool showOnlyDuplicates;

		private bool showGroupHeaders;

		private ComicsEditModes comicEditMode = ComicsEditModes.Default;

		private ThumbnailConfig thumbnailConfig;

		private Image listBackgroundImage;

		private IGrouper<IViewableItem> oldStacker;

		private string[] quickSearchCueTexts;

		private Point contextMenuMouseLocation;

		private long contextMenuCloseTime;

		private int oldQuickWidth;

		private int savedOptimizeToolstripRight;

		private int savedOptimizeToolstripWitdh;

		private string backgroundImageSource;

		private IBitmapCursor dragCursor;

		private bool ownDrop;

		private DragDropContainer dragBookContainer;

		private readonly ManualResetEvent abortBuildMenu = new ManualResetEvent(initialState: false);

		private Thread buildMenuThread;

		private bool blockQuickSearchUpdate;

		private CoverViewItem toolTipItem;

		private bool searchBrowserVisible;

		private ComicLibrary library;


		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IComicBookListProvider BookList
		{
			get
			{
				return bookList;
			}
			set
			{
				if (bookList == value)
				{
					return;
				}
				try
				{
					itemView.BeginUpdate();
					UnregisterBookList();
					bookList = value;
					RegisterBookList();
					OnCurrentBookListChanged();
					if (base.Main != null)
					{
						UpdatePending();
					}
				}
				finally
				{
					itemView.EndUpdate();
					if (bookList != null)
					{
						IDisplayListConfig displayListConfig = bookList.QueryService<IDisplayListConfig>();
						if (displayListConfig != null && displayListConfig.Display != null)
						{
							itemView.ScrollPosition = displayListConfig.Display.ScrollPosition;
							SetFocusedItem(displayListConfig.Display.FocusedComicId);
							Guid id = displayListConfig.Display.StackedComicId;
							if (id != Guid.Empty)
							{
								CoverViewItem coverViewItem = itemView.Items.OfType<CoverViewItem>().FirstOrDefault((CoverViewItem i) => i.Comic.Id == id);
								if (coverViewItem != null)
								{
									itemView.Select(coverViewItem);
									OpenStack(coverViewItem);
									itemView.ScrollPosition = displayListConfig.Display.StackScrollPosition;
									itemView.SelectAll(selectionState: false);
									SetFocusedItem(displayListConfig.Display.StackFocusedComicId);
								}
							}
						}
					}
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ItemViewConfig ViewConfig
		{
			get
			{
				return itemView.ViewConfig;
			}
			set
			{
				itemView.ViewConfig = value;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ItemViewMode ItemViewMode
		{
			get
			{
				return itemView.ItemViewMode;
			}
			set
			{
				itemView.ItemViewMode = value;
			}
		}

		[DefaultValue(null)]
		public string QuickSearch
		{
			get
			{
				return quickSearch;
			}
			set
			{
				if (!(quickSearch == value))
				{
					quickSearch = value;
					OnQuickSearchChanged();
				}
			}
		}

		public ItemView ItemView => itemView;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ComicBookAllPropertiesMatcher.MatcherOption QuickSearchType
		{
			get
			{
				return quickSearchType;
			}
			set
			{
				if (quickSearchType != value)
				{
					quickSearchType = value;
					UpdateSearch();
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ComicBookAllPropertiesMatcher.ShowOptionType ShowOptionType
		{
			get
			{
				return showOptionType;
			}
			set
			{
				if (showOptionType != value)
				{
					showOptionType = value;
					UpdateSearch();
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ComicBookAllPropertiesMatcher.ShowComicType ShowComicType
		{
			get
			{
				return showComicType;
			}
			set
			{
				if (showComicType != value)
				{
					showComicType = value;
					UpdateSearch();
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(false)]
		public bool ShowOnlyDuplicates
		{
			get
			{
				return showOnlyDuplicates;
			}
			set
			{
				if (showOnlyDuplicates != value)
				{
					showOnlyDuplicates = value;
					UpdateSearch();
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(false)]
		public bool ShowGroupHeaders
		{
			get
			{
				return showGroupHeaders;
			}
			set
			{
				if (showGroupHeaders != value)
				{
					showGroupHeaders = value;
					updateGroupList = true;
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ComicsEditModes ComicEditMode
		{
			get
			{
				return comicEditMode;
			}
			set
			{
				if (comicEditMode != value)
				{
					comicEditMode = value;
					OnComicEditModeChanged();
				}
			}
		}

		[DefaultValue(false)]
		public bool DisableViewConfigUpdate
		{
			get;
			set;
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public int SearchBrowserColumn1
		{
			get
			{
				return bookSelectorPanel.Column1;
			}
			set
			{
				bookSelectorPanel.Column1 = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public int SearchBrowserColumn2
		{
			get
			{
				return bookSelectorPanel.Column2;
			}
			set
			{
				bookSelectorPanel.Column2 = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public int SearchBrowserColumn3
		{
			get
			{
				return bookSelectorPanel.Column3;
			}
			set
			{
				bookSelectorPanel.Column3 = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ThumbnailConfig ThumbnailConfig
		{
			get
			{
				return thumbnailConfig;
			}
			set
			{
				if (value != null)
				{
					thumbnailConfig = value;
				}
			}
		}

		[DefaultValue(null)]
		public Image ListBackgroundImage
		{
			get
			{
				return listBackgroundImage;
			}
			set
			{
				listBackgroundImage = value;
				if (itemView.BackgroundImage == null)
				{
					SetListBackgroundImage();
				}
			}
		}

		[DefaultValue(false)]
		public bool HideNavigation
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool SearchBrowserVisible
		{
			get
			{
				return searchBrowserVisible;
			}
			set
			{
				if (searchBrowserVisible != value)
				{
					searchBrowserVisible = value;
					OnSearchBrowserVisibleChanged();
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ComicLibrary Library
		{
			get
			{
				return library;
			}
			set
			{
				if (library != value)
				{
					library = value;
					library.CustomValuesChanged += delegate
					{
						SetCustomColumns();
					};
					SetCustomColumns();
				}
			}
		}

		public string SelectionInfo
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				int count = itemView.Items.Count;
				int selectedCount = itemView.SelectedCount;
				IComicBookListProvider comicBookListProvider = BookList;
				if (comicBookListProvider != null && !string.IsNullOrEmpty(comicBookListProvider.Name))
				{
					stringBuilder.AppendFormat(comicBookListProvider.Name);
					stringBuilder.AppendFormat(": ");
				}
				stringBuilder.AppendFormat((count == 1) ? eComicText : eComicsText, count);
				if (totalCount != count)
				{
					stringBuilder.Append(" (");
					try
					{
						stringBuilder.AppendFormat(filteredText, totalCount - count);
					}
					catch
					{
					}
					stringBuilder.Append(")");
				}
				if (totalSize != 0L)
				{
					stringBuilder.Append(" / ");
					stringBuilder.AppendFormat(string.Format(new FileLengthFormat(), "{0}", new object[1]
					{
						totalSize
					}));
				}
				if (selectedCount != 0)
				{
					stringBuilder.Append(" - ");
					bool flag = true;
					if (selectedCount == 1)
					{
						ComicBook comicBook = GetBookList(ComicBookFilterType.IsLocal | ComicBookFilterType.IsNotFileless | ComicBookFilterType.Selected).FirstOrDefault();
						if (comicBook != null)
						{
							stringBuilder.Append(comicBook.FilePath);
							flag = false;
						}
					}
					if (flag)
					{
						stringBuilder.AppendFormat(selectedText, selectedCount);
					}
				}
				if (selectedSize != 0L)
				{
					stringBuilder.Append(" / ");
					stringBuilder.AppendFormat(string.Format(new FileLengthFormat(), "{0}", new object[1]
					{
						selectedSize
					}));
				}
				return stringBuilder.ToString();
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DisplayListConfig ListConfig
		{
			get
			{
				return new DisplayListConfig
				{
					View = ViewConfig,
					Thumbnail = new ThumbnailConfig(ThumbnailConfig)
				};
			}
			set
			{
				ViewConfig = value.View;
				ThumbnailConfig = new ThumbnailConfig(value.Thumbnail);
				FillBookList();
			}
		}

		public ToolStripItemCollection ListConfigMenu => tsListLayouts.DropDownItems;

		public event EventHandler CurrentBookListChanged;

		public event EventHandler SearchBrowserVisibleChanged;

		public event EventHandler QuickSearchChanged;

		public ComicBrowserControl()
		{
			InitializeComponent();
			searchBrowserContainer.Collapsed = true;
			itemView.ScrollResizeRefresh = Program.ExtendedSettings.OptimizedListScrolling;
			toolTip.OwnerDraw = true;
			toolTip.Draw += toolTip_Draw;
			ListStyles.SetOwnerDrawn(lvGroupHeaders);
			KeySearch.Create(lvGroupHeaders);
			browserContainer.Panel2Collapsed = true;
			LocalizeUtility.Localize(this, components);
			noneText = TR.Load(base.Name)["None", "None"];
			arrangedByText = TR.Load(base.Name)["ArrangedBy", "Arranged by {0}"];
			notArrangedText = TR.Load(base.Name)["NotArranged", "Not sorted"];
			groupedByText = TR.Load(base.Name)["GroupedBy", "Grouped by {0}"];
			notGroupedText = TR.Load(base.Name)["NotGrouped", "Not grouped"];
			stackedByText = TR.Load(base.Name)["StackedBy", "Stacked by {0}"];
			notStackedText = TR.Load(base.Name)["NotStacked", "Not stacked"];
			eComicText = TR.Load(base.Name)["ComicSingle", "{0} Book"];
			eComicsText = TR.Load(base.Name)["ComicMulti", "{0} Books"];
			selectedText = TR.Load(base.Name)["ComicSelected", "{0} selected"];
			filteredText = TR.Load(base.Name)["ComicFiltered", "{0} filtered"];
			customFieldDescription = TR.Load(base.Name)["CustomFieldDescription", "Shows the value of the custom field '{0}'"];
			contextRating.Renderer = new MenuRenderer(Resources.StarYellow);
			contextRating.Items.Insert(contextRating.Items.Count - 2, new ToolStripSeparator());
			RatingControl.InsertRatingControl(contextRating, contextRating.Items.Count - 2, Resources.StarYellow, () => base.Main.GetRatingEditor());
			FormUtility.EnableRightClickSplitButtons(toolStrip.Items);
			string[] strings = TR.Load("Columns").GetStrings("DateFormats", "Long|Short|Relative", '|');
			itemView.Columns.Add(new ItemViewColumn(101, "State", 60, new ComicListField("State", "State indicators for the Book"), new CoverViewItemBookComparer<ComicBookStatusComparer>()));
			itemView.Columns.Add(new ItemViewColumn(100, "Position", 30, new ComicListField("Position", "Position of the Book in the list"), new CoverViewItemPositionComparer(), null, visible: true, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(102, "Checked", 22, new ComicListField("Checked", "Book is checked"), new CoverViewItemBookComparer<ComicBookCheckedComparer>(), new CoverViewItemBookGrouper<ComicBookGroupChecked>()));
			itemView.Columns.Add(new ItemViewColumn(0, "Cover", 40, new ComicListField("Cover", "Cover image of the Book"), null, null, visible: true, StringAlignment.Center)
			{
				Name = "Cover"
			});
			itemView.Columns.Add(new ItemViewColumn(1, "Series", 200, new ComicListField("Series", "Series name", "Series"), new CoverViewItemBookComparer<ComicBookSeriesComparer>(), new CoverViewItemBookGrouper<ComicBookGroupSeries>()));
			itemView.Columns.Add(new ItemViewColumn(2, "Number", 40, new ComicListField("NumberAsText", "Number of the Book in the Series", "Number"), new CoverViewItemBookComparer<ComicBookNumberComparer>(), new CoverViewItemBookGrouper<ComicBookGroupNumber>(), visible: true, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(3, "Volume", 40, new ComicListField("VolumeAsText", "Volume number of the Series", "Volume"), new CoverViewItemBookComparer<ComicBookVolumeComparer>(), new CoverViewItemBookGrouper<ComicBookGroupVolume>()));
			itemView.Columns.Add(new ItemViewColumn(4, "Title", 200, new ComicListField("Title", "Title of the Book", "Title"), new CoverViewItemBookComparer<ComicBookTitleComparer>(), new CoverViewItemBookGrouper<ComicBookGroupTitle>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(5, "Opened", 40, new ComicListField("OpenedTime", "Last time the Book was opened", null, StringTrimming.Word, typeof(DateTime)), new CoverViewItemBookComparer<ComicBookOpenedComparer>(), new CoverViewItemBookGrouper<ComicBookGroupOpened>(), visible: true, StringAlignment.Far, strings));
			itemView.Columns.Add(new ItemViewColumn(6, "Added", 40, new ComicListField("AddedTime", "Time the Book was added to the Database", null, StringTrimming.Word, typeof(DateTime)), new CoverViewItemBookComparer<ComicBookAddedComparer>(), new CoverViewItemBookGrouper<ComicBookGroupAdded>(), visible: true, StringAlignment.Far, strings));
			itemView.Columns.Add(new ItemViewColumn(7, "Pages", 40, new ComicListField("PagesAsTextSimple", "The total count of pages"), new CoverViewItemBookComparer<ComicBookPageCountComparer>(), new CoverViewItemBookGrouper<ComicBookGroupPages>(), visible: true, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(39, "Published", 40, new ComicListField("PublishedAsText", "Publication date of the Book"), new CoverViewItemBookComparer<ComicBookPublishedComparer>(), new CoverViewItemBookGrouper<ComicBookGroupPublished>(), visible: true, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(9, "File Path", 200, new ComicListField("FilePath", "Location where the Book is stored", null, StringTrimming.EllipsisPath), new CoverViewItemBookComparer<ComicBookFileComparer>(), null, visible: false));
			itemView.Columns.Add(new ItemViewColumn(10, "File Name", 200, new ComicListField("FileName", "Filename without the extension", "FileName"), new CoverViewItemBookComparer<ComicBookFileNameComparer>(), new CoverViewItemBookGrouper<ComicBookGroupFileName>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(11, "Writer", 80, new ComicListField("Writer", "Writer of the Book", "Writer"), new CoverViewItemBookComparer<ComicBookWriterComparer>(), new CoverViewItemBookGrouper<ComicBookGroupWriter>()));
			itemView.Columns.Add(new ItemViewColumn(12, "Penciller", 80, new ComicListField("Penciller", "Penciller of the Book", "Penciller"), new CoverViewItemBookComparer<ComicBookPencillerComparer>(), new CoverViewItemBookGrouper<ComicBookGroupPenciller>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(13, "Inker", 80, new ComicListField("Inker", "Inker of the Book", "Inker"), new CoverViewItemBookComparer<ComicBookInkerComparer>(), new CoverViewItemBookGrouper<ComicBookGroupInker>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(14, "Colorist", 80, new ComicListField("Colorist", "Colorist of the Book", "Colorist"), new CoverViewItemBookComparer<ComicBookColoristComparer>(), new CoverViewItemBookGrouper<ComicBookGroupColorist>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(15, "My Rating", 50, new ComicListField("Rating", "Your rating of the Book"), new CoverViewItemRatingComparer(), new CoverViewItemBookGrouper<ComicBookGroupRating>()));
			itemView.Columns.Add(new ItemViewColumn(16, "Opened Count", 40, new ComicListField("OpenedCountAsText", "Times the Book has been opened"), new CoverViewItemBookComparer<ComicBookOpenCountComparer>(), new CoverViewItemBookGrouper<ComicBookGroupOpenCount>(), visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(17, "Read Percentage", 40, new ComicListField("ReadPercentageAsText", "How much has been read"), new CoverViewItemReadPercentageComparer(), new CoverViewItemBookGrouper<ComicBookGroupReadPercentage>(), visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(18, "File Modified", 40, new ComicListField("FileModifiedTime", "Time the Book was modified the last time", null, StringTrimming.Word, typeof(DateTime)), new CoverViewItemBookComparer<ComicBookModifiedComparer>(), new CoverViewItemBookGrouper<ComicBookGroupModified>(), visible: false, StringAlignment.Far, strings));
			itemView.Columns.Add(new ItemViewColumn(19, "Genre", 40, new ComicListField("Genre", "Genre of the Book", "Genre"), new CoverViewItemBookComparer<ComicBookGenreComparer>(), new CoverViewItemBookGrouper<ComicBookGroupGenre>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(20, "Publisher", 40, new ComicListField("Publisher", "Publisher of the Book", "Publisher"), new CoverViewItemBookComparer<ComicBookPublisherComparer>(), new CoverViewItemBookGrouper<ComicBookGroupPublisher>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(21, "Count", 40, new ComicListField("CountAsText", "Total number of issues in this series", "Count"), new CoverViewItemBookComparer<ComicBookCountComparer>(), new CoverViewItemBookGrouper<ComicBookGroupCount>(), visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(22, "Letterer", 80, new ComicListField("Letterer", "Letterer of the Book", "Letterer"), new CoverViewItemBookComparer<ComicBookLettererComparer>(), new CoverViewItemBookGrouper<ComicBookGroupLetterer>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(23, "Cover Artist", 80, new ComicListField("CoverArtist", "The artist responsible for the cover", "CoverArtist"), new CoverViewItemBookComparer<ComicBookCoverArtistComparer>(), new CoverViewItemBookGrouper<ComicBookGroupCoverArtist>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(24, "Editor", 80, new ComicListField("Editor", "Editor of the Book", "Editor"), new CoverViewItemBookComparer<ComicBookEditorComparer>(), new CoverViewItemBookGrouper<ComicBookGroupEditor>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(72, "Translator", 80, new ComicListField("Translator", "Translator of the Book", "Translator"), new CoverViewItemBookComparer<ComicBookTranslatorComparer>(), new CoverViewItemBookGrouper<ComicBookGroupTranslator>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(25, "File Size", 40, new ComicListField("FileSizeAsText", "Size of the Book file in bytes"), new CoverViewItemBookComparer<ComicBookSizeComparer>(), new CoverViewItemBookGrouper<ComicBookGroupSize>(), visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(26, "Alternate Series", 200, new ComicListField("AlternateSeries", "Crossover/story name", "AlternateSeries"), new CoverViewItemBookComparer<ComicBookAlternateSeriesComparer>(), new CoverViewItemBookGrouper<ComicBookGroupAlternateSeries>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(27, "Alternate Number", 40, new ComicListField("AlternateNumberAsText", "Issue number in the crossover/story", "AlternateNumber"), new CoverViewItemBookComparer<ComicBookAlternateNumberComparer>(), new CoverViewItemBookGrouper<ComicBookGroupAlternateNumber>(), visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(28, "Alternate Count", 40, new ComicListField("AlternateCountAsText", "Total issues of the crossover/story", "AlternateCount"), new CoverViewItemBookComparer<ComicBookAlternateCountComparer>(), new CoverViewItemBookGrouper<ComicBookGroupAlternateCount>(), visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(29, "Month", 40, new ComicListField("MonthAsText", "Month the Book was published", "Month"), new CoverViewItemBookComparer<ComicBookMonthComparer>(), new CoverViewItemBookGrouper<ComicBookGroupMonth>(), visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(30, "Caption", 200, new ComicListField("Caption", "Complete caption of the Book", "Series"), new CoverViewItemBookComparer<ComicBookSeriesComparer>(), new CoverViewItemBookGrouper<ComicBookGroupSeries>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(31, "Tags", 60, new ComicListField("Tags", "Tags of the Book", "Tags"), new CoverViewItemBookComparer<ComicBookTagsComparer>(), null, visible: false));
			itemView.Columns.Add(new ItemViewColumn(32, "Imprint", 40, new ComicListField("Imprint", "Imprint of the Book", "Imprint"), new CoverViewItemBookComparer<ComicBookImprintComparer>(), new CoverViewItemBookGrouper<ComicBookGroupImprint>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(33, "Language", 40, new ComicListField("LanguageAsText", "Language of the Book"), new CoverViewItemBookComparer<ComicBookLanguageComparer>(), new CoverViewItemBookGrouper<ComicBookGroupLanguage>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(34, "Format", 40, new ComicListField("Format", "Format of the Book", "Format"), new CoverViewItemBookComparer<ComicBookFormatComparer>(), new CoverViewItemBookGrouper<ComicBookGroupFormat>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(35, "B&W", 22, new ComicListField("BlackAndWhiteAsText", "Yes if the Book is black and white"), new CoverViewItemBookComparer<ComicBookBlackAndWhiteComparer>(), new CoverViewItemBookGrouper<ComicBookGroupBlackAndWhite>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(36, "Manga", 22, new ComicListField("MangaAsText", "Yes if the Book is a Manga"), new CoverViewItemBookComparer<ComicBookMangaComparer>(), new CoverViewItemBookGrouper<ComicBookGroupManga>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(37, "File Format", 40, new ComicListField("FileFormat", "File format of the Book"), new CoverViewItemBookComparer<ComicBookFileFormatComparer>(), new CoverViewItemBookGrouper<ComicBookGroupFileFormat>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(38, "Age Rating", 40, new ComicListField("AgeRating", "Age rating of the Book"), new CoverViewItemBookComparer<ComicBookAgeRatingComparer>(), new CoverViewItemBookGrouper<ComicBookGroupAgeRating>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(8, "Year", 60, new ComicListField("YearAsText", "Year the Book was published", "Year"), new CoverViewItemBookComparer<ComicBookYearComparer>(), new CoverViewItemBookGrouper<ComicBookGroupYear>(), visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(40, "Characters", 60, new ComicListField("Characters", "Plot characters of the Books", "Characters"), new CoverViewItemBookComparer<ComicBookCharactersComparer>(), new CoverViewItemBookGrouper<ComicBookGroupCharacters>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(41, "File Directory", 60, new ComicListField("FileDirectory", "Directory of the Book"), new CoverViewItemBookComparer<ComicBookDirectoryComparer>(), new CoverViewItemBookGrouper<ComicBookGroupDirectory>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(42, "File Created", 60, new ComicListField("FileCreationTime", "Time the Book has been created", null, StringTrimming.Word, typeof(DateTime)), new CoverViewItemBookComparer<ComicBookCreationComparer>(), new CoverViewItemBookGrouper<ComicBookGroupCreation>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(43, "Bookmark Count", 60, new ComicListField("BookmarkCountAsText", "Count of Bookmarks for this Book"), new CoverViewItemBookComparer<ComicBookBookmarkCountComparer>(), null, visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(44, "New Pages", 60, new ComicListField("NewPagesAsText", "Count of new Pages"), new CoverViewItemBookComparer<ComicBookNewPagesComparer>(), null, visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(45, "Teams", 60, new ComicListField("Teams", "Plot teams of the Books", "Teams"), new CoverViewItemBookComparer<ComicBookTeamsComparer>(), new CoverViewItemBookGrouper<ComicBookGroupTeams>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(46, "Locations", 60, new ComicListField("Locations", "Plot locations of the Books", "Locations"), new CoverViewItemBookComparer<ComicBookLocationsComparer>(), new CoverViewItemBookGrouper<ComicBookGroupLocations>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(47, "Web", 60, new ComicListField("Web", "A Web Link", "Web"), new CoverViewItemBookComparer<ComicBookWebComparer>(), new CoverViewItemBookGrouper<ComicBookGroupWeb>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(48, "Community Rating", 50, new ComicListField("CommunityRating", "Rating of the Book"), new CoverViewItemCommunityRatingComparer(), new CoverViewItemBookGrouper<ComicBookGroupCommunityRating>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(49, "Linked", 50, new ComicListField("IsLinkedAsText", "Book is linked to a file"), new CoverViewItemBookComparer<ComicBookLinkedComparer>(), new CoverViewItemBookGrouper<ComicBookGroupLinked>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(50, "Book Price", 50, new ComicListField("BookPriceAsText", "Price of the Book", "BookPriceAsText"), new CoverViewItemBookComparer<ComicBookBookPriceComparer>(), new CoverViewItemBookGrouper<ComicBookGroupBookPrice>(), visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(51, "Book Age", 50, new ComicListField("BookAge", "Age of the Book", "BookAge"), new CoverViewItemBookComparer<ComicBookBookAgeComparer>(), new CoverViewItemBookGrouper<ComicBookGroupBookAge>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(52, "Book Store", 50, new ComicListField("BookStore", "Store the Book was bought", "BookStore"), new CoverViewItemBookComparer<ComicBookBookStoreComparer>(), new CoverViewItemBookGrouper<ComicBookGroupBookStore>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(53, "Book Owner", 50, new ComicListField("BookOwner", "Owner of the Book", "BookOwner"), new CoverViewItemBookComparer<ComicBookBookOwnerComparer>(), new CoverViewItemBookGrouper<ComicBookGroupBookOwner>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(54, "Book Condition", 50, new ComicListField("BookCondition", "Condition of the Book", "BookCondition"), new CoverViewItemBookComparer<ComicBookBookConditionComparer>(), new CoverViewItemBookGrouper<ComicBookGroupBookCondition>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(55, "Book Collection Status", 50, new ComicListField("BookCollectionStatus", "Status of the Book in the Collection", "BookCollectionStatus"), new CoverViewItemBookComparer<ComicBookBookCollectionStatusComparer>(), new CoverViewItemBookGrouper<ComicBookGroupBookCollectionStatus>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(56, "Book Location", 50, new ComicListField("BookLocation", "Location of the Book", "BookLocation"), new CoverViewItemBookComparer<ComicBookBookLocationComparer>(), new CoverViewItemBookGrouper<ComicBookGroupBookLocation>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(57, "ISBN", 50, new ComicListField("ISBN", "ISBN Number", "ISBN"), new CoverViewItemBookComparer<ComicBookISBNComparer>(), null, visible: false));
			itemView.Columns.Add(new ItemViewColumn(58, "Series complete", 22, new ComicListField("SeriesCompleteAsText", "Series is complete"), new CoverViewItemBookComparer<ComicBookSeriesCompleteComparer>(), new CoverViewItemBookGrouper<ComicBookGroupSeriesComplete>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(59, "Proposed Values", 22, new ComicListField("EnableProposedAsText", "Uses proposed Values from Filename"), new CoverViewItemBookComparer<ComicBookSeriesEnableProposedComparer>(), new CoverViewItemBookGrouper<ComicBookGroupEnableProposed>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(60, "Gap Information", 16, new ComicListField("GapInformation", "Information if this issue is the end or start of a gap in the series"), new CoverViewItemBookComparer<ComicBookSeriesComparer>(), null, visible: false));
			itemView.Columns.Add(new ItemViewColumn(61, "Read", 22, new ComicListField("HasBeenReadAsText", "Book has been read"), new CoverViewItemBookComparer<ComicBookHasBeenReadComparer>(), new CoverViewItemBookGrouper<ComicBookGroupHasBeenRead>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(62, "Icons", 100, new ComicListField("Icons", "Icons for this Book"), null, null, visible: false));
			itemView.Columns.Add(new ItemViewColumn(63, "Scan Information", 100, new ComicListField("ScanInformation", "Information about the Scan", "ScanInformation"), new CoverViewItemBookComparer<ComicBookScanInformationComparer>(), new CoverViewItemBookGrouper<ComicBookGroupScanInformation>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(64, "Story Arc", 100, new ComicListField("StoryArc", "Story Arc the book is part of", "StoryArc"), new CoverViewItemBookComparer<ComicBookStoryArcComparer>(), new CoverViewItemBookGrouper<ComicBookGroupStoryArc>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(65, "Series Group", 100, new ComicListField("SeriesGroup", "Series Group the Book is part of", "SeriesGroup"), new CoverViewItemBookComparer<ComicBookSeriesGroupComparer>(), new CoverViewItemBookGrouper<ComicBookGroupSeriesGroup>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(66, "Main Character/Team", 100, new ComicListField("MainCharacterOrTeam", "Main Character or Team of the Book", "MainCharacterOrTeam"), new CoverViewItemBookComparer<ComicBookMainCharacterOrTeamComparer>(), new CoverViewItemBookGrouper<ComicBookGroupMainCharacterOrTeam>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(67, "Review", 100, new ComicListField("Review", "A short Review of the Book"), new CoverViewItemBookComparer<ComicBookReviewComparer>(), null, visible: false));
			itemView.Columns.Add(new ItemViewColumn(68, "Day", 40, new ComicListField("DayAsText", "Day the Book was published", "Day"), new CoverViewItemBookComparer<ComicBookDayComparer>(), new CoverViewItemBookGrouper<ComicBookGroupDay>(), visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(69, "Week", 40, new ComicListField("WeekAsText", "Week the Book was published"), new CoverViewItemBookComparer<ComicBookWeekComparer>(), new CoverViewItemBookGrouper<ComicBookGroupWeek>(), visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(70, "Released", 60, new ComicListField("ReleasedTime", "Time the Book was released", null, StringTrimming.Word, typeof(DateTime), ComicBook.TR["Unknown"]), new CoverViewItemBookComparer<ComicBookReleasedComparer>(), new CoverViewItemBookGrouper<ComicBookGroupReleased>(), visible: false, StringAlignment.Far, strings));
			itemView.Columns.Add(new ItemViewColumn(71, "Published (Regional)", 40, new ComicListField("PublishedRegional", "Publication date of the Book (regional formating)"), new CoverViewItemBookComparer<ComicBookPublishedComparer>(), new CoverViewItemBookGrouper<ComicBookGroupPublished>(), visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(200, "Series: Books", 50, new ComicListField("SeriesStatCountAsText", "Books in the Series"), new CoverViewItemStatsComparer<ComicBookSeriesStatsCountComparer>(), new CoverViewItemStatsGrouper<ComicBookStatsGroupCount>(), visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(201, "Series: Pages", 50, new ComicListField("SeriesStatPageCountAsText", "Pages in the Series"), new CoverViewItemStatsComparer<ComicBookSeriesStatsPageCountComparer>(), new CoverViewItemStatsGrouper<ComicBookStatsGroupPageCount>(), visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(202, "Series: Pages Read", 50, new ComicListField("SeriesStatPageReadCountAsText", "Pages of the Series read"), new CoverViewItemStatsComparer<ComicBookSeriesStatsPageReadCountComparer>(), new CoverViewItemStatsGrouper<ComicBookStatsGroupPageReadCount>(), visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(203, "Series: Percent Read", 50, new ComicListField("SeriesStatReadPercentageAsText", "Percentage of the Series read"), new CoverViewItemStatsComparer<ComicBookSeriesStatsReadPercentageComparer>(), new CoverViewItemStatsGrouper<ComicBookStatsGroupReadPercentage>(), visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(204, "Series: First Number", 50, new ComicListField("SeriesStatMinNumberAsText", "First Number of the Series"), new CoverViewItemStatsComparer<ComicBookSeriesStatsMinNumberComparer>(), null, visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(205, "Series: Last Number", 50, new ComicListField("SeriesStatMaxNumberAsText", "Last Number of the Series"), new CoverViewItemStatsComparer<ComicBookSeriesStatsMaxNumberComparer>(), null, visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(206, "Series: First Year", 50, new ComicListField("SeriesStatMinYearAsText", "First Year of the Series"), new CoverViewItemStatsComparer<ComicBookSeriesStatsMinYearComparer>(), new CoverViewItemStatsGrouper<ComicBookStatsGroupMinYear>(), visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(207, "Series: Last Year", 50, new ComicListField("SeriesStatMaxYearAsText", "Last Year of the Series"), new CoverViewItemStatsComparer<ComicBookSeriesStatsMaxYearComparer>(), new CoverViewItemStatsGrouper<ComicBookStatsGroupMaxYear>(), visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(208, "Series: Average Rating", 50, new ComicListField("SeriesStatAverageRating", "Average Rating of the Series"), new CoverViewItemStatsComparer<ComicBookSeriesStatsAverageRatingComparer>(), new CoverViewItemStatsGrouper<ComicBookStatsGroupAverageRating>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(209, "Series: Average Community Rating", 50, new ComicListField("SeriesStatAverageCommunityRating", "Average Community Rating of the Series"), new CoverViewItemStatsComparer<ComicBookSeriesStatsAverageCommunityRatingComparer>(), new CoverViewItemStatsGrouper<ComicBookStatsGroupAverageCommunityRating>(), visible: false));
			itemView.Columns.Add(new ItemViewColumn(210, "Series: Gaps", 50, new ComicListField("SeriesStatGapCountAsText", "Count of Gaps in the Series"), new CoverViewItemStatsComparer<ComicBookSeriesStatsGapCountComparer>(), new CoverViewItemStatsGrouper<ComicBookStatsGroupGapCount>(), visible: false, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(211, "Series: Book added", 50, new ComicListField("SeriesStatLastAddedTime", "Last time a book was added to the Series", null, StringTrimming.Word, typeof(DateTime)), new CoverViewItemStatsComparer<ComicBookSeriesStatsLastAddedTimeComparer>(), new CoverViewItemStatsGrouper<ComicBookStatsGroupLastAddedTime>(), visible: false, StringAlignment.Far, strings));
			itemView.Columns.Add(new ItemViewColumn(212, "Series: Opened", 50, new ComicListField("SeriesStatLastOpenedTime", "Last time a book was opened from the Series", null, StringTrimming.Word, typeof(DateTime)), new CoverViewItemStatsComparer<ComicBookSeriesStatsLastOpenedTimeComparer>(), new CoverViewItemStatsGrouper<ComicBookStatsGroupLastOpenedTime>(), visible: false, StringAlignment.Far, strings));
			itemView.Columns.Add(new ItemViewColumn(213, "Series: Book released", 50, new ComicListField("SeriesStatLastReleasedTime", "Last time a book of this Series was released", null, StringTrimming.Word, typeof(DateTime)), new CoverViewItemStatsComparer<ComicBookSeriesStatsLastReleasedTimeComparer>(), new CoverViewItemStatsGrouper<ComicBookStatsGroupLastReleasedTime>(), visible: false, StringAlignment.Far, strings));
			itemView.Columns.Add(new ItemViewColumn(214, "Actual File Format (slow)", 40, new ComicListField("ActualFileFormat", "Actual File format of the Book (based on the header)"), new CoverViewItemBookComparer<ComicBookActualFileFormatComparer>(), new CoverViewItemBookGrouper<ComicBookGroupActualFileFormat>(), visible: false));
			SetVirtualTagColumns();
			SubView.TranslateColumns(itemView.Columns);
			foreach (ItemViewColumn column in itemView.Columns)
			{
				column.TooltipText = ((ComicListField)column.Tag).Description;
				column.Width = FormUtility.ScaleDpiX(column.Width);
			}
			ThumbnailConfig = new ThumbnailConfig();
			ThumbnailConfig.CaptionIds.Add(30);
			tsQuickSearch.TextBox.SearchButtonImage = Resources.Search.ScaleDpi();
			tsQuickSearch.TextBox.ClearButtonImage = Resources.SmallCloseGray.ScaleDpi();
			itemView.Font = SystemFonts.IconTitleFont;
			itemView.ItemContextMenuStrip = contextMenuItems;
			itemView.ItemRowHeight = itemView.Font.Height + FormUtility.ScaleDpiY(6);
			itemView.ColumnHeaderHeight = itemView.ItemRowHeight;
			itemView.Items.Changed += ComicItemAdded;
			itemView.MouseWheel += itemView_MouseWheel;
			itemView.StackColumnSorter = GetStackColumnSorter;
			miPasteData.Click += new EventHandler((sender, e) => PasteComicData());
			KeySearch.Create(itemView);
		}

		private static readonly IComparer<IViewableItem> defaultSeriesComparer = new CoverViewItemBookComparer<ComicBookSeriesComparer>();
		/// <summary>
		/// Callback function that will sort the items in a stack based on the stack configuration.
		/// </summary>
		/// <param name="stackCaption"></param>
		/// <returns></returns>
		private IComparer<IViewableItem> GetStackColumnSorter(string stackCaption)
		{
			// If legacy stack sorting is enabled, return null to use the default stack sorting.
			if (Program.ExtendedSettings.LegacyStackSorting)
				return null;

			// Determine the stack configuration depending on program settings.
			ItemViewConfig config = stacksConfig?.GetStackViewConfig(Program.Settings.CommonListStackLayout ? BookList.Name : stackCaption);
			IColumn colInfo = itemView.ConvertKeyToColumns(config?.SortKey)?.FirstOrDefault(); // Get the IColumn that refers to the sort key
			IComparer<IViewableItem> stackColumnSorter = colInfo?.ColumnSorter; // Get the column sorter for the column.

			SortOrder sortOrder = config?.ItemSortOrder ?? SortOrder.None; // Default to None if there is no config
			if (sortOrder == SortOrder.Descending) 
				stackColumnSorter = stackColumnSorter?.Reverse(); // Reverse the sorter if the sort order is Descending.

			stackColumnSorter ??= defaultSeriesComparer; // Default comparer (series) if no stack configuration is found.
			return stackColumnSorter;
		}

		private void ComicItemAdded(object sender, SmartListChangedEventArgs<IViewableItem> e)
		{
			CoverViewItem coverViewItem = e.Item as CoverViewItem;
			if (coverViewItem != null)
			{
				coverViewItem.ThumbnailConfig = ThumbnailConfig;
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (base.DesignMode)
			{
				return;
			}
			commands.Add(delegate
			{
				QuickSearchType = ComicBookAllPropertiesMatcher.MatcherOption.All;
			}, true, () => QuickSearchType == ComicBookAllPropertiesMatcher.MatcherOption.All, miSearchAll);
			commands.Add(delegate
			{
				QuickSearchType = ComicBookAllPropertiesMatcher.MatcherOption.Series;
			}, true, () => QuickSearchType == ComicBookAllPropertiesMatcher.MatcherOption.Series, miSearchSeries);
			commands.Add(delegate
			{
				QuickSearchType = ComicBookAllPropertiesMatcher.MatcherOption.Writer;
			}, true, () => QuickSearchType == ComicBookAllPropertiesMatcher.MatcherOption.Writer, miSearchWriter);
			commands.Add(delegate
			{
				QuickSearchType = ComicBookAllPropertiesMatcher.MatcherOption.Artists;
			}, true, () => QuickSearchType == ComicBookAllPropertiesMatcher.MatcherOption.Artists, miSearchArtists);
			commands.Add(delegate
			{
				QuickSearchType = ComicBookAllPropertiesMatcher.MatcherOption.Descriptive;
			}, true, () => QuickSearchType == ComicBookAllPropertiesMatcher.MatcherOption.Descriptive, miSearchDescriptive);
			commands.Add(delegate
			{
				QuickSearchType = ComicBookAllPropertiesMatcher.MatcherOption.File;
			}, true, () => QuickSearchType == ComicBookAllPropertiesMatcher.MatcherOption.File, miSearchFile);
			commands.Add(delegate
			{
				QuickSearchType = ComicBookAllPropertiesMatcher.MatcherOption.Catalog;
			}, true, () => QuickSearchType == ComicBookAllPropertiesMatcher.MatcherOption.Catalog, miSearchCatalog);
			ContextMenuStrip autoViewContextMenuStrip = itemView.AutoViewContextMenuStrip;
			ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)autoViewContextMenuStrip.Items.Add(TR.Default["Layout", "Layout"]);
			toolStripMenuItem.DropDown.Opening += LayoutMenuOpening;
			miCopyListSetup = toolStripMenuItem.DropDown.Items.Add(TR.Default["Copy", "Copy"]) as ToolStripMenuItem;
			miPasteListSetup = toolStripMenuItem.DropDown.Items.Add(TR.Default["Paste", "Paste"]) as ToolStripMenuItem;
			autoViewContextMenuStrip.Items.Add(new ToolStripSeparator());
			ToolStripMenuItem toolStripMenuItem2;
			autoViewContextMenuStrip.Items.Add(toolStripMenuItem2 = miSelectAll.Clone());
			ToolStripMenuItem toolStripMenuItem3;
			autoViewContextMenuStrip.Items.Add(toolStripMenuItem3 = miResetListBackground.Clone());
			tsQuickSearch.TextBox.SearchMenu = contextQuickSearch;
			components.Add(commands);
			commands.Add(delegate
			{
				base.Main.Control.InvokeActiveService(delegate (IBrowseHistory t)
				{
					t.BrowsePrevious();
				});
			}, () => base.Main.Control.InvokeActiveService((IBrowseHistory t) => t.CanBrowsePrevious(), defaultReturn: false), btBrowsePrev);
			commands.Add(delegate
			{
				base.Main.Control.InvokeActiveService(delegate (IBrowseHistory t)
				{
					t.BrowseNext();
				});
			}, () => base.Main.Control.InvokeActiveService((IBrowseHistory t) => t.CanBrowseNext(), defaultReturn: false), btBrowseNext);
			commands.Add(delegate
			{
				ShowOptionType = ComicBookAllPropertiesMatcher.ShowOptionType.All;
			}, true, () => ShowOptionType == ComicBookAllPropertiesMatcher.ShowOptionType.All, miShowOnlyAllComics);
			commands.Add(delegate
			{
				ShowOptionType = ComicBookAllPropertiesMatcher.ShowOptionType.Unread;
			}, true, () => ShowOptionType == ComicBookAllPropertiesMatcher.ShowOptionType.Unread, miShowOnlyUnreadComics);
			commands.Add(delegate
			{
				ShowOptionType = ComicBookAllPropertiesMatcher.ShowOptionType.Reading;
			}, true, () => ShowOptionType == ComicBookAllPropertiesMatcher.ShowOptionType.Reading, miShowOnlyReadingComics);
			commands.Add(delegate
			{
				ShowOptionType = ComicBookAllPropertiesMatcher.ShowOptionType.Read;
			}, true, () => ShowOptionType == ComicBookAllPropertiesMatcher.ShowOptionType.Read, miShowOnlyReadComics);
			commands.Add(delegate
			{
				ShowOnlyDuplicates = !ShowOnlyDuplicates;
			}, true, () => ShowOnlyDuplicates, miShowOnlyDuplicates);
			commands.Add(ShowWeb, miShowWeb);
			commands.Add(delegate
			{
				ShowComicType = ((ShowComicType != ComicBookAllPropertiesMatcher.ShowComicType.Comics) ? ComicBookAllPropertiesMatcher.ShowComicType.Comics : ComicBookAllPropertiesMatcher.ShowComicType.All);
			}, true, () => ShowComicType == ComicBookAllPropertiesMatcher.ShowComicType.Comics, miShowOnlyComics);
			commands.Add(delegate
			{
				ShowComicType = ((ShowComicType != ComicBookAllPropertiesMatcher.ShowComicType.FilelessComics) ? ComicBookAllPropertiesMatcher.ShowComicType.FilelessComics : ComicBookAllPropertiesMatcher.ShowComicType.All);
			}, true, () => ShowComicType == ComicBookAllPropertiesMatcher.ShowComicType.FilelessComics, miShowOnlyFileless);
			commands.Add(itemView.ToggleGroups, () => itemView.AreGroupsVisible, miExpandAllGroups);
			commands.Add(delegate
			{
				ShowGroupHeaders = !ShowGroupHeaders;
			}, true, () => ShowGroupHeaders, miShowGroupHeaders);
			commands.Add(delegate
			{
				base.Main.Control.InvokeActiveService(delegate (ISidebar s)
				{
					s.Visible = !s.Visible;
				});
			}, true, () => base.Main.Control.InvokeActiveService((ISidebar s) => s.Visible, defaultReturn: false), tbSidebar);
			commands.Add(OpenComic, AllSelectedLinked, miRead);
			commands.Add(OpenComicNewTab, AllSelectedLinked, miReadTab);
			commands.Add(itemView.SelectAll, miSelectAll, toolStripMenuItem2);
			commands.Add(itemView.InvertSelection, miInvertSelection);
			commands.Add(RemoveBooks, CanRemoveBooks, miRemove);
			commands.Add(RevealInExplorer, () => ComicEditMode.CanShowComics() && AllSelectedLinked(), miRevealBrowser);
			commands.Add(delegate
			{
				base.Main.ShowInfo();
			}, () => itemView.SelectedCount > 0, miProperties);
			commands.Add(EditItem, () => ComicEditMode.CanEditProperties() && itemView.SelectedCount > 0, miEdit);
			commands.Add(RefreshInformation, miRefreshInformation);
			commands.Add(AddFilesToLibrary, () => !GetBookList(ComicBookFilterType.NotInLibrary | ComicBookFilterType.IsLocal | ComicBookFilterType.IsNotFileless).IsEmpty(), miAddLibrary);
			commands.Add(delegate
			{
				base.Main.ConvertComic(GetBookList(ComicBookFilterType.IsNotFileless | ComicBookFilterType.CanExport | ComicBookFilterType.Selected, asArray: true), null);
			}, () => ComicEditMode.CanExport() && AnySelectedLinked(), miExportComicsAs);
			commands.Add(delegate
			{
				base.Main.ConvertComic(GetBookList(ComicBookFilterType.IsNotFileless | ComicBookFilterType.CanExport | ComicBookFilterType.Selected, asArray: true), Program.Settings.CurrentExportSetting);
			}, () => Program.Settings.CurrentExportSetting != null && ComicEditMode.CanExport() && AnySelectedLinked(), miExportComicsWithPrevious);
			commands.Add(MarkSelectedRead, ComicEditMode.CanEditProperties() && itemView.SelectedCount > 0, miMarkRead);
			commands.Add(MarkSelectedNotRead, ComicEditMode.CanEditProperties() && itemView.SelectedCount > 0, miMarkUnread);
			commands.Add(MarkSelectedChecked, ComicEditMode.CanEditProperties() && itemView.SelectedCount > 0, miMarkChecked);
			commands.Add(MarkSelectedUnchecked, ComicEditMode.CanEditProperties() && itemView.SelectedCount > 0, miMarkUnchecked);
			commands.Add(SetSelectedComicAsListBackground, AllSelectedLinked, miSetListBackground);
			commands.Add(ResetListBackgroundImage, () => itemView.BackgroundImage != ListBackgroundImage, miResetListBackground, toolStripMenuItem3);
			commands.Add(delegate
			{
				MoveBooks(GetBookList(ComicBookFilterType.Library | ComicBookFilterType.Selected), bottom: false);
			}, () => CanReorderList(), miEditListMoveToTop);
			commands.Add(delegate
			{
				MoveBooks(GetBookList(ComicBookFilterType.Library | ComicBookFilterType.Selected), bottom: true);
			}, () => CanReorderList(), miEditListMoveToBottom);
			commands.Add(delegate
			{
				ApplyBookOrder(GetBookList(ComicBookFilterType.Library));
			}, () => CanReorderList(mustBeOrdered: false), miEditListApplyOrder);
			//commands.Add(PasteComicData, () => ComicEditMode.CanEditProperties() && Clipboard.ContainsData(ComicBook.ClipboardFormat) && !GetBookList(ComicBookFilterType.Selected).IsEmpty(), miPasteData);
			commands.Add(CopyComicData, () => itemView.SelectedCount > 0, miCopyData);
			commands.Add(ClearComicData, () => ComicEditMode.CanEditProperties() && !GetBookList(ComicBookFilterType.Selected).IsEmpty(), miClearData);
			commands.Add(UpdateFiles, AnySelectedLinked, miUpdateComicFiles);
			commands.Add(delegate
			{
				itemView.ItemViewMode = ItemViewMode.Thumbnail;
			}, true, () => itemView.ItemViewMode == ItemViewMode.Thumbnail, miViewThumbnails);
			commands.Add(delegate
			{
				itemView.ItemViewMode = ItemViewMode.Tile;
			}, true, () => itemView.ItemViewMode == ItemViewMode.Tile, miViewTiles);
			commands.Add(delegate
			{
				itemView.ItemViewMode = ItemViewMode.Detail;
			}, true, () => itemView.ItemViewMode == ItemViewMode.Detail, miViewDetails);
			commands.Add(delegate
			{
				base.Main.GetRatingEditor().SetRating(0f);
			}, () => base.Main.GetRatingEditor().IsValid(), () => Math.Round(base.Main.GetRatingEditor().GetRating()) == 0.0, miRating0);
			commands.Add(delegate
			{
				base.Main.GetRatingEditor().SetRating(1f);
			}, () => base.Main.GetRatingEditor().IsValid(), () => Math.Round(base.Main.GetRatingEditor().GetRating()) == 1.0, miRating1);
			commands.Add(delegate
			{
				base.Main.GetRatingEditor().SetRating(2f);
			}, () => base.Main.GetRatingEditor().IsValid(), () => Math.Round(base.Main.GetRatingEditor().GetRating()) == 2.0, miRating2);
			commands.Add(delegate
			{
				base.Main.GetRatingEditor().SetRating(3f);
			}, () => base.Main.GetRatingEditor().IsValid(), () => Math.Round(base.Main.GetRatingEditor().GetRating()) == 3.0, miRating3);
			commands.Add(delegate
			{
				base.Main.GetRatingEditor().SetRating(4f);
			}, () => base.Main.GetRatingEditor().IsValid(), () => Math.Round(base.Main.GetRatingEditor().GetRating()) == 4.0, miRating4);
			commands.Add(delegate
			{
				base.Main.GetRatingEditor().SetRating(5f);
			}, () => base.Main.GetRatingEditor().IsValid(), () => Math.Round(base.Main.GetRatingEditor().GetRating()) == 5.0, miRating5);
			commands.Add(delegate
			{
				base.Main.GetRatingEditor().QuickRatingAndReview();
			}, () => base.Main.GetRatingEditor().IsValid(), miQuickRating);
			commands.Add(CopyListSetup, miCopyListSetup);
			commands.Add(PasteListSetup, miPasteListSetup);
			commands.Add(SetTopOfStack, () => itemView.SelectedCount == 1 && !HasTop(), miSetTopOfStack);
			commands.Add(ResetTopOfStack, HasTop, miResetTopOfStack);
			commands.Add(SetStackThumbnail, true, miSetStackThumbnail);
			commands.Add(RemoveStackThumbnail, true, miRemoveStackThumbnail);
			miAutomation.DropDownItems.AddRange(ScriptUtility.CreateToolItems<ToolStripMenuItem>(this, "Books", () => GetBookList(ComicBookFilterType.Selected)).ToArray());
			miAutomation.Visible = miAutomation.DropDownItems.Count != 0;
			List<ToolStripItem> list = new List<ToolStripItem>();
			list.AddRange(ScriptUtility.CreateToolItems<ToolStripButton>(this, "Books", () => GetBookList(ComicBookFilterType.Selected), (Command c) => c.Image != null && c.Configure == null));
			list.AddRange(ScriptUtility.CreateToolItems<ToolStripSplitButton>(this, "Books", () => GetBookList(ComicBookFilterType.Selected), (Command c) => c.Image != null && c.Configure != null));
			if (list.Count != 0)
			{
				list.ForEach(delegate (ToolStripItem bt)
				{
					bt.DisplayStyle = ToolStripItemDisplayStyle.Image;
				});
				toolStrip.Items.Add(new ToolStripSeparator());
				toolStrip.Items.AddRange(list.ToArray());
			}
			ToolStripSeparator toolStripSeparator = sepDuplicateList;
			bool visible = (tbbDuplicateList.Visible = Library != null && Library.EditMode.CanEditList());
			toolStripSeparator.Visible = visible;
			miShowInList.Visible = Library != null;
			if (Library != Program.Database)
			{
				ToolStripSeparator toolStripSeparator2 = sepUndo;
				ToolStripButton toolStripButton = tbUndo;
				bool flag3 = (tbRedo.Visible = false);
				visible = (toolStripButton.Visible = flag3);
				toolStripSeparator2.Visible = visible;
			}
			else
			{
				commands.Add(Program.Database.Undo.Undo, () => Program.Database.Undo.CanUndo, tbUndo);
				commands.Add(Program.Database.Undo.Redo, () => Program.Database.Undo.CanRedo, tbRedo);
			}
			base.Controls.SetChildIndex(toolStrip, base.Controls.Count - 1);
			ItemView.MouseDown += ItemView_MouseDown;
		}

		private void ItemView_MouseDown(object sender, MouseEventArgs e)
		{
			Program.MainForm.MouseDownHandler(sender, e);
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			toolTip.SetToolTip(itemView, null);
		}

		protected override void OnMainFormChanged()
		{
			base.OnMainFormChanged();
			if (base.Main != null)
			{
				commands.Add(base.Main.EditListLayout, tsEditListLayout);
				commands.Add(base.Main.SaveListLayout, tsSaveListLayout);
				commands.Add(base.Main.EditListLayouts, () => Program.Settings.ListConfigurations.Count > 0, tsEditLayouts);
				base.Main.OpenBooks.BookClosed += OpenBooksChanged;
				base.Main.OpenBooks.BookOpened += OpenBooksChanged;
				UpdatePending();
			}
		}

		protected virtual void OnComicEditModeChanged()
		{
			itemView.LabelEdit = ComicEditMode.CanEditProperties();
			miRevealBrowser.Visible = ComicEditMode.CanShowComics();
			ToolStripMenuItem toolStripMenuItem = miCopyData;
			ToolStripMenuItem toolStripMenuItem2 = miPasteData;
			ToolStripMenuItem toolStripMenuItem3 = miClearData;
			bool flag2 = (tsCopySeparator.Visible = ComicEditMode.CanEditProperties());
			bool flag4 = (toolStripMenuItem3.Visible = flag2);
			bool visible = (toolStripMenuItem2.Visible = flag4);
			toolStripMenuItem.Visible = visible;
			ToolStripSeparator toolStripSeparator = toolStripRemoveSeparator;
			visible = (miRemove.Visible = ComicEditMode.CanDeleteComics());
			toolStripSeparator.Visible = visible;
		}

		private void lvGroupHeaders_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			itemView.GroupSortingOrder = ItemView.FlipSortOrder(itemView.GroupSortingOrder);
		}

		private void lvGroupHeaders_ClientSizeChanged(object sender, EventArgs e)
		{
			int num = lvGroupHeaders.ClientRectangle.Width - (lvGroupsCount.Width + 2);
			if (num > 0)
			{
				lvGroupsName.Width = num;
			}
		}

		private void browserContainer_DoubleClick(object sender, EventArgs e)
		{
			ShowGroupHeaders = !ShowGroupHeaders;
		}

		private void OpenBooksChanged(object sender, BookEventArgs e)
		{
			UpdateBookMarkers();
		}

		private void btCloseStack_Click(object sender, EventArgs e)
		{
			CloseStack(withUpdate: true);
		}

		private void btPrevStack_Click(object sender, EventArgs e)
		{
			CoverViewItem oldItem = stackItem as CoverViewItem;
			CloseStack(withUpdate: true);
			CoverViewItem[] array = itemView.DisplayedItems.OfType<CoverViewItem>().ToArray();
			int num = array.FindIndex((CoverViewItem ci) => (from sci in itemView.GetStackItems(ci).OfType<CoverViewItem>()
															 select sci.Comic).Contains(oldItem.Comic)) - 1;
			if (num < 0)
			{
				num = array.Length - 1;
			}
			try
			{
				OpenStack(array[num]);
			}
			catch (Exception)
			{
			}
		}

		private void btNextStack_Click(object sender, EventArgs e)
		{
			CoverViewItem oldItem = stackItem as CoverViewItem;
			CloseStack(withUpdate: true);
			CoverViewItem[] array = itemView.DisplayedItems.OfType<CoverViewItem>().ToArray();
			int num = array.FindIndex((CoverViewItem ci) => (from sci in itemView.GetStackItems(ci).OfType<CoverViewItem>()
															 select sci.Comic).Contains(oldItem.Comic)) + 1;
			if (num >= array.Length)
			{
				num = 0;
			}
			try
			{
				OpenStack(array[num]);
			}
			catch (Exception)
			{
			}
		}

		private void btDisplayAll_Click(object sender, EventArgs e)
		{
			ShowOnlyDuplicates = false;
			ShowOptionType = ComicBookAllPropertiesMatcher.ShowOptionType.All;
			ShowComicType = ComicBookAllPropertiesMatcher.ShowComicType.All;
		}

		private void itemView_MouseWheel(object sender, MouseEventArgs e)
		{
			if ((Control.ModifierKeys & Keys.Control) != 0 && itemView.ItemViewMode != ItemViewMode.Detail)
			{
				ItemSizeInfo itemSize = GetItemSize();
				SetItemSize(itemSize.Value + e.Delta / SystemInformation.MouseWheelScrollDelta * 16);
			}
		}

		private void tbbSort_DropDownOpening(object sender, EventArgs e)
		{
			FormUtility.SafeToolStripClear(tbbSort.DropDownItems);
			itemView.CreateArrangeMenu(tbbSort.DropDownItems);
		}

		private void tbbGroup_DropDownOpening(object sender, EventArgs e)
		{
			FormUtility.SafeToolStripClear(tbbGroup.DropDownItems);
			itemView.CreateGroupMenu(tbbGroup.DropDownItems);
		}

		private void tbbGroup_ButtonClick(object sender, EventArgs e)
		{
			itemView.GroupSortingOrder = ItemView.FlipSortOrder(itemView.GroupSortingOrder);
		}

		private void tbbStack_ButtonClick(object sender, EventArgs e)
		{
			itemView.ItemStacker = ((itemView.ItemStacker != null) ? null : oldStacker);
		}

		private void tbbStack_DropDownOpening(object sender, EventArgs e)
		{
			FormUtility.SafeToolStripClear(tbbStack.DropDownItems);
			itemView.CreateStackMenu(tbbStack.DropDownItems);
		}

		private void tbbSort_ButtonClick(object sender, EventArgs e)
		{
			itemView.ItemSortOrder = ItemView.FlipSortOrder(itemView.ItemSortOrder);
		}

		private void tbbView_ButtonClick(object sender, EventArgs e)
		{
			ItemViewMode itemViewMode = itemView.ItemViewMode;
			switch (itemViewMode)
			{
				case ItemViewMode.Thumbnail:
					itemViewMode = ItemViewMode.Tile;
					break;
				case ItemViewMode.Tile:
					itemViewMode = ItemViewMode.Detail;
					break;
				case ItemViewMode.Detail:
					itemViewMode = ItemViewMode.Thumbnail;
					break;
			}
			itemView.ItemViewMode = itemViewMode;
		}

		private void tsQuickSearch_TextChanged(object sender, EventArgs e)
		{
			QuickSearch = tsQuickSearch.TextBox.Text;
		}

		private void bookList_BookListRefreshed(object sender, EventArgs e)
		{
			OnCurrentBookListChanged();
		}

		private void bookSelectorPanel_FilterChanged(object sender, EventArgs e)
		{
			bookListDirty = true;
		}

		protected override void OnIdle()
		{
			if (base.DesignMode || !base.Visible)
			{
				return;
			}
			UpdatePending();
			if (updateGroupList)
			{
				updateGroupList = false;
				FillGroupList();
			}
			if (newGroupListWidth != 0 && showGroupHeaders)
			{
				int num = browserContainer.ClientRectangle.Width - newGroupListWidth;
				if (num > 0)
				{
					browserContainer.SplitterDistance = num;
				}
				newGroupListWidth = 0;
			}
			if (quickSearchCueTexts == null)
			{
				quickSearchCueTexts = new string[6]
				{
					miSearchAll.Text,
					miSearchSeries.Text,
					miSearchWriter.Text,
					miSearchArtists.Text,
					miSearchDescriptive.Text,
					miSearchFile.Text
				};
				for (int i = 0; i < quickSearchCueTexts.Length; i++)
				{
					quickSearchCueTexts[i] = TR.Default["Search", "Search"] + " " + quickSearchCueTexts[i].Replace("&", string.Empty);
				}
			}
			tsQuickSearch.TextBox.SetCueText(quickSearchCueTexts[(int)QuickSearchType]);
			tbSidebar.Visible = !HideNavigation && base.Main != null && base.Main.Control.FindActiveService<ISidebar>() != null;
			ToolStripSeparator toolStripSeparator = tbBrowseSeparator;
			ToolStripButton toolStripButton = btBrowseNext;
			bool flag2 = (btBrowsePrev.Visible = !HideNavigation && base.Main != null && base.Main.Control.FindActiveService<IBrowseHistory>() != null);
			bool visible = (toolStripButton.Visible = flag2);
			toolStripSeparator.Visible = visible;
			tbbSort.Enabled = itemView.Columns.Count != 0;
			tbbSort.Text = ((itemView.SortColumn != null) ? itemView.SortColumn.Text : noneText);
			tbbSort.ToolTipText = ((itemView.SortColumn != null) ? StringUtility.Format(arrangedByText, tbbSort.Text) : notArrangedText);
			tbbGroup.Enabled = itemView.Columns.Count != 0;
			tbbGroup.Text = ((itemView.GroupColumn != null) ? itemView.GroupColumn.Text : noneText);
			tbbGroup.ToolTipText = ((itemView.GroupColumn != null) ? StringUtility.Format(groupedByText, tbbGroup.Text) : notGroupedText);
			openStackPanel.Visible = stackFilter is StackMatcher;
			if (openStackPanel.Visible)
			{
				currentStackName = ((StackMatcher)stackFilter).Caption;
				lblOpenStackText.Text = currentStackName;
			}
			tbbStack.Visible = itemView.ItemViewMode != ItemViewMode.Detail && !openStackPanel.Visible;
			if (tbbStack.Visible)
			{
				tbbStack.Enabled = itemView.Columns.Count != 0;
				tbbStack.Text = ((itemView.StackColumn != null) ? itemView.StackColumn.Text : noneText);
				tbbStack.ToolTipText = ((itemView.StackColumn != null) ? StringUtility.Format(stackedByText, tbbStack.Text) : notStackedText);
			}
			if (itemView.ItemStacker != null)
			{
				oldStacker = itemView.ItemStacker;
			}
			tbbSort.Image = ((itemView.ItemSortOrder == SortOrder.Ascending) ? sortUp : sortDown);
			tbbGroup.Image = ((itemView.GroupSortingOrder == SortOrder.Ascending) ? groupDown : groupUp);
			if (tbUndo.Visible)
			{
				if (tbUndo.Tag == null)
				{
					tbUndo.Tag = tbUndo.Text;
				}
				string undoLabel = Program.Database.Undo.UndoLabel;
				tbUndo.ToolTipText = (string)tbUndo.Tag + (string.IsNullOrEmpty(undoLabel) ? string.Empty : (": " + undoLabel));
				if (tbRedo.Tag == null)
				{
					tbRedo.Tag = tbRedo.Text;
				}
				string text = Program.Database.Undo.RedoEntries.FirstOrDefault();
				tbRedo.ToolTipText = (string)tbRedo.Tag + (string.IsNullOrEmpty(text) ? string.Empty : (": " + text));
			}
			OptimizeToolstripDisplay();
		}

		private void UpdatePending()
		{
			if (bookListDirty)
			{
				bookListDirty = false;
				FillBookList();
			}
		}

		private void itemView_ItemActivate(object sender, EventArgs e)
		{
			IViewableItem focusedItem = itemView.FocusedItem;
			if (!itemView.IsStack(focusedItem) || itemView.GetStackCount(focusedItem) == 1)
			{
				CoverViewItem coverViewItem = focusedItem as CoverViewItem;
				if (coverViewItem != null && coverViewItem.Comic.IsLinked)
				{
					ExternalProgram ep = Program.Settings.ExternalPrograms.FirstOrDefault(x => x.Override);

					if (ep is null)
						OpenComic();
					else
						StartExternalProgram(ep, coverViewItem.Comic);
				}
				else
				{
					base.Main.ShowInfo();
				}
			}
			else
			{
				OpenStack(focusedItem);
			}
		}

		private void itemView_PostPaint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawShadow(itemView.DisplayRectangle, 8, Color.Black, 0.125f, BlurShadowType.Inside, BlurShadowParts.Edges);
		}

		private void searchBrowserContainer_ExpandedChanged(object sender, EventArgs e)
		{
			SearchBrowserVisible = searchBrowserContainer.Expanded;
		}

		private void itemView_SelectedIndexChanged(object sender, EventArgs e)
		{
			selectedSize = itemView.SelectedItems.Cast<CoverViewItem>().Sum((CoverViewItem cvi) => Math.Max(cvi.Comic.FileSize, 0L));
		}

		private void BookInListChanged(object sender, PropertyChangedEventArgs e)
		{
			string propertyName = e.PropertyName;
			if (propertyName == "OpenedTime")
			{
				UpdateBookMarkers();
			}
		}

		private void itemView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				RemoveBooks();
			}
		}

		private void contextMenuItems_Opened(object sender, EventArgs e)
		{
			contextMenuMouseLocation = Cursor.Position;
		}

		private void contextMenuItems_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			contextMenuCloseTime = Machine.Ticks;
		}

		private void quickSearchTimer_Tick(object sender, EventArgs e)
		{
			quickSearchTimer.Stop();
			UpdateSearch();
		}

		private void itemView_ProcessStack(object sender, ItemView.StackEventArgs e)
		{
			if (stacksConfig == null)
			{
				return;
			}
			StacksConfig.StackConfigItem stackConfigItem = stacksConfig.FindItem(e.Stack.Text);
			if (stackConfigItem == null)
			{
				return;
			}
			if (!string.IsNullOrEmpty(stackConfigItem.ThumbnailKey))
			{
				ISetCustomThumbnail setCustomThumbnail = e.Stack.Items.FirstOrDefault() as ISetCustomThumbnail;
				if (setCustomThumbnail != null)
				{
					setCustomThumbnail.CustomThumbnailKey = ThumbnailKey.GetResource(ThumbnailKey.CustomKey, stackConfigItem.ThumbnailKey);
				}
			}
			Guid id = stackConfigItem.TopId;
			if (id != Guid.Empty)
			{
				int num = e.Stack.Items.FindIndex((IViewableItem item) => ((CoverViewItem)item).Comic.Id == id);
				if (num != -1)
				{
					IViewableItem item2 = e.Stack.Items[num];
					e.Stack.Items.RemoveAt(num);
					e.Stack.Items.Insert(0, item2);
				}
			}
		}

		private void tsQuickSearch_Enter(object sender, EventArgs e)
		{
			oldQuickWidth = tsQuickSearch.Width;
			tsQuickSearch.AutoSize = false;
			tsQuickSearch.Width *= 2;
		}

		private void tsQuickSearch_Leave(object sender, EventArgs e)
		{
			tsQuickSearch.AutoSize = true;
			tsQuickSearch.Width = oldQuickWidth;
		}

		private void itemView_GroupDisplayChanged(object sender, EventArgs e)
		{
			updateGroupList = true;
		}

		private void itemView_ItemDisplayChanged(object sender, EventArgs e)
		{
			updateGroupList = true;
		}

		private void lvGroupHeaders_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lvGroupHeaders.SelectedIndices.Count != 0)
			{
				string text = lvGroupHeaders.SelectedItems[0].Text;
				IViewableItem firstGroupItem = itemView.GetFirstGroupItem(text);
				if (firstGroupItem != null)
				{
					itemView.ExpandGroups(expand: true, text);
					itemView.EnsureGroupVisible(text);
				}
			}
		}

		private void SetCustomColumns()
		{
			Dictionary<int, ItemViewColumn> dictionary = null;
			if (Library != null && Program.Settings.ShowCustomBookFields)
			{
				dictionary = (from customKey in Library.CustomValues
							  where Program.ExtendedSettings.ShowCustomScriptValues || !customKey.Contains('.')
							  select new
							  {
								  Id = 10000 + Math.Abs(customKey.GetHashCode()),
								  Prop = "{" + customKey + "}",
								  Key = customKey
							  } into customField
							  select new ItemViewColumn(customField.Id, customField.Key, 100, new ComicListField(customField.Prop, customFieldDescription.SafeFormat(customField.Key), customField.Prop), new CoverViewItemPropertyComparer(customField.Prop), new CoverViewItemPropertyGrouper(customField.Prop), visible: false)).ToDictionary((ItemViewColumn item) => item.Id);
			}
			for (int num = itemView.Columns.Count - 1; num >= 0; num--)
			{
				int id = itemView.Columns[num].Id;
				if (id >= 10000)
				{
					if (dictionary != null && dictionary.ContainsKey(id))
					{
						dictionary.Remove(id);
					}
					else
					{
						itemView.Columns.RemoveAt(num);
					}
				}
			}
			if (dictionary == null)
			{
				return;
			}
			foreach (ItemViewColumn value in dictionary.Values)
			{
				value.TooltipText = ((ComicListField)value.Tag).Description;
				itemView.Columns.Add(value);
			}
		}

		private void SetVirtualTagColumns()
		{
			//Send the settings to the Collection.
			VirtualTagsCollection.RegisterSettings(Program.Settings);

			//Create the list of columns that we want to show, based on our Virtual Tags settings
			Dictionary<int, ItemViewColumn> dictionary = new Dictionary<int, ItemViewColumn>();
			foreach (var vtag in VirtualTagsCollection.Tags.Values)
			{
				if (vtag != null && vtag.IsEnabled)
				{
					int id = 300 + vtag.ID;
					dictionary[id] = new ItemViewColumn(id, vtag.Name, 100,
											new ComicListField(vtag.PropertyName, vtag.Description),
												new CoverViewItemVirtualTagComparer(vtag.PropertyName),
												new CoverViewItemVirtualTagGrouper(vtag),
												visible: false);
				}
			}

			//Check the existing columns and remove the ones that we don't need anymore, or the columns that already exists from our dictionary
			for (int num = itemView.Columns.Count - 1; num >= 0; num--)
			{
				int id = itemView.Columns[num].Id;
				if (id >= 300 && id < 10000)
				{
					//Remove tag from the dict since it already exists and we won't need to add it later.
					if (dictionary != null && dictionary.ContainsKey(id))
						dictionary.Remove(id);
					else
						itemView.Columns.RemoveAt(num);//Remove tag from the UI since it isn't valid anymore
				}
			}

			if (dictionary is null)
				return;

			//Add the remaining columns to the interface
			foreach (ItemViewColumn value in dictionary.Values)
			{
				value.TooltipText = ((ComicListField)value.Tag).Description;
				itemView.Columns.Add(value);
			}

			//Update Search Browser
			bookSelectorPanel.FillTypeCombos(update: true);
		}

		private bool CanReorderList(bool mustBeOrdered = true)
		{
			IEditableComicBookListProvider editableComicBookListProvider = BookList.QueryService<IEditableComicBookListProvider>();
			if (ComicEditMode.CanEditList() && editableComicBookListProvider != null && !editableComicBookListProvider.IsLibrary)
			{
				if (mustBeOrdered)
				{
					return IsViewSortedByPosition();
				}
				return true;
			}
			return false;
		}

		private bool MoveBooks(IEnumerable<ComicBook> books, bool bottom)
		{
			if (!CanReorderList())
			{
				return false;
			}
			IEditableComicBookListProvider editableComicBookListProvider = BookList.QueryService<IEditableComicBookListProvider>();
			ComicBook[] source = books.Where(editableComicBookListProvider.Remove).ToArray();
			if (bottom)
			{
				foreach (ComicBook book in books)
				{
					editableComicBookListProvider.Add(book);
				}
			}
			else
			{
				foreach (ComicBook item in source.Reverse())
				{
					editableComicBookListProvider.Insert(0, item);
				}
			}
			return true;
		}

		private void ApplyBookOrder(IEnumerable<ComicBook> books)
		{
			if (!CanReorderList(mustBeOrdered: false))
			{
				return;
			}
			books = books.ToArray();
			IEditableComicBookListProvider editableComicBookListProvider = BookList.QueryService<IEditableComicBookListProvider>();
			ComicBook[] source = books.Where(editableComicBookListProvider.Remove).ToArray();
			foreach (ComicBook item in source.Reverse())
			{
				editableComicBookListProvider.Insert(0, item);
			}
		}

		private bool AllSelectedLinked()
		{
			return GetBookList(ComicBookFilterType.Selected).All((ComicBook cb) => cb.IsLinked);
		}

		private bool AnySelectedLinked()
		{
			return GetBookList(ComicBookFilterType.Selected).Any((ComicBook cb) => cb.IsLinked);
		}

		private void OptimizeToolstripDisplay()
		{
			IEnumerable<ToolStripItem> source = toolStrip.Items.OfType<ToolStripItem>();
			int num = source.Sum((ToolStripItem n) => n.Width);
			int num2 = toolStrip.Width - 20;
			if (num2 == savedOptimizeToolstripRight && num == savedOptimizeToolstripWitdh)
			{
				return;
			}
			if (num > num2)
			{
				SaveStyleSet(ToolStripItemDisplayStyle.Image, tbbView, tbbGroup, tbbSort, tbbStack);
			}
			if (num < num2)
			{
				SaveStyleSet(ToolStripItemDisplayStyle.ImageAndText, tbbView, tbbGroup, tbbSort, tbbStack);
				num = source.Sum((ToolStripItem n) => n.Width);
				if (num > num2)
				{
					SaveStyleSet(ToolStripItemDisplayStyle.Image, tbbView, tbbGroup, tbbSort, tbbStack);
				}
			}
			savedOptimizeToolstripRight = num2;
			savedOptimizeToolstripWitdh = source.Sum((ToolStripItem n) => n.Width);
		}

		private static void SaveStyleSet(ToolStripItemDisplayStyle style, params ToolStripItem[] items)
		{
			foreach (ToolStripItem toolStripItem in items)
			{
				if (toolStripItem.DisplayStyle != style)
				{
					toolStripItem.DisplayStyle = style;
				}
			}
		}

		private void SetSelectedComicAsListBackground()
		{
			ComicBook comicBook = GetBookList(ComicBookFilterType.Selected).FirstOrDefault();
			if (comicBook != null)
			{
				SetListBackgroundImage(comicBook);
			}
		}

		private void ResetListBackgroundImage()
		{
			SetListBackgroundImage((string)null);
		}

		private bool HasTop()
		{
			ComicBook comicBook = GetBookList(ComicBookFilterType.Selected).FirstOrDefault();
			IViewableItem viewableItem = itemView.SelectedItems.FirstOrDefault();
			return viewableItem != null && itemView.IsStack(viewableItem) && (stacksConfig?.IsTop(itemView.GetStackCaption(viewableItem), comicBook) ?? false);
		}

		private void SetTopOfStack()
		{
			ComicBook comicBook = GetBookList(ComicBookFilterType.Selected).FirstOrDefault();
			if (comicBook != null)
			{
				if (stacksConfig == null)
				{
					stacksConfig = new StacksConfig();
				}
				stacksConfig.SetStackTop(currentStackName, comicBook);
			}
		}

		private void ResetTopOfStack()
		{
			IViewableItem viewableItem = itemView.SelectedItems.FirstOrDefault();
			if (viewableItem != null && itemView.IsStack(viewableItem))
			{
				stacksConfig.ResetStackTop(itemView.GetStackCaption(viewableItem));
				FillBookList();
			}
		}

		private void SetStackThumbnail()
		{
			IViewableItem viewableItem = itemView.SelectedItems.FirstOrDefault();
			if (viewableItem == null || !itemView.IsStack(viewableItem))
			{
				return;
			}
			viewableItem = itemView.GetStackItems(viewableItem).FirstOrDefault();
			string text = Program.LoadCustomThumbnail(null, this, miSetStackThumbnail.Text.Replace("&", string.Empty));
			if (text != null)
			{
				if (stacksConfig == null)
				{
					stacksConfig = new StacksConfig();
				}
				stacksConfig.SetStackThumbnailKey(itemView.GetStackCaption(viewableItem), text);
				FillBookList();
			}
		}

		private void RemoveStackThumbnail()
		{
			IViewableItem viewableItem = itemView.SelectedItems.FirstOrDefault();
			if (viewableItem != null && itemView.IsStack(viewableItem))
			{
				stacksConfig.SetStackThumbnailKey(itemView.GetStackCaption(viewableItem), null);
				FillBookList();
			}
		}

		private void CloseStack(bool withUpdate)
		{
			using (new WaitCursor(this))
			{
				if (stackFilter == null)
				{
					return;
				}
				stackItem = null;
				if (withUpdate)
				{
					ItemView.BeginUpdate();
					try
					{
						if (stacksConfig != null)
						{
							stacksConfig.SetStackViewConfig(Program.Settings.CommonListStackLayout ? BookList.Name : currentStackName, itemView.ViewConfig);
						}
						itemView.StackDisplayEnabled = true;
						if (preStackConfig != null)
						{
							itemView.ViewConfig = preStackConfig;
						}
						stackFilter = null;
						FillBookList();
					}
					finally
					{
						itemView.EndUpdate();
						itemView.ScrollPosition = preStackScrollPosition;
					}
				}
				else
				{
					itemView.ItemStacker = ((StackMatcher)stackFilter).Grouper;
					stackFilter = null;
				}
			}
		}

		private void OpenStack(IViewableItem item)
		{
			OpenStack(item, storeConfig: true);
		}

		private void OpenStack(IViewableItem item, bool storeConfig)
		{
			using (new WaitCursor(this))
			{
				if (item == null)
				{
					return;
				}
				itemView.BeginUpdate();
				try
				{
					stackItem = item;
					HashSet<ComicBook> hashSet = new HashSet<ComicBook>();
					IViewableItem[] stackItems = itemView.GetStackItems(item);
					for (int i = 0; i < stackItems.Length; i++)
					{
						CoverViewItem coverViewItem = (CoverViewItem)stackItems[i];
						hashSet.Add(coverViewItem.Comic);
					}
					string stackCaption = itemView.GetStackCaption(item);
					stackFilter = new StackMatcher(itemView.ItemStacker, stackCaption, hashSet);
					if (storeConfig)
					{
						preStackConfig = itemView.ViewConfig;
						preStackScrollPosition = itemView.ScrollPosition;
						preStackFocusedId = GetFocusedId();
					}
					itemView.ItemStacker = null;
					if (stacksConfig == null)
					{
						stacksConfig = new StacksConfig();
					}
					ItemViewConfig stackViewConfig = stacksConfig.GetStackViewConfig(Program.Settings.CommonListStackLayout ? BookList.Name : stackCaption);
					if (stackViewConfig != null)
					{
						itemView.ViewConfig = stackViewConfig;
					}
					itemView.StackDisplayEnabled = false;
					FillBookList();
				}
				finally
				{
					itemView.EndUpdate();
				}
			}
		}

		private void RegisterBookList()
		{
			if (bookList == null)
			{
				return;
			}
			IDisplayListConfig displayListConfig = bookList.QueryService<IDisplayListConfig>();
			ResetListBackgroundImage();
			if (displayListConfig != null && displayListConfig.Display != null)
			{
				stacksConfig = CloneUtility.Clone(displayListConfig.Display.StackConfig);
				itemView.ViewConfig = displayListConfig.Display.View;
				ThumbnailConfig = displayListConfig.Display.Thumbnail;
				if (Program.Settings.LocalQuickSearch)
				{
					blockQuickSearchUpdate = true;
					ShowOptionType = displayListConfig.Display.ShowOptionType;
					ShowComicType = displayListConfig.Display.ShowComicType;
					ShowOnlyDuplicates = displayListConfig.Display.ShowOnlyDuplicates;
					QuickSearchType = displayListConfig.Display.QuickSearchType;
					QuickSearch = displayListConfig.Display.QuickSearch;
					blockQuickSearchUpdate = false;
					ShowGroupHeaders = displayListConfig.Display.ShowGroupHeaders;
					newGroupListWidth = displayListConfig.Display.ShowGroupHeadersWidth;
				}
				UpdateQuickFilter();
				if (displayListConfig.Display.View == null && bookList.QueryService<IEditableComicBookListProvider>() != null)
				{
					itemView.ItemGrouper = null;
					itemView.SortColumn = itemView.Columns.FindById(100);
				}
				SetListBackgroundImage(displayListConfig.Display.BackgroundImageSource);
			}
			bookList.BookListChanged += bookList_BookListRefreshed;
		}

		private void UnregisterBookList()
		{
			itemView.FocusedItem = null;
			StoreWorkspace(null);
			CloseStack(withUpdate: true);
			if (bookList != null)
			{
				bookList.BookListChanged -= bookList_BookListRefreshed;
			}
		}

		private void FillBookSelector()
		{
			FillBookSelector(GetCurrentList(withSelector: false), updateNow: true);
		}

		private void FillBookSelector(IEnumerable<ComicBook> books, bool updateNow)
		{
			bookSelectorPanel.Books.Clear();
			if (searchBrowserVisible && BookList != null)
			{
				bookSelectorPanel.Books.AddRange(books);
			}
			if (updateNow)
			{
				bookSelectorPanel.UpdateLists();
			}
		}

		private IEnumerable<ComicBook> GetCurrentList(bool withSelector)
		{
			if (BookList == null)
			{
				return Enumerable.Empty<ComicBook>();
			}
			ComicBookGroupMatcher currentMatcher = GetCurrentMatcher(withStack: true, withSelector);
			IEnumerable<ComicBook> books = BookList.GetBooks();
			if (currentMatcher != null)
			{
				return currentMatcher.Match(books);
			}
			return books;
		}

		private ComicBookGroupMatcher GetCurrentMatcher(bool withStack, bool withSelector)
		{
			ComicBookGroupMatcher comicBookGroupMatcher = new ComicBookGroupMatcher();
			if (stackFilter != null && withStack)
			{
				comicBookGroupMatcher.Matchers.Add(stackFilter);
			}
			if (quickFilter != null)
			{
				comicBookGroupMatcher.Matchers.Add(quickFilter);
			}
			if (bookSelectorPanel.CurrentMatcher != null && withSelector)
			{
				comicBookGroupMatcher.Matchers.Add(bookSelectorPanel.CurrentMatcher);
			}
			if (ShowOnlyDuplicates)
			{
				comicBookGroupMatcher.Matchers.Add(new ComicBookDuplicateMatcher());
			}
			return comicBookGroupMatcher;
		}

		private void FillBookList()
		{
			IViewableItem focusedItem = itemView.FocusedItem;
			int num = itemView.FocusedItemDisplayIndex;
			ComicBookGroupMatcher currentMatcher = GetCurrentMatcher(withStack: true, withSelector: false);
			ComicBookMatcher currentMatcher2 = bookSelectorPanel.CurrentMatcher;
			ComicBook comicBook = ((focusedItem != null) ? ((CoverViewItem)focusedItem).Comic : null);
			itemView.BeginUpdate();
			try
			{
				int num2 = 1;
				itemView.Items.Clear();
				totalCount = 0;
				totalSize = 0L;
				selectedSize = 0L;
				if (BookList != null)
				{
					ComicBook[] array = BookList.GetBooks().ToArray();
					IComicBookStatsProvider statsProvider = BookList as IComicBookStatsProvider;
					IEnumerable<ComicBook> enumerable = currentMatcher.Match(array).ToArray();
					totalCount = array.Length;
					FillBookSelector(enumerable, updateNow: true);
					if (currentMatcher2 != null)
					{
						// ComicBookValueMatcher's Match ignores negation, but ComicBookGroupMatcher's Match handles it. ComicBookGroupMatcher.Optimized returns only the value when a single filter is used.

						IEnumerable<ComicBook> enumerableTemp = currentMatcher2.Match(enumerable);
						if (currentMatcher2.Not)
							enumerableTemp = enumerable.Except(enumerableTemp);

						enumerable = enumerableTemp;

					}
					foreach (ComicBook item in enumerable)
					{
						CoverViewItem coverViewItem = CoverViewItem.Create(item, num2++, statsProvider);
						coverViewItem.BookChanged += BookInListChanged;
						itemView.Items.Add(coverViewItem);
						totalSize += Math.Max(item.FileSize, 0L);
					}
					UpdateBookMarkers();
				}
				if (comicBook == null || itemView.Items.Count == 0)
				{
					return;
				}
				foreach (CoverViewItem displayedItem in itemView.DisplayedItems)
				{
					IViewableItem[] stackItems = itemView.GetStackItems(displayedItem);
					for (int i = 0; i < stackItems.Length; i++)
					{
						CoverViewItem coverViewItem3 = (CoverViewItem)stackItems[i];
						if (coverViewItem3.Comic == comicBook)
						{
							displayedItem.Selected = true;
							displayedItem.Focused = true;
							itemView.EnsureItemVisible(displayedItem);
							return;
						}
					}
				}
				if (num < 0)
				{
					return;
				}
				if (num >= itemView.DisplayedItems.Count())
				{
					num = itemView.DisplayedItems.Count() - 1;
				}
				foreach (CoverViewItem displayedItem2 in itemView.DisplayedItems)
				{
					if (num == 0)
					{
						displayedItem2.Selected = true;
						displayedItem2.Focused = true;
						itemView.EnsureItemVisible(displayedItem2);
						break;
					}
					num--;
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				itemView.EndUpdate();
				updateGroupList = true;
			}
		}

		private void FillGroupList()
		{
			if (showGroupHeaders && itemView.AreGroupsVisible)
			{
				lvGroupHeaders.BeginUpdate();
				IColumn column = itemView.Columns.FirstOrDefault((IColumn c) => c.ColumnGrouper == itemView.ItemGrouper);
				lvGroupsName.Text = ((column == null) ? string.Empty : column.Text);
				lvGroupHeaders.Items.Clear();
				foreach (string displayedGroup in itemView.DisplayedGroups)
				{
					lvGroupHeaders.Items.Add(displayedGroup).SubItems.Add(itemView.GetGroupSizeFromCaption(displayedGroup).ToString());
				}
				lvGroupHeaders.EndUpdate();
				browserContainer.Panel2Collapsed = false;
			}
			else
			{
				browserContainer.Panel2Collapsed = true;
			}
		}

		public IEnumerable<ComicBook> GetOpenBooks()
		{
			try
			{
				return from nav in base.Main.OpenBooks.Slots
					   where nav != null
					   select nav.Comic;
			}
			catch (NullReferenceException)
			{
				return Enumerable.Empty<ComicBook>();
			}
		}

		private void UpdateBookMarkers()
		{
			DateTime t = DateTime.MinValue;
			ComicBook comicBook = null;
			foreach (CoverViewItem item in itemView.Items)
			{
				if (t < item.Comic.OpenedTime)
				{
					t = item.Comic.OpenedTime;
					comicBook = item.Comic;
				}
			}
			IEnumerable<ComicBook> openBooks = GetOpenBooks();
			foreach (CoverViewItem item2 in itemView.Items)
			{
				item2.Marker = ((item2.Comic == comicBook) ? MarkerType.IsLast : MarkerType.None);
				item2.Marker = (openBooks.Contains(item2.Comic) ? MarkerType.IsOpen : item2.Marker);
			}
		}

		private void AddFilesToLibrary()
		{
			Program.Scanner.ScanFilesOrFolders(from cb in GetBookList(ComicBookFilterType.NotInLibrary | ComicBookFilterType.IsNotFileless | ComicBookFilterType.Selected, asArray: true)
											   select cb.FilePath, all: false, removeMissing: false);
		}

		private void DuplicateList(ComicListItemFolder clif = null)
		{
			if (Library == null || !Library.EditMode.CanEditList())
			{
				return;
			}
			ComicBookGroupMatcher currentMatcher = GetCurrentMatcher(withStack: false, withSelector: true);
			if (currentMatcher == null || currentMatcher.Matchers.Count == 0)
			{
				return;
			}
			string name = string.Empty;
			ComicSmartListItem comicSmartListItem = new ComicSmartListItem("")
			{
				BaseListId = BookList.Id,
				MatcherMode = currentMatcher.MatcherMode
			};
			foreach (ComicBookMatcher matcher in currentMatcher.Matchers)
			{
				comicSmartListItem.Matchers.Add(matcher.Clone() as ComicBookMatcher);
			}
			foreach (ComicBookValueMatcher item in comicSmartListItem.Matchers.Recurse<ComicBookValueMatcher>((object cbm) => (!(cbm is ComicBookGroupMatcher)) ? null : ((ComicBookGroupMatcher)cbm).Matchers))
			{
				string text = item.MatchValue.Trim();
				if (!string.IsNullOrEmpty(text))
				{
					if (!string.IsNullOrEmpty(name))
					{
						name += "/";
					}
					name += text;
				}
			}
			if (string.IsNullOrEmpty(name))
			{
				name = NumberedString.StripNumber(BookList.Name);
			}
			int number = NumberedString.MaxNumber(from cli in Library.ComicLists.GetItems<ComicListItem>()
												  where NumberedString.StripNumber(cli.Name) == name
												  select cli.Name);
			comicSmartListItem.Name = NumberedString.Format(name, number);
			if (clif == null)
			{
				Library.TemporaryFolder.Items.Add(comicSmartListItem);
			}
			else
			{
				clif.Items.Add(comicSmartListItem);
			}
		}

		private void SetListBackgroundImage(ComicBook cb)
		{
			SetListBackgroundImage(cb?.Id.ToString());
		}

		private void SetListBackgroundImage(string source)
		{
			backgroundImageSource = source;
			try
			{
				ThreadUtility.RunInBackground("Create library backdrop", delegate
				{
					bool flag = false;
					try
					{
						if (!base.IsDisposed && source != null)
						{
							ComicBook comicBook = Library.Books[new Guid(source)];
							if (comicBook != null)
							{
								using (IItemLock<ThumbnailImage> itemLock = Program.ImagePool.GetThumbnail(comicBook.GetFrontCoverThumbnailKey(), comicBook))
								{
									Bitmap bitmap = ComicBox3D.CreateDefaultBook(itemLock.Item.Bitmap, null, EngineConfiguration.Default.ListCoverSize, comicBook.PageCount);
									bitmap.ChangeAlpha(EngineConfiguration.Default.ListCoverAlpha);
									SetListBackgroundImage(bitmap);
									flag = true;
								}
							}
						}
					}
					catch (Exception)
					{
					}
					finally
					{
						if (!flag)
						{
							SetListBackgroundImage();
						}
					}
				});
			}
			catch
			{
				SetListBackgroundImage();
			}
		}

		private void SetListBackgroundImage(Image image)
		{
			if (!itemView.BeginInvokeIfRequired(delegate
			{
				SetListBackgroundImage(image);
			}) && itemView.BackgroundImage != image)
			{
				if (itemView.BackgroundImage != null && itemView.BackgroundImage != ListBackgroundImage)
				{
					Image backgroundImage = itemView.BackgroundImage;
					itemView.BackgroundImage = null;
					backgroundImage.Dispose();
				}
				if (image != null && itemView.BackColor.GetBrightness() >= 0.95f)
				{
					itemView.BackgroundImage = image;
					itemView.BackgroundImageAlignment = System.Drawing.ContentAlignment.BottomRight;
				}
			}
		}

		private void SetListBackgroundImage()
		{
			SetListBackgroundImage(ListBackgroundImage);
		}

		private void bookSelectorPanel_ItemDrag(object sender, ItemDragEventArgs e)
		{
			if (ComicEditMode.CanEditList())
			{
				Control control = (Control)sender;
				control.DoDragDrop(e.Item, DragDropEffects.Copy);
			}
		}

		private void DragGiveDragCursorFeedback(object sender, GiveFeedbackEventArgs e)
		{
			if (dragCursor != null && !(dragCursor.Cursor == null))
			{
				e.UseDefaultCursors = false;
				dragCursor.OverlayCursor = ((e.Effect == DragDropEffects.None) ? Cursors.No : Cursors.Default);
				dragCursor.OverlayEffect = ((e.Effect == DragDropEffects.Copy) ? BitmapCursorOverlayEffect.Plus : BitmapCursorOverlayEffect.None);
				Cursor.Current = dragCursor.Cursor;
			}
		}

		private void DragQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		{
			if (e.Action == DragAction.Drop)
			{
				Cursor.Current = Cursors.WaitCursor;
			}
		}

		private void itemView_ItemDrag(object sender, ItemDragEventArgs e)
		{
			if (!ComicEditMode.CanEditList())
			{
				return;
			}
			ComicBookContainer comicBookContainer = new ComicBookContainer();
			ComicBookGroupMatcher comicBookGroupMatcher = new ComicBookGroupMatcher();
			comicBookContainer.Books.AddRange(GetBookList(ComicBookFilterType.Selected));
			if (comicBookContainer.Books.Count == 0)
			{
				return;
			}
			if (itemView.IsStacked)
			{
				IViewableItem[] array = itemView.SelectedItems.Where((IViewableItem si) => itemView.IsStack(si)).ToArray();
				comicBookContainer.Name = array.Select((IViewableItem si) => itemView.GetStackCaption(si)).FirstOrDefault();
				SingleComicGrouper[] array2 = (from bg in itemView.ItemStacker.GetGroupers().OfType<IBookGrouper>()
											   select bg.BookGrouper).OfType<SingleComicGrouper>().ToArray();
				IViewableItem[] array3 = array;
				foreach (IViewableItem item in array3)
				{
					ComicBookGroupMatcher comicBookGroupMatcher2 = new ComicBookGroupMatcher
					{
						MatcherMode = MatcherMode.And
					};
					IGroupInfo stackGroupInfo = itemView.GetStackGroupInfo(item);
					IEnumerable<IGroupInfo> source;
					if (!(stackGroupInfo is ICompoundGroupInfo))
					{
						source = ListExtensions.AsEnumerable<IGroupInfo>(stackGroupInfo);
					}
					else
					{
						IEnumerable<IGroupInfo> infos = ((ICompoundGroupInfo)stackGroupInfo).Infos;
						source = infos;
					}
					IGroupInfo[] array4 = source.ToArray();
					for (int j = 0; j < array2.Length; j++)
					{
						ComicBookMatcher comicBookMatcher = array2[j].CreateMatcher(array4[j]);
						if (comicBookMatcher != null)
						{
							comicBookGroupMatcher2.Matchers.Add(comicBookMatcher);
						}
					}
					if (comicBookGroupMatcher2.Matchers.Count > 1)
					{
						comicBookGroupMatcher.Matchers.Add(comicBookGroupMatcher2);
					}
					else if (comicBookGroupMatcher2.Matchers.Count == 1)
					{
						comicBookGroupMatcher.Matchers.Add(comicBookGroupMatcher2.Matchers[0]);
					}
				}
			}
			dragCursor = itemView.GetDragCursor(Program.ExtendedSettings.DragDropCursorAlpha);
			try
			{
				IEditableComicBookListProvider editableComicBookListProvider = BookList.QueryService<IEditableComicBookListProvider>();
				bool flag = editableComicBookListProvider != null;
				bool flag2 = editableComicBookListProvider?.IsLibrary ?? false;
				bool flag3 = !flag2 && flag;
				DragDropEffects dragDropEffects = DragDropEffects.Copy;
				itemView.AllowDrop = !flag2 && flag && IsViewSortedByPosition();
				if (flag3)
				{
					dragDropEffects |= DragDropEffects.Move;
				}
				DataObject dataObject = new DataObject();
				dataObject.SetData(comicBookContainer);
				StringCollection stringCollection = new StringCollection();
				stringCollection.AddRange(comicBookContainer.GetBookFiles().ToArray());
				dataObject.SetFileDropList(stringCollection);
				if (comicBookGroupMatcher.Matchers.Count > 0)
				{
					dataObject.SetData(ComicBookMatcher.ClipboardFormat, (comicBookGroupMatcher.Matchers.Count == 1) ? comicBookGroupMatcher.Matchers[0] : comicBookGroupMatcher);
				}
				ownDrop = false;
				DragDropEffects dragDropEffects2 = itemView.DoDragDrop(dataObject, dragDropEffects);
				if (dragDropEffects2 != DragDropEffects.Move || editableComicBookListProvider == null || ownDrop)
				{
					return;
				}
				foreach (ComicBook book in comicBookContainer.Books)
				{
					editableComicBookListProvider.Remove(book);
				}
			}
			finally
			{
				if (dragCursor != null)
				{
					dragCursor.Dispose();
					dragCursor = null;
				}
				itemView.AllowDrop = true;
			}
		}

		private bool CreateDragContainter(DragEventArgs e)
		{
			if (dragBookContainer == null)
			{
				dragBookContainer = DragDropContainer.Create(e.Data);
			}
			return dragBookContainer.IsValid;
		}

		private void InsertBooksIntoToList(IEditableComicBookListProvider list, int index)
		{
			if (list == null)
			{
				return;
			}
			if (list.IsLibrary && dragBookContainer.IsFilesContainer)
			{
				Program.Scanner.ScanFilesOrFolders(dragBookContainer.FilesOrFolders, all: true, removeMissing: false);
			}
			else
			{
				if (!dragBookContainer.IsBookContainer)
				{
					return;
				}
				foreach (ComicBook book in dragBookContainer.Books.GetBooks())
				{
					ComicBook comicBook = Program.BookFactory.Create(book.FilePath, CreateBookOption.AddToStorage);
					if (comicBook != null)
					{
						if (index != -1)
						{
							index = list.Insert(index, comicBook) + 1;
						}
						else
						{
							list.Add(comicBook);
						}
					}
				}
				FillBookList();
			}
		}

		private bool IsViewSortedByPosition()
		{
			if (itemView.SortColumn != null && itemView.SortColumn.Id == 100 && itemView.ItemSortOrder == SortOrder.Ascending && itemView.GroupColumn == null)
			{
				return !itemView.IsStacked;
			}
			return false;
		}

		private void SetDropEffects(DragEventArgs e)
		{
			IEditableComicBookListProvider editableComicBookListProvider = BookList.QueryService<IEditableComicBookListProvider>();
			e.Effect = DragDropEffects.None;
			if (ComicEditMode.CanEditList() && editableComicBookListProvider != null && CreateDragContainter(e))
			{
				if (dragCursor != null && IsViewSortedByPosition())
				{
					e.Effect = DragDropEffects.Move;
				}
				else if (editableComicBookListProvider.IsLibrary && dragBookContainer.IsFilesContainer)
				{
					e.Effect = DragDropEffects.Link;
				}
				else
				{
					e.Effect = e.AllowedEffect;
				}
			}
			if (e.Effect == DragDropEffects.None)
			{
				itemView.MarkerVisible = false;
				return;
			}
			Point pt = itemView.PointToClient(new Point(e.X, e.Y));
			CoverViewItem coverViewItem = itemView.ItemHitTest(pt) as CoverViewItem;
			if (coverViewItem != null && IsViewSortedByPosition() && dragBookContainer.IsBookContainer)
			{
				itemView.MarkerItem = coverViewItem;
				itemView.MarkerVisible = true;
			}
			else
			{
				itemView.MarkerVisible = false;
			}
		}

		private void itemView_DragEnter(object sender, DragEventArgs e)
		{
			SetDropEffects(e);
		}

		private void itemView_DragOver(object sender, DragEventArgs e)
		{
			SetDropEffects(e);
		}

		private void itemView_DragDrop(object sender, DragEventArgs e)
		{
			Point pt = itemView.PointToClient(new Point(e.X, e.Y));
			CoverViewItem coverViewItem = itemView.ItemHitTest(pt) as CoverViewItem;
			int index = -1;
			if (coverViewItem != null && IsViewSortedByPosition())
			{
				index = coverViewItem.Position - 1;
			}
			InsertBooksIntoToList(BookList.QueryService<IEditableComicBookListProvider>(), index);
			itemView.MarkerVisible = false;
			dragBookContainer = null;
			ownDrop = true;
		}

		private void itemView_DragLeave(object sender, EventArgs e)
		{
			itemView.MarkerVisible = false;
			dragBookContainer = null;
		}

		public virtual ItemSizeInfo GetItemSize()
		{
			switch (itemView.ItemViewMode)
			{
				case ItemViewMode.Thumbnail:
					return new ItemSizeInfo(FormUtility.ScaleDpiY(Program.MinThumbHeight), FormUtility.ScaleDpiY(Program.MaxThumbHeight), itemView.ItemThumbSize.Height);
				case ItemViewMode.Tile:
					return new ItemSizeInfo(FormUtility.ScaleDpiY(Program.MinTileHeight), FormUtility.ScaleDpiY(Program.MaxTileHeight), itemView.ItemTileSize.Height);
				case ItemViewMode.Detail:
					return new ItemSizeInfo(FormUtility.ScaleDpiY(Program.MinRowHeight), FormUtility.ScaleDpiY(Program.MaxRowHeight), itemView.ItemRowHeight);
				default:
					return null;
			}
		}

		public void SetItemSize(int height)
		{
			switch (itemView.ItemViewMode)
			{
				case ItemViewMode.Thumbnail:
					height = height.Clamp(FormUtility.ScaleDpiY(Program.MinThumbHeight), FormUtility.ScaleDpiY(Program.MaxThumbHeight));
					itemView.ItemThumbSize = new Size(height, height);
					break;
				case ItemViewMode.Tile:
					height = height.Clamp(FormUtility.ScaleDpiY(Program.MinTileHeight), FormUtility.ScaleDpiY(Program.MaxTileHeight));
					itemView.ItemTileSize = new Size(height * 2, height);
					break;
				case ItemViewMode.Detail:
					height = height.Clamp(FormUtility.ScaleDpiY(Program.MinRowHeight), FormUtility.ScaleDpiY(Program.MaxRowHeight));
					itemView.ItemRowHeight = height;
					break;
			}
		}

		public Guid GetBookListId()
		{
			if (BookList != null)
			{
				return BookList.Id;
			}
			return Guid.Empty;
		}

		public void RefreshInformation()
		{
			IEnumerable<CoverViewItem> selectedItems = from CoverViewItem cvi in new List<IViewableItem>(itemView.SelectedItems)
														select cvi;

			foreach (CoverViewItem item in selectedItems)
			{
				if (item.Comic.IsDynamicSource)
				{
					base.Main.UpdateWebComic(item.Comic, fullRefresh: true);
				}
				else
				{
					item.RefreshImage();
				}
			}
			if (ComicEditMode.CanScan())
			{
				// If Control is pressed, force a refresh of the information, otherwise only refresh if the comic is in the library.
				bool forceRefreshInfo = Control.ModifierKeys == Keys.Control;
				IEnumerable<CoverViewItem> itemToRefresh = forceRefreshInfo ? selectedItems : selectedItems.Where(cvi => cvi.Comic.IsInContainer);

				Program.Scanner.ScanFilesOrFolders(itemToRefresh.Select((CoverViewItem cvi) => cvi.Comic.FilePath), all: false, removeMissing: false, forceRefreshInfo: forceRefreshInfo);
			}
		}

		public void MarkSelectedRead()
		{
			foreach (ComicBook book in GetBookList(ComicBookFilterType.Library | ComicBookFilterType.Selected, asArray: true))
			{
				book.MarkAsRead();
			}
		}

		public void MarkSelectedNotRead()
		{
			foreach (ComicBook book in GetBookList(ComicBookFilterType.Library | ComicBookFilterType.Selected, asArray: true))
			{
				book.MarkAsNotRead();
			}
		}

		public void MarkSelectedChecked()
		{
			foreach (ComicBook book in GetBookList(ComicBookFilterType.Library | ComicBookFilterType.Selected, asArray: true))
			{
				book.Checked = true;
			}
		}

		public void MarkSelectedUnchecked()
		{
			foreach (ComicBook book in GetBookList(ComicBookFilterType.Library | ComicBookFilterType.Selected, asArray: true))
			{
				book.Checked = false;
			}
		}

		public void ShowWeb()
		{
			CoverViewItem coverViewItem = itemView.FocusedItem as CoverViewItem;
			if (coverViewItem != null && !string.IsNullOrEmpty(coverViewItem.Comic.Web))
			{
				Program.StartDocument(coverViewItem.Comic.Web);
			}
		}

		public void OpenComic()
		{
			CoverViewItem coverViewItem = itemView.FocusedItem as CoverViewItem;
			if (coverViewItem != null && base.Main != null)
			{
				coverViewItem.Comic.LastOpenedFromListId = BookList.Id;
				base.Main.OpenBooks.Open(coverViewItem.Comic, Program.Settings.OpenInNewTab ^ ((Control.ModifierKeys & Keys.Control) != 0 || (itemView.ActivateButton & MouseButtons.Middle) != 0));
			}
		}

		public void OpenComicNewTab()
		{
			CoverViewItem coverViewItem = itemView.FocusedItem as CoverViewItem;
			if (coverViewItem != null && base.Main != null)
			{
				coverViewItem.Comic.LastOpenedFromListId = BookList.Id;
				base.Main.OpenBooks.Open(coverViewItem.Comic, inNewSlot: true);
			}
		}

		public void EditItem()
		{
			if (itemView.LabelEdit && itemView.ItemViewMode == ItemViewMode.Detail && itemView.SelectedCount != 0)
			{
				if (Machine.Ticks - contextMenuCloseTime < 250)
				{
					itemView.EditItem(contextMenuMouseLocation);
				}
				else
				{
					itemView.EditItem(itemView.SelectedItems.FirstOrDefault());
				}
			}
		}

		public void CopyListSetup()
		{
			Clipboard.SetDataObject(new DisplayListConfig(ViewConfig, ThumbnailConfig, null, stacksConfig, backgroundImageSource));
		}

		public void PasteListSetup()
		{
			try
			{
				IDataObject dataObject = Clipboard.GetDataObject();
				if (dataObject != null)
				{
					DisplayListConfig displayListConfig = dataObject.GetData(typeof(DisplayListConfig)) as DisplayListConfig;
					if (displayListConfig != null)
					{
						itemView.ViewConfig = displayListConfig.View;
						ThumbnailConfig = displayListConfig.Thumbnail;
						stacksConfig = displayListConfig.StackConfig;
						FillBookList();
						SetListBackgroundImage(displayListConfig.BackgroundImageSource);
					}
				}
			}
			catch
			{
			}
		}

		public void RevealInExplorer()
		{
			ComicBook comicBook = GetBookList(ComicBookFilterType.IsNotFileless | ComicBookFilterType.Selected).FirstOrDefault();
			if (comicBook != null)
			{
				Program.ShowExplorer(comicBook.FilePath);
			}
		}

		public void ShowBook(ComicBook comicBook)
		{
			foreach (CoverViewItem item in itemView.Items)
			{
				if (item.Comic == comicBook)
				{
					item.Selected = true;
					item.Focused = true;
					item.EnsureVisible();
					break;
				}
			}
		}

		public void UpdateQuickFilter()
		{
			quickFilter = null;
			if (QuickSearchType == ComicBookAllPropertiesMatcher.MatcherOption.All)
			{
				try
				{
					string text = QuickSearch?.Trim() ?? "";
					if (text.StartsWith("NOT", StringComparison.OrdinalIgnoreCase) || text.StartsWith("MATCH", StringComparison.OrdinalIgnoreCase))
					{
						quickFilter = ComicBookGroupMatcher.CreateMatcherFromQuery(ComicSmartListItem.TokenizeQuery(text));
					}
				}
				catch
				{
				}
			}
			if (quickFilter == null)
			{
				quickFilter = ComicBookAllPropertiesMatcher.Create(QuickSearch, 3, QuickSearchType, ShowOptionType, ShowComicType);
			}
		}

		public void UpdateSearch()
		{
			UpdateQuickFilter();
			FillBookList();
			displayOptionPanel.Visible = ShowOptionType != 0 || ShowComicType != 0 || ShowOnlyDuplicates;
		}

		public void RemoveBooks(IEnumerable<ComicBook> books)
		{
			IRemoveBooks removeBooks = BookList.QueryService<IRemoveBooks>();
			if (removeBooks != null)
			{
				ItemView.BeginUpdate();
				try
				{
					removeBooks.RemoveBooks(books, Control.ModifierKeys != Keys.Control);
				}
				finally
				{
					ItemView.EndUpdate();
				}
			}
		}

		private bool CanRemoveBooks()
		{
			if (itemView.InplaceEditItem == null && ComicEditMode.CanDeleteComics() && BookList != null)
			{
				return BookList.QueryService<IRemoveBooks>() != null;
			}
			return false;
		}

		private void RemoveBooks()
		{
			RemoveBooks(GetBookList(ComicBookFilterType.Selected, asArray: true));
		}

		public void CopyComicData()
		{
			try
			{
				((CoverViewItem)itemView.SelectedItems.First()).Comic.ToClipboard();
			}
			catch (Exception)
			{
			}
		}

		public void ClearComicData()
		{
			if (Program.AskQuestion(this, TR.Messages["AskClearComicData", "Do you want to remove all entered data from the selected Books (can be reverted with Undo)?"], TR.Default["Clear"], HiddenMessageBoxes.AskClearData, null, TR.Default["No"]))
			{
				Program.Database.Undo.SetMarker(miClearData.Text);
				GetBookList(ComicBookFilterType.Selected).ForEachProgress(delegate (ComicBook cb)
				{
					cb.ResetProperties();
				}, this, TR.Messages["ClearComicData", "Clear Books"], TR.Messages["ClearComicDataText", "Removing all entered data from the Books"]);
			}
		}

		public void UpdateFiles()
		{
			GetBookList(ComicBookFilterType.IsNotFileless | ComicBookFilterType.Selected).ForEach(delegate (ComicBook cb)
			{
				Program.QueueManager.AddBookToFileUpdate(cb, alwaysWrite: true);
			});
		}

		private bool isPasteComicDataEnabled()
		{
			//Check if the clipboard contains data, if so enable the paste button.
			try
			{
				return ComicEditMode.CanEditProperties() && Clipboard.ContainsData(ComicBook.ClipboardFormat) && !GetBookList(ComicBookFilterType.Selected).IsEmpty();
			}
			catch (Exception)
			{
				return false;
			}
		}

		public void PasteComicData()
		{
			try
			{
				ComicBook comicBook = Clipboard.GetData(ComicBook.ClipboardFormat) as ComicBook;
				IEnumerable<ComicBook> enumerable = GetBookList(ComicBookFilterType.Selected, asArray: true);
				if (comicBook != null && !enumerable.IsEmpty())
				{
					ComicDataPasteDialog.ShowAndPaste(this, comicBook, enumerable);
				}
			}
			catch
			{
			}
		}

		private void tsListLayouts_DropDownOpening(object sender, EventArgs e)
		{
			base.Main.UpdateListConfigMenus(tsListLayouts.DropDownItems);
		}

		private void contextMenuItems_Opening(object sender, CancelEventArgs e)
		{
			IEnumerable<ComicBook> enumerable = GetBookList(ComicBookFilterType.Selected);
			IEnumerable<ComicBook> enumerable2 = GetBookList(ComicBookFilterType.Library | ComicBookFilterType.Selected);
			IEnumerable<ComicBook> list = GetBookList(ComicBookFilterType.NotInLibrary | ComicBookFilterType.Selected);
			CoverViewItem coverViewItem = itemView.FocusedItem as CoverViewItem;
			bool flag = ComicEditMode.CanEditProperties();
			bool flag2 = ComicEditMode.CanEditList();
			bool flag3 = !enumerable2.IsEmpty();
			miAddLibrary.Visible = !list.IsEmpty();
			miEdit.Visible = flag && itemView.ItemViewMode == ItemViewMode.Detail;
			miShowWeb.Visible = coverViewItem != null && coverViewItem.Comic != null && !string.IsNullOrEmpty(coverViewItem.Comic.Web);
			ToolStripMenuItem toolStripMenuItem = miMarkAs;
			bool visible = (miRateMenu.Visible = flag && flag3);
			toolStripMenuItem.Visible = visible;
			miResetTopOfStack.Visible = !(miSetTopOfStack.Visible = openStackPanel.Visible) && itemView.IsStack(itemView.SelectedItems.FirstOrDefault());
			miSetStackThumbnail.Visible = itemView.IsStack(itemView.SelectedItems.FirstOrDefault());
			miRemoveStackThumbnail.Visible = stacksConfig != null && !string.IsNullOrEmpty(stacksConfig.GetStackCustomThumbnail(itemView.GetStackCaption(itemView.SelectedItems.FirstOrDefault())));
			ComicLibrary comicLibrary = enumerable2.Select((ComicBook cb) => cb.Container as ComicLibrary).FirstOrDefault();
			miAddList.Visible = comicLibrary != null && !comicLibrary.ComicLists.GetItems<ComicIdListItem>().IsEmpty() && flag3;
			miEditList.Visible = CanReorderList(mustBeOrdered: false);
			miExportComics.Visible = ComicEditMode.CanExport();
			miSetListBackground.Visible = BookList is ComicListItem;
			ToolStripMenuItem toolStripMenuItem2 = miCopyData;
			visible = (miPasteData.Visible = ComicEditMode.CanEditProperties());
			toolStripMenuItem2.Visible = visible;
			FormUtility.SafeToolStripClear(miShowOnly.DropDownItems);
			for (int j = 0; j < 3; j++)
			{
				SearchBrowserControl.SelectionEntry selectionColumn = bookSelectorPanel.GetSelectionColumn(j);
				string text = null;
				if (selectionColumn == null)
				{
					continue;
				}
				foreach (ComicBook item in enumerable)
				{
					string stringPropertyValue = item.GetStringPropertyValue(selectionColumn.Property);
					if (text == null)
					{
						text = stringPropertyValue;
					}
					else if (text != stringPropertyValue)
					{
						text = null;
						break;
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					int i = j;
					miShowOnly.DropDownItems.Add(string.Format("{0} '{1}'", selectionColumn.Caption, text.Ellipsis(60 - selectionColumn.Caption.Length, "...")), null, delegate
					{
						SearchBrowserVisible = true;
						bookSelectorPanel.SelectEntry(i, text);
					});
				}
			}
			miShowOnly.Visible = miShowOnly.DropDownItems.Count != 0;
			miUpdateComicFiles.Visible = enumerable.Any((ComicBook cb) => cb.ComicInfoIsDirty);
			contextMenuItems.FixSeparators();
			miPasteData.Enabled = isPasteComicDataEnabled();
		}

		private void contextOpenWith_Opening(object sender, CancelEventArgs e)
		{
			UpdateOpenWithMenus(GetBookList(ComicBookFilterType.Selected));
		}

		private void UpdateOpenWithMenus(IEnumerable<ComicBook> enumerable)
		{
			FormUtility.SafeToolStripClear(miOpenWith.DropDownItems, 2);

			//Populate Open With menu
			for (int k = 0; k < Program.Settings.ExternalPrograms.Count; k++)
			{
				ExternalProgram ep = Program.Settings.ExternalPrograms[k];

				if (string.IsNullOrEmpty(ep.Name) || string.IsNullOrEmpty(ep.Path) || !File.Exists(ep.Path))
					continue;

				//Create menu item
				ToolStripItem tsi = new ToolStripMenuItem(ep.Name.Ellipsis(60 - ep.Name.Length, "..."), null, delegate
				{
					//For each book selected it will start a separate program
					foreach (ComicBook comicBook in enumerable)
					{
						StartExternalProgram(ep, comicBook);
					}
				});
				tsi.Tag = ep;
				tsi.Enabled = AllSelectedLinked();

				//Insert menu item
				contextOpenWith.Items.Add(tsi);
			}

			//Show the separator only if at least 1 external program was added
			toolStripSeparator2.Visible = contextOpenWith.Items.Count > 2;
		}

		private static void StartExternalProgram(ExternalProgram ep, ComicBook comicBook)
		{
			string args = string.IsNullOrEmpty(ep.Arguments) ? $"\"{comicBook.FilePath}\"" : $"{ep.Arguments} \"{comicBook.FilePath}\"";
			Program.StartProgram(ep.Path, args);
		}

		private void LayoutMenuOpening(object sender, CancelEventArgs e)
		{
			try
			{
				IDataObject dataObject = Clipboard.GetDataObject();
				miPasteListSetup.Enabled = dataObject?.GetDataPresent(typeof(DisplayListConfig)) ?? false;
			}
			catch
			{
				miPasteListSetup.Enabled = false;
			}
		}

		private void contextExport_Opening(object sender, CancelEventArgs e)
		{
			while (contextExport.Items.Count > 2)
			{
				ToolStripItem toolStripItem = contextExport.Items[2];
				contextExport.Items.RemoveAt(2);
				toolStripItem.Dispose();
			}
			bool enabled = AllSelectedLinked();
			AddConverterEntries(Program.ExportComicRackPresets, enabled);
			AddConverterEntries(Program.Settings.ExportUserPresets, enabled);
		}

		private void AddConverterEntries(ICollection<ExportSetting> converterSettingCollection, bool enabled)
		{
			if (converterSettingCollection.Count == 0)
			{
				return;
			}
			contextExport.Items.Add(new ToolStripSeparator());
			foreach (ExportSetting item in converterSettingCollection)
			{
				ExportSetting copy = item;
				contextExport.Items.Add(item.Name, null, delegate
				{
					base.Main.ConvertComic(GetBookList(ComicBookFilterType.Selected, asArray: true), copy);
				}).Enabled = enabled;
			}
		}

		private void miAddList_DropDownOpening(object sender, EventArgs e)
		{
			int listMenuSize = Program.ExtendedSettings.ListMenuSize;
			ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;
			int num = 0;
			IEnumerable<ComicBook> selectedBooks = GetBookList(ComicBookFilterType.Library | ComicBookFilterType.Selected);
			ComicLibrary comicLibrary = selectedBooks.Select((ComicBook cb) => cb.Container as ComicLibrary).FirstOrDefault();
			if (comicLibrary == null || !comicLibrary.EditMode.CanEditList())
			{
				return;
			}
			FormUtility.SafeToolStripClear(toolStripMenuItem.DropDownItems);
			foreach (ComicIdListItem item in from l in comicLibrary.ComicLists.GetItems<ComicIdListItem>()
											 where l != BookList
											 select l)
			{
				if (++num == listMenuSize + 1)
				{
					break;
				}
				ComicIdListItem li = item;
				int childLevel = comicLibrary.ComicLists.GetChildLevel((ComicListItem)li);
				string str = new string(' ', childLevel * 4);
				if (num == listMenuSize)
				{
					toolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem("...")
					{
						Enabled = false
					});
				}
				else
				{
					toolStripMenuItem.DropDownItems.Add(str + li.Name, GetComicListImage(li), delegate
					{
						li.AddRange(selectedBooks);
					});
				}
			}
			AddNoneEntry(toolStripMenuItem.DropDownItems);
		}

		private void tbbDuplicateList_DropDownOpening(object sender, EventArgs e)
		{
			if (Library == null)
			{
				return;
			}
			ToolStripDropDownItem toolStripDropDownItem = (ToolStripDropDownItem)sender;
			FormUtility.SafeToolStripClear(toolStripDropDownItem.DropDownItems);
			foreach (ComicListItemFolder item in Library.ComicLists.GetItems<ComicListItemFolder>())
			{
				ComicListItemFolder li = item;
				int childLevel = Library.ComicLists.GetChildLevel((ComicListItem)li);
				string str = new string(' ', childLevel * 4);
				toolStripDropDownItem.DropDownItems.Add(str + li.Name, GetComicListImage(li), delegate
				{
					DuplicateList(li);
				});
			}
			AddNoneEntry(toolStripDropDownItem.DropDownItems);
		}

		private void miShowInList_DropDownOpening(object sender, EventArgs e)
		{
			if (Library == null)
			{
				return;
			}
			int maxLists = Program.ExtendedSettings.ListMenuSize;
			ToolStripMenuItem menu = (ToolStripMenuItem)sender;
			ComicBook cb = GetBookList(ComicBookFilterType.Selected).FirstOrDefault();
			if (cb == null)
			{
				return;
			}
			try
			{
				buildMenuThread.Join(5000);
			}
			catch
			{
			}
			FormUtility.SafeToolStripClear(menu.DropDownItems);
			ToolStripItem dummy = menu.DropDownItems.Add(TR.Load(base.Name)["SearchingLists", "Searching list..."]);
			dummy.Enabled = false;
			abortBuildMenu.Reset();
			buildMenuThread = ThreadUtility.RunInBackground("Build list menu", delegate
			{
				try
				{
					Library.ComicListsLocked = true;
					int count = 0;
					foreach (ComicListItem item in from l in Library.ComicLists.GetItems<ComicListItem>()
												   where l.GetBooks().Contains(cb)
												   select l)
					{
						if (abortBuildMenu.WaitOne(0))
							return;

						if (++count == maxLists + 1)
							break;

						ComicListItem li = item;
						string prefix = new string(' ', Library.ComicLists.GetChildLevel(li) * 4);
						this.Invoke(delegate
						{
							ToolStripMenuItem value = ((count != maxLists)
								? new ToolStripMenuItem(prefix + li.Name.Replace("&", "&&"), GetComicListImage(li), delegate { ShowBookInList(li, cb); })
								: new ToolStripMenuItem("...") { Enabled = false });
							menu.DropDownItems.Insert(menu.DropDownItems.Count - 1, value);
						});
					}
					this.Invoke(delegate
					{
						menu.DropDownItems.Remove(dummy);
					});
				}
				catch (Exception)
				{
				}
				finally
				{
					Library.ComicListsLocked = false;
				}
			});
		}

		private void miShowInList_DropDownClosed(object sender, EventArgs e)
		{
			abortBuildMenu.Set();
		}

		private void ShowBookInList(ComicListItem list, ComicBook cb)
		{
			if (base.Main != null)
			{
				base.Main.ShowBookInList(Library, list, cb, switchToList: true);
			}
		}

		private void AddNoneEntry(ToolStripItemCollection ic)
		{
			if (ic.Count == 0)
			{
				ic.Add(TR.Default["None", "None"]).Enabled = false;
			}
		}

		private Image GetComicListImage(ComicListItem cli)
		{
			switch (cli.ImageKey)
			{
				case "Library":
					return Resources.Library;
				case "Folder":
					return Resources.SearchFolder;
				case "Search":
					return Resources.SearchDocument;
				case "List":
					return Resources.List;
				case "TempFolder":
					return Resources.TempFolder;
				default:
					return null;
			}
		}

		public IEnumerable<ComicBook> GetBookList(ComicBookFilterType cbft, bool asArray)
		{
			if (asArray)
			{
				cbft |= ComicBookFilterType.AsArray;
			}
			return GetBookList(cbft);
		}

		protected virtual void OnCurrentBookListChanged()
		{
			bookSelectorPanel.ClearNot();
			bookListDirty = true;
			if (this.CurrentBookListChanged != null)
			{
				this.CurrentBookListChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnQuickSearchChanged()
		{
			if (tsQuickSearch.TextBox.Text != quickSearch)
			{
				tsQuickSearch.TextBox.Text = quickSearch;
			}
			if (!blockQuickSearchUpdate)
			{
				quickSearchTimer.Stop();
				quickSearchTimer.Start();
				if (this.QuickSearchChanged != null)
				{
					this.QuickSearchChanged(this, EventArgs.Empty);
				}
			}
		}

		protected virtual void OnSearchBrowserVisibleChanged()
		{
			IMatcher<ComicBook> currentMatcher = bookSelectorPanel.CurrentMatcher;
			if (searchBrowserContainer.Expanded)
			{
				searchBrowserContainer.Expanded = SearchBrowserVisible;
			}
			FillBookSelector();
			if (!searchBrowserContainer.Expanded)
			{
				searchBrowserContainer.Expanded = SearchBrowserVisible;
			}
			FillBookList();
			if (this.SearchBrowserVisibleChanged != null)
			{
				this.SearchBrowserVisibleChanged(this, EventArgs.Empty);
			}
		}

		private void toolTip_Popup(object sender, PopupEventArgs e)
		{
			e.ToolTipSize = new Size(360, 120);
			e.Cancel = !Program.Settings.ShowToolTips || toolTipItem == null || itemView.ItemViewMode == ItemViewMode.Tile;
		}

		private void toolTip_Draw(object sender, DrawToolTipEventArgs e)
		{
			if (toolTipItem == null)
			{
				return;
			}
			using (IItemLock<ThumbnailImage> itemLock = toolTipItem.GetThumbnail(memoryOnly: true))
			{
				ComicBook comic = toolTipItem.Comic;
				VisualStyleElement normal = VisualStyleElement.ToolTip.Standard.Normal;
				if (VisualStyleRenderer.IsSupported && VisualStyleRenderer.IsElementDefined(normal))
				{
					VisualStyleRenderer visualStyleRenderer = new VisualStyleRenderer(normal);
					visualStyleRenderer.DrawBackground(e.Graphics, e.Bounds);
				}
				else
				{
					e.DrawBackground();
					e.DrawBorder();
				}
				Rectangle bounds = e.Bounds;
				bounds.Inflate(-10, -10);
				ThumbTileRenderer.DrawTile(e.Graphics, bounds, itemLock.Item.GetThumbnail(bounds.Height), comic, FC.GetRelative(Font, 1.2f), SystemColors.InfoText, Color.Transparent, ThumbnailDrawingOptions.DefaultWithoutBackground, ComicTextElements.DefaultComic, threeD: false);
			}
		}

		private void foundView_MouseLeave(object sender, EventArgs e)
		{
			toolTip.SetToolTip(this, null);
		}

		private void foundView_MouseHover(object sender, EventArgs e)
		{
			Point point = itemView.PointToClient(Cursor.Position);
			toolTipItem = itemView.ItemHitTest(point.X, point.Y) as CoverViewItem;
			if (toolTipItem != null && toolTipItem.Comic != null)
			{
				string text = toolTipItem.Comic.Id.ToString();
				if (toolTip.GetToolTip(itemView) != text && Program.Settings.ShowToolTips)
				{
					toolTip.SetToolTip(itemView, text);
				}
			}
		}

		private void foundView_MouseMove(object sender, MouseEventArgs e)
		{
			itemView.ResetMouse();
		}

		public void RefreshDisplay()
		{
			AutomaticProgressDialog.Process(this, TR.Messages["GettingList", "Getting Books List"], TR.Messages["GettingListText", "Retrieving all Books from the selected folder"], 1000, BookList.Refresh, AutomaticProgressDialogOptions.EnableCancel);
			FillBookList();
		}

		public void SettingsChanged()
		{
			SetCustomColumns();
			SetVirtualTagColumns();
			FillBookList();
		}

		public void FocusQuickSearch()
		{
			tsQuickSearch.TextBox.Focus();
		}

		public void SetWorkspace(DisplayWorkspace ws)
		{
			tsQuickSearch.TextBox.AutoCompleteList.AddRange(Program.Settings.QuickSearchList.ToArray());
		}

		public void StoreWorkspace(DisplayWorkspace ws)
		{
			if (bookList == null)
			{
				return;
			}
			UpdateViewConfig();
			if (!base.Disposing && tsQuickSearch.TextBox != null)
			{
				try
				{
					HashSet<string> collection = new HashSet<string>(tsQuickSearch.TextBox.AutoCompleteList.Cast<string>());
					Program.Settings.QuickSearchList.Clear();
					Program.Settings.QuickSearchList.AddRange(collection);
				}
				catch
				{
				}
			}
		}

		private void UpdateViewConfig()
		{
			if (DisableViewConfigUpdate)
			{
				return;
			}
			IDisplayListConfig displayListConfig = bookList.QueryService<IDisplayListConfig>();
			if (displayListConfig != null)
			{
				CoverViewItem coverViewItem = stackItem as CoverViewItem;
				if (coverViewItem != null)
				{
					displayListConfig.Display = new DisplayListConfig(preStackConfig, ThumbnailConfig, null, stacksConfig, backgroundImageSource)
					{
						ScrollPosition = preStackScrollPosition,
						FocusedComicId = preStackFocusedId,
						StackedComicId = coverViewItem.Comic.Id,
						StackScrollPosition = itemView.ScrollPosition,
						StackFocusedComicId = GetFocusedId()
					};
				}
				else
				{
					displayListConfig.Display = new DisplayListConfig(itemView.ViewConfig, ThumbnailConfig, null, stacksConfig, backgroundImageSource)
					{
						ScrollPosition = itemView.ScrollPosition,
						FocusedComicId = GetFocusedId()
					};
				}
				displayListConfig.Display.QuickSearch = QuickSearch;
				displayListConfig.Display.QuickSearchType = QuickSearchType;
				displayListConfig.Display.ShowOptionType = ShowOptionType;
				displayListConfig.Display.ShowComicType = ShowComicType;
				displayListConfig.Display.ShowOnlyDuplicates = ShowOnlyDuplicates;
				displayListConfig.Display.ShowGroupHeaders = ShowGroupHeaders;
				displayListConfig.Display.ShowGroupHeadersWidth = ((newGroupListWidth != 0) ? newGroupListWidth : (browserContainer.ClientRectangle.Width - browserContainer.SplitterDistance));
			}
		}

		private Guid GetFocusedId()
		{
			return ((itemView.FocusedItem ?? itemView.SelectedItems.FirstOrDefault()) as CoverViewItem)?.Comic.Id ?? Guid.Empty;
		}

		private void SetFocusedItem(Guid id)
		{
			if (!(id == Guid.Empty))
			{
				CoverViewItem coverViewItem = itemView.DisplayedItems.OfType<CoverViewItem>().FirstOrDefault((CoverViewItem ci) => ci.Comic.Id == id);
				if (coverViewItem != null)
				{
					coverViewItem.Focused = true;
					coverViewItem.Selected = true;
					coverViewItem.EnsureVisible();
				}
			}
		}

		public bool SelectComic(ComicBook comic)
		{
			CloseStack(withUpdate: true);
			CoverViewItem coverViewItem = itemView.DisplayedItems.OfType<CoverViewItem>().FirstOrDefault((CoverViewItem c) => (!itemView.IsStack(c)) ? (c.Comic == comic) : (itemView.GetStackItems(c).OfType<CoverViewItem>().FirstOrDefault((CoverViewItem sc) => sc.Comic == comic) != null));
			if (coverViewItem == null)
			{
				if (string.IsNullOrEmpty(QuickSearch) && ShowOptionType == ComicBookAllPropertiesMatcher.ShowOptionType.All && ShowComicType == ComicBookAllPropertiesMatcher.ShowComicType.All && !ShowOnlyDuplicates)
				{
					return false;
				}
				QuickSearch = string.Empty;
				ShowOptionType = ComicBookAllPropertiesMatcher.ShowOptionType.All;
				ShowComicType = ComicBookAllPropertiesMatcher.ShowComicType.All;
				ShowOnlyDuplicates = false;
				UpdateSearch();
				return SelectComic(comic);
			}
			if (itemView.IsStack(coverViewItem))
			{
				OpenStack(coverViewItem);
				coverViewItem = itemView.DisplayedItems.OfType<CoverViewItem>().FirstOrDefault((CoverViewItem c) => c.Comic == comic);
			}
			itemView.SelectAll(selectionState: false);
			if (coverViewItem != null)
			{
				coverViewItem.Selected = true;
				coverViewItem.Focused = true;
				coverViewItem.EnsureVisible();
			}
			return true;
		}

		public bool SelectComics(IEnumerable<ComicBook> books)
		{
			bool flag = true;
			HashSet<ComicBook> hashSet = new HashSet<ComicBook>(books);
			UpdatePending();
			itemView.SelectAll(selectionState: false);
			foreach (CoverViewItem displayedItem in itemView.DisplayedItems)
			{
				displayedItem.Selected = hashSet.Contains(displayedItem.Comic);
				if (flag && displayedItem.Selected)
				{
					displayedItem.EnsureVisible();
				}
				flag = false;
			}
			return true;
		}

		public IEnumerable<ComicBook> GetBookList(ComicBookFilterType cbft)
		{
			IEnumerable<ComicBook> books = ((!cbft.HasFlag(ComicBookFilterType.Selected)) ? (from CoverViewItem vi in itemView.DisplayedItems
																							 select vi.Comic) : (from CoverViewItem vi in itemView.SelectedItems
																												 select vi.Comic));
			books = ComicBookCollection.Filter(cbft, books);
			if (cbft.HasFlag(ComicBookFilterType.Sorted) && itemView.ItemSortOrder == SortOrder.Descending)
			{
				books = books.Reverse();
			}
			return books;
		}

		private void miOpenWithManager_Click(object sender, EventArgs e)
		{
			EditOpenWithPrograms();
		}

		private void EditOpenWithPrograms()
		{
			IList<ExternalProgram> list = OpenWithDialog.Show(Form.ActiveForm, TR.Messages["OpenWithPrograms", "Manage Open With Programs"], Program.Settings.ExternalPrograms,
				newAction: NewExternalProgram,
				editAction: ep => EditExternalProgram(ep),
				overrideAction: ep => ep.Override = !ep.Override);

			if (list != null)
			{
				Program.Settings.ExternalPrograms.Clear();
				Program.Settings.ExternalPrograms.AddRange(list);
			}
		}

		private ExternalProgram NewExternalProgram()
		{
			string name = "", path = "", args = "";

			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Filter = "Executable (*.EXE, *.BAT, *.CMD, *.PS1)|*.exe;*.bat;*.cmd;*.ps1|All files (*.*)|*.*";
				openFileDialog.Multiselect = false;
				openFileDialog.CheckFileExists = true;
				if (openFileDialog.ShowDialog(this) == DialogResult.OK)
				{
					path = openFileDialog.FileName;
				}
			}

			if (!string.IsNullOrEmpty(path) && File.Exists(path))
			{
				FileVersionInfo vi = FileVersionInfo.GetVersionInfo(path);
				name = string.IsNullOrEmpty(vi.ProductName)
					? SelectItemDialog.GetName(this, "Set External Program", "Program")
					: vi.ProductName;
				args = SelectItemDialog.GetName(this, "Set Program Argument", string.Empty) ?? string.Empty;
			}

			if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(path))
				return new ExternalProgram(name, path);

			return null;
		}

		private bool EditExternalProgram(ExternalProgram ep)
		{
			string oldName = ep.Name;
			string oldArgument = ep.Arguments;

			ep.Name = SelectItemDialog.GetName(this, "Rename External Program", ep.Name) ?? ep.Name;
			ep.Arguments = SelectItemDialog.GetName(this, "Change Program Argument", ep.Arguments) ?? string.Empty;

			return oldName != ep.Name || oldArgument != ep.Arguments;
		}
	}
}
