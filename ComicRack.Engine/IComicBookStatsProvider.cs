namespace cYo.Projects.ComicRack.Engine
{
	public interface IComicBookStatsProvider
	{
		ComicBookSeriesStatistics GetSeriesStats(ComicBook book);
	}
}
