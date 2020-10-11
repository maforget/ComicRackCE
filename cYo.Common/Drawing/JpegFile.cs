using System;
using System.Drawing;
using System.IO;
using cYo.Common.IO;

namespace cYo.Common.Drawing
{
	public class JpegFile
	{
		public bool IsValid
		{
			get;
			private set;
		}

		public Size Size
		{
			get;
			private set;
		}

		public int Width => Size.Width;

		public int Height => Size.Height;

		public JpegFile(Stream s)
		{
			Initialize(s);
		}

		public JpegFile(byte[] data)
		{
			if (data != null && data.Length >= 2 && data[0] == byte.MaxValue && data[1] == 216)
			{
				using (MemoryStream s = new MemoryStream(data))
				{
					Initialize(s);
				}
			}
		}

		private void Initialize(Stream s)
		{
			if (IsValid = GetImageSize(s, out var size))
			{
				Size = size;
			}
		}

		public static bool GetImageSize(Stream s, out Size size)
		{
			size = Size.Empty;
			try
			{
				using (BinaryReader binaryReader = new BinaryReader(s))
				{
					if (binaryReader.ReadByte() != byte.MaxValue || binaryReader.ReadByte() != 216)
					{
						return false;
					}
					while (true)
					{
						byte b;
						if ((b = binaryReader.ReadByte()) != byte.MaxValue)
						{
							continue;
						}
						while (true)
						{
							switch (b)
							{
							case byte.MaxValue:
								break;
							case 192:
							case 193:
							case 194:
							case 195:
							{
								binaryReader.BaseStream.Seek(3L, SeekOrigin.Current);
								int height = binaryReader.ReadUInt16BigEndian();
								int width = binaryReader.ReadUInt16BigEndian();
								size = new Size(width, height);
								goto end_IL_0012;
							}
							default:
								goto IL_0091;
							}
							b = binaryReader.ReadByte();
							continue;
							IL_0091:
							binaryReader.BaseStream.Seek(binaryReader.ReadUInt16BigEndian() - 2, SeekOrigin.Current);
							break;
						}
					}
					end_IL_0012:;
				}
				return !size.IsEmpty;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static bool RemoveExif(Stream inStream, Stream outStream)
		{
			byte[] array = new byte[2];
			inStream.Read(array, 0, array.Length);
			if (array[0] != byte.MaxValue || array[1] != 216)
			{
				return false;
			}
			outStream.WriteByte(byte.MaxValue);
			outStream.WriteByte(216);
			inStream.Read(array, 0, array.Length);
			while (array[0] == byte.MaxValue && array[1] >= 224 && array[1] <= 239)
			{
				int num = inStream.ReadByte();
				num <<= 8;
				num |= inStream.ReadByte();
				inStream.Position += num - 2;
				inStream.Read(array, 0, array.Length);
			}
			inStream.Position -= 2L;
			inStream.CopyTo(outStream);
			return true;
		}
	}
}
