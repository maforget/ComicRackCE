using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;

namespace cYo.Projects.ComicRack.Engine.IO
{
	public class ThumbnailImage : MemoryOptimizedImage, IDataSize
	{
		public const int MaxHeight = 512;

		private int currentHeight;

		public static bool MemoryOptimized = true;

		public static int SecondsToKeepDecodedImage = 15;

		private static int lastRequestHeight;

		private volatile int dataSize;

		public static BitmapResampling Resampling = EngineConfiguration.Default.ThumbnailResampling;

		public static int ThumbnailQuality = EngineConfiguration.Default.ThumbnailQuality;

		public Size OriginalSize
		{
			get;
			set;
		}

		public override Bitmap Bitmap
		{
			get
			{
				return GetThumbnail(MaxHeight);
			}
			set
			{
				base.Bitmap = value;
				if (value == null)
				{
					dataSize = base.Data.Length;
				}
			}
		}

		public int DataSize => dataSize;

		public ThumbnailImage(byte[] data, Size size, Size originalSize)
			: base(data, size)
		{
			OriginalSize = originalSize;
			base.TimeToStay = SecondsToKeepDecodedImage;
			base.Optimized = MemoryOptimized;
		}

		public Bitmap GetThumbnail(int height)
		{
			if (base.Data == null)
			{
				return null;
			}
			float num = 512f;
			while (num * 0.75f >= (float)height)
			{
				num *= 0.75f;
			}
			int num2 = (int)num;
			Bitmap bitmap = base.Bitmap;
			if (num2 == currentHeight && bitmap != null)
			{
				return bitmap;
			}
			using (Bitmap bitmap2 = BitmapExtensions.BitmapFromBytes(base.Data))
			{
				Size size = new Size(bitmap2.Width * num2 / bitmap2.Height, num2);
				bitmap = bitmap2.Scale(size, EngineConfiguration.Default.ThumbnailResampling).ToOptimized();
			}
			lastRequestHeight = (currentHeight = num2);
			return base.Bitmap = bitmap;
		}

		protected override Bitmap OnCreateImage(byte[] data)
		{
			return null;
		}

		public Size GetThumbnailSize(int height)
		{
			return Size.ToRectangle(new Size(0, height)).Size;
		}

		public override void Save(Stream s)
		{
			BinaryWriter binaryWriter = new BinaryWriter(s);
			binaryWriter.Write(Bitmap.Size.Width);
			binaryWriter.Write(Bitmap.Size.Height);
			binaryWriter.Write(OriginalSize.Width);
			binaryWriter.Write(OriginalSize.Height);
			binaryWriter.Write(base.Data.Length);
			s.Write(base.Data, 0, base.Data.Length);
		}

		public override byte[] ToBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream(base.Data.Length + 100))
			{
				Save(memoryStream);
				return memoryStream.ToArray();
			}
		}

		public static ThumbnailImage CreateFrom(string file)
		{
			using (FileStream stream = File.OpenRead(file))
			{
				return CreateFrom(stream);
			}
		}

		public static ThumbnailImage CreateFrom(byte[] data)
		{
			using (MemoryStream stream = new MemoryStream(data))
			{
				return CreateFrom(stream);
			}
		}

		public static ThumbnailImage CreateFrom(Stream stream)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			Size size = new Size(binaryReader.ReadInt32(), binaryReader.ReadInt32());
			Size originalSize = new Size(binaryReader.ReadInt32(), binaryReader.ReadInt32());
			int count = binaryReader.ReadInt32();
			ThumbnailImage thumbnailImage = new ThumbnailImage(binaryReader.ReadBytes(count), size, originalSize);
			if (lastRequestHeight != 0)
			{
				thumbnailImage.GetThumbnail(lastRequestHeight);
			}
			return thumbnailImage;
		}

		public static ThumbnailImage CreateFrom(Bitmap image, Size originalSize, bool supportTransparent = false)
		{
			if (image == null)
			{
				return null;
			}
			ThumbnailImage thumbnailImage;
			using (Image image2 = Scale(image, new Size(0, MaxHeight)))
			{
				thumbnailImage = ((!supportTransparent) ? new ThumbnailImage(image2.ImageToJpegBytes(ThumbnailQuality), image2.Size, originalSize) : new ThumbnailImage(image2.ImageToBytes(ImageFormat.Png), image2.Size, originalSize));
			}
			if (lastRequestHeight != 0)
			{
				thumbnailImage.GetThumbnail(lastRequestHeight);
			}
			return thumbnailImage;
		}

		private static Bitmap Scale(Bitmap bitmap, Size size)
		{
			return bitmap.Scale(size, Resampling);
		}
	}
}
