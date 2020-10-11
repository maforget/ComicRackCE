using System;

namespace cYo.Projects.ComicRack.Engine
{
	[Flags]
	public enum ComicBookFilterType
	{
		All = 0x0,
		Library = 0x1,
		NotInLibrary = 0x2,
		IsLocal = 0x4,
		IsNotLocal = 0x8,
		IsFileless = 0x10,
		IsNotFileless = 0x20,
		IsEditable = 0x100,
		IsNotEditable = 0x200,
		CanExport = 0x400,
		Selected = 0x1000,
		Sorted = 0x2000,
		AsArray = 0x4000
	}
}
