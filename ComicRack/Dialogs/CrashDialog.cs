using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using cYo.Common.Runtime;
using cYo.Common.Threading;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class CrashDialog : Form
	{
		private BarkType crashType = BarkType.ThreadException;

		public BarkType CrashType
		{
			get
			{
				return crashType;
			}
			set
			{
				crashType = value;
			}
		}

		public CrashDialog()
		{
			LocalizeUtility.UpdateRightToLeft(this);
			InitializeComponent();
			try
			{
				LocalizeUtility.Localize(this, null);
			}
			catch
			{
			}
		}

		private void btDetails_Click(object sender, EventArgs e)
		{
			btDetails.Visible = false;
			tbLog.Visible = true;
		}

		private void lockTimer_Tick(object sender, EventArgs e)
		{
			if (crashType == BarkType.Lock && !ThreadUtility.IsForegroundLocked)
			{
				Close();
			}
		}

		public static void Show(string report, BarkType crashType, bool enableSend)
		{
			using (CrashDialog crashDialog = new CrashDialog())
			{
				crashDialog.tbLog.Text = report;
				crashDialog.CrashType = crashType;
				if (!enableSend)
				{
					crashDialog.labelMessage.Visible = false;
				}
				DialogResult dialogResult = crashDialog.ShowDialog();
				if (crashType == BarkType.Lock && !ThreadUtility.IsForegroundLocked)
				{
					return;
				}
				try
				{
					switch (dialogResult)
					{
						case DialogResult.Retry:
							ThreadUtility.BreakForegroundLock();
							break;
						case DialogResult.OK:
							Application.Restart();
							break;
						case DialogResult.Cancel:
							Environment.Exit(1);
							break;
						case DialogResult.Abort:
							break;
					}
				}
				catch
				{
					Environment.Exit(1);
				}
			}
		}



		public static void OnBark(object sender, BarkEventArgs e)
		{
			if (e.Exception != null)
			{
				bool enableSend = !e.Exception.ToString().Contains("Microsoft.Scripting") && !e.Exception.ToString().Contains("Python");
				using (StringWriter stringWriter = new StringWriter())
				{
					Diagnostic.WriteProgramInfo(stringWriter);
					stringWriter.WriteLine(e.Bark.ToString().ToUpper());
					AddException(stringWriter, e.Exception);
					ThreadUtility.DumpStacks(stringWriter);
					stringWriter.WriteLine(new string('-', 20));
					stringWriter.WriteLine("Report generated at: {0}", DateTime.Now);
					Show(stringWriter.ToString(), e.Bark, enableSend);
				}
			}
		}


		private static void AddException(StringWriter sw, Exception exception)
		{
			if (exception != null)
			{
				sw.WriteLine(new string('-', 20));
				sw.WriteLine(exception.GetType().Name);
				if (exception.TargetSite != null)
				{
					sw.WriteLine(exception.TargetSite.ToString());
				}
				sw.WriteLine(exception.Message);
				sw.WriteLine(exception.StackTrace);
				sw.WriteLine(exception.InnerException);
			}
		}
	}
}
