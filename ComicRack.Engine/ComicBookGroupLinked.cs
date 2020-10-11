using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupLinked : SingleComicGrouper
	{
		private readonly string[] captions = GroupInfo.TRGroup.GetStrings("LinkedGroups", "Linked to File|Not linked to File", '|');

		public override IGroupInfo GetGroup(ComicBook item)
		{
			if (!item.IsLinked)
			{
				return new GroupInfo(captions[1], 1);
			}
			return new GroupInfo(captions[0], 0);
		}
	}
}
