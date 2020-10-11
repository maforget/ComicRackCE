using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Flags]
	public enum ComicPageType : short
	{
		FrontCover = 0x1,
		InnerCover = 0x2,
		Roundup = 0x4,
		Story = 0x8,
		Advertisement = 0x10,
		Editorial = 0x20,
		Letters = 0x40,
		Preview = 0x80,
		BackCover = 0x100,
		Other = 0x200,
		Deleted = 0x400,
		[Browsable(false)]
		All = 0x3FF,
		[Browsable(false)]
		AllWithDeleted = 0x7FF
	}
}
