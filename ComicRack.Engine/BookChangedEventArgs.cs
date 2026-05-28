using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
    public enum ComicInfoType
    {
        None,
        ComicInfo,
        ComicBook
    }

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

        private ComicInfoType ComicInfoType { get; set; }
        public bool IsComicInfo => ComicInfoType == ComicInfoType.ComicInfo;
        public bool IsComicBook => ComicInfoType == ComicInfoType.ComicBook;

        public int Page
        {
            get;
            private set;
        }

        public BookChangedEventArgs(string propertyName, int page, ComicInfoType comicInfoType)
            : base(propertyName)
        {
            ComicInfoType = comicInfoType;
            Page = page;
        }

        public BookChangedEventArgs(string propertyName, ComicInfoType comicInfoType)
            : this(propertyName, -1, comicInfoType)
        {
        }

        public BookChangedEventArgs(string propertyName, ComicInfoType comicInfoType, object oldValue, object newValue)
            : this(propertyName, -1, comicInfoType)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public BookChangedEventArgs(BookChangedEventArgs e)
            : base(e.PropertyName)
        {
            OldValue = e.OldValue;
            NewValue = e.NewValue;
            ComicInfoType = e.ComicInfoType;
            Page = e.Page;
        }
    }
}
