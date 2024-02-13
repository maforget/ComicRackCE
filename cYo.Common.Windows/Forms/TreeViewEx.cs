using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public class TreeViewEx : TreeView
	{
		private static class Native
		{
			public enum TVS_EX
			{
				TVS_EX_DOUBLEBUFFER = 4
			}

			public enum LVM
			{
				TVM_FIRST = 4352,
				TVM_SETEXTENDEDSTYLE = 4396,
				TVM_GETEXTENDEDSTYLE = 4397
			}

			public const int WM_SCROLL = 276;

			public const int WM_VSCROLL = 277;

			private const int SB_LINEUP = 0;

			private const int SB_LINELEFT = 0;

			private const int SB_LINEDOWN = 1;

			private const int SB_LINERIGHT = 1;

			private const int SB_PAGEUP = 2;

			private const int SB_PAGELEFT = 2;

			private const int SB_PAGEDOWN = 3;

			private const int SB_PAGERIGTH = 3;

			private const int SB_PAGETOP = 6;

			private const int SB_LEFT = 6;

			private const int SB_PAGEBOTTOM = 7;

			private const int SB_RIGHT = 7;

			private const int SB_ENDSCROLL = 8;

			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			private static extern int SendMessage(IntPtr handle, int messg, int wparam, int lparam);

			public static void SetExStyles(IntPtr handle, TVS_EX exStyle)
			{
				TVS_EX tVS_EX = (TVS_EX)SendMessage(handle, (int)LVM.TVM_GETEXTENDEDSTYLE, 0, 0);
				tVS_EX |= exStyle;
				SendMessage(handle, (int)LVM.TVM_SETEXTENDEDSTYLE, 0, (int)tVS_EX);
			}

			[DllImport("user32.dll", CharSet = CharSet.Unicode)]
			private static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

			public static void ScrollLines(IWin32Window window, int lines)
			{
				int num = Math.Abs(lines);
				for (int i = 0; i < num; i++)
				{
					SendMessage(window.Handle, WM_VSCROLL, (lines >= 0) ? 1 : 0, 0);
				}
			}
		}

		private Timer scrollTimer;

		private int dragScrollRegion = 10;

		private int delta;

		[DefaultValue(10)]
		public int DragScrollRegion
		{
			get
			{
				return dragScrollRegion;
			}
			set
			{
				dragScrollRegion = value;
			}
		}

		public event ScrollEventHandler Scroll;

		protected override void Dispose(bool disposing)
		{
			if (disposing && scrollTimer != null)
			{
				scrollTimer.Dispose();
			}
			base.Dispose(disposing);
		}

		public static void EnableDoubleBuffer(TreeView tv)
		{
			Native.SetExStyles(tv.Handle, Native.TVS_EX.TVS_EX_DOUBLEBUFFER);
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			EnableDoubleBuffer(this);
		}

		private void scrollTimer_Tick(object sender, EventArgs e)
		{
			Native.ScrollLines(this, delta);
		}

		protected override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver(e);
			if (scrollTimer == null)
			{
				scrollTimer = new Timer
				{
					Interval = 150
				};
				scrollTimer.Tick += scrollTimer_Tick;
			}
			Point point = PointToClient(new Point(e.X, e.Y));
			if (point.Y < dragScrollRegion)
			{
				delta = -1;
				scrollTimer.Enabled = true;
			}
			else if (point.Y > base.Height - dragScrollRegion)
			{
				delta = 1;
				scrollTimer.Enabled = true;
			}
			else
			{
				scrollTimer.Enabled = false;
			}
		}

		protected override void OnDragLeave(EventArgs e)
		{
			base.OnDragLeave(e);
			if (scrollTimer != null)
			{
				scrollTimer.Enabled = false;
			}
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			if (m.Msg == Native.WM_VSCROLL)
			{
				ScrollEventType type = (ScrollEventType)Enum.Parse(typeof(ScrollEventType), (m.WParam.ToInt32() & 0xFFFF).ToString());
				OnScroll(new ScrollEventArgs(type, (int)(m.WParam.ToInt64() >> 16) & 0xFF));
			}
		}

		protected virtual void OnScroll(ScrollEventArgs sea)
		{
			if (this.Scroll != null)
			{
				this.Scroll(this, sea);
			}
		}
	}
}
