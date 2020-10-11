using System;
using System.IO;

namespace cYo.Common.IO
{
	public class ProgressStream : Stream
	{
		private readonly Stream baseStream;

		public bool BaseStreamOwned
		{
			get;
			set;
		}

		public Stream BaseStream => baseStream;

		public override bool CanRead => baseStream.CanRead;

		public override bool CanSeek => baseStream.CanSeek;

		public override bool CanWrite => baseStream.CanWrite;

		public override long Length => baseStream.Length;

		public override long Position
		{
			get
			{
				return baseStream.Position;
			}
			set
			{
				baseStream.Position = value;
			}
		}

		public event EventHandler<ProgressStreamReadEventArgs> DataRead;

		public ProgressStream(Stream baseStream, bool baseStreamOwned = true)
		{
			this.baseStream = baseStream;
			BaseStreamOwned = baseStreamOwned;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && BaseStreamOwned)
			{
				baseStream.Dispose();
			}
			base.Dispose(disposing);
		}

		public override void Flush()
		{
			baseStream.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			count = baseStream.Read(buffer, offset, count);
			OnDataRead(count);
			return count;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return baseStream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			baseStream.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			baseStream.Write(buffer, offset, count);
		}

		protected virtual void OnDataRead(int count)
		{
			if (this.DataRead != null)
			{
				try
				{
					this.DataRead(this, new ProgressStreamReadEventArgs(count));
				}
				catch
				{
				}
			}
		}
	}
}
