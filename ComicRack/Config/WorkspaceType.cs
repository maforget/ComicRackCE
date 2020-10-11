using System;

namespace cYo.Projects.ComicRack.Viewer.Config
{
	[Flags]
	public enum WorkspaceType
	{
		WindowLayout = 0x1,
		ViewsSetup = 0x2,
		ComicPageLayout = 0x4,
		ComicPageDisplay = 0x8,
		Default = 0xF
	}
}
