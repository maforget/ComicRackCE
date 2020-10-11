using System;
using System.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Mathematics;
using cYo.Projects.ComicRack.Engine.Display;

namespace cYo.Projects.ComicRack.Viewer.Config
{
	[Serializable]
	public class BookPageLayout
	{
		private float pageZoom = 1f;

		[Browsable(false)]
		[DefaultValue(ImageFitMode.FitWidth)]
		public ImageFitMode PageDisplayMode
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool FitOnlyIfOversized
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool AutoRotate
		{
			get;
			set;
		}

		[Browsable(false)]
		[DefaultValue(ImageRotation.None)]
		public ImageRotation PageImageRotation
		{
			get;
			set;
		}

		[Browsable(false)]
		[DefaultValue(PageLayoutMode.Single)]
		public PageLayoutMode PageLayout
		{
			get;
			set;
		}

		[Browsable(false)]
		[DefaultValue(1f)]
		public float PageZoom
		{
			get
			{
				return pageZoom;
			}
			set
			{
				pageZoom = value.Clamp(1f, 8f);
			}
		}

		[DefaultValue(false)]
		public bool TwoPageAutoScroll
		{
			get;
			set;
		}

		public BookPageLayout()
		{
			FitOnlyIfOversized = true;
			PageDisplayMode = ImageFitMode.FitWidth;
		}
	}
}
