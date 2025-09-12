using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Scanning Information")]
	[ComicBookMatcherHint("ScanInformation")]
	public class ComicBookScanInformationMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.ScanInformation;
		}
	}
}
