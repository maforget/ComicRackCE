using System.Data.Common;
using System.Data.SqlClient;

namespace cYo.Projects.ComicRack.Engine.Database.Storage
{
	public class ComicStorageMsSql : ComicStorageBaseSql
	{
		protected override DbConnection CreateConnection(string connection)
		{
			return new SqlConnection(connection);
		}

		protected override bool CreateTables()
		{
			try
			{
				ExecuteCommand("Create table changes (id varchar(40) not null primary key, update_counter bigint default 0, delete_counter bigint default 0)");
				ExecuteCommand("Create table comics (id varchar(40) not null primary key, update_counter bigint default 0, data text)");
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
