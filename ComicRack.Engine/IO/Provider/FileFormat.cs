using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using cYo.Common.Win32;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	[Serializable]
	public class FileFormat : IComparable<FileFormat>
	{
		private readonly string[] extensionArray;

		private readonly string extensions;

		private int iconId = 1;

		public string Name
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public string ExtensionList => extensions;

		public IEnumerable<string> Extensions => extensionArray;

		public string ExtensionFilter
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				string[] array = extensionArray;
				foreach (string value in array)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append("; ");
					}
					stringBuilder.Append("*");
					stringBuilder.Append(value);
				}
				return stringBuilder.ToString();
			}
		}

		public int IconId
		{
			get
			{
				return iconId;
			}
			set
			{
				iconId = value;
			}
		}

		public bool SupportsUpdate
		{
			get;
			set;
		}

		public bool Dynamic
		{
			get;
			set;
		}

		public string MainExtension => extensionArray[0];

		public static bool CanRegisterShell => ShellRegister.CanRegisterShell;

		public FileFormat(string name, int id, string extensions)
		{
			Name = name;
			Id = id;
			this.extensions = extensions;
			extensionArray = extensions.Replace(" ", string.Empty).Split(';');
		}

		public bool HasExtension(string extension)
		{
			return extensionArray.Any((string ext) => string.Equals(extension, ext, StringComparison.OrdinalIgnoreCase));
		}

		public bool IsShellRegistered(string typeId)
		{
			try
			{
				return extensionArray.All((string ext) => ShellRegister.IsFileOpenRegistered(typeId, ext) || ShellRegister.IsFileOpenWithRegistered(ext, typeId));
			}
			catch
			{
				return false;
			}
		}

		public void RegisterShell(string typeId, string docName, bool overwrite)
		{
			try
			{
				string[] array = extensionArray;
				foreach (string docExtension in array)
				{
					bool flag = ShellRegister.IsFileOpenInUse(typeId, docExtension);
					if (!flag || overwrite)
					{
						ShellRegister.RegisterFileOpen(typeId, docExtension, docName, iconId);
					}
					else
					{
						ShellRegister.RegisterFileOpenWith(docExtension, typeId);
					}
				}
			}
			catch
			{
			}
		}

		public void UnregisterShell(string typeId)
		{
			try
			{
				string[] array = extensionArray;
				foreach (string docExtension in array)
				{
					ShellRegister.UnregisterFileOpen(typeId, docExtension);
					ShellRegister.UnregisterFileOpenWith(docExtension, typeId);
				}
			}
			catch
			{
			}
		}

		public bool Supports(string source)
		{
			return HasExtension(Path.GetExtension(source));
		}

		public override string ToString()
		{
			return Name + " (" + ExtensionFilter + ")";
		}

		public int CompareTo(FileFormat other)
		{
			return string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
		}
	}
}
