using System.Drawing;
using cYo.Common.ComponentModel;

namespace cYo.Common.Presentation.Panels
{
	public class PanelSurface : DisposableObject
	{
		private readonly Graphics graphics;

		public Graphics Graphics => graphics;

		public PanelSurface(Bitmap bitmap)
		{
			graphics = Graphics.FromImage(bitmap);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				graphics.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
