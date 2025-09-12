using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicPagePositionComparer : Comparer<ComicPageInfo>
	{
		public override int Compare(ComicPageInfo x, ComicPageInfo y)
		{
			return x.PagePosition.CompareTo(y.PagePosition);
		}
	}
}
