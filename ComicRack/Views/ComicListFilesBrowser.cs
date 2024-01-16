using System.ComponentModel;
using System.Windows.Forms;
using cYo.Common.Localize;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public partial class ComicListFilesBrowser : ComicListBrowser
	{
		private readonly FolderComicListProvider folderBooks = new FolderComicListProvider();

		public bool IncludeSubFolders
		{
			get
			{
				return folderBooks.IncludeSubFolders;
			}
			set
			{
				folderBooks.IncludeSubFolders = value;
			}
		}

		public ComicListFilesBrowser()
		{
			InitializeComponent();
			folderBooks.Window = this;
			BookList = folderBooks;
		}

		protected override IComicBookListProvider GetNewBookList()
		{
			return new FolderComicListProvider(folderBooks.Path);
		}

		public void FillBooks(string currentFolder)
		{
			AutomaticProgressDialog.Process(this, TR.Messages["GettingList", "Getting Books List"], TR.Messages["GettingListText", "Retrieving all Books from the selected folder"], 1000, delegate
			{
				folderBooks.Path = currentFolder;
			}, AutomaticProgressDialogOptions.EnableCancel);
		}

		public void SwitchIncludeSubFolders()
		{
			AutomaticProgressDialog.Process(this, TR.Messages["GettingList", "Getting Books List"], TR.Messages["GettingListText", "Retrieving all Books from the selected folder"], 1000, delegate
			{
				IncludeSubFolders = !IncludeSubFolders;
			}, AutomaticProgressDialogOptions.EnableCancel);
		}
	}
}
