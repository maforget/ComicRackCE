using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using cYo.Common.Drawing;

namespace cYo.Common.Windows.Forms
{
	[CLSCompliant(true)]
	[ToolboxItem(false)]
	public class Popup : ToolStripDropDown
	{
		[Flags]
		public enum PopupAnimations
		{
			None = 0x0,
			LeftToRight = 0x1,
			RightToLeft = 0x2,
			TopToBottom = 0x4,
			BottomToTop = 0x8,
			Center = 0x10,
			Slide = 0x40000,
			Blend = 0x80000,
			Roll = 0x100000,
			SystemDefault = 0x200000
		}

		public interface INotifyClose
		{
			void PopupClosed();
		}

		private struct GripBounds
		{
			private const int GripSize = 6;

			private const int CornerGripSize = 12;

			private readonly Rectangle clientRectangle;

			public Rectangle ClientRectangle => clientRectangle;

			public Rectangle Bottom
			{
				get
				{
					Rectangle result = ClientRectangle;
					result.Y = result.Bottom - GripSize + 1;
					result.Height = GripSize;
					return result;
				}
			}

			public Rectangle BottomRight
			{
				get
				{
					Rectangle result = ClientRectangle;
					result.Y = result.Bottom - CornerGripSize + 1;
					result.Height = CornerGripSize;
					result.X = result.Width - CornerGripSize + 1;
					result.Width = CornerGripSize;
					return result;
				}
			}

			public Rectangle Top
			{
				get
				{
					Rectangle result = ClientRectangle;
					result.Height = GripSize;
					return result;
				}
			}

			public Rectangle TopRight
			{
				get
				{
					Rectangle result = ClientRectangle;
					result.Height = CornerGripSize;
					result.X = result.Width - CornerGripSize + 1;
					result.Width = CornerGripSize;
					return result;
				}
			}

			public Rectangle Left
			{
				get
				{
					Rectangle result = ClientRectangle;
					result.Width = GripSize;
					return result;
				}
			}

			public Rectangle BottomLeft
			{
				get
				{
					Rectangle result = ClientRectangle;
					result.Width = CornerGripSize;
					result.Y = result.Height - CornerGripSize + 1;
					result.Height = CornerGripSize;
					return result;
				}
			}

			public Rectangle Right
			{
				get
				{
					Rectangle result = ClientRectangle;
					result.X = result.Right - GripSize + 1;
					result.Width = GripSize;
					return result;
				}
			}

			public Rectangle TopLeft
			{
				get
				{
					Rectangle result = ClientRectangle;
					result.Width = CornerGripSize;
					result.Height = CornerGripSize;
					return result;
				}
			}

			public GripBounds(Rectangle clientRectangle)
			{
				this.clientRectangle = clientRectangle;
			}
		}

		[SuppressUnmanagedCodeSecurity]
		private static class NativeMethods
		{
			[Flags]
			public enum AnimationFlags
			{
				Roll = 0x0,
				HorizontalPositive = 0x1,
				HorizontalNegative = 0x2,
				VerticalPositive = 0x4,
				VerticalNegative = 0x8,
				Center = 0x10,
				Hide = 0x10000,
				Activate = 0x20000,
				Slide = 0x40000,
				Blend = 0x80000,
				Mask = 0xFFFFF
			}

			public struct MINMAXINFO
			{
				public Point reserved;

				public Size maxSize;

				public Point maxPosition;

				public Size minTrackSize;

				public Size maxTrackSize;
			}

			public const int WM_NCHITTEST = 132;

			public const int WM_NCACTIVATE = 134;

			public const int WS_EX_NOACTIVATE = 134217728;

			public const int HTTRANSPARENT = -1;

			public const int HTLEFT = 10;

			public const int HTRIGHT = 11;

			public const int HTTOP = 12;

			public const int HTTOPLEFT = 13;

			public const int HTTOPRIGHT = 14;

			public const int HTBOTTOM = 15;

			public const int HTBOTTOMLEFT = 16;

			public const int HTBOTTOMRIGHT = 17;

			public const int WM_PRINT = 791;

			public const int WM_USER = 1024;

			public const int WM_REFLECT = 8192;

			public const int WM_COMMAND = 273;

			public const int CBN_DROPDOWN = 7;

			public const int WM_GETMINMAXINFO = 36;

			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			public static extern int AnimateWindow(IntPtr windowHandle, int time, AnimationFlags flags);

			public static int HIWORD(int n)
			{
				return (short)((n >> 16) & 0xFFFF);
			}

			public static int HIWORD(IntPtr n)
			{
				return HIWORD((int)(long)n);
			}

			public static int LOWORD(int n)
			{
				return (short)(n & 0xFFFF);
			}

			public static int LOWORD(IntPtr n)
			{
				return LOWORD((int)(long)n);
			}
		}

		private Popup ownerPopup;

		private Popup childPopup;

		private readonly ToolStripControlHost host;

		private Control content;

		private bool focusOnOpen = true;

		private bool acceptAlt = true;

		private bool _resizable;

		private bool resizable;

		private bool resizableTop;

		private bool resizableLeft;

		private VisualStyleRenderer sizeGripRenderer;

		public Control Content => content;

		public PopupAnimations ShowingAnimation
		{
			get;
			set;
		}

		public PopupAnimations HidingAnimation
		{
			get;
			set;
		}

		public int AnimationDuration
		{
			get;
			set;
		}

		public bool FocusOnOpen
		{
			get
			{
				return focusOnOpen;
			}
			set
			{
				focusOnOpen = value;
			}
		}

		public bool AcceptAlt
		{
			get
			{
				return acceptAlt;
			}
			set
			{
				acceptAlt = value;
			}
		}

		public bool Resizable
		{
			get
			{
				if (resizable)
				{
					return _resizable;
				}
				return false;
			}
			set
			{
				resizable = value;
			}
		}

		public new Size MinimumSize
		{
			get;
			set;
		}

		public new Size MaximumSize
		{
			get;
			set;
		}

		public bool AutoDispose
		{
			get;
			set;
		}

		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= NativeMethods.WS_EX_NOACTIVATE;
				return createParams;
			}
		}

		public event EventHandler PopupClosed;

		public Popup(Control content, bool autoDispose = false)
		{
			Popup popup = this;
			if (content == null)
			{
				throw new ArgumentNullException("content");
			}
			this.content = content;
			AutoDispose = autoDispose;
			host = new ToolStripControlHost(content);
			ShowingAnimation = PopupAnimations.SystemDefault;
			HidingAnimation = PopupAnimations.None;
			AnimationDuration = 100;
			_resizable = true;
			AutoSize = false;
			DoubleBuffered = true;
			base.ResizeRedraw = true;
			base.Padding = (base.Margin = (host.Padding = (host.Margin = Padding.Empty)));
			MinimumSize = content.MinimumSize;
			content.MinimumSize = content.Size;
			MaximumSize = content.MaximumSize;
			content.MaximumSize = content.Size;
			base.Size = content.Size;
			content.Location = Point.Empty;
			Items.Add(host);
			content.Disposed += delegate
			{
				content = null;
				popup.Dispose(disposing: true);
			};
			content.RegionChanged += delegate
			{
				UpdateRegion();
			};
			content.Paint += delegate(object sender, PaintEventArgs e)
			{
				PaintSizeGrip(e);
			};
			UpdateRegion();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && content != null)
			{
				content.Dispose();
				content = null;
			}
			base.Dispose(disposing);
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if ((base.Visible && ShowingAnimation == PopupAnimations.None) || (!base.Visible && HidingAnimation == PopupAnimations.None))
			{
				return;
			}
			NativeMethods.AnimationFlags animationFlags = ((!base.Visible) ? NativeMethods.AnimationFlags.Hide : NativeMethods.AnimationFlags.Roll);
			PopupAnimations popupAnimations = (base.Visible ? ShowingAnimation : HidingAnimation);
			if (popupAnimations == PopupAnimations.SystemDefault)
			{
				popupAnimations = (SystemInformation.IsMenuAnimationEnabled ? ((!SystemInformation.IsMenuFadeEnabled) ? (PopupAnimations.Slide | (base.Visible ? PopupAnimations.TopToBottom : PopupAnimations.BottomToTop)) : PopupAnimations.Blend) : PopupAnimations.None);
			}
			if ((popupAnimations & (PopupAnimations.Center | PopupAnimations.Slide | PopupAnimations.Blend | PopupAnimations.Roll)) == 0)
			{
				return;
			}
			if (resizableTop)
			{
				if ((popupAnimations & PopupAnimations.BottomToTop) != 0)
				{
					popupAnimations = (popupAnimations & ~PopupAnimations.BottomToTop) | PopupAnimations.TopToBottom;
				}
				else if ((popupAnimations & PopupAnimations.TopToBottom) != 0)
				{
					popupAnimations = (popupAnimations & ~PopupAnimations.TopToBottom) | PopupAnimations.BottomToTop;
				}
			}
			if (resizableLeft)
			{
				if ((popupAnimations & PopupAnimations.RightToLeft) != 0)
				{
					popupAnimations = (popupAnimations & ~PopupAnimations.RightToLeft) | PopupAnimations.LeftToRight;
				}
				else if ((popupAnimations & PopupAnimations.LeftToRight) != 0)
				{
					popupAnimations = (popupAnimations & ~PopupAnimations.LeftToRight) | PopupAnimations.RightToLeft;
				}
			}
			animationFlags = (NativeMethods.AnimationFlags)((int)animationFlags | (int)((PopupAnimations)1048575 & popupAnimations));
			NativeMethods.AnimateWindow(base.Handle, AnimationDuration, animationFlags);
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (acceptAlt && (keyData & Keys.Alt) == Keys.Alt)
			{
				if ((keyData & Keys.F4) != Keys.F4)
				{
					return false;
				}
				Close();
			}
			return base.ProcessDialogKey(keyData);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			if (content != null)
			{
				content.MinimumSize = base.Size;
				content.MaximumSize = base.Size;
				content.Size = base.Size;
				content.Location = Point.Empty;
			}
			base.OnSizeChanged(e);
		}

		protected override void OnOpening(CancelEventArgs e)
		{
			if (content.IsDisposed || content.Disposing)
			{
				e.Cancel = true;
				return;
			}
			UpdateRegion();
			base.OnOpening(e);
		}

		protected override void OnOpened(EventArgs e)
		{
			if (ownerPopup != null)
			{
				ownerPopup._resizable = false;
			}
			if (focusOnOpen)
			{
				content.Focus();
			}
			base.OnOpened(e);
		}

		protected override void OnClosed(ToolStripDropDownClosedEventArgs e)
		{
			if (ownerPopup != null)
			{
				ownerPopup._resizable = true;
			}
			base.OnClosed(e);
			if (content is INotifyClose)
			{
				((INotifyClose)content).PopupClosed();
			}
			if (this.PopupClosed != null)
			{
				this.PopupClosed(this, EventArgs.Empty);
			}
			this.BeginInvoke(Dispose);
		}

		protected void UpdateRegion()
		{
			if (base.Region != null)
			{
				base.Region.Dispose();
				base.Region = null;
			}
			if (content.Region != null)
			{
				base.Region = content.Region.Clone();
			}
		}

		protected void SetOwnerItem(Control control)
		{
			if (control != null)
			{
				Popup popup = control as Popup;
				if (popup != null)
				{
					ownerPopup = popup;
					ownerPopup.childPopup = this;
					base.OwnerItem = popup.Items[0];
				}
				else if (control.Parent != null)
				{
					SetOwnerItem(control.Parent);
				}
			}
		}

		public void Show(Control control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			SetOwnerItem(control);
			Show(control, control.ClientRectangle);
		}

		public void Show(Control control, Rectangle area)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			SetOwnerItem(control);
			resizableTop = (resizableLeft = false);
			Point p = control.PointToScreen(new Point(area.Left, area.Top + area.Height));
			Rectangle workingArea = Screen.FromControl(control).WorkingArea;
			if (p.X + base.Size.Width > workingArea.Left + workingArea.Width)
			{
				resizableLeft = true;
				p.X = workingArea.Left + workingArea.Width - base.Size.Width;
			}
			if (p.Y + base.Size.Height > workingArea.Top + workingArea.Height)
			{
				resizableTop = true;
				p.Y -= base.Size.Height + area.Height;
			}
			p = control.PointToClient(p);
			Show(control, p, ToolStripDropDownDirection.BelowRight);
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			if (!InternalProcessResizing(ref m, contentControl: false))
			{
				base.WndProc(ref m);
			}
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public bool ProcessResizing(ref Message m)
		{
			return InternalProcessResizing(ref m, contentControl: true);
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private bool InternalProcessResizing(ref Message m, bool contentControl)
		{
			if (m.Msg == NativeMethods.WM_NCACTIVATE && m.WParam != IntPtr.Zero && childPopup != null && childPopup.Visible)
			{
				childPopup.Hide();
			}
			if (!Resizable)
			{
				return false;
			}
			if (m.Msg == NativeMethods.WM_NCHITTEST)
			{
				return OnNcHitTest(ref m, contentControl);
			}
			if (m.Msg == NativeMethods.WM_GETMINMAXINFO)
			{
				return OnGetMinMaxInfo(ref m);
			}
			return false;
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private bool OnGetMinMaxInfo(ref Message m)
		{
			NativeMethods.MINMAXINFO mINMAXINFO = (NativeMethods.MINMAXINFO)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.MINMAXINFO));
			mINMAXINFO.maxTrackSize = MaximumSize;
			mINMAXINFO.minTrackSize = MinimumSize;
			Marshal.StructureToPtr((object)mINMAXINFO, m.LParam, fDeleteOld: false);
			return true;
		}

		private bool OnNcHitTest(ref Message m, bool contentControl)
		{
			int x = NativeMethods.LOWORD(m.LParam);
			int y = NativeMethods.HIWORD(m.LParam);
			Point pt = PointToClient(new Point(x, y));
			GripBounds gripBounds = new GripBounds(contentControl ? content.ClientRectangle : base.ClientRectangle);
			IntPtr intPtr = new IntPtr(NativeMethods.HTTRANSPARENT);
			if (resizableTop)
			{
				if (resizableLeft && gripBounds.TopLeft.Contains(pt))
				{
					m.Result = (contentControl ? intPtr : ((IntPtr)NativeMethods.HTTOPLEFT));
					return true;
				}
				if (!resizableLeft && gripBounds.TopRight.Contains(pt))
				{
					m.Result = (contentControl ? intPtr : ((IntPtr)NativeMethods.HTTOPRIGHT));
					return true;
				}
				if (gripBounds.Top.Contains(pt))
				{
					m.Result = (contentControl ? intPtr : ((IntPtr)NativeMethods.HTTOP));
					return true;
				}
			}
			else
			{
				if (resizableLeft && gripBounds.BottomLeft.Contains(pt))
				{
					m.Result = (contentControl ? intPtr : ((IntPtr)NativeMethods.HTBOTTOMLEFT));
					return true;
				}
				if (!resizableLeft && gripBounds.BottomRight.Contains(pt))
				{
					m.Result = (contentControl ? intPtr : ((IntPtr)NativeMethods.HTBOTTOMRIGHT));
					return true;
				}
				if (gripBounds.Bottom.Contains(pt))
				{
					m.Result = (contentControl ? intPtr : ((IntPtr)NativeMethods.HTBOTTOM));
					return true;
				}
			}
			if (resizableLeft && gripBounds.Left.Contains(pt))
			{
				m.Result = (contentControl ? intPtr : ((IntPtr)NativeMethods.HTLEFT));
				return true;
			}
			if (!resizableLeft && gripBounds.Right.Contains(pt))
			{
				m.Result = (contentControl ? intPtr : ((IntPtr)NativeMethods.HTRIGHT));
				return true;
			}
			return false;
		}

		public void PaintSizeGrip(PaintEventArgs e)
		{
			if (e == null || e.Graphics == null || !resizable)
			{
				return;
			}
			Size clientSize = content.ClientSize;
			using (Bitmap image = new Bitmap(16, 16))
			{
				using (Graphics graphics = Graphics.FromImage(image))
				{
					if (Application.RenderWithVisualStyles)
					{
						if (sizeGripRenderer == null)
						{
							sizeGripRenderer = new VisualStyleRenderer(VisualStyleElement.Status.Gripper.Normal);
						}
						sizeGripRenderer.DrawBackground(graphics, new Rectangle(0, 0, 16, 16));
					}
					else
					{
						ControlPaint.DrawSizeGrip(graphics, content.BackColor, 0, 0, 16, 16);
					}
				}
				using (e.Graphics.SaveState())
				{
					if (resizableTop)
					{
						if (resizableLeft)
						{
							e.Graphics.RotateTransform(180f);
							e.Graphics.TranslateTransform(-clientSize.Width, -clientSize.Height);
						}
						else
						{
							e.Graphics.ScaleTransform(1f, -1f);
							e.Graphics.TranslateTransform(0f, -clientSize.Height);
						}
					}
					else if (resizableLeft)
					{
						e.Graphics.ScaleTransform(-1f, 1f);
						e.Graphics.TranslateTransform(-clientSize.Width, 0f);
					}
					e.Graphics.DrawImage(image, clientSize.Width - 16, clientSize.Height - 16 + 1, 16, 16);
				}
			}
		}
	}
}
