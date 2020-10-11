using System;
using System.Drawing;
using cYo.Common.Drawing;
using cYo.Projects.ComicRack.Engine.IO.Cache;

namespace cYo.Projects.ComicRack.Engine.Display
{
	public interface IComicDisplay : IComicDisplayConfig
	{
		ComicBookNavigator Book
		{
			get;
			set;
		}

		IPagePool PagePool
		{
			get;
			set;
		}

		IThumbnailPool ThumbnailPool
		{
			get;
			set;
		}

		bool IsValid
		{
			get;
		}

		bool IsMovementFlipped
		{
			get;
		}

		int CurrentPage
		{
			get;
		}

		int CurrentMousePage
		{
			get;
		}

		ImageRotation CurrentImageRotation
		{
			get;
		}

		Size ImageSize
		{
			get;
		}

		int ImagePartCount
		{
			get;
		}

		bool IsDoubleImage
		{
			get;
		}

		ImagePartInfo ImageVisiblePart
		{
			get;
			set;
		}

		bool IsHardwareRenderer
		{
			get;
		}

		bool ShouldPagingBlend
		{
			get;
		}

		bool NavigationOverlayVisible
		{
			get;
			set;
		}

		event EventHandler BookChanged;

		event EventHandler DrawnPageCountChanged;

		event EventHandler<BrowseEventArgs> Browse;

		event EventHandler<BookPageEventArgs> PageChange;

		event EventHandler<BookPageEventArgs> PageChanged;

		event EventHandler<GestureEventArgs> Gesture;

		event EventHandler<GestureEventArgs> PreviewGesture;

		event EventHandler VisibleInfoOverlaysChanged;

		bool SetRenderer(bool hardware);

		object GetState();

		void Animate(object a, object b, int time);

		void Animate(Action<float> animate, int time);

		Bitmap CreatePageImage();

		void MovePartDown(float percent);

		bool MovePart(Point offset);

		bool DisplayPart(PartPageToDisplay ptd);

		void DisplayOpenMessage();

		void ZoomTo(Point location, float zoom);
	}
}
