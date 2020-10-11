using System;
using System.IO;
using System.Runtime.InteropServices;
using cYo.Common.ComponentModel;

namespace cYo.Common.Compression.SevenZip
{
	public class StreamWrapper : DisposableObject
	{
		private Stream baseStream;

		public Stream BaseStream
		{
			get
			{
				return baseStream;
			}
			protected set
			{
				baseStream = value;
			}
		}

		protected StreamWrapper(Stream baseStream)
		{
			this.baseStream = baseStream;
		}

		protected override void Dispose(bool disposing)
		{
			if (baseStream != null)
			{
				baseStream.Dispose();
			}
		}

		public virtual void Seek(long offset, int seekOrigin, IntPtr newPosition)
		{
			long val = baseStream.Seek(offset, (SeekOrigin)seekOrigin);
			if (newPosition != IntPtr.Zero)
			{
				Marshal.WriteInt64(newPosition, val);
			}
		}
	}
}
