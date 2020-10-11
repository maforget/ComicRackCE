using cYo.Common.ComponentModel;
using cYo.Common.Localize;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupReleased : SingleComicGrouper
	{
		private static readonly string UnknownText = TR.Default["Unknown"];

		public override IGroupInfo GetGroup(ComicBook item)
		{
			return GroupInfo.GetDateGroup(item.ReleasedTime, UnknownText);
		}
	}
}
