namespace cYo.Projects.ComicRack.Viewer
{
	public delegate void ServiceCommandHandler<in T>(T service) where T : class;
}
