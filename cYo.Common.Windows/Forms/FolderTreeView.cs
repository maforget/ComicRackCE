using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.Win32;
using cYo.Common.Windows.Forms.Theme;

namespace cYo.Common.Windows.Forms
{
	public class FolderTreeView : TreeViewEx
	{
		public static class NativeMethods
		{
			[DllImport("Shell32.dll", CharSet = CharSet.Auto)]
			private static extern uint ExtractIconEx(string lpszFile, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);

			public static Icon GetDesktopIcon()
			{
				IntPtr[] phiconLarge = new IntPtr[1];
				IntPtr[] array = new IntPtr[1];
				ExtractIconEx(Environment.SystemDirectory + "\\shell32.dll", 34, phiconLarge, array, 1u);
				return Icon.FromHandle(array[0]);
			}
		}

		public static class ShellOperations
		{
			public static Image GetImage(ShellPidl item, bool selected)
			{
				return item.GetImage(ShellIconType.Directory | (selected ? ShellIconType.Open : ShellIconType.None));
			}

			public static string GetFilePath(TreeNode tn)
			{
				try
				{
					ShellFolder shellFolder = (ShellFolder)tn.Tag;
					if (Directory.Exists(shellFolder.Pidl.PhysicalPath))
					{
						return shellFolder.Pidl.PhysicalPath;
					}
				}
				catch (Exception)
				{
				}
				return string.Empty;
			}

			public static void PopulateTree(TreeView tree, ImageList imageList, bool sortNetworkFolders)
			{
				ClearTree(tree);
				try
				{
					//TODO: Empty on new Windows 11 install if OneDrive installed, Top node shows OneDrive/Desktop
					ShellFolder shellFolder = new ShellFolder(Environment.SpecialFolder.Desktop);
					TreeNode treeNode = new TreeNode(shellFolder.Pidl.DisplayName, 0, 0)
					{
						Tag = shellFolder
					};
					tree.Nodes.Add(treeNode);
					FillNode(treeNode, imageList, sortNetworkFolders);
				}
				catch (Exception)
				{
				}
				if (tree.Nodes.Count > 1)
				{
					tree.SelectedNode = tree.Nodes[1];
					ExpandBranch(tree.Nodes[1], imageList, sortNetworkFolders);
				}
			}

			public static void ExpandBranch(TreeNode tn, ImageList imageList, bool sortNetworkFolders)
			{
				if (tn.Nodes.Count == 1 && tn.Nodes[0].Tag == null)
				{
					tn.Nodes.Clear();
					FillNode(tn, imageList, sortNetworkFolders);
				}
			}

			[DllImport("shlwapi.dll")]
			private static extern bool PathIsNetworkPath(string pszPath);

			public static void FillNode(TreeNode tn, ImageList imageList, bool sortNetworkFolders)
			{
				try
				{
					ShellFolder shellFolder = (ShellFolder)tn.Tag;
					List<ShellPidl> children = shellFolder.GetChildren(showHiddenObjects: false, showNonFolders: false, optimized: true);
					try
					{
						List<TreeNode> list = new List<TreeNode>();
						foreach (ShellPidl item3 in children)
						{
							if (!item3.IsBrowsable && !item3.IsNetwork && !item3.IsControlPanel && !item3.IsRecycleBin && (!string.IsNullOrEmpty(item3.PhysicalPath) || item3.HasSubfolders))
							{
								ShellFolder item = new ShellFolder(item3);
								TreeNode item2 = AddTreeNode(item, imageList, getIcons: true);
								list.Add(item2);
							}
						}
						if (sortNetworkFolders && PathIsNetworkPath(shellFolder.Pidl.PhysicalPath) 
							|| shellFolder.Pidl.IsFileSystem && !shellFolder.Pidl.IsDesktop && !PathIsNetworkPath(shellFolder.Pidl.PhysicalPath))
						{
							list.Sort((TreeNode a, TreeNode b) => string.Compare(a.Text, b.Text, StringComparison.InvariantCultureIgnoreCase));
						}
						foreach (TreeNode item4 in list)
						{
							tn.Nodes.Add(item4);
							CheckForSubDirs(item4);
						}
					}
					finally
					{
						children.ForEach(delegate(ShellPidl p)
						{
							p.Dispose();
						});
					}
				}
				catch
				{
				}
			}

			private static bool CheckForSubDirs(TreeNode tn)
			{
				if (tn.Nodes.Count != 0)
				{
					return true;
				}
				try
				{
					ShellFolder shellFolder = tn.Tag as ShellFolder;
					if (shellFolder != null && shellFolder.Pidl.HasSubfolders)
					{
						TreeNode node = new TreeNode
						{
							Tag = null
						};
						tn.Nodes.Add(node);
						return true;
					}
				}
				catch
				{
				}
				return false;
			}

			private static TreeNode AddTreeNode(ShellFolder item, ImageList imageList, bool getIcons)
			{
				TreeNode treeNode = new TreeNode
				{
					Text = item.Pidl.DisplayName,
					Tag = item
				};
				if (getIcons)
				{
					try
					{
						treeNode.ImageKey = item.Pidl.IconKey;
						treeNode.SelectedImageKey = treeNode.ImageKey + "S";
						if (!imageList.Images.ContainsKey(item.Pidl.IconKey))
						{
							Image image = GetImage(item.Pidl, selected: false);
							imageList.Images.Add(treeNode.ImageKey, image);
							image.Dispose();
							Image image2 = GetImage(item.Pidl, selected: true);
							imageList.Images.Add(treeNode.SelectedImageKey, image2);
							image2.Dispose();
						}
						return treeNode;
					}
					catch
					{
					}
				}
				treeNode.ImageIndex = 1;
				treeNode.SelectedImageIndex = 2;
				return treeNode;
			}
		}

		private readonly ImageList myImageList;

		[DefaultValue(true)]
		public bool SortNetworkFolders
		{
			get;
			set;
		}

		public FolderTreeView()
		{
			SortNetworkFolders = true;
			myImageList = new ImageList
			{
				ColorDepth = ColorDepth.Depth32Bit,
				ImageSize = new Size(16, 16),
				TransparentColor = Color.Transparent
			};
			myImageList.ImageSize = myImageList.ImageSize.ScaleDpi();
			myImageList.Images.Add(NativeMethods.GetDesktopIcon());
			base.ImageList = myImageList;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				ClearTree(this);
				myImageList.Dispose();
			}
			base.Dispose(disposing);
		}

		public void Init()
		{
			ClearTree(this);
			ShellOperations.PopulateTree(this, base.ImageList, SortNetworkFolders);
			if (base.Nodes.Count > 0)
			{
				base.Nodes[0].Expand();
			}
			this.SetSidePanelColor();
		}

		private static void ClearTree(TreeView tree)
		{
			(from tn in tree.AllNodes()
				select tn.Tag).OfType<IDisposable>().ForEach(delegate(IDisposable d)
			{
				d.Dispose();
			});
			tree.Nodes.Clear();
		}

		protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
		{
			BeginUpdate();
			try
			{
				ShellOperations.ExpandBranch(e.Node, base.ImageList, SortNetworkFolders);
			}
			finally
			{
				EndUpdate();
			}
		}

		protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
		{
			BeginUpdate();
			try
			{
				ShellOperations.ExpandBranch(e.Node, base.ImageList, SortNetworkFolders);
			}
			finally
			{
				EndUpdate();
			}
		}

		public string GetSelectedNodePath()
		{
			if (base.SelectedNode != null)
			{
				return ShellOperations.GetFilePath(base.SelectedNode);
			}
			return string.Empty;
		}

		public bool DrillToFolder(string folderPath)
		{
			bool folderFound = false;
			try
			{
				if (Directory.Exists(folderPath))
				{
					if (folderPath.Length > 3 && folderPath.LastIndexOf("\\") == folderPath.Length - 1)
					{
						folderPath = folderPath.Substring(0, folderPath.Length - 1);
					}
					DrillTree(base.Nodes[0].Nodes, folderPath, ref folderFound);
				}
			}
			catch
			{
			}
			if (!folderFound)
			{
				base.SelectedNode = base.Nodes[0];
			}
			return folderFound;
		}

		private void DrillTree(TreeNodeCollection tnc, string path, ref bool folderFound)
		{
			if (path == null)
			{
				return;
			}
			foreach (TreeNode item in tnc)
			{
				if (folderFound)
				{
					break;
				}
				string filePath = ShellOperations.GetFilePath(item);
				if (string.Equals(path, filePath, StringComparison.OrdinalIgnoreCase))
				{
					base.SelectedNode = item;
					item.EnsureVisible();
					folderFound = true;
				}
				else if (string.IsNullOrEmpty(filePath) || path.StartsWith(filePath, StringComparison.OrdinalIgnoreCase))
				{
					item.Expand();
					DrillTree(item.Nodes, path, ref folderFound);
				}
			}
		}
	}
}
