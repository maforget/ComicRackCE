using System;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class FileFormatAttribute : Attribute
	{
		public FileFormat Format
		{
			get;
			set;
		}

		public bool Dynamic
		{
			get
			{
				return Format.Dynamic;
			}
			set
			{
				Format.Dynamic = value;
			}
		}

		public bool EnableUpdate
		{
			get
			{
				return Format.SupportsUpdate;
			}
			set
			{
				Format.SupportsUpdate = value;
			}
		}

		public FileFormatAttribute(FileFormat format)
		{
			Format = format;
		}

		public FileFormatAttribute(string name, int id, string extensions)
			: this(new FileFormat(name, id, extensions))
		{
		}
	}
}
