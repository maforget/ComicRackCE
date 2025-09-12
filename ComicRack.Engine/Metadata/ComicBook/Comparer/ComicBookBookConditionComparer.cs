using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookBookConditionComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.BookCondition, y.BookCondition, ignoreCase: true);
		}
	}
}
