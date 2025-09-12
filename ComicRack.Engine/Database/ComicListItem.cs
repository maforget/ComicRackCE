using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Threading;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	public abstract class ComicListItem : NamedIdComponent, IComicBookListProvider, ILiteComponent, IDisposable, IIdentity, IComicBookList, IDisplayListConfig, IDeserializationCallback, IComicLibraryItem, IComicBookStatsProvider, ICachedComicBookList
	{
		public enum PendingCacheAction
		{
			Add,
			Remove,
			Update
		}

		[NonSerialized]
		private ComicLibrary library;

		[NonSerialized]
		private ComicListItemFolder parent;

		private bool favorite;

		private int bookCount;

		private int newBookCount;

		private int unreadBookCount;

		private string description = string.Empty;

		private DisplayListConfig displayListConfig = new DisplayListConfig();

        [NonSerialized]
		private ComicLibrary registeredLibrary;

		[NonSerialized]
		private volatile bool pendingCacheUpdate;

		[NonSerialized]
		private volatile HashSet<ComicBook> booksCache;

		[NonSerialized]
		private HashSet<ComicBook> unreadBooksCache;

		[NonSerialized]
		private HashSet<ComicBook> newBooksCache;

		[NonSerialized]
		private Dictionary<ComicBook, PendingCacheAction> pendingCacheItems;

		[NonSerialized]
		private DateTime now;

		[NonSerialized]
		private bool pendingCacheRetrieval;

		private volatile bool notifyShield;

		private static readonly string[] defaultDependentProperties = new string[3]
		{
			"AddedTime",
			"LastPageRead",
			"PageCount"
		};

		[NonSerialized]
		private IAsyncResult updateResult;

		[NonSerialized]
		private readonly ThreadLocal<bool> recurseShield = new ThreadLocal<bool>();

		[NonSerialized]
		private Dictionary<ComicBookSeriesStatistics.Key, ComicBookSeriesStatistics> seriesStats;

		[NonSerialized]
		private object seriesStatsLock = new object();

		[XmlIgnore]
		public virtual ComicLibrary Library
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
					OnLibraryChanged();
				}
			}
		}

		[XmlIgnore]
		public ComicListItemFolder Parent
		{
			get
			{
				return parent;
			}
			set
			{
				if (value != parent)
				{
					parent = value;
				}
			}
		}

		public virtual string ImageKey => "List";

		[XmlAttribute]
		[DefaultValue(false)]
		public virtual bool Favorite
		{
			get
			{
				return favorite;
			}
			set
			{
				if (value != favorite)
				{
					favorite = value;
					OnChanged(ComicListItemChange.Edited);
				}
			}
		}

		[DefaultValue(0)]
		public virtual int BookCount
		{
			get
			{
				return bookCount;
			}
			set
			{
				if (bookCount != value)
				{
					bookCount = value;
					OnChanged(ComicListItemChange.Statistic);
				}
			}
		}

		[DefaultValue(0)]
		public virtual int NewBookCount
		{
			get
			{
				return newBookCount;
			}
			set
			{
				if (newBookCount != value)
				{
					newBookCount = value;
					OnChanged(ComicListItemChange.Statistic);
				}
			}
		}

		[DefaultValue(typeof(DateTime), "01.01.0001")]
		public DateTime NewBookCountDate
		{
			get;
			set;
		}

		[DefaultValue(0)]
		public virtual int UnreadBookCount
		{
			get
			{
				return unreadBookCount;
			}
			set
			{
				if (unreadBookCount != value)
				{
					unreadBookCount = value;
					OnChanged(ComicListItemChange.Statistic);
				}
			}
		}

		[DefaultValue("")]
		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				if (!(description == value))
				{
					description = value;
					OnChanged(ComicListItemChange.Edited);
				}
			}
		}

		[DefaultValue(null)]
		public string CacheStorage
		{
			get;
			set;
		}

		public DisplayListConfig Display
		{
			get
			{
				return displayListConfig;
			}
			set
			{
				displayListConfig = value;
			}
		}

		public virtual bool CacheEnabled
		{
			get
			{
				if (Library != null && Library.IsLoaded)
				{
					return ComicLibrary.IsQueryCacheEnabled;
				}
				return false;
			}
		}

		public virtual bool PendingCacheUpdate
		{
			get
			{
				if (CacheEnabled)
				{
					if (booksCache != null || CacheStorage != null)
					{
						return pendingCacheUpdate;
					}
					return true;
				}
				return false;
			}
		}

		public virtual bool PendingCacheRetrieval
		{
			get
			{
				if (CacheEnabled && CacheStorage != null)
				{
					return pendingCacheRetrieval;
				}
				return false;
			}
		}

		public virtual bool OptimizedCacheUpdateDisabled => false;

		public virtual bool CustomCacheStorage => false;

        [field: NonSerialized]
		public event ComicListChangedEventHandler Changed;

		[field: NonSerialized]
		public event EventHandler BookListChanged;

		protected ComicListItem()
		{
			NewBookCountDate = DateTime.MinValue;
		}

		protected virtual void OnLibraryChanged()
		{
			if (registeredLibrary != null)
			{
				registeredLibrary.BookListChanged -= Library_BookListRefreshed;
			}
			if (Library != null)
			{
				Library.BookListChanged += Library_BookListRefreshed;
			}
			ResetCache();
			registeredLibrary = Library;
		}

		protected virtual void OnBookListRefreshed()
		{
			OnBookListChanged();
		}

		protected abstract IEnumerable<ComicBook> OnGetBooks();

		public virtual bool Filter(string filter)
		{
			if (filter != null)
			{
				return (base.Name ?? string.Empty).IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) != -1;
			}
			return false;
		}

		protected virtual void OnChanged(ComicListItemChangedEventArgs e)
		{
			if (Library != null)
			{
				Library.NotifyComicListChanged(e.Item, e.Change);
			}
			if (this.Changed != null)
			{
				this.Changed(this, e);
			}
		}

		protected void OnChanged(ComicListItemChange changeType, ComicListItem item = null)
		{
			OnChanged(new ComicListItemChangedEventArgs(item ?? this, changeType));
		}

		private void Library_BookListRefreshed(object sender, EventArgs e)
		{
			OnBookListRefreshed();
		}

		public IEnumerable<ComicBook> GetBooks()
		{
			seriesStats = null;
			try
			{
				if (!CacheEnabled)
				{
					return InvokeGetBooks();
				}
				CommitCache(block: true);
				return (booksCache == null) ? Enumerable.Empty<ComicBook>() : booksCache.Lock();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public virtual void Refresh()
		{
			ResetCache();
			OnBookListChanged();
		}

		protected virtual void OnBookListChanged()
		{
			if (this.BookListChanged != null)
			{
				this.BookListChanged(this, EventArgs.Empty);
			}
		}

		public void NotifyCacheRetrieval()
		{
			pendingCacheRetrieval = true;
		}

		public void Notify(Action<ComicListItem> action)
		{
			if (!notifyShield)
			{
				try
				{
					notifyShield = true;
					action(this);
				}
				finally
				{
					notifyShield = false;
				}
			}
		}

		public bool ResetCache(bool rebuildNow = false)
		{
			if (!CacheEnabled)
			{
				return false;
			}
			if (booksCache == null && CacheStorage == null)
			{
				return false;
			}
			booksCache = null;
			pendingCacheItems = null;
			CacheStorage = null;
			pendingCacheUpdate = true;
			if (Parent != null)
			{
				Parent.ResetCache();
			}
			if (Library != null)
			{
				Library.NotifyComicListCacheReset(this);
			}
			if (rebuildNow)
			{
				CommitCache(block: false);
			}
			return true;
		}

		public void ResetCacheWithStatistics(bool rebuildNow = false)
		{
			int num2 = (NewBookCount = 0);
			int num5 = (BookCount = (UnreadBookCount = num2));
			NewBookCountDate = DateTime.MinValue;
			ResetCache(rebuildNow);
		}

		public void RetrieveCache()
		{
			try
			{
				pendingCacheRetrieval = false;
				if (CacheStorage == null || Library == null)
				{
					return;
				}
				HashSet<ComicBook> hashSet = new HashSet<ComicBook>();
				string cacheStorage = CacheStorage;
				CacheStorage = null;
				using (ItemMonitor.Lock(hashSet))
				{
					if (cacheStorage == "Custom")
					{
						if (!OnRetrieveCustomCache(hashSet))
						{
							throw new NotImplementedException();
						}
					}
					else
					{
						string[] array = cacheStorage.Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
						foreach (string g in array)
						{
							Guid id = new Guid(g);
							ComicBook comicBook = Library.Books[id];
							if (comicBook != null)
							{
								hashSet.Add(comicBook);
							}
						}
					}
				}
				InitializeBookCounters(hashSet);
				booksCache = hashSet;
			}
			catch (Exception)
			{
				booksCache = null;
			}
		}

		public virtual IEnumerable<string> GetDependentProperties()
		{
			return defaultDependentProperties;
		}

		public virtual bool IsUpdateNeeded(string propertyHint)
		{
			return true;
		}

		public bool InvalidateCache(ComicBook cb, PendingCacheAction action, string propertyHint = null)
		{
			if (!CacheEnabled)
			{
				return false;
			}
			if (booksCache == null && CacheStorage == null)
			{
				return false;
			}
			if (!string.IsNullOrEmpty(propertyHint) && action == PendingCacheAction.Update && propertyHint != "AddedTime" && propertyHint != "LastPageRead" && propertyHint != "PageCount" && !IsUpdateNeeded(propertyHint))
			{
				return false;
			}
			if (OptimizedCacheUpdateDisabled)
			{
				ResetCache();
				return true;
			}
			pendingCacheUpdate = true;
			bool flag = false;
			using (ItemMonitor.Lock(pendingCacheItems))
			{
				if (pendingCacheItems == null)
				{
					pendingCacheItems = new Dictionary<ComicBook, PendingCacheAction>();
				}
				if (!pendingCacheItems.TryGetValue(cb, out var value) || value != action)
				{
					pendingCacheItems[cb] = action;
					flag = true;
				}
			}
			if (flag)
			{
				if (Parent != null)
				{
					Parent.InvalidateCache(cb, action);
				}
				if (Library != null)
				{
					Library.NotifyComicListCacheUpdate(this, cb, action);
				}
			}
			return flag;
		}

		public void CommitCache(bool block)
		{
			if (!CacheEnabled)
			{
				return;
			}
			RetrieveCache();
			if (!PendingCacheUpdate)
			{
				return;
			}
			HashSet<ComicBook> hashSet = booksCache;
			Dictionary<ComicBook, PendingCacheAction> dictionary = pendingCacheItems;
			pendingCacheUpdate = false;
			if (hashSet == null || dictionary == null || OptimizedCacheUpdateDisabled)
			{
				RunUpdate(block || !ComicLibrary.BackgroundQueryCacheUpdate, delegate
				{
					HashSet<ComicBook> hashSet3 = new HashSet<ComicBook>(InvokeGetBooks());
					InitializeBookCounters(hashSet3);
					booksCache = hashSet3;
					pendingCacheItems = null;
				});
				return;
			}
			try
			{
				using (ItemMonitor.Lock(dictionary))
				{
					using (ItemMonitor.Lock(hashSet))
					{
						ComicBook[] second = (from kvp in dictionary
							where kvp.Value == PendingCacheAction.Remove
							select kvp.Key).ToArray();
						ComicBook[] array = (from kvp in dictionary
							where kvp.Value != PendingCacheAction.Remove
							select kvp.Key).ToArray();
						HashSet<ComicBook> hashSet2 = null;
						now = DateTime.Now;
						foreach (ComicBook item in array.Concat(second))
						{
							if (hashSet.Remove(item))
							{
								if (hashSet2 == null)
								{
									hashSet2 = new HashSet<ComicBook>();
								}
								hashSet2.Add(item);
								if (newBooksCache != null && newBooksCache.Contains(item))
								{
									newBooksCache.Remove(item);
								}
								if (unreadBooksCache != null && unreadBooksCache.Contains(item))
								{
									unreadBooksCache.Remove(item);
								}
							}
						}
						foreach (ComicBook item2 in OnCacheMatch(array))
						{
							CreateBookCacheStatus(item2);
							hashSet.Add(item2);
							hashSet2?.Remove(item2);
						}
					}
				}
				BookCount = hashSet.Count;
				NewBookCount = ((newBooksCache != null) ? newBooksCache.Count : 0);
				UnreadBookCount = ((unreadBooksCache != null) ? unreadBooksCache.Count : 0);
				dictionary.Clear();
			}
			catch (Exception)
			{
				ResetCache();
			}
		}

		private void RunUpdate(bool block, Action action)
		{
			IAsyncResult asyncResult = updateResult;
			try
			{
				if (block)
				{
					if (asyncResult != null && !asyncResult.IsCompleted)
					{
						asyncResult.AsyncWaitHandle.WaitOne();
					}
					action();
				}
				else if (asyncResult == null || asyncResult.IsCompleted)
				{
					updateResult = ThreadUtility.RunInThreadQueue(action);
				}
			}
			catch (Exception)
			{
				action();
			}
		}

		public void StoreCache()
		{
			if (pendingCacheUpdate)
			{
				CommitCache(block: true);
			}
			if (CacheStorage != null)
			{
				return;
			}
			if (booksCache == null)
			{
				CacheStorage = null;
				return;
			}
			if (CustomCacheStorage)
			{
				CacheStorage = "Custom";
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ComicBook item in booksCache)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(item.Id);
			}
			CacheStorage = stringBuilder.ToString();
		}

		protected abstract IEnumerable<ComicBook> OnCacheMatch(IEnumerable<ComicBook> cbl);

		public virtual ISet<ComicBook> GetCache()
		{
			if (!CacheEnabled)
			{
				throw new InvalidOperationException("Caching is not turned on");
			}
			CommitCache(block: true);
			return booksCache;
		}

		private void InitializeBookCounters(ICollection<ComicBook> booksCache)
		{
			if (booksCache != null)
			{
				now = DateTime.Now;
				using (ItemMonitor.Lock(this))
				{
					newBooksCache = (unreadBooksCache = null);
					booksCache.ForEach(CreateBookCacheStatus);
					BookCount = booksCache.Count;
				}
				NewBookCount = ((newBooksCache != null) ? newBooksCache.Count : 0);
				NewBookCountDate = DateTime.UtcNow;
				UnreadBookCount = ((unreadBooksCache != null) ? unreadBooksCache.Count : 0);
			}
		}

		private void CreateBookCacheStatus(ComicBook cb)
		{
			if (cb.HasBeenRead)
			{
				return;
			}
			if ((now - cb.AddedTime).TotalDays < (double)EngineConfiguration.Default.IsRecentInDays)
			{
				if (newBooksCache == null)
				{
					newBooksCache = new HashSet<ComicBook>();
				}
				newBooksCache.Add(cb);
			}
			else
			{
				if (unreadBooksCache == null)
				{
					unreadBooksCache = new HashSet<ComicBook>();
				}
				unreadBooksCache.Add(cb);
			}
		}

		protected virtual bool OnRetrieveCustomCache(HashSet<ComicBook> books)
		{
			return false;
		}

		public override string ToString()
		{
			return Description ?? string.Empty;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && registeredLibrary != null)
			{
				registeredLibrary.BookListChanged -= Library_BookListRefreshed;
			}
			base.Dispose(disposing);
		}

		protected override void OnNameChanged()
		{
			base.OnNameChanged();
			OnChanged(ComicListItemChange.Edited);
		}

		protected IEnumerable<ComicBook> InvokeGetBooks()
		{
			return OnGetBooks();
		}

		public bool RecursionTest()
		{
			return RecursionTest(base.Id, ignoreSelfTest: true);
		}

		public bool RecursionTest(ComicListItem test)
		{
			return RecursionTest(test.Id);
		}

		public bool RecursionTest(Guid listId, bool ignoreSelfTest = false)
		{
			if (!ignoreSelfTest && base.Id == listId)
                return true;

            if (recurseShield.IsValueCreated && recurseShield.Value)
                return false;

            RecursionCacheItem cachedItem = RecursionCache.Items.GetValue(listId);
			if (cachedItem != null && cachedItem.TryGetValue(base.Id, out bool cachedResult))
				return cachedResult;

			bool result = false;
            try
			{
				recurseShield.Value = true;
				ComicListItemFolder comicListItemFolder = this as ComicListItemFolder;
				if (comicListItemFolder != null && comicListItemFolder.Items.Any((ComicListItem cli) => cli.RecursionTest(listId)))
                    result = true;

                ComicSmartListItem comicSmartListItem = this as ComicSmartListItem;
				if (comicSmartListItem != null)
				{
					ComicListItem baseList = comicSmartListItem.GetBaseList(withTest: false);
					if (baseList != null && baseList.RecursionTest(listId))
                        result = true;
                }
			}
			finally
			{
				recurseShield.Value = false;
			}

			if (cachedItem != null)
				cachedItem[base.Id] = result;

			return result;
        }

		void IDeserializationCallback.OnDeserialization(object sender)
		{
			if (registeredLibrary != null)
			{
				registeredLibrary.BookListChanged += Library_BookListRefreshed;
			}
		}

		public ComicBookSeriesStatistics GetSeriesStats(ComicBook book)
		{
			if (CacheEnabled)
			{
				return Library.GetSeriesStats(book);
			}
			using (ItemMonitor.Lock(seriesStatsLock))
			{
				seriesStats = seriesStats ?? ComicBookSeriesStatistics.Create(Library.GetBooks());
				return seriesStats[new ComicBookSeriesStatistics.Key(book)];
			}
		}
	}
}
