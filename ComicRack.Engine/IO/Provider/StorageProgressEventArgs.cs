using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	public class StorageProgressEventArgs : CancelEventArgs
	{
		private readonly int percentDone;

		public int PercentDone => percentDone;

		public StorageProgressEventArgs(int percentDone)
		{
			this.percentDone = percentDone;
		}
	}
}
