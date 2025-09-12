using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using sharpPDF;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Writers
{
	[FileFormat("PDF Document (PDF)", KnownFileFormats.PDF, ".pdf")]
	public class PdfStorageProvider : StorageProvider
	{
		protected override ComicInfo OnStore(IImageProvider provider, ComicInfo info, string target, StorageSetting setting)
		{
			pdfDocument pdfDocument = new pdfDocument(Path.GetFileNameWithoutExtension(target), Application.ProductName);
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
					using (Bitmap bitmap = pageResult.GetImage())
					{
						pdfPage pdfPage = pdfDocument.addPage(bitmap.Height, bitmap.Width);
						pdfPage.addImage(bitmap, 0, 0);
					}
					page = pageResult.Info;
					page.ImageIndex = num++;
					comicInfo.Pages.Add(page);
				}
			}
			pdfDocument.createPDF(target);
			comicInfo.PageCount = comicInfo.Pages.Count;
			return comicInfo;
		}
	}
}
