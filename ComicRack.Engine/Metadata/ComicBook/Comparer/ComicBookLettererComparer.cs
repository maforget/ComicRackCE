using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookLettererComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.Letterer, y.Letterer, ignoreCase: true);
		}
	}
}
