using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Presentation;
using cYo.Common.Threading;
using cYo.Projects.ComicRack.Engine.Display.Forms.Properties;

namespace cYo.Projects.ComicRack.Engine.Display.Forms
{
	public static class PanelRenderer
	{
		private struct PanelInfo
		{
			public readonly Bitmap Bitmap;

			public readonly Padding Margin;

			public readonly RectangleF Source;

			public readonly Color ForeColor;

			public PanelInfo(Bitmap bmp, Padding margin, RectangleF src, Color foreColor)
			{
				Bitmap = bmp;
				Margin = margin;
				Source = src;
				ForeColor = foreColor;
			}
		}

		private static readonly PanelInfo[] panels = new PanelInfo[2]
		{
			new PanelInfo(Resources.BlackGlassPanel, new Padding(5, 5, 10, 10), new RectangleF(5f, 5f, 215f, 125f), Color.White),
			new PanelInfo(Resources.BlueGlassPanel, new Padding(5, 5, 10, 10), new RectangleF(5f, 5f, 215f, 125f), Color.White)
		};

		public static RectangleF Draw(IBitmapRenderer gr, RectangleF dest, float opacity, PanelType pt = PanelType.BlackGlass)
		{
			PanelInfo panelInfo = panels[(int)pt];
			using (ItemMonitor.Lock(panelInfo.Bitmap))
			{
				return ScalableBitmap.Draw(gr, panelInfo.Bitmap, dest, panelInfo.Source, panelInfo.Margin, opacity);
			}
		}

		public static RectangleF DrawGraphics(Graphics gr, RectangleF dest, float opacity, PanelType pt = PanelType.BlackGlass)
		{
			return Draw(new BitmapGdiRenderer(gr), dest, opacity, pt);
		}

		public static Padding GetMargin(RectangleF dest, PanelType pt = PanelType.BlackGlass)
		{
			RectangleF rectangleF = Draw(null, dest, 1f, pt);
			return new Padding((int)(rectangleF.Left - dest.Left), (int)(rectangleF.Top - dest.Top), (int)(dest.Right - rectangleF.Right), (int)(dest.Bottom - rectangleF.Bottom));
		}

		public static Color GetForeColor(PanelType pt = PanelType.BlackGlass)
		{
			return panels[(int)pt].ForeColor;
		}
	}
}
