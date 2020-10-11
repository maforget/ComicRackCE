using System;
using cYo.Common.Collections;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	public class ComicBookMatcherCollection : SmartList<ComicBookMatcher>
	{
		public void Add(Type matchType, int matchOperator, string matchValue1, string matchValue2)
		{
			Add(ComicBookValueMatcher.Create(matchType, matchOperator, matchValue1, matchValue2));
		}
	}
}
