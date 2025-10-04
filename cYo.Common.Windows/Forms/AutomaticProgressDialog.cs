using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Threading;

namespace cYo.Common.Windows.Forms
{
	public partial class AutomaticProgressDialog : FormEx
	{
		private Thread thread;

		private Action exectuteMethod;

		private volatile bool threadCompleted;

		private Exception catchedException;

		private static readonly Dictionary<int, bool> stopLookup = new Dictionary<int, bool>();

		private static readonly Dictionary<int, int> valueLookup = new Dictionary<int, int>();

        private readonly ManualResetEvent finishEvent = new ManualResetEvent(initialState: false);

        public static bool ShouldAbort
		{
			get
			{
				using (ItemMonitor.Lock(stopLookup))
				{
					bool value;
					return stopLookup.TryGetValue(Thread.CurrentThread.ManagedThreadId, out value) && value;
				}
			}
		}

		public static int Value
		{
			get
			{
				using (ItemMonitor.Lock(valueLookup))
				{
					int value;
					return valueLookup.TryGetValue(Thread.CurrentThread.ManagedThreadId, out value) ? value : (-1);
				}
			}
			set
			{
				using (ItemMonitor.Lock(valueLookup))
				{
					valueLookup[Thread.CurrentThread.ManagedThreadId] = value;
				}
			}
		}

		public AutomaticProgressDialog()
		{
			InitializeComponent();
		}

		private void threadCheckTimer_Tick(object sender, EventArgs e)
		{
			if (threadCompleted)
			{
				Close();
			}
			else
			{
				SetProgressStyle();
			}
		}

		private void btCancel_Click(object sender, EventArgs e)
		{
			btCancel.Enabled = false;
			using (ItemMonitor.Lock(stopLookup))
			{
				stopLookup[thread.ManagedThreadId] = true;
			}
			try
			{
				if (!thread.Join(3000))
				{
					thread.Abort();
					thread.Join();
				}
			}
			catch
			{
			}
		}

		private void SetProgressStyle()
		{
			int value;
			using (ItemMonitor.Lock(valueLookup))
			{
				valueLookup.TryGetValue(thread.ManagedThreadId, out value);
			}
			progressBar.Style = ((value < 0) ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks);
			progressBar.Value = value.Clamp(progressBar.Minimum, progressBar.Maximum);
		}

		private bool DoProcess(IWin32Window parent, int timeToWait)
		{
			bool flag;
			using (new WaitCursor())
			{
				thread = ThreadUtility.CreateWorkerThread("DoProcess", Execute, ThreadPriority.Normal);
				thread.Start();
				flag = !finishEvent.WaitOne(timeToWait, exitContext: false);
			}
			if (flag)
			{
				SetProgressStyle();
				if (parent == null)
				{
					base.StartPosition = FormStartPosition.CenterScreen;
					ShowDialog();
				}
				else
				{
					ShowDialog(parent);
				}
			}
			if (catchedException != null)
			{
				throw catchedException;
			}
			return true;
		}

		private void Execute()
		{
			try
			{
				Value = -1;
				exectuteMethod();
			}
			catch (ThreadAbortException)
			{
			}
			catch (Exception ex2)
			{
				Exception ex3 = (catchedException = ex2);
			}
			finally
			{
				using (ItemMonitor.Lock(stopLookup))
				{
					stopLookup.Remove(thread.ManagedThreadId);
				}
				try
				{
					finishEvent.Set();
				}
				catch
				{
				}
				threadCompleted = true;
			}
		}

		public static bool Process(IWin32Window parent, string caption, string description, int timeToWait, Action exectuteMethod, AutomaticProgressDialogOptions options)
		{
			using (AutomaticProgressDialog automaticProgressDialog = new AutomaticProgressDialog())
			{
				automaticProgressDialog.Text = caption;
				automaticProgressDialog.labelCaption.Text = description;
				automaticProgressDialog.exectuteMethod = exectuteMethod;
				automaticProgressDialog.btCancel.Text = TR.Default["Cancel", "Cancel"];
				automaticProgressDialog.btCancel.Visible = (options & AutomaticProgressDialogOptions.EnableCancel) != 0;
				if (parent == null)
				{
					automaticProgressDialog.TopLevel = true;
					automaticProgressDialog.TopMost = true;
				}
				return automaticProgressDialog.DoProcess(parent, timeToWait);
			}
		}

		public static bool Process(Form parent, string caption, string description, int timeToWait, Action exectuteMethod, AutomaticProgressDialogOptions options)
		{
			IWin32Window parent2 = parent;
			if (parent != null && (!parent.Visible || parent.WindowState == FormWindowState.Minimized))
			{
				parent2 = null;
			}
			return Process(parent2, caption, description, timeToWait, exectuteMethod, options);
		}
	}
}
