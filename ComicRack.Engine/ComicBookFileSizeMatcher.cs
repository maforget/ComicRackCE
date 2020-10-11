using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("File Size")]
	[Category("File")]
	[ComicBookMatcherHint("FileSize")]
	public class ComicBookFileSizeMatcher : ComicBookNumericMatcher
	{
		private static readonly string textMB = ComicBookMatcher.TRMatcher["MB", "MB"];

		public override string UnitDescription => textMB;

		protected override float GetValue(ComicBook comicBook)
		{
			return (float)comicBook.FileSize / 1024f / 1024f;
		}
	}
}
