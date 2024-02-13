using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using cYo.Common.ComponentModel;

namespace cYo.Common.Presentation.Ceco
{
	public class TextRun : Span, IRender
	{
		private class TextRendererDC : DisposableObject, IDeviceContext, IDisposable
		{
			private class NativeMethods
			{
				[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
				public class XFORM
				{
					public float eM11;

					public float eM12;

					public float eM21;

					public float eM22;

					public float eDx;

					public float eDy;

					private XFORM()
					{
					}

					public XFORM(float[] elements)
					{
						eM11 = elements[0];
						eM12 = elements[1];
						eM21 = elements[2];
						eM22 = elements[3];
						eDx = elements[4];
						eDy = elements[5];
					}
				}

				public const int GM_ADVANCED = 2;

				[DllImport("Gdi32")]
				public static extern int SetGraphicsMode(HandleRef hdc, int mode);

				[DllImport("Gdi32")]
				public static extern bool SetWorldTransform(HandleRef hDC, XFORM xform);

				[DllImport("Gdi32")]
				public static extern int SelectClipRgn(HandleRef hDC, HandleRef hRgn);
			}

			private readonly Graphics graphics;

			private IntPtr dc;

			private TextRendererDC()
			{
			}

			public TextRendererDC(Graphics g)
			{
				graphics = g;
			}

			public IntPtr GetHdc()
			{
				NativeMethods.XFORM xform;
				using (Matrix matrix = graphics.Transform)
				{
					xform = new NativeMethods.XFORM(matrix.Elements);
				}
				IntPtr hrgn;
				using (Region region = graphics.Clip)
				{
					hrgn = region.GetHrgn(graphics);
				}
				dc = graphics.GetHdc();
				HandleRef hdc = new HandleRef(this, dc);
				HandleRef hRegion = new HandleRef(null, hrgn);
				SetTransform(hdc, xform);
				SetClip(hdc, hRegion);
				return dc;
			}

			public void ReleaseHdc()
			{
				if (dc != IntPtr.Zero)
				{
					graphics.ReleaseHdc();
					dc = IntPtr.Zero;
				}
			}

			protected override void Dispose(bool disposing)
			{
				ReleaseHdc();
			}

			private static void SetTransform(HandleRef hdc, NativeMethods.XFORM xform)
			{
				NativeMethods.SetGraphicsMode(hdc, NativeMethods.GM_ADVANCED);
				NativeMethods.SetWorldTransform(hdc, xform);
			}

			private static void SetClip(HandleRef hdc, HandleRef hRegion)
			{
				NativeMethods.SelectClipRgn(hdc, hRegion);
			}
		}

		private int lastMeasureWidth;

		private const TextFormatFlags formatFlags = TextFormatFlags.NoClipping | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform | TextFormatFlags.NoPadding;

		public static bool UseTextRenderer;

		public static TextRenderingHint TextRenderingHint;

		private static readonly StringFormat stringFormat;

		private string text;

		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
			}
		}

		public bool IsWhiteSpace
		{
			get
			{
				if (text != null)
				{
					return text.All((char c) => char.IsWhiteSpace(c));
				}
				return true;
			}
		}

		static TextRun()
		{
			UseTextRenderer = true;
			TextRenderingHint = TextRenderingHint.AntiAlias;
			stringFormat = new StringFormat(StringFormat.GenericTypographic);
			stringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
		}

		public TextRun()
		{
		}

		public TextRun(string text, bool wordBreak)
		{
			if (wordBreak)
			{
				base.Inlines.AddRange(GetWords(text));
			}
			else
			{
				this.text = text;
			}
		}

		public TextRun(string text)
			: this(text, wordBreak: true)
		{
		}

		public void Measure(Graphics gr, int maxWidth)
		{
			LayoutType pendingLayout = base.PendingLayout;
			base.PendingLayout = LayoutType.None;
			if ((pendingLayout != LayoutType.Full && !base.Size.IsEmpty) || (pendingLayout == LayoutType.Position && lastMeasureWidth == maxWidth))
			{
				return;
			}
			lastMeasureWidth = maxWidth;
			base.Size = Size.Empty;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (UseTextRenderer)
			{
				base.Size = TextRenderer.MeasureText(gr, text, Font, new Size(maxWidth, 1000), formatFlags);
			}
			else
			{
				TextRenderingHint textRenderingHint = gr.TextRenderingHint;
				gr.TextRenderingHint = TextRenderingHint;
				try
				{
					base.Size = gr.MeasureString(text, Font, maxWidth, stringFormat).ToSize();
				}
				finally
				{
					gr.TextRenderingHint = textRenderingHint;
				}
			}
			base.BaseLine = base.Size.Height - base.DescentHeight;
		}

		public void Draw(Graphics gr, Point location)
		{
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			Point location2 = base.Location;
			location2.Offset(location);
			if (UseTextRenderer)
			{
				TextRenderer.DrawText(gr, text, Font, location2, ForeColor, formatFlags);
				return;
			}
			TextRenderingHint textRenderingHint = gr.TextRenderingHint;
			gr.TextRenderingHint = TextRenderingHint;
			try
			{
				using (Brush brush = new SolidBrush(ForeColor))
				{
					gr.DrawString(text, Font, brush, location2, stringFormat);
				}
			}
			finally
			{
				gr.TextRenderingHint = textRenderingHint;
			}
		}

		public override string ToString()
		{
			return text;
		}

		public static IList<Inline> GetWords(string text)
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<Inline> list = new List<Inline>();
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (char.IsLetterOrDigit(c))
				{
					stringBuilder.Append(c);
					continue;
				}
				if (stringBuilder.Length > 0)
				{
					list.Add(new TextRun(stringBuilder.ToString(), wordBreak: false));
					stringBuilder.Length = 0;
				}
				list.Add(new TextRun(c.ToString(), wordBreak: false));
			}
			if (stringBuilder.Length > 0)
			{
				list.Add(new TextRun(stringBuilder.ToString(), wordBreak: false));
			}
			return list;
		}
	}
}
