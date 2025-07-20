using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using cYo.Common;
using cYo.Common.IO;
using cYo.Common.Localize;
using cYo.Common.Text;
using cYo.Common.Win32;
using cYo.Projects.ComicRack.Engine.IO.Cache;
using cYo.Projects.ComicRack.Engine.IO.Provider;
using cYo.Projects.ComicRack.Engine.IO.Provider.Readers;

namespace cYo.Projects.ComicRack.Engine.IO
{
	public class ComicExporter
	{
		private ExportSetting setting;

		private readonly List<ComicBook> comicBooks;

		private ComicInfo comicInfo;

		private volatile string lastError;

		private readonly int sequence;

		public ExportSetting Setting => setting;

		public ComicBook ComicBook => comicBooks[0];

		public List<ComicBook> ComicBooks => comicBooks;

		public ComicInfo ComicInfo => comicInfo;

		public string LastError => lastError;

		public int Sequence => sequence;

		public event EventHandler<StorageProgressEventArgs> Progress;

		public FileIsInDatabaseChecker FileIsInDatabase { get; set; }
		public delegate bool FileIsInDatabaseChecker(string targetPath, string sourcePath);


		public ComicExporter(IEnumerable<ComicBook> books, ExportSetting setting, int sequence)
		{
			comicBooks = books.ToList();
			comicInfo = CombinedComics.GetComicInfo(comicBooks);
			comicInfo.Tags = comicInfo.Tags.AppendUniqueValueToList(setting.TagsToAppend);
			this.setting = setting;
			this.sequence = sequence;
		}

		public string Export(IPagePool pagePool)
		{
			try
			{
				string targetPath = setting.GetTargetPath(ComicBook, sequence);
				if (File.Exists(targetPath) && !setting.Overwrite && setting.Target != ExportTarget.ReplaceSource)
				{
					throw new InvalidOperationException(StringUtility.Format(TR.Messages["OutputFileExists", "Output file '{0}' already exists!"], targetPath));
				}
				// Check if the file already exists in the database. Overwrite should always be false, added just in case
				bool existsInDatabase = FileIsInDatabase?.Invoke(targetPath, ComicBook.FilePath) ?? false;
				if (File.Exists(targetPath) && existsInDatabase && !setting.Overwrite && setting.Target == ExportTarget.ReplaceSource)
				{
					// If the file exists in the database and we are replacing the source, we throw an exception
					throw new InvalidOperationException(StringUtility.Format(TR.Messages["AlreadyExistsInDatabase", "Resulting operation would result in a duplicate entry in the database, Output file '{0}' already exists in the library"], targetPath));
				}
				if ((setting.AddToLibrary || setting.Target == ExportTarget.ReplaceSource) && Providers.Readers.GetFormatProviderType(setting.FormatId) == null)
				{
					throw new ArgumentException(TR.Messages["InvalidExportSettings", "The export settings do not match (e.g. adding a not supported format to the library)"]);
				}
				if (setting.ImageProcessingSource == ExportImageProcessingSource.FromComic)
				{
					setting = CloneUtility.Clone(setting);
					setting.ImageProcessing = ComicBook.ColorAdjustment.ChangeOption(setting.ImageProcessing.Options);
					if (setting.ImageProcessing.Sharpen != 0)
					{
						setting.ImageProcessing = setting.ImageProcessing.ChangeSharpness(setting.ImageProcessing.Sharpen);
					}
				}
				using (IImageProvider provider = CombinedComics.OpenProvider(comicBooks, pagePool))
				{
					FileFormat fileFormat = setting.GetFileFormat(ComicBook);
					string tempFile = null;
					try
					{
						using (StorageProvider storageProvider = Providers.Writers.CreateFormatProvider(fileFormat.Name))
						{
							if (storageProvider == null)
							{
								return null;
							}
							try
							{
								storageProvider.Progress += writer_Progress;
								if (File.Exists(targetPath))
								{
									tempFile = EngineConfiguration.Default.GetTempFileName();
									comicInfo = storageProvider.Store(provider, comicInfo, tempFile, setting);
									ShellFile.DeleteFile(targetPath);
									File.Move(tempFile, targetPath);
								}
								else
								{
									comicInfo = storageProvider.Store(provider, comicInfo, targetPath, setting);
								}
							}
							finally
							{
								storageProvider.Progress -= writer_Progress;
							}
						}
					}
					catch
					{
						FileUtility.SafeDelete(tempFile ?? targetPath);
						throw;
					}
				}
				return targetPath;
			}
			catch (Exception ex)
			{
				lastError = ex.Message;
				throw;
			}
		}

		private void writer_Progress(object sender, StorageProgressEventArgs e)
		{
			OnProgress(e);
		}

		protected virtual void OnProgress(StorageProgressEventArgs e)
		{
			if (this.Progress != null)
			{
				this.Progress(this, e);
			}
		}
	}
}
