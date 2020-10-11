using System;

namespace cYo.Common.Collections
{
	[Serializable]
	public class MruList<T> : SmartList<T>
	{
		public int MaxCount
		{
			get;
			set;
		}

		public MruList(int maxCount)
		{
			MaxCount = maxCount;
		}

		public MruList()
			: this(20)
		{
		}

		public void UpdateMostRecent(T item)
		{
			while (Remove(item))
			{
			}
			Insert(0, item);
			Trim(MaxCount);
		}
	}
}
