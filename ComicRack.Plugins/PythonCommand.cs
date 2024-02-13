using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using cYo.Common.Collections;
using cYo.Common.Runtime;
using cYo.Common.Text;
using cYo.Projects.ComicRack.Engine;
using IronPython.Hosting;
using IronPython.Modules;
using IronPython.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace cYo.Projects.ComicRack.Plugins
{
	public class PythonCommand : Command
	{
		private class PythonSettings
		{
			public bool PythonDebug
			{
				get;
				set;
			}

			public bool PythonEnableFrames
			{
				get;
				set;
			}

			public bool PythonEnableFullFrames
			{
				get;
				set;
			}
		}

		private delegate Delegate ScriptCreateHandler(ScriptScope scope, string method);

		private const string HostName = "ComicRack";

		private const string ScriptPathName = "ScriptPath";

		private Delegate script;

		private DateTime scriptModificationTime;

		private static readonly Dictionary<string, ScriptCreateHandler> hookTypes;

		private static readonly PythonSettings settings;

		private static ScriptEngine expressionEngine;

		private ScriptScope scope;

		private ScriptCreateHandler hookType;

		public static bool Optimized
		{
			get;
			set;
		}

		public static Stream Output
		{
			get;
			set;
		}

		public static bool EnableLog
		{
			get;
			set;
		}

		public string ScriptFile
		{
			get;
			set;
		}

		public string Method
		{
			get;
			set;
		}

		[XmlIgnore]
		public string LibPath
		{
			get;
			private set;
		}

		private ScriptScope Scope
		{
			get
			{
				PreCompile(handleException: false);
				return scope;
			}
		}

		private ScriptCreateHandler HookType
		{
			get
			{
				if (hookType == null)
				{
					string[] array = base.Hook.Split(',');
					foreach (string text in array)
					{
						if (hookTypes.TryGetValue(text.Trim(), out var value))
						{
							hookType = value;
						}
					}
				}
				return hookType;
			}
		}

		protected override bool IsValid
		{
			get
			{
				if (base.IsValid && !string.IsNullOrEmpty(Method))
				{
					return !string.IsNullOrEmpty(ScriptFile);
				}
				return false;
			}
		}

		private string ConfigFileName => ScriptFile + "-" + Method + ".config";

		static PythonCommand()
		{
			hookTypes = new Dictionary<string, ScriptCreateHandler>
			{
				{
                    PluginEngine.ScriptTypeLibrary,
					(ScriptScope s, string n) => s.GetVariable<Action<ComicBook[]>>(n)
				},
				{
                    PluginEngine.ScriptTypeBooks,
					(ScriptScope s, string n) => s.GetVariable<Action<ComicBook[]>>(n)
				},
				{
                    PluginEngine.ScriptTypeNewBooks,
					(ScriptScope s, string n) => s.GetVariable<Action<ComicBook[]>>(n)
				},
				{
                    PluginEngine.ScriptTypeParseComicPath,
					(ScriptScope s, string n) => s.GetVariable<Action<string, ComicNameInfo>>(n)
				},
				{
                    PluginEngine.ScriptTypeBookOpened,
					(ScriptScope s, string n) => s.GetVariable<Action<ComicBook>>(n)
				},
				{
                    PluginEngine.ScriptTypeCreateBookList,
					(ScriptScope s, string n) => s.GetVariable<Func<ComicBook[], string, string, IEnumerable<ComicBook>>>(n)
				},
				{
                    PluginEngine.ScriptTypeReaderResized,
					(ScriptScope s, string n) => s.GetVariable<Action<int, int>>(n)
				},
				{
                    PluginEngine.ScriptTypeSearch,
					(ScriptScope s, string n) => s.GetVariable<Func<string, string, int, Dictionary<string, string>>>(n)
				},
				{
                    PluginEngine.ScriptTypeConfig,
					(ScriptScope s, string n) => s.GetVariable<Action>(n)
				},
				{
                    PluginEngine.ScriptTypeStartup,
					(ScriptScope s, string n) => s.GetVariable<Action>(n)
				},
				{
                    PluginEngine.ScriptTypeShutdown,
					(ScriptScope s, string n) => s.GetVariable<Func<bool, bool>>(n)
				},
				{
                    PluginEngine.ScriptTypeComicInfoHtml,
					(ScriptScope s, string n) => s.GetVariable<Func<ComicBook[], string>>(n)
				},
				{
                    PluginEngine.ScriptTypeComicInfoUI,
					(ScriptScope s, string n) => s.GetVariable<Func<Control>>(n)
				},
				{
                    PluginEngine.ScriptTypeQuickOpenHtml,
					(ScriptScope s, string n) => s.GetVariable<Func<ComicBook[], string>>(n)
				},
				{
                    PluginEngine.ScriptTypeQuickOpenUI,
					(ScriptScope s, string n) => s.GetVariable<Func<Control>>(n)
				},
				{
                    PluginEngine.ScriptTypeDrawThumbnailOverlay,
					(ScriptScope s, string n) => s.GetVariable<Action<ComicBook, Graphics, Rectangle, int>>(n)
				}
			};
			settings = IniFile.Default.Register<PythonSettings>();
			Optimized = true;
		}

		private static ScriptEngine CreateEngine()
		{
			ScriptRuntimeSetup scriptRuntimeSetup = new ScriptRuntimeSetup
			{
				DebugMode = settings.PythonDebug
			};
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (settings.PythonEnableFrames)
			{
				dictionary["Frames"] = true;
			}
			if (settings.PythonEnableFullFrames)
			{
				dictionary["FullFrames"] = true;
			}
			scriptRuntimeSetup.LanguageSetups.Add(Python.CreateLanguageSetup(dictionary));
			ScriptRuntime scriptRuntime = new ScriptRuntime(scriptRuntimeSetup);
			ScriptEngine engineByTypeName = scriptRuntime.GetEngineByTypeName(typeof(PythonContext).AssemblyQualifiedName);
			engineByTypeName.Runtime.LoadAssembly(typeof(ArgumentNullException).Assembly);
			engineByTypeName.Runtime.LoadAssembly(typeof(ArrayModule).Assembly);
			engineByTypeName.Runtime.LoadAssembly(typeof(ComicBook).Assembly);
			if (Output != null)
			{
				scriptRuntime.IO.SetErrorOutput(Output, Encoding.Default);
				scriptRuntime.IO.SetOutput(Output, Encoding.Default);
			}
			return engineByTypeName;
		}

		public static T CompileExpression<T>(string source, params string[] parameters) where T : class
		{
			source = "def f(" + parameters.ToListString(",") + "):\n\treturn " + source.Trim();
			try
			{
				ScriptEngine scriptEngine = expressionEngine ?? (expressionEngine = CreateEngine());
				ScriptSource scriptSource = scriptEngine.CreateScriptSourceFromString(source);
				ScriptScope scriptScope = scriptEngine.CreateScope();
				scriptSource.Execute(scriptScope);
				T value;
				return scriptScope.TryGetVariable("f", out value) ? value : null;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static void LogDefault(string text, params object[] o)
		{
			if (Output != null && EnableLog)
			{
				using (StreamWriter streamWriter = new StreamWriter(Output))
				{
					streamWriter.WriteLine(text, o);
				}
			}
		}

		protected override void Log(string text, params object[] o)
		{
			base.Log(text, o);
			LogDefault(text, o);
		}

		protected override void OnInitialize(IPluginEnvironment env, string path)
		{
			base.OnInitialize(env, path);
			try
			{
				LibPath = path;
				Log("\nInitializing script '{0}' from '{1}'", Method, ScriptFile);
				ScriptFile = Command.GetFile(path, ScriptFile);
				if (!File.Exists(ScriptFile))
				{
					throw new FileNotFoundException("Script file not found");
				}
			}
			catch (Exception e)
			{
				HandleException(e);
				throw;
			}
		}

		protected override object OnInvoke(object[] data)
		{
			try
			{
				Log("Calling '{0}'...", Method);
				if (!Optimized)
				{
					CheckScript();
				}
				if ((object)script == null && HookType != null)
				{
					script = HookType(Scope, Method);
				}
				if ((object)script != null)
				{
					return script.DynamicInvoke(data);
				}
			}
			catch (Exception e)
			{
				HandleException(e);
				throw;
			}
			return null;
		}

		protected override void MakeDefaults()
		{
			if (string.IsNullOrEmpty(base.Key))
			{
				base.Key = Method;
			}
			if (string.IsNullOrEmpty(base.Name))
			{
				base.Name = Method;
			}
		}

		public override string LoadConfig()
		{
			try
			{
				return File.ReadAllText(ConfigFileName);
			}
			catch
			{
				return string.Empty;
			}
		}

		public override bool SaveConfig(string config)
		{
			try
			{
				File.WriteAllText(ConfigFileName, config);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public override void OnPreCompile(bool handleException)
		{
			try
			{
				if (scope == null)
				{
					Log("Compilation of '{0}'", ScriptFile);
					ScriptEngine scriptEngine = CreateEngine();
					if (!string.IsNullOrEmpty(LibPath) || !base.Environment.LibraryPaths.IsEmpty())
					{
						scriptEngine.SetSearchPaths(scriptEngine.GetSearchPaths().Concat(base.Environment.LibraryPaths).AddLast(LibPath)
							.Distinct(StringComparer.OrdinalIgnoreCase)
							.ToArray());
					}
					ScriptSource scriptSource = scriptEngine.CreateScriptSourceFromFile(ScriptFile);
					scriptModificationTime = File.GetLastWriteTimeUtc(ScriptFile);
					scope = scriptEngine.CreateScope();
					scope.SetVariable(HostName, base.Environment);
					scope.SetVariable(ScriptPathName, Path.GetDirectoryName(ScriptFile));
					scriptSource.Execute(scope);
					script = null;
				}
			}
			catch (Exception e)
			{
				if (handleException)
				{
					HandleException(e);
					return;
				}
				throw;
			}
		}

		private void HandleException(Exception e)
		{
			if (e.InnerException != null)
			{
				e = e.InnerException;
			}
			SyntaxErrorException ex = e as SyntaxErrorException;
			if (ex == null)
			{
				Log(e.Message);
				return;
			}
			Log("Syntax error at [{0}, {1}]: {2}", ex.Line, ex.Column, ex.Message);
			Log("\tin file '{0}'", ex.SourcePath);
		}

		private void CheckScript()
		{
			DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(ScriptFile);
			if (lastWriteTimeUtc != scriptModificationTime)
			{
				scope = null;
				script = null;
			}
		}
	}
}
