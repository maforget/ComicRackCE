using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupYear : SingleComicGrouper
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			string shadowYearAsText = item.ShadowYearAsText;
			return new GroupInfo(string.IsNullOrEmpty(shadowYearAsText) ? GroupInfo.Unspecified : shadowYearAsText);
		}
	}
}
