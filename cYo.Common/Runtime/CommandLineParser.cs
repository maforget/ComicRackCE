using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using cYo.Common.Collections;
using cYo.Common.Reflection;

namespace cYo.Common.Runtime
{
	public static class CommandLineParser
	{
		public static IEnumerable<string> Parse(object switches, IEnumerable<string> args, CommandLineParserOptions options = CommandLineParserOptions.UseIni)
		{
			try
			{
				if ((options & CommandLineParserOptions.UseIni) != 0)
				{
					IniFile.UpdateProperties(switches, withCommandLine: true);
				}
			}
			catch (Exception)
			{
			}
			List<string> list = new List<string>();
			IEnumerator<string> enumerator = args.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				if (current.StartsWith("-"))
				{
					try
					{
						ParseSwitch(switches, current.Substring(1), enumerator);
					}
					catch
					{
						if ((options & CommandLineParserOptions.FailOnError) != 0)
						{
							throw;
						}
					}
				}
				else
				{
					list.Add(current);
				}
			}
			if (switches != null)
			{
				SetFiles(switches, list);
			}
			return list;
		}

		public static IEnumerable<string> Parse(object switches, CommandLineParserOptions options)
		{
			return Parse(switches, Environment.GetCommandLineArgs().Skip(1), options);
		}

		public static IEnumerable<string> Parse(object switches)
		{
			return Parse(switches, Environment.GetCommandLineArgs().Skip(1));
		}

		public static T Parse<T>(CommandLineParserOptions options)
		{
			T val = Activator.CreateInstance<T>();
			try
			{
				Parse(val, options);
				return val;
			}
			catch (Exception)
			{
				return val;
			}
		}

		public static T Parse<T>()
		{
			return Parse<T>(CommandLineParserOptions.UseIni);
		}

		private static void ParseSwitch(object switches, string name, IEnumerator<string> args)
		{
			if (switches == null)
			{
				return;
			}
			var anon = (from p in switches.GetType().GetProperties()
				select new
				{
					Attr = p.GetAttribute<CommandLineSwitchAttribute>(),
					Property = p
				} into pinfo
				where pinfo.Attr != null
				select pinfo).FirstOrDefault(pinfo => string.Equals(name, pinfo.Attr.Name, StringComparison.OrdinalIgnoreCase) || string.Equals(name, pinfo.Attr.ShortName, StringComparison.OrdinalIgnoreCase));
			if (anon == null)
			{
				throw new ArgumentException("is not a valid command line switch", name);
			}
			PropertyInfo property = anon.Property;
			try
			{
				if (property.PropertyType == typeof(Action))
				{
					((Action)property.GetValue(switches, null))();
					return;
				}
				if (!property.CanWrite)
				{
					throw new InvalidOperationException("Not valid on a read only property");
				}
				if (property.PropertyType == typeof(bool))
				{
					property.SetValue(switches, !(bool)property.GetValue(switches, null), null);
					return;
				}
				if (!args.MoveNext())
				{
					throw new ArgumentException("switch needs a parameter", name);
				}
				if (new Type[5]
				{
					typeof(string),
					typeof(int),
					typeof(float),
					typeof(double),
					typeof(bool)
				}.Contains(property.PropertyType))
				{
					property.SetValue(switches, Convert.ChangeType(args.Current, property.PropertyType), null);
				}
				else if (property.PropertyType.IsEnum)
				{
					property.SetValue(switches, Enum.Parse(property.PropertyType, args.Current), null);
				}
			}
			catch (Exception innerException)
			{
				throw new ArgumentException("failed to parse command switch value", name, innerException);
			}
		}

		private static void SetFiles(object switches, IEnumerable<string> unnamed)
		{
			PropertyInfo[] properties = switches.GetType().GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (!Attribute.IsDefined(propertyInfo, typeof(CommandLineFilesAttribute)))
				{
					continue;
				}
				(propertyInfo.GetValue(switches, null) as ICollection<string>)?.AddRange(unnamed);
				if (propertyInfo.CanRead && propertyInfo.CanWrite)
				{
					if (propertyInfo.PropertyType == typeof(string[]))
					{
						propertyInfo.SetValue(switches, unnamed.ToArray(), null);
					}
					else if (propertyInfo.PropertyType == typeof(IEnumerable<string>))
					{
						propertyInfo.SetValue(switches, unnamed, null);
					}
				}
			}
		}
	}
}
