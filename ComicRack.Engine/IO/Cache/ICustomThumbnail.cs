using System.Collections.Generic;
using System.Drawing;

namespace cYo.Projects.ComicRack.Engine.IO.Cache
{
	public interface ICustomThumbnail
	{
		string AddCustomThumbnail(Bitmap bmp);

		ThumbnailImage GetCustomThumbnail(string key);

		IEnumerable<string> GetCustomThumbnailKeys();

		bool CustomThumbnailExists(string key);

		bool RemoveCustomThumbnail(string key);
	}
}
