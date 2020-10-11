using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using cYo.Common.Drawing;

namespace cYo.Common.Presentation.Panels
{
	public class LabelPanel : OverlayPanel
	{
		private string text;

		private ContentAlignment textAlignment = ContentAlignment.MiddleLeft;

		private Color textColor = Color.White;

		private string textFont = "Sans Serif";

		private float textSize = 8f;

		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				if (!(text == value))
				{
					text = value;
					Invalidate();
				}
			}
		}

		public ContentAlignment TextAlignment
		{
			get
			{
				return textAlignment;
			}
			set
			{
				if (textAlignment != value)
				{
					textAlignment = value;
					Invalidate();
				}
			}
		}

		public Color TextColor
		{
			get
			{
				return textColor;
			}
			set
			{
				if (!(textColor == value))
				{
					textColor = value;
					Invalidate();
				}
			}
		}

		public string TextFont
		{
			get
			{
				return textFont;
			}
			set
			{
				if (!(textFont == value))
				{
					textFont = value;
					Invalidate();
				}
			}
		}

		public float TextSize
		{
			get
			{
				return textSize;
			}
			set
			{
				if (textSize != value)
				{
					textSize = value;
					Invalidate();
				}
			}
		}

		public LabelPanel()
			: base(100, 12)
		{
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
			Rectangle clientRectangle = base.ClientRectangle;
			graphics.Clear(Color.Transparent);
			Font font = FC.Get(textFont, textSize);
			using (graphics.TextRendering(TextRenderingHint.AntiAliasGridFit))
			{
				using (Brush brush = new SolidBrush(textColor))
				{
					using (StringFormat format = new StringFormat
					{
						Alignment = textAlignment.ToAlignment(),
						LineAlignment = textAlignment.ToLineAlignment()
					})
					{
						graphics.DrawString(text, font, brush, clientRectangle, format);
					}
				}
			}
		}
	}
}
