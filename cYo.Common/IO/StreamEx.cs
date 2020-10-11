using System;
using System.IO;

namespace cYo.Common.IO
{
	public class StreamEx : Stream
	{
		private Stream baseStream;

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

		public event EventHandler Closed;

		public StreamEx(Stream baseStream)
		{
			this.baseStream = baseStream;
		}

		public override void Close()
		{
			base.Close();
			OnClosed();
		}

		protected virtual void OnClosed()
		{
			if (this.Closed != null)
			{
				this.Closed(this, EventArgs.Empty);
			}
		}

		public override void Flush()
		{
			baseStream.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return baseStream.Read(buffer, offset, count);
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
	}
}
