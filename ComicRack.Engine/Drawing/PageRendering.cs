using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace cYo.Projects.ComicRack.Engine.Drawing
{
	public static class PageRendering
	{
		public static Bitmap CreatePageBow(Size size, float angle)
		{
			Bitmap bitmap = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				Color color = Color.FromArgb(EngineConfiguration.Default.PageBowFromAlpha, EngineConfiguration.Default.PageBowColor);
				Color color2 = Color.FromArgb(EngineConfiguration.Default.PageBowToAlpha, EngineConfiguration.Default.PageBowColor);
				Rectangle rect = new Rectangle(0, 0, size.Width, size.Height);
				using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rect, color, color2, angle))
				{
					linearGradientBrush.SetSigmaBellShape(0f, 1f);
					graphics.FillRectangle(linearGradientBrush, rect);
					return bitmap;
				}
			}
		}
	}
}
