using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public interface IComicBookGroupMatcher
	{
		MatcherMode MatcherMode
		{
			get;
			set;
		}

		ComicBookMatcherCollection Matchers
		{
			get;
		}
	}
}
