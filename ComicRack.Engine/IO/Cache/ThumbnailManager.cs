using System.Drawing;
using cYo.Projects.ComicRack.Engine.IO.Provider;

namespace cYo.Projects.ComicRack.Engine.IO.Cache
{
	public class ThumbnailManager : ImageManagerBase<ThumbnailImage>
	{
		public ThumbnailManager(int itemCapacity, long sizeCapacity)
			: base(itemCapacity, sizeCapacity)
		{
		}

		protected override ThumbnailImage CreateNewFromProvider(ImageKey key, IImageProvider provider)
		{
			using (Bitmap bitmap = provider.GetImage(key.Index))
			{
				return ThumbnailImage.CreateFrom(bitmap, bitmap.Size);
			}
		}
	}
}
