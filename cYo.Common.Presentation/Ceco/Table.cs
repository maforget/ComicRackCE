using System;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Presentation.Ceco
{
	public class Table : Block
	{
		public class Cell : FlowBlock
		{
			private int columnSpan = 1;

			private int rowSpan = 1;

			public int ColumSpan
			{
				get
				{
					return columnSpan;
				}
				set
				{
					columnSpan = value;
				}
			}

			public int RowSpan
			{
				get
				{
					return rowSpan;
				}
				set
				{
					rowSpan = value;
				}
			}

			public void RecalcVAlign()
			{
				if (base.Height != 0)
				{
					switch (VAlign)
					{
					case VerticalAlignment.Middle:
						OffsetInlines(new Point(0, (base.Height - base.ActualSize.Height) / 2));
						break;
					case VerticalAlignment.Bottom:
						OffsetInlines(new Point(0, base.Height - base.ActualSize.Height));
						break;
					case VerticalAlignment.None:
					case VerticalAlignment.Top:
						break;
					}
				}
			}
		}

		public class Row : Block
		{
			private HorizontalAlignment align;

			public override HorizontalAlignment Align
			{
				get
				{
					return align;
				}
				set
				{
					align = value;
				}
			}

			public Row()
			{
				VAlign = VerticalAlignment.Middle;
			}

			protected override void CoreMeasure(Graphics gr, int maxWidth, LayoutType tbl)
			{
			}
		}

		private const int MaxTableSize = 64;

		private int cellSpacing = 2;

		private int cellPadding = 1;

		public override FlowBreak FlowBreak
		{
			get
			{
				if (Align != 0 && Align != HorizontalAlignment.Center)
				{
					return FlowBreak.None;
				}
				return FlowBreak.BreakLine | FlowBreak.After;
			}
		}

		public int CellSpacing
		{
			get
			{
				return cellSpacing;
			}
			set
			{
				cellSpacing = value;
			}
		}

		public int CellPadding
		{
			get
			{
				return cellPadding;
			}
			set
			{
				cellPadding = value;
			}
		}

		protected override void CoreMeasure(Graphics gr, int maxWidth, LayoutType tbl)
		{
			int num = ((base.Border >= 0) ? base.Border : 0);
			int num2 = ((num > 0) ? 1 : 0);
			int num3 = base.BlockWidth.GetSize(maxWidth) - 2 * num;
			int num4 = 0;
			int num5 = 0;
			int num6 = cellPadding + num2;
			int[] array = new int[MaxTableSize];
			Cell[,] array2 = new Cell[MaxTableSize, MaxTableSize];
			foreach (Row inline in base.Inlines)
			{
				int i = 0;
				int num7 = 0;
				foreach (Cell inline2 in inline.Inlines)
				{
					for (; array[i] > 0; i++)
					{
					}
					array2[i, num4] = inline2;
					inline2.Measure(gr, num3);
					num7 = i;
					int num8 = 0;
					while (num8 < inline2.ColumSpan)
					{
						array[i] = inline2.RowSpan;
						num8++;
						i++;
					}
				}
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j] > 0)
					{
						array[j]--;
					}
				}
				num5 = Math.Max(num7 + 1, num5);
				num4++;
			}
			int[] array3 = new int[num5];
			int[] array4 = new int[num5];
			for (int k = 0; k < num4; k++)
			{
				for (int l = 0; l < num5; l++)
				{
					Cell cell2 = array2[l, k];
					if (cell2 != null && cell2.ColumSpan == 1)
					{
						int num9 = (cell2.BlockWidth.IsFixed ? (cell2.Width - 2 * num6) : cell2.Width);
						if (num9 < cell2.MinimumWidth)
						{
							num9 = cell2.MinimumWidth;
						}
						int val = (cell2.BlockWidth.IsFixed ? num9 : cell2.MinimumWidth);
						array3[l] = Math.Max(array3[l], num9);
						array4[l] = Math.Max(array4[l], val);
					}
				}
			}
			for (int m = 0; m < num4; m++)
			{
				for (int n = 0; n < num5; n++)
				{
					Cell cell3 = array2[n, m];
					if (cell3 != null && cell3.ColumSpan != 1)
					{
						int spanSize = GetSpanSize(array3, n, cell3.ColumSpan, cellSpacing, num6);
						if (cell3.Width > spanSize)
						{
							DistributeWidth(cell3.Width, n, cell3.ColumSpan, array3, array4, spanSize);
						}
					}
				}
			}
			int num10 = GetSpanSize(array3, 0, array3.Length, cellSpacing, num6) + 2 * cellSpacing;
			if (base.BlockWidth.IsFixed || num10 >= num3)
			{
				num10 = DistributeWidth(num3, 0, num5, array3, array4, num10);
			}
			int[] array5 = new int[num4];
			for (int num11 = 0; num11 < num4; num11++)
			{
				int num12 = num + cellSpacing;
				for (int num13 = 0; num13 < num5; num13++)
				{
					Cell cell4 = array2[num13, num11];
					if (cell4 != null)
					{
						int num14 = GetSpanSize(array3, num13, cell4.ColumSpan, cellSpacing, num6) - 2 * num6;
						cell4.X = num12 + num6;
						cell4.Measure(gr, num14);
						cell4.Width = num14;
						if (cell4.RowSpan == 1)
						{
							int num15 = ((cell4.BlockHeight != 0) ? (cell4.Height - 2 * num6) : cell4.Height);
							if (num15 < 0)
							{
								num15 = 0;
							}
							array5[num11] = Math.Max(array5[num11], num15);
						}
					}
					num12 += array3[num13] + (2 * num6 + cellSpacing);
				}
			}
			for (int num16 = 0; num16 < num5; num16++)
			{
				for (int num17 = 0; num17 < num4; num17++)
				{
					Cell cell5 = array2[num16, num17];
					if (cell5 != null && cell5.RowSpan != 1)
					{
						int num18 = GetSpanSize(array5, num17, cell5.RowSpan, cellSpacing, num6) - 2 * num6;
						if (cell5.Height > num18)
						{
							DistributeWidth(cell5.Height, num17, cell5.RowSpan, array5, array5, num18);
						}
					}
				}
			}
			int num19 = num + cellSpacing;
			for (int num20 = 0; num20 < num4; num20++)
			{
				num19 += num6;
				for (int num21 = 0; num21 < num5; num21++)
				{
					Cell cell6 = array2[num21, num20];
					if (cell6 != null)
					{
						cell6.Y = num19;
						cell6.Height = GetSpanSize(array5, num20, cell6.RowSpan, cellSpacing, num6) - 2 * num6;
						cell6.RecalcVAlign();
					}
				}
				num19 += array5[num20] + num6 + cellSpacing;
			}
			base.Bounds = new Rectangle(0, 0, num10 + 2 * num, num19 + num);
		}

		public override void Draw(Graphics gr, Point location)
		{
			base.Draw(gr, location);
			Rectangle bounds = base.Bounds;
			int num = ((base.Border >= 0) ? base.Border : 0);
			int num2 = ((num > 0) ? 1 : 0);
			bounds.Offset(location);
			if (!BackColor.IsEmpty && BackColor != Color.Transparent)
			{
				using (Brush brush = new SolidBrush(BackColor))
				{
					gr.FillRectangle(brush, bounds);
				}
			}
			if (num > 0)
			{
				ControlPaint.DrawBorder3D(gr, bounds, Border3DStyle.RaisedOuter);
			}
			foreach (Row inline in base.Inlines)
			{
				foreach (Cell inline2 in inline.Inlines)
				{
					Rectangle bounds2 = inline2.Bounds;
					bounds2.Inflate(cellPadding, cellPadding);
					bounds2.Offset(bounds.Location);
					if (!inline2.BackColor.IsEmpty && inline2.BackColor != Color.Transparent)
					{
						using (Brush brush2 = new SolidBrush(inline2.BackColor))
						{
							gr.FillRectangle(brush2, bounds2);
						}
					}
					if (num2 > 0)
					{
						bounds2.Inflate(num2, num2);
						ControlPaint.DrawBorder3D(gr, bounds2, Border3DStyle.SunkenInner);
					}
					inline2.Draw(gr, bounds.Location);
				}
			}
		}

		public static int GetSpanSize(int[] widths, int index, int length, int spacing, int padding)
		{
			int num = 0;
			int num2 = Math.Min(length, widths.Length - index);
			for (int i = index; i < index + num2; i++)
			{
				num += padding + widths[i] + padding;
			}
			return num + (num2 - 1) * spacing;
		}

		private static int DistributeWidth(int availableWidth, int start, int cols, int[] maxWidths, int[] minWidths, int totalWidth)
		{
			int num = availableWidth - totalWidth;
			cols = Math.Min(cols, maxWidths.Length - start);
			for (int i = start; i < start + cols; i++)
			{
				int num2 = start + cols - i;
				int num3 = num / num2;
				int num4 = Math.Max(minWidths[i], maxWidths[i] + num3);
				int num5 = num4 - maxWidths[i];
				num -= num5;
				maxWidths[i] = num4;
			}
			totalWidth = availableWidth - num;
			return totalWidth;
		}
	}
}
