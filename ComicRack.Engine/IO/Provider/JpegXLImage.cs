using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using cYo.Common.Drawing;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
    /// <summary>
    /// Provides JPEG XL encoding and decoding functionality using libjxl.
    /// </summary>
    public static class JpegXLImage
    {
        #region Native Methods

        public static class NativeMethods
        {
            private const string JxlLibrary = "jxl.dll";

            #region Decoder Functions

            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr JxlDecoderCreate(IntPtr memoryManager);

            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern void JxlDecoderDestroy(IntPtr decoder);

            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern JxlDecoderStatus JxlDecoderSubscribeEvents(IntPtr decoder, int eventsWanted);

            [CLSCompliant(false)]
            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern JxlDecoderStatus JxlDecoderSetInput(IntPtr decoder, byte[] data, UIntPtr size);

            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern JxlDecoderStatus JxlDecoderProcessInput(IntPtr decoder);

            [CLSCompliant(false)]
            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern JxlDecoderStatus JxlDecoderGetBasicInfo(IntPtr decoder, out JxlBasicInfo info);

            [CLSCompliant(false)]
            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern JxlDecoderStatus JxlDecoderImageOutBufferSize(IntPtr decoder, ref JxlPixelFormat format, out UIntPtr size);

            [CLSCompliant(false)]
            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern JxlDecoderStatus JxlDecoderSetImageOutBuffer(IntPtr decoder, ref JxlPixelFormat format, IntPtr buffer, UIntPtr size);

            #endregion

            #region Encoder Functions

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

            [CLSCompliant(false)]
            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern JxlEncoderStatus JxlEncoderSetBasicInfo(IntPtr encoder, ref JxlBasicInfo info);

            [CLSCompliant(false)]
            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern JxlEncoderStatus JxlEncoderAddImageFrame(IntPtr frameSettings, ref JxlPixelFormat format, IntPtr buffer, UIntPtr size);

            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern void JxlEncoderCloseFrames(IntPtr encoder);

            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern void JxlEncoderCloseInput(IntPtr encoder);

            [CLSCompliant(false)]
            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static unsafe extern JxlEncoderStatus JxlEncoderProcessOutput(IntPtr encoder, byte** nextOut, UIntPtr* availOut);

            [CLSCompliant(false)]
            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern void JxlColorEncodingSetToSRGB(ref JxlColorEncoding colorEncoding, int isGray);

            [CLSCompliant(false)]
            [DllImport(JxlLibrary, CallingConvention = CallingConvention.Cdecl)]
            public static extern JxlEncoderStatus JxlEncoderSetColorEncoding(IntPtr encoder, ref JxlColorEncoding colorEncoding);

            #endregion

            #region Enums

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
                /// <summary>
                /// Sets encoder effort/speed level without affecting decoding speed. Valid
                /// values are, from faster to slower speed: 1:lightning 2:thunder 3:falcon
                /// 4:cheetah 5:hare 6:wombat 7:squirrel 8:kitten 9:tortoise.
                /// Default: squirrel (7).
                /// </summary>
                Effort = 0,

                /// <summary>
                /// Sets the decoding speed tier for the provided options. Minimum is 0
                /// (slowest to decode, best quality/density), and maximum is 4 (fastest to
                /// decode, at the cost of some quality/density). Default is 0.
                /// </summary>
                DecodingSpeed = 1,
            }

            #endregion

            #region Structures

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

            [CLSCompliant(false)]
            [StructLayout(LayoutKind.Sequential)]
            public struct JxlColorEncoding
            {
                public uint color_space;        // JxlColorSpace
                public uint white_point;        // JxlWhitePoint
                public uint primaries;          // JxlPrimaries
                public uint transfer_function;  // JxlTransferFunction
                public uint rendering_intent;   // JxlRenderingIntent
            }

            #endregion
        }

        #endregion

        #region API

        /// <summary>
        /// Converts a JPEG XL byte array to JPEG format.
        /// </summary>
        /// <param name="data">JPEG XL image data</param>
        /// <returns>JPEG image data, or original data if not a valid JXL file</returns>
        public static byte[] ConvertToJpeg(byte[] data)
        {
            if (!IsSupported(data))
                return data;

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

        /// <summary>
        /// Decodes JPEG XL data into a Bitmap.
        /// </summary>
        private static Bitmap DecodeFromBytes(byte[] data)
        {
            IntPtr decoder = NativeMethods.JxlDecoderCreate(IntPtr.Zero);
            if (decoder == IntPtr.Zero)
                throw new Exception("Failed to create JXL decoder");

            try
            {
                // Subscribe to basic info and full image events
                const int eventsWanted = (int)(NativeMethods.JxlDecoderStatus.BasicInfo | NativeMethods.JxlDecoderStatus.FullImage);
                CheckDecoderStatus(
                    NativeMethods.JxlDecoderSubscribeEvents(decoder, eventsWanted),
                    "Failed to subscribe to decoder events");

                // Set input data
                CheckDecoderStatus(
                    NativeMethods.JxlDecoderSetInput(decoder, data, new UIntPtr((ulong)data.Length)),
                    "Failed to set decoder input");

                // Process decoder events and get image
                return ProcessDecoderEvents(decoder);
            }
            finally
            {
                NativeMethods.JxlDecoderDestroy(decoder);
            }
        }

        /// <summary>
        /// Checks if the data is a valid JPEG XL file.
        /// </summary>
        public static bool IsSupported(byte[] data)
        {
            if (data == null || data.Length < 2 || !Environment.Is64BitProcess)
                return false;

            // Check for JXL container format header
            if (data.Length >= 12)
            {
                byte[] jxlSignature = { 0x00, 0x00, 0x00, 0x0C, 0x4A, 0x58, 0x4C, 0x20, 0x0D, 0x0A, 0x87, 0x0A };
                if (data.Take(12).SequenceEqual(jxlSignature))
                    return true;
            }

            // Check for JXL codestream header
            if (data.Length >= 2)
            {
                byte[] jxlSignature = { 0xFF, 0x0A };
                if (data.Take(2).SequenceEqual(jxlSignature))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Converts a Bitmap to JPEG XL format.
        /// </summary>
        /// <param name="bitmap">Source bitmap</param>
        /// <param name="quality">Quality level 0-100 (only for lossy compression)</param>
        /// <param name="lossless">Use lossless compression</param>
        /// <param name="effort">Compression effort 1-9 (higher = better compression but slower)</param>
        /// <returns>JPEG XL encoded data</returns>
        public static byte[] ConvertToJpegXL(Bitmap bitmap, int quality = 70, bool lossless = true, int effort = 7)
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));

            IntPtr encoder = NativeMethods.JxlEncoderCreate(IntPtr.Zero);
            if (encoder == IntPtr.Zero)
                throw new Exception("Failed to create JXL encoder");

            try
            {
                // Configure encoder with image metadata
                ConfigureEncoder(encoder, bitmap);

                // Get frame settings and configure quality/effort
                IntPtr frameSettings = NativeMethods.JxlEncoderFrameSettingsCreate(encoder, IntPtr.Zero);
                ConfigureFrameSettings(frameSettings, quality, lossless, effort);

                // Convert bitmap to raw pixel data and add to encoder
                AddBitmapFrame(encoder, frameSettings, bitmap);

                // Finalize and get encoded output
                return GetEncodedOutput(encoder);
            }
            finally
            {
                NativeMethods.JxlEncoderDestroy(encoder);
            }
        }

        #endregion

        #region Decoding

        /// <summary>
        /// Processes decoder events and extracts the image.
        /// </summary>
        private static Bitmap ProcessDecoderEvents(IntPtr decoder)
        {
            NativeMethods.JxlBasicInfo info = default;
            IntPtr pixels = IntPtr.Zero;
            int width = 0, height = 0;
            bool hasAlpha = false;

            try
            {
                while (true)
                {
                    var status = NativeMethods.JxlDecoderProcessInput(decoder);

                    switch (status)
                    {
                        case NativeMethods.JxlDecoderStatus.BasicInfo:
                            CheckDecoderStatus(
                                NativeMethods.JxlDecoderGetBasicInfo(decoder, out info),
                                "Failed to get basic info");

                            width = (int)info.xsize;
                            height = (int)info.ysize;
                            hasAlpha = info.num_extra_channels > 0;
                            break;

                        case NativeMethods.JxlDecoderStatus.NeedImageOutBuffer:
                            pixels = AllocateDecoderBuffer(decoder, width, height, hasAlpha);
                            break;

                        case NativeMethods.JxlDecoderStatus.FullImage:
                        case NativeMethods.JxlDecoderStatus.Success:
                            return ConvertPixelsToBitmap(pixels, width, height, hasAlpha);

                        default:
                            throw new Exception($"Decoder error: {status}");
                    }
                }
            }
            finally
            {
                if (pixels != IntPtr.Zero)
                    Marshal.FreeHGlobal(pixels);
            }
        }

        /// <summary>
        /// Allocates buffer for decoded image data.
        /// </summary>
        private static IntPtr AllocateDecoderBuffer(IntPtr decoder, int width, int height, bool hasAlpha)
        {
            var format = new NativeMethods.JxlPixelFormat
            {
                num_channels = hasAlpha ? 4u : 3u,
                data_type = NativeMethods.JxlDataType.Uint8,
                endianness = 0,
                align = UIntPtr.Zero
            };

            CheckDecoderStatus(
                NativeMethods.JxlDecoderImageOutBufferSize(decoder, ref format, out UIntPtr bufferSize),
                "Failed to get buffer size");

            IntPtr pixels = Marshal.AllocHGlobal((int)bufferSize);

            CheckDecoderStatus(
                NativeMethods.JxlDecoderSetImageOutBuffer(decoder, ref format, pixels, bufferSize),
                "Failed to set output buffer");

            return pixels;
        }

        /// <summary>
        /// Converts raw pixel data to a Bitmap, swapping RGB to BGR.
        /// </summary>
        private static Bitmap ConvertPixelsToBitmap(IntPtr pixels, int width, int height, bool hasAlpha)
        {
            if (pixels == IntPtr.Zero)
                throw new InvalidOperationException("No pixel data available");

            var pixelFormat = hasAlpha ? PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb;
            Bitmap bitmap = new Bitmap(width, height, pixelFormat);

            BitmapData bmpData = bitmap.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly,
                pixelFormat);

            try
            {
                int srcBytesPerPixel = hasAlpha ? 4 : 3;
                int srcStride = width * srcBytesPerPixel;
                int dstBytesPerPixel = hasAlpha ? 4 : 3;

                unsafe
                {
                    // Convert RGB(A) from JXL to BGR(A) for Bitmap
                    CopyAndSwapChannels(
                        width, height,
                        srcStride, bmpData.Stride,
                        (byte*)pixels, (byte*)bmpData.Scan0,
                        hasAlpha, srcBytesPerPixel, dstBytesPerPixel,
                        rgbToBgr: true);
                }

                return bitmap;
            }
            catch
            {
                bitmap.Dispose();
                throw;
            }
            finally
            {
                bitmap.UnlockBits(bmpData);
            }
        }

        #endregion

        #region Encoding

        /// <summary>
        /// Configures encoder with basic image information.
        /// </summary>
        private static void ConfigureEncoder(IntPtr encoder, Bitmap bitmap)
        {
            bool hasAlpha = bitmap.PixelFormat == PixelFormat.Format32bppArgb;

            var info = new NativeMethods.JxlBasicInfo
            {
                have_container = 0,
                xsize = (uint)bitmap.Width, // required 
                ysize = (uint)bitmap.Height, // required 
                bits_per_sample = 8, // required 
                exponent_bits_per_sample = 0,
                intensity_target = 255.0f, // SDR brightness
                min_nits = 0.0f,
                relative_to_max_display = 0,
                linear_below = 0.0f,
                uses_original_profile = 0, // setting this to 1 when lossless will include the original color profile instead of simply sRGB. But it increases size by 250%, not sure it is worth it especially since it requires special decoding options
                have_preview = 0,
                have_animation = 0,
                orientation = 1, // required to prevent "Failed to set basic info"
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

            CheckEncoderStatus(
                NativeMethods.JxlEncoderSetBasicInfo(encoder, ref info),
                "Failed to set basic info");

            // Set sRGB color encoding
            var colorEncoding = new NativeMethods.JxlColorEncoding();
            NativeMethods.JxlColorEncodingSetToSRGB(ref colorEncoding, 0);

            CheckEncoderStatus(
                NativeMethods.JxlEncoderSetColorEncoding(encoder, ref colorEncoding),
                "Failed to set color encoding");
        }

        /// <summary>
        /// Configures frame settings for quality and compression.
        /// </summary>
        private static void ConfigureFrameSettings(IntPtr frameSettings, int quality, bool lossless, int effort)
        {
            if (lossless)
            {
                // this overrides a set of existing options (such as distance, modular mode and color transform) that enables bit-for-bit lossless encoding.
                NativeMethods.JxlEncoderSetFrameLossless(frameSettings, 1);
            }
            else
            {
                // Convert quality (0-100) to distance (0.0-15.0)
                // 0.0 = mathematically lossless, 1.0 = visually lossless, recommended range: 0.5 to 3.0 (95% to 67%)
                float distance = 0.1f + (100 - quality) * 0.09f;
                CheckEncoderStatus(
                    NativeMethods.JxlEncoderSetFrameDistance(frameSettings, distance),
                   "Failed to set frame distance");
            }

            // Set compression effort (1-9)
            CheckEncoderStatus(
                NativeMethods.JxlEncoderFrameSettingsSetOption(
                    frameSettings,
                    NativeMethods.JxlEncoderFrameSettingId.Effort,
                    effort),
               "Failed to set effort");
        }

        /// <summary>
        /// Converts bitmap to raw pixels and adds to encoder.
        /// </summary>
        private static void AddBitmapFrame(IntPtr encoder, IntPtr frameSettings, Bitmap bitmap)
        {
            bool hasAlpha = bitmap.PixelFormat == PixelFormat.Format32bppArgb;
            int bytesPerPixel = hasAlpha ? 4 : 3;
            int stride = bitmap.Width * bytesPerPixel;
            int bufferSize = bitmap.Height * stride;

            IntPtr pixels = Marshal.AllocHGlobal(bufferSize);
            try
            {
                // Lock bitmap and copy pixel data
                BitmapData bmpData = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly,
                    bitmap.PixelFormat);

                try
                {
                    int srcBytesPerPixel = hasAlpha ? 4 : 3;

                    unsafe
                    {
                        // Convert BGR(A) from Bitmap to RGB(A) for JXL
                        CopyAndSwapChannels(
                            bitmap.Width, bitmap.Height,
                            bmpData.Stride, stride,
                            (byte*)bmpData.Scan0, (byte*)pixels,
                            hasAlpha, srcBytesPerPixel, bytesPerPixel,
                            rgbToBgr: false);
                    }
                }
                finally
                {
                    bitmap.UnlockBits(bmpData);
                }

                // Configure pixel format
                var format = new NativeMethods.JxlPixelFormat
                {
                    num_channels = (uint)bytesPerPixel,
                    data_type = NativeMethods.JxlDataType.Uint8,
                    endianness = 0,
                    align = new UIntPtr((ulong)stride) // Must match stride
                };

                // Add frame to encoder
                CheckEncoderStatus(
                    NativeMethods.JxlEncoderAddImageFrame(frameSettings, ref format, pixels, new UIntPtr((ulong)bufferSize)),
                    "Failed to add image frame");
            }
            finally
            {
                if (pixels != IntPtr.Zero)
                    Marshal.FreeHGlobal(pixels);
            }
        }

        /// <summary>
        /// Finalizes encoding and retrieves output data.
        /// </summary>
        private static byte[] GetEncodedOutput(IntPtr encoder)
        {
            // Finalize the encoding
            NativeMethods.JxlEncoderCloseFrames(encoder);
            NativeMethods.JxlEncoderCloseInput(encoder);

            var outputList = new List<byte>();
            byte[] outputBuffer = new byte[64 * 1024];

            GCHandle handle = GCHandle.Alloc(outputBuffer, GCHandleType.Pinned);
            try
            {
                unsafe
                {
                    byte* bufferPtr = (byte*)handle.AddrOfPinnedObject();

                    while (true)
                    {
                        byte* nextOut = bufferPtr;
                        UIntPtr availOut = new UIntPtr((ulong)outputBuffer.Length);

                        var status = NativeMethods.JxlEncoderProcessOutput(encoder, &nextOut, &availOut);

                        // Calculate bytes written (nextOut advances as data is written)
                        int bytesWritten = (int)(nextOut - bufferPtr);

                        if (bytesWritten > 0)
                            outputList.AddRange(outputBuffer.Take(bytesWritten));

                        if (status == NativeMethods.JxlEncoderStatus.Success)
                            break;
                        else if (status == NativeMethods.JxlEncoderStatus.NeedMoreOutput)
                            continue;
                        else
                            throw new Exception($"Encoder error: {status}");
                    }
                }
            }
            finally
            {
                handle.Free();
            }

            return outputList.ToArray();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Copies and swaps RGB/BGR channels between buffers.
        /// </summary>
        private static unsafe void CopyAndSwapChannels(
            int width, int height,
            int srcStride, int dstStride,
            byte* srcScan0, byte* dstScan0,
            bool hasAlpha,
            int srcBytesPerPixel, int dstBytesPerPixel,
            bool rgbToBgr)
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
                        // RGB(A) to BGR(A) - for decoding
                        dstRow[dstPos + 0] = srcRow[srcPos + 2]; // B
                        dstRow[dstPos + 1] = srcRow[srcPos + 1]; // G
                        dstRow[dstPos + 2] = srcRow[srcPos + 0]; // R
                        if (hasAlpha)
                            dstRow[dstPos + 3] = srcRow[srcPos + 3]; // A
                    }
                    else
                    {
                        // BGR(A) to RGB(A) - for encoding
                        dstRow[dstPos + 0] = srcRow[srcPos + 2]; // R
                        dstRow[dstPos + 1] = srcRow[srcPos + 1]; // G
                        dstRow[dstPos + 2] = srcRow[srcPos + 0]; // B
                        if (hasAlpha)
                            dstRow[dstPos + 3] = srcRow[srcPos + 3]; // A
                    }
                }
            }
        }

        /// <summary>
        /// Validates decoder status and throws on error.
        /// </summary>
        private static void CheckDecoderStatus(NativeMethods.JxlDecoderStatus status, string message)
        {
            if (status != NativeMethods.JxlDecoderStatus.Success)
                throw new Exception($"{message}: {status}");
        }

        /// <summary>
        /// Validates encoder status and throws on error.
        /// </summary>
        private static void CheckEncoderStatus(NativeMethods.JxlEncoderStatus status, string message)
        {
            if (status != NativeMethods.JxlEncoderStatus.Success)
                throw new Exception($"{message}: {status}");
        }

        #endregion
    }
}