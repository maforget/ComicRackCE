using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.Localize;
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.IO.Network;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class TasksDialog : FormEx
	{
		private readonly string counterFormat;

		private SimpleCache<string, Image> imageCache = new SimpleCache<string, Image>();

		private static readonly TR tr = TR.Load("TasksDialog");

		private static readonly string Clients = tr["Clients", "Clients: {0}"];

		private static readonly string Info = tr["Info", "Info Requests: {0}"];

		private static readonly string Library = tr["Library", "Library Requests: {0}/{1}"];

		private static readonly string Page = tr["Page", "Page Requests: {0}/{1}"];

		private static readonly string Thumbnail = tr["Thumbnail", "Thumbnail Requests: {0}/{1}"];

		private static readonly string FailedAuth = tr["FailedAuth", "Failed Authentications: {0}"];

		private static readonly string AllShares = tr["AllShares", "All Shares"];

		private static readonly string LastMinute = tr["LastMinute", "Last Minute"];

		private static readonly string Last5Minutes = tr["Last5Minutes", "Last 5 Minutes"];

		private static readonly string LastHour = tr["LastHour", "Last Hour"];

		private static readonly string Session = tr["Session", "Session"];

		private static readonly string waitingText = TR.Default["Waiting", "Waiting"];

		private static readonly string runningText = TR.Default["Running", "Running"];

		private static readonly string completedText = TR.Default["Completed", "Completed"];

		private IEnumerable<QueueManager.IPendingTasks> processes;

		public IEnumerable<QueueManager.IPendingTasks> Processes
		{
			get
			{
				return processes;
			}
			set
			{
				taskImages.Images.Clear();
				processes = value;
				foreach (QueueManager.IPendingTasks process in processes)
				{
					taskImages.Images.Add(process.Group, GetImage(process.TasksImageKey));
				}
				UpdateTaskList();
			}
		}

		public int SelectedTab
		{
			get
			{
				return tabs.SelectedIndex;
			}
			set
			{
				tabs.SelectedIndex = value;
			}
		}

		public TasksDialog()
		{
			LocalizeUtility.UpdateRightToLeft(this);
			InitializeComponent();
			LocalizeUtility.Localize(this, null);
			counterFormat = lblItemCount.Text;
			networkImages.Images.Add(Resources.RemoteDatabase);
			networkImages.Images.Add(Resources.Clock);
			networkImages.Images.Add(Resources.Minus);
			lvTasks.Columns.ScaleDpi();
			this.RestorePosition();
		}

		private Image GetImage(string imageKey)
		{
			return imageCache.Get(imageKey, (string k) => (Bitmap)Resources.ResourceManager.GetObject(imageKey));
		}

		private void UpdateTaskList()
		{
			if (processes == null)
			{
				return;
			}
			int totalPendingItems = 0;
			int totalAbortableItems = 0;
			int previousTopItemIndex = ((lvTasks.TopItem != null) ? lvTasks.TopItem.Index : 0);
			lvTasks.BeginUpdate();
			try
			{
				lvTasks.Items.Clear();
				foreach (QueueManager.IPendingTasks process in processes)
				{
					IEnumerable<object> pendingItems = process.GetPendingItems().OfType<object>();
					int pendingItemCount = pendingItems.Count();
					totalPendingItems += pendingItemCount;
					if (process.Abort != null)
					{
						totalAbortableItems += pendingItemCount;
					}
					ListViewGroup group = lvTasks.Groups[process.Group] ?? lvTasks.Groups.Add(process.Group, process.Group);
					foreach (object item in pendingItems.Take(10))
					{
						ListViewItem listViewItem = lvTasks.Items.Add(item.ToString());
						listViewItem.ImageKey = process.Group;
						listViewItem.Group = group;
						IProgressState progressState = item as IProgressState;
						if (progressState == null)
						{
							listViewItem.SubItems.Add(runningText);
							continue;
						}
						listViewItem.Tag = progressState;
						switch (progressState.State)
						{
							case ProgressState.Waiting:
								listViewItem.SubItems.Add(waitingText);
								break;
							case ProgressState.Running:
								listViewItem.SubItems.Add(progressState.ProgressAvailable ? $"{progressState.ProgressPercentage}%" : runningText);
								break;
							case ProgressState.Completed:
								listViewItem.SubItems.Add(completedText);
								break;
						}
					}
					if (pendingItemCount > 10)
					{
						ListViewItem listViewItem2 = lvTasks.Items.Add(string.Format(TR.Messages["ListMore", "{0} more..."], pendingItemCount - 10));
						listViewItem2.ForeColor = SystemColors.ControlLight;
						listViewItem2.Group = group;
					}
				}
				if (previousTopItemIndex < lvTasks.Items.Count)
				{
					lvTasks.TopItem = lvTasks.Items[previousTopItemIndex];
				}
			}
			finally
			{
				lvTasks.EndUpdate();
			}
			lblItemCount.Text = StringUtility.Format(counterFormat, totalPendingItems);
			btAbort.Enabled = totalAbortableItems > 0;
		}

		private static void AddNodeEntry(TreeNode tn, string text, params object[] p)
		{
			TreeNode treeNode = tn.Nodes[text];
			string text2 = string.Format(text, p);
			if (treeNode != null)
			{
				treeNode.Text = text2;
			}
			else
			{
				tn.Nodes.Add(text, text2, 2, 2);
			}
		}

		private void AddStats(TreeNode tnServer, string name, ServerStatistics.StatisticResult data)
		{
			FileLengthFormat fileLengthFormat = new FileLengthFormat();
			TreeNode tn = tnServer.Nodes[name] ?? tnServer.Nodes.Add(name, name, 1, 1);
			AddNodeEntry(tn, Clients, data.ClientCount);
			AddNodeEntry(tn, Info, data.InfoRequestCount);
			AddNodeEntry(tn, Library, data.LibraryRequestCount, fileLengthFormat.Format(null, data.LibraryRequestSize, null));
			AddNodeEntry(tn, Page, data.PageRequestCount, fileLengthFormat.Format(null, data.PageRequestSize, null));
			AddNodeEntry(tn, Thumbnail, data.ThumbnailRequestCount, fileLengthFormat.Format(null, data.ThumbnailRequestSize, null));
			AddNodeEntry(tn, FailedAuth, data.FailedAuthenticationCount);
		}

		private void UpdateServerStats()
		{
			tvStats.BeginUpdate();
			ServerStatistics.StatisticResult statisticResult = new ServerStatistics.StatisticResult();
			ServerStatistics.StatisticResult statisticResult2 = new ServerStatistics.StatisticResult();
			ServerStatistics.StatisticResult statisticResult3 = new ServerStatistics.StatisticResult();
			ServerStatistics.StatisticResult statisticResult4 = new ServerStatistics.StatisticResult();
			try
			{
				foreach (ComicLibraryServer runningServer in Program.NetworkManager.RunningServers)
				{
					TreeNode treeNode = tvStats.Nodes[runningServer.Config.Name];
					bool flag = false;
					if (treeNode == null)
					{
						treeNode = tvStats.Nodes.Add(runningServer.Config.Name, runningServer.Config.Name, 0, 0);
						flag = true;
					}
					ServerStatistics.StatisticResult result = runningServer.Statistics.GetResult(60);
					ServerStatistics.StatisticResult result2 = runningServer.Statistics.GetResult(300);
					ServerStatistics.StatisticResult result3 = runningServer.Statistics.GetResult(3600);
					ServerStatistics.StatisticResult result4 = runningServer.Statistics.GetResult();
					AddStats(treeNode, LastMinute, result);
					AddStats(treeNode, Last5Minutes, result2);
					AddStats(treeNode, LastHour, result3);
					AddStats(treeNode, Session, result4);
					if (flag)
					{
						treeNode.Expand();
					}
					statisticResult.Add(result);
					statisticResult2.Add(result2);
					statisticResult3.Add(result3);
					statisticResult4.Add(result4);
				}
				if (Program.NetworkManager.RunningServers.Count > 1)
				{
					TreeNode tnServer = tvStats.Nodes["All Shares"] ?? tvStats.Nodes.Add("All Shares", "All Shares", 0, 0);
					AddStats(tnServer, LastMinute, statisticResult);
					AddStats(tnServer, Last5Minutes, statisticResult2);
					AddStats(tnServer, LastHour, statisticResult3);
					AddStats(tnServer, Session, statisticResult4);
				}
				if (tvStats.Nodes.Count == 0)
				{
					if (tabs.TabPages.Contains(tabNetwork))
					{
						tabs.TabPages.Remove(tabNetwork);
					}
				}
				else if (!tabs.TabPages.Contains(tabNetwork))
				{
					tabs.TabPages.Add(tabNetwork);
				}
			}
			finally
			{
				tvStats.EndUpdate();
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			UpdateTaskList();
			UpdateServerStats();
		}

		private void updateTimer_Tick(object sender, EventArgs e)
		{
			UpdateTaskList();
			UpdateServerStats();
		}

		private void btAbort_Click(object sender, EventArgs e)
		{
			foreach (QueueManager.IPendingTasks process in processes)
			{
				if (process.Abort != null)
				{
					process.Abort();
				}
			}
		}

		private void contextMenuAbort_Opening(object sender, CancelEventArgs e)
		{
			FormUtility.SafeToolStripClear(contextMenuAbort.Items);
			foreach (QueueManager.IPendingTasks process in processes)
			{
				Action abort = process.Abort;
				if (abort != null && process.GetPendingItems().Count > 0)
				{
					contextMenuAbort.Items.Add(process.AbortCommandText, GetImage(process.TasksImageKey), delegate
					{
						abort();
					});
				}
			}
		}

		private void lvTasks_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
		{
			IProgressState progressState = e.Item.Tag as IProgressState;
			if (e.ColumnIndex != 1 || progressState == null || !progressState.ProgressAvailable || progressState.State != ProgressState.Running)
			{
				e.DrawDefault = true;
				return;
			}
			Rectangle bounds = e.Bounds;
			bounds.Width = Math.Min(bounds.Width, bounds.Width * progressState.ProgressPercentage / 100);
			bounds.Height--;
            //e.DrawBackground();
            e.DrawThemeBackground();
            // TODO : tweak so that this is more visible in Dark Mode
            e.Graphics.DrawStyledRectangle(bounds, StyledRenderer.AlphaStyle.Hot, Color.Green, StyledRenderer.Default.Frame(0, 1));
			ListViewItem.ListViewSubItem listViewSubItem = e.Item.SubItems[1];
			using (StringFormat format = new StringFormat
			{
				LineAlignment = StringAlignment.Center,
				Alignment = StringAlignment.Center,
				Trimming = StringTrimming.EllipsisCharacter
			})
			{
				using (Brush brush = new SolidBrush(listViewSubItem.ForeColor))
				{
					e.Graphics.DrawString(listViewSubItem.Text, listViewSubItem.Font, brush, e.Bounds, format);
				}
			}
		}

		private void lvTasks_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
			=> ThemeExtensions.ListView_DrawColumnHeader(sender, e);


        public static TasksDialog Show(IWin32Window parent, IEnumerable<QueueManager.IPendingTasks> processes, int tab = 0)
		{
			TasksDialog dlg = new TasksDialog
			{
				Processes = processes,
				tabs = 
				{
					SelectedIndex = tab
				}
			};
			dlg.Show(parent);
			dlg.FormClosed += delegate
			{
				dlg.Dispose();
			};
			dlg.btClose.Click += delegate
			{
				dlg.Close();
			};
			return dlg;
		}
	}
}
