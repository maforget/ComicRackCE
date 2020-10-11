using System;

namespace cYo.Projects.ComicRack.Engine
{
	[Flags]
	public enum ComicsEditModes
	{
		None = 0x0,
		Local = 0x1,
		EditProperties = 0x2,
		EditPages = 0x4,
		DeleteComics = 0x8,
		ExportComic = 0x10,
		Rescan = 0x20,
		EditComicList = 0x40,
		Default = 0x7F,
		Remote = 0x0,
		RemoteEditable = 0x2
	}
}
