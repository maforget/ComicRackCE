using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using cYo.Common.Text;
using cYo.Common.Threading;

namespace cYo.Common.Runtime
{
	public static class Diagnostic
	{
		[Conditional("DEBUG")]
		public static void WaitAndConsume(int ms)
		{
			long num = Machine.Ticks + ms;
			while (DateTime.Now.Ticks < num)
			{
			}
		}

		public static void StartWatchDog(BarkEventHandler bark, int lockTestTimeSeconds = 0)
		{
			ThreadUtility.AddActiveThread(Thread.CurrentThread);
			CrashWatchDog crashWatchDog = new CrashWatchDog
			{
				LockTestTime = TimeSpan.FromSeconds(lockTestTimeSeconds)
			};
			crashWatchDog.Bark += bark;
			crashWatchDog.Register();
		}

		public static void WriteProgramInfo(TextWriter sw)
		{
			sw.WriteLine("Application: {0}", Application.ProductName);
			sw.WriteLine("Version    : {0}", Application.ProductVersion);
			sw.WriteLine("Assembly   : {0}", Assembly.GetEntryAssembly().GetName().Version);
			sw.WriteLine("OS         : {0} {1}", Environment.OSVersion, Environment.Is64BitProcess ? "64" : "32");
			sw.WriteLine(".NET       : {0}", Environment.Version);
			sw.WriteLine("Processors : {0}", Environment.ProcessorCount);
			sw.WriteLine("Workingset : {0}", new FileLengthFormat().Format(null, Environment.WorkingSet, null));
		}
	}
}
