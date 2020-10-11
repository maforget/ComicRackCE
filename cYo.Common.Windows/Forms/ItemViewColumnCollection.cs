using System.Collections.Generic;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;

namespace cYo.Common.Windows.Forms
{
	public class ItemViewColumnCollection<T> : SmartList<T> where T : IColumn
	{
		public T FindById(int id)
		{
			return Find((T h) => h.Id == id);
		}

		public T FindBySorter(IComparer<IViewableItem> comp)
		{
			if (comp != null)
			{
				return Find((T h) => h.ColumnSorter == comp);
			}
			return default(T);
		}

		public T FindByGrouper(IGrouper<IViewableItem> comp)
		{
			if (comp != null)
			{
				return Find((T h) => h.ColumnGrouper == comp);
			}
			return default(T);
		}
	}
}
