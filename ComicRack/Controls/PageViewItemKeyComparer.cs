using System.Collections.Generic;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public class PageViewItemKeyComparer : Comparer<PageViewItem>, IComparer<IViewableItem>
	{
		int IComparer<IViewableItem>.Compare(IViewableItem x, IViewableItem y)
		{
			return Compare((PageViewItem)x, (PageViewItem)y);
		}

		public override int Compare(PageViewItem x, PageViewItem y)
		{
			return string.Compare(x.Key, y.Key);
		}
	}
}
