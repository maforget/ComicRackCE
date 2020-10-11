using System;

namespace cYo.Projects.ComicRack.Engine.Drawing
{
	[Flags]
	public enum ThumbnailDrawingOptions
	{
		None = 0x0,
		EnableShadow = 0x1,
		EnableBorder = 0x2,
		EnableRating = 0x4,
		EnableVerticalBookmarks = 0x8,
		EnableHorizontalBookmarks = 0x10,
		EnablePageNumber = 0x20,
		EnableBackImage = 0x40,
		EnableBackground = 0x80,
		EnableStates = 0x100,
		KeepAspect = 0x200,
		EnableBowShadow = 0x400,
		DisableMissingThumbnail = 0x1000,
		FastMode = 0x2000,
		NoOpaqueCover = 0x4000,
		Selected = 0x10000,
		Hot = 0x20000,
		Focused = 0x40000,
		Stacked = 0x80000,
		Bookmarked = 0x100000,
		AspectFill = 0x200000,
		DefaultWithoutBackground = 0x54F,
		Default = 0x5CF
	}
}
