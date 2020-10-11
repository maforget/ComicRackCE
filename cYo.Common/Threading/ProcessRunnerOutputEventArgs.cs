using System.ComponentModel;

namespace cYo.Common.Threading
{
	public class ProcessRunnerOutputEventArgs : CancelEventArgs
	{
		private readonly string text;

		public string Text => text;

		public ProcessRunnerOutputEventArgs(string text)
		{
			this.text = text;
		}
	}
}
