using System.Drawing;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;

namespace cYo.Common.Drawing3D
{
	public class BitmapFrameBuffer : DisposableObject, IFrameBuffer, ITexture
	{
		private readonly FastBitmap fastBitmap;

		private readonly Rectangle clip;

		private readonly Rectangle bounds;

		public Size Size => clip.Size;

		public BitmapFrameBuffer(Bitmap bitmap, Rectangle clip)
		{
			bounds = bitmap.Size.ToRectangle();
			if (clip.IsEmpty)
			{
				this.clip = bounds;
			}
			else
			{
				clip.Intersect(bounds);
				this.clip = clip;
			}
			fastBitmap = new FastBitmap(bitmap);
		}

		public BitmapFrameBuffer(Bitmap bitmap)
			: this(bitmap, Rectangle.Empty)
		{
		}

		protected override void Dispose(bool disposing)
		{
			if (fastBitmap != null)
			{
				fastBitmap.Dispose();
			}
			base.Dispose(disposing);
		}

		public Color GetColor(int x, int y)
		{
			x += clip.X;
			y += clip.Y;
			if (x < bounds.Width && y < bounds.Height && x >= 0 && y >= 0)
			{
				return fastBitmap.GetPixel(x, y);
			}
			return Color.Empty;
		}

		public void SetColor(int x, int y, Color color)
		{
			x += clip.X;
			y += clip.Y;
			if (x < bounds.Width && y < bounds.Height && x >= 0 && y >= 0)
			{
				fastBitmap.SetPixel(x, y, color);
			}
		}
	}
}
