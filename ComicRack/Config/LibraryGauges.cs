using System;

namespace cYo.Projects.ComicRack.Viewer.Config
{
	[Flags]
	public enum LibraryGauges
	{
		None = 0x0,
		New = 0x1,
		Unread = 0x2,
		Total = 0x4,
		Numeric = 0x1000,
		Default = 0x1007
	}
}
