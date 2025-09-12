using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.Localize;
using cYo.Common.Net.Search;
using cYo.Common.Text;

namespace cYo.Common.Windows.Forms
{
	public class TextBoxContextMenu : ContextMenuStrip
	{
		public class ContextMenuSource
		{
			public Control Control
			{
				get;
				set;
			}

			public string Text
			{
				get;
				set;
			}

			public ContextMenuSource()
			{
			}

			public ContextMenuSource(Control control, string text)
			{
				Control = control;
				Text = text;
			}
		}

		public const int MaxTextLength = 128;

		private IContainer components;

		private ToolStripMenuItem miCut;

		private ToolStripMenuItem miCopy;

		private ToolStripMenuItem miPaste;

		private ToolStripMenuItem miDelete;

		private ToolStripMenuItem miSelectAll;

		private ToolStripSeparator miDeleteSep;

		private ToolStripSeparator miUndoSep;

		private ToolStripMenuItem miUndo;

		public TextBoxContextMenu()
			: this(null)
		{
		}

		public TextBoxContextMenu(IContainer container)
		{
			container?.Add(this);
			InitializeComponent();
			LocalizeUtility.Localize(TR.Load("TextBoxContextMenu"), this);
		}

		public static void Register(TextBoxBase tb, Action<ContextMenuStrip, ContextMenuSource> opening = null)
		{
			if (tb == null)
			{
				return;
			}
			TextBoxContextMenu cm = new TextBoxContextMenu(tb.Container);
			cm.Opening += delegate
			{
				cm.miUndo.Enabled = tb.CanUndo;
				cm.miCopy.Enabled = tb.SelectionLength != 0;
				cm.miPaste.Enabled = Clipboard.ContainsText();
			};
			if (opening != null)
			{
				cm.Opening += delegate
				{
					opening(cm, new ContextMenuSource(tb, GetText(tb)));
				};
			}
			cm.miUndo.Click += delegate
			{
				tb.Undo();
			};
			cm.miPaste.Click += delegate
			{
				tb.SelectedText = SafeGetClipboardText();
			};
			cm.miCopy.Click += delegate
			{
				SafeSetClipboardText(tb.SelectedText);
			};
			cm.miDelete.Click += delegate
			{
				tb.SelectedText = string.Empty;
			};
			cm.miCut.Click += delegate
			{
				SafeSetClipboardText(tb.SelectedText);
				tb.SelectedText = string.Empty;
			};
			cm.miSelectAll.Click += delegate
			{
				tb.SelectAll();
			};
			tb.ContextMenuStrip = cm;
		}

		public static void Register(ComboBox cb, Action<ContextMenuStrip, ContextMenuSource> opening = null)
		{
			if (cb == null || cb.DropDownStyle != ComboBoxStyle.DropDown)
			{
				return;
			}
			TextBoxContextMenu cm = new TextBoxContextMenu(cb.Container);
			cm.Items.Remove(cm.miUndo);
			cm.Items.Remove(cm.miUndoSep);
			cm.Opening += delegate
			{
				ToolStripMenuItem toolStripMenuItem = cm.miCopy;
				bool enabled = (cm.miCut.Enabled = cb.SelectionLength != 0);
				toolStripMenuItem.Enabled = enabled;
				cm.miPaste.Enabled = Clipboard.ContainsText();
			};
			if (opening != null)
			{
				cm.Opening += delegate
				{
					opening(cm, new ContextMenuSource(cb, string.IsNullOrEmpty(cb.SelectedText) ? cb.Text : cb.SelectedText));
				};
			}
			cm.miPaste.Click += delegate
			{
				cb.SelectedText = SafeGetClipboardText();
			};
			cm.miCopy.Click += delegate
			{
				SafeSetClipboardText(cb.SelectedText);
			};
			cm.miSelectAll.Click += delegate
			{
				cb.SelectAll();
			};
			cm.miDelete.Click += delegate
			{
				cb.SelectedText = string.Empty;
			};
			cb.ContextMenuStrip = cm;
		}

		public static void Register(Form c, Action<ContextMenuStrip, ContextMenuSource> opening = null)
		{
			c.GetControls<TextBoxBase>().ForEach(delegate(TextBoxBase tb)
			{
				Register(tb, opening);
			});
			c.GetControls<ComboBox>().ForEach(delegate(ComboBox tb)
			{
				Register(tb, opening);
			});
		}

		public static Action<ContextMenuStrip, ContextMenuSource> AddSearchLinks(IEnumerable<INetSearch> searches, bool top = false)
		{
			return delegate(ContextMenuStrip cm, ContextMenuSource cms)
			{
				ToolStripItem[] array = cm.Items.OfType<ToolStripItem>().ToArray();
				foreach (ToolStripItem toolStripItem in array)
				{
					if (toolStripItem.Tag != null)
					{
						cm.Items.Remove(toolStripItem);
					}
				}
				string text = cms.Text ?? string.Empty;
				SearchContextMenuBuilder searchContextMenuBuilder = new SearchContextMenuBuilder();
				ToolStripMenuItem[] array2 = searchContextMenuBuilder.CreateMenuItems(searches, (cms.Control.Tag as string) ?? string.Empty, text).ToArray();
				if (array2.Length != 0)
				{
					cm.Items.Insert((!top) ? cm.Items.Count : 0, new ToolStripSeparator
					{
						Tag = text
					});
				}
				foreach (ToolStripMenuItem item in array2.Reverse())
				{
					item.Tag = cms.Text ?? text;
					cm.Items.Insert((!top) ? cm.Items.Count : 0, item);
				}
			};
		}

		private static string SafeGetClipboardText()
		{
			try
			{
				return Clipboard.GetText();
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}

		private static void SafeSetClipboardText(string text)
		{
			try
			{
				Clipboard.SetText(text);
			}
			catch (Exception)
			{
			}
		}

		private static string GetText(TextBoxBase tb)
		{
			string text = (string.IsNullOrEmpty(tb.SelectedText) ? tb.Text : tb.SelectedText);
			if (string.IsNullOrEmpty(text) && tb is IPromptText)
			{
				text = ((IPromptText)tb).PromptText;
			}
			return text.Left(MaxTextLength);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			miCut = new System.Windows.Forms.ToolStripMenuItem();
			miCopy = new System.Windows.Forms.ToolStripMenuItem();
			miPaste = new System.Windows.Forms.ToolStripMenuItem();
			miDelete = new System.Windows.Forms.ToolStripMenuItem();
			miSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			miDeleteSep = new System.Windows.Forms.ToolStripSeparator();
			miUndoSep = new System.Windows.Forms.ToolStripSeparator();
			miUndo = new System.Windows.Forms.ToolStripMenuItem();
			SuspendLayout();
			miCut.Name = "miCut";
			miCut.ShortcutKeys = System.Windows.Forms.Keys.X | System.Windows.Forms.Keys.Control;
			miCut.Size = new System.Drawing.Size(164, 22);
			miCut.Text = "Cut";
			miCopy.Name = "miCopy";
			miCopy.ShortcutKeys = System.Windows.Forms.Keys.C | System.Windows.Forms.Keys.Control;
			miCopy.Size = new System.Drawing.Size(164, 22);
			miCopy.Text = "Copy";
			miPaste.Name = "miPaste";
			miPaste.ShortcutKeys = System.Windows.Forms.Keys.V | System.Windows.Forms.Keys.Control;
			miPaste.Size = new System.Drawing.Size(164, 22);
			miPaste.Text = "Paste";
			miDelete.Name = "miDelete";
			miDelete.Size = new System.Drawing.Size(164, 22);
			miDelete.Text = "Delete";
			miSelectAll.Name = "miSelectAll";
			miSelectAll.ShortcutKeys = System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control;
			miSelectAll.Size = new System.Drawing.Size(164, 22);
			miSelectAll.Text = "Select All";
			miDeleteSep.Name = "miDeleteSep";
			miDeleteSep.Size = new System.Drawing.Size(161, 6);
			miUndoSep.Name = "miUndoSep";
			miUndoSep.Size = new System.Drawing.Size(161, 6);
			miUndo.Name = "miUndo";
			miUndo.ShortcutKeys = System.Windows.Forms.Keys.Z | System.Windows.Forms.Keys.Control;
			miUndo.Size = new System.Drawing.Size(164, 22);
			miUndo.Text = "Undo";
			Items.AddRange(new System.Windows.Forms.ToolStripItem[8]
			{
				miUndo,
				miUndoSep,
				miCut,
				miCopy,
				miPaste,
				miDelete,
				miDeleteSep,
				miSelectAll
			});
			base.Size = new System.Drawing.Size(165, 148);
			ResumeLayout(false);
		}
	}
}
