using System;
using System.IO;

namespace cYo.Common.IO
{
	public class XorStream : Stream
	{
		private int mask;

		private Stream stream;

		public override bool CanRead => stream.CanRead;

		public override bool CanSeek => stream.CanSeek;

		public override bool CanWrite => false;

		public override long Length => stream.Length;

		public override long Position
		{
			get
			{
				return stream.Position;
			}
			set
			{
				stream.Position = value;
			}
		}

		public XorStream(Stream stream, int mask)
		{
			this.stream = stream;
			this.mask = mask;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && stream != null)
			{
				stream.Dispose();
				stream = null;
			}
			base.Dispose(disposing);
		}

		public override void Close()
		{
			base.Close();
			stream.Close();
		}

		public override void Flush()
		{
			stream.Flush();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return stream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			stream.SetLength(value);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = stream.Read(buffer, offset, count);
			for (int i = 0; i < num; i++)
			{
				buffer[offset + i] ^= (byte)mask;
			}
			return num;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}
	}
}
