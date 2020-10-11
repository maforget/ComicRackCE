using System;

namespace cYo.Common.Runtime
{
	public class BarkEventArgs : EventArgs
	{
		public BarkType Bark
		{
			get;
			set;
		}

		public Exception Exception
		{
			get;
			set;
		}

		public BarkEventArgs(BarkType bark, Exception e)
		{
			Bark = bark;
			Exception = e;
		}
	}
}
