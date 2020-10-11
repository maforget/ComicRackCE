using System.Drawing;

namespace cYo.Common.Drawing3D
{
	public class ColorizeTexture : ITexture
	{
		private Color color;

		private readonly ITexture texture;

		public Size Size => texture.Size;

		public ColorizeTexture(ITexture texture, Color color)
		{
			this.texture = texture;
			this.color = color;
		}

		public Color GetColor(int x, int y)
		{
			Color color = texture.GetColor(x, y);
			return Color.FromArgb(this.color.A * color.A / 256, this.color.R * color.R / 256, this.color.G * color.G / 256, this.color.B * color.B / 256);
		}

		public void SetColor(int x, int y, Color color)
		{
			texture.SetColor(x, y, color);
		}
	}
}
