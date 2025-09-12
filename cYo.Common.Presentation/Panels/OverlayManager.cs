using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.Threading;

namespace cYo.Common.Presentation.Panels
{
	public class OverlayManager : Component
	{
		private Size alignmentBorder = new Size(10, 10);

		private Control control;

		private readonly OverlayPanelCollection panels = new OverlayPanelCollection();

		private bool mouseHandled;

		private const int animationSlice = 25;

		private Thread animationThread;

		private readonly ManualResetEvent animationSignal = new ManualResetEvent(initialState: false);

		private readonly ManualResetEvent animationStopSignal = new ManualResetEvent(initialState: false);

		private OverlayPanel currentPanel;

		private bool mouseDown;

		public Size AlignmentBorder
		{
			get
			{
				using (ItemMonitor.Lock(this))
				{
					return alignmentBorder;
				}
			}
			set
			{
				using (ItemMonitor.Lock(this))
				{
					if (alignmentBorder == value)
					{
						return;
					}
					alignmentBorder = value;
				}
				AlignPanels();
			}
		}

		public Control Control
		{
			get
			{
				return control;
			}
			set
			{
				if (control == value)
				{
					return;
				}
				if (control != null)
				{
					control.SizeChanged -= control_SizeChanged;
					control.MouseDown -= control_MouseDown;
					control.MouseUp -= control_MouseUp;
					control.MouseEnter -= control_MouseEnter;
					control.MouseMove -= control_MouseMove;
					control.MouseLeave -= control_MouseLeave;
					control.MouseClick -= control_MouseClick;
					control.MouseDoubleClick -= control_MouseDoubleClick;
				}
				if (control is IPanableControl)
				{
					IPanableControl panableControl = control as IPanableControl;
					panableControl.PanStart -= control_PanStart;
					panableControl.Pan -= control_Pan;
					panableControl.PanEnd -= control_PanEnd;
				}
				control = value;
				if (control != null)
				{
					control.SizeChanged += control_SizeChanged;
					control.MouseDown += control_MouseDown;
					control.MouseUp += control_MouseUp;
					control.MouseEnter += control_MouseEnter;
					control.MouseMove += control_MouseMove;
					control.MouseLeave += control_MouseLeave;
					control.MouseClick += control_MouseClick;
					control.MouseDoubleClick += control_MouseDoubleClick;
					if (control is IPanableControl)
					{
						IPanableControl panableControl2 = control as IPanableControl;
						panableControl2.PanStart += control_PanStart;
						panableControl2.Pan += control_Pan;
						panableControl2.PanEnd += control_PanEnd;
					}
				}
			}
		}

		public OverlayPanelCollection Panels => panels;

		public bool MouseHandled => mouseHandled;

		public bool AnimationEnabled
		{
			get
			{
				return animationThread != null;
			}
			set
			{
				using (ItemMonitor.Lock(this))
				{
					if (value)
					{
						if (animationThread == null)
						{
							animationThread = ThreadUtility.CreateWorkerThread("Overlay Animation Thread", RunAnimation, ThreadPriority.Lowest);
							animationSignal.Set();
							animationStopSignal.Reset();
							animationThread.Start();
						}
					}
					else if (animationThread != null)
					{
						animationStopSignal.Set();
						animationThread.Join();
						animationThread = null;
					}
				}
			}
		}

		protected virtual Rectangle ClientRectangle => control.ClientRectangle;

		protected virtual Rectangle DisplayRectangle
		{
			get
			{
				Rectangle clientRectangle = ClientRectangle;
				clientRectangle.Inflate(-alignmentBorder.Width, -alignmentBorder.Height);
				return clientRectangle;
			}
		}

		public OverlayManager(Control control)
		{
			Control = control;
			panels.Changed += OverlayPanelsChanged;
		}

		public OverlayManager()
			: this(null)
		{
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Control = null;
				AnimationEnabled = false;
			}
			base.Dispose(disposing);
		}

		private void RunAnimation()
		{
			ManualResetEvent[] waitHandles = new ManualResetEvent[2]
			{
				animationSignal,
				animationStopSignal
			};
			while (WaitHandle.WaitAny(waitHandles) == 0)
			{
				animationSignal.Reset();
				while (!animationStopSignal.WaitOne(animationSlice))
				{
					bool stillRunning = false;
					panels.ForEach(delegate(OverlayPanel op)
					{
						stillRunning |= op.Animate();
					}, copy: true);
					panels.RemoveRange(panels.FindAll((OverlayPanel op) => op.Animators.AllCompleted && op.DestroyAfterCompletion));
					if (!stillRunning)
					{
						break;
					}
				}
			}
		}

		public void NotifyAnimationStart()
		{
			animationSignal.Set();
		}

		public void Draw(IBitmapRenderer graphics)
		{
			OverlayPanel[] array = panels.ToArray();
			foreach (OverlayPanel overlayPanel in array)
			{
				if (overlayPanel.IsVisible && graphics.IsVisible(overlayPanel.Bounds))
				{
					overlayPanel.Draw(graphics, overlayPanel.Bounds);
				}
			}
		}

		public void Draw(Graphics graphics)
		{
			Draw(new BitmapGdiRenderer(graphics));
		}

		public OverlayPanel HitTest(Point pt)
		{
			return (from op in panels.ToArray().Reverse()
				select op.HitTest(pt)).FirstOrDefault((OverlayPanel hit) => hit != null);
		}

		public virtual void Invalidate()
		{
			if (Control != null)
			{
				Control.Invalidate();
			}
		}

		public virtual void Invalidate(Rectangle rc)
		{
			if (Control != null)
			{
				Control.Invalidate(rc);
			}
		}

		private void AlignPanels()
		{
			panels.ForEach(delegate(OverlayPanel op)
			{
				if (op.AutoAlign)
				{
					op.Align(op.IgnoreParentMargin ? ClientRectangle : DisplayRectangle, op.Alignment);
				}
			});
		}

		private void control_SizeChanged(object sender, EventArgs e)
		{
			AlignPanels();
		}

		private void OnPanelInvalidated(object sender, PanelInvalidateEventArgs e)
		{
			Invalidate(e.Bounds);
		}

		private void OnPanelSizeChanged(object sender, EventArgs e)
		{
			AlignPanels();
		}

		private void OnPanelAlignmentChanged(object sender, EventArgs e)
		{
			AlignPanels();
		}

		private void OverlayPanelsChanged(object sender, SmartListChangedEventArgs<OverlayPanel> e)
		{
			RegisterEvents(e.Item, e.Action == SmartListAction.Insert);
		}

		protected void RegisterEvents(OverlayPanel op, bool register)
		{
			if (op != null)
			{
				if (register)
				{
					op.Manager = this;
					op.PanelInvalidated += OnPanelInvalidated;
					op.SizeChanged += OnPanelSizeChanged;
					op.AlignmentChanged += OnPanelAlignmentChanged;
					AlignPanels();
					op.InvalidatePanel();
				}
				else
				{
					op.InvalidatePanel();
					op.Manager = null;
					op.PanelInvalidated -= OnPanelInvalidated;
					op.SizeChanged -= OnPanelSizeChanged;
					op.AlignmentChanged -= OnPanelAlignmentChanged;
				}
			}
		}

		private void control_PanStart(object sender, EventArgs e)
		{
			IPanableControl panableControl = control as IPanableControl;
			MouseEventArgs e2 = new MouseEventArgs(MouseButtons.Left, 1, panableControl.PanLocation.X, panableControl.PanLocation.Y, 0);
			control_MouseMove(sender, e2);
			control_MouseDown(sender, e2);
		}

		private void control_Pan(object sender, EventArgs e)
		{
			IPanableControl panableControl = control as IPanableControl;
			MouseEventArgs e2 = new MouseEventArgs(MouseButtons.Left, 1, panableControl.PanLocation.X, panableControl.PanLocation.Y, 0);
			control_MouseMove(sender, e2);
		}

		private void control_PanEnd(object sender, EventArgs e)
		{
			IPanableControl panableControl = control as IPanableControl;
			MouseEventArgs e2 = new MouseEventArgs(MouseButtons.Left, 1, panableControl.PanLocation.X, panableControl.PanLocation.Y, 0);
			control_MouseUp(sender, e2);
		}

		private static MouseEventArgs GetMouseEventArgsOffset(MouseEventArgs e, Point offset)
		{
			Point point = new Point(e.X - offset.X, e.Y - offset.Y);
			return new MouseEventArgs(e.Button, e.Clicks, point.X, point.Y, e.Delta);
		}

		private void control_MouseClick(object sender, MouseEventArgs e)
		{
			Point pt = Control.PointToClient(Cursor.Position);
			OverlayPanel overlayPanel = HitTest(pt);
			overlayPanel?.FireClick();
			mouseHandled = overlayPanel != null;
		}

		private void control_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			Point pt = Control.PointToClient(Cursor.Position);
			OverlayPanel overlayPanel = HitTest(pt);
			overlayPanel?.FireDoubleClick();
			mouseHandled = overlayPanel != null;
		}

		private void control_MouseEnter(object sender, EventArgs e)
		{
			Point point = Control.PointToClient(Cursor.Position);
			mouseHandled = HandleMouseMove(new MouseEventArgs(MouseButtons.None, 0, point.X, point.Y, 0));
		}

		private void control_MouseLeave(object sender, EventArgs e)
		{
			mouseHandled = HandleMouseMove(new MouseEventArgs(MouseButtons.None, 0, int.MaxValue, int.MinValue, 0));
		}

		private void control_MouseMove(object sender, MouseEventArgs e)
		{
			mouseHandled = HandleMouseMove(e);
		}

		private void control_MouseUp(object sender, MouseEventArgs e)
		{
			if (currentPanel != null)
			{
				currentPanel.FireMouseUp(GetMouseEventArgsOffset(e, currentPanel.GetAbsoluteLocation()));
				mouseHandled = HandleMouseMove(e);
			}
			else
			{
				mouseHandled = false;
			}
			mouseDown = false;
		}

		private void control_MouseDown(object sender, MouseEventArgs e)
		{
			mouseDown = true;
			if (currentPanel != null)
			{
				mouseHandled = true;
				currentPanel.FireMouseDown(GetMouseEventArgsOffset(e, currentPanel.GetAbsoluteLocation()));
			}
			else
			{
				mouseHandled = false;
			}
		}

		private bool HandleMouseMove(MouseEventArgs e)
		{
			if (mouseDown)
			{
				if (currentPanel != null)
				{
					currentPanel.FireMouseMove(GetMouseEventArgsOffset(e, currentPanel.GetAbsoluteLocation()));
					return true;
				}
			}
			else
			{
				OverlayPanel overlayPanel = HitTest(e.Location);
				if (currentPanel != null && overlayPanel != currentPanel)
				{
					currentPanel.FireMouseLeave(GetMouseEventArgsOffset(e, currentPanel.GetAbsoluteLocation()));
					currentPanel = null;
				}
				if (overlayPanel != null)
				{
					currentPanel = overlayPanel;
					currentPanel.FireMouseEnter(GetMouseEventArgsOffset(e, currentPanel.GetAbsoluteLocation()));
				}
				if (currentPanel != null)
				{
					currentPanel.FireMouseMove(GetMouseEventArgsOffset(e, currentPanel.GetAbsoluteLocation()));
					return true;
				}
			}
			return false;
		}
	}
}
