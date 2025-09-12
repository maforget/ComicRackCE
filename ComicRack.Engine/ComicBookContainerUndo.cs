using System;
using System.Collections.Generic;
using System.Linq;
using cYo.Common.Collections;
using cYo.Common.Threading;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookContainerUndo
	{
		private enum UndoItemType
		{
			ComicProperty,
			ComicDeleted,
			ComicInserted
		}

		private class UndoItem
		{
			public string Marker
			{
				get;
				set;
			}

			public UndoItemType Type
			{
				get;
				set;
			}

			public Guid BookId
			{
				get;
				set;
			}

			public string Property
			{
				get;
				set;
			}

			public object OldValue
			{
				get;
				set;
			}

			public object NewValue
			{
				get;
				set;
			}

			public ComicBook Book
			{
				get;
				set;
			}

			public bool IsMarker => !string.IsNullOrEmpty(Marker);
		}

		private const int UndoSize = 10;

		private readonly LinkedList<UndoItem> items = new LinkedList<UndoItem>();

		private LinkedListNode<UndoItem> currentItem;

		private bool inUpdate;

		private string pendingMarker;

		private ComicBookContainer container;

		public ComicBookContainer Container
		{
			get
			{
				return container;
			}
			set
			{
				if (value != container)
				{
					if (container != null)
					{
						container.BookChanged -= BookChanged;
						container.Books.Changed -= BookListChanged;
					}
					container = value;
					Clear();
					if (container != null)
					{
						container.BookChanged += BookChanged;
					}
				}
			}
		}

		public bool CanUndo => currentItem != null;

		public bool CanRedo
		{
			get
			{
				using (ItemMonitor.Lock(items))
				{
					if (currentItem == null)
					{
						return items.First != null;
					}
					return currentItem.Next != null;
				}
			}
		}

		public IEnumerable<string> UndoEntries
		{
			get
			{
				using (ItemMonitor.Lock(items))
				{
					for (LinkedListNode<UndoItem> node = currentItem; node != null; node = node.Previous)
					{
						if (node.Value.IsMarker)
						{
							yield return node.Value.Marker;
						}
					}
				}
			}
		}

		public IEnumerable<string> RedoEntries
		{
			get
			{
				using (ItemMonitor.Lock(items))
				{
					if (items.First == null)
					{
						yield break;
					}
					for (LinkedListNode<UndoItem> node = currentItem ?? items.First; node != null; node = node.Next)
					{
						if (node.Value.IsMarker)
						{
							yield return node.Value.Marker;
						}
					}
				}
			}
		}

		public string UndoLabel => UndoEntries.FirstOrDefault();

		public string RedoLabel => RedoEntries.FirstOrDefault();

		public void Clear()
		{
			using (ItemMonitor.Lock(items))
			{
				items.Clear();
				currentItem = null;
				pendingMarker = null;
			}
		}

		public void SetMarker(string name)
		{
			pendingMarker = name;
		}

		public void Undo()
		{
			using (ItemMonitor.Lock(items))
			{
				if (!CanUndo)
				{
					return;
				}
				inUpdate = true;
				try
				{
					while (currentItem != null)
					{
						UndoItem value = currentItem.Value;
						bool isMarker = value.IsMarker;
						switch (value.Type)
						{
						case UndoItemType.ComicProperty:
							container.Books[value.BookId]?.SetValue(value.Property, value.OldValue);
							break;
						case UndoItemType.ComicDeleted:
							container.Add(value.Book);
							break;
						case UndoItemType.ComicInserted:
							container.Remove(value.Book);
							break;
						}
						currentItem = currentItem.Previous;
						if (isMarker)
						{
							break;
						}
					}
				}
				finally
				{
					inUpdate = false;
				}
			}
		}

		public void Redo()
		{
			using (ItemMonitor.Lock(items))
			{
				if (!CanRedo)
				{
					return;
				}
				inUpdate = true;
				try
				{
					currentItem = ((currentItem == null) ? items.First : currentItem.Next);
					do
					{
						UndoItem value = currentItem.Value;
						switch (value.Type)
						{
						case UndoItemType.ComicProperty:
							container.Books[value.BookId]?.SetValue(value.Property, value.NewValue);
							break;
						case UndoItemType.ComicDeleted:
							container.Remove(value.Book);
							break;
						case UndoItemType.ComicInserted:
							container.Add(value.Book);
							break;
						}
						currentItem = currentItem.Next;
					}
					while (currentItem != null && !currentItem.Value.IsMarker);
					currentItem = ((currentItem == null) ? items.Last : currentItem.Previous);
				}
				finally
				{
					inUpdate = false;
				}
			}
		}

		private void BookChanged(object sender, ContainerBookChangedEventArgs e)
		{
			if (!inUpdate && e.OldValue != null && e.NewValue != null)
			{
				AddItem(new UndoItem
				{
					Type = UndoItemType.ComicProperty,
					BookId = e.Book.Id,
					Property = e.PropertyName,
					OldValue = e.OldValue,
					NewValue = e.NewValue
				});
			}
		}

		private void BookListChanged(object sender, SmartListChangedEventArgs<ComicBook> e)
		{
			if (!inUpdate)
			{
				switch (e.Action)
				{
				case SmartListAction.Insert:
					AddItem(new UndoItem
					{
						Type = UndoItemType.ComicInserted,
						BookId = e.Item.Id,
						Book = e.Item
					});
					break;
				case SmartListAction.Remove:
					AddItem(new UndoItem
					{
						Type = UndoItemType.ComicDeleted,
						BookId = e.Item.Id,
						Book = e.Item
					});
					break;
				}
			}
		}

		private void AddItem(UndoItem undoItem)
		{
			using (ItemMonitor.Lock(items))
			{
				Trim(currentItem);
				if (!string.IsNullOrEmpty(pendingMarker))
				{
					Trim(UndoSize);
					undoItem.Marker = pendingMarker;
					pendingMarker = null;
				}
				currentItem = items.AddLast(undoItem);
			}
		}

		private void Trim(LinkedListNode<UndoItem> trimNode)
		{
			if (trimNode != null)
			{
				LinkedListNode<UndoItem> linkedListNode = items.Last;
				while (linkedListNode != null && trimNode != linkedListNode)
				{
					linkedListNode = linkedListNode.Previous;
					items.RemoveLast();
				}
			}
		}

		private void Trim(int size)
		{
			int num = UndoEntries.Count();
			while (num > size)
			{
				num--;
				items.RemoveFirst();
				while (items.First != null && !items.First.Value.IsMarker)
				{
					items.RemoveFirst();
				}
			}
		}
	}
}
