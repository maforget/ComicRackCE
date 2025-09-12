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
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using OpenTK.Platform.Windows;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace cYo.Common.Presentation.Tao
{
    public class ControlOpenGlRenderer : DisposableObject, IControlRenderer, IBitmapRenderer, IDisposable, IHardwareRenderer
    {
        private TextureManager tm;

        private IWindowInfo windowInfo;
        private GraphicsContext graphicsContext;

        private bool isSoftwareRenderer;

        private ErrorCode errorCode;

        private int sceneCounter;

        private float opacity = 1f;

        private CompositingMode compositingMode;

        private RectangleF clip = Rectangle.Empty;

        private StencilMode stencilMode;

        public int ColorBits { get; set; }
        public int AccumBits { get; set; }
        public int DepthBits { get; set; }
        public int StencilBits { get; set; }
        public bool AutoMakeCurrent { get; set; }
        public bool AutoSwapBuffers { get; set; }
        public bool AutoFinish { get; set; }
        public bool AutoReshape { get; set; }
        public ErrorCode ErrorCode => errorCode;
        public Size Size => Control.ClientRectangle.Size;
        public Control Control { get; private set; }
        public TextureManagerSettings Settings { get; private set; }
        public bool IsLocked => sceneCounter > 0;
        public bool HighQuality { get; set; }
        public Matrix Transform
        {
            get { return GetMatrix(modelView: true); }
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
                GL.LoadMatrix(array);
            }
        }
        public bool IsHardware => true;
        public float Opacity
        {
            get { return opacity; }
            set { opacity = value; }
        }
        public CompositingMode CompositingMode
        {
            get { return compositingMode; }
            set { compositingMode = value; }
        }
        public RectangleF Clip
        {
            get { return clip; }
            set
            {
                clip = value;
                if (clip.IsEmpty)
                {
                    GL.Disable(EnableCap.ScissorTest);
                    return;
                }
                int[] array = new int[4];
                GL.Enable(EnableCap.ScissorTest);
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
                        GL.GetInteger(GetPName.Viewport, array);
                        Point point = new Point((int)((float)array[0] + (1f + array2[3].X) * (float)array[2] / 2f), (int)((float)array[1] + (1f + array2[3].Y) * (float)array[3] / 2f));
                        Point point2 = new Point((int)((float)array[0] + (1f + array2[1].X) * (float)array[2] / 2f), (int)((float)array[1] + (1f + array2[1].Y) * (float)array[3] / 2f));
                        GL.Scissor(point.X, point.Y, point2.X - point.X, point2.Y - point.Y);
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
                        GL.Enable(EnableCap.StencilTest);
                        GL.StencilFunc(StencilFunction.Always, 1, 1);
                        GL.StencilOp(StencilOp.Replace, StencilOp.Replace, StencilOp.Replace);
                        break;
                    case StencilMode.WriteNull:
                        GL.Enable(EnableCap.StencilTest);
                        GL.StencilFunc(StencilFunction.Always, 0, 0);
                        GL.StencilOp(StencilOp.Replace, StencilOp.Replace, StencilOp.Replace);
                        break;
                    case StencilMode.TestNull:
                        GL.Enable(EnableCap.StencilTest);
                        GL.StencilFunc(StencilFunction.Equal, 0, 1);
                        GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
                        break;
                    case StencilMode.TestOne:
                        GL.Enable(EnableCap.StencilTest);
                        GL.StencilFunc(StencilFunction.Equal, 1, 1);
                        GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
                        break;
                    default:
                        GL.Disable(EnableCap.StencilTest);
                        break;
                }
                stencilMode = value;
            }
        }
        public bool OptimizedTextures
        {
            get { return tm.IsOptimizedTexture; }
            set { tm.IsOptimizedTexture = value; }
        }
        public bool EnableFilter
        {
            get { return tm.EnableFilter; }
            set { tm.EnableFilter = value; }
        }
        public BlendingOperation BlendingOperation { get; set; }
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
            if (graphicsContext == null)
                throw new InvalidOperationException("GraphicsContext is not initialized.");
            graphicsContext.MakeCurrent(windowInfo);
        }

        public void SwapBuffers()
        {
            if (graphicsContext == null)
                throw new InvalidOperationException("GraphicsContext is not initialized.");
            graphicsContext.SwapBuffers();
        }

        public void Draw()
        {
            Control.Invalidate();
        }

        public void ReshapeFunc()
        {
            GL.Viewport(0, 0, Size.Width, Size.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0, Size.Width, Size.Height, 0.0, 0.0, 1.0);
            GL.MatrixMode(MatrixMode.Modelview0Ext);
            GL.LoadIdentity();
        }

        private void InitializeContexts(Control window)
        {
            if (window == null)
                throw new ArgumentNullException("window", "No valid window handle");
            Control = window;
            if (Control.Handle == IntPtr.Zero)
                throw new InvalidOperationException("Window not created");

            windowInfo = Utilities.CreateWindowsWindowInfo(Control.Handle);

            GraphicsMode mode = new GraphicsMode(
                ColorBits,
                DepthBits,
                StencilBits,
				GraphicsMode.Default.Samples, // samples
				AccumBits,
                GraphicsMode.Default.Buffers, // buffers
                false // stereo
            );

            graphicsContext = new GraphicsContext(mode, windowInfo, 3, 0, GraphicsContextFlags.Default);
            graphicsContext.MakeCurrent(windowInfo);
            graphicsContext.LoadAll();

            // Check renderer type
            string renderer = GL.GetString(StringName.Renderer);
            isSoftwareRenderer = renderer != null && renderer.IndexOf("software", StringComparison.InvariantCultureIgnoreCase) >= 0;
			if (OpenGlInfo.Version < 1.2f)
				isSoftwareRenderer = true;

			Settings.Validate();
            tm = new TextureManager
            {
                Settings = Settings
            };
        }

        public void DestroyContexts()
        {
            if (graphicsContext != null)
            {
                graphicsContext.Dispose();
                graphicsContext = null;
            }
            windowInfo = null;
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
                GL.Enable(EnableCap.Blend);
                BlendingOperation blendingOperation = BlendingOperation;
                if (blendingOperation == BlendingOperation.Blend || blendingOperation != BlendingOperation.Multiply)
                {
                    GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                }
                else
                {
                    GL.BlendFuncSeparate(BlendingFactorSrc.DstColor, BlendingFactorDest.Zero, BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                }
            }
        }

        private static Matrix GetMatrix(bool modelView)
        {
            float[] array = new float[16];
            GL.GetFloat(modelView ? GetPName.ModelviewMatrix : GetPName.ProjectionMatrix, array);
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
            GL.Disable(EnableCap.DepthTest);
            GL.ShadeModel(ShadingModel.Flat);
        }

        public bool BeginScene(Graphics gr)
        {
            if (sceneCounter == 0)
            {
                if (graphicsContext == null)
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
                    GL.Finish();
                }
                if (AutoSwapBuffers)
                {
                    SwapBuffers();
                }
                errorCode = GL.GetError();
            }
            sceneCounter--;
        }

        public void Clear(Color color)
        {
            GL.ClearColor((float)(int)color.R / 255f, (float)(int)color.G / 255f, (float)(int)color.B / 255f, (float)(int)color.A / 255f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public void DrawImage(RendererImage image, RectangleF dest, RectangleF src, BitmapAdjustment ajustment, float opacity)
        {
            opacity *= Opacity;
            GL.PushAttrib(AttribMask.EnableBit | AttribMask.ColorBufferBit);
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
            GL.PopAttrib();
        }

        public void FillRectangle(RectangleF bounds, Color color)
        {
            color = Color.FromArgb((int)(255f * opacity), color);
            if (color.A >= 5)
            {
                GL.PushAttrib(AttribMask.CurrentBit | AttribMask.EnableBit | AttribMask.ColorBufferBit);
                SetDefaultBlending();
                GL.Color4(color.R, color.G, color.B, color.A);
                GL.Rect(bounds.X, bounds.Y, bounds.Right, bounds.Bottom);
                GL.PopAttrib();
            }
        }

        public void DrawLine(IEnumerable<PointF> points, Color color, float width)
        {
            color = Color.FromArgb((int)(255f * opacity), color);
            if (color.A < 5)
            {
                return;
            }
            GL.PushAttrib(AttribMask.CurrentBit | AttribMask.PixelModeBit | AttribMask.DepthBufferBit | AttribMask.AccumBufferBit | AttribMask.ViewportBit | AttribMask.EnableBit | AttribMask.ColorBufferBit);
            SetDefaultBlending();
            GL.LineWidth(width);
            GL.Color4(color.R, color.G, color.B, color.A);
            GL.Begin(PrimitiveType.LineStrip);
            foreach (PointF point in points)
            {
                GL.Vertex2(point.X, point.Y);
            }
            GL.End();
            GL.PopAttrib();
        }

        public bool IsVisible(RectangleF bounds)
        {
            return true;
        }

        public IDisposable SaveState()
        {
            GL.PushMatrix();
            return new Disposer(GL.PopMatrix);
        }

        public void TranslateTransform(float dx, float dy)
        {
            GL.Translate(dx, dy, 0f);
        }

        public void ScaleTransform(float dx, float dy)
        {
            GL.Scale(dx, dy, 0f);
        }

        public void RotateTransform(float angel)
        {
            GL.Rotate(angel, 0f, 0f, 1f);
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
                GL.ReadPixels(rc.X, rc.Y, rc.Right, rc.Bottom, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
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
            GL.ClearStencil(0);
            GL.Clear(ClearBufferMask.StencilBufferBit);
        }

        //public static void SetProcessMemorySize(int size)
        //{
        //    Kernel.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, 0, size);
        //}
    }
}
