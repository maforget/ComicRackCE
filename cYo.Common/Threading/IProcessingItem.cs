namespace cYo.Common.Threading
{
	public interface IProcessingItem<T> : IProgressState
	{
		T Item
		{
			get;
		}
	}
}
