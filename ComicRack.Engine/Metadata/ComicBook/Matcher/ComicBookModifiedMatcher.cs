using System;
using System.ComponentModel;
using cYo.Common;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("File Modified")]
	[ComicBookMatcherHint("FileModifiedTime")]
	public class ComicBookModifiedMatcher : ComicBookDateMatcher
	{
		public ComicBookModifiedMatcher()
		{
			base.IgnoreTime = false;
		}

		protected override DateTime GetValue(ComicBook comicBook)
		{
			return comicBook.FileModifiedTime.SafeToLocalTime();
		}
	}
}
