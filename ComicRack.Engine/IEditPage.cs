using cYo.Common.Drawing;

namespace cYo.Projects.ComicRack.Engine
{
	public interface IEditPage
	{
		bool IsValid
		{
			get;
		}

		ComicPageType PageType
		{
			get;
			set;
		}

		ImageRotation Rotation
		{
			get;
			set;
		}
	}
}
