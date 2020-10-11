using System.Drawing;

namespace cYo.Common.Drawing3D
{
	public interface ITexture
	{
		Size Size
		{
			get;
		}

		Color GetColor(int x, int y);

		void SetColor(int x, int y, Color color);
	}
}
