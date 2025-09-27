using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Localize;
using cYo.Common.Runtime;
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.IO.Cache;

namespace cYo.Projects.ComicRack.Engine.Sync
{
	public class StorageSync
	{
		public class SyncErrorEventArgs : CancelEventArgs
		{
			public string Message
			{
				get;
				private set;
			}

			public SyncErrorEventArgs(string message)
			{
				Message = message;
			}
		}

		public class FatalSyncException : Exception
		{
			public FatalSyncException()
			{
			}

			public FatalSyncException(string message, Exception inner = null)
				: base(message, inner)
			{
			}
		}

		public class DeviceOutOfSpaceException : FatalSyncException
		{
			public DeviceOutOfSpaceException(string message, Exception inner = null)
				: base(message, inner)
			{
			}
		}

		private readonly ISyncProvider provider;

		public EventHandler<SyncErrorEventArgs> Error;

		private long lastUpdateTime;

		private string oldMessage;

		public StorageSync(ISyncProvider provider)
		{
			this.provider = provider;
		}

		protected virtual void OnError(SyncErrorEventArgs e)
		{
			if (Error != null)
			{
				Error(this, e);
			}
		}

		protected bool InvokeError(string message)
		{
			SyncErrorEventArgs syncErrorEventArgs = new SyncErrorEventArgs(message);
			OnError(syncErrorEventArgs);
			return !syncErrorEventArgs.Cancel;
		}

		public void Synchronize(DeviceSyncSettings settings, ComicBookContainer library, ComicListItemCollection comicLists, IPagePool pagePool, IProgressState progress)
		{
			try
			{
				IEnumerable<ComicIdListItem> lists = null;
				try
				{
					provider.ValidateDevice(provider.Device);
					provider.Start();
					UpdateProgress(progress, 0, TR.Messages["SyncGetStatus", "Retrieving current status from device"]);
					ComicBook[] array = provider.GetBooks().ToArray();
					if (progress.Abort)
					{
						return;
					}
					int percentStart = 10;
					int i = 0;
					ComicBook[] array2 = array;
					foreach (ComicBook comicBook in array2)
					{
						string message = TR.Messages["SyncUpdateLibrary", "Updating library from '{0}'"].SafeFormat(comicBook.CaptionWithoutTitle);
						if (!UpdateProgress(progress, percentStart + ++i * 10 / array.Length, message))
						{
							return;
						}
						ComicBook comicBook2 = library.Books.FindItemById(comicBook.Id);
						if (comicBook2 == null || !comicBook.ComicInfoIsDirty || comicBook.ExtraSyncInformation == null)
						{
							continue;
						}
						if (comicBook.ExtraSyncInformation.ReadingStateChanged && (comicBook.OpenedTime > comicBook2.OpenedTime || comicBook.OpenedCount == 0))
						{
							comicBook2.OpenedTime = comicBook.OpenedTime;
							comicBook2.OpenedCount = comicBook.OpenedCount;
							comicBook2.LastPageRead = comicBook.LastPageRead;
							comicBook2.CurrentPage = comicBook.CurrentPage;
						}
						if (comicBook.ExtraSyncInformation.InformationChanged)
						{
							comicBook2.Rating = comicBook.Rating;
							comicBook2.Manga = comicBook.Manga;
							if (!string.IsNullOrEmpty(comicBook.Review))
							{
								comicBook2.Review = comicBook.Review;
							}
						}
						if (comicBook.ExtraSyncInformation.BookmarksChanged)
						{
							for (int k = 0; k < comicBook.Pages.Count; k++)
							{
								comicBook2.UpdateBookmark(k, comicBook.GetPage(k).Bookmark);
							}
						}
						if (comicBook.ExtraSyncInformation.PageTypesChanged)
						{
							for (int l = 0; l < comicBook.Pages.Count; l++)
							{
								comicBook2.UpdatePageType(l, comicBook.GetPage(l).PageType);
							}
						}
						if (comicBook.ExtraSyncInformation.CheckChanged)
						{
							comicBook2.Checked = comicBook.Checked;
						}
						if (comicBook.ExtraSyncInformation.DataChanged)
						{
							comicBook2.SetInfo(comicBook, onlyUpdateEmpty: false, updatePages: false);
						}
					}
					ComicBook.ClearExtraSyncInformation();
					var source = (from sl in settings.Lists
						select new
						{
							List = comicLists.FindItem(sl.ListId),
							Setting = sl
						} into cli
						where cli.List != null
						select cli).Select(cli =>
					{
						cli.List.ResetCache();
						return new
						{
							List = LimitList(new ComicIdListItem(cli.List, (ComicBook cb) => cb.IsLinked && (cb.Checked || !cli.Setting.OnlyChecked)), library, cli.Setting, cli.List is ComicIdListItem),
							Setting = cli.Setting
						};
					}).ToList();
					int count2 = ((provider.Device.BookSyncLimit > 0) ? provider.Device.BookSyncLimit : int.MaxValue);
					Dictionary<Guid, ComicBook> dictionary = new Dictionary<Guid, ComicBook>((from id in source.SelectMany(bl => bl.List.BookIds).Distinct()
						select library.Books[id]).Take(count2).ToDictionary((ComicBook cb) => cb.Id));
					HashSet<Guid> hashSet = new HashSet<Guid>(source.Where(bl => bl.Setting.OptimizePortable).SelectMany(bl => bl.List.BookIds).Distinct());
					lists = source.Select(bl => bl.List);
					i = 0;
					percentStart += 10;
					ComicBook[] array3 = array;
					foreach (ComicBook comicBook3 in array3)
					{
						string message2 = TR.Messages["SyncRemoveBook", "Removing '{0}' from device"].SafeFormat(comicBook3.CaptionWithoutTitle);
						if (!UpdateProgress(progress, percentStart + ++i * 10 / array.Length, message2))
						{
							return;
						}
						if (!dictionary.ContainsKey(comicBook3.Id))
						{
							provider.Remove(comicBook3);
						}
					}
					int count = dictionary.Count();
					i = 0;
					percentStart += 10;
					string msg;
					foreach (ComicBook value in dictionary.Values)
					{
						if (progress.Abort)
						{
							break;
						}
						try
						{
							msg = TR.Messages["SyncCopyBook", "Copying '{0}' to device"].SafeFormat(value.CaptionWithoutTitle);
							provider.Add(value, hashSet.Contains(value.Id), pagePool, delegate
							{
								UpdateProgress(progress, percentStart + i * 65 / count);
							}, delegate
							{
								UpdateProgress(progress, percentStart + i * 65 / count, msg);
							}, delegate
							{
								UpdateProgress(progress, percentStart + ++i * 65 / count);
							});
						}
						catch (FatalSyncException)
						{
							throw;
						}
						catch (Exception ex2)
						{
							InvokeError(ex2.Message);
						}
					}
				}
				finally
				{
					try
					{
						provider.WaitForWritesCompleted();
						provider.SetLists(lists);
					}
					catch (Exception)
					{
					}
					provider.Completed();
				}
			}
			catch (Exception ex4)
			{
				InvokeError(ex4.Message);
			}
		}

		private bool UpdateProgress(IProgressState progress, int percent, string message = null)
		{
			bool flag = true;
			long ticks = Machine.Ticks;
			if (ticks > lastUpdateTime + 5000)
			{
				flag = provider.Progress(percent);
				lastUpdateTime = ticks;
			}
			if (progress != null)
			{
				if (!flag)
				{
					progress.Abort = true;
				}
				progress.ProgressPercentage = percent;
				string text2 = (oldMessage = (progress.ProgressMessage = message ?? oldMessage));
				if (!progress.ProgressAvailable)
				{
					progress.ProgressAvailable = true;
				}
				if (progress.Abort)
				{
					return false;
				}
			}
			return true;
		}

		private static ComicIdListItem LimitList(ComicIdListItem list, ComicBookContainer library, DeviceSyncSettings.SharedList setting, bool isOrderedList)
		{
			List<ComicBook> list2 = (from id in list.BookIds
				select library.Books[id] into cb
				where cb != null
				select cb).ToList();
			IComparer<ComicBook> comparer = null;
			IGrouper<ComicBook> grouper = null;
			if (setting.Sort)
			{
				switch (setting.ListSortType)
				{
					default:
						comparer = new ChainedComparer<ComicBook>(new ComicBookSeriesComparer(), new ComicBookPublishedComparer());
						grouper = new ComicBookGroupSeries();
						break;
					case DeviceSyncSettings.ListSort.Random:
						list2.Randomize();
						break;
					case DeviceSyncSettings.ListSort.AlternateSeries:
						comparer = new ChainedComparer<ComicBook>(new ComicBookAlternateSeriesComparer(), new ComicBookSeriesComparer(), new ComicBookPublishedComparer());
						grouper = new ComicBookGroupAlternateSeries();
						break;
					case DeviceSyncSettings.ListSort.Published:
						comparer = new ChainedComparer<ComicBook>(new ComicBookPublishedComparer(), new ComicBookSeriesComparer());
						break;
					case DeviceSyncSettings.ListSort.Added:
						comparer = new ComicBookAddedComparer();
						break;
					case DeviceSyncSettings.ListSort.ListOrder:
						comparer = ComicBookMetadataManager.GetComparers(list.Display.View.SortKey)?.Chain();
						grouper = ComicBookMetadataManager.GetGroupers(list.Display.View.GrouperId);
						break;
					case DeviceSyncSettings.ListSort.StoryArc:
						comparer = new ChainedComparer<ComicBook>(new ComicBookStoryArcComparer(), new ComicBookAlternateNumberComparer(), new ComicBookSeriesComparer(), new ComicBookPublishedComparer());
						grouper = new ComicBookGroupStoryArc();
						break;
				}
			}
			List<ComicBook>[] array = ((grouper != null) ? (from gc in new GroupManager<ComicBook>(grouper, list2).GetGroups()
				orderby gc.Key
				select gc.Items).ToArray() : new List<ComicBook>[1]
			{
				new List<ComicBook>(list2)
			});
			if (comparer != null)
			{
				List<ComicBook>[] array2 = array;
				foreach (List<ComicBook> list3 in array2)
				{
					list3.Sort(comparer);
				}
			}
			if (setting.OnlyUnread)
			{
				for (int k = 0; k < array.Length; k++)
				{
					array[k] = GetUnreadBooks(array[k], setting.KeepLastRead ? EngineConfiguration.Default.SyncKeepReadComics : 0);
				}
			}
			list2.Clear();
			int num = ((array.Length != 0) ? array.Max((List<ComicBook> gr) => gr.Count) : 0);
			int i;
			for (i = 0; i < num; i++)
			{
				list2.AddRange(from gr in array
					where i < gr.Count
					select gr[i]);
			}
			list.BookIds.Clear();
			if (!setting.Limit)
			{
				list.BookIds.AddRange(list2.Select((ComicBook cb) => cb.Id));
			}
			else
			{
				switch (setting.LimitValueType)
				{
				default:
					list.BookIds.AddRange(from cb in list2.Take(setting.LimitValue)
						select cb.Id);
					break;
				case DeviceSyncSettings.LimitType.MB:
					list.BookIds.AddRange(CopyIdsWithSizeLimit(list2, (long)setting.LimitValue * 1024L * 1024, setting.OptimizePortable));
					break;
				case DeviceSyncSettings.LimitType.GB:
					list.BookIds.AddRange(CopyIdsWithSizeLimit(list2, (long)setting.LimitValue * 1024L * 1024 * 1024, setting.OptimizePortable));
					break;
				}
			}
			return list;
		}

		private static IEnumerable<Guid> CopyIdsWithSizeLimit(IEnumerable<ComicBook> books, long size, bool optimized)
		{
			foreach (ComicBook book in books)
			{
				long num = (book.IsDynamicSource ? (book.PageCount * 250000) : book.FileSize);
				if (optimized)
				{
					num /= 2;
				}
				size -= num;
				if (size < 0)
				{
					break;
				}
				yield return book.Id;
			}
		}

		private static List<ComicBook> GetUnreadBooks(IList<ComicBook> books, int prologSize = 0)
		{
			List<ComicBook> list = new List<ComicBook>();
			for (int i = 0; i < books.Count; i++)
			{
				if (books[i].HasBeenRead)
				{
					continue;
				}
				if (list.Count == 0)
				{
					for (int j = Math.Max(0, i - prologSize); j < i; j++)
					{
						list.Add(books[j]);
					}
				}
				list.Add(books[i]);
			}
			return list;
		}
	}
}
