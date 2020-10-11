using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicPageInfoImageWidthComparer : Comparer<ComicPageInfo>
	{
		public override int Compare(ComicPageInfo x, ComicPageInfo y)
		{
			return x.ImageWidth.CompareTo(y.ImageWidth);
		}
	}
}
