using System;
using System.Collections.Generic;
using cYo.Common.Drawing;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Pdf
{
	public class PdfGhostScript : IComicAccessor
	{
		private int currentDpi = 75;

		public bool IsAvailable()
		{
			if (!string.IsNullOrEmpty(EngineConfiguration.Default.GhostscriptExecutable))
			{
				PdfImages.GhostscriptPath = EngineConfiguration.Default.GhostscriptExecutable;
			}
			if (EngineConfiguration.Default.PdfEngineToUse == EngineConfiguration.PdfEngine.Ghostscript)
			{
				return PdfImages.IsGhostscriptAvailable;
			}
			return false;
		}

		public bool IsFormat(string source)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<ProviderImageInfo> GetEntryList(string source)
		{
			PdfImages pdf = new PdfImages(source, EngineConfiguration.Default.TempPath);
			for (int i = 0; i < pdf.PageCount; i++)
			{
				yield return new ProviderImageInfo(i);
			}
		}

		public byte[] ReadByteImage(string source, ProviderImageInfo info)
		{
			int index = info.Index;
			PdfImages pdfImages = new PdfImages(source, EngineConfiguration.Default.TempPath);
			byte[] pageData = pdfImages.GetPageData(index, currentDpi);
			if (pageData == null)
			{
				return null;
			}
			JpegFile jpegFile = new JpegFile(pageData);
			if (!jpegFile.IsValid)
			{
				return null;
			}
			if (jpegFile.Height >= 1024 && jpegFile.Height < 2048)
			{
				return pageData;
			}
			currentDpi = currentDpi * 1536 / jpegFile.Height;
			return pdfImages.GetPageData(index, currentDpi);
		}

		public ComicInfo ReadInfo(string source)
		{
			return null;
		}

		public bool WriteInfo(string source, ComicInfo info)
		{
			return false;
		}
	}
}
