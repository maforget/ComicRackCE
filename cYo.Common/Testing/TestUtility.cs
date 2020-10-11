using System;
using System.Diagnostics;
using cYo.Common.ComponentModel;

namespace cYo.Common.Testing
{
	public static class TestUtility
	{
		public static IDisposable Time(string message = "Time needed: {0}")
		{
			Stopwatch sw = Stopwatch.StartNew();
			return new LeanDisposer(delegate
			{
				sw.Stop();
				Console.WriteLine(message, sw.Elapsed);
			});
		}
	}
}
