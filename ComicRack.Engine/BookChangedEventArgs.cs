using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class BookChangedEventArgs : PropertyChangedEventArgs
	{
		public object OldValue
		{
			get;
			private set;
		}

		public object NewValue
		{
			get;
			private set;
		}

		public bool IsComicInfo
		{
			get;
			private set;
		}

		public int Page
		{
			get;
			private set;
		}

		public BookChangedEventArgs(string propertyName, int page, bool isComicInfo)
			: base(propertyName)
		{
			IsComicInfo = isComicInfo;
			Page = page;
		}

		public BookChangedEventArgs(string propertyName, bool isComicInfo)
			: this(propertyName, -1, isComicInfo)
		{
		}

		public BookChangedEventArgs(string propertyName, bool isComicInfo, object oldValue, object newValue)
			: this(propertyName, -1, isComicInfo)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}

		public BookChangedEventArgs(BookChangedEventArgs e)
			: base(e.PropertyName)
		{
			OldValue = e.OldValue;
			NewValue = e.NewValue;
			IsComicInfo = e.IsComicInfo;
			Page = e.Page;
		}
	}
}
