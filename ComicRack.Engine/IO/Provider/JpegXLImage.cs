using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using cYo.Common.Drawing;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
    public static class JpegXLImage
    {
        [CLSCompliant(false)]
        public static class NativeMethods
        {
            private const string JxlLibrary = "jxl.dll";

            // Decoder
            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr JxlDecoderCreate(IntPtr memoryManager);

            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern void JxlDecoderDestroy(IntPtr decoder);

            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern JxlDecoderStatus JxlDecoderSubscribeEvents(IntPtr decoder, int eventsWanted);

            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern JxlDecoderStatus JxlDecoderSetInput(IntPtr decoder, byte[] data, UIntPtr size);

            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern JxlDecoderStatus JxlDecoderProcessInput(IntPtr decoder);

            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern JxlDecoderStatus JxlDecoderGetBasicInfo(IntPtr decoder, out JxlBasicInfo info);

            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern JxlDecoderStatus JxlDecoderImageOutBufferSize(IntPtr decoder, ref JxlPixelFormat format, out UIntPtr size);

            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern JxlDecoderStatus JxlDecoderSetImageOutBuffer(IntPtr decoder, ref JxlPixelFormat format, IntPtr buffer, UIntPtr size);

            // Encoder
            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr JxlEncoderCreate(IntPtr memoryManager);

            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern void JxlEncoderDestroy(IntPtr encoder);

            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr JxlEncoderFrameSettingsCreate(IntPtr encoder, IntPtr frameSettingsSource);

            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern JxlEncoderStatus JxlEncoderSetFrameDistance(IntPtr frameSettings, float distance);

            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern JxlEncoderStatus JxlEncoderSetFrameLossless(IntPtr frameSettings, int lossless);

            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern JxlEncoderStatus JxlEncoderFrameSettingsSetOption(IntPtr frameSettings, JxlEncoderFrameSettingId option, long value);

            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern JxlEncoderStatus JxlEncoderSetBasicInfo(IntPtr encoder, ref JxlBasicInfo info);

            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern JxlEncoderStatus JxlEncoderAddImageFrame(IntPtr frameSettings, ref JxlPixelFormat format, IntPtr buffer, UIntPtr size);

            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern void JxlEncoderCloseInput(IntPtr encoder);

            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern JxlEncoderStatus JxlEncoderProcessOutput(IntPtr encoder, out IntPtr nextOut, out UIntPtr availOut);

            public enum JxlDecoderStatus
            {
                Success = 0,
                Error = 1,
                NeedMoreInput = 2,
                NeedImageOutBuffer = 5,
                BasicInfo = 0x40,
                FullImage = 0x1000,
            }

            public enum JxlEncoderStatus
            {
                Success = 0,
                Error = 1,
                NeedMoreOutput = 2,
            }

            public enum JxlDataType
            {
                Uint8 = 2,
            }

            public enum JxlEncoderFrameSettingId
            {
                Effort = 0,          // 1-9, default 7 (higher = slower but better compression)
                DecodingSpeed = 1,   // 0-4, default 0 (higher = faster decode but larger file)
            }

            [CLSCompliant(false)]
            [StructLayout(LayoutKind.Sequential)]
            public struct JxlPixelFormat
            {
                public uint num_channels;
                public JxlDataType data_type;
                public uint endianness;
                public UIntPtr align;
            }

            [CLSCompliant(false)]
            [StructLayout(LayoutKind.Sequential)]
            public struct JxlBasicInfo
            {
                public uint have_container;
                public uint xsize;
                public uint ysize;
                public uint bits_per_sample;
                public uint exponent_bits_per_sample;
                public float intensity_target;
                public float min_nits;
                public uint relative_to_max_display;
                public float linear_below;
                public uint uses_original_profile;
                public uint have_preview;
                public uint have_animation;
                public uint orientation;
                public uint num_color_channels;
                public uint num_extra_channels;
                public uint alpha_bits;
                public uint alpha_exponent_bits;
                public uint alpha_premultiplied;
                public uint preview_xsize;
                public uint preview_ysize;
                public ulong animation_tps_numerator;
                public ulong animation_tps_denominator;
                public uint animation_num_loops;
                public uint animation_have_timecodes;
                public uint intrinsic_xsize;
                public uint intrinsic_ysize;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
                public byte[] padding;
            }
        }

        public static byte[] ConvertToJpeg(byte[] data)
        {
            if (!IsSupported(data))
            {
                return data;
            }
            try
            {
                using (Bitmap image = DecodeFromBytes(data))
                {
                    return image.ImageToJpegBytes();
                }
            }
            catch (Exception)
            {
                return data;
            }
        }

        private static Bitmap DecodeFromBytes(byte[] data)
        {
            IntPtr decoder = NativeMethods.JxlDecoderCreate(IntPtr.Zero);
            if (decoder == IntPtr.Zero)
                throw new Exception("Failed to create JXL decoder");

            try
            {
                const int eventsWanted = (int)(NativeMethods.JxlDecoderStatus.BasicInfo | NativeMethods.JxlDecoderStatus.FullImage);
                if (NativeMethods.JxlDecoderSubscribeEvents(decoder, eventsWanted) != NativeMethods.JxlDecoderStatus.Success)
                    throw new Exception("Failed to subscribe to decoder events");

                if (NativeMethods.JxlDecoderSetInput(decoder, data, new UIntPtr((ulong)data.Length)) != NativeMethods.JxlDecoderStatus.Success)
                    throw new Exception("Failed to set decoder input");

                NativeMethods.JxlBasicInfo info = default;
                IntPtr pixels = IntPtr.Zero;
                int width = 0, height = 0;
                bool hasAlpha = false;

                try
                {
                    while (true)
                    {
                        var status = NativeMethods.JxlDecoderProcessInput(decoder);

                        if (status == NativeMethods.JxlDecoderStatus.BasicInfo)
                        {
                            if (NativeMethods.JxlDecoderGetBasicInfo(decoder, out info) != NativeMethods.JxlDecoderStatus.Success)
                                throw new Exception("Failed to get basic info");

                            width = (int)info.xsize;
                            height = (int)info.ysize;
                            hasAlpha = info.num_extra_channels > 0;
                        }
                        else if (status == NativeMethods.JxlDecoderStatus.NeedImageOutBuffer)
                        {
                            var format = new NativeMethods.JxlPixelFormat
                            {
                                num_channels = hasAlpha ? 4u : 3u,
                                data_type = NativeMethods.JxlDataType.Uint8,
                                endianness = 0,
                                align = UIntPtr.Zero
                            };

                            UIntPtr bufferSize;
                            if (NativeMethods.JxlDecoderImageOutBufferSize(decoder, ref format, out bufferSize) != NativeMethods.JxlDecoderStatus.Success)
                                throw new Exception("Failed to get buffer size");

                            pixels = Marshal.AllocHGlobal((int)bufferSize);
                            if (NativeMethods.JxlDecoderSetImageOutBuffer(decoder, ref format, pixels, bufferSize) != NativeMethods.JxlDecoderStatus.Success)
                                throw new Exception("Failed to set output buffer");
                        }
                        else if (status == NativeMethods.JxlDecoderStatus.FullImage || status == NativeMethods.JxlDecoderStatus.Success)
                        {
                            break;
                        }
                        else
                        {
                            throw new Exception($"Decoder error: {status}");
                        }
                    }

                    var pixelFormat = hasAlpha ? PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb;
                    Bitmap bitmap = new Bitmap(width, height, pixelFormat);

                    BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, pixelFormat);
                    try
                    {
                        int srcBytesPerPixel = hasAlpha ? 4 : 3;
                        int srcStride = width * srcBytesPerPixel;
                        int dstBytesPerPixel = hasAlpha ? 4 : 3;

                        unsafe
                        {
                            CopyAndSwapChannels(width, height, srcStride, bmpData.Stride, (byte*)pixels, (byte*)bmpData.Scan0, hasAlpha, srcBytesPerPixel, dstBytesPerPixel, true);
                        }
                    }
                    finally
                    {
                        bitmap.UnlockBits(bmpData);
                    }

                    return bitmap;
                }
                finally
                {
                    if (pixels != IntPtr.Zero)
                        Marshal.FreeHGlobal(pixels);
                }
            }
            finally
            {
                NativeMethods.JxlDecoderDestroy(decoder);
            }
        }

        private static bool IsSupported(byte[] data)
        {
            if (data.Length < 12 || !Environment.Is64BitProcess)
                return false;

            if (data.Length >= 12)
            {
                // Check for JXL header
                byte[] jxlSignature = { 0x00, 0x00, 0x00, 0x0C, 0x4A, 0x58, 0x4C, 0x20, 0x0D, 0x0A, 0x87, 0x0A };
                if (data.Take(12).SequenceEqual(jxlSignature))
                    return true;
            }

            if (data.Length >= 2)
            {
                // Check for codestream header
                byte[] jxlSignature = { 0xFF, 0x0A };
                if (data.Take(2).SequenceEqual(jxlSignature))
                    return true;
            }

            return false;
        }

        public static byte[] ConvertToJpegXL(Bitmap bitmap, int quality = 70, bool lossless = false, int effort = 7)
        {
            IntPtr encoder = NativeMethods.JxlEncoderCreate(IntPtr.Zero);
            if (encoder == IntPtr.Zero)
                throw new Exception("Failed to create JXL encoder");

            try
            {
                IntPtr frameSettings = NativeMethods.JxlEncoderFrameSettingsCreate(encoder, IntPtr.Zero);

                if (lossless)
                {
                    NativeMethods.JxlEncoderSetFrameLossless(frameSettings, 1);
                }
                else
                {
                    float distance = (100 - quality) * 0.1f;
                    NativeMethods.JxlEncoderSetFrameDistance(frameSettings, distance);
                }

                // Set effort (1-9, default 7)
                NativeMethods.JxlEncoderFrameSettingsSetOption(frameSettings, NativeMethods.JxlEncoderFrameSettingId.Effort, effort);

                bool hasAlpha = bitmap.PixelFormat == PixelFormat.Format32bppArgb;
                var info = new NativeMethods.JxlBasicInfo
                {
                    have_container = 0,
                    xsize = (uint)bitmap.Width, // required 
                    ysize = (uint)bitmap.Height, // required 
                    bits_per_sample = 8, // required 
                    exponent_bits_per_sample = 0,
                    intensity_target = 255.0f, //important for SDR images
                    min_nits = 0.0f,
                    relative_to_max_display = 0,
                    linear_below = 0.0f,
                    uses_original_profile = 0,
                    have_preview = 0,
                    have_animation = 0,
                    orientation = 1, // required 
                    num_color_channels = 3,// required  
                    num_extra_channels = hasAlpha ? 1u : 0u, // required 
                    alpha_bits = hasAlpha ? 8u : 0u, // important if hasAlpha is true
                    alpha_exponent_bits = 0,
                    alpha_premultiplied = 0,
                    preview_xsize = 0,
                    preview_ysize = 0,
                    animation_tps_numerator = 0,
                    animation_tps_denominator = 0,
                    animation_num_loops = 0,
                    animation_have_timecodes = 0,
                    intrinsic_xsize = (uint)bitmap.Width,
                    intrinsic_ysize = (uint)bitmap.Height,
                    padding = new byte[4]
                };

                if (NativeMethods.JxlEncoderSetBasicInfo(encoder, ref info) != NativeMethods.JxlEncoderStatus.Success)
                    throw new Exception("Failed to set basic info");

                int bytesPerPixel = hasAlpha ? 4 : 3;
                int bufferSize = bitmap.Width * bitmap.Height * bytesPerPixel;
                IntPtr pixels = Marshal.AllocHGlobal(bufferSize);

                try
                {
                    BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
                    try
                    {
                        int dstStride = bitmap.Width * bytesPerPixel;
                        int srcBytesPerPixel = hasAlpha ? 4 : 3;

                        unsafe
                        {
                            CopyAndSwapChannels(bitmap.Width, bitmap.Height, bmpData.Stride, dstStride,
                                (byte*)bmpData.Scan0, (byte*)pixels, hasAlpha, srcBytesPerPixel, bytesPerPixel, false);
                        }
                    }
                    finally
                    {
                        bitmap.UnlockBits(bmpData);
                    }

                    var format = new NativeMethods.JxlPixelFormat
                    {
                        num_channels = (uint)bytesPerPixel,
                        data_type = NativeMethods.JxlDataType.Uint8,
                        endianness = 0,
                        align = UIntPtr.Zero
                    };

                    if (NativeMethods.JxlEncoderAddImageFrame(frameSettings, ref format, pixels, new UIntPtr((ulong)bufferSize)) != NativeMethods.JxlEncoderStatus.Success)
                        throw new Exception("Failed to add image frame");

                    NativeMethods.JxlEncoderCloseInput(encoder);

                    var outputList = new List<byte>();
                    byte[] chunk = new byte[64 * 1024];

                    while (true)
                    {
                        IntPtr nextOut;
                        UIntPtr availOut;
                        var status = NativeMethods.JxlEncoderProcessOutput(encoder, out nextOut, out availOut);

                        if (status == NativeMethods.JxlEncoderStatus.NeedMoreOutput)
                        {
                            int available = (int)availOut;
                            Marshal.Copy(nextOut, chunk, 0, Math.Min(available, chunk.Length));
                            outputList.AddRange(chunk.Take(available));
                        }
                        else if (status == NativeMethods.JxlEncoderStatus.Success)
                        {
                            break;
                        }
                        else
                        {
                            throw new Exception($"Encoder error: {status}");
                        }
                    }

                    return outputList.ToArray();
                }
                finally
                {
                    Marshal.FreeHGlobal(pixels);
                }
            }
            finally
            {
                NativeMethods.JxlEncoderDestroy(encoder);
            }
        }

        #region Common
        private static unsafe void CopyAndSwapChannels(int width, int height, int srcStride, int dstStride, byte* srcScan0, byte* dstScan0, bool hasAlpha, int srcBytesPerPixel, int dstBytesPerPixel, bool rgbToBgr)
        {
            for (int y = 0; y < height; y++)
            {
                byte* srcRow = srcScan0 + (y * srcStride);
                byte* dstRow = dstScan0 + (y * dstStride);

                for (int x = 0; x < width; x++)
                {
                    int srcPos = x * srcBytesPerPixel;
                    int dstPos = x * dstBytesPerPixel;

                    if (rgbToBgr)
                    {
                        // RGB(A) to BGR(A)
                        dstRow[dstPos + 0] = srcRow[srcPos + 2]; // B
                        dstRow[dstPos + 1] = srcRow[srcPos + 1]; // G
                        dstRow[dstPos + 2] = srcRow[srcPos + 0]; // R
                        if (hasAlpha)
                            dstRow[dstPos + 3] = srcRow[srcPos + 3]; // A
                    }
                    else
                    {
                        // BGR(A) to RGB(A)
                        dstRow[dstPos + 0] = srcRow[srcPos + 2]; // R
                        dstRow[dstPos + 1] = srcRow[srcPos + 1]; // G
                        dstRow[dstPos + 2] = srcRow[srcPos + 0]; // B
                        if (hasAlpha)
                            dstRow[dstPos + 3] = srcRow[srcPos + 3]; // A
                    }
                }
            }
        }
        #endregion
    }
}
