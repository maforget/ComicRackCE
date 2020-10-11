using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public class TreeViewSkinnerDrawInfo
	{
		public Graphics Graphics
		{
			get;
			set;
		}

		public Font Font
		{
			get;
			set;
		}

		public Rectangle ItemBounds
		{
			get;
			set;
		}

		public Rectangle LabelBounds
		{
			get;
			set;
		}

		public TreeNodeStates State
		{
			get;
			set;
		}

		public TreeNode Node
		{
			get;
			set;
		}

		public TreeViewSkinnerDrawInfo(Graphics graphics, Rectangle itemBounds, Rectangle labelBounds, TreeNode node, TreeNodeStates state, Font font)
		{
			Graphics = graphics;
			ItemBounds = itemBounds;
			LabelBounds = labelBounds;
			State = state;
			Node = node;
			Font = font;
		}

		public bool HasState(TreeNodeStates treeNodeStates)
		{
			return (State & treeNodeStates) != 0;
		}
	}
}
