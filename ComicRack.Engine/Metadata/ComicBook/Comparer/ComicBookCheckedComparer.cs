using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookCheckedComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.Checked.CompareTo(y.Checked);
		}
	}
}
