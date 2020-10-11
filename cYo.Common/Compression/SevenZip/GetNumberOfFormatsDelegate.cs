using System.Runtime.InteropServices;

namespace cYo.Common.Compression.SevenZip
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate int GetNumberOfFormatsDelegate(out int numFormats);
}
