using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using cYo.Common.Collections;
using cYo.Common.Runtime;

namespace cYo.Common.Threading
{
	public static class ThreadUtility
	{
		private class ThreadPoolState : IAsyncResult, IDisposable
		{
			public object AsyncState
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public WaitHandle AsyncWaitHandle
			{
				get;
				private set;
			}

			public bool CompletedSynchronously
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public bool IsCompleted
			{
				get;
				private set;
			}

			public ThreadPoolState()
			{
				IsCompleted = false;
				AsyncWaitHandle = new ManualResetEvent(initialState: false);
			}

			public void Dispose()
			{
				IsCompleted = true;
				((ManualResetEvent)AsyncWaitHandle).Set();
				AsyncWaitHandle.Dispose();
			}
		}

		[Flags]
		private enum EXECUTION_STATE : uint
		{
			ES_AWAYMODE_REQUIRED = 0x40u,
			ES_CONTINUOUS = 0x80000000u,
			ES_DISPLAY_REQUIRED = 0x2u,
			ES_SYSTEM_REQUIRED = 0x1u
		}

		private static readonly HashSet<Thread> activeThreads = new HashSet<Thread>();

		private const int MaxThreadQueueSize = 64;

		private static readonly HashSet<Thread> threadQueue = new HashSet<Thread>();

		private static HashSet<Action> blocks;

		public static IEnumerable<Thread> ActiveThreads => (from t in activeThreads.Lock()
			where t.IsAlive
			select t).ToArray();

		public static Thread ForgroundThread => ActiveThreads.FirstOrDefault((Thread t) => !t.IsBackground);

		public static bool IsForegroundLocked
		{
			get
			{
				Thread forgroundThread = ForgroundThread;
				if (forgroundThread != null)
				{
					return forgroundThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin;
				}
				return false;
			}
		}

		public static string StacksDump
		{
			get
			{
				StringWriter stringWriter = new StringWriter();
				DumpStacks(stringWriter);
				return stringWriter.ToString();
			}
		}

		public static Thread CreateWorkerThread(string name, ThreadStart threadStart, ThreadPriority priority)
		{
			return new Thread((ThreadStart)delegate
			{
				Thread currentThread = Thread.CurrentThread;
				AddActiveThread(currentThread);
				try
				{
					threadStart();
				}
				catch (ThreadInterruptedException)
				{
				}
				catch (ThreadAbortException)
				{
				}
				finally
				{
					RemoveActiveThread(currentThread);
				}
			})
			{
				Name = name,
				Priority = priority,
				IsBackground = true,
				CurrentCulture = Thread.CurrentThread.CurrentCulture,
				CurrentUICulture = Thread.CurrentThread.CurrentUICulture
			};
		}

		public static Thread RunInBackground(string name, ThreadStart method, ThreadPriority priority = ThreadPriority.BelowNormal)
		{
			Thread thread = CreateWorkerThread(name, method, priority);
			thread.Start();
			return thread;
		}

		public static IAsyncResult RunInThreadPool(Action method)
		{
			ThreadPoolState threadPoolState = new ThreadPoolState();
			try
			{
				ThreadPoolState localState = threadPoolState;
				ThreadPool.QueueUserWorkItem(delegate
				{
					try
					{
						method();
					}
					catch (Exception)
					{
					}
					finally
					{
						localState.Dispose();
					}
				});
				return threadPoolState;
			}
			catch (Exception)
			{
				threadPoolState.Dispose();
				return null;
			}
		}

		public static IAsyncResult RunInThreadQueue(Action method)
		{
			ThreadPoolState threadPoolState = new ThreadPoolState();
			try
			{
				ThreadPoolState localState = threadPoolState;
				Thread thread = null;
				Action action = delegate
				{
					try
					{
						method();
					}
					catch (Exception)
					{
					}
					finally
					{
						localState.Dispose();
					}
				};
				using (ItemMonitor.Lock(threadQueue))
				{
					if (threadQueue.Count < 64)
					{
						thread = new Thread((ThreadStart)delegate
						{
							action();
							using (ItemMonitor.Lock(threadQueue))
							{
								threadQueue.Remove(thread);
							}
						});
					}
				}
				if (thread != null)
				{
					thread.Start();
					return threadPoolState;
				}
				action();
				return threadPoolState;
			}
			catch (Exception)
			{
				threadPoolState.Dispose();
				return null;
			}
		}

		public static void AddActiveThread(Thread t)
		{
			try
			{
				using (ItemMonitor.Lock(activeThreads))
				{
					activeThreads.Add(t);
				}
			}
			catch (Exception)
			{
			}
		}

		public static void RemoveActiveThread(Thread t)
		{
			using (ItemMonitor.Lock(activeThreads))
			{
				activeThreads.Remove(t);
			}
		}

		public static void Abort(Thread thread, int timeOut)
		{
			if (thread != null && thread.IsAlive && !thread.Join(timeOut))
			{
				thread.Abort();
				thread.Join();
			}
		}

        private static void DumpThread(TextWriter tw, Thread t)
        {
            try
            {
                if (!t.IsAlive)
                {
                    return;
                }

                tw.WriteLine(new string('-', 20));
                tw.WriteLine($"{t.Name}: {t.ThreadState} ({(t.IsBackground ? "B" : string.Empty)})");

                StackTrace stackTrace;
                if (t == Thread.CurrentThread)
                {
                    stackTrace = new StackTrace();
                }
                else
                {
                    lock (t)
                    {
                        stackTrace = new StackTrace(true);
                    }
                }

                tw.WriteLine(stackTrace.ToString());
            }
            catch
            {
            }
        }

		public static void DumpStacks(TextWriter tw)
		{
			foreach (Thread activeThread in ActiveThreads)
			{
				try
				{
					if (activeThread.IsAlive)
					{
						DumpThread(tw, activeThread);
					}
				}
				catch
				{
				}
			}
		}

		public static void BreakForegroundLock()
		{
			Thread forgroundThread = ForgroundThread;
			if (forgroundThread != null && forgroundThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
			{
				forgroundThread.Interrupt();
			}
		}

		public static int Animate(int startTime, int endTime, Action<float> animate)
		{
			int num = endTime - startTime;
			long ticks = Machine.Ticks;
			long num2 = ticks + num;
			long ticks2;
			while ((ticks2 = Machine.Ticks) < num2)
			{
				animate((float)(ticks2 - ticks) / (float)(num2 - ticks));
			}
			animate(1f);
			return Math.Max((int)(ticks2 - num2), 0);
		}

		public static int Animate(int time, Action<float> animate)
		{
			return Animate(0, time, animate);
		}

		public static void Block(Action method)
		{
			if (blocks == null)
			{
				blocks = new HashSet<Action>();
			}
			if (!blocks.Contains(method))
			{
				try
				{
					blocks.Add(method);
					method();
				}
				catch
				{
					blocks.Remove(method);
				}
			}
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

		public static void KeepAlive(bool withDisplay = false)
		{
			EXECUTION_STATE eXECUTION_STATE = EXECUTION_STATE.ES_SYSTEM_REQUIRED;
			if (withDisplay)
			{
				eXECUTION_STATE |= EXECUTION_STATE.ES_DISPLAY_REQUIRED;
			}
			SetThreadExecutionState(eXECUTION_STATE);
		}
	}
}
