using System.Windows.Forms;
using cYo.Common.ComponentModel;

namespace cYo.Common.Windows.Forms
{
	public class WaitCursor : DisposableObject
	{
		private readonly Form form;

		public WaitCursor(Form form)
		{
			this.form = form;
			if (form == null)
			{
				Application.UseWaitCursor = true;
			}
			else
			{
				form.UseWaitCursor = true;
			}
			Cursor.Current = Cursors.WaitCursor;
		}

		public WaitCursor(Control c)
			: this(GetForm(c))
		{
		}

		public WaitCursor()
			: this(null)
		{
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Cursor.Current = Cursors.Default;
				if (form == null)
				{
					Application.UseWaitCursor = false;
				}
				else
				{
					form.UseWaitCursor = false;
				}
			}
		}

		private static Form GetForm(Control c)
		{
			if (c == null)
			{
				return null;
			}
			Form form = c as Form;
			return form ?? GetForm(c.Parent);
		}
	}
}
