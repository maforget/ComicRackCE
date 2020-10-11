using System.Collections.Generic;
using System.Drawing;

namespace cYo.Common.Windows.Forms
{
	public class GroupHeaderInformation
	{
		public string Caption
		{
			get;
			set;
		}

		public Rectangle Bounds
		{
			get;
			set;
		}

		public bool Collapsed
		{
			get;
			set;
		}

		public List<IViewableItem> Items
		{
			get;
			private set;
		}

		public int ItemCount
		{
			get;
			set;
		}

		public Rectangle ArrowBounds
		{
			get;
			set;
		}

		public Rectangle TextBounds
		{
			get;
			set;
		}

		public Rectangle ExpandedColumnBounds
		{
			get;
			set;
		}

		public GroupHeaderInformation(string caption, List<IViewableItem> items, bool collapsed = false)
		{
			Caption = caption;
			Items = items;
			Collapsed = collapsed;
			Bounds = Rectangle.Empty;
			ItemCount = Items.Count;
		}
	}
}
