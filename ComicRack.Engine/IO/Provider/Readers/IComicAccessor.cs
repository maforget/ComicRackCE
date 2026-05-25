using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers
{
	public interface IComicAccessor
	{
		bool IsFormat(string source);

		IEnumerable<ProviderImageInfo> GetEntryList(string source);

		byte[] ReadByteImage(string source, ProviderImageInfo info);

		T ReadInfo<T>(string source) where T : ComicInfo;

        bool WriteInfo(string source, ComicInfo info);
        bool WriteBook(string source, ComicBook info);
	}
}
