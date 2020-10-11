using System;
using System.IO;
using System.Runtime.InteropServices;

namespace cYo.Common.Compression.SevenZip
{
	public class OutStreamWrapper : StreamWrapper, ISequentialOutStream, IOutStream
	{
		public OutStreamWrapper(Stream baseStream)
			: base(baseStream)
		{
		}

		public int SetSize(long newSize)
		{
			base.BaseStream.SetLength(newSize);
			return 0;
		}

		public int Write(byte[] data, int size, IntPtr processedSize)
		{
			base.BaseStream.Write(data, 0, size);
			if (processedSize != IntPtr.Zero)
			{
				Marshal.WriteInt32(processedSize, size);
			}
			return 0;
		}
	}
}
