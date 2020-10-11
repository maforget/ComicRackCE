namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupAlternateCount : ItemGroupCount
	{
		protected override int GetInt(ComicBook item)
		{
			return item.AlternateCount;
		}
	}
}
