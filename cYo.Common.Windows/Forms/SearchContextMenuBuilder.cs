using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Localize;
using cYo.Common.Net.Search;
using cYo.Common.Text;

namespace cYo.Common.Windows.Forms
{
	public class SearchContextMenuBuilder
	{
		private const int Limit = 10;

		private const int MultiLimit = 20;

		public ToolStripMenuItem CreateMenuItem(INetSearch search, string hint, string text)
		{
			ToolStripMenuItem mi = new ToolStripMenuItem(TR.Load("SearchMenu")[search.Name, search.Name], search.Image);
			mi.DropDownOpening += delegate
			{
				mi.DropDownItems.Clear();
				mi.DropDownItems.AddRange(CreateItems(search, hint, text, withImages: false).ToArray());
			};
			mi.DropDownItems.Add(new ToolStripMenuItem("Dummy"));
			return mi;
		}

		public IEnumerable<ToolStripMenuItem> CreateMenuItems(IEnumerable<INetSearch> searches, string hint, string text)
		{
			foreach (INetSearch search2 in searches)
			{
				INetSearch search = search2;
				yield return CreateMenuItem(search, hint, text);
			}
		}

		public IEnumerable<ToolStripItem> CreateItems(INetSearch search, string hint, string text, bool withImages)
		{
			using (new WaitCursor())
			{
				List<ToolStripItem> list = new List<ToolStripItem>();
				Image image = (withImages ? search.Image : null);
				try
				{
					text = (text ?? string.Empty).Trim();
					if (!string.IsNullOrEmpty(text))
					{
						SearchResult[] array = search.Search(hint, text, Limit).ToArray();
						if (array.Length != 0)
						{
							AddEntries(list, array, image);
						}
						else
						{
							foreach (string item in text.Split(',').TrimStrings().RemoveEmpty()
								.Distinct())
							{
								if (AddEntries(list, search.Search(hint, item, 2), image) > 0)
								{
									list.Add(new ToolStripSeparator());
								}
								if (list.Count > MultiLimit)
								{
									break;
								}
							}
						}
					}
				}
				catch
				{
				}
				finally
				{
					if (list.Count == 0)
					{
						list.Add(new ToolStripMenuItem(TR.Load("SearchMenu")["NoResult", "No Results"])
						{
							Enabled = false
						});
					}
					string gl = search.GenericSearchLink(hint, text);
					if (!string.IsNullOrEmpty(gl))
					{
						if (list.Count > 0 && !(list[list.Count - 1] is ToolStripSeparator))
						{
							list.Add(new ToolStripSeparator());
						}
						list.Add(new ToolStripMenuItem(TR.Load("SearchMenu")["Search", "Search..."], image, delegate
						{
							SafeStart(gl);
						}));
					}
				}
				return list;
			}
		}

		public ContextMenuStrip CreateContextMenu(IEnumerable<INetSearch> searches, string hint, string text, Action<ContextMenuStrip> customItems)
		{
			ContextMenuStrip cm = new ContextMenuStrip();
			if (searches.Count() > 1)
			{
				cm.Items.AddRange(CreateMenuItems(searches, hint, text).ToArray());
				if (customItems != null)
				{
					customItems(cm);
				}
			}
			else
			{
				INetSearch search = searches.FirstOrDefault();
				if (search != null)
				{
					cm.Opening += delegate
					{
						cm.Items.Clear();
						cm.Items.AddRange(CreateItems(search, hint, text, withImages: true).ToArray());
						if (customItems != null)
						{
							customItems(cm);
						}
					};
					cm.Items.Add(new ToolStripMenuItem("Dummy"));
				}
			}
			return cm;
		}

		private int AddEntries(IList<ToolStripItem> cm, IEnumerable<SearchResult> results, Image image)
		{
			int num = 0;
			foreach (SearchResult result in results)
			{
				SearchResult sr = result;
				cm.Add(new ToolStripMenuItem(sr.Name, image, delegate
				{
					SafeStart(sr.Result);
				}));
				num++;
			}
			return num;
		}

		private void SafeStart(string address)
		{
			try
			{
				Process.Start(address);
			}
			catch (Exception)
			{
			}
		}
	}
}
