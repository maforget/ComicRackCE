using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers
{
	public interface IComicAccessor
	{
		bool IsFormat(string source);

		IEnumerable<ProviderImageInfo> GetEntryList(string source);

		byte[] ReadByteImage(string source, ProviderImageInfo info);

		ComicInfo ReadInfo(string source);

		bool WriteInfo(string source, ComicInfo info);
	}
}
