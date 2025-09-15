using System.Drawing;
using System.IO;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.IO;

namespace cYo.Projects.ComicRack.Engine.IO
{
	public class PageImage : MemoryOptimizedImage, IDataSize
	{
		public static int SecondsToKeepDecodedImage = 5;

		public static bool MemoryOptimized = true;

		public bool Merged { get; set; } = false;

        public Color BackgrounColor
		{
			get;
			set;
		}

		public int DataSize
		{
			get
			{
				int num = 0;
				if (base.Data != null)
				{
					num += base.Data.Length;
				}
				if (base.IsImage)
				{
					num += base.Width * base.Height * 4;
				}
				return num;
			}
		}

		private PageImage(byte[] data)
			: base(data)
		{
			base.TimeToStay = SecondsToKeepDecodedImage;
			base.Optimized = MemoryOptimized;
			BackgrounColor = Color.Empty;
		}

		private PageImage(byte[] data, Bitmap newImage)
			: this(data)
		{
			Bitmap = newImage;
		}

		public static PageImage CreateFrom(string file)
		{
			return new PageImage(File.ReadAllBytes(file));
		}

		public static PageImage CreateFrom(Stream s)
		{
			return new PageImage(s.ReadAllBytes());
		}

		public static PageImage Wrap(Bitmap newImage)
		{
			return new PageImage(newImage.ImageToJpegBytes(), newImage);
		}

		public static PageImage CreateFrom(byte[] data)
		{
			return new PageImage(data);
		}

		public static PageImage CreateFrom(Bitmap bmp)
		{
			return new PageImage(bmp.ImageToJpegBytes());
		}

        public static PageImage CreateFromMerged(Bitmap bmp)
        {
            PageImage newPageImage = CreateFrom(bmp);
			newPageImage.Merged = true;
			return newPageImage;
        }
    }
}
