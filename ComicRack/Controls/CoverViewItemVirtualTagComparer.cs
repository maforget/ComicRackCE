using System.Collections.Generic;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Metadata.VirtualTags;

namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public class CoverViewItemVirtualTagComparer : CoverViewItemComparer
	{
		private readonly IComparer<ComicBook> _comparer;

		public CoverViewItemVirtualTagComparer(string property)
		{
			_comparer = new ComicBookVirtualTagComparer(property);
		}

		protected override int OnCompare(CoverViewItem x, CoverViewItem y)
		{
			return _comparer.Compare(x.Comic, y.Comic);
		}
	}
}