using System.Drawing;

namespace cYo.Common.Presentation
{
	public abstract class RendererImage
	{
		public abstract Bitmap Bitmap
		{
			get;
		}

		public virtual bool IsValid => Bitmap != null;

		public virtual Size Size => Bitmap.Size;

		public int Width => Size.Width;

		public int Height => Size.Height;

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return ((RendererImage)obj).Bitmap == Bitmap;
		}

		public override int GetHashCode()
		{
			return Bitmap.GetHashCode();
		}

		public static implicit operator Bitmap(RendererImage image)
		{
			return image.Bitmap;
		}

		public static implicit operator RendererImage(Bitmap image)
		{
			return new RendererGdiImage(image);
		}
	}
}
