using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using cYo.Common;
using cYo.Common.Collections;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	public class ComicListItemFolder : ShareableComicListItem, IDeserializationCallback, ICloneable
	{
		private readonly ComicListItemCollection items = new ComicListItemCollection();

		[XmlArrayItem("Item")]
		public ComicListItemCollection Items => items;

		[XmlAttribute]
		[DefaultValue(false)]
		public bool Collapsed
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(false)]
		public bool Temporary
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(ComicFolderCombineMode.Or)]
		public ComicFolderCombineMode CombineMode
		{
			get;
			set;
		}

		[XmlIgnore]
		public override ComicLibrary Library
		{
			get
			{
				return base.Library;
			}
			set
			{
				base.Library = value;
				Items.ForEach(delegate(ComicListItem cli)
				{
					cli.Library = value;
				});
			}
		}

		public override string ImageKey
		{
			get
			{
				if (!Temporary)
				{
					return "Folder";
				}
				return "TempFolder";
			}
		}

		public ComicListItemFolder()
		{
			items.Changed += items_Changed;
		}

		public ComicListItemFolder(string name)
			: this()
		{
			base.Name = name;
		}

		public ComicListItemFolder(ComicListItemFolder item)
			: this(item.Name)
		{
			base.Description = item.Description;
			base.Display = item.Display;
			CombineMode = item.CombineMode;
			foreach (ComicListItem item2 in item.Items)
			{
				if (item2 is ICloneable)
				{
					Items.Add(((ICloneable)item2).Clone<ComicListItem>());
				}
			}
		}

		public override bool IsUpdateNeeded(string propertyHint)
		{
			return false;
		}

		protected override IEnumerable<ComicBook> OnCacheMatch(IEnumerable<ComicBook> cbl)
		{
			switch (CombineMode)
			{
			case ComicFolderCombineMode.And:
				foreach (ComicBook item in cbl.Where((ComicBook cb) => Items.All((ComicListItem list) => list.GetCache().Contains(cb))))
				{
					yield return item;
				}
				yield break;
			case ComicFolderCombineMode.Empty:
				yield break;
			}
			foreach (ComicBook item2 in cbl.Where((ComicBook cb) => Items.Any((ComicListItem list) => list.GetCache().Contains(cb))))
			{
				yield return item2;
			}
		}

		public override bool Filter(string filter)
		{
			return Items.Any((ComicListItem cli) => cli.Filter(filter));
		}

		private void items_Changed(object sender, SmartListChangedEventArgs<ComicListItem> e)
		{
			ResetCache();
			switch (e.Action)
			{
			case SmartListAction.Insert:
				e.Item.Changed += Item_Changed;
				e.Item.Parent = this;
				e.Item.Library = Library;
				OnChanged(ComicListItemChange.Added, e.Item);
				break;
			case SmartListAction.Remove:
				e.Item.Changed -= Item_Changed;
				e.Item.Parent = null;
				e.Item.Library = null;
				OnChanged(ComicListItemChange.Removed, e.Item);
				break;
			}
		}

		private void Item_Changed(object sender, ComicListItemChangedEventArgs e)
		{
			ResetCache();
			OnChanged(e);
		}

		protected override IEnumerable<ComicBook> OnGetBooks()
		{
			IEnumerable<ComicBook> enumerable = null;
			switch (CombineMode)
			{
			default:
				foreach (ComicListItem item in Items)
				{
					enumerable = ((enumerable == null) ? item.GetBooks() : enumerable.Union(item.GetBooks(), ComicBook.GuidEquality));
					if (Library != null && enumerable.Count() == Library.BookCount)
					{
						break;
					}
				}
				break;
			case ComicFolderCombineMode.And:
				foreach (ComicListItem item2 in Items)
				{
					enumerable = ((enumerable == null) ? item2.GetBooks() : enumerable.Intersect(item2.GetBooks(), ComicBook.GuidEquality));
					if (enumerable.IsEmpty())
					{
						break;
					}
				}
				break;
			case ComicFolderCombineMode.Empty:
				break;
			}
			return enumerable ?? Enumerable.Empty<ComicBook>();
		}

		void IDeserializationCallback.OnDeserialization(object sender)
		{
			items.Changed += items_Changed;
		}

		public override object Clone()
		{
			return new ComicListItemFolder(this);
		}
	}
}
