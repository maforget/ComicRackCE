using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Threading;
using Tao.OpenGl;

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
						Gl.glBindTexture(3553, Name);
					}
					else
					{
						Bitmap bitmap = Bitmap.Bitmap;
						using (ItemMonitor.Lock(bitmap))
						{
							try
							{
								Gl.glGenTextures(1, name);
								Gl.glBindTexture(3553, Name);
								Gl.glTexParameteri(3553, 10242, 33071);
								Gl.glTexParameteri(3553, 10243, 33071);
								Rectangle texturePartBounds = GetTexturePartBounds(bitmap, part, !manager.Settings.IsSquareTextures);
								textureTileGlSize = texturePartBounds.Size;
								using (FastBitmapLock fastBitmapLock = new FastBitmapLock(bitmap, texturePartBounds))
								{
									int num = bitmap.Width * bitmap.Height;
									int num2 = texturePartBounds.Width * texturePartBounds.Height;
									int internalformat = 6408;
									memory = num2 * 4;
									if (manager.Settings.IsTextureCompression)
									{
										internalformat = 34030;
										memory = num2 * 2;
									}
									if (manager.Settings.IsBigTexturesAs16Bit && num > 800000)
									{
										internalformat = 32848;
										memory = num2 * 2;
									}
									if (manager.Settings.IsBigTexturesAs24Bit && num > 800000)
									{
										internalformat = 32849;
										memory = num2 * 3;
									}
									if (manager.IsOptimizedTexture)
									{
										Gl.glTexParameteri(3553, 10241, 9728);
										Gl.glTexParameteri(3553, 10240, 9728);
										if (Gl.glGetError() != 0)
										{
											Gl.glTexParameteri(3553, 10240, 9729);
										}
									}
									else
									{
										Gl.glGetTexParameteriv(3553, 33169, out var _);
										if (Gl.glGetError() != 0 || !manager.Settings.IsMipMapFilter || !manager.EnableFilter)
										{
											Gl.glTexParameteri(3553, 10241, 9729);
										}
										else
										{
											Gl.glTexParameteri(3553, 33169, 1);
											Gl.glTexParameteri(3553, 10241, 9987);
											if (manager.Settings.IsAnisotropicFilter)
											{
												float[] array = new float[1];
												Gl.glGetFloatv(34047, array);
												Gl.glTexParameterf(3553, 34046, array[0]);
											}
										}
										Gl.glTexParameteri(3553, 10240, 9729);
									}
									Gl.glTexImage2D(3553, 0, internalformat, fastBitmapLock.Width, fastBitmapLock.Height, 0, 32993, 5121, fastBitmapLock.Data);
									if (Gl.glGetError() == 1285)
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
						Gl.glDeleteTextures(1, name);
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
			Gl.glPushMatrix();
			Gl.glPushAttrib(286721);
			try
			{
				Gl.glEnable(3553);
				Gl.glTexEnvf(8960, 8704, 8448f);
				Gl.glColor4f(1f, 1f, 1f, opacity);
				Gl.glTranslatef(dest.X, dest.Y, 0f);
				Gl.glScalef(dest.Width / src.Width, dest.Height / src.Height, 0f);
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
								Gl.glBegin(7);
								Gl.glTexCoord2f(rectangleF2.Left, rectangleF2.Top);
								Gl.glVertex2f(rectangleF.Left, rectangleF.Top);
								Gl.glTexCoord2f(rectangleF2.Right, rectangleF2.Top);
								Gl.glVertex2f(rectangleF.Right, rectangleF.Top);
								Gl.glTexCoord2f(rectangleF2.Right, rectangleF2.Bottom);
								Gl.glVertex2f(rectangleF.Right, rectangleF.Bottom);
								Gl.glTexCoord2f(rectangleF2.Left, rectangleF2.Bottom);
								Gl.glVertex2f(rectangleF.Left, rectangleF.Bottom);
								Gl.glEnd();
							}
						}
					}
				}
			}
			finally
			{
				Gl.glPopAttrib();
				Gl.glPopMatrix();
			}
		}
	}
}
