namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupOpenCount : ItemGroupCount
	{
		protected override int GetInt(ComicBook item)
		{
			return item.OpenedCount;
		}
	}
}
