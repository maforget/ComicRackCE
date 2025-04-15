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
					string text = null;
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
									text = EngineConfiguration.Default.GetTempFileName();
									comicInfo = storageProvider.Store(provider, comicInfo, text, setting);
									ShellFile.DeleteFile(targetPath);
									File.Move(text, targetPath);
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
						FileUtility.SafeDelete(text ?? targetPath);
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
