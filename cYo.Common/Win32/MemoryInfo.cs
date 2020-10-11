using System;
using System.Management;
using System.Runtime.InteropServices;

namespace cYo.Common.Win32
{
	public static class MemoryInfo
	{
		private static class NativeMethods
		{
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
			public class MEMORYSTATUSEX
			{
				public uint dwLength;

				public uint dwMemoryLoad;

				public ulong ullTotalPhys;

				public ulong ullAvailPhys;

				public ulong ullTotalPageFile;

				public ulong ullAvailPageFile;

				public ulong ullTotalVirtual;

				public ulong ullAvailVirtual;

				public ulong ullAvailExtendedVirtual;

				public MEMORYSTATUSEX()
				{
					dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
				}
			}

			[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool GlobalMemoryStatusEx([In][Out] MEMORYSTATUSEX lpBuffer);

			public static MEMORYSTATUSEX GetStatus()
			{
				MEMORYSTATUSEX mEMORYSTATUSEX = new MEMORYSTATUSEX();
				GlobalMemoryStatusEx(mEMORYSTATUSEX);
				return mEMORYSTATUSEX;
			}
		}

		private static int cpuSpeedInHz;

		public static long AvailablePhysicalMemory => (long)NativeMethods.GetStatus().ullAvailPhys;

		public static long InstalledPhysicalMemory => (long)NativeMethods.GetStatus().ullTotalPhys;

		public static int CpuSpeedInHz
		{
			get
			{
				if (cpuSpeedInHz == 0)
				{
					try
					{
						using (ManagementObject managementObject = new ManagementObject("Win32_Processor.DeviceID='CPU0'"))
						{
							object obj = managementObject["MaxClockSpeed"];
							cpuSpeedInHz = (int)(uint)obj;
						}
					}
					catch (Exception)
					{
						cpuSpeedInHz = -1;
					}
				}
				return cpuSpeedInHz;
			}
		}
	}
}
