using System.Runtime.InteropServices;

namespace cYo.Common.Compression.SevenZip
{
	[ComImport]
	[Guid("23170F69-40C1-278A-0000-000300010000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ISequentialInStream
	{
		int Read([Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] data, int size);
	}
}
