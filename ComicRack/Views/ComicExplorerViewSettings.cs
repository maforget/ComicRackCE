using System;
using System.ComponentModel;
using System.Drawing;
using System.Xml.Serialization;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	[Serializable]
	public class ComicExplorerViewSettings
	{
		[XmlAttribute]
		[DefaultValue(true)]
		public bool ShowBrowser
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(150)]
		public int BrowserSplit
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(150)]
		public int PreviewSplit
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(150)]
		public int TopBrowserSplit
		{
			get;
			set;
		}

		[DefaultValue(typeof(Size), "200, 150")]
		public Size InfoBrowserSize
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(false)]
		public bool InfoBrowserRight
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(false)]
		public bool ShowPreview
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(false)]
		public bool ShowInfo
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(false)]
		public bool ShowTopBrowser
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(false)]
		public bool ShowSearchBrowser
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(false)]
		public bool ShowSearchBar
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(false)]
		public bool TwoPagePreview
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public ItemViewConfig ItemViewConfig
		{
			get;
			set;
		}

		[DefaultValue(1)]
		public int SearchBrowserColumn1
		{
			get;
			set;
		}

		[DefaultValue(0)]
		public int SearchBrowserColumn2
		{
			get;
			set;
		}

		[DefaultValue(2)]
		public int SearchBrowserColumn3
		{
			get;
			set;
		}

		public ComicExplorerViewSettings()
		{
			SearchBrowserColumn3 = 2;
			SearchBrowserColumn1 = 1;
			TopBrowserSplit = FormUtility.ScaleDpiY(150);
			PreviewSplit = FormUtility.ScaleDpiY(200);
			BrowserSplit = FormUtility.ScaleDpiY(250);
			InfoBrowserSize = new Size(200, 150).ScaleDpi();
			ShowBrowser = true;
		}
	}
}
