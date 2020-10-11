using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine.Database.Storage
{
	public interface IComicStorage
	{
		bool IsConnected
		{
			get;
		}

		bool Open(string connection);

		void Close();

		void Delete(ComicBook book);

		bool Write(ComicBook book);

		IEnumerable<ComicBook> Load();

		bool Refresh(ComicBookContainer books);

		void BeginTransaction();

		void CommitTransaction();

		void RollbackTransaction();
	}
}
