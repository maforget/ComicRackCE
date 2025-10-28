using cYo.Common.Collections;
using cYo.Common.Localize;
using cYo.Common.Text;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme;
using cYo.Common.Windows.Forms.Theme.Resources;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Viewer.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static IronPython.Modules._ast;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class SmartListQueryDialog : FormEx, ISmartListDialog
	{
		private class UndoItem
		{
			public string Text
			{
				get;
				set;
			}

			public int SelectionStart
			{
				get;
				set;
			}

			public int SelectionLength
			{
				get;
				set;
			}
		}

		private ComicSmartListItem smartComicList;

		private CursorList<UndoItem> undoList = new CursorList<UndoItem>();

		private string coloredText;

		public ComicLibrary Library
		{
			get;
			set;
		}

		public Guid EditId
		{
			get;
			set;
		}

		public ComicSmartListItem SmartComicList
		{
			get
			{
				ComicSmartListItem comicSmartListItem = null;
				try
				{
					comicSmartListItem = new ComicSmartListItem(string.Empty, rtfQuery.Text, Library);
				}
				catch (Exception)
				{
				}
				if (comicSmartListItem == null)
				{
					return smartComicList;
				}
				string name = comicSmartListItem.Name;
				comicSmartListItem.Id = smartComicList.Id;
				comicSmartListItem.CopyExtraValues(smartComicList);
				if (!string.IsNullOrEmpty(name))
				{
					comicSmartListItem.Name = name;
				}
				return comicSmartListItem;
			}
			set
			{
				smartComicList = value;
				value.Library = Library;
				rtfQuery.Text = smartComicList.ToString();
				undoList = new CursorList<UndoItem>();
				TextHasChanged();
				Colorize(all: true, forced: true);
			}
		}

		public bool EnableNavigation
		{
			get
			{
				return btPrev.Visible;
			}
			set
			{
				Button button = btPrev;
				bool visible = (btNext.Visible = value);
				button.Visible = visible;
				if (value)
				{
					btDesigner.Left = btNext.Right + (btNext.Left - btPrev.Right);
				}
				else
				{
					btDesigner.Left = btPrev.Left;
				}
			}
		}

		public bool PreviousEnabled
		{
			get
			{
				return btPrev.Enabled;
			}
			set
			{
				btPrev.Enabled = value;
			}
		}

		public bool NextEnabled
		{
			get
			{
				return btNext.Enabled;
			}
			set
			{
				btNext.Enabled = value;
			}
		}

		public event EventHandler Apply;

		public event EventHandler Next;

		public event EventHandler Previous;

		public static class queryFont
		{
            public static readonly Font Default = new("Courier New", 10.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            public static readonly Font Language = new("Courier New", 10.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            public static readonly Font Exception = new("Courier New", 10.25F, FontStyle.Underline, GraphicsUnit.Point, ((byte)(0)));
        }

        public SmartListQueryDialog()
		{
			InitializeComponent();
            LocalizeUtility.UpdateRightToLeft(this);
			this.RestorePosition();
			LocalizeUtility.Localize(this, typeof(SmartListDialog).Name, components);
			LocalizeUtility.Localize(TR.Load("TextBoxContextMenu"), cmEdit);
			ContextMenuBuilder contextMenuBuilder = new ContextMenuBuilder();
			foreach (IComicBookValueMatcher availableMatcher in ComicBookValueMatcher.GetAvailableMatchers())
			{
				contextMenuBuilder.Add(availableMatcher.Description, topLevel: false, chk: false, OnInsertQuery, availableMatcher, DateTime.MinValue);
			}
			miInsertMatch.DropDownItems.AddRange(contextMenuBuilder.Create(20));
			contextMenuBuilder = new ContextMenuBuilder();
			foreach (string item in ComicBookMatcher.ComicProperties.Concat(ComicBookMatcher.SeriesStatsProperties))
			{
				contextMenuBuilder.Add(item, topLevel: false, chk: false, OnInsertField, item, DateTime.MinValue);
			}
			miInsertValue.DropDownItems.AddRange(contextMenuBuilder.Create(20));
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			rtfQuery.AutoWordSelection = false;
			ThemeExtensions.InvokeAction(() => Colorize(all: true, forced: true));
        }

		private void OnInsertQuery(object sender, EventArgs e)
		{
			IComicBookValueMatcher comicBookValueMatcher = ((ToolStripMenuItem)sender).Tag as IComicBookValueMatcher;
			string description = comicBookValueMatcher is ComicBookVirtualTagMatcher virtualTagMatcher
				? virtualTagMatcher.VirtualDescription
				: comicBookValueMatcher.Description;
			rtfQuery.SelectedText = "[" + description + "]";
		}

		private void OnInsertField(object sender, EventArgs e)
		{
			ToolStripMenuItem toolStripMenuItem = sender as ToolStripMenuItem;
			rtfQuery.SelectedText = "{" + (string)toolStripMenuItem.Tag + "}";
		}

		private void btApply_Click(object sender, EventArgs e)
		{
			if (this.Apply != null)
			{
				this.Apply(this, EventArgs.Empty);
			}
		}

		private void btOK_Click(object sender, EventArgs e)
		{
			if (this.Apply != null)
			{
				this.Apply(this, EventArgs.Empty);
			}
		}

		private void btPrev_Click(object sender, EventArgs e)
		{
			if (this.Apply != null)
			{
				this.Apply(this, EventArgs.Empty);
			}
			if (this.Previous != null)
			{
				this.Previous(this, EventArgs.Empty);
			}
		}

		private void btNext_Click(object sender, EventArgs e)
		{
			if (this.Apply != null)
			{
				this.Apply(this, EventArgs.Empty);
			}
			if (this.Next != null)
			{
				this.Next(this, EventArgs.Empty);
			}
		}

		private void rtfQuery_SelectionChanged(object sender, EventArgs e)
		{
			UpdatePositionDisplay();
		}

		private void rtfQuery_TextChanged(object sender, EventArgs e)
		{
			if (rtfQuery.Text.Contains("\t"))
			{
				using (rtfQuery.SuspendPainting())
				{
					rtfQuery.Text = rtfQuery.Text.Replace("\t", "    ");
				}
			}
			TextHasChanged();
			Colorize(all: false);
		}

		private void colorizeTimer_Tick(object sender, EventArgs e)
		{
			colorizeTimer.Stop();
			Colorize(all: true);
		}

		private void undoTimer_Tick(object sender, EventArgs e)
		{
			undoTimer.Stop();
			AddUndo();
		}

		private void rtfQuery_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
			case Keys.Return:
			{
				e.Handled = true;
				int lineFromCharIndex = rtfQuery.GetLineFromCharIndex(rtfQuery.SelectionStart);
				int val = rtfQuery.Lines[lineFromCharIndex].FindIndex((char c) => !char.IsWhiteSpace(c));
				InsertText("\n" + new string(' ', Math.Max(0, val)));
				break;
			}
			case Keys.Space:
			{
				if (!e.Modifiers.HasFlag(Keys.Control))
				{
					break;
				}
				e.Handled = true;
				Tokenizer.Token token = GetCurrentToken(rtfQuery.SelectionStart);
				if (token == null || !token.Text.StartsWith("["))
				{
					break;
				}
				string t = token.Text.Substring(0, rtfQuery.SelectionStart - token.Index);
				t = token.Text.Trim('[', ']', ' ', ',', '"', ';');
				ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
				ContextMenuBuilder contextMenuBuilder = new ContextMenuBuilder();
				foreach (IComicBookValueMatcher item in from m in ComicBookValueMatcher.GetAvailableMatchers()
					where m.DescriptionNeutral.StartsWith(t, StringComparison.OrdinalIgnoreCase)
					select m)
				{
					IComicBookValueMatcher i = item;
					contextMenuBuilder.Add(item.Description, topLevel: false, chk: false, delegate
					{
						rtfQuery.Select(token.Index, token.Length);
						rtfQuery.SelectedText = "[" + i.DescriptionNeutral + "]";
					}, item, DateTime.MinValue);
				}
				if (contextMenuBuilder.Count != 0)
				{
					contextMenuStrip.Items.AddRange(contextMenuBuilder.Create(20));
					Point positionFromCharIndex = rtfQuery.GetPositionFromCharIndex(rtfQuery.SelectionStart);
					positionFromCharIndex.Y += 20;
					contextMenuStrip.Show(this, positionFromCharIndex);
				}
				break;
			}
			case Keys.Z:
				if (e.Modifiers.HasFlag(Keys.Control))
				{
					e.Handled = true;
					Undo();
				}
				break;
			case Keys.Y:
				if (e.Modifiers.HasFlag(Keys.Control))
				{
					e.Handled = true;
					Redo();
				}
				break;
			case Keys.Tab:
				e.Handled = true;
				InsertText(new string(' ', 4));
				break;
			}
		}

		private void rtfQuery_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\t')
			{
				e.Handled = true;
			}
		}

		private void rtfQuery_DoubleClick(object sender, EventArgs e)
		{
			Point pt = rtfQuery.PointToClient(Cursor.Position);
			Tokenizer.Token currentToken = GetCurrentToken(rtfQuery.GetCharIndexFromPosition(pt));
			if (currentToken != null && !currentToken.Text.StartsWith("\""))
			{
				rtfQuery.Select(currentToken.Index, currentToken.Length);
			}
		}

		private void cmEdit_Opening(object sender, CancelEventArgs e)
		{
			miUndo.Enabled = CanUndo();
			miRedo.Enabled = CanRedo();
			ToolStripMenuItem toolStripMenuItem = miCopy;
			bool enabled = (miCut.Enabled = rtfQuery.SelectionLength > 0);
			toolStripMenuItem.Enabled = enabled;
			miPaste.Enabled = Clipboard.ContainsText();
		}

		private void miUndo_Click(object sender, EventArgs e)
		{
			Undo();
		}

		private void miRedo_Click(object sender, EventArgs e)
		{
			Redo();
		}

		private void miCut_Click(object sender, EventArgs e)
		{
			rtfQuery.Cut();
		}

		private void miCopy_Click(object sender, EventArgs e)
		{
			rtfQuery.Copy();
		}

		private void miPaste_Click(object sender, EventArgs e)
		{
			rtfQuery.Paste();
		}

		private void miSelectAll_Click(object sender, EventArgs e)
		{
			rtfQuery.SelectAll();
		}

		private void miQuickFormat_Click(object sender, EventArgs e)
		{
			try
			{
				ComicSmartListItem comicSmartListItem = new ComicSmartListItem(string.Empty, rtfQuery.Text, Library);
				rtfQuery.Text = comicSmartListItem.ToString();
				Colorize(all: true, forced: true);
			}
			catch (Exception)
			{
			}
		}

		private void AddUndo()
		{
			AddUndo(rtfQuery.Rtf, rtfQuery.SelectionStart, rtfQuery.SelectionLength);
		}

		private void AddUndo(string text, int selectionStart, int selectionLength)
		{
			if (undoList.CursorValue == null || undoList.CursorValue.Text != text || undoList.CursorValue.SelectionStart != selectionStart || undoList.CursorValue.SelectionLength != selectionLength)
			{
				undoList.AddAtCursor(new UndoItem
				{
					Text = text,
					SelectionStart = selectionStart,
					SelectionLength = selectionLength
				});
			}
		}

		private bool CanUndo()
		{
			return undoList.CanMoveCursorPrevious;
		}

		private void Undo()
		{
			if (!CanUndo())
			{
				return;
			}
			undoList.MoveCursorPrevious();
			UndoItem cursorValue = undoList.CursorValue;
			if (cursorValue.Text != null)
			{
				using (rtfQuery.SuspendPainting())
				{
					rtfQuery.Rtf = cursorValue.Text;
				}
			}
			rtfQuery.SelectionStart = cursorValue.SelectionStart;
			rtfQuery.SelectionLength = cursorValue.SelectionLength;
			coloredText = rtfQuery.Text;
		}

		private bool CanRedo()
		{
			return undoList.CanMoveCursorNext;
		}

		private void Redo()
		{
			if (!CanRedo())
			{
				return;
			}
			undoList.MoveCursorNext();
			UndoItem cursorValue = undoList.CursorValue;
			if (cursorValue.Text != null)
			{
				using (rtfQuery.SuspendPainting())
				{
					rtfQuery.Rtf = cursorValue.Text;
				}
			}
			rtfQuery.SelectionStart = cursorValue.SelectionStart;
			rtfQuery.SelectionLength = cursorValue.SelectionLength;
			coloredText = rtfQuery.Text;
		}

		private Tokenizer.Token GetCurrentToken(int n)
		{
			return ComicSmartListItem.TokenizeQuery(rtfQuery.Text).GetAll().FirstOrDefault((Tokenizer.Token t) => n >= t.Index && n <= t.Index + t.Length);
		}

		private void InsertText(string text)
		{
			using (rtfQuery.SuspendPainting())
			{
				rtfQuery.SelectedText = text;
			}
			rtfQuery.SelectionStart += text.Length;
			rtfQuery.SelectionLength = 0;
		}

		private void TextHasChanged()
		{
			UpdatePositionDisplay();
			colorizeTimer.Stop();
			colorizeTimer.Start();
			undoTimer.Stop();
			undoTimer.Start();
		}

		private void UpdatePositionDisplay()
		{
			int selectionStart = rtfQuery.SelectionStart;
			StringUtility.ConvertIndexToLineAndColumn(rtfQuery.Text, selectionStart, out var line, out var column);
			labelPosition.Text = $"{line}:{column}";
			undoTimer.Stop();
			undoTimer.Start();
		}

		public void Colorize(bool all, bool forced = false)
		{
			if (!forced && rtfQuery.Text == coloredText)
			{
				return;
			}
			if (all)
			{
				coloredText = rtfQuery.Text;
			}
			Tokenizer.ParseException ex = null;
			try
			{
				labelStatus.Text = "OK";
				new ComicSmartListItem(string.Empty, rtfQuery.Text, Library);
				Button button = btDesigner;
				Button button2 = btOK;
				bool flag2 = (btApply.Enabled = true);
				bool enabled = (button2.Enabled = flag2);
				button.Enabled = enabled;
			}
			catch (Exception ex2)
			{
				ex = ex2 as Tokenizer.ParseException;
				labelStatus.Text = ex2.Message;
				Button button3 = btDesigner;
				Button button4 = btOK;
				bool flag2 = (btApply.Enabled = false);
				bool enabled = (button4.Enabled = flag2);
				button3.Enabled = enabled;
			}
			using (rtfQuery.SuspendPainting())
			{
				if (all)
				{
					rtfQuery.SelectAll();
					rtfQuery.SelectionBackColor = ThemeColors.SmartQuery.Back;
					rtfQuery.SelectionColor = ThemeColors.SmartQuery.Text;
				}
				Tokenizer tokenizer = ComicSmartListItem.TokenizeQuery(rtfQuery.Text);
				int selectionStart = rtfQuery.SelectionStart;
				foreach (Tokenizer.Token item in tokenizer.GetAll())
				{
					Color selectionColor = ThemeColors.SmartQuery.Text;
                    string text = item.Text.ToLower();
					if (text.StartsWith("\""))
					{
						selectionColor = ThemeColors.SmartQuery.String;
                    }
					else if (text.StartsWith("["))
					{
						selectionColor = ThemeColors.SmartQuery.Keyword;
                    }
					else
					{
                        switch (text)
                        {
                            case "match":
                            case "in":
                            case "all":
                            case "any":
                            case "name":
                                selectionColor = ThemeColors.SmartQuery.Qualifier;
                                break;
                            case "not":
                                selectionColor = ThemeColors.SmartQuery.Negation;
                                break;
                        }
					}
					if (all || (selectionStart > item.Index - 5 && selectionStart < item.Index + item.Length + 5))
					{
						rtfQuery.Select(item.Index, item.Length);
						rtfQuery.SelectionColor = selectionColor;
						if (!text.StartsWith("\""))
							rtfQuery.SelectionFont = queryFont.Language;
                    }
				}
				if (ex != null && ex.Token != null)
				{
					rtfQuery.Select(ex.Token.Index, ex.Token.Length);
					rtfQuery.SelectionBackColor = ThemeColors.SmartQuery.Exception;
					rtfQuery.SelectionFont = queryFont.Exception;
                }
			}
			rtfQuery.ClearUndo();
		}
	}
}
