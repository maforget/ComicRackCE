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

		public static string ApplicationPath => Path.GetDirectoryName((Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).Location);

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

		public readonly bool UseLocal;

		public readonly string AlternateConfig;

		public SystemPaths(bool useLocal, string alternateConfig, string databasePath, string cachePath)
		{
			UseLocal = useLocal; AlternateConfig = alternateConfig; 
			ApplicationDataPath = GetApplicationDataPath(useLocal, alternateConfig);
            LocalApplicationDataPath = GetLocalApplicationDataPath(useLocal, alternateConfig);

            DatabasePath = Path.Combine(string.IsNullOrEmpty(databasePath) ? ApplicationDataPath : databasePath, "ComicDb");
			ThumbnailCachePath = Path.Combine(string.IsNullOrEmpty(cachePath) ? Path.Combine(LocalApplicationDataPath, "Cache") : cachePath, "Thumbnails");
			ImageCachePath = Path.Combine(string.IsNullOrEmpty(cachePath) ? Path.Combine(LocalApplicationDataPath, "Cache") : cachePath, "Images");
			FileCachePath = Path.Combine(string.IsNullOrEmpty(cachePath) ? Path.Combine(LocalApplicationDataPath, "Cache") : cachePath, "Files");
			CustomThumbnailPath = Path.Combine(string.IsNullOrEmpty(cachePath) ? Path.Combine(LocalApplicationDataPath, "Cache") : cachePath, "CustomThumbnails");
			ScriptPath = Path.Combine(ApplicationPath, "Scripts");
			ScriptPathSecondary = Path.Combine(ApplicationDataPath, "Scripts");
			PendingScriptsPath = Path.Combine(ScriptPathSecondary, ".Pending");
		}

        private static string MakeApplicationPath(string alternateConfig, string folder, bool useLocal)
		{
			//When local, we don't want the Compagny & Product folder
			if (!useLocal && !string.IsNullOrEmpty(CompanyName))
			{
				folder = Path.Combine(folder, CompanyName);
			}
			if (!useLocal && !string.IsNullOrEmpty(ProductName))
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

		public static string GetApplicationDataPath(bool useLocal, string alternateConfig)
		{
            string appDataFolder = useLocal ? Path.Combine(ApplicationPath, "Data") : Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return MakeApplicationPath(alternateConfig, appDataFolder, useLocal);
        }

        public static string GetLocalApplicationDataPath(bool useLocal, string alternateConfig)
        {
            string localAppDataFolder = useLocal ? Path.Combine(ApplicationPath, "Data") : Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return MakeApplicationPath(alternateConfig, localAppDataFolder, useLocal);
        }
    }
}
