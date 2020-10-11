using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupBookPrice : SingleComicGrouper
	{
		private readonly string[] captions = GroupInfo.TRGroup.GetStrings("PriceGroups", "Unknown|Free|0-10|10-20|20-30|30-40|40-50|50-100|over 100", '|');

		public override IGroupInfo GetGroup(ComicBook item)
		{
			int num = ((!(item.BookPrice < 0f)) ? ((item.BookPrice == 0f) ? 1 : ((item.BookPrice >= 0f && item.BookPrice < 10f) ? 2 : ((item.BookPrice >= 10f && item.BookPrice < 20f) ? 3 : ((item.BookPrice >= 20f && item.BookPrice < 30f) ? 4 : ((item.BookPrice >= 30f && item.BookPrice < 40f) ? 5 : ((item.BookPrice >= 40f && item.BookPrice < 50f) ? 6 : ((!(item.BookPrice >= 50f) || !(item.BookPrice < 100f)) ? 8 : 7))))))) : 0);
			return new GroupInfo(captions[num], num);
		}
	}
}
