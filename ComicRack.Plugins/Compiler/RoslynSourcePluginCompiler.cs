using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;

namespace cYo.Projects.ComicRack.Plugins.Compiler
{
	// ComicRack.Plugins creates this class by name the first time a source plugin has to be compiled.
	// Nothing else refers to this assembly, so Roslyn is not loaded when no plugin needs compiling.
	internal sealed class RoslynSourcePluginCompiler : ISourcePluginCompiler
	{
		private const string DefaultLanguageVersion = "14.0"; // Use same version set in directory build.props, using latest without updating PolySharp is not a good idea.

		public SourcePluginCompilationResult Compile(SourcePluginCompilation request)
		{
			if (!LanguageVersionFacts.TryParse(request.LanguageVersion ?? DefaultLanguageVersion, out LanguageVersion languageVersion))
			{
				return new SourcePluginCompilationResult
				{
					Errors = new[] { $"error: LangVersion '{request.LanguageVersion}' is not a C# language version" },
					Warnings = new string[0]
				};
			}

			var parseOptions = new CSharpParseOptions(languageVersion);
			var sources = request.SourceFiles.Select(file => new { File = file, Text = ReadSource(file) }).ToList();
			CSharpCompilation compilation = CSharpCompilation.Create(
				request.AssemblyName,
				sources.Select(source => CSharpSyntaxTree.ParseText(source.Text, parseOptions, source.File)),
				request.References.Select(reference => MetadataReference.CreateFromFile(reference)),
				new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Release, deterministic: true));
			string pdbFile = Path.ChangeExtension(request.OutputFile, ".pdb");
			EmitResult result;
			using (FileStream assembly = File.Create(request.OutputFile))
			using (FileStream pdb = File.Create(pdbFile))
			{
				// The sources are embedded in the PDB, so a debugger shows the code that was compiled even after the files changed.
				result = compilation.Emit(
					assembly,
					pdb,
					options: new EmitOptions(debugInformationFormat: DebugInformationFormat.PortablePdb, pdbFilePath: Path.GetFileName(pdbFile)),
					embeddedTexts: sources.Select(source => EmbeddedText.FromSource(source.File, source.Text)));
			}
			return new SourcePluginCompilationResult
			{
				Errors = Format(result, DiagnosticSeverity.Error),
				Warnings = Format(result, DiagnosticSeverity.Warning)
			};
		}

		private static IReadOnlyList<string> Format(EmitResult result, DiagnosticSeverity severity)
		{
			return result.Diagnostics.Where(diagnostic => diagnostic.Severity == severity).Select(diagnostic => diagnostic.ToString()).ToList();
		}

		private static SourceText ReadSource(string file)
		{
			using (FileStream stream = File.OpenRead(file))
			{
				return SourceText.From(stream, canBeEmbedded: true);
			}
		}
	}
}
