using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public class ComicListBrowser : SubView, IRefreshDisplay
	{
		protected readonly CursorList<IComicBookListProvider> history = new CursorList<IComicBookListProvider>();

		private IComicBookListProvider bookList;

		private IContainer components;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual IComicBookListProvider BookList
		{
			get
			{
				return bookList;
			}
			protected set
			{
				if (bookList != value)
				{
					IComicBookListProvider comicBookListProvider = bookList;
					bookList = value;
					if (bookList != null)
					{
						bookList.ServiceRequest += bookList_ServiceRequest;
					}
					OnBookListChanged();
					if (comicBookListProvider != null)
					{
						comicBookListProvider.ServiceRequest -= bookList_ServiceRequest;
					}
					history.AddAtCursor(bookList);
				}
			}
		}

		[Browsable(false)]
		public Guid BookListId
		{
			get
			{
				if (BookList != null)
				{
					return BookList.Id;
				}
				return Guid.Empty;
			}
		}

		public virtual bool TopBrowserVisible
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public virtual int TopBrowserSplit
		{
			get
			{
				return 100;
			}
			set
			{
			}
		}

		public event EventHandler BookListChanged;

		public event EventHandler RefreshLists;

		public ComicListBrowser()
		{
			InitializeComponent();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				BookList = null;
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		protected virtual void OnBookListChanged()
		{
			if (this.BookListChanged != null)
			{
				this.BookListChanged(this, EventArgs.Empty);
			}
		}

		private void bookList_ServiceRequest(object sender, ServiceRequestEventArgs e)
		{
			OnListServiceRequest(sender as IComicBookListProvider, e);
		}

		protected virtual void OnListServiceRequest(IComicBookListProvider senderList, ServiceRequestEventArgs e)
		{
		}

		protected virtual IComicBookListProvider GetNewBookList()
		{
			if (base.Main != null)
			{
				base.Main.StoreWorkspace();
			}
			return BookList;
		}

		protected virtual void OnRefreshDisplay()
		{
			if (this.RefreshLists != null)
			{
				this.RefreshLists(this, EventArgs.Empty);
			}
		}

		public void OpenListInNewWindow()
		{
			IListDisplays listDisplays = this.FindParentService<IListDisplays>();
			IComicBookListProvider newBookList = GetNewBookList();
			if (listDisplays != null && newBookList != null)
			{
				listDisplays.AddListWindow(null, newBookList);
			}
		}

		public void OpenListInNewTab(Image image)
		{
			IListDisplays listDisplays = this.FindParentService<IListDisplays>();
			IComicBookListProvider newBookList = GetNewBookList();
			if (listDisplays != null && newBookList != null)
			{
				listDisplays.AddListTab(image, newBookList);
			}
		}

		public void RefreshDisplay()
		{
			OnRefreshDisplay();
		}

		private void InitializeComponent()
		{
			SuspendLayout();
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.Name = "ComicListBrowser";
			base.Size = new System.Drawing.Size(448, 342);
			ResumeLayout(false);
		}
	}
}
