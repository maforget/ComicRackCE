using System;

namespace cYo.Common.Text
{
	[Flags]
	public enum ExtendedStringComparison
	{
		Default = 0x0,
		ZeroesFirst = 0x1,
		IgnoreArticles = 0x2,
		IgnoreCase = 0x4,
		Ordinal = 0x8
	}
}
