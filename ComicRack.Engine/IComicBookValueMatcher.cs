using System;
using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public interface IComicBookValueMatcher : IComicBookMatcher, IMatcher<ComicBook>, ICloneable
	{
		string Description
		{
			get;
		}

		string DescriptionNeutral
		{
			get;
		}

		string MatchValue
		{
			get;
			set;
		}

		string MatchValue2
		{
			get;
			set;
		}

		int MatchOperator
		{
			get;
			set;
		}

		string[] OperatorsListNeutral
		{
			get;
		}

		string[] OperatorsList
		{
			get;
		}

		int ArgumentCount
		{
			get;
		}

		string UnitDescription
		{
			get;
		}

		bool SwapOperatorArgument
		{
			get;
		}
	}
}
