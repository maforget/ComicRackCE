using System;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	[Flags]
	public enum ImageProviderCapabilities
	{
		Nothing = 0x0,
		FastPageInfo = 0x1,
		FastFormatCheck = 0x2
	}
}
