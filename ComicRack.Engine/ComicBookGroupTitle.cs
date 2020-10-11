using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupTitle : SingleComicGrouper
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			return GroupInfo.GetAlphabetGroup(item.ShadowTitle, articleAware: true);
		}
	}
}
