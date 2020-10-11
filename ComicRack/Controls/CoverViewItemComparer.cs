using System.Collections.Generic;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public abstract class CoverViewItemComparer : Comparer<CoverViewItem>, IComparer<IViewableItem>
	{
		protected abstract int OnCompare(CoverViewItem x, CoverViewItem y);

		public override int Compare(CoverViewItem x, CoverViewItem y)
		{
			return OnCompareInternal(x, y);
		}

		public int Compare(IViewableItem x, IViewableItem y)
		{
			return OnCompareInternal(x as CoverViewItem, y as CoverViewItem);
		}

		private int OnCompareInternal(CoverViewItem x, CoverViewItem y)
		{
			if (x == null && y == null)
			{
				return 0;
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			return OnCompare(x, y);
		}
	}
}
