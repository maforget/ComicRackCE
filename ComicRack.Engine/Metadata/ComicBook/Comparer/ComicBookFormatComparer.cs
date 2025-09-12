using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookFormatComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.ShadowFormat, y.ShadowFormat, ignoreCase: true);
		}
	}
}
