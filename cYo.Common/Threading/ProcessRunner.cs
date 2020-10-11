using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using cYo.Common.ComponentModel;

namespace cYo.Common.Threading
{
	public class ProcessRunner : DisposableObject
	{
		private Process currentProcess;

		private ProcessPriorityClass priority = ProcessPriorityClass.Normal;

		private readonly byte[] readBuffer = new byte[1024];

		private Stream readStream;

		public ProcessPriorityClass Priority
		{
			get
			{
				return priority;
			}
			set
			{
				if (priority != value)
				{
					if (currentProcess != null)
					{
						currentProcess.PriorityClass = value;
					}
					priority = value;
				}
			}
		}

		public bool IsRunning
		{
			get
			{
				using (ItemMonitor.Lock(this))
				{
					return currentProcess != null;
				}
			}
		}

		public DateTime StartTime
		{
			get
			{
				using (ItemMonitor.Lock(this))
				{
					return currentProcess.StartTime;
				}
			}
		}

		public TimeSpan RunningTime
		{
			get
			{
				using (ItemMonitor.Lock(this))
				{
					return DateTime.Now - StartTime;
				}
			}
		}

		public event EventHandler Stopped;

		public event EventHandler<ProcessRunnerOutputEventArgs> ParseOutput;

		protected override void Dispose(bool disposing)
		{
			Stop();
		}

		public void Run(string path, string arguments, bool waitForExit)
		{
			Stop();
			ProcessStartInfo startInfo = new ProcessStartInfo(path, arguments)
			{
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				WorkingDirectory = Path.GetDirectoryName(path),
				UseShellExecute = false
			};
			currentProcess = new Process
			{
				StartInfo = startInfo,
				EnableRaisingEvents = true
			};
			currentProcess.Exited += ProcessStopped;
			currentProcess.Start();
			currentProcess.PriorityClass = priority;
			readStream = currentProcess.StandardOutput.BaseStream;
			readStream.BeginRead(readBuffer, 0, readBuffer.Length, ReadOutput, null);
			if (waitForExit)
			{
				currentProcess.WaitForExit();
				Stop();
			}
		}

		public void Run(string path, string arguments)
		{
			Run(path, arguments, waitForExit: false);
		}

		public void Stop()
		{
			if (!IsRunning)
			{
				return;
			}
			using (ItemMonitor.Lock(this))
			{
				try
				{
					currentProcess.Kill();
					currentProcess.WaitForExit();
				}
				catch
				{
				}
				finally
				{
					currentProcess.Exited -= ProcessStopped;
					currentProcess.Dispose();
					currentProcess = null;
				}
			}
		}

		private void ReadOutput(IAsyncResult ar)
		{
			try
			{
				int num = readStream.EndRead(ar);
				if (num > 0)
				{
					string @string = Encoding.UTF8.GetString(readBuffer, 0, num);
					ProcessRunnerOutputEventArgs processRunnerOutputEventArgs = new ProcessRunnerOutputEventArgs(@string);
					OnParseOutput(processRunnerOutputEventArgs);
					if (processRunnerOutputEventArgs.Cancel)
					{
						Stop();
					}
					readStream.BeginRead(readBuffer, 0, readBuffer.Length, ReadOutput, null);
				}
			}
			catch
			{
			}
		}

		private void ProcessStopped(object sender, EventArgs e)
		{
			OnStopped();
		}

		protected virtual void OnStopped()
		{
			if (this.Stopped != null)
			{
				this.Stopped(this, EventArgs.Empty);
			}
		}

		protected virtual void OnParseOutput(ProcessRunnerOutputEventArgs poea)
		{
			if (this.ParseOutput != null)
			{
				this.ParseOutput(this, poea);
			}
		}

		public static int RunElevated(string file, string arguments)
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo(file);
			processStartInfo.Arguments = arguments;
			processStartInfo.UseShellExecute = true;
			processStartInfo.Verb = "runas";
			try
			{
				Process process = Process.Start(processStartInfo);
				process.WaitForExit();
				return process.ExitCode;
			}
			catch
			{
				return -1;
			}
		}
	}
}
