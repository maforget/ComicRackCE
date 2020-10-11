using System.IO;

namespace cYo.Common.IO
{
	public static class BinaryReaderExtension
	{
		public static int ReadUInt16BigEndian(this BinaryReader br)
		{
			return (ushort)(br.ReadByte() << 8) | br.ReadByte();
		}
	}
}
