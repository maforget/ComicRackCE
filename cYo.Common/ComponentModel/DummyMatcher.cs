using System.Collections.Generic;

namespace cYo.Common.ComponentModel
{
	public class DummyMatcher<T> : IMatcher<T>
	{
		public IEnumerable<T> Match(IEnumerable<T> items)
		{
			return items;
		}
	}
}
