using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupActualFileFormat : ComicBookStringGrouper<ComicBookFileFormatMatcher>
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			return new GroupInfo(string.IsNullOrEmpty(item.ActualFileFormat) ? GroupInfo.Unspecified : item.ActualFileFormat);
		}
	}
}
