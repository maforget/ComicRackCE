using System;

namespace cYo.Common.Runtime
{
	[Flags]
	public enum IniFileRegisterOptions
	{
		None = 0x0,
		WatchIniFile = 0x1,
		ReadCommandLine = 0x2,
		Default = 0x2
	}
}
