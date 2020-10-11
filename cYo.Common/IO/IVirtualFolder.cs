using System.Collections.Generic;
using System.IO;

namespace cYo.Common.IO
{
	public interface IVirtualFolder
	{
		Stream OpenRead(string path);

		Stream Create(string path);

		bool FileExists(string path);

		bool CreateFolder(string path);

		IEnumerable<string> GetFiles(string path);
	}
}
