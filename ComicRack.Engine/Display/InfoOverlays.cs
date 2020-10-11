using System;

namespace cYo.Projects.ComicRack.Engine.Display
{
	[Flags]
	public enum InfoOverlays
	{
		None = 0x0,
		PartInfo = 0x1,
		CurrentPage = 0x2,
		LoadPage = 0x4,
		PageBrowser = 0x8,
		PageBrowserOnTop = 0x100,
		CurrentPageShowsName = 0x200
	}
}
