using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupMainCharacterOrTeam : ComicBookStringGrouper<ComicBookMainCharacterOrTeamMatcher>
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			return SingleComicGrouper.GetNameGroup(item.MainCharacterOrTeam);
		}
	}
}
