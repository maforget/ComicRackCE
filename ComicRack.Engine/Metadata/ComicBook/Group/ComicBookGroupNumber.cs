using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupNumber : SingleComicGrouper
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			if (item.CompareNumber.IsNumber)
			{
				return ItemGroupCount.GetNumberGroup((int)item.CompareNumber.Number);
			}
			return SingleComicGrouper.GetNameGroup(item.CompareNumber.Text);
		}
	}
}
