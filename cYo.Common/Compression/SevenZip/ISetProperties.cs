using System;
using System.Runtime.InteropServices;

namespace cYo.Common.Compression.SevenZip
{
	[ComImport]
	[Guid("23170F69-40C1-278A-0000-000600030000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ISetProperties
	{
		void SetProperties([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 2)] string[] names, IntPtr values, int numProperties);
	}
}
