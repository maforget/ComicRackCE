using System;

namespace cYo.Projects.ComicRack.Viewer
{
	[Flags]
	public enum OpenComicOptions
	{
		None = 0x0,
		NoRefreshInfo = 0x1,
		NoIncreaseOpenedCount = 0x2,
		NoMoveToLastPage = 0x4,
		NoGlobalColorAdjustment = 0x8,
		NoUpdateCurrentPage = 0x10,
		OpenInNewSlot = 0x20,
		AppendNewSlots = 0x40,
		NoFileUpdate = 0x80,
		DisableAll = 0x1F,
		Default = 0x0
	}
}
