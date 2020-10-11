using System.Collections.Generic;
using System.IO;

namespace cYo.Common.IO
{
	public class VirtualFileFolder : IVirtualFolder
	{
		private readonly string basePath;

		public string BasePath => basePath;

		public VirtualFileFolder(string basePath)
		{
			this.basePath = basePath;
		}

		public VirtualFileFolder()
			: this(string.Empty)
		{
		}

		public Stream OpenRead(string path)
		{
			return File.OpenRead(Path.Combine(basePath, path));
		}

		public Stream Create(string path)
		{
			return File.Create(Path.Combine(basePath, path));
		}

		public bool FileExists(string path)
		{
			return File.Exists(Path.Combine(basePath, path));
		}

		public bool CreateFolder(string path)
		{
			try
			{
				Directory.CreateDirectory(Path.Combine(basePath, path));
				return true;
			}
			catch
			{
				return false;
			}
		}

		public IEnumerable<string> GetFiles(string path)
		{
			return Directory.GetFiles(Path.Combine(basePath, path));
		}
	}
}
