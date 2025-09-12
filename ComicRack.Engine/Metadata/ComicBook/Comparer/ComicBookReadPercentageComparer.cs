using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookReadPercentageComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.ReadPercentage.CompareTo(y.ReadPercentage);
		}
	}
}
