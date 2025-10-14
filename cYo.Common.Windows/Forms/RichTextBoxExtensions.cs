using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using cYo.Common.ComponentModel;

namespace cYo.Common.Windows.Forms
{
	public static class RichTextBoxExtensions
	{
		private class PaintSupend : DisposableObject
		{
			private const int WM_USER = 1024;

			private const int WM_SETREDRAW = 11;

			private const int EM_GETEVENTMASK = 1083;

			private const int EM_SETEVENTMASK = 1093;

			private const int EM_GETSCROLLPOS = 1245;

			private const int EM_SETSCROLLPOS = 1246;

			private Point scrollPoint;

			private readonly IntPtr eventMask;

			private readonly int suspendIndex;

			private readonly int suspendLength;

			private RichTextBox rtb;

			[DllImport("user32.dll")]
			private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, ref Point lParam);

			[DllImport("user32.dll")]
			private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, IntPtr lParam);

			public PaintSupend(RichTextBox rtb)
			{
				this.rtb = rtb;
				suspendIndex = rtb.SelectionStart;
				suspendLength = rtb.SelectionLength;
				SendMessage(rtb.Handle, EM_GETSCROLLPOS, 0, ref scrollPoint);
				SendMessage(rtb.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
				eventMask = SendMessage(rtb.Handle, EM_GETEVENTMASK, 0, IntPtr.Zero);
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					rtb.Select(suspendIndex, suspendLength);
					SendMessage(rtb.Handle, EM_SETSCROLLPOS, 0, ref scrollPoint);
					SendMessage(rtb.Handle, EM_SETEVENTMASK, 0, eventMask);
					SendMessage(rtb.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
					rtb.Invalidate();
				}
				base.Dispose(disposing);
			}
		}

		public static void RegisterColorize(this RichTextBox rtb, IEnumerable<ValuePair<Color, Regex>> colors)
		{
			Action action = delegate
			{
				using (rtb.SuspendPainting())
				{
					rtb.SelectAll();
					rtb.SelectionColor = SystemColors.WindowText;
					foreach (ValuePair<Color, Regex> color in colors)
					{
						foreach (Match item in color.Value.Matches(rtb.Text))
						{
							rtb.Select(item.Index, item.Length);
							rtb.SelectionColor = color.Key;
						}
					}
				}
			};
			rtb.TextChanged += delegate
			{
				action();
			};
			action();
		}

		public static void RegisterColorize(this RichTextBox rtb, IEnumerable<ValuePair<Color, string>> colors)
		{
			rtb.RegisterColorize(colors.Select((ValuePair<Color, string> vp) => new ValuePair<Color, Regex>(vp.Key, new Regex(vp.Value, RegexOptions.IgnoreCase))));
		}

		public static void RegisterColorize(this RichTextBox rtb, Color color, string expression)
		{
			rtb.RegisterColorize(new ValuePair<Color, string>[1]
			{
				new ValuePair<Color, string>(color, expression)
			});
		}

		public static void RegisterColorize(this RichTextBox rtb, Color color, Regex expression)
		{
			rtb.RegisterColorize(new ValuePair<Color, Regex>[1]
			{
				new ValuePair<Color, Regex>(color, expression)
			});
		}

		public static IDisposable SuspendPainting(this RichTextBox rtb)
		{
			return new PaintSupend(rtb);
		}
	}
}
