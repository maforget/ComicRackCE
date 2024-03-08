using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using PDFiumSharp;
using PDFiumSharp.Enums;
using cYo.Common.Drawing;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Pdf
{
    public class PdfiumReaderEngine : IComicAccessor
    {
        public IEnumerable<ProviderImageInfo> GetEntryList(string source)
        {
            using (var pdfDocument = new PdfDocument(source))
            {
                for (int i = 0; i < pdfDocument.Pages.Count; i++)
                {
                    using (PdfPage pdfPage = pdfDocument.Pages[i])
                    {
                        yield return new ProviderImageInfo(i);
                    }
                }
            }

        }

        public bool IsFormat(string source)
        {
            throw new NotImplementedException();
        }

        public byte[] ReadByteImage(string source, ProviderImageInfo info)
        {
            try
            {
                using (PdfDocument pdfDocument = new PdfDocument(source))
                {
                    using (PdfPage pdfPage = pdfDocument.Pages[info.Index])
                    {
                        int dpiX = 300;
                        int dpiY = 300;

                        int pageWidth = (int)(dpiX * pdfPage.Size.Width / 72);
                        int pageHeight = (int)(dpiY * pdfPage.Size.Height / 72);

                        using (Bitmap bitmap = new Bitmap(pageWidth, pageHeight, PixelFormat.Format24bppRgb))
                        {
                            pdfPage.Render(bitmap);
                            return bitmap.ImageToBytes(ImageFormat.Jpeg);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
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
