using System;
using System.Drawing;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	public interface IImageProvider : IDisposable
	{
		bool IsSlow
		{
			get;
		}

		string Source
		{
			get;
		}

		int Count
		{
			get;
		}

		Bitmap GetImage(int index);

		byte[] GetByteImage(int index);

        ExportImageContainer GetByteImageForExport(int index);

        ProviderImageInfo GetImageInfo(int index);

		ThumbnailImage GetThumbnail(int index);
    }
}
