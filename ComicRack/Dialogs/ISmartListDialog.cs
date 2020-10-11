using System;
using cYo.Projects.ComicRack.Engine.Database;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public interface ISmartListDialog
	{
		ComicLibrary Library
		{
			get;
			set;
		}

		Guid EditId
		{
			get;
			set;
		}

		ComicSmartListItem SmartComicList
		{
			get;
			set;
		}

		bool EnableNavigation
		{
			get;
			set;
		}

		bool PreviousEnabled
		{
			get;
			set;
		}

		bool NextEnabled
		{
			get;
			set;
		}

		event EventHandler Apply;

		event EventHandler Next;

		event EventHandler Previous;
	}
}
