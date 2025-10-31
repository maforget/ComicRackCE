using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using cYo.Common.Drawing;
using cYo.Common.Windows.Forms.Theme.Resources;
using cYo.Common.Windows.Properties;

namespace cYo.Common.Windows.Forms
{
	public class NiceTreeSkin : TreeViewSkinner
	{
		private readonly Bitmap down = Resources.SimpleArrowDown;

		private readonly Bitmap right = Resources.SimpleArrowRight;

		public NiceTreeSkin()
		{
		}

		public NiceTreeSkin(TreeView tv)
			: base(tv)
		{
		}

		protected override void DrawNodeBackground(TreeViewSkinnerDrawInfo di)
		{
			Color selectionColor = StyledRenderer.GetSelectionColor(base.TreeView.Focused);
			if (di.HasState(TreeNodeStates.Selected))
			{
				di.Graphics.DrawStyledRectangle(di.ItemBounds, StyledRenderer.GetAlphaStyle(di.HasState(TreeNodeStates.Selected), di.HasState(TreeNodeStates.Hot), di.HasState(TreeNodeStates.Focused)), selectionColor);
			}
			if (di.Node == base.DropNode)
			{
				if (!base.SeparatorDropNodeStyle)
				{
					di.Graphics.DrawStyledRectangle(di.ItemBounds, StyledRenderer.AlphaStyle.SelectedHot, selectionColor);
					return;
				}
				Rectangle itemBounds = di.ItemBounds;
				itemBounds.Height = 2;
				di.Graphics.FillRectangle(ThemeBrushes.NiceTreeSkin.Separator, itemBounds);
			}
		}

		protected override void DrawNodeContent(TreeViewSkinnerDrawInfo di)
		{
			ImageList imageList = base.TreeView.ImageList;
			Point point = new Point(di.LabelBounds.X, di.ItemBounds.Y);
			if (imageList != null)
			{
				string text = (di.HasState(TreeNodeStates.Selected) ? di.Node.SelectedImageKey : di.Node.ImageKey);
				int index = (di.HasState(TreeNodeStates.Selected) ? di.Node.SelectedImageIndex : di.Node.ImageIndex);
				using (Image image = (string.IsNullOrEmpty(text) ? imageList.Images[index] : imageList.Images[text]))
				{
					if (image != null)
					{
						point.X -= imageList.ImageSize.Width - 2;
						DrawNodeIcon(di, image, new Rectangle(point.X, point.Y + (di.ItemBounds.Height - image.Height) / 2, image.Width, image.Height));
					}
				}
			}
			if (base.TreeView.CheckBoxes)
			{
				int height = di.LabelBounds.Height;
				int num = height - FormUtility.ScaleDpiY(6);
				point.X -= num + 6;
				Rectangle rectangle = new Rectangle(point.X, point.Y + (height - num) / 2, num, num);
				ButtonState buttonState = ButtonState.Flat;
				if (di.HasState(TreeNodeStates.Checked))
				{
					buttonState |= ButtonState.Checked;
				}
				if (di.HasState(TreeNodeStates.Grayed))
				{
					buttonState |= ButtonState.Inactive;
				}
				ControlPaint.DrawCheckBox(di.Graphics, rectangle, buttonState);
			}
			if (di.Node.Nodes.Count != 0 && base.TreeView.ShowPlusMinus)
			{
				Image image2 = (di.Node.IsExpanded ? down : right);
				Size size = image2.Size.ScaleDpi();
				point.X -= size.Width - 1;
				di.Graphics.DrawImage(image2, point.X, point.Y + (di.ItemBounds.Height - size.Height) / 2, size.Width, size.Height);
			}
		}

		protected virtual void DrawNodeIcon(TreeViewSkinnerDrawInfo di, Image image, Rectangle bounds)
		{
			di.Graphics.DrawImage(image, bounds);
		}

		protected override void DrawNodeLabel(TreeViewSkinnerDrawInfo di)
		{
			Color foreColor = (di.HasState(TreeNodeStates.Grayed) ? SystemColors.GrayText : SystemColors.WindowText);
			di.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
			TextRenderer.DrawText(di.Graphics, di.Node.Text, di.HasState(TreeNodeStates.Selected) ? FC.Get(di.Font, FontStyle.Bold) : di.Font, di.LabelBounds, foreColor, TextFormatFlags.NoClipping | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.PreserveGraphicsTranslateTransform);
			di.LabelBounds = new Rectangle(width: TextRenderer.MeasureText(di.Graphics, di.Node.Text, di.HasState(TreeNodeStates.Selected) ? FC.Get(di.Font, FontStyle.Bold) : di.Font, di.LabelBounds.Size, TextFormatFlags.NoClipping | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.PreserveGraphicsTranslateTransform).Width, x: di.LabelBounds.X, y: di.LabelBounds.Y, height: di.LabelBounds.Height);
		}

		protected override void DrawNodeFrame(TreeViewSkinnerDrawInfo di)
		{
		}
	}
}
