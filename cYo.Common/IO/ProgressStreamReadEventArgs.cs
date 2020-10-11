using System;

namespace cYo.Common.IO
{
	public class ProgressStreamReadEventArgs : EventArgs
	{
		private readonly int count;

		public int Count => count;

		public ProgressStreamReadEventArgs(int count)
		{
			this.count = count;
		}
	}
}
