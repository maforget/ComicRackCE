using System.Runtime.InteropServices;

namespace cYo.Common.Compression.SevenZip
{
	[ComImport]
	[Guid("23170F69-40C1-278A-0000-000600A00000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IOutArchive
	{
		void UpdateItems(ISequentialOutStream outStream, int numItems, IArchiveUpdateCallback updateCallback);

		FileTimeType GetFileTimeType();
	}
}
