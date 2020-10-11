#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Controls;

namespace cYo.Projects.ComicRack.Plugins.Controls
{
	public class UIComicPageControl : ComicPageControl
	{
		public Control Plugin
		{
			get
			{
				if (base.Controls.Count != 0)
				{
					return base.Controls[0];
				}
				return null;
			}
			set
			{
				if (Plugin != value)
				{
					if (Plugin != null)
					{
						Control plugin = Plugin;
						base.Controls.Remove(plugin);
						plugin.Dispose();
					}
					value.Dock = DockStyle.Fill;
					value.Visible = true;
					base.Controls.Add(value);
				}
			}
		}

		public Func<Control> CreatePlugin
		{
			get;
			set;
		}

		public UIComicPageControl(Control c)
		{
			Plugin = c;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Plugin.Dispose();
			}
			base.Dispose(disposing);
		}

		protected override void OnShowInfo(IEnumerable<ComicBook> books)
		{
			base.OnShowInfo(books);
			dynamic plugin = Plugin;
			try
			{
				plugin.ShowInfo(books.ToArray());
			}
			catch (Exception ex)
			{
				Trace.WriteLine("Failed to execute view plugin: " + ex.Message);
			}
		}

		protected override bool ProcessKeyPreview(ref Message m)
		{
			if (CreatePlugin != null && m.Msg == 256)
			{
				KeyEventArgs keyEventArgs = new KeyEventArgs((Keys)((int)(long)m.WParam | (int)Control.ModifierKeys));
				Trace.WriteLine(keyEventArgs.KeyCode);
				if (keyEventArgs.KeyCode == Keys.R && Control.ModifierKeys == (Keys.Shift | Keys.Control | Keys.Alt))
				{
					Control control = null;
					try
					{
						control = CreatePlugin();
					}
					catch (Exception)
					{
					}
					if (control != null)
					{
						Plugin = control;
					}
				}
			}
			return base.ProcessKeyPreview(ref m);
		}
	}
}
