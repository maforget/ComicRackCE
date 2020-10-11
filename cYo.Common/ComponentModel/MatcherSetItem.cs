namespace cYo.Common.ComponentModel
{
	public class MatcherSetItem<T>
	{
		public bool Not
		{
			get;
			set;
		}

		public MatcherMode Mode
		{
			get;
			set;
		}

		public IMatcher<T> Matcher
		{
			get;
			set;
		}

		public MatcherSetItem(MatcherMode mode, bool not, IMatcher<T> matcher)
		{
			Mode = mode;
			Not = not;
			Matcher = matcher;
		}
	}
}
