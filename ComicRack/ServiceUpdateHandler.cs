namespace cYo.Projects.ComicRack.Viewer
{
	public delegate bool ServiceUpdateHandler<in T>(T service) where T : class;
}
