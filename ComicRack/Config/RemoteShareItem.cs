using System.ComponentModel;
using System.Xml.Serialization;
using cYo.Projects.ComicRack.Engine.IO.Network;

namespace cYo.Projects.ComicRack.Viewer.Config
{
	public class RemoteShareItem
	{
		[DefaultValue(null)]
		[XmlAttribute]
		public string Name
		{
			get;
			set;
		}

		[DefaultValue(null)]
		[XmlAttribute]
		public string Uri
		{
			get;
			set;
		}

		public RemoteShareItem()
		{
		}

		public RemoteShareItem(ShareInformation si)
		{
			Name = si.Name;
			Uri = si.Uri;
		}

		public RemoteShareItem(string name)
		{
			Uri = (Name = name);
		}

		public override string ToString()
		{
			if (!string.IsNullOrEmpty(Name))
			{
				return Name;
			}
			return Uri;
		}

		public override bool Equals(object obj)
		{
			RemoteShareItem remoteShareItem = obj as RemoteShareItem;
			if (remoteShareItem != null && remoteShareItem.Name == Name)
			{
				return remoteShareItem.Uri == Uri;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (Name ?? string.Empty).GetHashCode() ^ (Uri ?? string.Empty).GetHashCode();
		}
	}
}
