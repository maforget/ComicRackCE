using cYo.Common.ComponentModel;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupVolume : SingleComicGrouper
	{
		private readonly string[] captions = GroupInfo.TRGroup.GetStrings("VolumeGroups", "None|V{0}|Other", '|');

		public override IGroupInfo GetGroup(ComicBook item)
		{
			int shadowVolume = item.ShadowVolume;
			if (shadowVolume < 0)
			{
				return new GroupInfo(captions[0], 0);
			}
			if (shadowVolume > 10000)
			{
				return new GroupInfo(captions[2], 1002);
			}
			return new GroupInfo(StringUtility.Format(captions[1], shadowVolume), shadowVolume + 1);
		}

		public override ComicBookMatcher CreateMatcher(IGroupInfo info)
		{
			ComicBookVolumeMatcher comicBookVolumeMatcher = new ComicBookVolumeMatcher();
			if (info.Index == 0)
			{
				return comicBookVolumeMatcher;
			}
			if (info.Index == 1002)
			{
				comicBookVolumeMatcher.MatchOperator = ComicBookNumericMatcher.Greater;
				comicBookVolumeMatcher.MatchValue = "10000";
			}
			else
			{
				comicBookVolumeMatcher.MatchOperator = ComicBookNumericMatcher.Equal;
				comicBookVolumeMatcher.MatchValue = (info.Index - 1).ToString();
			}
			return comicBookVolumeMatcher;
		}
	}
}
