using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Drawing;

namespace cYo.Projects.ComicRack.Engine.Display
{
	public interface IComicDisplayConfig
	{
		ImageBackgroundMode ImageBackgroundMode
		{
			get;
			set;
		}

		Color BackColor
		{
			get;
			set;
		}

		string BackgroundTexture
		{
			get;
			set;
		}

		ImageLayout BackgroundImageLayout
		{
			get;
			set;
		}

		string PaperTexture
		{
			get;
			set;
		}

		float PaperTextureStrength
		{
			get;
			set;
		}

		ImageLayout PaperTextureLayout
		{
			get;
			set;
		}

		bool PageMargin
		{
			get;
			set;
		}

		float PageMarginPercentWidth
		{
			get;
			set;
		}

		PageLayoutMode PageLayout
		{
			get;
			set;
		}

		float DoublePageOverlap
		{
			get;
			set;
		}

		bool ImageAutoRotate
		{
			get;
			set;
		}

		ImageRotation ImageRotation
		{
			get;
			set;
		}

		float ImageZoom
		{
			get;
			set;
		}

		bool ImageFitOnlyIfOversized
		{
			get;
			set;
		}

		ImageFitMode ImageFitMode
		{
			get;
			set;
		}

		ImageDisplayOptions ImageDisplayOptions
		{
			get;
			set;
		}

		bool RealisticPages
		{
			get;
			set;
		}

		bool AutoHideCursor
		{
			get;
			set;
		}

		bool LeftRightMovementReversed
		{
			get;
			set;
		}

		bool RightToLeftReading
		{
			get;
			set;
		}

		RightToLeftReadingMode RightToLeftReadingMode
		{
			get;
			set;
		}

		bool TwoPageNavigation
		{
			get;
			set;
		}

		bool BlendWhilePaging
		{
			get;
			set;
		}

		float MagnifierOpacity
		{
			get;
			set;
		}

		Size MagnifierSize
		{
			get;
			set;
		}

		bool MagnifierVisible
		{
			get;
			set;
		}

		float MagnifierZoom
		{
			get;
			set;
		}

		MagnifierStyle MagnifierStyle
		{
			get;
			set;
		}

		bool AutoHideMagnifier
		{
			get;
			set;
		}

		bool AutoMagnifier
		{
			get;
			set;
		}

		float InfoOverlayScaling
		{
			get;
			set;
		}

		InfoOverlays VisibleInfoOverlays
		{
			get;
			set;
		}

		bool SmoothScrolling
		{
			get;
			set;
		}

		PageTransitionEffect PageTransitionEffect
		{
			get;
			set;
		}

		bool DisplayChangeAnimation
		{
			get;
			set;
		}

		bool FlowingMouseScrolling
		{
			get;
			set;
		}

		bool SoftwareFiltering
		{
			get;
			set;
		}

		bool HardwareFiltering
		{
			get;
			set;
		}
	}
}
