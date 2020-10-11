using System.Drawing;

namespace cYo.Common.Presentation.Ceco
{
	public interface IResources
	{
		Font GetFont(string fontFamily, float fontSize, FontStyle fontStyle);

		Image GetImage(string source);
	}
}
