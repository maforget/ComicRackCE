using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public abstract class ItemGroupCount : SingleComicGrouper
	{
		private static readonly string[] captions = GroupInfo.TRGroup.GetStrings("CountGroups", "0-20|21-50|51-100|101-200|201-500|501-1000|>1000|Unspecified", '|');

		public override IGroupInfo GetGroup(ComicBook item)
		{
			int @int = GetInt(item);
			return GetNumberGroup(@int);
		}

		protected abstract int GetInt(ComicBook item);

		public static IGroupInfo GetNumberGroup(int n)
		{
			int num = 0;
			if (n < 0)
			{
				num = captions.Length - 1;
			}
			else
			{
				if (n > 20)
				{
					num++;
				}
				if (n > 50)
				{
					num++;
				}
				if (n > 100)
				{
					num++;
				}
				if (n > 200)
				{
					num++;
				}
				if (n > 500)
				{
					num++;
				}
				if (n > 1000)
				{
					num++;
				}
			}
			return new GroupInfo(captions[num], num);
		}
	}
}
