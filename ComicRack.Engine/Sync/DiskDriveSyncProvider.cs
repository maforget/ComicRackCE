using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using cYo.Common.IO;

namespace cYo.Projects.ComicRack.Engine.Sync
{
	public class DiskDriveSyncProvider : SyncProviderBase
	{
		private readonly string syncPath;

		private readonly string rootPath;

		public DiskDriveSyncProvider(string rootPath, string deviceKey = null)
		{
			DriveInfo driveInfo = FileUtility.GetDriveInfo(rootPath);
			if (driveInfo == null || !driveInfo.IsReady)
			{
				throw new ArgumentException();
			}
			this.rootPath = rootPath;
			syncPath = FileUtility.GetFolders(rootPath, 3).FirstOrDefault((string fullPath) => FileUtility.SafeFileExists(Path.Combine(fullPath, MarkerFile)));
			if (syncPath == null)
			{
				throw new DriveNotFoundException();
			}
			if (!ReadMarkerFile(deviceKey))
			{
				throw new DriveNotFoundException();
			}
		}

		protected override void OnStart()
		{
		}

		protected override void OnCompleted()
		{
		}

		protected override bool FileExists(string file)
		{
			return File.Exists(GetFullPath(file));
		}

		protected override void WriteFile(string file, Stream data)
		{
			FileUtility.WriteStream(GetFullPath(file), data);
		}

		protected override Stream ReadFile(string fileName)
		{
			return File.OpenRead(GetFullPath(fileName));
		}

		protected override void DeleteFile(string fileName)
		{
			FileUtility.SafeDelete(GetFullPath(fileName));
		}

		protected override long GetFreeSpace()
		{
			return FileUtility.GetDriveInfo(rootPath).AvailableFreeSpace;
		}

		protected override IEnumerable<string> GetFileList()
		{
			return Directory.EnumerateFiles(syncPath).Select(Path.GetFileName);
		}

		private string GetFullPath(string fileName)
		{
			return Path.Combine(syncPath, fileName);
		}

		public static IEnumerable<DriveInfo> GetRemoveableDrives()
		{
			return from di in Environment.GetLogicalDrives().Select(FileUtility.GetDriveInfo)
				where di.IsReady && di.DriveType == DriveType.Removable
				select di;
		}
	}
}
