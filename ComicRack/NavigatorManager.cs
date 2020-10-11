using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Threading;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Display;
using cYo.Projects.ComicRack.Plugins.Automation;

namespace cYo.Projects.ComicRack.Viewer
{
	[ComVisible(true)]
	public class NavigatorManager : IOpenBooksManager
	{
		private int currentSlot = -1;

		private readonly SmartList<ComicBookNavigator> slots = new SmartList<ComicBookNavigator>();

		private readonly IComicDisplay comicDisplay;

		private bool blockCurrentSlotChanged;

		public ComicBookNavigator CurrentBook => comicDisplay.Book;

		public int CurrentSlot
		{
			get
			{
				return currentSlot;
			}
			set
			{
				value = value.Clamp(-1, slots.Count - 1);
				if (currentSlot == value)
				{
					return;
				}
				currentSlot = value;
				try
				{
					ComicBookNavigator book = ((currentSlot < 0) ? null : slots[currentSlot]);
					if (Win7.TabbedThumbnailsEnabled && CurrentBook != null && comicDisplay.Book != null)
					{
						CurrentBook.Thumbnail = comicDisplay.CreateThumbnail();
					}
					comicDisplay.Book = book;
				}
				catch
				{
					comicDisplay.Book = null;
				}
				OnCurrentSlotChanged();
			}
		}

		public SmartList<ComicBookNavigator> Slots => slots;

		public IComicDisplay ComicDisplay => comicDisplay;

		public int OpenCount => slots.Count((ComicBookNavigator nav) => nav != null);

		public IEnumerable<string> OpenFiles => from nav in slots
			where nav != null && nav.Comic != null && nav.Comic.EditMode.IsLocalComic()
			select nav.Comic.FilePath;

		public event EventHandler OpenComicsChanged;

		public event EventHandler CurrentSlotChanged;

		public event EventHandler<BookEventArgs> BookOpened;

		public event EventHandler<BookEventArgs> BookClosing;

		public event EventHandler<BookEventArgs> BookClosed;

		public NavigatorManager(IComicDisplay comicDisplay)
		{
			this.comicDisplay = comicDisplay;
			slots.Changed += slots_Changed;
		}

		public bool IsOpen(ComicBook cb)
		{
			return slots.Any((ComicBookNavigator nav) => nav != null && nav.Comic == cb);
		}

		public void AddSlot()
		{
			Slots.Add(null);
			CurrentSlot = Slots.Count - 1;
		}

		public void MoveSlot(int slot, int newSlot)
		{
			if (slot >= 0 && slot < slots.Count && newSlot >= 0 && newSlot < slots.Count)
			{
				CurrentSlot = -1;
				slots.Move(slots[slot], newSlot);
				CurrentSlot = newSlot;
				OnOpenComicsChanged();
			}
		}

		public void MoveCurrentSlot(int newSlot)
		{
			if (CurrentSlot != -1)
			{
				MoveSlot(CurrentSlot, newSlot);
			}
		}

		public bool Open(ComicBook cb, OpenComicOptions options, int page = 0)
		{
			ComicBookNavigator comicBookNavigator = null;
			if (cb != null && cb.IsLinked)
			{
				if (cb.EditMode.IsLocalComic() && Program.Settings.AddToLibraryOnOpen)
				{
					cb = Program.BookFactory.Create(cb.FilePath, Program.Settings.AddToLibraryOnOpen ? CreateBookOption.AddToStorage : CreateBookOption.AddToTemporary);
				}
				if ((Control.ModifierKeys & Keys.Shift) != 0)
				{
					options ^= OpenComicOptions.OpenInNewSlot;
				}
				if (page == 0 && Program.Settings.OpenLastPage && cb != null)
				{
					page = cb.CurrentPage;
				}
				comicBookNavigator = Slots.Find((ComicBookNavigator nav) => nav != null && nav.Comic == cb);
				try
				{
					if (comicBookNavigator != null)
					{
						CurrentSlot = slots.IndexOf(comicBookNavigator);
						OnBookOpened(new BookEventArgs(comicBookNavigator.Comic));
						if (page != 0)
						{
							comicBookNavigator.Navigate(page, PageSeekOrigin.Absolute);
						}
						return true;
					}
				}
				catch
				{
				}
				comicBookNavigator = OpenComic(cb, page, options);
				bool flag = false;
				bool flag2 = (options & OpenComicOptions.OpenInNewSlot) == 0 || comicBookNavigator == null;
				if (flag2 && CurrentSlot >= 0 && CurrentSlot < Slots.Count && Slots[CurrentSlot] != null)
				{
					OnBookClosing(new BookEventArgs(Slots[CurrentSlot].Comic));
				}
				try
				{
					using (ItemMonitor.Lock(slots.SyncRoot))
					{
						if (flag2 && CurrentSlot >= 0)
						{
							int index = CurrentSlot;
							blockCurrentSlotChanged = true;
							try
							{
								CurrentSlot = -1;
							}
							finally
							{
								blockCurrentSlotChanged = false;
							}
							Slots[index] = comicBookNavigator;
							CurrentSlot = index;
							flag = true;
						}
					}
				}
				catch
				{
				}
				if (!flag)
				{
					if ((options & OpenComicOptions.AppendNewSlots) == 0)
					{
						CurrentSlot = -1;
						slots.Insert(0, comicBookNavigator);
						CurrentSlot = 0;
					}
					else
					{
						slots.Add(comicBookNavigator);
						CurrentSlot = slots.Count - 1;
					}
				}
			}
			if (comicBookNavigator != null)
			{
				OnBookOpened(new BookEventArgs(comicBookNavigator.Comic));
			}
			comicDisplay.DisplayOpenMessage();
			return comicBookNavigator != null;
		}

		public bool Open(ComicBook cb, bool inNewSlot, int page = 0)
		{
			return Open(cb, OpenComicOptions.None | (inNewSlot ? OpenComicOptions.OpenInNewSlot : OpenComicOptions.None), page);
		}

		public bool Open(string file, OpenComicOptions options = OpenComicOptions.None, int page = 0)
		{
			return Open(Program.BookFactory.Create(file, Program.Settings.AddToLibraryOnOpen ? CreateBookOption.AddToStorage : CreateBookOption.AddToTemporary), options, page);
		}

		public bool Open(string file, bool inNewSlot, int page = 0)
		{
			return Open(file, OpenComicOptions.None | (inNewSlot ? OpenComicOptions.OpenInNewSlot : OpenComicOptions.None), page);
		}

		public bool Open(IEnumerable<string> files, OpenComicOptions options)
		{
			bool flag = true;
			foreach (string file in files)
			{
				if (!string.IsNullOrEmpty(file))
				{
					flag &= Open(file, options | OpenComicOptions.OpenInNewSlot);
				}
			}
			return flag;
		}

		public void Close(int slot)
		{
			if (slot < 0 || slot >= Slots.Count)
			{
				return;
			}
			if (Slots[slot] != null)
			{
				OnBookClosing(new BookEventArgs(Slots[slot].Comic));
			}
			try
			{
				if (CurrentSlot == slot)
				{
					if (Slots.Count == 1)
					{
						CurrentSlot = -1;
					}
					else if (slot < Slots.Count - 1)
					{
						CurrentSlot++;
					}
					else
					{
						CurrentSlot--;
					}
				}
				Slots.RemoveAt(slot);
				if (CurrentSlot > slot)
				{
					CurrentSlot--;
				}
			}
			catch
			{
			}
		}

		public void NextSlot()
		{
			if (CurrentSlot >= 0)
			{
				int num = CurrentSlot + 1;
				if (num >= Slots.Count)
				{
					num = 0;
				}
				CurrentSlot = num;
			}
		}

		public void PreviousSlot()
		{
			if (CurrentSlot >= 0)
			{
				int num = CurrentSlot - 1;
				if (num < 0)
				{
					num = Slots.Count - 1;
				}
				CurrentSlot = num;
			}
		}

		public void Close()
		{
			Close(CurrentSlot);
		}

		public void CloseAll()
		{
			Slots.Clear();
			CurrentSlot = -1;
		}

		public void CloseAllButCurrent()
		{
			Slots.Move(CurrentSlot, 0);
			for (int num = Slots.Count - 1; num >= 1; num--)
			{
				Close(num);
			}
		}

		public void CloseAllToTheRight()
		{
			for (int num = Slots.Count - 1; num > CurrentSlot; num--)
			{
				Close(num);
			}
		}

		public string GetSlotCaption(int i)
		{
			if (i < 0)
			{
				return string.Empty;
			}
			ComicBookNavigator itemOrDefault = slots.GetItemOrDefault(i);
			if (itemOrDefault != null)
			{
				return itemOrDefault.Comic.Caption;
			}
			return TR.Default["None", "None"];
		}

		protected virtual void OnBookOpened(BookEventArgs e)
		{
			if (this.BookOpened != null)
			{
				this.BookOpened(this, e);
			}
		}

		protected virtual void OnBookClosed(BookEventArgs e)
		{
			if (this.BookClosed != null)
			{
				this.BookClosed(this, e);
			}
		}

		protected virtual void OnBookClosing(BookEventArgs e)
		{
			if (this.BookClosing != null)
			{
				this.BookClosing(this, e);
			}
		}

		protected virtual void OnOpenComicsChanged()
		{
			if (this.OpenComicsChanged != null)
			{
				this.OpenComicsChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnCurrentSlotChanged()
		{
			if (!blockCurrentSlotChanged && this.CurrentSlotChanged != null)
			{
				this.CurrentSlotChanged(this, EventArgs.Empty);
			}
		}

		private void slots_Changed(object sender, SmartListChangedEventArgs<ComicBookNavigator> e)
		{
			switch (e.Action)
			{
			case SmartListAction.Insert:
				if (e.Item != null)
				{
					e.Item.Comic.BookChanged += Comic_PropertyChanged;
				}
				break;
			case SmartListAction.Remove:
				if (e.Item != null)
				{
					e.Item.Comic.BookChanged -= Comic_PropertyChanged;
					OnBookClosed(new BookEventArgs(e.Item.Comic));
					ThreadPool.QueueUserWorkItem(delegate
					{
						e.Item.Dispose();
					});
				}
				break;
			}
		}

		private void Comic_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnOpenComicsChanged();
		}

		public static ComicBookNavigator OpenComic(ComicBook comicBook, int page, OpenComicOptions options)
		{
			if (comicBook == null)
			{
				return null;
			}
			if (!comicBook.IsLinked)
			{
				return null;
			}
			if ((options & OpenComicOptions.NoMoveToLastPage) != 0)
			{
				page = 0;
			}
			if ((options & OpenComicOptions.NoRefreshInfo) == 0)
			{
				comicBook.RefreshInfoFromFile();
			}
			ComicBookNavigator comicBookNavigator = comicBook.CreateNavigator();
			if (comicBookNavigator == null)
			{
				return null;
			}
			if ((options & OpenComicOptions.NoUpdateCurrentPage) != 0)
			{
				comicBookNavigator.UpdateCurrentPageEnabled = false;
			}
			if ((options & OpenComicOptions.NoGlobalColorAdjustment) == 0)
			{
				comicBookNavigator.BaseColorAdjustment = Program.Settings.GlobalColorAdjustment;
			}
			if ((options & OpenComicOptions.NoIncreaseOpenedCount) == 0)
			{
				comicBookNavigator.Opened += delegate
				{
					comicBook.OpenedCount++;
				};
			}
			if (comicBook.IsDynamicSource)
			{
				Program.ImagePool.RefreshLastImage(comicBook.FilePath);
			}
			comicBookNavigator.Open(async: true, page);
			return comicBookNavigator;
		}

		public bool OpenFile(string file, bool inNewSlot, int page)
		{
			return Open(file, inNewSlot, page);
		}
	}
}
