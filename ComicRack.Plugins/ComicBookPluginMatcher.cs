using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Projects.ComicRack.Engine;

namespace cYo.Projects.ComicRack.Plugins
{
	[Serializable]
	[Description("User Scripts")]
	[ComicBookMatcherHint(true)]
	public class ComicBookPluginMatcher : ComicBookValueMatcher
	{
		public static PluginEngine PluginEngine
		{
			get;
			set;
		}

		public static IEnumerable<Command> Commands
		{
			get
			{
				try
				{
					return PluginEngine.GetCommands(PluginEngine.ScriptTypeCreateBookList);
				}
				catch
				{
					return Enumerable.Empty<Command>();
				}
			}
		}

		[XmlAttribute]
		[DefaultValue(null)]
		public string PluginKey
		{
			get;
			set;
		}

		public override int ArgumentCount
		{
			get
			{
				try
				{
					return Commands.FirstOrDefault((Command cmd) => cmd.Key == PluginKey).PCount.Clamp(0, 2);
				}
				catch
				{
					return 0;
				}
			}
		}

		public override string[] OperatorsList
		{
			get
			{
				List<string> list = new List<string>();
				list.Add(TR.Default["None", "None"]);
				list.AddRange(Commands.Select((Command cmd) => cmd.GetLocalizedName()));
				return list.ToArray();
			}
		}

		public override string[] OperatorsListNeutral => OperatorsList;

		[XmlAttribute]
		[DefaultValue(0)]
		public override int MatchOperator
		{
			get
			{
				int num = 1;
				foreach (Command command in Commands)
				{
					if (command.Key == PluginKey)
					{
						return num;
					}
					num++;
				}
				return 0;
			}
			set
			{
				base.MatchOperator = value;
			}
		}

		public override object Clone()
		{
			ComicBookPluginMatcher comicBookPluginMatcher = (ComicBookPluginMatcher)base.Clone();
			comicBookPluginMatcher.PluginKey = PluginKey;
			return comicBookPluginMatcher;
		}

		public override bool Set(ComicBookValueMatcher matcher)
		{
			bool flag = base.Set(matcher);
			ComicBookPluginMatcher comicBookPluginMatcher = matcher as ComicBookPluginMatcher;
			if (flag && comicBookPluginMatcher != null)
			{
				PluginKey = comicBookPluginMatcher.PluginKey;
			}
			return flag;
		}

		protected override void OnMatchOperatorChanged()
		{
			base.OnMatchOperatorChanged();
			try
			{
				if (base.MatchOperator == 0)
				{
					PluginKey = null;
				}
				else
				{
					PluginKey = Commands.ElementAt(base.MatchOperator - 1).Key;
				}
			}
			catch
			{
				PluginKey = null;
			}
		}

		public override IEnumerable<ComicBook> Match(IEnumerable<ComicBook> items)
		{
			try
			{
				Command command = Commands.FirstOrDefault((Command c) => c.Key == PluginKey);
				if (command == null)
				{
					return items;
				}
				object obj = command.Invoke(new object[3]
				{
					items.ToArray(),
					MatchValue,
					MatchValue2
				});
				return (IEnumerable<ComicBook>)obj;
			}
			catch
			{
				return Enumerable.Empty<ComicBook>();
			}
		}
	}
}
