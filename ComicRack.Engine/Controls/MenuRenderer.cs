using cYo.Common.Drawing;
using cYo.Common.Windows.Forms.Theme.Resources;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Engine.Controls
{
	public class MenuRenderer : ThemeToolStripProRenderer
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
                SetToolStripItemThemeColor(e);
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
    }
}
