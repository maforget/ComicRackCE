using System;

namespace cYo.Common.Win32
{
	[Flags]
	public enum FileSystemEnumeratorType
	{
		Folder = 0x1,
		File = 0x2,
		IncludeSystem = 0x10,
		IncludeHidden = 0x20,
		AttributeMask = 0xF0
	}
}
