using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicPageBookmarkComparer : Comparer<ComicPageInfo>
	{
		public override int Compare(ComicPageInfo x, ComicPageInfo y)
		{
			return string.Compare(x.Bookmark, y.Bookmark, ignoreCase: true);
		}
	}
}
