using System;
using System.IO;
using System.Threading;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.IO;
using cYo.Common.Localize;
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.Database.Storage;

namespace cYo.Projects.ComicRack.Engine
{
	public class DatabaseManager : DisposableObject
	{
		private Timer timer;

		private ComicDatabase database;

		private ManualResetEvent databaseInitialized = new ManualResetEvent(initialState: false);

		private ComicBookFactory comicBookFactory;

		private int backgroundSaveInteral;

		private readonly ProcessingQueue<ComicDatabase> saveDatabaseQueue = new ProcessingQueue<ComicDatabase>("Save Database Queue", ThreadPriority.Lowest);

		public ComicDatabase Database
		{
			get
			{
				if (databaseInitialized != null)
				{
					if (this.FirstDatabaseAccess != null)
					{
						this.FirstDatabaseAccess(this, EventArgs.Empty);
					}
					databaseInitialized.WaitOne();
					databaseInitialized.Close();
					databaseInitialized = null;
				}
				return database;
			}
		}

		public ComicBookFactory BookFactory => comicBookFactory ?? (comicBookFactory = new ComicBookFactory(Database.Books));

		public string OpenMessage
		{
			get;
			private set;
		}

		public string DatabaseFile
		{
			get;
			private set;
		}

		public int BackgroundSaveInterval
		{
			get
			{
				return backgroundSaveInteral;
			}
			set
			{
				if (value == backgroundSaveInteral)
				{
					return;
				}
				backgroundSaveInteral = value;
				if (timer != null)
				{
					timer.Dispose();
				}
				timer = null;
				if (backgroundSaveInteral > 0)
				{
					timer = new Timer(delegate
					{
						SaveInBackground();
					}, null, 0, backgroundSaveInteral * 1000);
				}
			}
		}

		public bool InitialConnectionError
		{
			get;
			set;
		}

		public event EventHandler FirstDatabaseAccess;

		public DatabaseManager()
		{
		}

		public DatabaseManager(string file, string connection = null)
		{
			Open(file, connection, dontLoadQueryCaches: true, null);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (timer != null)
				{
					timer.Dispose();
				}
				Save();
				saveDatabaseQueue.Dispose();
				Database.Dispose();
			}
			base.Dispose(disposing);
		}

		public bool Open(string file, string connection, bool dontLoadQueryCaches, Action<int> progress)
		{
			DatabaseFile = file;
			try
			{
				OpenMessage = null;
				ComicDatabase comicDatabase = null;
				try
				{
					string text = file + ".restore";
					if (File.Exists(text))
					{
						try
						{
							comicDatabase = ComicDatabase.LoadXml(text, progress);
						}
						catch (Exception)
						{
						}
						FileUtility.SafeDelete(text);
						if (comicDatabase != null)
						{
							OpenMessage = TR.Messages["DatabaseBackupRestored", "A previous database backup has been successfully restored!"];
						}
					}
					if (comicDatabase == null)
					{
						file += ".xml";
						try
						{
							comicDatabase = ComicDatabase.LoadXml(file, progress);
						}
						catch (Exception)
						{
							try
							{
								string text2 = file + ".bak";
								if (!File.Exists(text2))
								{
									throw new FileNotFoundException();
								}
								comicDatabase = ComicDatabase.LoadXml(text2);
								OpenMessage = TR.Messages["DatabaseRestored", "There was a problem with the Database. The last version to be known good has been restored. You may have lost some entires."];
							}
							catch
							{
								try
								{
									File.Copy(file, Path.Combine(Path.GetDirectoryName(file), FileUtility.MakeValidFilename(string.Concat("Corrupt Database Backup [", DateTime.Now, "].xml"))));
								}
								catch
								{
								}
								comicDatabase = ComicDatabase.CreateNew();
								OpenMessage = TR.Messages["DatabaseNewEmpty", "There was a problem opening the Database. A new empty Database has been created."];
							}
						}
					}
					if (!string.IsNullOrEmpty(connection))
					{
						try
						{
							comicDatabase.ComicStorage = ComicStorage.Create(comicDatabase, connection, null);
						}
						catch (Exception ex3)
						{
							InitialConnectionError = true;
							OpenMessage = TR.Messages["DataSourceProblem", "There was a problem ({0}) opening the data source. ComicRack is using a temporary database instead."].SafeFormat(ex3.Message);
						}
					}
				}
				finally
				{
					comicDatabase.FinalizeLoading();
				}
				if (dontLoadQueryCaches || comicDatabase.ComicStorage != null)
				{
					comicDatabase.ComicLists.GetItems<ComicListItem>().ForEach(delegate(ComicListItem cli)
					{
						cli.ResetCacheWithStatistics();
					});
				}
				database = comicDatabase;
				return true;
			}
			finally
			{
				databaseInitialized.Set();
			}
		}

		public void Save(string path = null, bool withBooks = false)
		{
			if (database == null || InitialConnectionError)
			{
				return;
			}
			if (path == null || !Path.GetExtension(path).Equals(".xml", StringComparison.InvariantCultureIgnoreCase))
			{
				path = $"{DatabaseFile}.xml";
			}
			try
			{
				if (Database.ComicStorage == null)
				{
					Database.SaveXml(path);
				}
				else
				{
					ComicDatabase.Attach(Database, withBooks: withBooks).SaveXml(path, commitCache: false);
				}
			}
			catch (Exception)
			{
			}
		}

		public void BackupTo(string file, string customThumbnailPath)
		{
			if (database != null && !InitialConnectionError)
			{
				string text = $"{DatabaseFile}.xml";
				Save(text); // Save the database normally

				// When using a SQL database, save a temp copy of the database that includes books
				if (Database.ComicStorage != null)
				{
					string temp = Path.Combine(EngineConfiguration.Default.TempPath, Path.GetFileName(text));
					Save(temp, withBooks: true); // Save the database with books to a temporary file
					text = temp;
				}
				ComicDatabase.Backup(file, text, customThumbnailPath);
			}
		}

		public void RestoreFrom(string file, string customThumbnailPath)
		{
			if (database != null)
			{
				ComicDatabase.RestoreBackup(file, DatabaseFile + ".restore", customThumbnailPath);
			}
		}

		public void SaveInBackground()
		{
			if (database == null || saveDatabaseQueue.IsActive || !Database.IsDirty)
			{
				return;
			}
			Database.IsDirty = false;
			saveDatabaseQueue.AddItem(Database, delegate
			{
				try
				{
					ComicDatabase.Attach(Database, Database.ComicStorage == null).SaveXml(DatabaseFile + ".xml", commitCache: false);
				}
				catch (Exception)
				{
				}
			});
		}
	}
}
