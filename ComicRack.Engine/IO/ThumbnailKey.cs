using System;
using System.Text.RegularExpressions;
using cYo.Common.Drawing;

namespace cYo.Projects.ComicRack.Engine.IO
{
	[Serializable]
	public class ThumbnailKey : ImageKey
	{
		public const string ResourceKey = "resource";

		public const string FileKey = "file";

		public const string CustomKey = "custom";

		private static readonly Regex rxResource = new Regex("\\A\\s*(?<type>[a-z]{4,}):\\\\\\\\(?<resource>.*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		[NonSerialized]
		private string resourceType;

		[NonSerialized]
		private string resourceLocation;

		public string ResourceType
		{
			get
			{
				if (resourceType == null)
				{
					CalcResource();
				}
				return resourceType;
			}
		}

		public string ResourceLocation
		{
			get
			{
				if (resourceLocation == null)
				{
					CalcResource();
				}
				return resourceLocation;
			}
		}

		public ThumbnailKey(object source, string location, long size, DateTime modified, int index, ImageRotation rotation)
			: base(source, location, size, modified, index, rotation)
		{
		}

		public ThumbnailKey(object source, string file, int index, ImageRotation rotation)
			: this(source, file, ImageKey.GetSafeSize(file), ImageKey.GetSafeModifiedTime(file), index, rotation)
		{
		}

		public ThumbnailKey(ImageKey key)
			: base(key)
		{
		}

		private void CalcResource()
		{
			Match match = rxResource.Match(base.Location);
			if (match.Success)
			{
				resourceType = match.Groups["type"].Value;
				resourceLocation = match.Groups[ResourceKey].Value;
			}
			else
			{
				resourceType = string.Empty;
				resourceLocation = base.Location;
			}
		}

		public static string GetResource(string key, string resource)
		{
			return $"{key}:\\\\{resource}";
		}
	}
}
