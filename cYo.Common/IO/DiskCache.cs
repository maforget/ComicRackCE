using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using cYo.Common.ComponentModel;
using cYo.Common.Runtime;
using cYo.Common.Text;
using cYo.Common.Threading;
using System.Xml.Serialization;

namespace cYo.Common.IO
{
	public abstract class DiskCache<K, T> : DisposableObject, IDiskCache<K, T>, IDisposable
	{
		[Serializable]
		public class CacheItem
		{
			private long length;
			private static Type[] extraTypes;

			public string File
			{
				get;
				set;
			}

			public K Key
			{
				get;
				set;
			}

			public long Length
			{
				get
				{
					return Interlocked.Read(ref length);
				}
				set
				{
					Interlocked.Exchange(ref length, value);
				}
			}

			public string FileName => Path.GetFileName(File);

			public CacheItem()
			{
			}

			public CacheItem(K key, string file, long length)
			{
				if (Path.IsPathRooted(file))
				{
					throw new ArgumentException("No rooted paths");
				}
				Key = key;
				File = file;
				this.length = length;
			}

			public static Type[] GetExtraXmlSerializationTypes()
			{
				if (extraTypes == null)
				{
					List<Type> list = new List<Type>();
					list.Add(typeof(K));
					list.AddRange(GetDerivedTypes(typeof(K)));
					extraTypes = list.ToArray();
				}
				return extraTypes;
			}

			private static IEnumerable<Type> GetDerivedTypes(Type baseType)
			{
				return AppDomain.CurrentDomain.GetAssemblies()
					.SelectMany(assembly => assembly.GetTypes())
					.Where(type => type.IsSubclassOf(baseType));
			}
		}

		private const string indexFile = "cache.idx";

		private readonly string cacheIndex;

		private readonly Dictionary<K, LinkedListNode<CacheItem>> fileDict = new Dictionary<K, LinkedListNode<CacheItem>>();

		private readonly LinkedList<CacheItem> fileList = new LinkedList<CacheItem>();

		private readonly LockFile lockFile;

		private Timer indexSaver;

		private readonly string cacheFolder;

		private volatile int cacheSizeMB = 50;

		private long size;

		private volatile bool enabled = true;

		private readonly Dictionary<K, ManualResetEvent> creationLocks = new Dictionary<K, ManualResetEvent>();

		public string CacheFolder => cacheFolder;

		public int CacheSizeMB
		{
			get
			{
				return cacheSizeMB;
			}
			set
			{
				cacheSizeMB = value;
				CleanUp();
			}
		}

		public long Size => Interlocked.Read(ref size);

		public int Count
		{
			get
			{
				using (ItemMonitor.Lock(fileList))
				{
					return fileList.Count;
				}
			}
		}

		public bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				enabled = value;
			}
		}

		public bool CacheIndexDirty
		{
			get;
			set;
		}

		public event EventHandler SizeChanged;

		protected DiskCache(string cacheFolder, int cacheSizeMB, int saveIndex = 10)
		{
			this.cacheFolder = cacheFolder;
			this.cacheSizeMB = cacheSizeMB;
			cacheIndex = Path.Combine(cacheFolder, indexFile);
			try
			{
				Directory.CreateDirectory(cacheFolder);
				lockFile = new LockFile(Path.Combine(cacheFolder, "cache.lock"));
				List<CacheItem> list = LoadCacheIndex(cacheIndex);
				foreach (CacheItem item in list)
				{
					LinkedListNode<CacheItem> value = fileList.AddLast(item);
					try
					{
						fileDict.Add(item.Key, value);
						size += item.Length;
					}
					catch (Exception)
					{
						FileUtility.SafeDelete(Path.Combine(cacheFolder, item.File));
						fileList.RemoveLast();
					}
				}
				if (list.Count == 0)
				{
					Clear();
				}
				if (lockFile.WasLocked)
				{
					ThreadUtility.CreateWorkerThread("Cache cleanup", BackgroundCleanUp, ThreadPriority.Lowest);
				}
			}
			catch (Exception)
			{
				enabled = false;
			}
			if (saveIndex == 0)
			{
				return;
			}
			indexSaver = new Timer(delegate
			{
				if (CacheIndexDirty)
				{
					SaveCacheIndex(cacheIndex);
				}
			}, null, 1000 * saveIndex, 1000 * saveIndex);
		}

		protected virtual string CreateCacheFileName()
		{
			byte[] array = Guid.NewGuid().ToByteArray();
			string str = array[0].ToString();
			char directorySeparatorChar = Path.DirectorySeparatorChar;
			return str + directorySeparatorChar + Base32.ToBase32String(array) + ".cache";
		}

		protected abstract T LoadItem(string file);

		protected abstract void StoreItem(string file, T item);

		protected override void Dispose(bool disposing)
		{
			indexSaver.SafeDispose();
			SaveCacheIndex(cacheIndex);
			if (lockFile != null)
			{
				lockFile.Dispose();
			}
			base.Dispose(disposing);
		}

		protected virtual void OnSizeChanged()
		{
			if (this.SizeChanged != null)
			{
				this.SizeChanged(this, EventArgs.Empty);
			}
		}

		private void IncSize(long add)
		{
			Interlocked.Add(ref size, add);
			OnSizeChanged();
		}

		private static List<CacheItem> LoadCacheIndex(string cacheIndexFile)
		{
			try
			{
				string cacheIndexLegacy = cacheIndexFile;
				string cacheIndexXml = $"{cacheIndexFile}.xml";
				List<CacheItem> index = new List<CacheItem>();

				try
				{
					if (File.Exists(cacheIndexXml))
					{
						index = LoadCacheIndexXml(cacheIndexXml);
					}
				}
				catch (Exception)
				{
				}

				if (index.Count > 0)
					return index;

				return LoadCacheIndexBinary(cacheIndexLegacy);
			}
			catch (Exception)
			{
				return new List<CacheItem>();
			}
		}

		private static List<CacheItem> LoadCacheIndexBinary(string cacheIndexFile)
		{
			try
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter
				{
					Binder = new VersionNeutralBinder()
				};
				using (Stream serializationStream = File.OpenRead(cacheIndexFile))
				{
					return (List<CacheItem>)binaryFormatter.Deserialize(serializationStream);
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		private static List<CacheItem> LoadCacheIndexXml(string cacheIndexFile)
		{
			try
			{
				using (FileStream inStream = File.OpenRead(cacheIndexFile))
				{
					return GetSerializer().Deserialize(inStream) as List<CacheItem>;
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		public void SaveCacheIndex(string cacheIndexFile)
		{
			string cacheIndexLegacy = cacheIndexFile;
			string cacheIndexXml = $"{cacheIndexFile}.xml";

			try
			{
				SaveCacheIndexXml(cacheIndexXml);
			}
			catch (Exception)
			{
			}
			finally
			{
				SaveCacheIndexBinary(cacheIndexLegacy);
			}
		}

		public void SaveCacheIndexBinary(string cacheIndexFile)
		{
			lock (this)
			{
				try
				{
					CacheIndexDirty = false;
					BinaryFormatter binaryFormatter = new BinaryFormatter
					{
						TypeFormat = FormatterTypeStyle.TypesWhenNeeded
					};
					List<CacheItem> graph;
					using (ItemMonitor.Lock(fileList))
					{
						graph = fileList.ToList();
					}
					using (Stream serializationStream = File.Create(cacheIndexFile))
					{
						binaryFormatter.Serialize(serializationStream, graph);
					}
				}
				catch (Exception)
				{
				}
			}
		}

		public void SaveCacheIndexXml(string cacheIndexFile)
		{
			lock (this)
			{
				try
				{
					CacheIndexDirty = false;
					XmlSerializer serializer = GetSerializer();
					List<CacheItem> graph;
					using (ItemMonitor.Lock(fileList))
					{
						graph = fileList.ToList();
					}
					using (Stream serializationStream = File.Create(cacheIndexFile))
					{
						serializer.Serialize(serializationStream, graph);
					}
				}
				catch (Exception)
				{
					throw;
				}
			}
		}

		private static XmlSerializer GetSerializer()
		{
			return new XmlSerializer(typeof(List<CacheItem>), CacheItem.GetExtraXmlSerializationTypes());
		}

		private LinkedListNode<CacheItem> GetCacheItem(K key)
		{
			if (!Enabled)
			{
				return null;
			}
			using (ItemMonitor.Lock(fileDict))
			{
				if (fileDict.TryGetValue(key, out var value) && !File.Exists(GetFullPath(value.Value)))
				{
					using (ItemMonitor.Lock(fileList))
					{
						try
						{
							fileList.Remove(value);
						}
						catch (InvalidOperationException)
						{
						}
					}
					fileDict.Remove(key);
					return null;
				}
				return value;
			}
		}

		private string GetFullPath(CacheItem cacheItem)
		{
			return Path.Combine(CacheFolder, cacheItem.File);
		}

		private void BackgroundCleanUp()
		{
			ILookup<string, K> lookup;
			using (ItemMonitor.Lock(fileList))
			{
				lookup = fileList.ToLookup(GetFullPath, (CacheItem f) => f.Key);
			}
			foreach (string file in FileUtility.GetFiles(CacheFolder, SearchOption.AllDirectories, ".cache"))
			{
				if (lookup[file].DefaultIfEmpty() == null)
				{
					FileUtility.SafeDelete(file);
				}
			}
		}

		public bool IsAvailable(K key)
		{
			if (Enabled)
			{
				return GetCacheItem(key) != null;
			}
			return false;
		}

		public T GetItem(K key)
		{
			LinkedListNode<CacheItem> cacheItem = GetCacheItem(key);
			T result = default(T);
			if (cacheItem != null)
			{
				using (ItemMonitor.Lock(cacheItem))
				{
					try
					{
						result = LoadItem(GetFullPath(cacheItem.Value));
						using (ItemMonitor.Lock(fileList))
						{
							fileList.Remove(cacheItem);
							fileList.AddFirst(cacheItem);
							CacheIndexDirty = true;
							return result;
						}
					}
					catch (Exception)
					{
						RemoveItem(key);
						return result;
					}
				}
			}
			return result;
		}

		public bool AddItem(K key, T item)
		{
			if (!Enabled)
			{
				return false;
			}
			ManualResetEvent manualResetEvent = null;
			ManualResetEvent value;
			using (ItemMonitor.Lock(creationLocks))
			{
				if (!creationLocks.TryGetValue(key, out value))
				{
					creationLocks.Add(key, manualResetEvent = new ManualResetEvent(initialState: false));
				}
			}
			if (value != null)
			{
				try
				{
					value.WaitOne();
				}
				catch
				{
				}
				return true;
			}
			try
			{
				LinkedListNode<CacheItem> cacheItem = GetCacheItem(key);
				if (cacheItem != null)
				{
					return false;
				}
				CacheItem cacheItem2 = new CacheItem(key, CreateCacheFileName(), 0L);
				string fullPath = GetFullPath(cacheItem2);
				string directoryName = Path.GetDirectoryName(fullPath);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				StoreItem(fullPath, item);
				cacheItem2.Length = new FileInfo(fullPath).Length;
				using (ItemMonitor.Lock(fileList))
				{
					using (ItemMonitor.Lock(fileDict))
					{
						fileDict[key] = fileList.AddFirst(cacheItem2);
					}
				}
				IncSize(cacheItem2.Length);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
			finally
			{
				using (ItemMonitor.Lock(creationLocks))
				{
					creationLocks.Remove(key);
				}
				if (manualResetEvent != null)
				{
					manualResetEvent.Set();
					manualResetEvent.Close();
				}
				CleanUp();
				CacheIndexDirty = true;
			}
		}

		public void RemoveItem(K key)
		{
			LinkedListNode<CacheItem> cacheItem = GetCacheItem(key);
			if (cacheItem == null)
			{
				return;
			}
			using (ItemMonitor.Lock(cacheItem))
			{
				FileUtility.SafeDelete(GetFullPath(cacheItem.Value));
				using (ItemMonitor.Lock(fileList))
				{
					fileDict.Remove(key);
					try
					{
						fileList.Remove(cacheItem);
					}
					catch (Exception)
					{
					}
				}
			}
			IncSize(-cacheItem.Value.Length);
			CacheIndexDirty = true;
		}

		public void UpdateKeys(Func<K, bool> select, Action<K> update)
		{
			List<LinkedListNode<CacheItem>> list = new List<LinkedListNode<CacheItem>>();
			using (ItemMonitor.Lock(fileList))
			{
				for (LinkedListNode<CacheItem> linkedListNode = fileList.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					if (select(linkedListNode.Value.Key))
					{
						list.Add(linkedListNode);
					}
				}
			}
			using (ItemMonitor.Lock(fileDict))
			{
				foreach (LinkedListNode<CacheItem> item in list)
				{
					K key = item.Value.Key;
					fileDict.Remove(key);
					update(key);
					fileDict[key] = item;
				}
			}
			CacheIndexDirty = true;
		}

		public void RemoveKeys(Func<K, bool> select)
		{
			List<K> list = new List<K>();
			using (ItemMonitor.Lock(fileList))
			{
				for (LinkedListNode<CacheItem> linkedListNode = fileList.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					if (select(linkedListNode.Value.Key))
					{
						list.Add(linkedListNode.Value.Key);
					}
				}
			}
			list.ForEach(RemoveItem);
		}

		public K[] GetKeys()
		{
			using (ItemMonitor.Lock(fileList))
			{
				return fileList.Select((CacheItem lln) => lln.Key).ToArray();
			}
		}

		public void CleanUp(long checkSize)
		{
			using (ItemMonitor.Lock(fileList))
			{
				LinkedListNode<CacheItem> linkedListNode = fileList.Last;
				long num = Size;
				while (linkedListNode != null && num > checkSize)
				{
					LinkedListNode<CacheItem> linkedListNode2 = linkedListNode;
					linkedListNode = linkedListNode2.Previous;
					try
					{
						FileUtility.SafeDelete(GetFullPath(linkedListNode2.Value));
						IncSize(-linkedListNode2.Value.Length);
						num -= linkedListNode2.Value.Length;
						fileList.RemoveLast();
						fileDict.Remove(linkedListNode2.Value.Key);
					}
					catch (Exception)
					{
					}
				}
			}
			CacheIndexDirty = true;
		}

		public void CleanUp()
		{
			CleanUp((long)cacheSizeMB * 1024L * 1024);
		}

		public void Clear()
		{
			using (ItemMonitor.Lock(fileList))
			{
				using (ItemMonitor.Lock(fileDict))
				{
					string[] directories = Directory.GetDirectories(CacheFolder);
					foreach (string path in directories)
					{
						try
						{
							Directory.Delete(path, recursive: true);
						}
						catch (Exception)
						{
						}
					}
					fileList.Clear();
					fileDict.Clear();
					if (Interlocked.Exchange(ref size, 0L) != 0L)
					{
						OnSizeChanged();
					}
				}
			}
			CacheIndexDirty = true;
		}
	}
}
