using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using cYo.Common.Xml;

namespace cYo.Projects.ComicRack.Engine.Database.Storage
{
	public abstract class ComicStorageBaseSql : IComicStorage
	{
		public static readonly Guid StorageGuid = Guid.Empty;

		public const int DbVersion = 1;

		private DbConnection dbConnection;

		private readonly Dictionary<Guid, long> updateMap = new Dictionary<Guid, long>();

		private long lastUpdate;

		private long lastDelete;

		private readonly Stack<DbTransaction> transactionStack = new Stack<DbTransaction>();

		public bool IsConnected
		{
			get
			{
				if (dbConnection != null && dbConnection.State != 0)
				{
					return dbConnection.State != ConnectionState.Broken;
				}
				return false;
			}
		}

		protected abstract DbConnection CreateConnection(string connection);

		protected abstract bool CreateTables();

		private DbCommand CreateCommand(string command, params object[] data)
		{
			if (dbConnection == null)
			{
				throw new InvalidOperationException();
			}
			if (dbConnection.State == ConnectionState.Broken || dbConnection.State == ConnectionState.Closed)
			{
				if (dbConnection.State == ConnectionState.Broken)
				{
					try
					{
						dbConnection.Close();
					}
					catch
					{
					}
				}
				dbConnection.Open();
				lock (transactionStack)
				{
					transactionStack.Clear();
				}
			}
			DbCommand dbCommand = dbConnection.CreateCommand();
			dbCommand.CommandText = string.Format(command, data);
			lock (transactionStack)
			{
				DbTransaction dbTransaction = ((transactionStack.Count == 0) ? null : transactionStack.Peek());
				if (dbTransaction != null)
				{
					dbCommand.Transaction = dbTransaction;
					return dbCommand;
				}
				return dbCommand;
			}
		}

		protected int ExecuteCommand(string command, params object[] data)
		{
			using (DbCommand dbCommand = CreateCommand(command, data))
			{
				return dbCommand.ExecuteNonQuery();
			}
		}

		private long ReadCounter(string name)
		{
			using (DbCommand dbCommand = CreateCommand("Select " + name + " from changes"))
			{
				object obj = dbCommand.ExecuteScalar();
				return (obj == null) ? 0 : ((long)obj);
			}
		}

		private void WriteCounter(string name, long counter)
		{
			using (DbCommand dbCommand = CreateCommand("Update changes set " + name + "={1} where id='{0}'", StorageGuid, counter))
			{
				if (dbCommand.ExecuteNonQuery() == 0)
				{
					dbCommand.CommandText = $"Insert into changes values ('{StorageGuid}', 0, 0)";
					dbCommand.ExecuteNonQuery();
					WriteCounter(name, counter);
				}
			}
		}

		private long ReadUpdateCounter()
		{
			return ReadCounter("update_counter");
		}

		private void WriteUpdateCounter(long counter)
		{
			WriteCounter("update_counter", counter);
			lastUpdate = counter;
		}

		private long ReadDeleteCounter()
		{
			return ReadCounter("delete_counter");
		}

		private void WriteDeleteCounter(long counter)
		{
			WriteCounter("delete_counter", counter);
			lastDelete = counter;
		}

		private void UpdateBook(ComicBook targetBook, ComicBook sourceBook)
		{
			targetBook.CopyFrom(sourceBook);
		}

		public bool Open(string connection)
		{
			dbConnection = CreateConnection(connection);
			dbConnection.Open();
			lock (transactionStack)
			{
				transactionStack.Clear();
			}
			return CreateTables();
		}

		public void Close()
		{
			dbConnection.Close();
			dbConnection = null;
			lock (transactionStack)
			{
				transactionStack.Clear();
			}
		}

		public void Delete(ComicBook book)
		{
			if (ExecuteCommand("Delete from comics where id='{0}';", book.Id) > 0)
			{
				updateMap.Remove(book.Id);
				WriteDeleteCounter(ReadDeleteCounter() + 1);
			}
		}

		public bool Write(ComicBook book)
		{
			string value = XmlUtility.ToString(book);
			long num = ReadUpdateCounter() + 1;
			using (DbCommand dbCommand = CreateCommand("Update comics set data=@data, update_counter={1} where id='{0}'", book.Id, num))
			{
				DbParameter dbParameter = dbCommand.CreateParameter();
				dbParameter.DbType = DbType.String;
				dbParameter.ParameterName = "@data";
				dbParameter.Value = value;
				dbCommand.Parameters.Add(dbParameter);
				if (dbCommand.ExecuteNonQuery() == 0)
				{
					dbCommand.CommandText = $"Insert into comics values ('{book.Id}', {num}, @data)";
					dbCommand.ExecuteNonQuery();
				}
			}
			WriteUpdateCounter(ReadUpdateCounter() + 1);
			return false;
		}

		public IEnumerable<ComicBook> Load()
		{
			using (DbCommand cmd = CreateCommand("Select update_counter, data from comics"))
			{
				lastUpdate = ReadUpdateCounter();
				using (DbDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						ComicBook comicBook;
						try
						{
							comicBook = XmlUtility.FromString<ComicBook>(reader[1].ToString());
							updateMap[comicBook.Id] = (long)reader[0];
						}
						catch
						{
							comicBook = null;
						}
						if (comicBook != null)
						{
							yield return comicBook;
						}
					}
				}
			}
		}

		public bool Refresh(ComicBookContainer bookContainer)
		{
			bool result = false;
			if (lastUpdate < ReadUpdateCounter())
			{
				using (DbCommand dbCommand = CreateCommand("Select id, update_counter, data from comics where update_counter>{0}", lastUpdate))
				{
					using (DbDataReader dbDataReader = dbCommand.ExecuteReader())
					{
						while (dbDataReader.Read())
						{
							Guid id = new Guid(dbDataReader[0].ToString());
							long value = (long)dbDataReader[1];
							string text = dbDataReader[2].ToString();
							ComicBook comicBook = XmlUtility.FromString<ComicBook>(text);
							if (bookContainer.Books[id] != null)
							{
								ComicBook comicBook2 = bookContainer.Books[id];
								UpdateBook(comicBook2, comicBook);
								updateMap[comicBook2.Id] = value;
							}
							else
							{
								bookContainer.Add(comicBook);
								updateMap[comicBook.Id] = value;
							}
						}
					}
				}
				lastUpdate = ReadUpdateCounter();
				result = true;
			}
			if (lastDelete < ReadDeleteCounter())
			{
				using (DbCommand dbCommand2 = CreateCommand("Select id from comics"))
				{
					using (DbDataReader dbDataReader2 = dbCommand2.ExecuteReader())
					{
						HashSet<Guid> ids = new HashSet<Guid>();
						while (dbDataReader2.Read())
						{
							ids.Add(new Guid(dbDataReader2[0].ToString()));
						}
						IEnumerable<ComicBook> list = from cb in bookContainer.Books.ToArray()
							where !ids.Contains(cb.Id)
							select cb;
						bookContainer.Books.RemoveRange(list);
					}
				}
				lastDelete = ReadDeleteCounter();
				result = true;
			}
			return result;
		}

		public void BeginTransaction()
		{
			lock (transactionStack)
			{
				transactionStack.Push(dbConnection.BeginTransaction());
			}
		}

		public void CommitTransaction()
		{
			lock (transactionStack)
			{
				transactionStack.Pop()?.Commit();
			}
		}

		public void RollbackTransaction()
		{
			lock (transactionStack)
			{
				transactionStack.Pop()?.Rollback();
			}
		}
	}
}
