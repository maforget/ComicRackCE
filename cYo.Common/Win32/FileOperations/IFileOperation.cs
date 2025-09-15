using System;
using Microsoft.VisualBasic.FileIO;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace cYo.Common.Win32.FileOperations
{
	internal class IFileOperation : FileOperation
	{
		//ref: https://github.com/RickStrahl/DeleteFiles/blob/master/DeleteFiles/ZetaLongPaths/Native/FileOperations/FileOperation.cs
		private readonly Native.IFileOperation _fileOperation;
		private readonly FileOperationProgressSink _callbackSink;
		private readonly uint _sinkCookie;

		private static readonly Guid CLSID_FileOperation = new Guid("3ad05575-8857-4850-9277-11b85bdb8e09");
		private static readonly Type _fileOperationType = Type.GetTypeFromCLSID(CLSID_FileOperation);
		private static Guid _shellItemGuid = typeof(Native.IShellItem).GUID;

		public static class Native
		{
			public enum HRESULT : uint
			{
				S_OK = 0,
				S_FALSE = 1,
				E_NOINTERFACE = 0x80004002,
				E_NOTIMPL = 0x80004001,
				E_FAIL = 0x80004005,
				E_INVALIDARG = 0x80070057,
				E_CANCELLED = 0x80070000 + ERROR_CANCELLED,
				E_UNEXPECTED = 0x8000FFFF
			}
			public const uint ERROR_CANCELLED = 1223;

			public enum SIGDN : uint
			{
				SIGDN_NORMALDISPLAY = 0x00000000,
				SIGDN_PARENTRELATIVEPARSING = 0x80018001,
				SIGDN_PARENTRELATIVEFORADDRESSBAR = 0x8001c001,
				SIGDN_DESKTOPABSOLUTEPARSING = 0x80028000,
				SIGDN_PARENTRELATIVEEDITING = 0x80031001,
				SIGDN_DESKTOPABSOLUTEEDITING = 0x8004c000,
				SIGDN_FILESYSPATH = 0x80058000,
				SIGDN_URL = 0x80068000
			}

			public enum CopyEngineResult : uint
			{
				COPYENGINE_OK = 0x0,

				COPYENGINE_S_YES = 0x00270001,
				COPYENGINE_S_NOT_HANDLED = 0x00270003,
				COPYENGINE_S_USER_RETRY = 0x00270004,
				COPYENGINE_S_USER_IGNORED = 0x00270005,
				COPYENGINE_S_MERGE = 0x00270006,
				COPYENGINE_S_DONT_PROCESS_CHILDREN = 0x00270008,
				COPYENGINE_S_ALREADY_DONE = 0x0027000A,
				COPYENGINE_S_PENDING = 0x0027000B,
				COPYENGINE_S_KEEP_BOTH = 0x0027000C,
				COPYENGINE_S_CLOSE_PROGRAM = 0x0027000D, // Close the program using the current file

				// Failure/Error codes
				COPYENGINE_E_USER_CANCELLED = 0x80270000,  // User wants to canceled entire job
				COPYENGINE_E_CANCELLED = 0x80270001,  // Engine wants to canceled entire job, don't set the CANCELLED bit
				COPYENGINE_E_REQUIRES_ELEVATION = 0x80270002,  // Need to elevate the process to complete the operation

				COPYENGINE_E_SAME_FILE = 0x80270003,  // Source and destination file are the same
				COPYENGINE_E_DIFF_DIR = 0x80270004,  // Trying to rename a file into a different location, use move instead
				COPYENGINE_E_MANY_SRC_1_DEST = 0x80270005,  // One source specified, multiple destinations

				COPYENGINE_E_DEST_SUBTREE = 0x80270009,  // The destination is a sub-tree of the source
				COPYENGINE_E_DEST_SAME_TREE = 0x8027000A,  // The destination is the same folder as the source

				COPYENGINE_E_FLD_IS_FILE_DEST = 0x8027000B,  // Existing destination file with same name as folder
				COPYENGINE_E_FILE_IS_FLD_DEST = 0x8027000C,  // Existing destination folder with same name as file

				COPYENGINE_E_FILE_TOO_LARGE = 0x8027000D,  // File too large for destination file system
				COPYENGINE_E_REMOVABLE_FULL = 0x8027000E,  // Destination device is full and happens to be removable

				COPYENGINE_E_DEST_IS_RO_CD = 0x8027000F,  // Destination is a Read-Only CDRom, possibly unformatted
				COPYENGINE_E_DEST_IS_RW_CD = 0x80270010,  // Destination is a Read/Write CDRom, possibly unformatted
				COPYENGINE_E_DEST_IS_R_CD = 0x80270011,  // Destination is a Recordable (Audio, CDRom, possibly unformatted

				COPYENGINE_E_DEST_IS_RO_DVD = 0x80270012,  // Destination is a Read-Only DVD, possibly unformatted
				COPYENGINE_E_DEST_IS_RW_DVD = 0x80270013,  // Destination is a Read/Wrote DVD, possibly unformatted
				COPYENGINE_E_DEST_IS_R_DVD = 0x80270014,  // Destination is a Recordable (Audio, DVD, possibly unformatted

				COPYENGINE_E_SRC_IS_RO_CD = 0x80270015,  // Source is a Read-Only CDRom, possibly unformatted
				COPYENGINE_E_SRC_IS_RW_CD = 0x80270016,  // Source is a Read/Write CDRom, possibly unformatted
				COPYENGINE_E_SRC_IS_R_CD = 0x80270017,  // Source is a Recordable (Audio, CDRom, possibly unformatted

				COPYENGINE_E_SRC_IS_RO_DVD = 0x80270018,  // Source is a Read-Only DVD, possibly unformatted
				COPYENGINE_E_SRC_IS_RW_DVD = 0x80270019,  // Source is a Read/Wrote DVD, possibly unformatted
				COPYENGINE_E_SRC_IS_R_DVD = 0x8027001A,  // Source is a Recordable (Audio, DVD, possibly unformatted

				COPYENGINE_E_INVALID_FILES_SRC = 0x8027001B,  // Invalid source path
				COPYENGINE_E_INVALID_FILES_DEST = 0x8027001C,  // Invalid destination path
				COPYENGINE_E_PATH_TOO_DEEP_SRC = 0x8027001D,  // Source Files within folders where the overall path is longer than MAX_PATH
				COPYENGINE_E_PATH_TOO_DEEP_DEST = 0x8027001E,  // Destination files would be within folders where the overall path is longer than MAX_PATH
				COPYENGINE_E_ROOT_DIR_SRC = 0x8027001F,  // Source is a root directory, cannot be moved or renamed
				COPYENGINE_E_ROOT_DIR_DEST = 0x80270020,  // Destination is a root directory, cannot be renamed
				COPYENGINE_E_ACCESS_DENIED_SRC = 0x80270021,  // Security problem on source
				COPYENGINE_E_ACCESS_DENIED_DEST = 0x80270022,  // Security problem on destination
				COPYENGINE_E_PATH_NOT_FOUND_SRC = 0x80270023,  // Source file does not exist, or is unavailable
				COPYENGINE_E_PATH_NOT_FOUND_DEST = 0x80270024,  // Destination file does not exist, or is unavailable
				COPYENGINE_E_NET_DISCONNECT_SRC = 0x80270025,  // Source file is on a disconnected network location
				COPYENGINE_E_NET_DISCONNECT_DEST = 0x80270026,  // Destination file is on a disconnected network location
				COPYENGINE_E_SHARING_VIOLATION_SRC = 0x80270027,  // Sharing Violation on source
				COPYENGINE_E_SHARING_VIOLATION_DEST = 0x80270028,  // Sharing Violation on destination

				COPYENGINE_E_ALREADY_EXISTS_NORMAL = 0x80270029, // Destination exists, cannot replace
				COPYENGINE_E_ALREADY_EXISTS_READONLY = 0x8027002A, // Destination with read-only attribute exists, cannot replace
				COPYENGINE_E_ALREADY_EXISTS_SYSTEM = 0x8027002B, // Destination with system attribute exists, cannot replace
				COPYENGINE_E_ALREADY_EXISTS_FOLDER = 0x8027002C, // Destination folder exists, cannot replace
				COPYENGINE_E_STREAM_LOSS = 0x8027002D, // Secondary Stream information would be lost
				COPYENGINE_E_EA_LOSS = 0x8027002E, // Extended Attributes would be lost
				COPYENGINE_E_PROPERTY_LOSS = 0x8027002F, // Property would be lost
				COPYENGINE_E_PROPERTIES_LOSS = 0x80270030, // Properties would be lost
				COPYENGINE_E_ENCRYPTION_LOSS = 0x80270031, // Encryption would be lost
				COPYENGINE_E_DISK_FULL = 0x80270032, // Entire operation likely won't fit
				COPYENGINE_E_DISK_FULL_CLEAN = 0x80270033, // Entire operation likely won't fit, clean-up wizard available
				COPYENGINE_E_CANT_REACH_SOURCE = 0x80270035, // Can't reach source folder")

				COPYENGINE_E_RECYCLE_UNKNOWN_ERROR = 0x80270035, // ???
				COPYENGINE_E_RECYCLE_FORCE_NUKE = 0x80270036, // Recycling not available (usually turned off,
				COPYENGINE_E_RECYCLE_SIZE_TOO_BIG = 0x80270037, // Item is too large for the recycle-bin
				COPYENGINE_E_RECYCLE_PATH_TOO_LONG = 0x80270038, // Folder is too deep to fit in the recycle-bin
				COPYENGINE_E_RECYCLE_BIN_NOT_FOUND = 0x8027003A, // Recycle bin could not be found or is unavailable
				COPYENGINE_E_NEWFILE_NAME_TOO_LONG = 0x8027003B, // Name of the new file being created is too long
				COPYENGINE_E_NEWFOLDER_NAME_TOO_LONG = 0x8027003C, // Name of the new folder being created is too long
				COPYENGINE_E_DIR_NOT_EMPTY = 0x8027003D, // The directory being processed is not empty

				//  error codes without a more specific group use FACILITY_SHELL and 0x01 in the second lowest byte.
				NETCACHE_E_NEGATIVE_CACHE = 0x80270100, // The item requested is in the negative net parsing cache
				EXECUTE_E_LAUNCH_APPLICATION = 0x80270101, // for returned by command delegates to indicate that they did no work 
				SHELL_E_WRONG_BITDEPTH = 0x80270102, // returned when trying to create a thumbnail extractor at too low a bitdepth for high fidelity
			}

			[ComImport]
			[Guid("947aab5f-0a5c-4c13-b4d6-4bf7836fc9f8")]
			[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
			internal interface IFileOperation
			{
				uint Advise(IFileOperationProgressSink pfops);
				void Unadvise(uint dwCookie);
				void SetOperationFlags(FileOperationFlags dwOperationFlags);
				void SetProgressMessage([MarshalAs(UnmanagedType.LPWStr)] string pszMessage);
				void SetProgressDialog([MarshalAs(UnmanagedType.Interface)] object popd);
				void SetProperties([MarshalAs(UnmanagedType.Interface)] object pproparray);
				void SetOwnerWindow(uint hwndParent);
				void ApplyPropertiesToItem(IShellItem psiItem);
				void ApplyPropertiesToItems([MarshalAs(UnmanagedType.Interface)] object punkItems);
				void RenameItem(IShellItem psiItem, [MarshalAs(UnmanagedType.LPWStr)] string pszNewName, IFileOperationProgressSink pfopsItem);
				void RenameItems([MarshalAs(UnmanagedType.Interface)] object pUnkItems, [MarshalAs(UnmanagedType.LPWStr)] string pszNewName);
				void MoveItem(IShellItem psiItem, IShellItem psiDestinationFolder, [MarshalAs(UnmanagedType.LPWStr)] string pszNewName, IFileOperationProgressSink pfopsItem);
				void MoveItems([MarshalAs(UnmanagedType.Interface)] object punkItems, IShellItem psiDestinationFolder);
				void CopyItem(IShellItem psiItem, IShellItem psiDestinationFolder, [MarshalAs(UnmanagedType.LPWStr)] string pszCopyName, IFileOperationProgressSink pfopsItem);
				void CopyItems([MarshalAs(UnmanagedType.Interface)] object punkItems, IShellItem psiDestinationFolder);
				void DeleteItem(IShellItem psiItem, IFileOperationProgressSink pfopsItem);
				void DeleteItems([MarshalAs(UnmanagedType.Interface)] object punkItems);
				uint NewItem(IShellItem psiDestinationFolder, FileAttributes dwFileAttributes, [MarshalAs(UnmanagedType.LPWStr)] string pszName, [MarshalAs(UnmanagedType.LPWStr)] string pszTemplateName, IFileOperationProgressSink pfopsItem);
				void PerformOperations(); [return: MarshalAs(UnmanagedType.Bool)] bool GetAnyOperationsAborted();
			}

			[ComImport]
			[Guid("04b0f1a7-9490-44bc-96e1-4296a31252e2")]
			[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
			public interface IFileOperationProgressSink
			{
				void StartOperations();
				void FinishOperations(uint hrResult);
				void PreRenameItem(uint dwFlags, IShellItem psiItem, [MarshalAs(UnmanagedType.LPWStr)] string pszNewName);
				void PostRenameItem(uint dwFlags, IShellItem psiItem, [MarshalAs(UnmanagedType.LPWStr)] string pszNewName, uint hrRename, IShellItem psiNewlyCreated);
				void PreMoveItem(uint dwFlags, IShellItem psiItem, IShellItem psiDestinationFolder, [MarshalAs(UnmanagedType.LPWStr)] string pszNewName);
				void PostMoveItem(uint dwFlags, IShellItem psiItem, IShellItem psiDestinationFolder, [MarshalAs(UnmanagedType.LPWStr)] string pszNewName, uint hrMove, IShellItem psiNewlyCreated);
				void PreCopyItem(uint dwFlags, IShellItem psiItem, IShellItem psiDestinationFolder, [MarshalAs(UnmanagedType.LPWStr)] string pszNewName);
				void PostCopyItem(uint dwFlags, IShellItem psiItem, IShellItem psiDestinationFolder, [MarshalAs(UnmanagedType.LPWStr)] string pszNewName, uint hrCopy, IShellItem psiNewlyCreated);
				void PreDeleteItem(uint dwFlags, IShellItem psiItem);
				void PostDeleteItem(uint dwFlags, IShellItem psiItem, uint hrDelete, IShellItem psiNewlyCreated);
				void PreNewItem(uint dwFlags, IShellItem psiDestinationFolder, [MarshalAs(UnmanagedType.LPWStr)] string pszNewName);
				void PostNewItem(uint dwFlags, IShellItem psiDestinationFolder, [MarshalAs(UnmanagedType.LPWStr)] string pszNewName, [MarshalAs(UnmanagedType.LPWStr)] string pszTemplateName, uint dwFileAttributes, uint hrNew, IShellItem psiNewItem);
				void UpdateProgress(uint iWorkTotal, uint iWorkSoFar); void ResetTimer(); void PauseTimer(); void ResumeTimer();
			}

			[ComImport]
			[Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
			[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
			public interface IShellItem
			{
				[return: MarshalAs(UnmanagedType.Interface)]
				object BindToHandler(IBindCtx pbc, ref Guid bhid, ref Guid riid);

				IShellItem GetParent();

				[return: MarshalAs(UnmanagedType.LPWStr)]
				string GetDisplayName(SIGDN sigdnName);

				uint GetAttributes(uint sfgaoMask);

				int Compare(IShellItem psi, uint hint);
			}

			[DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
			public static extern void SHCreateItemFromParsingName(
				[MarshalAs(UnmanagedType.LPWStr)] string pszPath,
				IntPtr pbc,
				ref Guid riid,
				[MarshalAs(UnmanagedType.Interface)] out IShellItem ppv
			);

			//[DllImport("shell32.dll", SetLastError = true, CharSet = CharSet.Unicode, PreserveSig = false)]
			//[return: MarshalAs(UnmanagedType.Interface)]
			//public static extern object SHCreateItemFromParsingName(
			//	[MarshalAs(UnmanagedType.LPWStr)] string pszPath,
			//	IBindCtx pbc,
			//	ref Guid riid);
		}

		public IFileOperation() : this(null)
		{
		}

		public IFileOperation(FileOperationProgressSink callbackSink) : this(callbackSink, null, ShellFileDeleteOptions.None)
		{
		}

		public IFileOperation(FileOperationProgressSink callbackSink, IWin32Window window, ShellFileDeleteOptions options) : base(options)
		{
			_callbackSink = callbackSink;
			_fileOperation = (Native.IFileOperation)Activator.CreateInstance(_fileOperationType);
			_fileOperation.SetOperationFlags(FileOperationFlags.FOF_NOCONFIRMMKDIR);

			if (_callbackSink != null)
				_sinkCookie = _fileOperation.Advise(_callbackSink);

			IntPtr ownerHandle = window?.Handle ?? IntPtr.Zero;
			if (ownerHandle != IntPtr.Zero)
				_fileOperation.SetOwnerWindow((uint)ownerHandle);
		}

		private class ComReleaser<T> : IDisposable where T : class
		{
			private T _obj;

			public ComReleaser(T obj)
			{
				if (obj == null)
					throw new ArgumentNullException("obj");

				if (!obj.GetType().IsCOMObject)
					throw new ArgumentOutOfRangeException("obj");

				_obj = obj;
			}

			public T Item { get { return _obj; } }

			public void Dispose()
			{
				if (_obj != null)
				{
					Marshal.FinalReleaseComObject(_obj);
					_obj = null;
				}
			}
		}

		private static ComReleaser<Native.IShellItem> CreateShellItem(string path)
		{
			Native.SHCreateItemFromParsingName(path, IntPtr.Zero, ref _shellItemGuid, out Native.IShellItem item);
			return new ComReleaser<Native.IShellItem>(item);
		}

		public class FileOperationProgressSink : Native.IFileOperationProgressSink
		{
			public virtual void StartOperations()
			{
				TraceAction("StartOperations", "", 0);
			}

			public virtual void FinishOperations(uint hrResult)
			{
				TraceAction("FinishOperations", "", hrResult);
			}

			public virtual void PreRenameItem(uint dwFlags,
				Native.IShellItem psiItem, string pszNewName)
			{
				TraceAction("PreRenameItem", psiItem, 0);
			}

			public virtual void PostRenameItem(uint dwFlags,
				Native.IShellItem psiItem, string pszNewName,
				uint hrRename, Native.IShellItem psiNewlyCreated)
			{
				TraceAction("PostRenameItem", psiNewlyCreated, hrRename);
			}

			public virtual void PreMoveItem(
				uint dwFlags, Native.IShellItem psiItem,
				Native.IShellItem psiDestinationFolder, string pszNewName)
			{
				TraceAction("PreMoveItem", psiItem, 0);
			}

			public virtual void PostMoveItem(
				uint dwFlags, Native.IShellItem psiItem,
				Native.IShellItem psiDestinationFolder,
				string pszNewName, uint hrMove,
				Native.IShellItem psiNewlyCreated)
			{
				TraceAction("PostMoveItem", psiNewlyCreated, hrMove);
			}

			public virtual void PreCopyItem(
				uint dwFlags, Native.IShellItem psiItem,
				Native.IShellItem psiDestinationFolder, string pszNewName)
			{
				TraceAction("PreCopyItem", psiItem, 0);
			}

			public virtual void PostCopyItem(
				uint dwFlags, Native.IShellItem psiItem,
				Native.IShellItem psiDestinationFolder, string pszNewName,
				uint hrCopy, Native.IShellItem psiNewlyCreated)
			{
				TraceAction("PostCopyItem", psiNewlyCreated, hrCopy);
			}

			public virtual void PreDeleteItem(
				uint dwFlags, Native.IShellItem psiItem)
			{
				TraceAction("PreDeleteItem", psiItem, 0);
			}

			public virtual void PostDeleteItem(
				uint dwFlags, Native.IShellItem psiItem,
				uint hrDelete, Native.IShellItem psiNewlyCreated)
			{
				TraceAction("PostDeleteItem", psiItem, hrDelete);
			}

			public virtual void PreNewItem(uint dwFlags,
				Native.IShellItem psiDestinationFolder, string pszNewName)
			{
				TraceAction("PreNewItem", pszNewName, 0);
			}

			public virtual void PostNewItem(uint dwFlags,
				Native.IShellItem psiDestinationFolder, string pszNewName,
				string pszTemplateName, uint dwFileAttributes,
				uint hrNew, Native.IShellItem psiNewItem)
			{
				TraceAction("PostNewItem", psiNewItem, hrNew);
			}

			public virtual void UpdateProgress(
				uint iWorkTotal, uint iWorkSoFar)
			{
				Debug.WriteLine("UpdateProgress: " + iWorkSoFar + "/" + iWorkTotal);
			}

			public void ResetTimer() { }
			public void PauseTimer() { }
			public void ResumeTimer() { }

			[Conditional("DEBUG")]
			private static void TraceAction(string action, string item, uint hresult)
			{
				string message = $"{action} ({(Native.CopyEngineResult)hresult})";
				if (!string.IsNullOrEmpty(item)) 
					message += " : " + item;

				Debug.WriteLine(message);
			}

			[Conditional("DEBUG")]
			private static void TraceAction(
				string action, Native.IShellItem item, uint hresult)
			{
				TraceAction(action,
					item != null ? item.GetDisplayName(Native.SIGDN.SIGDN_NORMALDISPLAY) : null,
					hresult);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_callbackSink != null) _fileOperation.Unadvise(_sinkCookie);
				Marshal.FinalReleaseComObject(_fileOperation);
			}
			base.Dispose(disposing);
		}

		private void ThrowIfDisposed()
		{
			if (IsDisposed) throw new ObjectDisposedException(GetType().Name);
		}

		protected override void VerifyFile(string file)
		{
			ThrowIfDisposed();
			base.VerifyFile(file);
		}

		public override void DeleteFile(string file)
		{
			VerifyFile(file);
			using (ComReleaser<Native.IShellItem> item = CreateShellItem(file))
			{
				_fileOperation.SetOperationFlags(GetDeleteFileFlags());
				_fileOperation.DeleteItem(item.Item, null);
				_fileOperation.PerformOperations();
			}
		}
	}
}
