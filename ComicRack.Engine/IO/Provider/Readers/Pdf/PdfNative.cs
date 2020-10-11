using System;
using System.Collections.Generic;
using System.IO;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Pdf
{
	public class PdfNative : IComicAccessor
	{
		private class ImageStreamInfo : ProviderImageInfo
		{
			private readonly long offset;

			public long Offset => offset;

			public ImageStreamInfo(string name, long size, long offset)
				: base(0, name, size)
			{
				this.offset = offset;
			}

			public ImageStreamInfo(long size, long offset)
				: this(null, size, offset)
			{
			}
		}

		private class Match
		{
			private int matchPos;

			private readonly char[] tag;

			public int Length => tag.Length;

			public Match(string tag)
			{
				this.tag = tag.ToCharArray();
			}

			public bool IsMatch(int b)
			{
				if (tag[matchPos] == b)
				{
					matchPos++;
				}
				else if (matchPos != 0)
				{
					matchPos = 0;
					if (tag[matchPos] == b)
					{
						matchPos++;
					}
				}
				if (matchPos == tag.Length)
				{
					matchPos = 0;
					return true;
				}
				return false;
			}
		}

		private class Reader
		{
			private byte[] readBuffer = new byte[100000];

			private long bufferOffset;

			private int bufferLen;

			private int bufferPos;

			private Stream readStream;

			public Reader(Stream readStream)
			{
				this.readStream = readStream;
			}

			public long GetPosition()
			{
				return bufferOffset + bufferPos;
			}

			public int ReadByte()
			{
				if (bufferPos >= bufferLen)
				{
					bufferOffset += bufferLen;
					bufferLen = readStream.Read(readBuffer, 0, readBuffer.Length);
					bufferPos = 0;
					if (bufferLen == 0)
					{
						return -1;
					}
				}
				return readBuffer[bufferPos++];
			}
		}

		public bool IsFormat(string source)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<ProviderImageInfo> GetEntryList(string source)
		{
			Match streamStartTag = new Match("stream");
			Match streamEndTag = new Match("endstream");
			using (FileStream readStream = File.OpenRead(source))
			{
				Reader reader = new Reader(readStream);
				int b3 = 0;
				while (b3 != -1)
				{
					int num;
					do
					{
						b3 = (num = reader.ReadByte());
					}
					while (num != -1 && !streamStartTag.IsMatch(b3));
					b3 = reader.ReadByte();
					while (true)
					{
						switch (b3)
						{
						case 10:
						case 13:
							goto IL_00b4;
						}
						break;
						IL_00b4:
						b3 = reader.ReadByte();
					}
					long num2 = reader.GetPosition() - 1;
					if (b3 != 255 || reader.ReadByte() != 216)
					{
						b3 = reader.ReadByte();
						continue;
					}
					do
					{
						b3 = (num = reader.ReadByte());
					}
					while (num != -1 && !streamEndTag.IsMatch(b3));
					long size = reader.GetPosition() - num2 - streamEndTag.Length - 1;
					yield return new ImageStreamInfo(size, num2);
				}
			}
		}

		public byte[] ReadByteImage(string source, ProviderImageInfo info)
		{
			return LoadBitmapData(source, (ImageStreamInfo)info);
		}

		public ComicInfo ReadInfo(string source)
		{
			return null;
		}

		public bool WriteInfo(string source, ComicInfo info)
		{
			return false;
		}

		private static byte[] LoadBitmapData(string file, ImageStreamInfo si)
		{
			try
			{
				byte[] array = new byte[si.Size];
				using (FileStream fileStream = File.OpenRead(file))
				{
					fileStream.Seek(si.Offset, SeekOrigin.Begin);
					if (fileStream.Read(array, 0, array.Length) != array.Length)
					{
						return null;
					}
					return array;
				}
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
