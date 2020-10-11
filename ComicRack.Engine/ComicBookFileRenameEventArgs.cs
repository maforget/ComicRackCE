using System;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookFileRenameEventArgs : EventArgs
	{
		private readonly string oldFile;

		private readonly string newFile;

		public string OldFile => oldFile;

		public string NewFile => newFile;

		public ComicBookFileRenameEventArgs(string oldFile, string newFile)
		{
			this.oldFile = oldFile;
			this.newFile = newFile;
		}
	}
}
