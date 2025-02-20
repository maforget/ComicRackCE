using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace cYo.Common.Win32
{
	public static class ShellRegister
	{
		private static class Native
		{
			[Flags]
			public enum HChangeNotifyEventID
			{
				SHCNE_ALLEVENTS = int.MaxValue,
				SHCNE_ASSOCCHANGED = 0x8000000,
				SHCNE_ATTRIBUTES = 0x800,
				SHCNE_CREATE = 0x2,
				SHCNE_DELETE = 0x4,
				SHCNE_DRIVEADD = 0x100,
				SHCNE_DRIVEADDGUI = 0x10000,
				SHCNE_DRIVEREMOVED = 0x80,
				SHCNE_EXTENDED_EVENT = 0x4000000,
				SHCNE_FREESPACE = 0x40000,
				SHCNE_MEDIAINSERTED = 0x20,
				SHCNE_MEDIAREMOVED = 0x40,
				SHCNE_MKDIR = 0x8,
				SHCNE_NETSHARE = 0x200,
				SHCNE_NETUNSHARE = 0x400,
				SHCNE_RENAMEFOLDER = 0x20000,
				SHCNE_RENAMEITEM = 0x1,
				SHCNE_RMDIR = 0x10,
				SHCNE_SERVERDISCONNECT = 0x4000,
				SHCNE_UPDATEDIR = 0x1000,
				SHCNE_UPDATEIMAGE = 0x8000
			}

			[Flags]
			public enum HChangeNotifyFlags
			{
				SHCNF_DWORD = 0x3,
				SHCNF_IDLIST = 0x0,
				SHCNF_PATHA = 0x1,
				SHCNF_PATHW = 0x5,
				SHCNF_PRINTERA = 0x2,
				SHCNF_PRINTERW = 0x6,
				SHCNF_FLUSH = 0x1000,
				SHCNF_FLUSHNOWAIT = 0x2000
			}

			[DllImport("shell32.dll")]
			public static extern void SHChangeNotify(HChangeNotifyEventID wEventId, HChangeNotifyFlags uFlags, IntPtr dwItem1, IntPtr dwItem2);
		}

		private static int result = -1;

		public static RegistryKey ClassesRoot => Registry.ClassesRoot;

		public static bool CanRegisterShell
		{
			get
			{
				if (result != -1)
				{
					return result != 0;
				}
				string subkey = Guid.NewGuid().ToString();
				try
				{
					using (ClassesRoot.CreateSubKey(subkey))
					{
					}
					result = 1;
					return true;
				}
				catch
				{
					result = 0;
					return false;
				}
				finally
				{
					try
					{
						ClassesRoot.DeleteSubKey(subkey);
					}
					catch
					{
					}
				}
			}
		}

		public static void RefreshShell()
		{
			Native.SHChangeNotify(Native.HChangeNotifyEventID.SHCNE_ASSOCCHANGED, Native.HChangeNotifyFlags.SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero);
		}

		public static void RegisterFileOpen(string typeId, string docExtension, string docName, string appPath, string iconPath)
		{
			using (RegistryKey registryKey = ClassesRoot.CreateSubKey(docExtension))
			{
				registryKey.SetValue(null, typeId);
			}
			using (RegistryKey registryKey2 = Registry.ClassesRoot.CreateSubKey(typeId))
			{
				registryKey2.SetValue(null, docName);
				using (RegistryKey registryKey3 = registryKey2.CreateSubKey("DefaultIcon"))
				{
					registryKey3.SetValue(null, iconPath);
				}
				using (RegistryKey registryKey4 = registryKey2.CreateSubKey("shell\\open\\command"))
				{
					registryKey4.SetValue(null, "\"" + appPath + "\" \"%1\"");
				}
			}
		}

		public static void RegisterFileOpen(string typeId, string docExtension, string docName, int icon)
		{
			string location = Assembly.GetEntryAssembly().Location;
			string iconPath = "\"" + location + "\"," + icon;
			RegisterFileOpen(typeId, docExtension, docName, location, iconPath);
		}

		public static void UnregisterFileOpen(string typeId, string docExtension)
		{
			if (IsFileOpenRegistered(typeId, docExtension))
			{
				using (RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(docExtension, true))
				{
					registryKey?.SetValue(null, string.Empty);
				}
			}
		}

		public static bool IsFileOpenRegistered(string typeId, string docExtension)
		{
			using (RegistryKey registryKey = ClassesRoot.OpenSubKey(docExtension))
			{
				return registryKey != null && typeId == (string)registryKey.GetValue(null);
			}
		}

		public static bool IsFileOpenInUse(string typeId, string docExtension)
		{
			if (IsFileOpenRegistered(typeId, docExtension))
			{
				return false;
			}
			using (RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(docExtension))
			{
				return registryKey != null;
			}
		}

		public static void RegisterFileOpenWith(string docExtension, string appPath, string friendlyName, string typeId)
		{
			string fileName = Path.GetFileName(appPath);
			using (RegistryKey registryKey = ClassesRoot.CreateSubKey(docExtension + "\\OpenWithProgIds"))
			{
				registryKey?.SetValue(typeId, string.Empty);
			}
			using (RegistryKey registryKey2 = ClassesRoot.CreateSubKey("Applications\\" + fileName + "\\shell\\Open"))
			{
				if (!string.IsNullOrEmpty(friendlyName))
				{
					registryKey2.SetValue("FriendlyAppName", friendlyName);
				}
				using (RegistryKey registryKey3 = ClassesRoot.CreateSubKey("command"))
				{
					registryKey3.SetValue(null, "\"" + appPath + "\" \"%1\"");
				}
			}
		}

		public static void RegisterFileOpenWith(string docExtension, string typeId)
		{
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			RegisterFileOpenWith(docExtension, entryAssembly.Location, entryAssembly.GetCustomAttributes(inherit: false).OfType<AssemblyTitleAttribute>().FirstOrDefault()
				.Title, typeId);
		}

		public static void UnregisterFileOpenWith(string docExtension, string appPath, string typeId)
		{
			string fileName = Path.GetFileName(appPath);
			Registry.ClassesRoot.DeleteSubKeyTree(docExtension + "\\OpenWithList\\" + fileName, false); //Delete any old entries
			using (RegistryKey registryKey = ClassesRoot.OpenSubKey(docExtension + "\\OpenWithProgIds", true))
			{
				registryKey?.DeleteValue(typeId, false);
			}
		}

		public static void UnregisterFileOpenWith(string docExtension, string typeId)
		{
			UnregisterFileOpenWith(docExtension, Assembly.GetEntryAssembly().Location, typeId);
		}

		public static bool IsFileOpenWithRegistered(string docExtension, string appPath, string typeId)
		{
			string fileName = Path.GetFileName(appPath);
			using (RegistryKey registryKey = ClassesRoot.OpenSubKey(docExtension + "\\OpenWithProgIds"))
			{
				return registryKey?.GetValue(typeId) != null;
			}
		}

		public static bool IsFileOpenWithRegistered(string docExtension, string typeId)
		{
			return IsFileOpenWithRegistered(docExtension, Assembly.GetEntryAssembly().Location, typeId);
		}

		public static void RegisterFileCommand(string docExtension, string verbName, string menuText, string appPath, string commandParameters)
		{
			verbName = verbName.Replace("&", "");
			using (RegistryKey registryKey = ClassesRoot.CreateSubKey(docExtension + "\\Shell\\" + verbName))
			{
				registryKey.SetValue(string.Empty, menuText);
				using (RegistryKey registryKey2 = registryKey.CreateSubKey("command"))
				{
					registryKey2.SetValue(string.Empty, "\"" + appPath + "\" " + commandParameters);
				}
			}
		}

		public static void RegisterFileCommand(string docExtension, string verbName, string menuText, string commandParameters)
		{
			RegisterFileCommand(docExtension, verbName, menuText, Assembly.GetEntryAssembly().Location, commandParameters);
		}

		public static void RegisterFileCommand(string docExtension, string menuText, string commandParameters)
		{
			RegisterFileCommand(docExtension, menuText, menuText, commandParameters);
		}

		public static void RegisterFileCommand(string docExtension, string menuText)
		{
			RegisterFileCommand(docExtension, menuText, "\"%1\"");
		}
	}
}
