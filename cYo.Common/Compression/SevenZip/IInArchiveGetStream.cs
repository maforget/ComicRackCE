using System.Runtime.InteropServices;

namespace cYo.Common.Compression.SevenZip
{
	[ComImport]
	[Guid("23170F69-40C1-278A-0000-000600400000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IInArchiveGetStream
	{
		[return: MarshalAs(UnmanagedType.Interface)]
		ISequentialInStream GetStream(int index);
	}
}
