using System;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public class NullableDateTimePicker : DateTimePicker
	{
		private DateTimePickerFormat oldFormat = DateTimePickerFormat.Long;

		private string oldCustomFormat;

		private bool isNull;

		public new DateTime Value
		{
			get
			{
				if (!isNull)
				{
					return base.Value;
				}
				return DateTime.MinValue;
			}
			set
			{
				if (value < base.MinDate || value > base.MaxDate)
				{
					if (!isNull)
					{
						oldFormat = base.Format;
						oldCustomFormat = base.CustomFormat;
						isNull = true;
					}
					if (!base.DesignMode)
					{
						base.Format = DateTimePickerFormat.Custom;
						base.CustomFormat = " ";
					}
					return;
				}
				if (isNull)
				{
					if (!base.DesignMode)
					{
						base.Format = oldFormat;
						base.CustomFormat = oldCustomFormat;
					}
					isNull = false;
				}
				base.Value = value;
			}
		}

		protected override void OnCloseUp(EventArgs eventargs)
		{
			if (!base.DesignMode && Control.MouseButtons == MouseButtons.None && isNull)
			{
				base.Format = oldFormat;
				base.CustomFormat = oldCustomFormat;
				isNull = false;
			}
			base.OnCloseUp(eventargs);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.KeyCode == Keys.Delete)
			{
				Value = DateTime.MinValue;
			}
		}
	}
}
