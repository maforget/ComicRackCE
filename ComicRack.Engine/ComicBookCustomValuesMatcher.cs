using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Custom Value")]
	[ComicBookMatcherHint("CustomValuesStore")]
	public class ComicBookCustomValuesMatcher : ComicBookStringMatcher
	{
		private static readonly string textName = ComicBookMatcher.TRMatcher["Name", "Name"];

		public override int ArgumentCount => 2;

		public override int MatchColumn => 1;

		public override bool SwapOperatorArgument => true;

		public override string UnitDescription => textName;

		protected override bool PreCheck(ComicBook comicBook)
		{
			return true;
		}

		protected override string GetValue(ComicBook comicBook)
		{
			string matchValue = GetMatchValue2(comicBook);
			if (matchValue == null)
			{
				return null;
			}
			return comicBook.GetCustomValue(matchValue);
		}
	}
}
