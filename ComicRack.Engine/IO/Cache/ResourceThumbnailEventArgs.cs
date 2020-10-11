using System;

namespace cYo.Projects.ComicRack.Engine.IO.Cache
{
	public class ResourceThumbnailEventArgs : EventArgs
	{
		public ThumbnailKey Key
		{
			get;
			private set;
		}

		public ThumbnailImage Image
		{
			get;
			set;
		}

		public ResourceThumbnailEventArgs(ThumbnailKey key)
		{
			Key = key;
		}
	}
}
