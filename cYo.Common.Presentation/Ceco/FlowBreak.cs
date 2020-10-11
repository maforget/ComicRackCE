using System;

namespace cYo.Common.Presentation.Ceco
{
	[Flags]
	public enum FlowBreak
	{
		None = 0x0,
		BreakLine = 0x1,
		BreakMarginLeft = 0x2,
		BreakMarginRight = 0x3,
		BreakMarginLeftRight = 0x4,
		Before = 0x100,
		After = 0x200,
		BreakMask = 0xF
	}
}
