using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupManga : SingleComicGrouper
	{
		private readonly string[] captions = GroupInfo.TRGroup.GetStrings("MangaGroups", "Manga (Right to Left)|Manga|No Manga|Unknown", '|');

		public override IGroupInfo GetGroup(ComicBook item)
		{
			switch (item.Manga)
			{
			case MangaYesNo.YesAndRightToLeft:
				return new GroupInfo(captions[0], 0);
			case MangaYesNo.Yes:
				return new GroupInfo(captions[1], 1);
			case MangaYesNo.No:
				return new GroupInfo(captions[2], 2);
			default:
				return new GroupInfo(captions[3], 3);
			}
		}
	}
}
