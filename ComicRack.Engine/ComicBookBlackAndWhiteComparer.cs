using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookBlackAndWhiteComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.BlackAndWhite.CompareTo(y.BlackAndWhite);
		}
	}
}
