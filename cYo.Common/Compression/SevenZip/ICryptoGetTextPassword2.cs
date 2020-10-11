using System.Runtime.InteropServices;

namespace cYo.Common.Compression.SevenZip
{
	[ComImport]
	[Guid("23170F69-40C1-278A-0000-000500110000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ICryptoGetTextPassword2
	{
		void CryptoGetTextPassword2([MarshalAs(UnmanagedType.Bool)] out bool passwordIsDefined, [MarshalAs(UnmanagedType.BStr)] out string password);
	}
}
