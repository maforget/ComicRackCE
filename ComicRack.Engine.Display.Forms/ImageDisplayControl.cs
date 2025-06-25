using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Mathematics;
using cYo.Common.Presentation;
using cYo.Common.Presentation.Tao;
using cYo.Common.Runtime;
using cYo.Common.Threading;
using cYo.Common.Windows.Forms;
using Windows7.Multitouch;
using Windows7.Multitouch.WinForms;

namespace cYo.Projects.ComicRack.Engine.Display.Forms
{
	public class ImageDisplayControl : ContainerControl, IMouseHWheel, IPanableControl
	{
		[Flags]
		public enum RenderType
		{
			None = 0x0,
			Background = 0x1,
			Image = 0x2,
			Effect = 0x4,
			Default = 0x7,
			WithoutBackground = 0x6,
			WithoutEffect = 0x3
		}

		public struct DisplayOutputConfig
		{
			private readonly Size viewSize;

			private readonly Size imageSize;

			private ImageRotation rotation;

			private readonly ImageFitMode imageDisplayMode;

			private readonly bool fitOnlyIfOversized;

			private readonly float imageZoom;

			private readonly float zoom;

			private readonly ImagePartInfo part;

			private readonly bool rightToLeftReading;

			private readonly RightToLeftReadingMode rightToLeftReadingMode;

			private readonly bool twoPageAutoScroll;

			public static readonly DisplayOutputConfig Empty = new DisplayOutputConfig(Size.Empty, Size.Empty, ImageFitMode.Original, fitOnlyIfOversized: true, RightToLeftReadingMode.FlipPages, rightToLeftReading: false, ImagePartInfo.Empty, 1f, 1f, ImageRotation.None, twoPageAutoScroll: true);

			public Size ViewSize => viewSize;

			public Size ImageSize => imageSize;

			public ImageRotation Rotation
			{
				get
				{
					return rotation;
				}
				set
				{
					rotation = value;
				}
			}

			public ImageFitMode ImageDisplayMode => imageDisplayMode;

			public bool FitOnlyIfOversized => fitOnlyIfOversized;

			public float ImageZoom => imageZoom;

			public float Zoom => zoom;

			public ImagePartInfo Part => part;

			public bool RightToLeftReading => rightToLeftReading;

			public RightToLeftReadingMode RightToLeftReadingMode => rightToLeftReadingMode;

			public bool TwoPageAutoScroll => twoPageAutoScroll;

			public bool IsEmpty
			{
				get
				{
					if (!viewSize.IsEmpty)
					{
						return imageSize.IsEmpty;
					}
					return true;
				}
			}

			public DisplayOutputConfig(Size viewSize, Size imageSize, ImageFitMode imageDisplayMode, bool fitOnlyIfOversized, RightToLeftReadingMode rightToLeftReadingMode, bool rightToLeftReading, ImagePartInfo part, float imageZoom, float zoom, ImageRotation rotation, bool twoPageAutoScroll)
			{
				this.viewSize = viewSize;
				this.imageSize = imageSize;
				this.rotation = rotation;
				this.imageZoom = imageZoom;
				this.zoom = zoom;
				this.imageDisplayMode = imageDisplayMode;
				this.fitOnlyIfOversized = fitOnlyIfOversized;
				this.part = part;
				this.rightToLeftReadingMode = rightToLeftReadingMode;
				this.rightToLeftReading = rightToLeftReading;
				this.twoPageAutoScroll = twoPageAutoScroll;
			}
		}

		public class DisplayOutput : DisposableObject, ICloneable
		{
			private DisplayOutputConfig config;

			private System.Drawing.Drawing2D.Matrix transform;

			private SizeF scale = CreateScale(1f);

			private Rectangle partBounds;

			private int part;

			private int partCount;

			private Rectangle[] parts = new Rectangle[0];

			public DisplayOutputConfig Config => config;

			public System.Drawing.Drawing2D.Matrix Transform => transform;

			public SizeF Scale => scale;

			public Rectangle PartBounds => partBounds;

			public Rectangle OutputBounds => PartBounds.Size.ToRectangle();

			public Rectangle OutputBoundsScreen
			{
				get
				{
					try
					{
						Point[] array = OutputBounds.ToPoints();
						Transform.TransformPoints(array);
						return array.ToRectangle();
					}
					catch (Exception)
					{
						return Rectangle.Empty;
					}
				}
			}

			public int Part => part;

			public int PartCount => partCount;

			public float ImageZoom
			{
				get;
				private set;
			}

			public bool PartIsStart => IsStartPart(partBounds);

			public bool PartIsEnd => IsEndPart(partBounds);

			public bool IsEmpty
			{
				get
				{
					if (!OutputBounds.IsEmpty)
					{
						return Transform == null;
					}
					return true;
				}
			}

			public bool IsAllVisible => parts.Length < 2;

			private DisplayOutput()
			{
			}

			private DisplayOutput(DisplayOutput copy)
				: this()
			{
				config = copy.config;
				transform = SafeMatrixCopy(copy.transform);
				scale = copy.scale;
				partBounds = copy.partBounds;
				part = copy.part;
				parts = copy.parts;
				partCount = copy.partCount;
				ImageZoom = copy.ImageZoom;
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing && transform != null)
				{
					transform.Dispose();
				}
				base.Dispose(disposing);
			}

			public Rectangle GetPart(int index)
			{
				if (parts.Length == 0)
				{
					return Rectangle.Empty;
				}
				return parts[index.Clamp(0, parts.Length - 1)];
			}

			public Point GetPartOffset(int partIndex, Point offset)
			{
				return GetClampedPartOffset(offset, config.ImageSize, GetPart(partIndex));
			}

			public Rectangle GetPart(int partIndex, Point offset)
			{
				Rectangle result = GetPart(partIndex);
				result.Offset(GetPartOffset(partIndex, offset));
				return result;
			}

			public Rectangle GetPart(ImagePartInfo ipi)
			{
				return GetPart(ipi.Part, ipi.Offset);
			}

			public bool IsStartPart(Rectangle part)
			{
				return RectangleEquals(part, GetPart(0));
			}

			public bool IsEndPart(Rectangle part)
			{
				return RectangleEquals(part, GetPart(PartCount));
			}

			public bool IsEndPart(ImagePartInfo ipi)
			{
				return IsEndPart(GetPart(ipi));
			}

			public bool IsStartPart(ImagePartInfo ipi)
			{
				return IsStartPart(GetPart(ipi));
			}

			public Rectangle[] GetParts(int startIndex, int endIndex)
			{
				if (startIndex >= endIndex)
				{
					return new Rectangle[0];
				}
				startIndex = Math.Max(startIndex, 0);
				endIndex = Math.Min(endIndex, parts.Length);
				Rectangle[] array = new Rectangle[endIndex - startIndex];
				for (int i = startIndex; i < endIndex; i++)
				{
					array[i - startIndex] = parts[i];
				}
				return array;
			}

			public Rectangle[] GetParts(int startIndex)
			{
				return GetParts(startIndex, parts.Length);
			}

			public ImagePartInfo GetBestPartFit(ImagePartInfo newPart)
			{
				int num = newPart.Part;
				Point offset = newPart.Offset;
				if (num < 0)
				{
					offset = Point.Empty;
					num = 0;
				}
				if (num >= PartCount)
				{
					offset = Point.Empty;
					num = PartCount - 1;
				}
				if (num < 0)
				{
					return newPart;
				}
				offset = GetPartOffset(num, offset);
				Rectangle test = GetPart(num);
				test.Offset(offset);
				int num2 = GetParts(0).IndexOfBestFit(test);
				if (num != num2)
				{
					Rectangle rectangle = GetPart(num);
					Rectangle rectangle2 = GetPart(num2);
					offset.Offset(rectangle.Location);
					offset.Offset(-rectangle2.X, -rectangle2.Y);
					num = num2;
				}
				return new ImagePartInfo(num, offset);
			}

			private static System.Drawing.Drawing2D.Matrix SafeMatrixCopy(System.Drawing.Drawing2D.Matrix matrix)
			{
				if (matrix == null)
				{
					return null;
				}
				try
				{
					float[] elements = matrix.Elements;
					return new System.Drawing.Drawing2D.Matrix(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5]);
				}
				catch
				{
					return null;
				}
			}

			private static bool Eps(int a, int b)
			{
				return Math.Abs(a - b) < 2;
			}

			private static bool RectangleEquals(Rectangle part, Rectangle rectangle)
			{
				if (Eps(part.X, rectangle.X) && Eps(part.Y, rectangle.Y) && Eps(part.Right, rectangle.Right))
				{
					return Eps(part.Bottom, rectangle.Bottom);
				}
				return false;
			}

			public static DisplayOutput Create(DisplayOutputConfig dp, float anamorphicTolerance)
			{
				DisplayOutput displayOutput = new DisplayOutput();
				int pageRotation = dp.Rotation.ToDegrees();
				displayOutput.config = dp;
				displayOutput.ImageZoom = dp.ImageZoom;
				if (dp.ImageSize.Width != 0 && dp.ImageSize.Height != 0)
				{
					try
					{
						Size size;
						using (System.Drawing.Drawing2D.Matrix rotationMatrix = MatrixUtility.GetRotationMatrix(dp.ViewSize, pageRotation))
						{
							size = new Rectangle(Point.Empty, dp.ViewSize).Rotate(rotationMatrix).Size;
						}
						SizeF realZoom = GetScale(size, dp.ImageSize, dp.ImageDisplayMode, dp.FitOnlyIfOversized, dp.Zoom, anamorphicTolerance);
						Size partGridSize = GetPartGridSize(size, dp.ImageSize, realZoom, !dp.TwoPageAutoScroll);
						int num = partGridSize.Width * partGridSize.Height;
						int num2 = dp.Part.Part.Clamp(0, num - 1);
						Rectangle[] array = new Rectangle[num];
						for (int i = 0; i < num; i++)
						{
							array[i] = GetPartRectangle(size, dp.ImageSize, realZoom, i, partGridSize, !dp.TwoPageAutoScroll, dp.RightToLeftReadingMode, dp.RightToLeftReading);
						}
						Rectangle partRectangle = array[num2];
						partRectangle.Offset(GetClampedPartOffset(dp.Part.Offset, dp.ImageSize, partRectangle));
						System.Drawing.Drawing2D.Matrix rotationMatrix2 = MatrixUtility.GetRotationMatrix(partRectangle.Size, pageRotation);
						Rectangle rectangle = new Rectangle(0, 0, partRectangle.Width, partRectangle.Height).Rotate(rotationMatrix2);
						rotationMatrix2.Translate(-rectangle.X, -rectangle.Y, MatrixOrder.Append);
						rotationMatrix2.Scale(realZoom.Width, realZoom.Height, MatrixOrder.Append);
						rotationMatrix2.Translate(((float)dp.ViewSize.Width - (float)rectangle.Width * realZoom.Width) / 2f, ((float)dp.ViewSize.Height - (float)rectangle.Height * realZoom.Height) / 2f, MatrixOrder.Append);
						displayOutput.transform = rotationMatrix2;
						displayOutput.scale = realZoom;
						displayOutput.partBounds = partRectangle;
						displayOutput.parts = array;
						displayOutput.part = num2;
						displayOutput.partCount = array.Length;
						return displayOutput;
					}
					catch
					{
						return displayOutput;
					}
				}
				return displayOutput;
			}

			public static DisplayOutput Interpolate(DisplayOutput a, DisplayOutput b, float p)
			{
				try
				{
					DisplayOutput displayOutput = new DisplayOutput
					{
						part = b.part,
						parts = b.parts,
						config = b.config,
						scale = a.scale + new SizeF((b.scale.Width - a.scale.Width) * p, (b.scale.Height - a.scale.Height) * p),
						ImageZoom = a.ImageZoom + (b.ImageZoom - a.ImageZoom) * p
					};
					float num = p * (b.ImageZoom / displayOutput.ImageZoom);
					float num2 = (float)(b.partBounds.X - a.PartBounds.X) * num;
					float num3 = (float)(b.partBounds.Y - a.PartBounds.Y) * num;
					float num4 = (float)(b.partBounds.Width - a.PartBounds.Width) * num;
					float num5 = (float)(b.partBounds.Height - a.PartBounds.Height) * num;
					displayOutput.partBounds.X = (int)((float)a.partBounds.X + num2);
					displayOutput.partBounds.Y = (int)((float)a.partBounds.Y + num3);
					displayOutput.partBounds.Width = (int)((float)a.partBounds.Width + num4);
					displayOutput.partBounds.Height = (int)((float)a.partBounds.Height + num5);
					float[] elements = a.transform.Elements;
					float[] elements2 = b.transform.Elements;
					float[] array = new float[elements.Length];
					for (int i = 0; i < elements.Length; i++)
					{
						array[i] = elements[i] + (elements2[i] - elements[i]) * p;
					}
					displayOutput.transform = new System.Drawing.Drawing2D.Matrix(array[0], array[1], array[2], array[3], array[4], array[5]);
					return displayOutput;
				}
				catch (Exception)
				{
					return b;
				}
			}

			private static SizeF CreateScale(float scale)
			{
				return new SizeF(scale, scale);
			}

			private static SizeF GetScale(Size clientSize, Size bitmapSize, ImageFitMode pageDisplayMode, bool onlyFitOversized, float zoom, float anamorphicTolerance)
			{
				bool flag = bitmapSize.Width > bitmapSize.Height;
				float num = bitmapSize.Width;
				float num2 = bitmapSize.Height;
				float num3 = (float)clientSize.Width / num * zoom;
				float num4 = (float)clientSize.Height / num2 * zoom;
				SizeF result = new SizeF(zoom, zoom);
				switch (pageDisplayMode)
				{
				default:
					return result;
				case ImageFitMode.FitWidth:
					if (onlyFitOversized && clientSize.Width > bitmapSize.Width)
					{
						return result;
					}
					if (num4.CompareTo(num3, num3 * anamorphicTolerance))
					{
						return new SizeF(num3, num4);
					}
					return CreateScale(num3);
				case ImageFitMode.FitWidthAdaptive:
					if (onlyFitOversized && clientSize.Width > bitmapSize.Width)
					{
						return result;
					}
					if (flag)
					{
						num3 *= 2f;
					}
					if (num4.CompareTo(num3, num3 * anamorphicTolerance))
					{
						return new SizeF(num3, num4);
					}
					return CreateScale(num3);
				case ImageFitMode.FitHeight:
					if (onlyFitOversized && clientSize.Height > bitmapSize.Height)
					{
						return result;
					}
					if (num3.CompareTo(num4, num4 * anamorphicTolerance))
					{
						return new SizeF(num3, num4);
					}
					return CreateScale(num4);
				case ImageFitMode.Fit:
				{
					if (onlyFitOversized && clientSize.Height > bitmapSize.Height && clientSize.Width > bitmapSize.Width)
					{
						return result;
					}
					float num5 = Math.Min(num3, num4);
					if (!num4.CompareTo(num5, num5 * anamorphicTolerance))
					{
						num4 = num5;
					}
					if (!num3.CompareTo(num5, num5 * anamorphicTolerance))
					{
						num3 = num5;
					}
					return new SizeF(num3, num4);
				}
				case ImageFitMode.BestFit:
				{
					if (onlyFitOversized && (clientSize.Height > bitmapSize.Height || clientSize.Width > bitmapSize.Width))
					{
						return result;
					}
					float num5 = Math.Max(num3, num4);
					if (!num4.CompareTo(num5, num5 * anamorphicTolerance))
					{
						num4 = num5;
					}
					if (!num3.CompareTo(num5, num5 * anamorphicTolerance))
					{
						num3 = num5;
					}
					return new SizeF(num3, num4);
				}
				}
			}

			private static Size GetPartGridSize(Size clientSize, Size bitmapSize, SizeF realZoom, bool doubleSpread)
			{
				int num = (int)((float)bitmapSize.Height * realZoom.Height);
				int num2 = (int)((float)bitmapSize.Width * realZoom.Width);
				if (clientSize.Width == 0 || clientSize.Height == 0)
				{
					return new Size(1, 1);
				}
				if (num <= clientSize.Height && num2 <= clientSize.Width)
				{
					return new Size(1, 1);
				}
				if (!doubleSpread || num > num2)
				{
					return new Size((num2 - 1) / clientSize.Width + 1, (num - 1) / clientSize.Height + 1);
				}
				return new Size(((num2 / 2 - 1) / clientSize.Width + 1) * 2, (num - 1) / clientSize.Height + 1);
			}

			private static Rectangle GetPartRectangle(Size clientSize, Size bitmapSize, SizeF realZoom, int part, Size partGrid, bool doubleSpread, RightToLeftReadingMode rightToLeftReadingMode, bool rightToLeftReading)
			{
				bool flag = bitmapSize.Width > bitmapSize.Height;
				int num = partGrid.Width * partGrid.Height;
				part = part.Clamp(0, num - 1);
				if (doubleSpread && flag)
				{
					Size partGridSize = GetPartGridSize(clientSize, bitmapSize, realZoom, doubleSpread: false);
					int num2 = (partGridSize.Width - 1) / 2 + 1;
					int num3 = num2 * partGridSize.Height;
					int num4 = part % num3;
					int num5 = part / num3 * (partGridSize.Width / 2);
					int num6 = partGridSize.Width * (num4 / num2) + num4 % num2 + num5;
					Rectangle partRectangle = GetPartRectangle(clientSize, bitmapSize, realZoom, num6, partGridSize, doubleSpread: false, rightToLeftReadingMode, rightToLeftReading: false);
					if (rightToLeftReading)
					{
						if (rightToLeftReadingMode == RightToLeftReadingMode.FlipParts)
						{
							partRectangle.X = bitmapSize.Width - partRectangle.Right;
							if (part < num3)
							{
								if (partRectangle.X < bitmapSize.Width / 2)
								{
									partRectangle.X = bitmapSize.Width / 2;
								}
								if (partRectangle.Right > bitmapSize.Width)
								{
									partRectangle.X -= partRectangle.Right - bitmapSize.Width;
								}
							}
							else
							{
								if (partRectangle.Right > bitmapSize.Width / 2)
								{
									partRectangle.X -= partRectangle.Right - bitmapSize.Width / 2;
								}
								if (partRectangle.X < 0)
								{
									partRectangle.X = 0;
								}
							}
						}
						else if (part < num3)
						{
							partRectangle.X = bitmapSize.Width / 2 - partRectangle.Right;
							if (partRectangle.X < 0)
							{
								partRectangle.X = 0;
							}
						}
						else
						{
							partRectangle.X -= bitmapSize.Width / 2;
							partRectangle.X = bitmapSize.Width / 2 - partRectangle.Right;
							partRectangle.X += bitmapSize.Width / 2;
							if (partRectangle.Right > bitmapSize.Width)
							{
								partRectangle.X -= partRectangle.Right - bitmapSize.Width;
							}
						}
					}
					else if (part < num3)
					{
						if (partRectangle.Right > bitmapSize.Width / 2)
						{
							partRectangle.X -= partRectangle.Right - bitmapSize.Width / 2;
						}
						if (partRectangle.X < 0)
						{
							partRectangle.X = 0;
						}
					}
					else
					{
						if (partRectangle.X < bitmapSize.Width / 2)
						{
							partRectangle.X = bitmapSize.Width / 2;
						}
						if (partRectangle.Right > bitmapSize.Width)
						{
							partRectangle.X -= partRectangle.Right - bitmapSize.Width;
						}
					}
					return partRectangle;
				}
				clientSize.Width = (int)((float)clientSize.Width / realZoom.Width);
				clientSize.Height = (int)((float)clientSize.Height / realZoom.Height);
				int num7 = bitmapSize.Height / partGrid.Height;
				int num8 = bitmapSize.Width / partGrid.Width;
				Rectangle result = Clamp(rect: new Rectangle(num8 * (part % partGrid.Width), num7 * (part / partGrid.Width), clientSize.Width, clientSize.Height), size: bitmapSize);
				if (rightToLeftReading)
				{
					result.X = bitmapSize.Width - result.Right;
				}
				return result;
			}

			private static Point GetClampedPartOffset(Point offset, Size imageSize, Rectangle partRectangle)
			{
				int num = offset.X;
				int num2 = offset.Y;
				if (num + partRectangle.Right > imageSize.Width)
				{
					num = imageSize.Width - partRectangle.Right;
				}
				if (num2 + partRectangle.Bottom > imageSize.Height)
				{
					num2 = imageSize.Height - partRectangle.Bottom;
				}
				if (num + partRectangle.Left < 0)
				{
					num = -partRectangle.Left;
				}
				if (num2 + partRectangle.Top < 0)
				{
					num2 = -partRectangle.Top;
				}
				return new Point(num, num2);
			}

			private static Rectangle Clamp(Size size, Rectangle rect)
			{
				if (rect.Right > size.Width)
				{
					rect.X = size.Width - rect.Width;
				}
				if (rect.Bottom > size.Height)
				{
					rect.Y = size.Height - rect.Height;
				}
				if (rect.Y < 0)
				{
					rect.Height += rect.Y;
					rect.Y = 0;
				}
				if (rect.X < 0)
				{
					rect.Width += rect.X;
					rect.X = 0;
				}
				return rect;
			}

			public override bool Equals(object obj)
			{
				DisplayOutput displayOutput = obj as DisplayOutput;
				if (displayOutput == null)
				{
					return false;
				}
				if (!object.Equals(Config, displayOutput.Config) || part != displayOutput.part || partBounds != displayOutput.partBounds || partCount != displayOutput.partCount || scale != displayOutput.scale)
				{
					return false;
				}
				if (transform == null && displayOutput.transform == null)
				{
					return true;
				}
				return object.Equals(transform, displayOutput.transform);
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			public object Clone()
			{
				return new DisplayOutput(this);
			}
		}

		public class GestureArea
		{
			public ContentAlignment Alignment
			{
				get;
				set;
			}

			public Rectangle Area
			{
				get;
				set;
			}

			public GestureArea(ContentAlignment alignment, Rectangle area)
			{
				Alignment = alignment;
				Area = area;
			}
		}

		public class RenderEventArgs : EventArgs
		{
			private readonly IBitmapRenderer renderer;

			private readonly DisplayOutput display;

			public IBitmapRenderer Graphics => renderer;

			public DisplayOutput Display => display;

			public RenderEventArgs(IBitmapRenderer renderer, DisplayOutput display)
			{
				this.renderer = renderer;
				this.display = display;
			}
		}

		public enum HardwareAccelerationType
		{
			Disabled,
			Enabled,
			Forced
		}

		private static class Native
		{
			public const int WM_MOUSEHWHEEL = 0x020E;
		}

		public const float MinimumZoom = 1f;

		public const float MaximumZoom = 8f;

		private volatile bool lowQualityOverride;

		protected IBitmapRenderer renderer;

		private readonly BackgroundRunner flowRunner = new BackgroundRunner();

		private readonly BackgroundRunner partScrollRunner = new BackgroundRunner();

		private int pageScrollingTime = 1000;

		private bool smoothScrolling = true;

		private ContentAlignment textAlignment = ContentAlignment.MiddleCenter;

		private ImageFitMode imageFitMode = ImageFitMode.Fit;

		private bool imageFitOnlyIfOversized;

		private bool rightToLeftReading;

		private RightToLeftReadingMode rightToLeftReadingMode;

		private bool twoPageNavigation = true;

		private ImagePartInfo imageVisiblePart = ImagePartInfo.Empty;

		private float imageZoom = 1f;

		private float pageMarginPercent = 0.05f;

		private bool pageMargin;

		private volatile ImageDisplayOptions imageDisplayOptions;

		private volatile ImageBackgroundMode imageBackgroundMode = ImageBackgroundMode.Color;

		private string backgroundTexture;

		private ImageRotation imageRotation;

		private bool imageAutoRotate;

		private int autoHideCursorDelay = 5000;

		private bool cursorAutoHide;

		private float anamorphicTolerance = 0.25f;

		private bool clipToDestination;

		private DisplayOutput display;

		private DisplayOutput lastRenderedDisplay;

		private bool hardwareFiltering;

		private PointF scrollStartOffs;

		private PointF scrollEndOffs;

		private PointF scrollDelta;

		private long scrollLastTime;

		private ImagePartInfo scrollPartEnd;

		private bool cursorVisible = true;

		private int autoHideCounter;

		private MouseButtons lastMouseButton;

		private MouseButtons pendingClick;

		private bool inPaint;

		private bool blockPaint;

		private bool mouseInView;

		private Point clickPoint;

		private ImagePartInfo orgPart;

		private float orgZoom;

		private Point flowLastPoint;

		private long flowLastPointTime;

		private long flowLastTime;

		private PointF flowMouseDelta;

		private PointF flowMinDelta;

		private GestureHandler gestureHandler;

		private ImageRotation gestureRotation;

		private double gestureRotationStart;

		private float gestureZoomStart;

		private bool zoomStart;

		private float zoomOffset;

		private Point panStart;

		private Point panLocation;

		private ImagePartInfo panPart;

		private Windows7.Multitouch.GestureEventArgs ignoreEvent;

		private static HardwareAccelerationType hardwareAcceleration = HardwareAccelerationType.Enabled;

		private static readonly TextureManagerSettings hardwareSettings = new TextureManagerSettings();

		private IContainer components;

		private Timer autoHideCursorTimer;

		private Timer mouseClickTimer;

		public bool IsHardwareRenderer
		{
			get
			{
				if (renderer != null)
				{
					return renderer.IsHardware;
				}
				return false;
			}
		}

		public virtual bool IsValid => true;

		[Category("Behavior")]
		[Description("Time full page scrolling needs in ms")]
		[DefaultValue(1000)]
		public virtual int PageScrollingTime
		{
			get
			{
				return pageScrollingTime;
			}
			set
			{
				pageScrollingTime = value;
			}
		}

		[Category("Behavior")]
		[Description("Scroll parts")]
		[DefaultValue(true)]
		public bool SmoothScrolling
		{
			get
			{
				return smoothScrolling;
			}
			set
			{
				smoothScrolling = value;
			}
		}

		[Category("Behavior")]
		[Description("Text alignment")]
		[DefaultValue(ContentAlignment.MiddleCenter)]
		public ContentAlignment TextAlignment
		{
			get
			{
				return textAlignment;
			}
			set
			{
				if (textAlignment != value)
				{
					textAlignment = value;
					Invalidate();
				}
			}
		}

		[Category("Behavior")]
		[Description("Page Display Mode")]
		[DefaultValue(ImageFitMode.Fit)]
		public ImageFitMode ImageFitMode
		{
			get
			{
				return imageFitMode;
			}
			set
			{
				if (imageFitMode != value)
				{
					imageFitMode = value;
					OnPageDisplayModeChanged();
					ImageVisiblePart = ImagePartInfo.Empty;
					Invalidate();
				}
			}
		}

		[Category("Behavior")]
		[Description("Fit only if oversized")]
		[DefaultValue(false)]
		public bool ImageFitOnlyIfOversized
		{
			get
			{
				return imageFitOnlyIfOversized;
			}
			set
			{
				if (imageFitOnlyIfOversized != value)
				{
					imageFitOnlyIfOversized = value;
					OnPageDisplayModeChanged();
					ImageVisiblePart = ImagePartInfo.Empty;
					Invalidate();
				}
			}
		}

		[DefaultValue(false)]
		public bool RightToLeftReading
		{
			get
			{
				return rightToLeftReading;
			}
			set
			{
				if (rightToLeftReading != value)
				{
					rightToLeftReading = value;
					OnReadingModeChanged();
					Invalidate();
				}
			}
		}

		[DefaultValue(RightToLeftReadingMode.FlipParts)]
		public RightToLeftReadingMode RightToLeftReadingMode
		{
			get
			{
				return rightToLeftReadingMode;
			}
			set
			{
				if (rightToLeftReadingMode != value)
				{
					rightToLeftReadingMode = value;
					OnReadingModeChanged();
					Invalidate();
				}
			}
		}

		[DefaultValue(true)]
		public bool TwoPageNavigation
		{
			get
			{
				return twoPageNavigation;
			}
			set
			{
				if (twoPageNavigation != value)
				{
					twoPageNavigation = value;
					Invalidate();
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ImagePartInfo ImageVisiblePart
		{
			get
			{
				using (ItemMonitor.Lock(this))
				{
					return imageVisiblePart;
				}
			}
			set
			{
				StopPartScrolling();
				flowRunner.Enabled = false;
				SetVisiblePart(value);
			}
		}

		[Description("Zoom percentage")]
		[DefaultValue(1f)]
		public float ImageZoom
		{
			get
			{
				return imageZoom;
			}
			set
			{
				DoZoom(Display.PartBounds.GetCenter(), value);
			}
		}

		[DefaultValue(0.05f)]
		public float PageMarginPercentWidth
		{
			get
			{
				return pageMarginPercent;
			}
			set
			{
				if (pageMarginPercent != value)
				{
					pageMarginPercent = value;
					Invalidate();
				}
			}
		}

		[DefaultValue(false)]
		public bool PageMargin
		{
			get
			{
				return pageMargin;
			}
			set
			{
				if (pageMargin != value)
				{
					pageMargin = value;
					Invalidate();
				}
			}
		}

		[Browsable(false)]
		public virtual int ImagePartCount => Display.PartCount;

		[Category("Appearance")]
		[Description("Options for the display")]
		[DefaultValue(ImageDisplayOptions.None)]
		public ImageDisplayOptions ImageDisplayOptions
		{
			get
			{
				return imageDisplayOptions;
			}
			set
			{
				if (imageDisplayOptions != value)
				{
					imageDisplayOptions = value;
					OnImageDisplayOptionsChanged();
					Invalidate();
				}
			}
		}

		[Category("Appearance")]
		[Description("Options for the background display")]
		[DefaultValue(ImageBackgroundMode.Color)]
		public ImageBackgroundMode ImageBackgroundMode
		{
			get
			{
				return imageBackgroundMode;
			}
			set
			{
				if (imageBackgroundMode != value)
				{
					imageBackgroundMode = value;
					OnImageDisplayOptionsChanged();
					Invalidate();
				}
			}
		}

		[Category("Appearance")]
		[Description("background texture file")]
		[DefaultValue(null)]
		public string BackgroundTexture
		{
			get
			{
				return backgroundTexture;
			}
			set
			{
				if (!(backgroundTexture == value))
				{
					backgroundTexture = value;
					Image backgroundImage = BackgroundImage;
					try
					{
						BackgroundImage = Image.FromFile(backgroundTexture);
					}
					catch (Exception)
					{
						BackgroundImage = null;
					}
					backgroundImage.SafeDispose();
					OnImageDisplayOptionsChanged();
					Invalidate();
				}
			}
		}

		[Category("Appearance")]
		[Description("Rotation of the page")]
		[DefaultValue(ImageRotation.None)]
		public ImageRotation ImageRotation
		{
			get
			{
				return imageRotation;
			}
			set
			{
				if (imageRotation != value)
				{
					imageRotation = value;
					OnPageDisplayModeChanged();
					Invalidate();
				}
			}
		}

		[Category("Appearance")]
		[Description("Auto Rotate page")]
		[DefaultValue(false)]
		public bool ImageAutoRotate
		{
			get
			{
				return imageAutoRotate;
			}
			set
			{
				if (imageAutoRotate != value)
				{
					imageAutoRotate = value;
					OnPageDisplayModeChanged();
					Invalidate();
				}
			}
		}

		[Category("Behavior")]
		[Description("Time in ms after the Cursor gets hidden in seconds")]
		[DefaultValue(5000)]
		public int AutoHideCursorDelay
		{
			get
			{
				return autoHideCursorDelay;
			}
			set
			{
				autoHideCursorDelay = value;
				UpdateCursorAutoHide();
			}
		}

		[Category("Behavior")]
		[Description("Turns Automatic Hiding of the cursor on or off")]
		[DefaultValue(false)]
		public bool AutoHideCursor
		{
			get
			{
				return cursorAutoHide;
			}
			set
			{
				cursorAutoHide = value;
				UpdateCursorAutoHide();
			}
		}

		[DefaultValue(true)]
		public bool DisplayChangeAnimation
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool FlowingMouseScrolling
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool DisableHardwareAcceleration
		{
			get;
			set;
		}

		[DefaultValue(0.25f)]
		public float AnamorphicTolerance
		{
			get
			{
				return anamorphicTolerance;
			}
			set
			{
				if (anamorphicTolerance != value)
				{
					anamorphicTolerance = value;
					Invalidate();
				}
			}
		}

		[DefaultValue(false)]
		public bool ClipToDestination
		{
			get
			{
				return clipToDestination;
			}
			set
			{
				if (clipToDestination != value)
				{
					clipToDestination = value;
					Invalidate();
				}
			}
		}

		public float CurrentAnamorphicTolerance
		{
			get
			{
				if ((ImageDisplayOptions & ImageDisplayOptions.AnamorphicScaling) == 0)
				{
					return 0f;
				}
				return AnamorphicTolerance;
			}
		}

		public ImageRotation CurrentImageRotation => LastRenderedDisplay.Config.Rotation;

		public Rectangle PagePartBounds => Display.PartBounds;

		public Size ImageSize => GetImageSize();

		public bool FullImageVisible
		{
			get
			{
				Size imageSize = ImageSize;
				Size size = PagePartBounds.Size;
				if (size.Width >= imageSize.Width - 2)
				{
					return size.Height >= imageSize.Height - 2;
				}
				return false;
			}
		}

		public bool MouseActionHappened
		{
			get;
			protected set;
		}

		protected bool DisableScrolling
		{
			get;
			set;
		}

		protected DisplayOutputConfig DisplayConfig
		{
			get
			{
				OnUpdateDisplayConfig();
				Size imageSize = GetImageSize();
				bool flag = imageSize.Width > imageSize.Height && !IsDoubleImage;
				ImageRotation rotation = ((ImageAutoRotate && imageSize.Width > imageSize.Height) ? ImageRotation.RotateLeft() : ImageRotation);
				return new DisplayOutputConfig(base.ClientRectangle.Size, imageSize, ImageFitMode, ImageFitOnlyIfOversized, (!flag) ? RightToLeftReadingMode : RightToLeftReadingMode.FlipParts, RightToLeftReading, ImageVisiblePart, ImageZoom, ImageZoom * (PageMargin ? (1f - PageMarginPercentWidth) : 1f), rotation, flag && TwoPageNavigation);
			}
		}

		protected DisplayOutput Display
		{
			get
			{
				if (display == null || !object.Equals(display.Config, DisplayConfig))
				{
					if (display != null)
					{
						display.Dispose();
					}
					display = DisplayOutput.Create(DisplayConfig, CurrentAnamorphicTolerance);
				}
				return display;
			}
		}

		protected DisplayOutput LastRenderedDisplay
		{
			get
			{
				return lastRenderedDisplay ?? Display;
			}
			set
			{
				lastRenderedDisplay.SafeDispose();
				lastRenderedDisplay = value.Clone() as DisplayOutput;
			}
		}

		public Color CurrentBackColor
		{
			get
			{
				Color color = ((ImageBackgroundMode == ImageBackgroundMode.Auto) ? GetAutoBackgroundColor() : BackColor);
				if (color == Color.Empty)
				{
					color = BackColor;
				}
				return color;
			}
		}

		protected bool IsTexturedBackground
		{
			get
			{
				if (imageBackgroundMode == ImageBackgroundMode.Texture)
				{
					return BackgroundImage is Bitmap;
				}
				return false;
			}
		}

		protected bool IsConstantBackground => imageBackgroundMode != ImageBackgroundMode.Auto;

		public bool HardwareFiltering
		{
			get
			{
				return hardwareFiltering;
			}
			set
			{
				hardwareFiltering = value;
				IHardwareRenderer hardwareRenderer = renderer as IHardwareRenderer;
				if (hardwareRenderer != null)
				{
					hardwareRenderer.EnableFilter = hardwareFiltering;
				}
			}
		}

		protected bool DisplayEventsDisabled
		{
			get;
			set;
		}

		public virtual bool IsDoubleImage => false;

		protected virtual bool MouseHandled => false;

		protected Point GestureLocation
		{
			get;
			private set;
		}

		public static HardwareAccelerationType HardwareAcceleration
		{
			get
			{
				return hardwareAcceleration;
			}
			set
			{
				hardwareAcceleration = value;
			}
		}

		public static TextureManagerSettings HardwareSettings => hardwareSettings;

		public Point PanLocation => panLocation;

		public event EventHandler PageDisplayModeChanged;

		public event EventHandler VisiblePartChanged;

		public event MouseEventHandler MouseHWheel;

		public event EventHandler UpdateDisplayConfig;

		public event EventHandler<RenderEventArgs> RendeImageOverlay;

		public event EventHandler<GestureEventArgs> PreviewGesture;

		public event EventHandler<GestureEventArgs> Gesture;

		public event EventHandler PanStart;

		public event EventHandler PanEnd;

		public event EventHandler Pan;

		public ImageDisplayControl()
		{
			FlowingMouseScrolling = true;
			DisplayChangeAnimation = true;
			InitializeComponent();
			components.Add(flowRunner);
			flowRunner.Synchronize = this;
			flowRunner.Interval = 25;
			flowRunner.Tick += FlowTimerTick;
			components.Add(flowRunner);
			partScrollRunner.Synchronize = this;
			partScrollRunner.Interval = 25;
			partScrollRunner.Tick += PartScrollTimerTick;
			SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
			SetStyle(ControlStyles.UserPaint, value: true);
			SetStyle(ControlStyles.ResizeRedraw, value: true);
			SetStyle(ControlStyles.Selectable, value: true);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (display != null)
				{
					display.Dispose();
				}
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		protected virtual void RenderImage(IBitmapRenderer renderer, DisplayOutput display, RenderType renderType = RenderType.Default)
		{
			if ((renderType & RenderType.Background) != 0)
			{
				RenderImageBackground(renderer, display);
			}
			using (renderer.SaveState())
			{
				try
				{
					renderer.HighQuality = HasDisplayOption(ImageDisplayOptions.HighQuality) && !lowQualityOverride;
					if (display.IsEmpty)
					{
						return;
					}
					System.Drawing.Drawing2D.Matrix transform = renderer.Transform;
					transform.Multiply(display.Transform);
					renderer.Transform = transform;
					if (renderer.IsVisible(display.OutputBounds))
					{
						if ((renderType & RenderType.Image) != 0)
						{
							DrawImage(renderer, display.OutputBounds, display.PartBounds);
						}
						if ((renderType & RenderType.Effect) != 0)
						{
							RenderImageEffect(renderer, display);
						}
					}
				}
				catch (Exception)
				{
					throw;
				}
			}
		}

		protected virtual void RenderImageBackground(IBitmapRenderer bitmapRenderer, DisplayOutput output)
		{
			if (!IsTexturedBackground)
			{
				bitmapRenderer.FillRectangle(base.ClientRectangle, CurrentBackColor);
				return;
			}
			Bitmap bitmap = (Bitmap)BackgroundImage;
			using (bitmapRenderer.SaveState())
			{
				float num = output?.ImageZoom ?? ImageZoom;
				bitmapRenderer.TranslateTransform((float)base.ClientRectangle.Width / 2f, (float)base.ClientRectangle.Height / 2f);
				bitmapRenderer.ScaleTransform(num, num);
				bitmapRenderer.TranslateTransform((float)(-base.ClientRectangle.Width) / 2f, (float)(-base.ClientRectangle.Height) / 2f);
				bitmapRenderer.FillRectangle(bitmap, BackgroundImageLayout, base.ClientRectangle, bitmap.Size.ToRectangle(), BitmapAdjustment.Empty, 1f);
			}
		}

		protected virtual void RenderImageEffect(IBitmapRenderer renderer, DisplayOutput display)
		{
		}

		protected virtual void RenderImageOverlay(IBitmapRenderer renderer, DisplayOutput output)
		{
			OnRenderImageOverlay(new RenderEventArgs(renderer, output));
		}

		protected void RenderImageSafe(IBitmapRenderer bitmapRenderer, DisplayOutput output, RenderType renderType = RenderType.Default)
		{
			try
			{
				RenderImage(bitmapRenderer, output, renderType);
			}
			catch
			{
			}
		}

		public void RenderScene(Graphics gr, DisplayOutput output)
		{
			IBitmapRenderer bitmapRenderer = renderer;
			if (!base.IsHandleCreated || bitmapRenderer == null)
			{
				return;
			}
			if (bitmapRenderer.IsLocked)
			{
				Invalidate();
				return;
			}
			try
			{
				if (bitmapRenderer.BeginScene(gr))
				{
					RenderImage(bitmapRenderer, output);
					RenderImageOverlay(bitmapRenderer, output);
				}
			}
			catch (Exception e)
			{
				if (HandleRendererError(e))
				{
					bitmapRenderer = null;
				}
				Invalidate();
			}
			finally
			{
				try
				{
					bitmapRenderer.EndScene();
				}
				catch
				{
				}
				LastRenderedDisplay = output;
			}
		}

		public virtual Bitmap CreatePageImage()
		{
			Bitmap bitmap = null;
			try
			{
				Rectangle rectangle = new Rectangle(Point.Empty, GetImageSize());
				bitmap = new Bitmap(rectangle.Width, rectangle.Height, PixelFormat.Format24bppRgb);
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					DrawImage(new BitmapGdiRenderer(graphics), rectangle, rectangle);
				}
				return bitmap;
			}
			catch
			{
				bitmap.SafeDispose();
				return null;
			}
		}

		public Point ClientToImage(Point pt, bool withOffset = true)
		{
			return ClientToImage(LastRenderedDisplay ?? Display, pt, withOffset);
		}

		public Point ClientToImage(DisplayOutput output, Point pt, bool withOffset = true)
		{
			if (output.Transform == null)
			{
				return Point.Empty;
			}
			try
			{
				Point[] array = new Point[1]
				{
					pt
				};
				System.Drawing.Drawing2D.Matrix transform = output.Transform;
				transform.Invert();
				transform.TransformPoints(array);
				transform.Invert();
				pt = array[0];
				if (withOffset)
				{
					pt.Offset(output.PartBounds.Location);
				}
				return pt;
			}
			catch
			{
				return Point.Empty;
			}
		}

		public void MovePartDown(float percent)
		{
			MovePart(new Point(0, (int)((float)Display.OutputBounds.Height * percent)));
		}

		public bool MovePart(Point offset)
		{
			ImagePartInfo ipi = (partScrollRunner.Enabled ? scrollPartEnd : ImageVisiblePart);
			Point offset2 = offset;
			offset2.Offset(ipi.Offset);
			Point partOffset = display.GetPartOffset(ipi.Part, offset2);
			ImagePartInfo ipi2 = new ImagePartInfo(ipi.Part, partOffset);
			if (SmoothScrolling)
			{
				ScrollToPart(ipi2);
			}
			else
			{
				ImageVisiblePart = ipi2;
			}
			Rectangle part = Display.GetPart(ipi2);
			Rectangle part2 = Display.GetPart(ipi);
			if (Math.Abs(part.X - part2.X) <= 1)
			{
				return Math.Abs(part.Y - part2.Y) > 1;
			}
			return true;
		}

		public bool DisplayPart(PartPageToDisplay ptd)
		{
			if (!IsValid)
			{
				return false;
			}
			ImagePartInfo imagePartInfo = ((ptd != 0 && ptd != PartPageToDisplay.Last && partScrollRunner.Enabled) ? scrollPartEnd : ImageVisiblePart);
			ImagePartInfo ipi;
			switch (ptd)
			{
			default:
				if (Display.IsStartPart(imagePartInfo))
				{
					return false;
				}
				ipi = new ImagePartInfo(0, 0, 0);
				break;
			case PartPageToDisplay.Previous:
				if (Display.IsStartPart(imagePartInfo))
				{
					return false;
				}
				ipi = Display.GetBestPartFit(imagePartInfo);
				ipi = new ImagePartInfo(ipi.Part - 1, 0, ipi.Offset.Y);
				break;
			case PartPageToDisplay.Next:
				if (Display.IsEndPart(imagePartInfo))
				{
					return false;
				}
				ipi = Display.GetBestPartFit(imagePartInfo);
				ipi = new ImagePartInfo(ipi.Part + 1, 0, ipi.Offset.Y);
				break;
			case PartPageToDisplay.Last:
				if (Display.IsEndPart(imagePartInfo))
				{
					return false;
				}
				ipi = new ImagePartInfo(Display.PartCount - 1, 0, 0);
				break;
			}
			if (SmoothScrolling)
			{
				ScrollToPart(ipi);
			}
			else
			{
				ImageVisiblePart = ipi;
			}
			return true;
		}

		public object GetState()
		{
			return DisplayOutput.Create(DisplayConfig, CurrentAnamorphicTolerance);
		}

		public void Animate(object state1, object state2, int time)
		{
			if (!DisplayChangeAnimation || !base.IsHandleCreated || renderer == null || !renderer.IsHardware)
			{
				return;
			}
			DisplayOutput a = state1 as DisplayOutput;
			DisplayOutput b = state2 as DisplayOutput;
			if (a != null && !a.IsEmpty && b != null && !b.IsEmpty && !a.Equals(b))
			{
				RenderScene(null, a);
				ThreadUtility.Animate(time, delegate(float p)
				{
					RenderScene(null, DisplayOutput.Interpolate(a, b, p));
				});
				RenderScene(null, b);
			}
		}

		public void Animate(Action<float> animate, int time)
		{
			if (!DisplayChangeAnimation || !base.IsHandleCreated || renderer == null || !renderer.IsHardware)
			{
				return;
			}
			try
			{
				blockPaint = true;
				ThreadUtility.Animate(time, delegate(float p)
				{
					animate(p);
					RenderScene(null, Display);
				});
			}
			finally
			{
				blockPaint = false;
			}
			Invalidate();
		}

		public bool SetRenderer(bool hardware)
		{
			try
			{
				if (hardware && HardwareAcceleration != 0 && !DisableHardwareAcceleration)
				{
					if (renderer != null && renderer.IsHardware)
					{
						return true;
					}
					if (renderer != null && renderer is IDisposable)
					{
						IDisposable disposable = renderer as IDisposable;
						renderer = null;
						disposable.Dispose();
					}
					ControlOpenGlRenderer controlOpenGlRenderer = null;
					bool flag;
					try
					{
						controlOpenGlRenderer = new ControlOpenGlRenderer(this, registerPaint: false, (TextureManagerSettings)HardwareSettings.Clone())
						{
							EnableFilter = hardwareFiltering
						};
						flag = controlOpenGlRenderer.IsSoftwareRenderer;
					}
					catch
					{
						flag = true;
					}
					if (!flag || HardwareAcceleration == HardwareAccelerationType.Forced)
					{
						renderer = controlOpenGlRenderer;
						return true;
					}
					controlOpenGlRenderer?.Dispose();
				}
				SetStyle(ControlStyles.OptimizedDoubleBuffer, value: true);
				if (renderer != null)
				{
					if (!renderer.IsHardware)
					{
						return true;
					}
					(renderer as IDisposable).SafeDispose();
				}
				renderer = new BitmapGdiRenderer();
				return !hardware;
			}
			finally
			{
				Invalidate();
			}
		}

		public void ZoomTo(Point location, float zoom)
		{
			if (location.IsEmpty)
			{
				location = PointToClient(Cursor.Position);
			}
			DoZoom(ClientToImage(location), zoom.Clamp(MinimumZoom, MaximumZoom));
		}

		private void ScrollToPart(ImagePartInfo ipi)
		{
			ipi = Display.GetBestPartFit(ipi);
			Point location = Display.PartBounds.Location;
			Point location2 = Display.GetPart(ipi.Part).Location;
			scrollStartOffs = new Point(location.X - location2.X, location.Y - location2.Y);
			scrollEndOffs = ipi.Offset;
			scrollDelta.X = scrollEndOffs.X - scrollStartOffs.X;
			scrollDelta.Y = scrollEndOffs.Y - scrollStartOffs.Y;
			if (!partScrollRunner.Enabled)
			{
				partScrollRunner.Start();
			}
			scrollPartEnd = ipi;
		}

		private void StopPartScrolling()
		{
			scrollLastTime = 0L;
			lowQualityOverride = false;
			partScrollRunner.Stop();
		}

		private void PartScrollTimerTick(object sender, EventArgs e)
		{
			long ticks = Machine.Ticks;
			int num = (int)((scrollLastTime != 0L) ? (ticks - scrollLastTime) : partScrollRunner.Interval);
			Size imageSize = Display.Config.ImageSize;
			float num2 = ((imageSize.Width == 0) ? 0f : (Math.Abs(scrollDelta.X) / (float)imageSize.Width));
			float num3 = ((imageSize.Height == 0) ? 0f : (Math.Abs(scrollDelta.Y) / (float)imageSize.Height));
			float num4 = num2 * (float)PageScrollingTime;
			float num5 = num3 * (float)PageScrollingTime;
			float num6 = ((num4 > 0f) ? (scrollDelta.X * (float)num / num4) : 0f);
			float num7 = ((num5 > 0f) ? (scrollDelta.Y * (float)num / num5) : 0f);
			scrollStartOffs.X += num6;
			scrollStartOffs.Y += num7;
			scrollStartOffs.X = ((scrollDelta.X < 0f) ? Math.Max(scrollStartOffs.X, scrollEndOffs.X) : Math.Min(scrollStartOffs.X, scrollEndOffs.X));
			scrollStartOffs.Y = ((scrollDelta.Y < 0f) ? Math.Max(scrollStartOffs.Y, scrollEndOffs.Y) : Math.Min(scrollStartOffs.Y, scrollEndOffs.Y));
			if (scrollStartOffs == scrollEndOffs)
			{
				ImageVisiblePart = scrollPartEnd;
				return;
			}
			lowQualityOverride = !renderer.IsHardware;
			SetVisiblePart(new ImagePartInfo(scrollPartEnd.Part, (int)scrollStartOffs.X, (int)scrollStartOffs.Y));
			scrollLastTime = ticks;
		}

		private void DoZoom(Point center, float zoom)
		{
			if (zoom < MinimumZoom || zoom > MaximumZoom)
			{
				throw new ArgumentOutOfRangeException("value", "zoom value is out of range");
			}
			if (imageZoom != zoom)
			{
				Rectangle partBounds = Display.PartBounds;
				Point point = center;
				float num = (float)(center.X - partBounds.X) / (float)partBounds.Width;
				float num2 = (float)(center.Y - partBounds.Y) / (float)partBounds.Height;
				imageZoom = zoom;
				Rectangle part = Display.GetPart(0);
				Point point2 = new Point((int)((float)part.X + (float)part.Width * num), (int)((float)part.Y + (float)part.Height * num2));
				ImagePartInfo imagePartInfo2 = (ImageVisiblePart = new ImagePartInfo(0, point.X - point2.X, point.Y - point2.Y));
				OnPageDisplayModeChanged();
				Invalidate();
			}
		}

		protected bool HandleRendererError(Exception e)
		{
			if (e != null && e.ToString().Contains("Tao."))
			{
				SetRenderer(hardware: false);
				return true;
			}
			return false;
		}

		private void SetVisiblePart(ImagePartInfo value)
		{
			using (ItemMonitor.Lock(this))
			{
				value = Display.GetBestPartFit(value);
				if (object.Equals(imageVisiblePart, value))
				{
					return;
				}
				imageVisiblePart = value;
			}
			OnVisiblePartChanged();
			Invalidate();
		}

		private void FireMouseHWheel(int wParam, int lParam)
		{
			Point point = new Point(lParam);
			int delta = wParam >> 16;
			OnMouseHWheel(new MouseEventArgs(MouseButtons.None, 0, point.X, point.Y, delta));
		}

		private void AutoHideCursorTimerTick(object sender, EventArgs e)
		{
			if (cursorAutoHide && autoHideCounter >= 0)
			{
				autoHideCounter -= autoHideCursorTimer.Interval;
				if (autoHideCounter < 0 && mouseInView)
				{
					ShowCursor(visible: false);
				}
			}
		}

		private void ShowCursor(bool visible)
		{
			if (cursorVisible != visible)
			{
				cursorVisible = visible;
				if (cursorVisible)
				{
					Cursor.Show();
				}
				else
				{
					Cursor.Hide();
				}
			}
		}

		private void UpdateCursorAutoHide()
		{
			ShowCursor(visible: true);
			if (cursorAutoHide && autoHideCursorDelay > 0 && mouseInView)
			{
				autoHideCounter = autoHideCursorDelay;
			}
		}

		protected virtual void OnMouseHWheel(MouseEventArgs e)
		{
			if (this.MouseHWheel != null)
			{
				this.MouseHWheel(this, e);
			}
		}

		protected virtual void OnPageDisplayModeChanged()
		{
			if (this.PageDisplayModeChanged != null)
			{
				this.PageDisplayModeChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnVisiblePartChanged()
		{
			if (this.VisiblePartChanged != null)
			{
				this.VisiblePartChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnUpdateDisplayConfig()
		{
			if (this.UpdateDisplayConfig != null)
			{
				this.UpdateDisplayConfig(this, EventArgs.Empty);
			}
		}

		protected virtual void OnPreviewGesture(GestureEventArgs e)
		{
			if (this.PreviewGesture != null)
			{
				this.PreviewGesture(this, e);
			}
		}

		protected virtual void OnGesture(GestureEventArgs e)
		{
			e.Handled = true;
			OnPreviewGesture(e);
			if (e.Handled)
			{
				e.Handled = false;
				if (this.Gesture != null)
				{
					this.Gesture(this, e);
				}
			}
		}

		protected virtual bool IsImageValid()
		{
			return false;
		}

		protected virtual Size GetImageSize()
		{
			return Size.Empty;
		}

		protected virtual void DrawImage(IBitmapRenderer renderer, Rectangle destination, Rectangle source, bool clipToDestination)
		{
		}

		protected void DrawImage(IBitmapRenderer renderer, Rectangle destination, Rectangle source)
		{
			DrawImage(renderer, destination, source, ClipToDestination);
		}

		protected virtual Color GetAutoBackgroundColor()
		{
			return Color.Empty;
		}

		protected virtual void OnRenderImageOverlay(RenderEventArgs e)
		{
			if (this.RendeImageOverlay != null)
			{
				this.RendeImageOverlay(this, e);
			}
		}

		protected virtual bool IsMouseOk(Point point)
		{
			return true;
		}

		protected virtual void OnImageDisplayOptionsChanged()
		{
		}

		protected virtual void OnReadingModeChanged()
		{
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			InitWindowsTouch();
			SetRenderer(hardware: true);
		}

		private void StopTimer()
		{
			mouseClickTimer.Stop();
		}

		private void StartTimer()
		{
			mouseClickTimer.Stop();
			mouseClickTimer.Start();
		}

		private void mouseClickTimer_Tick(object sender, EventArgs e)
		{
			if (pendingClick != 0)
			{
				HandleClick(pendingClick, doubleClick: false);
				pendingClick = MouseButtons.None;
			}
			StopTimer();
		}

		protected override void OnDoubleClick(EventArgs e)
		{
			StopTimer();
			pendingClick = MouseButtons.None;
			HandleClick(lastMouseButton, doubleClick: true, this.IsTouchMessage());
		}

		protected override void OnClick(EventArgs e)
		{
			pendingClick = lastMouseButton;
			StartTimer();
		}

		public GestureArea GestureHitTest(Point pt)
		{
			int width = FormUtility.ScaleDpiX(EngineConfiguration.Default.GestureAreaSize);
			int height = FormUtility.ScaleDpiY(EngineConfiguration.Default.GestureAreaSize);
			Rectangle rectangle = new Rectangle(0, 0, width, height);
			foreach (ContentAlignment value in Enum.GetValues(typeof(ContentAlignment)))
			{
				Rectangle area = rectangle.Align(base.ClientRectangle, value);
				if (area.Contains(pt))
				{
					return new GestureArea(value, area);
				}
			}
			return null;
		}

		private void HandleClick(MouseButtons button, bool doubleClick, bool isTouch = false)
		{
			if (!MouseHandled)
			{
				Point point = PointToClient(Cursor.Position);
				GestureArea gestureArea = GestureHitTest(point);
				bool flag = false;
				if (gestureArea != null)
				{
					GestureEventArgs gestureEventArgs = new GestureEventArgs(GestureType.Touch)
					{
						Area = gestureArea.Alignment,
						AreaBounds = gestureArea.Area,
						Double = doubleClick
					};
					OnGesture(gestureEventArgs);
					flag = gestureEventArgs.Handled;
				}
				if (!flag)
				{
					OnGesture(new GestureEventArgs(GestureType.Click)
					{
						MouseButton = button,
						Location = point,
						Double = doubleClick,
						IsTouch = isTouch
					});
				}
			}
		}

		// Handles the mouse wheel messages, specifically for horizontal scrolling.
		protected override void DefWndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == Native.WM_MOUSEHWHEEL)
			{
				try
				{
					FireMouseHWheel(m.WParam.ToInt32(), m.LParam.ToInt32());
					m.Result = (IntPtr)1;
				}
				catch (Exception)
				{
				}
			}
			base.DefWndProc(ref m);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (inPaint)
			{
				Invalidate();
			}
			else
			{
				if (blockPaint)
				{
					return;
				}
				try
				{
					inPaint = true;
					base.OnPaint(e);
					DisplayOutput displayOutput = Display;
					DisplayOutputConfig config = LastRenderedDisplay.Config;
					if (DisplayChangeAnimation && base.IsHandleCreated && renderer != null && renderer.IsHardware && IsImageValid() && displayOutput.Config.Rotation != config.Rotation)
					{
						using (DisplayOutput state = DisplayOutput.Create(config, CurrentAnamorphicTolerance))
						{
							Animate(state, displayOutput, EngineConfiguration.Default.AnimationDuration);
						}
					}
					RenderScene(e.Graphics, displayOutput);
					if (string.IsNullOrEmpty(Text))
					{
						return;
					}
					using (StringFormat format = new StringFormat
					{
						LineAlignment = StringAlignment.Center,
						Alignment = StringAlignment.Center
					})
					{
						using (SolidBrush brush = new SolidBrush(ForeColor))
						{
							e.Graphics.DrawString(Text, Font, brush, base.ClientRectangle, format);
						}
					}
				}
				finally
				{
					inPaint = false;
				}
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			lastMouseButton = e.Button;
			Focus();
			if (e.Button != MouseButtons.Right)
			{
				orgPart = ImageVisiblePart;
				orgZoom = ImageZoom;
				clickPoint = e.Location;
				flowMouseDelta = (flowMinDelta = PointF.Empty);
				MouseActionHappened = false;
				if (renderer != null && renderer.IsHardware && FlowingMouseScrolling)
				{
					flowRunner.Start();
				}
			}
			UpdateCursorAutoHide();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.Button == MouseButtons.Left && IsMouseOk(e.Location) && !clickPoint.IsEmpty && (Math.Abs(clickPoint.X - e.X) > 5 || Math.Abs(clickPoint.Y - e.Y) > 5))
			{
				MouseActionHappened = true;
				if (!FullImageVisible && !DisableScrolling)
				{
					Cursor.Current = Cursors.Hand;
					Point point = ClientToImage(clickPoint);
					Point point2 = ClientToImage(e.Location);
					Point offset = new Point(orgPart.Offset.X + (point.X - point2.X), orgPart.Offset.Y + (point.Y - point2.Y));
					SetVisiblePart(new ImagePartInfo(orgPart.Part, offset));
				}
			}
			if (e.Button == MouseButtons.Middle)
			{
				DoZoom(ClientToImage(clickPoint), (orgZoom + (float)(e.Location.Y - clickPoint.Y) / 100f).Clamp(MinimumZoom, MaximumZoom));
				MouseActionHappened = true;
			}
			UpdateCursorAutoHide();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			clickPoint = Point.Empty;
			base.OnMouseUp(e);
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			if (!MouseActionHappened)
			{
				base.OnMouseClick(e);
			}
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			mouseInView = true;
			UpdateCursorAutoHide();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			mouseInView = false;
			UpdateCursorAutoHide();
		}

		private void FlowTimerTick(object sender, EventArgs e)
		{
			long ticks = Machine.Ticks;
			if (!clickPoint.IsEmpty || !panStart.IsEmpty)
			{
				flowRunner.Interval = 25;
				flowLastTime = 0L;
				flowMinDelta = PointF.Empty;
				Point pt = PointToClient(panStart.IsEmpty ? Cursor.Position : panLocation);
				if (!flowLastPoint.IsEmpty)
				{
					Point point = ClientToImage(flowLastPoint);
					Point point2 = ClientToImage(pt);
					float num = (float)(ticks - flowLastPointTime) * 2f;
					flowMouseDelta = new PointF((float)(point.X - point2.X) / num, (float)(point.Y - point2.Y) / num);
				}
				flowLastPoint = pt;
				flowLastPointTime = ticks;
				return;
			}
			flowRunner.Interval = 10;
			flowLastPointTime = 0L;
			if (flowMinDelta.IsEmpty)
			{
				float num2 = 1000f / (float)flowRunner.Interval;
				flowMinDelta.X = flowMouseDelta.X / num2;
				flowMinDelta.Y = flowMouseDelta.Y / num2;
			}
			ImagePartInfo imagePartInfo = ImageVisiblePart;
			float num3 = ((flowLastTime <= 0) ? ((float)flowRunner.Interval) : ((float)(ticks - flowLastTime)));
			flowMouseDelta.X -= flowMinDelta.X;
			flowMouseDelta.Y -= flowMinDelta.Y;
			flowMouseDelta.X = ((flowMinDelta.X > 0f) ? Math.Max(0f, flowMouseDelta.X) : Math.Min(0f, flowMouseDelta.X));
			flowMouseDelta.Y = ((flowMinDelta.Y > 0f) ? Math.Max(0f, flowMouseDelta.Y) : Math.Min(0f, flowMouseDelta.Y));
			if (flowMouseDelta.IsEmpty)
			{
				flowRunner.Stop();
				flowRunner.Interval = 25;
			}
			else
			{
				Point offset = new Point((int)((float)imagePartInfo.Offset.X + flowMouseDelta.X * num3), (int)((float)imagePartInfo.Offset.Y + flowMouseDelta.Y * num3));
				SetVisiblePart(new ImagePartInfo(imagePartInfo.Part, offset));
			}
			flowLastTime = ticks;
		}

		public void FlipDisplayOption(ImageDisplayOptions mask)
		{
			SetDisplayOption(mask, !HasDisplayOption(mask));
		}

		public void SetDisplayOption(ImageDisplayOptions mask, bool on)
		{
			if (on)
			{
				ImageDisplayOptions |= mask;
			}
			else
			{
				ImageDisplayOptions ^= mask;
			}
		}

		public bool HasDisplayOption(ImageDisplayOptions mask)
		{
			return HasDisplayOption(ImageDisplayOptions, mask);
		}

		private void InitWindowsTouch()
		{
			try
			{
				gestureHandler = Factory.CreateHandler<GestureHandler>(this);
				gestureHandler.DisableGutter = true;
				gestureHandler.RotateBegin += gestureHandler_RotateBegin;
				gestureHandler.Rotate += gestureHandler_Rotate;
				gestureHandler.ZoomBegin += gestureHandler_ZoomBegin;
				gestureHandler.Zoom += gestureHandler_Zoom;
				gestureHandler.PanBegin += gestureHandler_PanBegin;
				gestureHandler.Pan += gestureHandler_Pan;
				gestureHandler.PanEnd += gestureHandler_PanEnd;
				gestureHandler.TwoFingerTap += gestureHandler_TwoFingerTap;
				gestureHandler.PressAndTap += gestureHandler_PressAndTap;
			}
			catch (Exception)
			{
			}
		}

		private void gestureHandler_RotateBegin(object sender, Windows7.Multitouch.GestureEventArgs e)
		{
			GestureLocation = e.Location;
			OnGestureStart();
			gestureRotation = ImageRotation;
			gestureRotationStart = e.RotateAngle;
		}

		private void gestureHandler_Rotate(object sender, Windows7.Multitouch.GestureEventArgs e)
		{
			double num = gestureRotationStart / Math.PI * 180.0;
			double num2 = e.RotateAngle / Math.PI * 180.0;
			double num3 = num - num2;
			ImageRotation imageRotation2 = (ImageRotation = gestureRotation.Add((int)num3 + 45));
		}

		private void gestureHandler_ZoomBegin(object sender, Windows7.Multitouch.GestureEventArgs e)
		{
			GestureLocation = e.Location;
			OnGestureStart();
			gestureZoomStart = ImageZoom;
			zoomStart = true;
		}

		private void gestureHandler_Zoom(object sender, Windows7.Multitouch.GestureEventArgs e)
		{
			if (zoomStart)
			{
				zoomStart = false;
				zoomOffset = (float)e.ZoomFactor - 1f;
			}
			float value = (float)(e.ZoomFactor - (double)zoomOffset) * gestureZoomStart;
			DoZoom(ClientToImage(GestureLocation), value.Clamp(MinimumZoom, MaximumZoom));
		}

		private void gestureHandler_PanBegin(object sender, Windows7.Multitouch.GestureEventArgs e)
		{
			panLocation = e.Location;
			GestureLocation = e.Location;
			OnGestureStart();
			OnPanStart();
			if (!MouseHandled)
			{
				panStart = panLocation;
				panPart = ImageVisiblePart;
			}
		}

		private void gestureHandler_Pan(object sender, Windows7.Multitouch.GestureEventArgs e)
		{
			if (ignoreEvent == e.LastBeginEvent)
			{
				return;
			}
			panLocation = e.Location;
			OnPan();
			if (MouseHandled || panStart.IsEmpty)
			{
				return;
			}
			if (Display.IsAllVisible && Math.Abs(e.PanVelocity.Width) > 2 && e.PanVelocity.Height == 0)
			{
				ignoreEvent = e.LastBeginEvent;
				if (e.PanVelocity.Width < 0)
				{
					OnGesture(new GestureEventArgs(GestureType.FlickLeft));
				}
				else
				{
					OnGesture(new GestureEventArgs(GestureType.FlickRight));
				}
			}
			else
			{
				Point point = ClientToImage(panStart);
				Point point2 = ClientToImage(panLocation);
				Point offset = new Point(panPart.Offset.X + (point.X - point2.X), panPart.Offset.Y + (point.Y - point2.Y));
				SetVisiblePart(new ImagePartInfo(panPart.Part, offset));
			}
		}

		private void gestureHandler_PanEnd(object sender, Windows7.Multitouch.GestureEventArgs e)
		{
			panLocation = e.Location;
			OnPanEnd();
			panStart = Point.Empty;
		}

		private void gestureHandler_PressAndTap(object sender, Windows7.Multitouch.GestureEventArgs e)
		{
			GestureLocation = (panLocation = e.Location);
			if (e.IsBegin)
			{
				OnGestureStart();
				OnGesture(new GestureEventArgs(GestureType.PressAndTap));
				OnPanStart();
			}
			else if (e.IsEnd)
			{
				OnPanEnd();
			}
			else
			{
				OnPan();
			}
		}

		private void gestureHandler_TwoFingerTap(object sender, Windows7.Multitouch.GestureEventArgs e)
		{
			GestureLocation = (panLocation = e.Location);
			if (e.IsBegin)
			{
				OnGestureStart();
				OnGesture(new GestureEventArgs(GestureType.TwoFingerTap));
				OnPanStart();
				OnPan();
				OnPanEnd();
			}
		}

		protected virtual void OnGestureStart()
		{
		}

		public static bool HasDisplayOption(ImageDisplayOptions option, ImageDisplayOptions mask)
		{
			return (option & mask) != 0;
		}

		protected virtual void OnPanStart()
		{
			if (this.PanStart != null)
			{
				this.PanStart(this, EventArgs.Empty);
			}
		}

		protected virtual void OnPan()
		{
			if (this.Pan != null)
			{
				this.Pan(this, EventArgs.Empty);
			}
		}

		protected virtual void OnPanEnd()
		{
			if (this.PanEnd != null)
			{
				this.PanEnd(this, EventArgs.Empty);
			}
			panStart = Point.Empty;
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			autoHideCursorTimer = new System.Windows.Forms.Timer(components);
			mouseClickTimer = new System.Windows.Forms.Timer(components);
			SuspendLayout();
			autoHideCursorTimer.Enabled = true;
			autoHideCursorTimer.Interval = 1000;
			autoHideCursorTimer.Tick += new System.EventHandler(AutoHideCursorTimerTick);
			mouseClickTimer.Interval = 250;
			mouseClickTimer.Tick += new System.EventHandler(mouseClickTimer_Tick);
			BackColor = System.Drawing.Color.Black;
			base.Name = "BookView";
			ResumeLayout(false);
		}
	}
}
