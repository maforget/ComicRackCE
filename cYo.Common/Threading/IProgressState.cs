namespace cYo.Common.Threading
{
	public interface IProgressState
	{
		bool ProgressAvailable
		{
			get;
			set;
		}

		int ProgressPercentage
		{
			get;
			set;
		}

		string ProgressMessage
		{
			get;
			set;
		}

		bool Abort
		{
			get;
			set;
		}

		ProgressState State
		{
			get;
		}
	}
}
