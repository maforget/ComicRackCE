using System;
using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookModifiedComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return DateTime.Compare(x.FileModifiedTime, y.FileModifiedTime);
		}
	}
}
