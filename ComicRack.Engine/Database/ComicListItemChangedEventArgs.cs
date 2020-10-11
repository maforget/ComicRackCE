using System;

namespace cYo.Projects.ComicRack.Engine.Database
{
	public class ComicListItemChangedEventArgs : EventArgs
	{
		public ComicListItem Item
		{
			get;
			private set;
		}

		public ComicListItemChange Change
		{
			get;
			private set;
		}

		public ComicListItemChangedEventArgs(ComicListItem changedItem, ComicListItemChange changeType)
		{
			Change = changeType;
			Item = changedItem;
		}
	}
}
