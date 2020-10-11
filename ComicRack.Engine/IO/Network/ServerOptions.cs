using System;

namespace cYo.Projects.ComicRack.Engine.IO.Network
{
	[Flags]
	public enum ServerOptions
	{
		None = 0x0,
		ShareNeedsPassword = 0x1,
		ShareIsEditable = 0x2,
		ShareIsExportable = 0x4,
		All = 0xFF
	}
}
