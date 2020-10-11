using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.Win32;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Controls;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public class ComicExplorerView : SubView, ISidebar
	{
		private ComicListBrowser comicListBrowser;

		private ComicBook[] comicInfoBooks = new ComicBook[0];

		private Size infoBrowserSize;

		private IContainer components;

		private ComicBrowserControl comicBrowser;

		private Timer previewTimer;

		private SmallComicPreview smallComicPreview;

		private SizableContainer sidePanel;

		private Panel treePanel;

		private SizableContainer previewPane;

		private SizableContainer pluginContainer;

		private ComicPageContainerControl comicInfo;

		private Panel pluginPlaceholder;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Horizontal
		{
			get
			{
				return sidePanel.Dock == DockStyle.Left;
			}
			set
			{
				sidePanel.Dock = ((!value) ? DockStyle.Top : DockStyle.Left);
				previewPane.Dock = (value ? DockStyle.Bottom : DockStyle.Right);
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ComicListBrowser ComicListBrowser
		{
			get
			{
				return comicListBrowser;
			}
			set
			{
				if (comicListBrowser != value)
				{
					if (comicListBrowser != null)
					{
						comicListBrowser.BookListChanged -= browserControl_BookListChanged;
					}
					comicBrowser.BookList = null;
					treePanel.Controls.Remove(comicListBrowser);
					comicListBrowser = value;
					if (comicListBrowser != null)
					{
						treePanel.Controls.Add(comicListBrowser);
						comicListBrowser.Dock = DockStyle.Fill;
						comicListBrowser.BookListChanged += browserControl_BookListChanged;
						comicBrowser.BookList = comicListBrowser.BookList;
					}
				}
			}
		}

		public ComicBrowserControl ComicBrowser => comicBrowser;

		public int SplitterDistance
		{
			get
			{
				return sidePanel.ExpandedWidth;
			}
			set
			{
				sidePanel.ExpandedWidth = value;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ComicExplorerViewSettings ViewSettings
		{
			get
			{
				ComicExplorerViewSettings comicExplorerViewSettings = new ComicExplorerViewSettings();
				try
				{
					comicExplorerViewSettings.BrowserSplit = SplitterDistance;
					comicExplorerViewSettings.ShowBrowser = sidePanel.Expanded;
					comicExplorerViewSettings.PreviewSplit = previewPane.ExpandedWidth;
					comicExplorerViewSettings.ShowSearchBrowser = comicBrowser.SearchBrowserVisible;
					comicExplorerViewSettings.ShowPreview = ((ISidebar)this).Preview;
					comicExplorerViewSettings.ShowTopBrowser = ((ISidebar)this).TopBrowser;
					comicExplorerViewSettings.TopBrowserSplit = ((ISidebar)this).TopBrowserSplit;
					comicExplorerViewSettings.ItemViewConfig = comicBrowser.ViewConfig;
					if (comicExplorerViewSettings.ItemViewConfig != null)
					{
						comicExplorerViewSettings.ItemViewConfig.GroupsStatus = null;
					}
					comicExplorerViewSettings.TwoPagePreview = smallComicPreview.TwoPageDisplay;
					comicExplorerViewSettings.SearchBrowserColumn1 = comicBrowser.SearchBrowserColumn1;
					comicExplorerViewSettings.SearchBrowserColumn2 = comicBrowser.SearchBrowserColumn2;
					comicExplorerViewSettings.SearchBrowserColumn3 = comicBrowser.SearchBrowserColumn3;
					comicExplorerViewSettings.ShowInfo = ((ISidebar)this).Info;
					comicExplorerViewSettings.InfoBrowserSize = ((ISidebar)this).InfoBrowserSize;
					comicExplorerViewSettings.InfoBrowserRight = ((ISidebar)this).InfoBrowserRight;
					return comicExplorerViewSettings;
				}
				catch (Exception)
				{
					return comicExplorerViewSettings;
				}
			}
			set
			{
				if (value == null)
				{
					return;
				}
				bool enableAnimation = SizableContainer.EnableAnimation;
				SizableContainer.EnableAnimation = false;
				try
				{
					((ISidebar)this).Preview = value.ShowPreview;
					((ISidebar)this).TopBrowser = value.ShowTopBrowser;
					((ISidebar)this).TopBrowserSplit = value.TopBrowserSplit;
					((ISidebar)this).Info = value.ShowInfo;
					((ISidebar)this).InfoBrowserRight = value.InfoBrowserRight;
					((ISidebar)this).InfoBrowserSize = value.InfoBrowserSize;
					SplitterDistance = value.BrowserSplit;
					sidePanel.Expanded = value.ShowBrowser;
					previewPane.ExpandedWidth = value.PreviewSplit;
					if (value.ItemViewConfig != null)
					{
						comicBrowser.ViewConfig = value.ItemViewConfig;
					}
					smallComicPreview.TwoPageDisplay = value.TwoPagePreview;
					comicBrowser.SearchBrowserColumn1 = value.SearchBrowserColumn1;
					comicBrowser.SearchBrowserColumn2 = value.SearchBrowserColumn2;
					comicBrowser.SearchBrowserColumn3 = value.SearchBrowserColumn3;
					comicBrowser.SearchBrowserVisible = value.ShowSearchBrowser;
				}
				catch (Exception)
				{
				}
				finally
				{
					SizableContainer.EnableAnimation = enableAnimation;
				}
			}
		}

		bool ISidebar.Visible
		{
			get
			{
				return sidePanel.Expanded;
			}
			set
			{
				sidePanel.Expanded = value;
			}
		}

		bool ISidebar.Preview
		{
			get
			{
				return previewPane.Expanded;
			}
			set
			{
				previewPane.Expanded = value;
			}
		}

		bool ISidebar.TopBrowser
		{
			get
			{
				return comicListBrowser.TopBrowserVisible;
			}
			set
			{
				comicListBrowser.TopBrowserVisible = value;
			}
		}

		int ISidebar.TopBrowserSplit
		{
			get
			{
				return comicListBrowser.TopBrowserSplit;
			}
			set
			{
				comicListBrowser.TopBrowserSplit = value;
			}
		}

		bool ISidebar.Info
		{
			get
			{
				return pluginContainer.Expanded;
			}
			set
			{
				pluginContainer.Expanded = value;
			}
		}

		bool ISidebar.HasInfoPanels => comicInfo.Pages.Count() > 0;

		bool ISidebar.InfoBrowserRight
		{
			get
			{
				return pluginContainer.Dock == DockStyle.Right;
			}
			set
			{
				DockStyle dockStyle = (value ? DockStyle.Right : DockStyle.Bottom);
				if (pluginContainer.Dock != dockStyle)
				{
					pluginContainer.Dock = dockStyle;
					UpdatePreviewPadding();
					if (!infoBrowserSize.IsEmpty)
					{
						((ISidebar)this).InfoBrowserSize = infoBrowserSize;
					}
				}
			}
		}

		Size ISidebar.InfoBrowserSize
		{
			get
			{
				if (pluginContainer.Dock == DockStyle.Right)
				{
					return new Size(pluginContainer.ExpandedWidth, infoBrowserSize.Height);
				}
				return new Size(infoBrowserSize.Width, pluginContainer.ExpandedWidth);
			}
			set
			{
				infoBrowserSize = value;
				if (pluginContainer.Dock == DockStyle.Right)
				{
					pluginContainer.ExpandedWidth = value.Width;
				}
				else
				{
					pluginContainer.ExpandedWidth = value.Height;
				}
			}
		}

		public ComicExplorerView()
		{
			InitializeComponent();
			pluginContainer.Expanded = false;
			((ISidebar)this).Preview = false;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			ComicBrowser.ItemView.SelectedIndexChanged += ItemView_SelectedIndexChanged;
		}

		private void UpdatePreviewPadding()
		{
			previewPane.Padding = new Padding(0, 0, 0, (pluginContainer.IsVisibleSet() && pluginContainer.Dock == DockStyle.Bottom) ? 6 : 0);
			pluginContainer.Padding = new Padding(0, 0, 0, (pluginContainer.Dock == DockStyle.Bottom) ? 6 : 0);
			pluginPlaceholder.Visible = !pluginContainer.IsVisibleSet() || pluginContainer.Dock != DockStyle.Bottom;
		}

		private void browserControl_BookListChanged(object sender, EventArgs e)
		{
			comicBrowser.BookList = comicListBrowser.BookList;
		}

		private void ItemView_SelectedIndexChanged(object sender, EventArgs e)
		{
			RestartPreviewTimer();
		}

		private void RestartPreviewTimer()
		{
			if (base.IsHandleCreated && !this.BeginInvokeIfRequired(RestartPreviewTimer))
			{
				previewTimer.Stop();
				previewTimer.Start();
			}
		}

		private void previewTimer_Tick(object sender, EventArgs e)
		{
			previewTimer.Stop();
			UpdatePreview();
		}

		private void sidePanel_ExpandedChanged(object sender, EventArgs e)
		{
			UpdatePreview();
		}

		private void pluginContainer_ExpandedChanged(object sender, EventArgs e)
		{
			UpdatePreview();
		}

		private void UpdatePreview()
		{
			bool flag = ((ISidebar)this).Preview && ((ISidebar)this).Visible;
			bool info = ((ISidebar)this).Info;
			comicInfoBooks.ForEach(delegate(ComicBook cb)
			{
				cb.BookChanged -= previewBookChanged;
			});
			comicInfoBooks = new ComicBook[0];
			if (!flag && !info)
			{
				return;
			}
			IEnumerable<ComicBook> bookList = ComicBrowser.GetBookList(ComicBookFilterType.Selected);
			if (flag)
			{
				if (bookList.IsEmpty())
				{
					smallComicPreview.ShowPreview(null);
				}
				else
				{
					smallComicPreview.ShowPreview(new ComicBook(bookList.First()));
				}
			}
			if (info)
			{
				comicInfoBooks = bookList.ToArray();
				comicInfoBooks.ForEach(delegate(ComicBook cb)
				{
					cb.BookChanged += previewBookChanged;
				});
				comicInfo.ShowInfo(bookList);
			}
		}

		private void previewBookChanged(object sender, BookChangedEventArgs e)
		{
			RestartPreviewTimer();
		}

		private void smallComicPreview_CloseClicked(object sender, EventArgs e)
		{
			((ISidebar)this).Preview = false;
		}

		void ISidebar.AddInfo(ComicPageControl page)
		{
			comicInfo.Controls.Add(page);
			pluginContainer.Visible = true;
			UpdatePreviewPadding();
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			smallComicPreview = new cYo.Projects.ComicRack.Viewer.Views.SmallComicPreview();
			comicBrowser = new cYo.Projects.ComicRack.Viewer.Views.ComicBrowserControl();
			previewTimer = new System.Windows.Forms.Timer(components);
			sidePanel = new cYo.Common.Windows.Forms.SizableContainer();
			treePanel = new System.Windows.Forms.Panel();
			previewPane = new cYo.Common.Windows.Forms.SizableContainer();
			pluginContainer = new cYo.Common.Windows.Forms.SizableContainer();
			comicInfo = new cYo.Projects.ComicRack.Engine.Controls.ComicPageContainerControl();
			pluginPlaceholder = new System.Windows.Forms.Panel();
			sidePanel.SuspendLayout();
			previewPane.SuspendLayout();
			pluginContainer.SuspendLayout();
			SuspendLayout();
			smallComicPreview.Caption = "";
			smallComicPreview.CaptionMargin = new System.Windows.Forms.Padding(2);
			smallComicPreview.Dock = System.Windows.Forms.DockStyle.Fill;
			smallComicPreview.Location = new System.Drawing.Point(2, 8);
			smallComicPreview.Name = "smallComicPreview";
			smallComicPreview.Size = new System.Drawing.Size(242, 197);
			smallComicPreview.TabIndex = 0;
			smallComicPreview.TwoPageDisplay = false;
			smallComicPreview.CloseClicked += new System.EventHandler(smallComicPreview_CloseClicked);
			comicBrowser.Caption = "";
			comicBrowser.CaptionMargin = new System.Windows.Forms.Padding(2);
			comicBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			comicBrowser.Location = new System.Drawing.Point(252, 0);
			comicBrowser.Name = "comicBrowser";
			comicBrowser.Size = new System.Drawing.Size(448, 370);
			comicBrowser.TabIndex = 0;
			previewTimer.Interval = 500;
			previewTimer.Tick += new System.EventHandler(previewTimer_Tick);
			sidePanel.AutoGripPosition = true;
			sidePanel.Controls.Add(treePanel);
			sidePanel.Controls.Add(previewPane);
			sidePanel.Dock = System.Windows.Forms.DockStyle.Left;
			sidePanel.Grip = cYo.Common.Windows.Forms.SizableContainer.GripPosition.Right;
			sidePanel.Location = new System.Drawing.Point(0, 0);
			sidePanel.Name = "sidePanel";
			sidePanel.Size = new System.Drawing.Size(252, 538);
			sidePanel.TabIndex = 1;
			sidePanel.ExpandedChanged += new System.EventHandler(sidePanel_ExpandedChanged);
			treePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			treePanel.Location = new System.Drawing.Point(0, 0);
			treePanel.Name = "treePanel";
			treePanel.Size = new System.Drawing.Size(246, 331);
			treePanel.TabIndex = 0;
			previewPane.AutoGripPosition = true;
			previewPane.BorderStyle = cYo.Common.Windows.Forms.ExtendedBorderStyle.Flat;
			previewPane.Controls.Add(smallComicPreview);
			previewPane.Dock = System.Windows.Forms.DockStyle.Bottom;
			previewPane.Location = new System.Drawing.Point(0, 331);
			previewPane.Name = "previewPane";
			previewPane.Size = new System.Drawing.Size(246, 207);
			previewPane.TabIndex = 1;
			previewPane.Text = "sizableContainer1";
			previewPane.ExpandedChanged += new System.EventHandler(sidePanel_ExpandedChanged);
			pluginContainer.AutoGripPosition = true;
			pluginContainer.Controls.Add(comicInfo);
			pluginContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
			pluginContainer.Location = new System.Drawing.Point(252, 370);
			pluginContainer.Name = "pluginContainer";
			pluginContainer.Size = new System.Drawing.Size(448, 162);
			pluginContainer.TabIndex = 2;
			pluginContainer.Visible = false;
			pluginContainer.ExpandedChanged += new System.EventHandler(pluginContainer_ExpandedChanged);
			comicInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			comicInfo.Location = new System.Drawing.Point(0, 6);
			comicInfo.Name = "comicInfo";
			comicInfo.Size = new System.Drawing.Size(448, 156);
			comicInfo.TabIndex = 0;
			pluginPlaceholder.Dock = System.Windows.Forms.DockStyle.Bottom;
			pluginPlaceholder.Location = new System.Drawing.Point(252, 532);
			pluginPlaceholder.Name = "pluginPlaceholder";
			pluginPlaceholder.Size = new System.Drawing.Size(448, 6);
			pluginPlaceholder.TabIndex = 3;
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.Controls.Add(comicBrowser);
			base.Controls.Add(pluginContainer);
			base.Controls.Add(pluginPlaceholder);
			base.Controls.Add(sidePanel);
			base.Name = "ComicExplorerView";
			base.Size = new System.Drawing.Size(700, 538);
			sidePanel.ResumeLayout(false);
			previewPane.ResumeLayout(false);
			pluginContainer.ResumeLayout(false);
			ResumeLayout(false);
		}
	}
}
