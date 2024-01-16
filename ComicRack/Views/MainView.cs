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
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Controls;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.Display.Forms;
using cYo.Projects.ComicRack.Engine.IO.Network;
using cYo.Projects.ComicRack.Viewer.Config;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public partial class MainView : SubView, IDisplayWorkspace, IListDisplays
	{
		[Flags]
		public enum AddRemoteLibraryOptions
		{
			None = 0x0,
			Open = 0x1,
			Select = 0x2,
			Auto = 0x4
		}

		private ComicExplorerView dbView;

		private ComicExplorerView fileView;

		private ComicPagesView pagesView;

		private readonly TabBar.TabBarItem tsbLibrary = new TabBar.TabBarItem("Library")
		{
			Name = "tsbLibrary",
			Image = Resources.Library,
			Padding = new Padding(8, 0, 0, 0),
			AdjustWidth = false
		};

		private readonly TabBar.TabBarItem tsbFolders = new TabBar.TabBarItem("Folders")
		{
			Name = "tsbFolders",
			Image = Resources.FileBrowser,
			Padding = new Padding(0, 0, 8, 0),
			AdjustWidth = false
		};

		private readonly TabBar.TabBarItem tsbPages = new TabBar.TabBarItem("Pages")
		{
			Name = "tsbPages",
			Image = Resources.ComicPage,
			Padding = new Padding(0, 0, 8, 0),
			AdjustWidth = false
		};

		private readonly CommandMapper commands = new CommandMapper();

		private readonly List<ComicBrowserForm> openBrowsers = new List<ComicBrowserForm>();

		private TabBar.TabBarItem lastBrowser;

		private Control comicViewer;

		private readonly HashSet<string> connectedMachines = new HashSet<string>();

		private readonly VisibilityAnimator tabStripVisibility;

		public bool IsComicViewer
		{
			get
			{
				if (comicViewer != null)
				{
					return comicViewer.Visible;
				}
				return false;
			}
		}

		private DockStyle ViewDock
		{
			get
			{
				return base.Parent.Dock;
			}
			set
			{
				base.Parent.Dock = value;
			}
		}

		public TabBar TabBar => tabStrip;

		public bool TabBarVisible
		{
			get
			{
				return tabStripVisibility.Visible;
			}
			set
			{
				tabStripVisibility.Visible = value;
			}
		}

		public bool IsComicVisible
		{
			get
			{
				if (tabStrip.SelectedTab != null)
				{
					return tabStrip.SelectedTab.Tag is int;
				}
				return false;
			}
		}

		public bool InfoPanelRight
		{
			get
			{
				return this.FindActiveService<ISidebar>()?.InfoBrowserRight ?? false;
			}
			set
			{
				ISidebar sidebar = this.FindActiveService<ISidebar>();
				if (sidebar != null)
				{
					sidebar.InfoBrowserRight = value;
				}
			}
		}

		public event EventHandler TabChanged;

		public event EventHandler ViewAdded;

		public event EventHandler ViewRemoved;

		public MainView()
		{
			components = new Container();
			InitializeComponent();
			tabStrip.Items.AddRange(new TabBar.TabBarItem[3]
			{
				tsbLibrary,
				tsbFolders,
				tsbPages
			});
			tabStripVisibility = new VisibilityAnimator(components, tabStrip);
			tsbLibrary.CaptionClick += tab_CaptionClick;
			tsbFolders.CaptionClick += tab_CaptionClick;
			tsbPages.CaptionClick += tab_CaptionClick;
			LocalizeUtility.Localize(this, components);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (base.DesignMode)
			{
				return;
			}
			if (base.ParentForm != null)
			{
				base.ParentForm.Resize += ParentForm_Resize;
			}
			FormUtility.EnableRightClickSplitButtons(tabToolStrip.Items);
			SuspendLayout();
			try
			{
				dbView = AddExplorerView(Program.Database, new ComicListLibraryBrowser(Program.Database), tsbLibrary);
				if (Program.ExtendedSettings.DisableFoldersView)
				{
					tabStrip.Items.Remove(tsbFolders);
				}
				else
				{
					fileView = AddExplorerView(null, new ComicListFolderFilesBrowser(Program.Settings.FavoriteFolders), tsbFolders);
				}
				pagesView = new ComicPagesView();
				AddView(pagesView, tsbPages);
				dbView.ComicBrowser.ItemView.UpdateItems();
			}
			finally
			{
				ResumeLayout();
			}
			commands.Add(SwitchDocking, tsbAlignment);
			commands.Add(delegate
			{
				ViewDock = DockStyle.Left;
			}, true, () => ViewDock == DockStyle.Left, tsbAlignLeft);
			commands.Add(delegate
			{
				ViewDock = DockStyle.Right;
			}, true, () => ViewDock == DockStyle.Right, tsbAlignRight);
			commands.Add(delegate
			{
				ViewDock = DockStyle.Bottom;
			}, true, () => ViewDock == DockStyle.Bottom, tsbAlignBottom);
			commands.Add(delegate
			{
				ViewDock = DockStyle.Fill;
			}, true, () => ViewDock == DockStyle.Fill, tsbAlignFill);
			commands.Add(delegate
			{
				InfoPanelRight = !InfoPanelRight;
			}, true, () => InfoPanelRight, tsbInfoPanelLeft);
			ShowView(Program.Settings.SelectedBrowser);
		}

		public void ShowLibrary(ComicLibrary library = null)
		{
			if (library == null)
			{
				ShowView(tsbLibrary);
				return;
			}
			TabBar.TabBarItem tabBarItem = tabStrip.Items.Where((TabBar.TabBarItem ti) => ti.Tag is ComicExplorerView && (ti.Tag as ComicExplorerView).ComicBrowser.Library == library).FirstOrDefault();
			ShowView(tabBarItem ?? tsbLibrary);
		}

		public void ShowFolders()
		{
			ShowView(tsbFolders);
		}

		public void ShowPages()
		{
			ShowView(tsbPages);
		}

		public void ShowLast()
		{
			if (lastBrowser != null)
			{
				ShowView(lastBrowser);
			}
			else
			{
				ShowLibrary();
			}
		}

		public void ShowView(int n)
		{
			OnGuiVisibility();
			if (n == -1)
			{
				ShowLast();
				return;
			}
			tabStrip.Items.Where((TabBar.TabBarItem btn) => btn.Visible && object.Equals(n, btn.Tag)).ForFirst(delegate(TabBar.TabBarItem btn)
			{
				ShowView(btn);
			});
		}

		public void ClearFileTabs()
		{
			for (int num = tabStrip.Items.Count - 1; num >= 0; num--)
			{
				TabBar.TabBarItem tabBarItem = tabStrip.Items[num];
				if (tabBarItem.Tag is int)
				{
					tabStrip.Items.RemoveAt(num);
				}
			}
		}

		public void RefreshFileTabs()
		{
			tabStrip.Refresh();
		}

		public IEnumerable<TabBar.TabBarItem> GetFileTabs(bool withPlus = false)
		{
			return tabStrip.Items.Where((TabBar.TabBarItem tbi) => tbi.Tag is int && ((int)tbi.Tag >= 0 || withPlus)).ToArray();
		}

		public void AddFileTab(TabBar.TabBarItem tsb)
		{
			tabStrip.Items.Add(tsb);
		}

		public void SetComicViewer(Control c)
		{
			if (c == null)
			{
				base.Controls.Remove(comicViewer);
				comicViewer = null;
				ShowLibrary();
			}
			else
			{
				comicViewer = c;
				base.Controls.Add(c);
				c.BringToFront();
				c.Dock = DockStyle.Fill;
				c.SetBounds(0, 0, c.Parent.Width, c.Parent.Height);
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (comicViewer != null && !comicViewer.Visible)
			{
				comicViewer.SetBounds(0, 0, comicViewer.Parent.Width, comicViewer.Parent.Height);
			}
		}

		public bool IsRemoteConnected(string address)
		{
			using (ItemMonitor.Lock(connectedMachines))
			{
				return connectedMachines.Contains(address);
			}
		}

		public IEnumerable<ComicLibrary> GetLibraries(bool includeRemote = true, bool onlyLocalRemote = false)
		{
			return from cev in tabStrip.Items.Select((TabBar.TabBarItem tb) => tb.Tag).OfType<ComicExplorerView>()
				select new
				{
					Client = (cev.Tag as ComicLibraryClient),
					Library = cev.ComicBrowser.Library
				} into cev
				where cev.Client == null || includeRemote
				where cev.Client == null || !onlyLocalRemote || cev.Client.ShareInformation.IsLocal
				select cev.Library;
		}

		public void AddRemoteLibrary(ComicLibraryClient client, AddRemoteLibraryOptions options)
		{
			if (!Program.ExtendedSettings.OwnRemoteConnect && (client == null || Program.NetworkManager.RunningServers.Any((ComicLibraryServer s) => s.Id == client.ShareInformation.Id)))
			{
				return;
			}
			SuspendLayout();
			RemoveRemoteLibrary(client.ShareInformation.Uri);
			try
			{
				RemoteConnectionView remoteConnectionView = new RemoteConnectionView(this, client, options);
				TabBar.TabBarItem tabBarItem = new TabBar.TabBarItem(client.ShareInformation.Name)
				{
					Tag = remoteConnectionView,
					Image = (client.ShareInformation.IsProtected ? Resources.RemoteDatabaseLocked : Resources.RemoteDatabase),
					CanClose = true,
					ToolTipText = client.ShareInformation.Comment
				};
				tabBarItem.CloseClick += delegate
				{
					RemoveRemoteLibrary(client.ShareInformation.Uri);
				};
				tabBarItem.CaptionClick += tab_CaptionClick;
				remoteConnectionView.Main = base.Main;
				remoteConnectionView.Tag = tabBarItem;
				tabStrip.Items.Insert(1, tabBarItem);
				AddView(remoteConnectionView, tabBarItem);
				using (ItemMonitor.Lock(connectedMachines))
				{
					connectedMachines.Add(client.ShareInformation.Uri);
				}
				if (options.HasFlag(AddRemoteLibraryOptions.Select))
				{
					ShowView(tabBarItem);
				}
			}
			finally
			{
				ResumeLayout();
			}
		}

		public void OnRefreshRemoteLists(object sender, EventArgs e)
		{
			ComicListLibraryBrowser comicListLibraryBrowser = sender as ComicListLibraryBrowser;
			if (comicListLibraryBrowser == null)
			{
				return;
			}
			ComicLibraryClient clc = comicListLibraryBrowser.Tag as ComicLibraryClient;
			if (clc != null)
			{
				ComicLibrary cl = null;
				AutomaticProgressDialog.Process(this, TR.Messages["RefreshServer", "Refreshing Server Library"], TR.Messages["GetServerLibraryText", "Retrieving the shared Library from the Server"], 1000, delegate
				{
					cl = clc.GetRemoteLibrary();
				}, AutomaticProgressDialogOptions.EnableCancel);
				if (cl != null)
				{
					comicListLibraryBrowser.Library = cl;
				}
			}
		}

		public void RemoveRemoteLibrary(string address)
		{
			foreach (TabBar.TabBarItem item in tabStrip.Items)
			{
				if (item.Tag is RemoteConnectionView)
				{
					RemoteConnectionView remoteConnectionView = item.Tag as RemoteConnectionView;
					ComicLibraryClient client = remoteConnectionView.Client;
					if (string.Equals(client.ShareInformation.Uri, address))
					{
						RemoveView(remoteConnectionView, item);
						ShowView(tsbLibrary);
						using (ItemMonitor.Lock(connectedMachines))
						{
							connectedMachines.Remove(address);
						}
						break;
					}
					continue;
				}
				ComicExplorerView comicExplorerView = item.Tag as ComicExplorerView;
				if (comicExplorerView != null && object.Equals(address, comicExplorerView.Tag))
				{
					Program.Settings.UpdateExplorerViewSetting(comicExplorerView.ComicBrowser.Library.Id, comicExplorerView.ViewSettings);
					tabStrip.Items.Remove(item);
					base.Controls.Remove(comicExplorerView);
					comicExplorerView.ComicBrowser.Library.ComicLists.GetItems<ShareableComicListItem>().ForEach(delegate(ShareableComicListItem l)
					{
						RemoveList(l, null);
					});
					if (comicExplorerView.ComicBrowser.Library != null)
					{
						comicExplorerView.ComicBrowser.Library.Dispose();
					}
					comicExplorerView.Dispose();
					using (ItemMonitor.Lock(connectedMachines))
					{
						connectedMachines.Remove(address);
					}
					ShowView(tsbLibrary);
					break;
				}
			}
		}

		protected override void OnIdle()
		{
			OnGuiVisibility();
		}

		protected virtual void OnGuiVisibility()
		{
			if (base.Main == null)
			{
				return;
			}
			foreach (TabBar.TabBarItem fileTab in GetFileTabs(withPlus: true))
			{
				fileTab.Visible = base.Main.BrowserDock == DockStyle.Fill && !base.Main.ReaderUndocked;
				int num = (int)fileTab.Tag;
				fileTab.FontBold = num >= 0 && num == base.Main.OpenBooks.CurrentSlot;
			}
			bool flag = base.Main.OpenBooks.CurrentBook != null;
			if (!flag && tabStrip.SelectedTab == tsbPages)
			{
				tabStrip.SelectedTab = tabStrip.Items[0];
			}
			tsbPages.Visible = flag;
			tsbAlignment.Visible = !base.Main.ReaderUndocked;
			switch (ViewDock)
			{
			case DockStyle.Bottom:
				tsbAlignment.Image = tsbAlignBottom.Image;
				break;
			case DockStyle.Fill:
				tsbAlignment.Image = tsbAlignFill.Image;
				break;
			case DockStyle.Left:
				tsbAlignment.Image = tsbAlignLeft.Image;
				break;
			case DockStyle.Right:
				tsbAlignment.Image = tsbAlignRight.Image;
				break;
			}
		}

		private void tabStrip_SelectedTabChanged(object sender, TabBar.SelectedTabChangedEventArgs e)
		{
			if (!e.Cancel)
			{
				ShowView(e.NewItem);
			}
			OnTabChanged();
		}

		private void ComicBrowserForm_Disposed(object sender, EventArgs e)
		{
			ComicBrowserForm item = sender as ComicBrowserForm;
			openBrowsers.Remove(item);
		}

		private void ComicBrowserForm_Resize(object sender, EventArgs e)
		{
			ComicBrowserForm comicBrowserForm = sender as ComicBrowserForm;
			if (comicBrowserForm.WindowState != FormWindowState.Maximized)
			{
				base.ParentForm.WindowState = comicBrowserForm.WindowState;
			}
			comicBrowserForm.Visible = comicBrowserForm.WindowState != FormWindowState.Minimized;
		}

		private void ParentForm_Resize(object sender, EventArgs e)
		{
			Form form = (Form)sender;
			if (form.WindowState == FormWindowState.Maximized)
			{
				return;
			}
			foreach (Form item in openBrowsers.OfType<Form>())
			{
				if (form.WindowState != FormWindowState.Minimized)
				{
					item.Visible = true;
				}
				item.WindowState = form.WindowState;
			}
		}

		private void tab_CaptionClick(object sender, CancelEventArgs e)
		{
			TabBar.TabBarItem tabBarItem = sender as TabBar.TabBarItem;
			IMain main = base.Main;
			if (base.Main != null && tabBarItem.IsSelected)
			{
				e.Cancel = true;
				base.Main.ToggleBrowser();
			}
		}

		protected virtual void OnTabChanged()
		{
			if (this.TabChanged != null)
			{
				this.TabChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnViewAdded()
		{
			if (this.ViewAdded != null)
			{
				this.ViewAdded(this, EventArgs.Empty);
			}
		}

		protected virtual void OnViewRemoved()
		{
			if (this.ViewRemoved != null)
			{
				this.ViewRemoved(this, EventArgs.Empty);
			}
		}

		public void SwitchDocking()
		{
			switch (ViewDock)
			{
			case DockStyle.Bottom:
				ViewDock = DockStyle.Left;
				break;
			case DockStyle.Fill:
				ViewDock = DockStyle.Bottom;
				break;
			case DockStyle.Left:
				ViewDock = DockStyle.Right;
				break;
			case DockStyle.Right:
				ViewDock = DockStyle.Fill;
				break;
			}
		}

		public void RefreshView()
		{
			TabBar.TabBarItem selectedTab = tabStrip.SelectedTab;
			if (selectedTab == null)
			{
				return;
			}
			foreach (TabBar.TabBarItem item in tabStrip.Items)
			{
				Control control = item.Tag as Control;
				if (control != null)
				{
					control.Visible = item == selectedTab;
				}
			}
		}

		private bool ShowView(TabBar.TabBarItem tabStripButton)
		{
			if (!tabStrip.Items.Contains(tabStripButton))
			{
				return false;
			}
			SuspendLayout();
			try
			{
				TabBar.TabBarItem selectedTab = tabStrip.SelectedTab;
				bool flag = tabStripButton.Tag is int;
				if (flag && comicViewer != null)
				{
					comicViewer.Show();
					comicViewer.GetControls<ComicDisplayControl>().FirstOrDefault().Focus();
				}
				else
				{
					TabBar.TabBarItem tabBarItem = tabStrip.Items.FirstOrDefault((TabBar.TabBarItem t) => t == tabStripButton && t.Tag is Control);
					if (tabBarItem != null)
					{
						Control control = tabBarItem.Tag as Control;
						control.Show();
						control.Focus();
					}
				}
				foreach (TabBar.TabBarItem item in tabStrip.Items.Where((TabBar.TabBarItem tsi) => tsi != tabStripButton && tsi.Tag is Control))
				{
					((Control)item.Tag).Hide();
				}
				if (!flag)
				{
					lastBrowser = tabStripButton;
					if (comicViewer != null)
					{
						comicViewer.Hide();
					}
				}
				tabStrip.SelectedTab = tabStripButton;
				return selectedTab == tabStripButton;
			}
			catch
			{
				return false;
			}
			finally
			{
				ResumeLayout();
			}
		}

		private bool ShowView(string name)
		{
			return ShowView(tabStrip.Items[name] ?? tsbLibrary);
		}

		private Control AddView(Control c, TabBar.TabBarItem tsb)
		{
			base.Controls.Add(c);
			c.Visible = false;
			c.Dock = DockStyle.Fill;
			tsb.Tag = c;
			base.Controls.SetChildIndex(c, 0);
			c.Disposed += delegate
			{
				OnViewRemoved();
			};
			OnViewAdded();
			return c;
		}

		private void RemoveView(Control c, TabBar.TabBarItem tsb)
		{
			tabStrip.Items.Remove(tsb);
			base.Controls.Remove(c);
			c.Dispose();
		}

		public ComicExplorerView AddExplorerView(ComicLibrary library, ComicListBrowser clb, TabBar.TabBarItem tsb, ComicExplorerViewSettings settings = null)
		{
			ComicExplorerView ev = new ComicExplorerView
			{
				ComicListBrowser = clb
			};
			ev.ComicBrowser.Library = library;
			ScriptUtility.CreateComicInfoPages().ForEach(delegate(ComicPageControl cip)
			{
				((ISidebar)ev).AddInfo(cip);
			});
			ev.ViewSettings = settings;
			AddView(ev, tsb);
			return ev;
		}

		private static void SetBackgroundImage(ComicExplorerView view, Image img)
		{
			view.ComicBrowser.ListBackgroundImage = img;
		}

		public void SetWorkspace(DisplayWorkspace workspace)
		{
			SuspendLayout();
			try
			{
				workspace.DatabaseView.ItemViewConfig = null;
				dbView.ViewSettings = workspace.DatabaseView;
				if (fileView != null)
				{
					fileView.ViewSettings = workspace.FileView;
				}
			}
			finally
			{
				ResumeLayout(performLayout: true);
			}
		}

		public void StoreWorkspace(DisplayWorkspace ws)
		{
			if (ws != null && ws.DatabaseView != null && dbView != null)
			{
				ws.DatabaseView = dbView.ViewSettings;
				if (fileView != null)
				{
					ws.FileView = fileView.ViewSettings;
				}
				ws.DatabaseView.ItemViewConfig = null;
				if (tabStrip != null && tabStrip.SelectedTab != null)
				{
					Program.Settings.SelectedBrowser = tabStrip.SelectedTab.Name;
				}
			}
		}

		public void AddListWindow(Image windowIcon, IComicBookListProvider bookList)
		{
			ComicBrowserForm comicBrowserForm = new ComicBrowserForm
			{
				Text = bookList.Name,
				ShowInTaskbar = false
			};
			comicBrowserForm.Disposed += ComicBrowserForm_Disposed;
			comicBrowserForm.Resize += ComicBrowserForm_Resize;
			comicBrowserForm.Show();
			comicBrowserForm.Main = base.Main;
			comicBrowserForm.BookList = bookList;
			openBrowsers.Add(comicBrowserForm);
		}

		public void AddListTab(Image tabImage, IComicBookListProvider bookList)
		{
			SuspendLayout();
			try
			{
				string name = NumberedString.StripNumber(bookList.Name);
				int number = NumberedString.MaxNumber(from tb in tabStrip.Items
					where NumberedString.StripNumber(tb.Text) == name
					select tb.Text);
				TabBar.TabBarItem tsb = new TabBar.TabBarItem(NumberedString.Format(name, number));
				ComicBrowserView comicBrowserView = new ComicBrowserView();
				Bitmap bitmap = tabImage.Clone() as Bitmap;
				bitmap.ToGrayScale();
				tsb.Image = bitmap;
				tsb.Tag = comicBrowserView;
				tsb.CanClose = true;
				tsb.CloseClick += delegate
				{
					RemoveList(bookList, tsb);
				};
				tsb.CaptionClick += tab_CaptionClick;
				tabStrip.Items.Insert(1, tsb);
				EventHandler rename = delegate
				{
					tsb.Text = bookList.Name;
				};
				bookList.NameChanged += rename;
				tsb.Removed += delegate
				{
					bookList.NameChanged -= rename;
				};
				AddView(comicBrowserView, tsb);
				ShowView(tsb);
				comicBrowserView.Main = base.Main;
				comicBrowserView.BookList = bookList;
			}
			finally
			{
				ResumeLayout();
			}
		}

		public void RemoveList(IComicBookListProvider bookList, object hint)
		{
			bool flag = false;
			ComicBrowserForm[] array = openBrowsers.ToArray();
			foreach (ComicBrowserForm comicBrowserForm in array)
			{
				if ((hint == null || comicBrowserForm == hint) && comicBrowserForm.BookList == bookList)
				{
					comicBrowserForm.SafeDispose();
				}
			}
			TabBar.TabBarItem[] array2 = tabStrip.Items.ToArray();
			foreach (TabBar.TabBarItem tabBarItem in array2)
			{
				Control control = tabBarItem.Tag as ComicBrowserView;
				if (control != null && (hint == null || tabBarItem == hint || control == hint))
				{
					tabStrip.Items.Remove(tabBarItem);
					base.Controls.Remove(control);
					control.Dispose();
					flag = true;
				}
			}
			if (flag)
			{
				ShowLibrary((bookList is IComicLibraryItem) ? (bookList as IComicLibraryItem).Library : null);
			}
		}
	}
}
