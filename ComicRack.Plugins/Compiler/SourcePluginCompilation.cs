using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Plugins.Compiler
{
	internal sealed class SourcePluginCompilation
	{
		public string AssemblyName { get; set; }

		// Null means the default version of RoslynSourcePluginCompiler.
		public string LanguageVersion { get; set; }

		public IReadOnlyList<string> SourceFiles { get; set; }

		public IReadOnlyList<string> References { get; set; }

		// The DLL to write. The PDB is written next to it with the same name.
		public string OutputFile { get; set; }

		public bool Optimized { get; set; }
	}

	internal sealed class SourcePluginCompilationResult
	{
		// Diagnostics have the form "file(line,column): error CS0000: message". No errors means OutputFile is valid.
		public IReadOnlyList<string> Errors { get; set; }

		public IReadOnlyList<string> Warnings { get; set; }
	}
}