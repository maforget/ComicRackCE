using System;
using System.Runtime.InteropServices;

namespace cYo.Common.Compression.SevenZip
{
	[ComImport]
	[Guid("23170F69-40C1-278A-0000-000600100000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IArchiveOpenCallback
	{
		void SetTotal(IntPtr files, IntPtr bytes);

		void SetCompleted(IntPtr files, IntPtr bytes);
	}
}
