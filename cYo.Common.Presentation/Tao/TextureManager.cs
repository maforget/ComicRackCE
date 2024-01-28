using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Threading;
using OpenTK.Graphics.OpenGL;
//using Tao.OpenGl;

namespace cYo.Common.Presentation.Tao
{
    public class TextureManager : DisposableObject
    {
        private class TextureElement : DisposableObject
        {
            private readonly int part;

            private readonly TextureManager manager;

            private readonly int[] name = new int[1];

            private readonly RendererImage bitmap;

            private int memory;

            private Size textureTileGlSize;

            public int Name => name[0];

            public RendererImage Bitmap => bitmap;

            public int Memory => memory;

            public Rectangle TextureTileBounds => GetTexturePartBounds(Bitmap, part, clamp: true);

            public Size TextureTileGlSize => textureTileGlSize;

            public RectangleF TextureTileCoord
            {
                get
                {
                    RectangleF rectangleF = TextureTileBounds;
                    Size size = TextureTileGlSize;
                    return new RectangleF(0f, 0f, rectangleF.Width / (float)size.Width, rectangleF.Height / (float)size.Height);
                }
            }

            public TextureElement(TextureManager manager, RendererImage bitmap, int part)
            {
                this.manager = manager;
                this.bitmap = bitmap;
                this.part = part;
            }

            protected override void Dispose(bool disposing)
            {
                Destroy();
                base.Dispose(disposing);
            }

            public bool Bind()
            {
                using (ItemMonitor.Lock(Bitmap))
                {
                    if (!IsValid(Bitmap))
                    {
                        return false;
                    }
                    if (Name != 0)
                    {
                        GL.BindTexture(TextureTarget.Texture2D, Name);
                    }
                    else
                    {
                        Bitmap bitmap = Bitmap.Bitmap;
                        using (ItemMonitor.Lock(bitmap))
                        {
                            try
                            {
                                GL.GenTextures(1, name);
                                GL.BindTexture(TextureTarget.Texture2D, Name);
                                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                                Rectangle texturePartBounds = GetTexturePartBounds(bitmap, part, !manager.Settings.IsSquareTextures);
                                textureTileGlSize = texturePartBounds.Size;
                                using (FastBitmapLock fastBitmapLock = new FastBitmapLock(bitmap, texturePartBounds))
                                {
                                    int num = bitmap.Width * bitmap.Height;
                                    int num2 = texturePartBounds.Width * texturePartBounds.Height;
                                    PixelInternalFormat internalformat = PixelInternalFormat.Rgba;
                                    memory = num2 * 4;
                                    if (manager.Settings.IsTextureCompression)
                                    {
                                        internalformat = PixelInternalFormat.CompressedRgba;
                                        memory = num2 * 2;
                                    }
                                    if (manager.Settings.IsBigTexturesAs16Bit && num > 800000)
                                    {
                                        internalformat = PixelInternalFormat.Rgb5;
                                        memory = num2 * 2;
                                    }
                                    if (manager.Settings.IsBigTexturesAs24Bit && num > 800000)
                                    {
                                        internalformat = PixelInternalFormat.Rgb8;
                                        memory = num2 * 3;
                                    }
                                    if (manager.IsOptimizedTexture)
                                    {
                                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Nearest);
                                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                                        if (GL.GetError() != ErrorCode.NoError)
                                        {
                                            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                                        }
                                    }
                                    else
                                    {
                                        GL.GetTexParameter(TextureTarget.Texture2D, GetTextureParameter.GenerateMipmap, out int _);

                                        if (GL.GetError() != ErrorCode.NoError || !manager.Settings.IsMipMapFilter || !manager.EnableFilter)
                                        {
                                            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);
                                        }
                                        else
                                        {
                                            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.GenerateMipmap, 1);
                                            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);

                                            if (manager.Settings.IsAnisotropicFilter)
                                            {
                                                float[] array = new float[1];
                                                GL.GetFloat(GetPName.MaxTextureMaxAnisotropy, array);
                                                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxAnisotropy, array[0]);
                                            }
                                        }
                                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                                    }

                                    GL.TexImage2D(TextureTarget.Texture2D, 0, internalformat, fastBitmapLock.Width, fastBitmapLock.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, fastBitmapLock.Data);

                                    if (GL.GetError() == ErrorCode.OutOfMemory)
                                    {
                                        throw new InvalidOperationException();
                                    }
                                }
                            }

                            catch (Exception)
                            {
                                Destroy();
                                return false;
                            }
                        }
                    }
                }
                return true;
            }

            public void Destroy()
            {
                if (Name != 0)
                {
                    try
                    {
                        GL.DeleteTextures(1, name);
                    }
                    catch (Exception)
                    {
                    }
                    name[0] = (memory = 0);
                }
            }

            private Rectangle GetTexturePartBounds(RendererImage bmp, int part, bool clamp)
            {
                if (!IsValid(bmp))
                {
                    return Rectangle.Empty;
                }
                return GetPartBounds(bmp.Size, part, manager.Settings.MinTextureTileSize, manager.Settings.MaxTextureTileSize, clamp);
            }

            private static Rectangle GetPartBounds(Size fullSize, int part, int minPartSize, int maxPartSize, bool clamp)
            {
                Size gridSize = GetGridSize(fullSize, maxPartSize);
                if (gridSize.IsEmpty)
                {
                    return Rectangle.Empty;
                }
                int num = part % gridSize.Width;
                int num2 = part / gridSize.Width;
                Rectangle result = new Rectangle(num * maxPartSize, num2 * maxPartSize, maxPartSize, maxPartSize);
                int num3 = ((result.Right > fullSize.Width) ? (fullSize.Width - result.X) : result.Width);
                int num4 = ((result.Height > fullSize.Height) ? (fullSize.Height - result.Y) : result.Height);
                if (clamp)
                {
                    result.Width = num3;
                    result.Height = num4;
                }
                else
                {
                    result.Width = NearestSquare(maxPartSize, Math.Max(num3, minPartSize));
                    result.Height = NearestSquare(maxPartSize, Math.Max(num4, minPartSize));
                }
                return result;
            }

            public override bool Equals(object obj)
            {
                TextureElement textureElement = obj as TextureElement;
                if (textureElement != null && part == textureElement.part)
                {
                    return object.Equals(Bitmap, textureElement.Bitmap);
                }
                return false;
            }

            public override int GetHashCode()
            {
                return part;
            }

            public static bool IsValid(RendererImage bmp)
            {
                using (ItemMonitor.Lock(bmp))
                {
                    try
                    {
                        return bmp != null && bmp.IsValid && bmp.Width != 0 && bmp.Height != 0;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            public static int NearestSquare(int value, int target)
            {
                int result = value;
                while (value > target)
                {
                    result = value;
                    value >>= 1;
                }
                return result;
            }

            public static Size GetGridSize(Size fullSize, int partSize)
            {
                return new Size((fullSize.Width - 1) / partSize + 1, (fullSize.Height - 1) / partSize + 1);
            }

            public static Size GetTextureGridSize(RendererImage bmp, int partSize)
            {
                if (!IsValid(bmp))
                {
                    return Size.Empty;
                }
                return GetGridSize(bmp.Size, partSize);
            }
        }

        private readonly LinkedList<TextureElement> textures = new LinkedList<TextureElement>();

        public TextureManagerSettings Settings
        {
            get;
            set;
        }

        public bool IsOptimizedTexture
        {
            get;
            set;
        }

        public bool EnableFilter
        {
            get;
            set;
        }

        protected override void Dispose(bool disposing)
        {
            textures.Dispose();
            base.Dispose(disposing);
        }

        private void CleanUp()
        {
            LinkedListNode<TextureElement> linkedListNode = textures.Last;
            int num = GetTotalMemory();
            int num2 = Settings.MaxTextureMemoryMB * 1024 * 1024;
            int num3 = textures.Count;
            while (linkedListNode != null)
            {
                LinkedListNode<TextureElement> linkedListNode2 = linkedListNode;
                TextureElement value = linkedListNode2.Value;
                linkedListNode = linkedListNode.Previous;
                if (value.Bitmap == null || !value.Bitmap.IsValid || num > num2 || num3 > Settings.MaxTextureCount)
                {
                    int memory = value.Memory;
                    value.Dispose();
                    textures.Remove(linkedListNode2);
                    num -= memory;
                    num3--;
                }
            }
        }

        private int GetTotalMemory()
        {
            return textures.Sum((TextureElement te) => te.Memory);
        }

        private TextureElement GetTextureElement(RendererImage bmp, int part)
        {
            TextureElement textureElement = new TextureElement(this, bmp, part);
            CleanUp();
            LinkedListNode<TextureElement> linkedListNode = textures.Find(textureElement);
            if (linkedListNode == null)
            {
                linkedListNode = textures.AddFirst(textureElement);
            }
            else
            {
                textureElement.Dispose();
                textures.Remove(linkedListNode);
                textures.AddFirst(linkedListNode);
            }
            return linkedListNode.Value;
        }

        private TextureElement BindTexturePart(RendererImage bmp, int n)
        {
            TextureElement textureElement = GetTextureElement(bmp, n);
            while (textures.Count > 0)
            {
                if (textureElement.Bind())
                {
                    return textureElement;
                }
                textures.Last.Value.Destroy();
                textures.RemoveLast();
            }
            return null;
        }

        public void DrawImage(RendererImage image, RectangleF dest, RectangleF src, float opacity)
        {
            if (opacity < 0.05f)
            {
                return;
            }
            GL.PushMatrix();
            GL.PushAttrib(AttribMask.CurrentBit | AttribMask.EnableBit | AttribMask.ColorBufferBit | AttribMask.TextureBit);
            try
            {
                GL.Enable(EnableCap.Texture2D);
                GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvMode.Modulate);
                GL.Color4(1f, 1f, 1f, opacity);
                GL.Translate(dest.X, dest.Y, 0f);
                GL.Scale(dest.Width / src.Width, dest.Height / src.Height, 0f);
                Size textureGridSize = TextureElement.GetTextureGridSize(image, Settings.MaxTextureTileSize);
                for (int i = 0; i < textureGridSize.Height; i++)
                {
                    for (int j = 0; j < textureGridSize.Width; j++)
                    {
                        TextureElement textureElement = BindTexturePart(image, i * textureGridSize.Width + j);
                        if (textureElement != null)
                        {
                            RectangleF a = textureElement.TextureTileBounds;
                            RectangleF rectangleF = RectangleF.Intersect(a, src);
                            if (!rectangleF.IsEmpty)
                            {
                                RectangleF textureTileCoord = textureElement.TextureTileCoord;
                                RectangleF rectangleF2 = new RectangleF(textureTileCoord.X + textureTileCoord.Width * (rectangleF.X - a.X) / a.Width, textureTileCoord.Y + textureTileCoord.Height * (rectangleF.Y - a.Y) / a.Height, textureTileCoord.Width * rectangleF.Width / a.Width, textureTileCoord.Height * rectangleF.Height / a.Height);
                                rectangleF.X -= src.X;
                                rectangleF.Y -= src.Y;
                                GL.Begin(PrimitiveType.Quads);
                                GL.TexCoord2(rectangleF2.Left, rectangleF2.Top);
                                GL.Vertex2(rectangleF.Left, rectangleF.Top);
                                GL.TexCoord2(rectangleF2.Right, rectangleF2.Top);
                                GL.Vertex2(rectangleF.Right, rectangleF.Top);
                                GL.TexCoord2(rectangleF2.Right, rectangleF2.Bottom);
                                GL.Vertex2(rectangleF.Right, rectangleF.Bottom);
                                GL.TexCoord2(rectangleF2.Left, rectangleF2.Bottom);
                                GL.Vertex2(rectangleF.Left, rectangleF.Bottom);
                                GL.End();
                            }
                        }
                    }
                }
            }
            finally
            {
                GL.PopAttrib();
                GL.PopMatrix();
            }
        }
    }
}
