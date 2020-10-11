using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	[Description("Series: First Number")]
	[ComicBookMatcherHint("Series", "Volume", "FilePath", "EnableProposed", "Number", DisableOptimizedUpdate = true)]
	[XmlType("SmartListSeriesMinNumbertMatcher")]
	public class SmartListSeriesFirstNumberMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			if (base.StatsProvider != null)
			{
				return base.StatsProvider.GetSeriesStats(comicBook).FirstNumber;
			}
			return 0f;
		}
	}
}
