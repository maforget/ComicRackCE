using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Permissions;
using cYo.Common.ComponentModel;

namespace cYo.Common.Win32
{
	[ComVisible(false)]
	public sealed class ShellFolder : DisposableObject
	{
		private static class NativeMethods
		{
			[Flags]
			public enum SHCONTF
			{
				SHCONTF_FOLDERS = 0x20,
				SHCONTF_NONFOLDERS = 0x40,
				SHCONTF_INCLUDEHIDDEN = 0x80,
				SHCONTF_INIT_ON_FIRST_NEXT = 0x100,
				SHCONTF_NETPRINTERSRCH = 0x200,
				SHCONTF_SHAREABLE = 0x400,
				SHCONTF_STORAGE = 0x800
			}

			[Flags]
			public enum SHCIDS : uint
			{
				SHCIDS_ALLFIELDS = 0x80000000u,
				SHCIDS_CANONICALONLY = 0x10000000u,
				SHCIDS_BITMASK = 0xFFFF0000u,
				SHCIDS_COLUMNMASK = 0xFFFFu
			}

			[Flags]
			public enum FOLDERFLAGS
			{
				FWF_AUTOARRANGE = 0x1,
				FWF_ABBREVIATEDNAMES = 0x2,
				FWF_SNAPTOGRID = 0x4,
				FWF_OWNERDATA = 0x8,
				FWF_BESTFITWINDOW = 0x10,
				FWF_DESKTOP = 0x20,
				FWF_SINGLESEL = 0x40,
				FWF_NOSUBFOLDERS = 0x80,
				FWF_TRANSPARENT = 0x100,
				FWF_NOCLIENTEDGE = 0x200,
				FWF_NOSCROLL = 0x400,
				FWF_ALIGNLEFT = 0x800,
				FWF_NOICONS = 0x1000,
				FWF_SHOWSELALWAYS = 0x2000,
				FWF_NOVISIBLE = 0x4000,
				FWF_SINGLECLICKACTIVATE = 0x8000,
				FWF_NOWEBVIEW = 0x10000,
				FWF_HIDEFILENAMES = 0x20000,
				FWF_CHECKSELECT = 0x40000
			}

			public enum FOLDERVIEWMODE
			{
				FVM_FIRST = 1,
				FVM_ICON = 1,
				FVM_SMALLICON = 2,
				FVM_LIST = 3,
				FVM_DETAILS = 4,
				FVM_THUMBNAIL = 5,
				FVM_TILE = 6,
				FVM_THUMBSTRIP = 7,
				FVM_LAST = 7
			}

			[StructLayout(LayoutKind.Explicit)]
			public struct STRRET
			{
				[FieldOffset(0)]
				private uint uType;

				[FieldOffset(4)]
				private IntPtr pOleStr;

				[FieldOffset(4)]
				private IntPtr pStr;

				[FieldOffset(4)]
				private uint uOffset;

				[FieldOffset(4)]
				private IntPtr cStr;
			}

			[Flags]
			public enum SFGAO : uint
			{
				SFGAO_CANCOPY = 0x1u,
				SFGAO_CANMOVE = 0x2u,
				SFGAO_CANLINK = 0x4u,
				SFGAO_STORAGE = 0x8u,
				SFGAO_CANRENAME = 0x10u,
				SFGAO_CANDELETE = 0x20u,
				SFGAO_HASPROPSHEET = 0x40u,
				SFGAO_DROPTARGET = 0x100u,
				SFGAO_CAPABILITYMASK = 0x177u,
				SFGAO_ENCRYPTED = 0x2000u,
				SFGAO_ISSLOW = 0x4000u,
				SFGAO_GHOSTED = 0x8000u,
				SFGAO_LINK = 0x10000u,
				SFGAO_SHARE = 0x20000u,
				SFGAO_READONLY = 0x40000u,
				SFGAO_HIDDEN = 0x80000u,
				SFGAO_DISPLAYATTRMASK = 0xFC000u,
				SFGAO_FILESYSANCESTOR = 0x10000000u,
				SFGAO_FOLDER = 0x20000000u,
				SFGAO_FILESYSTEM = 0x40000000u,
				SFGAO_HASSUBFOLDER = 0x80000000u,
				SFGAO_CONTENTSMASK = 0x80000000u,
				SFGAO_VALIDATE = 0x1000000u,
				SFGAO_REMOVABLE = 0x2000000u,
				SFGAO_COMPRESSED = 0x4000000u,
				SFGAO_BROWSABLE = 0x8000000u,
				SFGAO_NONENUMERATED = 0x100000u,
				SFGAO_NEWCONTENT = 0x200000u,
				SFGAO_CANMONIKER = 0x400000u,
				SFGAO_HASSTORAGE = 0x400000u,
				SFGAO_STREAM = 0x400000u,
				SFGAO_STORAGEANCESTOR = 0x800000u,
				SFGAO_STORAGECAPMASK = 0x70C50008u
			}

			[Flags]
			public enum SHGDN
			{
				SHGDN_NORMAL = 0x0,
				SHGDN_INFOLDER = 0x1,
				SHGDN_FORADDRESSBAR = 0x4000,
				SHGDN_FORPARSING = 0x8000
			}

			public struct RECT
			{
				public int Left;

				public int Top;

				public int Right;

				public int Bottom;
			}

			public struct FOLDERSETTINGS
			{
				public FOLDERFLAGS ViewMode;

				public FOLDERVIEWMODE fFlags;
			}

			public enum SVUIA_STATUS
			{
				SVUIA_DEACTIVATE,
				SVUIA_ACTIVATE_NOFOCUS,
				SVUIA_ACTIVATE_FOCUS,
				SVUIA_INPLACEACTIVATE
			}

			[ComImport]
			[Guid("000214F2-0000-0000-C000-000000000046")]
			[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
			public interface IEnumIDList
			{
				[PreserveSig]
				int Next(int celt, ref IntPtr rgelt, out int pceltFetched);

				[PreserveSig]
				int Skip(int celt);

				[PreserveSig]
				int Reset();

				[PreserveSig]
				int Clone(ref IEnumIDList ppenum);
			}

			[ComImport]
			[Guid("000214E2-0000-0000-C000-000000000046")]
			[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
			public interface IShellBrowser
			{
				[PreserveSig]
				int GetWindow(out IntPtr phwnd);

				[PreserveSig]
				int ContextSensitiveHelp(bool fEnterMode);

				[PreserveSig]
				int InsertMenusSB(IntPtr hmenuShared, ref IntPtr lpMenuWidths);

				[PreserveSig]
				int SetMenuSB(IntPtr hmenuShared, IntPtr holemenuRes, IntPtr hwndActiveObject);

				[PreserveSig]
				int RemoveMenusSB(IntPtr hmenuShared);

				[PreserveSig]
				int SetStatusTextSB(string pszStatusText);

				[PreserveSig]
				int EnableModelessSB(bool fEnable);

				[PreserveSig]
				int TranslateAcceleratorSB(IntPtr pmsg, short wID);

				[PreserveSig]
				int BrowseObject(IntPtr pidl, uint wFlags);

				[PreserveSig]
				int GetViewStateStream(long grfMode, ref IStream ppStrm);

				[PreserveSig]
				int GetControlWindow(uint id, ref IntPtr phwnd);

				[PreserveSig]
				int SendControlMsg(uint id, uint uMsg, short wParam, long lParam, ref long pret);

				[PreserveSig]
				int QueryActiveShellView(ref IShellView ppshv);

				[PreserveSig]
				int OnViewWindowActive(IShellView pshv);

				[PreserveSig]
				int SetToolbarItems(IntPtr lpButtons, uint nButtons, uint uFlags);
			}

			[ComImport]
			[Guid("000214E3-0000-0000-C000-000000000046")]
			[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
			public interface IShellView
			{
				[PreserveSig]
				int GetWindow(out IntPtr phwnd);

				[PreserveSig]
				int ContextSensitiveHelp(bool fEnterMode);

				[PreserveSig]
				int TranslateAcceleratorA(IntPtr pmsg);

				[PreserveSig]
				int EnableModeless(bool fEnable);

				[PreserveSig]
				int UIActivate(SVUIA_STATUS uState);

				[PreserveSig]
				int Refresh();

				[PreserveSig]
				int CreateViewWindow(IShellView psvPrevious, ref FOLDERSETTINGS pfs, IShellBrowser psb, ref RECT prcView, out IntPtr phWnd);

				[PreserveSig]
				int DestroyViewWindow();

				[PreserveSig]
				int GetCurrentInfo(ref FOLDERSETTINGS pfs);

				[PreserveSig]
				int AddPropertySheetPages(long dwReserved, ref IntPtr pfnPtr, int lparam);

				[PreserveSig]
				int SaveViewState();

				[PreserveSig]
				int SelectItem(IntPtr pidlItem, uint uFlags);

				[PreserveSig]
				int GetItemObject(uint uItem, ref Guid riid, ref IntPtr ppv);
			}

			[ComImport]
			[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
			[Guid("000214E6-0000-0000-C000-000000000046")]
			public interface IShellFolder
			{
				[PreserveSig]
				int ParseDisplayName(IntPtr hwnd, IntPtr pbc, [MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, ref uint pchEaten, out IntPtr ppidl, ref uint pdwAttributes);

				[PreserveSig]
				int EnumObjects(IntPtr hwnd, SHCONTF grfFlags, out IEnumIDList ppenumIDList);

				[PreserveSig]
				int BindToObject(IntPtr pidl, IntPtr pbc, ref Guid riid, out IntPtr ppv);

				[PreserveSig]
				int BindToStorage(IntPtr pidl, IntPtr pbc, ref Guid riid, out IntPtr ppv);

				[PreserveSig]
				int CompareIDs(SHCIDS lParam, IntPtr pidl1, IntPtr pidl2);

				[PreserveSig]
				int CreateViewObject(IntPtr hwndOwner, ref Guid riid, out IShellView ppv);

				[PreserveSig]
				int GetAttributesOf(uint cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] IntPtr[] apidl, ref SFGAO rgfInOut);

				[PreserveSig]
				int GetUIObjectOf(IntPtr hwndOwner, uint cidl, IntPtr[] apidl, Guid riid, ref uint rgfReserved, out IntPtr ppv);

				[PreserveSig]
				int GetDisplayNameOf(IntPtr pidl, SHGDN uFlags, out STRRET pName);

				[PreserveSig]
				int SetNameOf(IntPtr hwnd, IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] string pszName, uint uFlags, out IntPtr ppidlOut);
			}

			public static Guid IID_IShellFolder = new Guid("{000214E6-0000-0000-C000-000000000046}");

			[DllImport("shell32.dll")]
			public static extern int SHGetDesktopFolder(out IShellFolder ppshf);
		}

		private static readonly NativeMethods.IShellFolder c_desktopFolder;

		private ShellPidl m_pidl;

		private NativeMethods.IShellFolder m_folder;

		public ShellPidl Pidl => m_pidl;

		public object Interface => m_folder;

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		static ShellFolder()
		{
			NativeMethods.SHGetDesktopFolder(out c_desktopFolder);
		}

		public ShellFolder()
		{
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public ShellFolder(Environment.SpecialFolder specialFolder)
		{
			Open(specialFolder);
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public ShellFolder(string fullPath)
		{
			Open(fullPath);
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public ShellFolder(ShellPidl pidl)
		{
			Open(pidl);
		}

		protected override void Dispose(bool disposing)
		{
			Close();
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public void Open(Environment.SpecialFolder specialFolder)
		{
			Close();
			m_pidl = new ShellPidl(specialFolder);
			InitializeFolder();
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public void Open(string fullPath)
		{
			m_pidl = new ShellPidl(fullPath);
			InitializeFolder();
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public void Open(ShellPidl pidl)
		{
			m_pidl = (ShellPidl)pidl.Clone();
			InitializeFolder();
		}

		public void Close()
		{
			if (m_pidl != null)
			{
				m_pidl.Dispose();
			}
			m_pidl = null;
			if (m_folder != null)
			{
				Marshal.ReleaseComObject(m_folder);
			}
			m_folder = null;
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public List<ShellPidl> GetChildren(bool showHiddenObjects, bool showNonFolders, bool optimized)
		{
			List<ShellPidl> list = new List<ShellPidl>();
			NativeMethods.IEnumIDList ppenumIDList = null;
			try
			{
				if (m_pidl == null || !m_pidl.IsFolder || m_folder == null)
				{
					return list;
				}
				int num = m_folder.EnumObjects(IntPtr.Zero, NativeMethods.SHCONTF.SHCONTF_FOLDERS | (showNonFolders ? NativeMethods.SHCONTF.SHCONTF_NONFOLDERS : ((NativeMethods.SHCONTF)0)) | (showHiddenObjects ? NativeMethods.SHCONTF.SHCONTF_INCLUDEHIDDEN : ((NativeMethods.SHCONTF)0)), out ppenumIDList);
				if (num != 0)
				{
					Marshal.ThrowExceptionForHR(num);
				}
				IntPtr rgelt = IntPtr.Zero;
				int pceltFetched = 0;
				while (ppenumIDList.Next(1, ref rgelt, out pceltFetched) == 0)
				{
					if (pceltFetched == 1)
					{
						NativeMethods.SFGAO attr = (NativeMethods.SFGAO)0u;
						if (optimized)
						{
							attr = NativeMethods.SFGAO.SFGAO_FOLDER | NativeMethods.SFGAO.SFGAO_FILESYSTEM | NativeMethods.SFGAO.SFGAO_BROWSABLE;
							m_folder.GetAttributesOf(1u, new IntPtr[1]
							{
								rgelt
							}, ref attr);
							attr |= NativeMethods.SFGAO.SFGAO_HASSUBFOLDER;
						}
						ShellPidl item = new ShellPidl(m_pidl.Pidl, rgelt, (int)attr);
						list.Add(item);
						Marshal.FreeCoTaskMem(rgelt);
						continue;
					}
					return list;
				}
				return list;
			}
			finally
			{
				if (ppenumIDList != null)
				{
					Marshal.ReleaseComObject(ppenumIDList);
				}
			}
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private void InitializeFolder()
		{
			if (m_pidl.IsDesktop)
			{
				NativeMethods.SHGetDesktopFolder(out m_folder);
				return;
			}
			NativeMethods.IShellFolder shellFolder = null;
			try
			{
				IntPtr ppv;
				int num = c_desktopFolder.BindToObject(m_pidl.Pidl, IntPtr.Zero, ref NativeMethods.IID_IShellFolder, out ppv);
				if (num != 0)
				{
					Marshal.ThrowExceptionForHR(num);
				}
				m_folder = (NativeMethods.IShellFolder)Marshal.GetTypedObjectForIUnknown(ppv, typeof(NativeMethods.IShellFolder));
			}
			finally
			{
				if (shellFolder != null)
				{
					Marshal.ReleaseComObject(shellFolder);
				}
				shellFolder = null;
			}
		}
	}
}
