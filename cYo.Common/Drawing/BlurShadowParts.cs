using System;

namespace cYo.Common.Drawing
{
	[Flags]
	public enum BlurShadowParts
	{
		TopLeft = 0x1,
		TopCenter = 0x2,
		TopRight = 0x4,
		CenterRight = 0x8,
		BottomRight = 0x10,
		BottomCenter = 0x20,
		BottomLeft = 0x40,
		CenterLeft = 0x80,
		Center = 0x100,
		Top = 0x7,
		Right = 0x1C,
		Bottom = 0x70,
		Left = 0xC1,
		Edges = 0xFF,
		All = 0x1FF,
		Default = 0x7C
	}
}
