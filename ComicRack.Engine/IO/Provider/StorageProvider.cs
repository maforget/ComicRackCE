using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using cYo.Common.Drawing;
using cYo.Common.IO;
using cYo.Common.Localize;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
    public abstract class StorageProvider : FileProviderBase, IStorageProvider
    {
        public class PageResult
        {
            private readonly ComicPageInfo info;

            private readonly string extension;

            private byte[] data;

            private byte[] thumbnailData;

            private string dataStoragePath;

            public ComicPageInfo Info => info;

            public string Extension => extension;

            public byte[] Data => data;

            public byte[] ThumbnailData => thumbnailData;

            public PageResult(byte[] data, byte[] thumbnailData, ComicPageInfo info, string extension)
            {
                this.data = data;
                this.thumbnailData = thumbnailData;
                this.info = info;
                this.extension = extension;
            }

            public Bitmap GetImage()
            {
                byte[] array = null;
                array = WebpImage.ConvertToJpeg(data);
                array = DjVuImage.ConvertToJpeg(data);
                array = HeifAvifImage.ConvertToJpeg(data);
                return BitmapExtensions.BitmapFromBytes(array);
            }

            public byte[] GetThumbnailData(StorageSetting setting)
            {
                if (ThumbnailData != null)
                {
                    return thumbnailData;
                }
                using (Bitmap bmp = GetImage())
                {
                    return CreateThumbnail(bmp, setting);
                }
            }

            public static byte[] CreateThumbnail(Bitmap bmp, StorageSetting setting)
            {
                using (Bitmap bitmap = bmp.Scale(setting.ThumbnailSize))
                {
                    return (setting.PageType == StoragePageType.Webp) ? WebpImage.ConvertoToWebp(bitmap, setting.PageCompression) : bitmap.ImageToJpegBytes(setting.PageCompression);
                }
            }

            public void Store()
            {
                if (dataStoragePath == null && data != null)
                {
                    dataStoragePath = Path.GetTempFileName();
                    try
                    {
                        File.WriteAllBytes(dataStoragePath, data);
                        data = null;
                    }
                    catch
                    {
                        FileUtility.SafeDelete(dataStoragePath);
                        dataStoragePath = null;
                    }
                }
            }

            public void Restore()
            {
                if (dataStoragePath != null)
                {
                    try
                    {
                        data = File.ReadAllBytes(dataStoragePath);
                    }
                    finally
                    {
                        DeleteDataStorage();
                    }
                }
            }

            public void Clear()
            {
                data = null;
                thumbnailData = null;
                DeleteDataStorage();
            }

            private void DeleteDataStorage()
            {
                if (dataStoragePath != null)
                {
                    FileUtility.SafeDelete(dataStoragePath);
                    dataStoragePath = null;
                }
            }
        }

        public virtual string DefaultExtension => base.DefaultFileFormat.Extensions.FirstOrDefault();

        public event EventHandler<StorageProgressEventArgs> Progress;

        public ComicInfo Store(IImageProvider provider, ComicInfo info, string target, StorageSetting setting)
        {
            if (provider == null || provider.Count <= 0)
            {
                throw new InvalidDataException(TR.Messages["SourceComicNotValid", "Source Book is not valid"]);
            }
            string destFileName = target;
            string text = null;
            try
            {
                if (string.Equals(provider.Source, target, StringComparison.OrdinalIgnoreCase))
                {
                    target = (text = EngineConfiguration.Default.GetTempFileName());
                }
                ComicInfo result = OnStore(provider, info, target, setting);
                if (text != null)
                {
                    File.Copy(text, destFileName, overwrite: true);
                }
                return result;
            }
            finally
            {
                if (text != null)
                {
                    FileUtility.SafeDelete(text);
                }
            }
        }

        protected bool FireProgressEvent(int percent)
        {
            if (this.Progress == null)
            {
                return false;
            }
            StorageProgressEventArgs storageProgressEventArgs = new StorageProgressEventArgs(percent);
            this.Progress(this, storageProgressEventArgs);
            return storageProgressEventArgs.Cancel;
        }

        protected abstract ComicInfo OnStore(IImageProvider provider, ComicInfo info, string target, StorageSetting setting);

        private static Bitmap[] GetSubPages(Bitmap bmp, bool splitDouble, bool reverseSplit)
        {
            if (splitDouble && bmp.Height <= bmp.Width)
            {
                Bitmap bitmap = bmp.CreateCopy(new Rectangle(0, 0, bmp.Width / 2, bmp.Height));
                Bitmap bitmap2 = bmp.CreateCopy(new Rectangle(bmp.Width / 2, 0, bmp.Width / 2, bmp.Height));
                if (reverseSplit)
                {
                    return new Bitmap[2]
                    {
                        bitmap2,
                        bitmap
                    };
                }
                return new Bitmap[2]
                {
                    bitmap,
                    bitmap2
                };
            }
            return new Bitmap[1]
            {
                bmp
            };
        }

        public static PageResult[] GetImages(IImageProvider provider, ComicPageInfo cpi, string ext, StorageSetting setting, bool reverseSplit, bool createThumbnail, bool forExport = false)
        {
            byte[] array = null;
            if (setting.RemovePages && cpi.IsTypeOf(setting.RemovePageFilter))
            {
                return new PageResult[0];
            }

            StoragePageType storagePageType = (setting.PageType != StoragePageType.Original) ? setting.PageType : GetStoragePageTypeFromExtension(ext);
            if (!string.IsNullOrEmpty(ext) && setting.PageResize == StoragePageResize.Original && (setting.PageType == StoragePageType.Original || setting.PageType == GetStoragePageTypeFromExtension(ext)) && cpi.Rotation == ImageRotation.None && setting.DoublePages == DoublePageHandling.Keep && setting.ImageProcessing.IsEmpty)
            {
                array = provider.GetByteImage(cpi.ImageIndex);
                if (setting.PageType == StoragePageType.Jpeg)
                {
                    using (MemoryStream s = new MemoryStream(array))
                    {
                        if (!JpegFile.GetImageSize(s, out var size))
                        {
                            array = null;
                        }
                        else
                        {
                            cpi.ImageWidth = size.Width;
                            cpi.ImageHeight = size.Height;
                        }
                    }
                }

                if (forExport)
                {
                    ExportImageContainer data = provider.GetByteImageForExport(cpi.ImageIndex);
                    array = (data.NeedsToConvert) ? ConvertImage(storagePageType, data.Bitmap, setting) : data.Data;
                }

                if (array != null && array.Length != 0)
                {
                    return new PageResult[1]
                    {
                        new PageResult(array, null, cpi, ext)
                    };
                }
            }
            Bitmap bitmap = provider.GetImage(cpi.ImageIndex);
            if (bitmap == null)
            {
                if (setting.IgnoreErrorPages)
                {
                    return new PageResult[0];
                }
                throw new InvalidOperationException(StringUtility.Format(TR.Messages["FailedToReadImage", "Failed to read Image {0}"], cpi.ImageIndex + 1));
            }
            if (array != null && !string.IsNullOrEmpty(ext) && setting.PageResize == StoragePageResize.Original && (setting.PageType == StoragePageType.Original || setting.PageType == GetStoragePageTypeFromExtension(ext)) && cpi.Rotation == ImageRotation.None && (setting.DoublePages == DoublePageHandling.Keep || bitmap.Height <= bitmap.Width) && !setting.ImageProcessing.IsEmpty)
            {
                bitmap.Dispose();
                return new PageResult[1]
                {
                    new PageResult(array, null, cpi, ext)
                };
            }
            List<PageResult> list = new List<PageResult>();
            Bitmap[] array2 = new Bitmap[0];
            try
            {
                ImageRotation imageRotation = cpi.Rotation;
                if (setting.DoublePages == DoublePageHandling.Rotate)
                {
                    Size size2 = bitmap.Size.Rotate(imageRotation);
                    if (size2.Width > size2.Height)
                    {
                        imageRotation = imageRotation.RotateLeft();
                    }
                }
                if (imageRotation != 0)
                {
                    Bitmap bitmap2 = bitmap.Rotate(imageRotation);
                    bitmap.Dispose();
                    bitmap = bitmap2;
                }
                array2 = GetSubPages(bitmap, setting.DoublePages == DoublePageHandling.Split, reverseSplit);
                if (array2.Length > 1)
                {
                    bitmap.Dispose();
                }
                bitmap = null;
                for (int i = 0; i < array2.Length; i++)
                {
                    Bitmap bitmap3 = null;
                    try
                    {
                        Bitmap bitmap4 = array2[i];
                        int num = (setting.DontEnlarge ? Math.Min(bitmap4.Width, setting.PageWidth) : setting.PageWidth);
                        int height = (setting.DontEnlarge ? Math.Min(bitmap4.Height, setting.PageHeight) : setting.PageHeight);
                        switch (setting.PageResize)
                        {
                            case StoragePageResize.WidthHeight:
                                bitmap3 = bitmap4.Scale(new Size(num, height), setting.Resampling, PixelFormat.Format24bppRgb);
                                break;
                            case StoragePageResize.Width:
                                if (setting.DoublePages == DoublePageHandling.AdaptWidth && bitmap4.Width > bitmap4.Height)
                                {
                                    num *= 2;
                                }
                                bitmap3 = bitmap4.Scale(new Size(num, 0), setting.Resampling, PixelFormat.Format24bppRgb);
                                break;
                            case StoragePageResize.Height:
                                bitmap3 = bitmap4.Scale(new Size(0, height), setting.Resampling, PixelFormat.Format24bppRgb);
                                break;
                        }
                        if (bitmap3 != null)
                        {
                            array2[i].Dispose();
                            bitmap4 = (array2[i] = bitmap3);
                            bitmap3 = null;
                        }
                        cpi.ImageWidth = bitmap4.Width;
                        cpi.ImageHeight = bitmap4.Height;
                        if (!setting.ImageProcessing.IsEmpty)
                        {
                            try
                            {
                                Bitmap bitmap5 = bitmap4;
                                bitmap4 = (array2[i] = bitmap4.CreateAdjustedBitmap(setting.ImageProcessing, PixelFormat.Format24bppRgb, alwaysClone: true));
                                bitmap5.Dispose();
                            }
                            catch
                            {
                            }
                        }
                        if ((storagePageType == StoragePageType.Bmp || storagePageType == StoragePageType.Png) && bitmap4.PixelFormat != PixelFormat.Format24bppRgb)
                        {
                            Bitmap bitmap6 = bitmap4;
                            bitmap4 = (array2[i] = bitmap4.CreateCopy(PixelFormat.Format24bppRgb));
                            if (bitmap4 != bitmap6)
                            {
                                bitmap6.Dispose();
                            }
                        }
                        ext = GetExtensionFromStoragePageType(storagePageType);
                        array = ConvertImage(storagePageType, bitmap4, setting);
                        cpi.ImageFileSize = array.Length;
                        list.Add(new PageResult(array, createThumbnail ? PageResult.CreateThumbnail(bitmap4, setting) : null, cpi, ext));
                    }
                    finally
                    {
                        bitmap3?.Dispose();
                    }
                }
            }
            finally
            {
                for (int j = 0; j < array2.Length; j++)
                {
                    if (array2[j] != null)
                    {
                        array2[j].Dispose();
                    }
                }
                bitmap?.Dispose();
            }
            return list.ToArray();
        }

        private static StoragePageType GetStoragePageTypeFromExtension(string ext) => (ext ?? string.Empty).ToLower() switch
        {
            ".bmp" => StoragePageType.Bmp,
            ".tif" or ".tiff" => StoragePageType.Tiff,
            ".png" => StoragePageType.Png,
            ".gif" => StoragePageType.Gif,
            ".djvu" => StoragePageType.Djvu,
            ".webp" => StoragePageType.Webp,
            ".heif" or ".heic" => StoragePageType.Heif,
            ".avif" => StoragePageType.Avif,
            //".jxl" => StoragePageType.JpegXL,
            _ => StoragePageType.Jpeg,
        };

        private static string GetExtensionFromStoragePageType(StoragePageType storagePageType) => storagePageType switch
        {
            StoragePageType.Tiff => ".tif",
            StoragePageType.Png => ".png",
            StoragePageType.Bmp => ".bmp",
            StoragePageType.Gif => ".gif",
            StoragePageType.Djvu => ".djvu",
            StoragePageType.Webp => ".webp",
            StoragePageType.Heif => ".heif",
            StoragePageType.Avif => ".avif",
            //StoragePageType.JpegXL => ".jxl",
            _ => ".jpg",
        };

        private static byte[] ConvertImage(StoragePageType storagePageType, Bitmap bitmap, StorageSetting setting) => storagePageType switch
        {
            StoragePageType.Tiff => bitmap.ImageToBytes(ImageFormat.Tiff, 24),
            StoragePageType.Png => bitmap.ImageToBytes(ImageFormat.Png, 24),
            StoragePageType.Bmp => bitmap.ImageToBytes(ImageFormat.Bmp, 24),
            StoragePageType.Gif => bitmap.ImageToBytes(ImageFormat.Gif, 8),
            StoragePageType.Djvu => DjVuImage.ConvertToDjVu(bitmap),
            StoragePageType.Webp => WebpImage.ConvertoToWebp(bitmap, setting.PageCompression),
            StoragePageType.Heif => HeifAvifImage.ConvertToHeif(bitmap, setting.PageCompression, false),
            StoragePageType.Avif => HeifAvifImage.ConvertToHeif(bitmap, setting.PageCompression, true),
            //StoragePageType.JpegXL => JpegXLImage.ConvertToJpegXL(bitmap),
            _ => bitmap.ImageToBytes(ImageFormat.Jpeg, 24, setting.PageCompression),
        };
    }
}
