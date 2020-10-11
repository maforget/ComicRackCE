using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	public class ComicSmartListItem : ShareableComicListItem, IMatcher<ComicBook>, IComicBookGroupMatcher, IFilteredComicBookList
	{
		private ComicBookMatcherCollection matchers = new ComicBookMatcherCollection();

		private HashSet<Guid> filteredIds;

		private static readonly Regex rxTokenizer = new Regex("(?<!\\\\)\".*?((?<!\\\\)\"|$)|(?<!\\\\)\\[.*?((?<!\\\\)\\]|$)|(?<=\\]\\s+)\\s*Match|(?<=\\]\\s+)[\\w\\s]+|[\\w]+|{|}|,|;", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

		public override string ImageKey => "Search";

		public override bool OptimizedCacheUpdateDisabled => Matchers.Any((ComicBookMatcher m) => m.IsOptimizedCacheUpdateDisabled);

		public override bool CustomCacheStorage => Matchers.Any((ComicBookMatcher m) => m.TimeDependant);

		[XmlAttribute]
		[DefaultValue(MatcherMode.And)]
		public MatcherMode MatcherMode
		{
			get;
			set;
		}

		public ComicBookMatcherCollection Matchers => matchers;

		[DefaultValue(false)]
		public bool Limit
		{
			get;
			set;
		}

		[DefaultValue(ComicSmartListLimitType.Count)]
		public ComicSmartListLimitType LimitType
		{
			get;
			set;
		}

		[DefaultValue(25)]
		public int LimitValue
		{
			get;
			set;
		}

		[DefaultValue(ComicSmartListLimitSelectionType.Random)]
		public ComicSmartListLimitSelectionType LimitSelectionType
		{
			get;
			set;
		}

		[DefaultValue(0)]
		public int LimitRandomSeed
		{
			get;
			set;
		}

		public Guid BaseListId
		{
			get;
			set;
		}

		public bool BaseListIdSpecified => BaseListId != Guid.Empty;

		[DefaultValue(false)]
		public bool NotInBaseList
		{
			get;
			set;
		}

		public HashSet<Guid> FilteredIds => filteredIds ?? (filteredIds = new HashSet<Guid>());

		[DefaultValue(false)]
		public bool ShowFiltered
		{
			get;
			set;
		}

		public ComicSmartListItem()
		{
			BaseListId = Guid.Empty;
			LimitSelectionType = ComicSmartListLimitSelectionType.Random;
			LimitValue = 25;
			MatcherMode = MatcherMode.And;
			LimitType = ComicSmartListLimitType.Count;
		}

		public ComicSmartListItem(string name, string matchAll = null)
			: this()
		{
			base.Name = name;
			if (matchAll != null)
			{
				Matchers.Add(typeof(ComicBookAllPropertiesMatcher), 1, matchAll, string.Empty);
			}
		}

		public ComicSmartListItem(string name, MatcherMode mode, IEnumerable<ComicBookMatcher> matchers)
			: this(name)
		{
			MatcherMode = mode;
			Matchers.AddRange(matchers.Select((ComicBookMatcher cbm) => cbm.Clone() as ComicBookMatcher));
		}

		public ComicSmartListItem(ComicSmartListItem comicSmartListItem)
			: this(comicSmartListItem.Name, comicSmartListItem.MatcherMode, comicSmartListItem.Matchers)
		{
			base.Display = comicSmartListItem.Display;
			Limit = comicSmartListItem.Limit;
			LimitType = comicSmartListItem.LimitType;
			LimitValue = comicSmartListItem.LimitValue;
			LimitSelectionType = comicSmartListItem.LimitSelectionType;
			BaseListId = comicSmartListItem.BaseListId;
			NotInBaseList = comicSmartListItem.NotInBaseList;
			QuickOpen = comicSmartListItem.QuickOpen;
			base.Description = comicSmartListItem.Description;
			BookCount = comicSmartListItem.BookCount;
			NewBookCount = comicSmartListItem.NewBookCount;
			UnreadBookCount = comicSmartListItem.UnreadBookCount;
			if (comicSmartListItem.ShouldSerializeFilteredIds())
			{
				FilteredIds.AddRange(comicSmartListItem.FilteredIds);
			}
		}

		public ComicSmartListItem(string name, string query, ComicLibrary library = null)
			: this(name)
		{
			Tokenizer tokenizer = TokenizeQuery(query);
			if (tokenizer.IsOptional("NAME"))
			{
				tokenizer.Skip();
				base.Name = tokenizer.TakeString().Text;
			}
			if (tokenizer.IsOptional("IN", "NOT"))
			{
				if (tokenizer.IsOptional("NOT"))
				{
					tokenizer.Skip();
					NotInBaseList = true;
				}
				tokenizer.Skip();
				Tokenizer.Token list = tokenizer.Take("[", "]");
				list.Text = list.Text.Unescape("[]", '\\');
				if (library != null)
				{
					ComicListItem comicListItem = library.ComicLists.GetItems<ComicListItem>().FirstOrDefault((ComicListItem cl) => string.Equals(cl.Name, list.Text, StringComparison.CurrentCultureIgnoreCase));
					if (comicListItem != null)
					{
						BaseListId = comicListItem.Id;
					}
				}
			}
			ComicBookGroupMatcher.ConvertQueryToParamerters(this, tokenizer);
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(base.Name))
			{
				stringBuilder.Append("Name ");
				stringBuilder.Append("\"");
				stringBuilder.Append(base.Name.Escape());
				stringBuilder.Append("\"\n");
			}
			if (BaseListIdSpecified)
			{
				ComicListItem baseList = GetBaseList();
				if (baseList != null && !(baseList is ComicLibraryListItem))
				{
					if (NotInBaseList)
					{
						stringBuilder.Append("Not ");
					}
					stringBuilder.Append("In [");
					stringBuilder.Append(baseList.Name.Escape("[]", '\\'));
					stringBuilder.Append("]\n");
				}
			}
			stringBuilder.Append(ComicBookGroupMatcher.ConvertParametersToQuery(this));
			return stringBuilder.ToString();
		}

		protected override IEnumerable<ComicBook> OnGetBooks()
		{
			IEnumerable<ComicBook> enumerable = null;
			if (Library != null && Library.Books != null)
			{
				if (BaseListId == Guid.Empty)
				{
					enumerable = Library.Books;
				}
				else
				{
					IComicBookList baseList = GetBaseList();
					if (baseList != null)
					{
						enumerable = baseList.GetBooks();
					}
				}
				if (enumerable != null && NotInBaseList)
				{
					enumerable = Library.Books.Except(enumerable);
				}
			}
			if (enumerable != null)
			{
				return Match(enumerable);
			}
			return Enumerable.Empty<ComicBook>();
		}

		public bool IsSame(ComicSmartListItem comicSmartListItem)
		{
			if (comicSmartListItem != null && base.Name == comicSmartListItem.Name && base.Display == comicSmartListItem.Display && MatcherMode == comicSmartListItem.MatcherMode && NotInBaseList == comicSmartListItem.NotInBaseList && Limit == comicSmartListItem.Limit && LimitType == comicSmartListItem.LimitType && LimitValue == comicSmartListItem.LimitValue && LimitSelectionType == comicSmartListItem.LimitSelectionType && LimitRandomSeed == comicSmartListItem.LimitRandomSeed && BaseListId == comicSmartListItem.BaseListId && Matchers.SequenceEqual(comicSmartListItem.Matchers) && QuickOpen == comicSmartListItem.QuickOpen && base.Description == comicSmartListItem.Description)
			{
				return HasSameFilteredIds(comicSmartListItem);
			}
			return false;
		}

		public override object Clone()
		{
			return new ComicSmartListItem(this);
		}

		public override IEnumerable<string> GetDependentProperties()
		{
			return base.GetDependentProperties().Concat(Matchers.SelectMany((ComicBookMatcher m) => m.GetDependentProperties()));
		}

		public override bool IsUpdateNeeded(string propertyHint)
		{
			if (Matchers.All((ComicBookMatcher m) => !m.UsesProperty(propertyHint)))
			{
				return false;
			}
			return base.IsUpdateNeeded(propertyHint);
		}

		protected override IEnumerable<ComicBook> OnCacheMatch(IEnumerable<ComicBook> cbl)
		{
			ICachedComicBookList i = GetBaseList();
			return from cb in Match(cbl)
				where i == null || (i.GetCache().Contains(cb) ^ NotInBaseList)
				select cb;
		}

		protected override bool OnRetrieveCustomCache(HashSet<ComicBook> books)
		{
			Library.NotifyComicListCacheReset(this);
			return base.OnRetrieveCustomCache(books);
		}

		public override void Refresh()
		{
			LimitRandomSeed = 0;
			base.Refresh();
		}

		public IEnumerable<ComicBook> Match(IEnumerable<ComicBook> items)
		{
			MatcherSet<ComicBook> matcherSet = new MatcherSet<ComicBook>();
			foreach (ComicBookMatcher matcher in Matchers)
			{
				matcherSet.Add(matcher, MatcherMode, matcher.Not);
			}
			Matchers.Recurse<ComicBookMatcher>((object m) => (!(m is ComicBookGroupMatcher)) ? null : ((ComicBookGroupMatcher)m).Matchers).ForEach(delegate(ComicBookMatcher m)
			{
				m.StatsProvider = this;
			});
			IEnumerable<ComicBook> enumerable = matcherSet.Match(items);
			if (Limit)
			{
				switch (LimitSelectionType)
				{
				case ComicSmartListLimitSelectionType.SortedBySeries:
					enumerable = enumerable.ToList().OrderBy((ComicBook cb) => cb, new ComicBookSeriesComparer());
					break;
				default:
					if (LimitRandomSeed == 0)
					{
						LimitRandomSeed = new Random().Next();
					}
					enumerable = enumerable.OrderBy((ComicBook l) => l.Id).ToList().Randomize(LimitRandomSeed);
					break;
				case ComicSmartListLimitSelectionType.Position:
					break;
				}
				switch (LimitType)
				{
				case ComicSmartListLimitType.MB:
					enumerable = LimitBySize(enumerable, (long)LimitValue * 1024L * 1024);
					break;
				case ComicSmartListLimitType.GB:
					enumerable = LimitBySize(enumerable, (long)LimitValue * 1024L * 1024 * 1024);
					break;
				default:
					enumerable = enumerable.Take(LimitValue);
					break;
				}
			}
			if (!ShowFiltered && ShouldSerializeFilteredIds())
			{
				if (Library != null)
				{
					ComicBookCollection books = Library.Books;
					filteredIds.RemoveWhere((Guid id) => books[id] == null);
				}
				enumerable = enumerable.Where((ComicBook cb) => !filteredIds.Contains(cb.Id));
			}
			return enumerable;
		}

		public bool ShouldSerializeFilteredIds()
		{
			if (filteredIds != null)
			{
				return filteredIds.Count > 0;
			}
			return false;
		}

		public static Tokenizer TokenizeQuery(string query)
		{
			return new Tokenizer(rxTokenizer, query);
		}

		public ComicListItem GetBaseList(bool withTest = true)
		{
			if (Library == null || Library.ComicLists == null)
			{
				return null;
			}
			ComicListItem comicListItem = Library.ComicLists.FindItem(BaseListId);
			if (comicListItem != null && withTest && comicListItem.RecursionTest(this))
			{
				comicListItem = null;
			}
			return comicListItem;
		}

		public bool SetList(ComicSmartListItem item)
		{
			if (IsSame(item))
			{
				return false;
			}
			MatcherMode = item.MatcherMode;
			NotInBaseList = item.NotInBaseList;
			ComicSmartListItem comicSmartListItem = item.Clone() as ComicSmartListItem;
			BaseListId = comicSmartListItem.BaseListId;
			matchers = comicSmartListItem.matchers;
			CopyExtraValues(item);
			ResetCache();
			Refresh();
			return true;
		}

		public void CopyExtraValues(ComicSmartListItem item)
		{
			base.Name = item.Name;
			base.Description = item.Description;
			Limit = item.Limit;
			LimitValue = item.LimitValue;
			LimitSelectionType = item.LimitSelectionType;
			LimitRandomSeed = item.LimitRandomSeed;
			LimitType = item.LimitType;
			QuickOpen = item.QuickOpen;
			filteredIds = null;
			if (item.ShouldSerializeFilteredIds())
			{
				FilteredIds.AddRange(item.FilteredIds);
			}
		}

		public bool HasSameFilteredIds(ComicSmartListItem cli)
		{
			if (filteredIds == cli.filteredIds)
			{
				return true;
			}
			if (filteredIds != null && cli.FilteredIds != null)
			{
				return filteredIds.SetEquals(cli.filteredIds);
			}
			return false;
		}

		private static IEnumerable<ComicBook> LimitBySize(IEnumerable<ComicBook> cbl, long maxSize)
		{
			long size = 0L;
			return cbl.TakeWhile((ComicBook cb) => (size += cb.FileSize) < maxSize);
		}

		public bool IsFiltered(ComicBook ci)
		{
			if (filteredIds != null)
			{
				return filteredIds.Contains(ci.Id);
			}
			return false;
		}

		public void SetFiltered(ComicBook ci, bool filtered)
		{
			if (IsFiltered(ci) != filtered)
			{
				if (filtered)
				{
					FilteredIds.Add(ci.Id);
				}
				else
				{
					FilteredIds.Remove(ci.Id);
				}
				Refresh();
			}
		}

		public void ClearFiltered()
		{
			if (ShouldSerializeFilteredIds())
			{
				filteredIds = null;
				Refresh();
			}
		}
	}
}
