using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Net.News;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class NewsDialog : FormEx
	{
		private NewsStorage news;

		public NewsStorage News
		{
			get
			{
				return news;
			}
			set
			{
				news = value;
			}
		}

		public NewsDialog()
		{
			LocalizeUtility.UpdateRightToLeft(this);
			InitializeComponent();
			listNewItems.Columns.ScaleDpi();
			splitContainer.SplitterDistance = FormUtility.ScaleDpiX(splitContainer.SplitterDistance);
			LocalizeUtility.Localize(this, null);
			chkNewsStartup.Checked = Program.Settings.NewsStartup;
		}

		private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			string absoluteUri = e.Url.AbsoluteUri;
			if (absoluteUri != "about:blank")
			{
				e.Cancel = true;
				ShowUrlInBrowser(absoluteUri);
			}
		}

		private void listNewItems_SelectedIndexChanged(object sender, EventArgs e)
		{
			ShowItem(listNewItems.FocusedItem);
		}

		private void btMarkRead_Click(object sender, EventArgs e)
		{
			news.MarkAllRead();
			foreach (ListViewItem item in listNewItems.Items)
			{
				item.Font = FC.Get(item.Font, FontStyle.Regular);
			}
		}

		private void chkNewsStartup_CheckedChanged(object sender, EventArgs e)
		{
			Program.Settings.NewsStartup = chkNewsStartup.Checked;
		}

		private void btRefresh_Click(object sender, EventArgs e)
		{
			AutomaticProgressDialog.Process(this, TR.Messages["RetrieveNews", "Retrieving News"], TR.Messages["RetrieveNewsText", "Refreshing subscribed News Channels"], 1000, delegate
			{
				news.UpdateFeeds(0);
			}, AutomaticProgressDialogOptions.None);
			FillList();
		}

		private static void ShowUrlInBrowser(string url)
		{
			try
			{
				Process.Start(url);
			}
			catch
			{
			}
		}

		private void ShowItem(ListViewItem lvi)
		{
			if (lvi != null)
			{
				string text = string.Empty;
				try
				{
					text = File.ReadAllText(Path.Combine(Application.StartupPath, "NewsTemplate.html"));
				}
				catch
				{
				}
				NewsChannelItem newsChannelItem = lvi.Tag as NewsChannelItem;
				try
				{
					webBrowser.DocumentText = text.Replace("#TITLE#", newsChannelItem.Title).Replace("#TEXT#", newsChannelItem.Description).Replace("#LINK#", newsChannelItem.Link)
						.Replace("#DATE#", newsChannelItem.Published.ToString()).ReplaceWebColors();
				}
				catch
				{
				}
                news.NewsChannelItemInfos[newsChannelItem].IsRead = true;
				lvi.Font = FC.Get(lvi.Font, FontStyle.Regular);
			}
		}

		private void FillList()
		{
			listNewItems.Items.Clear();
			NewsChannelItemCollection items = news.Items;
			items.Sort((NewsChannelItem a, NewsChannelItem b) => DateTime.Compare(b.Published, a.Published));
			foreach (NewsChannelItem item in items)
			{
				ListViewItem listViewItem = listNewItems.Items.Add(item.Title);
				if (!news.NewsChannelItemInfos[item].IsRead)
				{
					listViewItem.Font = FC.Get(listViewItem.Font, FontStyle.Bold);
				}
				listViewItem.Tag = item;
			}
			SelectFirstNotRead();
		}

		private void SelectFirstNotRead()
		{
			if (listNewItems.Items.Count > 0)
			{
				listNewItems.Items[0].Selected = true;
				ShowItem(listNewItems.Items[0]);
			}
		}

		public static void ShowNews(IWin32Window parentForm, NewsStorage storage)
		{
			using (NewsDialog newsDialog = new NewsDialog())
			{
				newsDialog.News = storage;
				newsDialog.FillList();
				newsDialog.ShowDialog(parentForm);
			}
		}
	}
}
