using System;

namespace cYo.Projects.ComicRack.Engine.Display
{
	[Flags]
	public enum ImageDisplayOptions
	{
		None = 0x0,
		HighQuality = 0x1,
		AnamorphicScaling = 0x2,
		Default = 0x1
	}
}
