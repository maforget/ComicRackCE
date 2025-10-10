using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Drawing;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Engine.Controls
{
	public class MenuRenderer : ToolStripProfessionalRenderer
	{
		private Image starImage;

		public Image StarImage
		{
			get
			{
				return starImage;
			}
			set
			{
				starImage = value;
			}
		}

		public MenuRenderer(Image starImage, ProfessionalColorTable colorTable)
			: base(colorTable)
		{
			this.starImage = starImage;
		}

		public MenuRenderer(Image starImage)
			: this(starImage, GetManagerColors())
		{
			this.starImage = starImage;
		}

		public static ProfessionalColorTable GetManagerColors()
		{
			return (ToolStripManager.Renderer as ToolStripProfessionalRenderer)?.ColorTable;
		}

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
			ThemeExtensions.TryDrawTheme(() => e.ArrowColor = Color.White, onlyDrawIfDefault: false);
            base.OnRenderArrow(e);
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
		{
			if (starImage == null)
			{
				return;
			}
			string text = e.Text;
			Graphics graphics = e.Graphics;
			if (!text.StartsWith("*"))
			{
				ThemeExtensions.TryDrawTheme(() => e.TextColor = Color.White, onlyDrawIfDefault: false);
				base.OnRenderItemText(e);
				return;
			}
			float num = text.Count((char c) => c == '*');
			Rectangle textRectangle = e.TextRectangle;
			textRectangle.Inflate(-2, -2);
			int x = textRectangle.X;
			int y = textRectangle.Y;
			int height = textRectangle.Height;
			int num2 = height * starImage.Width / starImage.Height;
			using (graphics.SaveState())
			{
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				for (int i = 0; (float)i < num; i++)
				{
					graphics.DrawImage(starImage, x + num2 * i, y, num2, height);
				}
			}
		}

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            base.OnRenderItemCheck(e);
			ThemeExtensions.TryDrawTheme(() =>
			{
				var g = e.Graphics;
				var rect = e.ImageRectangle;
				g.FillRectangle(new SolidBrush(ColorTable.CheckPressedBackground), rect);

				using (var pen = new Pen(Color.White, 2))
				{
					g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
					g.DrawLines(pen, new[]
					{
						new Point(rect.Left + 4, rect.Top + rect.Height/2 - 1),
						new Point(rect.Left + rect.Width/3 + rect.Width/6, rect.Bottom - 5),
						new Point(rect.Right - 4, rect.Top + 3)
					});
				}
			}, onlyDrawIfDefault: false);
        }
    }
}
