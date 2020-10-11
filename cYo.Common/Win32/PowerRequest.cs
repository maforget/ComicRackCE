using System;
using System.ComponentModel;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace cYo.Common.Win32
{
	public class PowerRequest : IDisposable
	{
		[SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		public class PowerRequestHandle : SafeHandleMinusOneIsInvalid
		{
			private PowerRequestHandle()
				: base(ownsHandle: true)
			{
			}

			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
			protected override bool ReleaseHandle()
			{
				return NativeMethods.CloseHandle(handle);
			}
		}

		internal static class NativeMethods
		{
			internal enum PowerRequestType
			{
				PowerRequestDisplayRequired,
				PowerRequestSystemRequired,
				PowerRequestAwayModeRequired,
				PowerRequestExecutionRequired,
				PowerRequestMaximum
			}

			[Flags]
			internal enum EXECUTION_STATE : uint
			{
				ES_AWAYMODE_REQUIRED = 0x40u,
				ES_CONTINUOUS = 0x80000000u,
				ES_DISPLAY_REQUIRED = 0x2u,
				ES_SYSTEM_REQUIRED = 0x1u
			}

			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			internal struct POWER_REQUEST_CONTEXT
			{
				public uint Version;

				public uint Flags;

				[MarshalAs(UnmanagedType.LPWStr)]
				public string SimpleReasonString;
			}

			internal const int POWER_REQUEST_CONTEXT_VERSION = 0;

			internal const int POWER_REQUEST_CONTEXT_SIMPLE_STRING = 1;

			[DllImport("kernel32", SetLastError = true)]
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
			[return: MarshalAs(UnmanagedType.Bool)]
			internal static extern bool CloseHandle(IntPtr handle);

			[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
			internal static extern UIntPtr LoadLibrary(string lpFileName);

			[DllImport("kernel32", CharSet = CharSet.Unicode)]
			internal static extern uint RegisterApplicationRestart([MarshalAs(UnmanagedType.LPWStr)] string commandLine, int flags);

			[DllImport("kernel32", BestFitMapping = false, CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true, ThrowOnUnmappableChar = true)]
			internal static extern UIntPtr GetProcAddress(UIntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string procName);

			[DllImport("kernel32.dll", SetLastError = true)]
			internal static extern PowerRequestHandle PowerCreateRequest(ref POWER_REQUEST_CONTEXT Context);

			[DllImport("kernel32.dll", SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			internal static extern bool PowerSetRequest(PowerRequestHandle PowerRequestHandle, PowerRequestType RequestType);

			[DllImport("kernel32.dll", SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			internal static extern bool PowerClearRequest(PowerRequestHandle PowerRequestHandle, PowerRequestType RequestType);

			[DllImport("kernel32.dll", SetLastError = true)]
			internal static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

			internal static bool FunctionExists(string library, string function)
			{
				UIntPtr uIntPtr = LoadLibrary(library);
				if (uIntPtr == UIntPtr.Zero)
				{
					return false;
				}
				return GetProcAddress(uIntPtr, function) != UIntPtr.Zero;
			}
		}

		private PowerRequestHandle handle;

		public static bool IsAvailable => NativeMethods.FunctionExists("kernel32.dll", "PowerCreateRequest");

		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		public PowerRequest(string reason)
		{
			NativeMethods.POWER_REQUEST_CONTEXT Context = new NativeMethods.POWER_REQUEST_CONTEXT
			{
				Version = 0u,
				Flags = 1u,
				SimpleReasonString = reason
			};
			handle = NativeMethods.PowerCreateRequest(ref Context);
			if (handle.IsInvalid)
			{
				throw new Win32Exception();
			}
		}

		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		public void SetRequest(PowerRequestType type)
		{
			if (!NativeMethods.PowerSetRequest(handle, ToNativeType(type)))
			{
				throw new Win32Exception();
			}
		}

		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		public void ClearRequest(PowerRequestType type)
		{
			if (!NativeMethods.PowerClearRequest(handle, ToNativeType(type)))
			{
				throw new Win32Exception();
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		protected virtual void Dispose(bool disposing)
		{
			if (handle != null && !handle.IsInvalid)
			{
				handle.Dispose();
				handle = null;
			}
		}

		private static NativeMethods.PowerRequestType ToNativeType(PowerRequestType type)
		{
			switch (type)
			{
			case PowerRequestType.DisplayRequired:
				return NativeMethods.PowerRequestType.PowerRequestDisplayRequired;
			case PowerRequestType.SystemRequired:
				return NativeMethods.PowerRequestType.PowerRequestSystemRequired;
			case PowerRequestType.AwayModeRequired:
				return NativeMethods.PowerRequestType.PowerRequestAwayModeRequired;
			case PowerRequestType.ExecutionRequired:
				return NativeMethods.PowerRequestType.PowerRequestExecutionRequired;
			default:
				throw new ArgumentException("Invalid power request type");
			}
		}
	}
}
