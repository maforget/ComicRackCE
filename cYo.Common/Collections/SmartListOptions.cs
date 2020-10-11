using System;

namespace cYo.Common.Collections
{
	[Flags]
	public enum SmartListOptions
	{
		None = 0x0,
		Synchronized = 0x1,
		DisableOnSet = 0x2,
		DisableOnInsert = 0x4,
		DisableOnRemove = 0x8,
		DisableOnClear = 0x10,
		DisableOnRefresh = 0x20,
		ClearWithRemove = 0x40,
		DisposeOnRemove = 0x80,
		DisableCollectionChangedEvent = 0x100,
		CheckedSet = 0x200,
		Default = 0x243
	}
}
