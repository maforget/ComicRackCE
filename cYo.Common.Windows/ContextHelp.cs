using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using cYo.Common.IO;
using cYo.Common.Runtime;
using cYo.Common.Text;
using Microsoft.Win32;

namespace cYo.Common.Windows
{
	public class ContextHelp
	{
		private class HelpMessageFilter : IMessageFilter
		{
			private ContextHelp contextHelp;

			public HelpMessageFilter(ContextHelp contextHelp)
			{
				this.contextHelp = contextHelp;
			}

			bool IMessageFilter.PreFilterMessage(ref Message m)
			{
				int msg = m.Msg;
				if (msg == 256 && (int)m.WParam == 112 && Control.ModifierKeys == Keys.None)
				{
					return contextHelp.HelpRequest(Control.FromHandle(m.HWnd));
				}
				return false;
			}
		}

		private HelpMessageFilter filter;

		private static Regex rxLink = new Regex("^[a-z]+:[/\\\\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public string HelpName
		{
			get;
			private set;
		}

		public string HelpPath
		{
			get;
			set;
		}

		public Dictionary<string, string> Lookup
		{
			get;
			set;
		}

		public Dictionary<string, string> Variables
		{
			get;
			private set;
		}

		public IEnumerable<string> HelpSystems => from p in FileUtility.GetFiles(HelpPath, SearchOption.AllDirectories, ".ini")
			select Path.GetFileNameWithoutExtension(p);

		public bool ShowKey
		{
			get;
			set;
		}

		public ContextHelp(string helpPath)
		{
			Variables = new Dictionary<string, string>();
			HelpPath = helpPath;
		}

		public void Initialize()
		{
			if (filter == null)
			{
				filter = new HelpMessageFilter(this);
				Application.AddMessageFilter(filter);
			}
		}

		private string SubstituteVariabels(string text)
		{
			if (text != null)
			{
				foreach (string key in Variables.Keys)
				{
					text = text.Replace("$" + key, Variables[key]);
				}
				return text;
			}
			return text;
		}

		private string GetApplication(string key)
		{
			int num = 0;
			string value = null;
			do
			{
				string key2 = ((++num == 1) ? key : (key + num));
				if (!Lookup.TryGetValue(key2, out value))
				{
					return null;
				}
				value = SubstituteVariabels(value);
				if (value != null && value.StartsWith("HKEY"))
				{
					try
					{
						value = Registry.GetValue(value, string.Empty, string.Empty).ToString();
					}
					catch
					{
					}
				}
			}
			while (!File.Exists(value));
			return value;
		}

		private void StartLink(string link)
		{
			string value = null;
			if (Lookup != null)
			{
				Lookup.TryGetValue("HelpLink", out value);
			}
			link = SubstituteVariabels(link).Trim();
			if (rxLink.IsMatch(link))
			{
				StartDocument(link);
				return;
			}
			string application = GetApplication("HelpApp");
			if (!string.IsNullOrEmpty(value))
			{
				link = SubstituteVariabels(string.Format(value, link));
			}
			if (string.IsNullOrEmpty(application))
			{
				StartDocument(link);
			}
			else
			{
				StartProgram(application, link);
			}
		}

		private bool HelpRequest(Control c)
		{
			try
			{
				if (c == null)
				{
					c = Form.ActiveForm.ActiveControl;
				}
				Control control = c.TopParent();
				while (Lookup != null && c != null)
				{
					string text = c.Name;
					if (control != null && control.Name != text)
					{
						text = control.Name + ":" + text;
					}
					if (ShowKey)
					{
						MessageBox.Show(text);
					}
					if (Lookup.TryGetValue(text, out var value))
					{
						StartLink(value);
						return true;
					}
					c = c.Parent;
				}
			}
			catch (Exception)
			{
			}
			return false;
		}

		public bool Load(string help)
		{
			Lookup = IniFile.ReadFile(Path.Combine(HelpPath, help) + ".ini");
			HelpName = help;
			return Lookup.Count != 0;
		}

		public IEnumerable<ToolStripItem> GetCustomHelpMenu()
		{
			if (Lookup == null)
			{
				yield break;
			}
			var enumerable = Lookup.Where((KeyValuePair<string, string> n) => n.Key.StartsWith("HelpMenu")).Select(delegate(KeyValuePair<string, string> n)
			{
				string[] array = n.Value.Split(';').TrimStrings().RemoveEmpty()
					.ToArray();
				return new
				{
					Text = array[0],
					Document = ((array.Length < 2) ? string.Empty : SubstituteVariabels(array[1]))
				};
			});
			foreach (var item in enumerable)
			{
				var i = item;
				if (i.Text == "-")
				{
					yield return new ToolStripSeparator();
				}
				else if (!string.IsNullOrEmpty(i.Document))
				{
					yield return new ToolStripMenuItem(i.Text, null, delegate
					{
						StartDocument(i.Document);
					});
				}
			}
		}

		public bool Execute(string lookup)
		{
			if (Lookup == null)
			{
				return false;
			}
			if (!Lookup.TryGetValue(lookup, out var value))
			{
				return false;
			}
			StartLink(SubstituteVariabels(value));
			return true;
		}

		public static void StartDocument(string document)
		{
			try
			{
				Process.Start(document);
			}
			catch (Exception)
			{
			}
		}

		public static void StartProgram(string exe, string commandLine)
		{
			try
			{
				Process.Start(exe, commandLine);
			}
			catch (Exception)
			{
			}
		}
	}
}
