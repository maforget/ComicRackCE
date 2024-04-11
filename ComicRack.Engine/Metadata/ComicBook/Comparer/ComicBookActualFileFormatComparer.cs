using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookActualFileFormatComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.ActualFileFormat, y.ActualFileFormat);
		}
	}
}
