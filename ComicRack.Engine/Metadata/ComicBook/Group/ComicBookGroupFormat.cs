using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupFormat : ComicBookStringGrouper<ComicBookFormatMatcher>
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			return new GroupInfo(string.IsNullOrEmpty(item.ShadowFormat) ? GroupInfo.Unspecified : item.ShadowFormat);
		}
	}
}
