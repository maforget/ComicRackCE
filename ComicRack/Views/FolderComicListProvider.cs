using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.ComponentModel;
using cYo.Common.IO;
using cYo.Common.Localize;
using cYo.Common.Win32;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.IO.Provider;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public class FolderComicListProvider : NamedIdComponent, IComicBookListProvider, ILiteComponent, IDisposable, IIdentity, IComicBookList, IRemoveBooks
	{
		private string path;

		private bool includeSubFolders;

		private volatile List<ComicBook> currentBooks = new List<ComicBook>();

		public IWin32Window Window
		{
			get;
			set;
		}

		public string Path
		{
			get
			{
				return path;
			}
			set
			{
				if (!(path == value))
				{
					path = value;
					base.Name = System.IO.Path.GetFileName(path);
					Refresh();
				}
			}
		}

		public bool IncludeSubFolders
		{
			get
			{
				return includeSubFolders;
			}
			set
			{
				if (includeSubFolders != value)
				{
					includeSubFolders = value;
					Refresh();
				}
			}
		}

		public int BookCount
		{
			get;
			set;
		}

		public event EventHandler BookListChanged;

		public FolderComicListProvider()
		{
		}

		public FolderComicListProvider(string path)
			: this()
		{
			Path = path;
		}

		public IEnumerable<ComicBook> GetBooks()
		{
			return currentBooks;
		}

		public void Refresh()
		{
			currentBooks = GetFolderBookList(Path);
			if (this.BookListChanged != null)
			{
				this.BookListChanged(this, EventArgs.Empty);
			}
		}

		protected List<ComicBook> GetFolderBookList(string folder)
		{
			List<ComicBook> list = new List<ComicBook>();
			try
			{
				IEnumerable<string> fileExtensions = Providers.Readers.GetFileExtensions();
				foreach (string file in FileUtility.GetFiles(folder, includeSubFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
				{
					string f = file;
					if (fileExtensions.Any((string ext) => f.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
					{
						ComicBook comicBook = Program.BookFactory.Create(file, CreateBookOption.AddToTemporary, (list.Count > 100) ? RefreshInfoOptions.DontReadInformation : RefreshInfoOptions.None);
						if (comicBook != null)
						{
							list.Add(comicBook);
						}
					}
					if (AutomaticProgressDialog.ShouldAbort)
					{
						return list;
					}
				}
				return list;
			}
			catch
			{
				return list;
			}
		}

		public void RemoveBooks(IEnumerable<ComicBook> books, bool ask)
		{
			books = books.ToArray();
			if (ask)
			{
				using (Image image = Program.MakeBooksImage(books, new Size(256, 128), 5, onlyMemory: false))
				{
					QuestionResult questionResult = QuestionDialog.AskQuestion(Window, TR.Messages["AskMoveBin", "Are you sure you want to move these files to the Recycle Bin?"], TR.Messages["Remove", "Remove"], (Program.Settings.RemoveFilesfromDatabase ? "!" : string.Empty) + TR.Messages["AlsoRemoveFromLibrary", "&Additionally remove the books from the Library (all information not stored in the files will be lost)"], image);
					if (questionResult.HasFlag(QuestionResult.Cancel))
						return;

					Program.Settings.RemoveFilesfromDatabase = questionResult == QuestionResult.OkWithOption;
				}
			}
			bool flag = false;
			int num = 0;
			using (new WaitCursor())
			{
				foreach (ComicBook book in books)
				{
					try
					{
						ShellFile.DeleteFile(Window, ShellFileDeleteOptions.None, book.FilePath);
						if (currentBooks.Remove(book))
						{
							num++;
						}
					}
					catch
					{
					}
					if (File.Exists(book.FilePath))
					{
						flag = true;
						continue;
					}
					BookCount -= num;
					if (Program.Settings.RemoveFilesfromDatabase)
					{
						Program.Database.Books.RemoveRange(books);
					}
				}
			}
			if (!flag)
			{
				if (this.BookListChanged != null)
				{
					this.BookListChanged(this, EventArgs.Empty);
				}
			}
			else
			{
				MessageBox.Show(TR.Messages["FailedDeleteBooks", "Some books could not be deleted (maybe they are in use)!"], Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
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
