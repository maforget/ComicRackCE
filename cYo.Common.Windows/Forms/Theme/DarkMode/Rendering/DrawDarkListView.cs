using cYo.Common.Drawing;
using cYo.Common.Windows.Forms.Theme.DarkMode.Resources;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms.Theme.DarkMode.Rendering;

internal static partial class DrawDarkListView
{
	/// <summary>
	/// Custom <see cref="DrawListViewColumnHeaderEventHandler"/>. Draws light <see cref="System.Windows.Forms.ColumnHeader"/> text on dark background.
	/// </summary>
	/// <remarks>
	/// <c>DrawDefault</c> is executed unless <see cref="ListView.View"/> is set to <see cref="View.Details"/>
	/// </remarks>
	internal static void ColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
	{
		if ((sender as ListView)?.View != View.Details)
		{
			e.DrawDefault = true;
			return;
		}

		e.DrawDefault = false;
		e.Graphics.FillRectangle(DarkBrushes.Header.Back, e.Bounds);

		int x = e.Bounds.Right - 2;
		int y1 = e.Bounds.Top;
		int y2 = e.Bounds.Bottom;
		e.Graphics.DrawLine(DarkPens.Header.Separator, x, y1, x, y2);

		// Draw the header text with custom color and font
		e.Graphics.DrawString(
			e.Header.Text,
			e.Font,
			DarkBrushes.Header.Text,
			e.Bounds,
			new StringFormat
			{
				Alignment = StringAlignment.Near, // left align text
				LineAlignment = StringAlignment.Center, // vertically center text
				Trimming = StringTrimming.EllipsisCharacter
			});
	}

	internal static void Item(object sender, DrawListViewItemEventArgs e)
	{
		if (e.Item.ListView.View != View.Details)
			e.DrawDefault = true;
		}

	internal static void SubItem(object sender, DrawListViewSubItemEventArgs e)
	{
		if (e.Item.ListView.View != View.Details || e.Item == null)
		{
			e.DrawDefault = true;
			return;
		}
		ListView listView = (ListView)sender;
		Rectangle bounds = e.Bounds;
		Rectangle textRect = bounds; // Updated based on whether we have images
		ImageList imageList = listView.SmallImageList;
		ImageList checkImageList = listView.StateImageList;

		Color backColor = e.Item.BackColor;
		Color textColor = e.Item.ForeColor;

		if (e.Item.Selected)
		{
			backColor = DarkColors.SelectedText.Highlight;
			textColor = SystemColors.HighlightText; // currently same as ForeColor
		}

		// Fill background
		using (var backBrush = new SolidBrush(backColor))
			e.Graphics.FillRectangle(backBrush, bounds);
		if (e.Item.Selected)
			ControlPaint.DrawBorder(e.Graphics, e.Bounds, DarkColors.SelectedText.Focus, ButtonBorderStyle.Solid);

		// Handle first column having images and/or checkBoxes
		if (e.ColumnIndex == 0)
		{
			int padCheck, padImage;
			padCheck = padImage = 4;
			if (listView.CheckBoxes)
			{
				System.Windows.Forms.VisualStyles.CheckBoxState state = e.Item.Checked ?
					System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal :
					System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;

				Size glyphSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, state); // Should be DPI-aware
				Size checkSize = checkImageList is not null ? checkImageList.ImageSize : glyphSize;
				Rectangle checkRect = checkSize.Align(bounds, ContentAlignment.MiddleLeft);
				checkRect.X += padCheck;
				textRect.X = padCheck = checkRect.Right; // padding space for checkbox

				CheckBoxRenderer.DrawCheckBox(e.Graphics, checkRect.TopLeft(), state); // Draw the checkbox
			}
			if (imageList != null)
			{
				Size imageSize = imageList.ImageSize;
				Rectangle imageRect = imageSize.Align(bounds, ContentAlignment.MiddleLeft);
				imageRect.X += padCheck + padImage; // small padding from left
				textRect.X = imageRect.Right; // Shift text based on imageList != null, whether this item has an image or not

				// Draw the image if there is one to draw
				int imageIndex = e.Item.ImageIndex;
				imageIndex = imageIndex < 0 && !string.IsNullOrEmpty(e.Item.ImageKey) ? imageList.Images.IndexOfKey(e.Item.ImageKey) : imageIndex;
				if (imageIndex >= 0 && imageIndex < imageList.Images.Count)
					imageList.Draw(e.Graphics, imageRect.X, imageRect.Y, imageRect.Width, imageRect.Height, imageIndex);
			}
		}

		textRect.X += 2;
		textRect.Width = bounds.Right - textRect.X;

		// Draw text
		TextRenderer.DrawText(
			e.Graphics,
			e.SubItem.Text,
			e.SubItem.Font,
			textRect,
			textColor,
			backColor,
			TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

		// Draw column divider
		if (e.ColumnIndex < listView.Columns.Count)
		{
			using (var pen = new Pen(Color.FromArgb(77, 77, 77)))
				e.Graphics.DrawLine(pen, bounds.Right - 2, bounds.Top, bounds.Right - 2, bounds.Bottom);
		}
	}
}
