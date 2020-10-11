using System.IO;
using System.Text;
using cYo.Common.IO;

namespace cYo.Projects.ComicRack.Engine.IO.Cache
{
	public class FileCache : DiskCache<string, byte[]>
	{
		public static FileCache Default
		{
			get;
			set;
		}

		public FileCache(string path, int sizeMB)
			: base(path, sizeMB, 10)
		{
		}

		protected override byte[] LoadItem(string file)
		{
			return File.ReadAllBytes(file);
		}

		protected override void StoreItem(string file, byte[] item)
		{
			File.WriteAllBytes(file, item);
		}

		public bool AddText(string file, string text)
		{
			return AddItem(file, Encoding.UTF8.GetBytes(text));
		}

		public string GetText(string file)
		{
			byte[] item = GetItem(file);
			if (item != null)
			{
				return Encoding.UTF8.GetString(item);
			}
			return null;
		}
	}
}
