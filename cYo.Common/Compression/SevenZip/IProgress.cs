using System.Runtime.InteropServices;

namespace cYo.Common.Compression.SevenZip
{
	[ComImport]
	[Guid("23170F69-40C1-278A-0000-000000050000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IProgress
	{
		void SetTotal(long total);

		void SetCompleted([In] ref long completeValue);
	}
}
