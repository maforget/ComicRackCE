using System;

namespace cYo.Common.Runtime
{
	[Flags]
	public enum CommandLineParserOptions
	{
		None = 0x0,
		UseIni = 0x1,
		FailOnError = 0x2,
		Default = 0x1
	}
}
