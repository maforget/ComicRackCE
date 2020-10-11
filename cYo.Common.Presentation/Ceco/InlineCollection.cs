using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace cYo.Common.Presentation.Ceco
{
	public class InlineCollection : Collection<Inline>
	{
		public event CollectionChangeEventHandler Changed;

		public void AddRange(IEnumerable<Inline> items)
		{
			foreach (Inline item in items)
			{
				Add(item);
			}
		}

		protected override void InsertItem(int index, Inline item)
		{
			base.InsertItem(index, item);
			OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, item));
		}

		protected override void RemoveItem(int index)
		{
			Inline element = base[index];
			base.RemoveItem(index);
			OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, element));
		}

		protected override void SetItem(int index, Inline item)
		{
			Inline element = base[index];
			base.SetItem(index, item);
			OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, element));
			OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, item));
		}

		protected override void ClearItems()
		{
			using (IEnumerator<Inline> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Inline current = enumerator.Current;
					OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, current));
				}
			}
			base.ClearItems();
			OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
		}

		protected virtual void OnCollectionChanged(CollectionChangeEventArgs e)
		{
			if (this.Changed != null)
			{
				this.Changed(this, e);
			}
		}
	}
}
