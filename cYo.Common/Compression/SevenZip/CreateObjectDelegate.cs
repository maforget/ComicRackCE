using System;
using System.Runtime.InteropServices;

namespace cYo.Common.Compression.SevenZip
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate int CreateObjectDelegate([In] ref Guid classID, [In] ref Guid interfaceID, [MarshalAs(UnmanagedType.Interface)] out object outObject);
}
