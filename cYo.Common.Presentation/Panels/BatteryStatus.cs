using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Drawing;
using cYo.Common.Presentation.Properties;

namespace cYo.Common.Presentation.Panels
{
	public class BatteryStatus : OverlayPanel
	{
		private int percent = -1;

		private bool plug;

		public BatteryStatus()
			: base(20, 8)
		{
		}

		protected override void OnDrawing()
		{
			base.OnDrawing();
			int num = (int)(SystemInformation.PowerStatus.BatteryLifePercent * 100f);
			bool flag = SystemInformation.PowerStatus.PowerLineStatus == PowerLineStatus.Online;
			if (num == percent && flag == plug)
			{
				return;
			}
			percent = num;
			plug = flag;
			using (PanelSurface panelSurface = CreateSurface(empty: true))
			{
				Graphics graphics = panelSurface.Graphics;
				if (plug)
				{
					using (Bitmap bitmap = Resources.Plug)
					{
						graphics.DrawImage(bitmap, bitmap.Size.Align(base.ClientRectangle, ContentAlignment.MiddleCenter));
					}
					return;
				}
				Rectangle rectangle = base.ClientRectangle.Pad(0, 0, 1, 1);
				Color color = (((double)percent < 0.15) ? Color.Red : Color.Green);
				graphics.DrawRectangle(Pens.White, rectangle);
				rectangle = rectangle.Pad(1, 1);
				rectangle.Width = rectangle.Width * percent / 100;
				using (Brush brush = new SolidBrush(color))
				{
					graphics.FillRectangle(brush, rectangle);
				}
			}
		}
	}
}
