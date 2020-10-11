using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Drawing;
using cYo.Common.Win32;

namespace cYo.Common.Windows.Forms
{
	public class TreeViewSkinner : Component
	{
		private TreeNode dropNode;

		private bool separatorDropNodeStyle;

		private TreeView treeView;

		public TreeNode DropNode
		{
			get
			{
				return dropNode;
			}
			set
			{
				if (dropNode != value)
				{
					InvalidateNode(dropNode);
					dropNode = value;
					InvalidateNode(dropNode);
				}
			}
		}

		public bool SeparatorDropNodeStyle
		{
			get
			{
				return separatorDropNodeStyle;
			}
			set
			{
				if (separatorDropNodeStyle != value)
				{
					separatorDropNodeStyle = value;
					InvalidateNode(dropNode);
				}
			}
		}

		public TreeView TreeView
		{
			get
			{
				return treeView;
			}
			set
			{
				if (treeView != value)
				{
					if (treeView != null)
					{
						treeView.DrawMode = TreeViewDrawMode.Normal;
						treeView.DrawNode -= OnDrawNode;
						treeView.Disposed -= TreeViewDisposed;
					}
					treeView = value;
					if (treeView != null)
					{
						treeView.DrawMode = TreeViewDrawMode.OwnerDrawAll;
						treeView.ItemHeight = treeView.Font.Height + FormUtility.ScaleDpiY(8);
						treeView.DrawNode += OnDrawNode;
						treeView.Disposed += TreeViewDisposed;
					}
				}
			}
		}

		public TreeViewSkinner(TreeView tv)
		{
			TreeView = tv;
		}

		public TreeViewSkinner()
			: this(null)
		{
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				TreeView = null;
			}
			base.Dispose(disposing);
		}

		private void OnDrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			try
			{
				if (!e.Bounds.IsEmpty && !e.Node.Bounds.IsEmpty)
				{
					TreeNodeStates treeNodeStates = e.State;
					if (e.Node.Checked)
					{
						treeNodeStates |= TreeNodeStates.Checked;
					}
					Graphics graphics = e.Graphics;
					using (graphics.SaveState())
					{
						Rectangle bounds = e.Bounds;
						Rectangle bounds2 = e.Node.Bounds;
						bounds2.Offset(-e.Bounds.X, -e.Bounds.Y);
						bounds.Offset(-e.Bounds.X, -e.Bounds.Y);
						graphics.IntersectClip(e.Bounds);
						graphics.TranslateTransform(e.Bounds.X, e.Bounds.Y);
						bounds.Width--;
						bounds.Height--;
						TreeViewSkinnerDrawInfo treeViewSkinnerDrawInfo = new TreeViewSkinnerDrawInfo(graphics, bounds, bounds2, e.Node, treeNodeStates, treeView.Font);
						treeViewSkinnerDrawInfo.Graphics.Clear(TreeView.BackColor);
						DrawNode(treeViewSkinnerDrawInfo);
					}
				}
			}
			catch (Exception)
			{
				e.DrawDefault = true;
			}
		}

		protected virtual void DrawNodeBackground(TreeViewSkinnerDrawInfo di)
		{
		}

		protected virtual void DrawNodeContent(TreeViewSkinnerDrawInfo di)
		{
		}

		protected virtual void DrawNodeCheckBox(TreeViewSkinnerDrawInfo di)
		{
		}

		protected virtual void DrawNodeLabel(TreeViewSkinnerDrawInfo di)
		{
		}

		protected virtual void DrawNodeFrame(TreeViewSkinnerDrawInfo di)
		{
		}

		protected virtual void DrawNode(TreeViewSkinnerDrawInfo di)
		{
			if (di.ItemBounds.Width != 0 && di.ItemBounds.Height != 0)
			{
				DrawNodeBackground(di);
				DrawNodeContent(di);
				if (!di.Node.IsEditing)
				{
					DrawNodeLabel(di);
				}
				DrawNodeFrame(di);
			}
		}

		private void TreeViewDisposed(object sender, EventArgs e)
		{
			Dispose();
		}

		public static void InvalidateNode(TreeNode node)
		{
			if (node != null)
			{
				Color backColor = node.BackColor;
				node.BackColor = Color.Gainsboro;
				node.BackColor = backColor;
			}
		}

		public Bitmap GetBitmap(TreeNode node)
		{
			Rectangle bounds = node.Bounds;
			Rectangle itemBounds = new Rectangle(0, 0, treeView.ClientRectangle.Width, node.Bounds.Height);
			bounds.Y = 0;
			Bitmap bitmap = new Bitmap(itemBounds.Width, itemBounds.Height);
			try
			{
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					TreeNodeStates treeNodeStates = (TreeNodeStates)0;
					if (node.IsSelected)
					{
						treeNodeStates |= TreeNodeStates.Selected;
					}
					if (node.Checked)
					{
						treeNodeStates |= TreeNodeStates.Checked;
					}
					DrawNode(new TreeViewSkinnerDrawInfo(graphics, itemBounds, bounds, node, treeNodeStates, treeView.Font));
					return bitmap;
				}
			}
			catch
			{
				return bitmap;
			}
		}

		public IBitmapCursor GetDragCursor(TreeNode node, byte alpha, Point cursorLocation)
		{
			IBitmapCursor bitmapCursor = BitmapCursor.Create(GetBitmap(node));
			if (bitmapCursor != null)
			{
				cursorLocation.Offset(0, -node.Bounds.Y);
				bitmapCursor.BitmapOwned = true;
				bitmapCursor.HotSpot = cursorLocation;
				if (bitmapCursor.Bitmap != null)
				{
					bitmapCursor.Bitmap.ChangeAlpha(alpha);
				}
			}
			return bitmapCursor;
		}
	}
}
