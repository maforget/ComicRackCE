using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookCountComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.ShadowCount.CompareTo(y.ShadowCount);
		}
	}
}
