using System;
using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookReleasedComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return DateTime.Compare(x.ReleasedTime, y.ReleasedTime);
		}
	}
}
