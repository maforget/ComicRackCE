using System.Collections.Generic;
using System.Linq;

namespace cYo.Common.ComponentModel
{
	public class MatcherSet<T> : IMatcher<T>
	{
		private readonly List<MatcherSetItem<T>> matchers = new List<MatcherSetItem<T>>();

		public List<MatcherSetItem<T>> Matchers => matchers;

		public void Add(IMatcher<T> matcher, MatcherMode mode, bool not)
		{
			if (matcher != null)
			{
				matchers.Add(new MatcherSetItem<T>(mode, not, matcher));
			}
		}

		public void And(IMatcher<T> matcher, bool not = false)
		{
			Add(matcher, MatcherMode.And, not);
		}

		public void AndNot(IMatcher<T> matcher)
		{
			And(matcher, not: true);
		}

		public void Or(IMatcher<T> matcher, bool not = false)
		{
			Add(matcher, MatcherMode.Or, not);
		}

		public void OrNot(IMatcher<T> matcher)
		{
			Or(matcher, not: true);
		}

		public void And(IEnumerable<IMatcher<T>> em, bool not)
		{
			foreach (IMatcher<T> item in em)
			{
				And(item, not);
			}
		}

		public void And(IEnumerable<IMatcher<T>> em)
		{
			And(em, not: false);
		}

		public void AndNot(IEnumerable<IMatcher<T>> em)
		{
			And(em, not: true);
		}

		public void Or(IEnumerable<IMatcher<T>> em, bool not = false)
		{
			foreach (IMatcher<T> item in em)
			{
				Or(item, not);
			}
		}

		public void OrNot(IEnumerable<IMatcher<T>> em)
		{
			Or(em, not: true);
		}

		public IEnumerable<T> Match(IEnumerable<T> items)
		{
			return Match(Matchers, items);
		}

		public static IEnumerable<T> Match(IEnumerable<MatcherSetItem<T>> matchers, IEnumerable<T> items)
		{
			IEnumerable<T> enumerable = null;
			foreach (MatcherSetItem<T> matcher in matchers)
			{
				switch (matcher.Mode)
				{
				case MatcherMode.And:
				{
					IEnumerable<T> enumerable4 = enumerable ?? items;
					IEnumerable<T> enumerable5 = matcher.Matcher.Match(enumerable4).ToArray();
					IEnumerable<T> enumerable6;
					if (!matcher.Not)
					{
						enumerable6 = enumerable5;
					}
					else
					{
						IEnumerable<T> enumerable7 = enumerable4.Except(enumerable5).ToArray();
						enumerable6 = enumerable7;
					}
					enumerable = enumerable6;
					break;
				}
				case MatcherMode.Or:
				{
					IEnumerable<T> enumerable2 = ((enumerable == null) ? items : items.Except(enumerable));
					IEnumerable<T> enumerable3 = matcher.Matcher.Match(enumerable2).ToArray();
					if (matcher.Not)
					{
						enumerable3 = enumerable2.Except(enumerable3).ToArray();
					}
					enumerable = ((enumerable == null) ? enumerable3 : enumerable.Concat(enumerable3));
					break;
				}
				}
			}
			return enumerable;
		}
	}
}
