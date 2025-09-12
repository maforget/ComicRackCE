using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookAlternateNumberComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.CompareAlternateNumber.CompareTo(y.CompareAlternateNumber);
		}
	}
}
