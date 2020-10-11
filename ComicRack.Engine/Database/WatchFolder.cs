using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	public class WatchFolder : DisposableObject
	{
		private string folder;

		private bool watch;

		[NonSerialized]
		private FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();

		[XmlAttribute]
		public string Folder
		{
			get
			{
				return folder;
			}
			set
			{
				if (!(folder == value))
				{
					if (value == null)
					{
						throw new ArgumentNullException();
					}
					folder = value;
					UpdateWatcher(watch);
				}
			}
		}

		[DefaultValue(false)]
		[XmlAttribute]
		public bool Watch
		{
			get
			{
				return watch;
			}
			set
			{
				if (watch != value)
				{
					watch = value;
					UpdateWatcher(watch);
				}
			}
		}

		public FileSystemWatcher FileSystemWatcher => fileSystemWatcher;

		public WatchFolder()
		{
		}

		public WatchFolder(string path, bool watch)
		{
			Folder = path;
			this.watch = watch;
		}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			UpdateWatcher();
		}

		private void UpdateWatcher(bool watch)
		{
			try
			{
				fileSystemWatcher.EnableRaisingEvents = watch;
				fileSystemWatcher.IncludeSubdirectories = true;
				fileSystemWatcher.Path = folder;
			}
			catch (Exception)
			{
				fileSystemWatcher.EnableRaisingEvents = false;
			}
		}

		private void UpdateWatcher()
		{
			UpdateWatcher(watch);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				UpdateWatcher(watch: false);
				fileSystemWatcher.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
