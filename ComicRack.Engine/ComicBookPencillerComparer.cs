using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookPencillerComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.Penciller, y.Penciller, ignoreCase: true);
		}
	}
}
