using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace cYo.Common.Compression.SevenZip
{
	public class InStreamTimedWrapper : StreamWrapper, ISequentialInStream, IInStream
	{
		private const int KeepAliveInterval = 5000;

		private long baseStreamLastPosition;

		private Timer closeTimer;

		private string baseStreamFileName;

		public string BaseStreamFileName => baseStreamFileName;

		public InStreamTimedWrapper(Stream baseStream)
			: base(baseStream)
		{
			FileStream fileStream = baseStream as FileStream;
			if (fileStream != null && !baseStream.CanWrite && baseStream.CanSeek)
			{
				baseStreamFileName = fileStream.Name;
				closeTimer = new Timer(CloseStream, null, 5000, -1);
			}
		}

		protected override void Dispose(bool disposing)
		{
			CloseStream(null);
			baseStreamFileName = null;
		}

		private void CloseStream(object state)
		{
			if (closeTimer != null)
			{
				closeTimer.Dispose();
				closeTimer = null;
			}
			if (base.BaseStream != null)
			{
				if (base.BaseStream.CanSeek)
				{
					baseStreamLastPosition = base.BaseStream.Position;
				}
				base.BaseStream.Close();
				base.BaseStream = null;
			}
		}

		private void ReopenStream()
		{
			if (base.BaseStream == null || !base.BaseStream.CanRead)
			{
				if (baseStreamFileName == null)
				{
					throw new ObjectDisposedException("StreamWrapper");
				}
				base.BaseStream = new FileStream(baseStreamFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
				base.BaseStream.Position = baseStreamLastPosition;
				closeTimer = new Timer(CloseStream, null, KeepAliveInterval, -1);
			}
			else if (closeTimer != null)
			{
				closeTimer.Change(KeepAliveInterval, -1);
			}
		}

		public void Flush()
		{
			CloseStream(null);
		}

		public int Read(byte[] data, int size)
		{
			ReopenStream();
			return base.BaseStream.Read(data, 0, size);
		}

		public override void Seek(long offset, int seekOrigin, IntPtr newPosition)
		{
			if (base.BaseStream == null && baseStreamFileName != null && offset == 0L && seekOrigin == 0)
			{
				baseStreamLastPosition = 0L;
				if (newPosition != IntPtr.Zero)
				{
					Marshal.WriteInt64(newPosition, baseStreamLastPosition);
				}
			}
			else
			{
				ReopenStream();
				base.Seek(offset, seekOrigin, newPosition);
			}
		}
	}
}
