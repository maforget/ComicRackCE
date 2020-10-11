using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookVolumeComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.ShadowVolume.CompareTo(y.ShadowVolume);
		}
	}
}
