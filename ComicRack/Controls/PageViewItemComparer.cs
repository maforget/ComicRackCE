using System.Collections.Generic;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;

namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public class PageViewItemComparer<T> : Comparer<PageViewItem>, IComparer<IViewableItem> where T : IComparer<ComicPageInfo>, new()
	{
		private readonly T comparer = new T();

		int IComparer<IViewableItem>.Compare(IViewableItem x, IViewableItem y)
		{
			return Compare((PageViewItem)x, (PageViewItem)y);
		}

		public override int Compare(PageViewItem x, PageViewItem y)
		{
			T val = comparer;
			return val.Compare(x.Book.Comic.GetPage(x.Page), y.Book.Comic.GetPage(y.Page));
		}
	}
}
