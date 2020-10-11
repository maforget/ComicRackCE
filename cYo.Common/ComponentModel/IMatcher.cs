using System.Collections.Generic;

namespace cYo.Common.ComponentModel
{
	public interface IMatcher<T>
	{
		IEnumerable<T> Match(IEnumerable<T> items);
	}
}
