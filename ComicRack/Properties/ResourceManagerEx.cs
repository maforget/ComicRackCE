using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using cYo.Common.Localize;

namespace cYo.Projects.ComicRack.Viewer.Properties
{
	internal class ResourceManagerEx : System.Resources.ResourceManager
	{
		private readonly bool useDarkMode;
		private readonly Dictionary<string, string> darkResources = new Dictionary<string, string>();
		const string triggerWord = "Dark";

		public ResourceManagerEx(bool useDarkMode)
			: base("cYo.Projects.ComicRack.Viewer.Properties.Resources", typeof(Resources).Assembly)
		{
			this.useDarkMode = useDarkMode;
			DiscoverDarkResources();
		}

		private void DiscoverDarkResources()
		{
			//Get all resources
			var resourceSet = Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
			var imagesKeys = resourceSet.OfType<DictionaryEntry>() // all images keys
				.Where(i => i.Value is Bitmap)
				.Select(i => i.Key.ToString())
				.ToHashSet();
			var darkImagesKeys = imagesKeys.Where(i => i.StartsWith(triggerWord)).ToHashSet(); // all images keys that start with the trigger word

			foreach (string key in darkImagesKeys)
			{
				string lightKey = key.Replace(triggerWord, "");

				// Only add if the light version exists
				if (imagesKeys.TryGetValue(lightKey, out var _))
					darkResources[lightKey] = key;
			}
		}

		public override object GetObject(string name)
		{
			return GetObject(name, CultureInfo.CurrentUICulture);
		}

		public override object GetObject(string name, CultureInfo culture)
		{
			if (!useDarkMode)
				return base.GetObject(name, culture);

			return GetDarkObject(name, culture);
		}

		private object GetDarkObject(string name, CultureInfo culture)
		{
			if (darkResources.TryGetValue(name, out string darkValue))
				return base.GetObject(darkValue, culture);

			return base.GetObject(name, culture);
			//return base.GetObject($"{triggerWord}{name}", culture) ?? base.GetObject(name, culture); // If the Dark variant doesn't exist return the regular version
		}

		internal static void InitResourceManager(bool useDarkMode = false)
		{
			var innerField = typeof(Resources).GetField("resourceMan",
					BindingFlags.NonPublic | BindingFlags.Static);

			innerField?.SetValue(null, new ResourceManagerEx(useDarkMode));

			innerField = typeof(Resources).GetField("resourceCulture",
				BindingFlags.NonPublic | BindingFlags.Static);

			innerField.SetValue(null, CultureInfo.CurrentUICulture);
		}
	}
}