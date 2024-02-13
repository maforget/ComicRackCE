using System;
using System.Threading;
using System.Windows.Forms;
using cYo.Common.ComponentModel;
using cYo.Common.Threading;

namespace cYo.Common.Runtime
{
	public class CrashWatchDog : DisposableObject
	{
		private const int WatcherTimeSpanMS = 1000;

		private readonly object timeLock = new object();

		private TimeSpan lockTestTime = new TimeSpan(0, 0, 10);

		private DateTime lastTimeRunning = DateTime.Now;

		private bool inBark;

		private Thread lockWatcherThread;

		private readonly EventWaitHandle lockWatcherHandle = new EventWaitHandle(initialState: false, EventResetMode.ManualReset);

		public TimeSpan LockTestTime
		{
			get
			{
				using (ItemMonitor.Lock(timeLock))
				{
					return lockTestTime;
				}
			}
			set
			{
				using (ItemMonitor.Lock(timeLock))
				{
					lockTestTime = value;
				}
			}
		}

		public DateTime LastTimeRunning
		{
			get
			{
				using (ItemMonitor.Lock(timeLock))
				{
					return lastTimeRunning;
				}
			}
			protected set
			{
				using (ItemMonitor.Lock(timeLock))
				{
					lastTimeRunning = value;
				}
			}
		}

		public event BarkEventHandler Bark;

		protected override void Dispose(bool disposing)
		{
			lockWatcherHandle.Close();
			base.Dispose(disposing);
		}

		public void Register()
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += domain_UnhandledException;
			Application.ThreadException += Application_ThreadException;
			if (lockTestTime.TotalSeconds > 0.0)
			{
				StartLockWatcher();
			}
		}

		protected virtual void OnBark(BarkType bark, Exception e)
		{
			try
			{
				inBark = true;
				if (this.Bark != null)
				{
					this.Bark(this, new BarkEventArgs(bark, e));
				}
			}
			finally
			{
				inBark = false;
			}
		}

		protected virtual void OnLockDetected()
		{
			OnBark(BarkType.Lock, null);
			ThreadUtility.BreakForegroundLock();
		}

		protected virtual void OnThreadException(Exception e)
		{
			OnBark(BarkType.ThreadException, e);
		}

		protected virtual void OnDomainException(Exception e)
		{
			OnBark(BarkType.DomainException, e);
		}

		private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			OnThreadException(e.Exception);
		}

		private void domain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			OnDomainException(e.ExceptionObject as Exception);
		}

		private void StartLockWatcher()
		{
			lockWatcherThread = new Thread(LockWatcher)
			{
				IsBackground = true,
				Priority = ThreadPriority.Highest
			};
			lockWatcherThread.Start();
		}

		private void LockWatcher()
		{
			DateTime d = DateTime.Now;
			while (!lockWatcherHandle.WaitOne(WatcherTimeSpanMS, exitContext: false))
			{
				DateTime now = DateTime.Now;
				if (inBark || !ThreadUtility.IsForegroundLocked || now - d > LockTestTime)
				{
					LastTimeRunning = now;
				}
				else if (now - LastTimeRunning > LockTestTime)
				{
					LastTimeRunning = now;
					OnLockDetected();
				}
				d = now;
			}
		}
	}
}
