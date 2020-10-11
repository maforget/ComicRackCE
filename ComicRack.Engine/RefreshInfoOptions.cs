using System;

namespace cYo.Projects.ComicRack.Engine
{
	[Flags]
	public enum RefreshInfoOptions
	{
		None = 0x0,
		DontReadInformation = 0x1,
		ForceRefresh = 0x2,
		GetFastPageCount = 0x4,
		GetPageCount = 0x8
	}
}
