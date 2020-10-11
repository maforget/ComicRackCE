namespace cYo.Common.ComponentModel
{
	public interface IValueStore<T>
	{
		T GetValue();

		void SetValue(T value);
	}
}
