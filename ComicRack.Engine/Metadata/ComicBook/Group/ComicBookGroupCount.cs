namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupCount : ItemGroupCount
	{
		protected override int GetInt(ComicBook item)
		{
			return item.ShadowCount;
		}
	}
}
