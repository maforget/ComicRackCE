using System.Windows.Forms;

namespace cYo.Common.Windows
{
	public static class ScreenExtension
	{
		public static bool IsPortrait(this Screen screen)
		{
			return screen.Bounds.Width < screen.Bounds.Height;
		}

		public static bool IsLandscape(this Screen screen)
		{
			return !screen.IsPortrait();
		}
	}
}
