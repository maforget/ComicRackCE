using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace cYo.Common.Drawing
{
	public static class SimpleTextRenderer
	{
		public static Rectangle DrawText(Graphics gr, IEnumerable<TextLine> textLines, Rectangle rect, int offset = 0)
		{
			return WorkText(gr, textLines, rect, onlyMeassure: false, offset);
		}

		public static Rectangle MeasureText(Graphics gr, IEnumerable<TextLine> textLines, Rectangle rect)
		{
			return WorkText(gr, textLines, rect, onlyMeassure: true, 0);
		}

		private static float MeasureFirstTab(Graphics gr, IEnumerable<TextLine> textLines)
		{
			float num = 0f;
			foreach (TextLine textLine in textLines)
			{
				if (!textLine.Separator && textLine.Format.Alignment != StringAlignment.Far)
				{
					string[] array = textLine.Text.Split('\t');
					if (array.Length > 1)
					{
						num = Math.Max(gr.MeasureString(array[0], textLine.Font).Width, num);
					}
				}
			}
			return num;
		}

		private static Rectangle WorkText(Graphics gr, IEnumerable<TextLine> textLines, Rectangle bounds, bool onlyMeassure, int scrollOffset)
		{
			if (gr == null || textLines == null)
				throw new ArgumentNullException();

			Rectangle rect = bounds;
			using (gr.SaveState())
			{
				Rectangle a = Rectangle.Empty;
				int dy = 0;
				float num = MeasureFirstTab(gr, textLines) + 8f;
				gr.PageUnit = GraphicsUnit.Pixel;
				foreach (TextLine textLine in textLines)
				{
					if (textLine.ScrollStart && !onlyMeassure && scrollOffset != 0)
					{
						gr.SetClip(rect, CombineMode.Intersect);
						rect = rect.Pad(0, scrollOffset);
					}
					if (textLine.Separator)
					{
						dy = textLine.BeforeSpacing;
					}
					else
					{
						if (dy != 0)
						{
							Space(ref rect, dy);
							dy = 0;
						}
						if (!string.IsNullOrEmpty(textLine.Text))
							Space(ref rect, textLine.BeforeSpacing);

						using (Brush br = new SolidBrush(textLine.ForeColor))
						{
							StringFormat format = textLine.Format;
							if (format.Alignment != StringAlignment.Far)
							{
								format.SetTabStops(0f, new float[1]
								{
									num
								});
							}
							Rectangle b = DrawString(gr, textLine.Text, textLine.Font, br, ref rect, format, onlyMeassure);
							if (rect.Height != 0)
								a = (a.IsEmpty ? b : Rectangle.Union(a, b));
							else
								break;
						}
						if (!string.IsNullOrEmpty(textLine.Text))
						{
							Space(ref rect, textLine.AfterSpacing);
							a.Height += textLine.AfterSpacing;
						}
					}
				}
				return a;
			}
		}

		private static void Space(ref Rectangle rect, int dy)
		{
			if (rect.Height > 0)
			{
				rect.Y += dy;
				rect.Height -= dy;
				if (rect.Height < 0)
				{
					rect.Height = 0;
				}
			}
		}

		private static Rectangle DrawString(Graphics gr, string text, Font f, Brush br, ref Rectangle rect, StringFormat sf, bool onlyMeassure)
		{
			if (string.IsNullOrEmpty(text) || rect.Height <= 0)
			{
				return Rectangle.Empty;
			}
			sf.FormatFlags |= StringFormatFlags.LineLimit;
			Size size = gr.MeasureString(text, f, rect.Size, sf).ToSize();
			if (size.Height > rect.Height)
			{
				size.Height = rect.Height;
			}
			if (!onlyMeassure)
			{
				gr.DrawString(text, f, br, rect, sf);
			}
			Rectangle rectangle = new Rectangle(0, 0, size.Width, size.Height);
			rectangle = rectangle.Align(rect, EnumExtensions.FromAlignments(sf.Alignment, sf.LineAlignment));
			Space(ref rect, size.Height);
			return rectangle;
		}
	}
}
