using System;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public class DropDownHost<T> : ToolStripDropDown where T : Control, new()
	{
		private readonly T control = new T();

		public T Control => control;

		public DropDownHost()
		{
			Items.Add(new ToolStripControlHost(control));
		}

		protected override void OnOpened(EventArgs e)
		{
			base.OnOpened(e);
			control.Focus();
		}
	}
}
