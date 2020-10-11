using System;

namespace cYo.Common
{
	[Serializable]
	public class StringPair : ValuePair<string, string>
	{
		public StringPair()
		{
		}

		public StringPair(string key, string value)
			: base(key, value)
		{
		}
	}
}
