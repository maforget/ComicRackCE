using System;
using System.IO;
using System.Xml.Serialization;
using cYo.Common.Collections;
using cYo.Projects.ComicRack.Engine.IO.Provider;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookFactory
	{
		[NonSerialized]
		private ComicBookCollection temporaryBooks;

		public ComicBookCollection Storage
		{
			get;
			private set;
		}

		[XmlIgnore]
		public ComicBookCollection TemporaryBooks
		{
			get
			{
				if (temporaryBooks == null)
				{
					temporaryBooks = new ComicBookCollection();
					temporaryBooks.Changed += OnTemporaryBooksChanged;
				}
				return temporaryBooks;
			}
		}

		public bool TemporaryBookListDirty
		{
			get;
			set;
		}

		[field: NonSerialized]
		public event EventHandler<ContainerBookChangedEventArgs> TemporaryBookChanged;

		public ComicBookFactory(ComicBookCollection storage)
		{
			Storage = storage;
		}

		public ComicBook Create(string file, CreateBookOption addOptions, RefreshInfoOptions options)
		{
			if (string.IsNullOrEmpty(file))
			{
				return null;
			}
			ComicBook comicBook;
			if (file.StartsWith("id:", StringComparison.OrdinalIgnoreCase))
			{
				Guid id = new Guid(file.Substring(3));
				comicBook = Storage.FindItemById(id);
				if (comicBook == null)
				{
					return null;
				}
			}
			else
			{
				comicBook = Storage.FindItemByFile(file);
			}
			if (comicBook != null)
			{
				return RefreshComicBookInfo(options, comicBook);
			}
			if (!File.Exists(file) || Providers.Readers.GetSourceProviderType(file) == null)
			{
				TemporaryBooks.Remove(file);
				return null;
			}
			comicBook = RefreshComicBookInfo(options, TemporaryBooks[file]);
			bool flag = comicBook != null;
			comicBook = comicBook ?? ComicBook.Create(file, options);
			switch (addOptions)
			{
				case CreateBookOption.AddToStorage:
					comicBook.AddedTime = DateTime.Now;
					if ((options & RefreshInfoOptions.GetFastPageCount) != 0)
					{
						comicBook.RefreshInfoFromFile(RefreshInfoOptions.GetFastPageCount);
					}
					TemporaryBooks.Remove(comicBook);
					Storage.Add(comicBook);
					break;
				case CreateBookOption.AddToTemporary:
					if (!flag)
					{
						TemporaryBooks.Add(comicBook);
						comicBook.RefreshInfoFromFile(options);
						comicBook.BookChanged += OnTemporaryBookChanged;
					}
					break;
				default:
					throw new ArgumentOutOfRangeException("addOptions");
				case CreateBookOption.DoNotAdd:
					break;
			}
			return comicBook;
		}

		private static ComicBook RefreshComicBookInfo(RefreshInfoOptions options, ComicBook comicBook)
		{
			if (comicBook is null)
				return null;

			if (options.HasFlag(RefreshInfoOptions.ForceRefresh))
				comicBook.RefreshInfoFromFile(options);

			return comicBook;
		}

		public ComicBook Create(string file, CreateBookOption addOptions)
		{
			return Create(file, addOptions, RefreshInfoOptions.None);
		}

		private void OnTemporaryBookChanged(object sender, BookChangedEventArgs e)
		{
			if (this.TemporaryBookChanged != null)
			{
				this.TemporaryBookChanged(this, new ContainerBookChangedEventArgs((ComicBook)sender, e));
			}
		}

		private void OnTemporaryBooksChanged(object sender, SmartListChangedEventArgs<ComicBook> e)
		{
			if (e.Action == SmartListAction.Insert)
			{
				TemporaryBookListDirty = true;
			}
		}
	}
}
