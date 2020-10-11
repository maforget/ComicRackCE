using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupAlternateNumber : SingleComicGrouper
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			if (item.CompareAlternateNumber.IsNumber)
			{
				return ItemGroupCount.GetNumberGroup((int)item.CompareAlternateNumber.Number);
			}
			return SingleComicGrouper.GetNameGroup(item.CompareAlternateNumber.Text);
		}
	}
}
