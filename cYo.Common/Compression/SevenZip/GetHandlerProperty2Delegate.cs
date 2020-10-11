using System.Runtime.InteropServices;

namespace cYo.Common.Compression.SevenZip
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate int GetHandlerProperty2Delegate(int formatIndex, ArchivePropId propID, ref PropVariant value);
}
