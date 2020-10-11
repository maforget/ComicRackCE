using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupHasBeenRead : SingleComicGrouper
	{
		private readonly string[] captions = GroupInfo.TRGroup.GetStrings("HasBeenReadGroups", "Read|Not Read", '|');

		public override IGroupInfo GetGroup(ComicBook item)
		{
			if (!item.HasBeenRead)
			{
				return new GroupInfo(captions[1], 1);
			}
			return new GroupInfo(captions[0], 0);
		}
	}
}
