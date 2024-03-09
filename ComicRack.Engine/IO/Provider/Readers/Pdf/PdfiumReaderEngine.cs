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
                        Size size = CalculateSize(pdfPage.Width, pdfPage.Height);
                        using (Bitmap bitmap = new Bitmap(size.Width, size.Height, PixelFormat.Format24bppRgb))
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

        private Size CalculateSize(double width, double height)
        {
            //The width & height are returned in point (1/72 inch)
            //but PDFs created by CR will have the wrong page size. Which would mean that opening this PDF would mean the resolution would balloon up.
            //To prevent from the above mentioned ballooning, this will be the MAX resolution
            int maxWidth = 1920; //8.5in at 225dpi
            int maxHeight = 2540; //11in at 225dpi

            //Calculate the width based on the max height
            int targeWidth = (int)((width * maxHeight) / height);
            //Calculate the height based on the max width
            int targeHeight = (int)((height * maxWidth) / width);

            //if the page is a landscape page (width > height), use the max height, if not we use the max width
            Size outSize = width > height ? new Size(targeWidth, maxHeight) : new Size(maxWidth, targeHeight);

            return outSize;
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
