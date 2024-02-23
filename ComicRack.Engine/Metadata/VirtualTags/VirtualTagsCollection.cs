using cYo.Common.Collections;
using cYo.Projects.ComicRack.Engine.IO.Provider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Projects.ComicRack.Engine
{
    public class VirtualTagsCollection : Dictionary<int, IVirtualTag>
    {
        private static readonly Lazy<VirtualTagsCollection> instance = new Lazy<VirtualTagsCollection>(() => new VirtualTagsCollection(Init()));
        private static IVirtualTagSettings Settings;

        public static event EventHandler TagsRefresh;

        private VirtualTagsCollection(IEnumerable<IVirtualTag> list)
            : base(InitDictionary(list))
        {

        }

        public static VirtualTagsCollection Tags => instance.Value;

        public static void RegisterSettings(IVirtualTagSettings settings)
        {
            Settings = settings;

            if (instance.IsValueCreated)
                instance.Value.Refresh();

            //TODO: Refresh only when settings have changed
            OnTagsRefresh();
        }

        public IVirtualTag GetValue(int i)
        {
            if (Tags.TryGetValue(i, out var tag))
                return tag;

            return new VirtualTag();
        }

        private static Dictionary<int, IVirtualTag> InitDictionary(IEnumerable<IVirtualTag> list) => list.ToDictionary(v => v.ID, v => v);

        private static IEnumerable<IVirtualTag> Init()
        {
            return Settings?.VirtualTags
                .Where(x => x != null && x.IsEnabled && !string.IsNullOrEmpty(x.CaptionFormat) && !string.IsNullOrEmpty(x.Name))
                .ToList() ?? Enumerable.Empty<IVirtualTag>();
        }

        private void Refresh()
        {
            Clear();
            foreach (var v in Init())
            {
                if (!ContainsKey(v.ID))
                    Add(v.ID, v);
            }
        }

        private static void OnTagsRefresh()
        {
            TagsRefresh?.Invoke(null, EventArgs.Empty);
        }
    }
}
