using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public class AutoSizeComboBox : ComboBox
	{
		private int autoSizePadding = 24;

		private bool autoSizeEnabled = true;

		[DefaultValue(24)]
		public int AutoSizePadding
		{
			get
			{
				return autoSizePadding;
			}
			set
			{
				autoSizePadding = value;
			}
		}

		[DefaultValue(true)]
		public bool AutoSizeEnabled
		{
			get
			{
				return autoSizeEnabled;
			}
			set
			{
				autoSizeEnabled = value;
			}
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			if (autoSizeEnabled)
			{
				base.Width = TextRenderer.MeasureText(Text, Font).Width + autoSizePadding;
			}
		}
	}
}
