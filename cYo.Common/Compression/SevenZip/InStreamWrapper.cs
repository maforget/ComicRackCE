using System.IO;

namespace cYo.Common.Compression.SevenZip
{
	public class InStreamWrapper : StreamWrapper, ISequentialInStream, IInStream
	{
		public InStreamWrapper(Stream baseStream)
			: base(baseStream)
		{
		}

		public int Read(byte[] data, int size)
		{
			return base.BaseStream.Read(data, 0, size);
		}

		public long GetSize()
		{
			return base.BaseStream.Length;
		}
	}
}
