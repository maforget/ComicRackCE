using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookFileFormatComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.FileFormat, y.FileFormat);
		}
	}
}
