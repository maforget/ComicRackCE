using System;
using System.Drawing;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	public class ImageRetrievedEventArgs : EventArgs
	{
		private readonly Image image;

		private readonly int index;

		public Image Image => image;

		public int Index => index;

		public ImageRetrievedEventArgs(int index, Image image)
		{
			this.image = image;
			this.index = index;
		}
	}
}
