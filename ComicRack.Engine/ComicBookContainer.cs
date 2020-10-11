using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	public class ComicBookContainer : NamedIdComponent, IEditableComicBookListProvider, IComicBookListProvider, ILiteComponent, IDisposable, IIdentity, IComicBookList
	{
		private ComicBookCollection books = new ComicBookCollection();

		[NonSerialized]
		private volatile ComicsEditModes editMode = ComicsEditModes.Default;

		[XmlArrayItem("Book")]
		public ComicBookCollection Books => books;

		[XmlIgnore]
		public ComicsEditModes EditMode
		{
			get
			{
				return editMode;
			}
			set
			{
				editMode = value;
			}
		}

		public virtual bool IsLibrary => false;

		[XmlIgnore]
		public int BookCount
		{
			get
			{
				return books.Count;
			}
			set
			{
			}
		}

		[field: NonSerialized]
		public event EventHandler<ContainerBookChangedEventArgs> BookChanged;

		[field: NonSerialized]
		public event EventHandler BookListChanged;

		public ComicBookContainer()
		{
			books.Changed += books_Changed;
		}

		public ComicBookContainer(string name)
			: this()
		{
			base.Name = name;
		}

		private void books_Changed(object sender, SmartListChangedEventArgs<ComicBook> e)
		{
			switch (e.Action)
			{
			case SmartListAction.Insert:
				OnBookAdded(e.Item);
				e.Item.BookChanged += bookChanged;
				break;
			case SmartListAction.Remove:
				OnBookRemoved(e.Item);
				e.Item.BookChanged -= bookChanged;
				break;
			}
			Refresh();
		}

		private void bookChanged(object sender, BookChangedEventArgs e)
		{
			OnBookChanged(new ContainerBookChangedEventArgs((ComicBook)sender, e));
		}

		public void Refresh()
		{
			OnBookListChanged();
		}

		public void Consolidate()
		{
			foreach (ComicBook book in Books)
			{
				book.Pages.Consolidate();
				book.TrimExcessPageInfo();
			}
		}

		protected void AttachBooks(ComicBookCollection list)
		{
			books = list;
		}

		protected virtual void OnBookChanged(ContainerBookChangedEventArgs e)
		{
			if (this.BookChanged != null)
			{
				this.BookChanged(this, e);
			}
		}

		protected virtual void OnBookListChanged()
		{
			if (this.BookListChanged != null)
			{
				this.BookListChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnBookAdded(ComicBook book)
		{
		}

		protected virtual void OnBookRemoved(ComicBook book)
		{
		}

		public IEnumerable<ComicBook> GetBooks()
		{
			return Books;
		}

		public IEnumerable<string> GetBookFiles()
		{
			return from cb in Books
				where cb.IsLinked
				select cb.FilePath;
		}

		public int Add(ComicBook comicBook)
		{
			return Insert(Books.Count, comicBook);
		}

		public int Insert(int index, ComicBook comicBook)
		{
			int num = Books.IndexOf(Books[comicBook.FilePath]);
			if (num != -1)
			{
				return num;
			}
			num = Books.IndexOf(Books[comicBook.Id]);
			if (num != -1)
			{
				return num;
			}
			Books.Insert(index, comicBook);
			return index;
		}

		public bool Remove(ComicBook comicBook)
		{
			return Books.Remove(comicBook);
		}

        //Decompile Error
        //void IComicBookListProvider.add_NameChanged(EventHandler value)
        //{
        //    base.NameChanged += value;
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
