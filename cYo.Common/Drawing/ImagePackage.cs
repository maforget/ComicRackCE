using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.IO;
using cYo.Common.Runtime;
using cYo.Common.Text;

namespace cYo.Common.Drawing
{
	public class ImagePackage : DisposableObject, IImagePackage
	{
		private class ImageItem
		{
			public IVirtualFolder Package
			{
				get;
				set;
			}

			public string File
			{
				get;
				set;
			}

			public Image Image
			{
				get;
				set;
			}
		}

		private const string MapFile = "map.ini";

		private readonly Dictionary<string, ImageItem> imageDict;

		public bool EnableWidthCropping
		{
			get;
			set;
		}

		public bool EnableHeightCropping
		{
			get;
			set;
		}

		public IEnumerable<string> Keys => imageDict.Keys;

		public ImagePackage(IVirtualFolder package = null, bool caseSensitive = false)
		{
			if (caseSensitive)
			{
				imageDict = new Dictionary<string, ImageItem>();
			}
			else
			{
				imageDict = new Dictionary<string, ImageItem>(StringComparer.OrdinalIgnoreCase);
			}
			if (package != null)
			{
				Add(package);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				foreach (ImageItem value in imageDict.Values)
				{
					value.Image.SafeDispose();
				}
			}
			base.Dispose(disposing);
		}

		private void AddImage(IVirtualFolder package, string key, string value, Func<string, IEnumerable<string>> mapKeys)
		{
			ImageItem value2 = new ImageItem
			{
				File = key,
				Package = package
			};
			foreach (string item in mapKeys(value).TrimStrings().RemoveEmpty())
			{
				imageDict[item] = value2;
			}
		}

		public void Add(IVirtualFolder package, Func<string, IEnumerable<string>> mapKeys = null)
		{
			if (package == null)
			{
				return;
			}
			if (mapKeys == null)
			{
				mapKeys = (string s) => ListExtensions.AsEnumerable<string>(s);
			}
			if (package.FileExists(MapFile))
			{
				using (Stream stream = package.OpenRead(MapFile))
				{
					using (StreamReader tr = new StreamReader(stream))
					{
						foreach (KeyValuePair<string, string> value in IniFile.GetValues(tr))
						{
							AddImage(package, value.Key, value.Value, mapKeys);
						}
					}
				}
			}
			foreach (string item in from f in package.GetFiles(string.Empty)
				where string.Compare(Path.GetExtension(f), ".ini", ignoreCase: true) != 0
				select f)
			{
				AddImage(package, item, Path.GetFileNameWithoutExtension(item), mapKeys);
			}
		}

		public void AddRange(IEnumerable<IVirtualFolder> packages, Func<string, IEnumerable<string>> mapKeys = null)
		{
			packages.SafeForEach(delegate(IVirtualFolder p)
			{
				Add(p, mapKeys);
			});
		}

		public bool ImageExists(string key)
		{
			ImageItem value;
			return imageDict.TryGetValue(key, out value);
		}

		public bool ImageLoaded(string key)
		{
			if (imageDict.TryGetValue(key, out var value))
			{
				return value.Image != null;
			}
			return false;
		}

		public Image GetImage(string key)
		{
			if (!imageDict.TryGetValue(key, out var value))
			{
				return null;
			}
			if (value.Image == null && value.Package != null)
			{
				using (Stream stream = value.Package.OpenRead(value.File))
				{
					try
					{
						value.Image = Image.FromStream(stream).CreateCopy();
						if (EnableWidthCropping || EnableHeightCropping)
						{
							Bitmap bitmap = ((Bitmap)value.Image).CropTransparent(EnableWidthCropping, EnableHeightCropping, 16);
							if (bitmap != value.Image)
							{
								value.Image.Dispose();
								value.Image = bitmap;
							}
						}
					}
					catch
					{
						value.Package = null;
					}
				}
			}
			return value.Image;
		}
	}
}
