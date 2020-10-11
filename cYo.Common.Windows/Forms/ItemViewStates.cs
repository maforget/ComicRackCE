using System;

namespace cYo.Common.Windows.Forms
{
	[Flags]
	public enum ItemViewStates
	{
		None = 0x0,
		Selected = 0x1,
		Focused = 0x2,
		Hot = 0x4,
		All = 0x7
	}
}
