using System.Runtime.InteropServices;

namespace cYo.Common.Compression.SevenZip
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate int GetHandlerPropertyDelegate(ArchivePropId propID, ref PropVariant value);
}
