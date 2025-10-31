using cYo.Common.Drawing;
using cYo.Common.Windows.Forms.Theme;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public class TabControlEx : TabControl, ITheme
    {
		private Point downPoint;

		[DefaultValue(false)]
		public bool ReorderTabsWhileDragging
		{
			get;
			set;
		}

        public void ApplyTheme(Control? control = null)
        {
            ThemeExtensions.Theme(control ?? this);
        }

        protected override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver(e);
			DragTab(e, ReorderTabsWhileDragging);
		}

		private void DragTab(DragEventArgs e, bool reorder)
		{
			Point pt = PointToClient(new Point(e.X, e.Y));
			TabPage tabPageByTab = GetTabPageByTab(pt);
			if (tabPageByTab == null || !e.Data.GetDataPresent(typeof(TabPage)))
			{
				e.Effect = DragDropEffects.None;
				return;
			}
			e.Effect = DragDropEffects.Move;
			if (reorder)
			{
				TabPage tabPage = (TabPage)e.Data.GetData(typeof(TabPage));
				int num = base.TabPages.IndexOf(tabPage);
				int num2 = base.TabPages.IndexOf(tabPageByTab);
				if (num != num2)
				{
					base.TabPages.RemoveAt(num);
					base.TabPages.Insert(num2, tabPage);
					base.SelectedTab = tabPage;
				}
			}
		}

		protected override void OnDragDrop(DragEventArgs e)
		{
			base.OnDragDrop(e);
			DragTab(e, reorder: true);
			downPoint = Point.Empty;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			downPoint = e.Location;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (!downPoint.IsEmpty && downPoint.Distance(e.Location) >= 5)
			{
				TabPage tabPageByTab = GetTabPageByTab(e.Location);
				if (tabPageByTab != null)
				{
					DoDragDrop(tabPageByTab, DragDropEffects.Move);
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			downPoint = Point.Empty;
		}

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ApplyTheme();
        }

        private TabPage GetTabPageByTab(Point pt)
		{
			for (int i = 0; i < base.TabPages.Count; i++)
			{
				if (GetTabRect(i).Contains(pt))
				{
					return base.TabPages[i];
				}
			}
			return null;
		}
	}
}
