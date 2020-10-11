using System;
using System.Collections.Generic;
using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	public class ComicLibraryListItem : ComicListItem, IEditableComicBookListProvider, IComicBookListProvider, ILiteComponent, IDisposable, IIdentity, IComicBookList
	{
		public override string ImageKey => "Library";

		public override bool CustomCacheStorage => true;

		public bool IsLibrary => true;

		public ComicLibraryListItem(string name)
		{
			base.Name = name;
		}

		public ComicLibraryListItem()
			: this("Library")
		{
		}

		protected override IEnumerable<ComicBook> OnCacheMatch(IEnumerable<ComicBook> cbl)
		{
			return cbl;
		}

		protected override bool OnRetrieveCustomCache(HashSet<ComicBook> books)
		{
			Library.Books.ForEach(delegate(ComicBook b)
			{
				books.Add(b);
			});
			return true;
		}

		protected override IEnumerable<ComicBook> OnGetBooks()
		{
			return Library.GetBooks();
		}

		public int Add(ComicBook comicBook)
		{
			return Library.Add(comicBook);
		}

		public int Insert(int index, ComicBook comicBook)
		{
			return Library.Insert(index, comicBook);
		}

		public bool Remove(ComicBook comicBook)
		{
			return Library.Remove(comicBook);
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
