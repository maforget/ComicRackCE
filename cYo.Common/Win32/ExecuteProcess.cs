using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace cYo.Common.Win32
{
	public static class ExecuteProcess
	{
		[Flags]
		public enum Options
		{
			None = 0x0,
			StoreOutput = 0x1,
			EnableWindow = 0x2,
			EnableAppStartingCursor = 0x4
		}

		public class Result
		{
			private static class Native
			{
				[DllImport("kernel32.dll")]
				public static extern int GetOEMCP();
			}

			private readonly int exitCode;

			private readonly byte[] output;

			private string consoleText;

			public int ExitCode => exitCode;

			public byte[] Output => output;

			public string ConsoleText
			{
				get
				{
					if (output != null && consoleText == null)
					{
						consoleText = Encoding.GetEncoding(Native.GetOEMCP()).GetString(output);
					}
					return consoleText;
				}
			}

			public Result(int exitCode, byte[] output)
			{
				this.exitCode = exitCode;
				this.output = output;
			}
		}

		public static Result Execute(string application, string parameters, byte[] inputData, string currentDirectory, Options options)
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo(application, parameters);
			byte[] output = null;
			processStartInfo.WorkingDirectory = currentDirectory;
			processStartInfo.CreateNoWindow = (options & Options.EnableWindow) == 0;
			processStartInfo.RedirectStandardInput = inputData != null;
			processStartInfo.RedirectStandardOutput = (options & Options.StoreOutput) != 0;
			processStartInfo.UseShellExecute = false;
			Process p = Process.Start(processStartInfo);
			try
			{
				ManualResetEvent handle = (processStartInfo.RedirectStandardOutput ? new ManualResetEvent(initialState: false) : null);
				try
				{
					MemoryStream ms = new MemoryStream();
					try
					{
						byte[] buffer = new byte[65535];
						if (handle != null)
						{
							ThreadPool.QueueUserWorkItem(delegate
							{
								try
								{
									int count;
									while ((count = p.StandardOutput.BaseStream.Read(buffer, 0, buffer.Length)) != 0)
									{
										ms.Write(buffer, 0, count);
									}
								}
								catch (Exception)
								{
								}
								finally
								{
									try
									{
										handle.Set();
									}
									catch
									{
									}
								}
							});
						}
						if (processStartInfo.RedirectStandardInput)
						{
							p.StandardInput.BaseStream.Write(inputData, 0, inputData.Length);
							p.StandardInput.Close();
						}
						p.WaitForExit();
						if (handle != null)
						{
							try
							{
								handle.WaitOne();
								output = ms.ToArray();
							}
							catch
							{
							}
						}
					}
					finally
					{
						if (ms != null)
						{
							((IDisposable)ms).Dispose();
						}
					}
				}
				finally
				{
					if (handle != null)
					{
						((IDisposable)handle).Dispose();
					}
				}
				return new Result(p.ExitCode, output);
			}
			finally
			{
				if (p != null)
				{
					((IDisposable)p).Dispose();
				}
			}
		}

		public static Result Execute(string application, string parameters, string currentDirectory, Options options)
		{
			return Execute(application, parameters, null, currentDirectory, options);
		}

		public static Result Execute(string application, string parameters, string currentDirectory)
		{
			return Execute(application, parameters, currentDirectory, Options.None);
		}

		public static Result Execute(string application, string parameters, Options options)
		{
			return Execute(application, parameters, null, options);
		}

		public static Result Execute(string commandLine, Options options)
		{
			return Execute(null, commandLine, options);
		}

		public static Result Execute(string commandLine)
		{
			return Execute(commandLine, Options.None);
		}
	}
}
