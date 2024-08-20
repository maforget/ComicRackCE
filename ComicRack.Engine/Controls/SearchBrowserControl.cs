using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Text;
using cYo.Common.Win32;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Engine.Controls
{
	public partial class SearchBrowserControl : UserControl
	{
		public class SelectionEntry : IComparable<SelectionEntry>
		{
			public int Index
			{
				get;
				private set;
			}

			public int Id
			{
				get;
				private set;
			}

			public string Property
			{
				get;
				private set;
			}

			public Type MatcherType
			{
				get;
				private set;
			}

			public bool MultipleValues
			{
				get;
				private set;
			}

			public string Caption
			{
				get;
				private set;
			}

			public SelectionEntry(int index, int id, string property, string caption, Type matcherType, bool multiValue)
			{
				Index = index;
				Id = id;
				Property = property;
				Caption = TR.Load("SearchBrowser")[property, caption];
				MatcherType = matcherType;
				MultipleValues = multiValue;
			}

			public int CompareTo(SelectionEntry other)
			{
				return string.Compare(Caption, other.Caption);
			}
		}

		private class SelectionInfo : SelectionEntry
		{
			public readonly ListView ListView;

			public readonly HashSet<string> SelectedItems;

			public readonly Dictionary<int, ListViewItem> CachedItems;

			public List<string> Items;

			public bool Not;

			public SelectionInfo(int index, ListView listView, int id, string property, string caption, Type matcherType, bool multiValue, bool not)
				: base(index, id, property, caption, matcherType, multiValue)
			{
				ListView = listView;
				Items = new List<string>();
				SelectedItems = new HashSet<string>();
				CachedItems = new Dictionary<int, ListViewItem>();
				Not = not;
				listView.Tag = this;
				listView.VirtualListSize = 0;
				listView.Clear();
				listView.Columns.Add(base.Caption);
				listView.Columns[0].Width = listView.Width - 20;
			}

			public SelectionInfo(SelectionEntry se, ListView lv, bool not)
				: this(se.Index, lv, se.Id, se.Property, se.Caption, se.MatcherType, se.MultipleValues, not)
			{
			}
		}

		public const int ColumnCount = 3;

		private static readonly char[] listSeparators = new char[2]
		{
			',',
			';'
		};

		private readonly SelectionInfo[] workingSelectionInfos = new SelectionInfo[3];

		private readonly string allText = TR.Load("SearchBrowser")["AllItems", "All ({0} {1})"];

		private readonly ComicBookCollection books = new ComicBookCollection();

		private string unspecifiedText = TR.Load("SearchBrowser")["Unspecified", "Unspecified"];

		private volatile bool listIsDirty;

		private volatile bool shieldIndex;

		private bool shieldNot;

		private Bitmap dragBitmap;

		private IBitmapCursor dragCursor;


		public ComicBookCollection Books => books;

		public ComicBookMatcher CurrentMatcher => GetMatcherUpTo(null);

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Column1
		{
			get
			{
				return GetSelectedIndex(cbType1);
			}
			set
			{
				SetSelectedIndex(cbType1, value);
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Column2
		{
			get
			{
				return GetSelectedIndex(cbType2);
			}
			set
			{
				SetSelectedIndex(cbType2, value);
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Column3
		{
			get
			{
				return GetSelectedIndex(cbType3);
			}
			set
			{
				SetSelectedIndex(cbType3, value);
			}
		}

		[DefaultValue("Unspecified")]
		[Localizable(true)]
		public string UnspecifiedText
		{
			get
			{
				return unspecifiedText;
			}
			set
			{
				unspecifiedText = value;
			}
		}

		public event EventHandler CurrentMatcherChanged;

		public event ItemDragEventHandler ItemDrag;

		public SearchBrowserControl()
		{
			InitializeComponent();
			FillTypeCombo(cbType1, 0, listView1, btNot1, 1);
			FillTypeCombo(cbType2, 1, listView2, btNot2, 0);
			FillTypeCombo(cbType3, 2, listView3, btNot3, 2);
			ListStyles.SetOwnerDrawn(listView1);
			ListStyles.SetOwnerDrawn(listView2);
			ListStyles.SetOwnerDrawn(listView3);
			KeySearch.Create(listView1);
			KeySearch.Create(listView2);
			KeySearch.Create(listView3);
			string caption = TR.Default["LogicNot", "Negation"];
			toolTip.SetToolTip(btNot1, caption);
			toolTip.SetToolTip(btNot2, caption);
			toolTip.SetToolTip(btNot3, caption);
		}

		private static void FillTypeCombo(ComboBox cb, int index, ListView lv, CheckBox chk, int selected)
		{
			cb.Items.AddRange(new SelectionEntry[35]
			{
				new SelectionEntry(index, 0, "ShadowSeries", "Series", typeof(ComicBookSeriesMatcher), multiValue: false),
				new SelectionEntry(index, 1, "ShadowTitle", "Titles", typeof(ComicBookTitleMatcher), multiValue: false),
				new SelectionEntry(index, 2, "Genre", "Genres", typeof(ComicBookGenreMatcher), multiValue: true),
				new SelectionEntry(index, 3, "AlternateSeries", "Alternate Series", typeof(ComicBookAlternateSeriesMatcher), multiValue: false),
				new SelectionEntry(index, 4, "ShadowFormat", "Formats", typeof(ComicBookFormatMatcher), multiValue: false),
				new SelectionEntry(index, 5, "ShadowYearAsText", "Years", typeof(ComicBookYearMatcher), multiValue: false),
				new SelectionEntry(index, 6, "MonthAsText", "Months", typeof(ComicBookMonthMatcher), multiValue: false),
				new SelectionEntry(index, 7, "LanguageAsText", "Languages", typeof(ComicBookLanguageMatcher), multiValue: false),
				new SelectionEntry(index, 8, "AgeRating", "Age Ratings", typeof(ComicBookAgeRatingMatcher), multiValue: false),
				new SelectionEntry(index, 9, "RatingAsText", "My Ratings", typeof(ComicBookRatingMatcher), multiValue: false),
				new SelectionEntry(index, 10, "Writer", "Writers", typeof(ComicBookWriterMatcher), multiValue: true),
				new SelectionEntry(index, 11, "Penciller", "Pencillers", typeof(ComicBookPencillerMatcher), multiValue: true),
				new SelectionEntry(index, 12, "Inker", "Inkers", typeof(ComicBookInkerMatcher), multiValue: true),
				new SelectionEntry(index, 13, "Colorist", "Colorists", typeof(ComicBookColoristMatcher), multiValue: true),
				new SelectionEntry(index, 14, "CoverArtist", "Cover Artists", typeof(ComicBookCoverArtistMatcher), multiValue: true),
				new SelectionEntry(index, 15, "Editor", "Editors", typeof(ComicBookEditorMatcher), multiValue: true),
				new SelectionEntry(index, 16, "Publisher", "Publishers", typeof(ComicBookPublisherMatcher), multiValue: true),
				new SelectionEntry(index, 17, "Imprint", "Imprints", typeof(ComicBookImprintMatcher), multiValue: true),
				new SelectionEntry(index, 18, "Characters", "Characters", typeof(ComicBookCharactersMatcher), multiValue: true),
				new SelectionEntry(index, 19, "Tags", "Tags", typeof(ComicBookTagsMatcher), multiValue: true),
				new SelectionEntry(index, 20, "ShadowVolumeAsText", "Volumes", typeof(ComicBookVolumeMatcher), multiValue: false),
				new SelectionEntry(index, 21, "Teams", "Teams", typeof(ComicBookTeamsMatcher), multiValue: true),
				new SelectionEntry(index, 22, "Locations", "Locations", typeof(ComicBookLocationsMatcher), multiValue: true),
				new SelectionEntry(index, 23, "Letterer", "Letterers", typeof(ComicBookLettererMatcher), multiValue: true),
				new SelectionEntry(index, 24, "CommunityRatingAsText", "Community Ratings", typeof(ComicBookCommunityRatingMatcher), multiValue: false),
				new SelectionEntry(index, 25, "BookPriceAsText", "Book Prices", typeof(ComicBookBookPriceMatcher), multiValue: false),
				new SelectionEntry(index, 26, "BookAge", "Book Ages", typeof(ComicBookBookAgeMatcher), multiValue: false),
				new SelectionEntry(index, 27, "BookStore", "Book Stores", typeof(ComicBookBookStoreMatcher), multiValue: false),
				new SelectionEntry(index, 28, "BookOwner", "Book Owners", typeof(ComicBookBookOwnerMatcher), multiValue: false),
				new SelectionEntry(index, 29, "BookCondition", "Book Conditions", typeof(ComicBookBookConditionMatcher), multiValue: false),
				new SelectionEntry(index, 30, "BookCollectionStatus", "Book Collection Status", typeof(ComicBookBookCollectionStatusMatcher), multiValue: true),
				new SelectionEntry(index, 31, "BookLocation", "Book Locations", typeof(ComicBookBookLocationMatcher), multiValue: false),
				new SelectionEntry(index, 32, "MainCharacterOrTeam", "Main Characters/Teams", typeof(ComicBookMainCharacterOrTeamMatcher), multiValue: false),
				new SelectionEntry(index, 33, "SeriesGroup", "Series Group", typeof(ComicBookSeriesGroupMatcher), multiValue: false),
				new SelectionEntry(index, 34, "StoryArc", "Story Arcs", typeof(ComicBookStoryArcMatcher), multiValue: false)
			}.Sort());
			cb.Tag = lv;
			lv.Tag = chk;
			chk.Tag = index;
			cb.DisplayMember = "Caption";
			SetSelectedIndex(cb, selected);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (!base.DesignMode)
			{
				books.Changed += BooksChanged;
				IdleProcess.Idle += IdleUpdate;
			}
		}

		private static void SafeSetBounds(Control c, int x, int y, int width, int height)
		{
			if (c.Left != x || c.Top != y || c.Width != width || c.Height != height)
			{
				c.SetBounds(x, y, width, height);
			}
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			int num = DisplayRectangle.Width / 3;
			int height = DisplayRectangle.Height;
			int width = DisplayRectangle.Width;
			int width2 = btNot1.Width;
			int y = 0 + cbType1.Height + 2;
			SetListViewBounds(listView1, 0, y, num - 2, height);
			SetListViewBounds(listView2, num + 2, y, num - 2, height);
			SetListViewBounds(listView3, width - (num - 2), y, num - 2, height);
			SafeSetBounds(cbType1, listView1.Left, 0, listView1.Width - 1 - width2, cbType1.Height);
			SafeSetBounds(cbType2, listView2.Left, 0, listView2.Width - 1 - width2, cbType2.Height);
			SafeSetBounds(cbType3, listView3.Left, 0, listView3.Width - 1 - width2, cbType3.Height);
			SafeSetBounds(btNot1, cbType1.Right + 1, -1, width2, cbType1.Height);
			SafeSetBounds(btNot2, cbType2.Right + 1, -1, width2, cbType2.Height);
			SafeSetBounds(btNot3, cbType3.Right + 1, -1, width2, cbType3.Height);
			base.OnLayout(levent);
		}

		private static void SetListViewBounds(ListView lv, int x, int y, int w, int h)
		{
			SafeSetBounds(lv, x, y, w, h - y);
			if (lv.Columns.Count > 0)
			{
				lv.Columns[0].Width = lv.DisplayRectangle.Width - SystemInformation.VerticalScrollBarWidth;
				lv.Columns[0].Width = lv.DisplayRectangle.Width;
			}
		}

		private void BooksChanged(object sender, SmartListChangedEventArgs<ComicBook> e)
		{
			switch (e.Action)
			{
				case SmartListAction.Insert:
					e.Item.BookChanged += BookPropertyChanged;
					break;
				case SmartListAction.Remove:
					e.Item.BookChanged -= BookPropertyChanged;
					break;
			}
			listIsDirty = true;
		}

		private void BookPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			listIsDirty = workingSelectionInfos.Any((SelectionInfo si) => si.Property == e.PropertyName);
		}

		private void IdleUpdate(object sender, EventArgs e)
		{
			UpdateLists();
		}

		private void ListItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			HandleSelectionChange(sender as ListView);
		}

		private void ListViewVirtualItemsSelectionRangeChanged(object sender, ListViewVirtualItemsSelectionRangeChangedEventArgs e)
		{
			HandleSelectionChange(sender as ListView);
		}

		private void HandleSelectionChange(ListView lv)
		{
			if (shieldIndex)
			{
				return;
			}
			SelectionInfo selectionInfo = (SelectionInfo)lv.Tag;
			if (CheckSelection(selectionInfo))
			{
				if (selectionInfo.Index < 2)
				{
					BuildList(selectionInfo.Index + 1);
				}
				OnCurrentMatcherChanged();
			}
		}

		private void ListViewRetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
		{
			ListView listView = (ListView)sender;
			SelectionInfo selectionInfo = (SelectionInfo)listView.Tag;
			if (selectionInfo.CachedItems.TryGetValue(e.ItemIndex, out var value))
			{
				e.Item = value;
				return;
			}
			string text = selectionInfo.Items[e.ItemIndex];
			e.Item = new ListViewItem(string.IsNullOrEmpty(text) ? UnspecifiedText : text)
			{
				Tag = text
			};
			selectionInfo.CachedItems.Add(e.ItemIndex, e.Item);
		}

		private void cbType_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
			SelectionEntry selectionEntry = (SelectionEntry)comboBox.SelectedItem;
			ListView lv = comboBox.Tag as ListView;
			workingSelectionInfos[selectionEntry.Index] = new SelectionInfo(selectionEntry, lv, GetNotButton(selectionEntry.Index).Checked);
			listIsDirty = true;
		}

		private void btNot_CheckedChanged(object sender, EventArgs e)
		{
			if (!shieldNot)
			{
				CheckBox checkBox = (CheckBox)sender;
				SelectionInfo selectionInfo = workingSelectionInfos[(int)checkBox.Tag];
				selectionInfo.Not = checkBox.Checked;
				if (selectionInfo.Index < 2)
				{
					BuildList(selectionInfo.Index + 1);
				}
				OnCurrentMatcherChanged();
			}
		}

		protected virtual void OnCurrentMatcherChanged()
		{
			if (this.CurrentMatcherChanged != null)
			{
				this.CurrentMatcherChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnItemDrag(MouseButtons buttons, object data)
		{
			if (this.ItemDrag != null)
			{
				this.ItemDrag(this, new ItemDragEventArgs(buttons, data));
			}
		}

		public void UpdateLists()
		{
			while (listIsDirty)
			{
				listIsDirty = false;
				BuildLists();
			}
		}

		public SelectionEntry GetSelectionColumn(int column)
		{
			switch (column)
			{
				case 0:
					return cbType1.SelectedItem as SelectionEntry;
				case 1:
					return cbType2.SelectedItem as SelectionEntry;
				case 2:
					return cbType3.SelectedItem as SelectionEntry;
				default:
					return null;
			}
		}

		public void SelectEntry(int column, string value)
		{
			switch (column)
			{
				case 0:
					SelectListEntry(listView1, value);
					break;
				case 1:
					SelectListEntry(listView2, value);
					break;
				case 2:
					SelectListEntry(listView3, value);
					break;
			}
		}

		public void ClearNot()
		{
			try
			{
				shieldNot = true;
				CheckBox checkBox = btNot1;
				CheckBox checkBox2 = btNot2;
				bool flag2 = (btNot3.Checked = false);
				bool @checked = (checkBox2.Checked = flag2);
				checkBox.Checked = @checked;
			}
			finally
			{
				shieldNot = false;
			}
		}

		private CheckBox GetNotButton(int column)
		{
			return (new CheckBox[3]
			{
				btNot1,
				btNot2,
				btNot3
			})[column];
		}

		private static int GetSelectedIndex(ComboBox cb)
		{
			return ((SelectionEntry)cb.SelectedItem).Id;
		}

		private static void SetSelectedIndex(ComboBox cb, int id)
		{
			for (int i = 0; i < cb.Items.Count; i++)
			{
				SelectionEntry selectionEntry = cb.Items[i] as SelectionEntry;
				if (selectionEntry != null && selectionEntry.Id == id)
				{
					cb.SelectedIndex = i;
					return;
				}
			}
			cb.SelectedIndex = 0;
		}

		private ComicBookMatcher GetMatcherUpTo(SelectionInfo end)
		{
			ComicBookGroupMatcher comicBookGroupMatcher = new ComicBookGroupMatcher();
			if (books == null || books.Count == 0)
			{
				return null;
			}
			SelectionInfo[] array = workingSelectionInfos;
			foreach (SelectionInfo selectionInfo in array)
			{
				ComicBookMatcher comicBookMatcher = CreateMatcher(selectionInfo);
				if (comicBookMatcher != null)
				{
					comicBookGroupMatcher.Matchers.Add(comicBookMatcher);
				}
				if (selectionInfo == end)
				{
					break;
				}
			}
			return comicBookGroupMatcher.Optimized();
		}

		private bool CheckSelection(SelectionInfo si)
		{
			shieldIndex = true;
			ListView listView = si.ListView;
			HashSet<string> hashSet = new HashSet<string>(si.SelectedItems);
			try
			{
				if (listView.Items[0].Selected)
				{
					for (int i = 1; i < listView.Items.Count; i++)
					{
						listView.Items[i].Selected = false;
					}
					si.SelectedItems.Clear();
				}
				else
				{
					for (int j = 1; j < listView.Items.Count; j++)
					{
						if (listView.Items[j].Selected)
						{
							si.SelectedItems.Add(si.Items[j]);
						}
						else
						{
							si.SelectedItems.Remove(si.Items[j]);
						}
					}
					if (si.SelectedItems.Count > 0)
					{
						listView.Items[0].Selected = false;
					}
				}
			}
			finally
			{
				shieldIndex = false;
			}
			return hashSet != si.SelectedItems;
		}

		private void BuildList(int start)
		{
			int num = -1;
			IEnumerable<ComicBook> enumerable = books;
			MatcherSet<ComicBook> matcherSet = new MatcherSet<ComicBook>();
			if (enumerable == null)
			{
				enumerable = new ComicBook[0];
			}
			SelectionInfo[] array = workingSelectionInfos;
			foreach (SelectionInfo selectionInfo in array)
			{
				if (++num >= start)
				{
					if (matcherSet.Matchers.Count > 0)
					{
						enumerable = matcherSet.Match(enumerable).ToArray();
					}
					HashSet<string> hashSet = (EngineConfiguration.Default.SearchBrowserCaseSensitive ? new HashSet<string>() : new HashSet<string>(StringComparer.InvariantCultureIgnoreCase));
					foreach (ComicBook item in enumerable)
					{
						string stringPropertyValue = item.GetStringPropertyValue(selectionInfo.Property, ComicValueType.Shadow);
						if (!selectionInfo.MultipleValues || stringPropertyValue.IndexOfAny(listSeparators) == -1)
						{
							hashSet.Add(stringPropertyValue.Trim());
							continue;
						}
						hashSet.AddRange(from vs in stringPropertyValue.Split(listSeparators)
										 select vs.Trim());
					}
					List<string> list = hashSet.ToList();
					list.Sort(new ExtendedStringComparer(ExtendedStringComparison.IgnoreArticles | ExtendedStringComparison.IgnoreCase));
					list.Insert(0, StringUtility.Format(allText, list.Count, selectionInfo.Caption));
					if (!selectionInfo.Items.SequenceEqual(list, StringComparer.CurrentCultureIgnoreCase))
					{
						FillSelectonInfoList(selectionInfo, list);
					}
				}
				IMatcher<ComicBook> matcher = CreateMatcher(selectionInfo);
				if (matcher != null)
				{
					if (selectionInfo.Not)
					{
						matcherSet.AndNot(matcher);
					}
					else
					{
						matcherSet.And(matcher);
					}
				}
			}
		}

		private void BuildLists()
		{
			BuildList(0);
		}

		private static void FillSelectonInfoList(SelectionInfo si, List<string> names)
		{
			ListView listView = si.ListView;
			HashSet<string> hashSet = new HashSet<string>(si.SelectedItems);
			si.SelectedItems.Clear();
			si.CachedItems.Clear();
			listView.BeginUpdate();
			try
			{
				si.Items = names;
				listView.TopItem = null;
				listView.VirtualListSize = 0;
				listView.VirtualMode = false;
				listView.VirtualMode = true;
				listView.VirtualListSize = names.Count;
				for (int i = 1; i < names.Count; i++)
				{
					if (hashSet.Contains(names[i]))
					{
						listView.Items[i].Selected = true;
						si.SelectedItems.Add(names[i]);
					}
				}
				if (listView.Items.Count > 0 && listView.SelectedIndices.Count == 0)
				{
					listView.Items[0].Selected = true;
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				listView.EndUpdate();
			}
		}

		private static ComicBookMatcher CreateMatcher(SelectionInfo si)
		{
			switch (si.SelectedItems.Count)
			{
				case 0:
					return null;
				case 1:
					{
						//This will match the value even if it has trailing & leading spaces. With multiple values the ListContains already does this internally.
						string value = si.MultipleValues ? si.SelectedItems.First() : $"(?<=^)\\s*{Regex.Escape(si.SelectedItems.First())}\\s*(?=$)";
						ComicBookValueMatcher comicBookValueMatcher = ComicBookValueMatcher.Create(si.MatcherType,
							si.MultipleValues ? ComicBookStringMatcher.OperatorListContains : ComicBookStringMatcher.OperatorRegex,
							value, null);
						comicBookValueMatcher.Not = si.Not;
						return comicBookValueMatcher;
					}
				default:
					{
						ComicBookGroupMatcher subSet = new ComicBookGroupMatcher
						{
							MatcherMode = MatcherMode.Or,
							Not = si.Not
						};
						si.SelectedItems.ForEach(delegate (string s)
						{
							//This will match the value even if it has trailing & leading spaces. With multiple values the ListContains already does this internally.
							string value = si.MultipleValues ? s : $"(?<=^)\\s*{Regex.Escape(s)}\\s*(?=$)";
							subSet.Matchers.Add(si.MatcherType,
								si.MultipleValues ? ComicBookStringMatcher.OperatorListContains : ComicBookStringMatcher.OperatorRegex,
								value, null);
						});
						return subSet;
					}
			}
		}

		private static void SelectListEntry(ListView lv, string text)
		{
			for (int i = 0; i < lv.Items.Count; i++)
			{
				lv.Items[i].Selected = false;
			}
			if (!SelectListEntryInternal(lv, text))
			{
				string[] array = text.Split(listSeparators);
				foreach (string text2 in array)
				{
					SelectListEntryInternal(lv, text2.Trim());
				}
			}
		}

		private static bool SelectListEntryInternal(ListView lv, string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			for (int i = 0; i < lv.Items.Count; i++)
			{
				try
				{
					ListViewItem listViewItem = lv.Items[i];
					if (object.Equals(text, listViewItem.Tag))
					{
						listViewItem.Selected = true;
						listViewItem.EnsureVisible();
						return true;
					}
				}
				catch (Exception)
				{
				}
			}
			return false;
		}

		private void listView_ItemDrag(object sender, ItemDragEventArgs e)
		{
			ListView listView = (ListView)sender;
			SelectionInfo si = listView.Tag as SelectionInfo;
			ComicBookMatcher comicBookMatcher = CreateMatcher(si);
			ComicBookContainer comicBookContainer = new ComicBookContainer(((ListViewItem)e.Item).Text);
			DataObject dataObject = new DataObject();
			if (comicBookMatcher == null)
			{
				comicBookContainer.Books.AddRange(books);
			}
			else
			{
				comicBookContainer.Books.AddRange(comicBookMatcher.Match(books));
				dataObject.SetData(ComicBookMatcher.ClipboardFormat, comicBookMatcher);
			}
			dataObject.SetData(comicBookContainer);
			dragBitmap = CreateDragCursor(si);
			dragCursor = BitmapCursor.Create(dragBitmap, new Point(10, 10));
			try
			{
				OnItemDrag(e.Button, dataObject);
			}
			catch
			{
			}
			finally
			{
				dragBitmap.Dispose();
				if (dragCursor != null)
				{
					dragCursor.Dispose();
				}
			}
		}

		private void GiveDragFeedback(object sender, GiveFeedbackEventArgs e)
		{
			if (dragCursor != null && !(dragCursor.Cursor == null))
			{
				e.UseDefaultCursors = false;
				dragCursor.OverlayCursor = ((e.Effect == DragDropEffects.None) ? Cursors.No : Cursors.Default);
				Cursor.Current = dragCursor.Cursor;
			}
		}

		private Bitmap CreateDragCursor(SelectionInfo si)
		{
			ListView listView = si.ListView;
			List<string> list = si.SelectedItems.Take(10).ToList();
			if (list.Count == 0)
			{
				list.Add(si.Items[0]);
			}
			list.Sort();
			int height = Font.Height;
			Rectangle rectangle = new Rectangle(0, 0, listView.Width, height * list.Count + 4);
			Bitmap bitmap = new Bitmap(rectangle.Width + 1, rectangle.Height + 1);
			Rectangle r = rectangle;
			r.Inflate(-2, -2);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.Clear(Color.White);
				using (StringFormat format = new StringFormat(StringFormatFlags.NoWrap))
				{
					using (Brush brush = new SolidBrush(ForeColor))
					{
						for (int i = 0; i < list.Count; i++)
						{
							graphics.DrawString(list[i], Font, brush, r, format);
							r.Y += height;
						}
					}
				}
				using (Pen pen = new Pen(ForeColor))
				{
					graphics.DrawRectangle(pen, rectangle);
				}
				bitmap.ChangeAlpha(128);
				return bitmap;
			}
		}
	}
}
