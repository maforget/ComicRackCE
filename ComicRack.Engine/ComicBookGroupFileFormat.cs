using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupFileFormat : ComicBookStringGrouper<ComicBookFileFormatMatcher>
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			return new GroupInfo(string.IsNullOrEmpty(item.FileFormat) ? GroupInfo.Unspecified : item.FileFormat);
		}
	}
}
