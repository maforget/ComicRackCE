using System;
using System.Xml.Serialization;
using cYo.Common.Collections;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	public class StacksConfig
	{
		[Serializable]
		public class StackConfigItem
		{
			private Guid topId = Guid.Empty;

			public string Stack
			{
				get;
				set;
			}

			public Guid TopId
			{
				get
				{
					return topId;
				}
				set
				{
					topId = value;
				}
			}

			public string ThumbnailKey
			{
				get;
				set;
			}

			public ItemViewConfig Config
			{
				get;
				set;
			}

			[XmlIgnore]
			public bool TopIdSpecified => topId != Guid.Empty;

			public StackConfigItem()
			{
			}

			public StackConfigItem(string stack, Guid id, ItemViewConfig config)
			{
				topId = id;
				Stack = stack;
				Config = config;
			}

			public StackConfigItem(string stack, string thumbnailKey, ItemViewConfig config)
			{
				ThumbnailKey = thumbnailKey;
				Stack = stack;
				Config = config;
			}
		}

		[Serializable]
		public class StackConfigItemCollection : SmartList<StackConfigItem>
		{
		}

		private const int MaxConfigCount = 1024;

		private readonly StackConfigItemCollection configs = new StackConfigItemCollection();

		public StackConfigItemCollection Configs => configs;

		public StackConfigItem FindItem(string stack)
		{
			return configs.Find((StackConfigItem ti) => ti.Stack == stack);
		}

		public bool IsTop(string stack, ComicBook cb)
		{
			StackConfigItem stackConfigItem = FindItem(stack);
			if (stackConfigItem != null)
			{
				return stackConfigItem.TopId == cb.Id;
			}
			return false;
		}

		public void SetStackTop(string stack, ComicBook cb)
		{
			StackConfigItem stackConfigItem = FindItem(stack);
			if (stackConfigItem == null)
			{
				AddConfig(new StackConfigItem(stack, cb.Id, null));
				return;
			}
			stackConfigItem.ThumbnailKey = null;
			stackConfigItem.TopId = cb.Id;
		}

		public void ResetStackTop(string stack)
		{
			StackConfigItem stackConfigItem = FindItem(stack);
			if (stackConfigItem != null)
			{
				stackConfigItem.TopId = Guid.Empty;
			}
		}

		public void SetStackThumbnailKey(string stack, string key)
		{
			StackConfigItem stackConfigItem = FindItem(stack);
			if (stackConfigItem == null)
			{
				AddConfig(new StackConfigItem(stack, key, null));
			}
			else
			{
				stackConfigItem.ThumbnailKey = key;
			}
		}

		public Guid GetStackTopId(string stack)
		{
			return FindItem(stack)?.TopId ?? Guid.Empty;
		}

		public string GetStackCustomThumbnail(string stack)
		{
			return FindItem(stack)?.ThumbnailKey;
		}

		public ItemViewConfig GetStackViewConfig(string stack)
		{
			return FindItem(stack)?.Config;
		}

		public void SetStackViewConfig(string stack, ItemViewConfig config)
		{
			StackConfigItem stackConfigItem = FindItem(stack);
			if (stackConfigItem == null)
			{
				AddConfig(new StackConfigItem(stack, Guid.Empty, config));
			}
			else
			{
				stackConfigItem.Config = config;
			}
		}

		private void AddConfig(StackConfigItem sci)
		{
			configs.Insert(0, sci);
			configs.Trim(MaxConfigCount);
		}
	}
}
