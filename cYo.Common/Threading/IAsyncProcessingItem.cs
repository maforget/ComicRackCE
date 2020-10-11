using System;

namespace cYo.Common.Threading
{
	public interface IAsyncProcessingItem<T> : IAsyncResult, IProcessingItem<T>, IProgressState
	{
	}
}
