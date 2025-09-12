using System.Collections.Generic;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookFileComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return ExtendedStringComparer.Compare(x.FilePath, y.FilePath, ExtendedStringComparison.IgnoreCase);
		}
	}
}
