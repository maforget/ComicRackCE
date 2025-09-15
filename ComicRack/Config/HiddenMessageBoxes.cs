using System;

namespace cYo.Projects.ComicRack.Viewer.Config
{
	[Flags]
	public enum HiddenMessageBoxes
	{
		None = 0x0,
		RemoveFromList = 0x1,
		RemoveList = 0x2,
		RemoveFavorite = 0x4,
		ConvertComics = 0x8,
		SetAllListLayouts = 0x10,
		CloseExternalReader = 0x20,
		ComicRackMinimized = 0x40,
		AskDirtyItems = 0x80,
		AskClearData = 0x100,
		DoNotCheckForUpdate = 0x200
	}
}
