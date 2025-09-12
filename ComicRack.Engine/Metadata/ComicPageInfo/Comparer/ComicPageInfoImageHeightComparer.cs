using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicPageInfoImageHeightComparer : Comparer<ComicPageInfo>
	{
		public override int Compare(ComicPageInfo x, ComicPageInfo y)
		{
			return x.ImageHeight.CompareTo(y.ImageHeight);
		}
	}
}
