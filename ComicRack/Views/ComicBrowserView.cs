using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Projects.ComicRack.Engine;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public partial class ComicBrowserView : SubView
	{
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IComicBookListProvider BookList
		{
			get
			{
				return comicBrowser.BookList;
			}
			set
			{
				comicBrowser.SetExtraColumns(); // Ensure extra columns are set when adding a new tab
				comicBrowser.BookList = value;
			}
		}

		public ComicBrowserView()
		{
			InitializeComponent();
		}

		protected override void OnMainFormChanged()
		{
			base.OnMainFormChanged();
			comicBrowser.Main = base.Main;
		}
	}
}
