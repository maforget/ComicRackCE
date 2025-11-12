using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using cYo.Common;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class ValueEditorDialog : FormEx
	{
		public string MatchValue
		{
			get
			{
				return rtfMatchValue.Text;
			}
			set
			{
				rtfMatchValue.Text = value;
			}
		}

		public override UIComponent UIComponent => UIComponent.Content;

		public ValueEditorDialog()
		{
			InitializeComponent();
			btInsertValue.Image = ((Bitmap)btInsertValue.Image).ScaleDpi();
			LocalizeUtility.Localize(this, null);
			CreateValueContextMenu();
		}

		public void SyntaxColoring(IEnumerable<ValuePair<Color, Regex>> colors)
		{
			rtfMatchValue.RegisterColorize(colors);
		}

		private void AddField(object sender, EventArgs e)
		{
			ToolStripMenuItem toolStripMenuItem = sender as ToolStripMenuItem;
			rtfMatchValue.SelectedText = "{" + (string)toolStripMenuItem.Tag + "}";
			rtfMatchValue.Focus();
		}

		private void CreateValueContextMenu()
		{
			components = new Container();
			ContextMenuBuilder contextMenuBuilder = new ContextMenuBuilder();
			foreach (string item in ComicBookMatcher.ComicProperties.Concat(ComicBookMatcher.SeriesStatsProperties))
			{
				contextMenuBuilder.Add(item, topLevel: false, chk: false, AddField, item, DateTime.MinValue);
			}
			ContextMenuStrip cm = new ContextMenuStrip(components);
			cm.Items.AddRange(contextMenuBuilder.Create(20));
			btInsertValue.Click += delegate
			{
				cm.Show(btInsertValue, 0, btInsertValue.Height);
			};
		}
	}
}
