using System.Runtime.InteropServices;

namespace cYo.Common.Compression.SevenZip
{
	[ComImport]
	[Guid("23170F69-40C1-278A-0000-000600200000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IArchiveExtractCallback
	{
		void SetTotal(long total);

		void SetCompleted([In] ref long completeValue);

		[PreserveSig]
		int GetStream(int index, [MarshalAs(UnmanagedType.Interface)] out ISequentialOutStream outStream, AskMode askExtractMode);

		void PrepareOperation(AskMode askExtractMode);

		void SetOperationResult(OperationResult resultEOperationResult);
	}
}
