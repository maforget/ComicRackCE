using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.IO;
using cYo.Common.Text;

namespace cYo.Common.Runtime
{
	public class IniFile : DisposableObject
	{
		public const string MultiInitFileSeparator = "|";

		private FileSystemWatcher[] fsw;

		private readonly Dictionary<string, string> values = new Dictionary<string, string>();

		private static readonly Regex rxCommand = new Regex("[/-](?<switch>[a-z]+)[:=](?<value>.+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static List<string> extraDefaultLocation;

		private static string defaultIniFile;

		private static IniFile defaultIni;

		public Dictionary<string, string> Values => values;

		public static string StartupFolder => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

		public static string CommonApplicationDataFolder => MakeApplicationPath(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));

		public static string ApplicationDataFolder => MakeApplicationPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

		public static string DefaultIniFile
		{
			get
			{
				if (defaultIniFile == null)
				{
					try
					{
						string file = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) + ".ini";
						defaultIniFile = GetDefaultLocations(file).ToListString(MultiInitFileSeparator);
					}
					catch (Exception)
					{
						defaultIniFile = string.Empty;
					}
				}
				return defaultIniFile;
			}
		}

		public static IniFile Default
		{
			get
			{
				if (defaultIni == null)
				{
					defaultIni = new IniFile(DefaultIniFile);
				}
				return defaultIni;
			}
		}

		public event EventHandler ValuesChanged;

		public IniFile(string file = null, string section = null, object data = null)
		{
			if (!string.IsNullOrEmpty(file))
			{
				InitializeFile(file, section);
				Register(data);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				CloseWatcher();
			}
			base.Dispose(disposing);
		}

		public void InitializeFile(string file, string section = null)
		{
			CloseWatcher();
			try
			{
				UpdateValues(ReadFile(file, section));
				List<FileSystemWatcher> list = new List<FileSystemWatcher>();
				try
				{
					foreach (string file2 in GetFiles(file))
					{
						string watchFile = file2;
						FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(watchFile))
						{
							EnableRaisingEvents = true,
							NotifyFilter = NotifyFilters.LastWrite
						};
						fileSystemWatcher.Changed += delegate(object x, FileSystemEventArgs y)
						{
							if (y.FullPath == watchFile)
							{
								UpdateValues(ReadFile(watchFile, section));
							}
						};
						list.Add(fileSystemWatcher);
					}
				}
				catch (Exception)
				{
				}
				fsw = list.ToArray();
			}
			catch (Exception)
			{
			}
		}

		public T GetValue<T>(string name, T def = default(T))
		{
			return GetValue(values, name, def);
		}

		public T Register<T>(IniFileRegisterOptions options) where T : class
		{
			T val = Activator.CreateInstance<T>();
			Register(val, options);
			return val;
		}

		public T Register<T>() where T : class
		{
			return Register<T>(IniFileRegisterOptions.ReadCommandLine);
		}

		public void Register(object data, IniFileRegisterOptions options = IniFileRegisterOptions.ReadCommandLine)
		{
			if (data == null)
			{
				return;
			}
			if (options.IsSet(IniFileRegisterOptions.ReadCommandLine))
			{
				UpdateValues(ReadCommandLine());
			}
			UpdateProperties(data, values);
			if (options.IsSet(IniFileRegisterOptions.WatchIniFile))
			{
				ValuesChanged += delegate
				{
					UpdateProperties(data, Values);
				};
			}
		}

		protected virtual void OnValuesChanged()
		{
			if (this.ValuesChanged != null)
			{
				this.ValuesChanged(this, EventArgs.Empty);
			}
		}

		private void CloseWatcher()
		{
			if (fsw != null)
			{
				fsw.Dispose();
				fsw = null;
			}
		}

		private void UpdateValues(IDictionary<string, string> values)
		{
			try
			{
				this.values.AddRange(values);
				OnValuesChanged();
			}
			catch
			{
			}
		}

		private static PropertyInfo GetProperty(Type t, string name)
		{
			PropertyInfo[] properties = t.GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				string name2 = propertyInfo.Name;
				if (!propertyInfo.CanWrite)
				{
					continue;
				}
				IniFileAttribute iniFileAttribute = Attribute.GetCustomAttribute(propertyInfo, typeof(IniFileAttribute)) as IniFileAttribute;
				if (iniFileAttribute == null || iniFileAttribute.Enabled)
				{
					if (iniFileAttribute != null && !string.IsNullOrEmpty(iniFileAttribute.Name))
					{
						name2 = iniFileAttribute.Name;
					}
					if (name2.Equals(name, StringComparison.OrdinalIgnoreCase))
					{
						return propertyInfo;
					}
				}
			}
			return null;
		}

		public static object GetValue(Dictionary<string, string> values, string name, Type type, object def)
		{
			return GetValue(values, name, TypeDescriptor.GetConverter(type), def);
		}

		public static object GetValue(Dictionary<string, string> values, string name, TypeConverter converter, object def)
		{
			if (!values.TryGetValue(name, out var value))
			{
				return def;
			}
			try
			{
				return converter.ConvertFromInvariantString(value);
			}
			catch (InvalidCastException)
			{
				return def;
			}
		}

		public static T GetValue<T>(Dictionary<string, string> values, string name, T def)
		{
			return (T)GetValue(values, name, typeof(T), def);
		}

		public static T GetValue<T>(string file, string name, T defaultValue, string section = null)
		{
			try
			{
				if (!FileExists(file))
				{
					return defaultValue;
				}
				return GetValue(ReadFile(file, section), name, defaultValue);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}

		public static Dictionary<string, string> GetValues(TextReader tr, string section = null)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			bool flag = !string.IsNullOrEmpty(section);
			bool flag2 = !flag;
			foreach (string item in from s in tr.ReadLines().TrimStrings().RemoveEmpty()
				where !s.StartsWith(";") && !s.StartsWith("#")
				select s)
			{
				if (item.StartsWith("["))
				{
					if (!flag)
					{
						continue;
					}
					if (flag2)
					{
						return dictionary;
					}
					flag2 = string.Equals(item.Substring(1, item.Length - 2), section, StringComparison.OrdinalIgnoreCase);
				}
				if (!flag2)
				{
					continue;
				}
				int num = item.IndexOf('=');
				if (num != -1)
				{
					string[] array = item.Split(num, 1);
					string text = array[0].Trim();
					if (!string.IsNullOrEmpty(text))
					{
						dictionary[text] = array[1].TrimStart();
					}
				}
			}
			return dictionary;
		}

		public static Dictionary<string, string> ReadFile(string file, string section = null)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			try
			{
				foreach (string item in GetFiles(file).Where(File.Exists))
				{
					using (StreamReader tr = File.OpenText(item))
					{
						dictionary.AddRange(GetValues(tr, section));
					}
				}
				return dictionary;
			}
			catch (Exception)
			{
				return dictionary;
			}
		}

		public static IEnumerable<string> ReadSections(TextReader tr)
		{
			return from s in tr.ReadLines().TrimStrings().RemoveEmpty()
				where s.StartsWith("[")
				select s.Substring(1, s.Length - 2);
		}

		public static IEnumerable<string> ReadSections(string file)
		{
			List<string> list = new List<string>();
			try
			{
				foreach (string item in GetFiles(file).Where(File.Exists))
				{
					using (StreamReader tr = File.OpenText(item))
					{
						foreach (string item2 in ReadSections(tr))
						{
							if (!list.Contains(item2, StringComparer.OrdinalIgnoreCase))
							{
								list.Add(item2);
							}
						}
					}
				}
				return list;
			}
			catch (Exception)
			{
				return list;
			}
		}

		public static Dictionary<string, string> ReadCommandLine()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			foreach (string input in commandLineArgs)
			{
				Match match = rxCommand.Match(input);
				if (match.Success)
				{
					dictionary[match.Groups["switch"].Value] = match.Groups["value"].Value;
				}
			}
			return dictionary;
		}

		public static void UpdateProperties(object data, Dictionary<string, string> values, bool throwException = false)
		{
			if (data == null)
			{
				return;
			}
			Type type = data.GetType();
			foreach (string key in values.Keys)
			{
				PropertyInfo property = GetProperty(type, key);
				if (property == null)
				{
					continue;
				}
				try
				{
					TypeConverter converter = (from x in property.GetCustomAttributes(inherit: true).OfType<TypeConverterAttribute>()
						select Activator.CreateInstance(Type.GetType(x.ConverterTypeName))).Cast<TypeConverter>().FirstOrDefault() ?? TypeDescriptor.GetConverter(property.PropertyType);
					object value = GetValue(values, key, converter, null);
					if (value != null)
					{
						property.SetValue(data, value, null);
					}
				}
				catch
				{
					if (throwException)
					{
						throw;
					}
				}
			}
		}

		public static void UpdateProperties(object data, string file)
		{
			UpdateProperties(data, ReadFile(file));
		}

		public static void UpdateProperties(object data, bool withCommandLine)
		{
			UpdateProperties(data, DefaultIniFile);
			if (withCommandLine)
			{
				UpdateProperties(data, ReadCommandLine());
			}
		}

		public static IEnumerable<string> GetDefaultLocations(string file)
		{
			yield return Path.Combine(StartupFolder, file);
			yield return Path.Combine(CommonApplicationDataFolder, file);
			if (extraDefaultLocation == null)
			{
				yield return Path.Combine(ApplicationDataFolder, file);
				yield break;
			}
			foreach (string item in extraDefaultLocation)
			{
				yield return Path.Combine(item, file);
			}
		}

		public static void AddDefaultLocation(string path)
		{
			if (extraDefaultLocation == null)
			{
				extraDefaultLocation = new List<string>();
			}
			extraDefaultLocation.Add(path);
			defaultIniFile = null;
			defaultIni = null;
		}

		private static IEnumerable<string> GetFiles(string file)
		{
			return file.Split(MultiInitFileSeparator.ToCharArray());
		}

		private static bool FileExists(string file)
		{
			return GetFiles(file).Any((string f) => File.Exists(file));
		}

		private static string MakeApplicationPath(string folder)
		{
			folder = Path.Combine(folder, Application.CompanyName);
			folder = Path.Combine(folder, Application.ProductName);
			return folder;
		}
	}
}
