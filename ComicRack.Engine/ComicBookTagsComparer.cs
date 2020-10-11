using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookTagsComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.Tags, y.Tags, ignoreCase: true);
		}
	}
}
