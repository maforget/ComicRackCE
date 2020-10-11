using System;

namespace cYo.Common.Win32
{
	[Flags]
	public enum ShellIconType
	{
		None = 0x0,
		Open = 0x1,
		Large = 0x2,
		Link = 0x4,
		Directory = 0x10
	}
}
