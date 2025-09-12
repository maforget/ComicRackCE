using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Controls;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Viewer.Controls;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public partial class QuickOpenView : CaptionControl
	{
		private class CoverItemCustomGroupGrouper : IGrouper<IViewableItem>
		{
			public bool IsMultiGroup => false;

			public IGroupInfo GetGroup(IViewableItem item)
			{
				return ((CoverViewItem)item).CustomGroup;
			}

			public IEnumerable<IGroupInfo> GetGroups(IViewableItem item)
			{
				throw new NotImplementedException();
			}
		}

		private class ComicBookOpenedSorter : IComparer<ComicBook>
		{
			public int Compare(ComicBook cx, ComicBook cy)
			{
				int num = cy.OpenedTime.CompareTo(cx.OpenedTime);
				if (num != 0)
				{
					return num;
				}
				return cy.AddedTime.CompareTo(cx.AddedTime);
			}
		}

		private readonly ThumbnailConfig tc = new ThumbnailConfig
		{
			HideCaptions = true
		};

		public ComicBook SelectedBook => (itemView.SelectedItems.FirstOrDefault() as CoverViewItem)?.Comic;

		public bool ShowBrowserCommand
		{
			get
			{
				return btBrowser.Visible;
			}
			set
			{
				btBrowser.Visible = value;
			}
		}

		public int ThumbnailSize
		{
			get
			{
				return itemView.ItemThumbSize.Height;
			}
			set
			{
				value = value.Clamp(Program.MinThumbHeight, Program.MaxThumbHeight);
				itemView.ItemThumbSize = new Size(value, value);
			}
		}

		public event EventHandler BookActivated;
		public event EventHandler ShowBrowser;
		public event EventHandler OpenFile;

		public QuickOpenView()
		{
			InitializeComponent();
			itemView.ItemGrouper = new CoverItemCustomGroupGrouper();
			itemView.MouseWheel += itemView_MouseWheel;
			LocalizeUtility.Localize(this, components);
		}

		public void BeginUpdate()
		{
			itemView.Items.Clear();
			btOpen.Enabled = false;
		}

		public void AddGroup(IGroupInfo group, IEnumerable<ComicBook> books, int maxCount)
		{
			HashSet<Guid> h = new HashSet<Guid>(from item in itemView.Items.OfType<CoverViewItem>()
				select item.Comic.Id);
			int i = itemView.Items.Count;
			foreach (CoverViewItem item in from cb in (from cb in books.OrderBy((ComicBook cb) => cb, new ComicBookOpenedSorter())
					where cb.IsLinked
					where !h.Contains(cb.Id)
					select cb).Take(maxCount)
				select CoverViewItem.Create(cb, ++i, null))
			{
				item.CustomGroup = group;
				item.ThumbnailConfig = tc;
				itemView.Items.Add(item);
			}
		}

		public void EndUpdate()
		{
			comicPageContainer.ShowInfo((from cvi in itemView.Items.OfType<CoverViewItem>()
				select cvi.Comic).ToArray());
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			try
			{
				itemView.Text = TR.Messages["Books"];
				ScriptUtility.CreateQuickOpenPages().ForEach(delegate(ComicPageControl p)
				{
					p.MarkAsDirty();
					p.Visible = false;
					comicPageContainer.Controls.Add(p);
				});
			}
			catch
			{
			}
		}

		protected virtual void OnItemActivate()
		{
			if (this.BookActivated != null)
			{
				this.BookActivated(this, EventArgs.Empty);
			}
		}

		protected virtual void OnShowBrowser()
		{
			if (this.ShowBrowser != null)
			{
				this.ShowBrowser(this, EventArgs.Empty);
			}
		}

		protected virtual void OnOpenFile()
		{
			if (this.OpenFile != null)
			{
				this.OpenFile(this, EventArgs.Empty);
			}
		}

		private void itemView_SelectedIndexChanged(object sender, EventArgs e)
		{
			btOpen.Enabled = SelectedBook != null;
		}

		private void itemView_ItemActivate(object sender, EventArgs e)
		{
			OnItemActivate();
		}

		private void btOpen_Click(object sender, EventArgs e)
		{
			OnItemActivate();
		}

		private void btBrowser_Click(object sender, EventArgs e)
		{
			OnShowBrowser();
		}

		private void btOpenFile_Click(object sender, EventArgs e)
		{
			OnOpenFile();
		}

		private void itemView_MouseWheel(object sender, MouseEventArgs e)
		{
			if (Control.ModifierKeys.HasFlag(Keys.Control))
			{
				ThumbnailSize += e.Delta / SystemInformation.MouseWheelScrollDelta * 16;
			}
		}

		private void itemView_PostPaint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawShadow(itemView.DisplayRectangle, 8, Color.Black, 0.125f, BlurShadowType.Inside, BlurShadowParts.Edges);
		}

		private void itemView_VisibleChanged(object sender, EventArgs e)
		{
			btOpen.Visible = itemView.Visible;
		}
	}
}
