using System;
using System.Runtime.InteropServices;

namespace cYo.Common.Compression.SevenZip
{
	[ComImport]
	[Guid("23170F69-40C1-278A-0000-000600800000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IArchiveUpdateCallback
	{
		void SetTotal(long total);

		void SetCompleted([In] ref long completeValue);

		void GetUpdateItemInfo(int index, out int newData, out int newProperties, out int indexInArchive);

		void GetProperty(int index, ItemPropId propID, IntPtr value);

		void GetStream(int index, out ISequentialInStream inStream);

		void SetOperationResult(int operationResult);
	}
}
