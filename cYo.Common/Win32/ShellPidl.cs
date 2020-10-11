using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Permissions;
using System.Text;
using cYo.Common.ComponentModel;

namespace cYo.Common.Win32
{
	[ComVisible(false)]
	public class ShellPidl : DisposableObject, IComparable, ICloneable
	{
		private static class NativeMethods
		{
			public struct RECT
			{
				public int left;

				public int top;

				public int right;

				public int bottom;

				public RECT(Rectangle r)
				{
					left = r.Left;
					top = r.Top;
					right = r.Right;
					bottom = r.Bottom;
				}
			}

			[Flags]
			public enum CSIDL
			{
				CSIDL_FLAG_CREATE = 0x8000,
				CSIDL_ADMINTOOLS = 0x30,
				CSIDL_ALTSTARTUP = 0x1D,
				CSIDL_APPDATA = 0x1A,
				CSIDL_BITBUCKET = 0xA,
				CSIDL_CDBURN_AREA = 0x3B,
				CSIDL_COMMON_ADMINTOOLS = 0x2F,
				CSIDL_COMMON_ALTSTARTUP = 0x1E,
				CSIDL_COMMON_APPDATA = 0x23,
				CSIDL_COMMON_DESKTOPDIRECTORY = 0x19,
				CSIDL_COMMON_DOCUMENTS = 0x2E,
				CSIDL_COMMON_FAVORITES = 0x1F,
				CSIDL_COMMON_MUSIC = 0x35,
				CSIDL_COMMON_PICTURES = 0x36,
				CSIDL_COMMON_PROGRAMS = 0x17,
				CSIDL_COMMON_STARTMENU = 0x16,
				CSIDL_COMMON_STARTUP = 0x18,
				CSIDL_COMMON_TEMPLATES = 0x2D,
				CSIDL_COMMON_VIDEO = 0x37,
				CSIDL_CONTROLS = 0x3,
				CSIDL_COOKIES = 0x21,
				CSIDL_DESKTOP = 0x0,
				CSIDL_DESKTOPDIRECTORY = 0x10,
				CSIDL_DRIVES = 0x11,
				CSIDL_FAVORITES = 0x6,
				CSIDL_FONTS = 0x14,
				CSIDL_HISTORY = 0x22,
				CSIDL_INTERNET = 0x1,
				CSIDL_INTERNET_CACHE = 0x20,
				CSIDL_LOCAL_APPDATA = 0x1C,
				CSIDL_MYDOCUMENTS = 0xC,
				CSIDL_MYMUSIC = 0xD,
				CSIDL_MYPICTURES = 0x27,
				CSIDL_MYVIDEO = 0xE,
				CSIDL_NETHOOD = 0x13,
				CSIDL_NETWORK = 0x12,
				CSIDL_PERSONAL = 0x5,
				CSIDL_PRINTERS = 0x4,
				CSIDL_PRINTHOOD = 0x1B,
				CSIDL_PROFILE = 0x28,
				CSIDL_PROFILES = 0x3E,
				CSIDL_PROGRAM_FILES = 0x26,
				CSIDL_PROGRAM_FILES_COMMON = 0x2B,
				CSIDL_PROGRAMS = 0x2,
				CSIDL_RECENT = 0x8,
				CSIDL_SENDTO = 0x9,
				CSIDL_STARTMENU = 0xB,
				CSIDL_STARTUP = 0x7,
				CSIDL_SYSTEM = 0x25,
				CSIDL_TEMPLATES = 0x15,
				CSIDL_WINDOWS = 0x24
			}

			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
			public struct SHFILEINFO
			{
				public IntPtr hIcon;

				public int iIcon;

				public uint dwAttributes;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
				public string szDisplayName;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
				public string szTypeName;
			}

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
			public enum SHGFI : uint
			{
				SHGFI_ICON = 0x100u,
				SHGFI_DISPLAYNAME = 0x200u,
				SHGFI_TYPENAME = 0x400u,
				SHGFI_ATTRIBUTES = 0x800u,
				SHGFI_ICONLOCATION = 0x1000u,
				SHGFI_EXETYPE = 0x2000u,
				SHGFI_SYSICONINDEX = 0x4000u,
				SHGFI_LINKOVERLAY = 0x8000u,
				SHGFI_SELECTED = 0x10000u,
				SHGFI_ATTR_SPECIFIED = 0x20000u,
				SHGFI_LARGEICON = 0x0u,
				SHGFI_SMALLICON = 0x1u,
				SHGFI_OPENICON = 0x2u,
				SHGFI_SHELLICONSIZE = 0x4u,
				SHGFI_PIDL = 0x8u,
				SHGFI_USEFILEATTRIBUTES = 0x10u,
				SHGFI_ADDOVERLAYS = 0x20u,
				SHGFI_OVERLAYINDEX = 0x40u
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

			public enum SVUIA_STATUS
			{
				SVUIA_DEACTIVATE,
				SVUIA_ACTIVATE_NOFOCUS,
				SVUIA_ACTIVATE_FOCUS,
				SVUIA_INPLACEACTIVATE
			}

			public struct FOLDERSETTINGS
			{
				public FOLDERFLAGS ViewMode;

				public FOLDERVIEWMODE fFlags;
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

			public const uint FILE_ATTRIBUTE_DIRECTORY = 16u;

			[DllImport("shell32.dll", EntryPoint = "#18")]
			public static extern IntPtr ILClone(IntPtr pidl);

			[DllImport("shell32.dll", EntryPoint = "#25")]
			public static extern IntPtr ILCombine(IntPtr pidlA, IntPtr pidlB);

			[DllImport("shell32.dll", EntryPoint = "#17")]
			public static extern bool ILRemoveLastID(IntPtr pidl);

			[DllImport("shell32.dll")]
			public static extern int SHGetDesktopFolder(out IShellFolder ppshf);

			[DllImport("shell32.dll")]
			public static extern int SHGetFolderLocation(IntPtr hwndOwner, CSIDL nFolder, IntPtr hToken, uint dwReserved, out IntPtr ppidl);

			[DllImport("shell32.dll")]
			public static extern IntPtr SHGetFileInfo(string fileName, uint dwFileAttributes, ref SHFILEINFO psfi, int cbSizeFileInfo, SHGFI flags);

			[DllImport("shell32.dll", CharSet = CharSet.Auto)]
			public static extern IntPtr SHGetFileInfo(IntPtr pidl, uint dwFileAttributes, ref SHFILEINFO psfi, int cbSizeFileInfo, SHGFI flags);

			[DllImport("shell32.dll", CharSet = CharSet.Auto)]
			public static extern bool SHGetPathFromIDList(IntPtr pidl, StringBuilder pszPath);

			[DllImport("User32.dll")]
			public static extern int DestroyIcon(IntPtr hIcon);
		}

		private static readonly NativeMethods.IShellFolder DesktopFolder;

		private NativeMethods.SFGAO attributes;

		private static readonly IntPtr desktopPidl;

		private static readonly IntPtr networkPidl;

		private static readonly IntPtr controlPanelPidl;

		private static readonly IntPtr recycleBinPidl;

		private IntPtr myPidl;

		private string displayName;

		private string typeName;

		private string physicalPath;

		private int iconIndex;

		public IntPtr Pidl => myPidl;

		public string DisplayName => displayName;

		public string TypeName => typeName;

		public string PhysicalPath => physicalPath;

		public int IconIndex => iconIndex;

		public string IconKey => "K" + IconIndex;

		public bool CanRename => (attributes & NativeMethods.SFGAO.SFGAO_CANRENAME) != 0;

		public bool CanMove => (attributes & NativeMethods.SFGAO.SFGAO_CANMOVE) != 0;

		public bool CanDelete => (attributes & NativeMethods.SFGAO.SFGAO_CANDELETE) != 0;

		public bool CanCopy => (attributes & NativeMethods.SFGAO.SFGAO_CANCOPY) != 0;

		public bool IsReadOnly => (attributes & NativeMethods.SFGAO.SFGAO_READONLY) != 0;

		public bool IsEncrypted => (attributes & NativeMethods.SFGAO.SFGAO_ENCRYPTED) != 0;

		public bool IsLink => (attributes & NativeMethods.SFGAO.SFGAO_LINK) != 0;

		public bool IsHidden => (attributes & NativeMethods.SFGAO.SFGAO_HIDDEN) != 0;

		public bool IsSlow => (attributes & NativeMethods.SFGAO.SFGAO_ISSLOW) != 0;

		public bool IsGhosted => (attributes & NativeMethods.SFGAO.SFGAO_GHOSTED) != 0;

		public bool IsCompressed => (attributes & NativeMethods.SFGAO.SFGAO_COMPRESSED) != 0;

		public bool IsRemovable => (attributes & NativeMethods.SFGAO.SFGAO_REMOVABLE) != 0;

		public bool IsShared => (attributes & NativeMethods.SFGAO.SFGAO_SHARE) != 0;

		public bool IsFolder => (attributes & NativeMethods.SFGAO.SFGAO_FOLDER) != 0;

		public bool IsFileSystem => (attributes & NativeMethods.SFGAO.SFGAO_FILESYSTEM) != 0;

		public bool HasSubfolders => ((uint)attributes & 0x80000000u) != 0;

		public bool IsBrowsable => (attributes & NativeMethods.SFGAO.SFGAO_BROWSABLE) != 0;

		public bool IsRecycleBin => DesktopFolder.CompareIDs(NativeMethods.SHCIDS.SHCIDS_CANONICALONLY, myPidl, recycleBinPidl) == 0;

		public bool IsNetwork => DesktopFolder.CompareIDs(NativeMethods.SHCIDS.SHCIDS_CANONICALONLY, myPidl, networkPidl) == 0;

		public bool IsControlPanel => DesktopFolder.CompareIDs(NativeMethods.SHCIDS.SHCIDS_CANONICALONLY, myPidl, controlPanelPidl) == 0;

		public bool IsDesktop => DesktopFolder.CompareIDs(NativeMethods.SHCIDS.SHCIDS_CANONICALONLY, myPidl, desktopPidl) == 0;

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		static ShellPidl()
		{
			IntPtr zero = IntPtr.Zero;
			NativeMethods.SHGetDesktopFolder(out DesktopFolder);
			NativeMethods.SHGetFolderLocation(IntPtr.Zero, NativeMethods.CSIDL.CSIDL_DESKTOP, zero, 0u, out desktopPidl);
			NativeMethods.SHGetFolderLocation(IntPtr.Zero, NativeMethods.CSIDL.CSIDL_NETWORK, zero, 0u, out networkPidl);
			NativeMethods.SHGetFolderLocation(IntPtr.Zero, NativeMethods.CSIDL.CSIDL_CONTROLS, zero, 0u, out controlPanelPidl);
			NativeMethods.SHGetFolderLocation(IntPtr.Zero, NativeMethods.CSIDL.CSIDL_BITBUCKET, zero, 0u, out recycleBinPidl);
		}

		public ShellPidl()
		{
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public ShellPidl(IntPtr pidl)
		{
			Open(pidl);
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public ShellPidl(IntPtr parentPidl, IntPtr pidl, int attr)
		{
			Open(parentPidl, pidl, attr);
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public ShellPidl(Environment.SpecialFolder specialFolder)
		{
			Open(specialFolder);
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public ShellPidl(string fullPath)
		{
			Open(fullPath);
		}

		protected override void Dispose(bool disposing)
		{
			Close();
		}

		public override bool Equals(object obj)
		{
			ShellPidl shellPidl = obj as ShellPidl;
			if (shellPidl == null)
			{
				return false;
			}
			return DesktopFolder.CompareIDs(NativeMethods.SHCIDS.SHCIDS_CANONICALONLY, myPidl, shellPidl.myPidl) == 0;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode() ^ myPidl.GetHashCode();
		}

		public void Open(IntPtr pidl)
		{
			myPidl = NativeMethods.ILClone(pidl);
			InitializeObject();
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public void Open(IntPtr parentPidl, IntPtr pidl, int attr)
		{
			myPidl = NativeMethods.ILCombine(parentPidl, pidl);
			attributes = (NativeMethods.SFGAO)attr;
			InitializeObject();
			if (string.IsNullOrEmpty(typeName))
			{
				Open(pidl);
			}
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public void Open(Environment.SpecialFolder specialFolder)
		{
			IntPtr zero = IntPtr.Zero;
			int num = NativeMethods.SHGetFolderLocation(IntPtr.Zero, SpecialFolderToCSIDL(specialFolder), zero, 0u, out myPidl);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			InitializeObject();
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public void Open(string fullPath)
		{
			uint pdwAttributes = 0u;
			uint pchEaten = 0u;
			int num = DesktopFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, fullPath, ref pchEaten, out myPidl, ref pdwAttributes);
			if (num != 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
			InitializeObject();
		}

		public void Close()
		{
			if (!(myPidl == IntPtr.Zero))
			{
				Marshal.FreeCoTaskMem(myPidl);
				myPidl = IntPtr.Zero;
			}
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public ShellPidl GetParentFolder()
		{
			if (myPidl == IntPtr.Zero)
			{
				return null;
			}
			if (DesktopFolder.CompareIDs(NativeMethods.SHCIDS.SHCIDS_ALLFIELDS, myPidl, desktopPidl) == 0)
			{
				return null;
			}
			IntPtr intPtr = NativeMethods.ILClone(myPidl);
			if (!NativeMethods.ILRemoveLastID(intPtr))
			{
				Marshal.FreeCoTaskMem(intPtr);
				intPtr = IntPtr.Zero;
				return null;
			}
			return new ShellPidl(intPtr);
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public List<ShellPidl> GetAncestors()
		{
			List<ShellPidl> list = new List<ShellPidl>();
			if (myPidl == IntPtr.Zero)
			{
				return list;
			}
			for (ShellPidl shellPidl = (ShellPidl)Clone(); shellPidl != null; shellPidl = shellPidl.GetParentFolder())
			{
				list.Add(shellPidl);
			}
			return list;
		}

		public Image GetImage(ShellIconType iconType)
		{
			NativeMethods.SHGFI sHGFI = NativeMethods.SHGFI.SHGFI_ICON | NativeMethods.SHGFI.SHGFI_PIDL;
			if ((iconType & ShellIconType.Link) != 0)
			{
				sHGFI |= NativeMethods.SHGFI.SHGFI_LINKOVERLAY;
			}
			if ((iconType & ShellIconType.Open) != 0)
			{
				sHGFI |= NativeMethods.SHGFI.SHGFI_OPENICON;
			}
			sHGFI = (((iconType & ShellIconType.Large) != 0) ? (sHGFI | NativeMethods.SHGFI.SHGFI_LARGEICON) : (sHGFI | NativeMethods.SHGFI.SHGFI_SMALLICON));
			NativeMethods.SHFILEINFO psfi = default(NativeMethods.SHFILEINFO);
			try
			{
				NativeMethods.SHGetFileInfo(myPidl, 0u, ref psfi, Marshal.SizeOf((object)psfi), sHGFI);
				return Icon.FromHandle(psfi.hIcon).ToBitmap();
			}
			finally
			{
				NativeMethods.DestroyIcon(psfi.hIcon);
			}
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private void InitializeObject()
		{
			bool flag = attributes == (NativeMethods.SFGAO)0u;
			NativeMethods.SHFILEINFO psfi = default(NativeMethods.SHFILEINFO);
			NativeMethods.SHGetFileInfo(myPidl, 0u, ref psfi, Marshal.SizeOf((object)psfi), NativeMethods.SHGFI.SHGFI_DISPLAYNAME | NativeMethods.SHGFI.SHGFI_SYSICONINDEX | NativeMethods.SHGFI.SHGFI_SMALLICON | NativeMethods.SHGFI.SHGFI_PIDL | (flag ? NativeMethods.SHGFI.SHGFI_ATTRIBUTES : NativeMethods.SHGFI.SHGFI_LARGEICON) | NativeMethods.SHGFI.SHGFI_TYPENAME);
			displayName = psfi.szDisplayName;
			typeName = psfi.szTypeName;
			iconIndex = psfi.iIcon;
			if (flag)
			{
				attributes = (NativeMethods.SFGAO)psfi.dwAttributes;
			}
			StringBuilder stringBuilder = new StringBuilder(260);
			NativeMethods.SHGetPathFromIDList(myPidl, stringBuilder);
			physicalPath = stringBuilder.ToString();
		}

		private static NativeMethods.CSIDL SpecialFolderToCSIDL(Environment.SpecialFolder sf)
		{
			switch (sf)
			{
			case Environment.SpecialFolder.ApplicationData:
				return NativeMethods.CSIDL.CSIDL_APPDATA;
			case Environment.SpecialFolder.CommonApplicationData:
				return NativeMethods.CSIDL.CSIDL_COMMON_APPDATA;
			case Environment.SpecialFolder.CommonProgramFiles:
				return NativeMethods.CSIDL.CSIDL_COMMON_PROGRAMS;
			case Environment.SpecialFolder.Cookies:
				return NativeMethods.CSIDL.CSIDL_COOKIES;
			case Environment.SpecialFolder.DesktopDirectory:
				return NativeMethods.CSIDL.CSIDL_DESKTOPDIRECTORY;
			case Environment.SpecialFolder.Favorites:
				return NativeMethods.CSIDL.CSIDL_FAVORITES;
			case Environment.SpecialFolder.History:
				return NativeMethods.CSIDL.CSIDL_HISTORY;
			case Environment.SpecialFolder.InternetCache:
				return NativeMethods.CSIDL.CSIDL_INTERNET_CACHE;
			case Environment.SpecialFolder.LocalApplicationData:
				return NativeMethods.CSIDL.CSIDL_LOCAL_APPDATA;
			case Environment.SpecialFolder.MyComputer:
				return NativeMethods.CSIDL.CSIDL_DRIVES;
			case Environment.SpecialFolder.MyMusic:
				return NativeMethods.CSIDL.CSIDL_MYMUSIC;
			case Environment.SpecialFolder.MyPictures:
				return NativeMethods.CSIDL.CSIDL_MYPICTURES;
			case Environment.SpecialFolder.Personal:
				return NativeMethods.CSIDL.CSIDL_PERSONAL;
			case Environment.SpecialFolder.ProgramFiles:
				return NativeMethods.CSIDL.CSIDL_PROGRAM_FILES;
			case Environment.SpecialFolder.Programs:
				return NativeMethods.CSIDL.CSIDL_PROGRAMS;
			case Environment.SpecialFolder.Recent:
				return NativeMethods.CSIDL.CSIDL_RECENT;
			case Environment.SpecialFolder.SendTo:
				return NativeMethods.CSIDL.CSIDL_SENDTO;
			case Environment.SpecialFolder.StartMenu:
				return NativeMethods.CSIDL.CSIDL_STARTMENU;
			case Environment.SpecialFolder.Startup:
				return NativeMethods.CSIDL.CSIDL_STARTUP;
			case Environment.SpecialFolder.System:
				return NativeMethods.CSIDL.CSIDL_SYSTEM;
			case Environment.SpecialFolder.Templates:
				return NativeMethods.CSIDL.CSIDL_TEMPLATES;
			default:
				return NativeMethods.CSIDL.CSIDL_DESKTOP;
			}
		}

		public int CompareTo(object obj)
		{
			ShellPidl shellPidl = obj as ShellPidl;
			if (shellPidl == null)
			{
				return 0;
			}
			return DesktopFolder.CompareIDs(NativeMethods.SHCIDS.SHCIDS_CANONICALONLY, shellPidl.Pidl, myPidl);
		}

		public object Clone()
		{
			ShellPidl shellPidl = new ShellPidl();
			shellPidl.myPidl = NativeMethods.ILClone(myPidl);
			shellPidl.displayName = displayName;
			shellPidl.typeName = typeName;
			shellPidl.attributes = attributes;
			shellPidl.physicalPath = physicalPath;
			shellPidl.iconIndex = iconIndex;
			return shellPidl;
		}
	}
}
