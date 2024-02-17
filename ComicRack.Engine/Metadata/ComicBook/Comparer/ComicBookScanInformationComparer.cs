using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookScanInformationComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.ScanInformation, y.ScanInformation, ignoreCase: true);
		}
	}
}
