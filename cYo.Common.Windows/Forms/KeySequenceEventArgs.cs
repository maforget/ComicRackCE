using System;

namespace cYo.Common.Windows.Forms
{
	public class KeySequenceEventArgs : EventArgs
	{
		private readonly KeySequence sequence;

		public KeySequence Sequence => sequence;

		public KeySequenceEventArgs(KeySequence sequence)
		{
			this.sequence = sequence;
		}
	}
}
