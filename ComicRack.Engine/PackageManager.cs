using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using cYo.Common.Collections;
using cYo.Common.Drawing;
using cYo.Common.IO;
using cYo.Common.Runtime;
using cYo.Common.Text;
using ICSharpCode.SharpZipLib.Zip;

namespace cYo.Projects.ComicRack.Engine
{
	public class PackageManager
	{
		public enum PackageType
		{
			None,
			Installed,
			PendingInstall,
			PendingRemove
		}

		public class Package
		{
			public string Name
			{
				get;
				private set;
			}

			public string PackagePath
			{
				get;
				private set;
			}

			public PackageType PackageType
			{
				get;
				private set;
			}

			public string Description
			{
				get;
				private set;
			}

			public string Author
			{
				get;
				private set;
			}

			public string Version
			{
				get;
				private set;
			}

			public string HelpLink
			{
				get;
				private set;
			}

			public Image Image
			{
				get;
				private set;
			}

			public bool Installed
			{
				get
				{
					if (PackageType != PackageType.Installed)
					{
						return PackageType == PackageType.PendingInstall;
					}
					return true;
				}
			}

			public string[] KeepFiles
			{
				get;
				private set;
			}

			private Package(string name)
			{
				Name = name;
			}

			private T GetValue<T>(string value, T def)
			{
				return IniFile.GetValue(Path.Combine(PackagePath, "package.ini"), value, def);
			}

			private void InitValues()
			{
				IniFile iniFile = new IniFile(Path.Combine(PackagePath, "package.ini"));
				Name = iniFile.GetValue("Name", FileToName(Name));
				Description = iniFile.GetValue("Description", string.Empty);
				Author = iniFile.GetValue("Author", string.Empty);
				Version = iniFile.GetValue("Version", string.Empty);
				HelpLink = iniFile.GetValue("HelpLink", string.Empty);
				KeepFiles = iniFile.GetValue("KeepFiles", string.Empty).Split(',').TrimStrings()
					.RemoveEmpty()
					.ToArray();
				try
				{
					Image = BitmapExtensions.BitmapFromFile(Path.Combine(PackagePath, iniFile.GetValue("Image", string.Empty))).Scale(32, 32);
				}
				catch
				{
				}
			}

			private static string FileToName(string file)
			{
				return Path.GetFileNameWithoutExtension(file).RemoveDigits().Replace(".", " ")
					.Trim()
					.StartToUpper()
					.PascalToSpaced();
			}

			public static void UnzipFile(string packagePath, string targetPath)
			{
				Directory.CreateDirectory(targetPath);
				using (ZipFile zipFile = new ZipFile(packagePath))
				{
					foreach (ZipEntry item in from ZipEntry ze in zipFile
						where ze.IsFile
						select ze)
					{
						using (FileStream destination = File.Create(Path.Combine(targetPath, Path.GetFileName(item.Name))))
						{
							using (Stream stream = zipFile.GetInputStream(item))
							{
								stream.CopyTo(destination);
							}
						}
					}
				}
			}

			public static string GetName(string file)
			{
				try
				{
					return CreateFromFile(file).Name;
				}
				catch (Exception)
				{
					return null;
				}
			}

			public static Package CreateFromPath(string name, string path, bool pending)
			{
				Package package = new Package(name)
				{
					PackagePath = path
				};
				package.InitValues();
				if (pending)
				{
					package.PackageType = PackageType.PendingInstall;
				}
				else if (File.Exists(Path.Combine(path, ".remove")))
				{
					package.PackageType = PackageType.PendingRemove;
				}
				else
				{
					package.PackageType = PackageType.Installed;
				}
				return package;
			}

			public static Package CreateFromPath(string path, bool pending)
			{
				return CreateFromPath(FileToName(path), path, pending);
			}

			public static Package CreateFromFile(string file)
			{
				string text = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
				try
				{
					UnzipFile(file, text);
					Package package = CreateFromPath(FileToName(file), text, pending: false);
					package.PackagePath = file;
					package.PackageType = PackageType.None;
					return package;
				}
				catch (Exception)
				{
					return null;
				}
				finally
				{
					FileUtility.SafeDirectoryDelete(text);
				}
			}
		}

		private string packagePath;

		private string pendingPackagePath;

		public string PackagePath
		{
			get
			{
				return packagePath;
			}
			set
			{
				packagePath = value;
				try
				{
					Directory.CreateDirectory(packagePath);
				}
				catch
				{
				}
			}
		}

		public string PendingPackagePath
		{
			get
			{
				return pendingPackagePath;
			}
			set
			{
				pendingPackagePath = value;
				try
				{
					Directory.CreateDirectory(pendingPackagePath);
				}
				catch
				{
				}
			}
		}

		public bool IsValid
		{
			get
			{
				try
				{
					return Directory.Exists(PackagePath) && Directory.Exists(PendingPackagePath);
				}
				catch
				{
					return false;
				}
			}
		}

		public PackageManager(string path, string tempPath, bool commit)
		{
			PackagePath = path;
			PendingPackagePath = tempPath;
			if (commit)
			{
				Commit();
			}
		}

		public IList<Package> GetPackages()
		{
			List<Package> list = new List<Package>();
			try
			{
				string[] directories = Directory.GetDirectories(PackagePath);
				foreach (string text in directories)
				{
					if (!string.Equals(text, PendingPackagePath, StringComparison.OrdinalIgnoreCase))
					{
						list.Add(Package.CreateFromPath(text, pending: false));
					}
				}
				string[] directories2 = Directory.GetDirectories(PendingPackagePath);
				foreach (string path in directories2)
				{
					list.Add(Package.CreateFromPath(path, pending: true));
				}
				return list;
			}
			catch
			{
				return list;
			}
		}

		public IEnumerable<string> GetPackageNames()
		{
			return from p in GetPackages()
				select p.Name;
		}

		public bool PackageExists(string name)
		{
			return GetPackageNames().Contains(name, StringComparer.OrdinalIgnoreCase);
		}

		public bool PackageFileExists(string file)
		{
			try
			{
				return PackageExists(Package.CreateFromFile(file).Name);
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool Install(string packageFile)
		{
			Package package = Package.CreateFromFile(packageFile);
			if (package == null)
			{
				return false;
			}
			string text = GetPackagePath(package, pending: true);
			try
			{
				FileUtility.SafeDirectoryDelete(text);
				Package.UnzipFile(packageFile, text);
				return true;
			}
			catch (Exception)
			{
				FileUtility.SafeDirectoryDelete(text);
				return false;
			}
		}

		public bool Uninstall(Package package)
		{
			try
			{
				switch (package.PackageType)
				{
				case PackageType.PendingInstall:
					FileUtility.SafeDirectoryDelete(package.PackagePath);
					break;
				case PackageType.Installed:
					FileUtility.CreateEmpty(Path.Combine(package.PackagePath, ".remove"));
					break;
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

		public void Commit()
		{
			(from p in GetPackages()
				where p.PackageType == PackageType.PendingRemove
				select p).ForEach(delegate(Package p)
			{
				CommitUninstallPackage(p);
			});
			(from p in GetPackages()
				where p.PackageType == PackageType.PendingInstall
				select p).ForEach(delegate(Package p)
			{
				CommitInstallPackage(p);
			});
		}

		public void RemovePending()
		{
			GetPackages().ForEach(delegate(Package p)
			{
				FileUtility.SafeDelete(Path.Combine(p.PackagePath, ".remove"));
			});
			FileUtility.SafeDirectoryDelete(PendingPackagePath);
		}

		private bool CommitInstallPackage(Package package)
		{
			if (package.PackageType != PackageType.PendingInstall)
			{
				return false;
			}
			string text = GetPackagePath(package, pending: false);
			try
			{
				Directory.CreateDirectory(text);
				if (!package.KeepFiles.IsEmpty())
				{
                    string[] keep = package.KeepFiles;

                    // Convert wildcard patterns to regular expressions
                    List<Regex> keepPatterns = keep.Select(pattern =>
                    {
                        string regexPattern = $"^{Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".")}$";
                        return new Regex(regexPattern, RegexOptions.IgnoreCase);
                    }).ToList();

                    foreach (string item in from f in new DirectoryInfo(text).EnumerateFiles().Select(x => x.Name)
											where !keepPatterns.Any(regex => regex.IsMatch(f))
                                            select f)
					{
						FileUtility.SafeDelete(item);
					}

                    foreach (string item2 in from f in new DirectoryInfo(text).EnumerateDirectories().Select(x => x.Name)
                                             where !keepPatterns.Any(regex => regex.IsMatch(f))
                                            select f)
                    {
						FileUtility.SafeDirectoryDelete(item2);
					}
				}
				string[] files = Directory.GetFiles(package.PackagePath);
				foreach (string text2 in files)
				{
					File.Copy(text2, Path.Combine(text, Path.GetFileName(text2)), overwrite: true);
				}
				return true;
			}
			catch
			{
				FileUtility.SafeDirectoryDelete(text);
				return false;
			}
			finally
			{
				FileUtility.SafeDirectoryDelete(package.PackagePath);
			}
		}

		private static bool CommitUninstallPackage(Package package)
		{
			return FileUtility.SafeDirectoryDelete(package.PackagePath);
		}

		private string GetPackagePath(Package package, bool pending)
		{
			return Path.Combine(pending ? PendingPackagePath : PackagePath, package.Name);
		}
	}
}
