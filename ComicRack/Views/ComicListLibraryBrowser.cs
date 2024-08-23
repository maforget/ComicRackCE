#define TRACE
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using cYo.Common;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.IO;
using cYo.Common.Localize;
using cYo.Common.Text;
using cYo.Common.Win32;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.Sync;
using cYo.Projects.ComicRack.Viewer.Config;
using cYo.Projects.ComicRack.Viewer.Controls;
using cYo.Projects.ComicRack.Viewer.Dialogs;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public partial class ComicListLibraryBrowser : ComicListBrowser, IDisplayWorkspace, IImportComicList, ILibraryBrowser, IBrowseHistory
	{
		private class ViewConfigurationHandler : IDisplayListConfig
		{
			private readonly IDisplayListConfig vc;

			private readonly Guid id;

			public DisplayListConfig Display
			{
				get
				{
					return Program.Settings.GetRemoteViewConfig(id, vc.Display);
				}
				set
				{
					Program.Settings.UpdateRemoteViewConfig(id, value);
				}
			}

			public ViewConfigurationHandler(Guid id, IDisplayListConfig vc)
			{
				this.id = id;
				this.vc = vc;
			}
		}

		private class RemoveBookHandler : IRemoveBooks
		{
			private readonly ComicLibrary library;

			private readonly IComicBookListProvider cbl;

			private readonly IWin32Window parent;

			public RemoveBookHandler(IWin32Window parent, ComicLibrary library, IComicBookListProvider cbl)
			{
				this.parent = parent;
				this.library = library;
				this.cbl = cbl;
			}

			public void RemoveBooks(IEnumerable<ComicBook> books, bool ask)
			{
				books = books.ToArray();
				IEditableComicBookListProvider editableComicBookListProvider = cbl as IEditableComicBookListProvider;
				IFilteredComicBookList filteredComicBookList = cbl as IFilteredComicBookList;
				IBlackList blackList = library as IBlackList;
				Image booksImage = null;
				bool deleteFromLibrary;
				try
				{
					string moveToRecycleBin = null;
					if (ask)
					{
						if (books.Any((ComicBook b) => b.IsLinked))
						{
							moveToRecycleBin = (Program.Settings.MoveFilesToRecycleBin ? "!" : string.Empty) + TR.Messages["MoveBin", "&Also move the files to the Recycle Bin"];
						}
						booksImage = Program.MakeBooksImage(books, new Size(256, 128), 5, onlyMemory: false);
					}
					if (cbl is ComicLibraryListItem || (editableComicBookListProvider == null && filteredComicBookList == null))
					{
						deleteFromLibrary = true;
						if (ask)
						{
							QuestionResult questionResult = QuestionDialog.AskQuestion(parent, TR.Messages["AskRemoveFromLibrary", "Are you sure you want to remove these books from the library?\nAll information not stored in the files will be lost (like Open Count, Last Read etc.)!"], TR.Messages["Remove", "Remove"], moveToRecycleBin, booksImage);
							if (questionResult.HasFlag(QuestionResult.Cancel))
								return;

							Program.Settings.MoveFilesToRecycleBin = questionResult.HasFlag(QuestionResult.Option);
						}
					}
					else
					{
						deleteFromLibrary = ((filteredComicBookList != null) ? Program.Settings.AlsoRemoveFromLibraryFiltered : Program.Settings.AlsoRemoveFromLibrary);
						if (ask)
						{
							QuestionResult questionResult2 = QuestionDialog.AskQuestion(parent, TR.Messages["AskRemoveComics", "Are you sure you want to remove these books from the list?"], TR.Messages["Remove", "Remove"], delegate(QuestionDialog qd)
							{
								qd.OptionText = (deleteFromLibrary ? "!" : string.Empty) + TR.Messages["AlsoRemoveFromLibrary", "&Additionally remove the books from the Library (all information not stored in the files will be lost)"];
								qd.Option2Text = moveToRecycleBin;
								qd.Image = booksImage;
								qd.ShowCancel = true;
							});
							if (questionResult2.HasFlag(QuestionResult.Cancel))
								return;

							deleteFromLibrary = questionResult2.HasFlag(QuestionResult.Option);
							if (filteredComicBookList != null)
							{
								Program.Settings.AlsoRemoveFromLibraryFiltered = deleteFromLibrary;
							}
							else
							{
								Program.Settings.AlsoRemoveFromLibrary = deleteFromLibrary;
							}
							Program.Settings.MoveFilesToRecycleBin = questionResult2.HasFlag(QuestionResult.Option2);
						}
						foreach (ComicBook book in books)
						{
							editableComicBookListProvider?.Remove(book);
							if (filteredComicBookList != null && !deleteFromLibrary)
							{
								filteredComicBookList.SetFiltered(book, filtered: true);
							}
						}
					}
				}
				finally
				{
					booksImage.SafeDispose();
				}
				if (!deleteFromLibrary)
				{
					return;
				}
				bool flag = false;
				using (new WaitCursor())
				{
					foreach (ComicBook book2 in books)
					{
						if (book2.IsLinked && Program.Settings.MoveFilesToRecycleBin)
						{
							try
							{
								ShellFile.DeleteFile(parent, ShellFileDeleteOptions.None, book2.FilePath);
							}
							catch (Exception)
							{
							}
							if (File.Exists(book2.FilePath))
							{
								flag = true;
								continue;
							}
						}
						library.Remove(book2);
						if (book2.IsLinked)
						{
							blackList?.AddToBlackList(book2.FilePath);
						}
					}
				}
				if (flag)
				{
					MessageBox.Show(parent, TR.Messages["FailedDeleteBooks", "Some files could not be deleted (maybe they are in use)!"], Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}

		private readonly CommandMapper commands = new CommandMapper();

		private readonly Dictionary<Guid, TreeNode> nodeMap = new Dictionary<Guid, TreeNode>();

		private bool treeDirty;

		private readonly NiceTreeSkin treeSkin;

		private ComicLibrary library;

		private bool mouseActivate;

		private TreeNode dragNode;

		private DragDropContainer dragBookContainer;

		private IBitmapCursor dragCursor;

		public ComicLibrary Library
		{
			get
			{
				return library;
			}
			set
			{
				if (library != value)
				{
					if (library != null)
					{
						library.ComicListCachesUpdated -= library_ListCachesUpdated;
						library.ComicListsChanged -= library_ComicListsChanged;
					}
					library = value;
					if (library != null)
					{
						library.ComicListCachesUpdated += library_ListCachesUpdated;
						library.ComicListsChanged += library_ComicListsChanged;
					}
					OnLibraryChanged();
				}
			}
		}

		public ComicsEditModes ComicEditMode
		{
			get
			{
				if (Library != null)
				{
					return Library.EditMode;
				}
				return ComicsEditModes.Default;
			}
		}

		public override bool TopBrowserVisible
		{
			get
			{
				return favContainer.Expanded;
			}
			set
			{
				favContainer.Expanded = value;
			}
		}

		public override int TopBrowserSplit
		{
			get
			{
				return favContainer.ExpandedWidth;
			}
			set
			{
				favContainer.ExpandedWidth = value;
			}
		}

		public event EventHandler LibraryChanged;

		public ComicListLibraryBrowser()
		{
			InitializeComponent();
			treeImages.ImageSize = treeImages.ImageSize.ScaleDpi();
			treeSkin = new LibraryTreeSkin
			{
				TreeView = tvQueries
			};
			tvQueries.Font = SystemFonts.IconTitleFont;
			favContainer.Expanded = false;
			LocalizeUtility.Localize(this, components);
			quickSearch.SetCueText(tsQuickSearch.Text);
			queryCacheTimer.Interval = (ComicLibrary.IsQueryCacheInstantUpdate ? 100 : 2500);
			miPasteList.Click += new EventHandler((sender, e) => PasteList());
        }

        public ComicListLibraryBrowser(ComicLibrary library)
			: this()
		{
			Library = library;
		}

		protected virtual void OnLibraryChanged()
		{
			FillListTree();
			if (this.LibraryChanged != null)
			{
				this.LibraryChanged(this, EventArgs.Empty);
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (!base.DesignMode)
			{
				treeImages.Images.Add("Library", Resources.Library);
				treeImages.Images.Add("Folder", Resources.SearchFolder);
				treeImages.Images.Add("Search", Resources.SearchDocument);
				treeImages.Images.Add("List", Resources.List);
				treeImages.Images.Add("TempFolder", Resources.TempFolder);
				FillListTree();
				TreeNode treeNode = FindItemNode(Program.Settings.LastLibraryItem);
				if (treeNode != null)
				{
					tvQueries.SelectedNode = treeNode;
				}
				UpdateBookList();
				commands.Add(ExpandCollapseAllNodes, tbExpandCollapseAll);
				commands.Add(RenameNode, () => tvQueries.SelectedNode != null && ComicEditMode.CanEditList(), miQueryRename);
				commands.Add(delegate
				{
					EditSelectedComicList();
				}, () => tvQueries.SelectedNode != null && (tvQueries.SelectedNode.Tag is ComicSmartListItem || tvQueries.SelectedNode.Tag is ComicListItemFolder || tvQueries.SelectedNode.Tag is ComicIdListItem) && ComicEditMode.CanEditList(), miEditSmartList);
				commands.Add(NewSmartList, () => ComicEditMode.CanEditList(), miNewSmartList, tbNewSmartList);
				commands.Add(NewFolder, () => ComicEditMode.CanEditList(), miNewFolder, tbNewFolder);
				commands.Add(NewList, () => ComicEditMode.CanEditList(), miNewList, tbNewList);
				commands.Add(RemoveListOrFolder, () => tvQueries.SelectedNode != null && !tvQueries.SelectedNode.IsEditing && !(tvQueries.SelectedNode.Tag is ComicLibraryListItem) && ComicEditMode.CanEditList(), miRemoveListOrFolder);
				commands.Add(OpenWindow, miOpenWindow, tbOpenWindow);
				commands.Add(OpenTab, miOpenTab, tbOpenTab);
				commands.Add(RefreshDisplay, tbRefresh);
				commands.Add(SortList, () => tvQueries.SelectedNode != null && tvQueries.SelectedNode.Tag is ComicListItemFolder, miNodeSort);
				commands.Add(CopyList, () => tvQueries.SelectedNode != null && (tvQueries.SelectedNode.Tag is ShareableComicListItem || (Program.ExtendedSettings.AllowCopyListFolders && tvQueries.SelectedNode.Tag is ComicListItemFolder)) && ComicEditMode.CanEditList(), miCopyList);
				commands.Add(ExportList, () => tvQueries.SelectedNode != null && tvQueries.SelectedNode.Tag is ShareableComicListItem && ComicEditMode.CanEditList(), miExportReadingList);
				commands.Add(ImportLists, () => ComicEditMode.CanEditList(), miImportReadingList);
                //commands.Add(PasteList, () => (Clipboard.ContainsData(ShareableComicListItem.ClipboardFormat) || Clipboard.ContainsText()) && ComicEditMode.CanEditList(), miPasteList);
                commands.Add(AddToFavorites, miAddToFavorites);
				commands.Add(delegate
				{
					TopBrowserVisible = !TopBrowserVisible;
				}, true, () => TopBrowserVisible, tbFavorites);
				commands.Add(ToggleQuickSearch, true, () => quickSearchPanel.Visible, tsQuickSearch);
				commands.Add(RefreshFavorites, miFavRefresh);
				commands.Add(RemoveFavorite, miFavRemove);
                miQueryRename.Visible = miEditSmartList.Visible = miNewSmartList.Visible = tbNewSmartList.Visible = miNewList.Visible = tbNewList.Visible = 
					miNewFolder.Visible = tbNewFolder.Visible = miRemoveListOrFolder.Visible = miCopyList.Visible = miPasteList.Visible = miExportReadingList.Visible = 
					miImportReadingList.Visible = tssOpenWindow.Visible = miNewSeparator.Visible = miCopySeparator.Visible = miRemoveSeparator.Visible = 
					miAddToFavorites.Visible = ComicEditMode.CanEditList();
                miRefresh.Visible = tbRefreshSeparator.Visible = tbRefresh.Visible = !ComicEditMode.IsLocalComic();
            }
		}

		protected override void OnListServiceRequest(IComicBookListProvider senderList, ServiceRequestEventArgs e)
		{
			base.OnListServiceRequest(senderList, e);
			if (e.ServiceType == typeof(IDisplayListConfig) && !ComicEditMode.IsLocalComic())
			{
				IDisplayListConfig displayListConfig = senderList as IDisplayListConfig;
				if (displayListConfig != null)
				{
					e.Service = new ViewConfigurationHandler(senderList.Id, displayListConfig);
				}
			}
			if (e.Service == null && e.ServiceType == typeof(IRemoveBooks))
			{
				e.Service = new RemoveBookHandler(this, Library, senderList);
			}
		}

		private TreeNode FindItemNode(Guid id)
		{
			if (!nodeMap.TryGetValue(id, out var value))
			{
				return null;
			}
			return value;
		}

		private TreeNode FindItemNode(IComicBookListProvider item)
		{
			if (item == null)
			{
				return null;
			}
			return FindItemNode(item.Id);
		}

		[Conditional("BIGITEMS")]
		private static void SetDoubleHeight(TreeNode node)
		{
			node.SetHeight(2);
		}

		private TreeNode AddNode(TreeNodeCollection nodes, ComicListItem item)
		{
			TreeNode treeNode = nodes.Add(item.Name);
			nodeMap[item.Id] = treeNode;
			treeNode.Tag = item;
			string text2 = (treeNode.ImageKey = (treeNode.SelectedImageKey = item.ImageKey));
			_ = item is ComicLibraryListItem;
			return treeNode;
		}

		private bool RemoveNode(ComicListItem item)
		{
			TreeNode treeNode = FindItemNode(item);
			if (treeNode == null)
			{
				return false;
			}
			if (treeNode.Parent == null)
			{
				tvQueries.Nodes.Remove(treeNode);
			}
			else
			{
				treeNode.Parent.Nodes.Remove(treeNode);
			}
			nodeMap.Remove(item.Id);
			return true;
		}

		private void FillListTree(TreeNodeCollection tnc, ICollection<ComicListItem> items, string filter = null)
		{
			if (!string.IsNullOrEmpty(filter))
			{
				items = items.Where((ComicListItem cli) => cli is ComicLibraryListItem || cli.Filter(filter)).ToArray();
			}
			ComicListItem[] list = (from tn in tnc.OfType<TreeNode>()
				select tn.Tag as ComicListItem into cli
				where !items.Contains(cli)
				select cli).ToArray();
			list.ForEach(delegate(ComicListItem cli)
			{
				RemoveNode(cli);
			});
			int num = 0;
			foreach (ComicListItem item in items)
			{
				TreeNode treeNode = FindItemNode(item);
				bool flag = false;
				ComicListItemFolder comicListItemFolder = item as ComicListItemFolder;
				if (comicListItemFolder != null)
				{
					flag = !comicListItemFolder.Collapsed;
				}
				else if (item is ComicLibraryListItem)
				{
					flag = true;
				}
				if (treeNode == null)
				{
					treeNode = AddNode(tnc, item);
				}
				else
				{
					if (item.Name != treeNode.Text)
					{
						treeNode.Text = item.Name;
					}
					string text = item.Description.LineBreak(60);
					if (text != treeNode.ToolTipText)
					{
						treeNode.ToolTipText = text;
					}
				}
				int num2 = tnc.IndexOf(treeNode);
				if (num2 != num)
				{
					tnc.Remove(treeNode);
					tnc.Insert(num, treeNode);
					_ = item is ComicLibraryListItem;
				}
				num++;
				treeNode.ForeColor = (item.RecursionTest() ? Color.Red : SystemColors.WindowText);
				if (comicListItemFolder != null)
				{
					FillListTree(treeNode.Nodes, comicListItemFolder.Items, filter);
				}
				if (flag)
				{
					treeNode.Expand();
				}
				else
				{
					treeNode.Collapse();
				}
			}
		}

		private void FillListTree(ICollection<ComicListItem> items, string filter = null)
		{
			Guid bookListId = base.BookListId;
			if (items != null)
			{
				FillListTree(tvQueries.Nodes, items, filter);
			}
			if (tvQueries.Nodes.Count > 0)
			{
				TreeNode treeNode = FindItemNode(bookListId);
				tvQueries.SelectedNode = treeNode ?? tvQueries.Nodes[0];
			}
		}

		private void FillListTree()
		{
			FillListTree(Library.ComicLists, quickSearch.Text.Trim());
			FillFavorites();
		}

		private bool EditSelectedComicList()
		{
			if (!ComicEditMode.CanEditList())
			{
				return false;
			}
			TreeNode selectedNode = tvQueries.SelectedNode;
			if (selectedNode == null)
			{
				return false;
			}
			if (selectedNode.Tag is ComicSmartListItem)
			{
				return EditSmartListItem(selectedNode, selectedNode.Tag as ComicSmartListItem);
			}
			if (selectedNode.Tag is ComicListItemFolder || selectedNode.Tag is ComicIdListItem)
			{
				return EditListItem(selectedNode.Tag as ComicListItem);
			}
			return false;
		}

		private bool EditListItem(ComicListItem cli)
		{
			if (cli != null && EditListDialog.Edit(this, cli))
			{
				cli.Refresh();
				return true;
			}
			return false;
		}

		private bool EditSmartListItem(TreeNode tn, ComicSmartListItem csli)
		{
			if (csli == null)
			{
				return false;
			}
			bool flag = Control.ModifierKeys.HasFlag(Keys.Control);
			ComicSmartListItem comicSmartListItem = csli.Clone() as ComicSmartListItem;
			ComicSmartListItem oldList = csli.Clone() as ComicSmartListItem;
			ISmartListDialog sld;
			TreeNodeCollection tnc;
			Func<TreeNode> getNext;
			Func<TreeNode> getPrev;
			Action<Func<TreeNode>> setItem;
			while (comicSmartListItem != null)
			{
				using (Form form = (flag ? ((Form)new SmartListQueryDialog()) : ((Form)new SmartListDialog())))
				{
					sld = form as ISmartListDialog;
					sld.Library = csli.Library;
					sld.EditId = csli.Id;
					sld.SmartComicList = comicSmartListItem;
					tnc = ((tn.Parent != null) ? tn.Parent.Nodes : tvQueries.Nodes);
					sld.EnableNavigation = tnc.OfType<TreeNode>().Count((TreeNode n) => n.Tag is ComicSmartListItem) > 1;
					getNext = () => tnc.OfType<TreeNode>().SkipWhile((TreeNode n) => n != tn).Skip(1)
						.FirstOrDefault((TreeNode n) => n.Tag is ComicSmartListItem);
					getPrev = () => tnc.OfType<TreeNode>().Reverse().SkipWhile((TreeNode n) => n != tn)
						.Skip(1)
						.FirstOrDefault((TreeNode n) => n.Tag is ComicSmartListItem);
					setItem = delegate(Func<TreeNode> get)
					{
						TreeNode treeNode = get();
						if (treeNode != null)
						{
							using (new WaitCursor(this))
							{
								tvQueries.SelectedNode = (tn = treeNode);
								UpdateBookList();
							}
							csli = treeNode.Tag as ComicSmartListItem;
							oldList = csli.Clone() as ComicSmartListItem;
							sld.EditId = csli.Id;
							Application.DoEvents();
							sld.SmartComicList = csli.Clone() as ComicSmartListItem;
						}
						sld.PreviousEnabled = getPrev() != null;
						sld.NextEnabled = getNext() != null;
					};
					sld.PreviousEnabled = getPrev() != null;
					sld.NextEnabled = getNext() != null;
					sld.Apply += delegate
					{
						csli.SetList(sld.SmartComicList);

						//Remove recursionCache if we changed the source
						if (csli.BaseListId != oldList.BaseListId)
							RecursionCache.Items.RemoveReference(csli.Id);
					};
					sld.Next += delegate
					{
						setItem(getNext);
					};
					sld.Previous += delegate
					{
						setItem(getPrev);
					};
					switch (form.ShowDialog(this))
					{
					case DialogResult.Cancel:
						csli.SetList(oldList);
						return false;
					case DialogResult.Retry:
						comicSmartListItem = sld.SmartComicList;
						break;
					default:
						comicSmartListItem = null;
						break;
					}
				}
				flag = !flag;
			}
			return true;
		}

		protected override void OnIdle()
		{
			base.OnIdle();
			if (!Library.ComicListsLocked && treeDirty)
			{
				treeDirty = false;
				FillListTree();
				CommitListCacheChange();
			}
		}

		private ComicListItem GetCurrentNodeComicList()
		{
			if (tvQueries.SelectedNode != null)
			{
				return (ComicListItem)tvQueries.SelectedNode.Tag;
			}
			return null;
		}

		private ComicListItemCollection GetCurrentNodeComicListCollection()
		{
			return GetNodeComicListCollection(tvQueries.SelectedNode);
		}

		private ComicListItemCollection GetNodeComicListCollection(TreeNode sn)
		{
			if (sn == null)
			{
				return Library.ComicLists;
			}
			if (sn.Tag is ComicListItemFolder)
			{
				return ((ComicListItemFolder)sn.Tag).Items;
			}
			if (sn.Parent != null)
			{
				return ((ComicListItemFolder)sn.Parent.Tag).Items;
			}
			return Library.ComicLists;
		}

		private void OpenWindow()
		{
			try
			{
				OpenListInNewWindow();
			}
			catch
			{
			}
		}

		private void OpenTab()
		{
			try
			{
				OpenListInNewTab(treeImages.Images[tvQueries.SelectedNode.ImageKey]);
			}
			catch (Exception ex)
			{
				Trace.WriteLine("Failed to open in other tab: " + ex.Message);
			}
		}

		private void library_ComicListsChanged(object sender, ComicListItemChangedEventArgs e)
		{
			if (!treeDirty)
			{
				if (e.Change != ComicListItemChange.Statistic)
				{
					treeDirty = true;
				}
				else
				{
					tvQueries.Invalidate();
				}
			}
		}

		private void tvQueries_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (tvQueries.HitTest(e.Location).Location != TreeViewHitTestLocations.PlusMinus)
			{
				EditSelectedComicList();
			}
		}

		private void tvQueries_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (dragNode == null)
			{
				if (mouseActivate)
				{
					UpdateBookList();
				}
				else
				{
					updateTimer.Stop();
					updateTimer.Start();
				}
				mouseActivate = false;
			}
		}

		private void updateTimer_Tick(object sender, EventArgs e)
		{
			UpdateBookList();
		}

		private void UpdateBookList()
		{
			updateTimer.Stop();
			using (new WaitCursor())
			{
				if (tvQueries.SelectedNode != null)
				{
					BookList = tvQueries.SelectedNode.Tag as IComicBookListProvider;
				}
			}
		}

		private void tvQueries_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			e.CancelEdit = !ComicEditMode.CanEditList();
		}

		private void tvQueries_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			ComicListItem comicListItem = ((e.Node == null) ? null : (e.Node.Tag as ComicListItem));
			if (comicListItem == null || string.IsNullOrEmpty(e.Label))
			{
				e.CancelEdit = true;
			}
			else
			{
				comicListItem.Name = e.Label;
			}
		}

		private void tvQueries_ItemDrag(object sender, ItemDragEventArgs e)
		{
			if (!ComicEditMode.CanEditList())
			{
				return;
			}
			if (e.Button == MouseButtons.Left)
			{
				dragNode = e.Item as TreeNode;
			}
			if (dragNode != null && dragNode.Tag is ComicLibraryListItem)
			{
				dragNode = null;
			}
			if (dragNode == null)
			{
				return;
			}
			Point cursorLocation = tvQueries.PointToClient(Cursor.Position);
			dragCursor = treeSkin.GetDragCursor(dragNode, 64, cursorLocation);
			try
			{
				DataObjectEx dataObjectEx = new DataObjectEx();
				dataObjectEx.SetData(dragNode);
				ShareableComicListItem sc = dragNode.Tag as ShareableComicListItem;
				if (sc != null)
				{
					dataObjectEx.SetFile(FileUtility.MakeValidFilename(sc.Name + ".cbl"), delegate(Stream stream)
					{
						try
						{
							new ComicReadingListContainer(sc, Program.Settings.ExportedListsContainFilenames).Serialize(stream);
						}
						catch
						{
						}
					});
				}
				DragDropEffects dragDropEffects = tvQueries.DoDragDrop(dataObjectEx, DragDropEffects.Copy | DragDropEffects.Move);
				OnIdle();
				tvQueries.SelectedNode = ((dragDropEffects == DragDropEffects.None) ? dragNode : FindItemNode((ComicListItem)dragNode.Tag));
			}
			finally
			{
				if (dragCursor != null)
				{
					dragCursor.Dispose();
				}
				dragCursor = null;
				dragNode = null;
			}
		}

		private void tvQueries_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				TreeNode nodeAt = tvQueries.GetNodeAt(e.X, e.Y);
				if (nodeAt != null)
				{
					tvQueries.SelectedNode = nodeAt;
					UpdateBookList();
				}
			}
			mouseActivate = true;
		}

		private void tvQueries_AfterExpand(object sender, TreeViewEventArgs e)
		{
			ComicListItemFolder comicListItemFolder = e.Node.Tag as ComicListItemFolder;
			if (comicListItemFolder != null)
			{
				comicListItemFolder.Collapsed = false;
			}
		}

		private void tvQueries_AfterCollapse(object sender, TreeViewEventArgs e)
		{
			ComicListItemFolder comicListItemFolder = e.Node.Tag as ComicListItemFolder;
			if (comicListItemFolder != null)
			{
				comicListItemFolder.Collapsed = true;
			}
		}

		private void tvQueries_DrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			ComicListItem comicListItem = e.Node.Tag as ComicListItem;
			if (comicListItem != null && comicListItem.PendingCacheUpdate)
			{
				OnListCacheChanged();
			}
		}

		private void treeContextMenu_Opening(object sender, CancelEventArgs e)
		{
			miPasteList.Enabled = isPasteListEnabled();
            ComicListItem cli = tvQueries.SelectedNode.Tag as ComicListItem;
			bool flag = cli != null && Program.Settings.Devices.Count > 0;
			cmEditDevices.DropDownItems.Clear();
			cmEditDevices.Visible = flag;
			if (!flag)
			{
				return;
			}
			if (Program.Settings.Devices.Count == 1)
			{
				string format = TR.Load(base.Name)["SyncDevice", "Sync with {0}..."];
				DeviceSyncSettings deviceSyncSettings = Program.Settings.Devices[0];
				cmEditDevices.Text = string.Format(format, deviceSyncSettings.DeviceName);
				cmEditDevices.Tag = cli;
				return;
			}
			cmEditDevices.Text = TR.Load(base.Name)["SyncDevices", "Sync with"];
			cmEditDevices.Tag = null;
			foreach (DeviceSyncSettings device in Program.Settings.Devices)
			{
				ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(device.DeviceName + "...")
				{
					Checked = (device.Lists.FirstOrDefault((DeviceSyncSettings.SharedList l) => l.ListId == cli.Id) != null)
				};
				DeviceSyncSettings dss1 = device;
				toolStripMenuItem.Click += delegate
				{
					base.Main.ShowPortableDevices(dss1, cli.Id);
					tvQueries.Refresh();
				};
				cmEditDevices.DropDownItems.Add(toolStripMenuItem);
			}
        }

		private void cmEditDevices_Click(object sender, EventArgs e)
		{
			ComicListItem comicListItem = cmEditDevices.Tag as ComicListItem;
			if (comicListItem != null)
			{
				base.Main.ShowPortableDevices(Program.Settings.Devices[0], comicListItem.Id);
				tvQueries.Refresh();
			}
		}

		private void ToggleQuickSearch()
		{
			if (!quickSearchPanel.Visible)
			{
				quickSearchPanel.Show();
				quickSearch.Focus();
			}
			else
			{
				quickSearch.Text = string.Empty;
				quickSearchPanel.Hide();
			}
		}

		private void quickSearch_TextChanged(object sender, EventArgs e)
		{
			FillListTree();
		}

		private void FillFavorites(bool refreshThumbnails = false)
		{
			if (!favContainer.Expanded)
			{
				return;
			}
			favView.BeginUpdate();
			try
			{
				favView.Items.Clear();
				foreach (ComicListItem item in from cl in Library.ComicLists.GetItems<ComicListItem>()
					where cl.Favorite
					select cl)
				{
					FavoriteViewItem favoriteViewItem = FavoriteViewItem.Create(item);
					favoriteViewItem.Tag = item.Id;
					favView.Items.Add(favoriteViewItem);
					if (refreshThumbnails)
					{
						Program.ImagePool.Thumbs.RefreshImage(favoriteViewItem.ThumbnailKey);
					}
				}
			}
			finally
			{
				favView.EndUpdate();
			}
		}

		private void RefreshFavorites()
		{
			FillFavorites(refreshThumbnails: true);
		}

		private void AddToFavorites()
		{
			TreeNode selectedNode = tvQueries.SelectedNode;
			if (selectedNode != null)
			{
				ComicListItem comicListItem = selectedNode.Tag as ComicListItem;
				if (comicListItem != null)
				{
					comicListItem.Favorite = true;
				}
			}
		}

		private void RemoveFavorite()
		{
			FavoriteViewItem favoriteViewItem = favView.SelectedItems.OfType<FavoriteViewItem>().FirstOrDefault();
			if (favoriteViewItem != null)
			{
				favoriteViewItem.ComicListItem.Favorite = false;
			}
		}

		private void favView_SelectedIndexChanged(object sender, EventArgs e)
		{
			ItemViewItem itemViewItem = favView.FocusedItem as ItemViewItem;
			if (itemViewItem != null)
			{
				SelectList((Guid)itemViewItem.Tag);
			}
		}

		private void favView_Resize(object sender, EventArgs e)
		{
			int width = favView.ClientRectangle.Width - SystemInformation.VerticalScrollBarWidth - 8;
			favView.ItemTileSize = new Size(width, FormUtility.ScaleDpiY(50));
		}

		private void favContainer_ExpandedChanged(object sender, EventArgs e)
		{
			if (favContainer.Expanded)
			{
				FillFavorites();
			}
		}

		private bool CreateDragContainter(DragEventArgs e)
		{
			if (dragBookContainer == null)
			{
				dragBookContainer = DragDropContainer.Create(e.Data);
			}
			return dragBookContainer.IsValid;
		}

		private static void InsertBooksIntoToList(IEditableComicBookListProvider list, int index, DragDropContainer bookContainer)
		{
			if (bookContainer.IsFilesContainer)
			{
				Program.Scanner.ScanFilesOrFolders(bookContainer.FilesOrFolders, all: false, removeMissing: false);
			}
			else
			{
				if (list == null)
				{
					return;
				}
				foreach (ComicBook book in bookContainer.Books.GetBooks())
				{
					ComicBook comicBook = (book.IsLinked ? Program.BookFactory.Create(book.FilePath, CreateBookOption.AddToStorage) : book);
					if (index != -1)
					{
						index = list.Insert(index, comicBook) + 1;
					}
					else
					{
						list.Add(comicBook);
					}
				}
			}
		}

		private void SetDropEffects(DragEventArgs e)
		{
			e.Effect = DragDropEffects.None;
			if (!ComicEditMode.CanEditList())
			{
				return;
			}
			CreateDragContainter(e);
			Point point = tvQueries.PointToClient(new Point(e.X, e.Y));
			TreeNode node = tvQueries.GetNodeAt(point);
			if (dragNode != null)
			{
				if (node != dragNode && dragNode == e.Data.GetData(typeof(TreeNode)) && dragNode.Nodes.Find((TreeNode cn) => cn == node) == null)
				{
					e.Effect = ((dragNode.Tag is ShareableComicListItem && ((uint)e.KeyState & 8u) != 0) ? DragDropEffects.Copy : DragDropEffects.Move);
					Point point2 = point;
					if (node != null)
					{
						point2.Y -= node.Bounds.Y;
					}
					treeSkin.SeparatorDropNodeStyle = node != null && ((point2.Y >= 0 && point2.Y < 4) || !(node.Tag is ComicListItemFolder));
				}
			}
			else if (dragBookContainer.IsBookContainer)
			{
				if (node == null || node.Tag is IEditableComicBookListProvider || node.Tag is ComicListItemFolder || node.Tag is ComicSmartListItem)
				{
					e.Effect = e.AllowedEffect;
					treeSkin.SeparatorDropNodeStyle = false;
				}
			}
			else if (dragBookContainer.IsReadingListsContainer)
			{
				if (node == null || node.Tag is ComicListItemFolder)
				{
					e.Effect = e.AllowedEffect;
					treeSkin.SeparatorDropNodeStyle = false;
				}
			}
			else if (dragBookContainer.IsFilesContainer)
			{
				IEditableComicBookListProvider editableComicBookListProvider = ((node == null) ? null : (node.Tag as IEditableComicBookListProvider));
				if (editableComicBookListProvider != null && editableComicBookListProvider.IsLibrary)
				{
					e.Effect = e.AllowedEffect;
				}
				treeSkin.SeparatorDropNodeStyle = false;
			}
			treeSkin.DropNode = ((e.Effect == DragDropEffects.None) ? null : node);
		}

		private void tvQueries_DragEnter(object sender, DragEventArgs e)
		{
			SetDropEffects(e);
		}

		private void tvQueries_DragLeave(object sender, EventArgs e)
		{
			treeSkin.DropNode = null;
			dragBookContainer = null;
		}

		private void tvQueries_DragOver(object sender, DragEventArgs e)
		{
			SetDropEffects(e);
		}

		private void tvQueries_DragDrop(object sender, DragEventArgs e)
		{
			TreeNode dropNode = treeSkin.DropNode;
			bool separatorDropNodeStyle = treeSkin.SeparatorDropNodeStyle;
			DragDropContainer dragDropContainer = dragBookContainer;
			treeSkin.DropNode = null;
			dragBookContainer = null;
			if (dragNode != null)
			{
				if (dragNode != e.Data.GetData(typeof(TreeNode)))
				{
					return;
				}
				ComicListItemCollection comicListItemCollection = ((dragNode.Parent == null) ? Library.ComicLists : ((ComicListItemFolder)dragNode.Parent.Tag).Items);
				ComicListItem comicListItem = dragNode.Tag as ComicListItem;
                RecursionCache.Items.RemoveReference(comicListItem.Id);
                if (e.Effect == DragDropEffects.Copy && comicListItem is ShareableComicListItem)
				{
					comicListItem = ((ICloneable)(ShareableComicListItem)comicListItem).Clone<ShareableComicListItem>();
				}
				if (dropNode == null)
				{
					comicListItemCollection.Remove(comicListItem);
					Library.ComicLists.Add(comicListItem);
				}
				else if (separatorDropNodeStyle)
				{
					ComicListItemCollection comicListItemCollection2 = ((dropNode.Parent == null) ? Library.ComicLists : ((ComicListItemFolder)dropNode.Parent.Tag).Items);
					int num = dropNode.Index;
					int num2 = comicListItemCollection.IndexOf(comicListItem);
					if (comicListItemCollection.Remove(comicListItem) && comicListItemCollection == comicListItemCollection2 && num2 < num)
					{
						num--;
					}
					comicListItemCollection2.Insert(num, comicListItem);
				}
				else
				{
					comicListItemCollection.Remove(comicListItem);
					ComicListItemCollection items = ((ComicListItemFolder)dropNode.Tag).Items;
					items.Add(comicListItem);
				}
				dragNode.Tag = comicListItem;
				OnIdle();
				tvQueries.SelectedNode = FindItemNode(comicListItem);
				BookList = null;
				UpdateBookList();
			}
			else if (dragDropContainer.IsBookContainer)
			{
				IEditableComicBookListProvider editableComicBookListProvider = ((dropNode == null) ? null : (dropNode.Tag as IEditableComicBookListProvider));
				ComicSmartListItem comicSmartListItem = ((dropNode == null) ? null : (dropNode.Tag as ComicSmartListItem));
				ComicListItemFolder comicListItemFolder = ((dropNode == null) ? null : (dropNode.Tag as ComicListItemFolder));
				if (editableComicBookListProvider != null)
				{
					InsertBooksIntoToList(editableComicBookListProvider, -1, dragDropContainer);
					return;
				}
				if (comicSmartListItem != null)
				{
					comicSmartListItem.Matchers.AddRange(dragDropContainer.CreateSeriesGroupMatchers());
					comicSmartListItem.Refresh();
					return;
				}
				ComicListItem comicListItem2 = null;
				comicListItem2 = ((!Control.ModifierKeys.IsSet(Keys.Alt) && !dragDropContainer.HasMatcher) ? ((ShareableComicListItem)dragDropContainer.CreateComicIdList()) : ((ShareableComicListItem)dragDropContainer.CreateSeriesSmartList()));
				if (comicListItem2 != null)
				{
					if (string.IsNullOrEmpty(comicListItem2.Name))
					{
						comicListItem2.Name = TR.Load(base.Name)["NewList", "New List"];
					}
					if (comicListItemFolder == null)
					{
						Library.ComicLists.Add(comicListItem2);
					}
					else
					{
						comicListItemFolder.Items.Add(comicListItem2);
					}
				}
			}
			else if (dragDropContainer.IsReadingListsContainer)
			{
				ComicListItemCollection fc = ((dropNode == null) ? Library.ComicLists : ((dropNode.Tag is ComicListItemFolder) ? ((ComicListItemFolder)dropNode.Tag).Items : null));
				using (new WaitCursor(this))
				{
					foreach (string readingList in dragDropContainer.ReadingLists)
					{
						ImportList(fc, readingList);
					}
				}
			}
			else if (dragDropContainer.IsFilesContainer)
			{
				InsertBooksIntoToList(null, -1, dragDropContainer);
			}
		}

		private void GiveDragCursorFeedback(object sender, GiveFeedbackEventArgs e)
		{
			if (dragCursor != null && !(dragCursor.Cursor == null))
			{
				e.UseDefaultCursors = false;
				dragCursor.OverlayCursor = ((e.Effect == DragDropEffects.None) ? Cursors.No : Cursors.Default);
				dragCursor.OverlayEffect = ((e.Effect == DragDropEffects.Copy) ? BitmapCursorOverlayEffect.Plus : BitmapCursorOverlayEffect.None);
				Cursor.Current = dragCursor.Cursor;
			}
		}

		private void RenameNode()
		{
			if (tvQueries.SelectedNode != null)
			{
				tvQueries.SelectedNode.BeginEdit();
			}
		}

		private void ExpandCollapseAllNodes()
		{
			if (tvQueries.AllNodes().Any((TreeNode t) => t.IsExpanded))
			{
				tvQueries.CollapseAll();
			}
			else
			{
				tvQueries.ExpandAll();
			}
		}

		private void SortList()
		{
			ComicListItemCollection currentNodeComicListCollection = GetCurrentNodeComicListCollection();
			if (currentNodeComicListCollection != null)
			{
				currentNodeComicListCollection.Sort(delegate(ComicListItem a, ComicListItem b)
				{
					int num = ((a is ComicListItemFolder) ? 1 : 0);
					int num2 = ((b is ComicListItemFolder) ? 1 : 0);
					int num3 = Math.Sign(num2 - num);
					return (num3 == 0) ? ExtendedStringComparer.Compare(a.Name, b.Name, ExtendedStringComparison.ZeroesFirst | ExtendedStringComparison.IgnoreArticles | ExtendedStringComparison.IgnoreCase) : num3;
				});
				FillListTree();
			}
		}

		private void NewSmartList()
		{
			ComicListItem currentNodeComicList = GetCurrentNodeComicList();
			ComicListItemCollection currentNodeComicListCollection = GetCurrentNodeComicListCollection();
			if (currentNodeComicListCollection != null)
			{
				string name = TR.Load(base.Name)["NewSmartList", "New Smart List"];
				TreeNode selectedNode = tvQueries.SelectedNode;
				ComicSmartListItem item = new ComicSmartListItem(name, string.Empty);
				currentNodeComicListCollection.Insert(currentNodeComicListCollection.IndexOf(currentNodeComicList) + 1, item);
				FillListTree();
				tvQueries.SelectedNode = FindItemNode(item);
				UpdateBookList();
				if (!EditSelectedComicList())
				{
					currentNodeComicListCollection.Remove(item);
					FillListTree();
					tvQueries.SelectedNode = selectedNode;
					UpdateBookList();
				}
			}
		}

		private void NewFolder()
		{
			ComicListItem currentNodeComicList = GetCurrentNodeComicList();
			ComicListItemCollection currentNodeComicListCollection = GetCurrentNodeComicListCollection();
			if (currentNodeComicListCollection != null)
			{
				ComicListItemFolder item = new ComicListItemFolder(TR.Load(base.Name)["NewFolder", "New Folder"]);
				if (EditListDialog.Edit(this, item))
				{
					currentNodeComicListCollection.Insert(currentNodeComicListCollection.IndexOf(currentNodeComicList) + 1, item);
				}
			}
		}

		private void NewList()
		{
			ComicListItem currentNodeComicList = GetCurrentNodeComicList();
			ComicListItemCollection currentNodeComicListCollection = GetCurrentNodeComicListCollection();
			if (currentNodeComicListCollection != null)
			{
				ComicIdListItem item = new ComicIdListItem(TR.Load(base.Name)["NewList", "New List"]);
				if (EditListDialog.Edit(this, item))
				{
					currentNodeComicListCollection.Insert(currentNodeComicListCollection.IndexOf(currentNodeComicList) + 1, item);
				}
			}
		}

		private void CopyList()
		{
			if (tvQueries.SelectedNode == null)
			{
				return;
			}
			ComicListItem comicListItem = tvQueries.SelectedNode.Tag as ShareableComicListItem;
			if (comicListItem == null && Program.ExtendedSettings.AllowCopyListFolders)
			{
				comicListItem = tvQueries.SelectedNode.Tag as ComicListItemFolder;
			}
			if (comicListItem == null)
			{
				return;
			}
			try
			{
				DataObject dataObject = new DataObject();
				if (comicListItem is ComicSmartListItem)
				{
					dataObject.SetText(comicListItem.ToString());
				}
				dataObject.SetData(ShareableComicListItem.ClipboardFormat, comicListItem);
				Clipboard.SetDataObject(dataObject);
			}
			catch (Exception)
			{
			}
		}

		private void ExportList()
		{
			if (tvQueries.SelectedNode == null)
			{
				return;
			}
			ShareableComicListItem shareableComicListItem = tvQueries.SelectedNode.Tag as ShareableComicListItem;
			if (shareableComicListItem == null)
			{
				return;
			}
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				saveFileDialog.Title = miExportReadingList.Text.Replace("&", "");
				saveFileDialog.Filter = TR.Load("FileFilter")["ReadingListSaveFilter", "ComicRack Reading List|*.cbl|ComicRack Reading List (Single Entries)|*.cbl"];
				saveFileDialog.DefaultExt = ".cbl";
				saveFileDialog.FileName = FileUtility.MakeValidFilename(shareableComicListItem.Name);
				foreach (string favoritePath in Program.GetFavoritePaths())
				{
					saveFileDialog.CustomPlaces.Add(favoritePath);
				}
				if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
				{
					try
					{
						new ComicReadingListContainer(shareableComicListItem, Program.Settings.ExportedListsContainFilenames, saveFileDialog.FilterIndex != 1).Serialize(saveFileDialog.FileName);
					}
					catch
					{
						MessageBox.Show(StringUtility.Format(TR.Messages["ErrorWritingReadingList", "There was an error exporting the Reading List '{0}'"], Path.GetFileName(saveFileDialog.FileName)));
					}
				}
			}
		}

		private bool isPasteListEnabled()
		{
            //Check if the clipboard contains data, if so enable the paste button.
            try
            {
                return (Clipboard.ContainsData(ShareableComicListItem.ClipboardFormat) || Clipboard.ContainsText()) && ComicEditMode.CanEditList();
            }
            catch (Exception)
            {
                return false;
            }
        }

		private void PasteList()
		{
			if (!isPasteListEnabled())
				return;

			ComicListItem currentNodeComicList = GetCurrentNodeComicList();
			ComicListItemCollection currentNodeComicListCollection = GetCurrentNodeComicListCollection();
			if (currentNodeComicListCollection == null)
			{
				return;
			}
			ShareableComicListItem shareableComicListItem = Clipboard.GetData(ShareableComicListItem.ClipboardFormat) as ShareableComicListItem;
			if (shareableComicListItem != null)
			{
				shareableComicListItem = ((ICloneable)shareableComicListItem).Clone<ShareableComicListItem>();
				if (shareableComicListItem != null)
				{
					currentNodeComicListCollection.Insert(currentNodeComicListCollection.IndexOf(currentNodeComicList) + 1, shareableComicListItem);
				}
				return;
			}
			if (Program.ExtendedSettings.AllowCopyListFolders)
			{
				ComicListItemFolder comicListItemFolder = Clipboard.GetData(ShareableComicListItem.ClipboardFormat) as ComicListItemFolder;
				if (comicListItemFolder != null)
				{
					comicListItemFolder = ((ICloneable)comicListItemFolder).Clone<ComicListItemFolder>();
					if (comicListItemFolder != null)
					{
						currentNodeComicListCollection.Insert(currentNodeComicListCollection.IndexOf(currentNodeComicList) + 1, comicListItemFolder);
					}
					return;
				}
			}
			string text = Clipboard.GetText();
			try
			{
				ComicSmartListItem item = new ComicSmartListItem(TR.Load(base.Name)["NewList", "New List"], text, Library);
				currentNodeComicListCollection.Insert(currentNodeComicListCollection.IndexOf(currentNodeComicList) + 1, item);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, TR.Messages["ErrorPasteQuery", "Could not paste List because of following error:"] + "\n\n" + ex.Message, TR.Messages["Attention", "Attention"], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void ImportLists()
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Title = miImportReadingList.Text.Replace("&", "");
				openFileDialog.Filter = TR.Load("FileFilter")["ReadingListLoad", "ComicRack Reading List|*.cbl|Xml File|*.xml|All Files|*.*"];
				openFileDialog.CheckFileExists = true;
				openFileDialog.Multiselect = true;
				foreach (string favoritePath in Program.GetFavoritePaths())
				{
					openFileDialog.CustomPlaces.Add(favoritePath);
				}
				if (openFileDialog.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				using (new WaitCursor(this))
				{
					string[] fileNames = openFileDialog.FileNames;
					foreach (string file in fileNames)
					{
						ImportList(GetCurrentNodeComicListCollection(), file);
					}
				}
			}
		}

		private void RemoveListOrFolder()
		{
			TreeNode selectedNode = tvQueries.SelectedNode;
			if (selectedNode != null && !selectedNode.IsEditing)
			{
				ComicListItemCollection nodeComicListCollection = GetNodeComicListCollection((selectedNode.Tag is ComicListItemFolder) ? selectedNode.Parent : selectedNode);
				if (nodeComicListCollection != null && Program.AskQuestion(this, TR.Messages["AskRemoveItem", "Do you really want to remove this item?"], TR.Messages["Remove", "Remove"], HiddenMessageBoxes.RemoveList) && nodeComicListCollection.Remove(selectedNode.Tag as ComicListItem) && selectedNode.Parent != null)
				{
					tvQueries.SelectedNode = selectedNode.Parent;
				}
			}
		}

		public void SetWorkspace(DisplayWorkspace ws)
		{
			quickSearch.AutoCompleteList.AddRange(Program.Settings.LibraryQuickSearchList.ToArray());
		}

		public void StoreWorkspace(DisplayWorkspace ws)
		{
			if (!base.Disposing && quickSearch != null)
			{
				try
				{
					HashSet<string> collection = new HashSet<string>(quickSearch.AutoCompleteList.Cast<string>());
					Program.Settings.LibraryQuickSearchList.Clear();
					Program.Settings.LibraryQuickSearchList.AddRange(collection);
				}
				catch
				{
				}
			}
			if (ComicEditMode.CanEditList())
			{
				Program.Settings.LastLibraryItem = base.BookListId;
			}
		}

		public ComicListItem ImportList(string file)
		{
			return ImportList(null, file);
		}

		public ComicListItem ImportList(ComicListItemCollection fc, string file)
		{
			using (new WaitCursor(this))
			{
				try
				{
					ComicReadingListContainer crlc = ComicReadingListContainer.Deserialize(file);
					ComicListItem li = null;
					if (crlc.Matchers.Count > 0)
					{
						li = new ComicSmartListItem(crlc.Name ?? string.Empty, crlc.MatcherMode, crlc.Matchers);
					}
					else
					{
						List<ComicBook> newBooks = new List<ComicBook>();
						ComicIdListItem idli = null;
						AutomaticProgressDialog.Process(this, TR.Messages["ImportReadingList", "Import Reading List"], TR.Messages["MatchBooksWithLibrary", "Matching list with Library"], 3000, delegate
						{
							li = (idli = ComicIdListItem.CreateFromReadingList(Library.Books, crlc.Items, newBooks, delegate(int x)
							{
								AutomaticProgressDialog.Value = x;
								return !AutomaticProgressDialog.ShouldAbort;
							}));
						}, AutomaticProgressDialogOptions.EnableCancel);
						if (li == null)
						{
							return null;
						}
						li.Name = crlc.Name ?? string.Empty;
						if (newBooks.Count > 0)
						{
							string format = TR.Messages["UnsolvedBookItems", "The following Books were not found in the Library:\n{0}\nDo you still want to import the Reading List?"];
							string str = string.Empty;
							for (int i = 0; i < Math.Min(newBooks.Count, 25); i++)
							{
								if (i != 0)
								{
									str += "\n";
								}
								str += $"'{newBooks[i].Caption}'";
							}
							if (newBooks.Count > 25)
							{
								str += "\n...";
							}
							str += "\n";
							switch (QuestionDialog.AskQuestion(this, StringUtility.Format(format, str), TR.Default["Import", "Import"], TR.Messages["CreateMissingBooks", "Add missing Books to Library"]))
							{
							case var type when type.HasFlag(QuestionResult.Cancel):
								return null;
							case QuestionResult.OkWithOption:
								Library.Books.AddRange(newBooks);
								break;
							default:
								idli.BookIds.RemoveRange(newBooks.Select((ComicBook cb) => cb.Id));
								break;
							}
						}
					}
					(fc ?? Library.TemporaryFolder.Items).Add(li);
					FillListTree();
					SelectList(li.Id);
					return li;
				}
				catch
				{
					MessageBox.Show(StringUtility.Format(TR.Messages["ErrorOpeningReadingList", "There was an error importing the Reading List '{0}'"], Path.GetFileName(file)));
					return null;
				}
			}
		}

		public bool SelectList(Guid listId)
		{
			TreeNode treeNode = FindItemNode(listId);
			if (treeNode == null)
			{
				return false;
			}
			tvQueries.SelectedNode = treeNode;
			UpdateBookList();
			return true;
		}

		public bool CanBrowsePrevious()
		{
			return history.CanMoveCursorPrevious;
		}

		public bool CanBrowseNext()
		{
			return history.CanMoveCursorNext;
		}

		public void BrowsePrevious()
		{
			while (history.CanMoveCursorPrevious)
			{
				LinkedListNode<IComicBookListProvider> linkedListNode = history.MoveCursorPrevious();
				TreeNode treeNode = ((linkedListNode != null) ? FindItemNode(linkedListNode.Value) : null);
				if (treeNode != null)
				{
					tvQueries.SelectedNode = treeNode;
					UpdateBookList();
					break;
				}
			}
		}

		public void BrowseNext()
		{
			while (history.CanMoveCursorNext)
			{
				LinkedListNode<IComicBookListProvider> linkedListNode = history.MoveCursorNext();
				TreeNode treeNode = ((linkedListNode != null) ? FindItemNode(linkedListNode.Value) : null);
				if (treeNode != null)
				{
					tvQueries.SelectedNode = treeNode;
					UpdateBookList();
					break;
				}
			}
		}

		private void library_ListCachesUpdated(object sender, EventArgs e)
		{
			QueueListCacheChange();
		}

		private void queryCacheTimer_Tick(object sender, EventArgs e)
		{
			queryCacheTimer.Stop();
			CommitListCacheChange();
		}

		private void OnListCacheChanged()
		{
			if (ComicLibrary.IsQueryCacheEnabled)
			{
				queryCacheTimer.Stop();
				queryCacheTimer.Start();
			}
		}

		private void QueueListCacheChange()
		{
			if (ComicLibrary.IsQueryCacheEnabled)
			{
				this.AddIdleAction(OnListCacheChanged, "QueueListCacheChange");
			}
		}

		private void CommitListCacheChange()
		{
			if (base.Visible)
			{
				Library.CommitComicListCacheChanges((ComicListItem cli) => FindItemNode(cli)?.IsVisible ?? false);
			}
		}
	}
}
