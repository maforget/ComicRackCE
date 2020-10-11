using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Presentation.Panels;
using cYo.Common.Threading;
using cYo.Projects.ComicRack.Engine.Display.Forms.Properties;

namespace cYo.Projects.ComicRack.Engine.Display.Forms
{
	public class GestureOverlay : OverlayPanel
	{
		private readonly Bitmap bitmap = Resources.TouchPad;

		public GestureOverlay()
			: base(EngineConfiguration.Default.GestureAreaSize, EngineConfiguration.Default.GestureAreaSize)
		{
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			using (ItemMonitor.Lock(bitmap))
			{
				e.Graphics.DrawImage(bitmap, base.ClientRectangle);
			}
		}
	}
}
