using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicPageInfoImageSizeComparer : Comparer<ComicPageInfo>
	{
		public override int Compare(ComicPageInfo x, ComicPageInfo y)
		{
			return x.ImageFileSize.CompareTo(y.ImageFileSize);
		}
	}
}
