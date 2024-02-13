using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.ComponentModel;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Database;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public class DragDropContainer
	{
		private readonly ComicBookContainer books;

		private ComicBookMatcher matcher;

		private readonly IEnumerable<string> filesOrFolders = Enumerable.Empty<string>();

		private readonly IEnumerable<string> readingLists = Enumerable.Empty<string>();

		public ComicBookContainer Books => books;

		public ComicBookMatcher Matcher => matcher;

		public IEnumerable<string> FilesOrFolders => filesOrFolders;

		public IEnumerable<string> ReadingLists => readingLists;

		public bool IsReadingListsContainer => readingLists.Count() > 0;

		public bool IsBookContainer
		{
			get
			{
				if (books != null)
				{
					return books.Books.Count > 0;
				}
				return false;
			}
		}

		public bool IsFilesContainer => filesOrFolders.Count() > 0;

		public bool HasMatcher => Matcher != null;

		public bool IsValid
		{
			get
			{
				if (!IsBookContainer)
				{
					return IsFilesContainer;
				}
				return true;
			}
		}

		public DragDropContainer()
		{
		}

		public DragDropContainer(ComicBookContainer books, ComicBookMatcher matcher)
		{
			this.books = books;
			this.matcher = matcher;
		}

		public DragDropContainer(IEnumerable<string> filesOrFolders)
		{
			this.filesOrFolders = filesOrFolders;
			readingLists = filesOrFolders.Where((string file) => ".cbl".Equals(Path.GetExtension(file), StringComparison.OrdinalIgnoreCase));
		}

		public IEnumerable<ComicBookMatcher> CreateSeriesGroupMatchers(int maxEntries = 10)
		{
			if (!IsBookContainer)
			{
				yield break;
			}
			if (HasMatcher)
			{
				yield return Matcher;
				yield break;
			}
			foreach (ComicBookGroupMatcher item in (from t in Books.Books.Select((ComicBook cb) => new
				{
					Series = cb.ShadowSeries,
					Volume = cb.ShadowVolume
				}).Distinct()
				orderby t.Series, t.Volume
				select t).Take(maxEntries).Select(t =>
			{
				ComicBookGroupMatcher comicBookGroupMatcher = new ComicBookGroupMatcher
				{
					Matchers = 
					{
						(ComicBookMatcher)new ComicBookSeriesMatcher
						{
							MatchOperator = ComicBookStringMatcher.OperatorEquals,
							MatchValue = t.Series
						}
					}
				};
				if (t.Volume >= 0)
				{
					comicBookGroupMatcher.Matchers.Add(new ComicBookVolumeMatcher
					{
						MatchOperator = ComicBookNumberMatcher.Equal,
						MatchValue = t.Volume.ToString()
					});
				}
				return comicBookGroupMatcher;
			}))
			{
				yield return item;
			}
		}

		public ComicSmartListItem CreateSeriesSmartList()
		{
			if (!IsBookContainer)
			{
				return null;
			}
			ComicSmartListItem comicSmartListItem = new ComicSmartListItem(Books.Name ?? Books.Books[0].ShadowSeries)
			{
				MatcherMode = MatcherMode.Or
			};
			comicSmartListItem.Matchers.AddRange(CreateSeriesGroupMatchers());
			return comicSmartListItem;
		}

		public ComicIdListItem CreateComicIdList()
		{
			if (!IsBookContainer)
			{
				return null;
			}
			ComicIdListItem comicIdListItem = new ComicIdListItem(Books.Name);
			comicIdListItem.AddRange(Books.Books);
			return comicIdListItem;
		}

		public static DragDropContainer Create(IDataObject data)
		{
			if (data.GetDataPresent(typeof(ComicBookContainer)))
			{
				return new DragDropContainer(data.GetData(typeof(ComicBookContainer)) as ComicBookContainer, data.GetData(ComicBookMatcher.ClipboardFormat) as ComicBookMatcher);
			}
			if (data.GetDataPresent(DataFormats.FileDrop))
			{
				return new DragDropContainer((string[])data.GetData(DataFormats.FileDrop));
			}
			return new DragDropContainer();
		}
	}
}
