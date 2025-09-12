namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupPages : ItemGroupCount
	{
		protected override int GetInt(ComicBook item)
		{
			return item.PageCount;
		}
	}
}
