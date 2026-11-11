using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using cYo.Common.IO;
using cYo.Common.Runtime;
using cYo.Common.Windows.Forms.Theme;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Plugins.Compiler;

namespace cYo.Projects.ComicRack.Plugins
{
	// A source plugin is a folder with a "<Name>.csplugin" file. All .cs files of the folder are compiled into one
	// assembly, which is kept in the cache folder until a source, a reference or ComicRack itself changes.
	//
	// The .csplugin file is an INI file. All entries are optional:
	//   [Plugin]
	//   LangVersion=latest
	//   References=lib/Some.dll;lib/Other.dll              (relative to the plugin folder)
	//   FrameworkReferences=System.Web;System.Runtime.Serialization
	internal sealed class CSharpSourcePluginInitializer : PluginInitializer
	{
		private sealed class SourcePlugin
		{
			private static readonly string[] buildFolders = { "bin", "obj" };

			public string MarkerFile { get; private set; }

			public string Folder { get; private set; }

			public string Name { get; private set; }

			public string LanguageVersion { get; private set; }

			public List<string> SourceFiles { get; private set; }

			public List<string> References { get; private set; }

			public List<string> FrameworkReferences { get; private set; }

			public static SourcePlugin Read(string markerFile)
			{
				Dictionary<string, string> values = IniFile.ReadFile(markerFile, "Plugin");
				string folder = Path.GetDirectoryName(markerFile);
				var plugin = new SourcePlugin
				{
					MarkerFile = markerFile,
					Folder = folder,
					Name = Path.GetFileNameWithoutExtension(markerFile),
					LanguageVersion = values.TryGetValue("LangVersion", out string languageVersion) ? languageVersion.Trim() : null,
					SourceFiles = Directory.EnumerateFiles(folder, "*.cs", SearchOption.AllDirectories)
						.Where(file => ".cs".Equals(Path.GetExtension(file), StringComparison.OrdinalIgnoreCase) && !IsBuildOutput(folder, file))
						.OrderBy(file => file, StringComparer.OrdinalIgnoreCase)
						.ToList(),
					References = List(values, "References").Select(reference => FindReference(folder, reference)).ToList(),
					FrameworkReferences = List(values, "FrameworkReferences")
				};
				if (plugin.SourceFiles.Count == 0)
				{
					throw new FileNotFoundException("the folder contains no .cs files");
				}
				return plugin;
			}

			public string RelativePath(string file) => file.Substring(Folder.Length).TrimStart(Path.DirectorySeparatorChar);

			private static List<string> List(Dictionary<string, string> values, string name)
			{
				return values.TryGetValue(name, out string value)
					? value.Split(';').Select(item => item.Trim()).Where(item => item.Length > 0).ToList()
					: new List<string>();
			}

			// The author may also build the plugin with MSBuild in the same folder.
			private static bool IsBuildOutput(string folder, string file)
			{
				string first = file.Substring(folder.Length).TrimStart(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar)[0];
				return buildFolders.Contains(first, StringComparer.OrdinalIgnoreCase);
			}

			// The package installer of ComicRack puts all files of a .crplugin into one folder, so a reference may have lost its subfolder.
			private static string FindReference(string folder, string reference)
			{
				string file = Path.GetFullPath(Path.Combine(folder, reference));
				if (!File.Exists(file))
				{
					file = Path.Combine(folder, Path.GetFileName(file));
				}
				if (!File.Exists(file))
				{
					throw new FileNotFoundException($"the reference '{reference}' does not exist");
				}
				return file;
			}
		}

		private sealed class Reference
		{
			public string File { get; set; }

			// Changes when the content of the reference changes.
			public string Fingerprint { get; set; }
		}

		public const string MarkerExtension = ".csplugin";

		private const int KeyLength = 16;

		private const int FolderLength = 8;

		private static readonly string[] defaultFrameworkReferences =
		{
			"mscorlib", "netstandard", "Microsoft.CSharp", "System", "System.Core", "System.Data", "System.Drawing", "System.Net.Http", "System.Windows.Forms", "System.Xml", "System.Xml.Linq"
		};

		private static readonly Assembly[] hostReferences =
		{
			typeof(DotNetPlugin).Assembly, typeof(ComicBook).Assembly, typeof(FileUtility).Assembly, typeof(Themes).Assembly
		};

		private static readonly Regex cacheEntry = new Regex("^(?<entry>.+-[0-9a-f]{" + FolderLength + "}-[0-9a-f]{" + KeyLength + @"})\.(dll|pdb)$", RegexOptions.IgnoreCase);

		private readonly string cachePath;

		private readonly HashSet<string> usedCacheEntries = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		public CSharpSourcePluginInitializer(string cachePath)
		{
			this.cachePath = cachePath;
		}

		public static bool IsMarker(string file) => MarkerExtension.Equals(Path.GetExtension(file), StringComparison.OrdinalIgnoreCase);

		// The DLLs in a source plugin folder are its references and the build output of its author. The source is the plugin.
		public static bool IsShadowedBySource(string file, IEnumerable<string> sourcePluginFolders)
		{
			return ".dll".Equals(Path.GetExtension(file), StringComparison.OrdinalIgnoreCase)
				&& sourcePluginFolders.Any(folder => file.StartsWith(folder + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase));
		}

		public override IEnumerable<Command> GetCommands(string file)
		{
			if (!IsMarker(file))
				return Enumerable.Empty<Command>();

			try
			{
				SourcePlugin plugin = SourcePlugin.Read(file);
				string assemblyFile = GetAssemblyFile(plugin);
				if (assemblyFile == null)
					return Enumerable.Empty<Command>();

				PluginAssemblyResolver.Register(plugin.Folder, plugin.References.Select(Path.GetDirectoryName));
				Assembly assembly = Assembly.LoadFrom(assemblyFile);
				PluginAssemblyResolver.Register(assembly, plugin.Folder);
				return DotNetPluginLoader.CreateCommands(assembly, file);
			}
			catch (Exception e)
			{
				Command.LogDefault($"Failed to load source plugin '{file}': {e.Message}");
				return Enumerable.Empty<Command>();
			}
		}

		private string GetAssemblyFile(SourcePlugin plugin)
		{
			Assembly compilerAssembly = typeof(RoslynSourcePluginCompiler).Assembly;
			List<Reference> references = GetReferences(plugin);
			string key = ComputeKey(plugin, references, compilerAssembly).Substring(0, KeyLength);
			string assemblyName = plugin.Name + "-" + key;
			string folderHash = ComputeFolderHash(plugin);
			string cacheName = plugin.Name + "-" + folderHash + "-" + key;
			string assemblyFile = Path.Combine(cachePath, cacheName + ".dll");
			usedCacheEntries.Add(cacheName);
			if (File.Exists(assemblyFile))
			{
				Command.LogDefault($"Source plugin '{plugin.MarkerFile}' is unchanged, using '{assemblyFile}'");
				return assemblyFile;
			}
			Command.LogDefault($"Compiling source plugin '{plugin.MarkerFile}' to '{assemblyFile}'");
			string staging = Directory.CreateDirectory(Path.Combine(cachePath, Guid.NewGuid().ToString("N") + ".tmp")).FullName;
			try
			{
				var compiler = new RoslynSourcePluginCompiler();
				SourcePluginCompilationResult result = compiler.Compile(new SourcePluginCompilation
				{
					// The name carries the key, because the runtime loads only one assembly per name and tells assemblies apart by it.
					AssemblyName = assemblyName,
					LanguageVersion = plugin.LanguageVersion,
					SourceFiles = plugin.SourceFiles,
					References = references.Select(reference => reference.File).ToList(),
					OutputFile = Path.Combine(staging, cacheName + ".dll")
				});
				if (result.Errors.Count > 0)
				{
					Command.LogDefault($"Source plugin '{plugin.MarkerFile}' does not compile and is skipped:");
					foreach (string error in result.Errors)
					{
						Command.LogDefault(error);
					}
					return null;
				}
				if (result.Warnings.Count > 0)
				{
					foreach (string warning in result.Warnings)
					{
						Command.LogDefault(warning);
					}
					Command.LogDefault("Source plugin '{0}' compiled with {1} warning(s)", plugin.MarkerFile, result.Warnings.Count);
				}
				RemoveCachedVersions(plugin.Name, folderHash);
				// The DLL moves last: its presence is what marks the cache entry as complete.
				MoveIntoCache(Path.Combine(staging, cacheName + ".pdb"));
				MoveIntoCache(Path.Combine(staging, cacheName + ".dll"));
				return assemblyFile;
			}
			finally
			{
				FileUtility.SafeDirectoryDelete(staging);
			}
		}

		// Removes the cache entries of plugins that were deleted, moved or renamed. Call it once, after all scripts folders are scanned.
		public void SweepCache()
		{
			if (!Directory.Exists(cachePath))
			{
				return;
			}
			foreach (string file in Directory.EnumerateFiles(cachePath))
			{
				Match match = cacheEntry.Match(Path.GetFileName(file));
				if (match.Success && !usedCacheEntries.Contains(match.Groups["entry"].Value))
				{
					// An entry that a running ComicRack has loaded stays until a later start.
					FileUtility.SafeDelete(file);
				}
			}
		}

		private void MoveIntoCache(string file)
		{
			string target = Path.Combine(cachePath, Path.GetFileName(file));
			try
			{
				File.Move(file, target);
			}
			catch (IOException) when (File.Exists(target))
			{
				// Another ComicRack compiled the same plugin at the same time; both results are the same.
			}
		}

		// Plugins of the same name in different folders must not evict each other from the cache.
		private static string ComputeFolderHash(SourcePlugin plugin)
		{
			string folder = Path.GetFullPath(plugin.Folder).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
			return Hex(Hash(writer => writer.Write(folder.ToUpperInvariant()))).Substring(0, FolderLength);
		}

		private void RemoveCachedVersions(string name, string folderHash)
		{
			var cached = new Regex("^" + Regex.Escape(name) + "-" + folderHash + "-[0-9a-f]{" + KeyLength + @"}\.(dll|pdb)$", RegexOptions.IgnoreCase);
			foreach (string file in Directory.EnumerateFiles(cachePath).Where(file => cached.IsMatch(Path.GetFileName(file))))
			{
				// A version that a running ComicRack has loaded stays until a later start.
				FileUtility.SafeDelete(file);
			}
		}

		private static List<Reference> GetReferences(SourcePlugin plugin)
		{
			var references = new List<Reference>();
			foreach (string name in defaultFrameworkReferences.Concat(plugin.FrameworkReferences).Distinct(StringComparer.OrdinalIgnoreCase))
			{
				string file = FindFrameworkAssembly(name);
				var info = new FileInfo(file);
				references.Add(new Reference { File = file, Fingerprint = info.Length + " " + info.LastWriteTimeUtc.Ticks });
			}
			foreach (Assembly assembly in hostReferences)
			{
				references.Add(new Reference { File = assembly.Location, Fingerprint = assembly.ManifestModule.ModuleVersionId.ToString() });
			}
			foreach (string file in plugin.References)
			{
				PluginAssemblyResolver.NoteLibrary(file, AssemblyName.GetAssemblyName(file));
				references.Add(new Reference { File = file, Fingerprint = Hex(Hash(writer => writer.Write(File.ReadAllBytes(file)))) });
			}
			return references;
		}

		// A name is an assembly of the .NET Framework or an assembly that ComicRack ships.
		private static string FindFrameworkAssembly(string name)
		{
			string runtime = Path.GetDirectoryName(typeof(object).Assembly.Location);
			string file = new[] { runtime, Path.Combine(runtime, "WPF"), AppDomain.CurrentDomain.BaseDirectory }
				.Select(directory => Path.Combine(directory, name + ".dll"))
				.FirstOrDefault(File.Exists);
			return file ?? throw new FileNotFoundException($"the framework reference '{name}' does not exist");
		}

		private static string ComputeKey(SourcePlugin plugin, List<Reference> references, Assembly compilerAssembly)
		{
			return Hex(Hash(writer =>
			{
				writer.Write(File.ReadAllBytes(plugin.MarkerFile));
				foreach (string file in plugin.SourceFiles)
				{
					writer.Write(plugin.RelativePath(file));
					byte[] content = File.ReadAllBytes(file);
					writer.Write(content.Length);
					writer.Write(content);
				}
				foreach (Reference reference in references)
				{
					writer.Write(reference.File);
					writer.Write(reference.Fingerprint);
				}
				// A new build of the compiler assembly or another Roslyn version can produce another result.
				writer.Write(compilerAssembly.ManifestModule.ModuleVersionId.ToString());
				foreach (AssemblyName name in compilerAssembly.GetReferencedAssemblies())
				{
					writer.Write(name.FullName);
				}
			}));
		}

		private static byte[] Hash(Action<BinaryWriter> write)
		{
			using (SHA256 sha = SHA256.Create())
			{
				using (var stream = new CryptoStream(Stream.Null, sha, CryptoStreamMode.Write))
				using (var writer = new BinaryWriter(stream))
				{
					write(writer);
				}
				return sha.Hash;
			}
		}

		private static string Hex(byte[] bytes) => string.Concat(bytes.Select(b => b.ToString("x2")));
	}
}