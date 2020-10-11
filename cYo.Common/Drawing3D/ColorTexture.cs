using System.Drawing;

namespace cYo.Common.Drawing3D
{
	public class ColorTexture : ITexture
	{
		private Color color = Color.White;

		public Size Size => new Size(1, 1);

		public ColorTexture()
			: this(Color.White)
		{
		}

		public ColorTexture(Color color)
		{
			this.color = color;
		}

		public Color GetColor(int x, int y)
		{
			return color;
		}

		public void SetColor(int x, int y, Color color)
		{
			this.color = color;
		}
	}
}
