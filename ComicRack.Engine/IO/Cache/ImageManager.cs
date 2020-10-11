using System.Drawing;
using cYo.Common.Drawing;
using cYo.Projects.ComicRack.Engine.IO.Provider;

namespace cYo.Projects.ComicRack.Engine.IO.Cache
{
	public class ImageManager : ImageManagerBase<PageImage>
	{
		public ImageManager(int size)
			: base(size)
		{
		}

		protected override PageImage CreateNewFromProvider(ImageKey key, IImageProvider provider)
		{
			byte[] byteImage = provider.GetByteImage(key.Index);
			PageImage pageImage = ((byteImage != null) ? PageImage.CreateFrom(byteImage) : PageImage.Wrap(provider.GetImage(key.Index)));
			if (key.Rotation != 0)
			{
				Bitmap newImage = pageImage.Bitmap.Rotate(key.Rotation);
				pageImage.Dispose();
				pageImage = PageImage.Wrap(newImage);
			}
			return pageImage;
		}
	}
}
