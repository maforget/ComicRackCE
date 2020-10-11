using System.Collections.Generic;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public class CoverViewItemPositionComparer : Comparer<CoverViewItem>, IComparer<IViewableItem>
	{
		public override int Compare(CoverViewItem x, CoverViewItem y)
		{
			return x.Position.CompareTo(y.Position);
		}

		int IComparer<IViewableItem>.Compare(IViewableItem x, IViewableItem y)
		{
			return Compare((CoverViewItem)x, (CoverViewItem)y);
		}
	}
}
