using System.Drawing;
using cYo.Common.Drawing;

namespace cYo.Projects.ComicRack.Engine.Display
{
	public static class ComicDisplayExtensions
	{
		public static Bitmap CreateThumbnail(this IComicDisplay display)
		{
			return display.CreateThumbnail(new Size(0, 256));
		}

		public static Bitmap CreateThumbnail(this IComicDisplay display, Size size)
		{
			using (Bitmap bmp = display.CreatePageImage())
			{
				return bmp.Scale(size, BitmapResampling.FastBicubic);
			}
		}
	}
}
