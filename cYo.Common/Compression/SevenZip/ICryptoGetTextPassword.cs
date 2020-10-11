using System.Runtime.InteropServices;

namespace cYo.Common.Compression.SevenZip
{
	[ComImport]
	[Guid("23170F69-40C1-278A-0000-000500100000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ICryptoGetTextPassword
	{
		[PreserveSig]
		int CryptoGetTextPassword([MarshalAs(UnmanagedType.BStr)] out string password);
	}
}
