using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace cYo.Common.Presentation.Ceco
{
	public class Span : Inline
	{
		private InlineCollection inlines;

		public InlineCollection Inlines
		{
			get
			{
				if (inlines == null)
				{
					inlines = new InlineCollection();
					inlines.Changed += inlines_Changed;
				}
				return inlines;
			}
		}

		public override bool IsNode => inlines != null;

		public Span()
		{
		}

		public Span(params Inline[] inlines)
		{
			Inlines.AddRange(inlines);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && inlines != null)
			{
				foreach (Inline inline in inlines)
				{
					inline.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		public IList<Inline> GetSubItems(bool includeOwn)
		{
			List<Inline> list = new List<Inline>();
			if (includeOwn)
			{
				list.Add(this);
			}
			AddItems(this, list);
			return list;
		}

		public void OffsetInlines(Point offset)
		{
			if (offset.IsEmpty)
			{
				return;
			}
			foreach (Inline subItem in GetSubItems(includeOwn: false))
			{
				subItem.X += offset.X;
				subItem.Y += offset.Y;
			}
		}

		public override Inline GetHitItem(Point location, Point hitPoint)
		{
			Rectangle bounds = base.Bounds;
			bounds.Offset(location);
			foreach (Inline subItem in GetSubItems(includeOwn: false))
			{
				Inline hitItem = subItem.GetHitItem(bounds.Location, hitPoint);
				if (hitItem != null)
				{
					return hitItem;
				}
			}
			return base.GetHitItem(location, hitPoint);
		}

		protected override void InvokeLayout(LayoutType type)
		{
			base.InvokeLayout(type);
			if (type != LayoutType.Full)
			{
				return;
			}
			foreach (Inline subItem in GetSubItems(includeOwn: false))
			{
				subItem.PendingLayout = LayoutType.Full;
			}
		}

		private void inlines_Changed(object sender, CollectionChangeEventArgs e)
		{
			Inline inline = (Inline)e.Element;
			switch (e.Action)
			{
			case CollectionChangeAction.Add:
				inline.ParentInline = this;
				break;
			case CollectionChangeAction.Remove:
				inline.ParentInline = null;
				break;
			}
			InvokeLayout(LayoutType.Full);
		}

		private static void AddItems(Span span, ICollection<Inline> items)
		{
			if (span == null || span.inlines == null)
			{
				return;
			}
			foreach (Inline inline in span.inlines)
			{
				items.Add(inline);
				if (!inline.IsBlock)
				{
					AddItems(inline as Span, items);
				}
			}
		}
	}
}
