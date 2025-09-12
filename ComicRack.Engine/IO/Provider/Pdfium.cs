using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using PDFiumSharp;
using PDFiumSharp.Enums;
using PDFiumSharp.Types;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
    public static class Pdfium
    {
        public static PDFiumBitmap FromBitmap(Bitmap image)
        {
            BitmapFormats pdfFormat = GetBitmapFormat(image);
            BitmapData bitmapdata = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            PDFiumBitmap pdfBitmap = new PDFiumBitmap(image.Width, image.Height, pdfFormat, bitmapdata.Scan0, bitmapdata.Stride);
            image.UnlockBits(bitmapdata);
            return pdfBitmap;
        }

        public static PDFiumBitmap FromFile(string path)
        {
            return FromBitmap(Image.FromFile(path) as Bitmap);
        }

        /// <summary>
        /// Renders the page to a <see cref="Bitmap"/>
        /// </summary>
        /// <param name="page">The page which is to be rendered.</param>
        /// <param name="renderTarget">The bitmap to which the page is to be rendered.</param>
        /// <param name="rectDest">The destination rectangle in <paramref name="renderTarget"/>.</param>
        /// <param name="orientation">The orientation at which the page is to be rendered.</param>
        /// <param name="flags">The flags specifying how the page is to be rendered.</param>
        public static void Render(this PdfPage page, Bitmap renderTarget, (int left, int top, int width, int height) rectDest, PageOrientations orientation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
        {
            if (renderTarget == null)
                throw new ArgumentNullException(nameof(renderTarget));

            BitmapFormats format = GetBitmapFormat(renderTarget);
            BitmapData data = renderTarget.LockBits(new Rectangle(0, 0, renderTarget.Width, renderTarget.Height), ImageLockMode.ReadWrite, renderTarget.PixelFormat);
            using (PDFiumBitmap tmp = new PDFiumBitmap(renderTarget.Width, renderTarget.Height, format, data.Scan0, data.Stride))
            {
                FPDF_COLOR background = page.HasTransparency ? 0x00FFFFFF : 0xFFFFFFFF;
                tmp.FillRectangle(0, 0, tmp.Width, tmp.Height, background);
                page.Render(tmp, rectDest, orientation, flags);
            }

            renderTarget.UnlockBits(data);
        }

        /// <summary>
        /// Renders the page to a <see cref="Bitmap"/>
        /// </summary>
        /// <param name="page">The page which is to be rendered.</param>
        /// <param name="bitmap">The bitmap to which the page is to be rendered.</param>
        /// <param name="orientation">The orientation at which the page is to be rendered.</param>
        /// <param name="flags">The flags specifying how the page is to be rendered.</param>
        public static void Render(this PdfPage page, Bitmap bitmap, PageOrientations orientation = PageOrientations.Normal, RenderingFlags flags = RenderingFlags.None)
        {
            page.Render(bitmap, (0, 0, bitmap.Width, bitmap.Height), orientation, flags);
        }

        static BitmapFormats GetBitmapFormat(Bitmap bitmap)
        {
            switch (bitmap.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    return BitmapFormats.BGR;
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppArgb:
                    return BitmapFormats.BGRA;
                case PixelFormat.Format32bppRgb:
                    return BitmapFormats.BGRx;
                default:
                    throw new NotSupportedException($"Pixel format {bitmap.PixelFormat} is not supported.");
            }
        }
    }

}
