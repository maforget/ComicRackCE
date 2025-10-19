using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using cYo.Common;
using cYo.Common.Collections;
using cYo.Common.Localize;
using cYo.Common.Threading;
using cYo.Common.Xml;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	public class ComicLibrary : ComicBookContainer, IDeserializationCallback
	{
		[Serializable]
		private class ComicListItemLookup
		{
			private ReverseIndex<ComicListItem, string> reversePropertyIndex = new ReverseIndex<ComicListItem, string>();

			private ReverseIndex<ComicListItem, Guid> reverseBaseListIndex = new ReverseIndex<ComicListItem, Guid>();

			public void Add(ComicListItem item)
			{
				reversePropertyIndex.Add(item, item.GetDependentProperties());
				ComicSmartListItem comicSmartListItem = item as ComicSmartListItem;
				if (comicSmartListItem != null && comicSmartListItem.BaseListId != Guid.Empty)
				{
					reverseBaseListIndex.Add(item, comicSmartListItem.BaseListId);
				}
			}

			public void AddRange(IEnumerable<ComicListItem> items)
			{
				items.ForEach(Add);
			}

			public void AddRange(ComicListItem item)
			{
				Add(item);
				ComicListItemFolder comicListItemFolder = item as ComicListItemFolder;
				if (comicListItemFolder != null)
				{
					AddRange(comicListItemFolder.Items.GetItems<ComicListItem>());
				}
			}

			public void Remove(ComicListItem item)
			{
				reversePropertyIndex.Remove(item);
				reverseBaseListIndex.Remove(item);
			}

			public void RemoveRange(IEnumerable<ComicListItem> items)
			{
				items.ForEach(Remove);
			}

			public void RemoveRange(ComicListItem item)
			{
				Remove(item);
				ComicListItemFolder comicListItemFolder = item as ComicListItemFolder;
				if (comicListItemFolder != null)
				{
					RemoveRange(comicListItemFolder.Items.GetItems<ComicListItem>());
				}
			}

			public IEnumerable<ComicListItem> Match(string property)
			{
				return reversePropertyIndex.Match(property)
					.Concat(reversePropertyIndex.Match("*"))
					.Concat(reversePropertyIndex.Where(x => x.StartsWith("VirtualTag")));
			}

			public IEnumerable<ComicListItem> Match(Guid id)
			{
				return reverseBaseListIndex.Match(id);
			}
		}

		[NonSerialized]
		private bool isDirty;

		private bool isLoaded;

		private HashSet<string> customValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		private ComicListItemCollection comicLists = new ComicListItemCollection();

		private readonly ComicListItemLookup comicListItemLookup = new ComicListItemLookup();

		[NonSerialized]
		private Dictionary<ComicBookSeriesStatistics.Key, ComicBookSeriesStatistics> seriesStats;

		[NonSerialized]
		private object seriesStatsLock = new object();

		private static Type[] extraTypes;

		[XmlIgnore]
		public bool IsDirty
		{
			get
			{
				return isDirty;
			}
			set
			{
				isDirty = value;
			}
		}

		[XmlIgnore]
		public bool IsLoaded
		{
			get
			{
				return isLoaded;
			}
			set
			{
				isLoaded = value;
			}
		}

		[XmlIgnore]
		public ComicListItemFolder TemporaryFolder
		{
			get
			{
				ComicListItemFolder comicListItemFolder = ComicLists.GetItems<ComicListItemFolder>().FirstOrDefault((ComicListItemFolder c) => c.Temporary);
				if (comicListItemFolder == null)
				{
					comicListItemFolder = new ComicListItemFolder(TR.Load("ComicBook")["TempLists", "Temporary Lists"])
					{
						Temporary = true,
						Collapsed = false
					};
					ComicLists.Add(comicListItemFolder);
				}
				return comicListItemFolder;
			}
		}

		[XmlIgnore]
		public IEnumerable<string> CustomValues => customValues;

		public override bool IsLibrary => true;

		[XmlArrayItem("Item")]
		public ComicListItemCollection ComicLists => comicLists;

		[XmlIgnore]
		public bool ComicListsLocked
		{
			get;
			set;
		}

		public static QueryCacheMode QueryCacheMode
		{
			get;
			set;
		}

		public static bool BackgroundQueryCacheUpdate
		{
			get;
			set;
		}

		public static bool IsQueryCacheEnabled => QueryCacheMode != QueryCacheMode.Disabled;

		public static bool IsQueryCacheInstantUpdate => QueryCacheMode == QueryCacheMode.InstantUpdate;

		[field: NonSerialized]
		public event EventHandler CustomValuesChanged;

		[field: NonSerialized]
		public event ComicListChangedEventHandler ComicListsChanged;

		[field: NonSerialized]
		public event EventHandler ComicListCachesUpdated;

		public ComicLibrary()
		{
			comicLists.Changed += comicBookLists_Changed;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				comicLists.Changed -= comicBookLists_Changed;
			}
			base.Dispose(disposing);
		}

		protected override void OnBookChanged(ContainerBookChangedEventArgs e)
		{
			isDirty = true;
			if (e.PropertyName == "CustomValuesStore")
			{
				AddCustomValues(e.Book);
			}
			base.OnBookChanged(e);
			InvalidateComicListCaches(e.Book, ComicListItem.PendingCacheAction.Update, e.PropertyName);
		}

		protected override void OnBookAdded(ComicBook book)
		{
			isDirty = true;
			book.Container = this;
			AddCustomValues(book);
			base.OnBookAdded(book);
			InvalidateComicListCaches(book, ComicListItem.PendingCacheAction.Add);
		}

		protected override void OnBookRemoved(ComicBook book)
		{
			isDirty = true;
			book.Container = null;
			base.OnBookRemoved(book);
			InvalidateComicListCaches(book, ComicListItem.PendingCacheAction.Remove);
		}

		private void comicBookLists_Changed(object sender, SmartListChangedEventArgs<ComicListItem> e)
		{
			isDirty = true;
			switch (e.Action)
			{
			case SmartListAction.Insert:
				e.Item.Library = this;
				OnComicListChanged(new ComicListItemChangedEventArgs(e.Item, ComicListItemChange.Added));
				break;
			case SmartListAction.Remove:
				e.Item.Library = null;
				OnComicListChanged(new ComicListItemChangedEventArgs(e.Item, ComicListItemChange.Removed));
				break;
			}
		}

		private void Item_Changed(object sender, ComicListItemChangedEventArgs e)
		{
			OnComicListChanged(e);
		}

		public void InitializeDefaultLists()
		{
			ComicLibraryListItem item = new ComicLibraryListItem(ComicBook.TR["Library", "Library"]);
			comicLists.Add(item);
			ComicListItemFolder comicListItemFolder = new ComicListItemFolder(ComicBook.TR["SmartLists", "Smart Lists"]);
			comicLists.Add(comicListItemFolder);
			ComicSmartListItem comicSmartListItem = new ComicSmartListItem(ComicBook.TR["MyFavoritesList", "My Favorites"]);
			comicSmartListItem.Matchers.Add(typeof(ComicBookRatingMatcher), 1, "3", "");
			comicListItemFolder.Items.Add(comicSmartListItem);
			comicListItemFolder.Items.Add(DefaultRecentlyAddedList());
			comicListItemFolder.Items.Add(DefaultRecentlyReadList());
			comicSmartListItem = new ComicSmartListItem(ComicBook.TR["NeverReadList", "Never Read"]);
			comicSmartListItem.Matchers.Add(typeof(ComicBookReadPercentageMatcher), 2, EngineConfiguration.Default.IsNotReadCompletionPercentage.ToString(), "");
			comicListItemFolder.Items.Add(comicSmartListItem);
			comicListItemFolder.Items.Add(DefaultReadingList());
			comicSmartListItem = new ComicSmartListItem(ComicBook.TR["ReadList", "Read"]);
			comicSmartListItem.Matchers.Add(typeof(ComicBookReadPercentageMatcher), 1, EngineConfiguration.Default.IsReadCompletionPercentage.ToString(), "");
			comicListItemFolder.Items.Add(comicSmartListItem);
			comicSmartListItem = new ComicSmartListItem(ComicBook.TR["FilesUpdateList", "Files to update"]);
			comicSmartListItem.Matchers.Add(typeof(ComicBookModifiedInfoMatcher), 0, "", "");
			comicListItemFolder.Items.Add(comicSmartListItem);
		}

		public static ShareableComicListItem DefaultReadingList(ComicLibrary lib = null)
		{
			ComicSmartListItem comicSmartListItem = new ComicSmartListItem(ComicBook.TR["ReadingList", "Reading"]);
			comicSmartListItem.Matchers.Add(typeof(ComicBookReadPercentageMatcher), 3, EngineConfiguration.Default.IsNotReadCompletionPercentage.ToString(), EngineConfiguration.Default.IsReadCompletionPercentage.ToString());
			comicSmartListItem.Library = lib;
			return comicSmartListItem;
		}

		public static ShareableComicListItem DefaultRecentlyReadList(ComicLibrary lib = null)
		{
			ComicSmartListItem comicSmartListItem = new ComicSmartListItem(ComicBook.TR["RecentlyReadList", "Recently Read"]);
			comicSmartListItem.Matchers.Add(typeof(ComicBookOpenedMatcher), 3, EngineConfiguration.Default.IsRecentInDays.ToString(), "");
			comicSmartListItem.Library = lib;
			return comicSmartListItem;
		}

		public static ShareableComicListItem DefaultRecentlyAddedList(ComicLibrary lib = null)
		{
			ComicSmartListItem comicSmartListItem = new ComicSmartListItem(ComicBook.TR["RecentlyAddedList", "Recently Added"]);
			comicSmartListItem.Matchers.Add(typeof(ComicBookAddedMatcher), 3, EngineConfiguration.Default.IsRecentInDays.ToString(), "");
			comicSmartListItem.Library = lib;
			return comicSmartListItem;
		}

		public void ResetDisplayConfigs(DisplayListConfig cfg)
		{
			SetDisplayListConfig(ComicLists, cfg);
		}

		private static void SetDisplayListConfig(IEnumerable<ComicListItem> comicListItemCollection, DisplayListConfig cfg)
		{
			foreach (ComicListItem item in comicListItemCollection)
			{
				item.Display = CloneUtility.Clone(cfg);
				ComicListItemFolder comicListItemFolder = item as ComicListItemFolder;
				if (comicListItemFolder != null)
				{
					SetDisplayListConfig(comicListItemFolder.Items, cfg);
				}
			}
		}

		protected virtual void OnComicListChanged(ComicListItemChangedEventArgs e)
		{
			isDirty = true;
			UpdateQueryCacheListIndex(e);
			if (this.ComicListsChanged != null)
			{
				this.ComicListsChanged(this, e);
			}
		}

		public void NotifyComicListChanged(ComicListItem item, ComicListItemChange changeType)
		{
			OnComicListChanged(new ComicListItemChangedEventArgs(item, changeType));
		}

		protected void AttachComicLists(ComicListItemCollection comicLists)
		{
			this.comicLists = comicLists;
		}

		private void AddCustomValues(ComicBook book)
		{
			if (AddCustomValues(customValues, book) && this.CustomValuesChanged != null)
			{
				this.CustomValuesChanged(this, EventArgs.Empty);
			}
		}

		protected void UpdateQueryCacheListIndex(ComicListItemChangedEventArgs e)
		{
			if (IsQueryCacheEnabled)
			{
				switch (e.Change)
				{
				case ComicListItemChange.Added:
					comicListItemLookup.AddRange(e.Item);
					break;
				case ComicListItemChange.Removed:
					comicListItemLookup.RemoveRange(e.Item);
					break;
				case ComicListItemChange.Edited:
					comicListItemLookup.RemoveRange(e.Item);
					comicListItemLookup.AddRange(e.Item);
					break;
				}
			}
		}

		protected void InvalidateComicListCaches(ComicBook cb, ComicListItem.PendingCacheAction pendingCacheAction, string propertyHint = null)
		{
			if (!IsQueryCacheEnabled || ComicLists.Count == 0)
			{
				return;
			}
			if (pendingCacheAction != ComicListItem.PendingCacheAction.Update || string.IsNullOrEmpty(propertyHint) || ComicBookSeriesStatistics.StatisticProperties.Contains(propertyHint))
			{
				seriesStats = null;
			}
			IEnumerable<ComicListItem> enumerable;
			if (pendingCacheAction == ComicListItem.PendingCacheAction.Update)
			{
				enumerable = comicListItemLookup.Match(propertyHint);
				propertyHint = null;
			}
			else
			{
				enumerable = ComicLists.GetItems<ComicListItem>(bottomUp: true);
			}
			bool flag = false;
			foreach (ComicListItem item in enumerable)
			{
				flag |= item.InvalidateCache(cb, pendingCacheAction, propertyHint);
			}
			if (flag && this.ComicListCachesUpdated != null)
			{
				this.ComicListCachesUpdated(this, EventArgs.Empty);
			}
		}

		public void CommitComicListCacheChanges(Func<ComicListItem, bool> updatePredicate = null)
		{
			if (IsQueryCacheEnabled && ComicLists.Count != 0)
			{
				(from cli in ComicLists.GetItems<ComicListItem>(bottomUp: true)
					where (cli.PendingCacheUpdate || cli.PendingCacheRetrieval) && (updatePredicate == null || updatePredicate(cli))
					select cli).ForEach(delegate(ComicListItem cli)
				{
					cli.CommitCache(block: false);
				});
			}
		}

		public void NotifyDependingComicList(ComicListItem listItem, Action<ComicListItem> action)
		{
			comicListItemLookup.Match(listItem.Id).ForEach(delegate(ComicListItem item)
			{
				item.Notify(action);
			});
		}

		public void NotifyComicListCacheReset(ComicListItem listItem)
		{
			NotifyDependingComicList(listItem, delegate(ComicListItem cli)
			{
				cli.ResetCache();
			});
		}

		public void NotifyComicListCacheUpdate(ComicListItem listItem, ComicBook cb, ComicListItem.PendingCacheAction pendingCacheAction)
		{
			NotifyDependingComicList(listItem, delegate(ComicListItem cli)
			{
				cli.InvalidateCache(cb, pendingCacheAction);
			});
		}

		public ComicBookSeriesStatistics GetSeriesStats(ComicBook book)
		{
			using (ItemMonitor.Lock(seriesStatsLock))
			{
				seriesStats = seriesStats ?? ComicBookSeriesStatistics.Create(GetBooks());
				return seriesStats[new ComicBookSeriesStatistics.Key(book)];
			}
		}

		public byte[] ToByteArray()
		{
			return XmlUtility.Store(this, compressed: true);
		}

		public static ComicLibrary FromByteArray(byte[] data)
		{
			return XmlUtility.Load<ComicLibrary>(data);
		}

		void IDeserializationCallback.OnDeserialization(object sender)
		{
			comicLists.Changed += comicBookLists_Changed;
			comicLists.ForEach(delegate(ComicListItem cli)
			{
				cli.Library = this;
			});
		}

		public static ComicLibrary Attach(ComicLibrary library)
		{
			ComicLibrary comicLibrary = new ComicLibrary
			{
				Name = library.Name,
				Id = library.Id
			};
			comicLibrary.AttachBooks(library.Books);
			comicLibrary.AttachComicLists(library.ComicLists);
			return comicLibrary;
		}

		public static Type[] GetExtraXmlSerializationTypes()
		{
			if (extraTypes == null)
			{
				List<Type> list = new List<Type>();
				list.AddRange(ComicBookValueMatcher.GetAvailableMatcherTypes());
				list.Add(typeof(ComicBookGroupMatcher));
				extraTypes = list.ToArray();
			}
			return extraTypes;
		}

		private static bool AddCustomValues(ISet<string> customValues, ComicBook book, bool showAll = true)
		{
			bool result = false;
			foreach (string item in from p in book.GetCustomValues()
				select p.Key)
			{
				if ((showAll || !item.Contains('.')) && !customValues.Contains(item))
				{
					result = true;
					customValues.Add(item);
				}
			}
			return result;
		}

		public static IEnumerable<string> GetCustomFields(IEnumerable<ComicBook> comicBooks, bool showAll)
		{
			HashSet<string> result = new HashSet<string>();
			foreach (ComicBook comicBook in comicBooks)
			{
				AddCustomValues(result, comicBook, showAll);
			}
			return result;
		}
	}
}
