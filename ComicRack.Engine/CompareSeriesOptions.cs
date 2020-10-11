using System;

namespace cYo.Projects.ComicRack.Engine
{
	[Flags]
	public enum CompareSeriesOptions
	{
		None = 0x1,
		IgnoreVolumeInName = 0x2,
		StripDown = 0x4
	}
}
