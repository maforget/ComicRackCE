using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Localize;

namespace cYo.Common.Windows.Forms
{
    public class ContextMenuBuilder
    {
        private class MenuEntry
        {
            public string Text;

            public EventHandler Click;

            public bool Checked;

            public bool TopLevel;

            public object Tag;

            public DateTime LastTimeUsed;

            public ToolStripMenuItem Create()
            {
                ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem
                {
                    Text = Text,
                    Checked = Checked,
                    Tag = Tag
                };
                toolStripMenuItem.Click += Click;
                return toolStripMenuItem;
            }
        }

        private readonly List<MenuEntry> entries = new List<MenuEntry>();

        public int Count => entries.Count;

        public void Add(string text, bool topLevel, bool chk, EventHandler handler, object tag, DateTime lastTimeUsed)
        {
            MenuEntry item = new MenuEntry
            {
                Text = text,
                Checked = chk,
                TopLevel = topLevel,
                Click = handler,
                Tag = tag,
                LastTimeUsed = lastTimeUsed
            };
            entries.Add(item);
        }

        public ToolStripItem[] Create(int maxLength)
        {
            bool oneLevel = entries.Count < maxLength;
            List<ToolStripItem> list = (from tsi in entries
                                        where tsi.TopLevel || oneLevel
                                        select tsi.Create()).Cast<ToolStripItem>().ToList();
            if (oneLevel)
            {
                return list.ToArray();
            }
            if (list.Count > 0)
            {
                list.Add(new ToolStripSeparator());
            }
            ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(TR.Default["Recent", "Recent"]);
            list.Add(toolStripMenuItem);
            List<MenuEntry> lastUsed = new List<MenuEntry>();
			entries.ForEach(delegate(MenuEntry me)
            {
                if (!me.TopLevel)
                {
                    lastUsed.Add(me);
                }
            });
			lastUsed.Sort(delegate(MenuEntry a, MenuEntry b)
            {
                int num4 = -DateTime.Compare(a.LastTimeUsed, b.LastTimeUsed);
                return (num4 != 0) ? num4 : string.Compare(a.Text, b.Text);
            });
            for (int i = 0; i < Math.Min(maxLength, lastUsed.Count); i++)
            {
                MenuEntry menuEntry = lastUsed[i];
                if (menuEntry.LastTimeUsed == DateTime.MinValue)
                {
                    break;
                }
                toolStripMenuItem.DropDownItems.Add(menuEntry.Create());
            }
            toolStripMenuItem.Visible = toolStripMenuItem.DropDownItems.Count > 0;
            ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem(TR.Default["All"]);
            list.Add(toolStripMenuItem2);
            foreach (MenuEntry entry in entries)
            {
                if (!string.IsNullOrEmpty(entry.Text))
                    toolStripMenuItem2.DropDownItems.Add(entry.Create());
            }
            Dictionary<char, List<MenuEntry>> dictionary = new Dictionary<char, List<MenuEntry>>();
            foreach (MenuEntry entry2 in entries)
            {
                if (!string.IsNullOrEmpty(entry2.Text))
                {
                    char key = entry2.Text[0];
                    if (!dictionary.TryGetValue(key, out var value))
                    {
                        List<MenuEntry> list3 = (dictionary[key] = new List<MenuEntry>());
                        value = list3;
                    }
                    value.Add(entry2);
                }
            }
            List<char> list4 = new List<char>(dictionary.Keys);
            list4.Sort((char a, char b) => string.Compare(a.ToString(), b.ToString()));
            int num = -1;
            int num2 = 0;
            for (int j = 0; j < list4.Count; j++)
            {
                char c = list4[j];
                if (num == -1)
                {
                    num = j;
                }
                num2 += dictionary[c].Count;
                int num3 = ((j != list4.Count - 1) ? dictionary[list4[j + 1]].Count : 0);
                if (num2 + num3 < maxLength && j != list4.Count - 1)
                {
                    continue;
                }
                ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem
                {
                    Text = ((j == num) ? c.ToString() : $"{list4[num]}-{c}")
                };
                for (int k = num; k <= j; k++)
                {
                    foreach (MenuEntry item in dictionary[list4[k]].OrderBy((MenuEntry t) => t.Text))
                    {
                        toolStripMenuItem3.DropDownItems.Add(item.Create());
                    }
                }
                num2 = 0;
                num = -1;
                list.Add(toolStripMenuItem3);
            }
            return list.ToArray();
        }
    }
}
