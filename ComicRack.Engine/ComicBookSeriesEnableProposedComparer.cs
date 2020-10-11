using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookSeriesEnableProposedComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.EnableProposed.CompareTo(y.EnableProposed);
		}
	}
}
