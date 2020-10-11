using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public class AutoSizeTextBox : TextBox
	{
		private int autoSizePadding = 16;

		private bool autoSizeEnabled = true;

		[DefaultValue(16)]
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

		[DefaultValue(false)]
		public bool HandleTab
		{
			get;
			set;
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			if (autoSizeEnabled)
			{
				base.Width = TextRenderer.MeasureText(Text, Font).Width + autoSizePadding;
			}
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (HandleTab && (keyData & Keys.KeyCode) == Keys.Tab)
			{
				KeyEventArgs keyEventArgs = new KeyEventArgs(keyData);
				OnKeyDown(keyEventArgs);
				if (keyEventArgs.Handled)
				{
					return true;
				}
			}
			return base.ProcessDialogKey(keyData);
		}
	}
}
