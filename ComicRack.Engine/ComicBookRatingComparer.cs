using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookRatingComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.Rating.CompareTo(y.Rating);
		}
	}
}
