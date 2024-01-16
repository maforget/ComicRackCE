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
	public partial class ComicExplorerView : SubView, ISidebar
	{
		private ComicListBrowser comicListBrowser;

		private ComicBook[] comicInfoBooks = new ComicBook[0];

		private Size infoBrowserSize;

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
	}
}
