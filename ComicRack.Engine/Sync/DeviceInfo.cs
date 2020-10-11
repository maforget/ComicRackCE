using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Engine.Sync
{
	public class DeviceInfo
	{
		public string Name
		{
			get;
			private set;
		}

		public string Model
		{
			get;
			private set;
		}

		public string SerialNumber
		{
			get;
			private set;
		}

		public string Key
		{
			get;
			private set;
		}

		public string Manufacturer
		{
			get;
			private set;
		}

		public SyncAppEdition Edition
		{
			get;
			private set;
		}

		public int Version
		{
			get;
			private set;
		}

		public string DeviceHash
		{
			get;
			private set;
		}

		public Size ScreenPixelSize
		{
			get;
			private set;
		}

		public PointF ScreenDpi
		{
			get;
			private set;
		}

		public DeviceCapabilites Capabilites
		{
			get;
			private set;
		}

		public int BookSyncLimit
		{
			get;
			set;
		}

		public SizeF ScreenPhysicalSize => new SizeF((float)ScreenPixelSize.Width / ScreenDpi.X, (float)ScreenPixelSize.Height / ScreenDpi.Y);

		public DeviceInfo(IDictionary<string, string> values)
		{
			Name = GetProperty(values, "Name");
			Model = GetProperty(values, "Model");
			Manufacturer = GetProperty(values, "Manufacturer");
			SerialNumber = GetProperty(values, "Serial");
			Key = GetProperty(values, "ID");
			DeviceHash = GetProperty(values, "Hash");
			Version = int.Parse(values["Version"]);
			string property = GetProperty(values, "Edition");
			string a = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(Model + Manufacturer + SerialNumber + property + Version)).ToHexString(trimZeros: true);
			switch (property)
			{
			case "Android Free":
				Edition = SyncAppEdition.AndroidFree;
				break;
			case "Android Full":
				Edition = SyncAppEdition.AndroidFull;
				break;
			case "iOS":
				Edition = SyncAppEdition.iOS;
				break;
			default:
				Edition = SyncAppEdition.Unknown;
				break;
			}
			string[] array = GetProperty(values, "Screen").Split(',');
			ScreenPixelSize = new Size(int.Parse(array[0]), int.Parse(array[1]));
			ScreenDpi = new PointF(float.Parse(array[2], CultureInfo.InvariantCulture), float.Parse(array[3], CultureInfo.InvariantCulture));
			string text = GetProperty(values, "Capabilities") ?? string.Empty;
			if (text.Contains("WEBP"))
			{
				Capabilites |= DeviceCapabilites.WebP;
			}
			if (string.IsNullOrEmpty(Model) || string.IsNullOrEmpty(SerialNumber) || string.IsNullOrEmpty(Key) || ScreenPixelSize.Width == 0 || ScreenPixelSize.Height == 0 || ScreenDpi.X <= 0.1f || ScreenDpi.Y <= 0.1f || a != DeviceHash)
			{
				throw new InvalidDataException();
			}
		}

		private string GetProperty(IDictionary<string, string> bag, string key)
		{
			if (!bag.TryGetValue(key, out var value))
			{
				return null;
			}
			return value;
		}
	}
}
