using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using cYo.Common.ComponentModel;
using Microsoft.Win32.SafeHandles;

namespace cYo.Common.Compression.SevenZip
{
	public class SevenZipFactory : DisposableObject
	{
		private static class NativeMethods
		{
			public sealed class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
			{
				public SafeLibraryHandle()
					: base(ownsHandle: true)
				{
				}

				[DllImport(Kernel32Dll)]
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.Bool)]
				private static extern bool FreeLibrary(IntPtr hModule);

				[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
				protected override bool ReleaseHandle()
				{
					return FreeLibrary(handle);
				}
			}

			private const string Kernel32Dll = "kernel32.dll";

			[DllImport(Kernel32Dll, CharSet = CharSet.Auto, SetLastError = true)]
			public static extern SafeLibraryHandle LoadLibrary([MarshalAs(UnmanagedType.LPTStr)] string lpFileName);

			[DllImport(Kernel32Dll, CharSet = CharSet.Ansi, SetLastError = true)]
			public static extern IntPtr GetProcAddress(SafeLibraryHandle hModule, [MarshalAs(UnmanagedType.LPStr)] string procName);
		}

		private static readonly Dictionary<KnownSevenZipFormat, Guid> knownFormats = new Dictionary<KnownSevenZipFormat, Guid>
		{
			{
				KnownSevenZipFormat.SevenZip,
				new Guid("23170f69-40c1-278a-1000-000110070000")
			},
			{
				KnownSevenZipFormat.Arj,
				new Guid("23170f69-40c1-278a-1000-000110040000")
			},
			{
				KnownSevenZipFormat.BZip2,
				new Guid("23170f69-40c1-278a-1000-000110020000")
			},
			{
				KnownSevenZipFormat.Cab,
				new Guid("23170f69-40c1-278a-1000-000110080000")
			},
			{
				KnownSevenZipFormat.Chm,
				new Guid("23170f69-40c1-278a-1000-000110e90000")
			},
			{
				KnownSevenZipFormat.Compound,
				new Guid("23170f69-40c1-278a-1000-000110e50000")
			},
			{
				KnownSevenZipFormat.Cpio,
				new Guid("23170f69-40c1-278a-1000-000110ed0000")
			},
			{
				KnownSevenZipFormat.Deb,
				new Guid("23170f69-40c1-278a-1000-000110ec0000")
			},
			{
				KnownSevenZipFormat.GZip,
				new Guid("23170f69-40c1-278a-1000-000110ef0000")
			},
			{
				KnownSevenZipFormat.Iso,
				new Guid("23170f69-40c1-278a-1000-000110e70000")
			},
			{
				KnownSevenZipFormat.Lzh,
				new Guid("23170f69-40c1-278a-1000-000110060000")
			},
			{
				KnownSevenZipFormat.Lzma,
				new Guid("23170f69-40c1-278a-1000-0001100a0000")
			},
			{
				KnownSevenZipFormat.Nsis,
				new Guid("23170f69-40c1-278a-1000-000110090000")
			},
			{
				KnownSevenZipFormat.Rar,
				new Guid("23170f69-40c1-278a-1000-000110030000")
			},
			{
				KnownSevenZipFormat.Rpm,
				new Guid("23170f69-40c1-278a-1000-000110eb0000")
			},
			{
				KnownSevenZipFormat.Split,
				new Guid("23170f69-40c1-278a-1000-000110ea0000")
			},
			{
				KnownSevenZipFormat.Tar,
				new Guid("23170f69-40c1-278a-1000-000110ee0000")
			},
			{
				KnownSevenZipFormat.Wim,
				new Guid("23170f69-40c1-278a-1000-000110e60000")
			},
			{
				KnownSevenZipFormat.Z,
				new Guid("23170f69-40c1-278a-1000-000110050000")
			},
			{
				KnownSevenZipFormat.Zip,
				new Guid("23170f69-40c1-278a-1000-000110010000")
			},
			{
				KnownSevenZipFormat.Rar5,
				new Guid("23170f69-40c1-278a-1000-000110CC0000")
			}
		};

		private readonly NativeMethods.SafeLibraryHandle libHandle;

		private readonly CreateObjectDelegate createObject;

		public SevenZipFactory(string sevenZipLibPath)
		{
			libHandle = NativeMethods.LoadLibrary(sevenZipLibPath);
			if (libHandle.IsInvalid)
			{
				throw new Win32Exception();
			}
			createObject = (CreateObjectDelegate)Marshal.GetDelegateForFunctionPointer(NativeMethods.GetProcAddress(libHandle, "CreateObject"), typeof(CreateObjectDelegate));
			if (object.Equals(createObject, null))
			{
				libHandle.Close();
				throw new ArgumentException();
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (libHandle != null && !libHandle.IsClosed)
			{
				libHandle.Close();
			}
		}

		private T CreateInterface<T>(Guid classId) where T : class
		{
			Guid interfaceID = typeof(T).GUID;
			createObject(ref classId, ref interfaceID, out var outObject);
			return outObject as T;
		}

		public IInArchive CreateInArchive(KnownSevenZipFormat format)
		{
			return CreateInterface<IInArchive>(knownFormats[format]);
		}

		public IOutArchive CreateOutArchive(KnownSevenZipFormat format)
		{
			return CreateInterface<IOutArchive>(knownFormats[format]);
		}
	}
}
