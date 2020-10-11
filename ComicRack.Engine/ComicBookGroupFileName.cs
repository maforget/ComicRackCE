using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupFileName : SingleComicGrouper
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			return GroupInfo.GetAlphabetGroup(item.FileName, articleAware: false);
		}
	}
}
