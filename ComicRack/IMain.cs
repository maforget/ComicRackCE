using System;
using System.Collections.Generic;
using System.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.Display;
using cYo.Projects.ComicRack.Engine.IO;
using cYo.Projects.ComicRack.Engine.Sync;

namespace cYo.Projects.ComicRack.Viewer
{
	public interface IMain : IContainerControl
	{
		Control Control
		{
			get;
		}

		NavigatorManager OpenBooks
		{
			get;
		}

		ComicDisplay ComicDisplay
		{
			get;
		}

		bool BrowserVisible
		{
			get;
			set;
		}

		bool ReaderUndocked
		{
			get;
			set;
		}

		bool MinimalGui
		{
			get;
			set;
		}

		DockStyle BrowserDock
		{
			get;
			set;
		}

		void ConvertComic(IEnumerable<ComicBook> books, ExportSetting setting);

		void ShowInfo();

		bool ShowBookInList(ComicLibrary library, ComicListItem list, ComicBook cb, bool switchToList);

		IEditRating GetRatingEditor();

		IEditPage GetPageEditor();

		void EditListLayout();

		void SaveListLayout();

		void EditListLayouts();

		void UpdateListConfigMenus(ToolStripItemCollection items);

		void ShowComic();

		void UpdateWebComic(ComicBook comic, bool fullRefresh);

		void StoreWorkspace();

		void ToggleBrowser();

		void ShowPortableDevices(DeviceSyncSettings dss, Guid? guid);
	}
}
