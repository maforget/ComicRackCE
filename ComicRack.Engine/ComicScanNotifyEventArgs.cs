using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicScanNotifyEventArgs : CancelEventArgs
	{
		public bool ClearQueue
		{
			get;
			set;
		}

		public bool IgnoreFile
		{
			get;
			set;
		}

		public string File
		{
			get;
			private set;
		}

		public ComicScanNotifyEventArgs(string file)
		{
			File = file;
			ClearQueue = true;
		}
	}
}
