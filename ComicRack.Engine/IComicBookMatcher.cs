using System;
using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public interface IComicBookMatcher : IMatcher<ComicBook>, ICloneable
	{
		bool Not
		{
			get;
			set;
		}
	}
}
