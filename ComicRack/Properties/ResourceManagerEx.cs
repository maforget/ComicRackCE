using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using cYo.Common.Localize;
using cYo.Common.Windows.Forms.Theme;

namespace cYo.Projects.ComicRack.Viewer.Properties
{
	internal class ResourceManagerEx : System.Resources.ResourceManager
	{
		private readonly bool isThemed;
		private readonly Dictionary<string, string> darkResources = new Dictionary<string, string>();
		private readonly string triggerWord = string.Empty;

		public ResourceManagerEx(Type resourceType, Themes theme = Themes.Default )
			: base(resourceType.FullName, resourceType.Assembly)
		{
			isThemed = theme != Themes.Default ? true : false;
			triggerWord = isThemed ? theme.ToString() : string.Empty;
			DiscoverResources();
		}

		private void DiscoverResources()
		{
			//Get all resources
			var resourceSet = this.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
			var imagesKeys = resourceSet.OfType<DictionaryEntry>() // all images keys
				.Where(i => i.Value is Bitmap)
				.Select(i => i.Key.ToString())
				.ToHashSet();

			if (isThemed)
			{
                var darkImagesKeys = imagesKeys.Where(i => i.StartsWith(triggerWord)).ToHashSet(); // all images keys that start with the trigger word

                foreach (string key in darkImagesKeys)
                {
                    string lightKey = key.Replace(triggerWord, "");

                    // Only add if the light version exists
                    if (imagesKeys.TryGetValue(lightKey, out var _))
                        darkResources[lightKey] = key;
                }
            }
		}

		public override object GetObject(string name)
		{
			return GetObject(name, CultureInfo.CurrentUICulture);
		}

		public override object GetObject(string name, CultureInfo culture)
		{
			if (!isThemed)
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

		internal static void InitResourceManager(Themes theme)
		{
			Type[] types = 
				[
					typeof(cYo.Projects.ComicRack.Viewer.Properties.Resources), 
					typeof(cYo.Common.Windows.Properties.Resources)
				];

			foreach (Type type in types)
			{
				var innerField = type.GetField("resourceMan",
							BindingFlags.NonPublic | BindingFlags.Static);

				innerField?.SetValue(null, new ResourceManagerEx(type, theme));

				innerField = type.GetField("resourceCulture",
					BindingFlags.NonPublic | BindingFlags.Static);

				innerField.SetValue(null, CultureInfo.CurrentUICulture); 
			}
		}
	}
}