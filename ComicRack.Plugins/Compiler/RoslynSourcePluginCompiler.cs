using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace cYo.Projects.ComicRack.Plugins.Compiler
{
	// Only CSharpSourcePluginInitializer.GetAssemblyFile uses this class. The runtime loads Roslyn when the JIT compiles that method.
	internal sealed class RoslynSourcePluginCompiler
	{
		private sealed class SourceFile
		{
			public SourceFile(string filePath, SourceText text)
			{
				FilePath = filePath;
				Text = text;
			}

			public string FilePath { get; }
			public SourceText Text { get; }
		}

		private sealed class GeneratorOptionsProvider : AnalyzerConfigOptionsProvider
		{
			private readonly AnalyzerConfigOptions options;

			public GeneratorOptionsProvider(IReadOnlyDictionary<string, string> values) => options = new Options(values);

			public override AnalyzerConfigOptions GlobalOptions => options;
			public override AnalyzerConfigOptions GetOptions(SyntaxTree tree) => options;
			public override AnalyzerConfigOptions GetOptions(AdditionalText textFile) => options;

			private sealed class Options : AnalyzerConfigOptions
			{
				private readonly IReadOnlyDictionary<string, string> values;
				public Options(IReadOnlyDictionary<string, string> values) => this.values = values;
				public override bool TryGetValue(string key, out string value) => values.TryGetValue(key, out value);
			}
		}

		private const string DefaultLanguageVersion = "14.0"; // Use same version set in directory build.props, using latest without updating PolySharp is not a good idea.
		private const string PolySharpAssemblyName = "PolySharp.SourceGenerators.dll";

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
			List<SourceFile> sources = request.SourceFiles.Select(file => new SourceFile(file, ReadSource(file))).ToList();

			CSharpCompilation compilation = CreateCompilation(request, sources, parseOptions);
			compilation = RunGenerators(compilation, parseOptions, out ImmutableArray<Diagnostic> generatorDiagnostics, out List<string> generatorProblems);
			EmitResult result = Emit(compilation, request.OutputFile, sources);

			return new SourcePluginCompilationResult
			{
				Errors = Format(result, generatorDiagnostics, DiagnosticSeverity.Error),
				Warnings = Format(result, generatorDiagnostics, DiagnosticSeverity.Warning).Concat(generatorProblems).ToList()
			};
		}

		private static CSharpCompilation CreateCompilation(SourcePluginCompilation request, IEnumerable<SourceFile> sources, CSharpParseOptions parseOptions)
		{
			OptimizationLevel optimizationLevel = request.Optimized ? OptimizationLevel.Release : OptimizationLevel.Debug;
			return CSharpCompilation.Create(
				request.AssemblyName,
				sources.Select(source => CSharpSyntaxTree.ParseText(source.Text, parseOptions, source.FilePath)),
				request.References.Select(reference => MetadataReference.CreateFromFile(reference)),
				new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: optimizationLevel, deterministic: true));
		}

		// Runs PolySharp (and any other generator found next to the compiler) and returns the compilation with the generated polyfills.
		// Problems that stop a generator from running are reported as warnings, the plugin still compiles without the polyfills.
		private static CSharpCompilation RunGenerators(CSharpCompilation compilation, CSharpParseOptions parseOptions, out ImmutableArray<Diagnostic> diagnostics, out List<string> problems)
		{
			diagnostics = ImmutableArray<Diagnostic>.Empty;
			problems = new List<string>();

			// PolySharp v1.16 uses Roslyn 4.3, while we ship 5.9, so the resolver must bind it to the version we are shipping instead.
			PluginAssemblyResolver.Register(Path.GetDirectoryName(typeof(RoslynSourcePluginCompiler).Assembly.Location));
			IReadOnlyList<ISourceGenerator> generators = LoadGenerators(problems);
			if (generators.Count == 0)
				return compilation;

			GeneratorDriver driver = CSharpGeneratorDriver.Create(
				generators,
				additionalTexts: null,
				parseOptions: parseOptions,
				optionsProvider: new GeneratorOptionsProvider(new Dictionary<string, string>
				{
					["build_property.PolySharpIncludeRuntimeSupportedAttributes"] = "true", // enables polyfills for (dummy) runtime-supported attributes too.
					["build_property.PolySharpUseEmbeddedAttributeForGeneratedTypes"] = "true", // adds the [Embedded] attributes to generated types.
					//["build_property.PolySharpUsePublicAccessibilityForGeneratedTypes"] = "true", // makes all generated types public.
				}));
			driver.RunGeneratorsAndUpdateCompilation(compilation, out Compilation updated, out diagnostics);
			return (CSharpCompilation)updated;
		}

		private static IReadOnlyList<ISourceGenerator> LoadGenerators(List<string> problems)
		{
			string directory = Path.GetDirectoryName(typeof(RoslynSourcePluginCompiler).Assembly.Location);
			string path = Path.Combine(directory, PolySharpAssemblyName);
			if (!File.Exists(path))
			{
				problems.Add($"warning: PolySharp generator not found at '{path}', polyfills are disabled");
				return new ISourceGenerator[0];
			}

			try
			{
				Assembly assembly = Assembly.LoadFrom(path);
				Type[] types;
				try
				{
					types = assembly.GetTypes();
				}
				catch (ReflectionTypeLoadException ex)
				{
					types = ex.Types.Where(type => type != null).ToArray();
					foreach (string message in ex.LoaderExceptions.Select(loader => loader.Message).Distinct())
					{
						problems.Add($"warning: PolySharp load problem: {message}");
					}
				}

				return types
					.Where(type => !type.IsAbstract && typeof(IIncrementalGenerator).IsAssignableFrom(type))
					.Select(type => ((IIncrementalGenerator)Activator.CreateInstance(type)).AsSourceGenerator())
					.ToList();
			}
			catch (Exception ex)
			{
				problems.Add($"warning: PolySharp could not be loaded: {ex.Message}");
				return new ISourceGenerator[0];
			}
		}

		private static EmitResult Emit(CSharpCompilation compilation, string outputFile, IEnumerable<SourceFile> sources)
		{
			string pdbFile = Path.ChangeExtension(outputFile, ".pdb");
			using (FileStream assembly = File.Create(outputFile))
			using (FileStream pdb = File.Create(pdbFile))
			{
				// The sources are embedded in the PDB, so a debugger shows the code that was compiled even after the files changed.
				return compilation.Emit(
					assembly,
					pdb,
					options: new EmitOptions(debugInformationFormat: DebugInformationFormat.PortablePdb, pdbFilePath: Path.GetFileName(pdbFile)),
					embeddedTexts: sources.Select(source => EmbeddedText.FromSource(source.FilePath, source.Text)));
			}
		}

		private static IReadOnlyList<string> Format(EmitResult result, ImmutableArray<Diagnostic> generatorDiagnostics, DiagnosticSeverity severity)
		{
			return result.Diagnostics.Concat(generatorDiagnostics)
				.Where(diagnostic => diagnostic.Severity == severity)
				.Select(diagnostic => diagnostic.ToString())
				.ToList();
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
