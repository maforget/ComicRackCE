using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using cYo.Common.Threading;

namespace cYo.Common.Collections
{
	[Serializable]
	public class SmartList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection, IMoveable, ICloneable
	{
		private List<T> innerList = new List<T>();

		private volatile SmartListOptions flags;

		[NonSerialized]
		private ReaderWriterLockSlim slimLock;

		public SmartListOptions Flags
		{
			get
			{
				return flags;
			}
			set
			{
				flags = value;
			}
		}

		public T First
		{
			get
			{
				if (Count != 0)
				{
					return this[0];
				}
				return default(T);
			}
		}

		public T Last
		{
			get
			{
				if (Count != 0)
				{
					return this[Count - 1];
				}
				return default(T);
			}
		}

		public int Count
		{
			get
			{
				using (GetLock(write: false))
				{
					return innerList.Count;
				}
			}
		}

		public bool IsReadOnly => false;

		public T this[int index]
		{
			get
			{
				using (GetLock(write: false))
				{
					return innerList[index];
				}
			}
			set
			{
				T val = this[index];
				if ((flags & SmartListOptions.CheckedSet) == 0 || !object.Equals(val, value))
				{
					if ((flags & SmartListOptions.DisableOnInsert) == 0)
					{
						OnInsert(index, value);
					}
					if ((flags & SmartListOptions.DisableOnRemove) == 0)
					{
						OnRemove(index, val);
					}
					if ((flags & SmartListOptions.DisableOnSet) == 0)
					{
						OnSet(index, value, val);
					}
					using (GetLock(write: true))
					{
						innerList[index] = value;
					}
					if ((flags & SmartListOptions.DisableOnSet) == 0)
					{
						OnSetCompleted(index, value, val);
					}
					if ((flags & SmartListOptions.DisableOnRemove) == 0)
					{
						OnRemoveCompleted(index, val);
					}
					if ((flags & SmartListOptions.DisableOnInsert) == 0)
					{
						OnInsertCompleted(index, value);
					}
				}
			}
		}

		public bool IsSynchronized => (flags & SmartListOptions.Synchronized) != 0;

		public object SyncRoot => ((ICollection)innerList).SyncRoot;

		bool IList.IsFixedSize => false;

		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = (T)value;
			}
		}

		[field: NonSerialized]
		public event EventHandler<SmartListChangedEventArgs<T>> Changed;

		public SmartList(SmartListOptions flags)
		{
			this.flags = flags;
		}

		public SmartList()
			: this(SmartListOptions.Default)
		{
		}

		public SmartList(IEnumerable<T> list)
		{
			innerList = new List<T>(list);
		}

		public SmartList(SmartList<T> list)
		{
			using (list.GetLock(write: true))
			{
				innerList = new List<T>(list);
			}
			flags = list.flags;
		}

		public void AddRange(IEnumerable<T> list)
		{
			ICollection<T> collection = list as ICollection<T>;
			if (collection != null)
			{
				innerList.Capacity = Math.Max(innerList.Capacity, Count + collection.Count);
			}
			foreach (T item in list)
			{
				Add(item);
			}
		}

		public void RemoveRange(IEnumerable<T> list)
		{
			foreach (T item in list)
			{
				Remove(item);
			}
		}

		private void DoSort(Action sortCall)
		{
			using (GetLock(write: true))
			{
				sortCall();
			}
			if ((flags & SmartListOptions.DisableOnRefresh) == 0)
			{
				OnRefreshCompleted();
			}
		}

		public void Sort(IComparer<T> comparer)
		{
			DoSort(delegate
			{
				innerList.Sort(comparer);
			});
		}

		public void Sort(Comparison<T> comparison)
		{
			DoSort(delegate
			{
				innerList.Sort(comparison);
			});
		}

		public void Sort()
		{
			DoSort(innerList.Sort);
		}

		public SmartList<U> ConvertAll<U>(Converter<T, U> converter)
		{
			SmartList<U> smartList = new SmartList<U>();
			using (GetLock(write: false))
			{
				foreach (T inner in innerList)
				{
					smartList.Add(converter(inner));
				}
				return smartList;
			}
		}

		public bool TrueForAll(Predicate<T> predicate)
		{
			using (GetLock(write: false))
			{
				return innerList.TrueForAll(predicate);
			}
		}

		public T Find(Predicate<T> predicate)
		{
			using (GetLock(write: false))
			{
				return innerList.Find(predicate);
			}
		}

		public SmartList<T> FindAll(Predicate<T> predicate)
		{
			SmartList<T> smartList = new SmartList<T>();
			using (GetLock(write: false))
			{
				foreach (T inner in innerList)
				{
					if (predicate(inner))
					{
						smartList.Add(inner);
					}
				}
				return smartList;
			}
		}

		public bool Exists(Predicate<T> predicate)
		{
			using (GetLock(write: false))
			{
				return innerList.Exists(predicate);
			}
		}

		public void ForEach(Action<T> action, bool copy = false)
		{
			if (Count == 0)
			{
				return;
			}
			if (copy)
			{
				T[] array = ToArray();
				foreach (T obj in array)
				{
					action(obj);
				}
			}
			else
			{
				using (GetLock(write: false))
				{
					innerList.ForEach(action);
				}
			}
		}

		public T[] ToArray()
		{
			using (GetLock(write: false))
			{
				return innerList.ToArray();
			}
		}

		public List<T> ToList()
		{
			using (GetLock(write: false))
			{
				return new List<T>(innerList);
			}
		}

		public void Trim(int count)
		{
			if (count < 0)
			{
				throw new ArgumentException("count must be >= 0");
			}
			if (count == 0)
			{
				Clear();
				return;
			}
			while (Count > count)
			{
				RemoveAt(Count - 1);
			}
		}

		public void TrimExcess()
		{
			using (GetLock(write: true))
			{
				innerList.TrimExcess();
			}
		}

		public void Move(int oldIndex, int newIndex)
		{
			if (oldIndex == newIndex)
			{
				return;
			}
			T val;
			using (GetLock(write: true))
			{
				if (oldIndex < 0 || oldIndex >= Count || newIndex < 0 || newIndex >= Count)
				{
					return;
				}
				val = innerList[oldIndex];
				innerList.RemoveAt(oldIndex);
				if (newIndex > oldIndex + 1)
				{
					newIndex--;
				}
				innerList.Insert(newIndex, val);
			}
			InvokeChanged(SmartListAction.Move, newIndex, val, val);
		}

		public void Move(T item, int newIndex)
		{
			Move(IndexOf(item), newIndex);
		}

		public void MoveRelative(int n, int delta)
		{
			Move(n, n + delta);
		}

		public void MoveRelative(T item, int delta)
		{
			MoveRelative(IndexOf(item), delta);
		}

		public void MoveToBeginning(T item)
		{
			Move(item, 0);
		}

		public void MoveToEnd(T item)
		{
			using (GetLock(write: true))
			{
				Move(item, Count - 1);
			}
		}

		public bool IsAtStart(T item)
		{
			return IndexOf(item) == 0;
		}

		public bool IsAtEnd(T item)
		{
			return IndexOf(item) == Count - 1;
		}

		public T GetItemOrDefault(int i)
		{
			try
			{
				return this[i];
			}
			catch
			{
				return default(T);
			}
		}

		public void Add(T item)
		{
			bool flag = (flags & SmartListOptions.DisableOnInsert) == 0;
			if (flag)
			{
				OnInsert(Count, item);
			}
			int index;
			using (GetLock(write: true))
			{
				innerList.Add(item);
				index = innerList.Count - 1;
			}
			if (flag)
			{
				OnInsertCompleted(index, item);
			}
		}

		public void Clear()
		{
			if ((flags & SmartListOptions.DisableOnClear) != 0)
			{
				OnClear();
			}
			if ((flags & SmartListOptions.ClearWithRemove) != 0)
			{
				for (int num = Count - 1; num >= 0; num--)
				{
					try
					{
						RemoveAt(num);
					}
					catch (IndexOutOfRangeException)
					{
					}
					catch (ArgumentOutOfRangeException)
					{
					}
				}
			}
			else
			{
				using (GetLock(write: true))
				{
					innerList.Clear();
				}
			}
			if ((flags & SmartListOptions.DisableOnClear) == 0)
			{
				OnClearCompleted();
			}
		}

		public bool Contains(T item)
		{
			using (GetLock(write: false))
			{
				return innerList.Contains(item);
			}
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			using (GetLock(write: false))
			{
				innerList.CopyTo(array, arrayIndex);
			}
		}

		public bool Remove(T item)
		{
			bool flag = (flags & SmartListOptions.DisableOnRemove) == 0;
			int num = IndexOf(item);
			if (num == -1)
			{
				return false;
			}
			if (flag)
			{
				OnRemove(num, item);
			}
			using (GetLock(write: true))
			{
				innerList.Remove(item);
			}
			if (flag)
			{
				OnRemoveCompleted(num, item);
			}
			return true;
		}

		public int IndexOf(T item)
		{
			using (GetLock(write: false))
			{
				return innerList.IndexOf(item);
			}
		}

		public void Insert(int index, T item)
		{
			bool flag = (flags & SmartListOptions.DisableOnInsert) == 0;
			if (flag)
			{
				OnInsert(index, item);
			}
			using (GetLock(write: true))
			{
				innerList.Insert(index, item);
			}
			if (flag)
			{
				OnInsertCompleted(index, item);
			}
		}

		public void RemoveAt(int index)
		{
			bool flag = (flags & SmartListOptions.DisableOnRemove) == 0;
			T item = this[index];
			if (flag)
			{
				OnRemove(index, item);
			}
			using (GetLock(write: true))
			{
				innerList.RemoveAt(index);
			}
			if (flag)
			{
				OnRemoveCompleted(index, item);
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			if (IsSynchronized)
			{
				return LockedEnumerable().GetEnumerator();
			}
			return innerList.GetEnumerator();
		}

		void ICollection.CopyTo(Array array, int index)
		{
			using (GetLock(write: false))
			{
				((ICollection)innerList).CopyTo(array, index);
			}
		}

		int IList.Add(object value)
		{
			Add((T)value);
			return Count - 1;
		}

		bool IList.Contains(object value)
		{
			return Contains((T)value);
		}

		int IList.IndexOf(object value)
		{
			return IndexOf((T)value);
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, (T)value);
		}

		void IList.Remove(object value)
		{
			Remove((T)value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public object Clone()
		{
			return new SmartList<T>(this);
		}

		protected virtual void OnValidate(T item)
		{
		}

		protected virtual void OnInsert(int index, T item)
		{
			OnValidate(item);
		}

		protected virtual void OnInsertCompleted(int index, T item)
		{
			InvokeChanged(SmartListAction.Insert, index, item, item);
		}

		protected virtual void OnRemove(int index, T item)
		{
		}

		protected virtual void OnRemoveCompleted(int index, T item)
		{
			InvokeChanged(SmartListAction.Remove, index, item, item);
			if ((flags & SmartListOptions.DisposeOnRemove) != 0)
			{
				(item as IDisposable)?.Dispose();
			}
		}

		protected virtual void OnSet(int index, T newItem, T oldItem)
		{
			OnValidate(newItem);
		}

		protected virtual void OnSetCompleted(int index, T item, T oldItem)
		{
			InvokeChanged(SmartListAction.Set, index, item, oldItem);
		}

		protected virtual void OnClear()
		{
		}

		protected virtual void OnClearCompleted()
		{
			InvokeChanged(SmartListAction.Clear, -1, default(T), default(T));
		}

		protected virtual void OnRefreshCompleted()
		{
			InvokeChanged(SmartListAction.Refresh, -1, default(T), default(T));
		}

		private void InvokeChanged(SmartListAction action, int index, T item, T oldItem)
		{
			if (this.Changed != null && (flags & SmartListOptions.DisableCollectionChangedEvent) == 0)
			{
				this.Changed(this, new SmartListChangedEventArgs<T>(action, index, item, oldItem));
			}
		}

		protected IDisposable GetLock(bool write)
		{
			if (!IsSynchronized)
			{
				return null;
			}
			if (slimLock == null)
			{
				using (ItemMonitor.Lock(this))
				{
					if (slimLock == null)
					{
						slimLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
					}
				}
			}
			if (!write)
			{
				return slimLock.ReadLock();
			}
			return slimLock.WriteLock();
		}

		protected IEnumerable<T> LockedEnumerable()
		{
			using (GetLock(write: false))
			{
				foreach (T inner in innerList)
				{
					yield return inner;
				}
			}
		}

		public static SmartList<T> Adapter(List<T> list, SmartListOptions flags)
		{
			return new SmartList<T>(flags)
			{
				innerList = list
			};
		}

		public static SmartList<T> Adapter(List<T> list)
		{
			return Adapter(list, SmartListOptions.Default);
		}
	}
}
