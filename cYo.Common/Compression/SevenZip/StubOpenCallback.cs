using System;

namespace cYo.Common.Compression.SevenZip
{
	public class StubOpenCallback : IArchiveOpenCallback
	{
		public void SetTotal(IntPtr files, IntPtr bytes)
		{
		}

		public void SetCompleted(IntPtr files, IntPtr bytes)
		{
		}
	}
}
