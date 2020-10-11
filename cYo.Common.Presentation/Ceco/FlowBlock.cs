using System;
using System.Collections.Generic;
using System.Drawing;

namespace cYo.Common.Presentation.Ceco
{
	public class FlowBlock : Block
	{
		private Size actualSize;

		public Size ActualSize
		{
			get
			{
				return actualSize;
			}
			set
			{
				if (!(actualSize == value))
				{
					actualSize = value;
					OnActualSizeChanged();
				}
			}
		}

		public event EventHandler ActualSizeChanged;

		protected override void OnFontChanged()
		{
			base.OnFontChanged();
			base.PendingLayout = LayoutType.Full;
		}

		protected override void OnAlignChanged()
		{
			base.OnAlignChanged();
			base.PendingLayout = LayoutType.Position;
		}

		protected override void OnVAlignChanged()
		{
			base.OnVAlignChanged();
			base.PendingLayout = LayoutType.Position;
		}

		protected override void CoreMeasure(Graphics gr, int maxWidth, LayoutType tbl)
		{
			int size = base.BlockWidth.GetSize(maxWidth);
			int blockHeight = base.BlockHeight;
			int minWidth;
			Size size4 = (ActualSize = (base.Size = Layout(GetSubItems(includeOwn: false), gr, size, out minWidth)));
			if (!base.BlockWidth.IsAuto && base.Width < size)
			{
				minWidth = (base.Width = size);
			}
			if (base.Height < blockHeight)
			{
				base.Height = blockHeight;
			}
			base.MinimumWidth = minWidth;
		}

		public override void Draw(Graphics gr, Point location)
		{
			base.Draw(gr, location);
			location.Offset(base.Location);
			foreach (Inline subItem in GetSubItems(includeOwn: false))
			{
				IRender render = subItem as IRender;
				if (subItem.Visible && render != null)
				{
					Rectangle bounds = subItem.Bounds;
					bounds.Offset(location);
					if (gr.IsVisible(bounds))
					{
						render.Draw(gr, location);
					}
				}
			}
		}

		protected virtual void OnActualSizeChanged()
		{
			if (this.ActualSizeChanged != null)
			{
				this.ActualSizeChanged(this, EventArgs.Empty);
			}
		}

		private static Size Layout(IEnumerable<Inline> inlines, Graphics gr, int maxWidth, out int minWidth)
		{
			return Layout(inlines.GetEnumerator(), gr, maxWidth, out minWidth);
		}

		private static Size Layout(IEnumerator<Inline> inlines, Graphics gr, int maxWidth, out int minWidth)
		{
			Point pt = Point.Empty;
			Rectangle a = Rectangle.Empty;
			List<Inline> list = new List<Inline>();
			List<Inline> list2 = new List<Inline>();
			int leftMargin = 0;
			int rightMargin = 0;
			int leftMarginDown = 0;
			int rightMarginDown = 0;
			Inline inline = (inlines.MoveNext() ? inlines.Current : null);
			minWidth = 0;
			while (inline != null)
			{
				while (inline != null)
				{
					IRender render = inlines.Current as IRender;
					if (render == null || !inline.IsBlock || (inline.Align != HorizontalAlignment.Left && inline.Align != HorizontalAlignment.Right))
					{
						break;
					}
					inline.Visible = true;
					render.Measure(gr, maxWidth);
					list.Add(inline);
					inline = (inlines.MoveNext() ? inlines.Current : null);
				}
				foreach (Inline item in list)
				{
					item.Y = pt.Y;
					if (item.Align == HorizontalAlignment.Left)
					{
						item.X = leftMargin;
						leftMargin += item.Width;
						leftMarginDown = Math.Max(leftMarginDown, item.Height);
					}
					else
					{
						item.X = maxWidth - rightMargin - item.Width;
						rightMargin += item.Width;
						rightMarginDown = Math.Max(rightMarginDown, item.Height);
					}
					a = (a.IsEmpty ? item.Bounds : Rectangle.Union(a, item.Bounds));
				}
				list.Clear();
				int width = maxWidth - leftMargin - rightMargin;
				Rectangle rectangle;
				while (inline != null)
				{
					IRender render2 = inline as IRender;
					if ((inline.FlowBreak & FlowBreak.Before) != 0)
					{
						rectangle = Break(inline, list2, ref pt, ref leftMargin, ref leftMarginDown, ref rightMargin, ref rightMarginDown, ref width);
						a = (a.IsEmpty ? rectangle : Rectangle.Union(a, rectangle));
					}
					if (!inline.IsNode)
					{
						if (render2 == null || (list2.Count == 0 && render2.IsWhiteSpace))
						{
							if (render2 != null)
							{
								inline.Visible = false;
							}
						}
						else
						{
							inline.Visible = true;
							render2.Measure(gr, maxWidth);
							if (pt.X + inline.Bounds.Width > width && (pt.X != 0 || inline.Bounds.Width < width || leftMargin != 0 || rightMargin != 0))
							{
								break;
							}
							inline.Location = pt;
							pt.X += inline.Bounds.Width;
							minWidth = Math.Max(minWidth, leftMargin + rightMargin + inline.Bounds.Width);
							list2.Add(inline);
						}
					}
					if ((inline.FlowBreak & FlowBreak.After) != 0)
					{
						break;
					}
					inline = (inlines.MoveNext() ? inlines.Current : null);
				}
				rectangle = Break(inline, list2, ref pt, ref leftMargin, ref leftMarginDown, ref rightMargin, ref rightMarginDown, ref width);
				a = (a.IsEmpty ? rectangle : Rectangle.Union(a, rectangle));
				if (inline != null && (inline.FlowBreak & FlowBreak.After) != 0)
				{
					inline = (inlines.MoveNext() ? inlines.Current : null);
				}
			}
			return a.Size;
		}

		private static Rectangle Break(Inline current, ICollection<Inline> lineItems, ref Point pt, ref int leftMargin, ref int leftMarginDown, ref int rightMargin, ref int rightMarginDown, ref int width)
		{
			Rectangle result = LayoutSpan(lineItems, width, leftMargin);
			lineItems.Clear();
			if (result.Height == 0 && current != null)
			{
				result.Height = current.Font.Height;
			}
			int num = result.Height;
			if (current != null)
			{
				switch (current.FlowBreak & FlowBreak.BreakMask)
				{
				default:
					num += current.FlowBreakOffset;
					break;
				case FlowBreak.BreakMarginLeft:
					num = Math.Max(leftMarginDown, num);
					break;
				case FlowBreak.BreakMarginRight:
					num = Math.Max(rightMarginDown, num);
					break;
				case FlowBreak.BreakMarginLeftRight:
					num = Math.Max(Math.Max(leftMarginDown, rightMarginDown), num);
					break;
				}
			}
			pt.X = 0;
			pt.Y += num;
			leftMarginDown -= num;
			rightMarginDown -= num;
			if (leftMarginDown <= 0)
			{
				leftMargin = (leftMarginDown = 0);
			}
			if (rightMarginDown <= 0)
			{
				rightMargin = (rightMarginDown = 0);
			}
			return result;
		}

		private static Rectangle LayoutSpan(IEnumerable<Inline> span, int width, int leftMargin)
		{
			int num = 0;
			int num2 = 0;
			HorizontalAlignment horizontalAlignment = HorizontalAlignment.None;
			Rectangle a = Rectangle.Empty;
			foreach (Inline item in span)
			{
				num2 = Math.Max(item.BaseLine, num2);
				num = Math.Max(item.Height, num);
				horizontalAlignment = item.Align;
				a = (a.IsEmpty ? item.Bounds : Rectangle.Union(a, item.Bounds));
			}
			int num3;
			switch (horizontalAlignment)
			{
			case HorizontalAlignment.Center:
				num3 = (width - a.Width) / 2;
				break;
			case HorizontalAlignment.Right:
				num3 = width - a.Width;
				break;
			default:
				num3 = leftMargin;
				break;
			}
			a = Rectangle.Empty;
			foreach (Inline item2 in span)
			{
				switch (item2.BaseAlign)
				{
				case BaseAlignment.Top:
					item2.Y += -item2.DescentHeight;
					break;
				case BaseAlignment.Bottom:
					item2.Y += num - item2.Height + item2.DescentHeight;
					break;
				case BaseAlignment.Center:
					item2.Y += (num - item2.Height) / 2;
					break;
				default:
					item2.Y += num2 - item2.BaseLine;
					break;
				}
				item2.X += num3;
				a = (a.IsEmpty ? item2.Bounds : Rectangle.Union(a, item2.Bounds));
			}
			return a;
		}
	}
}
