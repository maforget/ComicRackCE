using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.Threading;

namespace cYo.Common.Win32
{
	public static class IdleProcess
	{
		private class IdleKey
		{
			public Control Form
			{
				get;
				set;
			}

			public object Key
			{
				get;
				set;
			}

			public Func<Control, bool> Action
			{
				get;
				set;
			}

			public override int GetHashCode()
			{
				return Form.GetHashCode() ^ Action.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				IdleKey idleKey = obj as IdleKey;
				if (!object.Equals(idleKey.Form, Form))
				{
					return false;
				}
				if (idleKey.Key != null || Key != null)
				{
					return object.Equals(idleKey.Key, Key);
				}
				return object.Equals(idleKey.Action, Action);
			}
		}

		private static EventHandler idle;

		private static HashSet<IdleKey> idleKeys;

		public static event EventHandler Idle
		{
			add
			{
				lock (typeof(IdleProcess))
				{
					if (idle == null)
					{
						Initialize();
					}
					idle = (EventHandler)Delegate.Combine(idle, value);
				}
			}
			remove
			{
				lock (typeof(IdleProcess))
				{
					idle = (EventHandler)Delegate.Remove(idle, value);
				}
			}
		}

		public static event CancelEventHandler CancelIdle;

		public static bool ShouldProcess(Form f)
		{
			if (f != null && f.Visible)
			{
				return f.WindowState != FormWindowState.Minimized;
			}
			return false;
		}

		public static void RaiseIdle()
		{
			Application.RaiseIdle(EventArgs.Empty);
		}

		public static bool AddIdleAction(this Control form, Func<Control, bool> action, string key = null)
		{
			if (idleKeys == null)
			{
				idleKeys = new HashSet<IdleKey>();
				Idle += IdleProcessing;
			}
			using (ItemMonitor.Lock(idleKeys))
			{
				IdleKey item = new IdleKey
				{
					Form = form,
					Action = action,
					Key = key
				};
				if (idleKeys.Contains(item))
				{
					return false;
				}
				idleKeys.Add(item);
				return true;
			}
		}

		public static bool AddIdleAction(this Control form, Action<Control> action, string key = null, bool continuous = false)
		{
			return form.AddIdleAction(delegate(Control f)
			{
				action(f);
				return continuous;
			}, key);
		}

		public static bool AddIdleAction(this Control form, Action action, string key = null, bool continuous = false)
		{
			return form.AddIdleAction(delegate
			{
				action();
				return continuous;
			}, key);
		}

		private static void IdleProcessing(object sender, EventArgs e)
		{
			IdleKey[] array = idleKeys.Lock().ToArray();
			foreach (IdleKey idleKey in array)
			{
				if (idleKey.Form.IsDisposed || !idleKey.Action(idleKey.Form))
				{
					using (ItemMonitor.Lock(idleKeys))
					{
						idleKeys.Remove(idleKey);
					}
				}
			}
		}

		private static void Initialize()
		{
			Application.Idle += Application_Idle;
		}

		private static void Application_Idle(object sender, EventArgs e)
		{
			InvokeIdle();
		}

		private static void InvokeIdle()
		{
			try
			{
				if (!CheckCancelIdle() && idle != null)
				{
					idle(null, EventArgs.Empty);
				}
			}
			catch (Exception)
			{
			}
		}

		private static bool CheckCancelIdle()
		{
			if (IdleProcess.CancelIdle == null)
			{
				return false;
			}
			CancelEventArgs cancelEventArgs = new CancelEventArgs();
			IdleProcess.CancelIdle(null, cancelEventArgs);
			return cancelEventArgs.Cancel;
		}
	}
}
