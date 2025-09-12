using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Permissions;
using System.Windows.Forms;

namespace cYo.Common.Win32
{
	public class DataObjectEx : DataObject, System.Runtime.InteropServices.ComTypes.IDataObject
	{
		private static class NativeMethods
		{
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
			public struct FILEDESCRIPTOR
			{
				public uint dwFlags;

				public Guid clsid;

				public Size sizel;

				public Point pointl;

				public uint dwFileAttributes;

				public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;

				public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;

				public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;

				public uint nFileSizeHigh;

				public uint nFileSizeLow;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
				public string cFileName;
			}

			public const string CFSTR_PREFERREDDROPEFFECT = "Preferred DropEffect";

			public const string CFSTR_PERFORMEDDROPEFFECT = "Performed DropEffect";

			public const string CFSTR_FILEDESCRIPTORW = "FileGroupDescriptorW";

			public const string CFSTR_FILECONTENTS = "FileContents";

			public const int FD_CLSID = 1;

			public const int FD_SIZEPOINT = 2;

			public const int FD_ATTRIBUTES = 4;

			public const int FD_CREATETIME = 8;

			public const int FD_ACCESSTIME = 16;

			public const int FD_WRITESTIME = 32;

			public const int FD_FILESIZE = 64;

			public const int FD_PROGRESSUI = 16384;

			public const int FD_LINKUI = 32768;

			public const int GMEM_MOVEABLE = 2;

			public const int GMEM_ZEROINIT = 64;

			public const int GHND = 66;

			public const int GMEM_DDESHARE = 8192;

			public const int DV_E_TYMED = -2147221399;

			[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
			public static extern IntPtr GlobalAlloc(int uFlags, int dwBytes);

			[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
			public static extern IntPtr GlobalFree(HandleRef handle);
		}

		public class VirtualFileItemWriteEventArgs : EventArgs
		{
			private readonly Stream stream;

			public Stream Stream => stream;

			public VirtualFileItemWriteEventArgs(Stream stream)
			{
				this.stream = stream;
			}
		}

		public class VirtualFileItem
		{
			private long fileSize = -1L;

			public string FileName
			{
				get;
				set;
			}

			public long FileSize
			{
				get
				{
					return fileSize;
				}
				set
				{
					fileSize = value;
				}
			}

			public DateTime WriteTime
			{
				get;
				set;
			}

			public object Tag
			{
				get;
				set;
			}

			public event EventHandler<VirtualFileItemWriteEventArgs> WriteData;

			public VirtualFileItem(string fileName, long fileSize, DateTime writeTime)
			{
				FileName = fileName;
				FileSize = fileSize;
				WriteTime = writeTime;
			}

			public VirtualFileItem(string fileName)
				: this(fileName, -1L, DateTime.Now)
			{
			}

			public void Write(Stream s)
			{
				OnWriteData(new VirtualFileItemWriteEventArgs(s));
			}

			protected virtual void OnWriteData(VirtualFileItemWriteEventArgs e)
			{
				if (this.WriteData != null)
				{
					this.WriteData(this, e);
				}
			}
		}

		private VirtualFileItem currentVirtualFileItem;

		private readonly List<VirtualFileItem> virtualFiles = new List<VirtualFileItem>();

		private static readonly TYMED[] usableTymeds = new TYMED[5]
		{
			TYMED.TYMED_HGLOBAL,
			TYMED.TYMED_ISTREAM,
			TYMED.TYMED_ENHMF,
			TYMED.TYMED_MFPICT,
			TYMED.TYMED_GDI
		};

		public void SetFile(VirtualFileItem vfi)
		{
			VirtualFileItem virtualFileItem = virtualFiles.Find((VirtualFileItem v) => string.Equals(vfi.FileName, v.FileName, StringComparison.OrdinalIgnoreCase));
			if (virtualFileItem != null)
			{
				virtualFiles.Remove(virtualFileItem);
			}
			virtualFiles.Add(vfi);
			SetData(NativeMethods.CFSTR_FILEDESCRIPTORW, null);
			SetData(NativeMethods.CFSTR_FILECONTENTS, null);
			SetData(NativeMethods.CFSTR_PERFORMEDDROPEFFECT, null);
		}

		public void SetFile(string fileName, Action<Stream> handler)
		{
			VirtualFileItem virtualFileItem = new VirtualFileItem(fileName);
			virtualFileItem.WriteData += delegate(object sender, VirtualFileItemWriteEventArgs e)
			{
				handler(e.Stream);
			};
			SetFile(virtualFileItem);
		}

		public override object GetData(string format, bool autoConvert)
		{
			switch (format)
			{
			case NativeMethods.CFSTR_FILEDESCRIPTORW:
				if (virtualFiles != null)
				{
					base.SetData(NativeMethods.CFSTR_FILEDESCRIPTORW, GetVirtualFilesDescriptor(virtualFiles));
				}
				break;
			case NativeMethods.CFSTR_FILECONTENTS:
				base.SetData(NativeMethods.CFSTR_FILECONTENTS, GetFileContents(currentVirtualFileItem));
				break;
			}
			return base.GetData(format, autoConvert);
		}

		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		void System.Runtime.InteropServices.ComTypes.IDataObject.GetData(ref FORMATETC formatetc, out STGMEDIUM medium)
		{
			if (!GetTymedUseable(formatetc.tymed))
			{
				Marshal.ThrowExceptionForHR(NativeMethods.DV_E_TYMED);
			}
			if (formatetc.cfFormat == (short)DataFormats.GetFormat(NativeMethods.CFSTR_FILECONTENTS).Id)
			{
				try
				{
					currentVirtualFileItem = virtualFiles[formatetc.lindex];
				}
				catch (IndexOutOfRangeException)
				{
					currentVirtualFileItem = null;
				}
			}
			medium = default(STGMEDIUM);
			if ((formatetc.tymed & TYMED.TYMED_HGLOBAL) != 0)
			{
				medium.tymed = TYMED.TYMED_HGLOBAL;
				medium.unionmember = NativeMethods.GlobalAlloc(NativeMethods.GMEM_MOVEABLE | NativeMethods.GMEM_ZEROINIT | NativeMethods.GMEM_DDESHARE, 1);
				if (medium.unionmember == IntPtr.Zero)
				{
					throw new OutOfMemoryException();
				}
				try
				{
					((System.Runtime.InteropServices.ComTypes.IDataObject)this).GetDataHere(ref formatetc, ref medium);
				}
				catch
				{
					NativeMethods.GlobalFree(new HandleRef(medium, medium.unionmember));
					medium.unionmember = IntPtr.Zero;
					throw;
				}
			}
			else
			{
				medium.tymed = formatetc.tymed;
				((System.Runtime.InteropServices.ComTypes.IDataObject)this).GetDataHere(ref formatetc, ref medium);
			}
		}

		private static MemoryStream GetVirtualFilesDescriptor(ICollection<VirtualFileItem> virtualFileItems)
		{
			MemoryStream memoryStream = new MemoryStream();
			memoryStream.Write(BitConverter.GetBytes(virtualFileItems.Count), 0, 4);
			foreach (VirtualFileItem virtualFileItem in virtualFileItems)
			{
				NativeMethods.FILEDESCRIPTOR fILEDESCRIPTOR = default(NativeMethods.FILEDESCRIPTOR);
				long num = virtualFileItem.WriteTime.ToFileTimeUtc();
				fILEDESCRIPTOR.cFileName = virtualFileItem.FileName;
				fILEDESCRIPTOR.ftLastWriteTime.dwHighDateTime = (int)(num >> 32);
				fILEDESCRIPTOR.ftLastWriteTime.dwLowDateTime = (int)(num & 0xFFFFFFFFu);
				fILEDESCRIPTOR.dwFlags = 16416u;
				if (virtualFileItem.FileSize != -1)
				{
					fILEDESCRIPTOR.nFileSizeHigh = (uint)(virtualFileItem.FileSize >> 32);
					fILEDESCRIPTOR.nFileSizeLow = (uint)(virtualFileItem.FileSize & 0xFFFFFFFFu);
					fILEDESCRIPTOR.dwFlags |= 64u;
				}
				int num2 = Marshal.SizeOf((object)fILEDESCRIPTOR);
				IntPtr intPtr = Marshal.AllocHGlobal(num2);
				byte[] array = new byte[num2];
				try
				{
					Marshal.StructureToPtr((object)fILEDESCRIPTOR, intPtr, fDeleteOld: true);
					Marshal.Copy(intPtr, array, 0, num2);
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
				}
				memoryStream.Write(array, 0, array.Length);
			}
			return memoryStream;
		}

		private MemoryStream GetFileContents(VirtualFileItem vfi)
		{
			if (vfi == null)
			{
				return null;
			}
			MemoryStream memoryStream = new MemoryStream();
			OnWriteVirtualFile(memoryStream, vfi);
			if (memoryStream.Length == 0L)
			{
				memoryStream.WriteByte(0);
			}
			return memoryStream;
		}

		protected virtual void OnWriteVirtualFile(Stream s, VirtualFileItem vfi)
		{
			vfi.Write(s);
		}

		private static bool GetTymedUseable(TYMED tymed)
		{
			return usableTymeds.Any((TYMED tm) => (tymed & tm) != 0);
		}
	}
}
