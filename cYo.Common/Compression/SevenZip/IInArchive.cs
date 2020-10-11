using System.Runtime.InteropServices;

namespace cYo.Common.Compression.SevenZip
{
	[ComImport]
	[Guid("23170F69-40C1-278A-0000-000600600000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IInArchive
	{
		[PreserveSig]
		int Open(IInStream stream, [In] ref long maxCheckStartPosition, [MarshalAs(UnmanagedType.Interface)] IArchiveOpenCallback openArchiveCallback);

		[PreserveSig]
		int Close();

		int GetNumberOfItems();

		void GetProperty(int index, ItemPropId propID, ref PropVariant value);

		[PreserveSig]
		int Extract([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] indices, int numItems, int testMode, [MarshalAs(UnmanagedType.Interface)] IArchiveExtractCallback extractCallback);

		void GetArchiveProperty(int propID, ref PropVariant value);

		int GetNumberOfProperties();

		void GetPropertyInfo(int index, [MarshalAs(UnmanagedType.BStr)] out string name, out ItemPropId propID, out short varType);

		int GetNumberOfArchiveProperties();

		void GetArchivePropertyInfo(int index, [MarshalAs(UnmanagedType.BStr)] string name, ref int propID, ref short varType);
	}
}
