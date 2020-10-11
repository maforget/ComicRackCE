using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using cYo.Common.Drawing;

namespace cYo.Common.Presentation.Panels
{
	public class SimpleButtonPanel : OverlayPanel
	{
		private ScalableBitmap background;

		private ScalableBitmap icon;

		private float hilightBrightness = 0.4f;

		public ScalableBitmap Background
		{
			get
			{
				return background;
			}
			set
			{
				if (background != value)
				{
					background = value;
					Invalidate();
				}
			}
		}

		public ScalableBitmap Icon
		{
			get
			{
				return icon;
			}
			set
			{
				if (icon != value)
				{
					icon = value;
					Invalidate();
				}
			}
		}

		public float HilightBrightness
		{
			get
			{
				return hilightBrightness;
			}
			set
			{
				if (hilightBrightness != value)
				{
					hilightBrightness = value;
					Invalidate();
				}
			}
		}

		public SimpleButtonPanel(Size size)
			: base(size)
		{
		}

		protected override void OnPanelStateChanged()
		{
			base.OnPanelStateChanged();
			Invalidate();
		}

		protected override void OnMarginChanged()
		{
			base.OnMarginChanged();
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
			IBitmapRenderer gr = new BitmapGdiRenderer(graphics);
			Rectangle clientRectangle = base.ClientRectangle;
			BitmapAdjustment itf = ((base.PanelState == PanelState.Selected) ? new BitmapAdjustment(0f, hilightBrightness) : BitmapAdjustment.Empty);
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			if (background != null)
			{
				background.Draw(gr, clientRectangle, itf, 1f);
			}
			if (icon != null)
			{
				clientRectangle = clientRectangle.Pad(base.Margin);
				clientRectangle = icon.Bitmap.Size.ToRectangle(clientRectangle, RectangleScaleMode.Center | RectangleScaleMode.OnlyShrink);
				icon.Draw(gr, clientRectangle, itf, 1f);
			}
			base.Opacity = ((base.PanelState == PanelState.Normal) ? 0.9f : 1f);
		}
	}
}
