using System.Runtime.InteropServices;

namespace cYo.Common.Compression.SevenZip
{
	[ComImport]
	[Guid("23170F69-40C1-278A-0000-000300060000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IStreamGetSize
	{
		long GetSize();
	}
}
