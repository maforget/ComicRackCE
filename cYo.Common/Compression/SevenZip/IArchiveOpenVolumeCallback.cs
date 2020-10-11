using System;
using System.Runtime.InteropServices;

namespace cYo.Common.Compression.SevenZip
{
	[ComImport]
	[Guid("23170F69-40C1-278A-0000-000600300000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IArchiveOpenVolumeCallback
	{
		void GetProperty(ItemPropId propID, IntPtr value);

		[PreserveSig]
		int GetStream([MarshalAs(UnmanagedType.LPWStr)] string name, [MarshalAs(UnmanagedType.Interface)] out IInStream inStream);
	}
}
