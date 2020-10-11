using System.Collections.Generic;

namespace cYo.Common.Collections
{
	public class CursorList<T> : LinkedList<T>
	{
		public const int DefaultMaxSize = 50;

		private LinkedListNode<T> cursorNode;

		public int MaxSize
		{
			get;
			set;
		}

		public LinkedListNode<T> CursorNode => cursorNode;

		public T CursorValue
		{
			get
			{
				if (cursorNode != null)
				{
					return cursorNode.Value;
				}
				return default(T);
			}
		}

		public bool CanMoveCursorPrevious
		{
			get
			{
				if (cursorNode != null)
				{
					return cursorNode.Previous != null;
				}
				return false;
			}
		}

		public bool CanMoveCursorNext
		{
			get
			{
				if (cursorNode != null)
				{
					return cursorNode.Next != null;
				}
				return false;
			}
		}

		public CursorList(int maxSize)
		{
			MaxSize = maxSize;
		}

		public CursorList()
			: this(50)
		{
		}

		public LinkedListNode<T> MoveCursorStart()
		{
			return cursorNode = base.First;
		}

		public LinkedListNode<T> MoveCursorEnd()
		{
			return cursorNode = base.Last;
		}

		public LinkedListNode<T> MoveCursorPrevious()
		{
			if (CanMoveCursorPrevious)
			{
				cursorNode = cursorNode.Previous;
			}
			return cursorNode;
		}

		public LinkedListNode<T> MoveCursorNext()
		{
			if (CanMoveCursorNext)
			{
				cursorNode = cursorNode.Next;
			}
			return cursorNode;
		}

		public void AddAtCursor(LinkedListNode<T> node)
		{
			if (cursorNode != null)
			{
				if (object.Equals(cursorNode.Value, node.Value))
				{
					return;
				}
				LinkedListNode<T> linkedListNode = base.Last;
				while (linkedListNode != cursorNode)
				{
					linkedListNode = linkedListNode.Previous;
					RemoveLast();
				}
			}
			AddLast(node);
			while (base.Count > MaxSize)
			{
				RemoveFirst();
			}
			cursorNode = base.Last;
		}

		public void AddAtCursor(T value)
		{
			AddAtCursor(new LinkedListNode<T>(value));
		}
	}
}
