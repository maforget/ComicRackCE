using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicPageRotationComparer : Comparer<ComicPageInfo>
	{
		public override int Compare(ComicPageInfo x, ComicPageInfo y)
		{
			return x.Rotation.CompareTo(y.Rotation);
		}
	}
}
