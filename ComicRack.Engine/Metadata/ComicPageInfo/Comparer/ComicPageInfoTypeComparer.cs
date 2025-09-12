using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicPageInfoTypeComparer : Comparer<ComicPageInfo>
	{
		public override int Compare(ComicPageInfo x, ComicPageInfo y)
		{
			return string.Compare(x.PageTypeAsText, y.PageTypeAsText, ignoreCase: true);
		}
	}
}
