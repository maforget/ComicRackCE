using System;

namespace cYo.Common.Windows.Forms
{
	public class ItemViewColumnHeaderClickEventArgs : EventArgs
	{
		public IColumn Header
		{
			get;
			private set;
		}

		public ItemViewColumnHeaderClickEventArgs(IColumn header)
		{
			Header = header;
		}
	}
}
