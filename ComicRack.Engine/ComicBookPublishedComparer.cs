using System;
using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookPublishedComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return DateTime.Compare(x.Published, y.Published);
		}
	}
}
