using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using cYo.Common.IO;
using cYo.Common.Win32;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Writers
{
	[FileFormat("DjVu Document (DJVU)", KnownFileFormats.DJVU, ".djvu")]
	public class DjVuStorageProvider : StorageProvider, IValidateProvider
	{
		private static readonly string combineExe = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources\\djvm.exe");

		public bool IsValid => File.Exists(combineExe);

		protected override ComicInfo OnStore(IImageProvider provider, ComicInfo info, string target, StorageSetting setting)
		{
			ComicInfo comicInfo = new ComicInfo(info);
			comicInfo.Pages.Clear();
			int num = 0;
			for (int i = 0; i < provider.Count; i++)
			{
				ComicPageInfo page = info.GetPage(i);
				if (!setting.IsValidPage(i))
				{
					continue;
				}
				if (FireProgressEvent(i * 100 / provider.Count))
				{
					throw new OperationCanceledException("Export operation was cancelled by the user.");
				}
				PageResult[] images = StorageProvider.GetImages(provider, page, null, setting, info.Manga == MangaYesNo.YesAndRightToLeft, createThumbnail: false);
				foreach (PageResult pageResult in images)
				{
					using (Bitmap bmp = pageResult.GetImage())
					{
						string tempFileName = Path.GetTempFileName();
						try
						{
							DjVuImage.SaveDjVu(bmp, tempFileName);
							if (ExecuteProcess.Execute(combineExe, string.Format("-{0} \"{1}\" \"{2}\"", (num == 0) ? "c" : "i", target, tempFileName), ExecuteProcess.Options.None).ExitCode != 0)
							{
								throw new InvalidDataException();
							}
						}
						finally
						{
							FileUtility.SafeDelete(tempFileName);
						}
					}
					page = pageResult.Info;
					page.ImageIndex = num++;
					comicInfo.Pages.Add(page);
				}
			}
			comicInfo.PageCount = comicInfo.Pages.Count;
			return comicInfo;
		}
	}
}
