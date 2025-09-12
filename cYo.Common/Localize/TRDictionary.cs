using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using cYo.Common.Collections;
using cYo.Common.IO;
using cYo.Common.Threading;
using cYo.Common.Xml;

namespace cYo.Common.Localize
{
	public class TRDictionary : Dictionary<string, TR>
	{
		public const string LanguageInfoFile = "LanguageInfo.xml";

		private IVirtualFolder resourceFolder = GetDefaultResourceFolder();

		private static TRInfo defaultInfo;

		public CultureInfo DefaultCulture
		{
			get;
			set;
		}

		public IVirtualFolder ResourceFolder
		{
			get
			{
				return resourceFolder;
			}
			set
			{
				resourceFolder = value;
			}
		}

		public TRInfo Info
		{
			get
			{
				if (defaultInfo == null)
				{
					CultureInfo cultureInfo = DefaultCulture ?? CultureInfo.CurrentUICulture;
					defaultInfo = GetLanguageInfo(cultureInfo);
					if (defaultInfo == null)
					{
						defaultInfo = new TRInfo(cultureInfo.Name);
					}
				}
				return defaultInfo;
			}
		}

		public TRDictionary()
		{
		}

		public TRDictionary(IVirtualFolder folder, string path)
		{
			foreach (string file in folder.GetFiles(path))
			{
				if (!".xml".Equals(Path.GetExtension(file), StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}
				try
				{
					using (Stream s = folder.OpenRead(file))
					{
						TR tR = XmlUtility.Load<TR>(s, compressed: false);
						tR.File = file;
						Add(Path.GetFileName(file), tR);
					}
				}
				catch
				{
				}
			}
		}

		public TRDictionary(string path)
			: this(new VirtualFileFolder(), path)
		{
		}

		public void Save(string path)
		{
			string[] files = Directory.GetFiles(path, "*.xml");
			foreach (string path2 in files)
			{
				string fileName = Path.GetFileName(path2);
				if (!string.Equals(fileName, LanguageInfoFile, StringComparison.OrdinalIgnoreCase) && !ContainsKey(fileName))
				{
					File.Delete(path2);
				}
			}
			foreach (string key in base.Keys)
			{
				XmlUtility.Store(Path.Combine(path, key), base[key]);
			}
		}

		public HashSet<TREntry> CreateSet()
		{
			HashSet<TREntry> hashSet = new HashSet<TREntry>();
			foreach (TR value in base.Values)
			{
				hashSet.AddRange(value.Texts);
			}
			return hashSet;
		}

		public void SaveAllText(string path)
		{
			using (StreamWriter streamWriter = File.CreateText(path))
			{
				foreach (TREntry item in CreateSet())
				{
					streamWriter.WriteLine(item.Text);
				}
			}
		}

		public void Update(TRDictionary newPack)
		{
			string[] array = base.Keys.ToArray();
			foreach (string key in array)
			{
				if (!newPack.ContainsKey(key))
				{
					Remove(key);
				}
			}
			foreach (string key2 in newPack.Keys)
			{
				TR tR = newPack[key2];
				if (!TryGetValue(key2, out var value))
				{
					value = new TR(tR.Name, tR.Culture);
					foreach (TREntry text in tR.Texts)
					{
						value.Texts.Add(new TREntry(text.Key, text.Comment, text.Comment));
					}
					Add(key2, value);
				}
				else
				{
					value.Texts.Update(tR.Texts);
				}
			}
		}

		public float CompletionPercent(TRDictionary newPack)
		{
			int num = 0;
			int num2 = 0;
			foreach (string key in newPack.Keys)
			{
				TR tR = newPack[key];
				num += tR.Texts.Count;
				if (!TryGetValue(key, out var value))
				{
					continue;
				}
				foreach (TREntry text in tR.Texts)
				{
					TREntry tREntry = value.Texts[text.Key];
					if (tREntry != null && tREntry.Comment == text.Comment)
					{
						num2++;
					}
				}
			}
			if (num != 0)
			{
				return 100f * (float)num2 / (float)num;
			}
			return 100f;
		}

		public TR Load(string name, CultureInfo culture)
		{
			using (ItemMonitor.Lock(this))
			{
				if (TryGetValue(name, out var value))
				{
					return value;
				}
				CultureInfo topLevelCulture = GetTopLevelCulture(culture);
				value = LoadFile(name, topLevelCulture);
				if (culture.Name != topLevelCulture.Name)
				{
					value.Texts.Merge(LoadFile(name, culture).Texts);
				}
				return base[name] = value;
			}
		}

		public TR Load(string name)
		{
			return Load(name, DefaultCulture ?? CultureInfo.CurrentUICulture);
		}

		public void Save()
		{
			using (ItemMonitor.Lock(this))
			{
				foreach (TR value in base.Values)
				{
					value.Save(ResourceFolder, GetFileName(value.Name, value.Culture));
				}
			}
		}

		public TRInfo GetLanguageInfo(CultureInfo culture)
		{
			string path = Path.Combine(culture.Name, LanguageInfoFile);
			TRInfo tRInfo = null;
			try
			{
				if (ResourceFolder.FileExists(path))
				{
					using (Stream s = ResourceFolder.OpenRead(path))
					{
						tRInfo = XmlUtility.Load<TRInfo>(s, compressed: false);
						tRInfo.CultureName = culture.Name;
						return tRInfo;
					}
				}
				return tRInfo;
			}
			catch
			{
				return tRInfo;
			}
		}

		public IEnumerable<TRInfo> GetLanguageInfos()
		{
			return from tri in CultureInfo.GetCultures(CultureTypes.AllCultures).Select(GetLanguageInfo)
				where tri != null
				select tri;
		}

		public static TR LoadFile(IVirtualFolder folder, string name, CultureInfo culture)
		{
			try
			{
				string fileName = GetFileName(name, culture);
				TR tR;
				using (Stream s = folder.OpenRead(fileName))
				{
					tR = XmlUtility.Load<TR>(s, compressed: false);
				}
				tR.File = fileName;
				foreach (TREntry text in tR.Texts)
				{
					text.Text = text.Text.Replace("\\n", "\r\n").Replace("\\r", "").Replace("\\t", "\t");
				}
				return tR;
			}
			catch (Exception)
			{
				return new TR(name, culture);
			}
		}

		public TR LoadFile(string name, CultureInfo culture)
		{
			return LoadFile(ResourceFolder, name, culture);
		}

		private static VirtualFileFolder GetDefaultResourceFolder()
		{
			try
			{
				return new VirtualFileFolder(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Languages"));
			}
			catch (Exception)
			{
				return null;
			}
		}

		private static CultureInfo GetTopLevelCulture(CultureInfo culture)
		{
			try
			{
				string[] array = culture.Name.Split('-');
				return new CultureInfo(array[0]);
			}
			catch
			{
				return culture;
			}
		}

		private static string GetFileName(string name, CultureInfo culture)
		{
			return Path.Combine(culture.Name, name + ".xml");
		}
	}
}
