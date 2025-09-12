using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;

namespace cYo.Common.Threading
{
	public class ProcessingQueue<K> : DisposableObject
	{
		private class QueueItem : DisposableObject, IAsyncProcessingItem<K>, IAsyncResult, IProcessingItem<K>, IProgressState
		{
			private readonly object defaultCallbackKey;

			private readonly AsyncCallback defaultCallback;

			private volatile Dictionary<object, AsyncCallback> additionalCallbacks;

			private ManualResetEvent waitHandle;

			private volatile ProgressState state;

			public K Item
			{
				get;
				private set;
			}

			public object AsyncState => Item;

			public WaitHandle AsyncWaitHandle
			{
				get
				{
					if (waitHandle == null)
					{
						waitHandle = new ManualResetEvent(state == ProgressState.Completed);
					}
					return waitHandle;
				}
			}

			public bool CompletedSynchronously => false;

			public bool IsCompleted => state == ProgressState.Completed;

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

			public QueueItem(K item, object callbackKey, AsyncCallback callback)
			{
				Item = item;
				defaultCallbackKey = callbackKey;
				defaultCallback = callback;
			}

			public void AddCallback(AsyncCallback ac, object key)
			{
				if (key == null || key == defaultCallbackKey)
				{
					return;
				}
				if (additionalCallbacks == null)
				{
					additionalCallbacks = new Dictionary<object, AsyncCallback>();
				}
				using (ItemMonitor.Lock(additionalCallbacks))
				{
					if (!additionalCallbacks.ContainsKey(key))
					{
						additionalCallbacks[key] = ac;
					}
				}
			}

			public void ProcessCallbacks()
			{
				state = ProgressState.Running;
				defaultCallback(this);
				if (additionalCallbacks == null)
				{
					return;
				}
				using (ItemMonitor.Lock(additionalCallbacks))
				{
					additionalCallbacks.Values.ForEach(delegate(AsyncCallback ac)
					{
						ac(this);
					});
				}
			}

			public void SetCompleted()
			{
				state = ProgressState.Completed;
				if (waitHandle != null)
				{
					waitHandle.Set();
				}
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing && waitHandle != null)
				{
					waitHandle.Close();
				}
				base.Dispose(disposing);
			}
		}

		private class ProcessData
		{
			public Thread Thread
			{
				get;
				set;
			}

			public AutoResetEvent Event
			{
				get;
				set;
			}

			public bool IsActive
			{
				get;
				set;
			}
		}

		private bool abort;

		private bool stop;

		private readonly List<ProcessData> processThreads = new List<ProcessData>();

		private readonly LinkedList<K> processQueue = new LinkedList<K>();

		private readonly Dictionary<K, QueueItem> itemDict = new Dictionary<K, QueueItem>();

		private volatile ProcessingQueueAddMode defaultProcessingQueueAddMode;

		private volatile int size = int.MaxValue;

		public CultureInfo CurrentUICulture
		{
			get
			{
				return processThreads[0].Thread.CurrentUICulture;
			}
			set
			{
				processThreads.ForEach(delegate(ProcessData pd)
				{
					pd.Thread.CurrentUICulture = value;
				});
			}
		}

		public ThreadPriority Priority
		{
			get
			{
				return processThreads[0].Thread.Priority;
			}
			set
			{
				processThreads.ForEach(delegate(ProcessData pd)
				{
					pd.Thread.Priority = value;
				});
			}
		}

		public ProcessingQueueAddMode DefaultProcessingQueueAddMode
		{
			get
			{
				return defaultProcessingQueueAddMode;
			}
			set
			{
				defaultProcessingQueueAddMode = value;
			}
		}

		public int Size
		{
			get
			{
				return size;
			}
			set
			{
				if (size != value)
				{
					size = value;
					Trim(value);
				}
			}
		}

		public int Count
		{
			get
			{
				using (ItemMonitor.Lock(processQueue))
				{
					return processQueue.Count;
				}
			}
		}

		public bool IsActive => processThreads.Any((ProcessData pd) => pd.IsActive);

		public IEnumerable<K> PendingItems
		{
			get
			{
				using (ItemMonitor.Lock(processQueue))
				{
					return processQueue.ToList();
				}
			}
		}

		public IEnumerable<IProcessingItem<K>> PendingItemInfos
		{
			get
			{
				using (ItemMonitor.Lock(processQueue))
				{
					QueueItem q;
					return (from item in PendingItems
						select (!itemDict.TryGetValue(item, out q)) ? null : q into item
						where item != null
						select item).OfType<IProcessingItem<K>>().ToList();
				}
			}
		}

		public ProcessingQueue(int threadCount, string name, ThreadPriority priority, int size)
		{
			Size = size;
			for (int i = 0; i < threadCount; i++)
			{
				string name2 = ((threadCount < 2) ? name : $"{name} #{i + 1}");
				ProcessData pd = new ProcessData
				{
					Event = new AutoResetEvent(initialState: false)
				};
				Thread thread2 = (pd.Thread = ThreadUtility.CreateWorkerThread(name2, delegate
				{
					ProcessThread(pd);
				}, priority));
				Thread thread3 = thread2;
				processThreads.Add(pd);
				thread3.Start();
			}
		}

		public ProcessingQueue(string name, ThreadPriority priority, int size)
			: this(1, name, priority, size)
		{
		}

		public ProcessingQueue(string name, ThreadPriority priority = ThreadPriority.BelowNormal)
			: this(name, priority, int.MaxValue)
		{
		}

		private void ProcessThread(ProcessData pd)
		{
			while (pd.Event.WaitOne())
			{
				pd.IsActive = true;
				try
				{
					while (true)
					{
						if (abort)
							return;

						K item = default(K);
						QueueItem queueItem = null;
						using (ItemMonitor.Lock(processQueue))
						{
							for (LinkedListNode<K> linkedListNode = processQueue.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
							{
								K value = linkedListNode.Value;
								if (itemDict.TryGetValue(value, out var value2) && value2.State == ProgressState.Waiting)
								{
									item = value;
									queueItem = value2;
									break;
								}
							}
							if (queueItem == null)
							{
								if (stop)
									return;

								break;
							}
						}
						try
						{
							queueItem.ProcessCallbacks();
						}
						catch (Exception)
						{
						}
						finally
						{
							RemoveItem(item, dispose: false);
							queueItem.SetCompleted();
							queueItem.Dispose();
						}
					}
				}
				finally
				{
					pd.IsActive = false;
				}
			}
		}

		public IAsyncProcessingItem<K> AddItem(K item, object callbackKey, AsyncCallback processCallback, ProcessingQueueAddMode mode)
		{
			QueueItem value;
			using (ItemMonitor.Lock(processQueue))
			{
				if (base.IsDisposed)
				{
					return null;
				}
				itemDict.TryGetValue(item, out value);
				if (value == null)
				{
					value = (itemDict[item] = new QueueItem(item, callbackKey, processCallback));
					switch (mode)
					{
					case ProcessingQueueAddMode.AddToBottom:
						processQueue.AddLast(item);
						break;
					default:
						processQueue.AddFirst(item);
						break;
					}
				}
				else
				{
					value.AddCallback(processCallback, callbackKey);
					if (mode == ProcessingQueueAddMode.AddToTop)
					{
						processQueue.Remove(item);
						processQueue.AddFirst(item);
					}
				}
			}
			Trim(size);
			processThreads.ForEach(delegate(ProcessData pd)
			{
				pd.Event.Set();
			});
			return value;
		}

		public IAsyncProcessingItem<K> AddItem(K item, object callbackKey, AsyncCallback processCallback)
		{
			return AddItem(item, callbackKey, processCallback, DefaultProcessingQueueAddMode);
		}

		public IAsyncResult AddItem(K item, AsyncCallback processCallback)
		{
			return AddItem(item, null, processCallback);
		}

		public void RemoveItem(K item, bool dispose = true)
		{
			using (ItemMonitor.Lock(processQueue))
			{
				if (itemDict.TryGetValue(item, out var value))
				{
					value.Abort = true;
					if (dispose)
					{
						value.Dispose();
					}
					itemDict.Remove(item);
				}
				processQueue.Remove(item);
			}
		}

		public void RemoveItems<TK>(Predicate<TK> predicate) where TK : K
		{
			using (ItemMonitor.Lock(processQueue))
			{
				(from k in itemDict.Keys.OfType<TK>()
					where predicate(k)
					select k).ToArray().ForEach(delegate(TK k)
				{
					RemoveItem((K)(object)k);
				});
			}
		}

		public void Trim(int size)
		{
			using (ItemMonitor.Lock(processQueue))
			{
				while (processQueue.Count > size)
				{
					RemoveItem(processQueue.Last.Value);
				}
			}
		}

		public void Clear()
		{
			Trim(0);
		}

		public void Stop(bool abort, int timeOut)
		{
			if (abort)
			{
				this.abort = true;
			}
			else
			{
				stop = true;
			}
			processThreads.ForEach(delegate(ProcessData pd)
			{
				pd.Event.Set();
			});
			processThreads.ForEach(delegate(ProcessData pd)
			{
				pd.Thread.Join(timeOut);
			});
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				using (ItemMonitor.Lock(processQueue))
				{
					processQueue.Clear();
					itemDict.Values.ForEach(delegate(QueueItem qi)
					{
						qi.Dispose();
					});
				}
				Stop(abort: true, 5000);
				processThreads.ForEach(delegate(ProcessData pd)
				{
					pd.Event.Close();
				});
			}
			base.Dispose(disposing);
		}
	}
}
