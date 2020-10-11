using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public class WrappingCheckBox : CheckBox
	{
		private Size cachedSizeOfOneLineOfText = Size.Empty;

		private readonly Dictionary<Size, Size> preferredSizeHash = new Dictionary<Size, Size>(3);

		protected override void OnAutoSizeChanged(EventArgs e)
		{
			base.OnAutoSizeChanged(e);
			CacheTextSize();
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			CacheTextSize();
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			CacheTextSize();
		}

		private void CacheTextSize()
		{
			preferredSizeHash.Clear();
			cachedSizeOfOneLineOfText = (string.IsNullOrEmpty(Text) ? Size.Empty : TextRenderer.MeasureText(Text, Font, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.WordBreak));
		}

		public override Size GetPreferredSize(Size proposedSize)
		{
			Size size = base.GetPreferredSize(proposedSize);
			if (size.Width > proposedSize.Width && ((!string.IsNullOrEmpty(Text) && !proposedSize.Width.Equals(int.MaxValue)) || !proposedSize.Height.Equals(int.MaxValue)))
			{
				Size size2 = size - cachedSizeOfOneLineOfText;
				Size size3 = proposedSize - size2 - new Size(3, 0);
				if (!preferredSizeHash.ContainsKey(size3))
				{
					size = size2 + TextRenderer.MeasureText(Text, Font, size3, TextFormatFlags.WordBreak);
					preferredSizeHash[size3] = size;
				}
				else
				{
					size = preferredSizeHash[size3];
				}
			}
			return size;
		}
	}
}
