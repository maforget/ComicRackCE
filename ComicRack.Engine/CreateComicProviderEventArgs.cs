using System;
using cYo.Projects.ComicRack.Engine.IO.Provider;

namespace cYo.Projects.ComicRack.Engine
{
	public class CreateComicProviderEventArgs : EventArgs
	{
		public ImageProvider Provider
		{
			get;
			set;
		}
	}
}
