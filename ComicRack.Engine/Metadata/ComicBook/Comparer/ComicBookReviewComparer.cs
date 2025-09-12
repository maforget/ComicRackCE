using System.Collections.Generic;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookReviewComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return ExtendedStringComparer.Compare(x.Review, y.Review, ExtendedStringComparison.IgnoreCase);
		}
	}
}
