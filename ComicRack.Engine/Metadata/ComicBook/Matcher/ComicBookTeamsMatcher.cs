using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Teams")]
	[ComicBookMatcherHint("Teams")]
	public class ComicBookTeamsMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.Teams;
		}
	}
}
