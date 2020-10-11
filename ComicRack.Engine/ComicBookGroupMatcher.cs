using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	public class ComicBookGroupMatcher : ComicBookMatcher, IComicBookGroupMatcher
	{
		private MatcherMode matcherMode;

		private readonly ComicBookMatcherCollection matchers = new ComicBookMatcherCollection();

		public override bool TimeDependant => Matchers.Any((ComicBookMatcher m) => m.TimeDependant);

		[XmlAttribute]
		[DefaultValue(MatcherMode.And)]
		public MatcherMode MatcherMode
		{
			get
			{
				return matcherMode;
			}
			set
			{
				matcherMode = value;
			}
		}

		public ComicBookMatcherCollection Matchers => matchers;

		[XmlAttribute]
		[DefaultValue(false)]
		public bool Collapsed
		{
			get;
			set;
		}

		public override string ToString()
		{
			return (base.Not ? "Not " : string.Empty) + ConvertParametersToQuery(this);
		}

		public override IEnumerable<ComicBook> Match(IEnumerable<ComicBook> items)
		{
			if (Matchers.Count == 0)
			{
				return items;
			}
			MatcherSet<ComicBook> matcherSet = new MatcherSet<ComicBook>();
			foreach (ComicBookMatcher matcher in Matchers)
			{
				matcherSet.Add(matcher, MatcherMode, matcher.Not);
			}
			return matcherSet.Match(items);
		}

		public override object Clone()
		{
			ComicBookGroupMatcher comicBookGroupMatcher = new ComicBookGroupMatcher
			{
				Not = base.Not,
				MatcherMode = MatcherMode,
				Collapsed = Collapsed
			};
			comicBookGroupMatcher.Matchers.AddRange(Matchers.Select((ComicBookMatcher matcher) => matcher.Clone() as ComicBookMatcher));
			return comicBookGroupMatcher;
		}

		public override bool IsSame(ComicBookMatcher cbm)
		{
			ComicBookGroupMatcher comicBookGroupMatcher = cbm as ComicBookGroupMatcher;
			if (comicBookGroupMatcher != null && base.IsSame(cbm) && comicBookGroupMatcher.MatcherMode == MatcherMode && comicBookGroupMatcher.Collapsed == Collapsed)
			{
				return Matchers.SequenceEqual(Matchers);
			}
			return false;
		}

		public override IEnumerable<string> GetDependentProperties()
		{
			return Matchers.SelectMany((ComicBookMatcher m) => m.GetDependentProperties());
		}

		public override bool UsesProperty(string propertyHint)
		{
			return Matchers.Any((ComicBookMatcher m) => m.UsesProperty(propertyHint));
		}

		public ComicBookMatcher Optimized()
		{
			if (Matchers.Count == 0)
			{
				return null;
			}
			if (Matchers.Count == 1)
			{
				return Matchers[0];
			}
			return this;
		}

		public static void ConvertQueryToParamerters(IComicBookGroupMatcher gm, Tokenizer tokens)
		{
			tokens.Expect("MATCH");
			gm.MatcherMode = MatcherMode.And;
			if (tokens.IsOptional("ANY"))
			{
				gm.MatcherMode = MatcherMode.Or;
				tokens.Skip();
			}
			else if (tokens.IsOptional("ALL"))
			{
				tokens.Skip();
			}
			bool flag = tokens.IsOptional("{");
			if (flag)
			{
				tokens.Skip();
			}
			gm.Matchers.Clear();
			while (true)
			{
				if (flag && tokens.Is("}"))
				{
					tokens.Skip();
					break;
				}
				gm.Matchers.Add(CreateMatcherFromQuery(tokens));
				if (flag)
				{
					if (tokens.Is("}"))
					{
						tokens.Skip();
						break;
					}
					tokens.Expect(",");
					continue;
				}
				break;
			}
		}

		public static ComicBookMatcher CreateMatcherFromQuery(Tokenizer tokens)
		{
			bool not = false;
			if (tokens.IsOptional("NOT"))
			{
				not = true;
				tokens.Skip();
			}
			if (tokens.Is("MATCH"))
			{
				ComicBookGroupMatcher comicBookGroupMatcher = new ComicBookGroupMatcher
				{
					Not = not
				};
				ConvertQueryToParamerters(comicBookGroupMatcher, tokens);
				return comicBookGroupMatcher;
			}
			Tokenizer.Token token = tokens.Take("[", "]");
			token.Text = token.Text.Unescape("[]", '\\');
			IComicBookValueMatcher comicBookValueMatcher = ComicBookValueMatcher.Create(token.Text);
			if (comicBookValueMatcher == null)
			{
				token.ThrowParserException("Invalid name {0} encountered");
			}
			comicBookValueMatcher.Not = not;
			Tokenizer.Token op = tokens.Expect(comicBookValueMatcher.OperatorsListNeutral);
			int num = comicBookValueMatcher.OperatorsListNeutral.FindIndex((string o) => string.Equals(o, op.Text, StringComparison.OrdinalIgnoreCase));
			if (num == -1)
			{
				token.ThrowParserException("Invalid operator {0} encountered");
			}
			comicBookValueMatcher.MatchOperator = num;
			for (int i = 0; i < comicBookValueMatcher.ArgumentCount; i++)
			{
				if (i == 0)
				{
					comicBookValueMatcher.MatchValue = tokens.TakeString().Text;
				}
				else
				{
					comicBookValueMatcher.MatchValue2 = tokens.TakeString().Text;
				}
			}
			return comicBookValueMatcher as ComicBookMatcher;
		}

		public static string ConvertParametersToQuery(IComicBookGroupMatcher gm, bool format = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int count = gm.Matchers.Count;
			stringBuilder.Append("Match");
			if (count > 0)
			{
				stringBuilder.Append(" ");
				if (count > 1)
				{
					switch (gm.MatcherMode)
					{
					case MatcherMode.And:
						stringBuilder.Append("All");
						break;
					case MatcherMode.Or:
						stringBuilder.Append("Any");
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
					if (format)
					{
						stringBuilder.Append(Environment.NewLine);
					}
					else
					{
						stringBuilder.Append(' ');
					}
					stringBuilder.Append("{");
					if (format)
					{
						stringBuilder.Append(Environment.NewLine);
					}
				}
				for (int i = 0; i < count; i++)
				{
					ComicBookMatcher comicBookMatcher = gm.Matchers[i];
					string text = comicBookMatcher.ToString();
					if (format && count > 1)
					{
						text = text.Intent(4);
					}
					stringBuilder.Append(text);
					if (i != count - 1 && count > 1)
					{
						stringBuilder.Append(",");
						stringBuilder.Append(format ? Environment.NewLine : " ");
					}
				}
				if (count > 1)
				{
					if (format)
					{
						stringBuilder.Append(Environment.NewLine);
					}
					stringBuilder.Append("}");
				}
			}
			return stringBuilder.ToString();
		}
	}
}
