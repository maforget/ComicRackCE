using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;

namespace cYo.Common.Threading
{
	public class TaskQueue<K> : DisposableObject
	{
		public class ProcessingItem : IProcessingItem<K>, IProgressState
		{
			private volatile ProgressState state;

			public Action<IProcessingItem<K>> Action
			{
				get;
				private set;
			}

			public K Item
			{
				get;
				private set;
			}

			public ProgressState State => state;

			public int ProgressPercentage
			{
				get;
				set;
			}

			public string ProgressMessage
			{
				get;
				set;
			}

			public bool ProgressAvailable
			{
				get;
				set;
			}

			public bool Abort
			{
				get;
				set;
			}

			public event Action<IProcessingItem<K>> Completed;

			public ProcessingItem(K key, Action<IProcessingItem<K>> action)
			{
				Item = key;
				Action = action;
			}

			public void Execute()
			{
				state = ProgressState.Running;
				try
				{
					Action(this);
				}
				catch
				{
				}
				finally
				{
					state = ProgressState.Completed;
					if (this.Completed != null)
					{
						this.Completed(this);
					}
				}
			}

			public override bool Equals(object x)
			{
				ProcessingItem processingItem = x as ProcessingItem;
				if (processingItem != null)
				{
					return object.Equals(processingItem.Item, Item);
				}
				return false;
			}

			public override int GetHashCode()
			{
				return Item.GetHashCode();
			}
		}

		private CancellationTokenSource cts = new CancellationTokenSource();

		private AutoResetEvent queueChanged = new AutoResetEvent(initialState: false);

		private IDictionary<K, ProcessingItem> working = new ConcurrentDictionary<K, ProcessingItem>();

		private ConcurrentBag<Task> runningTasks = new ConcurrentBag<Task>();

		private IProducerConsumerCollection<ProcessingItem> queue;

		public TaskQueue(IProducerConsumerCollection<ProcessingItem> queue, int workerCount = 1)
		{
			this.queue = queue;
			AddWorkers(workerCount);
		}

		public TaskQueue(int capacity, int workerCount = 1)
			: this((IProducerConsumerCollection<ProcessingItem>)new PriorityQueue<ProcessingItem>
			{
				Capacity = capacity
			}, workerCount)
		{
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			cts.Cancel();
			Task.WaitAll(runningTasks.ToArray());
		}

		public bool Enqeue(K key, Action<IProcessingItem<K>> action, Action<IProcessingItem<K>> completed = null)
		{
			if (working.TryGetValue(key, out var value))
			{
				if (completed != null)
				{
					value.Completed += completed;
				}
			}
			else
			{
				value = new ProcessingItem(key, action);
				if (completed != null)
				{
					value.Completed += completed;
				}
				if (!queue.TryAdd(value))
				{
					return false;
				}
				queueChanged.Set();
			}
			return true;
		}

		public void AddWorkers(int count)
		{
			CancellationToken ct = cts.Token;
			for (int i = 0; i < count; i++)
			{
				runningTasks.Add(Task.Factory.StartNew(delegate
				{
					while (true)
					{
						WaitHandle.WaitAny(new WaitHandle[2]
						{
							queueChanged,
							ct.WaitHandle
						});
						ct.ThrowIfCancellationRequested();
						ProcessingItem item;
						while (queue.TryTake(out item))
						{
							ct.ThrowIfCancellationRequested();
							working[item.Item] = item;
							item.Execute();
							working.Remove(item.Item);
						}
						ct.ThrowIfCancellationRequested();
					}
				}, ct));
			}
		}
	}
}
