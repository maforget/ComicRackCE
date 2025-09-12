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
			bool flag;
			try
			{
				using BinaryReader binaryReader = new BinaryReader(s);

				// Validate JPEG SOI marker
				if (binaryReader.ReadByte() != byte.MaxValue || binaryReader.ReadByte() != 0xD8)
				{
					return false;
				}
				while (true)
				{
					byte marker;
					do
					{
						marker = binaryReader.ReadByte();
					} while (marker == byte.MaxValue);

					// Check for start of frame (SOF) markers that contain image dimensions
					if (marker >= 0xC0 && marker <= 0xC3)
					{
						binaryReader.BaseStream.Seek(3, SeekOrigin.Current);
						int height = binaryReader.ReadUInt16BigEndian();
						int width = binaryReader.ReadUInt16BigEndian();
						size = new Size(width, height);
						return true;
					}

					// Skip unnecessary segments
					int segmentLength = binaryReader.ReadUInt16BigEndian() - 2;
					binaryReader.BaseStream.Seek(segmentLength, SeekOrigin.Current);
				}
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
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
