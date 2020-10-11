using System;

namespace cYo.Projects.ComicRack.Viewer.Config
{
	[Flags]
	public enum TabLayouts
	{
		None = 0x0,
		Paste = 0x1,
		Export = 0x2,
		Multiple = 0x4,
		ReaderSettings = 0x8,
		BehaviorSettings = 0x10,
		LibrarySettings = 0x20,
		ScriptSettings = 0x40,
		AdvancedSettings = 0x80
	}
}
