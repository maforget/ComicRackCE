using System;
using System.Collections.Generic;
using System.Linq;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Localize;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	public class ComicIdListItem : ShareableComicListItem, IEditableComicBookListProvider, IComicBookListProvider, ILiteComponent, IDisposable, IIdentity, IComicBookList
	{
		private readonly SmartList<Guid> bookIds = new SmartList<Guid>();

		public SmartList<Guid> BookIds => bookIds;

		public bool IsLibrary => false;

		public override bool CustomCacheStorage => true;

		public ComicIdListItem(string name)
		{
			base.Name = name;
		}

		public ComicIdListItem()
			: this(TR.Default["New", "New"])
		{
		}

		public ComicIdListItem(ComicListItem item, Func<ComicBook, bool> predicate = null)
		{
			base.Name = item.Name;
			base.Description = item.Description;
			base.Display = item.Display;
			if (item is ComicIdListItem && predicate == null)
			{
				BookIds.AddRange(((ComicIdListItem)item).BookIds);
				return;
			}
			BookIds.AddRange(from cb in item.GetBooks()
				where predicate == null || predicate(cb)
				select cb.Id);
		}

		protected override IEnumerable<ComicBook> OnGetBooks()
		{
			HashSet<ComicBook> hashSet = new HashSet<ComicBook>();
			if (Library != null && Library.Books != null)
			{
				List<Guid> list = null;
				List<Guid> list2 = null;
				foreach (Guid bookId in bookIds)
				{
					ComicBook comicBook = Library.Books[bookId];
					if (comicBook != null)
					{
						if (hashSet.Contains(comicBook))
						{
							if (list2 == null)
							{
								list2 = new List<Guid>();
							}
							list2.Add(bookId);
						}
						hashSet.Add(comicBook);
					}
					else
					{
						if (list == null)
						{
							list = new List<Guid>();
						}
						list.Add(bookId);
					}
				}
				if (list != null)
				{
					bookIds.RemoveRange(list);
				}
				if (list2 != null)
				{
					bookIds.RemoveRange(list2);
				}
			}
			return hashSet;
		}

		public int Add(ComicBook comicBook)
		{
			return Insert(bookIds.Count, comicBook);
		}

		public void AddRange(IEnumerable<ComicBook> books)
		{
			books.ForEach(delegate(ComicBook b)
			{
				Add(b);
			});
		}

		public int Insert(int index, ComicBook comicBook)
		{
			int num = bookIds.IndexOf(comicBook.Id);
			if (num != index)
			{
				if (num != -1)
				{
					bookIds.RemoveAt(num);
					if (index > bookIds.Count)
					{
						index = bookIds.Count;
					}
					if (num < index)
					{
						index--;
					}
				}
				bookIds.Insert(index, comicBook.Id);
				Refresh();
			}
			return index;
		}

		public bool Remove(ComicBook comicBook)
		{
			bool flag = bookIds.Remove(comicBook.Id);
			if (flag)
			{
				Refresh();
			}
			return flag;
		}

		public override object Clone()
		{
			return new ComicIdListItem(this);
		}

		protected override IEnumerable<ComicBook> OnCacheMatch(IEnumerable<ComicBook> cbl)
		{
			return cbl.Where((ComicBook cb) => BookIds.Contains(cb.Id));
		}

		protected override bool OnRetrieveCustomCache(HashSet<ComicBook> books)
		{
			InvokeGetBooks().ForEach(delegate(ComicBook b)
			{
				books.Add(b);
			});
			return true;
		}

		public static ComicIdListItem CreateFromReadingList(ComicBookCollection library, IEnumerable<ComicReadingListItem> readingItems, IList<ComicBook> booksToAdd = null, Func<int, bool> progress = null)
		{
			ComicIdListItem comicIdListItem = new ComicIdListItem();
			int num = readingItems.Count();
			int num2 = 0;
			foreach (ComicReadingListItem crli in readingItems)
			{
				if (progress != null && !progress(num2++ * 100 / num))
				{
					return null;
				}
				ComicBook comicBook = library[crli.Id];
				if (comicBook != null)
				{
					comicIdListItem.BookIds.Add(comicBook.Id);
					continue;
				}
				if (!string.IsNullOrEmpty(crli.FileName))
				{
					comicBook = library.FindItemByFileName(crli.FileName);
					if (comicBook != null)
					{
						comicIdListItem.BookIds.Add(comicBook.Id);
						continue;
					}
				}
				crli.SetFileNameInfo();
				IEnumerable<ComicBook> enumerable = library.Where((ComicBook ci) => ci.ShadowNumber == crli.Number && ComicInfo.SeriesEquals(ci.ShadowSeries, crli.Series, CompareSeriesOptions.None));
				if (enumerable.Count() == 0)
				{
					enumerable = library.Where((ComicBook ci) => ci.ShadowNumber == crli.Number && ComicInfo.SeriesEquals(ci.ShadowSeries, crli.Series, CompareSeriesOptions.IgnoreVolumeInName));
				}
				if (enumerable.Count() == 0)
				{
					enumerable = library.Where((ComicBook ci) => ci.ShadowNumber == crli.Number && ComicInfo.SeriesEquals(ci.ShadowSeries, crli.Series, CompareSeriesOptions.IgnoreVolumeInName | CompareSeriesOptions.StripDown));
				}
				if (enumerable.Count() > 1)
				{
					IEnumerable<ComicBook> enumerable2 = enumerable;
					enumerable = enumerable2.Where((ComicBook ci) => Math.Abs(ci.ShadowYear - crli.Year) <= 1);
					if (enumerable.Count() == 0)
					{
						enumerable = enumerable2;
					}
				}
				if (enumerable.Count() > 1)
				{
					IEnumerable<ComicBook> enumerable3 = enumerable;
					enumerable = enumerable3.Where((ComicBook ci) => ci.ShadowVolume == crli.Volume);
					if (enumerable.Count() == 0)
					{
						enumerable = enumerable3;
					}
				}
				if (enumerable.Count() > 1 && !string.IsNullOrEmpty(crli.Format))
				{
					IEnumerable<ComicBook> enumerable4 = enumerable;
					enumerable = enumerable4.Where((ComicBook ci) => string.Compare(ci.ShadowFormat, crli.Format, StringComparison.OrdinalIgnoreCase) == 0);
					if (enumerable.Count() == 0)
					{
						enumerable = enumerable4;
					}
				}
				comicBook = enumerable.FirstOrDefault();
				if (comicBook == null)
				{
					comicBook = new ComicBook
					{
						AddedTime = DateTime.Now,
						Series = crli.Series,
						Number = crli.Number,
						Volume = crli.Volume,
						Year = crli.Year,
						Format = crli.Format
					};
					booksToAdd?.Add(comicBook);
				}
				comicIdListItem.BookIds.Add(comicBook.Id);
			}
			return comicIdListItem;
		}

		//Decompile Error
		//void IComicBookListProvider.add_NameChanged(EventHandler value)
		//{
		//	base.NameChanged += value;
		//}

		//void IComicBookListProvider.remove_NameChanged(EventHandler value)
		//{
		//	base.NameChanged -= value;
		//}

		//Guid IIdentity.get_Id()
		//{
		//	return base.Id;
		//}

		//string IComicBookList.get_Name()
		//{
		//	return base.Name;
		//}
	}
}
