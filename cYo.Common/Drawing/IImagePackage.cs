using System.Drawing;

namespace cYo.Common.Drawing
{
	public interface IImagePackage
	{
		Image GetImage(string key);

		bool ImageExists(string key);

		bool ImageLoaded(string key);
	}
}
