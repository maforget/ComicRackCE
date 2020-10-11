using System.Drawing;

namespace cYo.Common.Presentation.Ceco
{
	public interface IRender
	{
		bool IsWhiteSpace
		{
			get;
		}

		void Measure(Graphics gr, int maxWidth);

		void Draw(Graphics gr, Point location);
	}
}
