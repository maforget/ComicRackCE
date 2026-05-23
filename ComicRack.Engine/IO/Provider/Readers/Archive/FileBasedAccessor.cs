using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Archive
{
	public abstract class FileBasedAccessor : IComicAccessor
	{
		private IEnumerable<byte> signature;

		protected bool HasSignature => signature != null;

		public int Format
		{
			get;
			private set;
		}

		public FileBasedAccessor(int format)
		{
			Format = format;
			signature = KnownFileFormats.GetSignature(format);
		}

		public abstract IEnumerable<ProviderImageInfo> GetEntryList(string source);

		public abstract byte[] ReadByteImage(string source, ProviderImageInfo info);

		public abstract ComicInfo ReadInfo(string source);

        public abstract ComicBook ReadBook(string source);

		public virtual bool WriteInfo(string source, ComicInfo info)
		{
			return SevenZipEngine.UpdateComicInfo(source, Format, standalone: false, comicInfo: info); // Since SevenZip is still used for updates when another engine is set and since the format might not be supported by the standalone exe, we need to use the console (32bit) version.
        }

        public virtual bool WriteBook(string source, ComicBook book)
        {
            return SevenZipEngine.UpdateComicInfo(source, Format, standalone: false, comicInfo: book); // Since SevenZip is still used for updates when another engine is set and since the format might not be supported by the standalone exe, we need to use the console (32bit) version.
        }

        public virtual bool IsFormat(string source)
		{
			if (signature == null)
			{
				return true;
			}
			try
			{
				using (FileStream fileStream = File.OpenRead(source))
				{
					IEnumerable<byte> enumerable = signature;
					byte[] array = new byte[enumerable.Count()];
					fileStream.Read(array, 0, array.Length);
					return enumerable.SequenceEqual(array);
				}
			}
			catch
			{
			}
			return true;
		}

    }
}
