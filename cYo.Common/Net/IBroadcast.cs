using System;

namespace cYo.Common.Net
{
	public interface IBroadcast<T>
	{
		event EventHandler<BroadcastEventArgs<T>> Recieved;

		bool Broadcast(T data);
	}
}
