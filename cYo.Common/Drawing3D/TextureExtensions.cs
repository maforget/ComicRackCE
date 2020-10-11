using System.Drawing;

namespace cYo.Common.Drawing3D
{
	public static class TextureExtensions
	{
		public static void SetColor(this IFrameBuffer fb, Point pt, Color color)
		{
			fb.SetColor(pt.X, pt.Y, color);
		}
	}
}
