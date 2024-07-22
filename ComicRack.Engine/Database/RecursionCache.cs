using cYo.Common.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cYo.Projects.ComicRack.Engine.Database
{
    public class RecursionCache: Dictionary<Guid, RecursionCacheItem>
	{
        private static Lazy<RecursionCache> instance = new Lazy<RecursionCache>(() => new RecursionCache());

        public static RecursionCache Items => instance.Value;

        private RecursionCache() : base()
        {

        }

		public RecursionCacheItem GetValue(Guid guid)
		{
			if (Items.TryGetValue(guid, out RecursionCacheItem cachedResult))
			{
				return cachedResult;
			}

			RecursionCacheItem item = RecursionCacheItem.Empty();
			this[guid] = item;
			return item;
		}

        public void RemoveReference(Guid guid)
        {
            //Remove all the cache for this list
            Items.Remove(guid);
            //And all it's reference
            Items.Where(cacheItem => cacheItem.Value.ContainsKey(guid)).SafeForEach(x => x.Value.Remove(guid));
        }
    }

    public class RecursionCacheItem : Dictionary<Guid, bool>
    {
        private RecursionCacheItem() : base()
        {

        }

        public static RecursionCacheItem Empty()
        {
            return new RecursionCacheItem();
        }
    }
}
