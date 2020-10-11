using System;
using System.Runtime.InteropServices;

namespace cYo.Common.Compression.SevenZip
{
	[ComImport]
	[Guid("23170F69-40C1-278A-0000-000300040000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOutStream
	{
		[PreserveSig]
		int Write([In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data, int size, IntPtr processedSize);

		void Seek(long offset, int seekOrigin, IntPtr newPosition);

		[PreserveSig]
		int SetSize(long newSize);
	}
}
