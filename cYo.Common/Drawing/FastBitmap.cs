using System.Drawing;
using System.Drawing.Imaging;
using cYo.Common.ComponentModel;

namespace cYo.Common.Drawing
{
	public class FastBitmap : DisposableObject
	{
		private struct PixelData
		{
			public byte Blue;

			public byte Green;

			public byte Red;

			public byte Alpha;

			public override string ToString()
			{
				return "(" + Alpha + ", " + Red + ", " + Green + ", " + Blue + ")";
			}
		}

		private int width;

		private BitmapData bitmapData;

		private unsafe byte* pBase = null;

		private readonly Bitmap bitmap;

		private unsafe PixelData* pixelData = null;

		public Bitmap Bitmap => bitmap;

		public unsafe FastBitmap(Bitmap inputBitmap, bool lockBitmap = true)
		{
			bitmap = inputBitmap;
			if (lockBitmap)
			{
				LockImage();
			}
		}

		protected override void Dispose(bool disposing)
		{
			UnlockImage();
			base.Dispose(disposing);
		}

		public unsafe void LockImage()
		{
			if (bitmapData == null)
			{
				Rectangle rect = new Rectangle(Point.Empty, bitmap.Size);
				width = rect.Width * sizeof(PixelData);
				if (width % 4 != 0)
				{
					width = 4 * (width / 4 + 1);
				}
				bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
				pBase = (byte*)bitmapData.Scan0.ToPointer();
			}
		}

		public unsafe Color GetPixel(int x, int y)
		{
			pixelData = (PixelData*)(pBase + y * width + x * sizeof(PixelData));
			return Color.FromArgb(pixelData->Alpha, pixelData->Red, pixelData->Green, pixelData->Blue);
		}

		public unsafe Color GetPixelNext()
		{
			pixelData++;
			return Color.FromArgb(pixelData->Alpha, pixelData->Red, pixelData->Green, pixelData->Blue);
		}

		public unsafe void SetPixel(int x, int y, Color color)
		{
			PixelData* ptr = (PixelData*)(pBase + y * width + x * sizeof(PixelData));
			ptr->Alpha = color.A;
			ptr->Green = color.G;
			ptr->Blue = color.B;
			ptr->Red = color.R;
		}

		public unsafe void UnlockImage()
		{
			if (bitmapData != null)
			{
				bitmap.UnlockBits(bitmapData);
				bitmapData = null;
				pBase = null;
			}
		}
	}
}
