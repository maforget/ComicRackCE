using System;

namespace cYo.Common.Collections
{
	public class SmartListChangedEventArgs<T> : EventArgs
	{
		private readonly T item;

		private readonly T oldItem;

		private readonly int index;

		private readonly SmartListAction action;

		public T Item => item;

		public T OldItem => oldItem;

		public int Index => index;

		public SmartListAction Action => action;

		public SmartListChangedEventArgs(SmartListAction action, int index, T item, T oldItem)
		{
			this.action = action;
			this.index = index;
			this.item = item;
			this.oldItem = oldItem;
		}
	}
}
