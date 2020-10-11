using System;

namespace cYo.Common.Win32
{
	[Flags]
	public enum ShellFileDeleteOptions
	{
		None = 0x0,
		NoRecycleBin = 0x1,
		Confirmation = 0x2
	}
}
