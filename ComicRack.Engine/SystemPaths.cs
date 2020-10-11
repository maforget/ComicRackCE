using System;
using System.IO;
using System.Reflection;
using cYo.Common.Reflection;

namespace cYo.Projects.ComicRack.Engine
{
	public class SystemPaths
	{
		public static readonly string ProductName = AssemblyInfo.GetProductName(Assembly.GetEntryAssembly());

		public static readonly string CompanyName = AssemblyInfo.GetCompanyName(Assembly.GetEntryAssembly());

		public readonly string ApplicationPath;

		public readonly string ApplicationDataPath;

		public readonly string LocalApplicationDataPath;

		public readonly string DatabasePath;

		public readonly string ThumbnailCachePath;

		public readonly string ImageCachePath;

		public readonly string FileCachePath;

		public readonly string CustomThumbnailPath;

		public readonly string ScriptPath;

		public readonly string ScriptPathSecondary;

		public readonly string PendingScriptsPath;

		public SystemPaths(bool useLocal, string alternateConfig, string databasePath, string cachePath)
		{
			ApplicationPath = Path.GetDirectoryName((Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).Location);
			if (useLocal)
			{
				ApplicationDataPath = Path.Combine(ApplicationPath, "Data");
				LocalApplicationDataPath = Path.Combine(ApplicationPath, "Data");
			}
			else
			{
				ApplicationDataPath = MakeApplicationPath(alternateConfig, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
				LocalApplicationDataPath = MakeApplicationPath(alternateConfig, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
			}
			DatabasePath = Path.Combine(string.IsNullOrEmpty(databasePath) ? ApplicationDataPath : databasePath, "ComicDb");
			ThumbnailCachePath = Path.Combine(string.IsNullOrEmpty(cachePath) ? Path.Combine(LocalApplicationDataPath, "Cache") : cachePath, "Thumbnails");
			ImageCachePath = Path.Combine(string.IsNullOrEmpty(cachePath) ? Path.Combine(LocalApplicationDataPath, "Cache") : cachePath, "Images");
			FileCachePath = Path.Combine(string.IsNullOrEmpty(cachePath) ? Path.Combine(LocalApplicationDataPath, "Cache") : cachePath, "Files");
			CustomThumbnailPath = Path.Combine(string.IsNullOrEmpty(cachePath) ? Path.Combine(LocalApplicationDataPath, "Cache") : cachePath, "CustomThumbnails");
			ScriptPath = Path.Combine(ApplicationPath, "Scripts");
			ScriptPathSecondary = Path.Combine(ApplicationDataPath, "Scripts");
			PendingScriptsPath = Path.Combine(ScriptPathSecondary, ".Pending");
		}

		public static string MakeApplicationPath(string alternateConfig, string folder)
		{
			if (!string.IsNullOrEmpty(CompanyName))
			{
				folder = Path.Combine(folder, CompanyName);
			}
			if (!string.IsNullOrEmpty(ProductName))
			{
				folder = Path.Combine(folder, ProductName);
			}
			if (!string.IsNullOrEmpty(alternateConfig))
			{
				folder = Path.Combine(folder, Path.Combine("Configurations", alternateConfig));
			}
			Directory.CreateDirectory(folder);
			return folder;
		}
	}
}
