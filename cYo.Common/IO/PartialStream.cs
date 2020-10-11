using System;
using System.IO;

namespace cYo.Common.IO
{
	public class PartialStream : Stream
	{
		private readonly long length;

		private readonly long start;

		private readonly Stream baseStream;

		public override bool CanRead => baseStream.CanRead;

		public override bool CanSeek => baseStream.CanSeek;

		public override bool CanWrite => false;

		public override long Length => length;

		public override long Position
		{
			get
			{
				return baseStream.Position - start;
			}
			set
			{
				baseStream.Position = value + start;
			}
		}

		public PartialStream(Stream baseStream, long start, long length)
		{
			if (baseStream == null)
			{
				throw new ArgumentNullException("baseStream");
			}
			this.baseStream = baseStream;
			this.length = length;
			if (start >= 0)
			{
				baseStream.Seek(start, SeekOrigin.Begin);
			}
			this.start = baseStream.Position;
		}

		public PartialStream(string file, long start, long length)
			: this(File.OpenRead(file), start, length)
		{
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && baseStream != null)
			{
				baseStream.Dispose();
			}
			base.Dispose(disposing);
		}

		public override void Close()
		{
			if (baseStream != null)
			{
				baseStream.Close();
			}
			base.Close();
		}

		public override void Flush()
		{
			throw new IOException("Can not write");
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return baseStream.Read(buffer, offset, Math.Min(count, (int)(length - Position)));
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			long num = 0L;
			switch (origin)
			{
			case SeekOrigin.Begin:
				num = offset;
				break;
			case SeekOrigin.Current:
				num = Position + offset;
				break;
			case SeekOrigin.End:
				num = length + offset;
				break;
			}
			return baseStream.Seek(start + num, SeekOrigin.Begin) - start;
		}

		public override void SetLength(long value)
		{
			throw new Exception("This stream does not support this action");
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new Exception("This stream does not support this action");
		}
	}
}
