using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace cYo.Common.Presentation.Tao
{
	public class ControlOpenGlRenderer : DisposableObject, IControlRenderer, IBitmapRenderer, IDisposable, IHardwareRenderer
	{
		private TextureManager tm;

		private IntPtr deviceContext = IntPtr.Zero;

		private IntPtr renderingContext = IntPtr.Zero;

		private IntPtr windowHandle = IntPtr.Zero;

		private bool isSoftwareRenderer;

		private int errorCode;

		private int sceneCounter;

		private float opacity = 1f;

		private CompositingMode compositingMode;

		private RectangleF clip = Rectangle.Empty;

		private StencilMode stencilMode;

		public int ColorBits
		{
			get;
			set;
		}

		public int AccumBits
		{
			get;
			set;
		}

		public int DepthBits
		{
			get;
			set;
		}

		public int StencilBits
		{
			get;
			set;
		}

		public bool AutoMakeCurrent
		{
			get;
			set;
		}

		public bool AutoSwapBuffers
		{
			get;
			set;
		}

		public bool AutoFinish
		{
			get;
			set;
		}

		public bool AutoReshape
		{
			get;
			set;
		}

		public int ErrorCode => errorCode;

		public Size Size => Control.ClientRectangle.Size;

		public Control Control
		{
			get;
			private set;
		}

		public TextureManagerSettings Settings
		{
			get;
			private set;
		}

		public bool IsLocked => sceneCounter > 0;

		public bool HighQuality
		{
			get;
			set;
		}

		public Matrix Transform
		{
			get
			{
				return GetMatrix(modelView: true);
			}
			set
			{
				float[] array = new float[16];
				float[] elements = value.Elements;
				array[0] = elements[0];
				array[4] = elements[2];
				array[8] = 0f;
				array[12] = elements[4];
				array[1] = elements[1];
				array[5] = elements[3];
				array[9] = 0f;
				array[13] = elements[5];
				array[2] = 0f;
				array[6] = 0f;
				array[10] = 1f;
				array[14] = 0f;
				array[3] = 0f;
				array[7] = 0f;
				array[11] = 0f;
				array[15] = 1f;
				Gl.glLoadMatrixf(array);
			}
		}

		public bool IsHardware => true;

		public float Opacity
		{
			get
			{
				return opacity;
			}
			set
			{
				opacity = value;
			}
		}

		public CompositingMode CompositingMode
		{
			get
			{
				return compositingMode;
			}
			set
			{
				compositingMode = value;
			}
		}

		public RectangleF Clip
		{
			get
			{
				return clip;
			}
			set
			{
				clip = value;
				if (clip.IsEmpty)
				{
					Gl.glDisable(3089);
					return;
				}
				int[] array = new int[4];
				Gl.glEnable(3089);
				using (Matrix matrix = GetMatrix(modelView: true))
				{
					using (Matrix matrix2 = GetMatrix(modelView: false))
					{
						PointF[] array2 = new PointF[4]
						{
							new PointF(clip.X, clip.Y),
							new PointF(clip.Right, clip.Y),
							new PointF(clip.Right, clip.Bottom),
							new PointF(clip.X, clip.Bottom)
						};
						matrix.TransformPoints(array2);
						matrix2.TransformPoints(array2);
						Gl.glGetIntegerv(2978, array);
						Point point = new Point((int)((float)array[0] + (1f + array2[3].X) * (float)array[2] / 2f), (int)((float)array[1] + (1f + array2[3].Y) * (float)array[3] / 2f));
						Point point2 = new Point((int)((float)array[0] + (1f + array2[1].X) * (float)array[2] / 2f), (int)((float)array[1] + (1f + array2[1].Y) * (float)array[3] / 2f));
						Gl.glScissor(point.X, point.Y, point2.X - point.X, point2.Y - point.Y);
					}
				}
			}
		}

		public bool IsSoftwareRenderer => isSoftwareRenderer;

		public StencilMode StencilMode
		{
			get
			{
				return stencilMode;
			}
			set
			{
				switch (value)
				{
				case StencilMode.WriteOne:
					Gl.glEnable(2960);
					Gl.glStencilFunc(519, 1, 1);
					Gl.glStencilOp(7681, 7681, 7681);
					break;
				case StencilMode.WriteNull:
					Gl.glEnable(2960);
					Gl.glStencilFunc(519, 0, 0);
					Gl.glStencilOp(7681, 7681, 7681);
					break;
				case StencilMode.TestNull:
					Gl.glEnable(2960);
					Gl.glStencilFunc(514, 0, 1);
					Gl.glStencilOp(7680, 7680, 7680);
					break;
				case StencilMode.TestOne:
					Gl.glEnable(2960);
					Gl.glStencilFunc(514, 1, 1);
					Gl.glStencilOp(7680, 7680, 7680);
					break;
				default:
					Gl.glDisable(2960);
					break;
				}
				stencilMode = value;
			}
		}

		public bool OptimizedTextures
		{
			get
			{
				return tm.IsOptimizedTexture;
			}
			set
			{
				tm.IsOptimizedTexture = value;
			}
		}

		public bool EnableFilter
		{
			get
			{
				return tm.EnableFilter;
			}
			set
			{
				tm.EnableFilter = value;
			}
		}

		public BlendingOperation BlendingOperation
		{
			get;
			set;
		}

		public event EventHandler Paint;

		public ControlOpenGlRenderer(Control window, bool registerPaint, TextureManagerSettings settings)
		{
			ColorBits = 32;
			AutoReshape = true;
			AutoFinish = true;
			AutoSwapBuffers = true;
			AutoMakeCurrent = true;
			StencilBits = 1;
			SetStyle(window, ControlStyles.ResizeRedraw, enable: true);
			SetStyle(window, ControlStyles.OptimizedDoubleBuffer, enable: false);
			SetStyle(window, ControlStyles.AllPaintingInWmPaint, enable: true);
			SetStyle(window, ControlStyles.UserPaint, enable: true);
			Settings = settings;
			InitializeContexts(window);
			if (registerPaint)
			{
				window.Paint += window_Paint;
			}
			window.Disposed += window_Disposed;
			BlendingOperation = BlendingOperation.Blend;
			OnInitOpenGl();
		}

		protected override void Dispose(bool disposing)
		{
			if (Control != null)
			{
				Control.Paint -= window_Paint;
				Control.Disposed -= window_Disposed;
				Control = null;
			}
			if (tm != null)
			{
				tm.Dispose();
				tm = null;
			}
			DestroyContexts();
		}

		public void MakeCurrent()
		{
			if (!Wgl.wglMakeCurrent(deviceContext, renderingContext))
			{
				throw new InvalidOperationException("Can not activate the GL rendering context.");
			}
		}

		public void SwapBuffers()
		{
			Gdi.SwapBuffersFast(deviceContext);
		}

		public void Draw()
		{
			Control.Invalidate();
		}

		public void ReshapeFunc()
		{
			Gl.glViewport(0, 0, Size.Width, Size.Height);
			Gl.glMatrixMode(5889);
			Gl.glLoadIdentity();
			Gl.glOrtho(0.0, Size.Width, Size.Height, 0.0, 0.0, 1.0);
			Gl.glMatrixMode(5888);
			Gl.glLoadIdentity();
		}

		private void InitializeContexts(Control window)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window", "No valid window handle");
			}
			Control = window;
			if (Control.Handle == IntPtr.Zero)
			{
				throw new InvalidOperationException("Window not created");
			}
			windowHandle = window.Handle;
			Gdi.PIXELFORMATDESCRIPTOR pixelFormatDescriptor = default(Gdi.PIXELFORMATDESCRIPTOR);
			pixelFormatDescriptor.nSize = (short)Marshal.SizeOf((object)pixelFormatDescriptor);
			pixelFormatDescriptor.nVersion = 1;
			pixelFormatDescriptor.dwFlags = 37;
			pixelFormatDescriptor.iPixelType = 0;
			pixelFormatDescriptor.cColorBits = (byte)ColorBits;
			pixelFormatDescriptor.cRedBits = 0;
			pixelFormatDescriptor.cRedShift = 0;
			pixelFormatDescriptor.cGreenBits = 0;
			pixelFormatDescriptor.cGreenShift = 0;
			pixelFormatDescriptor.cBlueBits = 0;
			pixelFormatDescriptor.cBlueShift = 0;
			pixelFormatDescriptor.cAlphaBits = 0;
			pixelFormatDescriptor.cAlphaShift = 0;
			pixelFormatDescriptor.cAccumBits = (byte)AccumBits;
			pixelFormatDescriptor.cAccumRedBits = 0;
			pixelFormatDescriptor.cAccumGreenBits = 0;
			pixelFormatDescriptor.cAccumBlueBits = 0;
			pixelFormatDescriptor.cAccumAlphaBits = 0;
			pixelFormatDescriptor.cDepthBits = (byte)DepthBits;
			pixelFormatDescriptor.cStencilBits = (byte)StencilBits;
			pixelFormatDescriptor.cAuxBuffers = 0;
			pixelFormatDescriptor.iLayerType = 0;
			pixelFormatDescriptor.bReserved = 0;
			pixelFormatDescriptor.dwLayerMask = 0;
			pixelFormatDescriptor.dwVisibleMask = 0;
			pixelFormatDescriptor.dwDamageMask = 0;
			deviceContext = User.GetDC(windowHandle);
			if (deviceContext == IntPtr.Zero)
			{
				throw new InvalidOperationException("Can not create a GL device context.");
			}
			int num = Gdi.ChoosePixelFormat(deviceContext, ref pixelFormatDescriptor);
			if (num == 0)
			{
				throw new InvalidOperationException("Can not find a suitable PixelFormat.");
			}
			if (!Gdi.SetPixelFormat(deviceContext, num, ref pixelFormatDescriptor))
			{
				throw new InvalidOperationException("Can not set the chosen PixelFormat.  Chosen PixelFormat was " + num + ".");
			}
			renderingContext = Wgl.wglCreateContext(deviceContext);
			if (renderingContext == IntPtr.Zero)
			{
				throw new InvalidOperationException("Can not create a GL rendering context.");
			}
			MakeCurrent();
			Wgl.wglDescribePixelFormat(deviceContext, num, Marshal.SizeOf(typeof(Gdi.PIXELFORMATDESCRIPTOR)), ref pixelFormatDescriptor);
			isSoftwareRenderer = (pixelFormatDescriptor.dwFlags & 0x1000) == 0 && (pixelFormatDescriptor.dwFlags & 0x40) != 0;
			if (OpenGlInfo.Version < 1.2f)
			{
				isSoftwareRenderer = true;
			}
			Settings.Validate();
			tm = new TextureManager
			{
				Settings = Settings
			};
		}

		public void DestroyContexts()
		{
			if (renderingContext != IntPtr.Zero)
			{
				Wgl.wglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
				Wgl.wglDeleteContext(renderingContext);
				renderingContext = IntPtr.Zero;
			}
			if (deviceContext != IntPtr.Zero)
			{
				if (windowHandle != IntPtr.Zero)
				{
					User.ReleaseDC(windowHandle, deviceContext);
				}
				deviceContext = IntPtr.Zero;
			}
		}

		private static void SetStyle(Control window, ControlStyles styles, bool enable)
		{
			typeof(Control).GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(window, new object[2]
			{
				styles,
				enable
			});
		}

		private void PaintFrame()
		{
			if (BeginScene(null))
			{
				OnPaint();
				EndScene();
			}
		}

		private void SetDefaultBlending()
		{
			if (CompositingMode == CompositingMode.SourceOver)
			{
				Gl.glEnable(3042);
				BlendingOperation blendingOperation = BlendingOperation;
				if (blendingOperation == BlendingOperation.Blend || blendingOperation != BlendingOperation.Multiply)
				{
					Gl.glBlendFunc(770, 771);
				}
				else
				{
					Gl.glBlendFuncSeparate(774, 0, 770, 771);
				}
			}
		}

		private static Matrix GetMatrix(bool modelView)
		{
			float[] array = new float[16];
			Gl.glGetFloatv(modelView ? 2982 : 2983, array);
			return new Matrix(array[0], array[1], array[4], array[5], array[12], array[13]);
		}

		private void window_Disposed(object sender, EventArgs e)
		{
			Dispose();
		}

		private void window_Paint(object sender, PaintEventArgs e)
		{
			PaintFrame();
		}

		protected virtual void OnPaint()
		{
			if (this.Paint != null)
			{
				this.Paint(this, EventArgs.Empty);
			}
		}

		protected virtual void OnInitOpenGl()
		{
			Gl.glDisable(2929);
			Gl.glShadeModel(7424);
		}

		public bool BeginScene(Graphics gr)
		{
			if (sceneCounter == 0)
			{
				if (deviceContext == IntPtr.Zero || renderingContext == IntPtr.Zero)
				{
					return false;
				}
				if (AutoMakeCurrent)
				{
					MakeCurrent();
				}
				if (AutoReshape)
				{
					ReshapeFunc();
				}
				Clear(Control.BackColor);
			}
			sceneCounter++;
			return true;
		}

		public void EndScene()
		{
			if (sceneCounter == 0)
			{
				return;
			}
			if (sceneCounter == 1)
			{
				if (AutoFinish)
				{
					Gl.glFinish();
				}
				if (AutoSwapBuffers)
				{
					SwapBuffers();
				}
				errorCode = Gl.glGetError();
			}
			sceneCounter--;
		}

		public void Clear(Color color)
		{
			Gl.glClearColor((float)(int)color.R / 255f, (float)(int)color.G / 255f, (float)(int)color.B / 255f, (float)(int)color.A / 255f);
			Gl.glClear(16384);
		}

		public void DrawImage(RendererImage image, RectangleF dest, RectangleF src, BitmapAdjustment ajustment, float opacity)
		{
			opacity *= Opacity;
			Gl.glPushAttrib(24576);
			SetDefaultBlending();
			if (ajustment.IsEmpty)
			{
				tm.DrawImage(image, dest, src, opacity);
			}
			else
			{
				using (Bitmap image2 = image.Bitmap.CreateAdjustedBitmap(ajustment, PixelFormat.Format32bppArgb, alwaysClone: true))
				{
					tm.DrawImage(image2, dest, src, opacity);
				}
			}
			Gl.glPopAttrib();
		}

		public void FillRectangle(RectangleF bounds, Color color)
		{
			color = Color.FromArgb((int)(255f * opacity), color);
			if (color.A >= 5)
			{
				Gl.glPushAttrib(24577);
				SetDefaultBlending();
				Gl.glColor4ub(color.R, color.G, color.B, color.A);
				Gl.glRectf(bounds.X, bounds.Y, bounds.Right, bounds.Bottom);
				Gl.glPopAttrib();
			}
		}

		public void DrawLine(IEnumerable<PointF> points, Color color, float width)
		{
			color = Color.FromArgb((int)(255f * opacity), color);
			if (color.A < 5)
			{
				return;
			}
			Gl.glPushAttrib(27425);
			SetDefaultBlending();
			Gl.glLineWidth(width);
			Gl.glColor4ub(color.R, color.G, color.B, color.A);
			Gl.glBegin(3);
			foreach (PointF point in points)
			{
				Gl.glVertex2f(point.X, point.Y);
			}
			Gl.glEnd();
			Gl.glPopAttrib();
		}

		public bool IsVisible(RectangleF bounds)
		{
			return true;
		}

		public IDisposable SaveState()
		{
			Gl.glPushMatrix();
			return new Disposer(Gl.glPopMatrix);
		}

		public void TranslateTransform(float dx, float dy)
		{
			Gl.glTranslatef(dx, dy, 0f);
		}

		public void ScaleTransform(float dx, float dy)
		{
			Gl.glScalef(dx, dy, 0f);
		}

		public void RotateTransform(float angel)
		{
			Gl.glRotatef(angel, 0f, 0f, 1f);
		}

		public void DrawBlurredImage(RendererImage image, RectangleF dest, RectangleF src, float blur)
		{
			DrawImage(image, dest, src, BitmapAdjustment.Empty, opacity);
		}

		public unsafe Bitmap GetFramebuffer(Rectangle rc, bool flip)
		{
			Bitmap bitmap = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			try
			{
				Gl.glReadPixels(rc.X, rc.Y, rc.Right, rc.Bottom, 32993, 5121, bitmapData.Scan0);
				if (flip)
				{
					IntPtr scan = bitmapData.Scan0;
					int num = bitmapData.Stride / 4;
					int num2 = bitmap.Height / 2;
					uint* ptr = (uint*)(void*)scan;
					uint* ptr2 = ptr + num * (rc.Height - 1);
					for (int i = 0; i < num2; i++)
					{
						for (int j = 0; j < num; j++)
						{
							uint num3 = ptr[j];
							ptr[j] = ptr2[j];
							ptr2[j] = num3;
						}
						ptr += num;
						ptr2 -= num;
					}
				}
			}
			finally
			{
				bitmap.UnlockBits(bitmapData);
			}
			return bitmap;
		}

		public void ClearStencil()
		{
			Gl.glClearStencil(0);
			Gl.glClear(1024);
		}

		public static void SetProcessMemorySize(int size)
		{
			Kernel.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, 0, size);
		}
	}
}
