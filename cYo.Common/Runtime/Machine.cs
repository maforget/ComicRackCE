using System;
using System.Threading;

namespace cYo.Common.Runtime
{
	public static class Machine
	{
		public static long Ticks => DateTime.Now.Ticks / 10000;

		public static bool Is64Bit => Environment.Is64BitProcess;

		public static void Sleep(int ms)
		{
			Thread.Sleep(ms);
		}
	}
}
