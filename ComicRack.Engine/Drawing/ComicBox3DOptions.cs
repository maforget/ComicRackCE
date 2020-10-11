using System;

namespace cYo.Projects.ComicRack.Engine.Drawing
{
	[Flags]
	public enum ComicBox3DOptions
	{
		Trim = 0x1,
		Filter = 0x2,
		SimpleShadow = 0x4,
		Wireless = 0x8,
		SplitDoublePages = 0x10,
		Default = 0x17
	}
}
