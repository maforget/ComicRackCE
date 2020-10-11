using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Mathematics;
using cYo.Common.Threading;

namespace cYo.Common.Presentation.Panels
{
	public class OverlayPanel : DisposableObject
	{
		private Bitmap surface;

		private readonly object surfaceLock = new object();

		private volatile OverlayPanelCollection panels;

		private volatile object tag;

		private volatile bool enabled = true;

		private volatile bool alignmentEnabled;

		private volatile ContentAlignment alignment = ContentAlignment.MiddleCenter;

		private Point alignmentOffset;

		private bool ignoreParentMargin;

		private Padding margin = new Padding(4);

		private volatile bool visible = true;

		private volatile bool destroyAfterCompletion;

		private volatile float opacity = 1f;

		private volatile float saturation;

		private volatile float contrast;

		private volatile float brightness;

		private volatile float scale = 1f;

		private Point location;

		private Size size;

		private readonly AnimatorCollection animators = new AnimatorCollection();

		private PanelState panelState;

		private HitTestType hitTestType = HitTestType.Alpha;

		private Color backgroundColor = Color.Transparent;

		private OverlayManager manager;

		private volatile OverlayPanel parent;

		private readonly List<Rectangle> invalidatedBounds = new List<Rectangle>();

		private bool dirty;

		public OverlayPanelCollection Panels
		{
			get
			{
				if (panels == null)
				{
					panels = new OverlayPanelCollection();
					panels.Changed += overlayPanels_Changed;
				}
				return panels;
			}
		}

		public object Tag
		{
			get
			{
				return tag;
			}
			set
			{
				tag = value;
			}
		}

		public bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				enabled = value;
			}
		}

		public bool AutoAlign
		{
			get
			{
				return alignmentEnabled;
			}
			set
			{
				if (alignmentEnabled != value)
				{
					alignmentEnabled = value;
					InvalidatePanel();
					OnAlignmentChanged();
				}
			}
		}

		public ContentAlignment Alignment
		{
			get
			{
				return alignment;
			}
			set
			{
				if (alignment != value)
				{
					alignment = value;
					InvalidatePanel();
					OnAlignmentChanged();
				}
			}
		}

		public Point AlignmentOffset
		{
			get
			{
				using (ItemMonitor.Lock(this))
				{
					return alignmentOffset;
				}
			}
			set
			{
				using (ItemMonitor.Lock(this))
				{
					if (!(alignmentOffset == value))
					{
						alignmentOffset = value;
						InvalidatePanel();
						OnAlignmentChanged();
					}
				}
			}
		}

		public bool IgnoreParentMargin
		{
			get
			{
				return ignoreParentMargin;
			}
			set
			{
				if (ignoreParentMargin != value)
				{
					ignoreParentMargin = value;
					InvalidatePanel();
					OnAlignmentChanged();
				}
			}
		}

		public Padding Margin
		{
			get
			{
				return margin;
			}
			set
			{
				if (!(margin == value))
				{
					margin = value;
					InvalidatePanel();
					OnMarginChanged();
				}
			}
		}

		public bool Visible
		{
			get
			{
				return visible;
			}
			set
			{
				if (visible != value)
				{
					visible = value;
					InvalidatePanel(always: true);
					OnVisibleChanged();
				}
			}
		}

		public bool IsVisible
		{
			get
			{
				if (visible)
				{
					return opacity > 0.05f;
				}
				return false;
			}
		}

		public bool DestroyAfterCompletion
		{
			get
			{
				return destroyAfterCompletion;
			}
			set
			{
				destroyAfterCompletion = value;
			}
		}

		public float Opacity
		{
			get
			{
				return opacity;
			}
			set
			{
				value = value.Clamp(0f, 1f);
				if (opacity != value)
				{
					opacity = value;
					OnOpacityChanged();
				}
			}
		}

		public float Saturation
		{
			get
			{
				return saturation;
			}
			set
			{
				if (saturation != value)
				{
					saturation = value;
					OnSaturationChanged();
				}
			}
		}

		public float Contrast
		{
			get
			{
				return contrast;
			}
			set
			{
				if (contrast != value)
				{
					contrast = value;
					OnContrastChanged();
				}
			}
		}

		public float Brightness
		{
			get
			{
				return brightness;
			}
			set
			{
				if (brightness != value)
				{
					brightness = value;
					OnBrightnessChanged();
				}
			}
		}

		public float Scale
		{
			get
			{
				return scale;
			}
			set
			{
				if (scale != value)
				{
					FireInvalidate(Bounds, always: false);
					scale = value;
					FireInvalidate(Bounds, always: false);
					OnScaleChanged();
				}
			}
		}

		public Point Location
		{
			get
			{
				using (ItemMonitor.Lock(this))
				{
					return location;
				}
			}
			set
			{
				if (!(Location == value))
				{
					Point point;
					using (ItemMonitor.Lock(this))
					{
						point = Location;
						location = value;
					}
					FireInvalidate(new Rectangle(point, GetScaledSize(Size)), always: false);
					FireInvalidate(new Rectangle(value, GetScaledSize(Size)), always: false);
					OnLocationChanged();
				}
			}
		}

		public Point CenterLocation
		{
			get
			{
				Point result = Location;
				result.Offset(Width / 2, Height / 2);
				return result;
			}
			set
			{
				value.Offset(-Width / 2, -Height / 2);
				Location = value;
			}
		}

		public Size Size
		{
			get
			{
				using (ItemMonitor.Lock(this))
				{
					return size;
				}
			}
			set
			{
				if (Size == value)
				{
					return;
				}
				using (ItemMonitor.Lock(surfaceLock))
				{
					if (surface != null && surface.Size != value)
					{
						Bitmap image = new Bitmap(value.Width, value.Height, PixelFormat.Format32bppArgb);
						using (Graphics graphics = Graphics.FromImage(image))
						{
							graphics.DrawImage(surface, new Rectangle(Point.Empty, value));
						}
						surface.SafeDispose();
						surface = image;
					}
				}
				Size sz;
				using (ItemMonitor.Lock(this))
				{
					sz = Size;
					size = value;
				}
				FireInvalidate(new Rectangle(Location, GetScaledSize(sz)), always: false);
				FireInvalidate(new Rectangle(Location, GetScaledSize(value)), always: false);
				OnSizeChanged();
			}
		}

		public Rectangle Bounds
		{
			get
			{
				using (ItemMonitor.Lock(this))
				{
					return new Rectangle(Location, GetScaledSize());
				}
			}
		}

		public Rectangle PhysicalBounds
		{
			get
			{
				using (ItemMonitor.Lock(this))
				{
					return new Rectangle(Location, Size);
				}
			}
		}

		public Rectangle ClientRectangle => new Rectangle(Point.Empty, Size);

		public Rectangle DisplayRectangle => ClientRectangle.Pad(margin);

		public int X
		{
			get
			{
				return Location.X;
			}
			set
			{
				Location = new Point(value, Y);
			}
		}

		public int Y
		{
			get
			{
				return Location.Y;
			}
			set
			{
				Location = new Point(X, value);
			}
		}

		public int Width
		{
			get
			{
				return Size.Width;
			}
			set
			{
				Size = new Size(value, Height);
			}
		}

		public int Height
		{
			get
			{
				return Size.Height;
			}
			set
			{
				Size = new Size(Width, value);
			}
		}

		public AnimatorCollection Animators => animators;

		public PanelState PanelState
		{
			get
			{
				return panelState;
			}
			set
			{
				if (panelState != value)
				{
					panelState = value;
					OnPanelStateChanged();
				}
			}
		}

		public HitTestType HitTestType
		{
			get
			{
				return hitTestType;
			}
			set
			{
				hitTestType = value;
			}
		}

		public bool HasMouse
		{
			get
			{
				if (PanelState != 0)
				{
					return true;
				}
				if (panels != null)
				{
					return panels.Find((OverlayPanel x) => x.HasMouse) != null;
				}
				return false;
			}
		}

		public Color BackgroundColor
		{
			get
			{
				using (ItemMonitor.Lock(this))
				{
					return backgroundColor;
				}
			}
			set
			{
				using (ItemMonitor.Lock(this))
				{
					if (backgroundColor == value)
					{
						return;
					}
					backgroundColor = value;
				}
				Invalidate();
			}
		}

		public OverlayManager Manager
		{
			get
			{
				for (OverlayPanel overlayPanel = this; overlayPanel != null; overlayPanel = overlayPanel.Parent)
				{
					if (overlayPanel.manager != null)
					{
						return overlayPanel.manager;
					}
				}
				return null;
			}
			set
			{
				manager = value;
			}
		}

		protected OverlayPanel Parent
		{
			get
			{
				return parent;
			}
			set
			{
				parent = value;
			}
		}

		public event EventHandler<PanelInvalidateEventArgs> PanelInvalidated;

		public event EventHandler VisibleChanged;

		public event EventHandler LocationChanged;

		public event EventHandler SizeChanged;

		public event EventHandler ScaleChanged;

		public event EventHandler OpacityChanged;

		public event EventHandler BrightnessChanged;

		public event EventHandler ContrastChanged;

		public event EventHandler SaturationChanged;

		public event EventHandler AlignmentChanged;

		public event EventHandler MarginChanged;

		public event EventHandler PanelStateChanged;

		public event MouseEventHandler MouseEnter;

		public event MouseEventHandler MouseLeave;

		public event MouseEventHandler MouseDown;

		public event MouseEventHandler MouseUp;

		public event MouseEventHandler MouseMove;

		public event EventHandler Click;

		public event EventHandler DoubleClick;

		public event EventHandler Drawing;

		public event PaintEventHandler Paint;

		public event PaintEventHandler PaintBackground;

		public event EventHandler<PanelRenderEventArgs> RenderSurface;

		public OverlayPanel()
		{
			Animators.Changed += Animators_Changed;
		}

		public OverlayPanel(Size size)
			: this()
		{
			this.size = size;
		}

		public OverlayPanel(int width, int height)
			: this(new Size(width, height))
		{
		}

		public OverlayPanel(Image image)
			: this()
		{
			surface = image.CreateCopy();
		}

		public OverlayPanel(Image image, ContentAlignment alignment)
			: this(image)
		{
			this.alignment = alignment;
		}

		public OverlayPanel(int width, int height, ContentAlignment alignment)
			: this(width, height)
		{
			this.alignment = alignment;
		}

		public OverlayPanel(int width, int height, ContentAlignment alignment, IEnumerable<Animator> animators)
			: this(width, height, alignment)
		{
			Animators.AddRange(animators);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				using (ItemMonitor.Lock(surfaceLock))
				{
					if (surface != null)
					{
						surface.Dispose();
					}
					surface = null;
				}
			}
			base.Dispose(disposing);
		}

		public OverlayPanel HitTest(Point pt)
		{
			if (!Enabled || !IsVisible)
			{
				return null;
			}
			if (panels != null)
			{
				Point pt2 = new Point(pt.X - X, pt.Y - Y);
				foreach (OverlayPanel item in panels.ToArray().Reverse())
				{
					OverlayPanel overlayPanel = item.HitTest(pt2);
					if (overlayPanel != null)
					{
						return overlayPanel;
					}
				}
			}
			if (hitTestType != 0 && Bounds.Contains(pt))
			{
				using (ItemMonitor.Lock(surfaceLock))
				{
					if (HitTestType == HitTestType.Bounds)
					{
						return this;
					}
					if (surface == null)
					{
						return null;
					}
					int num = (int)((float)(pt.X - X) / scale);
					int num2 = (int)((float)(pt.Y - Y) / scale);
					if (num >= 0 && num2 >= 0 && num < surface.Width && num2 < surface.Height && (float)(int)surface.GetPixel(num, num2).A * Opacity > 0f)
					{
						return this;
					}
				}
			}
			return null;
		}

		public void Align(Rectangle rc, ContentAlignment alignment)
		{
			Point point = new Rectangle(rc.Location, Bounds.Size).Align(rc, alignment).Location;
			Point point2 = AlignmentOffset;
			point.Offset((int)((float)point2.X * Scale), (int)((float)point2.Y * Scale));
			Location = point;
		}

		public PanelSurface CreateSurface(bool empty)
		{
			using (ItemMonitor.Lock(surfaceLock))
			{
				Bitmap bmp = ((surface == null || empty) ? new Bitmap(Width, Height, PixelFormat.Format32bppArgb) : (surface.Clone() as Bitmap));
				PanelSurface panelSurface = new PanelSurface(bmp);
				panelSurface.Disposed += delegate
				{
					using (ItemMonitor.Lock(surfaceLock))
					{
						if (surface != null)
						{
							surface.Dispose();
						}
						surface = bmp;
					}
					Size = bmp.Size;
				};
				return panelSurface;
			}
		}

		public PanelSurface CreateSurface()
		{
			return CreateSurface(empty: false);
		}

		public virtual void Draw(IBitmapRenderer gr, Rectangle rc)
		{
			if (gr == null)
			{
				throw new ArgumentNullException();
			}
			if (!Visible)
			{
				return;
			}
			try
			{
				Rectangle bounds = Bounds;
				if (gr.IsVisible(bounds))
				{
					OnDrawing();
					using (ItemMonitor.Lock(surfaceLock))
					{
						if (surface != null)
						{
							gr.DrawImage(surface, bounds, new Rectangle(0, 0, surface.Width, surface.Height), new BitmapAdjustment(saturation, brightness, contrast), opacity);
						}
					}
					using (gr.SaveState())
					{
						gr.TranslateTransform(bounds.Location.X, bounds.Location.Y);
						gr.ScaleTransform(scale, scale);
						OnRenderSurface(new PanelRenderEventArgs(gr));
					}
				}
			}
			catch
			{
				throw;
			}
			if (panels == null)
			{
				return;
			}
			using (gr.SaveState())
			{
				gr.TranslateTransform(X, Y);
				panels.ForEach(delegate(OverlayPanel p)
				{
					p.Draw(gr, rc);
				}, copy: true);
			}
		}

		public bool Animate()
		{
			bool stillRunning = false;
			animators.ForEach(delegate(Animator a)
			{
				stillRunning |= a.Animate(this);
			}, copy: true);
			if (panels != null)
			{
				panels.ForEach(delegate(OverlayPanel p)
				{
					stillRunning |= p.Animate();
				}, copy: true);
			}
			return stillRunning;
		}

		public void FireMouseEnter(MouseEventArgs e)
		{
			OnMouseEnter(GetMouseEventArgsScaled(e));
		}

		public void FireMouseLeave(MouseEventArgs e)
		{
			OnMouseLeave(GetMouseEventArgsScaled(e));
		}

		public void FireMouseMove(MouseEventArgs e)
		{
			OnMouseMove(GetMouseEventArgsScaled(e));
		}

		public void FireMouseDown(MouseEventArgs e)
		{
			OnMouseDown(GetMouseEventArgsScaled(e));
		}

		public void FireMouseUp(MouseEventArgs e)
		{
			OnMouseUp(GetMouseEventArgsScaled(e));
		}

		public void FireClick()
		{
			OnClick();
		}

		public void FireDoubleClick()
		{
			OnDoubleClick();
		}

		public Point GetAbsoluteLocation()
		{
			OverlayPanel overlayPanel = Parent;
			Point result = Location;
			while (overlayPanel != null)
			{
				result.Offset(overlayPanel.Location);
				overlayPanel = overlayPanel.Parent;
			}
			return result;
		}

		public void Fill(Color color)
		{
			using (PanelSurface panelSurface = CreateSurface())
			{
				panelSurface.Graphics.Clear(color);
			}
		}

		internal void InvalidatePanel()
		{
			InvalidatePanel(always: false);
		}

		private void InvalidatePanel(bool always)
		{
			InvalidatePanel(ClientRectangle, always);
		}

		private void InvalidatePanel(Rectangle bounds, bool always)
		{
			Rectangle panelBounds = bounds;
			panelBounds.Offset(Location);
			FireInvalidate(panelBounds, always);
		}

		private Size GetScaledSize(Size sz)
		{
			return new Size((int)((float)sz.Width * scale), (int)((float)sz.Height * scale));
		}

		private MouseEventArgs GetMouseEventArgsScaled(MouseEventArgs e)
		{
			int x = (int)((float)e.X / scale);
			int y = (int)((float)e.Y / scale);
			return new MouseEventArgs(e.Button, e.Clicks, x, y, e.Delta);
		}

		private Size GetScaledSize()
		{
			return GetScaledSize(Size);
		}

		private void AlignPanels()
		{
			Rectangle b = DisplayRectangle.Scale(scale);
			Rectangle f = ClientRectangle.Scale(scale);
			if (panels == null)
			{
				return;
			}
			panels.ForEach(delegate(OverlayPanel op)
			{
				if (op.AutoAlign)
				{
					op.Align(op.IgnoreParentMargin ? f : b, op.Alignment);
				}
			});
		}

		protected void FireInvalidate(Rectangle panelBounds, bool always)
		{
			if (IsVisible || always)
			{
				OnPanelInvalidated(new PanelInvalidateEventArgs(panelBounds, always));
			}
		}

		protected virtual void OnVisibleChanged()
		{
			if (this.VisibleChanged != null)
			{
				this.VisibleChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnLocationChanged()
		{
			if (this.LocationChanged != null)
			{
				this.LocationChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnSizeChanged()
		{
			if (this.SizeChanged != null)
			{
				this.SizeChanged(this, EventArgs.Empty);
			}
			AlignPanels();
		}

		protected virtual void OnScaleChanged()
		{
			if (this.ScaleChanged != null)
			{
				this.ScaleChanged(this, EventArgs.Empty);
			}
			AlignPanels();
		}

		protected virtual void OnOpacityChanged()
		{
			if (Visible)
			{
				InvalidatePanel(always: true);
			}
			if (this.OpacityChanged != null)
			{
				this.OpacityChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnContrastChanged()
		{
			if (Visible)
			{
				InvalidatePanel(always: true);
			}
			if (this.ContrastChanged != null)
			{
				this.ContrastChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnBrightnessChanged()
		{
			if (Visible)
			{
				InvalidatePanel(always: true);
			}
			if (this.BrightnessChanged != null)
			{
				this.BrightnessChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnSaturationChanged()
		{
			if (Visible)
			{
				InvalidatePanel(always: true);
			}
			if (this.SaturationChanged != null)
			{
				this.SaturationChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnAlignmentChanged()
		{
			if (this.AlignmentChanged != null)
			{
				this.AlignmentChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnMarginChanged()
		{
			if (this.MarginChanged != null)
			{
				this.MarginChanged(this, EventArgs.Empty);
			}
			AlignPanels();
		}

		protected virtual void OnPanelStateChanged()
		{
			if (this.PanelStateChanged != null)
			{
				this.PanelStateChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnMouseEnter(MouseEventArgs e)
		{
			PanelState = PanelState.Hot;
			if (this.MouseEnter != null)
			{
				this.MouseEnter(this, e);
			}
		}

		protected virtual void OnMouseLeave(MouseEventArgs e)
		{
			PanelState = PanelState.Normal;
			if (this.MouseLeave != null)
			{
				this.MouseLeave(this, e);
			}
		}

		protected virtual void OnMouseDown(MouseEventArgs e)
		{
			PanelState = PanelState.Selected;
			if (this.MouseDown != null)
			{
				this.MouseDown(this, e);
			}
		}

		protected virtual void OnMouseUp(MouseEventArgs e)
		{
			PanelState = PanelState.Hot;
			if (this.MouseUp != null)
			{
				this.MouseUp(this, e);
			}
		}

		protected virtual void OnMouseMove(MouseEventArgs e)
		{
			if (this.MouseMove != null)
			{
				this.MouseMove(this, e);
			}
		}

		protected virtual void OnClick()
		{
			if (this.Click != null)
			{
				this.Click(this, EventArgs.Empty);
			}
		}

		protected virtual void OnDoubleClick()
		{
			if (this.DoubleClick != null)
			{
				this.DoubleClick(this, EventArgs.Empty);
			}
		}

		protected virtual void OnPaintBackground(PaintEventArgs e)
		{
			if (this.PaintBackground != null)
			{
				this.PaintBackground(this, e);
			}
			e.Graphics.Clear(BackgroundColor);
		}

		protected virtual void OnPaint(PaintEventArgs e)
		{
			if (this.Paint != null)
			{
				this.Paint(this, e);
			}
		}

		protected virtual void OnDrawing()
		{
			if (this.Drawing != null)
			{
				this.Drawing(this, EventArgs.Empty);
			}
			while (dirty)
			{
				dirty = false;
				PaintSurface();
			}
		}

		protected virtual void OnRenderSurface(PanelRenderEventArgs e)
		{
			if (this.RenderSurface != null)
			{
				this.RenderSurface(this, e);
			}
		}

		protected virtual void OnPanelInvalidated(PanelInvalidateEventArgs e)
		{
			if (this.PanelInvalidated != null)
			{
				this.PanelInvalidated(this, e);
			}
		}

		private void Animators_Changed(object sender, SmartListChangedEventArgs<Animator> e)
		{
			switch (e.Action)
			{
			case SmartListAction.Insert:
				e.Item.Started += AnimatorStarted;
				break;
			case SmartListAction.Remove:
				e.Item.Started -= AnimatorStarted;
				break;
			}
		}

		private void NotifyManagerStart()
		{
			Manager?.NotifyAnimationStart();
		}

		private void AnimatorStarted(object sender, EventArgs e)
		{
			NotifyManagerStart();
		}

		private void overlayPanels_Changed(object sender, SmartListChangedEventArgs<OverlayPanel> e)
		{
			RegisterEvents(e.Item, e.Action == SmartListAction.Insert);
			NotifyManagerStart();
		}

		protected void RegisterEvents(OverlayPanel op, bool register)
		{
			if (op != null)
			{
				if (register)
				{
					op.Parent = this;
					op.PanelInvalidated += OnPanelInvalidated;
					op.SizeChanged += OnPanelSizeChanged;
					op.AlignmentChanged += OnPanelAlignmentChanged;
					AlignPanels();
					op.InvalidatePanel();
				}
				else
				{
					op.Parent = null;
					op.InvalidatePanel();
					op.PanelInvalidated -= OnPanelInvalidated;
					op.SizeChanged -= OnPanelSizeChanged;
					op.AlignmentChanged -= OnPanelAlignmentChanged;
				}
			}
		}

		private void OnPanelInvalidated(object sender, PanelInvalidateEventArgs e)
		{
			InvalidatePanel(e.Bounds, e.Always);
		}

		private void OnPanelSizeChanged(object sender, EventArgs e)
		{
			AlignPanels();
		}

		private void OnPanelAlignmentChanged(object sender, EventArgs e)
		{
			AlignPanels();
		}

		public void Invalidate()
		{
			Invalidate(Rectangle.Empty);
		}

		public void Invalidate(Rectangle bounds)
		{
			dirty = true;
			using (ItemMonitor.Lock(invalidatedBounds))
			{
				invalidatedBounds.Add(bounds);
			}
			InvalidatePanel();
		}

		private void PaintSurface()
		{
			using (PanelSurface panelSurface = CreateSurface())
			{
				Graphics graphics = panelSurface.Graphics;
				graphics.SetClip(Rectangle.Empty);
				using (ItemMonitor.Lock(invalidatedBounds))
				{
					foreach (Rectangle invalidatedBound in invalidatedBounds)
					{
						if (!invalidatedBound.IsEmpty)
						{
							if (graphics.IsClipEmpty)
							{
								graphics.SetClip(invalidatedBound);
							}
							else
							{
								graphics.SetClip(invalidatedBound, CombineMode.Union);
							}
							continue;
						}
						graphics.SetClip(ClientRectangle);
						break;
					}
					invalidatedBounds.Clear();
				}
				PaintEventArgs e = new PaintEventArgs(graphics, Rectangle.Round(graphics.ClipBounds));
				OnPaintBackground(e);
				OnPaint(e);
			}
		}
	}
}
