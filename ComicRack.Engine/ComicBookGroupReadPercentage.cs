using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupReadPercentage : SingleComicGrouper
	{
		private static readonly string[] captions = GroupInfo.TRGroup.GetStrings("ReadPercentageGroups", "Not Read|0%|10%|20%|30%|40%|50%|60%|70%|80%|90%|Completed", '|');

		public override IGroupInfo GetGroup(ComicBook item)
		{
			return CreatePercentageGroup(item.ReadPercentage);
		}

		public static IGroupInfo CreatePercentageGroup(int p)
		{
			if (p != 0)
			{
				p = ((p < 100) ? (p / 10 + 1) : (captions.Length - 1));
			}
			return new GroupInfo(captions[p], p);
		}
	}
}
