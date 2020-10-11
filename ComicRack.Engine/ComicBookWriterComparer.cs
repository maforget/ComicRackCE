using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookWriterComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.Writer, y.Writer, ignoreCase: true);
		}
	}
}
