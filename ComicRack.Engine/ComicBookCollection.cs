using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using cYo.Common.Collections;
using cYo.Common.Threading;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	public class ComicBookCollection : SmartList<ComicBook>, IDeserializationCallback
	{
		private readonly bool updateDictionaries;

		[NonSerialized]
		private readonly Dictionary<string, ComicBook> fileDictionary = new Dictionary<string, ComicBook>(StringComparer.OrdinalIgnoreCase);

		[NonSerialized]
		private readonly Dictionary<Guid, ComicBook> guidDictionary = new Dictionary<Guid, ComicBook>();

		public ComicBook this[string file] => FindItemByFile(file);

		public ComicBook this[Guid id] => FindItemById(id);

		public ComicBookCollection(IEnumerable<ComicBook> items = null, bool updateDictionaries = true)
		{
			this.updateDictionaries = updateDictionaries;
			if (items != null)
			{
				AddRange(items);
			}
		}

		protected override void OnInsertCompleted(int index, ComicBook item)
		{
			OnBookAdded(item);
			base.OnInsertCompleted(index, item);
		}

		protected override void OnRemoveCompleted(int index, ComicBook item)
		{
			base.OnRemoveCompleted(index, item);
			OnBookRemoved(item);
		}

		private void ComicBookFile_Renamed(object sender, ComicBookFileRenameEventArgs e)
		{
			using (ItemMonitor.Lock(fileDictionary))
			{
				fileDictionary.Remove(e.OldFile);
				if (!string.IsNullOrEmpty(e.NewFile))
				{
					fileDictionary[e.NewFile] = sender as ComicBook;
				}
			}
		}

		public void Remove(string file)
		{
			Remove(this[file]);
		}

		public ComicBook FindItemByFile(string file)
		{
			using (GetLock(write: false))
			{
				using (ItemMonitor.Lock(fileDictionary))
				{
					ComicBook value;
					return fileDictionary.TryGetValue(file, out value) ? value : null;
				}
			}
		}

		public ComicBook FindItemById(Guid id)
		{
			using (GetLock(write: false))
			{
				using (ItemMonitor.Lock(guidDictionary))
				{
					ComicBook value;
					return guidDictionary.TryGetValue(id, out value) ? value : null;
				}
			}
		}

		public ComicBook FindItemByFileName(string fileName)
		{
			return Find((ComicBook cb) => string.Equals(cb.FileName, fileName, StringComparison.OrdinalIgnoreCase));
		}

		public ComicBook FindItemByFileNameSize(string file)
		{
			FileInfo fi = new FileInfo(file);
			string name = Path.GetFileNameWithoutExtension(file);
			return Find((ComicBook cb) => string.Equals(cb.FileName, name, StringComparison.OrdinalIgnoreCase) && cb.FileSize == fi.Length);
		}

		public ComicBook FindItemByHash(ComicBook comicBook)
		{
			string hash = comicBook.Hash(asIndex: true);
			if (!string.IsNullOrEmpty(hash))
			{
				return Find((ComicBook cb) => string.Equals(cb.CreatePageHash(), hash, StringComparison.OrdinalIgnoreCase));
			}
			return null;
		}

		private void OnBookAdded(ComicBook item)
		{
			using (ItemMonitor.Lock(fileDictionary))
			{
				if (item.IsLinked)
				{
					fileDictionary[item.FilePath] = item;
				}
				guidDictionary[item.Id] = item;
			}
			if (updateDictionaries)
			{
				item.FileRenamed += ComicBookFile_Renamed;
			}
		}

		private void OnBookRemoved(ComicBook item)
		{
			using (ItemMonitor.Lock(fileDictionary))
			{
				if (item.IsLinked)
				{
					fileDictionary.Remove(item.FilePath);
				}
				guidDictionary.Remove(item.Id);
			}
			if (updateDictionaries)
			{
				item.FileRenamed -= ComicBookFile_Renamed;
			}
		}

		void IDeserializationCallback.OnDeserialization(object sender)
		{
			ForEach(OnBookAdded);
		}

		public static IEnumerable<ComicBook> Filter(ComicBookFilterType filter, IEnumerable<ComicBook> books)
		{
			if (filter.HasFlag(ComicBookFilterType.Library))
			{
				books = books.Where((ComicBook cb) => cb.IsInContainer);
			}
			if (filter.HasFlag(ComicBookFilterType.NotInLibrary))
			{
				books = books.Where((ComicBook cb) => !cb.IsInContainer);
			}
			if (filter.HasFlag(ComicBookFilterType.IsLocal))
			{
				books = books.Where((ComicBook cb) => cb.EditMode.IsLocalComic());
			}
			if (filter.HasFlag(ComicBookFilterType.IsNotLocal))
			{
				books = books.Where((ComicBook cb) => !cb.EditMode.IsLocalComic());
			}
			if (filter.HasFlag(ComicBookFilterType.IsFileless))
			{
				books = books.Where((ComicBook cb) => !cb.IsLinked);
			}
			if (filter.HasFlag(ComicBookFilterType.IsNotFileless))
			{
				books = books.Where((ComicBook cb) => cb.IsLinked);
			}
			if (filter.HasFlag(ComicBookFilterType.IsEditable))
			{
				books = books.Where((ComicBook cb) => cb.EditMode.CanEditProperties());
			}
			if (filter.HasFlag(ComicBookFilterType.IsNotEditable))
			{
				books = books.Where((ComicBook cb) => !cb.EditMode.CanEditProperties());
			}
			if (filter.HasFlag(ComicBookFilterType.CanExport))
			{
				books = books.Where((ComicBook cb) => cb.EditMode.CanExport());
			}
			if (filter.HasFlag(ComicBookFilterType.AsArray))
			{
				books = books.Lock().ToArray();
			}
			return books;
		}
	}
}
