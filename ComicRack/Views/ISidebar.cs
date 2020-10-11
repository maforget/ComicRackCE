using System.Drawing;
using cYo.Projects.ComicRack.Engine.Controls;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public interface ISidebar
	{
		bool Visible
		{
			get;
			set;
		}

		bool Preview
		{
			get;
			set;
		}

		bool Info
		{
			get;
			set;
		}

		bool TopBrowser
		{
			get;
			set;
		}

		bool HasInfoPanels
		{
			get;
		}

		int TopBrowserSplit
		{
			get;
			set;
		}

		bool InfoBrowserRight
		{
			get;
			set;
		}

		Size InfoBrowserSize
		{
			get;
			set;
		}

		void AddInfo(ComicPageControl page);
	}
}
