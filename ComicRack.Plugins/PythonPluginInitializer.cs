using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using cYo.Common.IO;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Plugins
{
	public class PythonPluginInitializer : PluginInitializer
	{
		private static readonly Regex rxComment = new Regex("#\\s*@(?<name>[A-Za-z][\\w_]*)\\s+(?<value>.*)", RegexOptions.Compiled);

		private static readonly Regex rxFunction = new Regex("def\\s+(?<function>[A-Za-z][\\w_]+)", RegexOptions.Compiled);

		public override IEnumerable<Command> GetCommands(string file)
		{
			List<Command> list = new List<Command>();
			try
			{
				if (!".py".Equals(Path.GetExtension(file), StringComparison.OrdinalIgnoreCase))
				{
					return list.ToArray();
				}
				PythonCommand pythonCommand = null;
				foreach (string item in FileUtility.ReadLines(file).TrimStrings().RemoveEmpty())
				{
					Match match = rxComment.Match(item);
					Match match2 = rxFunction.Match(item);
					if (match.Success)
					{
						if (pythonCommand == null)
						{
							pythonCommand = new PythonCommand
							{
								ScriptFile = Path.GetFileName(file)
							};
						}
						try
						{
							string value = match.Groups[1].Value;
							PropertyInfo property = pythonCommand.GetType().GetProperty(value);
							object value2 = Convert.ChangeType(match.Groups[2].Value, property.PropertyType);
							property.SetValue(pythonCommand, value2, null);
						}
						catch
						{
						}
					}
					if (match2.Success && pythonCommand != null)
					{
						pythonCommand.Method = match2.Groups[1].Value;
						list.Add(pythonCommand);
						pythonCommand = null;
					}
				}
				return list;
			}
			catch (Exception)
			{
				return list;
			}
		}
	}
}
