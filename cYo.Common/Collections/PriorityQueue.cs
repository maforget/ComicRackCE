using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using cYo.Common.Threading;

namespace cYo.Common.Collections
{
	public class PriorityQueue<T> : IProducerConsumerCollection<T>, IEnumerable<T>, IEnumerable, ICollection
	{
		private readonly ReaderWriterLockSlim rwlock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

		private readonly IEqualityComparer<T> equality;

		private readonly LinkedList<T> queue = new LinkedList<T>();

		private int capacity = 100;

		public PriorityQueueAddMode AddMode
		{
			get;
			set;
		}

		public int Capacity
		{
			get
			{
				return capacity;
			}
			set
			{
				if (capacity != value)
				{
					Trim(capacity = value);
				}
			}
		}

		public int Count
		{
			get
			{
				using (rwlock.ReadLock())
				{
					return queue.Count;
				}
			}
		}

		public bool IsSynchronized => true;

		public object SyncRoot => rwlock;

		public PriorityQueue(IEqualityComparer<T> equality = null)
		{
			this.equality = equality ?? EqualityComparer<T>.Default;
			AddMode = PriorityQueueAddMode.AddToTop;
		}

		public bool Add(T item)
		{
			bool flag = false;
			using (rwlock.WriteLock())
			{
				if (TryFindNode(item, out var node))
				{
					queue.Remove(node);
					node.Value = item;
				}
				else
				{
					node = new LinkedListNode<T>(item);
					flag = true;
				}
				switch (AddMode)
				{
				case PriorityQueueAddMode.AddToBottom:
					if (flag)
					{
						Trim(Capacity - 1);
					}
					queue.AddLast(node);
					return flag;
				default:
					queue.AddFirst(node);
					if (flag)
					{
						Trim(Capacity);
						return flag;
					}
					return flag;
				}
			}
		}

		public bool Remove(T item)
		{
			using (rwlock.UpgradeableReadLock())
			{
				if (TryFindNode(item, out var node))
				{
					using (rwlock.WriteLock())
					{
						queue.Remove(node);
					}
					return true;
				}
			}
			return false;
		}

		public void Trim(int capacity)
		{
			using (rwlock.UpgradeableReadLock())
			{
				if (queue.Count <= capacity)
				{
					return;
				}
				using (rwlock.WriteLock())
				{
					while (queue.Count > capacity)
					{
						LinkedListNode<T> last = queue.Last;
						queue.RemoveLast();
					}
				}
			}
		}

		public void Clear()
		{
			using (rwlock.WriteLock())
			{
				queue.Clear();
			}
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public IEnumerator<T> GetEnumerator()
		{
			using (rwlock.ReadLock())
			{
				return ToArray().OfType<T>().GetEnumerator();
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void CopyTo(T[] array, int index)
		{
			using (rwlock.ReadLock())
			{
				queue.CopyTo(array, index);
			}
		}

		public T[] ToArray()
		{
			using (rwlock.ReadLock())
			{
				return queue.ToArray();
			}
		}

		public bool TryAdd(T item)
		{
			Add(item);
			return true;
		}

		public bool TryTake(out T item)
		{
			item = default(T);
			using (rwlock.UpgradeableReadLock())
			{
				if (queue.First == null)
				{
					return false;
				}
				item = queue.First.Value;
				using (rwlock.WriteLock())
				{
					queue.RemoveFirst();
				}
				return true;
			}
		}

		public void CopyTo(Array array, int index)
		{
			using (rwlock.ReadLock())
			{
				foreach (T item in queue)
				{
					array.SetValue(item, index++);
				}
			}
		}

		private bool TryFindNode(T value, out LinkedListNode<T> node)
		{
			for (node = queue.First; node != null; node = node.Next)
			{
				if (equality.Equals(node.Value, value))
				{
					return true;
				}
			}
			return false;
		}
	}
}
