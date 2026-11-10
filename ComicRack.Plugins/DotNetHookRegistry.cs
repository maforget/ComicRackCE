using System;
using System.Collections.Generic;
using System.Linq;

namespace cYo.Projects.ComicRack.Plugins
{
	internal static class DotNetHookRegistry
	{
		private static readonly Dictionary<string, IDotNetHook> hookTypes = new()
		{
			[PluginEngine.ScriptTypeCreateBookList] = new DotNetHook<ICreateBookList>(PluginEngine.ScriptTypeCreateBookList),
			[PluginEngine.ScriptTypeParseComicPath] = new DotNetHook<IParseComicPath>(PluginEngine.ScriptTypeParseComicPath),
			[PluginEngine.ScriptTypeLibrary] = new DotNetHook<ILibrary>(PluginEngine.ScriptTypeLibrary),
			[PluginEngine.ScriptTypeEditor] = new DotNetHook<IEditor>(PluginEngine.ScriptTypeEditor),
			[PluginEngine.ScriptTypeBooks] = new DotNetHook<IBooks>(PluginEngine.ScriptTypeBooks),
			[PluginEngine.ScriptTypeNewBooks] = new DotNetHook<INewBooks>(PluginEngine.ScriptTypeNewBooks),
			[PluginEngine.ScriptTypeBookOpened] = new DotNetHook<IBookOpened>(PluginEngine.ScriptTypeBookOpened),
			[PluginEngine.ScriptTypeReaderResized] = new DotNetHook<IReaderResized>(PluginEngine.ScriptTypeReaderResized),
			[PluginEngine.ScriptTypeSearch] = new DotNetHook<ISearch>(PluginEngine.ScriptTypeSearch),
			[PluginEngine.ScriptTypeConfig] = new DotNetHook<IConfig>(PluginEngine.ScriptTypeConfig),
			[PluginEngine.ScriptTypeStartup] = new DotNetHook<IStartup>(PluginEngine.ScriptTypeStartup),
			[PluginEngine.ScriptTypeShutdown] = new DotNetHook<IShutdown>(PluginEngine.ScriptTypeShutdown),
			[PluginEngine.ScriptTypeComicInfoHtml] = new DotNetHook<IComicInfoHtml>(PluginEngine.ScriptTypeComicInfoHtml),
			[PluginEngine.ScriptTypeComicInfoUI] = new DotNetHook<IComicInfoUI>(PluginEngine.ScriptTypeComicInfoUI),
			[PluginEngine.ScriptTypeQuickOpenHtml] = new DotNetHook<IQuickOpenHtml>(PluginEngine.ScriptTypeQuickOpenHtml),
			[PluginEngine.ScriptTypeQuickOpenUI] = new DotNetHook<IQuickOpenUI>(PluginEngine.ScriptTypeQuickOpenUI),
			[PluginEngine.ScriptTypeDrawThumbnailOverlay] = new DotNetHook<IDrawThumbnailOverlay>(PluginEngine.ScriptTypeDrawThumbnailOverlay)
		};

		public static bool TryGetHook(string hook, out IDotNetHook dotNetHook) => hookTypes.TryGetValue(hook, out dotNetHook);

		public static IDotNetHook GetHookFromType(Type interfaceType) => hookTypes.Values.FirstOrDefault(hook => hook.HookType == interfaceType);
		public static IEnumerable<IDotNetHook> GetHooks(DotNetPlugin plugin) => hookTypes.Values.Where(hook => hook.HookType.IsInstanceOfType(plugin));
	}

	internal interface IDotNetHook
	{
		string Hook { get; }
		Type HookType { get; }
	}

	internal sealed class DotNetHook<T> : IDotNetHook where T : IDotNetCommand
	{
		public string Hook { get; init; }
		public Type HookType { get; init; }

		public DotNetHook(string hook)
		{
			Hook = hook;
			HookType = typeof(T);
		}
	}
}