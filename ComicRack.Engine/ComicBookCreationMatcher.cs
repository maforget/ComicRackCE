using System;
using System.ComponentModel;
using cYo.Common;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("File Created")]
	[ComicBookMatcherHint("FileCreationTime")]
	public class ComicBookCreationMatcher : ComicBookDateMatcher
	{
		public ComicBookCreationMatcher()
		{
			base.IgnoreTime = false;
		}

		protected override DateTime GetValue(ComicBook comicBook)
		{
			return comicBook.FileCreationTime.SafeToLocalTime();
		}
	}
}
