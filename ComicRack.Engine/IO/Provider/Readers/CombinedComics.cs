using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Projects.ComicRack.Engine.IO.Cache;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers
{
	public static class CombinedComics
	{
		private class Provider
		{
			public IImageProvider ImageProvider
			{
				get;
				set;
			}

			public IImageKeyProvider KeyProvider
			{
				get;
				set;
			}
		}

		private class CombinedImageProvider : DisposableObject, IImageProvider, IDisposable
		{
			private readonly IList<Provider> providers = new List<Provider>();

			public IList<Provider> Providers => providers;

			public IPagePool PagePool
			{
				get;
				set;
			}

			public bool IsSlow => providers.Any((Provider p) => p.ImageProvider.IsSlow);

			public string Source => providers[0].ImageProvider.Source;

			public int Count => providers.Sum((Provider p) => p.ImageProvider.Count);

			public ProviderImageInfo GetImageInfo(int index)
			{
				return GetProvider(ref index).ImageProvider.GetImageInfo(index);
			}

			public ThumbnailImage GetThumbnail(int index)
			{
				return GetProvider(ref index).ImageProvider.GetThumbnail(index);
			}

			public Bitmap GetImage(int index)
			{
				Provider provider = GetProvider(ref index);
				if (PagePool != null)
				{
					using (IItemLock<PageImage> itemLock = PagePool.GetPage(new PageKey(provider.KeyProvider.GetImageKey(index)), onlyMemory: false))
					{
						if (itemLock != null && itemLock.Item != null)
						{
							return itemLock.Item.Bitmap.CreateCopy(alwaysTrueCopy: true);
						}
					}
				}
				return provider.ImageProvider.GetImage(index);
			}

			public byte[] GetByteImage(int index)
			{
				Provider provider = GetProvider(ref index);
				if (PagePool != null)
				{
					using (IItemLock<PageImage> itemLock = PagePool.GetPage(new PageKey(provider.KeyProvider.GetImageKey(index)), onlyMemory: false))
					{
						if (itemLock != null && itemLock.Item != null)
						{
							return itemLock.Item.Data;
						}
					}
				}
				return provider.ImageProvider.GetByteImage(index);
			}

            public ExportImageContainer GetByteImageForExport(int index)
            {
                Provider provider = GetProvider(ref index);
                if (PagePool != null)
                {
                    using (IItemLock<PageImage> itemLock = PagePool.GetPage(new PageKey(provider.KeyProvider.GetImageKey(index)), onlyMemory: false))
                    {
                        if (itemLock != null && itemLock.Item != null && itemLock.Item.Merged)
                        {
							return new ExportImageContainer()
							{
								Data = itemLock.Item.Data,
								NeedsToConvert = true
							};
                        }
                    }
                }
				return provider.ImageProvider.GetByteImageForExport(index);
            }

            protected override void Dispose(bool disposing)
			{
				foreach (Provider provider in providers)
				{
					provider.ImageProvider.SafeDispose();
				}
			}

			private Provider GetProvider(ref int imageIndex)
			{
				foreach (Provider provider in providers)
				{
					if (imageIndex < provider.ImageProvider.Count)
					{
						return provider;
					}
					imageIndex -= provider.ImageProvider.Count;
					if (imageIndex < 0)
					{
						break;
					}
				}
				return null;
			}
		}

		public static IImageProvider OpenProvider(IEnumerable<ComicBook> books, IPagePool pool)
		{
			CombinedImageProvider combinedImageProvider = new CombinedImageProvider();
			foreach (ComicBook book in books)
			{
				if (book.IsDynamicSource)
				{
					pool.RefreshLastImage(book.FilePath);
				}
				combinedImageProvider.Providers.Add(new Provider
				{
					ImageProvider = book.OpenProvider(),
					KeyProvider = book
				});
			}
			combinedImageProvider.PagePool = pool;
			return combinedImageProvider;
		}

		public static ComicInfo GetComicInfo(IEnumerable<ComicBook> books)
		{
			ComicInfo comicInfo = null;
			int num = 0;
			bool flag = books.Count() > 1;
			foreach (ComicBook book in books)
			{
				if (comicInfo == null)
				{
					comicInfo = new ComicInfo(book);
					comicInfo.Pages.Clear();
					book.SetShadowValues(comicInfo);
				}
				for (int i = 0; i < book.PageCount; i++)
				{
					ComicPageInfo page = book.GetPage(i);
					page.ImageIndex += num;
					if (flag && i == 0)
					{
						page.Bookmark = book.Caption;
					}
					comicInfo.Pages.Add(page);
				}
				num += book.PageCount;
			}
			return comicInfo;
		}
	}
}
