using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Main Character/Team")]
	[ComicBookMatcherHint("MainCharacterOrTeam")]
	public class ComicBookMainCharacterOrTeamMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.MainCharacterOrTeam;
		}
	}
}
