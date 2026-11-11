using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Plugins.Compiler
{
	internal sealed class SourcePluginCompilation
	{
		public string AssemblyName { get; set; }

		// Null means the newest version the compiler knows.
		public string LanguageVersion { get; set; }

		public IReadOnlyList<string> SourceFiles { get; set; }

		public IReadOnlyList<string> References { get; set; }

		// The DLL to write. The PDB is written next to it with the same name.
		public string OutputFile { get; set; }
	}

	internal sealed class SourcePluginCompilationResult
	{
		public IReadOnlyList<string> Errors { get; set; }

		public IReadOnlyList<string> Warnings { get; set; }
	}

	// Implemented in ComicRack.Plugins.Compiler, the only assembly that references Roslyn.
	internal interface ISourcePluginCompiler
	{
		// Diagnostics have the form "file(line,column): error CS0000: message". No errors means OutputFile is valid.
		SourcePluginCompilationResult Compile(SourcePluginCompilation compilation);
	}
}