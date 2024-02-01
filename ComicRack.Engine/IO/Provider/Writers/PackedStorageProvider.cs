using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using cYo.Common.Drawing;
using cYo.Common.IO;
using cYo.Common.Mathematics;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Writers
{
	public abstract class PackedStorageProvider : StorageProvider
	{
		private class IndexedPageResult : IComparable<IndexedPageResult>
		{
			public int Index
			{
				get;
				set;
			}

			public int Offset
			{
				get;
				set;
			}

			public PageResult Page
			{
				get;
				set;
			}

			public string OriginalName
			{
				get;
				set;
			}

			public int CompareTo(IndexedPageResult other)
			{
				int num = Index.CompareTo(other.Index);
				if (num != 0)
				{
					return num;
				}
				return Offset.CompareTo(other.Offset);
			}
		}

		protected abstract void OnCreateFile(string target, StorageSetting setting);

		protected abstract void OnCloseFile();

		protected abstract void AddEntry(string name, byte[] data);

		protected override ComicInfo OnStore(IImageProvider provider, ComicInfo info, string target, StorageSetting setting)
		{
			OnCreateFile(target, setting);
			List<IndexedPageResult> pages = new List<IndexedPageResult>();
			try
			{
				int loopCount = 0;
				long totalPageMemory = 0L;
				Exception ce = null;
				ParallelOptions parallelOptions = new ParallelOptions();
				parallelOptions.MaxDegreeOfParallelism = EngineConfiguration.Default.ParallelConversions.Clamp(1, Environment.ProcessorCount);
				Parallel.For(0, provider.Count, parallelOptions, delegate(int n, ParallelLoopState ls)
				{
					try
					{
						if (setting.IsValidPage(n))
						{
							ComicPageInfo page = info.GetPage(n);
							ProviderImageInfo imageInfo = provider.GetImageInfo(page.ImageIndex);
							string ext = ((imageInfo != null && !string.IsNullOrEmpty(imageInfo.Name)) ? Path.GetExtension(imageInfo.Name) : ".jpg");
							int num2 = 0;
							PageResult[] images = StorageProvider.GetImages(provider, page, ext, setting, info.Manga == MangaYesNo.YesAndRightToLeft, setting.CreateThumbnails);
							foreach (PageResult pageResult in images)
							{
								bool flag;
								lock (this)
								{
									totalPageMemory += pageResult.Data.Length;
									flag = totalPageMemory > 52428800;
								}
								if (flag)
								{
									pageResult.Store();
								}
								lock (pages)
								{
									pages.Add(new IndexedPageResult
									{
										Page = pageResult,
										Index = n,
										Offset = num2++,
										OriginalName = imageInfo.Name
									});
								}
							}
							if (FireProgressEvent(++loopCount * 100 / provider.Count))
							{
								ls.Break();
							}
						}
					}
					catch (Exception ex)
					{
						ce = ex;
						ls.Break();
					}
				});
				if (ce != null)
				{
					throw ce;
				}
				int num = 0;
				ComicInfo comicInfo = new ComicInfo(info);
				comicInfo.Pages.Clear();
				pages.Sort();
				HashSet<string> nameTable = new HashSet<string>();
				foreach (IndexedPageResult item in pages)
				{
					item.Page.Restore();
					try
					{
						WritePage(item, num++, comicInfo, setting, nameTable);
					}
					finally
					{
						item.Page.Clear();
					}
				}
				comicInfo.PageCount = comicInfo.Pages.Count;
				if (setting.EmbedComicInfo)
				{
					AddInfo(comicInfo);
				}
				return comicInfo;
			}
			finally
			{
				foreach (IndexedPageResult item2 in pages)
				{
					item2.Page.Clear();
				}
				OnCloseFile();
			}
		}

		private void WritePage(IndexedPageResult ipr, int k, ComicInfo myInfo, StorageSetting setting, ISet<string> nameTable)
		{
			PageResult page = ipr.Page;
			ComicPageInfo info = page.Info;
			string text = null;
			if (setting.KeepOriginalImageNames && !string.IsNullOrEmpty(ipr.OriginalName))
			{
				text = Path.GetFileNameWithoutExtension(ipr.OriginalName);
			}
			if (string.IsNullOrEmpty(text))
			{
				text = $"P{k + 1:00000}";
			}
			if (!setting.KeepOriginalImageNames && info.IsBookmark)
			{
				text = text + " - " + FileUtility.MakeValidFilename(info.Bookmark.Left(25));
			}
			string arg = text;
			int num = 2;
			while (nameTable.Contains(text))
			{
				text = $"{arg}_{num++}";
			}
			nameTable.Add(text);
			string text2 = text + page.Extension;
			AddEntry(text2, page.Data);
			if (setting.CreateThumbnails)
			{
				AddEntry($"Thumbnails\\{text}.tb", page.GetThumbnailData(setting));
			}
			info.ImageIndex = k++;
			info.Rotation = ImageRotation.None;
			if (setting.AddKeyToPageInfo)
			{
				info.Key = text2;
			}
			myInfo.Pages.Add(info);
		}

		protected virtual void AddInfo(ComicInfo comicInfo)
		{
			AddEntry("ComicInfo.xml", comicInfo.ToArray());
		}
	}
}
