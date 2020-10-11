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
	public class QuickOpenView : CaptionControl
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

		private IContainer components;

		private ItemView itemView;

		private Panel panelStatus;

		private Button btBrowser;

		private Button btOpen;

		private Button btOpenFile;

		private ComicPageContainerControl comicPageContainer;

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
				value = value.Clamp(96, 512);
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

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
				itemView.Items.Clear();
			}
			base.Dispose(disposing);
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

		private void InitializeComponent()
		{
			itemView = new cYo.Common.Windows.Forms.ItemView();
			panelStatus = new System.Windows.Forms.Panel();
			btOpenFile = new System.Windows.Forms.Button();
			btBrowser = new System.Windows.Forms.Button();
			btOpen = new System.Windows.Forms.Button();
			comicPageContainer = new cYo.Projects.ComicRack.Engine.Controls.ComicPageContainerControl();
			panelStatus.SuspendLayout();
			comicPageContainer.SuspendLayout();
			SuspendLayout();
			itemView.AutomaticHeaderMenu = false;
			itemView.AutomaticViewMenu = false;
			itemView.BackColor = System.Drawing.SystemColors.Window;
			itemView.Dock = System.Windows.Forms.DockStyle.Fill;
			itemView.EnableStick = false;
			itemView.GroupCollapsedImage = cYo.Projects.ComicRack.Viewer.Properties.Resources.ArrowRight;
			itemView.GroupDisplayEnabled = true;
			itemView.GroupExpandedImage = cYo.Projects.ComicRack.Viewer.Properties.Resources.ArrowDown;
			itemView.HorizontalItemAlignment = System.Windows.Forms.HorizontalAlignment.Center;
			itemView.LabelEdit = false;
			itemView.Location = new System.Drawing.Point(0, 0);
			itemView.Multiselect = false;
			itemView.Name = "itemView";
			itemView.SelectionMode = System.Windows.Forms.SelectionMode.One;
			itemView.ShowGroupCount = false;
			itemView.Size = new System.Drawing.Size(573, 383);
			itemView.SortColumn = null;
			itemView.SortColumns = new cYo.Common.Windows.Forms.IColumn[0];
			itemView.SortColumnsKey = "";
			itemView.TabIndex = 0;
			itemView.ItemActivate += new System.EventHandler(itemView_ItemActivate);
			itemView.SelectedIndexChanged += new System.EventHandler(itemView_SelectedIndexChanged);
			itemView.PostPaint += new System.Windows.Forms.PaintEventHandler(itemView_PostPaint);
			itemView.VisibleChanged += new System.EventHandler(itemView_VisibleChanged);
			panelStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			panelStatus.Controls.Add(btOpenFile);
			panelStatus.Controls.Add(btBrowser);
			panelStatus.Controls.Add(btOpen);
			panelStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
			panelStatus.Location = new System.Drawing.Point(0, 402);
			panelStatus.Name = "panelStatus";
			panelStatus.Size = new System.Drawing.Size(573, 37);
			panelStatus.TabIndex = 1;
			btOpenFile.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btOpenFile.Location = new System.Drawing.Point(3, 6);
			btOpenFile.Name = "btOpenFile";
			btOpenFile.Size = new System.Drawing.Size(90, 23);
			btOpenFile.TabIndex = 0;
			btOpenFile.Text = "Open File...";
			btOpenFile.UseVisualStyleBackColor = true;
			btOpenFile.Click += new System.EventHandler(btOpenFile_Click);
			btBrowser.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btBrowser.Location = new System.Drawing.Point(478, 6);
			btBrowser.Name = "btBrowser";
			btBrowser.Size = new System.Drawing.Size(90, 23);
			btBrowser.TabIndex = 2;
			btBrowser.Text = "Browser";
			btBrowser.UseVisualStyleBackColor = true;
			btBrowser.Click += new System.EventHandler(btBrowser_Click);
			btOpen.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btOpen.Location = new System.Drawing.Point(99, 6);
			btOpen.Name = "btOpen";
			btOpen.Size = new System.Drawing.Size(90, 23);
			btOpen.TabIndex = 1;
			btOpen.Text = "Open";
			btOpen.UseVisualStyleBackColor = true;
			btOpen.Click += new System.EventHandler(btOpen_Click);
			comicPageContainer.Controls.Add(itemView);
			comicPageContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			comicPageContainer.Location = new System.Drawing.Point(0, 19);
			comicPageContainer.Name = "comicPageContainer";
			comicPageContainer.Size = new System.Drawing.Size(573, 383);
			comicPageContainer.TabIndex = 2;
			comicPageContainer.Text = "Books";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.SystemColors.Window;
			base.Caption = "Quick Open";
			base.Controls.Add(comicPageContainer);
			base.Controls.Add(panelStatus);
			base.Name = "QuickOpenView";
			base.Size = new System.Drawing.Size(573, 439);
			panelStatus.ResumeLayout(false);
			comicPageContainer.ResumeLayout(false);
			comicPageContainer.PerformLayout();
			ResumeLayout(false);
		}
	}
}
