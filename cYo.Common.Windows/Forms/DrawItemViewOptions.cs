using System;

namespace cYo.Common.Windows.Forms
{
	[Flags]
	public enum DrawItemViewOptions
	{
		Background = 0x1,
		BackgroundImage = 0x2,
		ColumnHeaders = 0x4,
		GroupHeaders = 0x8,
		SelectedOnly = 0x10,
		FocusRectangle = 0x20,
		Default = 0x2F
	}
}
