using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using cYo.Common;
using cYo.Common.Collections;

namespace cYo.Projects.ComicRack.Engine.IO.Network
{
	[Serializable]
	public class ComicLibraryServerConfig
	{
		public class EqualityComparer : IEqualityComparer<ComicLibraryServerConfig>
		{
			public bool Equals(ComicLibraryServerConfig x, ComicLibraryServerConfig y)
			{
				if (x.Name == y.Name && x.Password == y.Password && x.Options == y.Options && x.Description == y.Description && x.IsInternet == y.IsInternet && x.IsPrivate == y.IsPrivate && x.LibraryShareMode == y.LibraryShareMode && x.SharedItems.SequenceEqual(y.SharedItems) && x.ThumbnailQuality == y.ThumbnailQuality)
				{
					return x.PageQuality == y.PageQuality;
				}
				return false;
			}

			public int GetHashCode(ComicLibraryServerConfig obj)
			{
				return obj.GetHashCode();
			}
		}

		public const int DefaultPrivateServicePort = 7612;

		public const int DefaultPublicServicePort = 7612;

		public const string DefaultServiceName = "Share";

		private SmartList<Guid> sharedItems = new SmartList<Guid>();

		[DefaultValue(LibraryShareMode.All)]
		public LibraryShareMode LibraryShareMode
		{
			get;
			set;
		}

		[DefaultValue("")]
		public string Name
		{
			get;
			set;
		}

		[DefaultValue("")]
		public string Description
		{
			get;
			set;
		}

		[DefaultValue("")]
		public string Password
		{
			get;
			set;
		}

		[DefaultValue(ServerOptions.None)]
		public ServerOptions Options
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool IsInternet
		{
			//Always return false since Public Servers are disabled
			get =>  false;
			set => _ = value;
		}

		[DefaultValue(false)]
		public bool IsPrivate
		{
			get;
			set;
		}

		[DefaultValue(100)]
		public int PageQuality
		{
			get;
			set;
		}

		[DefaultValue(100)]
		public int ThumbnailQuality
		{
			get;
			set;
		}

		public SmartList<Guid> SharedItems => sharedItems;

		[XmlIgnore]
		[DefaultValue(ComicLibraryServerConfig.DefaultPrivateServicePort)]
		public int ServicePort
		{
			get;
			set;
		}

		[XmlIgnore]
		[DefaultValue(ComicLibraryServerConfig.DefaultServiceName)]
		public string ServiceName
		{
			get;
			set;
		}

		[XmlIgnore]
		[DefaultValue(false)]
		public bool OnlyPrivateConnections
		{
			get;
			set;
		}

		[XmlIgnore]
		[DefaultValue("")]
		public string PrivateListPassword
		{
			get;
			set;
		}

		public bool IsValidShare
		{
			get
			{
				if (LibraryShareMode != 0 && !string.IsNullOrEmpty(Name))
				{
					if (string.IsNullOrEmpty(PrivateListPassword))
					{
						if (IsInternet)
						{
							return !IsPrivate;
						}
						return true;
					}
					return true;
				}
				return false;
			}
		}

		public string ProtectionPassword
		{
			get
			{
				if (!IsProtected)
				{
					return string.Empty;
				}
				return Password;
			}
		}

		public bool IsProtected
		{
			get
			{
				if (Options.IsSet(ServerOptions.ShareNeedsPassword))
				{
					return !string.IsNullOrEmpty(Password);
				}
				return false;
			}
			set
			{
				Options = Options.SetMask(ServerOptions.ShareNeedsPassword, value);
			}
		}

		public bool IsEditable
		{
			get
			{
				return Options.IsSet(ServerOptions.ShareIsEditable);
			}
			set
			{
				Options = Options.SetMask(ServerOptions.ShareIsEditable, value);
			}
		}

		public bool IsExportable
		{
			get
			{
				return Options.IsSet(ServerOptions.ShareIsExportable);
			}
			set
			{
				Options = Options.SetMask(ServerOptions.ShareIsExportable, value);
			}
		}

		public ComicLibraryServerConfig()
		{
			ServicePort = ComicLibraryServerConfig.DefaultPrivateServicePort;
			LibraryShareMode = LibraryShareMode.All;
			Name = string.Empty;
			Description = string.Empty;
			Password = string.Empty;
			PageQuality = 100;
			ThumbnailQuality = 100;
			Options = ServerOptions.None;
		}
	}
}
